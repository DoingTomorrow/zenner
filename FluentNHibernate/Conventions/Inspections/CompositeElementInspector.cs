// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.CompositeElementInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class CompositeElementInspector : ICompositeElementInspector, IInspector
  {
    private readonly InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping> mappedProperties = new InspectorModelMapper<ICompositeElementInspector, CompositeElementMapping>();
    private readonly CompositeElementMapping mapping;

    public CompositeElementInspector(CompositeElementMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Class.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public TypeReference Class => this.mapping.Class;

    public IParentInspector Parent
    {
      get
      {
        return this.mapping.Parent == null ? (IParentInspector) new ParentInspector(new ParentMapping()) : (IParentInspector) new ParentInspector(this.mapping.Parent);
      }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
      get
      {
        return this.mapping.Properties.Select<PropertyMapping, PropertyInspector>((Func<PropertyMapping, PropertyInspector>) (x => new PropertyInspector(x))).Cast<IPropertyInspector>();
      }
    }

    public IEnumerable<IManyToOneInspector> References
    {
      get
      {
        return this.mapping.References.Select<ManyToOneMapping, ManyToOneInspector>((Func<ManyToOneMapping, ManyToOneInspector>) (x => new ManyToOneInspector(x))).Cast<IManyToOneInspector>();
      }
    }
  }
}
