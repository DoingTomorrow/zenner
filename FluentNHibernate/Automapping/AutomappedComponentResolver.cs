// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutomappedComponentResolver
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutomappedComponentResolver : IComponentReferenceResolver
  {
    private readonly AutoMapper mapper;
    private IAutomappingConfiguration cfg;

    public AutomappedComponentResolver(AutoMapper mapper, IAutomappingConfiguration cfg)
    {
      this.mapper = mapper;
      this.cfg = cfg;
    }

    public ExternalComponentMapping Resolve(
      ComponentResolutionContext context,
      IEnumerable<IExternalComponentMappingProvider> componentProviders)
    {
      ExternalComponentMapping componentMapping = new ExternalComponentMapping(ComponentType.Component);
      componentMapping.Member = context.ComponentMember;
      componentMapping.ContainingEntityType = context.EntityType;
      ExternalComponentMapping mapping = componentMapping;
      mapping.Set<string>((Expression<Func<ComponentMapping, string>>) (x => x.Name), 0, context.ComponentMember.Name);
      mapping.Set<Type>((Expression<Func<ComponentMapping, Type>>) (x => x.Type), 0, context.ComponentType);
      if (context.ComponentMember.IsProperty && !context.ComponentMember.CanWrite)
        mapping.Set<string>((Expression<Func<ComponentMapping, string>>) (x => x.Access), 0, this.cfg.GetAccessStrategyForReadOnlyProperty(context.ComponentMember).ToString());
      this.mapper.FlagAsMapped(context.ComponentType);
      this.mapper.MergeMap(context.ComponentType, (ClassMappingBase) mapping, (IList<Member>) new List<Member>());
      return mapping;
    }
  }
}
