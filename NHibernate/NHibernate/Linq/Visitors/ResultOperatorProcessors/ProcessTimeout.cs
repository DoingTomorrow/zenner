// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessTimeout
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  internal class ProcessTimeout : IResultOperatorProcessor<TimeoutResultOperator>
  {
    public void Process(
      TimeoutResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      tree.AddAdditionalCriteria((Action<IQuery, IDictionary<string, Tuple<object, IType>>>) ((q, p) => q.SetTimeout((int) resultOperator.Timeout.Value)));
    }
  }
}
