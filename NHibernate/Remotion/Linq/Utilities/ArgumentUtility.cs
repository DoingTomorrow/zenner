// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.ArgumentUtility
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using JetBrains.Annotations;
using System;
using System.Collections;

#nullable disable
namespace Remotion.Linq.Utilities
{
  public static class ArgumentUtility
  {
    [AssertionMethod]
    public static T CheckNotNull<T>([InvokerParameterName] string argumentName, [NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T actualValue)
    {
      return (object) actualValue != null ? actualValue : throw new ArgumentNullException(argumentName);
    }

    [AssertionMethod]
    public static string CheckNotNullOrEmpty([InvokerParameterName] string argumentName, [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string actualValue)
    {
      ArgumentUtility.CheckNotNull<string>(argumentName, actualValue);
      return actualValue.Length != 0 ? actualValue : throw new ArgumentEmptyException(argumentName);
    }

    [AssertionMethod]
    public static TExpected CheckNotNullAndType<TExpected>([InvokerParameterName] string argumentName, [NoEnumeration, AssertionCondition(AssertionConditionType.IS_NOT_NULL)] object actualValue)
    {
      if (actualValue == null)
        throw new ArgumentNullException(argumentName);
      return actualValue is TExpected expected ? expected : throw new ArgumentTypeException(argumentName, typeof (TExpected), actualValue.GetType());
    }

    [AssertionMethod]
    public static Type CheckTypeIsAssignableFrom(
      [InvokerParameterName] string argumentName,
      Type actualType,
      [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] Type expectedType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (expectedType), expectedType);
      return actualType == null || expectedType.IsAssignableFrom(actualType) ? actualType : throw new ArgumentTypeException(string.Format("Argument {0} is a {2}, which cannot be assigned to type {1}.", (object) argumentName, (object) expectedType, (object) actualType), argumentName, expectedType, actualType);
    }

    [AssertionMethod]
    public static T CheckNotEmpty<T>([InvokerParameterName] string argumentName, T enumerable) where T : IEnumerable
    {
      if ((object) enumerable != null)
      {
        if (enumerable is ICollection collection)
        {
          if (collection.Count == 0)
            throw new ArgumentEmptyException(argumentName);
          return enumerable;
        }
        IEnumerator enumerator = enumerable.GetEnumerator();
        using (enumerator as IDisposable)
        {
          if (!enumerator.MoveNext())
            throw new ArgumentEmptyException(argumentName);
        }
      }
      return enumerable;
    }
  }
}
