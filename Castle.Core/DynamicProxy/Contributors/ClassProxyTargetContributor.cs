// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ClassProxyTargetContributor
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
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class ClassProxyTargetContributor : CompositeTypeContributor
  {
    private readonly IList<MethodInfo> methodsToSkip;
    private readonly Type targetType;

    public ClassProxyTargetContributor(
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
      ClassMembersCollector membersCollector = new ClassMembersCollector(this.targetType);
      membersCollector.Logger = this.Logger;
      ClassMembersCollector targetItem = membersCollector;
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
      if (this.ExplicitlyImplementedInterfaceMethod(method))
        return this.ExplicitlyImplementedInterfaceMethodGenerator(method, @class, options, overrideMethod);
      Type invocationType = this.GetInvocationType(method, @class, options);
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocationType, (GetTargetExpressionDelegate) ((c, m) => (Expression) new TypeTokenExpression(this.targetType)), overrideMethod, (IInvocationCreationContributor) null);
    }

    private Type BuildInvocationType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      MethodInfo method1 = method.Method;
      if (!method.HasTarget)
        return new InheritanceInvocationTypeGenerator(this.targetType, method, (MethodInfo) null, (IInvocationCreationContributor) null).Generate(@class, options, this.namingScope).BuildType();
      MethodBuilder callbackMethod = this.CreateCallbackMethod(@class, method1, method.MethodOnTarget);
      return new InheritanceInvocationTypeGenerator(callbackMethod.DeclaringType, method, (MethodInfo) callbackMethod, (IInvocationCreationContributor) null).Generate(@class, options, this.namingScope).BuildType();
    }

    private MethodBuilder CreateCallbackMethod(
      ClassEmitter emitter,
      MethodInfo methodInfo,
      MethodInfo methodOnTarget)
    {
      MethodInfo methodInfo1 = methodOnTarget ?? methodInfo;
      MethodEmitter method = emitter.CreateMethod(this.namingScope.GetUniqueName(methodInfo.Name + "_callback"), methodInfo1);
      if (methodInfo1.IsGenericMethod)
        methodInfo1 = methodInfo1.MakeGenericMethod((Type[]) method.GenericTypeParams);
      Expression[] expressionArray = new Expression[method.Arguments.Length];
      for (int index = 0; index < method.Arguments.Length; ++index)
        expressionArray[index] = method.Arguments[index].ToExpression();
      method.CodeBuilder.AddStatement((Statement) new ReturnStatement((Expression) new MethodInvocationExpression((Reference) SelfReference.Self, methodInfo1, expressionArray)));
      return method.MethodBuilder;
    }

    private bool ExplicitlyImplementedInterfaceMethod(MetaMethod method)
    {
      return method.MethodOnTarget.IsPrivate;
    }

    private MethodGenerator ExplicitlyImplementedInterfaceMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      IInvocationCreationContributor contributor = this.GetContributor(this.GetDelegateType(method, @class, options), method);
      Type invocation = new InheritanceInvocationTypeGenerator(this.targetType, method, (MethodInfo) null, contributor).Generate(@class, options, this.namingScope).BuildType();
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocation, (GetTargetExpressionDelegate) ((c, m) => (Expression) new TypeTokenExpression(this.targetType)), overrideMethod, contributor);
    }

    private IInvocationCreationContributor GetContributor(Type @delegate, MetaMethod method)
    {
      return !@delegate.IsGenericType ? (IInvocationCreationContributor) new InvocationWithDelegateContributor(@delegate, this.targetType, method, this.namingScope) : (IInvocationCreationContributor) new InvocationWithGenericDelegateContributor(@delegate, method, (Reference) new FieldReference(InvocationMethods.ProxyObject));
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

    private Type GetInvocationType(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options)
    {
      return this.BuildInvocationType(method, @class, options);
    }
  }
}
