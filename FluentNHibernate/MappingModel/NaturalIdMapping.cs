// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.NaturalIdMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class NaturalIdMapping : MappingBase
  {
    private readonly AttributeStore attributes;
    private readonly IList<PropertyMapping> properties = (IList<PropertyMapping>) new List<PropertyMapping>();
    private readonly IList<ManyToOneMapping> manyToOnes = (IList<ManyToOneMapping>) new List<ManyToOneMapping>();

    public NaturalIdMapping()
      : this(new AttributeStore())
    {
    }

    public NaturalIdMapping(AttributeStore attributes) => this.attributes = attributes;

    public bool Mutable => this.attributes.GetOrDefault<bool>(nameof (Mutable));

    public IEnumerable<PropertyMapping> Properties
    {
      get => (IEnumerable<PropertyMapping>) this.properties;
    }

    public IEnumerable<ManyToOneMapping> ManyToOnes
    {
      get => (IEnumerable<ManyToOneMapping>) this.manyToOnes;
    }

    public void AddProperty(PropertyMapping mapping) => this.properties.Add(mapping);

    public void AddReference(ManyToOneMapping mapping) => this.manyToOnes.Add(mapping);

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessNaturalId(this);
      foreach (PropertyMapping property in (IEnumerable<PropertyMapping>) this.properties)
        visitor.Visit(property);
      foreach (ManyToOneMapping manyToOne in (IEnumerable<ManyToOneMapping>) this.manyToOnes)
        visitor.Visit(manyToOne);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }
  }
}
