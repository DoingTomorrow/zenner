// Decompiled with JetBrains decompiler
// Type: Excel.Exceptions.BiffRecordException
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Exceptions
{
  public class BiffRecordException : Exception
  {
    public BiffRecordException()
    {
    }

    public BiffRecordException(string message)
      : base(message)
    {
    }

    public BiffRecordException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
