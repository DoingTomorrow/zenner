// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.JoinMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class JoinMapping : IMapping
  {
    private readonly AttributeStore attributes;
    private readonly MappedMembers mappedMembers;

    public JoinMapping()
      : this(new AttributeStore())
    {
    }

    public JoinMapping(AttributeStore attributes)
    {
      this.attributes = attributes;
      this.mappedMembers = new MappedMembers();
    }

    public KeyMapping Key => this.attributes.GetOrDefault<KeyMapping>(nameof (Key));

    public IEnumerable<PropertyMapping> Properties => this.mappedMembers.Properties;

    public IEnumerable<ManyToOneMapping> References => this.mappedMembers.References;

    public IEnumerable<IComponentMapping> Components => this.mappedMembers.Components;

    public IEnumerable<AnyMapping> Anys => this.mappedMembers.Anys;

    public IEnumerable<CollectionMapping> Collections => this.mappedMembers.Collections;

    public void AddProperty(PropertyMapping property) => this.mappedMembers.AddProperty(property);

    public void AddReference(ManyToOneMapping manyToOne)
    {
      this.mappedMembers.AddReference(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
      this.mappedMembers.AddComponent(componentMapping);
    }

    public void AddAny(AnyMapping mapping) => this.mappedMembers.AddAny(mapping);

    public void AddCollection(CollectionMapping collectionMapping)
    {
      this.mappedMembers.AddCollection(collectionMapping);
    }

    public string TableName => this.attributes.GetOrDefault<string>(nameof (TableName));

    public string Schema => this.attributes.GetOrDefault<string>(nameof (Schema));

    public string Catalog => this.attributes.GetOrDefault<string>(nameof (Catalog));

    public string Subselect => this.attributes.GetOrDefault<string>(nameof (Subselect));

    public string Fetch => this.attributes.GetOrDefault<string>(nameof (Fetch));

    public bool Inverse => this.attributes.GetOrDefault<bool>(nameof (Inverse));

    public bool Optional => this.attributes.GetOrDefault<bool>(nameof (Optional));

    public Type ContainingEntityType { get; set; }

    public void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessJoin(this);
      if (this.Key != null)
        visitor.Visit(this.Key);
      this.mappedMembers.AcceptVisitor(visitor);
    }

    public bool Equals(JoinMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.mappedMembers, (object) this.mappedMembers) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (JoinMapping) && this.Equals((JoinMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.mappedMembers != null ? this.mappedMembers.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<JoinMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<JoinMapping, T>().Name, layer, (object) value);
    }

    public void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
