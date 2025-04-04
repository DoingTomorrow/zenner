// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.ResultOperatorRewriterResult
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.StreamedData;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class ResultOperatorRewriterResult
  {
    public ResultOperatorRewriterResult(
      IEnumerable<ResultOperatorBase> rewrittenOperators,
      IStreamedDataInfo evaluationType)
    {
      this.RewrittenOperators = rewrittenOperators;
      this.EvaluationType = evaluationType;
    }

    public IEnumerable<ResultOperatorBase> RewrittenOperators { get; private set; }

    public IStreamedDataInfo EvaluationType { get; private set; }
  }
}
