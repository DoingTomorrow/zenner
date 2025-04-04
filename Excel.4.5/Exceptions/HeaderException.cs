// Decompiled with JetBrains decompiler
// Type: Excel.Exceptions.HeaderException
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Exceptions
{
  public class HeaderException : Exception
  {
    public HeaderException()
    {
    }

    public HeaderException(string message)
      : base(message)
    {
    }

    public HeaderException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
