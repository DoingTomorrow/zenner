// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.MainFromClause
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
  public class MainFromClause(string itemName, Type itemType, Expression fromExpression) : 
    FromClauseBase(ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName), ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType), ArgumentUtility.CheckNotNull<Expression>(nameof (fromExpression), fromExpression))
  {
    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitMainFromClause(this, queryModel);
    }

    public MainFromClause Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      MainFromClause mainFromClause = new MainFromClause(this.ItemName, this.ItemType, this.FromExpression);
      cloneContext.QuerySourceMapping.AddMapping((IQuerySource) this, (Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return mainFromClause;
    }
  }
}
