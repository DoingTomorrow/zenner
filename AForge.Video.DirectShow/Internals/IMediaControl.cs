// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IMediaControl
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56A868B1-0AD4-11CE-B03A-0020AF0BA770")]
  [InterfaceType(ComInterfaceType.InterfaceIsDual)]
  [ComImport]
  internal interface IMediaControl
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Run();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Pause();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Stop();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetState(int timeout, out int filterState);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int RenderFile(string fileName);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int AddSourceFilter([In] string fileName, [MarshalAs(UnmanagedType.IDispatch)] out object filterInfo);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int get_FilterCollection([MarshalAs(UnmanagedType.IDispatch)] out object collection);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int get_RegFilterCollection([MarshalAs(UnmanagedType.IDispatch)] out object collection);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int StopWhenReady();
  }
}
