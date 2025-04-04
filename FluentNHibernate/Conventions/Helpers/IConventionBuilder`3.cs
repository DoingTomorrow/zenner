// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.IConventionBuilder`3
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers
{
  public interface IConventionBuilder<TConvention, TInspector, TInstance>
    where TConvention : IConvention<TInspector, TInstance>
    where TInspector : IInspector
    where TInstance : TInspector
  {
    TConvention Always(Action<TInstance> convention);

    TConvention When(
      Action<IAcceptanceCriteria<TInspector>> expectations,
      Action<TInstance> convention);
  }
}
