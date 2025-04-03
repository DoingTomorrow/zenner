// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.DelegateMembersCollector
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class DelegateMembersCollector(Type type) : MembersCollector(type)
  {
    protected override MetaMethod GetMethodToGenerate(
      MethodInfo method,
      IProxyGenerationHook hook,
      bool isStandalone)
    {
      return !this.AcceptMethod(method, true, hook) ? (MetaMethod) null : new MetaMethod(method, method, isStandalone, true, !method.IsAbstract);
    }
  }
}
