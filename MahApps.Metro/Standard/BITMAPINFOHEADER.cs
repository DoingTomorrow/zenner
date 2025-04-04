// Decompiled with JetBrains decompiler
// Type: Standard.BITMAPINFOHEADER
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  internal struct BITMAPINFOHEADER
  {
    public int biSize;
    public int biWidth;
    public int biHeight;
    public short biPlanes;
    public short biBitCount;
    public BI biCompression;
    public int biSizeImage;
    public int biXPelsPerMeter;
    public int biYPelsPerMeter;
    public int biClrUsed;
    public int biClrImportant;
  }
}
