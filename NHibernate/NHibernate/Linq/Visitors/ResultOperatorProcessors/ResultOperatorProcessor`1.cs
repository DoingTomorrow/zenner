// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ResultOperatorProcessor`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ResultOperatorProcessor<T> : ResultOperatorProcessorBase where T : ResultOperatorBase
  {
    private readonly IResultOperatorProcessor<T> _processor;

    public ResultOperatorProcessor(IResultOperatorProcessor<T> processor)
    {
      this._processor = processor;
    }

    public override void Process(
      ResultOperatorBase resultOperator,
      QueryModelVisitor queryModel,
      IntermediateHqlTree tree)
    {
      this._processor.Process((T) resultOperator, queryModel, tree);
    }
  }
}
