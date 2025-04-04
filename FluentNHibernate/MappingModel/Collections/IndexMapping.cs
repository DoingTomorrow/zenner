// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.IndexMapping
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
  public class IndexMapping : MappingBase, IIndexMapping, IHasColumnMappings
  {
    private readonly AttributeStore attributes;
    private readonly LayeredColumns columns = new LayeredColumns();

    public IndexMapping()
      : this(new AttributeStore())
    {
    }

    public IndexMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessIndex(this);
      foreach (ColumnMapping column in this.Columns)
        visitor.Visit(column);
    }

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public int Offset => this.attributes.GetOrDefault<int>(nameof (Offset));

    public System.Type ContainingEntityType { get; set; }

    public IEnumerable<ColumnMapping> Columns => this.columns.Columns;

    public void AddColumn(int layer, ColumnMapping mapping)
    {
      this.columns.AddColumn(layer, mapping);
    }

    public void MakeColumnsEmpty(int layer) => this.columns.MakeColumnsEmpty(layer);

    public bool Equals(IndexMapping other)
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
      return obj.GetType() == typeof (IndexMapping) && this.Equals((IndexMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.columns != null ? this.columns.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<IndexMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<IndexMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
