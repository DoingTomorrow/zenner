// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleTargetHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.Targets
{
  internal static class ConsoleTargetHelper
  {
    public static bool IsConsoleAvailable(out string reason)
    {
      reason = string.Empty;
      try
      {
        if (!Environment.UserInteractive)
        {
          if (PlatformDetector.IsMono && Console.In is StreamReader)
            return true;
          reason = "Environment.UserInteractive = False";
          return false;
        }
        if (Console.OpenStandardInput(1) == Stream.Null)
        {
          reason = "Console.OpenStandardInput = Null";
          return false;
        }
      }
      catch (Exception ex)
      {
        reason = string.Format("Unexpected exception: {0}:{1}", (object) ex.GetType().Name, (object) ex.Message);
        InternalLogger.Warn(ex, "Failed to detect whether console is available.");
        return false;
      }
      return true;
    }

    public static Encoding GetConsoleOutputEncoding(
      Encoding currentEncoding,
      bool isInitialized,
      bool pauseLogging)
    {
      if (currentEncoding != null)
        return currentEncoding;
      return isInitialized && !pauseLogging || ConsoleTargetHelper.IsConsoleAvailable(out string _) ? Console.OutputEncoding : Encoding.Default;
    }

    public static bool SetConsoleOutputEncoding(
      Encoding newEncoding,
      bool isInitialized,
      bool pauseLogging)
    {
      if (!isInitialized)
        return true;
      if (pauseLogging)
        return false;
      Console.OutputEncoding = newEncoding;
      return true;
    }
  }
}
