// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ProxyInstanceContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Serialization;
using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection.Emit;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public abstract class ProxyInstanceContributor : ITypeContributor
  {
    protected readonly Type targetType;
    private readonly string proxyTypeId;
    private readonly Type[] interfaces;

    protected ProxyInstanceContributor(Type targetType, Type[] interfaces, string proxyTypeId)
    {
      this.targetType = targetType;
      this.proxyTypeId = proxyTypeId;
      this.interfaces = interfaces ?? Type.EmptyTypes;
    }

    protected abstract Expression GetTargetReferenceExpression(ClassEmitter emitter);

    public virtual void Generate(ClassEmitter @class, ProxyGenerationOptions options)
    {
      FieldReference field = @class.GetField("__interceptors");
      this.ImplementGetObjectData(@class);
      this.ImplementProxyTargetAccessor(@class, field);
      foreach (CustomAttributeBuilder inheritableAttribute in this.targetType.GetNonInheritableAttributes())
        @class.DefineCustomAttribute(inheritableAttribute);
    }

    protected void ImplementProxyTargetAccessor(
      ClassEmitter emitter,
      FieldReference interceptorsField)
    {
      emitter.CreateMethod("DynProxyGetTarget", typeof (object)).CodeBuilder.AddStatement((Statement) new ReturnStatement((Expression) new ConvertExpression(typeof (object), this.targetType, this.GetTargetReferenceExpression(emitter))));
      emitter.CreateMethod("GetInterceptors", typeof (IInterceptor[])).CodeBuilder.AddStatement((Statement) new ReturnStatement((Reference) interceptorsField));
    }

    protected void ImplementGetObjectData(ClassEmitter emitter)
    {
      MethodEmitter method = emitter.CreateMethod("GetObjectData", typeof (void), typeof (SerializationInfo), typeof (StreamingContext));
      ArgumentReference argumentReference = method.Arguments[0];
      LocalReference target = method.CodeBuilder.DeclareLocal(typeof (Type));
      method.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) target, (Expression) new MethodInvocationExpression((Reference) null, TypeMethods.StaticGetType, new Expression[3]
      {
        new ConstReference((object) typeof (ProxyObjectReference).AssemblyQualifiedName).ToExpression(),
        new ConstReference((object) 1).ToExpression(),
        new ConstReference((object) 0).ToExpression()
      })));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) argumentReference, SerializationInfoMethods.SetType, new Expression[1]
      {
        target.ToExpression()
      })));
      foreach (FieldReference allField in emitter.GetAllFields())
      {
        if (!allField.Reference.IsStatic && !allField.Reference.IsNotSerialized)
          this.AddAddValueInvocation(argumentReference, method, allField);
      }
      LocalReference localReference = method.CodeBuilder.DeclareLocal(typeof (string[]));
      method.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) localReference, (Expression) new NewArrayExpression(this.interfaces.Length, typeof (string))));
      for (int targetPosition = 0; targetPosition < this.interfaces.Length; ++targetPosition)
        method.CodeBuilder.AddStatement((Statement) new AssignArrayStatement((Reference) localReference, targetPosition, new ConstReference((object) this.interfaces[targetPosition].AssemblyQualifiedName).ToExpression()));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) argumentReference, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__interfaces").ToExpression(),
        localReference.ToExpression()
      })));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) argumentReference, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__baseType").ToExpression(),
        new ConstReference((object) emitter.BaseType.AssemblyQualifiedName).ToExpression()
      })));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) argumentReference, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__proxyGenerationOptions").ToExpression(),
        emitter.GetField("proxyGenerationOptions").ToExpression()
      })));
      method.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) argumentReference, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__proxyTypeId").ToExpression(),
        new ConstReference((object) this.proxyTypeId).ToExpression()
      })));
      this.CustomizeGetObjectData((AbstractCodeBuilder) method.CodeBuilder, argumentReference, method.Arguments[1], emitter);
      method.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    protected virtual void AddAddValueInvocation(
      ArgumentReference serializationInfo,
      MethodEmitter getObjectData,
      FieldReference field)
    {
      getObjectData.CodeBuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) serializationInfo, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) field.Reference.Name).ToExpression(),
        field.ToExpression()
      })));
    }

    protected abstract void CustomizeGetObjectData(
      AbstractCodeBuilder builder,
      ArgumentReference serializationInfo,
      ArgumentReference streamingContext,
      ClassEmitter emitter);

    public void CollectElementsToProxy(IProxyGenerationHook hook, MetaType model)
    {
    }
  }
}
