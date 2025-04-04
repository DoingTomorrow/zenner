// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.CREATESTRUCT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace MahApps.Metro.Native
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct CREATESTRUCT
  {
    public IntPtr lpCreateParams;
    public IntPtr hInstance;
    public IntPtr hMenu;
    public IntPtr hwndParent;
    public int cy;
    public int cx;
    public int y;
    public int x;
    public int style;
    public string lpszName;
    public string lpszClass;
    public int dwExStyle;
  }
}
