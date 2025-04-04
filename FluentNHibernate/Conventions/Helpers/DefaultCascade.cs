// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.DefaultCascade
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public static class DefaultCascade
  {
    public static IHibernateMappingConvention All()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultCascade.All()));
    }

    public static IHibernateMappingConvention None()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultCascade.None()));
    }

    public static IHibernateMappingConvention SaveUpdate()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultCascade.SaveUpdate()));
    }

    public static IHibernateMappingConvention Delete()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultCascade.Delete()));
    }

    public static IHibernateMappingConvention Merge()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultCascade.Merge()));
    }
  }
}
