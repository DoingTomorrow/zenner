// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.ClassProxyInstanceContributor
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
using System.Reflection.Emit;
using System.Runtime.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class ClassProxyInstanceContributor : ProxyInstanceContributor
  {
    private readonly bool delegateToBaseGetObjectData;
    private readonly bool implementISerializable;
    private ConstructorInfo serializationConstructor;
    private readonly IList<FieldReference> serializedFields = (IList<FieldReference>) new List<FieldReference>();

    public ClassProxyInstanceContributor(
      Type targetType,
      IList<MethodInfo> methodsToSkip,
      Type[] interfaces,
      string typeId)
      : base(targetType, interfaces, typeId)
    {
      if (!targetType.IsSerializable)
        return;
      this.implementISerializable = true;
      this.delegateToBaseGetObjectData = this.VerifyIfBaseImplementsGetObjectData(targetType, methodsToSkip);
    }

    protected override Expression GetTargetReferenceExpression(ClassEmitter emitter)
    {
      return SelfReference.Self.ToExpression();
    }

    public override void Generate(ClassEmitter @class, ProxyGenerationOptions options)
    {
      FieldReference field = @class.GetField("__interceptors");
      if (this.implementISerializable)
      {
        this.ImplementGetObjectData(@class);
        this.Constructor(@class);
      }
      this.ImplementProxyTargetAccessor(@class, field);
      foreach (CustomAttributeBuilder inheritableAttribute in this.targetType.GetNonInheritableAttributes())
        @class.DefineCustomAttribute(inheritableAttribute);
    }

    protected override void AddAddValueInvocation(
      ArgumentReference serializationInfo,
      MethodEmitter getObjectData,
      FieldReference field)
    {
      this.serializedFields.Add(field);
      base.AddAddValueInvocation(serializationInfo, getObjectData, field);
    }

    protected override void CustomizeGetObjectData(
      AbstractCodeBuilder codebuilder,
      ArgumentReference serializationInfo,
      ArgumentReference streamingContext,
      ClassEmitter emitter)
    {
      codebuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression((Reference) serializationInfo, SerializationInfoMethods.AddValue_Bool, new Expression[2]
      {
        new ConstReference((object) "__delegateToBase").ToExpression(),
        new ConstReference((object) this.delegateToBaseGetObjectData).ToExpression()
      })));
      if (!this.delegateToBaseGetObjectData)
        this.EmitCustomGetObjectData(codebuilder, serializationInfo);
      else
        this.EmitCallToBaseGetObjectData(codebuilder, serializationInfo, streamingContext);
    }

    private void EmitCustomGetObjectData(
      AbstractCodeBuilder codebuilder,
      ArgumentReference serializationInfo)
    {
      LocalReference target1 = codebuilder.DeclareLocal(typeof (MemberInfo[]));
      LocalReference target2 = codebuilder.DeclareLocal(typeof (object[]));
      MethodInvocationExpression invocationExpression1 = new MethodInvocationExpression((Reference) null, FormatterServicesMethods.GetSerializableMembers, new Expression[1]
      {
        (Expression) new TypeTokenExpression(this.targetType)
      });
      codebuilder.AddStatement((Statement) new AssignStatement((Reference) target1, (Expression) invocationExpression1));
      MethodInvocationExpression invocationExpression2 = new MethodInvocationExpression((Reference) null, TypeUtilMethods.Sort, new Expression[1]
      {
        target1.ToExpression()
      });
      codebuilder.AddStatement((Statement) new AssignStatement((Reference) target1, (Expression) invocationExpression2));
      MethodInvocationExpression invocationExpression3 = new MethodInvocationExpression((Reference) null, FormatterServicesMethods.GetObjectData, new Expression[2]
      {
        SelfReference.Self.ToExpression(),
        target1.ToExpression()
      });
      codebuilder.AddStatement((Statement) new AssignStatement((Reference) target2, (Expression) invocationExpression3));
      MethodInvocationExpression invocationExpression4 = new MethodInvocationExpression((Reference) serializationInfo, SerializationInfoMethods.AddValue_Object, new Expression[2]
      {
        new ConstReference((object) "__data").ToExpression(),
        target2.ToExpression()
      });
      codebuilder.AddStatement((Statement) new ExpressionStatement((Expression) invocationExpression4));
    }

    private void EmitCallToBaseGetObjectData(
      AbstractCodeBuilder codebuilder,
      ArgumentReference serializationInfo,
      ArgumentReference streamingContext)
    {
      MethodInfo method = this.targetType.GetMethod("GetObjectData", new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      });
      codebuilder.AddStatement((Statement) new ExpressionStatement((Expression) new MethodInvocationExpression(method, new Expression[2]
      {
        serializationInfo.ToExpression(),
        streamingContext.ToExpression()
      })));
    }

    private void Constructor(ClassEmitter emitter)
    {
      if (!this.delegateToBaseGetObjectData)
        return;
      this.GenerateSerializationConstructor(emitter);
    }

    private void GenerateSerializationConstructor(ClassEmitter emitter)
    {
      ArgumentReference owner = new ArgumentReference(typeof (SerializationInfo));
      ArgumentReference argumentReference = new ArgumentReference(typeof (StreamingContext));
      ConstructorEmitter constructor = emitter.CreateConstructor(owner, argumentReference);
      constructor.CodeBuilder.AddStatement((Statement) new ConstructorInvocationStatement(this.serializationConstructor, new Expression[2]
      {
        owner.ToExpression(),
        argumentReference.ToExpression()
      }));
      foreach (FieldReference serializedField in (IEnumerable<FieldReference>) this.serializedFields)
      {
        MethodInvocationExpression right = new MethodInvocationExpression((Reference) owner, SerializationInfoMethods.GetValue, new Expression[2]
        {
          new ConstReference((object) serializedField.Reference.Name).ToExpression(),
          (Expression) new TypeTokenExpression(serializedField.Reference.FieldType)
        });
        constructor.CodeBuilder.AddStatement((Statement) new AssignStatement((Reference) serializedField, (Expression) new ConvertExpression(serializedField.Reference.FieldType, typeof (object), (Expression) right)));
      }
      constructor.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    private bool VerifyIfBaseImplementsGetObjectData(Type baseType, IList<MethodInfo> methodsToSkip)
    {
      if (!typeof (ISerializable).IsAssignableFrom(baseType) || this.IsDelegate(baseType))
        return false;
      MethodInfo targetMethod = baseType.GetInterfaceMap(typeof (ISerializable)).TargetMethods[0];
      if (targetMethod.IsPrivate)
        return false;
      if (!targetMethod.IsVirtual || targetMethod.IsFinal)
        throw new ArgumentException(string.Format("The type {0} implements ISerializable, but GetObjectData is not marked as virtual. Dynamic Proxy needs types implementing ISerializable to mark GetObjectData as virtual to ensure correct serialization process.", (object) baseType.FullName));
      methodsToSkip.Add(targetMethod);
      this.serializationConstructor = baseType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      }, (ParameterModifier[]) null);
      if (this.serializationConstructor == null)
        throw new ArgumentException(string.Format("The type {0} implements ISerializable, but failed to provide a deserialization constructor", (object) baseType.FullName));
      return true;
    }

    private bool IsDelegate(Type baseType) => baseType.BaseType == typeof (MulticastDelegate);
  }
}
