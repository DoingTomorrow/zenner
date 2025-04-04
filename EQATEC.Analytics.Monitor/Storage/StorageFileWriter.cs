// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.StorageFileWriter
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal class StorageFileWriter : IStorageFileWriter, IDisposable
  {
    private readonly FileStream m_filestream;

    internal StorageFileWriter(FileStream fs) => this.m_filestream = fs;

    public static IStorageFileWriter CreateStorageWriter(string filename)
    {
      try
      {
        return (IStorageFileWriter) new StorageFileWriter(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None));
      }
      catch
      {
        return (IStorageFileWriter) null;
      }
    }

    public static IStorageFileWriter CreateIsolatedStorageWriter(
      string filename,
      IsolatedStorageFile isolatedStorage)
    {
      try
      {
        return (IStorageFileWriter) new StorageFileWriter((FileStream) new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, isolatedStorage));
      }
      catch
      {
        return (IStorageFileWriter) null;
      }
    }

    public bool Write(byte[] data)
    {
      try
      {
        this.m_filestream.Position = 0L;
        this.m_filestream.Write(data, 0, data.Length);
        this.m_filestream.SetLength((long) data.Length);
        this.m_filestream.Flush();
        return true;
      }
      catch
      {
        return false;
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
