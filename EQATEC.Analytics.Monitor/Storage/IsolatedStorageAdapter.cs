// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.IsolatedStorageAdapter
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.IO.IsolatedStorage;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class IsolatedStorageAdapter : FileSystemAdapter
  {
    private readonly IsolatedStorageFile m_isolatedStore;

    internal IsolatedStorageAdapter()
    {
      this.m_isolatedStore = IsolatedStorageFile.GetUserStoreForAssembly();
    }

    internal IsolatedStorageAdapter(IsolatedStorageScope scope)
    {
      this.m_isolatedStore = IsolatedStorageFile.GetStore(scope, (Type) null);
    }

    internal override IStorageFileReader TryOpenFileReader(string filename)
    {
      return StorageFileReader.CreateIsolatedStorageReader(filename, this.m_isolatedStore);
    }

    internal override IStorageFileWriter TryOpenFileWriter(string filename)
    {
      return StorageFileWriter.CreateIsolatedStorageWriter(filename, this.m_isolatedStore);
    }

    internal override string[] GetFiles(string searchPattern)
    {
      return this.m_isolatedStore.GetFileNames(searchPattern);
    }

    internal override void DeleteFile(string fileName)
    {
      if (!this.FileExists(fileName))
        return;
      this.m_isolatedStore.DeleteFile(fileName);
    }

    internal override bool FileExists(string fileName)
    {
      return this.m_isolatedStore.GetFileNames(fileName).Length > 0;
    }

    internal override string RootFolder => "(unknown)";
  }
}
