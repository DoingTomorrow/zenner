// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.ISpecifyPropertyPages
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("B196B28B-BAB4-101A-B69C-00AA00341D07")]
  [ComImport]
  internal interface ISpecifyPropertyPages
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetPages(out CAUUID pPages);
  }
}
