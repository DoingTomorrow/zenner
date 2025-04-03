// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.Win32
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  internal static class Win32
  {
    [DllImport("ole32.dll")]
    public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

    [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
    public static extern int MkParseDisplayName(
      IBindCtx pbc,
      string szUserName,
      ref int pchEaten,
      out IMoniker ppmk);

    [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe int memcpy(byte* dst, byte* src, int count);

    [DllImport("oleaut32.dll")]
    public static extern int OleCreatePropertyFrame(
      IntPtr hwndOwner,
      int x,
      int y,
      [MarshalAs(UnmanagedType.LPWStr)] string caption,
      int cObjects,
      [MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
      int cPages,
      IntPtr lpPageClsID,
      int lcid,
      int dwReserved,
      IntPtr lpvReserved);
  }
}
