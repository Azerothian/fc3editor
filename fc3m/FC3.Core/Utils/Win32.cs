using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Nomad.Utils
{
	public  class Win32
	{
		public struct Point
		{
			public int x;
			public int y;
			public Point(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		public struct Size
		{
			public int cx;
			public int cy;
			public Size(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}
		}
		public struct Rect
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
			public Rect(int left, int top, int width, int height)
			{
				this.left = left;
				this.top = top;
				this.right = left + width;
				this.bottom = top + height;
			}
		}
		public struct Message
		{
			public IntPtr hWnd;
			public int message;
			public IntPtr wParam;
			public IntPtr lParam;
			public int time;
			public Win32.Point pt;
		}
		public struct COPYDATASTRUCT
		{
			public IntPtr dwData;
			public int cbData;
			public IntPtr lpData;
		}
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public int code;
		}
		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
		[StructLayout(LayoutKind.Sequential)]
		public class ScrollInfo
		{
			public int cbSize = Marshal.SizeOf(typeof(Win32.ScrollInfo));
			public int fMask;
			public int nMin;
			public int nMax;
			public int nPage;
			public int nPos;
			public int nTrackPos;
		}
		[StructLayout(LayoutKind.Sequential)]
		public class DrawTextParams
		{
			public int cbSize = Marshal.SizeOf(typeof(Win32.DrawTextParams));
			public int iTabLength;
			public int iLeftMargin;
			public int iRightMargin;
			public uint uiLengthDrawn;
		}
		public struct ABC
		{
			public int A;
			public int B;
			public int C;
		}
		public struct TextMetric
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}
		public struct BlendFunction
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}
		public enum EXTENDED_NAME_FORMAT
		{
			NameUnknown,
			NameFullyQualifiedDN,
			NameSamCompatible,
			NameDisplay,
			NameUniqueId = 6,
			NameCanonical,
			NameUserPrincipal,
			NameCanonicalEx,
			NameServicePrincipal,
			NameDnsDomain = 12
		}
		public const int CS_DROPSHADOW = 131072;
		public const int WM_SETREDRAW = 11;
		public const int WM_ERASEBKGND = 20;
		public const int WM_COPYDATA = 74;
		public const int WM_NOTIFY = 78;
		public const int WM_NCACTIVATE = 134;
		public const int WM_GETDLGCODE = 135;
		public const int WM_NCMOUSEMOVE = 160;
		public const int WM_NCLBUTTONDOWN = 161;
		public const int WM_NCLBUTTONUP = 162;
		public const int WM_NCLBUTTONDBLCLK = 163;
		public const int WM_NCRBUTTONDOWN = 164;
		public const int WM_NCRBUTTONUP = 165;
		public const int WM_NCRBUTTONDBLCLK = 166;
		public const int WM_NCMBUTTONDOWN = 167;
		public const int WM_NCMBUTTONUP = 168;
		public const int WM_NCMBUTTONDBLCLK = 169;
		public const int WM_NCXBUTTONDOWN = 171;
		public const int WM_NCXBUTTONUP = 172;
		public const int WM_NCXBUTTONDBLCLK = 173;
		public const int WM_KEYDOWN = 256;
		public const int WM_KEYUP = 257;
		public const int WM_CHAR = 258;
		public const int WM_DEADCHAR = 259;
		public const int WM_SYSKEYDOWN = 260;
		public const int WM_SYSKEYUP = 261;
		public const int WM_SYSCHAR = 262;
		public const int WM_SYSDEADCHAR = 263;
		public const int WM_COMMAND = 273;
		public const int WM_HSCROLL = 276;
		public const int WM_VSCROLL = 277;
		public const int WM_MOUSEMOVE = 512;
		public const int WM_LBUTTONDOWN = 513;
		public const int WM_LBUTTONUP = 514;
		public const int WM_LBUTTONDBLCLK = 515;
		public const int WM_RBUTTONDOWN = 516;
		public const int WM_RBUTTONUP = 517;
		public const int WM_RBUTTONDBLCLK = 518;
		public const int WM_MBUTTONDOWN = 519;
		public const int WM_MBUTTONUP = 520;
		public const int WM_MBUTTONDBLCLK = 521;
		public const int WM_MOUSEWHEEL = 522;
		public const int WM_XBUTTONDOWN = 523;
		public const int WM_XBUTTONUP = 524;
		public const int WM_XBUTTONDBLCLK = 525;
		public const int WM_USER = 1024;
		public const int WF_REFLECT = 8192;
		public const int WM_REFLECT_NOTIFY = 8270;
		public const int WS_HSCROLL = 1048576;
		public const int WS_VSCROLL = 2097152;
		public const int WS_POPUP = -2147483648;
		public const int WS_EX_TOPMOST = 8;
		public const int WS_EX_TRANSPARENT = 32;
		public const int WS_EX_CLIENTEDGE = 512;
		public const int WS_EX_LAYERED = 524288;
		public const int WS_EX_NOACTIVATE = 134217728;
		public const int DLGC_WANTALLKEYS = 4;
		public const int TBS_ENABLESELRANGE = 32;
		public const int TBM_SETSEL = 1034;
		public const int TBM_SETSELSTART = 1035;
		public const int TBM_SETSELEND = 1036;
		public const int TBM_GETCHANNELRECT = 1050;
		public const int LVS_EX_BORDERSELECT = 32768;
		public const int LVM_FIRST = 4096;
		public const int LVM_GETITEMSPACING = 4147;
		public const int LVM_SETICONSPACING = 4149;
		public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;
		public const int LVN_FIRST = -100;
		public const int LVN_BEGINSCROLL = -180;
		public const int LVN_ENDSCROLL = -181;
		public const int TV_FIRST = 4352;
		public const int TVM_SETINSERTMARK = 4378;
		public const int SW_SHOWNA = 8;
		public const int VK_CTRL = 17;
		public const int VK_LEFT = 37;
		public const int VK_UP = 38;
		public const int VK_RIGHT = 39;
		public const int VK_DOWN = 40;
		public const int VK_LSHIFT = 160;
		public const int VK_RSHIFT = 161;
		public const int CBN_DROPDOWN = 7;
		public const int CBN_CLOSEUP = 8;
		public const int RDW_INVALIDATE = 1;
		public const int RDW_FRAME = 1024;
		public const int GWL_STYLE = -16;
		public const int GWL_EXSTYLE = -20;
		public const int SWP_NOSIZE = 1;
		public const int SWP_NOMOVE = 2;
		public const int SWP_NOZORDER = 4;
		public const int SWP_FRAMECHANGED = 32;
		public const int SB_HORZ = 0;
		public const int SB_VERT = 1;
		public const int SB_CTL = 2;
		public const int SB_LINEUP = 0;
		public const int SB_LINEDOWN = 1;
		public const int SB_PAGEUP = 2;
		public const int SB_PAGEDOWN = 3;
		public const int SB_THUMBPOSITION = 4;
		public const int SB_THUMBTRACK = 5;
		public const int SB_TOP = 6;
		public const int SB_BOTTOM = 7;
		public const int SB_ENDSCROLL = 8;
		public const int SIF_RANGE = 1;
		public const int SIF_PAGE = 2;
		public const int SIF_POS = 4;
		public const int SIF_DISABLENOSCROLL = 8;
		public const int SIF_TRACKPOS = 16;
		public const int SIF_ALL = 31;
		public const int SW_INVALIDATE = 2;
		public const int DT_TOP = 0;
		public const int DT_LEFT = 0;
		public const int DT_CENTER = 1;
		public const int DT_RIGHT = 2;
		public const int DT_VCENTER = 4;
		public const int DT_BOTTOM = 8;
		public const int DT_WORDBREAK = 16;
		public const int DT_SINGLELINE = 32;
		public const int DT_EXPANDTABS = 64;
		public const int DT_TABSTOP = 128;
		public const int DT_NOCLIP = 256;
		public const int DT_EXTERNALLEADING = 512;
		public const int DT_CALCRECT = 1024;
		public const int DT_NOPREFIX = 2048;
		public const int DT_INTERNAL = 4096;
		public const int PS_SOLID = 0;
		public const int PS_DASH = 1;
		public const int PS_DOT = 2;
		public const int PS_DASHDOT = 3;
		public const int PS_DASHDOTDOT = 4;
		public const int PS_NULL = 5;
		public const int PS_INSIDEFRAME = 6;
		public const byte AC_SRC_OVER = 0;
		public const byte AC_SRC_ALPHA = 1;
		public const int ULW_COLORKEY = 1;
		public const int ULW_ALPHA = 2;
		public const int ULW_OPAQUE = 4;
		[DllImport("kernel32.dll")]
		public static extern void RtlMoveMemory(IntPtr dest, IntPtr src, int size);
		[DllImport("kernel32.dll")]
		public static extern void GetPrivateProfileStringW([MarshalAs(UnmanagedType.LPWStr)] string lpAppName, [MarshalAs(UnmanagedType.LPWStr)] string lpKeyName, [MarshalAs(UnmanagedType.LPWStr)] string lpDefault, IntPtr lpReturnedString, int nSize, [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
		public static void GetPrivateProfileStringW(string lpAppName, string lpKeyName, string lpDefault, out string lpReturnedString, string lpFileName)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(514);
			Win32.GetPrivateProfileStringW(lpAppName, lpKeyName, lpDefault, intPtr, 256, lpFileName);
			lpReturnedString = Marshal.PtrToStringUni(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}
		public static int LoWord(int dw)
		{
			return dw & 65535;
		}
		public static int HiWord(int dw)
		{
			return dw >> 16;
		}
		public static int MakeLong(int lw, int hw)
		{
			return (lw & 65535) | (hw & 65535) << 16;
		}
		[DllImport("user32.dll")]
		public static extern bool GetMessage(out Win32.Message msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref Win32.Rect lParam);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref Win32.COPYDATASTRUCT lParam);
		[DllImport("user32.dll")]
		public static extern bool PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool TranslateMessage(ref Win32.Message msg);
		[DllImport("user32.dll")]
		public static extern bool DispatchMessage(ref Win32.Message msg);
		[DllImport("user32.dll")]
		public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref Win32.Point pt, int cPoints);
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll")]
		public static extern IntPtr GetActiveWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetParent(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern bool IsWindowEnabled(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags);
		[DllImport("user32.dll")]
		public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
		[DllImport("user32.dll")]
		public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern int EnumWindows(Win32.EnumWindowsProc ewp, IntPtr lParam);
		[DllImport("user32.dll")]
		public static extern IntPtr GetProp(IntPtr hWnd, string lpString);
		[DllImport("user32.dll")]
		public static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);
		[DllImport("user32.dll")]
		public static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);
		[DllImport("user32.dll")]
		public static extern IntPtr GetCapture();
		[DllImport("user32.dll")]
		public static extern void SetCapture(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern void ReleaseCapture();
		public static void SetRedraw(Control control, bool redraw)
		{
			Win32.SendMessage(control.Handle, 11, redraw ? 1 : 0, 0);
		}
		[DllImport("user32.dll")]
		public static extern ushort GetKeyState(int nVirtKey);
		public static bool IsKeyDown(int nVirtKey)
		{
			return (Win32.GetKeyState(nVirtKey) & 32768) != 0;
		}
		[DllImport("user32.dll")]
		public static extern IntPtr GetKeyboardLayout(int idThread);
		[DllImport("user32.dll")]
		public static extern int MapVirtualKeyEx(int uCode, int uMapType, IntPtr dwhkl);
		[DllImport("user32.dll")]
		public static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
		[DllImport("user32.dll")]
		public static extern bool DestroyCaret();
		[DllImport("user32.dll")]
		public static extern bool ShowCaret(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern bool HideCaret(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern void GetCaretPos(out Win32.Point pt);
		[DllImport("user32.dll")]
		public static extern bool SetCaretPos(int x, int y);
		[DllImport("user32.dll")]
		public static extern int GetScrollInfo(IntPtr hWnd, int nBar, [In] Win32.ScrollInfo scrollInfo);
		[DllImport("user32.dll")]
		public static extern int SetScrollInfo(IntPtr hWnd, int nBar, [In] Win32.ScrollInfo scrollInfo, bool bRedraw);
		[DllImport("user32.dll")]
		public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
		[DllImport("user32.dll")]
		public static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, ref Win32.Rect prcScroll, ref Win32.Rect prcClip, IntPtr hrgnUpdate, IntPtr prcUpdate, int flags);
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DrawTextExW")]
		public static extern int DrawTextEx(IntPtr hdc, string lpchText, int cchText, ref Win32.Rect lprc, uint dwDTFormat, [In] [Out] Win32.DrawTextParams lpDTParams);
		[DllImport("user32.dll")]
		public static extern int FillRect(IntPtr hDC, ref Win32.Rect lprc, IntPtr hbr);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hdc);
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(int crColor);
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
		[DllImport("gdi32.dll")]
		public static extern int SetTextColor(IntPtr hdc, int crColor);
		[DllImport("gdi32.dll")]
		public static extern int SetBkColor(IntPtr hdc, int crColor);
		[DllImport("gdi32.dll")]
		public static extern bool GetCharABCWidths(IntPtr hdc, uint uFirstChar, uint uLastChar, [Out] Win32.ABC[] lpabc);
		[DllImport("gdi32.dll")]
		public static extern bool GetTextExtentExPoint(IntPtr hdc, string lpszStr, int cchString, int nMaxExtent, out int lpnFit, IntPtr alpDx, out Win32.Size lpSize);
		[DllImport("gdi32.dll")]
		public static extern bool GetTextMetrics(IntPtr hdc, out Win32.TextMetric lptm);
		[DllImport("user32.dll")]
		public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Win32.Point pptDst, ref Win32.Size psize, IntPtr hdcSrc, ref Win32.Point pptSrc, int crKey, ref Win32.BlendFunction pblend, int dwFlags);
		public static void UpdateLayeredWindowHelper(Control control, Bitmap bmp)
		{
			IntPtr dC = Win32.GetDC(IntPtr.Zero);
			Win32.Point point = new Win32.Point(control.Left, control.Top);
			Win32.Size size = new Win32.Size(bmp.Width, bmp.Height);
			IntPtr intPtr = Win32.CreateCompatibleDC(IntPtr.Zero);
			IntPtr hbitmap = bmp.GetHbitmap(Color.Black);
			Win32.SelectObject(intPtr, hbitmap);
			Win32.Point point2 = new Win32.Point(0, 0);
			Win32.BlendFunction blendFunction = default(Win32.BlendFunction);
			blendFunction.BlendOp = 0;
			blendFunction.BlendFlags = 0;
			blendFunction.SourceConstantAlpha = 255;
			blendFunction.AlphaFormat = 1;
			Win32.UpdateLayeredWindow(control.Handle, dC, ref point, ref size, intPtr, ref point2, 0, ref blendFunction, 2);
			Win32.DeleteObject(hbitmap);
			Win32.DeleteDC(intPtr);
			Win32.ReleaseDC(IntPtr.Zero, dC);
		}
		public static string GetUserNameEx(Win32.EXTENDED_NAME_FORMAT NameFormat)
		{
			string result = null;
			IntPtr intPtr = Marshal.AllocHGlobal(512);
			uint num = 256u;
			bool flag = Win32.GetUserNameExW(NameFormat, intPtr, ref num) != 0;
			if (flag)
			{
				result = Marshal.PtrToStringUni(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}
		[DllImport("secur32.dll")]
		public static extern int GetUserNameExW(Win32.EXTENDED_NAME_FORMAT NameFormat, IntPtr lpNameBuffer, ref uint nSize);
	}
}
