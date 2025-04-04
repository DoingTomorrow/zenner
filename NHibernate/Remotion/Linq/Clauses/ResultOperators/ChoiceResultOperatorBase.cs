// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ChoiceResultOperatorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public abstract class ChoiceResultOperatorBase : ValueFromSequenceResultOperatorBase
  {
    protected ChoiceResultOperatorBase(bool returnDefaultWhenEmpty)
    {
      this.ReturnDefaultWhenEmpty = returnDefaultWhenEmpty;
    }

    public bool ReturnDefaultWhenEmpty { get; set; }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      return (IStreamedDataInfo) new StreamedSingleValueInfo(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo).ResultItemType, this.ReturnDefaultWhenEmpty);
    }
  }
}
