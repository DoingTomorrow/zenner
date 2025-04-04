// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.FetchOneRequest
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.EagerFetching
{
  public class FetchOneRequest(MemberInfo relationMember) : FetchRequestBase(ArgumentUtility.CheckNotNull<MemberInfo>(nameof (relationMember), relationMember))
  {
    protected override void ModifyFetchQueryModel(QueryModel fetchQueryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (fetchQueryModel), fetchQueryModel);
      fetchQueryModel.SelectClause.Selector = (Expression) Expression.MakeMemberAccess(fetchQueryModel.SelectClause.Selector, this.RelationMember);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      FetchOneRequest fetchOneRequest = new FetchOneRequest(this.RelationMember);
      foreach (FetchRequestBase innerFetchRequest in this.InnerFetchRequests)
        fetchOneRequest.GetOrAddInnerFetchRequest((FetchRequestBase) innerFetchRequest.Clone(cloneContext));
      return (ResultOperatorBase) fetchOneRequest;
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }
  }
}
