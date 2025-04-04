// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.WorkItem
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zlib
{
  internal class WorkItem
  {
    public byte[] buffer;
    public byte[] compressed;
    public int crc;
    public int index;
    public int ordinal;
    public int inputBytesAvailable;
    public int compressedBytesAvailable;
    public ZlibCodec compressor;

    public WorkItem(
      int size,
      CompressionLevel compressLevel,
      CompressionStrategy strategy,
      int ix)
    {
      this.buffer = new byte[size];
      this.compressed = new byte[size + (size / 32768 + 1) * 5 * 2];
      this.compressor = new ZlibCodec();
      this.compressor.InitializeDeflate(compressLevel, false);
      this.compressor.OutputBuffer = this.compressed;
      this.compressor.InputBuffer = this.buffer;
      this.index = ix;
    }
  }
}
