// Decompiled with JetBrains decompiler
// Type: ICSharpCode.SharpZipLib.Zip.IEntryFactory
// Assembly: ICSharpCode.SharpZipLib, Version=0.85.5.452, Culture=neutral, PublicKeyToken=1b03e6acf1164f73
// MVID: 6C7CFC05-3661-46A0-98AB-FC6F81793937
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ICSharpCode.SharpZipLib.dll

using ICSharpCode.SharpZipLib.Core;

#nullable disable
namespace ICSharpCode.SharpZipLib.Zip
{
  public interface IEntryFactory
  {
    ZipEntry MakeFileEntry(string fileName);

    ZipEntry MakeFileEntry(string fileName, bool useFileSystem);

    ZipEntry MakeDirectoryEntry(string directoryName);

    ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);

    INameTransform NameTransform { get; set; }
  }
}
