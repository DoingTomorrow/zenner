// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MethodGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public abstract class MethodGenerator : IGenerator<MethodEmitter>
  {
    private readonly MetaMethod method;
    private readonly OverrideMethodDelegate overrideMethod;

    protected MethodInfo MethodToOverride => this.method.Method;

    protected MethodInfo MethodOnTarget => this.method.MethodOnTarget;

    protected MethodGenerator(MetaMethod method, OverrideMethodDelegate overrideMethod)
    {
      this.method = method;
      this.overrideMethod = overrideMethod;
    }

    public MethodEmitter Generate(
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      MethodEmitter methodEmitter = this.BuildProxiedMethodBody(this.overrideMethod(this.method.Name, this.method.Attributes, this.MethodToOverride), @class, options, namingScope);
      if (this.MethodToOverride.DeclaringType.IsInterface)
        @class.TypeBuilder.DefineMethodOverride((MethodInfo) methodEmitter.MethodBuilder, this.MethodToOverride);
      return methodEmitter;
    }

    protected abstract MethodEmitter BuildProxiedMethodBody(
      MethodEmitter emitter,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope);
  }
}
