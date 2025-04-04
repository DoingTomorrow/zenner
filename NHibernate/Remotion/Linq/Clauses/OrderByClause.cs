// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.OrderByClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Collections;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class OrderByClause : IBodyClause, IClause
  {
    public OrderByClause()
    {
      this.Orderings = new ObservableCollection<Ordering>();
      this.Orderings.ItemInserted += new EventHandler<ObservableCollectionChangedEventArgs<Ordering>>(this.Orderings_ItemAdded);
      this.Orderings.ItemSet += new EventHandler<ObservableCollectionChangedEventArgs<Ordering>>(this.Orderings_ItemAdded);
    }

    public ObservableCollection<Ordering> Orderings { get; private set; }

    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitOrderByClause(this, queryModel, index);
    }

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      foreach (Ordering ordering in (Collection<Ordering>) this.Orderings)
        ordering.TransformExpressions(transformation);
    }

    public OrderByClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      OrderByClause orderByClause = new OrderByClause();
      foreach (Ordering ordering1 in (Collection<Ordering>) this.Orderings)
      {
        Ordering ordering2 = ordering1.Clone(cloneContext);
        orderByClause.Orderings.Add(ordering2);
      }
      return orderByClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext)
    {
      return (IBodyClause) this.Clone(cloneContext);
    }

    public override string ToString()
    {
      return "orderby " + SeparatedStringBuilder.Build<Ordering>(", ", (IEnumerable<Ordering>) this.Orderings);
    }

    private void Orderings_ItemAdded(
      object sender,
      ObservableCollectionChangedEventArgs<Ordering> e)
    {
      ArgumentUtility.CheckNotNull<Ordering>("e.Item", e.Item);
    }
  }
}
