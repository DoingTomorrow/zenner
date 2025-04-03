// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.FilterInfo
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [ComVisible(false)]
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  internal struct FilterInfo
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string Name;
    public IFilterGraph FilterGraph;
  }
}
