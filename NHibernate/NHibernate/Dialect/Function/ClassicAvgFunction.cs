// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Function.ClassicAvgFunction
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
  public class ClassicAvgFunction : ClassicAggregateFunction
  {
    public ClassicAvgFunction()
      : base("avg", false)
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
      SqlType sqlType = sqlTypeArray.Length == 1 ? sqlTypeArray[0] : throw new QueryException("multi-column type can not be in avg()");
      return sqlType.DbType == DbType.Int16 || sqlType.DbType == DbType.Int32 || sqlType.DbType == DbType.Int64 ? (IType) NHibernateUtil.Single : columnType;
    }
  }
}
