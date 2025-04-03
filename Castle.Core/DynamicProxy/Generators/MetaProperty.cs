// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.MetaProperty
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class MetaProperty : MetaTypeElement, IEquatable<MetaProperty>
  {
    private string name;
    private readonly Type type;
    private readonly MetaMethod getter;
    private readonly MetaMethod setter;
    private readonly PropertyAttributes attributes;
    private readonly IEnumerable<CustomAttributeBuilder> customAttributes;
    private readonly Type[] arguments;
    private PropertyEmitter emitter;

    public MetaProperty(
      string name,
      Type propertyType,
      Type declaringType,
      MetaMethod getter,
      MetaMethod setter,
      IEnumerable<CustomAttributeBuilder> customAttributes,
      Type[] arguments)
      : base(declaringType)
    {
      this.name = name;
      this.type = propertyType;
      this.getter = getter;
      this.setter = setter;
      this.attributes = PropertyAttributes.None;
      this.customAttributes = customAttributes;
      this.arguments = arguments ?? Type.EmptyTypes;
    }

    public bool CanRead => this.getter != null;

    public bool CanWrite => this.setter != null;

    public MethodInfo GetMethod
    {
      get
      {
        if (!this.CanRead)
          throw new InvalidOperationException();
        return this.getter.Method;
      }
    }

    public MethodInfo SetMethod
    {
      get
      {
        if (!this.CanWrite)
          throw new InvalidOperationException();
        return this.setter.Method;
      }
    }

    public PropertyEmitter Emitter
    {
      get
      {
        return this.emitter != null ? this.emitter : throw new InvalidOperationException("Emitter is not initialized. You have to initialize it first using 'BuildPropertyEmitter' method");
      }
    }

    public MetaMethod Getter => this.getter;

    public MetaMethod Setter => this.setter;

    public bool Equals(MetaProperty other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      if (!this.type.Equals(other.type) || !StringComparer.OrdinalIgnoreCase.Equals(this.name, other.name) || this.Arguments.Length != other.Arguments.Length)
        return false;
      for (int index = 0; index < this.Arguments.Length; ++index)
      {
        if (!this.Arguments[index].Equals(other.Arguments[index]))
          return false;
      }
      return true;
    }

    public Type[] Arguments => this.arguments;

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (MetaProperty) && this.Equals((MetaProperty) obj);
    }

    public override int GetHashCode()
    {
      return (this.GetMethod != null ? this.GetMethod.GetHashCode() : 0) * 397 ^ (this.SetMethod != null ? this.SetMethod.GetHashCode() : 0);
    }

    public void BuildPropertyEmitter(ClassEmitter classEmitter)
    {
      if (this.emitter != null)
        throw new InvalidOperationException("Emitter is already created. It is illegal to invoke this method twice.");
      this.emitter = classEmitter.CreateProperty(this.name, this.attributes, this.type, this.arguments);
      foreach (CustomAttributeBuilder customAttribute in this.customAttributes)
        this.emitter.DefineCustomAttribute(customAttribute);
    }

    internal override void SwitchToExplicitImplementation()
    {
      this.name = string.Format("{0}.{1}", (object) this.sourceType.Name, (object) this.name);
      if (this.setter != null)
        this.setter.SwitchToExplicitImplementation();
      if (this.getter == null)
        return;
      this.getter.SwitchToExplicitImplementation();
    }
  }
}
