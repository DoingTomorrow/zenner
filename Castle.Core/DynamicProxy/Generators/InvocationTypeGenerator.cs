// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.InvocationTypeGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public abstract class InvocationTypeGenerator : IGenerator<AbstractTypeEmitter>
  {
    protected readonly Type targetType;
    protected readonly MetaMethod method;
    private readonly MethodInfo callback;
    private readonly bool canChangeTarget;
    private readonly IInvocationCreationContributor contributor;

    protected InvocationTypeGenerator(
      Type targetType,
      MetaMethod method,
      MethodInfo callback,
      bool canChangeTarget,
      IInvocationCreationContributor contributor)
    {
      this.targetType = targetType;
      this.method = method;
      this.callback = callback;
      this.canChangeTarget = canChangeTarget;
      this.contributor = contributor;
    }

    public AbstractTypeEmitter Generate(
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      MethodInfo method = this.method.Method;
      Type[] interfaces = new Type[0];
      if (this.canChangeTarget)
        interfaces = new Type[1]
        {
          typeof (IChangeProxyTarget)
        };
      AbstractTypeEmitter emitter = this.GetEmitter(@class, interfaces, namingScope, method);
      emitter.CopyGenericParametersFromMethod(method);
      this.CreateConstructor(emitter, options);
      FieldReference targetReference = this.GetTargetReference();
      if (this.canChangeTarget)
        this.ImplementChangeProxyTargetInterface(@class, emitter, targetReference);
      this.ImplemementInvokeMethodOnTarget(emitter, method.GetParameters(), targetReference, this.callback);
      emitter.DefineCustomAttribute<SerializableAttribute>();
      return emitter;
    }

    private void CreateConstructor(AbstractTypeEmitter invocation, ProxyGenerationOptions options)
    {
      ConstructorInfo baseConstructor;
      ArgumentReference[] baseCtorArguments = this.GetBaseCtorArguments(this.targetType, options, out baseConstructor);
      ConstructorEmitter constructor = this.CreateConstructor(invocation, baseCtorArguments);
      constructor.CodeBuilder.InvokeBaseConstructor(baseConstructor, baseCtorArguments);
      constructor.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    private ConstructorEmitter CreateConstructor(
      AbstractTypeEmitter invocation,
      ArgumentReference[] baseCtorArguments)
    {
      return this.contributor == null ? invocation.CreateConstructor(baseCtorArguments) : this.contributor.CreateConstructor(baseCtorArguments, invocation);
    }

    protected abstract FieldReference GetTargetReference();

    private AbstractTypeEmitter GetEmitter(
      ClassEmitter @class,
      Type[] interfaces,
      INamingScope namingScope,
      MethodInfo methodInfo)
    {
      string suggestedName = string.Format("Castle.Proxies.Invocations.{0}_{1}", (object) methodInfo.DeclaringType.Name, (object) methodInfo.Name);
      string uniqueName = namingScope.ParentScope.GetUniqueName(suggestedName);
      return (AbstractTypeEmitter) new ClassEmitter(@class.ModuleScope, uniqueName, this.GetBaseType(), (IEnumerable<Type>) interfaces);
    }

    protected abstract Type GetBaseType();

    private void ImplementChangeProxyTargetInterface(
      ClassEmitter @class,
      AbstractTypeEmitter invocation,
      FieldReference targetField)
    {
      this.ImplementChangeInvocationTarget(invocation, targetField);
      this.ImplementChangeProxyTarget(invocation, @class);
    }

    private void ImplementChangeProxyTarget(AbstractTypeEmitter invocation, ClassEmitter @class)
    {
      MethodEmitter method = invocation.CreateMethod("ChangeProxyTarget", typeof (void), typeof (object));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new ConvertExpression((Type) @class.TypeBuilder, new FieldReference(InvocationMethods.ProxyObject).ToExpression())));
      FieldReference field = @class.GetField("__target");
      MethodCodeBuilder codeBuilder = method.CodeBuilder;
      FieldReference target = new FieldReference(field.Reference);
      target.OwnerReference = (Reference) null;
      AssignStatement stmt = new AssignStatement((Reference) target, (Expression) new ConvertExpression(field.Fieldbuilder.FieldType, method.Arguments[0].ToExpression()));
      codeBuilder.AddStatement((Statement) stmt);
      method.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    private void ImplementChangeInvocationTarget(
      AbstractTypeEmitter invocation,
      FieldReference targetField)
    {
      MethodEmitter method = invocation.CreateMethod("ChangeInvocationTarget", typeof (void), typeof (object));
      method.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) targetField, (Expression) new ConvertExpression(this.targetType, method.Arguments[0].ToExpression())));
      method.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    private void ImplemementInvokeMethodOnTarget(
      AbstractTypeEmitter invocation,
      ParameterInfo[] parameters,
      FieldReference targetField,
      MethodInfo callbackMethod)
    {
      MethodEmitter method = invocation.CreateMethod("InvokeMethodOnTarget", typeof (void));
      this.ImplementInvokeMethodOnTarget(invocation, parameters, method, (Reference) targetField);
    }

    protected virtual void ImplementInvokeMethodOnTarget(
      AbstractTypeEmitter invocation,
      ParameterInfo[] parameters,
      MethodEmitter invokeMethodOnTarget,
      Reference targetField)
    {
      MethodInfo callbackMethod = this.GetCallbackMethod(invocation);
      if (callbackMethod == null)
      {
        this.EmitCallThrowOnNoTarget(invokeMethodOnTarget);
      }
      else
      {
        if (this.canChangeTarget)
          this.EmitCallEnsureValidTarget(invokeMethodOnTarget);
        Expression[] args = new Expression[parameters.Length];
        Dictionary<int, LocalReference> dictionary = new Dictionary<int, LocalReference>();
        for (int key = 0; key < parameters.Length; ++key)
        {
          ParameterInfo parameter = parameters[key];
          Type closedParameterType = invocation.GetClosedParameterType(parameter.ParameterType);
          if (closedParameterType.IsByRef)
          {
            LocalReference localReference = invokeMethodOnTarget.CodeBuilder.DeclareLocal(closedParameterType.GetElementType());
            invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) localReference, (Expression) new ConvertExpression(closedParameterType.GetElementType(), (Expression) new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.GetArgumentValue, new Expression[1]
            {
              (Expression) new LiteralIntExpression(key)
            }))));
            ByRefReference byRefReference = new ByRefReference(localReference);
            args[key] = (Expression) new ReferenceExpression((Reference) byRefReference);
            dictionary[key] = localReference;
          }
          else
            args[key] = (Expression) new ConvertExpression(closedParameterType, (Expression) new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.GetArgumentValue, new Expression[1]
            {
              (Expression) new LiteralIntExpression(key)
            }));
        }
        MethodInvocationExpression methodInvocation = this.GetCallbackMethodInvocation(invocation, args, callbackMethod, targetField, invokeMethodOnTarget);
        LocalReference target = (LocalReference) null;
        if (callbackMethod.ReturnType != typeof (void))
        {
          Type closedParameterType = invocation.GetClosedParameterType(callbackMethod.ReturnType);
          target = invokeMethodOnTarget.CodeBuilder.DeclareLocal(closedParameterType);
          invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) target, (Expression) methodInvocation));
        }
        else
          invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) methodInvocation));
        foreach (KeyValuePair<int, LocalReference> keyValuePair in dictionary)
        {
          int key = keyValuePair.Key;
          LocalReference localReference = keyValuePair.Value;
          invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.SetArgumentValue, new Expression[2]
          {
            (Expression) new LiteralIntExpression(key),
            (Expression) new ConvertExpression(typeof (object), localReference.Type, (Expression) new ReferenceExpression((Reference) localReference))
          })));
        }
        if (callbackMethod.ReturnType != typeof (void))
        {
          MethodInvocationExpression invocationExpression = new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.SetReturnValue, new Expression[1]
          {
            (Expression) new ConvertExpression(typeof (object), target.Type, target.ToExpression())
          });
          invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) invocationExpression));
        }
        invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ReturnStatement());
      }
    }

    private void EmitCallThrowOnNoTarget(MethodEmitter invokeMethodOnTarget)
    {
      ExpressionStatement stmt = new ExpressionStatement((Expression) new MethodInvocationExpression(InvocationMethods.ThrowOnNoTarget, new Expression[0]));
      invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) stmt);
      invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    private AbstractCodeBuilder EmitCallEnsureValidTarget(MethodEmitter invokeMethodOnTarget)
    {
      return invokeMethodOnTarget.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) SelfReference.Self, InvocationMethods.EnsureValidTarget, new Expression[0])));
    }

    protected virtual MethodInvocationExpression GetCallbackMethodInvocation(
      AbstractTypeEmitter invocation,
      Expression[] args,
      MethodInfo callbackMethod,
      Reference targetField,
      MethodEmitter invokeMethodOnTarget)
    {
      if (this.contributor != null)
        return this.contributor.GetCallbackMethodInvocation(invocation, args, targetField, invokeMethodOnTarget);
      return new MethodInvocationExpression((Reference) new AsTypeReference(targetField, callbackMethod.DeclaringType), callbackMethod, args)
      {
        VirtualCall = true
      };
    }

    protected abstract ArgumentReference[] GetBaseCtorArguments(
      Type targetFieldType,
      ProxyGenerationOptions proxyGenerationOptions,
      out ConstructorInfo baseConstructor);

    private MethodInfo GetCallbackMethod(AbstractTypeEmitter invocation)
    {
      if (this.contributor != null)
        return this.contributor.GetCallbackMethod();
      MethodInfo callback = this.callback;
      if (callback == null)
        return (MethodInfo) null;
      return !callback.IsGenericMethod ? callback : callback.MakeGenericMethod(invocation.GetGenericArgumentsFor(callback));
    }
  }
}
