// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Builders.ManyToManyCollectionConventionBuilder
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
  public class ManyToManyCollectionConventionBuilder : 
    IConventionBuilder<IHasManyToManyConvention, IManyToManyCollectionInspector, IManyToManyCollectionInstance>
  {
    public IHasManyToManyConvention Always(Action<IManyToManyCollectionInstance> convention)
    {
      return (IHasManyToManyConvention) new BuiltManyToManyCollectionConvention((Action<IAcceptanceCriteria<IManyToManyCollectionInspector>>) (criteria => { }), convention);
    }

    public IHasManyToManyConvention When(
      Action<IAcceptanceCriteria<IManyToManyCollectionInspector>> expectations,
      Action<IManyToManyCollectionInstance> convention)
    {
      return (IHasManyToManyConvention) new BuiltManyToManyCollectionConvention(expectations, convention);
    }
  }
}
