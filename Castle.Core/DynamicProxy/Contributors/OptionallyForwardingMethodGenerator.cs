// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.OptionallyForwardingMethodGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class OptionallyForwardingMethodGenerator : MethodGenerator
  {
    private readonly GetTargetReferenceDelegate getTargetReference;

    public OptionallyForwardingMethodGenerator(
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
      Reference reference = this.getTargetReference(@class, this.MethodToOverride);
      emitter.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new IfNullExpression(reference, this.IfNull(emitter.ReturnType), this.IfNotNull(reference))));
      return emitter;
    }

    private Expression IfNotNull(Reference targetReference)
    {
      MultiStatementExpression statementExpression = new MultiStatementExpression();
      ReferenceExpression[] referenceExpression = ArgumentsUtil.ConvertToArgumentReferenceExpression(this.MethodToOverride.GetParameters());
      statementExpression.AddStatement((Statement) new ReturnStatement((Expression) new MethodInvocationExpression(targetReference, this.MethodToOverride, (Expression[]) referenceExpression)
      {
        VirtualCall = true
      }));
      return (Expression) statementExpression;
    }

    private Expression IfNull(Type returnType)
    {
      MultiStatementExpression expression = new MultiStatementExpression();
      this.InitOutParameters(expression, this.MethodToOverride.GetParameters());
      if (returnType == typeof (void))
        expression.AddStatement((Statement) new ReturnStatement());
      else
        expression.AddStatement((Statement) new ReturnStatement((Expression) new DefaultValueExpression(returnType)));
      return (Expression) expression;
    }

    private void InitOutParameters(MultiStatementExpression expression, ParameterInfo[] parameters)
    {
      for (int index = 0; index < parameters.Length; ++index)
      {
        ParameterInfo parameter = parameters[index];
        if (parameter.IsOut)
          expression.AddStatement((Statement) new AssignArgumentStatement(new ArgumentReference(parameter.ParameterType, index + 1), (Expression) new DefaultValueExpression(parameter.ParameterType)));
      }
    }
  }
}
