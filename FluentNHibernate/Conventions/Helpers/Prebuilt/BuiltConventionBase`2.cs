// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Helpers.Prebuilt.BuiltConventionBase`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Helpers.Prebuilt
{
  [Multiple]
  public abstract class BuiltConventionBase<TInspector, TInstance> where TInspector : IInspector
  {
    private readonly Action<IAcceptanceCriteria<TInspector>> accept;
    private readonly Action<TInstance> convention;

    public BuiltConventionBase(
      Action<IAcceptanceCriteria<TInspector>> accept,
      Action<TInstance> convention)
    {
      this.accept = accept;
      this.convention = convention;
    }

    public void Accept(IAcceptanceCriteria<TInspector> acceptance) => this.accept(acceptance);

    public void Apply(TInstance instance) => this.convention(instance);
  }
}
