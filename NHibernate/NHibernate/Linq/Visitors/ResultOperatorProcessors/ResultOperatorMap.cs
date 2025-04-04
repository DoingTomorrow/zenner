// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ResultOperatorMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ResultOperatorMap
  {
    private readonly Dictionary<Type, ResultOperatorProcessorBase> _map = new Dictionary<Type, ResultOperatorProcessorBase>();

    public void Add<TOperator, TProcessor>()
      where TOperator : ResultOperatorBase
      where TProcessor : IResultOperatorProcessor<TOperator>, new()
    {
      this._map.Add(typeof (TOperator), (ResultOperatorProcessorBase) new ResultOperatorProcessor<TOperator>((IResultOperatorProcessor<TOperator>) new TProcessor()));
    }

    public void Process(
      ResultOperatorBase resultOperator,
      QueryModelVisitor queryModel,
      IntermediateHqlTree tree)
    {
      ResultOperatorProcessorBase operatorProcessorBase;
      if (!this._map.TryGetValue(resultOperator.GetType(), out operatorProcessorBase))
        throw new NotSupportedException(string.Format("The {0} result operator is not current supported", (object) resultOperator.GetType().Name));
      operatorProcessorBase.Process(resultOperator, queryModel, tree);
    }
  }
}
