// Decompiled with JetBrains decompiler
// Type: Fluent.NativeMethods
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Fluent
{
  internal static class NativeMethods
  {
    public const int CC_ANYCOLOR = 256;
    public const int CC_ENABLEHOOK = 16;
    public const int CC_ENABLETEMPLATE = 32;
    public const int CC_ENABLETEMPLATEHANDLE = 64;
    public const int CC_FULLOPEN = 2;
    public const int CC_PREVENTFULLOPEN = 4;
    public const int CC_RGBINIT = 1;
    public const int CC_SHOWHELP = 8;
    public const int CC_SOLIDCOLOR = 8;
    public const int WM_SYSCOMMAND = 274;
    public const int WM_GETSYSMENU = 787;
    public const int WM_DWMCOMPOSITIONCHANGED = 798;
    public const int WM_NCRBUTTONUP = 165;
    public const int WM_NCACTIVATE = 134;
    public const int WM_PAINT = 15;
    public const int WM_ERASEBKGND = 20;
    public const int WM_SETREDRAW = 11;
    public const int WM_CREATE = 1;
    public const int WM_SETTEXT = 12;
    public const int WM_SETICON = 128;
    public const int WM_WINDOWPOSCHANGED = 71;
    public const int WM_SETTINGCHANGE = 26;
    public const int WM_ENTERSIZEMOVE = 561;
    public const int WM_EXITSIZEMOVE = 562;
    public const int WVR_HREDRAW = 256;
    public const int WVR_VREDRAW = 512;
    public const int WVR_REDRAW = 768;
    public const uint MF_ENABLED = 0;
    public const uint MF_BYCOMMAND = 0;
    public const uint MF_GRAYED = 1;
    public const uint MF_DISABLED = 2;
    public const int RGN_AND = 1;
    public const int RGN_OR = 2;
    public const int RGN_XOR = 3;
    public const int RGN_DIFF = 4;
    public const int RGN_COPY = 5;
    public const int SW_HIDE = 0;
    public const int SW_SHOWNORMAL = 1;
    public const int SW_NORMAL = 1;
    public const int SW_SHOWMINIMIZED = 2;
    public const int SW_SHOWMAXIMIZED = 3;
    public const int SW_MAXIMIZE = 3;
    public const int SW_SHOWNOACTIVATE = 4;
    public const int SW_SHOW = 5;
    public const int SW_MINIMIZE = 6;
    public const int SW_SHOWMINNOACTIVE = 7;
    public const int SW_SHOWNA = 8;
    public const int SW_RESTORE = 9;
    public const int SW_SHOWDEFAULT = 10;
    public const int SW_FORCEMINIMIZE = 11;
    public const int SC_SIZE = 61440;
    public const int SC_MOVE = 61456;
    public const int SC_MINIMIZE = 61472;
    public const int SC_MAXIMIZE = 61488;
    public const int SC_NEXTWINDOW = 61504;
    public const int SC_PREVWINDOW = 61520;
    public const int SC_CLOSE = 61536;
    public const int SC_VSCROLL = 61552;
    public const int SC_HSCROLL = 61568;
    public const int SC_MOUSEMENU = 61584;
    public const int SC_KEYMENU = 61696;
    public const int SC_ARRANGE = 61712;
    public const int SC_RESTORE = 61728;
    public const int SC_TASKLIST = 61744;
    public const int SC_SCREENSAVE = 61760;
    public const int SC_HOTKEY = 61776;
    public const int SC_DEFAULT = 61792;
    public const int SC_MONITORPOWER = 61808;
    public const int SC_CONTEXTHELP = 61824;
    public const int HTNOWHERE = 0;
    public const int HTCLIENT = 1;
    public const int HTCAPTION = 2;
    public const int HTSYSMENU = 3;
    public const int HTLEFT = 10;
    public const int HTRIGHT = 11;
    public const int HTTOP = 12;
    public const int HTTOPLEFT = 13;
    public const int HTTOPRIGHT = 14;
    public const int HTBOTTOM = 15;
    public const int HTBOTTOMLEFT = 16;
    public const int HTBOTTOMRIGHT = 17;
    public const int WM_NCHITTEST = 132;
    public const int WM_NCPAINT = 133;
    public const int WM_NCCREATE = 129;
    public const int WM_NCDESTROY = 130;
    public const int WM_NCCALCSIZE = 131;
    public const int WM_GETMINMAXINFO = 36;
    public const int WM_SIZE = 5;
    public const int WM_MOVE = 3;
    public const int SIZE_RESTORED = 0;
    public const int SIZE_MINIMIZED = 1;
    public const int SIZE_MAXIMIZED = 2;
    public const int WS_POPUP = -2147483648;
    public const int WS_VISIBLE = 268435456;
    public const int WS_CLIPSIBLINGS = 67108864;
    public const int WS_CLIPCHILDREN = 33554432;
    public const int WS_CAPTION = 12582912;
    public const int WS_THICKFRAME = 262144;
    public const int WS_BORDER = 8388608;
    public const int WS_SYSMENU = 524288;
    public const long WS_MINIMIZEBOX = 131072;
    public const long WS_MAXIMIZEBOX = 65536;
    public const long WS_OVERLAPPED = 0;
    public const long WS_MAXIMIZE = 16777216;
    public const long WS_MINIMIZE = 536870912;
    public const int WS_EX_LEFT = 0;
    public const int WS_EX_LTRREADING = 0;
    public const int WS_EX_RIGHTSCROLLBAR = 0;
    public const int WS_EX_WINDOWEDGE = 256;
    public const int WS_EX_APPWINDOW = 262144;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int GWL_HWNDPARENT = -8;
    public const int HWND_TOP = 0;
    public const int HWND_NOTOPMOST = -2;
    public const int SWP_NOSIZE = 1;
    public const int SWP_NOMOVE = 2;
    public const int SWP_NOACTIVATE = 16;
    public const int SWP_ASYNCWINDOWPOS = 16384;
    public const int SWP_DEFERERASE = 8192;
    public const int SWP_DRAWFRAME = 32;
    public const int SWP_FRAMECHANGED = 32;
    public const int SWP_HIDEWINDOW = 128;
    public const int SWP_NOCOPYBITS = 256;
    public const int SWP_NOOWNERZORDER = 512;
    public const int SWP_NOREDRAW = 8;
    public const int SWP_NOREPOSITION = 512;
    public const int SWP_NOSENDCHANGING = 1024;
    public const int SWP_NOZORDER = 4;
    public const int SWP_SHOWWINDOW = 64;
    private static bool idDwmDllNotFound;

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(
      IntPtr hhk,
      int nCode,
      IntPtr wParam,
      IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(
      NativeMethods.HookType hookType,
      NativeMethods.HookProc lpfn,
      IntPtr hMod,
      int dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(
      IntPtr hwnd,
      NativeMethods.DWMWINDOWATTRIBUTE dwAttribute,
      IntPtr pvAttribute,
      int cbAttribute);

    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(
      IntPtr hwnd,
      NativeMethods.DWMWINDOWATTRIBUTE dwAttribute,
      ref NativeMethods.Rect pvAttribute,
      int cbAttribute);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll", EntryPoint = "DefWindowProcW", CharSet = CharSet.Unicode)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromRect([In] ref NativeMethods.Rect lprc, uint dwFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern IntPtr CreateRectRgnIndirect([In] ref NativeMethods.Rect lprc);

    [DllImport("gdi32.dll")]
    public static extern int CombineRgn(
      IntPtr hrgnDest,
      IntPtr hrgnSrc1,
      IntPtr hrgnSrc2,
      int fnCombineMode);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] NativeMethods.MonitorInfo lpmi);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [SuppressMessage("Microsoft.Portability", "CA1901")]
    [SuppressMessage("Microsoft.Performance", "CA1811")]
    [DllImport("user32.dll")]
    public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

    [SuppressMessage("Microsoft.Performance", "CA1811")]
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("dwmapi.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DwmDefWindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref IntPtr plResult);

    [DllImport("dwmapi.dll")]
    private static extern IntPtr DwmGetCompositionTimingInfo(
      IntPtr hwnd,
      ref NativeMethods.DWM_TIMING_INFO pTimingInfo);

    public static NativeMethods.DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
    {
      if (Environment.OSVersion.Version < new Version("6.0"))
        return new NativeMethods.DWM_TIMING_INFO?();
      NativeMethods.DWM_TIMING_INFO pTimingInfo = new NativeMethods.DWM_TIMING_INFO()
      {
        cbSize = Marshal.SizeOf(typeof (NativeMethods.DWM_TIMING_INFO))
      };
      NativeMethods.DwmGetCompositionTimingInfo(hwnd, ref pTimingInfo);
      return new NativeMethods.DWM_TIMING_INFO?(pTimingInfo);
    }

    [DllImport("dwmapi.dll", PreserveSig = false)]
    public static extern void DwmExtendFrameIntoClientArea(
      IntPtr hWnd,
      NativeMethods.MARGINS pMargins);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DwmIsCompositionEnabled();

    public static bool IsDwmEnabled()
    {
      if (NativeMethods.idDwmDllNotFound)
        return false;
      try
      {
        return NativeMethods.DwmIsCompositionEnabled();
      }
      catch (DllNotFoundException ex)
      {
        NativeMethods.idDwmDllNotFound = true;
        return false;
      }
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern int EnableMenuItem(IntPtr hMenu, int uIDEnableItem, uint uEnable);

    [DllImport("User32.dll")]
    public static extern uint TrackPopupMenuEx(
      IntPtr hmenu,
      uint fuFlags,
      int x,
      int y,
      IntPtr hwnd,
      IntPtr lptpm);

    [DllImport("User32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

    [DllImport("User32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(IntPtr hwnd, NativeMethods.WINDOWPLACEMENT lpwndpl);

    public static int LowWord(IntPtr value)
    {
      return (int) (short) (value.ToInt64() & (long) ushort.MaxValue);
    }

    public static int HiWord(IntPtr value)
    {
      return (int) (short) (value.ToInt64() >> 16 & (long) ushort.MaxValue);
    }

    [SuppressMessage("Microsoft.Performance", "CA1811")]
    public static IntPtr MakeDWord(int lo, int hi)
    {
      return (IntPtr) ((int) (short) hi << 16 | lo & (int) ushort.MaxValue);
    }

    [DllImport("User32.dll")]
    public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

    [DllImport("Gdi32.dll")]
    public static extern IntPtr CreateRoundRectRgn(
      int nLeftRect,
      int nTopRect,
      int nRightRect,
      int nBottomRect,
      int nWidthEllipse,
      int nHeightEllipse);

    [DllImport("Gdi32.dll")]
    public static extern IntPtr CreateRectRgn(
      int nLeftRect,
      int nTopRect,
      int nRightRect,
      int nBottomRect);

    [DllImport("Gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject(IntPtr hObject);

    public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
      return new IntPtr(IntPtr.Size == 8 ? NativeMethods.GetWindowLongPtr64(hWnd, nIndex) : (long) NativeMethods.GetWindowLongPtr32(hWnd, nIndex));
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [SuppressMessage("Microsoft.Interoperability", "CA1400")]
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern long GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
      return IntPtr.Size == 8 ? NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : new IntPtr(NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [SuppressMessage("Microsoft.Interoperability", "CA1400")]
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int X,
      int Y,
      int cx,
      int cy,
      int uFlags);

    [SuppressMessage("Microsoft.Performance", "CA1811")]
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(
      IntPtr hWndParent,
      NativeMethods.EnumChildProc lpEnumFunc,
      IntPtr lParam);

    [SuppressMessage("Microsoft.Performance", "CA1811")]
    [DllImport("user32.dll", PreserveSig = false)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowInfo(IntPtr hwnd, ref NativeMethods.WINDOWINFO pwi);

    public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
    {
      return IntPtr.Size > 4 ? NativeMethods.GetClassLongPtr64(hWnd, nIndex) : new IntPtr((long) NativeMethods.GetClassLongPtr32(hWnd, nIndex));
    }

    [DllImport("user32.dll", EntryPoint = "GetClassLong")]
    private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

    [SuppressMessage("Microsoft.Interoperability", "CA1400")]
    [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
    private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr LoadImage(
      IntPtr hinst,
      IntPtr lpszName,
      uint uType,
      int cxDesired,
      int cyDesired,
      uint fuLoad);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref NativeMethods.Rect lpRect);

    [SuppressMessage("Microsoft.Performance", "CA1811")]
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(IntPtr hWnd, ref NativeMethods.Rect lpRect);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AdjustWindowRectEx(
      ref NativeMethods.Rect lpRect,
      int dwStyle,
      [MarshalAs(UnmanagedType.Bool)] bool bMenu,
      int dwExStyle);

    [DllImport("comdlg32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ChooseColor(NativeMethods.CHOOSECOLOR lpcc);

    [DllImport("user32.dll")]
    public static extern int ToUnicodeEx(
      uint wVirtKey,
      uint wScanCode,
      byte[] lpKeyState,
      [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pwszBuff,
      int cchBuff,
      uint wFlags,
      IntPtr dwhkl);

    [SuppressMessage("Microsoft.Portability", "CA1901")]
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

    [SuppressMessage("Microsoft.Globalization", "CA2101")]
    [DllImport("user32.dll")]
    public static extern IntPtr LoadKeyboardLayout(string cultureId, uint flags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnloadKeyboardLayout(IntPtr hkl);

    [DllImport("user32.dll")]
    public static extern int GetKeyboardLayoutList(int nBuff, [MarshalAs(UnmanagedType.LPArray), Out] IntPtr[] lpList);

    public enum HookType
    {
      WH_JOURNALRECORD,
      WH_JOURNALPLAYBACK,
      WH_KEYBOARD,
      WH_GETMESSAGE,
      WH_CALLWNDPROC,
      WH_CBT,
      WH_SYSMSGFILTER,
      WH_MOUSE,
      WH_HARDWARE,
      WH_DEBUG,
      WH_SHELL,
      WH_FOREGROUNDIDLE,
      WH_CALLWNDPROCRET,
      WH_KEYBOARD_LL,
      WH_MOUSE_LL,
    }

    public struct MOUSEHOOKSTRUCT
    {
      public NativeMethods.POINT pt;
      public IntPtr hwnd;
      public uint wHitTestCode;
      public IntPtr dwExtraInfo;
    }

    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

    public enum DWMWINDOWATTRIBUTE
    {
      DWMWA_NCRENDERING_ENABLED = 1,
      DWMWA_NCRENDERING_POLICY = 2,
      DWMWA_TRANSITIONS_FORCEDISABLED = 3,
      DWMWA_ALLOW_NCPAINT = 4,
      DWMWA_CAPTION_BUTTON_BOUNDS = 5,
      DWMWA_NONCLIENT_RTL_LAYOUT = 6,
      DWMWA_FORCE_ICONIC_REPRESENTATION = 7,
      DWMWA_FLIP3D_POLICY = 8,
      DWMWA_EXTENDED_FRAME_BOUNDS = 9,
      DWMWA_HAS_ICONIC_BITMAP = 10, // 0x0000000A
      DWMWA_DISALLOW_PEEK = 11, // 0x0000000B
      DWMWA_EXCLUDED_FROM_PEEK = 12, // 0x0000000C
      DWMWA_LAST = 13, // 0x0000000D
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CHOOSECOLOR
    {
      public int lStructSize = Marshal.SizeOf(typeof (NativeMethods.CHOOSECOLOR));
      public IntPtr hwndOwner;
      public IntPtr hInstance = IntPtr.Zero;
      public int rgbResult;
      public IntPtr lpCustColors = IntPtr.Zero;
      public int Flags;
      public IntPtr lCustData = IntPtr.Zero;
      public IntPtr lpfnHook = IntPtr.Zero;
      public IntPtr lpTemplateName = IntPtr.Zero;
    }

    public struct NCCALCSIZE_PARAMS
    {
      public NativeMethods.Rect rect0;
      public NativeMethods.Rect rect1;
      public NativeMethods.Rect rect2;
      public IntPtr lppos;
    }

    public struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public int flags;
    }

    public struct Rect
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MonitorInfo
    {
      public int Size = Marshal.SizeOf(typeof (NativeMethods.MonitorInfo));
      public NativeMethods.Rect Monitor;
      public NativeMethods.Rect Work;
      public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MARGINS
    {
      public int cxLeftWidth;
      public int cxRightWidth;
      public int cyTopHeight;
      public int cyBottomHeight;

      public MARGINS(int left, int top, int right, int bottom)
      {
        this.cxLeftWidth = left;
        this.cyTopHeight = top;
        this.cxRightWidth = right;
        this.cyBottomHeight = bottom;
      }
    }

    public struct POINT
    {
      public int x;
      public int y;
    }

    public struct MINMAXINFO
    {
      public NativeMethods.POINT ptReserved;
      public NativeMethods.POINT ptMaxSize;
      public NativeMethods.POINT ptMaxPosition;
      public NativeMethods.POINT ptMinTrackSize;
      public NativeMethods.POINT ptMaxTrackSize;
    }

    public struct WINDOWINFO
    {
      public uint cbSize;
      public NativeMethods.Rect rcWindow;
      public NativeMethods.Rect rcClient;
      public uint dwStyle;
      public uint dwExStyle;
      public uint dwWindowStatus;
      public uint cxWindowBorders;
      public uint cyWindowBorders;
      public ushort atomWindowType;
      public ushort wCreatorVersion;
    }

    internal struct UNSIGNED_RATIO
    {
      public uint uiNumerator;
      public uint uiDenominator;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DWM_TIMING_INFO
    {
      public int cbSize;
      public NativeMethods.UNSIGNED_RATIO rateRefresh;
      public ulong qpcRefreshPeriod;
      public NativeMethods.UNSIGNED_RATIO rateCompose;
      public ulong qpcVBlank;
      public ulong cRefresh;
      public uint cDXRefresh;
      public ulong qpcCompose;
      public ulong cFrame;
      public uint cDXPresent;
      public ulong cRefreshFrame;
      public ulong cFrameSubmitted;
      public uint cDXPresentSubmitted;
      public ulong cFrameConfirmed;
      public uint cDXPresentConfirmed;
      public ulong cRefreshConfirmed;
      public uint cDXRefreshConfirmed;
      public ulong cFramesLate;
      public uint cFramesOutstanding;
      public ulong cFrameDisplayed;
      public ulong qpcFrameDisplayed;
      public ulong cRefreshFrameDisplayed;
      public ulong cFrameComplete;
      public ulong qpcFrameComplete;
      public ulong cFramePending;
      public ulong qpcFramePending;
      public ulong cFramesDisplayed;
      public ulong cFramesComplete;
      public ulong cFramesPending;
      public ulong cFramesAvailable;
      public ulong cFramesDropped;
      public ulong cFramesMissed;
      public ulong cRefreshNextDisplayed;
      public ulong cRefreshNextPresented;
      public ulong cRefreshesDisplayed;
      public ulong cRefreshesPresented;
      public ulong cRefreshStarted;
      public ulong cPixelsReceived;
      public ulong cPixelsDrawn;
      public ulong cBuffersEmpty;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class WINDOWPLACEMENT
    {
      public int length = Marshal.SizeOf(typeof (NativeMethods.WINDOWPLACEMENT));
      public int flags;
      public int showCmd;
      public NativeMethods.POINT ptMinPosition;
      public NativeMethods.POINT ptMaxPosition;
      public NativeMethods.Rect rcNormalPosition;
    }

    public delegate bool EnumChildProc(IntPtr hwnd, IntPtr lParam);
  }
}
