// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FormatHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;

#nullable disable
namespace NLog.Internal
{
  internal static class FormatHelper
  {
    internal static string ConvertToString(object o, IFormatProvider formatProvider)
    {
      if (formatProvider == null && !(o is string))
      {
        LoggingConfiguration configuration = LogManager.Configuration;
        if (configuration != null)
          formatProvider = (IFormatProvider) configuration.DefaultCultureInfo;
      }
      return Convert.ToString(o, formatProvider);
    }
  }
}
