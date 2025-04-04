// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.DescriptorData
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  public class DescriptorData
  {
    private long size;
    private long compressedSize;
    private long crc;

    public long CompressedSize
    {
      get => this.compressedSize;
      set => this.compressedSize = value;
    }

    public long Size
    {
      get => this.size;
      set => this.size = value;
    }

    public long Crc
    {
      get => this.crc;
      set => this.crc = value & (long) uint.MaxValue;
    }
  }
}
