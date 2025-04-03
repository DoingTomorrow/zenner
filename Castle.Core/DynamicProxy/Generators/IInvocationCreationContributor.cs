// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.IInvocationCreationContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public interface IInvocationCreationContributor
  {
    ConstructorEmitter CreateConstructor(
      ArgumentReference[] baseCtorArguments,
      AbstractTypeEmitter invocation);

    MethodInvocationExpression GetCallbackMethodInvocation(
      AbstractTypeEmitter invocation,
      Expression[] args,
      Reference targetField,
      MethodEmitter invokeMethodOnTarget);

    MethodInfo GetCallbackMethod();

    Expression[] GetConstructorInvocationArguments(Expression[] arguments, ClassEmitter proxy);
  }
}
