// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IAMCrossbar
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("C6E13380-30AC-11D0-A18C-00A0C9118956")]
  [SuppressUnmanagedCodeSecurity]
  [ComImport]
  internal interface IAMCrossbar
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int get_PinCounts(out int outputPinCount, out int inputPinCount);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int CanRoute([In] int outputPinIndex, [In] int inputPinIndex);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Route([In] int outputPinIndex, [In] int inputPinIndex);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int get_IsRoutedTo([In] int outputPinIndex, out int inputPinIndex);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int get_CrossbarPinInfo(
      [MarshalAs(UnmanagedType.Bool), In] bool isInputPin,
      [In] int pinIndex,
      out int pinIndexRelated,
      out PhysicalConnectorType physicalType);
  }
}
