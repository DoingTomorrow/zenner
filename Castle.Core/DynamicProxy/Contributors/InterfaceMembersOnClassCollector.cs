// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceMembersOnClassCollector
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InterfaceMembersOnClassCollector : MembersCollector
  {
    private readonly bool onlyProxyVirtual;
    private readonly InterfaceMapping map;

    public InterfaceMembersOnClassCollector(Type type, bool onlyProxyVirtual, InterfaceMapping map)
      : base(type)
    {
      this.onlyProxyVirtual = onlyProxyVirtual;
      this.map = map;
    }

    protected override MetaMethod GetMethodToGenerate(
      MethodInfo method,
      IProxyGenerationHook hook,
      bool isStandalone)
    {
      if (!this.IsAccessible((MethodBase) method))
        return (MetaMethod) null;
      if (this.onlyProxyVirtual && this.IsVirtuallyImplementedInterfaceMethod(method))
        return (MetaMethod) null;
      MethodInfo methodOnTarget = this.GetMethodOnTarget(method);
      bool proxyable = this.AcceptMethod(method, this.onlyProxyVirtual, hook);
      return new MetaMethod(method, methodOnTarget, isStandalone, proxyable, !methodOnTarget.IsPrivate);
    }

    private MethodInfo GetMethodOnTarget(MethodInfo method)
    {
      int index = Array.IndexOf<MethodInfo>(this.map.InterfaceMethods, method);
      return index == -1 ? (MethodInfo) null : this.map.TargetMethods[index];
    }

    private bool IsVirtuallyImplementedInterfaceMethod(MethodInfo method)
    {
      MethodInfo methodOnTarget = this.GetMethodOnTarget(method);
      return methodOnTarget != null && !methodOnTarget.IsFinal;
    }
  }
}
