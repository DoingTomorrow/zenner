// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IAMCameraControl
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("C6E13370-30AC-11d0-A18C-00A0C9118956")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IAMCameraControl
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetRange(
      [In] CameraControlProperty Property,
      out int pMin,
      out int pMax,
      out int pSteppingDelta,
      out int pDefault,
      out CameraControlFlags pCapsFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Set([In] CameraControlProperty Property, [In] int lValue, [In] CameraControlFlags Flags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Get([In] CameraControlProperty Property, out int lValue, out CameraControlFlags Flags);
  }
}
