// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.WrappedClassMembersCollector
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class WrappedClassMembersCollector(Type type) : ClassMembersCollector(type)
  {
    public override void CollectMembersToProxy(IProxyGenerationHook hook)
    {
      base.CollectMembersToProxy(hook);
      this.CollectFields(hook);
    }

    protected override MetaMethod GetMethodToGenerate(
      MethodInfo method,
      IProxyGenerationHook hook,
      bool isStandalone)
    {
      if (!this.IsAccessible((MethodBase) method))
        return (MetaMethod) null;
      bool proxyable = this.AcceptMethod(method, true, hook);
      if (!proxyable && !method.IsAbstract)
        return (MetaMethod) null;
      bool hasTarget = true;
      return new MetaMethod(method, method, isStandalone, proxyable, hasTarget);
    }

    protected bool IsGeneratedByTheCompiler(FieldInfo field)
    {
      return Attribute.IsDefined((MemberInfo) field, typeof (CompilerGeneratedAttribute));
    }

    protected virtual bool IsOKToBeOnProxy(FieldInfo field) => this.IsGeneratedByTheCompiler(field);

    private void CollectFields(IProxyGenerationHook hook)
    {
      foreach (FieldInfo allField in this.type.GetAllFields())
      {
        if (!this.IsOKToBeOnProxy(allField))
          hook.NonProxyableMemberNotification(this.type, (MemberInfo) allField);
      }
    }
  }
}
