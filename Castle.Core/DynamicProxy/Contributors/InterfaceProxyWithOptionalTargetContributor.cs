// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InterfaceProxyWithOptionalTargetContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InterfaceProxyWithOptionalTargetContributor : InterfaceProxyWithoutTargetContributor
  {
    private readonly GetTargetReferenceDelegate getTargetReference;

    public InterfaceProxyWithOptionalTargetContributor(
      INamingScope namingScope,
      GetTargetExpressionDelegate getTarget,
      GetTargetReferenceDelegate getTargetReference)
      : base(namingScope, getTarget)
    {
      this.getTargetReference = getTargetReference;
    }

    protected override MethodGenerator GetMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      return !method.Proxyable ? (MethodGenerator) new OptionallyForwardingMethodGenerator(method, overrideMethod, this.getTargetReference) : base.GetMethodGenerator(method, @class, options, overrideMethod);
    }
  }
}
