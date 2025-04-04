// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.CompositeIdentityInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class CompositeIdentityInspector : 
    ICompositeIdentityInspector,
    IIdentityInspectorBase,
    IInspector
  {
    private readonly InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping> mappedProperties = new InspectorModelMapper<ICompositeIdentityInspector, CompositeIdMapping>();
    private readonly CompositeIdMapping mapping;

    public CompositeIdentityInspector(CompositeIdMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public TypeReference Class => this.mapping.Class;

    public IEnumerable<IKeyManyToOneInspector> KeyManyToOnes
    {
      get
      {
        return this.mapping.Keys.Where<ICompositeIdKeyMapping>((Func<ICompositeIdKeyMapping, bool>) (x => x is KeyManyToOneMapping)).Select<ICompositeIdKeyMapping, KeyManyToOneInspector>((Func<ICompositeIdKeyMapping, KeyManyToOneInspector>) (x => new KeyManyToOneInspector((KeyManyToOneMapping) x))).Cast<IKeyManyToOneInspector>();
      }
    }

    public IEnumerable<IKeyPropertyInspector> KeyProperties
    {
      get
      {
        return this.mapping.Keys.Where<ICompositeIdKeyMapping>((Func<ICompositeIdKeyMapping, bool>) (x => x is KeyPropertyMapping)).Select<ICompositeIdKeyMapping, KeyPropertyInspector>((Func<ICompositeIdKeyMapping, KeyPropertyInspector>) (x => new KeyPropertyInspector((KeyPropertyMapping) x))).Cast<IKeyPropertyInspector>();
      }
    }

    public bool Mapped => this.mapping.Mapped;

    public string Name => this.mapping.Name;

    public string UnsavedValue => this.mapping.UnsavedValue;
  }
}
