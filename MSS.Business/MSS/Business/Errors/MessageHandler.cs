// Decompiled with JetBrains decompiler
// Type: MSS.Business.Errors.MessageHandler
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Utils.Utils;
using Newtonsoft.Json;
using NLog;
using System;

#nullable disable
namespace MSS.Business.Errors
{
  public static class MessageHandler
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static string GetMessage(MessageCodes errorCode, params object[] values)
    {
      return string.Format(errorCode.GetStringValue(), values);
    }

    public static string LogException(Exception ex, MessageCodes messageCode)
    {
      string message = string.Format("Error message: {0} Error code: {1}. Exception is :'{2}'. Stack trace: {3}", (object) ex.Message, (object) messageCode, (object) ex.GetBaseException().GetType().ToString(), (object) ex.StackTrace);
      MessageHandler.Logger.Log(NLog.LogLevel.Error, message);
      return message;
    }

    public static string LogException(Exception ex)
    {
      if (ex == null)
        return string.Empty;
      string message = string.Format("Error occured. Exception is :'{0}'. Message: {1} Stack trace: {2}", (object) ex.GetBaseException().GetType().ToString(), (object) ex.GetBaseException().Message, (object) ex.GetBaseException().StackTrace);
      MessageHandler.Logger.Log(NLog.LogLevel.Error, message);
      return message;
    }

    public static void LogDebug(string message, object objectToLog = null)
    {
      MessageHandler.Logger.Log(NLog.LogLevel.Debug, message);
      if (objectToLog == null)
        return;
      MessageHandler.Logger.Debug(objectToLog.ToJson());
    }

    public static void LogInfo(string message) => MessageHandler.Logger.Log(NLog.LogLevel.Info, message);

    public static void LogError(MessageCodes messageCode)
    {
      MessageHandler.Logger.Log(NLog.LogLevel.Error, "Error code: " + (object) messageCode);
    }

    public static void LogException(string messageCode)
    {
      MessageHandler.Logger.Log(NLog.LogLevel.Error, "Error code: " + messageCode);
    }

    public static void LogGMMExceptionMessage(string message)
    {
      MessageHandler.Logger.Log(NLog.LogLevel.Error, "GMM error: " + message);
    }

    public static string ToJson(this object value)
    {
      JsonSerializerSettings settings = new JsonSerializerSettings()
      {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
      };
      return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
    }
  }
}
