// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ExceptionHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using System;
using System.Threading;

#nullable disable
namespace NLog.Internal
{
  internal static class ExceptionHelper
  {
    private const string LoggedKey = "NLog.ExceptionLoggedToInternalLogger";

    public static void MarkAsLoggedToInternalLogger(this Exception exception)
    {
      if (exception == null)
        return;
      exception.Data[(object) "NLog.ExceptionLoggedToInternalLogger"] = (object) true;
    }

    public static bool IsLoggedToInternalLogger(this Exception exception)
    {
      return exception != null && (exception.Data[(object) "NLog.ExceptionLoggedToInternalLogger"] as bool? ?? false);
    }

    public static bool MustBeRethrown(this Exception exception)
    {
      if (exception.MustBeRethrownImmediately())
        return true;
      bool flag = exception is NLogConfigurationException;
      if (!exception.IsLoggedToInternalLogger())
      {
        NLog.LogLevel level = flag ? NLog.LogLevel.Warn : NLog.LogLevel.Error;
        InternalLogger.Log(exception, level, "Error has been raised.");
      }
      return !flag ? LogManager.ThrowExceptions : LogManager.ThrowConfigExceptions ?? LogManager.ThrowExceptions;
    }

    public static bool MustBeRethrownImmediately(this Exception exception)
    {
      switch (exception)
      {
        case StackOverflowException _:
          return true;
        case ThreadAbortException _:
          return true;
        case OutOfMemoryException _:
          return true;
        default:
          return false;
      }
    }
  }
}
