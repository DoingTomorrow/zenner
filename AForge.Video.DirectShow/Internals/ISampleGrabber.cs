// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.ISampleGrabber
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("6B652FFF-11FE-4FCE-92AD-0266B5D7C78F")]
  [ComImport]
  internal interface ISampleGrabber
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetOneShot([MarshalAs(UnmanagedType.Bool), In] bool oneShot);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetMediaType([MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetConnectedMediaType([MarshalAs(UnmanagedType.LPStruct), Out] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetBufferSamples([MarshalAs(UnmanagedType.Bool), In] bool bufferThem);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetCurrentBuffer(ref int bufferSize, IntPtr buffer);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetCurrentSample(IntPtr sample);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetCallback(ISampleGrabberCB callback, int whichMethodToCallback);
  }
}
