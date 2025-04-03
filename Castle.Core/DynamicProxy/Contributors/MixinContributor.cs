// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.MixinContributor
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
  public class MixinContributor : CompositeTypeContributor
  {
    private readonly bool canChangeTarget;
    private readonly IList<Type> empty = (IList<Type>) new List<Type>();
    private readonly IDictionary<Type, FieldReference> fields = (IDictionary<Type, FieldReference>) new Dictionary<Type, FieldReference>();
    private readonly GetTargetExpressionDelegate getTargetExpression;

    public MixinContributor(INamingScope namingScope, bool canChangeTarget)
      : base(namingScope)
    {
      this.canChangeTarget = canChangeTarget;
      this.getTargetExpression = this.BuildGetTargetExpression();
    }

    public IEnumerable<FieldReference> Fields => (IEnumerable<FieldReference>) this.fields.Values;

    private GetTargetExpressionDelegate BuildGetTargetExpression()
    {
      return !this.canChangeTarget ? (GetTargetExpressionDelegate) ((c, m) => this.fields[m.DeclaringType].ToExpression()) : (GetTargetExpressionDelegate) ((c, m) => (Expression) new NullCoalescingOperatorExpression(new AsTypeReference((Reference) c.GetField("__target"), m.DeclaringType).ToExpression(), this.fields[m.DeclaringType].ToExpression()));
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

    public override void Generate(ClassEmitter @class, ProxyGenerationOptions options)
    {
      foreach (Type type in (IEnumerable<Type>) this.interfaces)
        this.fields[type] = this.BuildTargetField(@class, type);
      foreach (Type type in (IEnumerable<Type>) this.empty)
        this.fields[type] = this.BuildTargetField(@class, type);
      base.Generate(@class, options);
    }

    public void AddEmptyInterface(Type @interface) => this.empty.Add(@interface);

    protected override MethodGenerator GetMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      if (!method.Proxyable)
        return (MethodGenerator) new ForwardingMethodGenerator(method, overrideMethod, (GetTargetReferenceDelegate) ((c, i) => (Reference) this.fields[i.DeclaringType]));
      Type invocationType = this.GetInvocationType(method, @class, options);
      return (MethodGenerator) new MethodWithInvocationGenerator(method, (Reference) @class.GetField("__interceptors"), invocationType, this.getTargetExpression, overrideMethod, (IInvocationCreationContributor) null);
    }

    private Type GetInvocationType(
      MetaMethod method,
      ClassEmitter emitter,
      ProxyGenerationOptions options)
    {
      ModuleScope moduleScope = emitter.ModuleScope;
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
      Type type = new CompositionInvocationTypeGenerator(method.Method.DeclaringType, method, method.Method, this.canChangeTarget, (IInvocationCreationContributor) null).Generate(emitter, options, this.namingScope).BuildType();
      moduleScope.RegisterInCache(key, type);
      return type;
    }

    private FieldReference BuildTargetField(ClassEmitter @class, Type type)
    {
      string suggestedName = "__mixin_" + type.FullName.Replace(".", "_");
      return @class.CreateField(this.namingScope.GetUniqueName(suggestedName), type);
    }
  }
}
