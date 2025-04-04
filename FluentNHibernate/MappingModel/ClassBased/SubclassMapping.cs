// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.SubclassMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class SubclassMapping : ClassMappingBase
  {
    private AttributeStore attributes;

    public SubclassType SubclassType { get; private set; }

    public SubclassMapping(SubclassType subclassType)
      : this(subclassType, new AttributeStore())
    {
    }

    public SubclassMapping(SubclassType subclassType, AttributeStore attributes)
      : base(attributes)
    {
      this.SubclassType = subclassType;
      this.attributes = attributes;
    }

    public Type Extends => this.attributes.GetOrDefault<Type>(nameof (Extends));

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessSubclass(this);
      if (this.SubclassType == SubclassType.JoinedSubclass && this.Key != null)
        visitor.Visit(this.Key);
      base.AcceptVisitor(visitor);
    }

    public override string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public override Type Type => this.attributes.GetOrDefault<Type>(nameof (Type));

    public object DiscriminatorValue
    {
      get => this.attributes.GetOrDefault<object>(nameof (DiscriminatorValue));
    }

    public bool Lazy => this.attributes.GetOrDefault<bool>(nameof (Lazy));

    public string Proxy => this.attributes.GetOrDefault<string>(nameof (Proxy));

    public bool DynamicUpdate => this.attributes.GetOrDefault<bool>(nameof (DynamicUpdate));

    public bool DynamicInsert => this.attributes.GetOrDefault<bool>(nameof (DynamicInsert));

    public bool SelectBeforeUpdate
    {
      get => this.attributes.GetOrDefault<bool>(nameof (SelectBeforeUpdate));
    }

    public bool Abstract => this.attributes.GetOrDefault<bool>(nameof (Abstract));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public string TableName => this.attributes.GetOrDefault<string>(nameof (TableName));

    public KeyMapping Key => this.attributes.GetOrDefault<KeyMapping>(nameof (Key));

    public string Check => this.attributes.GetOrDefault<string>(nameof (Check));

    public string Schema => this.attributes.GetOrDefault<string>(nameof (Schema));

    public string Subselect => this.attributes.GetOrDefault<string>(nameof (Subselect));

    public TypeReference Persister
    {
      get => this.attributes.GetOrDefault<TypeReference>(nameof (Persister));
    }

    public int BatchSize => this.attributes.GetOrDefault<int>(nameof (BatchSize));

    public void OverrideAttributes(AttributeStore store) => this.attributes = store;

    public bool Equals(SubclassMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ClassMappingBase) other) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as SubclassMapping);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }

    public override string ToString() => "Subclass(" + this.Type.Name + ")";

    public void Set<T>(Expression<Func<SubclassMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<SubclassMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
