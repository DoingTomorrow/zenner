// Decompiled with JetBrains decompiler
// Type: MSS.Utils.Utils.GenericMethodInvoker`1
// Assembly: MSS.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8365EDE-890D-4A42-AEA4-3B8FCE5E7B93
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Utils.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace MSS.Utils.Utils
{
  public class GenericMethodInvoker<T>
  {
    private readonly ConcurrentDictionary<Type[], Func<object, object[], object>> Cache = new ConcurrentDictionary<Type[], Func<object, object[], object>>((IEqualityComparer<Type[]>) new GenericMethodInvoker<T>.TypeArrayEquals());
    public readonly MethodInfo GenericMethodInfo;

    public GenericMethodInvoker(string methodName)
    {
      this.GenericMethodInfo = ((IEnumerable<MethodInfo>) typeof (T).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (_ => _.IsGenericMethodDefinition && _.Name == methodName));
    }

    public object Invoke(Type[] types, object @this, params object[] parameters)
    {
      types = types.AssertParameterNotNull<Type[]>(nameof (types));
      parameters = parameters.AssertParameterNotNull<object[]>(nameof (parameters));
      return this.Cache.GetOrAdd(types, (Func<Type[], Func<object, object[], object>>) (_ =>
      {
        MethodInfo method = this.GenericMethodInfo.MakeGenericMethod(types);
        ParameterExpression parameterExpression;
        ParameterExpression param;
        return Expression.Lambda<Func<object, object[], object>>((Expression) Expression.Call(method.IsStatic ? (Expression) null : (Expression) Expression.Convert((Expression) parameterExpression, method.DeclaringType.AssertNotNull<Type>()), method, ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, Expression>((Func<ParameterInfo, int, Expression>) ((o, i) => (Expression) Expression.Convert((Expression) Expression.ArrayIndex((Expression) param, (Expression) Expression.Constant((object) i)), o.ParameterType))).ToArray<Expression>()), parameterExpression, param).Compile();
      }))(@this, parameters);
    }

    public class TypeArrayEquals : IEqualityComparer<Type[]>
    {
      public bool Equals(Type[] x, Type[] y)
      {
        return ((IEnumerable<Type>) x).SequenceEqual<Type>((IEnumerable<Type>) y);
      }

      public int GetHashCode(Type[] obj) => obj.Length == 0 ? 0 : obj[0].GetHashCode();
    }
  }
}
