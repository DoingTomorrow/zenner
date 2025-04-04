// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.ExceptionEntry
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class ExceptionEntry : IEquatable<ExceptionEntry>
  {
    internal TimeSpan Time { get; set; }

    internal string ExtraInfo { get; set; }

    internal string Type { get; set; }

    internal string Message { get; set; }

    internal string StackTrace { get; set; }

    internal ExceptionEntry InnerException { get; set; }

    internal bool CustomStacktrace { get; set; }

    private ExceptionEntry()
    {
    }

    internal ExceptionEntry(
      TimeSpan time,
      string extraInfo,
      string type,
      string message,
      string stackTrace,
      ExceptionEntry innerException,
      bool isCustomStacktrace)
    {
      this.Time = time;
      this.ExtraInfo = extraInfo;
      this.Type = type;
      this.Message = message;
      this.StackTrace = stackTrace;
      this.InnerException = innerException;
      this.CustomStacktrace = isCustomStacktrace;
    }

    internal static ExceptionEntry Create(
      TimeSpan time,
      Exception ex,
      string extraInfo,
      SettingsRestrictions sizeRestrictions,
      ILogAnalyticsMonitor log)
    {
      return ex == null ? (ExceptionEntry) null : ExceptionEntry.CreateInternal(time, ex, extraInfo, sizeRestrictions, 0, log);
    }

    private static ExceptionEntry CreateInternal(
      TimeSpan time,
      Exception ex,
      string extraInfo,
      SettingsRestrictions sizeRestrictions,
      int level,
      ILogAnalyticsMonitor log)
    {
      ExceptionEntry exceptionEntry = new ExceptionEntry();
      try
      {
        exceptionEntry.Time = time;
        exceptionEntry.ExtraInfo = StringUtil.Chop(extraInfo, sizeRestrictions.MaxExceptionExtraInfo.Value);
        try
        {
          exceptionEntry.Type = ex.GetType().FullName;
        }
        catch
        {
          exceptionEntry.Type = "[null]";
        }
        try
        {
          exceptionEntry.Message = StringUtil.Chop(ex.Message, sizeRestrictions.MaxExceptionMessage.Value);
        }
        catch
        {
          exceptionEntry.Message = "[null]";
        }
        try
        {
          exceptionEntry.StackTrace = StringUtil.ChopToEnds(ex.StackTrace, sizeRestrictions.MaxExceptionStackTrace.Value);
        }
        catch
        {
          exceptionEntry.StackTrace = "at [null]";
        }
        try
        {
          exceptionEntry.CustomStacktrace = ExceptionEntry.IsCustomStacktrace(ex);
        }
        catch
        {
          exceptionEntry.CustomStacktrace = false;
        }
        if (ex.InnerException != null)
        {
          if (level < sizeRestrictions.MaxNestedExceptions.Value)
            exceptionEntry.InnerException = ExceptionEntry.CreateInternal(time, ex.InnerException, (string) null, sizeRestrictions, level + 1, log);
        }
      }
      catch (Exception ex1)
      {
        log.LogMessage("Reading of exception failed. Not all exception data (inner exceptions) will be sent to the server. Exception message is " + ex1.Message);
      }
      return exceptionEntry;
    }

    internal static bool IsCustomStacktrace(Exception exception)
    {
      return exception.GetType() != typeof (Exception) && exception.GetType().GetProperty("StackTrace").DeclaringType == exception.GetType();
    }

    internal ExceptionEntry Copy()
    {
      return new ExceptionEntry(this.Time, this.ExtraInfo, this.Type, this.Message, this.StackTrace, this.InnerException != null ? this.InnerException.Copy() : (ExceptionEntry) null, this.CustomStacktrace);
    }

    public bool Equals(ExceptionEntry other) => this.Equals(other, 0);

    private bool Equals(ExceptionEntry other, int level)
    {
      if (level >= 10)
        return true;
      if (other == null || !(this.Time == other.Time) || !(this.ExtraInfo == other.ExtraInfo) || !(this.Type == other.Type) || !(this.Message == other.Message) || !(this.StackTrace == other.StackTrace) || (this.InnerException != null || other.InnerException != null) && (this.InnerException == null || other.InnerException == null))
        return false;
      return this.InnerException == null || this.InnerException.Equals(other.InnerException, level + 1);
    }
  }
}
