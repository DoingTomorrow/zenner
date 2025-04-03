// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IAMVideoControl
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("6A2E0670-28E4-11D0-A18c-00A0C9118956")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IAMVideoControl
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetCaps([In] IPin pin, [MarshalAs(UnmanagedType.I4)] out VideoControlFlags flags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetMode([In] IPin pin, [MarshalAs(UnmanagedType.I4), In] VideoControlFlags mode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetMode([In] IPin pin, [MarshalAs(UnmanagedType.I4)] out VideoControlFlags mode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetCurrentActualFrameRate([In] IPin pin, [MarshalAs(UnmanagedType.I8)] out long actualFrameRate);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetMaxAvailableFrameRate(
      [In] IPin pin,
      [In] int index,
      [In] Size dimensions,
      out long maxAvailableFrameRate);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetFrameRateList(
      [In] IPin pin,
      [In] int index,
      [In] Size dimensions,
      out int listSize,
      out IntPtr frameRate);
  }
}
