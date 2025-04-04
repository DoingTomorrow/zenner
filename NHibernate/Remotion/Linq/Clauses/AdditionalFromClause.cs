// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.AdditionalFromClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class AdditionalFromClause(string itemName, Type itemType, Expression fromExpression) : 
    FromClauseBase(ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName), ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType), ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression)),
    IBodyClause,
    IClause
  {
    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitAdditionalFromClause(this, queryModel, index);
    }

    public virtual AdditionalFromClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      AdditionalFromClause additionalFromClause = new AdditionalFromClause(this.ItemName, this.ItemType, this.FromExpression);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) additionalFromClause));
      return additionalFromClause;
    }

    IBodyClause IBodyClause.Clone(CloneContext cloneContext)
    {
      return (IBodyClause) this.Clone(cloneContext);
    }
  }
}
