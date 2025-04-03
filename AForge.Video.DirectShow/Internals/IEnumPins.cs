// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IEnumPins
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56A86892-0AD4-11CE-B03A-0020AF0BA770")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IEnumPins
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Next([In] int cPins, [MarshalAs(UnmanagedType.LPArray), Out] IPin[] pins, out int pinsFetched);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Skip([In] int cPins);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Reset();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Clone(out IEnumPins enumPins);
  }
}
