// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceProxyWithoutTargetContributor
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
  public class InterfaceProxyWithoutTargetContributor : CompositeTypeContributor
  {
    private readonly GetTargetExpressionDelegate getTargetExpression;

    public InterfaceProxyWithoutTargetContributor(
      INamingScope namingScope,
      GetTargetExpressionDelegate getTarget)
      : base(namingScope)
    {
      this.getTargetExpression = getTarget;
    }

    protected override IEnumerable<MembersCollector> CollectElementsToProxyInternal(
      IProxyGenerationHook hook)
    {
      foreach (Type @interface in (IEnumerable<Type>) this.interfaces)
      {
        InterfaceMembersCollector item = new InterfaceMembersCollector(@interface);
        item.CollectMembersToProxy(hook);
        yield return (MembersCollector) item;
      }
    }

    protected override MethodGenerator GetMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      if (!method.Proxyable)
        return (MethodGenerator) new MinimialisticMethodGenerator(method, overrideMethod);
      Type invocationType = this.GetInvocationType(method, @class, options);
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocationType, this.getTargetExpression, overrideMethod, (IInvocationCreationContributor) null);
    }

    private Type GetInvocationType(
      MetaMethod method,
      ClassEmitter emitter,
      ProxyGenerationOptions options)
    {
      ModuleScope moduleScope = emitter.ModuleScope;
      CacheKey key = new CacheKey((MemberInfo) method.Method, CompositionInvocationTypeGenerator.BaseType, (Type[]) null, (ProxyGenerationOptions) null);
      Type fromCache = moduleScope.GetFromCache(key);
      if (fromCache != null)
        return fromCache;
      Type type = new CompositionInvocationTypeGenerator(method.Method.DeclaringType, method, method.Method, false, (IInvocationCreationContributor) null).Generate(emitter, options, this.namingScope).BuildType();
      moduleScope.RegisterInCache(key, type);
      return type;
    }
  }
}
