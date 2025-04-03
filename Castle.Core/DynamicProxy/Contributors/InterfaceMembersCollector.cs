// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceMembersCollector
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InterfaceMembersCollector(Type @interface) : MembersCollector(@interface)
  {
    protected override MetaMethod GetMethodToGenerate(
      MethodInfo method,
      IProxyGenerationHook hook,
      bool isStandalone)
    {
      if (!this.IsAccessible((MethodBase) method))
        return (MetaMethod) null;
      bool proxyable = this.AcceptMethod(method, false, hook);
      return new MetaMethod(method, method, isStandalone, proxyable, false);
    }
  }
}
