// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.EntryPatchData
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  internal class EntryPatchData
  {
    private long sizePatchOffset_;
    private long crcPatchOffset_;

    public long SizePatchOffset
    {
      get => this.sizePatchOffset_;
      set => this.sizePatchOffset_ = value;
    }

    public long CrcPatchOffset
    {
      get => this.crcPatchOffset_;
      set => this.crcPatchOffset_ = value;
    }
  }
}
