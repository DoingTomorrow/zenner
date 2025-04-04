// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.DynamicInsert
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
  public static class DynamicInsert
  {
    public static IClassConvention AlwaysTrue()
    {
      return (IClassConvention) new BuiltClassConvention((Action<IAcceptanceCriteria<IClassInspector>>) (criteria => criteria.Expect((Expression<Func<IClassInspector, object>>) (x => (object) x.DynamicInsert), Is.Not.Set)), (Action<IClassInstance>) (x => x.DynamicInsert()));
    }

    public static IClassConvention AlwaysFalse()
    {
      return (IClassConvention) new BuiltClassConvention((Action<IAcceptanceCriteria<IClassInspector>>) (criteria => criteria.Expect((Expression<Func<IClassInspector, object>>) (x => (object) x.DynamicInsert), Is.Not.Set)), (Action<IClassInstance>) (x => x.Not.DynamicInsert()));
    }
  }
}
