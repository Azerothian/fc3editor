using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using FC3Editor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolObject : ITool, IToolBase, IParameterProvider, IInputSink, IParameterProviderDynamic, IContextHelpDynamic
	{
		public class Mode : ITool, IToolBase, IParameterProvider, IInputSink
		{
			protected ToolObject m_context;
			public Mode(ToolObject context)
			{
				this.m_context = context;
			}
			public virtual string GetToolName()
			{
				return null;
			}
			public virtual Image GetToolImage()
			{
				return null;
			}
			public virtual string GetContextHelp()
			{
				return null;
			}
			public virtual IEnumerable<IParameter> GetParameters()
			{
				yield break;
			}
			public virtual IParameter GetMainParameter()
			{
				return null;
			}
			public virtual void UpdateParams()
			{
			}
			public virtual void Activate()
			{
			}
			public virtual void Deactivate()
			{
			}
			public virtual void AfterActivate()
			{
			}
			public virtual void OnInputAcquire()
			{
			}
			public virtual void OnInputRelease()
			{
			}
			public virtual bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				return false;
			}
			public virtual bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
			{
				return false;
			}
			public virtual void OnEditorEvent(uint eventType, IntPtr eventPtr)
			{
			}
			public virtual void Update(float dt)
			{
			}
		}
		public class RotateMode : ToolObject.Mode
		{
			private ParamVector m_paramRotation = new ParamVector(Localizer.Localize("PARAM_ROTATION"), default(Vec3), ParamVectorUIType.Angles);
			private ParamBool m_paramSnap = new ParamBool(Localizer.Localize("PARAM_USE_SNAP_ANGLES"), false);
			private ParamEnumAngles m_paramSnapSize = new ParamEnumAngles(Localizer.Localize("PARAM_SNAP_ANGLE"), 90f, ParamEnumUIType.Buttons);
			private ParamButton m_actionResetAngles = new ParamButton(Localizer.Localize("PARAM_RESET_TILT"), null);
			private Keys m_keyStart;
			private bool m_keyMoving;
			public RotateMode(ToolObject context) : base(context)
			{
				this.m_paramRotation.ValueChanged += new EventHandler(this.rotation_ValueChanged);
				this.m_paramSnap.ValueChanged += new EventHandler(this.snap_ValueChanged);
				this.m_actionResetAngles.Callback = new ParamButton.ButtonDelegate(this.action_ResetAngles);
				this.m_paramSnapSize.Enabled = false;
			}
			public override string GetToolName()
			{
				return "Rotate objects";
			}
			public override Image GetToolImage()
			{
				return Resources.Tool_Rotate;
			}
			public override string GetContextHelp()
			{
				return string.Concat(new string[]
				{
					Localizer.Localize("HELP_DELETE_OBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_TOOL_ROTATEOBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_GROUP_SELECTION"),
					"\r\n\r\n",
					Localizer.Localize("HELP_NEIGHBORHOOD_SELECTION"),
					"\r\n\r\n",
					Localizer.Localize("HELP_AXIS_TYPE")
				});
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_context.m_textSelected;
				yield return this.m_context.m_paramGroupSelection;
				yield return this.m_context.m_paramMagicWand;
				yield return this.m_context.m_paramAxisType;
				yield return this.m_paramRotation;
				yield return this.m_paramSnap;
				yield return this.m_paramSnapSize;
				yield return this.m_actionResetAngles;
				yield return this.m_context.m_actionCopyClipboard;
				yield return this.m_context.m_actionDelete;
				yield return this.m_context.m_paramObjectSelection;
				yield return this.m_context.m_actionGotoObject;
				yield break;
			}
			public override void UpdateParams()
			{
				if (this.m_context.m_paramAxisType.Value == AxisType.World && this.m_context.m_selection.Count == 1)
				{
					this.m_paramRotation.Value = this.m_context.m_selection[0].Angles;
				}
				else
				{
					this.m_paramRotation.Value = default(Vec3);
				}
				this.m_actionResetAngles.Enabled = (this.m_context.m_selection.Count > 0);
			}
			private void snap_ValueChanged(object sender, EventArgs e)
			{
				this.m_paramSnapSize.Enabled = this.m_paramSnap.Value;
			}
			private void rotation_ValueChanged(object sender, EventArgs e)
			{
				if (this.m_context.m_selection.Count == 0)
				{
					return;
				}
				UndoManager.RecordUndo();
				if (!this.m_context.m_paramGroupSelection.Value)
				{
					switch (this.m_context.m_paramAxisType.Value)
					{
					case AxisType.Local:
						if (this.m_context.m_selection.Count == 1)
						{
							this.m_context.m_selection.Rotate(this.m_paramRotation.Value, this.m_context.m_selection[0].Angles, this.m_context.m_selection[0].Position, false);
						}
						else
						{
							this.m_context.m_selection.RotateLocal(this.m_paramRotation.Value);
						}
						this.m_paramRotation.Value = default(Vec3);
						break;

					case AxisType.World:
						this.m_context.m_selection.SetAngles(this.m_paramRotation.Value);
						break;
					}
				}
				else
				{
					this.m_context.m_selection.ComputeCenter();
					this.m_context.m_selection.Rotate(this.m_paramRotation.Value, new Vec3(0f, 0f, 0f), this.m_context.m_selection.Center, false);
				}
				UndoManager.CommitUndo();
				this.m_context.UpdateSelection();
			}
			private void action_ResetAngles()
			{
				foreach (EditorObject current in this.m_context.m_selection.GetObjects())
				{
					Vec3 angles = current.Angles;
					angles.X = 0f;
					angles.Y = 0f;
					current.Angles = angles;
				}
				this.m_context.UpdateSelection();
			}
			public override void Activate()
			{
				this.m_context.EnableGizmo(true);
				this.m_context.SetGizmoRotationMode(true);
			}
			public override void Deactivate()
			{
				this.m_context.EnableGizmo(false);
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (mouseEvent == Editor.MouseEvent.MouseDown)
				{
					if (this.m_context.m_gizmoActive)
					{
						Vec3 position = this.m_context.m_gizmo.Position;
						Vec3 rotationAxis = default(Vec3);
						switch (this.m_context.m_gizmo.Active)
						{
						case Axis.X:
							rotationAxis = this.m_context.m_gizmo.Axis.axisX;
							break;

						case Axis.Y:
							rotationAxis = this.m_context.m_gizmo.Axis.axisY;
							break;

						case Axis.Z:
							rotationAxis = this.m_context.m_gizmo.Axis.axisZ;
							break;
						}
						ToolObject.RotateAction rotateAction = new ToolObject.RotateAction(this.m_context);
						if (this.m_paramSnap.Value)
						{
							rotateAction.SetSnap(this.m_paramSnapSize.Value);
						}
						rotateAction.Start(position, rotationAxis);
					}
					else
					{
						Vec3 pos;
						EditorObject objectFromScreenPoint = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out pos);
						if (objectFromScreenPoint.IsValid)
						{
							if (!this.m_context.m_selection.Contains(objectFromScreenPoint))
							{
								EditorObjectSelection editorObjectSelection = EditorObjectSelection.Create();
								if ((Control.ModifierKeys & Keys.Control) != Keys.None)
								{
									this.m_context.m_selection.Clone(editorObjectSelection, false);
								}
								this.m_context.SelectObject(editorObjectSelection, objectFromScreenPoint);
								this.m_context.SetSelection(editorObjectSelection, objectFromScreenPoint);
							}
							else
							{
								this.m_context.SetupGizmo(objectFromScreenPoint);
							}
							EditorObjectPivot editorObjectPivot;
							Vec3 position2;
							if (objectFromScreenPoint.GetClosestPivot(pos, out editorObjectPivot))
							{
								position2 = editorObjectPivot.position;
							}
							else
							{
								position2 = objectFromScreenPoint.Position;
							}
							Vec3 rotationAxis2 = new Vec3(0f, 0f, 1f);
							ToolObject.RotateAction rotateAction2 = new ToolObject.RotateAction(this.m_context);
							if (this.m_paramSnap.Value)
							{
								rotateAction2.SetSnap(this.m_paramSnapSize.Value);
							}
							rotateAction2.Start(position2, rotationAxis2);
						}
						else
						{
							ToolObject.SelectAction selectAction = new ToolObject.SelectAction(this.m_context);
							selectAction.Start();
						}
					}
				}
				return false;
			}
			public override bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
			{
				switch (keyEvent)
				{
				case Editor.KeyEvent.KeyDown:
					switch (keyEventArgs.KeyCode)
					{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
						this.m_keyStart = keyEventArgs.KeyCode;
						if (!this.m_keyMoving)
						{
							this.m_keyMoving = true;
							UndoManager.RecordUndo();
						}
						break;
					}
					break;

				case Editor.KeyEvent.KeyChar:
					if (this.m_keyMoving && keyEventArgs.KeyCode == this.m_keyStart && this.m_context.m_gizmo.IsValid)
					{
						Vec3 vec = default(Vec3);
						switch (this.m_keyStart)
						{
						case Keys.Left:
							if (!keyEventArgs.Control)
							{
								vec.Z = -1f;
							}
							else
							{
								vec.Y = -1f;
							}
							break;

						case Keys.Up:
							if (!keyEventArgs.Control)
							{
								vec.Z = -1f;
							}
							else
							{
								vec.X = -1f;
							}
							break;

						case Keys.Right:
							if (!keyEventArgs.Control)
							{
								vec.Z = 1f;
							}
							else
							{
								vec.Y = 1f;
							}
							break;

						case Keys.Down:
							if (!keyEventArgs.Control)
							{
								vec.Z = 1f;
							}
							else
							{
								vec.X = 1f;
							}
							break;
						}
						CoordinateSystem arg_159_0 = Camera.Axis;
						CoordinateSystem axis = this.m_context.m_gizmo.Axis;
						if (keyEventArgs.Shift)
						{
							vec *= MathUtils.Deg2Rad(0.25f);
						}
						else
						{
							vec *= MathUtils.Deg2Rad(1f);
						}
						this.m_context.m_selection.Rotate(vec, axis.ToAngles(), this.m_context.m_gizmo.Position, false);
					}
					break;

				case Editor.KeyEvent.KeyUp:
					if (this.m_keyMoving && keyEventArgs.KeyCode == this.m_keyStart)
					{
						UndoManager.CommitUndo();
						this.m_keyMoving = false;
					}
					break;
				}
				return false;
			}
		}
		private class RotateAction : InputBase
		{
			private ToolObject m_context;
			private Vec3 m_rotationPivot;
			private Vec3 m_rotationAxis;
			private float m_rotationDelta;
			private bool m_snap;
			private float m_snapSize;
			public RotateAction(ToolObject context)
			{
				this.m_context = context;
			}
			public void ClearSnap()
			{
				this.m_snap = false;
			}
			public void SetSnap(float snapSize)
			{
				this.m_snap = true;
				this.m_snapSize = snapSize;
			}
			public bool Start(Vec3 rotationPivot, Vec3 rotationAxis)
			{
				this.m_rotationPivot = rotationPivot;
				this.m_rotationAxis = rotationAxis;
				base.AcquireInput();
				this.m_context.m_selection.SaveState();
				return true;
			}
			public override void OnInputAcquire()
			{
				UndoManager.RecordUndo();
				Editor.Viewport.CaptureMouse = true;
			}
			public override void OnInputRelease()
			{
				Editor.Viewport.CaptureMouse = false;
				UndoManager.CommitUndo();
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseUp:
					base.ReleaseInput();
					break;

				case Editor.MouseEvent.MouseMoveDelta:
					{
						Vec3 rotationAxis = this.m_rotationAxis;
						Vec2 xZ = Camera.Axis.ConvertFromWorld(rotationAxis).XZ;
						xZ.Normalize();
						xZ.Rotate90CW();
						Vec2 v = new Vec2((float)mouseEventArgs.X, (float)(-(float)mouseEventArgs.Y));
						float num = Vec2.Dot(xZ, v);
						float angle;
						if (!this.m_snap)
						{
							angle = num * 0.025f;
						}
						else
						{
							this.m_rotationDelta += num;
							float num2 = (float)Math.IEEERemainder((double)this.m_rotationDelta, 25.0);
							angle = (this.m_rotationDelta - num2) / 25f * MathUtils.Deg2Rad(this.m_snapSize);
							this.m_rotationDelta = num2;
						}
						this.m_context.m_selection.LoadState();
						switch (this.m_context.m_paramAxisType.Value)
						{
						case AxisType.Local:
							this.m_context.m_selection.Rotate(angle, rotationAxis, this.m_rotationPivot, false);
							break;

						case AxisType.World:
							if (this.m_context.m_selection.Count > 1)
							{
								this.m_context.m_selection.RotateCenter(angle, rotationAxis);
							}
							else
							{
								this.m_context.m_selection.Rotate(angle, rotationAxis, this.m_rotationPivot, false);
							}
							break;
						}
						this.m_context.m_selection.SaveState();
						this.m_context.m_selection.SnapToClosestObjects();
						this.m_context.UpdateSelection();
						break;
					}
				}
				return false;
			}
		}
		public class SnapMode : ToolObject.Mode
		{
			private ParamBool m_paramUseSnapAngle = new ParamBool(Localizer.Localize("PARAM_USE_SNAP_ANGLES"), false);
			private ParamEnumAngles m_paramSnapAngle = new ParamEnumAngles(Localizer.Localize("PARAM_SNAP_ANGLE"), 90f, ParamEnumUIType.Buttons);
			private ParamEnum<RotationDirection> m_paramAngleDir = new ParamEnum<RotationDirection>(Localizer.Localize("PARAM_ANGLE_DIRECTION"), RotationDirection.CCW, ParamEnumUIType.Buttons);
			private ParamBool m_paramPreserveOrientation = new ParamBool(Localizer.Localize("PARAM_PRESERVE_ORIENTATION"), false);
			public SnapMode(ToolObject context) : base(context)
			{
				this.m_paramUseSnapAngle.ValueChanged += new EventHandler(this.paramUseSnapAngle_ValueChanged);
				this.m_paramPreserveOrientation.ValueChanged += new EventHandler(this.paramPreserveOrientation_ValueChanged);
				this.m_paramUseSnapAngle.Enabled = !this.m_paramPreserveOrientation.Value;
				this.m_paramSnapAngle.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
				this.m_paramAngleDir.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
			}
			public override string GetToolName()
			{
				return "Snap objects";
			}
			public override Image GetToolImage()
			{
				return Resources.Tool_Link;
			}
			public override string GetContextHelp()
			{
				return Localizer.LocalizeCommon("HELP_TOOL_SNAPOBJECT") + "\r\n\r\n" + Localizer.Localize("HELP_TOOL_SNAPOBJECT");
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_context.m_textSelected;
				yield return this.m_paramUseSnapAngle;
				yield return this.m_paramSnapAngle;
				yield return this.m_paramAngleDir;
				yield return this.m_paramPreserveOrientation;
				yield return this.m_context.m_actionCopyClipboard;
				yield return this.m_context.m_actionDelete;
				yield return this.m_context.m_paramObjectSelection;
				yield return this.m_context.m_actionGotoObject;
				yield break;
			}
			private void paramPreserveOrientation_ValueChanged(object sender, EventArgs e)
			{
				this.m_paramUseSnapAngle.Enabled = !this.m_paramPreserveOrientation.Value;
				this.m_paramSnapAngle.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
				this.m_paramAngleDir.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
			}
			private void paramUseSnapAngle_ValueChanged(object sender, EventArgs e)
			{
				this.m_paramSnapAngle.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
				this.m_paramAngleDir.Enabled = (!this.m_paramPreserveOrientation.Value && this.m_paramUseSnapAngle.Value);
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (mouseEvent == Editor.MouseEvent.MouseDown)
				{
					ToolObject.SnapAction snapAction = new ToolObject.SnapAction(this.m_context);
					snapAction.PreserveOrientation = this.m_paramPreserveOrientation.Value;
					if (this.m_paramUseSnapAngle.Value)
					{
						snapAction.AngleSnap = MathUtils.Deg2Rad((this.m_paramAngleDir.Value == RotationDirection.CCW) ? this.m_paramSnapAngle.Value : (-this.m_paramSnapAngle.Value));
					}
					if (!snapAction.Start())
					{
						ToolObject.SelectAction selectAction = new ToolObject.SelectAction(this.m_context);
						selectAction.Start();
					}
				}
				return false;
			}
		}
		private class SnapAction : InputBase
		{
			private ToolObject m_context;
			private bool m_preserveOrientation;
			private float m_angle;
			private EditorObject m_source;
			private EditorObjectPivot m_sourcePivot;
			private EditorObject m_target;
			private EditorObjectPivot m_targetPivot;
			public bool PreserveOrientation
			{
				get
				{
					return this.m_preserveOrientation;
				}
				set
				{
					this.m_preserveOrientation = value;
				}
			}
			public float AngleSnap
			{
				get
				{
					return this.m_angle;
				}
				set
				{
					this.m_angle = value;
				}
			}
			public SnapAction(ToolObject context)
			{
				this.m_context = context;
			}
			public bool Start()
			{
				Vec3 pos;
				this.m_source = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out pos);
				if (!this.m_source.IsValid)
				{
					return false;
				}
				if (!this.m_source.GetClosestPivot(pos, out this.m_sourcePivot))
				{
					return false;
				}
				if (!this.m_context.m_selection.Contains(this.m_source))
				{
					EditorObjectSelection editorObjectSelection = EditorObjectSelection.Create();
					if ((Control.ModifierKeys & Keys.Control) != Keys.None)
					{
						this.m_context.m_selection.Clone(editorObjectSelection, false);
					}
					this.m_context.SelectObject(editorObjectSelection, this.m_source);
					this.m_context.SetSelection(editorObjectSelection, this.m_source);
				}
				base.AcquireInput();
				return true;
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseUp:
					{
						Vec3 pos;
						EditorObject objectFromScreenPoint = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out pos, false, this.m_source);
						if (objectFromScreenPoint.IsValid && objectFromScreenPoint.GetClosestPivot(pos, out this.m_targetPivot))
						{
							UndoManager.RecordUndo();
							this.m_context.m_selection.Center = this.m_sourcePivot.position;
							this.m_context.m_selection.SnapToPivot(this.m_sourcePivot, this.m_targetPivot, this.PreserveOrientation, this.m_angle);
							UndoManager.CommitUndo();
						}
						base.ReleaseInput();
						break;
					}

				case Editor.MouseEvent.MouseMove:
					{
						Vec3 pos2;
						this.m_target = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out pos2, false, this.m_source);
						if (this.m_target.IsValid)
						{
							this.m_target.GetClosestPivot(pos2, out this.m_targetPivot);
						}
						break;
					}
				}
				return false;
			}
			public override void Update(float dt)
			{
				bool flag = false;
				bool flag2 = false;
				Vec2 center = default(Vec2);
				Vec2 center2 = default(Vec2);
				if (this.m_source.IsValid)
				{
					flag = Editor.GetScreenPointFromWorldPos(this.m_sourcePivot.position, out center);
				}
				if (this.m_target.IsValid)
				{
					flag2 = Editor.GetScreenPointFromWorldPos(this.m_targetPivot.position, out center2);
				}
				if (flag)
				{
					Render.DrawScreenCircleOutlined(center, 0f, 0.0005f, 0.001f, Color.Red);
				}
				if (flag2)
				{
					Render.DrawScreenCircleOutlined(center2, 0f, 0.0005f, 0.001f, Color.Yellow);
				}
			}
		}
		public class SelectMode : ToolObject.Mode
		{
			public SelectMode(ToolObject context) : base(context)
			{
			}
			public override string GetToolName()
			{
				return "Select objects";
			}
			public override Image GetToolImage()
			{
				return Resources.Tool_Select;
			}
			public override string GetContextHelp()
			{
				return string.Concat(new string[]
				{
					Localizer.Localize("HELP_CONTROLS_SELECTOBJECT"),
					"\r\n",
					Localizer.Localize("HELP_DELETE_OBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_TOOL_SELECTOBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_GROUP_SELECTION"),
					"\r\n\r\n",
					Localizer.Localize("HELP_NEIGHBORHOOD_SELECTION")
				});
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_context.m_textSelected;
				yield return this.m_context.m_paramGroupSelection;
				yield return this.m_context.m_paramMagicWand;
				yield return this.m_context.m_actionCopyClipboard;
				yield return this.m_context.m_actionDelete;
				yield return this.m_context.m_actionFreeze;
				yield return this.m_context.m_actionUnfreeze;
				yield return this.m_context.m_paramObjectSelection;
				yield return this.m_context.m_actionGotoObject;
				yield break;
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (mouseEvent == Editor.MouseEvent.MouseDown)
				{
					ToolObject.SelectAction selectAction = new ToolObject.SelectAction(this.m_context);
					selectAction.Start();
				}
				return false;
			}
		}
		private class SelectAction : InputBase
		{
			private ToolObject m_context;
			private Vec2 m_dragStart;
			private RectangleF DragRectangle
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
			public SelectAction(ToolObject context)
			{
				this.m_context = context;
			}
			public bool Start()
			{
				this.m_dragStart = Editor.Viewport.NormalizedMousePos;
				base.AcquireInput();
				return true;
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (mouseEvent == Editor.MouseEvent.MouseUp)
				{
					Vec2 arg_11_0 = Editor.Viewport.NormalizedMousePos;
					bool flag = (Control.ModifierKeys & Keys.Control) != Keys.None;
					bool flag2 = (Control.ModifierKeys & Keys.Alt) != Keys.None;
					bool flag3 = (Control.ModifierKeys & Keys.Shift) != Keys.None;
					EditorObjectSelection editorObjectSelection = EditorObjectSelection.Create();
					if (flag || flag3 || flag2)
					{
						this.m_context.m_selection.Clone(editorObjectSelection, false);
					}
					EditorObject gizmoObject = EditorObject.Null;
					RectangleF dragRectangle = this.DragRectangle;
					if (this.IsDragRectangle(dragRectangle))
					{
						EditorObjectSelection selection;
						if (flag || flag2)
						{
							selection = EditorObjectSelection.Create();
						}
						else
						{
							selection = editorObjectSelection;
						}
						ObjectManager.GetObjectsFromScreenRect(selection, this.DragRectangle);
						if (flag)
						{
							editorObjectSelection.ToggleSelection(selection);
							selection.Dispose();
						}
						else
						{
							if (flag2)
							{
								editorObjectSelection.RemoveSelection(selection);
								selection.Dispose();
							}
						}
					}
					else
					{
						Vec3 vec;
						EditorObject objectFromScreenPoint = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec);
						if (objectFromScreenPoint.IsValid)
						{
							this.m_context.SelectObject(editorObjectSelection, objectFromScreenPoint);
							gizmoObject = objectFromScreenPoint;
						}
					}
					this.m_context.SetSelection(editorObjectSelection, gizmoObject);
					base.ReleaseInput();
				}
				return false;
			}
			public override void Update(float dt)
			{
				RectangleF dragRectangle = this.DragRectangle;
				if (this.IsDragRectangle(dragRectangle))
				{
					Render.DrawScreenRectangleOutlined(dragRectangle, 1f, 0.00125f, Color.White);
				}
			}
			private bool IsDragRectangle(RectangleF rect)
			{
				return rect.Size.Width > 0.01f && rect.Size.Height > 0.01f;
			}
		}
		public class MoveMode : ToolObject.Mode
		{
			private ParamVector m_paramPosition = new ParamVector(Localizer.Localize("PARAM_POSITION"), default(Vec3), ParamVectorUIType.Position);
			private ParamBool m_paramSnap = new ParamBool(Localizer.Localize("PARAM_USE_SNAP_GRID"), false);
			private ParamFloat m_paramSnapSize = new ParamFloat(Localizer.Localize("PARAM_SNAP_GRID_SIZE"), 1f, 1f, 16f, 0.25f);
			private ParamBool m_paramSnapObjectSize = new ParamBool(Localizer.Localize("PARAM_SNAP_OBJECT_SIZE"), false);
			private ParamBool m_paramUseGizmos = new ParamBool(Localizer.Localize("PARAM_USE_GIZMO"), true);
			private ParamBool m_paramGrabAnchor = new ParamBool(Localizer.Localize("PARAM_GRAB_ANCHOR"), false);
			private ParamCheckButton m_actionAlignToObject = new ParamCheckButton(Localizer.Localize("PARAM_ALIGN_TO_OBJECT"));
			private ParamButton m_actionDropToPhysics = new ParamButton(Localizer.Localize("PARAM_SELECTION_DROP"), null);
			private Keys m_keyStart;
			private bool m_keyMoving;
			public MoveMode(ToolObject context) : base(context)
			{
				this.m_paramSnapSize.Enabled = false;
				this.m_paramSnapObjectSize.Enabled = false;
				this.m_paramPosition.ValueChanged += new EventHandler(this.position_ValueChanged);
				this.m_paramSnap.ValueChanged += new EventHandler(this.snap_ValueChanged);
				this.m_paramSnapObjectSize.ValueChanged += new EventHandler(this.snapObjectSize_ValueChanged);
				this.m_paramUseGizmos.ValueChanged += new EventHandler(this.useGizmos_ValueChanged);
				this.m_actionAlignToObject.ActivateCallback = new ParamCheckButton.ButtonDelegate(this.alignToObject_Activate);
				this.m_actionAlignToObject.DeactivateCallback = new ParamCheckButton.ButtonDelegate(this.alignToObject_Deactivate);
				this.m_actionDropToPhysics.Callback = new ParamButton.ButtonDelegate(this.action_DropToPhysics);
			}
			public override string GetToolName()
			{
				return "Move objects";
			}
			public override Image GetToolImage()
			{
				return Resources.Tool_Move;
			}
			public override string GetContextHelp()
			{
				return string.Concat(new string[]
				{
					Localizer.Localize("HELP_DELETE_OBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_TOOL_MOVEOBJECT"),
					"\r\n\r\n",
					Localizer.Localize("HELP_GROUP_SELECTION"),
					"\r\n\r\n",
					Localizer.Localize("HELP_NEIGHBORHOOD_SELECTION"),
					"\r\n\r\n",
					Localizer.Localize("HELP_AXIS_TYPE")
				});
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_context.m_textSelected;
				yield return this.m_context.m_paramGroupSelection;
				yield return this.m_context.m_paramMagicWand;
				yield return this.m_context.m_paramAxisType;
				yield return this.m_paramPosition;
				yield return this.m_paramSnap;
				yield return this.m_paramSnapSize;
				yield return this.m_paramSnapObjectSize;
				yield return this.m_paramUseGizmos;
				yield return this.m_actionAlignToObject;
				yield return this.m_actionDropToPhysics;
				yield return this.m_context.m_actionDelete;
				yield return this.m_context.m_paramObjectSelection;
				yield return this.m_context.m_actionGotoObject;
				yield break;
			}
			public override void UpdateParams()
			{
				if (this.m_context.m_paramAxisType.Value == AxisType.World && this.m_context.m_selection.Count > 0)
				{
					this.m_paramPosition.Value = this.m_context.m_selection.Center;
				}
				else
				{
					this.m_paramPosition.Value = default(Vec3);
				}
				this.m_actionAlignToObject.Enabled = (this.m_context.m_selection.Count > 0);
				this.m_actionDropToPhysics.Enabled = (this.m_context.m_selection.Count > 0);
			}
			private void position_ValueChanged(object sender, EventArgs e)
			{
				if (this.m_context.m_selection.Count == 0)
				{
					return;
				}
				UndoManager.RecordUndo();
				this.m_context.m_selection.ComputeCenter();
				switch (this.m_context.m_paramAxisType.Value)
				{
				case AxisType.Local:
					if (this.m_context.m_selection.Count == 1)
					{
						this.m_context.m_selection.MoveTo(this.m_context.m_selection.Center + CoordinateSystem.FromAngles(this.m_context.m_selection[0].Angles).ConvertToWorld(this.m_paramPosition.Value), EditorObjectSelection.MoveMode.MoveNormal);
					}
					else
					{
						this.m_context.m_selection.MoveTo(this.m_context.m_selection.Center + this.m_paramPosition.Value, EditorObjectSelection.MoveMode.MoveNormal);
					}
					this.m_paramPosition.Value = default(Vec3);
					break;

				case AxisType.World:
					this.m_context.m_selection.MoveTo(this.m_paramPosition.Value, EditorObjectSelection.MoveMode.MoveNormal);
					break;
				}
				UndoManager.CommitUndo();
				this.m_context.UpdateSelection();
			}
			private void snap_ValueChanged(object sender, EventArgs e)
			{
				this.m_paramSnapSize.Enabled = (this.m_paramSnap.Value && !this.m_paramSnapObjectSize.Value);
				this.m_paramSnapObjectSize.Enabled = this.m_paramSnap.Value;
			}
			private void snapObjectSize_ValueChanged(object sender, EventArgs e)
			{
				this.m_paramSnapSize.Enabled = (this.m_paramSnap.Value && !this.m_paramSnapObjectSize.Value);
			}
			private void useGizmos_ValueChanged(object sender, EventArgs e)
			{
				this.m_context.EnableGizmo(this.m_paramUseGizmos.Value);
			}
			private void alignToObject_Activate()
			{
				this.m_context.m_selection.SaveState();
				this.m_context.EnableGizmo(false);
			}
			private void alignToObject_Deactivate()
			{
				this.m_context.m_selection.LoadState();
				this.m_context.EnableGizmo(this.m_paramUseGizmos.Value);
			}
			private bool OnAlignToObjectMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseUp:
					break;

				case Editor.MouseEvent.MouseMove:
					{
						this.m_context.m_selection.LoadState();
						Vec3 vec;
						Vec3 vec2;
						Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec, out vec2);
						Vec3 vec3;
						EditorObject objectFromScreenPoint = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec3, false, this.m_context.m_selection);
						if (!objectFromScreenPoint.IsValid)
						{
							return false;
						}
						using (IEnumerator<EditorObject> enumerator = this.m_context.m_selection.GetObjects().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								EditorObject current = enumerator.Current;
								current.Position = objectFromScreenPoint.Position;
								current.Angles = objectFromScreenPoint.Angles;
							}
							return false;
						}
						break;
					}

				default:
					return false;
				}
				if (this.m_context.m_selection.Count != 0)
				{
					Vec3 position = this.m_context.m_selection[0].Position;
					Vec3 angles = this.m_context.m_selection[0].Angles;
					this.m_context.m_selection.LoadState();
					UndoManager.RecordUndo();
					foreach (EditorObject current2 in this.m_context.m_selection.GetObjects())
					{
						current2.Position = position;
						current2.Angles = angles;
					}
					UndoManager.CommitUndo();
					this.m_context.m_selection.SaveState();
					this.m_actionAlignToObject.Checked = false;
				}
				return false;
			}
			private void action_DropToPhysics()
			{
				UndoManager.RecordUndo();
				this.m_context.m_selection.DropToGround(true, this.m_context.m_paramGroupSelection.Value);
				UndoManager.CommitUndo();
				this.m_context.UpdateSelection();
			}
			public override void Activate()
			{
				this.m_context.EnableGizmo(this.m_paramUseGizmos.Value);
				this.m_context.SetGizmoRotationMode(false);
			}
			public override void Deactivate()
			{
				this.m_context.EnableGizmo(false);
				this.m_actionAlignToObject.Checked = false;
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (this.m_actionAlignToObject.Checked)
				{
					return this.OnAlignToObjectMouseEvent(mouseEvent, mouseEventArgs);
				}
				if (mouseEvent == Editor.MouseEvent.MouseDown)
				{
					if (this.m_context.m_gizmoActive)
					{
						UndoManager.RecordUndo();
						if ((Control.ModifierKeys & Keys.Control) == Keys.None)
						{
							if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
							{
								EditorObjectSelection editorObjectSelection = EditorObjectSelection.Create();
								this.m_context.m_selection.Clone(editorObjectSelection, true);
								int num = this.m_context.m_selection.IndexOf(this.m_context.m_gizmoObject);
								this.m_context.SetSelection(editorObjectSelection, (num != -1) ? editorObjectSelection[num] : EditorObject.Null);
							}
							ToolObject.MoveAction moveAction = new ToolObject.MoveAction(this.m_context);
							if (this.m_paramSnap.Value)
							{
								if (this.m_paramSnapObjectSize.Value && this.m_context.m_gizmoObject.IsValid)
								{
									moveAction.SetSnap(this.m_context.m_gizmoObject);
								}
								else
								{
									moveAction.SetSnap(this.m_paramSnapSize.Value);
								}
							}
							moveAction.Start(this.m_context.m_gizmo.Position);
						}
						else
						{
							ToolObject.RotateAction rotateAction = new ToolObject.RotateAction(this.m_context);
							Vec3 rotationAxis = default(Vec3);
							switch (this.m_context.m_gizmo.Active)
							{
							case Axis.X:
								rotationAxis = this.m_context.m_gizmo.Axis.axisX;
								break;

							case Axis.Y:
								rotationAxis = this.m_context.m_gizmo.Axis.axisY;
								break;

							case Axis.Z:
								rotationAxis = this.m_context.m_gizmo.Axis.axisZ;
								break;
							}
							rotateAction.Start(this.m_context.m_gizmo.Position, rotationAxis);
						}
					}
					else
					{
						UndoManager.RecordUndo();
						bool flag = true;
						Vec3 vec;
						EditorObject objectFromScreenPoint = ObjectManager.GetObjectFromScreenPoint(Editor.Viewport.NormalizedMousePos, out vec);
						if (objectFromScreenPoint.IsValid)
						{
							if (!this.m_context.m_selection.Contains(objectFromScreenPoint))
							{
								EditorObjectSelection editorObjectSelection2 = EditorObjectSelection.Create();
								if ((Control.ModifierKeys & Keys.Control) != Keys.None || (Control.ModifierKeys & Keys.Shift) != Keys.None)
								{
									this.m_context.m_selection.Clone(editorObjectSelection2, false);
								}
								this.m_context.SelectObject(editorObjectSelection2, objectFromScreenPoint);
								this.m_context.SetSelection(editorObjectSelection2, objectFromScreenPoint);
							}
							else
							{
								this.m_context.SetupGizmo(objectFromScreenPoint);
							}
							EditorObjectPivot editorObjectPivot;
							Vec3 position;
							if (this.m_paramGrabAnchor.Value && objectFromScreenPoint.GetClosestPivot(vec, out editorObjectPivot, (objectFromScreenPoint.Position - vec).Length * 1.1f))
							{
								position = editorObjectPivot.position;
							}
							else
							{
								position = objectFromScreenPoint.Position;
							}
							ToolObject.MovePhysicsAction movePhysicsAction = new ToolObject.MovePhysicsAction(this.m_context);
							movePhysicsAction.Start(position);
							flag = false;
						}
						if (flag)
						{
							ToolObject.SelectAction selectAction = new ToolObject.SelectAction(this.m_context);
							selectAction.Start();
						}
					}
				}
				return false;
			}
			public override bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
			{
				switch (keyEvent)
				{
				case Editor.KeyEvent.KeyDown:
					{
						Keys keyCode = keyEventArgs.KeyCode;
						if (keyCode != Keys.ControlKey)
						{
							switch (keyCode)
							{
							case Keys.Left:
							case Keys.Up:
							case Keys.Right:
							case Keys.Down:
								this.m_keyStart = keyEventArgs.KeyCode;
								if (!this.m_keyMoving)
								{
									this.m_keyMoving = true;
									UndoManager.RecordUndo();
								}
								break;
							}
						}
						else
						{
							this.m_context.SetGizmoRotationMode(true);
						}
						break;
					}

				case Editor.KeyEvent.KeyChar:
					if (this.m_keyMoving && keyEventArgs.KeyCode == this.m_keyStart && this.m_context.m_gizmo.IsValid)
					{
						Vec3 vec = default(Vec3);
						Vec3 vec2 = default(Vec3);
						switch (this.m_keyStart)
						{
						case Keys.Left:
							if (!keyEventArgs.Control)
							{
								vec.X = -1f;
							}
							else
							{
								vec2.Z = -1f;
							}
							break;

						case Keys.Up:
							if (!keyEventArgs.Control)
							{
								vec.Y = 1f;
							}
							else
							{
								vec.Z = 1f;
							}
							break;

						case Keys.Right:
							if (!keyEventArgs.Control)
							{
								vec.X = 1f;
							}
							else
							{
								vec2.Z = 1f;
							}
							break;

						case Keys.Down:
							if (!keyEventArgs.Control)
							{
								vec.Y = -1f;
							}
							else
							{
								vec.Z = -1f;
							}
							break;
						}
						Vec3 vec3 = default(Vec3);
						CoordinateSystem axis = Camera.Axis;
						CoordinateSystem axis2 = this.m_context.m_gizmo.Axis;
						float value = Vec3.Dot(axis2.axisX, axis.axisX);
						float value2 = Vec3.Dot(axis2.axisY, axis.axisX);
						if (Math.Abs(value) > Math.Abs(value2))
						{
							vec3 = axis2.axisX * vec.X * (float)Math.Sign(value) + axis2.axisY * vec.Y * (float)Math.Sign(Vec3.Dot(axis2.axisY, axis.axisZ));
						}
						else
						{
							vec3 = axis2.axisY * vec.X * (float)Math.Sign(value2) + axis2.axisX * vec.Y * (float)Math.Sign(Vec3.Dot(axis2.axisX, axis.axisZ));
						}
						vec3 += axis2.axisZ * vec.Z * (float)Math.Sign(Vec3.Dot(axis2.axisZ, axis.axisZ));
						if (keyEventArgs.Shift)
						{
							vec3 *= 0.0025f;
							vec2 *= MathUtils.Deg2Rad(0.25f);
						}
						else
						{
							vec3 *= 0.01f;
							vec2 *= MathUtils.Deg2Rad(1f);
						}
						this.m_context.m_selection.Center = this.m_context.m_gizmo.Position;
						this.m_context.m_selection.MoveTo(this.m_context.m_gizmo.Position + vec3, EditorObjectSelection.MoveMode.MoveNormal);
						this.m_context.m_selection.Rotate(vec2, axis2.ToAngles(), this.m_context.m_gizmo.Position, false);
					}
					break;

				case Editor.KeyEvent.KeyUp:
					if (this.m_keyMoving && keyEventArgs.KeyCode == this.m_keyStart)
					{
						UndoManager.CommitUndo();
						this.m_keyMoving = false;
					}
					if (keyEventArgs.KeyCode == Keys.ControlKey)
					{
						this.m_context.SetGizmoRotationMode(false);
					}
					break;
				}
				return false;
			}
		}
		private class MoveAction : InputBase
		{
			private ToolObject m_context;
			private Vec3 m_startPosition;
			private Vec3 m_virtualStart;
			private Vec3 m_pivot;
			private Gizmo m_refGizmo;
			private GizmoHelper m_gizmoHelper = new GizmoHelper();
			private bool m_snap;
			private float m_snapSize;
			private EditorObject m_snapObject;
			public MoveAction(ToolObject context)
			{
				this.m_context = context;
			}
			public void ClearSnap()
			{
				this.m_snap = false;
				this.m_snapObject = EditorObject.Null;
			}
			public void SetSnap(float snapSize)
			{
				this.m_snap = true;
				this.m_snapSize = snapSize;
				this.m_snapObject = EditorObject.Null;
			}
			public void SetSnap(EditorObject snapObject)
			{
				this.m_snap = true;
				this.m_snapObject = snapObject;
			}
			public bool Start(Vec3 pivot)
			{
				this.m_refGizmo = this.m_context.m_gizmo;
				this.m_gizmoHelper.InitVirtualPlane(this.m_refGizmo.Position, this.m_refGizmo.Axis, this.m_refGizmo.Active);
				if (!this.m_gizmoHelper.GetVirtualPos(out this.m_virtualStart))
				{
					return false;
				}
				this.m_pivot = pivot;
				this.m_startPosition = pivot;
				base.AcquireInput();
				this.m_context.m_selection.SaveState();
				return true;
			}
			public override void OnInputAcquire()
			{
				this.m_context.m_selection.Center = this.m_pivot;
				this.m_context.UpdateSelection();
				UndoManager.RecordUndo();
			}
			public override void OnInputRelease()
			{
				UndoManager.CommitUndo();
				this.m_context.UpdateSelection(true);
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseUp:
					base.ReleaseInput();
					break;

				case Editor.MouseEvent.MouseMove:
					{
						Vec3 v;
						if (this.m_gizmoHelper.GetVirtualPos(out v))
						{
							Vec3 vec = v - this.m_virtualStart;
							if (this.m_snap)
							{
								vec = this.m_refGizmo.Axis.ConvertFromWorld(vec);
								if (!this.m_snapObject.IsValid)
								{
									vec.Snap(this.m_snapSize);
								}
								else
								{
									if (this.m_snapObject.IsLoaded)
									{
										vec.Snap(this.m_snapObject.LocalBounds.Length);
									}
									else
									{
										vec = new Vec3(0f, 0f, 0f);
									}
								}
								vec = this.m_refGizmo.Axis.ConvertToWorld(vec);
							}
							Vec3 vec2 = this.m_startPosition + vec;
							this.m_context.m_selection.LoadState();
							this.m_context.m_selection.MoveTo(vec2, EditorObjectSelection.MoveMode.MoveNormal);
							this.m_context.m_selection.SnapToClosestObjects();
							this.m_pivot = vec2;
							this.m_context.UpdateSelection();
						}
						break;
					}
				}
				return false;
			}
		}
		private class MovePhysicsAction : InputBase
		{
			protected ToolObject m_context;
			protected bool m_delayedMove;
			protected Point m_delayedMoveStart;
			protected bool m_localRotate;
			protected Vec3 m_pivot;
			public MovePhysicsAction(ToolObject context)
			{
				this.m_context = context;
			}
			public virtual bool Start(Vec3 pivot)
			{
				this.m_pivot = pivot;
				this.m_delayedMove = true;
				this.m_delayedMoveStart = Cursor.Position;
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					this.m_localRotate = true;
					Editor.Viewport.CaptureMouse = true;
				}
				base.AcquireInput();
				this.m_context.m_selection.SaveState();
				return true;
			}
			public override void OnInputAcquire()
			{
				Editor.Viewport.CaptureWheel = true;
				this.m_context.m_selection.Center = this.m_pivot;
				this.m_context.UpdateSelection();
				UndoManager.RecordUndo();
			}
			public override void OnInputRelease()
			{
				Editor.Viewport.CaptureWheel = false;
				this.SetLocalRotate(false);
				UndoManager.CommitUndo();
				this.m_context.UpdateSelection(true);
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseUp:
					base.ReleaseInput();
					break;

				case Editor.MouseEvent.MouseMove:
					{
						if (this.m_delayedMove)
						{
							if (Math.Abs(this.m_delayedMoveStart.X - Cursor.Position.X) < 2 && Math.Abs(this.m_delayedMoveStart.Y - Cursor.Position.Y) < 2)
							{
								break;
							}
							if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
							{
								EditorObjectSelection editorObjectSelection = EditorObjectSelection.Create();
								this.m_context.m_selection.Clone(editorObjectSelection, true);
								int num = this.m_context.m_selection.IndexOf(this.m_context.m_gizmoObject);
								this.m_context.SetSelection(editorObjectSelection, (num != -1) ? editorObjectSelection[num] : EditorObject.Null);
							}
							Vec2 normalizedMousePos;
							if (Editor.GetScreenPointFromWorldPos(this.m_pivot, out normalizedMousePos))
							{
								Editor.Viewport.NormalizedMousePos = normalizedMousePos;
							}
							this.m_delayedMove = false;
						}
						Vec3 raySrc;
						Vec3 rayDir;
						Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out raySrc, out rayDir);
						Vec3 vec;
						float num2;
						Vec3 normal;
						if (Editor.RayCastPhysics(raySrc, rayDir, this.m_context.m_selection, out vec, out num2, out normal))
						{
							this.m_context.m_selection.Center = this.m_pivot;
							this.m_context.m_selection.LoadState();
							if (this.m_context.m_selection.Count == 1)
							{
								EditorObject editorObject = this.m_context.m_selection[0];
								if (editorObject.Entry.AutoOrientation)
								{
									Vec3 angles;
									editorObject.ComputeAutoOrientation(ref vec, out angles, normal);
									editorObject.Angles = angles;
								}
							}
							this.m_context.m_selection.MoveTo(vec, EditorObjectSelection.MoveMode.MoveNormal);
							this.m_context.m_selection.SnapToClosestObjects();
							this.m_pivot = vec;
							this.m_context.UpdateSelection();
						}
						break;
					}

				case Editor.MouseEvent.MouseMoveDelta:
					this.m_context.m_selection.LoadState();
					this.m_context.m_selection.RotateCenter(0.025f * (float)mouseEventArgs.X, new Vec3(0f, 0f, 1f));
					this.m_context.m_selection.SaveState();
					this.m_context.m_selection.SnapToClosestObjects();
					break;

				case Editor.MouseEvent.MouseWheel:
					{
						this.m_context.m_selection.LoadState();
						Vec3 center = this.m_context.m_selection.Center;
						center.Z += (float)((0.3f * (float)mouseEventArgs.Delta > 0f) ? 1 : -1);
						this.m_context.m_selection.MoveTo(center, EditorObjectSelection.MoveMode.MoveNormal);
						this.m_context.m_selection.SaveState();
						break;
					}
				}
				return false;
			}
			public override bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
			{
				switch (keyEvent)
				{
				case Editor.KeyEvent.KeyDown:
					if (keyEventArgs.KeyCode == Keys.ControlKey)
					{
						this.SetLocalRotate(true);
						return true;
					}
					break;

				case Editor.KeyEvent.KeyUp:
					if (keyEventArgs.KeyCode == Keys.ControlKey)
					{
						this.SetLocalRotate(false);
						return true;
					}
					break;
				}
				return false;
			}
			private void SetLocalRotate(bool localRotate)
			{
				if (this.m_localRotate == localRotate)
				{
					return;
				}
				if (localRotate)
				{
					this.m_context.m_selection.LoadState();
					this.m_localRotate = true;
					Editor.Viewport.CaptureMouse = true;
					return;
				}
				this.m_context.m_selection.SaveState();
				this.m_localRotate = false;
				Editor.Viewport.CaptureMouse = false;
			}
		}
		private class ParamObjectSelection : Parameter
		{
			protected EditorObjectSelection m_objectSelection;
			protected int m_value = -1;
			protected EditorObject m_editorObject;
			public event EventHandler ValueChanged;
			public event EventHandler ActionPerformed;
			public EditorObjectSelection ObjectSelection
			{
				get
				{
					return this.m_objectSelection;
				}
				set
				{
					this.m_objectSelection = value;
					base.UpdateUIControls();
				}
			}
			public int Value
			{
				get
				{
					return this.m_value;
				}
				set
				{
					this.m_value = value;
					base.UpdateUIControls();
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this, new EventArgs());
					}
				}
			}
			public EditorObject EditorObject
			{
				get
				{
					return this.m_editorObject;
				}
				set
				{
					this.m_editorObject = value;
				}
			}
			public ParamObjectSelection(string display) : base(display)
			{
			}
			protected override Control CreateUIControl()
			{
				ParamObjectSelectionList paramObjectSelectionList = new ParamObjectSelectionList();
				paramObjectSelectionList.Value = this.m_value;
				paramObjectSelectionList.ValueChanged += delegate(object sender, EventArgs e)
				{
					this.Value = ((ParamObjectSelectionList)sender).Value;
				}
				;
				paramObjectSelectionList.PerformAction += delegate(object sender, EventArgs e)
				{
					this.ActionPerformed(sender, e);
				}
				;
				paramObjectSelectionList.CurrentObjectChanged += delegate(object sender, EventArgs e)
				{
					this.EditorObject = ((ParamObjectSelectionList)sender).CurrentObject;
				}
				;
				this.UpdateUIControl(paramObjectSelectionList);
				return paramObjectSelectionList;
			}
			protected override void UpdateUIControl(Control control)
			{
				ParamObjectSelectionList paramObjectSelectionList = (ParamObjectSelectionList)control;
				paramObjectSelectionList.ObjectSelection = this.m_objectSelection;
				paramObjectSelectionList.Value = this.m_value;
			}
		}
		private class AdminMode : ToolObject.Mode
		{
			private ParamObjectAdmin m_objectAdmin = new ParamObjectAdmin("Object Admin");
			public AdminMode(ToolObject context) : base(context)
			{
			}
			public override string GetToolName()
			{
				return "Admin Mode";
			}
			public override Image GetToolImage()
			{
				return Resources.Admin;
			}
			public override string GetContextHelp()
			{
				return "This tool is used only for development purposes! It will not be included in retail, so do not make tests on it.";
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_objectAdmin;
				yield break;
			}
			public override IParameter GetMainParameter()
			{
				return this.m_objectAdmin;
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				if (mouseEvent == Editor.MouseEvent.MouseDown)
				{
					ToolObject.SelectAction selectAction = new ToolObject.SelectAction(this.m_context);
					selectAction.Start();
				}
				return base.OnMouseEvent(mouseEvent, mouseEventArgs);
			}
		}
		public class AddMode : ToolObject.Mode
		{
			private class ParamInventoryObject : Parameter
			{
				protected ObjectInventory.Entry m_folder;
				protected ObjectInventory.Entry m_value;
				protected ObjectInventory.Entry m_searchDirectory;
				public event EventHandler FolderChanged;
				public event EventHandler ValueChanged;
				public event EventHandler SearchChanged;
				public ObjectInventory.Entry Folder
				{
					get
					{
						return this.m_folder;
					}
				}
				public ObjectInventory.Entry Value
				{
					get
					{
						return this.m_value;
					}
					set
					{
						this.m_value = value;
						base.UpdateUIControls();
					}
				}
				public ParamInventoryObject(string display) : base(display)
				{
					this.m_searchDirectory = (ObjectInventory.Entry)ObjectInventory.Instance.CreateDirectory();
				}
				public void UpdateSize(AABB size)
				{
					foreach (Control current in this.m_uiControls.Keys)
					{
						ParamObjectInventoryTree paramObjectInventoryTree = current as ParamObjectInventoryTree;
						if (paramObjectInventoryTree != null)
						{
							paramObjectInventoryTree.ObjectSize = size;
						}
					}
				}
				public int GetFirstVisibleIndex()
				{
					foreach (Control current in this.m_uiControls.Keys)
					{
						ParamObjectInventoryTree paramObjectInventoryTree = current as ParamObjectInventoryTree;
						if (paramObjectInventoryTree != null)
						{
							return paramObjectInventoryTree.ObjectListFirstVisibleIndex;
						}
					}
					return -1;
				}
				public void SetFirstVisibleIndex(int visibleIndex)
				{
					foreach (Control current in this.m_uiControls.Keys)
					{
						ParamObjectInventoryTree paramObjectInventoryTree = current as ParamObjectInventoryTree;
						if (paramObjectInventoryTree != null)
						{
							paramObjectInventoryTree.ObjectListFirstVisibleIndex = visibleIndex;
						}
					}
				}
				protected override Control CreateUIControl()
				{
					ParamObjectInventoryTree paramObjectInventoryTree = new ParamObjectInventoryTree(this.m_searchDirectory);
					paramObjectInventoryTree.ValueChanged += delegate(object sender, EventArgs e)
					{
						this.OnValueChanged(((ParamObjectInventoryTree)sender).Value);
					}
					;
					paramObjectInventoryTree.FolderChanged += delegate(object sender, EventArgs e)
					{
						this.OnFolderChanged(((ParamObjectInventoryTree)sender).Folder);
					}
					;
					paramObjectInventoryTree.SearchChanged += delegate(object sender, EventArgs e)
					{
						this.OnSearchChanged(((ParamObjectInventoryTree)sender).SearchCriteria);
					}
					;
					this.UpdateUIControl(paramObjectInventoryTree);
					return paramObjectInventoryTree;
				}
				protected override void UpdateUIControl(Control control)
				{
					ParamObjectInventoryTree paramObjectInventoryTree = (ParamObjectInventoryTree)control;
					paramObjectInventoryTree.Value = this.m_value;
				}
				protected void OnValueChanged(ObjectInventory.Entry value)
				{
					this.m_value = value;
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this, new EventArgs());
					}
				}
				protected void OnFolderChanged(ObjectInventory.Entry folder)
				{
					this.m_folder = folder;
					if (this.FolderChanged != null)
					{
						this.FolderChanged(this, new EventArgs());
					}
				}
				protected void OnSearchChanged(string criteria)
				{
					ObjectInventory.Instance.SearchInventory(criteria, this.m_searchDirectory);
					if (this.SearchChanged != null)
					{
						this.SearchChanged(this, new EventArgs());
					}
				}
			}
			private ToolObject.AddMode.ParamInventoryObject m_paramObject = new ToolObject.AddMode.ParamInventoryObject(null);
			private ParamCheckButton m_pasteFromClipboard = new ParamCheckButton(Localizer.Localize("PARAM_SELECTION_PASTE_CLIPBOARD"));
			private ParamBool m_paramBatchAdd = new ParamBool(Localizer.Localize("PARAM_BATCH_ADD"), false);
			private ParamBool m_paramPreview = new ParamBool(Localizer.Localize("PARAM_OBJECT_PREVIEW"), false);
			private int m_firstVisibleIndexBkp = -1;
			private bool m_localRotate;
			private bool m_newObjectPending;
			private bool m_newObjectValid;
			private float m_newObjectAngle;
			public AddMode(ToolObject context) : base(context)
			{
				this.m_paramObject.ValueChanged += new EventHandler(this.paramObject_ValueChanged);
				this.m_paramObject.FolderChanged += new EventHandler(this.paramObject_FolderChanged);
				ParamCheckButton expr_8F = this.m_pasteFromClipboard;
				expr_8F.ActivateCallback = (ParamCheckButton.ButtonDelegate)Delegate.Combine(expr_8F.ActivateCallback, new ParamCheckButton.ButtonDelegate(this.pasteFromClipboard_Activate));
				ParamCheckButton expr_B6 = this.m_pasteFromClipboard;
				expr_B6.DeactivateCallback = (ParamCheckButton.ButtonDelegate)Delegate.Combine(expr_B6.DeactivateCallback, new ParamCheckButton.ButtonDelegate(this.pasteFromClipboard_Deactivate));
				this.m_paramPreview.ValueChanged += new EventHandler(this.paramPreview_ValueChanged);
			}
			public override string GetToolName()
			{
				return "Add object";
			}
			public override Image GetToolImage()
			{
				return Resources.Object_Add;
			}
			public override string GetContextHelp()
			{
				return Localizer.Localize("HELP_TOOL_ADDOBJECT");
			}
			public override IEnumerable<IParameter> GetParameters()
			{
				yield return this.m_paramObject;
				yield return this.m_pasteFromClipboard;
				yield return this.m_paramBatchAdd;
				yield return this.m_paramPreview;
				yield break;
			}
			public override IParameter GetMainParameter()
			{
				return this.m_paramObject;
			}
			private void paramObject_ValueChanged(object sender, EventArgs e)
			{
				this.SetNewObject();
			}
			private void pasteFromClipboard_Activate()
			{
				this.SetNewObject();
			}
			private void pasteFromClipboard_Deactivate()
			{
				this.SetNewObject();
			}
			private void paramObject_FolderChanged(object sender, EventArgs e)
			{
				this.UpdatePreview();
			}
			private void paramPreview_ValueChanged(object sender, EventArgs e)
			{
				this.UpdatePreview();
			}
			public void SetGotoObject(ObjectInventory.Entry entry)
			{
				if (!entry.IsValid)
				{
					return;
				}
				this.m_paramObject.Value = entry;
				this.SetNewObject();
			}
			public override void Activate()
			{
				this.m_context.ClearSelection();
				this.m_newObjectAngle = 0f;
				this.ClearObjectParam();
				this.SetNewObject();
				this.UpdatePreview();
			}
			public override void Deactivate()
			{
				this.m_firstVisibleIndexBkp = this.m_paramObject.GetFirstVisibleIndex();
				this.ClearPreview();
				this.ClearObjectParam();
				ObjectRenderer.Clear();
				this.m_context.ClearSelection();
			}
			public override void AfterActivate()
			{
				this.m_paramObject.SetFirstVisibleIndex(this.m_firstVisibleIndexBkp);
			}
			public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
			{
				switch (mouseEvent)
				{
				case Editor.MouseEvent.MouseDown:
					if (!this.m_paramPreview.Value && (Control.ModifierKeys & Keys.Control) != Keys.None)
					{
						this.m_localRotate = true;
						Editor.Viewport.CaptureMouse = true;
						return false;
					}
					return false;

				case Editor.MouseEvent.MouseUp:
					if (!this.m_paramPreview.Value)
					{
						if (this.m_localRotate)
						{
							this.m_localRotate = false;
							Editor.Viewport.CaptureMouse = false;
							return false;
						}
						if (!this.m_newObjectValid)
						{
							return false;
						}
						ObjectInventory.Entry value = this.m_paramObject.Value;
						if (value != null && value.IsAI && !EditorDocument.NavmeshEnabled)
						{
							if (LocalizedMessageBox.Show(MainForm.Instance, Localizer.LocalizeCommon("EDITOR_NAVMESH_PROMPT"), Localizer.Localize("EDITOR_CONFIRMATION"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), null, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
							{
								return false;
							}
							EditorDocument.NavmeshEnabled = true;
						}
						UndoManager.RecordUndo();
						EditorObjectSelection newSelection = EditorObjectSelection.Create();
						this.m_context.m_selection.Clone(newSelection, true);
						newSelection.Dispose();
						foreach (EditorObject current in this.m_context.m_selection.GetObjects())
						{
							ObjectManager.OnObjectAddedFromTool(current);
						}
						UndoManager.CommitUndo();
						if (!this.m_paramBatchAdd.Value)
						{
							this.ClearObjectParam();
							return false;
						}
						return false;
					}
					else
					{
						ObjectInventory.Entry entryFromScreenPoint = ObjectLegoBox.GetEntryFromScreenPoint(Editor.Viewport.NormalizedMousePos);
						if (entryFromScreenPoint.IsValid)
						{
							this.m_paramObject.Value = entryFromScreenPoint;
							this.m_paramPreview.Value = false;
							this.UpdatePreview();
							return false;
						}
						return false;
					}
					break;

				case Editor.MouseEvent.MouseMove:
					{
						if (this.m_paramPreview.Value || this.m_context.m_selection.Count <= 0)
						{
							return false;
						}
						this.m_newObjectValid = false;
						Vec3 raySrc;
						Vec3 rayDir;
						Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out raySrc, out rayDir);
						Vec3 pos;
						float num;
						Vec3 normal;
						if (Editor.RayCastPhysics(raySrc, rayDir, this.m_context.m_selection, out pos, out num, out normal))
						{
							this.m_context.m_selection.LoadState();
							foreach (EditorObject current2 in this.m_context.m_selection.GetObjects())
							{
								if (current2.Entry.AutoOrientation)
								{
									Vec3 angles;
									current2.ComputeAutoOrientation(ref pos, out angles, normal);
									current2.Angles = angles;
								}
							}
							this.m_context.m_selection.MoveTo(pos, EditorObjectSelection.MoveMode.MoveNormal);
							this.m_context.m_selection.SaveState();
							this.m_context.m_selection.SnapToClosestObjects();
							this.m_newObjectValid = true;
						}
						using (IEnumerator<EditorObject> enumerator3 = this.m_context.m_selection.GetObjects().GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								EditorObject current3 = enumerator3.Current;
								current3.Visible = this.m_newObjectValid;
							}
							return false;
						}
						break;
					}

				case Editor.MouseEvent.MouseMoveDelta:
					break;

				default:
					return false;
				}
				if (!this.m_paramPreview.Value && this.m_context.m_selection.Count > 0)
				{
					float num2 = 0.025f * (float)mouseEventArgs.X;
					this.m_context.m_selection.LoadState();
					this.m_newObjectAngle += num2;
					this.m_context.m_selection.RotateCenter(num2, new Vec3(0f, 0f, 1f));
					this.m_context.m_selection.SaveState();
					this.m_context.m_selection.SnapToClosestObjects();
				}
				return false;
			}
			public override bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
			{
				if (keyEvent == Editor.KeyEvent.KeyUp && keyEventArgs.KeyCode == Keys.Escape && this.m_context.m_selection.Count > 0)
				{
					this.ClearObjectParam();
					return true;
				}
				return false;
			}
			public override void Update(float dt)
			{
				if (this.m_newObjectPending)
				{
					bool flag = true;
					foreach (EditorObject current in this.m_context.m_selection.GetObjects())
					{
						if (!current.IsLoaded)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						this.m_paramObject.UpdateSize(this.m_context.m_selection.WorldBounds);
						this.m_newObjectPending = false;
					}
				}
				this.UpdateNewObject();
			}
			private void ClearNewObject()
			{
				if (this.m_context.m_selection.Count > 0)
				{
					foreach (EditorObject current in this.m_context.m_selection.GetObjects())
					{
						current.Destroy();
					}
					this.m_context.ClearSelection();
					this.m_newObjectPending = false;
					this.m_newObjectValid = false;
				}
			}
			private void SetNewObject(EditorObjectSelection selection)
			{
				this.ClearNewObject();
				this.m_newObjectPending = true;
				this.m_newObjectValid = false;
				selection.RotateCenter(this.m_newObjectAngle, new Vec3(0f, 0f, 1f));
				foreach (EditorObject current in selection.GetObjects())
				{
					current.Visible = false;
				}
				this.m_context.SetSelection(selection, EditorObject.Null);
				this.m_context.m_selection.SaveState();
				this.UpdateNewObject();
				if (ObjectViewer.Active)
				{
					this.UpdatePreview();
				}
			}
			private void SetNewObject()
			{
				EditorObjectSelection newObject = EditorObjectSelection.Create();
				if (this.m_pasteFromClipboard.Checked)
				{
					bool flag = false;
					string text = Clipboard.GetText();
					if (text != null)
					{
						flag = newObject.LoadFromXml(text, true);
					}
					if (!flag)
					{
						this.m_pasteFromClipboard.Checked = false;
					}
				}
				if (!this.m_pasteFromClipboard.Checked && this.m_paramObject.Value != null && !this.m_paramObject.Value.IsDirectory)
				{
					EditorObject obj = EditorObject.CreateFromEntry(this.m_paramObject.Value, false);
					newObject.AddObject(obj);
				}
				this.SetNewObject(newObject);
			}
			private void UpdateNewObject()
			{
				if (this.m_context.m_selection.Count == 0)
				{
					return;
				}
				if (this.m_paramPreview.Value)
				{
					using (IEnumerator<EditorObject> enumerator = this.m_context.m_selection.GetObjects().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							EditorObject current = enumerator.Current;
							current.Visible = true;
							current.HighlightState = false;
						}
						return;
					}
				}
				foreach (EditorObject current2 in this.m_context.m_selection.GetObjects())
				{
					if (!Editor.Viewport.MouseOver)
					{
						current2.Visible = false;
					}
					current2.HighlightState = true;
				}
			}
			private void ClearObjectParam()
			{
				this.ClearNewObject();
				if (this.m_paramObject.Value != null && !this.m_paramObject.Value.IsDirectory)
				{
					this.m_paramObject.Value = (ObjectInventory.Entry)this.m_paramObject.Value.Parent;
				}
				this.m_pasteFromClipboard.Checked = false;
			}
			private void ClearPreview()
			{
				this.SetNewObject();
				ObjectLegoBox.Active = false;
			}
			private void SetPreview()
			{
				this.ClearNewObject();
				ObjectLegoBox.Active = true;
				ObjectInventory.Entry folder = this.m_paramObject.Folder;
				ObjectLegoBox.ClearEntries();
				if (folder != null)
				{
					Inventory.Entry[] children = folder.Children;
					for (int i = 0; i < children.Length; i++)
					{
						ObjectInventory.Entry entry = (ObjectInventory.Entry)children[i];
						ObjectLegoBox.AddEntry(entry);
					}
				}
				ObjectLegoBox.CreateLegoBox();
			}
			private void UpdatePreview()
			{
				if (this.m_paramPreview.Value)
				{
					this.SetPreview();
					return;
				}
				this.ClearPreview();
			}
		}
		private static ToolObject s_instance;
		private ToolObject.SelectMode m_selectMode;
		private ToolObject.MoveMode m_moveMode;
		private ToolObject.RotateMode m_rotateMode;
		private ToolObject.AddMode m_addMode;
		private ToolObject.Mode m_mode;
		private ParamEnumBase<ToolObject.Mode> m_paramMode = new ParamEnumBase<ToolObject.Mode>(Localizer.Localize("PARAM_MODE"), null, ParamEnumUIType.Buttons);
		private ParamText m_textSelected = new ParamText("");
		private ParamBool m_paramGroupSelection = new ParamBool(Localizer.Localize("PARAM_OBJECT_GROUP_SELECTION"), false);
		private ParamBool m_paramMagicWand = new ParamBool(Localizer.Localize("PARAM_OBJECT_MAGIC_WAND"), false);
		private ParamEnum<AxisType> m_paramAxisType = new ParamEnum<AxisType>(Localizer.Localize("PARAM_AXIS_TYPE"), AxisType.Local, ParamEnumUIType.Buttons);
		private ParamButton m_actionCopyClipboard = new ParamButton(Localizer.Localize("PARAM_SELECTION_COPY_CLIPBOARD"), null);
		private ParamButton m_actionDelete = new ParamButton(Localizer.Localize("PARAM_SELECTION_DELETE"), null);
		private ParamButton m_actionFreeze = new ParamButton(Localizer.Localize("PARAM_SELECTION_FREEZE"), null);
		private ParamButton m_actionUnfreeze = new ParamButton(Localizer.Localize("PARAM_SELECTION_UNFREEZE"), null);
		private ToolObject.ParamObjectSelection m_paramObjectSelection = new ToolObject.ParamObjectSelection(null);
		private ParamButton m_actionGotoObject = new ParamButton(Localizer.Localize("PARAM_GOTO_OBJECT"), null);
		private EditorObjectSelection m_selection;
		private Gizmo m_gizmo;
		private bool m_gizmoActive;
		private EditorObject m_gizmoObject;
		private bool m_gizmoEnabled;
		private bool m_gizmoRotationMode;
		public event EventHandler ContextHelpChanged;
		public event EventHandler ParamsChanged;
		public static ToolObject Instance
		{
			get
			{
				return ToolObject.s_instance;
			}
		}
		public ToolObject.SelectMode SelectModeObj
		{
			get
			{
				return this.m_selectMode;
			}
		}
		public ToolObject.MoveMode MoveModeObj
		{
			get
			{
				return this.m_moveMode;
			}
		}
		public ToolObject.RotateMode RotateModeObj
		{
			get
			{
				return this.m_rotateMode;
			}
		}
		public ToolObject.AddMode AddModeObj
		{
			get
			{
				return this.m_addMode;
			}
		}
		public EditorObjectSelection Selection
		{
			get
			{
				return this.m_selection;
			}
		}
		public ToolObject()
		{
			ToolObject.s_instance = this;
			this.m_selectMode = new ToolObject.SelectMode(this);
			this.m_moveMode = new ToolObject.MoveMode(this);
			this.m_rotateMode = new ToolObject.RotateMode(this);
			this.m_addMode = new ToolObject.AddMode(this);
			this.m_paramMode.Names = new string[]
			{
				Localizer.Localize("TOOL_OBJECT_MODE_SELECT") + " (1)",
				Localizer.Localize("TOOL_OBJECT_MODE_MOVE") + " (2)",
				Localizer.Localize("TOOL_OBJECT_MODE_ROTATE") + " (3)",
				Localizer.Localize("TOOL_OBJECT_MODE_ADD") + " (4)"
			};
			this.m_paramMode.Values = new ToolObject.Mode[]
			{
				this.m_selectMode,
				this.m_moveMode,
				this.m_rotateMode,
				this.m_addMode
			};
			this.m_paramMode.Images = new Image[]
			{
				this.m_selectMode.GetToolImage(),
				this.m_moveMode.GetToolImage(),
				this.m_rotateMode.GetToolImage(),
				this.m_addMode.GetToolImage()
			};
			this.m_paramMode.ValueChanged += new EventHandler(this.editTool_ValueChanged);
			this.m_paramMode.Value = this.m_addMode;
			this.m_paramAxisType.Names = new string[]
			{
				Localizer.Localize("PARAM_AXIS_LOCAL"),
				Localizer.Localize("PARAM_AXIS_WORLD")
			};
			this.m_paramAxisType.ValueChanged += new EventHandler(this.axisType_ValueChanged);
			this.m_actionCopyClipboard.Callback = new ParamButton.ButtonDelegate(this.action_CopyClipboard);
			this.m_actionDelete.Callback = new ParamButton.ButtonDelegate(this.action_Delete);
			this.m_actionFreeze.Callback = new ParamButton.ButtonDelegate(this.action_Freeze);
			this.m_actionUnfreeze.Callback = new ParamButton.ButtonDelegate(this.action_Unfreeze);
			this.m_actionGotoObject.Callback = new ParamButton.ButtonDelegate(this.action_GotoObject);
			this.m_paramObjectSelection.ActionPerformed += new EventHandler(this.action_GotoObject);
			this.m_paramObjectSelection.ValueChanged += new EventHandler(this.action_SelectionValueChanged);
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_OBJECT");
		}
		public Image GetToolImage()
		{
			return Resources.Object_Edit;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramMode;
			if (this.m_paramMode.Value != null)
			{
				foreach (IParameter current in this.m_paramMode.Value.GetParameters())
				{
					yield return current;
				}
			}
			yield break;
		}
		public IParameter GetMainParameter()
		{
			if (this.m_paramMode.Value != null)
			{
				return this.m_paramMode.Value.GetMainParameter();
			}
			return null;
		}
		public string GetContextHelp()
		{
			return this.m_paramMode.Value.GetContextHelp();
		}
		public void UpdateContextHelp()
		{
			if (this.ContextHelpChanged != null)
			{
				this.ContextHelpChanged(this, null);
			}
		}
		private void UpdateParams()
		{
			bool flag = this.m_selection.Count > 0;
			this.m_actionCopyClipboard.Enabled = flag;
			this.m_actionDelete.Enabled = flag;
			this.m_actionFreeze.Enabled = flag;
			this.m_textSelected.DisplayName = this.m_selection.Count + " " + ((this.m_selection.Count > 1) ? Localizer.Localize("PARAM_OBJECTS_SELECTED_COUNT") : Localizer.Localize("PARAM_OBJECT_SELECTED_COUNT"));
			if (this.m_paramMode.Value != null)
			{
				this.m_paramMode.Value.UpdateParams();
			}
			this.m_paramObjectSelection.ObjectSelection = this.m_selection;
			this.m_actionGotoObject.Enabled = (flag && this.m_paramObjectSelection.Value != -1);
		}
		private void ClearMode(ToolObject.Mode mode)
		{
			mode.Deactivate();
			Editor.PopInput(mode);
		}
		private void SetMode(ToolObject.Mode mode)
		{
			this.m_mode = mode;
			Editor.PushInput(mode);
			mode.Activate();
			if (this.ParamsChanged != null)
			{
				this.ParamsChanged(this, null);
			}
			this.UpdateContextHelp();
			mode.AfterActivate();
		}
		private void SwitchMode(ToolObject.Mode prevMode, ToolObject.Mode mode)
		{
			this.ClearMode(prevMode);
			this.m_paramMode.Value = mode;
			this.SetMode(mode);
		}
		public void SwitchMode(ToolObject.Mode mode)
		{
			this.SwitchMode(this.m_paramMode.Value, mode);
		}
		private void editTool_ValueChanged(object sender, EventArgs e)
		{
			this.SwitchMode(this.m_mode, this.m_paramMode.Value);
		}
		private void axisType_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateSelection();
		}
		private void action_CopyClipboard()
		{
			string text = this.m_selection.SaveToXml();
			Clipboard.SetText(text);
		}
		private void action_Delete()
		{
			this.DeleteSelection();
		}
		private void action_Freeze()
		{
			for (int i = 0; i < this.m_selection.Count; i++)
			{
				EditorObject editorObject = this.m_selection[i];
				editorObject.Frozen = true;
			}
			this.ClearSelection();
		}
		private void action_Unfreeze()
		{
			ObjectManager.UnfreezeObjects();
		}
		private void action_GotoObject(object sender, EventArgs e)
		{
			this.action_GotoObject();
		}
		private void action_GotoObject()
		{
			if (!this.m_paramObjectSelection.EditorObject.IsValid || (this.m_paramObjectSelection.EditorObject.Entry.IsValid && this.m_paramObjectSelection.EditorObject.Entry.IsObsolete))
			{
				return;
			}
			this.SwitchMode(this.m_mode, this.m_addMode);
			this.m_addMode.SetGotoObject(this.m_paramObjectSelection.EditorObject.Entry);
		}
		private void action_SelectionValueChanged(object sender, EventArgs e)
		{
			this.m_actionGotoObject.Enabled = (this.m_paramObjectSelection.Value != -1);
		}
		public void Activate()
		{
			this.CreateSelection();
			this.SetMode(this.m_paramMode.Value);
		}
		public void Deactivate()
		{
			this.ClearMode(this.m_paramMode.Value);
			this.DestroySelection();
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			if (mouseEvent == Editor.MouseEvent.MouseMove)
			{
				this.TestGizmo();
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
					this.SwitchMode(this.m_selectMode);
					return true;

				case Keys.D2:
					this.SwitchMode(this.m_moveMode);
					return true;

				case Keys.D3:
					this.SwitchMode(this.m_rotateMode);
					return true;

				case Keys.D4:
					this.SwitchMode(this.m_addMode);
					break;
				}
				break;

			case Editor.KeyEvent.KeyUp:
				{
					Keys keyCode = keyEventArgs.KeyCode;
					if (keyCode != Keys.Escape)
					{
						if (keyCode == Keys.Delete)
						{
							this.DeleteSelection();
							return true;
						}
					}
					else
					{
						if (this.m_selection.Count > 0)
						{
							this.ClearSelection();
							return true;
						}
					}
					break;
				}
			}
			return false;
		}
		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			if (eventType == EditorEventUndo.TypeId)
			{
				this.ClearSelectionState();
				this.m_selection.RemoveInvalidObjects();
				this.m_selection.ComputeCenter();
				this.UpdateSelectionState();
				this.UpdateSelection();
			}
		}
		public void Update(float dt)
		{
			if (this.m_gizmo.IsValid)
			{
				this.UpdateGizmo();
				if (this.m_gizmoEnabled)
				{
					this.m_gizmo.Redraw();
				}
				else
				{
					this.m_gizmo.Hide();
				}
			}
			if (this.m_paramGroupSelection.Value)
			{
				AABB worldBounds = this.m_selection.WorldBounds;
				Vec3 length = worldBounds.Length;
				Vec3 pos = worldBounds.min + length * 0.5f;
				pos.Z = worldBounds.min.Z;
				Render.DrawWireBoxFromBottomZ(pos, length, 0.005f);
			}
		}
		private void CreateSelection()
		{
			this.m_selection = EditorObjectSelection.Create();
			this.UpdateSelection();
		}
		private void DestroySelection()
		{
			this.ClearSelection();
			this.m_selection.Dispose();
			this.m_paramObjectSelection.ObjectSelection = this.m_selection;
		}
		private void ClearSelectionState()
		{
			for (int i = 0; i < this.m_selection.Count; i++)
			{
				EditorObject editorObject = this.m_selection[i];
				editorObject.HighlightState = false;
			}
		}
		private void UpdateSelectionState()
		{
			for (int i = 0; i < this.m_selection.Count; i++)
			{
				EditorObject editorObject = this.m_selection[i];
				editorObject.HighlightState = true;
			}
		}
		private void ClearSelection()
		{
			this.ClearSelectionState();
			this.ClearGizmo();
			this.m_selection.Clear();
			this.UpdateSelection();
		}
		public void SetSelection(EditorObjectSelection selection, EditorObject gizmoObject)
		{
			this.ClearSelectionState();
			this.m_selection.Dispose();
			this.m_selection = selection;
			this.m_selection.ComputeCenter();
			if (!this.m_selection.Contains(gizmoObject))
			{
				gizmoObject = EditorObject.Null;
			}
			if (!gizmoObject.IsValid && this.m_selection.Count > 0)
			{
				gizmoObject = this.m_selection[0];
			}
			if (gizmoObject.IsValid)
			{
				this.SetupGizmo(gizmoObject);
			}
			else
			{
				this.ClearGizmo();
			}
			this.UpdateSelectionState();
			this.UpdateSelection();
			this.m_paramObjectSelection.Value = -1;
		}
		private void UpdateSelection()
		{
			this.UpdateSelection(false);
		}
		private void UpdateSelection(bool updateCenter)
		{
			if (updateCenter)
			{
				this.m_selection.ComputeCenter();
			}
			this.UpdateGizmo();
			this.UpdateParams();
		}
		private void DeleteSelection()
		{
			if (this.m_selection.Count == 0)
			{
				return;
			}
			UndoManager.RecordUndo();
			this.m_selection.Delete();
			UndoManager.CommitUndo();
			this.UpdateSelection();
		}
		private void SelectObject(EditorObjectSelection selection, EditorObject obj)
		{
			bool flag = (Control.ModifierKeys & Keys.Control) != Keys.None;
			Keys arg_17_0 = Control.ModifierKeys;
			bool flag2 = (Control.ModifierKeys & Keys.Alt) != Keys.None;
			if (this.m_paramMagicWand.Value)
			{
				using (EditorObjectSelection selection2 = EditorObjectSelection.Create())
				{
					ObjectManager.GetObjectsFromMagicWand(selection2, obj);
					if (flag)
					{
						selection.ToggleSelection(selection2);
					}
					else
					{
						if (flag2)
						{
							selection.RemoveSelection(selection2);
						}
						else
						{
							selection.AddSelection(selection2);
						}
					}
				}
				return;
			}
			if (flag)
			{
				selection.ToggleObject(obj);
				return;
			}
			if (flag2)
			{
				selection.RemoveObject(obj);
				return;
			}
			selection.AddObject(obj);
		}
		private void ClearGizmo()
		{
			if (this.m_gizmo.IsValid)
			{
				this.m_gizmo.Dispose();
				this.m_gizmo = Gizmo.Null;
			}
			this.m_gizmoObject = EditorObject.Null;
		}
		private void SetupGizmo(EditorObject gizmoObject)
		{
			this.ClearGizmo();
			this.m_gizmo = Gizmo.Create();
			this.m_gizmo.RotationMode = this.m_gizmoRotationMode;
			this.m_gizmoObject = gizmoObject;
			this.UpdateGizmo();
			this.TestGizmo();
		}
		private void UpdateGizmo()
		{
			if (!this.m_gizmo.IsValid)
			{
				return;
			}
			if (this.m_selection.Count == 0)
			{
				this.ClearGizmo();
				return;
			}
			if (!this.m_paramGroupSelection.Value)
			{
				CoordinateSystem coordinateSystem = CoordinateSystem.FromAngles(this.m_gizmoObject.Angles);
				this.m_gizmo.Axis = ((this.m_paramAxisType.Value == AxisType.World) ? CoordinateSystem.Standard : coordinateSystem);
				this.m_gizmo.Position = this.m_gizmoObject.Position;
				return;
			}
			this.m_gizmo.Axis = CoordinateSystem.Standard;
			this.m_gizmo.Position = this.m_selection.GetComputeCenter();
		}
		private void TestGizmo()
		{
			if (this.m_gizmo.IsValid && this.m_gizmoEnabled)
			{
				Vec3 raySrc;
				Vec3 rayDir;
				Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out raySrc, out rayDir);
				Axis axis = this.m_gizmo.HitTest(raySrc, rayDir);
				if (this.m_gizmo.Active != axis)
				{
					this.m_gizmo.Active = axis;
				}
				this.m_gizmoActive = (axis != Axis.None);
				return;
			}
			this.m_gizmoActive = false;
		}
		private void EnableGizmo(bool enable)
		{
			this.m_gizmoEnabled = enable;
		}
		private void SetGizmoRotationMode(bool enable)
		{
			this.m_gizmoRotationMode = enable;
			if (this.m_gizmo.IsValid)
			{
				this.m_gizmo.RotationMode = enable;
			}
		}
	}
}
