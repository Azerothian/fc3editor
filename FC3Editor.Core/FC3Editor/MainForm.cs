using FC3Editor.Nomad;
using FC3Editor.Properties;
using FC3Editor.Tools;
using FC3Editor.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TD.SandBar;
using TD.SandDock;
namespace FC3Editor
{
	internal class MainForm : NomadForm, IInputSink
	{
		private enum StatusBarMode
		{
			None,
			Loading,
			Navmesh,
			Ingame
		}
		private IContainer components;
		private SandBarManager sandBarManager;
		private ToolBarContainer leftSandBarDock;
		private ToolBarContainer rightSandBarDock;
		private ToolBarContainer bottomSandBarDock;
		private ToolBarContainer topSandBarDock;
		private MenuBar menuBar;
		private MenuBarItem menuBarItem1;
		private TD.SandBar.ToolBar toolBarMain;
		private MenuButtonItem menuItem_newMap;
		private MenuButtonItem menuItem_loadMap;
		private MenuButtonItem menuItem_saveMap;
		private MenuButtonItem menuItem_exit;
		private ButtonItem buttonItem1;
		private ButtonItem buttonItem2;
		private ButtonItem buttonItem3;
		private ViewportControl viewport;
		private SandDockManager sandDockManager;
		private ToolParametersDock toolParametersDock;
		private ToolTip toolTip;
		private MenuBarItem menuBarItem2;
		private MenuButtonItem menuItem_viewToolParameters;
		private MenuButtonItem menuItem_viewEditorSettings;
		private OpenFileDialog openMapDialog;
		private SaveFileDialog saveMapDialog;
		private MenuButtonItem menuItem_saveMapAs;
		private MenuBarItem menuBarItem3;
		private MenuButtonItem menuItem_Undo;
		private MenuButtonItem menuItem_Redo;
		private ButtonItem buttonItem4;
		private ButtonItem buttonItem5;
		private Timer timerUIUpdate;
		private MenuBarItem menuBarItem4;
		private MenuButtonItem menuItem_TestIngame;
		private EditorSettingsDock editorSettingsDock;
		private ContextHelpDock contextHelpDock;
		private MenuButtonItem menuItem_viewContextHelp;
		private StatusStrip statusBar;
		private ToolStripStatusLabel statusCaption;
		private ToolStripStatusLabel statusBarFpsItem;
		private ToolStripStatusLabel statusBarCameraPos;
		private ToolStripStatusLabel statusBarCursorPos;
		private ToolStripDropDownButton statusBarCameraSpeed;
		private ContextMenuStrip cameraSpeedStrip;
		private MenuBarItem menuBarItem5;
		private MenuButtonItem menuItem_OpenCodeEditor;
		private MenuButtonItem menuItem_ExportToConsole;
		private FolderBrowserDialog folderBrowserDialog;
		private ToolStripStatusLabel statusBarObjectUsage;
		private ToolStripStatusLabel statusBarMemoryUsage;
		private MenuButtonItem menuItem_ResetLayout;
		private ContextMenuStrip statusBarContextMenu;
		private ToolStripMenuItem whatsThisToolStripMenuItem;
		private MenuButtonItem menuItem_newWilderness;
		private MenuBarItem menuBarItem6;
		private MenuButtonItem menuItem_visitWebsite;
		private MenuButtonItem menuItem_about;
		private MenuButtonItem menuItem_ExportToPC;
		private MenuButtonItem menuItem_ReloadEditor;
		private MenuButtonItem menuItem_DumpMap;
		private MenuButtonItem menuItem_ExtractBigFile;
		private MenuButtonItem menuItem_PrepareThumbnails;
		private MenuButtonItem menuItem_ImportWorld;
		private MenuButtonItem menuItem_ObjectAdmin;
		private List<ButtonItem> m_toolItemList = new List<ButtonItem>();
		private Dictionary<Keys, ButtonItem> m_toolShortcuts = new Dictionary<Keys, ButtonItem>();
		private ImportWorldForm m_importWorldForm;
		private Form m_objectAdminForm;
		private string m_helpString;
		private static ITool m_currentTool;
		private string m_initMapPath;
		private MainForm.StatusBarMode m_statusBarMode;
		private float m_lastUpdate;
		private float m_currentMemoryUsage;
		private float m_currentObjectUsage;
		private Vec3 m_currentCameraPos;
		private bool m_currentCursorValid;
		private Vec3 m_currentCursorPos;
		private bool m_cursorPhysics = true;
		private float m_currentFps;
		private string m_documentPath;
		private Watermark m_watermark;
		private static MainForm s_instance;
		public bool EnableShortcuts
		{
			get
			{
				return this.menuBar.ShortcutListener.Listening;
			}
			set
			{
				this.menuBar.ShortcutListener.Listening = value;
				if (value)
				{
					this.viewport.BlockNextKeyRepeats = true;
				}
			}
		}
		public ITool CurrentTool
		{
			get
			{
				return MainForm.m_currentTool;
			}
			set
			{
				if (MainForm.m_currentTool != null)
				{
					MainForm.m_currentTool.Deactivate();
					if (MainForm.m_currentTool is IInputSink)
					{
						Editor.PopInput((IInputSink)MainForm.m_currentTool);
					}
				}
				MainForm.m_currentTool = value;
				if (MainForm.m_currentTool != null)
				{
					if (MainForm.m_currentTool is IInputSink)
					{
						Editor.PushInput((IInputSink)MainForm.m_currentTool);
					}
					MainForm.m_currentTool.Activate();
				}
				this.UpdateCurrentTool();
				if (MainForm.m_currentTool != null)
				{
					this.toolParametersDock.Open();
				}
			}
		}
		public string InitMapPath
		{
			set
			{
				this.m_initMapPath = value;
			}
		}
		public bool CloseSaveConfirmed
		{
			get;
			set;
		}
		public bool CursorPhysics
		{
			get
			{
				return this.m_cursorPhysics;
			}
			set
			{
				this.m_cursorPhysics = value;
			}
		}
		public string DocumentPath
		{
			get
			{
				return this.m_documentPath;
			}
			set
			{
				this.m_documentPath = value;
			}
		}
		public ViewportControl Viewport
		{
			get
			{
				return this.viewport;
			}
		}
		public ToolTip ToolTip
		{
			get
			{
				return this.toolTip;
			}
		}
		public static bool IsActive
		{
			get
			{
				IntPtr intPtr = Win32.GetActiveWindow();
				while (intPtr != IntPtr.Zero)
				{
					if (intPtr == MainForm.Instance.Handle)
					{
						return true;
					}
					intPtr = Win32.GetParent(intPtr);
				}
				return false;
			}
		}
		public static MainForm Instance
		{
			get
			{
				return MainForm.s_instance;
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.DisposeInternal();
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.sandDockManager = new SandDockManager();
			this.sandBarManager = new SandBarManager(this.components);
			this.leftSandBarDock = new ToolBarContainer();
			this.rightSandBarDock = new ToolBarContainer();
			this.bottomSandBarDock = new ToolBarContainer();
			this.topSandBarDock = new ToolBarContainer();
			this.menuBar = new MenuBar();
			this.menuBarItem1 = new MenuBarItem();
			this.menuItem_newMap = new MenuButtonItem();
			this.menuItem_newWilderness = new MenuButtonItem();
			this.menuItem_loadMap = new MenuButtonItem();
			this.menuItem_saveMap = new MenuButtonItem();
			this.menuItem_saveMapAs = new MenuButtonItem();
			this.menuItem_exit = new MenuButtonItem();
			this.menuBarItem3 = new MenuBarItem();
			this.menuItem_Undo = new MenuButtonItem();
			this.menuItem_Redo = new MenuButtonItem();
			this.menuBarItem2 = new MenuBarItem();
			this.menuItem_viewToolParameters = new MenuButtonItem();
			this.menuItem_viewEditorSettings = new MenuButtonItem();
			this.menuItem_viewContextHelp = new MenuButtonItem();
			this.menuBarItem4 = new MenuBarItem();
			this.menuItem_TestIngame = new MenuButtonItem();
			this.menuBarItem5 = new MenuBarItem();
			this.menuItem_OpenCodeEditor = new MenuButtonItem();
			this.menuItem_DumpMap = new MenuButtonItem();
			this.menuItem_ExtractBigFile = new MenuButtonItem();
			this.menuItem_ExportToPC = new MenuButtonItem();
			this.menuItem_ExportToConsole = new MenuButtonItem();
			this.menuItem_ResetLayout = new MenuButtonItem();
			this.menuItem_PrepareThumbnails = new MenuButtonItem();
			this.menuItem_ReloadEditor = new MenuButtonItem();
			this.menuItem_ImportWorld = new MenuButtonItem();
			this.menuItem_ObjectAdmin = new MenuButtonItem();
			this.menuBarItem6 = new MenuBarItem();
			this.menuItem_visitWebsite = new MenuButtonItem();
			this.menuItem_about = new MenuButtonItem();
			this.toolBarMain = new TD.SandBar.ToolBar();
			this.buttonItem1 = new ButtonItem();
			this.buttonItem2 = new ButtonItem();
			this.buttonItem3 = new ButtonItem();
			this.buttonItem4 = new ButtonItem();
			this.buttonItem5 = new ButtonItem();
			this.toolTip = new ToolTip(this.components);
			this.openMapDialog = new OpenFileDialog();
			this.saveMapDialog = new SaveFileDialog();
			this.timerUIUpdate = new Timer(this.components);
			this.statusBar = new StatusStrip();
			this.statusBarContextMenu = new ContextMenuStrip(this.components);
			this.whatsThisToolStripMenuItem = new ToolStripMenuItem();
			this.statusCaption = new ToolStripStatusLabel();
			this.statusBarCameraSpeed = new ToolStripDropDownButton();
			this.cameraSpeedStrip = new ContextMenuStrip(this.components);
			this.statusBarCameraPos = new ToolStripStatusLabel();
			this.statusBarCursorPos = new ToolStripStatusLabel();
			this.statusBarMemoryUsage = new ToolStripStatusLabel();
			this.statusBarObjectUsage = new ToolStripStatusLabel();
			this.statusBarFpsItem = new ToolStripStatusLabel();
			this.folderBrowserDialog = new FolderBrowserDialog();
			this.viewport = new ViewportControl();
			this.topSandBarDock.SuspendLayout();
			this.statusBar.SuspendLayout();
			this.statusBarContextMenu.SuspendLayout();
			base.SuspendLayout();
			this.sandDockManager.DockSystemContainer = this;
			this.sandDockManager.MaximumDockContainerSize = 2000;
			this.sandDockManager.OwnerForm = this;
			this.sandBarManager.OwnerForm = this;
			this.sandBarManager.Renderer = new WhidbeyRenderer();
			this.leftSandBarDock.Dock = DockStyle.Left;
			this.leftSandBarDock.Guid = new Guid("5af42533-b4bf-4ff9-b113-19c06ff822ad");
			this.leftSandBarDock.Location = new Point(0, 49);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new Size(0, 592);
			this.leftSandBarDock.TabIndex = 0;
			this.rightSandBarDock.Dock = DockStyle.Right;
			this.rightSandBarDock.Guid = new Guid("dd1d865e-52de-4df1-b60b-8d95778c6a99");
			this.rightSandBarDock.Location = new Point(973, 49);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new Size(0, 592);
			this.rightSandBarDock.TabIndex = 1;
			this.bottomSandBarDock.Dock = DockStyle.Bottom;
			this.bottomSandBarDock.Guid = new Guid("241b4d86-e08e-4fbd-9187-85417e2a6762");
			this.bottomSandBarDock.Location = new Point(0, 641);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new Size(973, 0);
			this.bottomSandBarDock.TabIndex = 2;
			this.topSandBarDock.Controls.Add(this.menuBar);
			this.topSandBarDock.Controls.Add(this.toolBarMain);
			this.topSandBarDock.Dock = DockStyle.Top;
			this.topSandBarDock.Guid = new Guid("9d4d899c-2f1c-48a5-b6b3-c2965fee85b0");
			this.topSandBarDock.Location = new Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new Size(973, 49);
			this.topSandBarDock.TabIndex = 3;
			this.menuBar.Guid = new Guid("67d4f929-8914-4b23-9c2d-b9539a54dd9b");
			this.menuBar.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuBarItem1,
				this.menuBarItem3,
				this.menuBarItem2,
				this.menuBarItem4,
				this.menuBarItem5,
				this.menuBarItem6
			});
			this.menuBar.Location = new Point(2, 0);
			this.menuBar.Movable = false;
			this.menuBar.Name = "menuBar";
			this.menuBar.OwnerForm = this;
			this.menuBar.Size = new Size(971, 23);
			this.menuBar.TabIndex = 0;
			this.menuBar.Text = "menuBar1";
			this.menuBarItem1.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_newMap,
				this.menuItem_newWilderness,
				this.menuItem_loadMap,
				this.menuItem_saveMap,
				this.menuItem_saveMapAs,
				this.menuItem_exit
			});
			this.menuBarItem1.Text = "MENU_FILE";
			this.menuItem_newMap.Shortcut = Shortcut.CtrlN;
			this.menuItem_newMap.Text = "MENUITEM_FILE_NEW_MAP";
			this.menuItem_newMap.Activate += new EventHandler(this.menuItem_newMap_Activate);
			this.menuItem_newWilderness.Text = "MENUITEM_FILE_NEW_WILDERNESS";
			this.menuItem_newWilderness.Activate += new EventHandler(this.menuItem_newWilderness_Activate);
			this.menuItem_loadMap.Shortcut = Shortcut.CtrlO;
			this.menuItem_loadMap.Text = "MENUITEM_FILE_LOAD_MAP";
			this.menuItem_loadMap.Activate += new EventHandler(this.menuItem_loadMap_Activate);
			this.menuItem_saveMap.BeginGroup = true;
			this.menuItem_saveMap.Shortcut = Shortcut.CtrlS;
			this.menuItem_saveMap.Text = "MENUITEM_FILE_SAVE_MAP";
			this.menuItem_saveMap.Activate += new EventHandler(this.menuItem_saveMap_Activate);
			this.menuItem_saveMapAs.Text = "MENUITEM_FILE_SAVE_MAP_AS";
			this.menuItem_saveMapAs.Activate += new EventHandler(this.menuItem_saveMapAs_Activate);
			this.menuItem_exit.BeginGroup = true;
			this.menuItem_exit.Text = "MENUITEM_FILE_EXIT";
			this.menuItem_exit.Activate += new EventHandler(this.menuItem_exit_Activate);
			this.menuBarItem3.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_Undo,
				this.menuItem_Redo
			});
			this.menuBarItem3.Text = "MENU_EDIT";
			this.menuItem_Undo.Shortcut = Shortcut.CtrlZ;
			this.menuItem_Undo.Text = "MENUITEM_EDIT_UNDO";
			this.menuItem_Undo.Activate += new EventHandler(this.menuItem_Undo_Activate);
			this.menuItem_Redo.Shortcut = Shortcut.CtrlShiftZ;
			this.menuItem_Redo.Text = "MENUITEM_EDIT_REDO";
			this.menuItem_Redo.Activate += new EventHandler(this.menuItem_Redo_Activate);
			this.menuBarItem2.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_viewToolParameters,
				this.menuItem_viewEditorSettings,
				this.menuItem_viewContextHelp
			});
			this.menuBarItem2.Text = "MENU_VIEW";
			this.menuItem_viewToolParameters.Text = "DOCK_TOOL_PARAMETERS";
			this.menuItem_viewToolParameters.Activate += new EventHandler(this.menuItem_viewToolParameters_Activate);
			this.menuItem_viewEditorSettings.Text = "DOCK_EDITOR_SETTINGS";
			this.menuItem_viewEditorSettings.Activate += new EventHandler(this.menuItem_viewEditorSettings_Activate);
			this.menuItem_viewContextHelp.Text = "DOCK_CONTEXT_HELP";
			this.menuItem_viewContextHelp.Activate += new EventHandler(this.menuItem_viewContextHelp_Activate);
			this.menuBarItem4.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_TestIngame
			});
			this.menuBarItem4.Text = "MENU_GAME";
			this.menuItem_TestIngame.Shortcut = Shortcut.CtrlG;
			this.menuItem_TestIngame.Text = "MENUITEM_GAME_TEST_INGAME";
			this.menuItem_TestIngame.Activate += new EventHandler(this.menuItem_TestIngame_Activate);
			this.menuBarItem5.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_OpenCodeEditor,
				this.menuItem_DumpMap,
				this.menuItem_ExtractBigFile,
				this.menuItem_ExportToPC,
				this.menuItem_ExportToConsole,
				this.menuItem_ResetLayout,
				this.menuItem_PrepareThumbnails,
				this.menuItem_ReloadEditor,
				this.menuItem_ImportWorld,
				this.menuItem_ObjectAdmin
			});
			this.menuBarItem5.Text = "MENU_ADVANCED";
			this.menuItem_OpenCodeEditor.Text = "*Code Editor";
			this.menuItem_OpenCodeEditor.Activate += new EventHandler(this.menuItem_OpenCodeEditor_Activate);
			this.menuItem_DumpMap.BeginGroup = true;
			this.menuItem_DumpMap.Text = "*Dump map";
			this.menuItem_DumpMap.Activate += new EventHandler(this.menuItem_DumpMap_Activate);
			this.menuItem_ExtractBigFile.Text = "*Extract Bigfile";
			this.menuItem_ExtractBigFile.Activate += new EventHandler(this.menuItem_ExtractBigFile_Activate);
			this.menuItem_ExportToPC.BeginGroup = true;
			this.menuItem_ExportToPC.Text = "*Export to PC";
			this.menuItem_ExportToPC.Activate += new EventHandler(this.menuItem_ExportToPC_Activate);
			this.menuItem_ExportToConsole.Text = "*Export to Console";
			this.menuItem_ExportToConsole.Activate += new EventHandler(this.menuItem_ExportToConsole_Activate);
			this.menuItem_ResetLayout.Text = "MENUITEM_ADVANCED_RESET_LAYOUT";
			this.menuItem_ResetLayout.Activate += new EventHandler(this.menuItem_ResetLayout_Activate);
			this.menuItem_PrepareThumbnails.BeginGroup = true;
			this.menuItem_PrepareThumbnails.Text = "*Prepare thumbnails";
			this.menuItem_PrepareThumbnails.Activate += new EventHandler(this.menuItem_PrepareThumbnails_Activate);
			this.menuItem_ReloadEditor.BeginGroup = true;
			this.menuItem_ReloadEditor.Text = "*Reload editor DLL";
			this.menuItem_ReloadEditor.Activate += new EventHandler(this.menuItem_ReloadEditor_Activate);
			this.menuItem_ImportWorld.BeginGroup = true;
			this.menuItem_ImportWorld.Text = "*Import world";
			this.menuItem_ImportWorld.Activate += new EventHandler(this.menuItem_ImportWorld_Activate);
			this.menuItem_ObjectAdmin.Text = "*Object admin";
			this.menuItem_ObjectAdmin.Activate += new EventHandler(this.menuItem_ObjectAdmin_Activate);
			this.menuBarItem6.Items.AddRange(new ToolbarItemBase[]
			{
				this.menuItem_visitWebsite,
				this.menuItem_about
			});
			this.menuBarItem6.Text = "MENU_HELP";
			this.menuItem_visitWebsite.Text = "MENUITEM_HELP_VISIT_WEBSITE";
			this.menuItem_visitWebsite.Activate += new EventHandler(this.menuItem_visitWebsite_Activate);
			this.menuItem_about.BeginGroup = true;
			this.menuItem_about.Text = "MENUITEM_HELP_ABOUT";
			this.menuItem_about.Activate += new EventHandler(this.menuItem_about_Activate);
			this.toolBarMain.DockLine = 1;
			this.toolBarMain.FlipLastItem = true;
			this.toolBarMain.Guid = new Guid("472238c1-4ade-4ff2-ba4f-4429ed30f50f");
			this.toolBarMain.Items.AddRange(new ToolbarItemBase[]
			{
				this.buttonItem1,
				this.buttonItem2,
				this.buttonItem3,
				this.buttonItem4,
				this.buttonItem5
			});
			this.toolBarMain.Location = new Point(2, 23);
			this.toolBarMain.Name = "toolBarMain";
			this.toolBarMain.Size = new Size(146, 26);
			this.toolBarMain.TabIndex = 1;
			this.toolBarMain.Text = "TOOLBAR_MAIN";
			this.buttonItem1.BuddyMenu = this.menuItem_newMap;
			this.buttonItem1.Image = Resources.newMap;
			this.buttonItem2.BuddyMenu = this.menuItem_loadMap;
			this.buttonItem2.Image = Resources.openMap;
			this.buttonItem3.BuddyMenu = this.menuItem_saveMap;
			this.buttonItem3.Image = Resources.save;
			this.buttonItem4.BeginGroup = true;
			this.buttonItem4.BuddyMenu = this.menuItem_Undo;
			this.buttonItem4.Image = Resources.undo;
			this.buttonItem5.BuddyMenu = this.menuItem_Redo;
			this.buttonItem5.Image = Resources.redo;
			this.openMapDialog.DefaultExt = "fc3map";
			this.openMapDialog.Filter = "Far Cry 3 maps (*.fc3map)|*.fc3map|All files (*.*)|*.*";
			this.openMapDialog.Title = "Open Far Cry 3 map";
			this.saveMapDialog.DefaultExt = "fc3map";
			this.saveMapDialog.Filter = "Far Cry 3 maps (*.fc3map)|*.fc3map|All files (*.*)|*.*";
			this.saveMapDialog.Title = "Save Far Cry 3 map";
			this.timerUIUpdate.Enabled = true;
			this.timerUIUpdate.Tick += new EventHandler(this.timerUIUpdate_Tick);
			this.statusBar.ContextMenuStrip = this.statusBarContextMenu;
			this.statusBar.Items.AddRange(new ToolStripItem[]
			{
				this.statusCaption,
				this.statusBarCameraSpeed,
				this.statusBarCameraPos,
				this.statusBarCursorPos,
				this.statusBarMemoryUsage,
				this.statusBarObjectUsage,
				this.statusBarFpsItem
			});
			this.statusBar.Location = new Point(0, 641);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new Size(973, 22);
			this.statusBar.TabIndex = 7;
			this.statusBar.Text = "statusStrip1";
			this.statusBarContextMenu.Items.AddRange(new ToolStripItem[]
			{
				this.whatsThisToolStripMenuItem
			});
			this.statusBarContextMenu.Name = "statusBarContextMenu";
			this.statusBarContextMenu.Size = new Size(179, 26);
			this.statusBarContextMenu.Opening += new CancelEventHandler(this.statusBarContextMenu_Opening);
			this.whatsThisToolStripMenuItem.Name = "whatsThisToolStripMenuItem";
			this.whatsThisToolStripMenuItem.Size = new Size(178, 22);
			this.whatsThisToolStripMenuItem.Text = "HELP_WHATS_THIS";
			this.whatsThisToolStripMenuItem.Click += new EventHandler(this.whatsThisToolStripMenuItem_Click);
			this.statusCaption.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusCaption.Name = "statusCaption";
			this.statusCaption.Size = new Size(408, 17);
			this.statusCaption.Spring = true;
			this.statusCaption.Text = "Ready";
			this.statusCaption.TextAlign = ContentAlignment.MiddleLeft;
			this.statusBarCameraSpeed.AutoSize = false;
			this.statusBarCameraSpeed.DropDown = this.cameraSpeedStrip;
			this.statusBarCameraSpeed.Image = Resources.speed;
			this.statusBarCameraSpeed.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarCameraSpeed.ImageTransparentColor = Color.Magenta;
			this.statusBarCameraSpeed.Name = "statusBarCameraSpeed";
			this.statusBarCameraSpeed.Size = new Size(55, 20);
			this.statusBarCameraSpeed.Text = "0";
			this.statusBarCameraSpeed.TextAlign = ContentAlignment.MiddleLeft;
			this.cameraSpeedStrip.Name = "cameraSpeedStrip";
			this.cameraSpeedStrip.OwnerItem = this.statusBarCameraSpeed;
			this.cameraSpeedStrip.ShowImageMargin = false;
			this.cameraSpeedStrip.Size = new Size(36, 4);
			this.statusBarCameraPos.AutoSize = false;
			this.statusBarCameraPos.Image = Resources.camera;
			this.statusBarCameraPos.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarCameraPos.Name = "statusBarCameraPos";
			this.statusBarCameraPos.Size = new Size(150, 17);
			this.statusBarCameraPos.Text = "(0, 0, 0)";
			this.statusBarCameraPos.TextAlign = ContentAlignment.MiddleLeft;
			this.statusBarCursorPos.AutoSize = false;
			this.statusBarCursorPos.Image = Resources.cursor;
			this.statusBarCursorPos.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarCursorPos.Name = "statusBarCursorPos";
			this.statusBarCursorPos.Size = new Size(150, 17);
			this.statusBarCursorPos.Text = "(0, 0, 0)";
			this.statusBarCursorPos.TextAlign = ContentAlignment.MiddleLeft;
			this.statusBarMemoryUsage.AutoSize = false;
			this.statusBarMemoryUsage.Image = Resources.MemoryUsage;
			this.statusBarMemoryUsage.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarMemoryUsage.Name = "statusBarMemoryUsage";
			this.statusBarMemoryUsage.Size = new Size(60, 17);
			this.statusBarMemoryUsage.Text = "0";
			this.statusBarMemoryUsage.TextAlign = ContentAlignment.MiddleLeft;
			this.statusBarObjectUsage.AutoSize = false;
			this.statusBarObjectUsage.Image = Resources.ObjectUsage;
			this.statusBarObjectUsage.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarObjectUsage.Name = "statusBarObjectUsage";
			this.statusBarObjectUsage.Size = new Size(60, 17);
			this.statusBarObjectUsage.Text = "0";
			this.statusBarObjectUsage.TextAlign = ContentAlignment.MiddleLeft;
			this.statusBarFpsItem.AutoSize = false;
			this.statusBarFpsItem.Image = Resources.frametime;
			this.statusBarFpsItem.ImageAlign = ContentAlignment.MiddleLeft;
			this.statusBarFpsItem.Name = "statusBarFpsItem";
			this.statusBarFpsItem.Size = new Size(75, 17);
			this.statusBarFpsItem.Text = "0.0 fps";
			this.statusBarFpsItem.TextAlign = ContentAlignment.MiddleLeft;
			this.folderBrowserDialog.Description = "Select export folder";
			this.viewport.BackColor = SystemColors.AppWorkspace;
			this.viewport.BlockNextKeyRepeats = false;
			this.viewport.BorderStyle = BorderStyle.Fixed3D;
			this.viewport.CameraEnabled = true;
			this.viewport.CaptureMouse = false;
			this.viewport.CaptureWheel = false;
			this.viewport.Cursor = Cursors.Default;
			this.viewport.DefaultCursor = Cursors.Default;
			this.viewport.Dock = DockStyle.Fill;
			this.viewport.Location = new Point(0, 49);
			this.viewport.Name = "viewport";
			this.viewport.Size = new Size(973, 592);
			this.viewport.TabIndex = 5;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(973, 663);
			base.Controls.Add(this.viewport);
			base.Controls.Add(this.leftSandBarDock);
			base.Controls.Add(this.rightSandBarDock);
			base.Controls.Add(this.bottomSandBarDock);
			base.Controls.Add(this.topSandBarDock);
			base.Controls.Add(this.statusBar);
			base.Name = "MainForm";
			this.Text = "Far Cry 3 Editor";
			base.Activated += new EventHandler(this.MainForm_Activated);
			base.Deactivate += new EventHandler(this.MainForm_Deactivate);
			base.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
			base.FormClosed += new FormClosedEventHandler(this.MainForm_FormClosed);
			base.Load += new EventHandler(this.MainForm_Load);
			base.LocationChanged += new EventHandler(this.MainForm_LocationChanged);
			base.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
			this.topSandBarDock.ResumeLayout(false);
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.statusBarContextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public MainForm()
		{
			MainForm.s_instance = this;
			this.InitializeComponent();
			Win32.SetProp(base.Handle, Program.programGuid, base.Handle);
			Editor.PushInput(this);
		}
		private void DisposeInternal()
		{
			Editor.PopInput(this);
			Win32.RemoveProp(base.Handle, Program.programGuid);
		}
		public void InitializeDocks()
		{
			this.toolParametersDock = new ToolParametersDock();
			this.toolParametersDock.Guid = new Guid("3b44d7d6-f472-4373-9bac-a5d4cc471425");
			this.toolParametersDock.Manager = this.sandDockManager;
			this.contextHelpDock = new ContextHelpDock();
			this.contextHelpDock.Guid = new Guid("4a3bcfd3-d4b0-44a0-bac0-bfd030fbc69b");
			this.contextHelpDock.Manager = this.sandDockManager;
			this.editorSettingsDock = new EditorSettingsDock();
			this.editorSettingsDock.Guid = new Guid("ad8a4d52-f3ba-4463-9ce3-4cbed143ec05");
			this.editorSettingsDock.Manager = this.sandDockManager;
		}
		public void EnableUI(bool enable)
		{
			foreach (Control control in base.Controls)
			{
				control.Enabled = enable;
			}
			if (enable)
			{
				this.Viewport.Focus();
				this.editorSettingsDock.RefreshSettings();
			}
		}
		private TD.SandBar.ToolBar CreateToolbar(string id, string text, int row, TD.SandBar.ToolBar afterToolbar)
		{
			TD.SandBar.ToolBar toolBar = new TD.SandBar.ToolBar();
			toolBar.Guid = new Guid(id.GetHashCode(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			toolBar.DockLine = row;
			toolBar.DockOffset = afterToolbar.DockOffset + afterToolbar.Width;
			this.sandBarManager.AddToolbar(toolBar);
			toolBar.Redock(this.sandBarManager.FindSuitableContainer(DockStyle.Top));
			toolBar.Text = text;
			return toolBar;
		}
		private ButtonItem AddTool(TD.SandBar.ToolBar toolbar, IToolBase tool, Keys shortcut)
		{
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.Image = tool.GetToolImage();
			buttonItem.ToolTipText = StringUtils.EscapeUIString(tool.GetToolName()) + ((shortcut != Keys.None) ? (" (" + shortcut.ToString() + ")") : "");
			buttonItem.Tag = tool;
			if (tool is ITool)
			{
				buttonItem.Activate += new EventHandler(this.tool_Activate);
			}
			else
			{
				if (tool is IToolAction)
				{
					buttonItem.Activate += new EventHandler(this.toolBase_Activate);
				}
			}
			toolbar.Items.Add(buttonItem);
			this.m_toolItemList.Add(buttonItem);
			if (shortcut != Keys.None)
			{
				this.m_toolShortcuts.Add(shortcut, buttonItem);
			}
			return buttonItem;
		}
		public void InitializeTools()
		{
			this.AddTool(this.toolBarMain, new ToolProperties(), Keys.None).BeginGroup = true;
			this.AddTool(this.toolBarMain, new ToolValidation(), Keys.None);
			TD.SandBar.ToolBar toolBar = this.CreateToolbar("Terrain", Localizer.Localize("TOOLBAR_TERRAIN"), 1, this.toolBarMain);
			this.AddTool(toolBar, new ToolTerrainBump(), Keys.F1);
			this.AddTool(toolBar, new ToolTerrainRaiseLower(), Keys.F2);
			this.AddTool(toolBar, new ToolTerrainFlatten(), Keys.F3);
			this.AddTool(toolBar, new ToolTerrainSetHeight(), Keys.None);
			this.AddTool(toolBar, new ToolTerrainSmooth(), Keys.F4);
			this.AddTool(toolBar, new ToolTerrainRamp(), Keys.F5);
			this.AddTool(toolBar, new ToolTerrainNoise(), Keys.F6);
			this.AddTool(toolBar, new ToolTerrainErosion(), Keys.F7);
			this.AddTool(toolBar, new ToolTerrainHole(), Keys.None);
			this.AddTool(toolBar, new ToolTexture(), Keys.F8);
			this.AddTool(toolBar, new ToolWater(), Keys.None);
			TD.SandBar.ToolBar toolBar2 = this.CreateToolbar("Objects", Localizer.Localize("TOOLBAR_OBJECTS"), 1, toolBar);
			this.AddTool(toolBar2, new ToolObject(), Keys.F9);
			this.AddTool(toolBar2, new ToolCollection(), Keys.F10);
			this.AddTool(toolBar2, new ToolRoad(), Keys.F11);
			this.AddTool(toolBar2, new ToolPlayableZone(), Keys.F12);
			TD.SandBar.ToolBar toolbar = this.CreateToolbar("Miscellaneous", Localizer.Localize("TOOLBAR_MISCELLANEOUS"), 1, toolBar2);
			this.AddTool(toolbar, new ToolNavmesh(), Keys.None);
			this.AddTool(toolbar, new ToolEnvironment(), Keys.None);
		}
		private void toolBase_Activate(object sender, EventArgs e)
		{
			IToolAction toolAction = (IToolAction)((ButtonItem)sender).Tag;
			toolAction.Fire();
		}
		private void tool_Activate(object sender, EventArgs e)
		{
			ITool tool = (ITool)((ButtonItem)sender).Tag;
			if (this.CurrentTool == tool)
			{
				this.CurrentTool = null;
				return;
			}
			this.CurrentTool = tool;
		}
		private void UpdateToolbar()
		{
			foreach (ButtonItem current in this.m_toolItemList)
			{
				if (current.Tag == this.CurrentTool)
				{
					if (!current.Checked)
					{
						current.Checked = true;
					}
				}
				else
				{
					if (current.Checked)
					{
						current.Checked = false;
					}
				}
			}
		}
		public void ClearMapPath()
		{
			this.m_documentPath = null;
			this.UpdateTitleBar();
		}
		private bool PromptSave(EditorDocument.SaveCompletedCallback callback)
		{
			DialogResult dialogResult = LocalizedMessageBox.Show(MainForm.Instance, Localizer.Localize("EDITOR_CHANGE_MAP_PROMPT"), Localizer.Localize("EDITOR_CONFIRMATION"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), Localizer.Localize("Generic", "GENERIC_CANCEL"), MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			DialogResult dialogResult2 = dialogResult;
			if (dialogResult2 == DialogResult.Cancel)
			{
				return false;
			}
			switch (dialogResult2)
			{
			case DialogResult.Yes:
				this.SaveMap(false, true, callback);
				return false;

			case DialogResult.No:
				return true;

			default:
				return false;
			}
		}
		public bool NewMap()
		{
			if (!this.PromptSave(delegate(bool success)
			{
				if (success)
				{
					this.NewMapInternal();
				}
			}
			))
			{
				return false;
			}
			this.NewMapInternal();
			return true;
		}
		private void NewMapInternal()
		{
			this.CurrentTool = null;
			EditorDocument.Reset();
			this.m_documentPath = null;
			this.UpdateTitleBar();
			this.editorSettingsDock.RefreshSettings();
		}
		public bool NewWilderness()
		{
			if (!this.PromptSave(delegate(bool success)
			{
				if (success)
				{
					this.NewWildernessInternal();
				}
			}
			))
			{
				return false;
			}
			this.NewWildernessInternal();
			return true;
		}
		private void NewWildernessInternal()
		{
			this.NewMapInternal();
			using (PromptInventory promptInventory = new PromptInventory())
			{
				promptInventory.Root = WildernessInventory.Instance.Root;
				if (promptInventory.ShowDialog(this) == DialogResult.OK)
				{
					Wilderness.RunScriptEntry((WildernessInventory.Entry)promptInventory.Value);
				}
			}
		}
		public void SetupImportMap(string title)
		{
			EditorDocument.Reset();
			this.m_documentPath = null;
			this.SetTitleBar(title);
		}
		private void LoadMap(string fileName, EditorDocument.LoadCompletedCallback callback)
		{
			if (!this.PromptSave(delegate(bool success)
			{
				if (success)
				{
					this.LoadMapInternal(null, callback);
				}
			}
			))
			{
				return;
			}
			this.LoadMapInternal(fileName, callback);
		}
		private void LoadMapInternal(string fileName, EditorDocument.LoadCompletedCallback callback)
		{
			if (fileName == null)
			{
				if (this.openMapDialog.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				fileName = this.openMapDialog.FileName;
			}
			this.m_documentPath = fileName;
			this.CurrentTool = null;
			EditorDocument.Load(this.m_documentPath, callback);
			this.UpdateTitleBar();
		}
		private bool SaveMap(bool saveAs, bool silent, EditorDocument.SaveCompletedCallback callback)
		{
			if (!EditorDocument.Validate() && LocalizedMessageBox.Show(Localizer.Localize("ERROR_VALIDATION_FAILED"), Localizer.Localize("ERROR"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), null, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1) == DialogResult.No)
			{
				return false;
			}
			string text = this.m_documentPath;
			if (saveAs || this.m_documentPath == null)
			{
				this.saveMapDialog.FileName = text;
				if (this.saveMapDialog.ShowDialog(this) != DialogResult.OK)
				{
					return false;
				}
				text = this.saveMapDialog.FileName;
			}
			if (EditorDocument.MapName == EditorDocument.DefaultMapName)
			{
				using (PromptForm promptForm = new PromptForm(Localizer.LocalizeCommon("VKEYBOARD_TITLE_MAPNAME")))
				{
					promptForm.Text = Localizer.LocalizeCommon("VKEYBOARD_DESC_MAPNAME");
					promptForm.Input = Path.GetFileNameWithoutExtension(text);
					promptForm.MaxLength = 20;
					if (promptForm.ShowDialog(this) != DialogResult.OK)
					{
						bool result = false;
						return result;
					}
					EditorDocument.MapName = promptForm.Input;
				}
			}
			string text2 = null;
			if (string.IsNullOrEmpty(EditorDocument.CreatorName))
			{
				using (PromptForm promptForm2 = new PromptForm(Localizer.Localize("PROMPT_CREATOR_TEXT")))
				{
					promptForm2.Text = Localizer.Localize("PROMPT_CREATOR_TITLE");
					promptForm2.Input = EditorDocument.AuthorName;
					promptForm2.MaxLength = 20;
					if (promptForm2.ShowDialog(this) != DialogResult.OK)
					{
						bool result = false;
						return result;
					}
					text2 = promptForm2.Input;
				}
			}
			if (saveAs)
			{
				EditorDocument.MapId = Guid.NewGuid();
			}
			if (text2 != null)
			{
				EditorDocument.CreatorName = text2;
			}
			this.m_documentPath = text;
			EditorDocument.Save(this.m_documentPath, callback);
			this.UpdateTitleBar();
			return true;
		}
		private void ExportMap(bool toConsole)
		{
			if (this.openMapDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			if (this.folderBrowserDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			EditorDocument.Export(this.openMapDialog.FileName, this.folderBrowserDialog.SelectedPath, toConsole);
		}
		private void DumpMap()
		{
			if (this.openMapDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			EditorDocument.Dump(this.openMapDialog.FileName, this.openMapDialog.FileName + ".dump");
		}
		private void ExtractBigFile()
		{
			if (this.openMapDialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			EditorDocument.ExtractBigFile(this.openMapDialog.FileName, Path.GetDirectoryName(this.openMapDialog.FileName), Path.GetFileNameWithoutExtension(this.openMapDialog.FileName));
		}
		private void EnableIngameUI(bool enable)
		{
			this.Viewport.CaptureMouse = enable;
			foreach (Control control in base.Controls)
			{
				if (control != this.Viewport && control != this.statusBar)
				{
					control.Enabled = !enable;
					control.Visible = !enable;
				}
			}
			foreach (ToolStripItem toolStripItem in this.statusBar.Items)
			{
				if (toolStripItem != this.statusCaption && toolStripItem != this.statusBarFpsItem)
				{
					toolStripItem.Visible = !enable;
				}
			}
			DockControl[] dockControls = this.sandDockManager.GetDockControls();
			for (int i = 0; i < dockControls.Length; i++)
			{
				DockControl dockControl = dockControls[i];
				dockControl.Enabled = !enable;
				dockControl.Visible = !enable;
				if (dockControl.IsFloating && dockControl.ParentForm != null)
				{
					dockControl.ParentForm.Visible = !enable;
				}
			}
			TD.SandBar.ToolBar[] toolBars = this.sandBarManager.GetToolBars();
			for (int j = 0; j < toolBars.Length; j++)
			{
				TD.SandBar.ToolBar toolBar = toolBars[j];
				toolBar.Enabled = !enable;
				toolBar.Visible = !enable;
			}
			this.Viewport.Focus();
		}
		public void EnterIngame()
		{
			this.CurrentTool = null;
			this.EnableIngameUI(true);
		}
		public void ExitIngame()
		{
			this.EnableIngameUI(false);
		}
		private void menuItem_newMap_Activate(object sender, EventArgs e)
		{
			this.NewMap();
		}
		private void menuItem_newWilderness_Activate(object sender, EventArgs e)
		{
			this.NewWilderness();
		}
		private void menuItem_loadMap_Activate(object sender, EventArgs e)
		{
			this.LoadMap(null, null);
		}
		private void menuItem_saveMap_Activate(object sender, EventArgs e)
		{
			this.SaveMap(false, false, null);
		}
		private void menuItem_saveMapAs_Activate(object sender, EventArgs e)
		{
			this.SaveMap(true, false, null);
		}
		private void menuItem_exit_Activate(object sender, EventArgs e)
		{
			base.Close();
		}
		private void menuItem_Undo_Activate(object sender, EventArgs e)
		{
			UndoManager.Undo();
		}
		private void menuItem_Redo_Activate(object sender, EventArgs e)
		{
			UndoManager.Redo();
		}
		private void menuItem_viewToolParameters_Activate(object sender, EventArgs e)
		{
			this.toolParametersDock.Open();
		}
		private void menuItem_viewEditorSettings_Activate(object sender, EventArgs e)
		{
			this.editorSettingsDock.Open();
		}
		private void menuItem_viewContextHelp_Activate(object sender, EventArgs e)
		{
			this.contextHelpDock.Open();
		}
		private void menuItem_TestIngame_Activate(object sender, EventArgs e)
		{
			Editor.EnterIngame("FCXEditor");
		}
		private void menuItem_OpenCodeEditor_Activate(object sender, EventArgs e)
		{
			if (CodeEditor.Instance == null)
			{
				new CodeEditor();
			}
			CodeEditor.Instance.Show();
		}
		private void menuItem_FlushCache_Activate(object sender, EventArgs e)
		{
			ObjectRenderer.ClearCache();
		}
		private void menuItem_ResetLayout_Activate(object sender, EventArgs e)
		{
			try
			{
				this.sandDockManager.SetLayout(Resources.DefaultSandDockLayout);
			}
			catch (Exception)
			{
			}
			try
			{
				this.sandBarManager.SetLayout(Resources.DefaultSandBarLayout);
			}
			catch (Exception)
			{
			}
		}
		private void menuItem_PrepareThumbnails_Activate(object sender, EventArgs e)
		{
			ObjectRenderer.ResizeThumbnails();
		}
		private void menuItem_DumpMap_Activate(object sender, EventArgs e)
		{
			this.DumpMap();
		}
		private void menuItem_ExtractBigFile_Activate(object sender, EventArgs e)
		{
			this.ExtractBigFile();
		}
		private void menuItem_ExportToPC_Activate(object sender, EventArgs e)
		{
			this.ExportMap(false);
		}
		private void menuItem_ExportToConsole_Activate(object sender, EventArgs e)
		{
			this.ExportMap(true);
		}
		private void menuItem_ReloadEditor_Activate(object sender, EventArgs e)
		{
			Engine.Reload(Engine.EvaluateReloadState());
		}
		private void menuItem_ImportWorld_Activate(object sender, EventArgs e)
		{
			if (this.m_importWorldForm != null && !this.m_importWorldForm.IsDisposed)
			{
				this.m_importWorldForm.Show();
				return;
			}
			this.m_importWorldForm = new ImportWorldForm();
			this.m_importWorldForm.Show();
		}
		private void menuItem_ObjectAdmin_Activate(object sender, EventArgs e)
		{
			if (this.m_objectAdminForm != null && !this.m_objectAdminForm.IsDisposed)
			{
				this.m_objectAdminForm.Show();
				return;
			}
			this.m_objectAdminForm = new Form();
			this.m_objectAdminForm.Text = "Object admin";
			ObjectAdmin objectAdmin = new ObjectAdmin();
			this.m_objectAdminForm.Controls.Add(objectAdmin);
			objectAdmin.Dock = DockStyle.Fill;
			this.m_objectAdminForm.Size = new Size(1024, 768);
			this.m_objectAdminForm.StartPosition = FormStartPosition.CenterParent;
			this.m_objectAdminForm.Show();
		}
		private void menuItem_visitWebsite_Activate(object sender, EventArgs e)
		{
			try
			{
				Process.Start("http://www.farcrygame.com");
			}
			catch (Exception)
			{
			}
		}
		private void menuItem_about_Activate(object sender, EventArgs e)
		{
			using (SplashForm splashForm = new SplashForm())
			{
				splashForm.AboutMode = true;
				splashForm.ShowDialog(this);
			}
		}
		private void statusBarContextMenu_Opening(object sender, CancelEventArgs e)
		{
			Point position = Cursor.Position;
			this.m_helpString = null;
			if (this.statusBar.RectangleToScreen(this.statusBarFpsItem.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_PERFORMANCE");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusBarObjectUsage.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_OBJECT_COST");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusBarMemoryUsage.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_MEMORY_USAGE");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusBarCursorPos.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_CURSOR_POS");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusBarCameraPos.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_CAMERA_POS");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusBarCameraSpeed.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_CAMERA_SPEED");
				return;
			}
			if (this.statusBar.RectangleToScreen(this.statusCaption.Bounds).Contains(position))
			{
				this.m_helpString = Localizer.Localize("HELP_STATUSBAR_STATE");
			}
		}
		private void whatsThisToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.m_helpString != null)
			{
				LocalizedMessageBox.Show(this, this.m_helpString, Localizer.Localize("HELP_WHATS_THIS"), Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
				return;
			}
			MessageBox.Show(this, "Help text not defined.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		private void LoadSettings()
		{
			RegistryKey registrySettings = Editor.GetRegistrySettings();
			Rectangle rectangle = default(Rectangle);
			rectangle.X = Editor.GetRegistryInt(registrySettings, "MainFormX", base.Bounds.X);
			rectangle.Y = Editor.GetRegistryInt(registrySettings, "MainFormY", base.Bounds.Y);
			rectangle.Width = Editor.GetRegistryInt(registrySettings, "MainFormW", base.Bounds.Width);
			rectangle.Height = Editor.GetRegistryInt(registrySettings, "MainFormH", base.Bounds.Height);
			Screen[] allScreens = Screen.AllScreens;
			for (int i = 0; i < allScreens.Length; i++)
			{
				Screen screen = allScreens[i];
				if (screen.Bounds.IntersectsWith(rectangle))
				{
					base.Bounds = rectangle;
					base.StartPosition = FormStartPosition.Manual;
					break;
				}
			}
			try
			{
				this.sandDockManager.SetLayout(Editor.GetRegistryString(registrySettings, "SandDock", Resources.DefaultSandDockLayout));
			}
			catch (Exception)
			{
			}
			try
			{
				this.sandBarManager.SetLayout(Editor.GetRegistryString(registrySettings, "SandBar", Resources.DefaultSandBarLayout));
			}
			catch (Exception)
			{
			}
			TD.SandBar.ToolBar[] toolBars = this.sandBarManager.GetToolBars();
			for (int j = 0; j < toolBars.Length; j++)
			{
				TD.SandBar.ToolBar toolBar = toolBars[j];
				if (toolBar.Parent != null)
				{
					toolBar.Parent.Visible = true;
				}
			}
			registrySettings.Close();
		}
		private void SaveSettings()
		{
			RegistryKey registrySettings = Editor.GetRegistrySettings();
			registrySettings.SetValue("MainFormX", base.Bounds.X);
			registrySettings.SetValue("MainFormY", base.Bounds.Y);
			registrySettings.SetValue("MainFormW", base.Bounds.Width);
			registrySettings.SetValue("MainFormH", base.Bounds.Height);
			registrySettings.SetValue("SandDock", this.sandDockManager.GetLayout());
			registrySettings.SetValue("SandBar", this.sandBarManager.GetLayout());
			registrySettings.Close();
		}
		private void UpdateCurrentTool()
		{
			this.toolParametersDock.Tool = this.CurrentTool;
			this.contextHelpDock.Tool = this.CurrentTool;
			this.UpdateToolbar();
		}
		private void SetTitleBar(string title)
		{
			this.Text = title + " - " + Localizer.Localize("EDITOR_NAME");
		}
		private void UpdateTitleBar()
		{
			string titleBar;
			if (this.m_documentPath == null)
			{
				titleBar = Localizer.Localize("EDITOR_UNTITLED");
			}
			else
			{
				titleBar = Path.GetFileNameWithoutExtension(this.m_documentPath);
			}
			this.SetTitleBar(titleBar);
		}
		private void Localize()
		{
			Localizer.Localize(this.menuBar);
			this.toolBarMain.Text = Localizer.Localize(this.toolBarMain.Text);
			this.whatsThisToolStripMenuItem.Text = Localizer.Localize(this.whatsThisToolStripMenuItem.Text);
			SandBarLanguage.AddRemoveButtonsText = Localizer.Localize("SANDBAR_ADDREMOVEBUTTONSTEXT");
			SandBarLanguage.CloseMenuText = Localizer.Localize("SANDBAR_CLOSEMENUTEXT");
			SandBarLanguage.CloseWindowText = Localizer.Localize("SANDBAR_CLOSEWINDOWTEXT");
			SandBarLanguage.MaximizeMenuText = Localizer.Localize("SANDBAR_MAXIMIZEMENUTEXT");
			SandBarLanguage.MinimizeMenuText = Localizer.Localize("SANDBAR_MINIMIZEMENUTEXT");
			SandBarLanguage.MinimizeWindowText = Localizer.Localize("SANDBAR_MINIMIZEWINDOWTEXT");
			SandBarLanguage.MoveMenuText = Localizer.Localize("SANDBAR_MOVEMENUTEXT");
			SandBarLanguage.RestoreMenuText = Localizer.Localize("SANDBAR_RESTOREMENUTEXT");
			SandBarLanguage.RestoreWindowText = Localizer.Localize("SANDBAR_RESTOREWINDOWTEXT");
			SandBarLanguage.SizeMenuText = Localizer.Localize("SANDBAR_SIZEMENUTEXT");
			SandBarLanguage.ToolbarOptionsText = Localizer.Localize("SANDBAR_TOOLBAROPTIONSTEXT");
			SandDockLanguage.AutoHideText = Localizer.Localize("SANDDOCK_AUTOHIDETEXT");
			SandDockLanguage.CloseText = Localizer.Localize("SANDDOCK_CLOSETEXT");
			SandDockLanguage.ScrollLeftText = Localizer.Localize("SANDDOCK_SCROLLLEFTTEXT");
			SandDockLanguage.ScrollRightText = Localizer.Localize("SANDDOCK_SCROLLRIGHTTEXT");
			SandDockLanguage.WindowPositionText = Localizer.Localize("SANDDOCK_WINDOWPOSITIONTEXT");
		}
		private void MainForm_Load(object sender, EventArgs e)
		{
			this.Localize();
			this.InitCameraSpeed();
			this.InitializeDocks();
			this.InitializeTools();
			this.LoadSettings();
			this.UpdateCurrentTool();
			this.UpdateTitleBar();
			this.openMapDialog.InitialDirectory = Engine.PersonalPath;
			this.saveMapDialog.InitialDirectory = Engine.PersonalPath;
			if (!Debugger.IsAttached || !Program.HasArgument("-usereload"))
			{
				this.menuItem_ReloadEditor.Visible = false;
			}
		}
		public void PostLoad()
		{
			string text = (this.m_initMapPath != null) ? this.m_initMapPath : Program.GetMapArgument();
			if (text != null)
			{
				this.LoadMapInternal(text, null);
				return;
			}
			if (Program.HasArgument("-generateObjectThumbnails"))
			{
				Engine.TickAlways = true;
				ObjectRenderer.GenerateThumbnails();
				return;
			}
			if (Program.HasArgument("-generateCollectionThumbnails"))
			{
				Engine.TickAlways = true;
				CollectionRenderer.GenerateThumbnails();
				return;
			}
			if (Program.HasArgument("-resizeThumbnails"))
			{
				ObjectRenderer.ResizeThumbnails();
				return;
			}
			if (!Program.HasArgument("-nowilderness"))
			{
				List<Inventory.Entry> list = new List<Inventory.Entry>(WildernessInventory.Instance.Root.GetRecursiveEntries());
				Random random = new Random();
				WildernessInventory.Entry entry = (WildernessInventory.Entry)list[random.Next(list.Count)];
				Wilderness.RunScriptEntry(entry);
				return;
			}
			EditorDocument.Reset();
		}
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Editor.IsIngame)
			{
				Editor.ExitIngame();
			}
			if (!this.CloseSaveConfirmed && !this.PromptSave(delegate(bool success)
			{
				if (success)
				{
					this.CloseSaveConfirmed = true;
					base.Close();
				}
			}
			))
			{
				e.Cancel = true;
				return;
			}
			this.SaveSettings();
		}
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
		}
		private void MainForm_Activated(object sender, EventArgs e)
		{
			this.Viewport.UpdateFocus();
		}
		private void MainForm_Deactivate(object sender, EventArgs e)
		{
			this.Viewport.UpdateFocus();
		}
		private void MainForm_LocationChanged(object sender, EventArgs e)
		{
			this.UpdateWatermark();
		}
		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			this.UpdateWatermark();
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.F10 && Editor.Viewport.Focused && !Editor.IsIngame)
			{
				this.OnKeyEvent(Editor.KeyEvent.KeyDown, new KeyEventArgs(Keys.F10));
				this.OnKeyEvent(Editor.KeyEvent.KeyUp, new KeyEventArgs(Keys.F10));
				return true;
			}
			return Editor.IsIngame || base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 74 && Engine.Initialized)
			{
				string fileName = Marshal.PtrToStringUni(((Win32.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Win32.COPYDATASTRUCT))).lpData);
				this.LoadMap(fileName, null);
			}
			base.WndProc(ref m);
		}
		private void timerUIUpdate_Tick(object sender, EventArgs e)
		{
			if (!Engine.Initialized)
			{
				return;
			}
			bool flag = UndoManager.UndoCount > 0;
			bool flag2 = UndoManager.RedoCount > 0;
			if (this.menuItem_Undo.Enabled != flag)
			{
				this.menuItem_Undo.Enabled = flag;
			}
			if (this.menuItem_Redo.Enabled != flag2)
			{
				this.menuItem_Redo.Enabled = flag2;
			}
		}
		private void InitCameraSpeed()
		{
			Camera.Speed = 16f;
			float num = 2f;
			int i = 0;
			ToolStripItem toolStripItem;
			while (i < 6)
			{
				toolStripItem = this.cameraSpeedStrip.Items.Add(num.ToString());
				toolStripItem.Tag = num;
				toolStripItem.Click += new EventHandler(this.cameraSpeedItem_Click);
				i++;
				num *= 2f;
			}
			toolStripItem = this.cameraSpeedStrip.Items.Add(Localizer.Localize("EDITOR_CAMERA_SPEED_CUSTOM"));
			toolStripItem.Click += new EventHandler(this.cameraSpeedItem_Click);
			this.UpdateCameraSpeed();
		}
		private void UpdateCameraSpeed()
		{
			this.statusBarCameraSpeed.Text = Camera.Speed.ToString();
		}
		private void cameraSpeedItem_Click(object sender, EventArgs e)
		{
			object tag = ((ToolStripItem)sender).Tag;
			float speed;
			if (tag != null)
			{
				speed = (float)tag;
			}
			else
			{
				using (PromptForm promptForm = new PromptForm(Localizer.Localize("EDITOR_CAMERA_SPEED_PROMPT"), Localizer.Localize("EDITOR_CAMERA_SPEED_PROMPT_TITLE")))
				{
					promptForm.Validation = PromptForm.GetFloatValidation(0.001f, 50f);
					if (promptForm.ShowDialog(this) != DialogResult.OK)
					{
						return;
					}
					speed = float.Parse(promptForm.Input);
				}
			}
			Camera.Speed = speed;
			this.UpdateCameraSpeed();
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			return false;
		}
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			if (keyEvent == Editor.KeyEvent.KeyUp)
			{
				Keys keyCode = keyEventArgs.KeyCode;
				if (keyCode == Keys.Escape)
				{
					this.CurrentTool = null;
					return true;
				}
				ButtonItem buttonItem = null;
				if (this.m_toolShortcuts.TryGetValue(keyEventArgs.KeyCode, out buttonItem))
				{
					this.CurrentTool = (ITool)buttonItem.Tag;
					return true;
				}
			}
			return false;
		}
		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			if (StringHash.MakeStringID("CFCXEditorEventThumbnailsFinished") == eventType)
			{
				this.menuItem_PrepareThumbnails_Activate(null, null);
			}
		}
		private void UpdateStatusBar(MainForm.StatusBarMode mode)
		{
			if (mode == MainForm.StatusBarMode.None)
			{
				if (this.m_statusBarMode != MainForm.StatusBarMode.None)
				{
					this.statusCaption.Image = null;
					this.statusCaption.Text = Localizer.Localize("EDITOR_STATUS_READY");
					this.statusCaption.BackColor = SystemColors.Control;
					this.statusCaption.ForeColor = SystemColors.ControlText;
				}
			}
			else
			{
				if (mode == MainForm.StatusBarMode.Loading)
				{
					if (this.m_statusBarMode != MainForm.StatusBarMode.Loading)
					{
						this.statusCaption.Image = Resources.hourglass;
						this.statusCaption.Text = Localizer.Localize("EDITOR_STATUS_LOADING");
						this.statusCaption.BackColor = Color.LightCoral;
						this.statusCaption.ForeColor = Color.Black;
					}
				}
				else
				{
					if (mode == MainForm.StatusBarMode.Navmesh)
					{
						if (this.m_statusBarMode != MainForm.StatusBarMode.Navmesh)
						{
							this.statusCaption.Image = Resources.hourglass;
						}
						this.statusCaption.Text = string.Format("Generating {0} navmesh tiles...", Navmesh.PendingTilesCount);
						this.statusCaption.BackColor = Color.LightCoral;
						this.statusCaption.ForeColor = Color.Black;
					}
					else
					{
						if (mode == MainForm.StatusBarMode.Ingame && this.m_statusBarMode != MainForm.StatusBarMode.Ingame)
						{
							this.statusCaption.Image = null;
							this.statusCaption.Text = Localizer.Localize("EDITOR_STATUS_INGAME");
							this.statusCaption.BackColor = Color.FromArgb(32, 32, 32);
							this.statusCaption.ForeColor = Color.GhostWhite;
						}
					}
				}
			}
			this.m_statusBarMode = mode;
		}
		public void Update(float dt)
		{
			this.m_lastUpdate += dt;
			if (this.m_lastUpdate >= 0.25f)
			{
				this.m_lastUpdate = 0f;
				MainForm.StatusBarMode mode = MainForm.StatusBarMode.None;
				if (Editor.IsIngame)
				{
					mode = MainForm.StatusBarMode.Ingame;
				}
				else
				{
					if (Navmesh.PendingTilesCount > 0)
					{
						mode = MainForm.StatusBarMode.Navmesh;
					}
					else
					{
						if (Editor.IsLoadPending)
						{
							mode = MainForm.StatusBarMode.Loading;
						}
					}
				}
				this.UpdateStatusBar(mode);
				float num = (float)BudgetManager.MemoryUsage / 1048576f;
				if (Math.Abs(this.m_currentMemoryUsage - num) >= 0.1f)
				{
					this.m_currentMemoryUsage = num;
					string text = num.ToString("F1");
					this.statusBarMemoryUsage.Text = text;
				}
				float objectUsage = BudgetManager.ObjectUsage;
				if (Math.Abs(this.m_currentObjectUsage - objectUsage) >= 0.1f)
				{
					this.m_currentObjectUsage = objectUsage;
					string text2 = objectUsage.ToString("F0");
					this.statusBarObjectUsage.Text = text2;
				}
				float num2 = 1f / Editor.FrameTime;
				if (num2 != this.m_currentFps)
				{
					this.m_currentFps = num2;
					string text3 = num2.ToString("F1") + " fps";
					this.statusBarFpsItem.Text = text3;
				}
				Vec3 position = Camera.Position;
				if (position != this.m_currentCameraPos)
				{
					this.m_currentCameraPos = position;
					this.statusBarCameraPos.Text = position.ToString("F2");
				}
				Vec3 vec;
				bool flag = this.m_cursorPhysics ? Editor.RayCastPhysicsFromMouse(out vec) : Editor.RayCastTerrainFromMouse(out vec);
				if (this.m_currentCursorValid != flag || vec != this.m_currentCursorPos)
				{
					this.m_currentCursorPos = vec;
					this.m_currentCursorValid = flag;
					if (flag)
					{
						this.statusBarCursorPos.Text = vec.ToString("F2");
					}
					else
					{
						this.statusBarCursorPos.Text = Localizer.Localize("PARAM_NA");
					}
				}
			}
			ObjectRenderer.Update(dt);
		}
		private void CreateWatermark()
		{
			this.m_watermark = new Watermark();
			this.m_watermark.Show(this);
			this.UpdateWatermark();
		}
		private void UpdateWatermark()
		{
			if (this.m_watermark == null)
			{
				return;
			}
			this.m_watermark.Location = new Point(base.Location.X + base.Width - this.m_watermark.Width, base.Location.Y + 50);
		}
	}
}
