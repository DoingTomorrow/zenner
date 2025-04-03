// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IPin
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56A86891-0AD4-11CE-B03A-0020AF0BA770")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IPin
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Connect([In] IPin receivePin, [MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ReceiveConnection([In] IPin receivePin, [MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Disconnect();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ConnectedTo(out IPin pin);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ConnectionMediaType([MarshalAs(UnmanagedType.LPStruct), Out] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryPinInfo(out PinInfo pinInfo);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryDirection(out PinDirection pinDirection);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryId([MarshalAs(UnmanagedType.LPWStr)] out string id);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryAccept([MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EnumMediaTypes(IntPtr enumerator);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int QueryInternalConnections(IntPtr apPin, [In, Out] ref int nPin);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EndOfStream();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int BeginFlush();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EndFlush();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int NewSegment(long start, long stop, double rate);
  }
}
