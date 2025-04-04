// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Identity.IdMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.Identity
{
  [Serializable]
  public class IdMapping(AttributeStore underlyingStore) : 
    ColumnBasedMappingBase(underlyingStore),
    IIdentityMapping,
    IMapping
  {
    public IdMapping()
      : this(new AttributeStore())
    {
    }

    public Member Member { get; set; }

    public GeneratorMapping Generator
    {
      get => this.attributes.GetOrDefault<GeneratorMapping>(nameof (Generator));
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessId(this);
      foreach (ColumnMapping column in this.Columns)
        visitor.Visit(column);
      if (this.Generator == null)
        return;
      visitor.Visit(this.Generator);
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public string UnsavedValue => this.attributes.GetOrDefault<string>(nameof (UnsavedValue));

    public System.Type ContainingEntityType { get; set; }

    public void Set(Expression<Func<IdMapping, object>> expression, int layer, object value)
    {
      this.Set(expression.ToMember<IdMapping, object>().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public bool Equals(IdMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ColumnBasedMappingBase) other) && object.Equals((object) other.Member, (object) this.Member) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as IdMapping);
    }

    public override int GetHashCode()
    {
      return (base.GetHashCode() * 397 ^ (this.Member != (Member) null ? this.Member.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
