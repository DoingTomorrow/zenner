// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ZR_ClassLibMessages
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Threading;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ZR_ClassLibMessages
  {
    internal static ResourceManager ZR_ClassMessage = new ResourceManager("ZR_ClassLibrary.ZR_ClassLibMessages", typeof (ZR_ClassLibMessages).Assembly);
    public static SortedList<int, ZR_ClassLibMessages> ThreadErrorMsgLists;
    private ZR_ClassLibMessages.LastErrors LastError = ZR_ClassLibMessages.LastErrors.NoError;
    private StringBuilder ErrorDescription = new StringBuilder(200);
    private StringBuilder Warnings = new StringBuilder(200);
    private StringBuilder Infos = new StringBuilder(200);
    private StringBuilder UnhandledData = new StringBuilder();
    private Exception LastException;

    public static void RegisterThreadErrorMsgList()
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.ThreadErrorMsgLists = new SortedList<int, ZR_ClassLibMessages>();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        int managedThreadId = Thread.CurrentThread.ManagedThreadId;
        if (ZR_ClassLibMessages.ThreadErrorMsgLists.ContainsKey(managedThreadId))
          return;
        ZR_ClassLibMessages.ThreadErrorMsgLists.Add(managedThreadId, new ZR_ClassLibMessages());
      }
    }

    public static void DeRegisterThreadErrorMsgList()
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        return;
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        int managedThreadId = Thread.CurrentThread.ManagedThreadId;
        if (ZR_ClassLibMessages.ThreadErrorMsgLists.ContainsKey(managedThreadId))
          return;
        ZR_ClassLibMessages.ThreadErrorMsgLists.Remove(managedThreadId);
      }
    }

    private static ZR_ClassLibMessages GetMessageList()
    {
      return ZR_ClassLibMessages.GetMessageList(Thread.CurrentThread.ManagedThreadId);
    }

    private static ZR_ClassLibMessages GetMessageList(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      if (!ZR_ClassLibMessages.ThreadErrorMsgLists.ContainsKey(ThreadId))
        ZR_ClassLibMessages.ThreadErrorMsgLists.Add(ThreadId, new ZR_ClassLibMessages());
      int index = ZR_ClassLibMessages.ThreadErrorMsgLists.IndexOfKey(ThreadId);
      if (index < 0)
        throw new Exception("ZR_ClassLibMessages: Thread error handler not available!");
      return ZR_ClassLibMessages.ThreadErrorMsgLists.Values[index];
    }

    public static void ClearErrors()
    {
      ZR_ClassLibMessages.ClearErrors(Thread.CurrentThread.ManagedThreadId);
    }

    public static void ClearErrors(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        messageList.LastError = ZR_ClassLibMessages.LastErrors.NoError;
        messageList.ErrorDescription.Length = 0;
        messageList.Warnings.Length = 0;
        messageList.Infos.Length = 0;
        messageList.UnhandledData.Length = 0;
        messageList.LastException = (Exception) null;
      }
    }

    public static void ClearErrorText()
    {
      ZR_ClassLibMessages.ClearErrorText(Thread.CurrentThread.ManagedThreadId);
    }

    public static void ClearErrorText(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        messageList.LastError = ZR_ClassLibMessages.LastErrors.NoError;
        messageList.ErrorDescription.Length = 0;
      }
    }

    public static void ClearErrorWarnings()
    {
      ZR_ClassLibMessages.ClearErrorWarnings(Thread.CurrentThread.ManagedThreadId);
    }

    public static void ClearErrorWarnings(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        messageList.Warnings.Length = 0;
        messageList.UnhandledData.Length = 0;
      }
    }

    public static void ClearErrorInfo()
    {
      ZR_ClassLibMessages.ClearErrorInfo(Thread.CurrentThread.ManagedThreadId);
    }

    public static void ClearErrorInfo(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
        ZR_ClassLibMessages.GetMessageList(ThreadId).Infos.Length = 0;
    }

    public static void AddErrorDescriptionAndException(
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription,
      Logger theLogger)
    {
      theLogger.Error(TheDescription);
      ZR_ClassLibMessages.AddErrorDescriptionAndException(Thread.CurrentThread.ManagedThreadId, TheError, TheDescription);
    }

    public static void AddErrorDescriptionAndException(
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription)
    {
      ZR_ClassLibMessages.AddErrorDescriptionAndException(Thread.CurrentThread.ManagedThreadId, TheError, TheDescription);
    }

    public static void AddErrorDescriptionAndException(
      int ThreadId,
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription)
    {
      ZR_ClassLibMessages.AddErrorDescription(ThreadId, TheError, TheDescription);
      throw new Exception(TheDescription);
    }

    public static bool AddErrorDescription(ZR_ClassLibMessages.LastErrors TheError)
    {
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, TheError);
    }

    public static bool AddErrorDescription(int ThreadId, ZR_ClassLibMessages.LastErrors TheError)
    {
      ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        messageList.LastError = TheError;
        if (messageList.ErrorDescription.Length > 0)
          messageList.ErrorDescription.Insert(0, TheError.ToString() + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine);
        else
          messageList.ErrorDescription.Append(TheError.ToString());
      }
      return false;
    }

    public static bool AddErrorDescription(string TheDescription, Logger theLogger)
    {
      theLogger.Error(TheDescription);
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, TheDescription);
    }

    public static bool AddErrorDescription(string TheDescription)
    {
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, TheDescription);
    }

    public static bool AddErrorDescription(Exception ex)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Exception:");
      stringBuilder.AppendLine(ex.ToString());
      for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
      {
        stringBuilder.AppendLine("Inner exception:");
        stringBuilder.AppendLine(innerException.ToString());
      }
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, stringBuilder.ToString());
    }

    public static bool AddErrorDescription(int ThreadId, string TheDescription)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (messageList.ErrorDescription.Length > 0)
        {
          if (messageList.ErrorDescription.ToString().IndexOf(TheDescription) >= 0)
            return false;
          messageList.ErrorDescription.Insert(0, TheDescription + ZR_Constants.SystemNewLine);
        }
        else
          messageList.ErrorDescription.Append(TheDescription);
      }
      return false;
    }

    public static bool AddErrorDescription(
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription,
      Logger theLogger)
    {
      theLogger.Error(TheDescription);
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, TheError, TheDescription);
    }

    public static bool AddErrorDescription(
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription)
    {
      return ZR_ClassLibMessages.AddErrorDescription(Thread.CurrentThread.ManagedThreadId, TheError, TheDescription);
    }

    public static bool AddErrorDescription(
      int ThreadId,
      ZR_ClassLibMessages.LastErrors TheError,
      string TheDescription)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (messageList.LastError != ZR_ClassLibMessages.LastErrors.NoError && messageList.LastError != TheError)
        {
          messageList.ErrorDescription.Append(ZR_Constants.SystemNewLine);
          messageList.ErrorDescription.Append("LastError changed from: '" + ZR_ClassLibMessages.ZR_ClassMessage.GetString(messageList.LastError.ToString()));
          messageList.LastError = TheError;
          messageList.ErrorDescription.Append("' to: '" + ZR_ClassLibMessages.ZR_ClassMessage.GetString(messageList.LastError.ToString()));
          messageList.ErrorDescription.Append("'" + ZR_Constants.SystemNewLine);
        }
        else
          messageList.LastError = TheError;
        if (messageList.ErrorDescription.Length > 0)
        {
          if (messageList.ErrorDescription.ToString().IndexOf(TheDescription) >= 0)
            return false;
          messageList.ErrorDescription.Insert(0, TheDescription + ZR_Constants.SystemNewLine + ZR_Constants.SystemNewLine);
        }
        else
          messageList.ErrorDescription.Append(TheDescription);
      }
      return false;
    }

    public static void AddWarning(string warning)
    {
      ZR_ClassLibMessages.AddWarning(Thread.CurrentThread.ManagedThreadId, warning, (string) null, (Logger) null);
    }

    public static void AddWarning(string warning, Logger theLogger)
    {
      ZR_ClassLibMessages.AddWarning(Thread.CurrentThread.ManagedThreadId, warning, (string) null, theLogger);
    }

    public static void AddWarning(int ThreadId, string warning)
    {
      ZR_ClassLibMessages.AddWarning(ThreadId, warning, (string) null, (Logger) null);
    }

    public static void AddWarning(int ThreadId, string warning, Logger theLogger)
    {
      ZR_ClassLibMessages.AddWarning(ThreadId, warning, (string) null, theLogger);
    }

    public static void AddWarning(string warning, string unhandledData)
    {
      ZR_ClassLibMessages.AddWarning(Thread.CurrentThread.ManagedThreadId, warning, unhandledData, (Logger) null);
    }

    public static void AddWarning(int ThreadId, string warning, string unhandledData)
    {
      ZR_ClassLibMessages.AddWarning(ThreadId, warning, unhandledData, (Logger) null);
    }

    public static void AddWarning(
      int ThreadId,
      string warning,
      string unhandledData,
      Logger theLogger)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        if (theLogger != null)
        {
          theLogger.Info(warning);
          if (!string.IsNullOrEmpty(unhandledData))
            theLogger.Info("unhandeld: " + unhandledData);
        }
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (!string.IsNullOrEmpty(unhandledData))
        {
          messageList.UnhandledData.Append(unhandledData + ZR_Constants.SystemNewLine);
          messageList.LastError = ZR_ClassLibMessages.LastErrors.UnhandledData;
        }
        if (messageList.Warnings.Length > 0)
          messageList.Warnings.Insert(0, warning + ZR_Constants.SystemNewLine);
        else
          messageList.Warnings.Append(warning);
      }
    }

    public static void AddException(Exception exc)
    {
      ZR_ClassLibMessages.AddException(Thread.CurrentThread.ManagedThreadId, exc);
    }

    public static void AddException(int ThreadId, Exception exc)
    {
      string exceptionMessages = ZR_ClassLibMessages.GetAggregateExceptionMessages(exc);
      ZR_ClassLibMessages.AddErrorDescription(ThreadId, exceptionMessages);
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        messageList.LastException = exc;
        messageList.LastError = ZR_ClassLibMessages.LastErrors.Exception;
      }
    }

    private static string GetAggregateExceptionMessages(Exception exc)
    {
      if (!(exc is AggregateException))
        return exc.Message + Environment.NewLine;
      AggregateException aggregateException = (AggregateException) exc;
      string empty = string.Empty;
      for (int index = 0; index < aggregateException.InnerExceptions.Count; ++index)
        empty += ZR_ClassLibMessages.GetAggregateExceptionMessages(aggregateException.InnerExceptions[index]);
      return empty;
    }

    public static void AddInfo(string Info)
    {
      ZR_ClassLibMessages.AddInfo(Thread.CurrentThread.ManagedThreadId, Info);
    }

    public static void AddInfo(int ThreadId, string Info)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (messageList.Infos.Length > 0)
          messageList.Infos.Insert(0, Info + ZR_Constants.SystemNewLine);
        else
          messageList.Infos.Append(Info);
      }
    }

    public static ZR_ClassLibMessages.LastErrors GetLastError()
    {
      return ZR_ClassLibMessages.GetLastError(Thread.CurrentThread.ManagedThreadId);
    }

    public static ZR_ClassLibMessages.LastErrors GetLastError(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
        return ZR_ClassLibMessages.GetMessageList(ThreadId).LastError;
    }

    public static string GetLastErrorStringTranslated()
    {
      return ZR_ClassLibMessages.GetLastErrorStringTranslated(Thread.CurrentThread.ManagedThreadId);
    }

    public static string GetLastErrorStringTranslated(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        return ZR_ClassLibMessages.ZR_ClassMessage.GetString(messageList.LastError.ToString());
      }
    }

    public static string GetLastErrorMessageAndClearError()
    {
      return ZR_ClassLibMessages.GetLastErrorMessageAndClearError(Thread.CurrentThread.ManagedThreadId);
    }

    public static string GetLastErrorMessageAndClearError(int ThreadId)
    {
      string errorText = ZR_ClassLibMessages.GetErrorText(ThreadId);
      ZR_ClassLibMessages.ClearErrors(ThreadId);
      return errorText;
    }

    public static ZR_ClassLibMessages.LastErrorInfo GetLastErrorAndClearError()
    {
      return ZR_ClassLibMessages.GetLastErrorAndClearError(Thread.CurrentThread.ManagedThreadId);
    }

    public static ZR_ClassLibMessages.LastErrorInfo GetLastErrorAndClearError(int ThreadId)
    {
      ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo(ThreadId);
      ZR_ClassLibMessages.ClearErrors(ThreadId);
      return lastErrorInfo;
    }

    public static ZR_ClassLibMessages.LastErrorInfo GetLastErrorInfo()
    {
      return ZR_ClassLibMessages.GetLastErrorInfo(Thread.CurrentThread.ManagedThreadId);
    }

    public static ZR_ClassLibMessages.LastErrorInfo GetLastErrorInfo(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        return new ZR_ClassLibMessages.LastErrorInfo(messageList.LastError, messageList.ErrorDescription.ToString(), messageList.Warnings.ToString(), messageList.Infos.ToString(), messageList.UnhandledData.ToString(), messageList.LastException);
      }
    }

    private static string GetErrorText()
    {
      return ZR_ClassLibMessages.GetErrorText(Thread.CurrentThread.ManagedThreadId);
    }

    private static string GetErrorText(int ThreadId)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (messageList.LastError == ZR_ClassLibMessages.LastErrors.NoError && messageList.Warnings.Length == 0 && messageList.Infos.Length == 0)
          return string.Empty;
      }
      StringBuilder stringBuilder = new StringBuilder();
      string str = " -------------------- ";
      ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo(ThreadId);
      if (lastErrorInfo.LastError != 0)
      {
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_ClassLibMessages.ZR_ClassMessage.GetString("LastError"));
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(lastErrorInfo.LastErrorAsTranslatedString + ZR_Constants.SystemNewLine);
      }
      if (lastErrorInfo.LastErrorDescription.Length > 0)
      {
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_ClassLibMessages.ZR_ClassMessage.GetString("LastErrorDescription"));
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(lastErrorInfo.LastErrorDescription);
      }
      if (lastErrorInfo.LastWarnings.Length > 0)
      {
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_ClassLibMessages.ZR_ClassMessage.GetString("LastWarnings"));
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(lastErrorInfo.LastWarnings);
      }
      if (lastErrorInfo.LastInfos.Length > 0)
      {
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_ClassLibMessages.ZR_ClassMessage.GetString("LastInfos"));
        stringBuilder.Append(str);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
        stringBuilder.Append(lastErrorInfo.LastInfos);
      }
      return stringBuilder.ToString();
    }

    public static void ShowAndClearErrors()
    {
      ZR_ClassLibMessages.ShowAndClearErrors(Thread.CurrentThread.ManagedThreadId);
    }

    public static void ShowAndClearErrors(int ThreadId)
    {
      ZR_ClassLibMessages.ShowAndClearErrors(ThreadId, string.Empty, string.Empty);
    }

    public static void ShowAndClearErrors(string Caption)
    {
      ZR_ClassLibMessages.ShowAndClearErrors(Thread.CurrentThread.ManagedThreadId, Caption, string.Empty);
    }

    public static void ShowAndClearErrors(int ThreadId, string Caption)
    {
      ZR_ClassLibMessages.ShowAndClearErrors(ThreadId, Caption, string.Empty);
    }

    public static void ShowAndClearErrors(string Caption, string FirstLineText)
    {
      ZR_ClassLibMessages.ShowAndClearErrors(Thread.CurrentThread.ManagedThreadId, Caption, FirstLineText);
    }

    public static void ShowAndClearErrors(int ThreadId, string Caption, string FirstLineText)
    {
      if (ZR_ClassLibMessages.ThreadErrorMsgLists == null)
        ZR_ClassLibMessages.RegisterThreadErrorMsgList();
      lock (ZR_ClassLibMessages.ThreadErrorMsgLists)
      {
        ZR_ClassLibMessages messageList = ZR_ClassLibMessages.GetMessageList(ThreadId);
        if (messageList.LastError == ZR_ClassLibMessages.LastErrors.NoError && messageList.Warnings.Length == 0 && messageList.Infos.Length == 0)
          return;
      }
      StringBuilder stringBuilder = new StringBuilder();
      if (FirstLineText != null && FirstLineText.Length > 0)
      {
        stringBuilder.Append(FirstLineText + ZR_Constants.SystemNewLine);
        stringBuilder.Append(ZR_Constants.SystemNewLine);
      }
      stringBuilder.Append(ZR_ClassLibMessages.GetErrorText(ThreadId));
      ZR_ClassLibMessages.ClearErrors();
      int num = (int) GMM_MessageBox.ShowMessage(Caption, stringBuilder.ToString(), true);
    }

    public static void ShowException(string headerText, Exception ex)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int index = 0;
      StringBuilder stringBuilder2 = new StringBuilder();
      if (!string.IsNullOrEmpty(headerText))
      {
        stringBuilder2.AppendLine(headerText);
        stringBuilder2.AppendLine("==========================================");
        index = stringBuilder2.Length;
        stringBuilder2.AppendLine();
      }
      string messageAndClearError = ZR_ClassLibMessages.GetLastErrorMessageAndClearError();
      if (!string.IsNullOrEmpty(messageAndClearError))
      {
        stringBuilder2.AppendLine("GMM message:");
        stringBuilder2.AppendLine(messageAndClearError);
        stringBuilder2.AppendLine("==========================================");
        stringBuilder2.AppendLine();
      }
      Exception exception = ex;
      while (exception != null)
      {
        int? fromLanguageText = Ot.GetMessageNumberFromLanguageText(exception.Message);
        if (fromLanguageText.HasValue)
        {
          if (stringBuilder1.Length > 0)
            stringBuilder1.Append(';');
          stringBuilder1.Append(fromLanguageText.ToString());
        }
        stringBuilder2.AppendLine(exception.Message);
        if (exception.InnerException != null)
        {
          exception = exception.InnerException;
          stringBuilder2.AppendLine("------------------------------------------");
          stringBuilder2.AppendLine();
        }
        else
          exception = (Exception) null;
      }
      if (stringBuilder1.Length > 0)
        stringBuilder2.Insert(index, "Message tree: " + stringBuilder1.ToString() + Environment.NewLine);
      stringBuilder2.AppendLine();
      stringBuilder2.AppendLine("++++++++++++++++++++++++++++++++++++++++++");
      stringBuilder2.AppendLine(ex.ToString());
      int num = (int) GMM_MessageBox.ShowMessage("Exception", stringBuilder2.ToString(), true);
    }

    public enum LastErrors
    {
      NoError,
      Timeout,
      FunctionNotImplemented,
      NoPermission,
      InternalError,
      LoadComponentError,
      DeviceNotFound,
      CommunicationError,
      FramingError,
      MissingData,
      OperationCancelled,
      IllegalData,
      ComOpenError,
      UnhandledData,
      DatabaseError,
      NAK_Received,
      Exception,
      TimeoutReceiveIncomplete,
    }

    public class LastErrorInfo
    {
      public readonly ZR_ClassLibMessages.LastErrors LastError;
      public readonly string LastErrorAsTranslatedString;
      public readonly string LastErrorDescription;
      public readonly string LastWarnings;
      public readonly string LastInfos;
      public readonly string LastUnhandledData;
      public readonly Exception LastErrorException;

      public LastErrorInfo(
        ZR_ClassLibMessages.LastErrors LastError,
        string LastErrorDescription,
        string LastWarnings,
        string LastInfos,
        string LastUnhandledData,
        Exception LastErrorException)
      {
        this.LastError = LastError;
        this.LastErrorAsTranslatedString = ZR_ClassLibMessages.ZR_ClassMessage.GetString(LastError.ToString());
        this.LastErrorDescription = LastErrorDescription;
        this.LastWarnings = LastWarnings;
        this.LastInfos = LastInfos;
        this.LastUnhandledData = LastUnhandledData;
        this.LastErrorException = LastErrorException;
      }
    }
  }
}
