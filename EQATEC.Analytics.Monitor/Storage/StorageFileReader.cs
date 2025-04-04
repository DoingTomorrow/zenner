// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.StorageFileReader
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class StorageFileReader : IStorageFileReader, IDisposable
  {
    private readonly FileStream m_filestream;

    internal StorageFileReader(FileStream fs) => this.m_filestream = fs;

    public static IStorageFileReader CreateStorageReader(string filename)
    {
      try
      {
        return (IStorageFileReader) new StorageFileReader(new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None));
      }
      catch
      {
        return (IStorageFileReader) null;
      }
    }

    public static IStorageFileReader CreateIsolatedStorageReader(
      string filename,
      IsolatedStorageFile isolatedStorage)
    {
      try
      {
        return (IStorageFileReader) new StorageFileReader((FileStream) new IsolatedStorageFileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, isolatedStorage));
      }
      catch
      {
        return (IStorageFileReader) null;
      }
    }

    public byte[] Read()
    {
      try
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) this.m_filestream))
          return binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
      }
      catch
      {
        return new byte[0];
      }
    }

    public void Dispose()
    {
      try
      {
        this.m_filestream.Close();
        if (this.m_filestream == null)
          return;
        this.m_filestream.Dispose();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
