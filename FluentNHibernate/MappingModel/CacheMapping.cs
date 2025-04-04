// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.CacheMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class CacheMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public CacheMapping()
      : this(new AttributeStore())
    {
    }

    public CacheMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor) => visitor.ProcessCache(this);

    public string Region => this.attributes.GetOrDefault<string>(nameof (Region));

    public string Usage => this.attributes.GetOrDefault<string>(nameof (Usage));

    public string Include => this.attributes.GetOrDefault<string>(nameof (Include));

    public Type ContainedEntityType { get; set; }

    public bool Equals(CacheMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainedEntityType, (object) this.ContainedEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (CacheMapping) && this.Equals((CacheMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.ContainedEntityType != null ? this.ContainedEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<CacheMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<CacheMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
