// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MethodWithInvocationGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Tokens;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MethodWithInvocationGenerator : MethodGenerator
  {
    private readonly Reference interceptors;
    private readonly GetTargetExpressionDelegate getTargetExpression;
    private readonly Type invocation;
    private readonly IInvocationCreationContributor contributor;

    public MethodWithInvocationGenerator(
      MetaMethod method,
      Reference interceptors,
      Type invocation,
      GetTargetExpressionDelegate getTargetExpression,
      OverrideMethodDelegate createMethod,
      IInvocationCreationContributor contributor)
      : base(method, createMethod)
    {
      this.invocation = invocation;
      this.getTargetExpression = getTargetExpression;
      this.interceptors = interceptors;
      this.contributor = contributor;
    }

    protected override MethodEmitter BuildProxiedMethodBody(
      MethodEmitter emitter,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      Type type = this.invocation;
      Trace.Assert(this.MethodToOverride.IsGenericMethod == type.IsGenericTypeDefinition);
      Type[] typeArray = Type.EmptyTypes;
      ConstructorInfo constructor = this.invocation.GetConstructors()[0];
      Expression proxiedMethodTokenExpression;
      if (this.MethodToOverride.IsGenericMethod)
      {
        typeArray = emitter.MethodBuilder.GetGenericArguments();
        type = type.MakeGenericType(typeArray);
        constructor = TypeBuilder.GetConstructor(type, constructor);
        proxiedMethodTokenExpression = (Expression) new MethodTokenExpression(this.MethodToOverride.MakeGenericMethod(typeArray));
      }
      else
      {
        FieldReference staticField = @class.CreateStaticField(namingScope.GetUniqueName("token_" + this.MethodToOverride.Name), typeof (MethodInfo));
        @class.ClassConstructor.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) staticField, (Expression) new MethodTokenExpression(this.MethodToOverride)));
        proxiedMethodTokenExpression = staticField.ToExpression();
      }
      TypeReference[] dereferencedArguments = IndirectReference.WrapIfByRef((TypeReference[]) emitter.Arguments);
      Expression[] ctorArguments = this.GetCtorArguments(@class, namingScope, proxiedMethodTokenExpression, dereferencedArguments);
      Expression[] expressionArray = this.ModifyArguments(@class, ctorArguments);
      LocalReference localReference = emitter.CodeBuilder.DeclareLocal(type);
      emitter.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) localReference, (Expression) new NewInstanceExpression(constructor, expressionArray)));
      if (this.MethodToOverride.ContainsGenericParameters)
        this.EmitLoadGenricMethodArguments(emitter, this.MethodToOverride.MakeGenericMethod(typeArray), (Reference) localReference);
      ExpressionStatement stmt = new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) localReference, InvocationMethods.Proceed, new Expression[0]));
      emitter.CodeBuilder.AddStatement((Statement) stmt);
      GeneratorUtil.CopyOutAndRefParameters(dereferencedArguments, localReference, this.MethodToOverride, emitter);
      if (this.MethodToOverride.ReturnType != typeof (void))
      {
        MethodInvocationExpression right = new MethodInvocationExpression((Reference) localReference, InvocationMethods.GetReturnValue, new Expression[0]);
        emitter.CodeBuilder.AddStatement((Statement) new ReturnStatement((Expression) new ConvertExpression(emitter.ReturnType, (Expression) right)));
      }
      else
        emitter.CodeBuilder.AddStatement((Statement) new ReturnStatement());
      return emitter;
    }

    private Expression[] ModifyArguments(ClassEmitter @class, Expression[] arguments)
    {
      return this.contributor == null ? arguments : this.contributor.GetConstructorInvocationArguments(arguments, @class);
    }

    private Expression[] GetCtorArguments(
      ClassEmitter @class,
      INamingScope namingScope,
      Expression proxiedMethodTokenExpression,
      TypeReference[] dereferencedArguments)
    {
      FieldReference field = @class.GetField("__selector");
      return field != null ? new Expression[7]
      {
        this.getTargetExpression(@class, this.MethodToOverride),
        SelfReference.Self.ToExpression(),
        this.interceptors.ToExpression(),
        proxiedMethodTokenExpression,
        (Expression) new ReferencesToObjectArrayExpression(dereferencedArguments),
        field.ToExpression(),
        (Expression) new AddressOfReferenceExpression((Reference) this.BuildMethodInterceptorsField(@class, this.MethodToOverride, namingScope))
      } : new Expression[5]
      {
        this.getTargetExpression(@class, this.MethodToOverride),
        SelfReference.Self.ToExpression(),
        this.interceptors.ToExpression(),
        proxiedMethodTokenExpression,
        (Expression) new ReferencesToObjectArrayExpression(dereferencedArguments)
      };
    }

    protected FieldReference BuildMethodInterceptorsField(
      ClassEmitter @class,
      MethodInfo method,
      INamingScope namingScope)
    {
      FieldReference field = @class.CreateField(namingScope.GetUniqueName(string.Format("interceptors_{0}", (object) method.Name)), typeof (IInterceptor[]), false);
      @class.DefineCustomAttributeFor<XmlIgnoreAttribute>(field);
      return field;
    }

    private void EmitLoadGenricMethodArguments(
      MethodEmitter methodEmitter,
      MethodInfo method,
      Reference invocationLocal)
    {
      Type[] all = Array.FindAll<Type>(method.GetGenericArguments(), (Predicate<Type>) (t => t.IsGenericParameter));
      LocalReference localReference = methodEmitter.CodeBuilder.DeclareLocal(typeof (Type[]));
      methodEmitter.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) localReference, (Expression) new NewArrayExpression(all.Length, typeof (Type))));
      for (int targetPosition = 0; targetPosition < all.Length; ++targetPosition)
        methodEmitter.CodeBuilder.AddStatement((Statement) new AssignArrayStatement((Reference) localReference, targetPosition, (Expression) new TypeTokenExpression(all[targetPosition])));
      methodEmitter.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression(invocationLocal, InvocationMethods.SetGenericMethodArguments, new Expression[1]
      {
        (Expression) new ReferenceExpression((Reference) localReference)
      })));
    }
  }
}
