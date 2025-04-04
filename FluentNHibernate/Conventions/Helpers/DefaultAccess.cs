// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.DefaultAccess
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Properties;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public static class DefaultAccess
  {
    public static IHibernateMappingConvention Field()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.Field()));
    }

    public static IHibernateMappingConvention BackField()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.BackField()));
    }

    public static IHibernateMappingConvention Property()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.Property()));
    }

    public static IHibernateMappingConvention ReadOnlyProperty()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyProperty()));
    }

    public static IHibernateMappingConvention NoOp()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.NoOp()));
    }

    public static IHibernateMappingConvention None()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.None()));
    }

    public static IHibernateMappingConvention CamelCaseField()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.CamelCaseField()));
    }

    public static IHibernateMappingConvention CamelCaseField(CamelCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.CamelCaseField(prefix)));
    }

    public static IHibernateMappingConvention LowerCaseField()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.LowerCaseField()));
    }

    public static IHibernateMappingConvention LowerCaseField(LowerCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.LowerCaseField(prefix)));
    }

    public static IHibernateMappingConvention PascalCaseField(PascalCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.PascalCaseField(prefix)));
    }

    public static IHibernateMappingConvention ReadOnlyPropertyThroughCamelCaseField()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyPropertyThroughCamelCaseField()));
    }

    public static IHibernateMappingConvention ReadOnlyPropertyThroughCamelCaseField(
      CamelCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyPropertyThroughCamelCaseField(prefix)));
    }

    public static IHibernateMappingConvention ReadOnlyPropertyThroughLowerCaseField()
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyPropertyThroughLowerCaseField()));
    }

    public static IHibernateMappingConvention ReadOnlyPropertyThroughLowerCaseField(
      LowerCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyPropertyThroughLowerCaseField(prefix)));
    }

    public static IHibernateMappingConvention ReadOnlyPropertyThroughPascalCaseField(
      PascalCasePrefix prefix)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.ReadOnlyPropertyThroughPascalCaseField(prefix)));
    }

    public static IHibernateMappingConvention Using(string value)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.Using(value)));
    }

    public static IHibernateMappingConvention Using(Type access)
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.Using(access)));
    }

    public static IHibernateMappingConvention Using<T>() where T : IPropertyAccessor
    {
      return (IHibernateMappingConvention) new BuiltHibernateMappingConvention((Action<IAcceptanceCriteria<IHibernateMappingInspector>>) (criteria => { }), (Action<IHibernateMappingInstance>) (instance => instance.DefaultAccess.Using<T>()));
    }
  }
}
