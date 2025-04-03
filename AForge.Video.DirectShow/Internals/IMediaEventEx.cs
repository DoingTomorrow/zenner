// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IMediaEventEx
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56a868c0-0ad4-11ce-b03a-0020af0ba770")]
  [InterfaceType(ComInterfaceType.InterfaceIsDual)]
  [ComVisible(true)]
  [ComImport]
  internal interface IMediaEventEx
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetEventHandle(out IntPtr hEvent);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetEvent([MarshalAs(UnmanagedType.I4)] out DsEvCode lEventCode, out IntPtr lParam1, out IntPtr lParam2, int msTimeout);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int WaitForCompletion(int msTimeout, out int pEvCode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int CancelDefaultHandling(int lEvCode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int RestoreDefaultHandling(int lEvCode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int FreeEventParams([MarshalAs(UnmanagedType.I4), In] DsEvCode lEvCode, IntPtr lParam1, IntPtr lParam2);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetNotifyWindow(IntPtr hwnd, int lMsg, IntPtr lInstanceData);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetNotifyFlags(int lNoNotifyFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetNotifyFlags(out int lplNoNotifyFlags);
  }
}
