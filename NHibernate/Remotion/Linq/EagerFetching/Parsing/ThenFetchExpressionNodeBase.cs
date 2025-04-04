// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.Parsing.ThenFetchExpressionNodeBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.EagerFetching.Parsing
{
  public abstract class ThenFetchExpressionNodeBase(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression relatedObjectSelector) : FetchExpressionNodeBase(parseInfo, relatedObjectSelector)
  {
    protected abstract FetchRequestBase CreateFetchRequest();

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      throw new NotImplementedException("Call ApplyNodeSpecificSemantics instead.");
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      if (!(clauseGenerationContext.GetContextInfo(this.Source) is FetchRequestBase contextInfo))
        throw new ParserException("ThenFetchMany must directly follow another Fetch request.");
      FetchRequestBase fetchRequest = this.CreateFetchRequest();
      FetchRequestBase innerFetchRequest = contextInfo.GetOrAddInnerFetchRequest(fetchRequest);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) innerFetchRequest);
      return queryModel;
    }
  }
}
