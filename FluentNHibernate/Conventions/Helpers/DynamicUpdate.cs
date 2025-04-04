// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.DynamicUpdate
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers.Prebuilt;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public static class DynamicUpdate
  {
    public static IClassConvention AlwaysTrue()
    {
      return (IClassConvention) new BuiltClassConvention((Action<IAcceptanceCriteria<IClassInspector>>) (criteria => criteria.Expect((Expression<Func<IClassInspector, object>>) (x => (object) x.DynamicUpdate), Is.Not.Set)), (Action<IClassInstance>) (x => x.DynamicUpdate()));
    }

    public static IClassConvention AlwaysFalse()
    {
      return (IClassConvention) new BuiltClassConvention((Action<IAcceptanceCriteria<IClassInspector>>) (criteria => criteria.Expect((Expression<Func<IClassInspector, object>>) (x => (object) x.DynamicUpdate), Is.Not.Set)), (Action<IClassInstance>) (x => x.Not.DynamicUpdate()));
    }
  }
}
