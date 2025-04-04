// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.RemoveUnnecessaryBodyOperators
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.EagerFetching;
using System;
using System.Linq;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class RemoveUnnecessaryBodyOperators : QueryModelVisitorBase
  {
    private RemoveUnnecessaryBodyOperators()
    {
    }

    public static void ReWrite(QueryModel queryModel)
    {
      new RemoveUnnecessaryBodyOperators().VisitQueryModel(queryModel);
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      if (resultOperator is CountResultOperator || resultOperator is LongCountResultOperator)
      {
        foreach (IBodyClause bodyClause in queryModel.BodyClauses.Where<IBodyClause>((Func<IBodyClause, bool>) (bc => bc is OrderByClause)).ToList<IBodyClause>())
          queryModel.BodyClauses.Remove(bodyClause);
      }
      if (resultOperator is CastResultOperator)
        Array.ForEach<CastResultOperator>(queryModel.ResultOperators.OfType<CastResultOperator>().ToArray<CastResultOperator>(), (Action<CastResultOperator>) (castOperator => queryModel.ResultOperators.Remove((ResultOperatorBase) castOperator)));
      if (resultOperator is AnyResultOperator)
      {
        Array.ForEach<FetchOneRequest>(queryModel.ResultOperators.OfType<FetchOneRequest>().ToArray<FetchOneRequest>(), (Action<FetchOneRequest>) (op => queryModel.ResultOperators.Remove((ResultOperatorBase) op)));
        Array.ForEach<FetchManyRequest>(queryModel.ResultOperators.OfType<FetchManyRequest>().ToArray<FetchManyRequest>(), (Action<FetchManyRequest>) (op => queryModel.ResultOperators.Remove((ResultOperatorBase) op)));
      }
      base.VisitResultOperator(resultOperator, queryModel, index);
    }
  }
}
