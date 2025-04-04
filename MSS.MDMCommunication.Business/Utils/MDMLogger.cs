// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Utils.MDMLogger
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using NLog;

#nullable disable
namespace MSS.MDMCommunication.Business.Utils
{
  public static class MDMLogger
  {
    public static Logger GetLogger() => LogManager.GetCurrentClassLogger();

    public static void WriteMessageToLogger(this Logger logger, LogLevel logLevel, string message)
    {
      logger.Log(logLevel, message);
    }
  }
}
