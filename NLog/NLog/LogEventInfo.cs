﻿// Decompiled with JetBrains decompiler
// Type: NLog.LogEventInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using NLog.MessageTemplates;
using NLog.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog
{
  public class LogEventInfo
  {
    public static readonly DateTime ZeroDate = DateTime.UtcNow;
    internal static readonly LogMessageFormatter StringFormatMessageFormatter = new LogMessageFormatter(LogEventInfo.GetStringFormatMessageFormatter);
    private static int globalSequenceId;
    private string _formattedMessage;
    private string _message;
    private object[] _parameters;
    private IFormatProvider _formatProvider;
    private LogMessageFormatter _messageFormatter = LogEventInfo.DefaultMessageFormatter;
    private IDictionary<Layout, object> _layoutCache;
    private PropertiesDictionary _properties;

    internal static LogMessageFormatter DefaultMessageFormatter { get; private set; } = LogMessageTemplateFormatter.DefaultAuto.MessageFormatter;

    public LogEventInfo()
    {
      this.TimeStamp = TimeSource.Current.Time;
      this.SequenceID = Interlocked.Increment(ref LogEventInfo.globalSequenceId);
    }

    public LogEventInfo(LogLevel level, string loggerName, [Localizable(false)] string message)
      : this(level, loggerName, (IFormatProvider) null, message, (object[]) null, (Exception) null)
    {
    }

    public LogEventInfo(
      LogLevel level,
      string loggerName,
      [Localizable(false)] string message,
      IList<MessageTemplateParameter> messageTemplateParameters)
      : this(level, loggerName, (IFormatProvider) null, message, (object[]) null, (Exception) null)
    {
      if (messageTemplateParameters == null || messageTemplateParameters.Count <= 0)
        return;
      this._properties = new PropertiesDictionary(messageTemplateParameters);
    }

    public LogEventInfo(
      LogLevel level,
      string loggerName,
      IFormatProvider formatProvider,
      [Localizable(false)] string message,
      object[] parameters)
      : this(level, loggerName, formatProvider, message, parameters, (Exception) null)
    {
    }

    public LogEventInfo(
      LogLevel level,
      string loggerName,
      IFormatProvider formatProvider,
      [Localizable(false)] string message,
      object[] parameters,
      Exception exception)
      : this()
    {
      this.Level = level;
      this.LoggerName = loggerName;
      this.Message = message;
      this.Parameters = parameters;
      this.FormatProvider = formatProvider;
      this.Exception = exception;
      if (!LogEventInfo.NeedToPreformatMessage(parameters))
        return;
      this.CalcFormattedMessage();
    }

    public int SequenceID { get; private set; }

    public DateTime TimeStamp { get; set; }

    public LogLevel Level { get; set; }

    internal CallSiteInformation CallSiteInformation { get; private set; }

    internal CallSiteInformation GetCallSiteInformationInternal()
    {
      return this.CallSiteInformation ?? (this.CallSiteInformation = new CallSiteInformation());
    }

    public bool HasStackTrace => this.CallSiteInformation?.StackTrace != null;

    public StackFrame UserStackFrame => this.CallSiteInformation?.UserStackFrame;

    public int UserStackFrameNumber
    {
      get
      {
        int? frameNumberLegacy = (int?) this.CallSiteInformation?.UserStackFrameNumberLegacy;
        if (frameNumberLegacy.HasValue)
          return frameNumberLegacy.GetValueOrDefault();
        CallSiteInformation callSiteInformation = this.CallSiteInformation;
        return callSiteInformation == null ? 0 : callSiteInformation.UserStackFrameNumber;
      }
    }

    public StackTrace StackTrace => this.CallSiteInformation?.StackTrace;

    public string CallerClassName
    {
      get => this.CallSiteInformation?.GetCallerClassName((MethodBase) null, true, true, true);
    }

    public string CallerMemberName
    {
      get => this.CallSiteInformation?.GetCallerMemberName((MethodBase) null, false, true, true);
    }

    public string CallerFilePath => this.CallSiteInformation?.GetCallerFilePath(0);

    public int CallerLineNumber
    {
      get
      {
        CallSiteInformation callSiteInformation = this.CallSiteInformation;
        return callSiteInformation == null ? 0 : callSiteInformation.GetCallerLineNumber(0);
      }
    }

    public Exception Exception { get; set; }

    public string LoggerName { get; set; }

    [Obsolete("This property should not be used. Marked obsolete on NLog 2.0")]
    public string LoggerShortName
    {
      get
      {
        int num = this.LoggerName.LastIndexOf('.');
        return num >= 0 ? this.LoggerName.Substring(num + 1) : this.LoggerName;
      }
    }

    public string Message
    {
      get => this._message;
      set
      {
        bool rebuildMessageTemplateParameters = this.ResetMessageTemplateParameters();
        this._message = value;
        this.ResetFormattedMessage(rebuildMessageTemplateParameters);
      }
    }

    public object[] Parameters
    {
      get => this._parameters;
      set
      {
        bool rebuildMessageTemplateParameters = this.ResetMessageTemplateParameters();
        this._parameters = value;
        this.ResetFormattedMessage(rebuildMessageTemplateParameters);
      }
    }

    public IFormatProvider FormatProvider
    {
      get => this._formatProvider;
      set
      {
        if (this._formatProvider == value)
          return;
        this._formatProvider = value;
        this.ResetFormattedMessage(false);
      }
    }

    public LogMessageFormatter MessageFormatter
    {
      get => this._messageFormatter;
      set
      {
        this._messageFormatter = value ?? LogEventInfo.StringFormatMessageFormatter;
        this.ResetFormattedMessage(false);
      }
    }

    public string FormattedMessage
    {
      get
      {
        if (this._formattedMessage == null)
          this.CalcFormattedMessage();
        return this._formattedMessage;
      }
    }

    public bool HasProperties
    {
      get
      {
        return this._properties != null ? this._properties.Count > 0 : this.HasMessageTemplateParameters;
      }
    }

    public IDictionary<object, object> Properties
    {
      get => (IDictionary<object, object>) this.CreateOrUpdatePropertiesInternal();
    }

    internal PropertiesDictionary CreateOrUpdatePropertiesInternal(
      bool forceCreate = true,
      IList<MessageTemplateParameter> templateParameters = null)
    {
      PropertiesDictionary properties = this._properties;
      if (properties == null)
      {
        if (forceCreate || templateParameters != null && templateParameters.Count > 0)
        {
          Interlocked.CompareExchange<PropertiesDictionary>(ref this._properties, new PropertiesDictionary(templateParameters), (PropertiesDictionary) null);
          if (forceCreate && this.HasMessageTemplateParameters)
            this.CalcFormattedMessage();
        }
      }
      else if (templateParameters != null)
        properties.MessageProperties = templateParameters;
      return this._properties;
    }

    internal bool HasMessageTemplateParameters
    {
      get
      {
        return this._messageFormatter?.Target is ILogMessageFormatter target && target.HasProperties(this);
      }
    }

    public MessageTemplateParameters MessageTemplateParameters
    {
      get
      {
        return this._properties != null && this._properties.MessageProperties.Count > 0 ? new MessageTemplateParameters(this._properties.MessageProperties, this._message, this._parameters) : new MessageTemplateParameters(this._message, this._parameters);
      }
    }

    [Obsolete("Use LogEventInfo.Properties instead.  Marked obsolete on NLog 2.0", true)]
    public IDictionary Context => this.CreateOrUpdatePropertiesInternal().EventContext;

    public static LogEventInfo CreateNullEvent()
    {
      return new LogEventInfo(LogLevel.Off, string.Empty, string.Empty);
    }

    public static LogEventInfo Create(LogLevel logLevel, string loggerName, [Localizable(false)] string message)
    {
      return new LogEventInfo(logLevel, loggerName, (IFormatProvider) null, message, (object[]) null);
    }

    public static LogEventInfo Create(
      LogLevel logLevel,
      string loggerName,
      IFormatProvider formatProvider,
      [Localizable(false)] string message,
      object[] parameters)
    {
      return new LogEventInfo(logLevel, loggerName, formatProvider, message, parameters);
    }

    public static LogEventInfo Create(
      LogLevel logLevel,
      string loggerName,
      IFormatProvider formatProvider,
      object message)
    {
      return new LogEventInfo(logLevel, loggerName, formatProvider, "{0}", new object[1]
      {
        message
      });
    }

    [Obsolete("use Create(LogLevel logLevel, string loggerName, Exception exception, IFormatProvider formatProvider, string message) instead. Marked obsolete before v4.3.11")]
    public static LogEventInfo Create(
      LogLevel logLevel,
      string loggerName,
      [Localizable(false)] string message,
      Exception exception)
    {
      return new LogEventInfo(logLevel, loggerName, (IFormatProvider) null, message, (object[]) null, exception);
    }

    public static LogEventInfo Create(
      LogLevel logLevel,
      string loggerName,
      Exception exception,
      IFormatProvider formatProvider,
      [Localizable(false)] string message)
    {
      return LogEventInfo.Create(logLevel, loggerName, exception, formatProvider, message, (object[]) null);
    }

    public static LogEventInfo Create(
      LogLevel logLevel,
      string loggerName,
      Exception exception,
      IFormatProvider formatProvider,
      [Localizable(false)] string message,
      object[] parameters)
    {
      return new LogEventInfo(logLevel, loggerName, formatProvider, message, parameters, exception);
    }

    public AsyncLogEventInfo WithContinuation(AsyncContinuation asyncContinuation)
    {
      return new AsyncLogEventInfo(this, asyncContinuation);
    }

    public override string ToString()
    {
      return string.Format("Log Event: Logger='{0}' Level={1} Message='{2}' SequenceID={3}", (object) this.LoggerName, (object) this.Level, (object) this.FormattedMessage, (object) this.SequenceID);
    }

    public void SetStackTrace(StackTrace stackTrace, int userStackFrame)
    {
      this.GetCallSiteInformationInternal().SetStackTrace(stackTrace, userStackFrame, new int?());
    }

    public void SetCallerInfo(
      string callerClassName,
      string callerMemberName,
      string callerFilePath,
      int callerLineNumber)
    {
      this.GetCallSiteInformationInternal().SetCallerInfo(callerClassName, callerMemberName, callerFilePath, callerLineNumber);
    }

    internal void AddCachedLayoutValue(Layout layout, object value)
    {
      if (this._layoutCache == null)
        Interlocked.CompareExchange<IDictionary<Layout, object>>(ref this._layoutCache, (IDictionary<Layout, object>) new Dictionary<Layout, object>(), (IDictionary<Layout, object>) null);
      lock (this._layoutCache)
        this._layoutCache[layout] = value;
    }

    internal bool TryGetCachedLayoutValue(Layout layout, out object value)
    {
      if (this._layoutCache == null)
      {
        value = (object) null;
        return false;
      }
      lock (this._layoutCache)
      {
        if (this._layoutCache.Count != 0)
          return this._layoutCache.TryGetValue(layout, out value);
        value = (object) null;
        return false;
      }
    }

    private static bool NeedToPreformatMessage(object[] parameters)
    {
      return parameters != null && parameters.Length != 0 && (parameters.Length > 3 || !LogEventInfo.IsSafeToDeferFormatting(parameters[0]) || parameters.Length >= 2 && !LogEventInfo.IsSafeToDeferFormatting(parameters[1]) || parameters.Length >= 3 && !LogEventInfo.IsSafeToDeferFormatting(parameters[2]));
    }

    private static bool IsSafeToDeferFormatting(object value)
    {
      return value == null || Convert.GetTypeCode(value) != TypeCode.Object;
    }

    private static string GetStringFormatMessageFormatter(LogEventInfo logEvent)
    {
      return logEvent.Parameters == null || logEvent.Parameters.Length == 0 ? logEvent.Message : string.Format(logEvent.FormatProvider ?? (IFormatProvider) CultureInfo.CurrentCulture, logEvent.Message, logEvent.Parameters);
    }

    private void CalcFormattedMessage()
    {
      try
      {
        this._formattedMessage = this._messageFormatter(this);
      }
      catch (Exception ex)
      {
        this._formattedMessage = this.Message;
        InternalLogger.Warn(ex, "Error when formatting a message.");
        if (!ex.MustBeRethrown())
          return;
        throw;
      }
    }

    internal void AppendFormattedMessage(
      ILogMessageFormatter messageFormatter,
      StringBuilder builder)
    {
      if (this._formattedMessage != null)
      {
        builder.Append(this._formattedMessage);
      }
      else
      {
        int length = builder.Length;
        try
        {
          messageFormatter.AppendFormattedMessage(this, builder);
        }
        catch (Exception ex)
        {
          builder.Length = length;
          builder.Append(this._message ?? string.Empty);
          InternalLogger.Warn(ex, "Error when formatting a message.");
          if (!ex.MustBeRethrown())
            return;
          throw;
        }
      }
    }

    private void ResetFormattedMessage(bool rebuildMessageTemplateParameters)
    {
      this._formattedMessage = (string) null;
      if (!rebuildMessageTemplateParameters || !this.HasMessageTemplateParameters)
        return;
      this.CalcFormattedMessage();
    }

    private bool ResetMessageTemplateParameters()
    {
      if (this._properties == null || !this.HasMessageTemplateParameters)
        return false;
      this._properties.MessageProperties = (IList<MessageTemplateParameter>) null;
      return true;
    }

    internal static void SetDefaultMessageFormatter(bool? mode)
    {
      bool? nullable1 = mode;
      bool flag1 = true;
      if ((nullable1.GetValueOrDefault() == flag1 ? (nullable1.HasValue ? 1 : 0) : 0) != 0)
      {
        InternalLogger.Info("Message Template Format always enabled");
        LogEventInfo.DefaultMessageFormatter = LogMessageTemplateFormatter.Default.MessageFormatter;
      }
      else
      {
        bool? nullable2 = mode;
        bool flag2 = false;
        if ((nullable2.GetValueOrDefault() == flag2 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
        {
          InternalLogger.Info("Message Template String Format always enabled");
          LogEventInfo.DefaultMessageFormatter = LogEventInfo.StringFormatMessageFormatter;
        }
        else
        {
          InternalLogger.Info("Message Template Auto Format enabled");
          LogEventInfo.DefaultMessageFormatter = LogMessageTemplateFormatter.DefaultAuto.MessageFormatter;
        }
      }
    }
  }
}
