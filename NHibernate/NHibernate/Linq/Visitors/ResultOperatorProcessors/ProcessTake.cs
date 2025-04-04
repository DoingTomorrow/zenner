// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessTake
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query;
using NHibernate.Hql.Ast;
using NHibernate.Param;
using NHibernate.Type;
using Remotion.Linq.Clauses.ResultOperators;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessTake : IResultOperatorProcessor<TakeResultOperator>
  {
    public void Process(
      TakeResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      VisitorParameters visitorParameters = queryModelVisitor.VisitorParameters;
      NamedParameter namedParameter;
      if (visitorParameters.ConstantToParameterMap.TryGetValue(resultOperator.Count as ConstantExpression, out namedParameter))
      {
        visitorParameters.RequiredHqlParameters.Add(new NamedParameterDescriptor(namedParameter.Name, (IType) null, false));
        tree.AddTakeClause((HqlExpression) tree.TreeBuilder.Parameter(namedParameter.Name));
      }
      else
        tree.AddTakeClause((HqlExpression) tree.TreeBuilder.Constant((object) resultOperator.GetConstantCount()));
    }
  }
}
