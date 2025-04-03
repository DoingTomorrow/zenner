// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.CompositionInvocationTypeGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class CompositionInvocationTypeGenerator(
    Type target,
    MetaMethod method,
    MethodInfo callback,
    bool canChangeTarget,
    IInvocationCreationContributor contributor) : InvocationTypeGenerator(target, method, callback, canChangeTarget, contributor)
  {
    public static readonly Type BaseType = typeof (CompositionInvocation);

    protected override FieldReference GetTargetReference()
    {
      return new FieldReference(InvocationMethods.Target);
    }

    protected override Type GetBaseType() => CompositionInvocationTypeGenerator.BaseType;

    protected override void ImplementInvokeMethodOnTarget(
      AbstractTypeEmitter invocation,
      ParameterInfo[] parameters,
      MethodEmitter invokeMethodOnTarget,
      Reference targetField)
    {
      invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.EnsureValidTarget, new Expression[0])));
      base.ImplementInvokeMethodOnTarget(invocation, parameters, invokeMethodOnTarget, targetField);
    }

    protected override ArgumentReference[] GetBaseCtorArguments(
      Type targetFieldType,
      ProxyGenerationOptions proxyGenerationOptions,
      out ConstructorInfo baseConstructor)
    {
      if (proxyGenerationOptions.Selector == null)
      {
        baseConstructor = InvocationMethods.CompositionInvocationConstructorNoSelector;
        return new ArgumentReference[5]
        {
          new ArgumentReference(targetFieldType),
          new ArgumentReference(typeof (object)),
          new ArgumentReference(typeof (IInterceptor[])),
          new ArgumentReference(typeof (MethodInfo)),
          new ArgumentReference(typeof (object[]))
        };
      }
      baseConstructor = InvocationMethods.CompositionInvocationConstructorWithSelector;
      return new ArgumentReference[7]
      {
        new ArgumentReference(targetFieldType),
        new ArgumentReference(typeof (object)),
        new ArgumentReference(typeof (IInterceptor[])),
        new ArgumentReference(typeof (MethodInfo)),
        new ArgumentReference(typeof (object[])),
        new ArgumentReference(typeof (IInterceptorSelector)),
        new ArgumentReference(typeof (IInterceptor[]).MakeByRefType())
      };
    }
  }
}
