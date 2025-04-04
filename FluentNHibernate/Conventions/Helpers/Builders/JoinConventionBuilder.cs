// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Builders.JoinConventionBuilder
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
  public class JoinConventionBuilder : 
    IConventionBuilder<IJoinConvention, IJoinInspector, IJoinInstance>
  {
    public IJoinConvention Always(Action<IJoinInstance> convention)
    {
      return (IJoinConvention) new BuiltJoinConvention((Action<IAcceptanceCriteria<IJoinInspector>>) (accept => { }), convention);
    }

    public IJoinConvention When(
      Action<IAcceptanceCriteria<IJoinInspector>> expectations,
      Action<IJoinInstance> convention)
    {
      return (IJoinConvention) new BuiltJoinConvention(expectations, convention);
    }
  }
}
