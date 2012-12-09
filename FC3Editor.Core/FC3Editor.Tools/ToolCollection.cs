using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolCollection : ToolPaint
	{
		private ParamCollection m_paramCollection = new ParamCollection(null);
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_COLLECTIONS");
		}
		public override Image GetToolImage()
		{
			return Resources.Collection;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_square;
			yield return this.m_radius;
			yield return this.m_paramCollection;
			yield break;
		}
		public override IParameter GetMainParameter()
		{
			return this.m_paramCollection;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				base.GetPaintContextHelp(),
				"\r\n",
				base.GetShortcutContextHelp(),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_COLLECTION")
			});
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			int num;
			if (Control.ModifierKeys != Keys.Control)
			{
				num = this.m_paramCollection.Value;
				if (num == -1)
				{
					return;
				}
				CollectionInventory.Entry collectionEntryFromId = CollectionManager.GetCollectionEntryFromId(num);
				if (!collectionEntryFromId.IsValid)
				{
					return;
				}
			}
			else
			{
				num = CollectionManager.EmptyCollectionId;
			}
			CollectionManipulator.Paint(pos, num, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			CollectionManipulator.Paint_End();
		}
		public override void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			if (eventType == EditorEventUndo.TypeId)
			{
				this.m_paramCollection.UpdateUIControls();
			}
		}
	}
}
