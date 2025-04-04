// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.CollectionExpectation`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class CollectionExpectation<TInspector, TCollectionItem> : IExpectation where TInspector : IInspector
  {
    private readonly Expression<Func<TInspector, IEnumerable<TCollectionItem>>> expression;
    private readonly ICollectionAcceptanceCriterion<TCollectionItem> criterion;

    public CollectionExpectation(
      Expression<Func<TInspector, IEnumerable<TCollectionItem>>> expression,
      ICollectionAcceptanceCriterion<TCollectionItem> criterion)
    {
      this.expression = expression;
      this.criterion = criterion;
    }

    public bool Matches(TInspector inspector)
    {
      return this.criterion.IsSatisfiedBy<TInspector>(this.expression, inspector);
    }

    bool IExpectation.Matches(IInspector inspector)
    {
      return inspector is TInspector inspector1 && this.Matches(inspector1);
    }
  }
}
