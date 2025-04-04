// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedSequenceInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public class StreamedSequenceInfo : IStreamedDataInfo, IEquatable<IStreamedDataInfo>
  {
    private static readonly MethodInfo s_executeMethod = typeof (StreamedSequenceInfo).GetMethod("ExecuteCollectionQueryModel");

    public StreamedSequenceInfo(Type dataType, Expression itemExpression)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (itemExpression), itemExpression);
      this.ResultItemType = ReflectionUtility.GetItemTypeOfIEnumerable(dataType, nameof (dataType));
      if (!this.ResultItemType.IsAssignableFrom(itemExpression.Type))
        throw new ArgumentTypeException(string.Format("ItemExpression is of type '{0}', but should be '{1}' (or derived from it).", (object) itemExpression.Type, (object) this.ResultItemType), nameof (itemExpression), this.ResultItemType, itemExpression.Type);
      this.DataType = dataType;
      this.ItemExpression = itemExpression;
    }

    public Type ResultItemType { get; private set; }

    public Expression ItemExpression { get; private set; }

    public Type DataType { get; private set; }

    public virtual IStreamedDataInfo AdjustDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      if (dataType.IsGenericTypeDefinition)
      {
        try
        {
          dataType = dataType.MakeGenericType(this.ResultItemType);
        }
        catch (ArgumentException ex)
        {
          throw new ArgumentException(string.Format("The generic type definition '{0}' could not be closed over the type of the ResultItemType ('{1}'). {2}", (object) dataType, (object) this.ResultItemType, (object) ex.Message), nameof (dataType));
        }
      }
      try
      {
        return (IStreamedDataInfo) new StreamedSequenceInfo(dataType, this.ItemExpression);
      }
      catch (ArgumentTypeException ex)
      {
        throw new ArgumentException(string.Format("'{0}' cannot be used as the data type for a sequence with an ItemExpression of type '{1}'.", (object) dataType, (object) this.ResultItemType), nameof (dataType));
      }
    }

    public virtual MethodInfo MakeClosedGenericExecuteMethod(MethodInfo genericMethodDefinition)
    {
      ArgumentUtility.CheckNotNull<MethodInfo>(nameof (genericMethodDefinition), genericMethodDefinition);
      if (!genericMethodDefinition.IsGenericMethodDefinition)
        throw new ArgumentException("GenericMethodDefinition must be a generic method definition.", nameof (genericMethodDefinition));
      return genericMethodDefinition.GetGenericArguments().Length == 1 ? genericMethodDefinition.MakeGenericMethod(this.ResultItemType) : throw new ArgumentException("GenericMethodDefinition must have exactly one generic parameter.", nameof (genericMethodDefinition));
    }

    public IStreamedData ExecuteQueryModel(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      IQueryable sequence = ((Func<QueryModel, IQueryExecutor, IEnumerable>) Delegate.CreateDelegate(typeof (Func<QueryModel, IQueryExecutor, IEnumerable>), (object) this, StreamedSequenceInfo.s_executeMethod.MakeGenericMethod(this.ResultItemType)))(queryModel, executor).AsQueryable();
      return (IStreamedData) new StreamedSequence((IEnumerable) sequence, new StreamedSequenceInfo(sequence.GetType(), this.ItemExpression));
    }

    public virtual IEnumerable ExecuteCollectionQueryModel<T>(
      QueryModel queryModel,
      IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (IEnumerable) executor.ExecuteCollection<T>(queryModel);
    }

    public override sealed bool Equals(object obj) => this.Equals(obj as IStreamedDataInfo);

    public virtual bool Equals(IStreamedDataInfo obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      StreamedSequenceInfo streamedSequenceInfo = (StreamedSequenceInfo) obj;
      return this.DataType.Equals(streamedSequenceInfo.DataType) && this.ItemExpression.Equals((object) streamedSequenceInfo.ItemExpression);
    }

    public override int GetHashCode()
    {
      return this.DataType.GetHashCode() ^ this.ItemExpression.GetHashCode();
    }
  }
}
