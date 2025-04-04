// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.FileStorage
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  public class FileStorage : IStorage, IDisposable
  {
    private readonly FileSystemAdapter m_filesystemAdapter;
    private readonly object m_lock = new object();
    private readonly string m_uniqueId;
    private byte[] m_legacyFileBytes;
    private bool m_hasAttemptedToLoadLegacyFile;
    private IStorageFileWriter m_sessionFile;
    private bool m_sessionFileFailed;
    private TimeSpan m_uptimeForLastSessionFileFailure;

    public FileStorage(string rootPath)
      : this((FileSystemAdapter) new FilePathStorageAdapter(rootPath))
    {
    }

    public FileStorage(string rootPath, string uniqueUserId)
      : this((FileSystemAdapter) new FilePathStorageAdapter(rootPath, uniqueUserId))
    {
    }

    public FileStorage(IsolatedStorageScope scope)
      : this((FileSystemAdapter) new IsolatedStorageAdapter(scope))
    {
    }

    internal FileStorage(FileSystemAdapter filesystemAdapter)
    {
      this.m_uniqueId = Guid.NewGuid().ToString("N");
      this.m_filesystemAdapter = filesystemAdapter;
    }

    internal string RootPath => this.m_filesystemAdapter.RootFolder;

    private string GetFileName(string productId, StorageDataType type)
    {
      string typeName = FileStorage.GetTypeName(type);
      return type == StorageDataType.SessionData ? string.Format("storage_{0}_{1}_{2}.bin", (object) productId, (object) typeName, (object) this.m_uniqueId) : string.Format("storage_{0}_{1}.bin", (object) productId, (object) typeName);
    }

    internal static string GetTypeName(StorageDataType type) => type.ToString().ToLower();

    internal static string GetSearchPattern(string productId, StorageDataType type)
    {
      return string.Format("storage_{0}_{1}_*.bin", (object) productId, (object) FileStorage.GetTypeName(type));
    }

    protected virtual string GetLegacyFileName(StorageDataDescriptor descriptor)
    {
      return string.Format("storage_{0}.bin", (object) descriptor.ProductId);
    }

    public virtual void Save(StorageDataDescriptor descriptor, byte[] data)
    {
      if (data == null)
        return;
      lock (this.m_lock)
      {
        if (descriptor.DataType == StorageDataType.SessionData)
          this.WriteSessionData(descriptor, data);
        else
          this.SaveToFile(descriptor, data);
      }
    }

    private void WriteSessionData(StorageDataDescriptor descriptor, byte[] data)
    {
      string fileName = this.GetFileName(descriptor.ProductId, descriptor.DataType);
      if (this.m_sessionFile == null)
      {
        if (this.m_sessionFileFailed)
        {
          TimeSpan uptime = Timing.Uptime;
          Random random = new Random();
          if (uptime.Subtract(this.m_uptimeForLastSessionFileFailure) < TimeSpan.FromSeconds((double) random.Next(1, 5)))
            return;
        }
        this.m_sessionFile = this.m_filesystemAdapter.TryOpenFileWriter(fileName);
      }
      lock (this.m_lock)
      {
        if (this.m_sessionFile != null && this.m_sessionFile.Write(data))
        {
          this.m_sessionFileFailed = false;
        }
        else
        {
          if (this.m_sessionFile != null)
            this.m_sessionFile.Dispose();
          this.m_sessionFile = (IStorageFileWriter) null;
          this.m_sessionFileFailed = true;
          this.m_uptimeForLastSessionFileFailure = Timing.Uptime;
        }
      }
    }

    private void SaveToFile(StorageDataDescriptor descriptor, byte[] data)
    {
      string fileName = this.GetFileName(descriptor.ProductId, descriptor.DataType);
      lock (this.m_lock)
      {
        using (IStorageFileWriter storageFileWriter = this.m_filesystemAdapter.TryOpenFileWriter(fileName))
          storageFileWriter?.Write(data);
      }
    }

    private byte[] LoadFromFile(StorageDataDescriptor descriptor)
    {
      string fileName = this.GetFileName(descriptor.ProductId, descriptor.DataType);
      lock (this.m_lock)
        return this.ReadBinaryFile(fileName) ?? this.LoadFromLegacyFile(descriptor);
    }

    private byte[] LoadFromFileOtherwiseFromLegacyFile(StorageDataDescriptor descriptor)
    {
      if (!this.m_filesystemAdapter.FileExists(this.GetFileName(descriptor.ProductId, descriptor.DataType)))
        return this.LoadFromLegacyFile(descriptor);
      lock (this.m_lock)
      {
        if (this.m_sessionFile != null)
          this.m_sessionFile.Dispose();
        this.m_sessionFile = (IStorageFileWriter) null;
        return this.LoadFromFile(descriptor);
      }
    }

    private byte[] LoadFromLegacyFile(StorageDataDescriptor descriptor)
    {
      if (!this.m_hasAttemptedToLoadLegacyFile)
      {
        this.m_hasAttemptedToLoadLegacyFile = true;
        string legacyFileName = this.GetLegacyFileName(descriptor);
        if (this.m_filesystemAdapter.FileExists(legacyFileName))
        {
          this.m_legacyFileBytes = this.ReadBinaryFile(legacyFileName);
          this.m_filesystemAdapter.DeleteFile(legacyFileName);
        }
      }
      return this.m_legacyFileBytes;
    }

    private byte[] ReadBinaryFile(string filename)
    {
      byte[] numArray = (byte[]) null;
      if (this.m_filesystemAdapter.FileExists(filename))
      {
        using (IStorageFileReader storageFileReader = this.m_filesystemAdapter.TryOpenFileReader(filename))
        {
          if (storageFileReader != null)
            numArray = storageFileReader.Read();
        }
      }
      return numArray;
    }

    public virtual byte[] Load(StorageDataDescriptor descriptor)
    {
      byte[] numArray = (byte[]) null;
      lock (this.m_lock)
        numArray = descriptor.DataType != StorageDataType.SessionData ? this.LoadFromFile(descriptor) : this.LoadFromFileOtherwiseFromLegacyFile(descriptor);
      return numArray;
    }

    public virtual byte[] ReadAbandonedSessionData(string productId)
    {
      try
      {
        string searchPattern = FileStorage.GetSearchPattern(productId, StorageDataType.SessionData);
        string fileName = this.GetFileName(productId, StorageDataType.SessionData);
        foreach (string str in new List<string>((IEnumerable<string>) this.m_filesystemAdapter.GetFiles(searchPattern)))
        {
          if (!fileName.Equals(str, StringComparison.OrdinalIgnoreCase))
          {
            try
            {
              byte[] numArray = (byte[]) null;
              using (IStorageFileReader storageFileReader = this.m_filesystemAdapter.TryOpenFileReader(str))
              {
                if (storageFileReader != null)
                  numArray = storageFileReader.Read();
                else
                  continue;
              }
              this.m_filesystemAdapter.DeleteFile(str);
              if (numArray != null)
                return numArray;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      return (byte[]) null;
    }

    public void Dispose()
    {
      if (this.m_sessionFile == null)
        return;
      lock (this.m_lock)
      {
        this.m_sessionFile.Dispose();
        this.m_sessionFile = (IStorageFileWriter) null;
      }
    }
  }
}
