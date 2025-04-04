// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModelBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Remotion.Linq
{
  public class QueryModelBuilder
  {
    private readonly List<ResultOperatorBase> _resultOperators = new List<ResultOperatorBase>();
    private readonly List<IBodyClause> _bodyClauses = new List<IBodyClause>();

    public MainFromClause MainFromClause { get; private set; }

    public SelectClause SelectClause { get; private set; }

    public ReadOnlyCollection<IBodyClause> BodyClauses => this._bodyClauses.AsReadOnly();

    public ReadOnlyCollection<ResultOperatorBase> ResultOperators
    {
      get => this._resultOperators.AsReadOnly();
    }

    public void AddClause(IClause clause)
    {
      ArgumentUtility.CheckNotNull<IClause>(nameof (clause), clause);
      switch (clause)
      {
        case MainFromClause mainFromClause:
          this.MainFromClause = this.MainFromClause == null ? mainFromClause : throw new InvalidOperationException("Builder already has a MainFromClause.");
          break;
        case SelectClause selectClause:
          this.SelectClause = this.SelectClause == null ? selectClause : throw new InvalidOperationException("Builder already has a SelectClause.");
          break;
        case IBodyClause bodyClause:
          this._bodyClauses.Add(bodyClause);
          break;
        default:
          throw new ArgumentTypeException(string.Format("Cannot add clause of type '{0}' to a query model. Only instances of IBodyClause, MainFromClause, or ISelectGroupClause are supported.", (object) clause.GetType()), nameof (clause), (Type) null, clause.GetType());
      }
    }

    public void AddResultOperator(ResultOperatorBase resultOperator)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>(nameof (resultOperator), resultOperator);
      this._resultOperators.Add(resultOperator);
    }

    public QueryModel Build()
    {
      if (this.MainFromClause == null)
        throw new InvalidOperationException("No MainFromClause was added to the builder.");
      QueryModel queryModel = this.SelectClause != null ? new QueryModel(this.MainFromClause, this.SelectClause) : throw new InvalidOperationException("No SelectOrGroupClause was added to the builder.");
      foreach (IBodyClause bodyClause in this.BodyClauses)
        queryModel.BodyClauses.Add(bodyClause);
      foreach (ResultOperatorBase resultOperator in this.ResultOperators)
        queryModel.ResultOperators.Add(resultOperator);
      return queryModel;
    }
  }
}
