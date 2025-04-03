// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InvocationWithDelegateContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class InvocationWithDelegateContributor : IInvocationCreationContributor
  {
    private readonly Type delegateType;
    private readonly Type targetType;
    private readonly MetaMethod method;
    private readonly INamingScope namingScope;

    public InvocationWithDelegateContributor(
      Type delegateType,
      Type targetType,
      MetaMethod method,
      INamingScope namingScope)
    {
      this.delegateType = delegateType;
      this.targetType = targetType;
      this.method = method;
      this.namingScope = namingScope;
    }

    public ConstructorEmitter CreateConstructor(
      ArgumentReference[] baseCtorArguments,
      AbstractTypeEmitter invocation)
    {
      ArgumentReference[] arguments = this.GetArguments(baseCtorArguments);
      ConstructorEmitter constructor = invocation.CreateConstructor(arguments);
      FieldReference field = invocation.CreateField("delegate", this.delegateType);
      constructor.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) field, (Expression) new ReferenceExpression((Reference) arguments[0])));
      return constructor;
    }

    public MethodInvocationExpression GetCallbackMethodInvocation(
      AbstractTypeEmitter invocation,
      Expression[] args,
      Reference targetField,
      MethodEmitter invokeMethodOnTarget)
    {
      Expression[] allArgs = this.GetAllArgs(args, targetField);
      return new MethodInvocationExpression((Reference) invocation.GetField("delegate"), this.GetCallbackMethod(), allArgs);
    }

    public MethodInfo GetCallbackMethod() => this.delegateType.GetMethod("Invoke");

    public Expression[] GetConstructorInvocationArguments(
      Expression[] arguments,
      ClassEmitter proxy)
    {
      Expression[] destinationArray = new Expression[arguments.Length + 1];
      destinationArray[0] = (Expression) new ReferenceExpression((Reference) this.BuildDelegateToken(proxy));
      Array.Copy((Array) arguments, 0, (Array) destinationArray, 1, arguments.Length);
      return destinationArray;
    }

    private FieldReference BuildDelegateToken(ClassEmitter proxy)
    {
      FieldReference staticField = proxy.CreateStaticField(this.namingScope.GetUniqueName("callback_" + this.method.Method.Name), this.delegateType);
      MethodInvocationExpression right = new MethodInvocationExpression((Reference) null, DelegateMethods.CreateDelegate, new Expression[3]
      {
        (Expression) new TypeTokenExpression(this.delegateType),
        (Expression) NullExpression.Instance,
        (Expression) new MethodTokenExpression(this.method.MethodOnTarget)
      });
      AssignStatement stmt = new AssignStatement((Reference) staticField, (Expression) new ConvertExpression(this.delegateType, (Expression) right));
      proxy.ClassConstructor.CodeBuilder.AddStatement((Statement) stmt);
      return staticField;
    }

    private ArgumentReference[] GetArguments(ArgumentReference[] baseCtorArguments)
    {
      ArgumentReference[] arguments = new ArgumentReference[baseCtorArguments.Length + 1];
      arguments[0] = new ArgumentReference(this.delegateType);
      baseCtorArguments.CopyTo((Array) arguments, 1);
      return arguments;
    }

    private Expression[] GetAllArgs(Expression[] args, Reference targetField)
    {
      Expression[] allArgs = new Expression[args.Length + 1];
      args.CopyTo((Array) allArgs, 1);
      allArgs[0] = (Expression) new ConvertExpression(this.targetType, targetField.ToExpression());
      return allArgs;
    }
  }
}
