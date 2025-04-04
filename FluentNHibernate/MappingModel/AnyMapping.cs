// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.AnyMapping
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
  public class AnyMapping : MappingBase
  {
    private readonly AttributeStore attributes;
    private readonly LayeredColumns typeColumns = new LayeredColumns();
    private readonly LayeredColumns identifierColumns = new LayeredColumns();
    private readonly IList<MetaValueMapping> metaValues = (IList<MetaValueMapping>) new List<MetaValueMapping>();

    public AnyMapping()
      : this(new AttributeStore())
    {
    }

    public AnyMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessAny(this);
      foreach (MetaValueMapping metaValue in (IEnumerable<MetaValueMapping>) this.metaValues)
        visitor.Visit(metaValue);
      foreach (ColumnMapping typeColumn in this.TypeColumns)
        visitor.Visit(typeColumn);
      foreach (ColumnMapping identifierColumn in this.IdentifierColumns)
        visitor.Visit(identifierColumn);
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string IdType => this.attributes.GetOrDefault<string>(nameof (IdType));

    public TypeReference MetaType => this.attributes.GetOrDefault<TypeReference>(nameof (MetaType));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public bool Insert => this.attributes.GetOrDefault<bool>(nameof (Insert));

    public bool Update => this.attributes.GetOrDefault<bool>(nameof (Update));

    public string Cascade => this.attributes.GetOrDefault<string>(nameof (Cascade));

    public bool Lazy => this.attributes.GetOrDefault<bool>(nameof (Lazy));

    public bool OptimisticLock => this.attributes.GetOrDefault<bool>(nameof (OptimisticLock));

    public IEnumerable<ColumnMapping> TypeColumns => this.typeColumns.Columns;

    public IEnumerable<ColumnMapping> IdentifierColumns => this.identifierColumns.Columns;

    public IEnumerable<MetaValueMapping> MetaValues
    {
      get => (IEnumerable<MetaValueMapping>) this.metaValues;
    }

    public Type ContainingEntityType { get; set; }

    public void AddTypeColumn(int layer, ColumnMapping column)
    {
      this.typeColumns.AddColumn(layer, column);
    }

    public void AddIdentifierColumn(int layer, ColumnMapping column)
    {
      this.identifierColumns.AddColumn(layer, column);
    }

    public void AddMetaValue(MetaValueMapping metaValue) => this.metaValues.Add(metaValue);

    public bool Equals(AnyMapping other)
    {
      return object.Equals((object) other.attributes, (object) this.attributes) && other.typeColumns.ContentEquals(this.typeColumns) && other.identifierColumns.ContentEquals(this.identifierColumns) && other.metaValues.ContentEquals<MetaValueMapping>((IEnumerable<MetaValueMapping>) this.metaValues) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (AnyMapping) && this.Equals((AnyMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.typeColumns != null ? this.typeColumns.GetHashCode() : 0)) * 397 ^ (this.identifierColumns != null ? this.identifierColumns.GetHashCode() : 0)) * 397 ^ (this.metaValues != null ? this.metaValues.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<AnyMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<AnyMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
