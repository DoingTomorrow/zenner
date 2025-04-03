// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.MinimialisticMethodGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class MinimialisticMethodGenerator(
    MetaMethod method,
    OverrideMethodDelegate overrideMethod) : MethodGenerator(method, overrideMethod)
  {
    protected override MethodEmitter BuildProxiedMethodBody(
      MethodEmitter emitter,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      this.InitOutParameters(emitter, this.MethodToOverride.GetParameters());
      if (emitter.ReturnType == typeof (void))
        emitter.CodeBuilder.AddStatement((Statement) new ReturnStatement());
      else
        emitter.CodeBuilder.AddStatement((Statement) new ReturnStatement((Expression) new DefaultValueExpression(emitter.ReturnType)));
      return emitter;
    }

    private void InitOutParameters(MethodEmitter emitter, ParameterInfo[] parameters)
    {
      for (int index = 0; index < parameters.Length; ++index)
      {
        ParameterInfo parameter = parameters[index];
        if (parameter.IsOut)
          emitter.CodeBuilder.AddStatement((Statement) new AssignArgumentStatement(new ArgumentReference(parameter.ParameterType, index + 1), (Expression) new DefaultValueExpression(parameter.ParameterType)));
      }
    }
  }
}
