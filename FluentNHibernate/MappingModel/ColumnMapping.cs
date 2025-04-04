// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ColumnMapping
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
  public class ColumnMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public ColumnMapping()
      : this(new AttributeStore())
    {
    }

    public ColumnMapping(string defaultColumnName)
      : this()
    {
      this.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, defaultColumnName);
    }

    public ColumnMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor) => visitor.ProcessColumn(this);

    public Member Member { get; set; }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public int Length => this.attributes.GetOrDefault<int>(nameof (Length));

    public bool NotNull => this.attributes.GetOrDefault<bool>(nameof (NotNull));

    public bool Unique => this.attributes.GetOrDefault<bool>(nameof (Unique));

    public string UniqueKey => this.attributes.GetOrDefault<string>(nameof (UniqueKey));

    public string SqlType => this.attributes.GetOrDefault<string>(nameof (SqlType));

    public string Index => this.attributes.GetOrDefault<string>(nameof (Index));

    public string Check => this.attributes.GetOrDefault<string>(nameof (Check));

    public int Precision => this.attributes.GetOrDefault<int>(nameof (Precision));

    public int Scale => this.attributes.GetOrDefault<int>(nameof (Scale));

    public string Default => this.attributes.GetOrDefault<string>(nameof (Default));

    public ColumnMapping Clone() => new ColumnMapping(this.attributes.Clone());

    public bool Equals(ColumnMapping other)
    {
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.Member, (object) this.Member);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (ColumnMapping) && this.Equals((ColumnMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.Member != (Member) null ? this.Member.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<ColumnMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<ColumnMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    public void MergeAttributes(AttributeStore columnAttributes)
    {
      this.attributes.Merge(columnAttributes);
    }
  }
}
