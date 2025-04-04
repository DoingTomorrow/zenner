// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchManyRequest
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public class FetchManyRequest : FetchRequestBase
  {
    private readonly Type _relatedObjectType;

    public FetchManyRequest(MemberInfo relationMember)
      : base(ArgumentUtility.CheckNotNull<MemberInfo>(nameof (relationMember), relationMember))
    {
      this._relatedObjectType = ReflectionUtility.GetItemTypeOfIEnumerable(ReflectionUtility.GetMemberReturnType(relationMember), nameof (relationMember));
    }

    protected override void ModifyFetchQueryModel(QueryModel fetchQueryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (fetchQueryModel), fetchQueryModel);
      MemberExpression fromExpression = Expression.MakeMemberAccess((Expression) new QuerySourceReferenceExpression((IQuerySource) fetchQueryModel.MainFromClause), this.RelationMember);
      AdditionalFromClause additionalFromClause = new AdditionalFromClause(fetchQueryModel.GetNewName("#fetch"), this._relatedObjectType, (Expression) fromExpression);
      fetchQueryModel.BodyClauses.Add((IBodyClause) additionalFromClause);
      SelectClause selectClause = new SelectClause((Expression) new QuerySourceReferenceExpression((IQuerySource) additionalFromClause));
      fetchQueryModel.SelectClause = selectClause;
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      FetchManyRequest fetchManyRequest = new FetchManyRequest(this.RelationMember);
      foreach (FetchRequestBase innerFetchRequest in this.InnerFetchRequests)
        fetchManyRequest.GetOrAddInnerFetchRequest((FetchRequestBase) innerFetchRequest.Clone(cloneContext));
      return (ResultOperatorBase) fetchManyRequest;
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }
  }
}
