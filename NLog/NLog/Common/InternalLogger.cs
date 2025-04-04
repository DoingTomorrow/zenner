// Decompiled with JetBrains decompiler
// Type: NLog.Common.InternalLogger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using JetBrains.Annotations;
using NLog.Internal;
using NLog.Time;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Common
{
  public static class InternalLogger
  {
    private static readonly object LockObject = new object();
    private static string _logFile;

    public static bool IsTraceEnabled => NLog.LogLevel.Trace >= InternalLogger.LogLevel;

    public static bool IsDebugEnabled => NLog.LogLevel.Debug >= InternalLogger.LogLevel;

    public static bool IsInfoEnabled => NLog.LogLevel.Info >= InternalLogger.LogLevel;

    public static bool IsWarnEnabled => NLog.LogLevel.Warn >= InternalLogger.LogLevel;

    public static bool IsErrorEnabled => NLog.LogLevel.Error >= InternalLogger.LogLevel;

    public static bool IsFatalEnabled => NLog.LogLevel.Fatal >= InternalLogger.LogLevel;

    [MessageTemplateFormatMethod("message")]
    public static void Trace([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, message, args);
    }

    public static void Trace([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, message, (object[]) null);
    }

    public static void Trace([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsTraceEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Trace(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Trace, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Trace<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsTraceEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Trace<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsTraceEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Trace<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsTraceEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Trace(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Trace, message, (object[]) null);
    }

    public static void Trace(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsTraceEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Trace, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Debug([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, message, args);
    }

    public static void Debug([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, message, (object[]) null);
    }

    public static void Debug([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Debug(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Debug, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Debug<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Debug<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Debug<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Debug(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Debug, message, (object[]) null);
    }

    public static void Debug(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsDebugEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Debug, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Info([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Info, message, args);
    }

    public static void Info([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Info, message, (object[]) null);
    }

    public static void Info([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsInfoEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Info, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Info(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Info, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Info<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsInfoEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Info<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsInfoEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Info<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsInfoEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Info(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Info, message, (object[]) null);
    }

    public static void Info(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsInfoEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Info, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Warn([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, message, args);
    }

    public static void Warn([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, message, (object[]) null);
    }

    public static void Warn([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsWarnEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Warn(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Warn, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Warn<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsWarnEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Warn<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsWarnEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Warn<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsWarnEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Warn(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Warn, message, (object[]) null);
    }

    public static void Warn(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsWarnEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Warn, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Error([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Error, message, args);
    }

    public static void Error([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Error, message, (object[]) null);
    }

    public static void Error([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsErrorEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Error, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Error(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Error, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Error<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsErrorEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Error<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsErrorEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Error<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsErrorEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Error(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Error, message, (object[]) null);
    }

    public static void Error(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsErrorEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Error, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Fatal([Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, message, args);
    }

    public static void Fatal([Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, message, (object[]) null);
    }

    public static void Fatal([Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsFatalEnabled)
        return;
      InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, messageFunc(), (object[]) null);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Fatal(Exception ex, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Fatal, message, args);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Fatal<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
    {
      if (!InternalLogger.IsFatalEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Fatal<TArgument1, TArgument2>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1)
    {
      if (!InternalLogger.IsFatalEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0, (object) arg1);
    }

    [MessageTemplateFormatMethod("message")]
    public static void Fatal<TArgument1, TArgument2, TArgument3>(
      [Localizable(false)] string message,
      TArgument1 arg0,
      TArgument2 arg1,
      TArgument3 arg2)
    {
      if (!InternalLogger.IsFatalEnabled)
        return;
      InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0, (object) arg1, (object) arg2);
    }

    public static void Fatal(Exception ex, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, NLog.LogLevel.Fatal, message, (object[]) null);
    }

    public static void Fatal(Exception ex, [Localizable(false)] Func<string> messageFunc)
    {
      if (!InternalLogger.IsFatalEnabled)
        return;
      InternalLogger.Write(ex, NLog.LogLevel.Fatal, messageFunc(), (object[]) null);
    }

    static InternalLogger() => InternalLogger.Reset();

    public static void Reset()
    {
      InternalLogger.LogToConsole = InternalLogger.GetSetting<bool>("nlog.internalLogToConsole", "NLOG_INTERNAL_LOG_TO_CONSOLE", false);
      InternalLogger.LogToConsoleError = InternalLogger.GetSetting<bool>("nlog.internalLogToConsoleError", "NLOG_INTERNAL_LOG_TO_CONSOLE_ERROR", false);
      InternalLogger.LogLevel = InternalLogger.GetSetting("nlog.internalLogLevel", "NLOG_INTERNAL_LOG_LEVEL", NLog.LogLevel.Info);
      InternalLogger.LogFile = InternalLogger.GetSetting<string>("nlog.internalLogFile", "NLOG_INTERNAL_LOG_FILE", string.Empty);
      InternalLogger.LogToTrace = InternalLogger.GetSetting<bool>("nlog.internalLogToTrace", "NLOG_INTERNAL_LOG_TO_TRACE", false);
      InternalLogger.IncludeTimestamp = InternalLogger.GetSetting<bool>("nlog.internalLogIncludeTimestamp", "NLOG_INTERNAL_INCLUDE_TIMESTAMP", true);
      InternalLogger.Info("NLog internal logger initialized.");
      InternalLogger.ExceptionThrowWhenWriting = false;
      InternalLogger.LogWriter = (TextWriter) null;
    }

    public static NLog.LogLevel LogLevel { get; set; }

    public static bool LogToConsole { get; set; }

    public static bool LogToConsoleError { get; set; }

    public static bool LogToTrace { get; set; }

    public static string LogFile
    {
      get => InternalLogger._logFile;
      set
      {
        InternalLogger._logFile = value;
        if (string.IsNullOrEmpty(InternalLogger._logFile))
          return;
        InternalLogger.CreateDirectoriesIfNeeded(InternalLogger._logFile);
      }
    }

    public static TextWriter LogWriter { get; set; }

    public static bool IncludeTimestamp { get; set; }

    internal static bool ExceptionThrowWhenWriting { get; private set; }

    [StringFormatMethod("message")]
    public static void Log(NLog.LogLevel level, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write((Exception) null, level, message, args);
    }

    public static void Log(NLog.LogLevel level, [Localizable(false)] string message)
    {
      InternalLogger.Write((Exception) null, level, message, (object[]) null);
    }

    public static void Log(NLog.LogLevel level, [Localizable(false)] Func<string> messageFunc)
    {
      if (!(level >= InternalLogger.LogLevel))
        return;
      InternalLogger.Write((Exception) null, level, messageFunc(), (object[]) null);
    }

    [StringFormatMethod("message")]
    public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] Func<string> messageFunc)
    {
      if (!(level >= InternalLogger.LogLevel))
        return;
      InternalLogger.Write(ex, level, messageFunc(), (object[]) null);
    }

    [StringFormatMethod("message")]
    public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] string message, params object[] args)
    {
      InternalLogger.Write(ex, level, message, args);
    }

    public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] string message)
    {
      InternalLogger.Write(ex, level, message, (object[]) null);
    }

    private static void Write([CanBeNull] Exception ex, NLog.LogLevel level, string message, [CanBeNull] object[] args)
    {
      if (InternalLogger.IsSeriousException(ex))
        return;
      if (!InternalLogger.IsLoggingEnabled(level))
        return;
      try
      {
        string message1 = InternalLogger.FormatMessage(ex, level, message, args);
        InternalLogger.WriteToLogFile(message1);
        InternalLogger.WriteToTextWriter(message1);
        InternalLogger.WriteToConsole(message1);
        InternalLogger.WriteToErrorConsole(message1);
        InternalLogger.WriteToTrace(message1);
      }
      catch (Exception ex1)
      {
        InternalLogger.ExceptionThrowWhenWriting = true;
        if (!ex1.MustBeRethrownImmediately())
          return;
        throw;
      }
    }

    private static string FormatMessage(
      [CanBeNull] Exception ex,
      NLog.LogLevel level,
      string message,
      [CanBeNull] object[] args)
    {
      string str = args == null ? message : string.Format((IFormatProvider) CultureInfo.InvariantCulture, message, args);
      StringBuilder stringBuilder = new StringBuilder(str.Length + "yyyy-MM-dd HH:mm:ss.ffff".Length + (ex?.ToString()?.Length ?? 0) + 25);
      if (InternalLogger.IncludeTimestamp)
        stringBuilder.Append(TimeSource.Current.Time.ToString("yyyy-MM-dd HH:mm:ss.ffff", (IFormatProvider) CultureInfo.InvariantCulture)).Append(" ");
      stringBuilder.Append((object) level).Append(" ").Append(str);
      if (ex != null)
      {
        ex.MarkAsLoggedToInternalLogger();
        stringBuilder.Append(" ").Append("Exception: ").Append((object) ex);
      }
      return stringBuilder.ToString();
    }

    private static bool IsSeriousException(Exception exception)
    {
      return exception != null && exception.MustBeRethrownImmediately();
    }

    private static bool IsLoggingEnabled(NLog.LogLevel logLevel)
    {
      if (logLevel == NLog.LogLevel.Off || logLevel < InternalLogger.LogLevel)
        return false;
      return !string.IsNullOrEmpty(InternalLogger.LogFile) || InternalLogger.LogToConsole || InternalLogger.LogToConsoleError || InternalLogger.LogToTrace || InternalLogger.LogWriter != null;
    }

    private static void WriteToLogFile(string message)
    {
      string logFile = InternalLogger.LogFile;
      if (string.IsNullOrEmpty(logFile))
        return;
      lock (InternalLogger.LockObject)
      {
        using (StreamWriter streamWriter = File.AppendText(logFile))
          streamWriter.WriteLine(message);
      }
    }

    private static void WriteToTextWriter(string message)
    {
      TextWriter logWriter = InternalLogger.LogWriter;
      if (logWriter == null)
        return;
      lock (InternalLogger.LockObject)
        logWriter.WriteLine(message);
    }

    private static void WriteToConsole(string message)
    {
      if (!InternalLogger.LogToConsole)
        return;
      lock (InternalLogger.LockObject)
        Console.WriteLine(message);
    }

    private static void WriteToErrorConsole(string message)
    {
      if (!InternalLogger.LogToConsoleError)
        return;
      lock (InternalLogger.LockObject)
        Console.Error.WriteLine(message);
    }

    private static void WriteToTrace(string message)
    {
      if (!InternalLogger.LogToTrace)
        return;
      System.Diagnostics.Trace.WriteLine(message, "NLog");
    }

    public static void LogAssemblyVersion(Assembly assembly)
    {
      try
      {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        InternalLogger.Info<string, string, string>("{0}. File version: {1}. Product version: {2}.", assembly.FullName, versionInfo.FileVersion, versionInfo.ProductVersion);
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) assembly.FullName
        };
        InternalLogger.Error(ex, "Error logging version of assembly {0}.", objArray);
      }
    }

    private static string GetAppSettings(string configName)
    {
      try
      {
        return System.Configuration.ConfigurationManager.AppSettings[configName];
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
      }
      return (string) null;
    }

    private static string GetSettingString(string configName, string envName)
    {
      try
      {
        string appSettings = InternalLogger.GetAppSettings(configName);
        if (appSettings != null)
          return appSettings;
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
      }
      try
      {
        string environmentVariable = EnvironmentHelper.GetSafeEnvironmentVariable(envName);
        if (!string.IsNullOrEmpty(environmentVariable))
          return environmentVariable;
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
      }
      return (string) null;
    }

    private static NLog.LogLevel GetSetting(
      string configName,
      string envName,
      NLog.LogLevel defaultValue)
    {
      string settingString = InternalLogger.GetSettingString(configName, envName);
      if (settingString == null)
        return defaultValue;
      try
      {
        return NLog.LogLevel.FromString(settingString);
      }
      catch (Exception ex)
      {
        if (!ex.MustBeRethrownImmediately())
          return defaultValue;
        throw;
      }
    }

    private static T GetSetting<T>(string configName, string envName, T defaultValue)
    {
      string settingString = InternalLogger.GetSettingString(configName, envName);
      if (settingString == null)
        return defaultValue;
      try
      {
        return (T) Convert.ChangeType((object) settingString, typeof (T), (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (Exception ex)
      {
        if (!ex.MustBeRethrownImmediately())
          return defaultValue;
        throw;
      }
    }

    private static void CreateDirectoriesIfNeeded(string filename)
    {
      try
      {
        if (InternalLogger.LogLevel == NLog.LogLevel.Off)
          return;
        string directoryName = Path.GetDirectoryName(filename);
        if (string.IsNullOrEmpty(directoryName))
          return;
        Directory.CreateDirectory(directoryName);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Cannot create needed directories to '{0}'.", (object) filename);
        if (!ex.MustBeRethrownImmediately())
          return;
        throw;
      }
    }
  }
}
