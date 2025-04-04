// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.SumQueryFunctionInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Function
{
  [Serializable]
  internal class SumQueryFunctionInfo : ClassicAggregateFunction
  {
    public SumQueryFunctionInfo()
      : base("sum", false)
    {
    }

    public override IType ReturnType(IType columnType, IMapping mapping)
    {
      if (columnType == null)
        throw new ArgumentNullException(nameof (columnType));
      SqlType[] sqlTypeArray;
      try
      {
        sqlTypeArray = columnType.SqlTypes(mapping);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
      if (sqlTypeArray.Length != 1)
        throw new QueryException("multi-column type can not be in sum()");
      switch (sqlTypeArray[0].DbType)
      {
        case DbType.Byte:
        case DbType.UInt16:
        case DbType.UInt32:
        case DbType.UInt64:
          return (IType) NHibernateUtil.UInt64;
        case DbType.Double:
        case DbType.Single:
          return (IType) NHibernateUtil.Double;
        case DbType.Int16:
        case DbType.Int32:
        case DbType.Int64:
        case DbType.SByte:
          return (IType) NHibernateUtil.Int64;
        default:
          return columnType;
      }
    }
  }
}
