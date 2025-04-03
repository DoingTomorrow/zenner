// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ForwardingMethodGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class ForwardingMethodGenerator : MethodGenerator
  {
    private readonly GetTargetReferenceDelegate getTargetReference;

    public ForwardingMethodGenerator(
      MetaMethod method,
      OverrideMethodDelegate overrideMethod,
      GetTargetReferenceDelegate getTargetReference)
      : base(method, overrideMethod)
    {
      this.getTargetReference = getTargetReference;
    }

    protected override MethodEmitter BuildProxiedMethodBody(
      MethodEmitter emitter,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      Reference owner = this.getTargetReference(@class, this.MethodToOverride);
      ReferenceExpression[] referenceExpression = ArgumentsUtil.ConvertToArgumentReferenceExpression(this.MethodToOverride.GetParameters());
      emitter.CodeBuilder.AddStatement((Statement) new ReturnStatement((Expression) new MethodInvocationExpression(owner, this.MethodToOverride, (Expression[]) referenceExpression)
      {
        VirtualCall = true
      }));
      return emitter;
    }
  }
}
