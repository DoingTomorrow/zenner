// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public class StreamedValue : IStreamedData
  {
    public StreamedValue(object value, StreamedValueInfo streamedValueInfo)
    {
      ArgumentUtility.CheckNotNull<StreamedValueInfo>(nameof (streamedValueInfo), streamedValueInfo);
      if (value != null && !streamedValueInfo.DataType.IsInstanceOfType(value))
        throw new ArgumentTypeException(nameof (value), streamedValueInfo.DataType, value.GetType());
      this.Value = value;
      this.DataInfo = streamedValueInfo;
    }

    public StreamedValueInfo DataInfo { get; private set; }

    public object Value { get; private set; }

    IStreamedDataInfo IStreamedData.DataInfo => (IStreamedDataInfo) this.DataInfo;

    public T GetTypedValue<T>()
    {
      try
      {
        return (T) this.Value;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as type '{0}' because it is of type '{1}'.", (object) typeof (T).FullName, (object) this.Value.GetType().FullName), (Exception) ex);
      }
      catch (NullReferenceException ex)
      {
        throw new InvalidOperationException(string.Format("Cannot retrieve the current value as type '{0}' because it is null.", (object) typeof (T).FullName), (Exception) ex);
      }
    }
  }
}
