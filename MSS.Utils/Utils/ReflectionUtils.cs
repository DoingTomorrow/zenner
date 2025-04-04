// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.ReflectionUtils
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace MSS.Utils.Utils
{
  public static class ReflectionUtils
  {
    public static bool IsSubtypeOf<T>(this Type type)
    {
      type = type.AssertParameterNotNull<Type>(nameof (type));
      return typeof (T).IsAssignableFrom(type);
    }

    public static bool IsCollection(this Type type)
    {
      type = type.AssertParameterNotNull<Type>(nameof (type));
      return !(type == typeof (string)) && type.IsSubtypeOf<IEnumerable>();
    }

    public static bool IsIEnumerableOfType<T>(this Type type)
    {
      type = type.AssertParameterNotNull<Type>(nameof (type));
      return type.IsCollection() && type.IsGenericType && type.GetGenericArguments().Length == 1 && type.GetGenericArguments()[0].IsSubtypeOf<T>();
    }

    public static TType AssertParameterNotNull<TType>([ValidatedNotNull] this TType @this, string parameterName) where TType : class
    {
      return (object) @this != null ? @this : throw new ArgumentNullException(parameterName);
    }

    [DebuggerHidden]
    public static TType AssertNotNull<TType>(
      this TType @this,
      string message = "Unexpected null",
      params object[] messageParameters)
      where TType : class
    {
      if ((object) @this != null)
        ;
      return @this;
    }
  }
}
