// Decompiled with JetBrains decompiler
// Type: Excel.Log.ILog
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;

#nullable disable
namespace Excel.Log
{
  public interface ILog
  {
    void Debug(Func<string> message);

    void Info(Func<string> message);

    void Warn(Func<string> message);

    void Error(Func<string> message);

    void Fatal(Func<string> message);

    void InitializeFor(string loggerName);

    void Debug(string message, params object[] formatting);

    void Info(string message, params object[] formatting);

    void Warn(string message, params object[] formatting);

    void Error(string message, params object[] formatting);

    void Fatal(string message, params object[] formatting);
  }
}
