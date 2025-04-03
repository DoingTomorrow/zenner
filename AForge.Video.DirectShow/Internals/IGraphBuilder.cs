// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IGraphBuilder
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
  [Guid("56A868A9-0AD4-11CE-B03A-0020AF0BA770")]
  [ComImport]
  internal interface IGraphBuilder
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AddFilter([In] IBaseFilter filter, [MarshalAs(UnmanagedType.LPWStr), In] string name);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int RemoveFilter([In] IBaseFilter filter);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int EnumFilters(out IEnumFilters enumerator);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int FindFilterByName([MarshalAs(UnmanagedType.LPWStr), In] string name, out IBaseFilter filter);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ConnectDirect([In] IPin pinOut, [In] IPin pinIn, [MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Reconnect([In] IPin pin);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Disconnect([In] IPin pin);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetDefaultSyncSource();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Connect([In] IPin pinOut, [In] IPin pinIn);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Render([In] IPin pinOut);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int RenderFile([MarshalAs(UnmanagedType.LPWStr), In] string file, [MarshalAs(UnmanagedType.LPWStr), In] string playList);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AddSourceFilter([MarshalAs(UnmanagedType.LPWStr), In] string fileName, [MarshalAs(UnmanagedType.LPWStr), In] string filterName, out IBaseFilter filter);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetLogFile(IntPtr hFile);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Abort();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int ShouldOperationContinue();
  }
}
