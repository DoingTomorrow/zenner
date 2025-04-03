// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.MediaType
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [ComVisible(false)]
  internal static class MediaType
  {
    public static readonly Guid Video = new Guid(1935960438, (short) 0, (short) 16, (byte) 128, (byte) 0, (byte) 0, (byte) 170, (byte) 0, (byte) 56, (byte) 155, (byte) 113);
    public static readonly Guid Interleaved = new Guid(1937138025, (short) 0, (short) 16, (byte) 128, (byte) 0, (byte) 0, (byte) 170, (byte) 0, (byte) 56, (byte) 155, (byte) 113);
    public static readonly Guid Audio = new Guid(1935963489, (short) 0, (short) 16, (byte) 128, (byte) 0, (byte) 0, (byte) 170, (byte) 0, (byte) 56, (byte) 155, (byte) 113);
    public static readonly Guid Text = new Guid(1937012852, (short) 0, (short) 16, (byte) 128, (byte) 0, (byte) 0, (byte) 170, (byte) 0, (byte) 56, (byte) 155, (byte) 113);
    public static readonly Guid Stream = new Guid(3828804483U, (ushort) 21071, (ushort) 4558, (byte) 159, (byte) 83, (byte) 0, (byte) 32, (byte) 175, (byte) 11, (byte) 167, (byte) 112);
  }
}
