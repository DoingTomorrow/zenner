﻿// Decompiled with JetBrains decompiler
// Type: SQLite.Orm
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace SQLite
{
  public static class Orm
  {
    public const int DefaultMaxStringLength = 140;
    public const string ImplicitPkName = "Id";
    public const string ImplicitIndexSuffix = "Id";

    public static string SqlDecl(TableMapping.Column p, bool storeDateTimeAsTicks)
    {
      string str = "\"" + p.Name + "\" " + Orm.SqlType(p, storeDateTimeAsTicks) + " ";
      if (p.IsPK)
        str += "primary key ";
      if (p.IsAutoInc)
        str += "autoincrement ";
      if (!p.IsNullable)
        str += "not null ";
      if (!string.IsNullOrEmpty(p.Collation))
        str = str + "collate " + p.Collation + " ";
      return str;
    }

    public static string SqlType(TableMapping.Column p, bool storeDateTimeAsTicks)
    {
      Type columnType = p.ColumnType;
      if (columnType == typeof (bool) || columnType == typeof (byte) || columnType == typeof (ushort) || columnType == typeof (sbyte) || columnType == typeof (short) || columnType == typeof (int))
        return "integer";
      if (columnType == typeof (uint) || columnType == typeof (long))
        return "bigint";
      if (columnType == typeof (float) || columnType == typeof (double) || columnType == typeof (Decimal))
        return "float";
      if (columnType == typeof (string))
      {
        int? maxStringLength = p.MaxStringLength;
        return maxStringLength.HasValue ? "varchar(" + (object) maxStringLength.Value + ")" : "varchar";
      }
      if (columnType == typeof (TimeSpan))
        return "bigint";
      if (columnType == typeof (DateTime))
        return storeDateTimeAsTicks ? "bigint" : "datetime";
      if (columnType == typeof (DateTimeOffset))
        return "bigint";
      if (columnType.IsEnum)
        return "integer";
      if (columnType == typeof (byte[]))
        return "blob";
      if (columnType == typeof (Guid))
        return "varchar(36)";
      throw new NotSupportedException("Don't know about " + (object) columnType);
    }

    public static bool IsPK(MemberInfo p)
    {
      return p.GetCustomAttributes(typeof (PrimaryKeyAttribute), true).Length != 0;
    }

    public static string Collation(MemberInfo p)
    {
      object[] customAttributes = p.GetCustomAttributes(typeof (CollationAttribute), true);
      return customAttributes.Length != 0 ? ((CollationAttribute) customAttributes[0]).Value : string.Empty;
    }

    public static bool IsAutoInc(MemberInfo p)
    {
      return p.GetCustomAttributes(typeof (AutoIncrementAttribute), true).Length != 0;
    }

    public static IEnumerable<IndexedAttribute> GetIndices(MemberInfo p)
    {
      return p.GetCustomAttributes(typeof (IndexedAttribute), true).Cast<IndexedAttribute>();
    }

    public static int? MaxStringLength(PropertyInfo p)
    {
      object[] customAttributes = p.GetCustomAttributes(typeof (MaxLengthAttribute), true);
      return customAttributes.Length != 0 ? new int?(((MaxLengthAttribute) customAttributes[0]).Value) : new int?();
    }

    public static bool IsMarkedNotNull(MemberInfo p)
    {
      return p.GetCustomAttributes(typeof (NotNullAttribute), true).Length != 0;
    }
  }
}
