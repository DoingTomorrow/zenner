// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.ComponentMapComponentReferenceResolver
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
  public class ComponentMapComponentReferenceResolver : IComponentReferenceResolver
  {
    public ExternalComponentMapping Resolve(
      ComponentResolutionContext context,
      IEnumerable<IExternalComponentMappingProvider> componentProviders)
    {
      IEnumerable<IExternalComponentMappingProvider> source = componentProviders.Where<IExternalComponentMappingProvider>((Func<IExternalComponentMappingProvider, bool>) (x => x.Type == context.ComponentType));
      if (source.Count<IExternalComponentMappingProvider>() > 1)
        throw new AmbiguousComponentReferenceException(context.ComponentType, context.EntityType, context.ComponentMember);
      return source.SingleOrDefault<IExternalComponentMappingProvider>()?.GetComponentMapping();
    }
  }
}
