// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.TableMapping
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  public class TableMapping
  {
    private readonly TableMapping.Column autoPk;
    private TableMapping.Column[] insertColumns;
    private TableMapping.Column[] insertOrReplaceColumns;

    public Type MappedType { get; private set; }

    public string TableName { get; private set; }

    public TableMapping.Column[] Columns { get; private set; }

    public TableMapping.Column[] PrimaryKeys { get; private set; }

    public string GetByPrimaryKeySql { get; private set; }

    public string GetPrimaryKeysWhereClause { get; private set; }

    public TableMapping(Type type)
    {
      this.MappedType = type;
      TableAttribute tableAttribute = (TableAttribute) ((IEnumerable<object>) type.GetCustomAttributes(typeof (TableAttribute), true)).FirstOrDefault<object>();
      this.TableName = tableAttribute != null ? tableAttribute.Name : this.MappedType.Name;
      PropertyInfo[] properties = this.MappedType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
      List<TableMapping.Column> columnList1 = new List<TableMapping.Column>();
      foreach (PropertyInfo prop in properties)
      {
        bool flag = prop.GetCustomAttributes(typeof (IgnoreAttribute), true).Length != 0;
        if (prop.CanWrite && !flag)
          columnList1.Add(new TableMapping.Column(prop));
      }
      this.Columns = columnList1.ToArray();
      List<TableMapping.Column> columnList2 = new List<TableMapping.Column>();
      foreach (TableMapping.Column column in this.Columns)
      {
        if (column.IsAutoInc && column.IsPK)
          this.autoPk = column;
        if (column.IsPK)
          columnList2.Add(column);
      }
      this.HasAutoIncPK = this.autoPk != null;
      if (columnList2.Count > 0)
      {
        this.PrimaryKeys = columnList2.ToArray();
        this.GetPrimaryKeysWhereClause = string.Join(" and ", (IEnumerable<string>) ((IEnumerable<TableMapping.Column>) this.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (primaryKey => string.Format("\"{0}\" = ? ", (object) primaryKey.Name))).ToList<string>());
        this.GetByPrimaryKeySql = string.Format("select * from \"{0}\" where {1}", (object) this.TableName, (object) this.GetPrimaryKeysWhereClause);
      }
      else
        this.GetByPrimaryKeySql = string.Format("select * from \"{0}\" limit 1", (object) this.TableName);
    }

    public bool HasAutoIncPK { get; private set; }

    public void SetAutoIncPK(object obj, long id)
    {
      if (this.autoPk == null)
        return;
      this.autoPk.SetValue(obj, Convert.ChangeType((object) id, this.autoPk.ColumnType, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public TableMapping.Column[] InsertColumns
    {
      get
      {
        if (this.insertColumns == null)
          this.insertColumns = ((IEnumerable<TableMapping.Column>) this.Columns).Where<TableMapping.Column>((Func<TableMapping.Column, bool>) (c => !c.IsAutoInc)).ToArray<TableMapping.Column>();
        return this.insertColumns;
      }
    }

    public TableMapping.Column[] InsertOrReplaceColumns
    {
      get
      {
        if (this.insertOrReplaceColumns == null)
          this.insertOrReplaceColumns = ((IEnumerable<TableMapping.Column>) this.Columns).ToArray<TableMapping.Column>();
        return this.insertOrReplaceColumns;
      }
    }

    public TableMapping.Column FindColumnWithPropertyName(string propertyName)
    {
      return ((IEnumerable<TableMapping.Column>) this.Columns).FirstOrDefault<TableMapping.Column>((Func<TableMapping.Column, bool>) (c => c.PropertyName == propertyName));
    }

    public TableMapping.Column FindColumn(string columnName)
    {
      return ((IEnumerable<TableMapping.Column>) this.Columns).FirstOrDefault<TableMapping.Column>((Func<TableMapping.Column, bool>) (c => c.Name == columnName));
    }

    protected internal void Dispose()
    {
    }

    public class Column
    {
      private PropertyInfo _prop;

      public string Name { get; private set; }

      public string PropertyName => this._prop.Name;

      public Type ColumnType { get; private set; }

      public string Collation { get; private set; }

      public bool IsAutoInc { get; private set; }

      public bool IsAutoGuid { get; private set; }

      public bool IsPK { get; private set; }

      public IEnumerable<IndexedAttribute> Indices { get; set; }

      public bool IsNullable { get; private set; }

      public int MaxStringLength { get; private set; }

      public Column(PropertyInfo prop)
      {
        ColumnAttribute columnAttribute = (ColumnAttribute) ((IEnumerable<object>) prop.GetCustomAttributes(typeof (ColumnAttribute), true)).FirstOrDefault<object>();
        this._prop = prop;
        this.Name = columnAttribute == null ? prop.Name : columnAttribute.Name;
        Type type = Nullable.GetUnderlyingType(prop.PropertyType);
        if ((object) type == null)
          type = prop.PropertyType;
        this.ColumnType = type;
        this.Collation = Orm.Collation((MemberInfo) prop);
        this.IsPK = Orm.IsPK((MemberInfo) prop);
        bool flag = Orm.IsAutoInc((MemberInfo) prop);
        this.IsAutoGuid = flag && this.ColumnType == typeof (Guid);
        this.IsAutoInc = flag && !this.IsAutoGuid;
        this.Indices = Orm.GetIndices((MemberInfo) prop);
        if (!this.Indices.Any<IndexedAttribute>() && !this.IsPK && this.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
          this.Indices = (IEnumerable<IndexedAttribute>) new IndexedAttribute[1]
          {
            new IndexedAttribute()
          };
        this.IsNullable = !this.IsPK;
        this.MaxStringLength = Orm.MaxStringLength(prop);
      }

      public void SetValue(object obj, object val)
      {
        this._prop.SetValue(obj, val, (object[]) null);
      }

      public object GetValue(object obj) => this._prop.GetValue(obj, (object[]) null);
    }
  }
}
