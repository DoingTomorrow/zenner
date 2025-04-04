// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.FileSystemAdapter
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal abstract class FileSystemAdapter
  {
    internal abstract bool FileExists(string filename);

    internal abstract IStorageFileReader TryOpenFileReader(string filename);

    internal abstract IStorageFileWriter TryOpenFileWriter(string filename);

    internal abstract string[] GetFiles(string searchPattern);

    internal abstract void DeleteFile(string fileName);

    internal abstract string RootFolder { get; }
  }
}
