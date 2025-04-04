// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.ImprovedNamingStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Text;

#nullable disable
namespace NHibernate.Cfg
{
  public class ImprovedNamingStrategy : INamingStrategy
  {
    public static readonly INamingStrategy Instance = (INamingStrategy) new ImprovedNamingStrategy();

    private ImprovedNamingStrategy()
    {
    }

    public string ClassToTableName(string className)
    {
      return this.AddUnderscores(StringHelper.Unqualify(className));
    }

    public string PropertyToColumnName(string propertyName)
    {
      return this.AddUnderscores(StringHelper.Unqualify(propertyName));
    }

    public string TableName(string tableName) => this.AddUnderscores(tableName);

    public string ColumnName(string columnName) => this.AddUnderscores(columnName);

    public string PropertyToTableName(string className, string propertyName)
    {
      return this.AddUnderscores(StringHelper.Unqualify(propertyName));
    }

    public string LogicalColumnName(string columnName, string propertyName)
    {
      return !StringHelper.IsNotEmpty(columnName) ? StringHelper.Unqualify(propertyName) : columnName;
    }

    private string AddUnderscores(string name)
    {
      char[] charArray = name.Replace('.', '_').ToCharArray();
      StringBuilder stringBuilder = new StringBuilder(charArray.Length);
      char c1 = 'A';
      foreach (char c2 in charArray)
      {
        if (c2 != '_' && char.IsUpper(c2) && !char.IsUpper(c1))
          stringBuilder.Append('_');
        stringBuilder.Append(char.ToLowerInvariant(c2));
        c1 = c2;
      }
      return stringBuilder.ToString();
    }
  }
}
