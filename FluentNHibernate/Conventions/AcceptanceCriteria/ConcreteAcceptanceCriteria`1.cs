// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.AcceptanceCriteria.ConcreteAcceptanceCriteria`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.AcceptanceCriteria
{
  public class ConcreteAcceptanceCriteria<TInspector> : IAcceptanceCriteria<TInspector> where TInspector : IInspector
  {
    private readonly List<IExpectation> expectations = new List<IExpectation>();

    public IAcceptanceCriteria<TInspector> SameAs<T>() where T : IConventionAcceptance<TInspector>, new()
    {
      new T().Accept((IAcceptanceCriteria<TInspector>) this);
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> OppositeOf<T>() where T : IConventionAcceptance<TInspector>, new()
    {
      InverterAcceptanceCriteria<TInspector> criteria = new InverterAcceptanceCriteria<TInspector>();
      new T().Accept((IAcceptanceCriteria<TInspector>) criteria);
      foreach (IExpectation expectation in criteria.Expectations)
        this.expectations.Add(expectation);
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public virtual IAcceptanceCriteria<TInspector> Expect(
      Expression<Func<TInspector, object>> propertyExpression,
      IAcceptanceCriterion value)
    {
      this.expectations.Add(this.CreateExpectation(propertyExpression, value));
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> Expect(
      Expression<Func<TInspector, string>> expression,
      IAcceptanceCriterion value)
    {
      this.expectations.Add(this.CreateExpectation(ExpressionBuilder.Create<TInspector>(expression.ToMember<TInspector, string>()), value));
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> Expect(Expression<Func<TInspector, bool>> evaluation)
    {
      this.expectations.Add(this.CreateEvalExpectation(evaluation));
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> Expect<TCollectionItem>(
      Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property,
      ICollectionAcceptanceCriterion<TCollectionItem> value)
      where TCollectionItem : IInspector
    {
      this.expectations.Add(this.CreateCollectionExpectation<TCollectionItem>(property, value));
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> Any(
      params Action<IAcceptanceCriteria<TInspector>>[] criteriaAlterations)
    {
      List<IAcceptanceCriteria<TInspector>> acceptanceCriteriaList = new List<IAcceptanceCriteria<TInspector>>();
      foreach (Action<IAcceptanceCriteria<TInspector>> criteriaAlteration in criteriaAlterations)
      {
        ConcreteAcceptanceCriteria<TInspector> acceptanceCriteria = new ConcreteAcceptanceCriteria<TInspector>();
        criteriaAlteration((IAcceptanceCriteria<TInspector>) acceptanceCriteria);
        acceptanceCriteriaList.Add((IAcceptanceCriteria<TInspector>) acceptanceCriteria);
      }
      return this.Any(acceptanceCriteriaList.ToArray());
    }

    public IAcceptanceCriteria<TInspector> Any(
      params IAcceptanceCriteria<TInspector>[] subCriteria)
    {
      this.expectations.Add((IExpectation) new AnyExpectation<TInspector>((IEnumerable<IAcceptanceCriteria<TInspector>>) subCriteria));
      return (IAcceptanceCriteria<TInspector>) this;
    }

    public IAcceptanceCriteria<TInspector> Either(
      Action<IAcceptanceCriteria<TInspector>> criteriaAlterationA,
      Action<IAcceptanceCriteria<TInspector>> criteriaAlterationB)
    {
      return this.Any(new Action<IAcceptanceCriteria<TInspector>>[2]
      {
        criteriaAlterationA,
        criteriaAlterationB
      });
    }

    public IAcceptanceCriteria<TInspector> Either(
      IAcceptanceCriteria<TInspector> subCriteriaA,
      IAcceptanceCriteria<TInspector> subCriteriaB)
    {
      return this.Any(new IAcceptanceCriteria<TInspector>[2]
      {
        subCriteriaA,
        subCriteriaB
      });
    }

    public IEnumerable<IExpectation> Expectations => (IEnumerable<IExpectation>) this.expectations;

    public bool Matches(IInspector inspector)
    {
      return this.Expectations.All<IExpectation>((Func<IExpectation, bool>) (x => x.Matches(inspector)));
    }

    protected virtual IExpectation CreateExpectation(
      Expression<Func<TInspector, object>> expression,
      IAcceptanceCriterion value)
    {
      return (IExpectation) new Expectation<TInspector>(expression, value);
    }

    protected virtual IExpectation CreateEvalExpectation(
      Expression<Func<TInspector, bool>> expression)
    {
      return (IExpectation) new EvalExpectation<TInspector>(expression);
    }

    protected virtual IExpectation CreateCollectionExpectation<TCollectionItem>(
      Expression<Func<TInspector, IEnumerable<TCollectionItem>>> property,
      ICollectionAcceptanceCriterion<TCollectionItem> value)
      where TCollectionItem : IInspector
    {
      return (IExpectation) new CollectionExpectation<TInspector, TCollectionItem>(property, value);
    }
  }
}
