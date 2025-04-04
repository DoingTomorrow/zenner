// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.SequenceFromSequenceResultOperatorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public abstract class SequenceFromSequenceResultOperatorBase : ResultOperatorBase
  {
    public abstract StreamedSequence ExecuteInMemory<T>(StreamedSequence input);

    public override IStreamedData ExecuteInMemory(IStreamedData input)
    {
      ArgumentUtility.CheckNotNull<IStreamedData>(nameof (input), input);
      return (IStreamedData) this.InvokeGenericExecuteMethod<StreamedSequence, StreamedSequence>(input, new Func<StreamedSequence, StreamedSequence>(this.ExecuteInMemory<object>));
    }
  }
}
