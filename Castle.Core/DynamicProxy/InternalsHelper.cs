// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.InternalsHelper
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Castle.DynamicProxy
{
  public class InternalsHelper
  {
    private static readonly Lock internalsToDynProxyLock = Lock.Create();
    private static readonly IDictionary<Assembly, bool> internalsToDynProxy = (IDictionary<Assembly, bool>) new Dictionary<Assembly, bool>();

    public static bool IsInternalToDynamicProxy(Assembly asm)
    {
      using (IUpgradeableLockHolder upgradeableLockHolder = InternalsHelper.internalsToDynProxyLock.ForReadingUpgradeable())
      {
        if (InternalsHelper.internalsToDynProxy.ContainsKey(asm))
          return InternalsHelper.internalsToDynProxy[asm];
        upgradeableLockHolder.Upgrade();
        if (InternalsHelper.internalsToDynProxy.ContainsKey(asm))
          return InternalsHelper.internalsToDynProxy[asm];
        InternalsVisibleToAttribute[] customAttributes = (InternalsVisibleToAttribute[]) asm.GetCustomAttributes(typeof (InternalsVisibleToAttribute), false);
        bool dynamicProxy = false;
        foreach (InternalsVisibleToAttribute visibleToAttribute in customAttributes)
        {
          if (visibleToAttribute.AssemblyName.Contains(ModuleScope.DEFAULT_ASSEMBLY_NAME))
          {
            dynamicProxy = true;
            break;
          }
        }
        InternalsHelper.internalsToDynProxy.Add(asm, dynamicProxy);
        return dynamicProxy;
      }
    }

    public static bool IsInternal(MethodInfo method)
    {
      if (method.IsAssembly)
        return true;
      return method.IsFamilyAndAssembly && !method.IsFamilyOrAssembly;
    }
  }
}
