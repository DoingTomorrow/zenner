// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Builders.MapConventionBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Builders
{
  [Obsolete("Use CollectionConventionBuilder")]
  internal class MapConventionBuilder : 
    IConventionBuilder<IMapConvention, IMapInspector, IMapInstance>
  {
    public IMapConvention Always(Action<IMapInstance> convention)
    {
      return (IMapConvention) new BuiltMapConvention((Action<IAcceptanceCriteria<IMapInspector>>) (accept => { }), convention);
    }

    public IMapConvention When(
      Action<IAcceptanceCriteria<IMapInspector>> expectations,
      Action<IMapInstance> convention)
    {
      return (IMapConvention) new BuiltMapConvention(expectations, convention);
    }
  }
}
