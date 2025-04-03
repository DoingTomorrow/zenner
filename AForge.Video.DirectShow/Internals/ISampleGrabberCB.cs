// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.ISampleGrabberCB
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
  [Guid("0579154A-2B53-4994-B0D0-E773148EFF85")]
  [ComImport]
  internal interface ISampleGrabberCB
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SampleCB(double sampleTime, IntPtr sample);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int BufferCB(double sampleTime, IntPtr buffer, int bufferLen);
  }
}
