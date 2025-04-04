// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.BaseFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using Microsoft.Win32.SafeHandles;
using NLog.Common;
using NLog.Time;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders
{
  [SecuritySafeCritical]
  internal abstract class BaseFileAppender : IDisposable
  {
    private readonly Random _random = new Random();
    private DateTime _creationTimeUtc;

    protected BaseFileAppender(string fileName, ICreateFileParameters createParameters)
    {
      this.CreateFileParameters = createParameters;
      this.FileName = fileName;
      this.OpenTimeUtc = DateTime.UtcNow;
      this.LastWriteTimeUtc = DateTime.MinValue;
      this.CaptureLastWriteTime = createParameters.CaptureLastWriteTime;
    }

    protected bool CaptureLastWriteTime { get; private set; }

    public string FileName { get; private set; }

    public DateTime CreationTimeUtc
    {
      get => this._creationTimeUtc;
      internal set
      {
        this._creationTimeUtc = value;
        this.CreationTimeSource = TimeSource.Current.FromSystemTime(value);
      }
    }

    public DateTime CreationTimeSource { get; private set; }

    public DateTime OpenTimeUtc { get; private set; }

    public DateTime LastWriteTimeUtc { get; private set; }

    public ICreateFileParameters CreateFileParameters { get; private set; }

    public void Write(byte[] bytes) => this.Write(bytes, 0, bytes.Length);

    public abstract void Write(byte[] bytes, int offset, int count);

    public abstract void Flush();

    public abstract void Close();

    public abstract DateTime? GetFileCreationTimeUtc();

    public abstract DateTime? GetFileLastWriteTimeUtc();

    public abstract long? GetFileLength();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.Close();
    }

    protected void FileTouched()
    {
      if (!this.CaptureLastWriteTime)
        return;
      this.FileTouched(DateTime.UtcNow);
    }

    protected void FileTouched(DateTime dateTime) => this.LastWriteTimeUtc = dateTime;

    protected FileStream CreateFileStream(bool allowFileSharedWriting)
    {
      int writeAttemptDelay = this.CreateFileParameters.ConcurrentWriteAttemptDelay;
      InternalLogger.Trace<string, bool>("Opening {0} with allowFileSharedWriting={1}", this.FileName, allowFileSharedWriting);
      for (int index = 0; index < this.CreateFileParameters.ConcurrentWriteAttempts; ++index)
      {
        try
        {
          try
          {
            return this.TryCreateFileStream(allowFileSharedWriting);
          }
          catch (DirectoryNotFoundException ex1)
          {
            if (!this.CreateFileParameters.CreateDirs)
            {
              throw;
            }
            else
            {
              string directoryName = Path.GetDirectoryName(this.FileName);
              try
              {
                Directory.CreateDirectory(directoryName);
              }
              catch (DirectoryNotFoundException ex2)
              {
                throw new NLogRuntimeException("Could not create directory {0}", new object[1]
                {
                  (object) directoryName
                });
              }
              return this.TryCreateFileStream(allowFileSharedWriting);
            }
          }
        }
        catch (IOException ex)
        {
          if (!this.CreateFileParameters.ConcurrentWrites || index + 1 == this.CreateFileParameters.ConcurrentWriteAttempts)
          {
            throw;
          }
          else
          {
            int num = this._random.Next(writeAttemptDelay);
            InternalLogger.Warn<int, string, int>("Attempt #{0} to open {1} failed. Sleeping for {2}ms", index, this.FileName, num);
            writeAttemptDelay *= 2;
            AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds((double) num));
          }
        }
      }
      throw new InvalidOperationException("Should not be reached.");
    }

    private FileStream WindowsCreateFile(string fileName, bool allowFileSharedWriting)
    {
      int dwShareMode = 1;
      if (allowFileSharedWriting)
        dwShareMode |= 2;
      if (this.CreateFileParameters.EnableFileDelete && PlatformDetector.CurrentOS != RuntimeOS.Windows)
        dwShareMode |= 4;
      SafeFileHandle handle = (SafeFileHandle) null;
      FileStream file = (FileStream) null;
      try
      {
        handle = Win32FileNativeMethods.CreateFile(fileName, Win32FileNativeMethods.FileAccess.GenericWrite, dwShareMode, IntPtr.Zero, Win32FileNativeMethods.CreationDisposition.OpenAlways, this.CreateFileParameters.FileAttributes, IntPtr.Zero);
        if (handle.IsInvalid)
          Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        file = new FileStream(handle, System.IO.FileAccess.Write, this.CreateFileParameters.BufferSize);
        file.Seek(0L, SeekOrigin.End);
        return file;
      }
      catch
      {
        file?.Dispose();
        if (handle != null && !handle.IsClosed)
          handle.Close();
        throw;
      }
    }

    private FileStream TryCreateFileStream(bool allowFileSharedWriting)
    {
      this.UpdateCreationTime();
      try
      {
        if (!this.CreateFileParameters.ForceManaged)
        {
          if (PlatformDetector.IsDesktopWin32)
          {
            if (!PlatformDetector.IsMono)
              return this.WindowsCreateFile(this.FileName, allowFileSharedWriting);
          }
        }
      }
      catch (SecurityException ex)
      {
        InternalLogger.Debug<string>("Could not use native Windows create file, falling back to managed filestream: {0}", this.FileName);
      }
      FileShare share = allowFileSharedWriting ? FileShare.ReadWrite : FileShare.Read;
      if (this.CreateFileParameters.EnableFileDelete)
        share |= FileShare.Delete;
      return new FileStream(this.FileName, FileMode.Append, System.IO.FileAccess.Write, share, this.CreateFileParameters.BufferSize);
    }

    private void UpdateCreationTime()
    {
      FileInfo fileInfo = new FileInfo(this.FileName);
      if (fileInfo.Exists)
      {
        this.CreationTimeUtc = FileCharacteristicsHelper.ValidateFileCreationTime<FileInfo>(fileInfo, (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetCreationTimeUtc())), (Func<FileInfo, DateTime?>) (f => new DateTime?(f.GetLastWriteTimeUtc()))).Value;
      }
      else
      {
        File.Create(this.FileName).Dispose();
        this.CreationTimeUtc = DateTime.UtcNow;
        File.SetCreationTimeUtc(this.FileName, this.CreationTimeUtc);
      }
    }
  }
}
