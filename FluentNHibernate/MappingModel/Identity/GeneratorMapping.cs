// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Identity.GeneratorMapping
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
  public class GeneratorMapping : MappingBase
  {
    private readonly AttributeStore attributes = new AttributeStore();

    public GeneratorMapping()
    {
      this.Params = (IDictionary<string, string>) new Dictionary<string, string>();
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessGenerator(this);
    }

    public string Class => this.attributes.GetOrDefault<string>(nameof (Class));

    public IDictionary<string, string> Params { get; private set; }

    public Type ContainingEntityType { get; set; }

    public bool Equals(GeneratorMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.Params.ContentEquals<string, string>(this.Params) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (GeneratorMapping) && this.Equals((GeneratorMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.Params != null ? this.Params.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<GeneratorMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<GeneratorMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
