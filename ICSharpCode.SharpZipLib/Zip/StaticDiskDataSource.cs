// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.StaticDiskDataSource
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using System.IO;

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  public class StaticDiskDataSource : IStaticDataSource
  {
    private string fileName_;

    public StaticDiskDataSource(string fileName) => this.fileName_ = fileName;

    public Stream GetSource() => (Stream) File.OpenRead(this.fileName_);
  }
}
