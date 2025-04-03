// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IReferenceClock
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("56a86897-0ad4-11ce-b03a-0020af0ba770")]
  [SuppressUnmanagedCodeSecurity]
  [ComImport]
  internal interface IReferenceClock
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetTime(out long pTime);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AdviseTime([In] long baseTime, [In] long streamTime, [In] IntPtr hEvent, out int pdwAdviseCookie);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AdvisePeriodic(
      [In] long startTime,
      [In] long periodTime,
      [In] IntPtr hSemaphore,
      out int pdwAdviseCookie);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Unadvise([In] int dwAdviseCookie);
  }
}
