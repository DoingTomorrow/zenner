// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessSingle
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessSingle : 
    ProcessFirstOrSingleBase,
    IResultOperatorProcessor<SingleResultOperator>
  {
    public void Process(
      SingleResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      MethodInfo methodDefinition;
      if (!resultOperator.ReturnDefaultWhenEmpty)
        methodDefinition = ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).Single<object>()));
      else
        methodDefinition = ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).SingleOrDefault<object>()));
      ProcessFirstOrSingleBase.AddClientSideEval(methodDefinition, queryModelVisitor, tree);
    }
  }
}
