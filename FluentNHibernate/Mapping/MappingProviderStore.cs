// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.MappingProviderStore
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class MappingProviderStore
  {
    public IList<IPropertyMappingProvider> Properties { get; set; }

    public IList<IComponentMappingProvider> Components { get; set; }

    public IList<IOneToOneMappingProvider> OneToOnes { get; set; }

    public Dictionary<Type, ISubclassMappingProvider> Subclasses { get; set; }

    public IList<ICollectionMappingProvider> Collections { get; set; }

    public IList<IManyToOneMappingProvider> References { get; set; }

    public IList<IAnyMappingProvider> Anys { get; set; }

    public IList<IFilterMappingProvider> Filters { get; set; }

    public IList<IStoredProcedureMappingProvider> StoredProcedures { get; set; }

    public IList<IJoinMappingProvider> Joins { get; set; }

    public IIdentityMappingProvider Id { get; set; }

    public ICompositeIdMappingProvider CompositeId { get; set; }

    public INaturalIdMappingProvider NaturalId { get; set; }

    public IVersionMappingProvider Version { get; set; }

    public IDiscriminatorMappingProvider Discriminator { get; set; }

    public TuplizerMapping TuplizerMapping { get; set; }

    public MappingProviderStore()
    {
      this.Properties = (IList<IPropertyMappingProvider>) new List<IPropertyMappingProvider>();
      this.Components = (IList<IComponentMappingProvider>) new List<IComponentMappingProvider>();
      this.OneToOnes = (IList<IOneToOneMappingProvider>) new List<IOneToOneMappingProvider>();
      this.Subclasses = new Dictionary<Type, ISubclassMappingProvider>();
      this.Collections = (IList<ICollectionMappingProvider>) new List<ICollectionMappingProvider>();
      this.References = (IList<IManyToOneMappingProvider>) new List<IManyToOneMappingProvider>();
      this.Anys = (IList<IAnyMappingProvider>) new List<IAnyMappingProvider>();
      this.Filters = (IList<IFilterMappingProvider>) new List<IFilterMappingProvider>();
      this.StoredProcedures = (IList<IStoredProcedureMappingProvider>) new List<IStoredProcedureMappingProvider>();
      this.Joins = (IList<IJoinMappingProvider>) new List<IJoinMappingProvider>();
    }
  }
}
