// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DefaultDynamicProxyMethodCheckerExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy
{
  public static class DefaultDynamicProxyMethodCheckerExtensions
  {
    public static bool IsProxiable(this MethodInfo method)
    {
      if (method.IsFinal || method.DeclaringType == typeof (MarshalByRefObject) || method.DeclaringType == typeof (object) && "finalize".Equals(method.Name.ToLowerInvariant()))
        return false;
      if ((method.IsPublic || method.IsFamily) && (method.IsVirtual || method.IsAbstract))
        return true;
      if (!method.IsFamilyOrAssembly)
        return false;
      return method.IsVirtual || method.IsAbstract;
    }

    public static bool ShouldBeProxiable(this MethodInfo method)
    {
      if (method.DeclaringType == typeof (MarshalByRefObject) || method.DeclaringType == typeof (object) && "finalize".Equals(method.Name.ToLowerInvariant()) || method.DeclaringType == typeof (object) && "GetType".Equals(method.Name) || method.DeclaringType == typeof (object) && "obj_address".Equals(method.Name) || DefaultDynamicProxyMethodCheckerExtensions.IsDisposeMethod(method))
        return false;
      return method.IsPublic || method.IsAssembly || method.IsFamilyOrAssembly;
    }

    public static bool ShouldBeProxiable(this PropertyInfo propertyInfo)
    {
      return propertyInfo == null || ((IEnumerable<MethodInfo>) propertyInfo.GetAccessors(true)).Where<MethodInfo>((Func<MethodInfo, bool>) (x => x.IsPublic || x.IsAssembly || x.IsFamilyOrAssembly)).Any<MethodInfo>();
    }

    private static bool IsDisposeMethod(MethodInfo method)
    {
      return method.Name.Equals("Dispose") && method.MemberType == MemberTypes.Method && method.GetParameters().Length == 0;
    }
  }
}
