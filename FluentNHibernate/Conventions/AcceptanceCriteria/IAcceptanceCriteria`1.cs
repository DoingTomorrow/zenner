// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.IAcceptanceCriteria`1
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
  public interface IAcceptanceCriteria<TInspector> where TInspector : IInspector
  {
    IAcceptanceCriteria<TInspector> SameAs<T>() where T : IConventionAcceptance<TInspector>, new();

    IAcceptanceCriteria<TInspector> OppositeOf<T>() where T : IConventionAcceptance<TInspector>, new();

    IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, bool>> evaluation);

    IAcceptanceCriteria<TInspector> Expect(
      Expression<Func<TInspector, object>> propertyExpression,
      IAcceptanceCriterion value);

    IAcceptanceCriteria<TInspector> Expect(
      Expression<Func<TInspector, string>> propertyExpression,
      IAcceptanceCriterion value);

    IAcceptanceCriteria<TInspector> Expect<TCollectionItem>(
      Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property,
      ICollectionAcceptanceCriterion<TCollectionItem> value)
      where TCollectionItem : IInspector;

    IAcceptanceCriteria<TInspector> Any(
      params Action<IAcceptanceCriteria<TInspector>>[] criteriaAlterations);

    IAcceptanceCriteria<TInspector> Any(
      params IAcceptanceCriteria<TInspector>[] subCriteria);

    IAcceptanceCriteria<TInspector> Either(
      Action<IAcceptanceCriteria<TInspector>> criteriaAlterationA,
      Action<IAcceptanceCriteria<TInspector>> criteriaAlterationB);

    IAcceptanceCriteria<TInspector> Either(
      IAcceptanceCriteria<TInspector> subCriteriaA,
      IAcceptanceCriteria<TInspector> subCriteriaB);

    IEnumerable<IExpectation> Expectations { get; }

    bool Matches(IInspector inspector);
  }
}
