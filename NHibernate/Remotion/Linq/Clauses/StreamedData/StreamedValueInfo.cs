// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedValueInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public abstract class StreamedValueInfo : IStreamedDataInfo, IEquatable<IStreamedDataInfo>
  {
    protected StreamedValueInfo(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      this.DataType = dataType;
    }

    public Type DataType { get; private set; }

    public abstract IStreamedData ExecuteQueryModel(QueryModel queryModel, IQueryExecutor executor);

    protected abstract StreamedValueInfo CloneWithNewDataType(Type dataType);

    public virtual IStreamedDataInfo AdjustDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      return dataType.IsAssignableFrom(this.DataType) ? (IStreamedDataInfo) this.CloneWithNewDataType(dataType) : throw new ArgumentException(string.Format("'{0}' cannot be used as the new data type for a value of type '{1}'.", (object) dataType, (object) this.DataType), nameof (dataType));
    }

    public MethodInfo MakeClosedGenericExecuteMethod(MethodInfo genericMethodDefinition)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (genericMethodDefinition), genericMethodDefinition);
      if (!genericMethodDefinition.IsGenericMethodDefinition)
        throw new ArgumentException("GenericMethodDefinition must be a generic method definition.", nameof (genericMethodDefinition));
      return genericMethodDefinition.GetGenericArguments().Length == 1 ? genericMethodDefinition.MakeGenericMethod(this.DataType) : throw new ArgumentException("GenericMethodDefinition must have exactly one generic parameter.", nameof (genericMethodDefinition));
    }

    public override sealed bool Equals(object obj) => this.Equals(obj as IStreamedDataInfo);

    public virtual bool Equals(IStreamedDataInfo obj)
    {
      return obj != null && this.GetType() == obj.GetType() && this.DataType.Equals(((StreamedValueInfo) obj).DataType);
    }

    public override int GetHashCode() => this.DataType.GetHashCode();
  }
}
