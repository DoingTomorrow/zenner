// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Identity.CompositeIdMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.Identity
{
  [Serializable]
  public class CompositeIdMapping : MappingBase, IIdentityMapping, IMapping
  {
    private readonly AttributeStore attributes;
    private readonly IList<ICompositeIdKeyMapping> keys = (IList<ICompositeIdKeyMapping>) new List<ICompositeIdKeyMapping>();

    public CompositeIdMapping()
      : this(new AttributeStore())
    {
    }

    public CompositeIdMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessCompositeId(this);
      foreach (ICompositeIdKeyMapping key in (IEnumerable<ICompositeIdKeyMapping>) this.keys)
      {
        if (key is KeyPropertyMapping)
          visitor.Visit((KeyPropertyMapping) key);
        if (key is KeyManyToOneMapping)
          visitor.Visit((KeyManyToOneMapping) key);
      }
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public bool Mapped
    {
      get
      {
        return this.attributes.GetOrDefault<bool>(nameof (Mapped)) || !string.IsNullOrEmpty(this.Name);
      }
    }

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public string UnsavedValue => this.attributes.GetOrDefault<string>(nameof (UnsavedValue));

    public IEnumerable<ICompositeIdKeyMapping> Keys
    {
      get => (IEnumerable<ICompositeIdKeyMapping>) this.keys;
    }

    public Type ContainingEntityType { get; set; }

    public void AddKey(ICompositeIdKeyMapping mapping) => this.keys.Add(mapping);

    public bool Equals(CompositeIdMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.keys.ContentEquals<ICompositeIdKeyMapping>((IEnumerable<ICompositeIdKeyMapping>) this.keys) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (CompositeIdMapping) && this.Equals((CompositeIdMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.keys != null ? this.keys.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<CompositeIdMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<CompositeIdMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
