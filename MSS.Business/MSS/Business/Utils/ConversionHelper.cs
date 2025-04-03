// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.ConversionHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace MSS.Business.Utils
{
  public class ConversionHelper
  {
    public static object ConvertValue(Type valueType, string value)
    {
      if (valueType == typeof (bool))
      {
        switch (value.ToUpper())
        {
          case "YES":
            value = true.ToString();
            break;
          case "NO":
            value = false.ToString();
            break;
        }
      }
      if (valueType == typeof (double))
        value = value.TrimEnd('%');
      if (valueType == typeof (double) || valueType == typeof (int) || valueType == typeof (float))
      {
        string numberGroupSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator;
        value = value.Replace(numberGroupSeparator, string.Empty);
      }
      if (!valueType.IsGenericType || !(valueType.GetGenericTypeDefinition() == typeof (Nullable<>)))
        return Convert.ChangeType((object) value, valueType, (IFormatProvider) Thread.CurrentThread.CurrentCulture);
      Activator.CreateInstance(valueType);
      object obj1 = (object) value;
      // ISSUE: reference to a compiler-generated field
      if (ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ConversionHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof (ConversionHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target2 = ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p1 = ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "IsNullOrEmpty", (IEnumerable<Type>) null, typeof (ConversionHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__0, typeof (string), obj1);
      object obj3 = target2((CallSite) p1, obj2);
      if (!target1((CallSite) p2, obj3))
        return (object) null;
      // ISSUE: reference to a compiler-generated field
      if (ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__3 = CallSite<Func<CallSite, Type, object, Type, CultureInfo, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ChangeType", (IEnumerable<Type>) null, typeof (ConversionHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__3.Target((CallSite) ConversionHelper.\u003C\u003Eo__0.\u003C\u003Ep__3, typeof (Convert), obj1, Nullable.GetUnderlyingType(valueType), Thread.CurrentThread.CurrentCulture);
    }
  }
}
