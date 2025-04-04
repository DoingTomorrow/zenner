// Decompiled with JetBrains decompiler
// Type: NLog.Internal.CallSiteInformation
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace NLog.Internal
{
  internal class CallSiteInformation
  {
    public void SetStackTrace(StackTrace stackTrace, int userStackFrame, int? userStackFrameLegacy)
    {
      this.StackTrace = stackTrace;
      this.UserStackFrameNumber = userStackFrame;
      int? nullable = userStackFrameLegacy;
      int num = userStackFrame;
      this.UserStackFrameNumberLegacy = (nullable.GetValueOrDefault() == num ? (!nullable.HasValue ? 1 : 0) : 1) != 0 ? userStackFrameLegacy : new int?();
    }

    public void SetCallerInfo(
      string callerClassName,
      string callerMemberName,
      string callerFilePath,
      int callerLineNumber)
    {
      this.CallerClassName = callerClassName;
      this.CallerMemberName = callerMemberName;
      this.CallerFilePath = callerFilePath;
      this.CallerLineNumber = new int?(callerLineNumber);
    }

    public StackFrame UserStackFrame
    {
      get
      {
        return this.StackTrace?.GetFrame(this.UserStackFrameNumberLegacy ?? this.UserStackFrameNumber);
      }
    }

    public int UserStackFrameNumber { get; private set; }

    public int? UserStackFrameNumberLegacy { get; private set; }

    public StackTrace StackTrace { get; private set; }

    public MethodBase GetCallerStackFrameMethod(int skipFrames)
    {
      return this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames)?.GetMethod();
    }

    public string GetCallerClassName(
      MethodBase method,
      bool includeNameSpace,
      bool cleanAsyncMoveNext,
      bool cleanAnonymousDelegates)
    {
      if (!string.IsNullOrEmpty(this.CallerClassName))
      {
        if (includeNameSpace)
          return this.CallerClassName;
        int num = this.CallerClassName.LastIndexOf('.');
        return num < 0 || num >= this.CallerClassName.Length - 1 ? this.CallerClassName : this.CallerClassName.Substring(num + 1);
      }
      MethodBase methodBase = method;
      if ((object) methodBase == null)
        methodBase = this.GetCallerStackFrameMethod(0);
      method = methodBase;
      if (method == (MethodBase) null)
        return string.Empty;
      int? frameNumberLegacy;
      int num1;
      if (!cleanAsyncMoveNext)
      {
        frameNumberLegacy = this.UserStackFrameNumberLegacy;
        num1 = frameNumberLegacy.HasValue ? 1 : 0;
      }
      else
        num1 = 1;
      cleanAsyncMoveNext = num1 != 0;
      int num2;
      if (!cleanAnonymousDelegates)
      {
        frameNumberLegacy = this.UserStackFrameNumberLegacy;
        num2 = frameNumberLegacy.HasValue ? 1 : 0;
      }
      else
        num2 = 1;
      cleanAnonymousDelegates = num2 != 0;
      return StackTraceUsageUtils.GetStackFrameMethodClassName(method, includeNameSpace, cleanAsyncMoveNext, cleanAnonymousDelegates) ?? string.Empty;
    }

    public string GetCallerMemberName(
      MethodBase method,
      bool includeMethodInfo,
      bool cleanAsyncMoveNext,
      bool cleanAnonymousDelegates)
    {
      if (!string.IsNullOrEmpty(this.CallerMemberName))
        return this.CallerMemberName;
      MethodBase methodBase = method;
      if ((object) methodBase == null)
        methodBase = this.GetCallerStackFrameMethod(0);
      method = methodBase;
      if (method == (MethodBase) null)
        return string.Empty;
      int? frameNumberLegacy;
      int num1;
      if (!cleanAsyncMoveNext)
      {
        frameNumberLegacy = this.UserStackFrameNumberLegacy;
        num1 = frameNumberLegacy.HasValue ? 1 : 0;
      }
      else
        num1 = 1;
      cleanAsyncMoveNext = num1 != 0;
      int num2;
      if (!cleanAnonymousDelegates)
      {
        frameNumberLegacy = this.UserStackFrameNumberLegacy;
        num2 = frameNumberLegacy.HasValue ? 1 : 0;
      }
      else
        num2 = 1;
      cleanAnonymousDelegates = num2 != 0;
      return StackTraceUsageUtils.GetStackFrameMethodName(method, includeMethodInfo, cleanAsyncMoveNext, cleanAnonymousDelegates) ?? string.Empty;
    }

    public string GetCallerFilePath(int skipFrames)
    {
      return !string.IsNullOrEmpty(this.CallerFilePath) ? this.CallerFilePath : this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames)?.GetFileName() ?? string.Empty;
    }

    public int GetCallerLineNumber(int skipFrames)
    {
      if (this.CallerLineNumber.HasValue)
        return this.CallerLineNumber.Value;
      StackFrame frame = this.StackTrace?.GetFrame(this.UserStackFrameNumber + skipFrames);
      return frame == null ? 0 : frame.GetFileLineNumber();
    }

    public string CallerClassName { get; private set; }

    public string CallerMemberName { get; private set; }

    public string CallerFilePath { get; private set; }

    public int? CallerLineNumber { get; private set; }
  }
}
