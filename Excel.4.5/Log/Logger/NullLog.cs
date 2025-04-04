// Decompiled with JetBrains decompiler
// Type: Excel.Log.Logger.NullLog
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Log.Logger
{
  public class NullLog : ILog, ILog<NullLog>
  {
    public void Debug(Func<string> message)
    {
    }

    public void Info(Func<string> message)
    {
    }

    public void Warn(Func<string> message)
    {
    }

    public void Error(Func<string> message)
    {
    }

    public void Fatal(Func<string> message)
    {
    }

    public void InitializeFor(string loggerName)
    {
    }

    public void Debug(string message, params object[] formatting)
    {
    }

    public void Info(string message, params object[] formatting)
    {
    }

    public void Warn(string message, params object[] formatting)
    {
    }

    public void Error(string message, params object[] formatting)
    {
    }

    public void Fatal(string message, params object[] formatting)
    {
    }
  }
}
