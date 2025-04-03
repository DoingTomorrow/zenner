// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IMediaFilter
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
  [Guid("56a86899-0ad4-11ce-b03a-0020af0ba770")]
  [SuppressUnmanagedCodeSecurity]
  [ComImport]
  internal interface IMediaFilter : IPersist
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    new int GetClassID(out Guid pClassID);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Stop();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Pause();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Run([In] long tStart);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetState([In] int dwMilliSecsTimeout, out FilterState filtState);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetSyncSource([In] IReferenceClock pClock);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetSyncSource(out IReferenceClock pClock);
  }
}
