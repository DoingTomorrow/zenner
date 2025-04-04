// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ManyToOneMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

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
  public class ManyToOneMapping : MappingBase, IHasColumnMappings, IRelationship
  {
    private readonly AttributeStore attributes;
    private readonly LayeredColumns columns = new LayeredColumns();

    public ManyToOneMapping()
      : this(new AttributeStore())
    {
    }

    public ManyToOneMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessManyToOne(this);
      foreach (ColumnMapping column in this.Columns)
        visitor.Visit(column);
    }

    public Type ContainingEntityType { get; set; }

    public Member Member { get; set; }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public string Cascade => this.attributes.GetOrDefault<string>(nameof (Cascade));

    public string Fetch => this.attributes.GetOrDefault<string>(nameof (Fetch));

    public bool Update => this.attributes.GetOrDefault<bool>(nameof (Update));

    public bool Insert => this.attributes.GetOrDefault<bool>(nameof (Insert));

    public string Formula => this.attributes.GetOrDefault<string>(nameof (Formula));

    public string ForeignKey => this.attributes.GetOrDefault<string>(nameof (ForeignKey));

    public string PropertyRef => this.attributes.GetOrDefault<string>(nameof (PropertyRef));

    public string NotFound => this.attributes.GetOrDefault<string>(nameof (NotFound));

    public string Lazy => this.attributes.GetOrDefault<string>(nameof (Lazy));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public bool OptimisticLock => this.attributes.GetOrDefault<bool>(nameof (OptimisticLock));

    public IEnumerable<ColumnMapping> Columns => this.columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
      this.columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer) => this.columns.MakeColumnsEmpty(layer);

    public void Set<T>(Expression<Func<ManyToOneMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<ManyToOneMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public bool Equals(ManyToOneMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.columns.ContentEquals(this.columns) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType) && object.Equals((object) other.Member, (object) this.Member);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (ManyToOneMapping) && this.Equals((ManyToOneMapping) obj);
    }

    public override int GetHashCode()
    {
      return (((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.columns != null ? this.columns.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0)) * 397 ^ (this.Member != (Member) null ? this.Member.GetHashCode() : 0);
    }

    public IRelationship OtherSide { get; set; }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
