// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.DefaultNamingStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public class DefaultNamingStrategy : INamingStrategy
  {
    public static readonly INamingStrategy Instance = (INamingStrategy) new DefaultNamingStrategy();

    private DefaultNamingStrategy()
    {
    }

    public string ClassToTableName(string className) => StringHelper.Unqualify(className);

    public string PropertyToColumnName(string propertyName) => StringHelper.Unqualify(propertyName);

    public string TableName(string tableName) => tableName;

    public string ColumnName(string columnName) => columnName;

    public string PropertyToTableName(string className, string propertyName)
    {
      return StringHelper.Unqualify(propertyName);
    }

    public string LogicalColumnName(string columnName, string propertyName)
    {
      return !StringHelper.IsNotEmpty(columnName) ? StringHelper.Unqualify(propertyName) : columnName;
    }
  }
}
