// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.Expectation`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class Expectation<TInspector> : IExpectation where TInspector : IInspector
  {
    private readonly Expression<Func<TInspector, object>> expression;
    private readonly IAcceptanceCriterion value;

    public Expectation(Expression<Func<TInspector, object>> expression, IAcceptanceCriterion value)
    {
      this.expression = expression;
      this.value = value;
    }

    public bool Matches(TInspector inspector)
    {
      return this.value.IsSatisfiedBy<TInspector>(this.expression, inspector);
    }

    bool IExpectation.Matches(IInspector inspector)
    {
      return inspector is TInspector inspector1 && this.Matches(inspector1);
    }
  }
}
