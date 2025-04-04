// Decompiled with JetBrains decompiler
// Type: Standard.THUMBBUTTON
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
  internal struct THUMBBUTTON
  {
    public const int THBN_CLICKED = 6144;
    public THB dwMask;
    public uint iId;
    public uint iBitmap;
    public IntPtr hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szTip;
    public THBF dwFlags;
  }
}
