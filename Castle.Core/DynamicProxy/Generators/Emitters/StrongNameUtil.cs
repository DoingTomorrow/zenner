// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.StrongNameUtil
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public static class StrongNameUtil
  {
    private static readonly IDictionary<Assembly, bool> signedAssemblyCache = (IDictionary<Assembly, bool>) new Dictionary<Assembly, bool>();
    private static readonly bool canStrongNameAssembly;
    private static readonly object lockObject = new object();

    static StrongNameUtil()
    {
      try
      {
        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        StrongNameUtil.canStrongNameAssembly = true;
      }
      catch (SecurityException ex)
      {
        StrongNameUtil.canStrongNameAssembly = false;
      }
    }

    public static bool IsAssemblySigned(Assembly assembly)
    {
      lock (StrongNameUtil.lockObject)
      {
        if (!StrongNameUtil.signedAssemblyCache.ContainsKey(assembly))
        {
          bool flag = StrongNameUtil.ContainsPublicKey(assembly);
          StrongNameUtil.signedAssemblyCache.Add(assembly, flag);
        }
        return StrongNameUtil.signedAssemblyCache[assembly];
      }
    }

    private static bool ContainsPublicKey(Assembly assembly)
    {
      return assembly.FullName != null && !assembly.FullName.Contains("PublicKeyToken=null");
    }

    public static bool IsAnyTypeFromUnsignedAssembly(IEnumerable<Type> types)
    {
      return types.Any<Type>((Func<Type, bool>) (t => !StrongNameUtil.IsAssemblySigned(t.Assembly)));
    }

    public static bool IsAnyTypeFromUnsignedAssembly(Type baseType, IEnumerable<Type> interfaces)
    {
      return baseType != null && !StrongNameUtil.IsAssemblySigned(baseType.Assembly) || StrongNameUtil.IsAnyTypeFromUnsignedAssembly(interfaces);
    }

    public static bool CanStrongNameAssembly => StrongNameUtil.canStrongNameAssembly;
  }
}
