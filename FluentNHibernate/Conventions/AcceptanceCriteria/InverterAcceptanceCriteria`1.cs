// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.InverterAcceptanceCriteria`1
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
  public class InverterAcceptanceCriteria<TInspector> : ConcreteAcceptanceCriteria<TInspector> where TInspector : IInspector
  {
    protected override IExpectation CreateExpectation(
      Expression<Func<TInspector, object>> expression,
      IAcceptanceCriterion value)
    {
      return (IExpectation) new InvertedExpectation(base.CreateExpectation(expression, value));
    }

    protected override IExpectation CreateEvalExpectation(
      Expression<Func<TInspector, bool>> expression)
    {
      return (IExpectation) new InvertedExpectation(base.CreateEvalExpectation(expression));
    }

    protected override IExpectation CreateCollectionExpectation<TCollectionItem>(
      Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property,
      ICollectionAcceptanceCriterion<TCollectionItem> value)
    {
      return (IExpectation) new InvertedExpectation(base.CreateCollectionExpectation<TCollectionItem>(property, value));
    }
  }
}
