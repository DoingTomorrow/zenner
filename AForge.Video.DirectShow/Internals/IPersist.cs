// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IPersist
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsDual)]
  [Guid("0000010c-0000-0000-C000-000000000046")]
  [ComImport]
  internal interface IPersist
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetClassID(out Guid pClassID);
  }
}
