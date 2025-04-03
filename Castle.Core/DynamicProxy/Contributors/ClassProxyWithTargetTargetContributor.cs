// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ClassProxyWithTargetTargetContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class ClassProxyWithTargetTargetContributor : CompositeTypeContributor
  {
    private readonly Type targetType;
    private readonly IList<MethodInfo> methodsToSkip;

    public ClassProxyWithTargetTargetContributor(
      Type targetType,
      IList<MethodInfo> methodsToSkip,
      INamingScope namingScope)
      : base(namingScope)
    {
      this.targetType = targetType;
      this.methodsToSkip = methodsToSkip;
    }

    protected override IEnumerable<MembersCollector> CollectElementsToProxyInternal(
      IProxyGenerationHook hook)
    {
      WrappedClassMembersCollector membersCollector = new WrappedClassMembersCollector(this.targetType);
      membersCollector.Logger = this.Logger;
      WrappedClassMembersCollector targetItem = membersCollector;
      targetItem.CollectMembersToProxy(hook);
      yield return (MembersCollector) targetItem;
      foreach (Type @interface in (IEnumerable<Type>) this.interfaces)
      {
        InterfaceMembersOnClassCollector onClassCollector = new InterfaceMembersOnClassCollector(@interface, true, this.targetType.GetInterfaceMap(@interface));
        onClassCollector.Logger = this.Logger;
        InterfaceMembersOnClassCollector item = onClassCollector;
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
      if (this.methodsToSkip.Contains(method.Method))
        return (MethodGenerator) null;
      if (!method.Proxyable)
        return (MethodGenerator) new MinimialisticMethodGenerator(method, overrideMethod);
      if (!this.IsDirectlyAccessible(method))
        return this.IndirectlyCalledMethodGenerator(method, @class, options, overrideMethod);
      Type invocationType = this.GetInvocationType(method, @class, options);
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocationType, (GetTargetExpressionDelegate) ((c, m) => c.GetField("__target").ToExpression()), overrideMethod, (IInvocationCreationContributor) null);
    }

    private IInvocationCreationContributor GetContributor(Type @delegate, MetaMethod method)
    {
      return !@delegate.IsGenericType ? (IInvocationCreationContributor) new InvocationWithDelegateContributor(@delegate, this.targetType, method, this.namingScope) : (IInvocationCreationContributor) new InvocationWithGenericDelegateContributor(@delegate, method, (Reference) new FieldReference(InvocationMethods.ProxyObject));
    }

    private MethodGenerator IndirectlyCalledMethodGenerator(
      MetaMethod method,
      ClassEmitter proxy,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      IInvocationCreationContributor contributor = this.GetContributor(this.GetDelegateType(method, proxy, options), method);
      Type invocation = new CompositionInvocationTypeGenerator(this.targetType, method, (MethodInfo) null, false, contributor).Generate(proxy, options, this.namingScope).BuildType();
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) proxy.GetField("__interceptors"), invocation, (GetTargetExpressionDelegate) ((c, m) => c.GetField("__target").ToExpression()), overrideMethod, contributor);
    }

    private Type GetDelegateType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      ModuleScope moduleScope = @class.ModuleScope;
      CacheKey key = new CacheKey((MemberInfo) typeof (Delegate), this.targetType, ((IEnumerable<Type>) new Type[1]
      {
        method.MethodOnTarget.ReturnType
      }).Concat<Type>((IEnumerable<Type>) ArgumentsUtil.GetTypes(method.MethodOnTarget.GetParameters())).ToArray<Type>(), (ProxyGenerationOptions) null);
      Type fromCache = moduleScope.GetFromCache(key);
      if (fromCache != null)
        return fromCache;
      Type type = new DelegateTypeGenerator(method, this.targetType).Generate(@class, options, this.namingScope).BuildType();
      moduleScope.RegisterInCache(key, type);
      return type;
    }

    private bool IsDirectlyAccessible(MetaMethod method) => method.MethodOnTarget.IsPublic;

    private Type GetInvocationType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      ModuleScope moduleScope = @class.ModuleScope;
      Type[] interfaces = new Type[1]
      {
        typeof (IInvocation)
      };
      CacheKey key = new CacheKey((MemberInfo) method.Method, CompositionInvocationTypeGenerator.BaseType, interfaces, (ProxyGenerationOptions) null);
      Type fromCache = moduleScope.GetFromCache(key);
      if (fromCache != null)
        return fromCache;
      Type type = this.BuildInvocationType(method, @class, options);
      moduleScope.RegisterInCache(key, type);
      return type;
    }

    private Type BuildInvocationType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      return !method.HasTarget ? new InheritanceInvocationTypeGenerator(this.targetType, method, (MethodInfo) null, (IInvocationCreationContributor) null).Generate(@class, options, this.namingScope).BuildType() : new CompositionInvocationTypeGenerator(method.Method.DeclaringType, method, method.Method, false, (IInvocationCreationContributor) null).Generate(@class, options, this.namingScope).BuildType();
    }
  }
}
