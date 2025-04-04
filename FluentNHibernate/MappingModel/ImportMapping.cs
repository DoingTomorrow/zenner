// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ImportMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Visitors;
using System;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class ImportMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public ImportMapping()
      : this(new AttributeStore())
    {
    }

    public ImportMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor) => visitor.ProcessImport(this);

    public string Rename => this.attributes.GetOrDefault<string>(nameof (Rename));

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public bool Equals(ImportMapping other)
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
      return obj.GetType() == typeof (ImportMapping) && this.Equals((ImportMapping) obj);
    }

    public override int GetHashCode()
    {
      return this.attributes == null ? 0 : this.attributes.GetHashCode();
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }
  }
}
