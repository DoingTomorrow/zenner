// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.SequenceTypePreservingResultOperatorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System.Linq;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public abstract class SequenceTypePreservingResultOperatorBase : 
    SequenceFromSequenceResultOperatorBase
  {
    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      StreamedSequenceInfo streamedSequenceInfo = ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(streamedSequenceInfo.ResultItemType), streamedSequenceInfo.ItemExpression);
    }
  }
}
