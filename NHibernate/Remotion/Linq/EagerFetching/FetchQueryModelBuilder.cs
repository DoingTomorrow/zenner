// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchQueryModelBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Linq;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public class FetchQueryModelBuilder
  {
    private QueryModel _cachedFetchModel;

    public FetchQueryModelBuilder(
      FetchRequestBase fetchRequest,
      QueryModel queryModel,
      int resultOperatorPosition)
    {
      ArgumentUtility.CheckNotNull<FetchRequestBase>(nameof (fetchRequest), fetchRequest);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      this.FetchRequest = fetchRequest;
      this.SourceItemQueryModel = queryModel;
      this.ResultOperatorPosition = resultOperatorPosition;
    }

    public FetchRequestBase FetchRequest { get; private set; }

    public QueryModel SourceItemQueryModel { get; private set; }

    public int ResultOperatorPosition { get; private set; }

    public QueryModel GetOrCreateFetchQueryModel()
    {
      if (this._cachedFetchModel == null)
      {
        QueryModel sourceItemQueryModel = this.SourceItemQueryModel.Clone();
        sourceItemQueryModel.ResultTypeOverride = (Type) null;
        int num = sourceItemQueryModel.ResultOperators.Count - this.ResultOperatorPosition;
        for (int index = 0; index < num; ++index)
          sourceItemQueryModel.ResultOperators.RemoveAt(this.ResultOperatorPosition);
        this._cachedFetchModel = this.FetchRequest.CreateFetchQueryModel(sourceItemQueryModel);
      }
      return this._cachedFetchModel;
    }

    public FetchQueryModelBuilder[] CreateInnerBuilders()
    {
      return this.FetchRequest.InnerFetchRequests.Select<FetchRequestBase, FetchQueryModelBuilder>((Func<FetchRequestBase, FetchQueryModelBuilder>) (request => new FetchQueryModelBuilder(request, this.GetOrCreateFetchQueryModel(), 0))).ToArray<FetchQueryModelBuilder>();
    }
  }
}
