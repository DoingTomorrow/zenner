// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ComponentReferenceResolutionVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public class ComponentReferenceResolutionVisitor : DefaultMappingModelVisitor
  {
    private readonly IEnumerable<IComponentReferenceResolver> resolvers;
    private readonly IEnumerable<IExternalComponentMappingProvider> componentProviders;

    public ComponentReferenceResolutionVisitor(
      IEnumerable<IComponentReferenceResolver> resolvers,
      IEnumerable<IExternalComponentMappingProvider> componentProviders)
    {
      this.resolvers = resolvers;
      this.componentProviders = componentProviders;
    }

    public override void ProcessComponent(ReferenceComponentMapping mapping)
    {
      ComponentResolutionContext context = new ComponentResolutionContext()
      {
        ComponentType = mapping.Type,
        ComponentMember = mapping.Member,
        EntityType = mapping.ContainingEntityType
      };
      mapping.AssociateExternalMapping(this.resolvers.Select<IComponentReferenceResolver, ExternalComponentMapping>((Func<IComponentReferenceResolver, ExternalComponentMapping>) (x => x.Resolve(context, this.componentProviders))).FirstOrDefault<ExternalComponentMapping>((Func<ExternalComponentMapping, bool>) (x => x != null)) ?? throw new MissingExternalComponentException(mapping.Type, mapping.ContainingEntityType, mapping.Member));
      mapping.MergedModel.AcceptVisitor((IMappingModelVisitor) this);
    }
  }
}
