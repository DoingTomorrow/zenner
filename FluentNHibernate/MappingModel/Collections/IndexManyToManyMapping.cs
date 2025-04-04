// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.IndexManyToManyMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class IndexManyToManyMapping : MappingBase, IIndexMapping, IHasColumnMappings
  {
    private readonly AttributeStore attributes;
    private readonly LayeredColumns columns = new LayeredColumns();

    public IndexManyToManyMapping()
      : this(new AttributeStore())
    {
    }

    public IndexManyToManyMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessIndex(this);
      foreach (ColumnMapping column in this.Columns)
        visitor.Visit(column);
    }

    public Type ContainingEntityType { get; set; }

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public IEnumerable<ColumnMapping> Columns => this.columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
      this.columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer) => this.columns.MakeColumnsEmpty(layer);

    public string ForeignKey => this.attributes.GetOrDefault<string>(nameof (ForeignKey));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public bool Equals(IndexManyToManyMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.columns.ContentEquals(this.columns) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (IndexManyToManyMapping) && this.Equals((IndexManyToManyMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.columns != null ? this.columns.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(
      Expression<Func<IndexManyToManyMapping, T>> expression,
      int layer,
      T value)
    {
      this.Set(expression.ToMember<IndexManyToManyMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
