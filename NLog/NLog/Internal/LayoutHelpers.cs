// Decompiled with JetBrains decompiler
// Type: NLog.Internal.LayoutHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Layouts;
using System;
using System.Globalization;

#nullable disable
namespace NLog.Internal
{
  internal static class LayoutHelpers
  {
    public static short RenderShort(
      this Layout layout,
      LogEventInfo logEvent,
      short defaultValue,
      string layoutName)
    {
      if (layout == null)
      {
        InternalLogger.Debug(layoutName + " is null so default value of " + (object) defaultValue);
        return defaultValue;
      }
      if (logEvent == null)
      {
        InternalLogger.Debug(layoutName + ": logEvent is null so default value of " + (object) defaultValue);
        return defaultValue;
      }
      string s = layout.Render(logEvent);
      short result;
      if (short.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        return result;
      InternalLogger.Warn(layoutName + ": parse of value '" + s + "' failed, return " + (object) defaultValue);
      return defaultValue;
    }

    public static int RenderInt(
      this Layout layout,
      LogEventInfo logEvent,
      int defaultValue,
      string layoutName)
    {
      if (layout == null)
      {
        InternalLogger.Debug(layoutName + " is null so default value of " + (object) defaultValue);
        return defaultValue;
      }
      if (logEvent == null)
      {
        InternalLogger.Debug(layoutName + ": logEvent is null so default value of " + (object) defaultValue);
        return defaultValue;
      }
      string s = layout.Render(logEvent);
      int result;
      if (int.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        return result;
      InternalLogger.Warn(layoutName + ": parse of value '" + s + "' failed, return " + (object) defaultValue);
      return defaultValue;
    }

    public static bool RenderBool(
      this Layout layout,
      LogEventInfo logEvent,
      bool defaultValue,
      string layoutName)
    {
      if (layout == null)
      {
        InternalLogger.Debug(layoutName + " is null so default value of " + defaultValue.ToString());
        return defaultValue;
      }
      if (logEvent == null)
      {
        InternalLogger.Debug(layoutName + ": logEvent is null so default value of " + defaultValue.ToString());
        return defaultValue;
      }
      string str = layout.Render(logEvent);
      bool result;
      if (bool.TryParse(str, out result))
        return result;
      InternalLogger.Warn(layoutName + ": parse of value '" + str + "' failed, return " + defaultValue.ToString());
      return defaultValue;
    }
  }
}
