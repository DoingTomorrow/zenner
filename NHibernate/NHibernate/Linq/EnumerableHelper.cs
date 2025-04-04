// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.EnumerableHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq
{
  public static class EnumerableHelper
  {
    public static MethodInfo GetMethod(string name, Type[] parameterTypes)
    {
      return ((IEnumerable<MethodInfo>) typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == name && EnumerableHelper.ParameterTypesMatch(m.GetParameters(), parameterTypes))).Single<MethodInfo>();
    }

    public static MethodInfo GetMethod(
      string name,
      Type[] parameterTypes,
      Type[] genericTypeParameters)
    {
      return ((IEnumerable<MethodInfo>) typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == name && m.ContainsGenericParameters && ((IEnumerable<Type>) m.GetGenericArguments()).Count<Type>() == genericTypeParameters.Length && EnumerableHelper.ParameterTypesMatch(m.GetParameters(), parameterTypes))).Single<MethodInfo>().MakeGenericMethod(genericTypeParameters);
    }

    private static bool ParameterTypesMatch(ParameterInfo[] parameters, Type[] types)
    {
      if (parameters.Length != types.Length)
        return false;
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (parameters[index].ParameterType != types[index] && (!parameters[index].ParameterType.ContainsGenericParameters || !types[index].ContainsGenericParameters || parameters[index].ParameterType.GetGenericArguments().Length != types[index].GetGenericArguments().Length))
          return false;
      }
      return true;
    }
  }
}
