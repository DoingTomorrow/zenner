// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.Constants
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;

#nullable disable
namespace MahApps.Metro.Native
{
  internal static class Constants
  {
    public const int MONITOR_DEFAULTTONEAREST = 2;
    public const int WM_NCLBUTTONDOWN = 161;
    public const int WM_NCCALCSIZE = 131;
    public const int WM_NCPAINT = 133;
    public const int WM_NCACTIVATE = 134;
    public const int WM_GETMINMAXINFO = 36;
    public const int WM_CREATE = 1;
    public const long WS_MAXIMIZE = 16777216;
    public const int GCLP_HBRBACKGROUND = -10;
    public const int WM_NCHITTEST = 132;
    public const int HT_CAPTION = 2;
    public const int HTLEFT = 10;
    public const int HTRIGHT = 11;
    public const int HTTOP = 12;
    public const int HTTOPLEFT = 13;
    public const int HTTOPRIGHT = 14;
    public const int HTBOTTOM = 15;
    public const int HTBOTTOMLEFT = 16;
    public const int HTBOTTOMRIGHT = 17;
    public const uint TPM_RETURNCMD = 256;
    public const uint TPM_LEFTBUTTON = 0;
    public const uint SYSCOMMAND = 274;
    public const int WM_INITMENU = 278;
    public const int SC_MAXIMIZE = 61488;
    public const int SC_SIZE = 61440;
    public const int SC_MINIMIZE = 61472;
    public const int SC_RESTORE = 61728;
    public const int SC_MOVE = 61456;
    public const int MF_GRAYED = 1;
    public const int MF_BYCOMMAND = 0;
    public const int MF_ENABLED = 0;
    public const uint SWP_NOSIZE = 1;
    public const uint SWP_NOMOVE = 2;
    public const uint SWP_NOZORDER = 4;
    public const uint SWP_NOREDRAW = 8;
    public const uint SWP_NOACTIVATE = 16;
    public const uint SWP_FRAMECHANGED = 32;
    public const uint SWP_SHOWWINDOW = 64;
    public const uint SWP_HIDEWINDOW = 128;
    public const uint SWP_NOCOPYBITS = 256;
    public const uint SWP_NOOWNERZORDER = 512;
    public const uint SWP_NOSENDCHANGING = 1024;
    public const int WM_MOVE = 3;
    public const uint TOPMOST_FLAGS = 1563;

    public enum ShowWindowCommands
    {
      SW_HIDE = 0,
      SW_NORMAL = 1,
      SW_SHOWNORMAL = 1,
      SW_SHOWMINIMIZED = 2,
      SW_MAXIMIZE = 3,
      SW_SHOWMAXIMIZED = 3,
      SW_SHOWNOACTIVATE = 4,
      SW_SHOW = 5,
      SW_MINIMIZE = 6,
      SW_SHOWMINNOACTIVE = 7,
      SW_SHOWNA = 8,
      SW_RESTORE = 9,
    }

    [Flags]
    public enum RedrawWindowFlags : uint
    {
      Invalidate = 1,
      InternalPaint = 2,
      Erase = 4,
      Validate = 8,
      NoInternalPaint = 16, // 0x00000010
      NoErase = 32, // 0x00000020
      NoChildren = 64, // 0x00000040
      AllChildren = 128, // 0x00000080
      UpdateNow = 256, // 0x00000100
      EraseNow = 512, // 0x00000200
      Frame = 1024, // 0x00000400
      NoFrame = 2048, // 0x00000800
    }
  }
}
