// Decompiled with JetBrains decompiler
// Type: SQLitePCL.PlatformStorage
// Assembly: SQLitePCL.Ext, Version=3.8.5.0, Culture=neutral, PublicKeyToken=bddade01e9c850c5
// MVID: 28DC4D07-0E35-45C1-8EF3-CED69BFBD581
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLitePCL.Ext.dll

using System.IO;

#nullable disable
namespace SQLitePCL
{
  internal sealed class PlatformStorage : IPlatformStorage
  {
    private static IPlatformStorage instance = (IPlatformStorage) new PlatformStorage();

    private PlatformStorage()
    {
    }

    internal static IPlatformStorage Instance => PlatformStorage.instance;

    string IPlatformStorage.GetLocalFilePath(string filename) => Path.GetFullPath(filename);

    string IPlatformStorage.GetTemporaryDirectoryPath() => Path.GetTempPath();
  }
}
