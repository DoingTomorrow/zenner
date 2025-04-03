// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceProxyTargetContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InterfaceProxyTargetContributor : CompositeTypeContributor
  {
    private readonly Type proxyTargetType;
    private readonly bool canChangeTarget;

    public InterfaceProxyTargetContributor(
      Type proxyTargetType,
      bool canChangeTarget,
      INamingScope namingScope)
      : base(namingScope)
    {
      this.proxyTargetType = proxyTargetType;
      this.canChangeTarget = canChangeTarget;
    }

    protected override IEnumerable<MembersCollector> CollectElementsToProxyInternal(
      IProxyGenerationHook hook)
    {
      foreach (Type @interface in (IEnumerable<Type>) this.interfaces)
      {
        MembersCollector item = this.GetCollectorForInterface(@interface);
        item.Logger = this.Logger;
        item.CollectMembersToProxy(hook);
        yield return item;
      }
    }

    protected virtual MembersCollector GetCollectorForInterface(Type @interface)
    {
      return (MembersCollector) new InterfaceMembersOnClassCollector(@interface, false, this.proxyTargetType.GetInterfaceMap(@interface));
    }

    protected override MethodGenerator GetMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      if (!method.Proxyable)
        return (MethodGenerator) new ForwardingMethodGenerator(method, overrideMethod, (GetTargetReferenceDelegate) ((c, m) => (Reference) c.GetField("__target")));
      Type invocationType = this.GetInvocationType(method, @class, options);
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocationType, (GetTargetExpressionDelegate) ((c, m) => c.GetField("__target").ToExpression()), overrideMethod, (IInvocationCreationContributor) null);
    }

    private Type GetInvocationType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      ModuleScope moduleScope = @class.ModuleScope;
      Type[] interfaces;
      if (this.canChangeTarget)
        interfaces = new Type[2]
        {
          typeof (IInvocation),
          typeof (IChangeProxyTarget)
        };
      else
        interfaces = new Type[1]{ typeof (IInvocation) };
      CacheKey key = new CacheKey((MemberInfo) method.Method, CompositionInvocationTypeGenerator.BaseType, interfaces, (ProxyGenerationOptions) null);
      Type fromCache = moduleScope.GetFromCache(key);
      if (fromCache != null)
        return fromCache;
      Type type = new CompositionInvocationTypeGenerator(method.Method.DeclaringType, method, method.Method, this.canChangeTarget, (IInvocationCreationContributor) null).Generate(@class, options, this.namingScope).BuildType();
      moduleScope.RegisterInCache(key, type);
      return type;
    }
  }
}
