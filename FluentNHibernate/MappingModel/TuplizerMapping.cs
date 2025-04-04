// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.TuplizerMapping
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
  public class TuplizerMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public TuplizerMapping()
      : this(new AttributeStore())
    {
    }

    public TuplizerMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessTuplizer(this);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    public TuplizerMode Mode => this.attributes.GetOrDefault<TuplizerMode>(nameof (Mode));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public TypeReference Type => this.attributes.GetOrDefault<TypeReference>(nameof (Type));

    public bool Equals(TuplizerMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (TuplizerMapping) && this.Equals((TuplizerMapping) obj);
    }

    public override int GetHashCode()
    {
      return this.attributes == null ? 0 : this.attributes.GetHashCode();
    }

    public void Set<T>(Expression<Func<TuplizerMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<TuplizerMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }
  }
}
