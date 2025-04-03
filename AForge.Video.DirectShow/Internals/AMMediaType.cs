// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.AMMediaType
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [ComVisible(false)]
  [StructLayout(LayoutKind.Sequential)]
  internal class AMMediaType : IDisposable
  {
    public Guid MajorType;
    public Guid SubType;
    [MarshalAs(UnmanagedType.Bool)]
    public bool FixedSizeSamples = true;
    [MarshalAs(UnmanagedType.Bool)]
    public bool TemporalCompression;
    public int SampleSize = 1;
    public Guid FormatType;
    public IntPtr unkPtr;
    public int FormatSize;
    public IntPtr FormatPtr;

    ~AMMediaType() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.FormatSize != 0 && this.FormatPtr != IntPtr.Zero)
      {
        Marshal.FreeCoTaskMem(this.FormatPtr);
        this.FormatSize = 0;
      }
      if (!(this.unkPtr != IntPtr.Zero))
        return;
      Marshal.Release(this.unkPtr);
      this.unkPtr = IntPtr.Zero;
    }
  }
}
