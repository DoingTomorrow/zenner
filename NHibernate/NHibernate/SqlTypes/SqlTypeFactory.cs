// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlTypes.SqlTypeFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.SqlTypes
{
  [Serializable]
  public static class SqlTypeFactory
  {
    private static readonly IDictionary<string, SqlType> SqlTypes = (IDictionary<string, SqlType>) new ThreadSafeDictionary<string, SqlType>((IDictionary<string, SqlType>) new Dictionary<string, SqlType>(128));
    public static readonly SqlType Guid = new SqlType(DbType.Guid);
    public static readonly SqlType Boolean = new SqlType(DbType.Boolean);
    public static readonly SqlType Byte = new SqlType(DbType.Byte);
    public static readonly SqlType Currency = new SqlType(DbType.Currency);
    public static readonly SqlType Date = new SqlType(DbType.Date);
    public static readonly SqlType DateTime = new SqlType(DbType.DateTime);
    public static readonly SqlType DateTime2 = new SqlType(DbType.DateTime2);
    public static readonly SqlType DateTimeOffSet = new SqlType(DbType.DateTimeOffset);
    public static readonly SqlType Decimal = new SqlType(DbType.Decimal);
    public static readonly SqlType Double = new SqlType(DbType.Double);
    public static readonly SqlType Int16 = new SqlType(DbType.Int16);
    public static readonly SqlType Int32 = new SqlType(DbType.Int32);
    public static readonly SqlType Int64 = new SqlType(DbType.Int64);
    public static readonly SqlType SByte = new SqlType(DbType.SByte);
    public static readonly SqlType Single = new SqlType(DbType.Single);
    public static readonly SqlType Time = new SqlType(DbType.Time);
    public static readonly SqlType UInt16 = new SqlType(DbType.UInt16);
    public static readonly SqlType UInt32 = new SqlType(DbType.UInt32);
    public static readonly SqlType UInt64 = new SqlType(DbType.UInt64);
    public static readonly SqlType[] NoTypes = new SqlType[0];

    private static T GetTypeWithLen<T>(
      int length,
      SqlTypeFactory.TypeWithLenCreateDelegate createDelegate)
      where T : SqlType
    {
      string keyForLengthBased = SqlTypeFactory.GetKeyForLengthBased(typeof (T).Name, length);
      SqlType typeWithLen;
      if (!SqlTypeFactory.SqlTypes.TryGetValue(keyForLengthBased, out typeWithLen))
      {
        lock (SqlTypeFactory.SqlTypes)
        {
          if (!SqlTypeFactory.SqlTypes.TryGetValue(keyForLengthBased, out typeWithLen))
          {
            typeWithLen = createDelegate(length);
            SqlTypeFactory.SqlTypes.Add(keyForLengthBased, typeWithLen);
          }
        }
      }
      return (T) typeWithLen;
    }

    private static SqlType GetTypeWithPrecision(DbType dbType, byte precision, byte scale)
    {
      string precisionScaleBased = SqlTypeFactory.GetKeyForPrecisionScaleBased(dbType.ToString(), precision, scale);
      SqlType typeWithPrecision;
      if (!SqlTypeFactory.SqlTypes.TryGetValue(precisionScaleBased, out typeWithPrecision))
      {
        typeWithPrecision = new SqlType(dbType, precision, scale);
        SqlTypeFactory.SqlTypes.Add(precisionScaleBased, typeWithPrecision);
      }
      return typeWithPrecision;
    }

    public static AnsiStringSqlType GetAnsiString(int length)
    {
      return SqlTypeFactory.GetTypeWithLen<AnsiStringSqlType>(length, (SqlTypeFactory.TypeWithLenCreateDelegate) (l => (SqlType) new AnsiStringSqlType(l)));
    }

    public static BinarySqlType GetBinary(int length)
    {
      return SqlTypeFactory.GetTypeWithLen<BinarySqlType>(length, (SqlTypeFactory.TypeWithLenCreateDelegate) (l => (SqlType) new BinarySqlType(l)));
    }

    public static BinaryBlobSqlType GetBinaryBlob(int length)
    {
      return SqlTypeFactory.GetTypeWithLen<BinaryBlobSqlType>(length, (SqlTypeFactory.TypeWithLenCreateDelegate) (l => (SqlType) new BinaryBlobSqlType(l)));
    }

    public static StringSqlType GetString(int length)
    {
      return SqlTypeFactory.GetTypeWithLen<StringSqlType>(length, (SqlTypeFactory.TypeWithLenCreateDelegate) (l => (SqlType) new StringSqlType(l)));
    }

    public static StringClobSqlType GetStringClob(int length)
    {
      return SqlTypeFactory.GetTypeWithLen<StringClobSqlType>(length, (SqlTypeFactory.TypeWithLenCreateDelegate) (l => (SqlType) new StringClobSqlType(l)));
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static SqlType GetSqlType(DbType dbType, byte precision, byte scale)
    {
      return SqlTypeFactory.GetTypeWithPrecision(dbType, precision, scale);
    }

    private static string GetKeyForLengthBased(string name, int length)
    {
      return name + "(" + (object) length + ")";
    }

    private static string GetKeyForPrecisionScaleBased(string name, byte precision, byte scale)
    {
      return name + "(" + (object) precision + ", " + (object) scale + ")";
    }

    private delegate SqlType TypeWithLenCreateDelegate(int length);
  }
}
