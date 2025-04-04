// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Identity.KeyPropertyMapping
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
  public class KeyPropertyMapping : MappingBase, ICompositeIdKeyMapping
  {
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly IList<ColumnMapping> columns = (IList<ColumnMapping>) new List<ColumnMapping>();

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessKeyProperty(this);
      foreach (ColumnMapping column in (IEnumerable<ColumnMapping>) this.columns)
        visitor.Visit(column);
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public int Length => this.attributes.GetOrDefault<int>(nameof (Length));

    public IEnumerable<ColumnMapping> Columns => (IEnumerable<ColumnMapping>) this.columns;

    public System.Type ContainingEntityType { get; set; }

    public void AddColumn(ColumnMapping mapping) => this.columns.Add(mapping);

    public bool Equals(KeyPropertyMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.columns.ContentEquals<ColumnMapping>((IEnumerable<ColumnMapping>) this.columns) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (KeyPropertyMapping) && this.Equals((KeyPropertyMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.columns != null ? this.columns.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<KeyPropertyMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<KeyPropertyMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
