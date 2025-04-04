// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ValueFromSequenceResultOperatorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public abstract class ValueFromSequenceResultOperatorBase : ResultOperatorBase
  {
    public abstract StreamedValue ExecuteInMemory<T>(StreamedSequence sequence);

    public override IStreamedData ExecuteInMemory(IStreamedData input)
    {
      ArgumentUtility.CheckNotNull<IStreamedData>(nameof (input), input);
      return (IStreamedData) this.InvokeGenericExecuteMethod<StreamedSequence, StreamedValue>(input, new Func<StreamedSequence, StreamedValue>(this.ExecuteInMemory<object>));
    }
  }
}
