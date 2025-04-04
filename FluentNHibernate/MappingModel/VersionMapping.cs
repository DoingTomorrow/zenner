// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.VersionMapping
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
  public class VersionMapping(AttributeStore underlyingStore) : ColumnBasedMappingBase(underlyingStore)
  {
    public VersionMapping()
      : this(new AttributeStore())
    {
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessVersion(this);
      this.Columns.Each<ColumnMapping>(new Action<ColumnMapping>(visitor.Visit));
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public string UnsavedValue => this.attributes.GetOrDefault<string>(nameof (UnsavedValue));

    public string Generated => this.attributes.GetOrDefault<string>(nameof (Generated));

    public System.Type ContainingEntityType { get; set; }

    public bool Equals(VersionMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ColumnBasedMappingBase) other) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as VersionMapping);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<VersionMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<VersionMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
