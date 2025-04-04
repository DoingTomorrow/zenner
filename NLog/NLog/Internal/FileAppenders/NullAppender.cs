// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.NullAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal.FileAppenders
{
  internal class NullAppender(string fileName, ICreateFileParameters createParameters) : 
    BaseFileAppender(fileName, createParameters)
  {
    public static readonly IFileAppenderFactory TheFactory = (IFileAppenderFactory) new NullAppender.Factory();

    public override void Close()
    {
    }

    public override void Flush()
    {
    }

    public override DateTime? GetFileCreationTimeUtc() => new DateTime?(DateTime.UtcNow);

    public override DateTime? GetFileLastWriteTimeUtc() => new DateTime?(DateTime.UtcNow);

    public override long? GetFileLength() => new long?(0L);

    public override void Write(byte[] bytes, int offset, int count)
    {
    }

    private class Factory : IFileAppenderFactory
    {
      BaseFileAppender IFileAppenderFactory.Open(string fileName, ICreateFileParameters parameters)
      {
        return (BaseFileAppender) new NullAppender(fileName, parameters);
      }
    }
  }
}
