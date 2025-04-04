// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.RetryingMultiProcessFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.IO;
using System.Security;

#nullable disable
namespace NLog.Internal.FileAppenders
{
  [SecuritySafeCritical]
  internal class RetryingMultiProcessFileAppender(string fileName, ICreateFileParameters parameters) : 
    BaseMutexFileAppender(fileName, parameters)
  {
    public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new RetryingMultiProcessFileAppender.Factory();

    public override void Write(byte[] bytes, int offset, int count)
    {
      using (FileStream fileStream = this.CreateFileStream(false))
        fileStream.Write(bytes, offset, count);
      if (!this.CaptureLastWriteTime)
        return;
      this.FileTouched();
    }

    public override void Flush()
    {
    }

    public override void Close()
    {
    }

    public override DateTime? GetFileCreationTimeUtc()
    {
      FileInfo fileInfo = new FileInfo(this.FileName);
      return fileInfo.Exists ? new DateTime?(fileInfo.GetCreationTimeUtc()) : new DateTime?();
    }

    public override DateTime? GetFileLastWriteTimeUtc()
    {
      FileInfo fileInfo = new FileInfo(this.FileName);
      return fileInfo.Exists ? new DateTime?(fileInfo.GetLastWriteTimeUtc()) : new DateTime?();
    }

    public override long? GetFileLength()
    {
      FileInfo fileInfo = new FileInfo(this.FileName);
      return fileInfo.Exists ? new long?(fileInfo.Length) : new long?();
    }

    private class Factory : IFileAppenderFactory
    {
      BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
      {
        return (BaseFileAppender) new RetryingMultiProcessFileAppender(fileName, parameters);
      }
    }
  }
}
