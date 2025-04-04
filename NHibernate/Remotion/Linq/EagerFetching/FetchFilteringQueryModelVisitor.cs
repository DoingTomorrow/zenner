// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchFilteringQueryModelVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public class FetchFilteringQueryModelVisitor : QueryModelVisitorBase
  {
    private readonly List<FetchQueryModelBuilder> _fetchQueryModelBuilders = new List<FetchQueryModelBuilder>();

    public static FetchQueryModelBuilder[] RemoveFetchRequestsFromQueryModel(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      FetchFilteringQueryModelVisitor visitor = new FetchFilteringQueryModelVisitor();
      queryModel.Accept((IQueryModelVisitor) visitor);
      return visitor.FetchQueryModelBuilders.ToArray<FetchQueryModelBuilder>();
    }

    protected FetchFilteringQueryModelVisitor()
    {
    }

    protected ReadOnlyCollection<FetchQueryModelBuilder> FetchQueryModelBuilders
    {
      get => this._fetchQueryModelBuilders.AsReadOnly();
    }

    public override void VisitResultOperator(
      ResultOperatorBase resultOperator,
      QueryModel queryModel,
      int index)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>(nameof (resultOperator), resultOperator);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (!(resultOperator is FetchRequestBase fetchRequest))
        return;
      queryModel.ResultOperators.RemoveAt(index);
      this._fetchQueryModelBuilders.Add(new FetchQueryModelBuilder(fetchRequest, queryModel, index));
    }
  }
}
