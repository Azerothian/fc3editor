using FC3Editor.Nomad;
using FC3Editor.Parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal abstract class ToolPaintStrict : ITool, IToolBase, IParameterProvider, IInputSink
	{
		public enum PaintingMode
		{
			None,
			Plus,
			Minus,
			Shortcut
		}
		protected ToolPaintStrict.PaintingMode m_painting;
		protected Vec3 m_cursorPos;
		protected Vec3 m_snappedPos;
		protected Rectangle m_snappedRect;
		protected bool m_cursorValid;
		protected ParamFloat m_radius = new ParamFloat(Localizer.Localize("PARAM_RADIUS"), 8f, 1f, 128f, 1f);
		public abstract string GetToolName();
		public abstract Image GetToolImage();
		public IEnumerable<IParameter> _GetParameters()
		{
			yield return this.m_radius;
			yield break;
		}
		public virtual IEnumerable<IParameter> GetParameters()
		{
			return this._GetParameters();
		}
		public virtual IParameter GetMainParameter()
		{
			return null;
		}
		public abstract string GetContextHelp();
		public virtual void Activate()
		{
			MainForm.Instance.CursorPhysics = false;
		}
		public virtual void Deactivate()
		{
			MainForm.Instance.CursorPhysics = true;
		}
		public virtual void OnInputAcquire()
		{
		}
		public virtual void OnInputRelease()
		{
		}
		public virtual bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			switch (mouseEvent)
			{
			case Editor.MouseEvent.MouseDown:
				if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
				{
					Vec3 cursorPos;
					if (Editor.RayCastTerrainFromMouse(out cursorPos))
					{
						this.UpdateCursorPos(cursorPos);
						this.OnBeginPaint();
					}
				}
				else
				{
					this.m_painting = ToolPaintStrict.PaintingMode.Shortcut;
				}
				if (this.m_painting != ToolPaintStrict.PaintingMode.None)
				{
					Editor.Viewport.CaptureMouse = true;
					Editor.Viewport.CameraEnabled = false;
				}
				break;

			case Editor.MouseEvent.MouseUp:
				if (this.m_painting != ToolPaintStrict.PaintingMode.None)
				{
					switch (this.m_painting)
					{
					case ToolPaintStrict.PaintingMode.Plus:
					case ToolPaintStrict.PaintingMode.Minus:
						this.OnEndPaint();
						break;
					}
					this.m_cursorPos.Z = TerrainManager.GetHeightAtWithWater(this.m_cursorPos.XY);
					Vec2 captureMousePos;
					if (Editor.GetScreenPointFromWorldPos(this.m_cursorPos, out captureMousePos, true))
					{
						Editor.Viewport.CaptureMousePos = captureMousePos;
					}
					Editor.Viewport.CaptureMouse = false;
					Editor.Viewport.CameraEnabled = true;
					this.m_painting = ToolPaintStrict.PaintingMode.None;
				}
				break;

			case Editor.MouseEvent.MouseMove:
				switch (this.m_painting)
				{
				case ToolPaintStrict.PaintingMode.None:
				case ToolPaintStrict.PaintingMode.Plus:
				case ToolPaintStrict.PaintingMode.Minus:
					{
						Vec3 cursorPos2;
						this.m_cursorValid = Editor.RayCastTerrainFromMouse(out cursorPos2);
						this.UpdateCursorPos(cursorPos2);
						break;
					}
				}
				break;

			case Editor.MouseEvent.MouseMoveDelta:
				switch (this.m_painting)
				{
				case ToolPaintStrict.PaintingMode.Plus:
				case ToolPaintStrict.PaintingMode.Minus:
					{
						Vec3 cursorPos3 = this.m_cursorPos;
						Editor.ApplyScreenDeltaToWorldPos(new Vec2((float)mouseEventArgs.X / (float)Editor.Viewport.Width, (float)mouseEventArgs.Y / (float)Editor.Viewport.Height), ref cursorPos3);
						cursorPos3.Z = TerrainManager.GetHeightAtWithWater(cursorPos3.XY);
						this.UpdateCursorPos(cursorPos3);
						break;
					}

				case ToolPaintStrict.PaintingMode.Shortcut:
					{
						float delta;
						if (Math.Abs(mouseEventArgs.X) > Math.Abs(mouseEventArgs.Y))
						{
							delta = (float)mouseEventArgs.X;
						}
						else
						{
							delta = (float)(-(float)mouseEventArgs.Y);
						}
						this.OnShortcutDelta(delta);
						break;
					}
				}
				break;
			}
			return false;
		}
		public virtual bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}
		public virtual void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		protected void UpdateCursorPos(Vec3 cursorPos)
		{
			this.m_cursorPos = cursorPos;
			int num = (int)Math.Round((double)this.m_radius.Value);
			float num2 = (float)(num % 2) * 0.5f;
			this.m_snappedPos.X = (float)Math.Round((double)(cursorPos.X + num2)) - num2;
			this.m_snappedPos.Y = (float)Math.Round((double)(cursorPos.Y + num2)) - num2;
			this.m_snappedPos.Z = cursorPos.Z;
			float num3 = (float)num / 2f;
			this.m_snappedRect = new Rectangle((int)Math.Round((double)(this.m_snappedPos.X - num3)), (int)Math.Round((double)(this.m_snappedPos.Y - num3)), num, num);
			this.m_snappedPos.X = (float)this.m_snappedRect.Left + (float)this.m_snappedRect.Width / 2f;
			this.m_snappedPos.Y = (float)this.m_snappedRect.Top + (float)this.m_snappedRect.Height / 2f;
		}
		protected virtual void OnBeginPaint()
		{
			MainForm.Instance.EnableShortcuts = false;
			this.m_painting = (((Control.ModifierKeys & Keys.Control) == Keys.None) ? ToolPaintStrict.PaintingMode.Plus : ToolPaintStrict.PaintingMode.Minus);
			UndoManager.RecordUndo();
		}
		protected virtual void OnPaint()
		{
		}
		protected virtual void OnEndPaint()
		{
			UndoManager.CommitUndo();
			MainForm.Instance.EnableShortcuts = true;
			TerrainManipulator.Hole_End();
		}
		protected virtual void OnShortcutDelta(float delta)
		{
			this.m_radius.Value += delta * 0.05f;
		}
		public virtual void Update(float dt)
		{
			this.UpdateCursorPos(this.m_cursorPos);
			if (this.m_painting == ToolPaintStrict.PaintingMode.Plus || this.m_painting == ToolPaintStrict.PaintingMode.Minus)
			{
				this.OnPaint();
			}
			if (this.m_cursorValid)
			{
				Color color = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.White : Color.Black;
				Color borderColor = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.Black : Color.White;
				float length = (Camera.Position - this.m_snappedPos).Length;
				Render.DrawTerrainSquare(this.m_snappedPos.XY, (float)this.m_snappedRect.Width / 2f, length * 0.01f, color, 0f, 0f, borderColor);
				Render.DrawTerrainCircle(this.m_cursorPos.XY, length * 0.00375f, length * 0.0075f, color, 0f, 0f, borderColor);
			}
		}
	}
}
