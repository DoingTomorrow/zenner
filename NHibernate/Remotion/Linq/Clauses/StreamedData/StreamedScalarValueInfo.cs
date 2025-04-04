// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedScalarValueInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public class StreamedScalarValueInfo(Type dataType) : StreamedValueInfo(dataType)
  {
    private static readonly MethodInfo s_executeMethod = typeof (StreamedScalarValueInfo).GetMethod("ExecuteScalarQueryModel");

    public override IStreamedData ExecuteQueryModel(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (IStreamedData) new StreamedValue(((Func<QueryModel, IQueryExecutor, object>) Delegate.CreateDelegate(typeof (Func<QueryModel, IQueryExecutor, object>), (object) this, StreamedScalarValueInfo.s_executeMethod.MakeGenericMethod(this.DataType)))(queryModel, executor), (StreamedValueInfo) this);
    }

    protected override StreamedValueInfo CloneWithNewDataType(Type dataType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (dataType), dataType);
      return (StreamedValueInfo) new StreamedScalarValueInfo(dataType);
    }

    public object ExecuteScalarQueryModel<T>(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (object) executor.ExecuteScalar<T>(queryModel);
    }
  }
}
