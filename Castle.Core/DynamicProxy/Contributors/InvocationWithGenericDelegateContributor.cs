// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.InvocationWithGenericDelegateContributor
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
  public class InvocationWithGenericDelegateContributor : IInvocationCreationContributor
  {
    private readonly Type delegateType;
    private readonly MetaMethod method;
    private readonly Reference targetReference;

    public InvocationWithGenericDelegateContributor(
      Type delegateType,
      MetaMethod method,
      Reference targetReference)
    {
      this.delegateType = delegateType;
      this.method = method;
      this.targetReference = targetReference;
    }

    public ConstructorEmitter CreateConstructor(
      ArgumentReference[] baseCtorArguments,
      AbstractTypeEmitter invocation)
    {
      return invocation.CreateConstructor(baseCtorArguments);
    }

    public MethodInvocationExpression GetCallbackMethodInvocation(
      AbstractTypeEmitter invocation,
      Expression[] args,
      Reference targetField,
      MethodEmitter invokeMethodOnTarget)
    {
      return new MethodInvocationExpression(this.GetDelegate(invocation, invokeMethodOnTarget), this.GetCallbackMethod(), args);
    }

    public MethodInfo GetCallbackMethod() => this.delegateType.GetMethod("Invoke");

    public Expression[] GetConstructorInvocationArguments(
      Expression[] arguments,
      ClassEmitter proxy)
    {
      return arguments;
    }

    private Reference GetDelegate(
      AbstractTypeEmitter invocation,
      MethodEmitter invokeMethodOnTarget)
    {
      Type type = this.delegateType.MakeGenericType((Type[]) invocation.GenericTypeParams);
      LocalReference localDelegate = invokeMethodOnTarget.CodeBuilder.DeclareLocal(type);
      MethodInfo closedMethodOnTarget = this.method.MethodOnTarget.MakeGenericMethod((Type[]) invocation.GenericTypeParams);
      ReferenceExpression localTarget = new ReferenceExpression(this.targetReference);
      invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) this.SetDelegate(localDelegate, localTarget, type, closedMethodOnTarget));
      return (Reference) localDelegate;
    }

    private AssignStatement SetDelegate(
      LocalReference localDelegate,
      ReferenceExpression localTarget,
      Type closedDelegateType,
      MethodInfo closedMethodOnTarget)
    {
      MethodInvocationExpression right = new MethodInvocationExpression((Reference) null, DelegateMethods.CreateDelegate, new Expression[3]
      {
        (Expression) new TypeTokenExpression(closedDelegateType),
        (Expression) localTarget,
        (Expression) new MethodTokenExpression(closedMethodOnTarget)
      });
      return new AssignStatement((Reference) localDelegate, (Expression) new ConvertExpression(closedDelegateType, (Expression) right));
    }
  }
}
