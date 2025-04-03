// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IBaseFilter
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56A86895-0AD4-11CE-B03A-0020AF0BA770")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IBaseFilter
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetClassID(out Guid ClassID);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Stop();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Pause();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Run(long start);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetState(int milliSecsTimeout, out int filterState);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetSyncSource([In] IntPtr clock);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetSyncSource(out IntPtr clock);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EnumPins(out IEnumPins enumPins);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int FindPin([MarshalAs(UnmanagedType.LPWStr), In] string id, out IPin pin);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryFilterInfo(out FilterInfo filterInfo);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int JoinFilterGraph([In] IFilterGraph graph, [MarshalAs(UnmanagedType.LPWStr), In] string name);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryVendorInfo([MarshalAs(UnmanagedType.LPWStr)] out string vendorInfo);
  }
}
