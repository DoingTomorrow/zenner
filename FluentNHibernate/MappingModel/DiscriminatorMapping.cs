// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.DiscriminatorMapping
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
  public class DiscriminatorMapping(AttributeStore underlyingStore) : ColumnBasedMappingBase(underlyingStore)
  {
    public DiscriminatorMapping()
      : this(new AttributeStore())
    {
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessDiscriminator(this);
      this.Columns.Each<ColumnMapping>(new Action<ColumnMapping>(visitor.Visit));
    }

    public bool Force => this.attributes.GetOrDefault<bool>(nameof (Force));

    public bool Insert => this.attributes.GetOrDefault<bool>(nameof (Insert));

    public string Formula => this.attributes.GetOrDefault<string>(nameof (Formula));

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public System.Type ContainingEntityType { get; set; }

    public bool Equals(DiscriminatorMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType) && other.Columns.ContentEquals<ColumnMapping>(this.Columns) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (DiscriminatorMapping) && this.Equals((DiscriminatorMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0) * 397 ^ (this.Columns != null ? this.Columns.GetHashCode() : 0) * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }

    public void Set(
      Expression<Func<DiscriminatorMapping, object>> expression,
      int layer,
      object value)
    {
      this.Set(expression.ToMember<DiscriminatorMapping, object>().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
