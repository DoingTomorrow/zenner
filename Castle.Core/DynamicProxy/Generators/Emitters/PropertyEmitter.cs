// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.PropertyEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class PropertyEmitter : IMemberEmitter
  {
    private readonly PropertyBuilder builder;
    private readonly AbstractTypeEmitter parentTypeEmitter;
    private MethodEmitter getMethod;
    private MethodEmitter setMethod;

    public PropertyEmitter(
      AbstractTypeEmitter parentTypeEmitter,
      string name,
      PropertyAttributes attributes,
      Type propertyType,
      Type[] arguments)
    {
      this.parentTypeEmitter = parentTypeEmitter;
      if (TypeBuilderMethods.DefineProperty == null)
        this.builder = new PropertyEmitter.DefineProperty_Clr2_0(parentTypeEmitter.TypeBuilder.DefineProperty)(name, attributes, propertyType, arguments);
      else
        this.builder = ((PropertyEmitter.DefineProperty_Clr_2_0_SP1) Delegate.CreateDelegate(typeof (PropertyEmitter.DefineProperty_Clr_2_0_SP1), (object) parentTypeEmitter.TypeBuilder, TypeBuilderMethods.DefineProperty))(name, attributes, CallingConventions.HasThis, propertyType, (Type[]) null, (Type[]) null, arguments, (Type[][]) null, (Type[][]) null);
    }

    public MethodEmitter CreateGetMethod(
      string name,
      MethodAttributes attrs,
      MethodInfo methodToOverride,
      params Type[] parameters)
    {
      if (this.getMethod != null)
        throw new InvalidOperationException("A get method exists");
      this.getMethod = new MethodEmitter(this.parentTypeEmitter, name, attrs, methodToOverride);
      return this.getMethod;
    }

    public MethodEmitter CreateGetMethod(
      string name,
      MethodAttributes attributes,
      MethodInfo methodToOverride)
    {
      return this.CreateGetMethod(name, attributes, methodToOverride, Type.EmptyTypes);
    }

    public MethodEmitter CreateSetMethod(
      string name,
      MethodAttributes attrs,
      MethodInfo methodToOverride,
      params Type[] parameters)
    {
      if (this.setMethod != null)
        throw new InvalidOperationException("A set method exists");
      this.setMethod = new MethodEmitter(this.parentTypeEmitter, name, attrs, methodToOverride);
      return this.setMethod;
    }

    public MethodEmitter CreateSetMethod(
      string name,
      MethodAttributes attributes,
      MethodInfo methodToOverride)
    {
      return this.CreateSetMethod(name, attributes, methodToOverride, Type.EmptyTypes);
    }

    public MemberInfo Member => (MemberInfo) null;

    public Type ReturnType => this.builder.PropertyType;

    public void Generate()
    {
      if (this.setMethod != null)
      {
        this.setMethod.Generate();
        this.builder.SetSetMethod(this.setMethod.MethodBuilder);
      }
      if (this.getMethod == null)
        return;
      this.getMethod.Generate();
      this.builder.SetGetMethod(this.getMethod.MethodBuilder);
    }

    public void EnsureValidCodeBlock()
    {
      if (this.setMethod != null)
        this.setMethod.EnsureValidCodeBlock();
      if (this.getMethod == null)
        return;
      this.getMethod.EnsureValidCodeBlock();
    }

    public void DefineCustomAttribute(CustomAttributeBuilder attribute)
    {
      this.builder.SetCustomAttribute(attribute);
    }

    private delegate PropertyBuilder DefineProperty_Clr2_0(
      string name,
      PropertyAttributes attributes,
      Type propertyType,
      Type[] parameters);

    public delegate PropertyBuilder DefineProperty_Clr_2_0_SP1(
      string name,
      PropertyAttributes attributes,
      CallingConventions callingConvention,
      Type returnType,
      Type[] returnTypeRequiredCustomModifiers,
      Type[] returnTypeOptionalCustomModifiers,
      Type[] parameterTypes,
      Type[][] parameterTypeRequiredCustomModifiers,
      Type[][] parameterTypeOptionalCustomModifiers);
  }
}
