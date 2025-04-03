// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MethodSignatureComparer
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MethodSignatureComparer : IEqualityComparer<MethodInfo>
  {
    public static readonly MethodSignatureComparer Instance = new MethodSignatureComparer();

    public bool Equals(MethodInfo x, MethodInfo y)
    {
      if (x == null && y == null)
        return true;
      return x != null && y != null && this.EqualNames(x, y) && this.EqualGenericParameters(x, y) && this.EqualSignatureTypes(x.ReturnType, y.ReturnType) && this.EqualParameters(x, y);
    }

    private bool EqualNames(MethodInfo x, MethodInfo y) => x.Name == y.Name;

    public bool EqualGenericParameters(MethodInfo x, MethodInfo y)
    {
      if (x.IsGenericMethod != y.IsGenericMethod)
        return false;
      if (x.IsGenericMethod)
      {
        Type[] genericArguments1 = x.GetGenericArguments();
        Type[] genericArguments2 = y.GetGenericArguments();
        if (genericArguments1.Length != genericArguments2.Length)
          return false;
        for (int index = 0; index < genericArguments1.Length; ++index)
        {
          if (genericArguments1[index].IsGenericParameter != genericArguments2[index].IsGenericParameter || !genericArguments1[index].IsGenericParameter && !genericArguments1[index].Equals(genericArguments2[index]))
            return false;
        }
      }
      return true;
    }

    public bool EqualParameters(MethodInfo x, MethodInfo y)
    {
      ParameterInfo[] parameters1 = x.GetParameters();
      ParameterInfo[] parameters2 = y.GetParameters();
      if (parameters1.Length != parameters2.Length)
        return false;
      for (int index = 0; index < parameters1.Length; ++index)
      {
        if (!this.EqualSignatureTypes(parameters1[index].ParameterType, parameters2[index].ParameterType))
          return false;
      }
      return true;
    }

    public bool EqualSignatureTypes(Type x, Type y)
    {
      if (x.IsGenericParameter != y.IsGenericParameter)
        return false;
      if (x.IsGenericParameter)
      {
        if (x.GenericParameterPosition != y.GenericParameterPosition)
          return false;
      }
      else if (!x.Equals(y))
        return false;
      return true;
    }

    public int GetHashCode(MethodInfo obj) => obj.Name.GetHashCode() ^ obj.GetParameters().Length;
  }
}
