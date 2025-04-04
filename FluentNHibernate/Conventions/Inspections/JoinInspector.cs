// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.JoinInspector
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
  public class JoinInspector : IJoinInspector, IInspector
  {
    private readonly InspectorModelMapper<IJoinInspector, JoinMapping> propertyMappings = new InspectorModelMapper<IJoinInspector, JoinMapping>();
    private readonly JoinMapping mapping;

    public JoinInspector(JoinMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.TableName;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public IEnumerable<IAnyInspector> Anys
    {
      get
      {
        return this.mapping.Anys.Select<AnyMapping, AnyInspector>((Func<AnyMapping, AnyInspector>) (x => new AnyInspector(x))).Cast<IAnyInspector>();
      }
    }

    public Fetch Fetch => Fetch.FromString(this.mapping.Fetch);

    public bool Inverse => this.mapping.Inverse;

    public IKeyInspector Key
    {
      get
      {
        return this.mapping.Key == null ? (IKeyInspector) new KeyInspector(new KeyMapping()) : (IKeyInspector) new KeyInspector(this.mapping.Key);
      }
    }

    public bool Optional => this.mapping.Optional;

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

    public IEnumerable<ICollectionInspector> Collections
    {
      get
      {
        return this.mapping.Collections.Select<CollectionMapping, CollectionInspector>((Func<CollectionMapping, CollectionInspector>) (x => new CollectionInspector(x))).Cast<ICollectionInspector>();
      }
    }

    public string Schema => this.mapping.Schema;

    public string TableName => this.mapping.TableName;

    public string Catalog => this.mapping.Catalog;

    public string Subselect => this.mapping.Subselect;
  }
}
