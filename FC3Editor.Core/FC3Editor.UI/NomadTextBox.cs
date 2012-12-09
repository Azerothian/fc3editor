using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class NomadTextBox : Control
	{
		public struct Position
		{
			public int line;
			public int column;
			public Position(int line, int column)
			{
				this.line = line;
				this.column = column;
			}
			public static bool operator <(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return p1.line < p2.line || (p1.line == p2.line && p1.column < p2.column);
			}
			public static bool operator >(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return p1.line > p2.line || (p1.line == p2.line && p1.column > p2.column);
			}
			public static bool operator <=(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return p1.line < p2.line || (p1.line == p2.line && p1.column <= p2.column);
			}
			public static bool operator >=(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return p1.line > p2.line || (p1.line == p2.line && p1.column >= p2.column);
			}
			public static bool operator ==(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return p1.line == p2.line && p1.column == p2.column;
			}
			public static bool operator !=(NomadTextBox.Position p1, NomadTextBox.Position p2)
			{
				return !(p1 == p2);
			}
			public override bool Equals(object obj)
			{
				if (!(obj is NomadTextBox.Position))
				{
					return base.Equals(obj);
				}
				return this == (NomadTextBox.Position)obj;
			}
			public override int GetHashCode()
			{
				return this.line.GetHashCode() ^ this.column.GetHashCode();
			}
		}
		protected struct Selection
		{
			public NomadTextBox.Position start;
			public NomadTextBox.Position end;
			public bool IsEmpty
			{
				get
				{
					return this.start == this.end;
				}
			}
			public Selection(NomadTextBox.Position pos)
			{
				this.start = pos;
				this.end = pos;
			}
			public Selection(NomadTextBox.Position start, NomadTextBox.Position end)
			{
				this.start = start;
				this.end = end;
			}
			public void Normalize()
			{
				if (this.start.line < this.end.line)
				{
					return;
				}
				if (this.start.line > this.end.line || this.start.column > this.end.column)
				{
					NomadTextBox.Position position = this.start;
					this.start = this.end;
					this.end = position;
				}
			}
			public bool Contains(NomadTextBox.Position pos)
			{
				return pos >= this.start && pos < this.end;
			}
			public static bool operator ==(NomadTextBox.Selection s1, NomadTextBox.Selection s2)
			{
				return s1.start == s2.start && s1.end == s2.end;
			}
			public static bool operator !=(NomadTextBox.Selection s1, NomadTextBox.Selection s2)
			{
				return !(s1 == s2);
			}
			public override bool Equals(object obj)
			{
				if (!(obj is NomadTextBox.Selection))
				{
					return base.Equals(obj);
				}
				return this == (NomadTextBox.Selection)obj;
			}
			public override int GetHashCode()
			{
				return this.start.GetHashCode() ^ this.end.GetHashCode();
			}
		}
		protected class Line
		{
			public string line;
			public IDisposable tag;
			public Line(string line)
			{
				this.line = line;
			}
		}
		protected class Lines : List<NomadTextBox.Line>
		{
			public string Text
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < base.Count; i++)
					{
						stringBuilder.AppendLine(base[i].line);
					}
					return stringBuilder.ToString();
				}
			}
			public Lines()
			{
				base.Add(new NomadTextBox.Line(""));
			}
			public void ClearTags(int start, int num)
			{
				for (int i = start; i < start + num; i++)
				{
					if (base[i].tag != null)
					{
						base[i].tag.Dispose();
						base[i].tag = null;
					}
				}
			}
			public void ClearTags()
			{
				this.ClearTags(0, base.Count);
			}
		}
		protected abstract class UndoCommand
		{
			public abstract void ToggleState(NomadTextBox textBox);
		}
		protected class UndoTextCommand : NomadTextBox.UndoCommand
		{
			private NomadTextBox.Selection m_selection;
			private string m_text;
			private bool m_insert;
			public UndoTextCommand(NomadTextBox.Selection selection, string text, bool insert)
			{
				this.m_selection = selection;
				this.m_text = text;
				this.m_insert = insert;
			}
			public override void ToggleState(NomadTextBox textBox)
			{
				if (this.m_insert)
				{
					textBox.DeleteSelection(this.m_selection, false);
				}
				else
				{
					textBox.Paste(this.m_text, this.m_selection.start, false);
				}
				this.m_insert = !this.m_insert;
			}
		}
		protected class UndoIndentCommand : NomadTextBox.UndoCommand
		{
			protected NomadTextBox.Selection selection;
			protected bool indent;
			public UndoIndentCommand(NomadTextBox.Selection selection, bool indent)
			{
				this.selection = selection;
				this.indent = indent;
			}
			public override void ToggleState(NomadTextBox textBox)
			{
				textBox.m_selection = this.selection;
				textBox.IndentSelection(!this.indent, false);
				this.selection = textBox.m_selection;
				this.indent = !this.indent;
			}
		}
		protected class UndoEntry
		{
			protected List<NomadTextBox.UndoCommand> m_undoList = new List<NomadTextBox.UndoCommand>();
			private bool m_undo = true;
			protected NomadTextBox.Position m_oldCaret;
			protected NomadTextBox.Selection m_oldSelection;
			protected NomadTextBox.Position m_newCaret;
			protected NomadTextBox.Selection m_newSelection;
			public void Start(NomadTextBox textBox)
			{
				this.m_oldCaret = textBox.m_caretPosition;
				this.m_oldSelection = textBox.m_selection;
			}
			public bool Stop(NomadTextBox textBox)
			{
				this.m_newCaret = textBox.m_caretPosition;
				this.m_newSelection = textBox.m_selection;
				return this.m_undoList.Count > 0;
			}
			public void AddCommand(NomadTextBox.UndoCommand cmd)
			{
				this.m_undoList.Add(cmd);
			}
			public void ToggleState(NomadTextBox textBox)
			{
				if (this.m_undo)
				{
					for (int i = this.m_undoList.Count - 1; i >= 0; i--)
					{
						this.m_undoList[i].ToggleState(textBox);
					}
				}
				else
				{
					for (int j = 0; j < this.m_undoList.Count; j++)
					{
						this.m_undoList[j].ToggleState(textBox);
					}
				}
				textBox.SetCaretPosition(this.m_oldCaret, true);
				textBox.SetSelection(this.m_oldSelection);
				NomadTextBox.Position oldCaret = this.m_oldCaret;
				this.m_oldCaret = this.m_newCaret;
				this.m_newCaret = oldCaret;
				NomadTextBox.Selection oldSelection = this.m_oldSelection;
				this.m_oldSelection = this.m_newSelection;
				this.m_newSelection = oldSelection;
				this.m_undo = !this.m_undo;
			}
		}
		protected NomadTextBox.Lines m_lines = new NomadTextBox.Lines();
		protected NomadTextBox.Position m_caretPosition;
		protected NomadTextBox.Position m_anchorPosition;
		protected NomadTextBox.Selection m_selection = default(NomadTextBox.Selection);
		protected Win32.TextMetric m_fontMetrics;
		protected NomadTextBox.Position m_origin;
		protected List<NomadTextBox.UndoEntry> m_undoEntries = new List<NomadTextBox.UndoEntry>();
		protected NomadTextBox.UndoEntry m_undoEntry;
		private int undoLevel;
		private static TextFormatFlags s_textFormatFlags = TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.TextBoxControl | TextFormatFlags.NoPadding;
		private static uint s_drawTextFlags = 2272u;
		private static string[] s_lineSeparators = new string[]
		{
			"\r\n",
			"\r",
			"\n"
		};
		public event EventHandler CaretPositionChanged;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 3145728;
				createParams.ExStyle |= 512;
				return createParams;
			}
		}
		public NomadTextBox.Position CaretPosition
		{
			get
			{
				return this.m_caretPosition;
			}
			set
			{
				this.SetCaretPosition(value, true);
			}
		}
		protected int LineHeight
		{
			get
			{
				return this.m_fontMetrics.tmHeight + 1;
			}
		}
		protected int CharWidth
		{
			get
			{
				return this.m_fontMetrics.tmAveCharWidth;
			}
		}
		protected int NumVisibleLines
		{
			get
			{
				return base.ClientRectangle.Height / this.LineHeight;
			}
		}
		protected int NumVisibleChars
		{
			get
			{
				return base.ClientRectangle.Width / this.CharWidth;
			}
		}
		protected NomadTextBox.Selection Viewport
		{
			get
			{
				return new NomadTextBox.Selection(this.m_origin, new NomadTextBox.Position(this.m_origin.line + this.NumVisibleLines, this.m_origin.column + this.NumVisibleChars));
			}
		}
		protected int LineStartPixel
		{
			get
			{
				return -this.m_origin.column * this.CharWidth + this.LeftMarginWidth;
			}
		}
		protected int LeftMarginWidth
		{
			get
			{
				return 16;
			}
		}
		public override string Text
		{
			get
			{
				return this.m_lines.Text;
			}
			set
			{
				this.m_undoEntries.Clear();
				this.SelectAll();
				this.DeleteSelection(false);
				this.Paste(value, false);
				this.SetCaretPosition(default(NomadTextBox.Position), true);
			}
		}
		public NomadTextBox()
		{
			this.BackColor = SystemColors.Window;
			this.Cursor = Cursors.IBeam;
			this.Font = new Font("Lucida Console", 10f);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
		}
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Win32.CreateCaret(base.Handle, IntPtr.Zero, 1, this.LineHeight);
			Win32.ShowCaret(base.Handle);
			this.SetCaretPosition(this.m_caretPosition, false);
		}
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Win32.HideCaret(base.Handle);
			Win32.DestroyCaret();
		}
		protected override void OnFontChanged(EventArgs e)
		{
			IntPtr intPtr = Win32.CreateCompatibleDC(IntPtr.Zero);
			Win32.SelectObject(intPtr, this.Font.ToHfont());
			Win32.GetTextMetrics(intPtr, out this.m_fontMetrics);
			Win32.DeleteObject(intPtr);
			base.OnFontChanged(e);
		}
		protected override void OnSizeChanged(EventArgs e)
		{
			this.UpdateScrollbars();
			base.OnSizeChanged(e);
		}
		protected int DrawTextRaw(Graphics g, IntPtr hFont, string s, int x, int y, bool draw, Color color, Color backColor)
		{
			IntPtr hdc = g.GetHdc();
			Win32.SelectObject(hdc, hFont);
			Win32.Rect rect = new Win32.Rect(x, y, 0, this.LineHeight);
			Win32.DrawTextParams drawTextParams = new Win32.DrawTextParams();
			drawTextParams.iTabLength = 4;
			Win32.DrawTextEx(hdc, s, s.Length, ref rect, NomadTextBox.s_drawTextFlags | 1024u, drawTextParams);
			int result = rect.right - rect.left;
			rect.bottom = y + this.LineHeight;
			if (draw)
			{
				IntPtr intPtr = Win32.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
				Win32.FillRect(hdc, ref rect, intPtr);
				Win32.DeleteObject(intPtr);
				Win32.SetTextColor(hdc, ColorTranslator.ToWin32(color));
				Win32.SetBkColor(hdc, ColorTranslator.ToWin32(backColor));
				Win32.DrawTextEx(hdc, s, s.Length, ref rect, NomadTextBox.s_drawTextFlags, drawTextParams);
			}
			g.ReleaseHdc();
			return result;
		}
		protected int DrawText(Graphics g, IntPtr hFont, NomadTextBox.Position position, int x, int y, string s, Color color, bool selected)
		{
			int num = 0;
			if (!this.m_selection.IsEmpty)
			{
				if (!selected && this.m_selection.start.line == position.line && this.m_selection.start.column >= position.column && this.m_selection.start.column < position.column + s.Length)
				{
					string text = s.Substring(0, this.m_selection.start.column - position.column);
					if (text.Length > 0)
					{
						num = this.DrawText(g, hFont, new NomadTextBox.Position(position.line, position.column), x, y, text, color, false);
						num += this.DrawText(g, hFont, new NomadTextBox.Position(position.line, position.column + text.Length), x + num, y, s.Substring(this.m_selection.start.column - position.column), color, true);
						return num;
					}
					selected = true;
				}
				if (selected && this.m_selection.end.line == position.line && this.m_selection.end.column >= position.column && this.m_selection.end.column < position.column + s.Length)
				{
					string text2 = s.Substring(0, this.m_selection.end.column - position.column);
					if (text2.Length > 0)
					{
						num = this.DrawText(g, hFont, new NomadTextBox.Position(position.line, position.column), x, y, text2, color, true);
						num += this.DrawText(g, hFont, new NomadTextBox.Position(position.line, position.column + text2.Length), x + num, y, s.Substring(this.m_selection.end.column - position.column), color, false);
						return num;
					}
					selected = false;
				}
			}
			num = this.DrawTextRaw(g, hFont, s, x, y, true, selected ? SystemColors.HighlightText : color, selected ? SystemColors.Highlight : this.BackColor);
			if (selected && this.m_selection.end.line > position.line)
			{
				using (SolidBrush solidBrush = new SolidBrush(SystemColors.Highlight))
				{
					g.FillRectangle(solidBrush, new Rectangle(x + num, y, 3, this.LineHeight));
				}
			}
			return num;
		}
		protected int DrawText(Graphics g, IntPtr hFont, NomadTextBox.Position position, int x, int y, string s, Color color)
		{
			bool selected = this.m_selection.Contains(position);
			return this.DrawText(g, hFont, position, x, y, s, color, selected);
		}
		protected virtual void DrawTextFormat(Graphics g, IntPtr hFont, NomadTextBox.Position position, int x, int y, string s)
		{
			this.DrawText(g, hFont, position, x, y, s, this.ForeColor);
		}
		protected virtual void OnPaintMargin(PaintEventArgs e)
		{
			using (SolidBrush solidBrush = new SolidBrush(SystemColors.Control))
			{
				e.Graphics.FillRectangle(solidBrush, new Rectangle(0, 0, this.LeftMarginWidth, base.ClientRectangle.Height));
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			int num = this.NumVisibleLines + 1;
			int line = this.m_origin.line;
			int num2 = this.m_origin.line + num;
			if (num2 > this.m_lines.Count)
			{
				num2 = this.m_lines.Count;
			}
			IntPtr intPtr = this.Font.ToHfont();
			int num3 = 0;
			for (int i = line; i < num2; i++)
			{
				this.DrawTextFormat(e.Graphics, intPtr, new NomadTextBox.Position(i, 0), this.LineStartPixel, num3, this.m_lines[i].line);
				num3 += this.LineHeight;
			}
			Win32.DeleteObject(intPtr);
			this.OnPaintMargin(e);
		}
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 135)
			{
				m.Result = (IntPtr)4;
				return;
			}
			switch (msg)
			{
			case 276:
				this.OnScroll(false, m);
				return;

			case 277:
				this.OnScroll(true, m);
				return;

			default:
				base.WndProc(ref m);
				return;
			}
		}
		protected void OnScroll(bool vertical, Message m)
		{
			int nBar = vertical ? 1 : 0;
			Win32.ScrollInfo scrollInfo = new Win32.ScrollInfo();
			scrollInfo.fMask = 31;
			Win32.GetScrollInfo(base.Handle, nBar, scrollInfo);
			int num = scrollInfo.nPos;
			switch (Win32.LoWord((int)m.WParam))
			{
			case 0:
				num--;
				break;

			case 1:
				num++;
				break;

			case 2:
				num -= scrollInfo.nPage;
				break;

			case 3:
				num += scrollInfo.nPage;
				break;

			case 4:
			case 5:
				num = scrollInfo.nTrackPos;
				break;

			case 6:
				num = scrollInfo.nMin;
				break;

			case 7:
				num = scrollInfo.nMax;
				break;
			}
			NomadTextBox.Position origin = this.m_origin;
			if (vertical)
			{
				origin.line = num;
			}
			else
			{
				origin.column = num;
			}
			this.ScrollTo(origin);
		}
		protected NomadTextBox.Position GetPositionFromPoint(Point pt, out int x, out int y)
		{
			NomadTextBox.Position result = default(NomadTextBox.Position);
			result.line = this.m_origin.line + pt.Y / this.LineHeight;
			this.ClipPositionLine(ref result);
			y = (result.line - this.m_origin.line) * this.LineHeight;
			string line = this.m_lines[result.line].line;
			x = this.LineStartPixel;
			using (Graphics graphics = Graphics.FromHwnd(base.Handle))
			{
				result.column = 0;
				while (result.column < line.Length)
				{
					string text = (line[result.column] == '\t') ? new string(' ', 4 - result.column % 4) : new string(line[result.column], 1);
					Size size = TextRenderer.MeasureText(graphics, text, this.Font, default(Size), NomadTextBox.s_textFormatFlags);
					int num = x + size.Width;
					if (pt.X < num - size.Width / 2)
					{
						break;
					}
					x = num;
					result.column++;
				}
			}
			this.ClipPositionColumn(ref result);
			return result;
		}
		protected void GetPointFromPosition(NomadTextBox.Position position, out int x, out int y)
		{
			string line = this.m_lines[position.line].line;
			using (Graphics graphics = Graphics.FromHwnd(base.Handle))
			{
				IntPtr intPtr = this.Font.ToHfont();
				y = this.LineHeight * (position.line - this.m_origin.line);
				int num = this.DrawTextRaw(graphics, intPtr, line.Substring(0, position.column), 0, 0, false, Color.Empty, Color.Empty);
				x = this.LineStartPixel + num;
				Win32.DeleteObject(intPtr);
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			int x;
			int y;
			NomadTextBox.Position positionFromPoint = this.GetPositionFromPoint(e.Location, out x, out y);
			NomadTextBox.Selection selection;
			if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
			{
				selection = new NomadTextBox.Selection(this.m_anchorPosition, positionFromPoint);
			}
			else
			{
				selection = new NomadTextBox.Selection(positionFromPoint);
			}
			if ((Control.ModifierKeys & Keys.Control) != Keys.None)
			{
				this.ExpandSelectionWords(ref selection);
			}
			this.SetSelection(selection);
			if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
			{
				this.m_anchorPosition = positionFromPoint;
			}
			this.SetCaretPosition(selection.end, x, y, false, true);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if ((e.Button & MouseButtons.Left) != MouseButtons.None)
			{
				int x;
				int y;
				NomadTextBox.Position positionFromPoint = this.GetPositionFromPoint(e.Location, out x, out y);
				NomadTextBox.Selection selection = new NomadTextBox.Selection(this.m_anchorPosition, positionFromPoint);
				if ((Control.ModifierKeys & Keys.Control) != Keys.None)
				{
					this.ExpandSelectionWords(ref selection);
				}
				this.SetSelection(selection);
				if (positionFromPoint < this.m_anchorPosition)
				{
					this.SetCaretPosition(this.m_selection.start, x, y, false, true);
					return;
				}
				this.SetCaretPosition(this.m_selection.end, x, y, false, true);
			}
		}
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			NomadTextBox.Selection selection = new NomadTextBox.Selection(this.m_caretPosition);
			this.ExpandSelectionWords(ref selection);
			this.SetSelection(selection);
			this.m_anchorPosition = selection.start;
			this.SetCaretPosition(selection.end, false);
			base.OnMouseDoubleClick(e);
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int num = -e.Delta / SystemInformation.MouseWheelScrollDelta * SystemInformation.MouseWheelScrollLines;
			NomadTextBox.Position origin = this.m_origin;
			origin.line += num;
			this.ScrollTo(origin);
			base.OnMouseWheel(e);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			if (e.KeyChar >= ' ')
			{
				this.BeginUndo();
				this.Paste(new string(e.KeyChar, 1));
				this.EndUndo();
			}
			base.OnKeyPress(e);
		}
		protected int SkipSpaceStart(string line, int index)
		{
			for (index--; index >= 0; index--)
			{
				if (line[index] != ' ' && line[index] != '\t')
				{
					return index + 1;
				}
			}
			return 0;
		}
		protected int SkipSpaceEnd(string line, int index)
		{
			while (index < line.Length)
			{
				if (line[index] != ' ' && line[index] != '\t')
				{
					return index;
				}
				index++;
			}
			return line.Length;
		}
		protected int FindWordStart(string line, int index)
		{
			index--;
			int num = index;
			while (index >= 0)
			{
				if ((line[index] < 'A' || line[index] > 'Z') && (line[index] < 'a' || line[index] > 'z') && (line[index] < '0' || line[index] > '9') && line[index] != '_')
				{
					if (index == num)
					{
						index--;
					}
					return index + 1;
				}
				index--;
			}
			return 0;
		}
		protected int FindWordEnd(string line, int index)
		{
			int num = index;
			while (index < line.Length)
			{
				if ((line[index] < 'A' || line[index] > 'Z') && (line[index] < 'a' || line[index] > 'z') && (line[index] < '0' || line[index] > '9') && line[index] != '_')
				{
					if (index == num)
					{
						index++;
					}
					return index;
				}
				index++;
			}
			return line.Length;
		}
		protected void MoveTo(NomadTextBox.Position newPos)
		{
			this.ClipPosition(ref newPos);
			this.SetCaretPosition(newPos, (Control.ModifierKeys & Keys.Shift) == Keys.None);
			NomadTextBox.Selection selection = new NomadTextBox.Selection(this.m_anchorPosition, newPos);
			this.SetSelection(selection);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode <= Keys.Return)
			{
				switch (keyCode)
				{
				case Keys.Back:
					if (!this.m_selection.IsEmpty)
					{
						this.BeginUndo();
						this.DeleteSelection();
						this.EndUndo();
					}
					else
					{
						NomadTextBox.Selection selection = new NomadTextBox.Selection(this.m_caretPosition, this.m_caretPosition);
						if (this.m_caretPosition.column > 0)
						{
							selection.start.column = selection.start.column - 1;
						}
						else
						{
							if (this.m_caretPosition.line <= 0)
							{
								break;
							}
							selection.start.line = selection.start.line - 1;
							selection.start.column = this.m_lines[selection.start.line].line.Length;
						}
						this.BeginUndo();
						this.DeleteSelection(selection);
						this.EndUndo();
					}
					break;

				case Keys.Tab:
					this.BeginUndo();
					if (this.m_selection.IsEmpty)
					{
						if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
						{
							if (this.m_caretPosition.column > 0 && this.m_lines[this.m_caretPosition.line].line[this.m_caretPosition.column - 1] == '\t')
							{
								NomadTextBox.Selection selection2 = new NomadTextBox.Selection(this.m_caretPosition, this.m_caretPosition);
								selection2.start.column = selection2.start.column - 1;
								this.DeleteSelection(selection2);
							}
						}
						else
						{
							this.Paste("\t");
						}
					}
					else
					{
						this.IndentSelection((Control.ModifierKeys & Keys.Shift) == Keys.None, true);
					}
					this.EndUndo();
					break;

				default:
					if (keyCode == Keys.Return)
					{
						string line = this.m_lines[this.m_caretPosition.line].line;
						int num = 0;
						while (num < line.Length && (line[num] == ' ' || line[num] == '\t'))
						{
							num++;
						}
						this.BeginUndo();
						this.Paste("\n" + line.Substring(0, num));
						this.EndUndo();
					}
					break;
				}
			}
			else
			{
				switch (keyCode)
				{
				case Keys.Prior:
					{
						NomadTextBox.Position caretPosition = this.m_caretPosition;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							caretPosition.line = this.m_origin.line;
						}
						else
						{
							NomadTextBox.Position origin = this.m_origin;
							origin.line -= this.NumVisibleLines;
							this.ScrollTo(origin);
							caretPosition.line -= this.NumVisibleLines;
						}
						this.MoveTo(caretPosition);
						break;
					}

				case Keys.Next:
					{
						NomadTextBox.Position caretPosition2 = this.m_caretPosition;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							caretPosition2.line = this.m_origin.line + this.NumVisibleLines - 1;
						}
						else
						{
							NomadTextBox.Position origin2 = this.m_origin;
							origin2.line += this.NumVisibleLines;
							this.ScrollTo(origin2);
							caretPosition2.line += this.NumVisibleLines;
						}
						this.MoveTo(caretPosition2);
						break;
					}

				case Keys.End:
					{
						NomadTextBox.Position caretPosition3 = this.m_caretPosition;
						if ((e.Modifiers & Keys.Control) != Keys.None)
						{
							caretPosition3.line = 2147483647;
						}
						caretPosition3.column = 2147483647;
						this.MoveTo(caretPosition3);
						break;
					}

				case Keys.Home:
					{
						NomadTextBox.Position caretPosition4 = this.m_caretPosition;
						if ((e.Modifiers & Keys.Control) != Keys.None)
						{
							caretPosition4.line = 0;
						}
						caretPosition4.column = 0;
						this.MoveTo(caretPosition4);
						break;
					}

				case Keys.Left:
					{
						NomadTextBox.Position caretPosition5 = this.m_caretPosition;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							string line2 = this.m_lines[this.m_caretPosition.line].line;
							int num2 = this.m_caretPosition.column;
							num2 = this.SkipSpaceStart(line2, num2);
							num2 = this.FindWordStart(line2, num2);
							if (num2 == this.m_caretPosition.column)
							{
								num2--;
							}
							caretPosition5.column = num2;
						}
						else
						{
							if (caretPosition5.column > 0)
							{
								caretPosition5.column--;
							}
							else
							{
								if (caretPosition5.line > 0)
								{
									caretPosition5.line--;
									caretPosition5.column = this.m_lines[caretPosition5.line].line.Length;
								}
							}
						}
						this.MoveTo(caretPosition5);
						break;
					}

				case Keys.Up:
					{
						NomadTextBox.Position caretPosition6 = this.m_caretPosition;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							NomadTextBox.Position origin3 = this.m_origin;
							origin3.line--;
							this.ScrollTo(origin3);
							if (this.m_caretPosition.line >= this.m_origin.line + this.NumVisibleLines)
							{
								caretPosition6.line--;
							}
						}
						else
						{
							if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
							{
								caretPosition6.line--;
							}
							else
							{
								caretPosition6.line = this.m_selection.start.line - 1;
							}
						}
						this.MoveTo(caretPosition6);
						break;
					}

				case Keys.Right:
					{
						NomadTextBox.Position caretPosition7 = this.m_caretPosition;
						string line3 = this.m_lines[this.m_caretPosition.line].line;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							int num3 = this.m_caretPosition.column;
							num3 = this.FindWordEnd(line3, num3);
							num3 = this.SkipSpaceEnd(line3, num3);
							caretPosition7.column = num3;
							if (num3 == this.m_caretPosition.column)
							{
								num3++;
							}
						}
						else
						{
							if (caretPosition7.column < line3.Length)
							{
								caretPosition7.column++;
							}
							else
							{
								if (caretPosition7.line < this.m_lines.Count - 1)
								{
									caretPosition7.line++;
									caretPosition7.column = 0;
								}
							}
						}
						this.MoveTo(caretPosition7);
						break;
					}

				case Keys.Down:
					{
						NomadTextBox.Position caretPosition8 = this.m_caretPosition;
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							NomadTextBox.Position origin4 = this.m_origin;
							origin4.line++;
							this.ScrollTo(origin4);
							if (this.m_caretPosition.line < this.m_origin.line)
							{
								caretPosition8.line++;
							}
						}
						else
						{
							if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
							{
								caretPosition8.line++;
							}
							else
							{
								caretPosition8.line = this.m_selection.end.line + 1;
							}
						}
						this.MoveTo(caretPosition8);
						break;
					}

				case Keys.Select:
				case Keys.Print:
				case Keys.Execute:
				case Keys.Snapshot:
					break;

				case Keys.Insert:
					if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
					{
						this.Paste();
					}
					else
					{
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							this.Copy();
						}
					}
					break;

				case Keys.Delete:
					if (!this.m_selection.IsEmpty)
					{
						if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
						{
							this.Cut();
						}
						else
						{
							this.BeginUndo();
							this.DeleteSelection();
							this.EndUndo();
						}
					}
					else
					{
						NomadTextBox.Selection selection3 = new NomadTextBox.Selection(this.m_caretPosition, this.m_caretPosition);
						if (this.m_caretPosition.column < this.m_lines[this.m_caretPosition.line].line.Length)
						{
							selection3.end.column = selection3.end.column + 1;
						}
						else
						{
							if (this.m_caretPosition.line >= this.m_lines.Count - 1)
							{
								break;
							}
							selection3.end.line = selection3.end.line + 1;
							selection3.end.column = 0;
						}
						this.BeginUndo();
						this.DeleteSelection(selection3);
						this.EndUndo();
					}
					break;

				default:
					switch (keyCode)
					{
					case Keys.A:
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							this.SelectAll();
						}
						break;

					case Keys.B:
						break;

					case Keys.C:
						if ((Control.ModifierKeys & Keys.Control) != Keys.None)
						{
							this.Copy();
						}
						break;

					default:
						switch (keyCode)
						{
						case Keys.V:
							if ((Control.ModifierKeys & Keys.Control) != Keys.None)
							{
								this.Paste();
							}
							break;

						case Keys.X:
							if ((Control.ModifierKeys & Keys.Control) != Keys.None)
							{
								this.Cut();
							}
							break;

						case Keys.Z:
							if ((Control.ModifierKeys & Keys.Control) != Keys.None)
							{
								this.Undo();
							}
							break;
						}
						break;
					}
					break;
				}
			}
			base.OnKeyDown(e);
		}
		protected void OnContentChanged(NomadTextBox.Position start, NomadTextBox.Position end, bool expand)
		{
			NomadTextBox.Position oldRef;
			NomadTextBox.Position newRef;
			if (expand)
			{
				oldRef = start;
				newRef = end;
			}
			else
			{
				oldRef = end;
				newRef = start;
			}
			this.OnContentChanged(oldRef, newRef);
		}
		protected void UpdatePosition(ref NomadTextBox.Position pos, NomadTextBox.Position oldRef, NomadTextBox.Position newRef)
		{
			if (pos >= oldRef)
			{
				pos = this.MoveRelativePosition(oldRef, newRef, pos);
			}
			this.ClipPosition(ref pos);
		}
		protected virtual void OnContentChanged(NomadTextBox.Position oldRef, NomadTextBox.Position newRef)
		{
			this.UpdatePosition(ref this.m_selection.start, oldRef, newRef);
			this.UpdatePosition(ref this.m_selection.end, oldRef, newRef);
			this.UpdatePosition(ref this.m_caretPosition, oldRef, newRef);
			this.SetCaretPosition(this.m_caretPosition, false);
			this.UpdatePosition(ref this.m_anchorPosition, oldRef, newRef);
			this.UpdateScrollbars();
			base.Invalidate();
		}
		protected void SetCaretPosition(NomadTextBox.Position position, bool anchor)
		{
			this.SetCaretPosition(position, anchor, true);
		}
		protected void SetCaretPosition(NomadTextBox.Position position, bool anchor, bool autoScroll)
		{
			this.ClipPosition(ref position);
			int x;
			int y;
			this.GetPointFromPosition(position, out x, out y);
			this.SetCaretPosition(position, x, y, anchor, autoScroll);
		}
		protected void SetCaretPosition(NomadTextBox.Position position, int x, int y, bool anchor, bool autoScroll)
		{
			if (this.m_selection.IsEmpty && this.m_selection.start == this.m_caretPosition)
			{
				this.m_selection = new NomadTextBox.Selection(position);
			}
			this.m_caretPosition = position;
			if (anchor)
			{
				this.m_anchorPosition = this.m_caretPosition;
			}
			if (x < this.LeftMarginWidth)
			{
				Win32.SetCaretPos(-1, -1);
			}
			else
			{
				Win32.SetCaretPos(x, y);
			}
			if (autoScroll)
			{
				int num = x - (base.ClientRectangle.Left + this.LeftMarginWidth);
				int num2 = x - base.ClientRectangle.Right;
				int num3 = y - base.ClientRectangle.Top;
				int num4 = y - base.ClientRectangle.Bottom;
				NomadTextBox.Position origin = this.m_origin;
				if (num < 0)
				{
					origin.column += num / this.CharWidth - 1;
				}
				else
				{
					if (num2 >= 0)
					{
						origin.column += num2 / this.CharWidth + 1;
					}
				}
				if (num3 < 0)
				{
					origin.line += num3 / this.LineHeight - 1;
				}
				else
				{
					if (num4 >= 0)
					{
						origin.line += num4 / this.LineHeight + 1;
					}
				}
				if (origin != this.m_origin)
				{
					this.ScrollTo(origin);
				}
			}
			this.OnCaretPositionChanged();
		}
		protected virtual void OnCaretPositionChanged()
		{
			if (this.CaretPositionChanged != null)
			{
				this.CaretPositionChanged(this, null);
			}
		}
		protected void ClipPosition(ref NomadTextBox.Position position)
		{
			this.ClipPositionLine(ref position);
			this.ClipPositionColumn(ref position);
		}
		protected void ClipPositionLine(ref NomadTextBox.Position position)
		{
			if (position.line < 0)
			{
				position.line = 0;
				return;
			}
			if (position.line >= this.m_lines.Count)
			{
				position.line = this.m_lines.Count - 1;
			}
		}
		protected void ClipPositionColumn(ref NomadTextBox.Position position)
		{
			if (position.column < 0)
			{
				position.column = 0;
				return;
			}
			if (position.column > this.m_lines[position.line].line.Length)
			{
				position.column = this.m_lines[position.line].line.Length;
			}
		}
		protected NomadTextBox.Position MoveRelativePosition(NomadTextBox.Position oldRef, NomadTextBox.Position newRef, NomadTextBox.Position pos)
		{
			int num = pos.column - oldRef.column;
			int num2 = pos.line - oldRef.line;
			NomadTextBox.Position result;
			result.line = newRef.line + num2;
			if (num2 == 0)
			{
				result.column = newRef.column + num;
			}
			else
			{
				result.column = pos.column;
			}
			return result;
		}
		protected void ClearSelection()
		{
			if (!this.m_selection.IsEmpty)
			{
				base.Invalidate();
			}
			this.m_selection.start = this.m_caretPosition;
			this.m_selection.end = this.m_caretPosition;
		}
		protected void SetSelection(NomadTextBox.Selection selection)
		{
			this.ClipPosition(ref selection.start);
			this.ClipPosition(ref selection.end);
			selection.Normalize();
			if (this.m_selection != selection)
			{
				this.m_selection = selection;
				base.Invalidate();
			}
		}
		protected void ExpandSelectionWords(ref NomadTextBox.Selection selection)
		{
			selection.Normalize();
			selection.start.column = this.FindWordStart(this.m_lines[selection.start.line].line, selection.start.column);
			selection.end.column = this.FindWordEnd(this.m_lines[selection.end.line].line, selection.end.column);
			base.Invalidate();
		}
		protected string DeleteSelection()
		{
			return this.DeleteSelection(true);
		}
		protected string DeleteSelection(bool undo)
		{
			string result = this.DeleteSelection(this.m_selection, undo);
			this.ClearSelection();
			return result;
		}
		protected string DeleteSelection(NomadTextBox.Selection selection)
		{
			return this.DeleteSelection(selection, true);
		}
		protected string DeleteSelection(NomadTextBox.Selection selection, bool undo)
		{
			if (selection.IsEmpty)
			{
				return "";
			}
			string selectionText = this.GetSelectionText(selection);
			if (undo)
			{
				this.AddUndoCommand(new NomadTextBox.UndoTextCommand(selection, selectionText, false));
			}
			this.m_lines[selection.start.line].line = this.m_lines[selection.start.line].line.Substring(0, selection.start.column) + this.m_lines[selection.end.line].line.Substring(selection.end.column);
			int num = selection.start.line + 1;
			int num2 = selection.end.line - num + 1;
			if (num < this.m_lines.Count)
			{
				this.m_lines.ClearTags(num, num2);
				this.m_lines.RemoveRange(num, num2);
			}
			this.OnContentChanged(selection.start, selection.end, false);
			return selectionText;
		}
		protected void SelectAll()
		{
			this.m_selection.start = new NomadTextBox.Position(0, 0);
			this.m_selection.end = new NomadTextBox.Position(this.m_lines.Count - 1, this.m_lines[this.m_lines.Count - 1].line.Length);
			this.SetCaretPosition(this.m_selection.start, true);
			base.Invalidate();
		}
		protected void CreateSelection(ref NomadTextBox.Selection selection, int delta)
		{
			selection.end.line = selection.start.line;
			int num = this.m_lines[selection.end.line].line.Length - selection.start.column;
			while (delta > num)
			{
				delta -= num;
				if (selection.end.line == this.m_lines.Count - 1)
				{
					selection.end.column = num;
					return;
				}
				selection.end.line = selection.end.line + 1;
				selection.end.column = 0;
			}
			selection.end.column = selection.end.column + delta;
		}
		protected string GetSelectionText()
		{
			return this.GetSelectionText(this.m_selection);
		}
		protected string GetSelectionText(NomadTextBox.Selection selection)
		{
			if (selection.IsEmpty)
			{
				return "";
			}
			string result;
			if (selection.start.line == selection.end.line)
			{
				result = this.m_lines[selection.start.line].line.Substring(selection.start.column, selection.end.column - selection.start.column);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.m_lines[selection.start.line].line.Substring(selection.start.column));
				for (int i = selection.start.line + 1; i < selection.end.line; i++)
				{
					stringBuilder.AppendLine(this.m_lines[i].line);
				}
				stringBuilder.Append(this.m_lines[selection.end.line].line.Substring(0, selection.end.column));
				result = stringBuilder.ToString();
			}
			return result;
		}
		protected void IndentSelection(bool indent, bool undo)
		{
			if (!indent)
			{
				for (int i = this.m_selection.start.line; i <= this.m_selection.end.line; i++)
				{
					if (this.m_lines[i].line.Length > 0 && this.m_lines[i].line[0] == '\t')
					{
						NomadTextBox.Selection selection = new NomadTextBox.Selection(new NomadTextBox.Position(i, 0), new NomadTextBox.Position(i, 1));
						this.DeleteSelection(selection, false);
					}
				}
			}
			else
			{
				for (int j = this.m_selection.start.line; j <= this.m_selection.end.line; j++)
				{
					this.Paste("\t", new NomadTextBox.Position(j, 0), false);
				}
			}
			if (undo)
			{
				this.AddUndoCommand(new NomadTextBox.UndoIndentCommand(this.m_selection, true));
			}
		}
		public void Cut()
		{
			this.Copy();
			this.BeginUndo();
			this.DeleteSelection();
			this.EndUndo();
		}
		public void Copy()
		{
			string selectionText = this.GetSelectionText();
			if (selectionText.Length == 0)
			{
				Clipboard.Clear();
				return;
			}
			Clipboard.SetText(selectionText);
		}
		public void Paste()
		{
			this.Paste(Clipboard.GetText());
		}
		public void Paste(string text)
		{
			this.Paste(text, true);
		}
		protected void Paste(string text, bool undo)
		{
			if (undo)
			{
				this.BeginUndo();
			}
			this.DeleteSelection(undo);
			this.Paste(text, this.m_caretPosition, undo);
			if (undo)
			{
				this.EndUndo();
			}
		}
		protected void Paste(string text, NomadTextBox.Position position)
		{
			this.Paste(text, position, true);
		}
		protected void Paste(string text, NomadTextBox.Position position, bool undo)
		{
			string[] array = text.Split(NomadTextBox.s_lineSeparators, StringSplitOptions.None);
			if (array.Length == 0)
			{
				return;
			}
			this.ClipPosition(ref position);
			string str = this.m_lines[position.line].line.Substring(position.column);
			this.m_lines[position.line].line = this.m_lines[position.line].line.Substring(0, position.column) + array[0];
			NomadTextBox.Line[] array2 = new NomadTextBox.Line[array.Length - 1];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = new NomadTextBox.Line(array[i + 1]);
			}
			this.m_lines.InsertRange(position.line + 1, array2);
			int num = position.line + array.Length - 1;
			NomadTextBox.Position end = new NomadTextBox.Position(num, this.m_lines[num].line.Length);
			NomadTextBox.Line expr_F9 = this.m_lines[num];
			expr_F9.line += str;
			if (undo)
			{
				this.AddUndoCommand(new NomadTextBox.UndoTextCommand(new NomadTextBox.Selection(position, end), text, true));
			}
			this.OnContentChanged(position, end, true);
		}
		protected void ScrollTo(NomadTextBox.Position pos)
		{
			this.ClipPositionLine(ref pos);
			if (pos.column < 0)
			{
				pos.column = 0;
			}
			int num = pos.column - this.m_origin.column;
			int num2 = pos.line - this.m_origin.line;
			this.m_origin = pos;
			if (num != 0)
			{
				Win32.Rect rect = new Win32.Rect(this.LeftMarginWidth, 0, base.ClientRectangle.Width - this.LeftMarginWidth, base.ClientRectangle.Height);
				Win32.ScrollWindowEx(base.Handle, -num * this.CharWidth, 0, ref rect, ref rect, IntPtr.Zero, IntPtr.Zero, 2);
			}
			if (num2 != 0)
			{
				Win32.Rect rect2 = new Win32.Rect(0, 0, base.ClientRectangle.Width, base.ClientRectangle.Height);
				Win32.ScrollWindowEx(base.Handle, 0, -num2 * this.LineHeight, ref rect2, ref rect2, IntPtr.Zero, IntPtr.Zero, 2);
			}
			this.UpdateScrollbars();
			this.SetCaretPosition(this.m_caretPosition, false, false);
		}
		protected void UpdateScrollbars()
		{
			this.UpdateScrollbar(true);
			this.UpdateScrollbar(false);
		}
		protected void UpdateScrollbar(bool vertical)
		{
			int nBar;
			int num;
			if (vertical)
			{
				nBar = 1;
				num = this.m_origin.line;
			}
			else
			{
				nBar = 0;
				num = this.m_origin.column;
			}
			Win32.ScrollInfo scrollInfo = new Win32.ScrollInfo();
			scrollInfo.fMask = 31;
			Win32.GetScrollInfo(base.Handle, nBar, scrollInfo);
			if (vertical)
			{
				scrollInfo.nPage = this.NumVisibleLines;
				scrollInfo.nMin = 0;
				scrollInfo.nMax = this.m_lines.Count - 2 + scrollInfo.nPage;
			}
			else
			{
				scrollInfo.nPage = this.NumVisibleChars;
				scrollInfo.nMin = 0;
				scrollInfo.nMax = 100;
			}
			if (num < scrollInfo.nMin)
			{
				num = scrollInfo.nMin;
			}
			else
			{
				if (num > scrollInfo.nMax)
				{
					num = scrollInfo.nMax;
				}
			}
			scrollInfo.nPos = num;
			Win32.SetScrollInfo(base.Handle, nBar, scrollInfo, true);
		}
		public void Undo()
		{
			if (this.m_undoEntries.Count == 0)
			{
				return;
			}
			NomadTextBox.UndoEntry undoEntry = this.m_undoEntries[this.m_undoEntries.Count - 1];
			undoEntry.ToggleState(this);
			this.m_undoEntries.RemoveAt(this.m_undoEntries.Count - 1);
		}
		protected void TrimUndo()
		{
			if (this.m_undoEntries.Count > 100)
			{
				this.m_undoEntries.RemoveAt(0);
			}
		}
		protected void BeginUndo()
		{
			if (this.undoLevel == 0)
			{
				if (this.m_undoEntry != null)
				{
					this.EndUndo();
				}
				this.m_undoEntry = new NomadTextBox.UndoEntry();
				this.m_undoEntry.Start(this);
			}
			this.undoLevel++;
		}
		protected void AddUndoCommand(NomadTextBox.UndoCommand cmd)
		{
			this.m_undoEntry.AddCommand(cmd);
		}
		protected void EndUndo()
		{
			if (this.undoLevel == 1)
			{
				if (this.m_undoEntry.Stop(this))
				{
					this.m_undoEntries.Add(this.m_undoEntry);
				}
				this.m_undoEntry = null;
				this.TrimUndo();
			}
			this.undoLevel--;
		}
	}
}
