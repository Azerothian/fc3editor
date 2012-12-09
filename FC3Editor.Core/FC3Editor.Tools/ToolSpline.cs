using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal abstract class ToolSpline : ITool, IToolBase, IParameterProvider, IInputSink
	{
		public enum EditTool
		{
			Select,
			Paint,
			Add,
			Remove
		}
		protected enum State
		{
			None,
			Dragging,
			Moving,
			Drawing,
			Removing
		}
		protected const float penWidth = 0.005f;
		protected const float hitWidth = 0.015f;
		protected const int maxSplinePoints = 100;
		protected ParamEnum<ToolSpline.EditTool> m_paramEditTool = new ParamEnum<ToolSpline.EditTool>(Localizer.Localize("PARAM_SPLINE_MODE"), ToolSpline.EditTool.Select, ParamEnumUIType.Buttons);
		protected bool m_forward;
		protected ToolSpline.State m_state;
		protected Vec2 m_dragStart;
		protected SplineController.SelectMode m_dragMode;
		protected float m_drawLastUpdate;
		protected int m_hitPoint = -1;
		protected Vec2 m_hitPos2;
		protected Vec2 m_hitDelta;
		protected Spline m_spline;
		protected SplineController m_splineController;
		protected RectangleF DragRectangle
		{
			get
			{
				Vec2 dragStart = this.m_dragStart;
				Vec2 normalizedMousePos = Editor.Viewport.NormalizedMousePos;
				Vec2 vec = new Vec2(Math.Min(dragStart.X, normalizedMousePos.X), Math.Min(dragStart.Y, normalizedMousePos.Y));
				Vec2 vec2 = new Vec2(Math.Max(dragStart.X, normalizedMousePos.X), Math.Max(dragStart.Y, normalizedMousePos.Y));
				return new RectangleF(vec.X, vec.Y, vec2.X - vec.X, vec2.Y - vec.Y);
			}
		}
		public ToolSpline()
		{
			this.m_paramEditTool.Names = new string[]
			{
				Localizer.Localize("PARAM_SPLINE_MODE_SELECT") + " (1)",
				Localizer.Localize("PARAM_SPLINE_MODE_DRAW") + " (2)",
				Localizer.Localize("PARAM_SPLINE_MODE_ADD") + " (3)",
				Localizer.Localize("PARAM_SPLINE_MODE_REMOVE") + " (4)"
			};
			this.m_paramEditTool.Images = new Image[]
			{
				Resources.select,
				Resources.brush,
				Resources.add,
				Resources.remove
			};
		}
		public abstract string GetToolName();
		public abstract Image GetToolImage();
		public abstract IEnumerable<IParameter> GetParameters();
		public virtual IParameter GetMainParameter()
		{
			return null;
		}
		public abstract string GetContextHelp();
		public string GetSplineHelp()
		{
			return Localizer.Localize("HELP_SPLINE");
		}
		protected void SetSpline(Spline spline)
		{
			this.m_spline = spline;
			this.m_splineController.SetSpline(this.m_spline);
			if (this.m_spline.IsValid)
			{
				this.m_spline.UpdateSplineHeight();
			}
			this.m_paramEditTool.Enabled = this.m_spline.IsValid;
		}
		public virtual void Activate()
		{
			MainForm.Instance.CursorPhysics = false;
			this.m_splineController = SplineController.Create();
		}
		public void Deactivate()
		{
			MainForm.Instance.CursorPhysics = true;
			this.m_splineController.Dispose();
			this.m_spline = Spline.Null;
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		protected bool TestPoints()
		{
			bool flag = this.m_spline.HitTestPoints(Editor.Viewport.NormalizedMousePos, 0.005f, 0.015f, out this.m_hitPoint, out this.m_hitPos2);
			if (flag)
			{
				this.m_hitDelta = this.m_hitPos2 - Editor.Viewport.NormalizedMousePos;
			}
			return flag;
		}
		protected bool TestSegments()
		{
			Vec3 vec;
			Vec3 vec2;
			Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec, out vec2);
			Vec3 vec3;
			return Editor.RayCastTerrainFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec3) && this.m_spline.HitTestSegments(vec3.XY, 4f, out this.m_hitPoint, out this.m_hitPos2);
		}
		protected void StartDrag(SplineController.SelectMode dragMode)
		{
			this.m_state = ToolSpline.State.Dragging;
			this.m_dragStart = Editor.Viewport.NormalizedMousePos;
			this.m_dragMode = dragMode;
		}
		protected void MovePointsToMouse(bool add)
		{
			if (this.m_hitPoint >= 0)
			{
				Vec3 raySrc;
				Vec3 rayDir;
				Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos + this.m_hitDelta, out raySrc, out rayDir);
				Vec3 vec;
				float num;
				if (Editor.RayCastTerrain(raySrc, rayDir, out vec, out num))
				{
					if (add && this.m_spline.Count < 100)
					{
						if (this.m_forward)
						{
							if ((vec.XY - this.m_spline[this.m_hitPoint - 1]).Length > 15f)
							{
								this.m_spline.InsertPoint(vec.XY, this.m_hitPoint);
								this.m_hitPoint++;
								if (this.m_hitPoint > 2 && this.m_spline.OptimizePoint(this.m_hitPoint - 2))
								{
									this.m_hitPoint--;
								}
							}
							this.m_splineController.ClearSelection();
							this.m_splineController.SetSelected(this.m_hitPoint, true);
						}
						else
						{
							if (!this.m_forward)
							{
								if ((vec.XY - this.m_spline[this.m_hitPoint + 1]).Length > 15f)
								{
									this.m_spline.InsertPoint(vec.XY, this.m_hitPoint);
									if (this.m_hitPoint + 2 < this.m_spline.Count - 1)
									{
										this.m_spline.OptimizePoint(this.m_hitPoint + 2);
									}
								}
								this.m_splineController.ClearSelection();
								this.m_splineController.SetSelected(this.m_hitPoint, true);
							}
						}
					}
					this.m_splineController.MoveSelection(vec.XY - this.m_spline[this.m_hitPoint]);
					this.m_spline.UpdateSpline();
				}
			}
		}
		protected void RemovePointUnderMouse()
		{
			if (this.TestPoints())
			{
				this.m_splineController.ClearSelection();
				this.m_splineController.SetSelected(this.m_hitPoint, true);
				this.m_splineController.DeleteSelection();
				this.m_spline.RemoveSimilarPoints();
				this.m_spline.UpdateSpline();
			}
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			switch (mouseEvent)
			{
			case Editor.MouseEvent.MouseDown:
				if (this.m_spline.IsValid)
				{
					UndoManager.RecordUndo();
					switch (this.m_paramEditTool.Value)
					{
					case ToolSpline.EditTool.Select:
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							this.StartDrag(SplineController.SelectMode.Toggle);
						}
						else
						{
							if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
							{
								this.StartDrag(SplineController.SelectMode.Add);
							}
							else
							{
								if (this.TestPoints())
								{
									if (!this.m_splineController.IsSelected(this.m_hitPoint))
									{
										this.m_splineController.ClearSelection();
										this.m_splineController.SetSelected(this.m_hitPoint, true);
									}
									this.m_state = ToolSpline.State.Moving;
								}
								else
								{
									this.StartDrag(SplineController.SelectMode.Replace);
								}
							}
						}
						break;

					case ToolSpline.EditTool.Paint:
					case ToolSpline.EditTool.Add:
						{
							this.m_hitPoint = -1;
							this.m_hitDelta = new Vec2(0f, 0f);
							Vec3 vec;
							if (this.m_spline.Count < 100 && Editor.RayCastTerrainFromMouse(out vec))
							{
								if (this.m_spline.Count <= 1)
								{
									if (this.m_spline.Count < 1)
									{
										this.m_spline.AddPoint(vec.XY);
									}
									this.m_spline.AddPoint(vec.XY);
									this.m_hitPoint = 1;
								}
								else
								{
									if (this.TestPoints())
									{
										if (this.m_hitPoint == 0)
										{
											this.m_spline.InsertPoint(vec.XY, 0);
										}
										else
										{
											if (this.m_hitPoint == this.m_spline.Count - 1)
											{
												this.m_spline.InsertPoint(vec.XY, this.m_hitPoint + 1);
												this.m_hitPoint++;
											}
										}
									}
									else
									{
										if (this.TestSegments())
										{
											this.m_hitPoint++;
											this.m_spline.InsertPoint(vec.XY, this.m_hitPoint);
										}
									}
								}
								if (this.m_hitPoint != -1)
								{
									this.m_splineController.ClearSelection();
									this.m_splineController.SetSelected(this.m_hitPoint, true);
									this.m_spline.UpdateSpline();
									if (this.m_paramEditTool.Value == ToolSpline.EditTool.Paint)
									{
										this.m_state = ToolSpline.State.Drawing;
										if (this.m_hitPoint == 0)
										{
											this.m_forward = false;
										}
										else
										{
											if (this.m_hitPoint == this.m_spline.Count - 1)
											{
												this.m_forward = true;
											}
											else
											{
												this.m_state = ToolSpline.State.Moving;
											}
										}
									}
									else
									{
										this.m_state = ToolSpline.State.Moving;
									}
								}
							}
							break;
						}

					case ToolSpline.EditTool.Remove:
						this.RemovePointUnderMouse();
						this.m_state = ToolSpline.State.Removing;
						break;
					}
					if (this.m_state != ToolSpline.State.None)
					{
						MainForm.Instance.EnableShortcuts = false;
					}
					else
					{
						UndoManager.CommitUndo();
					}
				}
				break;

			case Editor.MouseEvent.MouseUp:
				if (this.m_spline.IsValid && this.m_state != ToolSpline.State.None)
				{
					UndoManager.CommitUndo();
					switch (this.m_state)
					{
					case ToolSpline.State.Dragging:
						{
							RectangleF dragRectangle = this.DragRectangle;
							this.m_splineController.SelectFromScreenRect(dragRectangle, 0.015f, this.m_dragMode);
							break;
						}

					case ToolSpline.State.Drawing:
						{
							bool flag = false;
							if (this.m_forward && this.m_hitPoint >= 1)
							{
								flag = this.m_spline.OptimizePoint(this.m_hitPoint - 1);
							}
							else
							{
								if (!this.m_forward && this.m_hitPoint < this.m_spline.Count - 1)
								{
									flag = this.m_spline.OptimizePoint(this.m_hitPoint + 1);
								}
							}
							if (flag)
							{
								this.m_spline.UpdateSpline();
							}
							break;
						}
					}
					if (this.m_spline.RemoveSimilarPoints())
					{
						this.m_spline.UpdateSpline();
						this.m_splineController.ClearSelection();
					}
					if (this.m_state != ToolSpline.State.None)
					{
						this.m_spline.FinalizeSpline();
						MainForm.Instance.EnableShortcuts = true;
					}
					this.m_hitPoint = -1;
					this.m_state = ToolSpline.State.None;
				}
				break;

			case Editor.MouseEvent.MouseMove:
				if (this.m_spline.IsValid)
				{
					switch (this.m_state)
					{
					case ToolSpline.State.Moving:
						this.MovePointsToMouse(false);
						break;

					case ToolSpline.State.Drawing:
						this.MovePointsToMouse(true);
						break;

					case ToolSpline.State.Removing:
						this.RemovePointUnderMouse();
						break;
					}
				}
				break;
			}
			return false;
		}
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			switch (keyEvent)
			{
			case Editor.KeyEvent.KeyDown:
				switch (keyEventArgs.KeyCode)
				{
				case Keys.D1:
					this.m_paramEditTool.Value = ToolSpline.EditTool.Select;
					break;

				case Keys.D2:
					this.m_paramEditTool.Value = ToolSpline.EditTool.Paint;
					break;

				case Keys.D3:
					this.m_paramEditTool.Value = ToolSpline.EditTool.Add;
					break;

				case Keys.D4:
					this.m_paramEditTool.Value = ToolSpline.EditTool.Remove;
					break;
				}
				break;

			case Editor.KeyEvent.KeyUp:
				if (keyEventArgs.KeyCode == Keys.Delete)
				{
					this.DeleteSelection();
					this.m_spline.RemoveSimilarPoints();
					return true;
				}
				break;
			}
			return false;
		}
		public virtual void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		public void Update(float dt)
		{
			if (this.m_spline.IsValid)
			{
				this.m_spline.Draw(0.005f, this.m_splineController);
			}
			ToolSpline.State state = this.m_state;
			if (state != ToolSpline.State.Dragging)
			{
				return;
			}
			RectangleF dragRectangle = this.DragRectangle;
			if (this.IsDragRectangle(dragRectangle))
			{
				Render.DrawScreenRectangleOutlined(dragRectangle, 1f, 0.00125f, Color.White);
			}
		}
		protected void DeleteSelection()
		{
			UndoManager.RecordUndo();
			if (this.m_spline.IsValid)
			{
				this.m_splineController.DeleteSelection();
				this.m_spline.UpdateSpline();
			}
			UndoManager.CommitUndo();
		}
		protected bool IsDragRectangle(RectangleF rect)
		{
			return rect.Size.Width > 0.01f && rect.Size.Height > 0.01f;
		}
	}
}
