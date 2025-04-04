// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Utilities.ExtensionMethods
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Utilities
{
  public static class ExtensionMethods
  {
    public static void IsNotNullOrEmpty(this ArgumentInfo<string> val)
    {
      if (string.IsNullOrEmpty(val.Value))
        throw new ArgumentException(val.Name + " cannot be null or empty");
    }

    public static void IsNotNull<T>(this ArgumentInfo<T> val) where T : class
    {
      if ((object) val.Value == null)
        throw new ArgumentNullException(val.Name);
    }

    public static bool IsNumeric(this object obj)
    {
      if (obj == null)
        return false;
      if (!obj.GetType().IsPrimitive)
      {
        switch (obj)
        {
          case double _:
          case Decimal _:
          case DateTime _:
            break;
          default:
            return obj is TimeSpan;
        }
      }
      return true;
    }
  }
}
