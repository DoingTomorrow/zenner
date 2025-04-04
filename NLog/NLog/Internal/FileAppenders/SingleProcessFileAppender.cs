// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.SingleProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders
{
  [SecuritySafeCritical]
  internal class SingleProcessFileAppender : BaseFileAppender
  {
    public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new SingleProcessFileAppender.Factory();
    private FileStream _file;

    public SingleProcessFileAppender(string fileName, ICreateFileParameters parameters)
      : base(fileName, parameters)
    {
      if (this.CaptureLastWriteTime)
      {
        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.Exists)
          this.FileTouched(fileInfo.GetLastWriteTimeUtc());
        else
          this.FileTouched();
      }
      this._file = this.CreateFileStream(false);
    }

    public override void Write(byte[] bytes, int offset, int count)
    {
      if (this._file == null)
        return;
      this._file.Write(bytes, offset, count);
      if (!this.CaptureLastWriteTime)
        return;
      this.FileTouched();
    }

    public override void Flush()
    {
      if (this._file == null)
        return;
      this._file.Flush();
      this.FileTouched();
    }

    public override void Close()
    {
      if (this._file == null)
        return;
      InternalLogger.Trace<string>("Closing '{0}'", this.FileName);
      try
      {
        this._file.Close();
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) this.FileName
        };
        InternalLogger.Warn(ex, "Failed to close file '{0}'", objArray);
        AsyncHelpers.WaitForDelay(TimeSpan.FromMilliseconds(1.0));
      }
      finally
      {
        this._file = (FileStream) null;
      }
    }

    public override DateTime? GetFileCreationTimeUtc() => new DateTime?(this.CreationTimeUtc);

    public override DateTime? GetFileLastWriteTimeUtc() => new DateTime?(this.LastWriteTimeUtc);

    public override long? GetFileLength()
    {
      return this._file == null ? new long?() : new long?(this._file.Length);
    }

    private class Factory : IFileAppenderFactory
    {
      BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
      {
        return (BaseFileAppender) new SingleProcessFileAppender(fileName, parameters);
      }
    }
  }
}
