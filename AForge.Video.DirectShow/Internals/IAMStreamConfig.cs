// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IAMStreamConfig
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("C6E13340-30AC-11d0-A18C-00A0C9118956")]
  [ComImport]
  internal interface IAMStreamConfig
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetFormat([MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetFormat([MarshalAs(UnmanagedType.LPStruct)] out AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetNumberOfCapabilities(out int count, out int size);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetStreamCaps([In] int index, [MarshalAs(UnmanagedType.LPStruct)] out AMMediaType mediaType, [MarshalAs(UnmanagedType.LPStruct), In] VideoStreamConfigCaps streamConfigCaps);
  }
}
