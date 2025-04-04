// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.StreamedData.StreamedSingleValueInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.StreamedData
{
  public class StreamedSingleValueInfo : StreamedValueInfo
  {
    private static readonly MethodInfo s_executeMethod = typeof (StreamedSingleValueInfo).GetMethod("ExecuteSingleQueryModel");
    private readonly bool _returnDefaultWhenEmpty;

    public StreamedSingleValueInfo(Type dataType, bool returnDefaultWhenEmpty)
      : base(dataType)
    {
      this._returnDefaultWhenEmpty = returnDefaultWhenEmpty;
    }

    public bool ReturnDefaultWhenEmpty => this._returnDefaultWhenEmpty;

    public override IStreamedData ExecuteQueryModel(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (IStreamedData) new StreamedValue(((Func<QueryModel, IQueryExecutor, object>) Delegate.CreateDelegate(typeof (Func<QueryModel, IQueryExecutor, object>), (object) this, StreamedSingleValueInfo.s_executeMethod.MakeGenericMethod(this.DataType)))(queryModel, executor), (StreamedValueInfo) this);
    }

    protected override StreamedValueInfo CloneWithNewDataType(Type dataType)
    {
      return (StreamedValueInfo) new StreamedSingleValueInfo(dataType, this._returnDefaultWhenEmpty);
    }

    public object ExecuteSingleQueryModel<T>(QueryModel queryModel, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return (object) executor.ExecuteSingle<T>(queryModel, this._returnDefaultWhenEmpty);
    }

    public override bool Equals(IStreamedDataInfo obj)
    {
      return base.Equals(obj) && ((StreamedSingleValueInfo) obj)._returnDefaultWhenEmpty == this._returnDefaultWhenEmpty;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ this._returnDefaultWhenEmpty.GetHashCode();
    }
  }
}
