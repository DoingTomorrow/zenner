// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.ComparableExtensions
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class ComparableExtensions
  {
    public static bool IsEqualTo<T>(this T obj, T value) where T : IComparable<T>
    {
      return obj.CompareTo(value) == 0;
    }

    public static bool IsGreaterThan<T>(this T obj, T value) where T : IComparable<T>
    {
      return obj.CompareTo(value) > 0;
    }

    public static bool IsGreaterThanOrEqualTo<T>(this T obj, T value) where T : IComparable<T>
    {
      return obj.CompareTo(value) >= 0;
    }

    public static bool IsLessThan<T>(this T obj, T value) where T : IComparable<T>
    {
      return obj.CompareTo(value) < 0;
    }

    public static bool IsLessThanOrEqualTo<T>(this T obj, T value) where T : IComparable<T>
    {
      return obj.CompareTo(value) <= 0;
    }
  }
}
