// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedSequence
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public class StreamedSequence : IStreamedData
  {
    public StreamedSequence(IEnumerable sequence, StreamedSequenceInfo streamedSequenceInfo)
    {
      ArgumentUtility.CheckNotNull<IEnumerable>(nameof (sequence), sequence);
      ArgumentUtility.CheckNotNull<StreamedSequenceInfo>(nameof (streamedSequenceInfo), streamedSequenceInfo);
      if (!streamedSequenceInfo.DataType.IsInstanceOfType((object) sequence))
        throw new ArgumentTypeException(nameof (sequence), streamedSequenceInfo.DataType, sequence.GetType());
      this.DataInfo = streamedSequenceInfo;
      this.Sequence = sequence;
    }

    public StreamedSequenceInfo DataInfo { get; private set; }

    object IStreamedData.Value => (object) this.Sequence;

    IStreamedDataInfo IStreamedData.DataInfo => (IStreamedDataInfo) this.DataInfo;

    public IEnumerable Sequence { get; private set; }

    public IEnumerable<T> GetTypedSequence<T>()
    {
      try
      {
        return (IEnumerable<T>) this.Sequence;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as a sequence with item type '{0}' because its items are of type '{1}'.", (object) typeof (T).FullName, (object) this.DataInfo.ResultItemType.FullName), (Exception) ex);
      }
    }
  }
}
