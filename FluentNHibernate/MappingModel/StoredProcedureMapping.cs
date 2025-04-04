// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.StoredProcedureMapping
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
  public class StoredProcedureMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public StoredProcedureMapping()
      : this("sql-insert", "")
    {
    }

    public StoredProcedureMapping(AttributeStore attributes) => this.attributes = attributes;

    public StoredProcedureMapping(string spType, string innerText)
      : this(spType, innerText, new AttributeStore())
    {
    }

    public StoredProcedureMapping(string spType, string innerText, AttributeStore attributes)
    {
      this.attributes = attributes;
      this.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.SPType), 0, spType);
      this.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.Query), 0, innerText);
      this.Set<string>((Expression<Func<StoredProcedureMapping, string>>) (x => x.Check), 0, "none");
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public Type Type => this.attributes.GetOrDefault<Type>(nameof (Type));

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessStoredProcedure(this);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    public string Check => this.attributes.GetOrDefault<string>(nameof (Check));

    public string SPType => this.attributes.GetOrDefault<string>(nameof (SPType));

    public string Query => this.attributes.GetOrDefault<string>(nameof (Query));

    public bool Equals(StoredProcedureMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as StoredProcedureMapping);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }

    public void Set<T>(
      Expression<Func<StoredProcedureMapping, T>> expression,
      int layer,
      T value)
    {
      this.Set(expression.ToMember<StoredProcedureMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }
  }
}
