// Decompiled with JetBrains decompiler
// Type: NLog.LoggerImpl
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Config;
using NLog.Filters;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog
{
  internal static class LoggerImpl
  {
    private const int StackTraceSkipMethods = 0;
    private static readonly Assembly nlogAssembly = typeof (LoggerImpl).GetAssembly();
    private static readonly Assembly mscorlibAssembly = typeof (string).GetAssembly();
    private static readonly Assembly systemAssembly = typeof (Debug).GetAssembly();

    internal static void Write(
      [NotNull] Type loggerType,
      TargetWithFilterChain targets,
      LogEventInfo logEvent,
      LogFactory factory)
    {
      if (targets == null)
        return;
      StackTraceUsage stackTraceUsage = targets.GetStackTraceUsage();
      if (stackTraceUsage != StackTraceUsage.None && !logEvent.HasStackTrace)
      {
        StackTrace stackTrace = new StackTrace(0, stackTraceUsage == StackTraceUsage.WithSource);
        if (stackTrace != null)
        {
          StackFrame[] frames = stackTrace.GetFrames();
          int? methodOnStackTrace = LoggerImpl.FindCallingMethodOnStackTrace(frames, loggerType);
          int? userStackFrameLegacy = methodOnStackTrace.HasValue ? new int?(LoggerImpl.SkipToUserStackFrameLegacy(frames, methodOnStackTrace.Value)) : new int?();
          logEvent.GetCallSiteInformationInternal().SetStackTrace(stackTrace, methodOnStackTrace ?? 0, userStackFrameLegacy);
        }
      }
      AsyncContinuation onException = (AsyncContinuation) (ex => { });
      if (factory.ThrowExceptions)
      {
        int originalThreadId = AsyncHelpers.GetManagedThreadId();
        onException = (AsyncContinuation) (ex =>
        {
          if (ex != null && AsyncHelpers.GetManagedThreadId() == originalThreadId)
            throw new NLogRuntimeException("Exception occurred in NLog", ex);
        });
      }
      if (targets.NextInChain == null && logEvent.Parameters != null && logEvent.Parameters.Length != 0)
      {
        string message = logEvent.Message;
        if ((message != null ? (message.Length < 256 ? 1 : 0) : 0) != 0 && logEvent.MessageFormatter == LogMessageTemplateFormatter.DefaultAuto.MessageFormatter)
          logEvent.MessageFormatter = LogMessageTemplateFormatter.DefaultAutoSingleTarget.MessageFormatter;
      }
      TargetWithFilterChain targetListHead = targets;
      while (targetListHead != null && LoggerImpl.WriteToTargetWithFilterChain(targetListHead, logEvent, onException))
        targetListHead = targetListHead.NextInChain;
    }

    internal static int? FindCallingMethodOnStackTrace(StackFrame[] stackFrames, [NotNull] Type loggerType)
    {
      if (stackFrames == null || stackFrames.Length == 0)
        return new int?();
      int? nullable1 = new int?();
      int? nullable2 = new int?();
      for (int index = 0; index < stackFrames.Length; ++index)
      {
        StackFrame stackFrame = stackFrames[index];
        if (!LoggerImpl.SkipAssembly(stackFrame))
        {
          if (!nullable2.HasValue)
            nullable2 = new int?(index);
          if (LoggerImpl.IsLoggerType(stackFrame, loggerType))
            nullable1 = new int?();
          else if (!nullable1.HasValue)
            nullable1 = new int?(index);
        }
      }
      return nullable1 ?? nullable2;
    }

    internal static int SkipToUserStackFrameLegacy(
      StackFrame[] stackFrames,
      int firstUserStackFrame)
    {
      for (int stackFrameLegacy = firstUserStackFrame; stackFrameLegacy < stackFrames.Length; ++stackFrameLegacy)
      {
        StackFrame stackFrame = stackFrames[stackFrameLegacy];
        if (!LoggerImpl.SkipAssembly(stackFrame))
        {
          if (stackFrame.GetMethod()?.Name == "MoveNext" && stackFrames.Length > stackFrameLegacy)
          {
            Type declaringType = stackFrames[stackFrameLegacy + 1].GetMethod()?.DeclaringType;
            if (declaringType == typeof (AsyncTaskMethodBuilder) || declaringType == typeof (AsyncTaskMethodBuilder<>))
              continue;
          }
          return stackFrameLegacy;
        }
      }
      return firstUserStackFrame;
    }

    private static bool SkipAssembly(StackFrame frame)
    {
      Assembly assembly = StackTraceUsageUtils.LookupAssemblyFromStackFrame(frame);
      return assembly == (Assembly) null || LogManager.IsHiddenAssembly(assembly);
    }

    private static bool IsLoggerType(StackFrame frame, Type loggerType)
    {
      Type declaringType = frame.GetMethod()?.DeclaringType;
      if (!(declaringType != (Type) null))
        return false;
      return loggerType == declaringType || declaringType.IsSubclassOf(loggerType) || loggerType.IsAssignableFrom(declaringType);
    }

    private static bool WriteToTargetWithFilterChain(
      TargetWithFilterChain targetListHead,
      LogEventInfo logEvent,
      AsyncContinuation onException)
    {
      FilterResult filterResult = LoggerImpl.GetFilterResult(targetListHead.FilterChain, logEvent);
      switch (filterResult)
      {
        case FilterResult.Ignore:
        case FilterResult.IgnoreFinal:
          if (InternalLogger.IsDebugEnabled)
            InternalLogger.Debug<string, LogLevel>("{0}.{1} Rejecting message because of a filter.", logEvent.LoggerName, logEvent.Level);
          return filterResult != FilterResult.IgnoreFinal;
        default:
          targetListHead.Target.WriteAsyncLogEvent(logEvent.WithContinuation(onException));
          return filterResult != FilterResult.LogFinal;
      }
    }

    private static FilterResult GetFilterResult(IList<NLog.Filters.Filter> filterChain, LogEventInfo logEvent)
    {
      FilterResult filterResult = FilterResult.Neutral;
      if (filterChain != null)
      {
        if (filterChain.Count != 0)
        {
          try
          {
            for (int index = 0; index < filterChain.Count; ++index)
            {
              filterResult = filterChain[index].GetFilterResult(logEvent);
              if (filterResult != FilterResult.Neutral)
                break;
            }
            return filterResult;
          }
          catch (Exception ex)
          {
            InternalLogger.Warn(ex, "Exception during filter evaluation. Message will be ignore.");
            if (!ex.MustBeRethrown())
              return FilterResult.Ignore;
            throw;
          }
        }
      }
      return filterResult;
    }
  }
}
