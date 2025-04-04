// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.Parsing.OuterFetchExpressionNodeBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.EagerFetching.Parsing
{
  public abstract class OuterFetchExpressionNodeBase(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression relatedObjectSelector) : FetchExpressionNodeBase(parseInfo, relatedObjectSelector)
  {
    protected abstract FetchRequestBase CreateFetchRequest();

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      FetchRequestBase contextInfo = queryModel.ResultOperators.OfType<FetchRequestBase>().FirstOrDefault<FetchRequestBase>((Func<FetchRequestBase, bool>) (r => r.RelationMember.Equals((object) this.RelationMember)));
      if (contextInfo == null)
        return base.ApplyNodeSpecificSemantics(queryModel, clauseGenerationContext);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      return queryModel;
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      FetchRequestBase fetchRequest = this.CreateFetchRequest();
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) fetchRequest);
      return (ResultOperatorBase) fetchRequest;
    }
  }
}
