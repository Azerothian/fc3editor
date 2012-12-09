using FC3Editor.Nomad;
using FC3Editor.Parameters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal abstract class ToolPaint : ITool, IToolBase, IParameterProvider, IInputSink
	{
		public enum PaintingMode
		{
			None,
			Plus,
			Minus,
			Shortcut
		}
		protected ParamBool m_square = new ParamBool(Localizer.Localize("PARAM_SQUARE_BRUSH"), false);
		protected ParamFloat m_radius = new ParamFloat(Localizer.Localize("PARAM_RADIUS"), 8f, 1f, 128f, 0.5f);
		protected ParamFloat m_hardness = new ParamFloat(Localizer.Localize("PARAM_HARDNESS"), 0.4f, 0f, 1f, 0.01f);
		protected ParamFloat m_opacity = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 1f, 0f, 1f, 0.01f);
		protected ParamFloat m_distortion = new ParamFloat(Localizer.Localize("PARAM_DISTORTION"), 0f, 0f, 1f, 0.01f);
		protected ParamBool m_grabMode = new ParamBool(Localizer.Localize("PARAM_GRAB_MODE"), false);
		protected ToolPaint.PaintingMode m_painting;
		protected Vec3 m_cursorPos;
		protected bool m_cursorValid;
		protected PaintBrush m_brush;
		public abstract string GetToolName();
		public abstract Image GetToolImage();
		protected IEnumerable<IParameter> _GetParameters()
		{
			yield return this.m_square;
			yield return this.m_radius;
			yield return this.m_hardness;
			yield return this.m_distortion;
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
		protected string GetPaintContextHelp()
		{
			return Localizer.Localize("HELP_PAINT");
		}
		protected string GetShortcutContextHelp()
		{
			return Localizer.Localize("HELP_SHORTCUT");
		}
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
						this.m_cursorPos = cursorPos;
						this.OnBeginPaint();
					}
				}
				else
				{
					this.m_painting = ToolPaint.PaintingMode.Shortcut;
				}
				if (this.m_painting != ToolPaint.PaintingMode.None)
				{
					Editor.Viewport.CaptureMouse = true;
					Editor.Viewport.CameraEnabled = false;
				}
				break;

			case Editor.MouseEvent.MouseUp:
				if (this.m_painting != ToolPaint.PaintingMode.None)
				{
					switch (this.m_painting)
					{
					case ToolPaint.PaintingMode.Plus:
					case ToolPaint.PaintingMode.Minus:
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
					this.m_painting = ToolPaint.PaintingMode.None;
				}
				break;

			case Editor.MouseEvent.MouseMove:
				switch (this.m_painting)
				{
				case ToolPaint.PaintingMode.None:
				case ToolPaint.PaintingMode.Plus:
				case ToolPaint.PaintingMode.Minus:
					this.m_cursorValid = Editor.RayCastTerrainFromMouse(out this.m_cursorPos);
					break;
				}
				break;

			case Editor.MouseEvent.MouseMoveDelta:
				switch (this.m_painting)
				{
				case ToolPaint.PaintingMode.Plus:
				case ToolPaint.PaintingMode.Minus:
					if (!this.m_grabMode.Value)
					{
						Editor.ApplyScreenDeltaToWorldPos(new Vec2((float)mouseEventArgs.X / (float)Editor.Viewport.Width, (float)mouseEventArgs.Y / (float)Editor.Viewport.Height), ref this.m_cursorPos);
						this.m_cursorPos.Z = TerrainManager.GetHeightAtWithWater(this.m_cursorPos.XY);
					}
					else
					{
						this.OnPaintGrab((float)mouseEventArgs.X, (float)mouseEventArgs.Y);
					}
					break;

				case ToolPaint.PaintingMode.Shortcut:
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
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}
		public virtual void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		protected virtual void OnBeginPaint()
		{
			MainForm.Instance.EnableShortcuts = false;
			this.m_painting = (((Control.ModifierKeys & Keys.Control) == Keys.None) ? ToolPaint.PaintingMode.Plus : ToolPaint.PaintingMode.Minus);
			UndoManager.RecordUndo();
			this.CreateBrush();
		}
		protected virtual void OnPaint(float dt, Vec2 pos)
		{
		}
		protected virtual void OnPaintGrab(float x, float y)
		{
		}
		protected virtual void OnEndPaint()
		{
			this.DestroyBrush();
			UndoManager.CommitUndo();
			MainForm.Instance.EnableShortcuts = true;
		}
		protected virtual void OnShortcutDelta(float delta)
		{
			this.m_radius.Value += delta * 0.5f;
		}
		public virtual void Update(float dt)
		{
			if (!this.m_grabMode.Value && (this.m_painting == ToolPaint.PaintingMode.Plus || this.m_painting == ToolPaint.PaintingMode.Minus))
			{
				this.OnPaint(dt, this.m_cursorPos.XY);
			}
			if (this.m_cursorValid)
			{
				Color color = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.White : Color.Black;
				Color borderColor = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.Black : Color.White;
				float length = (Camera.Position - this.m_cursorPos).Length;
				if (this.m_square.Value)
				{
					Render.DrawTerrainSquare(this.m_cursorPos.XY, this.m_radius.Value, length * 0.01f, color, 0f, 0f, borderColor);
					Render.DrawTerrainSquare(this.m_cursorPos.XY, this.m_radius.Value * this.m_hardness.Value, length * 0.01f, Color.Yellow, 0.001f, 0f);
				}
				else
				{
					Render.DrawTerrainCircle(this.m_cursorPos.XY, this.m_radius.Value, length * 0.01f, color, 0f, 0f, borderColor);
					Render.DrawTerrainCircle(this.m_cursorPos.XY, this.m_radius.Value * this.m_hardness.Value, length * 0.01f, Color.Yellow, 0.001f, 0f);
				}
				Render.DrawTerrainCircle(this.m_cursorPos.XY, length * 0.00375f, length * 0.0075f, color, 0f, 0f, borderColor);
			}
		}
		protected void CreateBrush()
		{
			if (this.m_brush.IsValid)
			{
				this.DestroyBrush();
			}
			this.m_brush = PaintBrush.Create(!this.m_square.Value, this.m_radius.Value, this.m_hardness.Value, this.m_opacity.Value, this.m_distortion.Value * this.m_radius.Value * 0.7f);
		}
		protected void DestroyBrush()
		{
			this.m_brush.Destroy();
		}
	}
}
