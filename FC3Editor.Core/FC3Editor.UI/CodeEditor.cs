using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandBar;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class CodeEditor : Form
	{
		private static CodeEditor s_instance;
		private IContainer components;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripStatusLabel lineStatusLabel;
		private ToolStripStatusLabel columnStatusLabel;
		private SandDockManager sandDockManager1;
		private ToolBarContainer leftSandBarDock;
		private SandBarManager sandBarManager1;
		private ToolBarContainer rightSandBarDock;
		private ToolBarContainer bottomSandBarDock;
		private ToolBarContainer topSandBarDock;
		private MenuBar menuBar1;
		private MenuBarItem menuBarItem1;
		private MenuButtonItem newFileMenuItem;
		private MenuBarItem menuBarItem2;
		private MenuBarItem menuBarItem3;
		private MenuBarItem menuBarItem4;
		private MenuBarItem menuBarItem5;
		private TD.SandBar.ToolBar toolBar1;
		private MenuButtonItem saveScriptMenuItem;
		private MenuButtonItem openScriptMenuItem;
		private MenuButtonItem cutMenuItem;
		private MenuButtonItem copyMenuItem;
		private ButtonItem buttonItem1;
		private ButtonItem buttonItem3;
		private ButtonItem buttonItem2;
		private ButtonItem buttonItem4;
		private MenuButtonItem pasteMenuItem;
		private ButtonItem buttonItem5;
		private ButtonItem buttonItem6;
		private MenuButtonItem runMenuItem;
		private ButtonItem buttonItem7;
		private CodeMapViewerDock codeMapViewerDock1;
		private MenuButtonItem undoMenuItem;
		private MenuButtonItem redoMenuItem;
		private ButtonItem buttonItem8;
		private ButtonItem buttonItem9;
		private MenuButtonItem exitMenuItem;
		private MenuButtonItem mapViewerMenuItem;
		private OpenFileDialog openFileDialog;
		private ContextMenuStrip contextMenuStrip1;
		private ToolStripMenuItem cutToolStripMenuItem;
		private ToolStripMenuItem copyToolStripMenuItem;
		private ToolStripMenuItem pasteToolStripMenuItem;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem insertFunctionToolStripMenuItem;
		private ToolStripMenuItem insertTextureEntryIDToolStripMenuItem;
		private ToolStripMenuItem insertCollectionEntryIDToolStripMenuItem;
		public static CodeEditor Instance
		{
			get
			{
				return CodeEditor.s_instance;
			}
		}
		public CodeEditor()
		{
			CodeEditor.s_instance = this;
			this.InitializeComponent();
			base.Icon = Resources.appIcon;
			NomadCodeBox.InitFunctions();
			this.InitContextMenu();
		}
		private void DisposeInternal()
		{
			CodeEditor.s_instance = null;
		}
		private CodeDocument CreateDocument()
		{
			return new CodeDocument
			{
				Manager = this.sandDockManager1,
				Content = 
				{
					ContextMenuStrip = this.contextMenuStrip1
				}
			};
		}
		private void newFileMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.CreateDocument();
			codeDocument.Open();
		}
		private void openScriptMenuItem_Activate(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			CodeDocument codeDocument = this.CreateDocument();
			codeDocument.LoadFile(this.openFileDialog.FileName);
			codeDocument.Open();
		}
		private void saveScriptMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.SaveFile();
		}
		private void undoMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Undo();
		}
		private void cutMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Cut();
		}
		private void copyMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Copy();
		}
		private void pasteMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Paste();
		}
		private void runMenuItem_Activate(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Run();
			this.UpdateCaretPosition();
		}
		private void mapViewerMenuItem_Activate(object sender, EventArgs e)
		{
			this.codeMapViewerDock1.Open();
		}
		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Cut();
		}
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Copy();
		}
		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Paste();
		}
		private void functionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			codeDocument.Content.Paste(((ToolStripMenuItem)sender).Text + "(");
			codeDocument.Content.ShowCodeHelper();
		}
		private void InitContextMenuFunctions()
		{
			this.insertFunctionToolStripMenuItem.DropDownItems.Clear();
			SortedList<string, Wilderness.FunctionDef> sortedList = new SortedList<string, Wilderness.FunctionDef>();
			for (int i = 0; i < Wilderness.NumFunctions; i++)
			{
				Wilderness.FunctionDef function = Wilderness.GetFunction(i);
				sortedList.Add(function.Name, function);
			}
			foreach (string current in sortedList.Keys)
			{
				ToolStripMenuItem value = new ToolStripMenuItem(current, null, new EventHandler(this.functionToolStripMenuItem_Click));
				this.insertFunctionToolStripMenuItem.DropDownItems.Add(value);
			}
		}
		private void InitContextMenu()
		{
			this.InitContextMenuFunctions();
		}
		public void UpdateCaretPosition()
		{
			CodeDocument codeDocument = this.sandDockManager1.ActiveTabbedDocument as CodeDocument;
			if (codeDocument == null)
			{
				return;
			}
			NomadTextBox.Position caretPosition = codeDocument.Content.CaretPosition;
			this.lineStatusLabel.Text = "Line " + (caretPosition.line + 1);
			this.columnStatusLabel.Text = "Col " + (caretPosition.column + 1);
			this.codeMapViewerDock1.Image = codeDocument.GetLineImage(codeDocument.Content.CaretPosition.line);
		}
		public void UpdateCaretPosition(CodeDocument document)
		{
			if (this.sandDockManager1.ActiveTabbedDocument != document)
			{
				return;
			}
			this.UpdateCaretPosition();
		}
		private void sandDockManager1_ActiveTabbedDocumentChanged(object sender, EventArgs e)
		{
			this.UpdateCaretPosition();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
			this.DisposeInternal();
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.codeMapViewerDock1 = new CodeMapViewerDock();
			this.sandDockManager1 = new SandDockManager();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.lineStatusLabel = new ToolStripStatusLabel();
			this.columnStatusLabel = new ToolStripStatusLabel();
			this.sandBarManager1 = new SandBarManager(this.components);
			this.leftSandBarDock = new ToolBarContainer();
			this.rightSandBarDock = new ToolBarContainer();
			this.bottomSandBarDock = new ToolBarContainer();
			this.topSandBarDock = new ToolBarContainer();
			this.menuBar1 = new MenuBar();
			this.menuBarItem1 = new MenuBarItem();
			this.newFileMenuItem = new MenuButtonItem();
			this.openScriptMenuItem = new MenuButtonItem();
			this.saveScriptMenuItem = new MenuButtonItem();
			this.exitMenuItem = new MenuButtonItem();
			this.menuBarItem2 = new MenuBarItem();
			this.undoMenuItem = new MenuButtonItem();
			this.redoMenuItem = new MenuButtonItem();
			this.cutMenuItem = new MenuButtonItem();
			this.copyMenuItem = new MenuButtonItem();
			this.pasteMenuItem = new MenuButtonItem();
			this.menuBarItem3 = new MenuBarItem();
			this.mapViewerMenuItem = new MenuButtonItem();
			this.menuBarItem4 = new MenuBarItem();
			this.runMenuItem = new MenuButtonItem();
			this.menuBarItem5 = new MenuBarItem();
			this.toolBar1 = new TD.SandBar.ToolBar();
			this.buttonItem1 = new ButtonItem();
			this.buttonItem3 = new ButtonItem();
			this.buttonItem2 = new ButtonItem();
			this.buttonItem4 = new ButtonItem();
			this.buttonItem5 = new ButtonItem();
			this.buttonItem6 = new ButtonItem();
			this.buttonItem8 = new ButtonItem();
			this.buttonItem9 = new ButtonItem();
			this.buttonItem7 = new ButtonItem();
			this.openFileDialog = new OpenFileDialog();
			this.contextMenuStrip1 = new ContextMenuStrip(this.components);
			this.cutToolStripMenuItem = new ToolStripMenuItem();
			this.copyToolStripMenuItem = new ToolStripMenuItem();
			this.pasteToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.insertFunctionToolStripMenuItem = new ToolStripMenuItem();
			this.insertTextureEntryIDToolStripMenuItem = new ToolStripMenuItem();
			this.insertCollectionEntryIDToolStripMenuItem = new ToolStripMenuItem();
			DockContainer dockContainer = new DockContainer();
			dockContainer.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.topSandBarDock.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			dockContainer.Controls.Add(this.codeMapViewerDock1);
			dockContainer.Dock = DockStyle.Right;
			dockContainer.LayoutSystem = new SplitLayoutSystem(250, 312, Orientation.Horizontal, new LayoutSystemBase[]
			{
				new ControlLayoutSystem(250, 312, new DockControl[]
				{
					this.codeMapViewerDock1
				}, this.codeMapViewerDock1)
			});
			dockContainer.Location = new Point(274, 49);
			dockContainer.Manager = this.sandDockManager1;
			dockContainer.Name = "dockContainer1";
			dockContainer.Size = new Size(254, 312);
			dockContainer.TabIndex = 6;
			this.codeMapViewerDock1.Guid = new Guid("40b77176-f5ac-44ce-a28c-d2f296197e1d");
			this.codeMapViewerDock1.Image = null;
			this.codeMapViewerDock1.Location = new Point(4, 18);
			this.codeMapViewerDock1.Name = "codeMapViewerDock1";
			this.codeMapViewerDock1.Size = new Size(250, 270);
			this.codeMapViewerDock1.TabIndex = 0;
			this.codeMapViewerDock1.Text = "Map Viewer";
			this.sandDockManager1.DockSystemContainer = this;
			this.sandDockManager1.OwnerForm = this;
			this.sandDockManager1.ActiveTabbedDocumentChanged += new EventHandler(this.sandDockManager1_ActiveTabbedDocumentChanged);
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1,
				this.lineStatusLabel,
				this.columnStatusLabel
			});
			this.statusStrip1.Location = new Point(0, 361);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(528, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(447, 17);
			this.toolStripStatusLabel1.Spring = true;
			this.lineStatusLabel.Name = "lineStatusLabel";
			this.lineStatusLabel.Size = new Size(35, 17);
			this.lineStatusLabel.Text = "Line 0";
			this.columnStatusLabel.Name = "columnStatusLabel";
			this.columnStatusLabel.Size = new Size(31, 17);
			this.columnStatusLabel.Text = "Col 0";
			this.sandBarManager1.OwnerForm = this;
			this.leftSandBarDock.Dock = DockStyle.Left;
			this.leftSandBarDock.Guid = new Guid("f51f9d3c-d12d-4b04-b3c1-dba150a785e6");
			this.leftSandBarDock.Location = new Point(0, 49);
			this.leftSandBarDock.Manager = this.sandBarManager1;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new Size(0, 334);
			this.leftSandBarDock.TabIndex = 2;
			this.rightSandBarDock.Dock = DockStyle.Right;
			this.rightSandBarDock.Guid = new Guid("c3e7b6e1-de74-4788-aa84-badf07d4feb3");
			this.rightSandBarDock.Location = new Point(528, 49);
			this.rightSandBarDock.Manager = this.sandBarManager1;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new Size(0, 334);
			this.rightSandBarDock.TabIndex = 3;
			this.bottomSandBarDock.Dock = DockStyle.Bottom;
			this.bottomSandBarDock.Guid = new Guid("214365d9-c066-4c57-b2e9-f4e9b8b58fd5");
			this.bottomSandBarDock.Location = new Point(0, 383);
			this.bottomSandBarDock.Manager = this.sandBarManager1;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new Size(528, 0);
			this.bottomSandBarDock.TabIndex = 4;
			this.topSandBarDock.Controls.Add(this.menuBar1);
			this.topSandBarDock.Controls.Add(this.toolBar1);
			this.topSandBarDock.Dock = DockStyle.Top;
			this.topSandBarDock.Guid = new Guid("31bfec4a-f321-4810-8e02-c4eccad1d7f9");
			this.topSandBarDock.Location = new Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager1;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new Size(528, 49);
			this.topSandBarDock.TabIndex = 5;
			this.menuBar1.Guid = new Guid("0188290c-f307-4042-a99f-b3a2a1517a38");
			this.menuBar1.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuBarItem1,
				this.menuBarItem2,
				this.menuBarItem3,
				this.menuBarItem4,
				this.menuBarItem5
			});
			this.menuBar1.Location = new Point(2, 0);
			this.menuBar1.Name = "menuBar1";
			this.menuBar1.OwnerForm = this;
			this.menuBar1.Size = new Size(526, 23);
			this.menuBar1.TabIndex = 0;
			this.menuBar1.Text = "menuBar1";
			this.menuBarItem1.Items.AddRange(new ToolbarItemBase[]
			{
				this.newFileMenuItem,
				this.openScriptMenuItem,
				this.saveScriptMenuItem,
				this.exitMenuItem
			});
			this.menuBarItem1.Text = "&File";
			this.newFileMenuItem.Image = Resources.newMap;
			this.newFileMenuItem.Shortcut = Shortcut.CtrlN;
			this.newFileMenuItem.Text = "New script";
			this.newFileMenuItem.Activate += new EventHandler(this.newFileMenuItem_Activate);
			this.openScriptMenuItem.Image = Resources.openMap;
			this.openScriptMenuItem.Shortcut = Shortcut.CtrlO;
			this.openScriptMenuItem.Text = "Open script";
			this.openScriptMenuItem.Activate += new EventHandler(this.openScriptMenuItem_Activate);
			this.saveScriptMenuItem.BeginGroup = true;
			this.saveScriptMenuItem.Image = Resources.save;
			this.saveScriptMenuItem.Shortcut = Shortcut.CtrlS;
			this.saveScriptMenuItem.Text = "Save script";
			this.saveScriptMenuItem.Activate += new EventHandler(this.saveScriptMenuItem_Activate);
			this.exitMenuItem.Text = "Exit";
			this.menuBarItem2.Items.AddRange(new ToolbarItemBase[]
			{
				this.undoMenuItem,
				this.redoMenuItem,
				this.cutMenuItem,
				this.copyMenuItem,
				this.pasteMenuItem
			});
			this.menuBarItem2.Text = "&Edit";
			this.undoMenuItem.Image = Resources.undo;
			this.undoMenuItem.Shortcut = Shortcut.CtrlZ;
			this.undoMenuItem.Text = "&Undo";
			this.undoMenuItem.Activate += new EventHandler(this.undoMenuItem_Activate);
			this.redoMenuItem.Image = Resources.redo;
			this.redoMenuItem.Shortcut = Shortcut.CtrlShiftZ;
			this.redoMenuItem.Text = "&Redo";
			this.cutMenuItem.BeginGroup = true;
			this.cutMenuItem.Image = Resources.cut;
			this.cutMenuItem.Shortcut = Shortcut.CtrlX;
			this.cutMenuItem.Text = "C&ut";
			this.cutMenuItem.Activate += new EventHandler(this.cutMenuItem_Activate);
			this.copyMenuItem.Image = Resources.copy;
			this.copyMenuItem.Shortcut = Shortcut.CtrlC;
			this.copyMenuItem.Text = "&Copy";
			this.copyMenuItem.Activate += new EventHandler(this.copyMenuItem_Activate);
			this.pasteMenuItem.Image = Resources.paste;
			this.pasteMenuItem.Shortcut = Shortcut.CtrlV;
			this.pasteMenuItem.Text = "&Paste";
			this.pasteMenuItem.Activate += new EventHandler(this.pasteMenuItem_Activate);
			this.menuBarItem3.Items.AddRange(new ToolbarItemBase[]
			{
				this.mapViewerMenuItem
			});
			this.menuBarItem3.Text = "&View";
			this.mapViewerMenuItem.Text = "Map Viewer";
			this.mapViewerMenuItem.Activate += new EventHandler(this.mapViewerMenuItem_Activate);
			this.menuBarItem4.Items.AddRange(new ToolbarItemBase[]
			{
				this.runMenuItem
			});
			this.menuBarItem4.MdiWindowList = true;
			this.menuBarItem4.Text = "&Script";
			this.runMenuItem.Image = Resources.PlayHS;
			this.runMenuItem.Shortcut = Shortcut.F5;
			this.runMenuItem.Text = "&Run";
			this.runMenuItem.Activate += new EventHandler(this.runMenuItem_Activate);
			this.menuBarItem5.Text = "&Help";
			this.toolBar1.DockLine = 1;
			this.toolBar1.Guid = new Guid("46756475-373b-4e41-8b89-f8ab1b41c370");
			this.toolBar1.Items.AddRange(new ToolbarItemBase[]
			{
				this.buttonItem1,
				this.buttonItem3,
				this.buttonItem2,
				this.buttonItem4,
				this.buttonItem5,
				this.buttonItem6,
				this.buttonItem8,
				this.buttonItem9,
				this.buttonItem7
			});
			this.toolBar1.Location = new Point(2, 23);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.Size = new Size(252, 26);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.Text = "toolBar1";
			this.buttonItem1.BuddyMenu = this.newFileMenuItem;
			this.buttonItem1.Image = Resources.newMap;
			this.buttonItem3.BuddyMenu = this.openScriptMenuItem;
			this.buttonItem3.Image = Resources.openMap;
			this.buttonItem2.BuddyMenu = this.saveScriptMenuItem;
			this.buttonItem2.Image = Resources.save;
			this.buttonItem4.BeginGroup = true;
			this.buttonItem4.BuddyMenu = this.cutMenuItem;
			this.buttonItem4.Image = Resources.cut;
			this.buttonItem5.BuddyMenu = this.copyMenuItem;
			this.buttonItem5.Image = Resources.copy;
			this.buttonItem6.BuddyMenu = this.pasteMenuItem;
			this.buttonItem6.Image = Resources.paste;
			this.buttonItem8.BeginGroup = true;
			this.buttonItem8.BuddyMenu = this.undoMenuItem;
			this.buttonItem8.Image = Resources.undo;
			this.buttonItem9.BuddyMenu = this.redoMenuItem;
			this.buttonItem9.Image = Resources.redo;
			this.buttonItem7.BeginGroup = true;
			this.buttonItem7.BuddyMenu = this.runMenuItem;
			this.buttonItem7.Image = Resources.PlayHS;
			this.openFileDialog.DefaultExt = "lua";
			this.openFileDialog.FileName = "openFileDialog1";
			this.openFileDialog.Filter = "Far Cry 2 script files (*.lua)|*.lua|All files (*.*)|*.*";
			this.openFileDialog.Title = "Open script";
			this.contextMenuStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.cutToolStripMenuItem,
				this.copyToolStripMenuItem,
				this.pasteToolStripMenuItem,
				this.toolStripSeparator1,
				this.insertFunctionToolStripMenuItem,
				this.insertTextureEntryIDToolStripMenuItem,
				this.insertCollectionEntryIDToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new Size(194, 164);
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new Size(193, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new EventHandler(this.cutToolStripMenuItem_Click);
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new Size(193, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new EventHandler(this.copyToolStripMenuItem_Click);
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new Size(193, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new EventHandler(this.pasteToolStripMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(190, 6);
			this.insertFunctionToolStripMenuItem.Name = "insertFunctionToolStripMenuItem";
			this.insertFunctionToolStripMenuItem.Size = new Size(193, 22);
			this.insertFunctionToolStripMenuItem.Text = "Insert function";
			this.insertTextureEntryIDToolStripMenuItem.Name = "insertTextureEntryIDToolStripMenuItem";
			this.insertTextureEntryIDToolStripMenuItem.Size = new Size(193, 22);
			this.insertTextureEntryIDToolStripMenuItem.Text = "Insert texture entry ID";
			this.insertCollectionEntryIDToolStripMenuItem.Name = "insertCollectionEntryIDToolStripMenuItem";
			this.insertCollectionEntryIDToolStripMenuItem.Size = new Size(193, 22);
			this.insertCollectionEntryIDToolStripMenuItem.Text = "Insert collection entry ID";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(528, 383);
			base.Controls.Add(dockContainer);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.leftSandBarDock);
			base.Controls.Add(this.rightSandBarDock);
			base.Controls.Add(this.bottomSandBarDock);
			base.Controls.Add(this.topSandBarDock);
			base.Name = "CodeEditor";
			this.Text = "Far Cry 2 Script Editor";
			dockContainer.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.topSandBarDock.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
