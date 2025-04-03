// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.AbstractTypeEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public abstract class AbstractTypeEmitter
  {
    private const MethodAttributes defaultAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;
    private readonly TypeBuilder typebuilder;
    private readonly ConstructorCollection constructors;
    private readonly MethodCollection methods;
    private readonly PropertiesCollection properties;
    private readonly EventCollection events;
    private readonly NestedClassCollection nested;
    private readonly Dictionary<string, GenericTypeParameterBuilder> name2GenericType;
    private GenericTypeParameterBuilder[] genericTypeParams;
    private readonly IDictionary<string, FieldReference> fields = (IDictionary<string, FieldReference>) new Dictionary<string, FieldReference>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    protected AbstractTypeEmitter(TypeBuilder typeBuilder)
    {
      this.typebuilder = typeBuilder;
      this.nested = new NestedClassCollection();
      this.methods = new MethodCollection();
      this.constructors = new ConstructorCollection();
      this.properties = new PropertiesCollection();
      this.events = new EventCollection();
      this.name2GenericType = new Dictionary<string, GenericTypeParameterBuilder>();
    }

    public Type GetGenericArgument(string genericArgumentName)
    {
      return (Type) this.name2GenericType[genericArgumentName];
    }

    public Type[] GetGenericArgumentsFor(Type genericType)
    {
      List<Type> typeList = new List<Type>();
      foreach (Type genericArgument in genericType.GetGenericArguments())
      {
        if (genericArgument.IsGenericParameter)
          typeList.Add((Type) this.name2GenericType[genericArgument.Name]);
        else
          typeList.Add(genericArgument);
      }
      return typeList.ToArray();
    }

    public Type[] GetGenericArgumentsFor(MethodInfo genericMethod)
    {
      List<Type> typeList = new List<Type>();
      foreach (Type genericArgument in genericMethod.GetGenericArguments())
        typeList.Add((Type) this.name2GenericType[genericArgument.Name]);
      return typeList.ToArray();
    }

    public void AddCustomAttributes(ProxyGenerationOptions proxyGenerationOptions)
    {
      foreach (Attribute addToGeneratedType in (IEnumerable<Attribute>) proxyGenerationOptions.attributesToAddToGeneratedTypes)
      {
        CustomAttributeBuilder builder = AttributeUtil.CreateBuilder(addToGeneratedType);
        if (builder != null)
          this.typebuilder.SetCustomAttribute(builder);
      }
      foreach (CustomAttributeBuilder additionalAttribute in (IEnumerable<CustomAttributeBuilder>) proxyGenerationOptions.AdditionalAttributes)
        this.typebuilder.SetCustomAttribute(additionalAttribute);
    }

    public void CreateDefaultConstructor()
    {
      if (this.TypeBuilder.IsInterface)
        throw new InvalidOperationException("Interfaces cannot have constructors.");
      this.constructors.Add(new ConstructorEmitter(this, new ArgumentReference[0]));
    }

    public ConstructorEmitter CreateConstructor(params ArgumentReference[] arguments)
    {
      if (this.TypeBuilder.IsInterface)
        throw new InvalidOperationException("Interfaces cannot have constructors.");
      ConstructorEmitter constructor = new ConstructorEmitter(this, arguments);
      this.constructors.Add(constructor);
      return constructor;
    }

    public ConstructorEmitter CreateTypeConstructor()
    {
      TypeConstructorEmitter typeConstructor = new TypeConstructorEmitter(this);
      this.constructors.Add((ConstructorEmitter) typeConstructor);
      this.ClassConstructor = typeConstructor;
      return (ConstructorEmitter) typeConstructor;
    }

    public TypeConstructorEmitter ClassConstructor { get; private set; }

    public MethodEmitter CreateMethod(
      string name,
      MethodAttributes attrs,
      Type returnType,
      params Type[] argumentTypes)
    {
      MethodEmitter method = new MethodEmitter(this, name, attrs, returnType, argumentTypes ?? Type.EmptyTypes);
      this.methods.Add(method);
      return method;
    }

    public MethodEmitter CreateMethod(string name, Type returnType, params Type[] parameterTypes)
    {
      return this.CreateMethod(name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, returnType, parameterTypes);
    }

    public MethodEmitter CreateMethod(string name, MethodInfo methodToUseAsATemplate)
    {
      return this.CreateMethod(name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, methodToUseAsATemplate);
    }

    public MethodEmitter CreateMethod(
      string name,
      MethodAttributes attributes,
      MethodInfo methodToUseAsATemplate)
    {
      MethodEmitter method = new MethodEmitter(this, name, attributes, methodToUseAsATemplate);
      this.methods.Add(method);
      return method;
    }

    public FieldReference CreateStaticField(string name, Type fieldType)
    {
      return this.CreateStaticField(name, fieldType, FieldAttributes.Public);
    }

    public FieldReference CreateStaticField(string name, Type fieldType, FieldAttributes atts)
    {
      atts |= FieldAttributes.Static;
      return this.CreateField(name, fieldType, atts);
    }

    public FieldReference CreateField(string name, Type fieldType)
    {
      return this.CreateField(name, fieldType, true);
    }

    public FieldReference CreateField(string name, Type fieldType, bool serializable)
    {
      FieldAttributes atts = FieldAttributes.Public;
      if (!serializable)
        atts |= FieldAttributes.NotSerialized;
      return this.CreateField(name, fieldType, atts);
    }

    public FieldReference CreateField(string name, Type fieldType, FieldAttributes atts)
    {
      FieldReference field = new FieldReference(this.typebuilder.DefineField(name, fieldType, atts));
      this.fields[name] = field;
      return field;
    }

    public PropertyEmitter CreateProperty(
      string name,
      PropertyAttributes attributes,
      Type propertyType,
      Type[] arguments)
    {
      PropertyEmitter property = new PropertyEmitter(this, name, attributes, propertyType, arguments);
      this.properties.Add(property);
      return property;
    }

    public EventEmitter CreateEvent(string name, EventAttributes atts, Type type)
    {
      EventEmitter eventEmitter = new EventEmitter(this, name, atts, type);
      this.events.Add(eventEmitter);
      return eventEmitter;
    }

    public void DefineCustomAttribute(CustomAttributeBuilder attribute)
    {
      this.typebuilder.SetCustomAttribute(attribute);
    }

    public void DefineCustomAttribute<TAttribute>(object[] constructorArguments) where TAttribute : Attribute
    {
      this.typebuilder.SetCustomAttribute(AttributeUtil.CreateBuilder(typeof (TAttribute), constructorArguments));
    }

    public void DefineCustomAttribute<TAttribute>() where TAttribute : Attribute, new()
    {
      this.typebuilder.SetCustomAttribute(AttributeUtil.CreateBuilder<TAttribute>());
    }

    public void DefineCustomAttributeFor<TAttribute>(FieldReference field) where TAttribute : Attribute, new()
    {
      CustomAttributeBuilder builder = AttributeUtil.CreateBuilder<TAttribute>();
      (field.Fieldbuilder ?? throw new ArgumentException("Invalid field reference.This reference does not point to field on type being generated", nameof (field))).SetCustomAttribute(builder);
    }

    public ConstructorCollection Constructors => this.constructors;

    public NestedClassCollection Nested => this.nested;

    public TypeBuilder TypeBuilder => this.typebuilder;

    public Type BaseType
    {
      get
      {
        return !this.TypeBuilder.IsInterface ? this.TypeBuilder.BaseType : throw new InvalidOperationException("This emitter represents an interface; interfaces have no base types.");
      }
    }

    public GenericTypeParameterBuilder[] GenericTypeParams => this.genericTypeParams;

    public void SetGenericTypeParameters(
      GenericTypeParameterBuilder[] genericTypeParameterBuilders)
    {
      this.genericTypeParams = genericTypeParameterBuilders;
    }

    public void CopyGenericParametersFromMethod(MethodInfo methodToCopyGenericsFrom)
    {
      if (this.genericTypeParams != null)
        throw new ProxyGenerationException("CopyGenericParametersFromMethod: cannot invoke me twice");
      this.SetGenericTypeParameters(GenericUtil.CopyGenericArguments(methodToCopyGenericsFrom, this.typebuilder, this.name2GenericType));
    }

    public FieldReference GetField(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (FieldReference) null;
      FieldReference field;
      this.fields.TryGetValue(name, out field);
      return field;
    }

    public IEnumerable<FieldReference> GetAllFields()
    {
      return (IEnumerable<FieldReference>) this.fields.Values;
    }

    public virtual Type BuildType()
    {
      this.EnsureBuildersAreInAValidState();
      Type type = this.CreateType(this.typebuilder);
      foreach (AbstractTypeEmitter abstractTypeEmitter in (Collection<NestedClassEmitter>) this.nested)
        abstractTypeEmitter.BuildType();
      return type;
    }

    protected virtual void EnsureBuildersAreInAValidState()
    {
      if (!this.typebuilder.IsInterface && this.constructors.Count == 0)
        this.CreateDefaultConstructor();
      foreach (IMemberEmitter property in (Collection<PropertyEmitter>) this.properties)
      {
        property.EnsureValidCodeBlock();
        property.Generate();
      }
      foreach (IMemberEmitter memberEmitter in (Collection<EventEmitter>) this.events)
      {
        memberEmitter.EnsureValidCodeBlock();
        memberEmitter.Generate();
      }
      foreach (IMemberEmitter constructor in (Collection<ConstructorEmitter>) this.constructors)
      {
        constructor.EnsureValidCodeBlock();
        constructor.Generate();
      }
      foreach (IMemberEmitter method in (Collection<MethodEmitter>) this.methods)
      {
        method.EnsureValidCodeBlock();
        method.Generate();
      }
    }

    protected Type CreateType(TypeBuilder type)
    {
      try
      {
        return type.CreateType();
      }
      catch (BadImageFormatException ex)
      {
        if (!Debugger.IsAttached)
          throw;
        else if (!ex.Message.Contains("HRESULT: 0x8007000B"))
          throw;
        else if (!type.IsGenericTypeDefinition)
        {
          throw;
        }
        else
        {
          ProxyGenerationException generationException = new ProxyGenerationException("This is a DynamicProxy2 error: It looks like you enoutered a bug in Visual Studio debugger, which causes this exception when proxying types with generic methods having constraints on their generic arguments.This code will work just fine without the debugger attached. If you wish to use debugger you may have to switch to Visual Studio 2010 where this bug was fixed.");
          generationException.Data.Add((object) "ProxyType", (object) type.ToString());
          throw generationException;
        }
      }
    }
  }
}
