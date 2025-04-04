// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeTableColumns
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class SqlCeTableColumns : IEnumerable
  {
    public string TableName;
    private readonly Dictionary<string, SqlCeInfoSchemaColumn> columns;
    private readonly Dictionary<string, SqlCeInfoSchemaColumn> parameters;
    private readonly StringComparer _comparer = StringComparer.OrdinalIgnoreCase;

    private SqlCeTableColumns()
    {
    }

    public SqlCeTableColumns(string tableName)
    {
      this.TableName = tableName;
      this.columns = new Dictionary<string, SqlCeInfoSchemaColumn>((IEqualityComparer<string>) this._comparer);
      this.parameters = new Dictionary<string, SqlCeInfoSchemaColumn>((IEqualityComparer<string>) this._comparer);
    }

    public void Add(SqlCeInfoSchemaColumn column)
    {
      this.columns.Add(column.ColumnName, column);
      this.parameters.Add(column.ParamName, column);
    }

    public void Clear(string newTableName)
    {
      this.columns.Clear();
      this.parameters.Clear();
      this.TableName = newTableName;
    }

    public int Count => this.columns != null ? this.columns.Count : 0;

    public SqlCeInfoSchemaColumn GetValueByParameterName(string paramName)
    {
      SqlCeInfoSchemaColumn valueByParameterName = (SqlCeInfoSchemaColumn) null;
      if (!this.parameters.TryGetValue(paramName, out valueByParameterName))
        throw new InvalidOperationException("SqlCeTableColumns.GetValueByParameterName");
      return valueByParameterName;
    }

    public SqlCeInfoSchemaColumn GetValueByColumnName(string columnName)
    {
      SqlCeInfoSchemaColumn valueByColumnName = (SqlCeInfoSchemaColumn) null;
      if (!this.columns.TryGetValue(columnName, out valueByColumnName))
        throw new InvalidOperationException("SqlCeTableColumns.GetValueByColumnName");
      return valueByColumnName;
    }

    public string ParameterNameOf(string columnName)
    {
      SqlCeInfoSchemaColumn infoSchemaColumn = (SqlCeInfoSchemaColumn) null;
      this.columns.TryGetValue(columnName, out infoSchemaColumn);
      return infoSchemaColumn?.ParamName;
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new KeyValuePairsEnumerator<string, SqlCeInfoSchemaColumn>((IEnumerator<KeyValuePair<string, SqlCeInfoSchemaColumn>>) this.columns.GetEnumerator());
    }
  }
}
