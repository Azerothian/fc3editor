using FC3Editor.Nomad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class NomadCodeBox : NomadTextBox
	{
		public interface IIcon : IDisposable
		{
			void Draw(Graphics g, Rectangle rect);
		}
		private Dictionary<int, Image> m_icons = new Dictionary<int, Image>();
		private CodeHelper m_codeHelper;
		private NomadTextBox.Selection m_codeHelperRange;
		private static Dictionary<string, string> s_keywords;
		private static string[] s_keywordList;
		private static Dictionary<string, Wilderness.FunctionDef> s_functions;
		static NomadCodeBox()
		{
			NomadCodeBox.s_keywords = new Dictionary<string, string>();
			NomadCodeBox.s_keywordList = new string[]
			{
				"if",
				"then",
				"else",
				"end"
			};
			NomadCodeBox.s_functions = new Dictionary<string, Wilderness.FunctionDef>();
			string[] array = NomadCodeBox.s_keywordList;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				NomadCodeBox.s_keywords.Add(text, text);
			}
		}
		protected override void OnPaintMargin(PaintEventArgs e)
		{
			base.OnPaintMargin(e);
			int line = this.m_origin.line;
			int num = this.m_origin.line + base.NumVisibleLines;
			if (num > this.m_lines.Count)
			{
				num = this.m_lines.Count;
			}
			int num2 = Math.Min(base.LineHeight, base.LeftMarginWidth);
			int num3 = 0;
			for (int i = line; i < num; i++)
			{
				NomadCodeBox.IIcon icon = (NomadCodeBox.IIcon)this.m_lines[i].tag;
				if (icon != null)
				{
					icon.Draw(e.Graphics, new Rectangle(0, num3, num2, num2));
				}
				num3 += base.LineHeight;
			}
		}
		public NomadCodeBox.IIcon GetIcon(int line)
		{
			return (NomadCodeBox.IIcon)this.m_lines[line].tag;
		}
		public void SetIcon(int line, NomadCodeBox.IIcon icon)
		{
			this.m_lines[line].tag = icon;
			base.Invalidate();
		}
		public void ClearIcons()
		{
			this.m_lines.ClearTags();
			base.Invalidate();
		}
		protected string GetToken(string s, int index)
		{
			index = base.SkipSpaceEnd(s, index);
			int num = base.FindWordEnd(s, index);
			if (num == index)
			{
				return null;
			}
			if (num - index == 1 && index < s.Length - 1 && s[index] == '-' && s[index + 1] == '-')
			{
				return "--";
			}
			return s.Substring(index, num - index);
		}
		protected override void DrawTextFormat(Graphics g, IntPtr hFont, NomadTextBox.Position position, int x, int y, string s)
		{
			Color color = this.ForeColor;
			int i;
			int num;
			for (i = 0; i < s.Length; i = num)
			{
				i = base.SkipSpaceEnd(s, i);
				string token = this.GetToken(s, i);
				if (token == null)
				{
					break;
				}
				num = i + token.Length;
				if (token == "--")
				{
					x += base.DrawText(g, hFont, position, x, y, s.Substring(position.column, i - position.column), color);
					color = Color.Green;
					position.column = i;
					i = s.Length;
					break;
				}
				Color color2;
				if (NomadCodeBox.s_keywords.ContainsKey(token))
				{
					color2 = Color.Blue;
				}
				else
				{
					if (NomadCodeBox.s_functions.ContainsKey(token))
					{
						color2 = Color.Maroon;
					}
					else
					{
						color2 = this.ForeColor;
					}
				}
				if (color != color2)
				{
					x += base.DrawText(g, hFont, position, x, y, s.Substring(position.column, i - position.column), color);
					color = color2;
					position.column = i;
				}
			}
			x += base.DrawText(g, hFont, position, x, y, s.Substring(position.column, i - position.column), color);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			switch (e.KeyChar)
			{
			case '(':
				this.ShowCodeHelper();
				return;

			case ')':
				this.HideCodeHelper();
				return;

			default:
				return;
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode != Keys.Return)
			{
				if (keyCode != Keys.Escape)
				{
					if (keyCode == Keys.Space && (Control.ModifierKeys & Keys.Control) != Keys.None && (Control.ModifierKeys & Keys.Shift) != Keys.None)
					{
						this.ShowCodeHelper();
						return;
					}
				}
				else
				{
					this.HideCodeHelper();
				}
			}
			else
			{
				this.HideCodeHelper();
			}
			base.OnKeyDown(e);
		}
		protected override void OnContentChanged(NomadTextBox.Position oldRef, NomadTextBox.Position newRef)
		{
			base.UpdatePosition(ref this.m_codeHelperRange.start, oldRef, newRef);
			base.UpdatePosition(ref this.m_codeHelperRange.end, oldRef, newRef);
			base.OnContentChanged(oldRef, newRef);
		}
		protected override void OnCaretPositionChanged()
		{
			if (this.m_caretPosition <= this.m_codeHelperRange.start || this.m_caretPosition > this.m_codeHelperRange.end)
			{
				this.HideCodeHelper();
			}
			base.OnCaretPositionChanged();
		}
		public void ShowCodeHelper()
		{
			if (this.m_caretPosition.column == 0)
			{
				return;
			}
			string line = this.m_lines[this.m_caretPosition.line].line;
			int num = line.LastIndexOf('(', this.m_caretPosition.column - 1);
			if (num == -1)
			{
				return;
			}
			int num2 = base.FindWordStart(line, num);
			string key = line.Substring(num2, num - num2);
			Wilderness.FunctionDef function;
			if (!NomadCodeBox.s_functions.TryGetValue(key, out function))
			{
				return;
			}
			NomadTextBox.Position position = new NomadTextBox.Position(this.m_caretPosition.line, num + 1);
			int x;
			int num3;
			base.GetPointFromPosition(position, out x, out num3);
			int num4 = line.IndexOf(')', this.m_caretPosition.column - 1);
			if (num4 == -1)
			{
				num4 = line.Length;
			}
			this.m_codeHelperRange = new NomadTextBox.Selection(new NomadTextBox.Position(this.m_caretPosition.line, num), new NomadTextBox.Position(this.m_caretPosition.line, num4));
			if (this.m_codeHelper == null)
			{
				this.m_codeHelper = new CodeHelper();
			}
			else
			{
				this.m_codeHelper.Visible = false;
			}
			this.m_codeHelper.Setup(function);
			this.m_codeHelper.Location = new Point(x, num3 + base.LineHeight);
			base.SuspendLayout();
			this.m_codeHelper.Parent = this;
			base.ResumeLayout();
			this.m_codeHelper.Visible = true;
		}
		public void HideCodeHelper()
		{
			if (this.m_codeHelper != null)
			{
				this.m_codeHelper.Dispose();
				this.m_codeHelper = null;
			}
		}
		public static void InitFunctions()
		{
			NomadCodeBox.s_functions.Clear();
			for (int i = 0; i < Wilderness.NumFunctions; i++)
			{
				Wilderness.FunctionDef function = Wilderness.GetFunction(i);
				NomadCodeBox.s_functions.Add(function.Name, function);
			}
		}
	}
}
