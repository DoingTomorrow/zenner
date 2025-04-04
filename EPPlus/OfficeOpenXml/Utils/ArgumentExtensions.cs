// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.ArgumentExtensions
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Utils
{
  public static class ArgumentExtensions
  {
    public static void IsNotNull<T>(this IArgument<T> argument, string argumentName) where T : class
    {
      argumentName = string.IsNullOrEmpty(argumentName) ? "value" : argumentName;
      if ((object) argument.Value == null)
        throw new ArgumentNullException(argumentName);
    }

    public static void IsNotNullOrEmpty(this IArgument<string> argument, string argumentName)
    {
      if (string.IsNullOrEmpty(argument.Value))
        throw new ArgumentNullException(argumentName);
    }

    public static void IsInRange<T>(this IArgument<T> argument, T min, T max, string argumentName) where T : IComparable
    {
      if (argument.Value.CompareTo((object) min) < 0 || argument.Value.CompareTo((object) max) > 0)
        throw new ArgumentOutOfRangeException(argumentName);
    }
  }
}
