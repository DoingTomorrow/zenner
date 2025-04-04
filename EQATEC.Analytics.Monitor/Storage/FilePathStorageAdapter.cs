// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.FilePathStorageAdapter
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System.IO;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class FilePathStorageAdapter : FileSystemAdapter
  {
    private readonly string m_rootFolder;
    private readonly string m_uniquePrefix;

    internal FilePathStorageAdapter(string rootFolder)
      : this(rootFolder, string.Empty)
    {
    }

    internal FilePathStorageAdapter(string rootFolder, string uniquePrefix)
    {
      this.m_rootFolder = rootFolder;
      this.m_uniquePrefix = string.IsNullOrEmpty(uniquePrefix) ? "" : StringUtil.ToValidFileName(uniquePrefix);
      if (Directory.Exists(this.m_rootFolder))
        return;
      Directory.CreateDirectory(this.m_rootFolder);
    }

    internal override IStorageFileReader TryOpenFileReader(string filename)
    {
      return StorageFileReader.CreateStorageReader(this.GetFullPath(filename));
    }

    internal override IStorageFileWriter TryOpenFileWriter(string filename)
    {
      return StorageFileWriter.CreateStorageWriter(this.GetFullPath(filename));
    }

    internal override string[] GetFiles(string searchPattern)
    {
      string[] files = Directory.GetFiles(this.m_rootFolder, this.GetFileName(searchPattern));
      for (int index = 0; index < files.Length; ++index)
        files[index] = Path.GetFileName(files[index]);
      return files;
    }

    internal override void DeleteFile(string fileName)
    {
      string fullPath = this.GetFullPath(fileName);
      if (!File.Exists(fullPath))
        return;
      File.Delete(fullPath);
    }

    internal override bool FileExists(string fileName) => File.Exists(this.GetFullPath(fileName));

    internal override string RootFolder => this.GetFullPath("");

    private string GetFileName(string fileName)
    {
      return string.Format("{0}{1}", (object) this.m_uniquePrefix, (object) fileName);
    }

    private string GetFullPath(string fileName)
    {
      return Path.Combine(this.m_rootFolder, this.GetFileName(fileName));
    }
  }
}
