// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.SaveGMMValuesLogger
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Errors;
using MSS.Utils.Utils;
using NLog;
using System;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public static class SaveGMMValuesLogger
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    public static string GetMessage(MessageCodes errorCode, params object[] values)
    {
      return string.Format(errorCode.GetStringValue(), values);
    }

    public static string LogException(Exception ex, MessageCodes messageCode)
    {
      string message = string.Format("Error message: {0} Error code: {1}. Exception is :'{2}'. Stack trace: {3}", (object) ex.Message, (object) messageCode, (object) ex.GetBaseException().GetType().ToString(), (object) ex.StackTrace);
      SaveGMMValuesLogger.logger.Log(NLog.LogLevel.Error, message);
      return message;
    }

    public static string LogException(Exception ex)
    {
      if (ex == null)
        return string.Empty;
      string message = string.Format("Error occured. Exception is :'{0}'. Message: {1} Stack trace: {2}", (object) ex.GetBaseException().GetType().ToString(), (object) ex.GetBaseException().Message, (object) ex.GetBaseException().StackTrace);
      SaveGMMValuesLogger.logger.Log(NLog.LogLevel.Error, message);
      return message;
    }

    public static void LogDebug(string message)
    {
      SaveGMMValuesLogger.logger.Log(NLog.LogLevel.Debug, message);
    }

    public static void LogError(MessageCodes messageCode)
    {
      SaveGMMValuesLogger.logger.Log(NLog.LogLevel.Error, "Error code: " + messageCode.ToString());
    }

    public static void LogException(string messageCode)
    {
      SaveGMMValuesLogger.logger.Log(NLog.LogLevel.Error, "Error code: " + messageCode);
    }
  }
}
