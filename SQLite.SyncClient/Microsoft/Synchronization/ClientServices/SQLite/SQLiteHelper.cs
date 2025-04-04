// Decompiled with JetBrains decompiler
// Type: Microsoft.Synchronization.ClientServices.SQLite.SQLiteHelper
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using Microsoft.Synchronization.ClientServices.Common;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Microsoft.Synchronization.ClientServices.SQLite
{
  internal class SQLiteHelper
  {
    private SQLiteManager manager;
    private string localFilePath;

    public SQLiteHelper(string localfilePath, SQLiteManager sqLiteManager)
    {
      this.manager = sqLiteManager;
      this.localFilePath = localfilePath;
    }

    internal void CreateTable(Type ty)
    {
      using (SQLiteConnection connection = new SQLiteConnection(this.localFilePath))
      {
        try
        {
          TableMapping mapping = this.manager.GetMapping(ty);
          string createTable = SQLiteConstants.CreateTable;
          string createTrackingTable = SQLiteConstants.CreateTrackingTable;
          List<string> stringList1 = new List<string>();
          List<string> stringList2 = new List<string>();
          List<string> stringList3 = new List<string>();
          foreach (TableMapping.Column column in mapping.Columns)
          {
            string str = "\"" + column.Name + "\" " + Orm.SqlType(column) + " ";
            stringList1.Add(str);
            if (column.IsPK)
            {
              stringList2.Add(str);
              stringList3.Add(column.Name + " ");
            }
          }
          string str1 = string.Join(",\n", stringList3.ToArray());
          stringList2.AddRange(SQLiteHelper.GetOfflineEntityMetadataSQlDecl());
          string str2 = string.Join(",\n", stringList1.ToArray());
          string str3 = string.Join(",\n", stringList2.ToArray());
          string str4 = string.Empty;
          if (stringList2.Count > 0)
            str4 = string.Format(",\n PRIMARY KEY ({0})", (object) str1);
          string sql1 = string.Format(createTable, (object) mapping.TableName, (object) str2, (object) str4);
          string sql2 = string.Format(createTrackingTable, (object) mapping.TableName, (object) str3, (object) str4);
          using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql1))
          {
            int num = (int) sqLiteStatement.Step();
          }
          using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql2))
          {
            int num = (int) sqLiteStatement.Step();
          }
          Dictionary<string, SQLiteHelper.IndexInfo> dictionary = new Dictionary<string, SQLiteHelper.IndexInfo>();
          foreach (TableMapping.Column column in mapping.Columns)
          {
            foreach (IndexedAttribute index in column.Indices)
            {
              string key = index.Name ?? mapping.TableName + "_" + column.Name;
              SQLiteHelper.IndexInfo indexInfo;
              if (!dictionary.TryGetValue(key, out indexInfo))
              {
                indexInfo = new SQLiteHelper.IndexInfo()
                {
                  TableName = mapping.TableName,
                  Unique = index.Unique,
                  Columns = new List<SQLiteHelper.IndexedColumn>()
                };
                dictionary.Add(key, indexInfo);
              }
              if (index.Unique != indexInfo.Unique)
                throw new Exception("All the columns in an index must have the same value for their Unique property");
              indexInfo.Columns.Add(new SQLiteHelper.IndexedColumn()
              {
                Order = index.Order,
                ColumnName = column.Name
              });
            }
          }
          foreach (string key in dictionary.Keys)
          {
            SQLiteHelper.IndexInfo indexInfo = dictionary[key];
            string str5 = string.Join("\",\"", indexInfo.Columns.OrderBy<SQLiteHelper.IndexedColumn, int>((Func<SQLiteHelper.IndexedColumn, int>) (i => i.Order)).Select<SQLiteHelper.IndexedColumn, string>((Func<SQLiteHelper.IndexedColumn, string>) (i => i.ColumnName)).ToArray<string>());
            string sql3 = string.Format("create {3} index if not exists \"{0}\" on \"{1}\"(\"{2}\")", (object) key, (object) indexInfo.TableName, (object) str5, indexInfo.Unique ? (object) "unique" : (object) "");
            using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql3))
            {
              int num = (int) sqLiteStatement.Step();
            }
          }
          this.CreateTriggers(ty, connection);
        }
        catch (Exception ex)
        {
          throw;
        }
      }
    }

    private void DisableTriggers(TableMapping map, SQLiteConnection connection)
    {
      try
      {
        string sql1 = string.Format(SQLiteConstants.DeleteTriggerAfterDelete, (object) map.TableName);
        string sql2 = string.Format(SQLiteConstants.DeleteTriggerAfterInsert, (object) map.TableName);
        string.Format(SQLiteConstants.DeleteTriggerAfterUpdate, (object) map.TableName);
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql1))
        {
          int num = (int) sqLiteStatement.Step();
        }
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql2))
        {
          int num = (int) sqLiteStatement.Step();
        }
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql2))
        {
          int num = (int) sqLiteStatement.Step();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private void CreateTriggers(Type ty, SQLiteConnection connection)
    {
      try
      {
        TableMapping mapping = this.manager.GetMapping(ty);
        List<string> list = ((IEnumerable<TableMapping.Column>) mapping.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (primaryKey => " new." + primaryKey.Name)).ToList<string>();
        string str1 = string.Join(", ", ((IEnumerable<TableMapping.Column>) mapping.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (column => column.Name)));
        string str2 = string.Join(", ", (IEnumerable<string>) list);
        string str3 = string.Join(" and ", (IEnumerable<string>) ((IEnumerable<TableMapping.Column>) mapping.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (primaryKey => string.Format("\"{0}\" = old.\"{0}\" ", (object) primaryKey.Name))).ToList<string>());
        string sql1 = string.Format(SQLiteConstants.CreateTriggerAfterInsert, (object) mapping.TableName, (object) str1, (object) str2);
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql1))
        {
          int num = (int) sqLiteStatement.Step();
        }
        string sql2 = string.Format(SQLiteConstants.CreateTriggerAfterUpdate, (object) mapping.TableName, (object) str3);
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql2))
        {
          int num = (int) sqLiteStatement.Step();
        }
        string sql3 = string.Format(SQLiteConstants.CreateTriggerAfterDelete, (object) mapping.TableName, (object) str3);
        using (ISQLiteStatement sqLiteStatement = connection.Prepare(sql3))
        {
          int num = (int) sqLiteStatement.Step();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    internal void DeleteTombstoneTrackingEntities(Type ty, List<SQLiteOfflineEntity> entities)
    {
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        TableMapping mapping = this.manager.GetMapping(ty);
        string str = string.Join(" and ", (IEnumerable<string>) ((IEnumerable<TableMapping.Column>) mapping.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (primaryKey => string.Format("\"{0}\" = ? ", (object) primaryKey.Name))).ToList<string>());
        string sql = string.Format(SQLiteConstants.DeleteTrackingFromChanges, (object) mapping.TableName, (object) str);
        try
        {
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Begin Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          foreach (SQLiteOfflineEntity entity in entities)
          {
            using (ISQLiteStatement stmt = sqLiteConnection.Prepare(sql))
            {
              SQLiteHelper.BindParameter(stmt, 1, (object) entity.ServiceMetadata.Id);
              int num = (int) stmt.Step();
              stmt.Reset();
              stmt.ClearBindings();
            }
          }
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Commit Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
        }
        catch (Exception ex)
        {
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Rollback Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          throw;
        }
      }
    }

    internal void UpdateDirtyTrackingEntities(Type ty, List<SQLiteOfflineEntity> entities)
    {
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        TableMapping mapping = this.manager.GetMapping(ty);
        string sql = string.Format(SQLiteConstants.UpdateDirtyTracking, (object) mapping.TableName);
        try
        {
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Begin Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          using (ISQLiteStatement stmt = sqLiteConnection.Prepare(sql))
          {
            foreach (SQLiteOfflineEntity entity in entities)
            {
              SQLiteHelper.BindParameter(stmt, 1, (object) entity.ServiceMetadata.IsTombstone);
              SQLiteHelper.BindParameter(stmt, 2, (object) 0);
              SQLiteHelper.BindParameter(stmt, 3, (object) entity.ServiceMetadata.ETag);
              string str = string.Empty;
              if (entity.ServiceMetadata.EditUri != (Uri) null && entity.ServiceMetadata.EditUri.IsAbsoluteUri)
                str = entity.ServiceMetadata.EditUri.AbsoluteUri;
              SQLiteHelper.BindParameter(stmt, 4, (object) str);
              SQLiteHelper.BindParameter(stmt, 5, (object) entity.ServiceMetadata.Id);
              int num = (int) stmt.Step();
              stmt.Reset();
              stmt.ClearBindings();
            }
          }
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Commit Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
        }
        catch (Exception ex)
        {
          using (ISQLiteStatement sqLiteStatement = sqLiteConnection.Prepare("Rollback Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          throw;
        }
      }
    }

    internal bool IsConflicted(Type ty, SQLiteOfflineEntity entity)
    {
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        TableMapping mapping = this.manager.GetMapping(ty);
        string sql = string.Format(SQLiteConstants.SelectChanges, (object) mapping.TableName);
        sqLiteConnection.Prepare(sql);
      }
      return false;
    }

    internal void MergeEntities(Type ty, List<SQLiteOfflineEntity> entities)
    {
      using (SQLiteConnection connection = new SQLiteConnection(this.localFilePath))
      {
        TableMapping mapping = this.manager.GetMapping(ty);
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        List<string> stringList3 = new List<string>();
        List<string> stringList4 = new List<string>();
        foreach (TableMapping.Column column in mapping.Columns)
        {
          stringList1.Add("[" + column.Name + "]");
          stringList2.Add("? ");
        }
        foreach (TableMapping.Column primaryKey in mapping.PrimaryKeys)
        {
          stringList3.Add("\"" + primaryKey.Name + "\"");
          stringList4.Add("? ");
        }
        string str1 = string.Join(",", stringList1.ToArray());
        string str2 = string.Join(",", stringList2.ToArray());
        string str3 = string.Join(",", stringList3.ToArray());
        string str4 = string.Join(",", stringList4.ToArray());
        string sql1 = string.Format(SQLiteConstants.InsertOrReplaceFromChanges, (object) mapping.TableName, (object) str1, (object) str2);
        string sql2 = string.Format(SQLiteConstants.InsertOrReplaceTrackingFromChanges, (object) mapping.TableName, (object) str3, (object) str4);
        string sql3 = string.Format(SQLiteConstants.DeleteFromChanges, (object) mapping.TableName, (object) mapping.GetPrimaryKeysWhereClause);
        string sql4 = string.Format(SQLiteConstants.DeleteTrackingFromChanges, (object) mapping.TableName, (object) mapping.GetPrimaryKeysWhereClause);
        string str5 = string.Join(", ", ((IEnumerable<TableMapping.Column>) mapping.PrimaryKeys).Select<TableMapping.Column, string>((Func<TableMapping.Column, string>) (column => column.Name)));
        string sql5 = string.Format(SQLiteConstants.SelectItemPrimaryKeyFromTrackingChangesWithOemID, (object) mapping.TableName, (object) str5);
        try
        {
          using (ISQLiteStatement sqLiteStatement = connection.Prepare("Begin Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          this.DisableTriggers(mapping, connection);
          ISQLiteStatement stmt1 = connection.Prepare(sql1);
          ISQLiteStatement stmt2 = connection.Prepare(sql5);
          ISQLiteStatement stmt3 = connection.Prepare(sql3);
          ISQLiteStatement stmt4 = connection.Prepare(sql4);
          ISQLiteStatement stmt5 = connection.Prepare(sql2);
          foreach (SQLiteOfflineEntity entity in entities)
          {
            if (entity.ServiceMetadata.IsTombstone)
            {
              SQLiteHelper.BindParameter(stmt2, 1, (object) entity.ServiceMetadata.Id);
              object[] objArray = new object[mapping.PrimaryKeys.Length];
              while (stmt2.Step() == SQLiteResult.ROW)
              {
                for (int index = 0; index < objArray.Length; ++index)
                  objArray[index] = SQLiteHelper.ReadCol(stmt2, index, mapping.PrimaryKeys[index].ColumnType);
              }
              stmt2.Reset();
              for (int index = 0; index < objArray.Length; ++index)
              {
                SQLiteHelper.BindParameter(stmt3, index + 1, objArray[index]);
                SQLiteHelper.BindParameter(stmt4, index + 1, objArray[index]);
              }
              int num1 = (int) stmt3.Step();
              stmt3.Reset();
              stmt3.ClearBindings();
              int num2 = (int) stmt4.Step();
              stmt4.Reset();
              stmt4.ClearBindings();
            }
            else
            {
              TableMapping.Column[] columns = mapping.Columns;
              for (int index = 0; index < columns.Length; ++index)
              {
                object obj = columns[index].GetValue((object) entity);
                SQLiteHelper.BindParameter(stmt1, index + 1, obj);
              }
              int num3 = (int) stmt1.Step();
              stmt1.Reset();
              stmt1.ClearBindings();
              SQLiteHelper.BindParameter(stmt5, 1, (object) entity.ServiceMetadata.IsTombstone);
              SQLiteHelper.BindParameter(stmt5, 2, (object) 0);
              SQLiteHelper.BindParameter(stmt5, 3, (object) entity.ServiceMetadata.Id);
              SQLiteHelper.BindParameter(stmt5, 4, (object) "ETag");
              string str6 = string.Empty;
              if (entity.ServiceMetadata.EditUri != (Uri) null && entity.ServiceMetadata.EditUri.IsAbsoluteUri)
                str6 = entity.ServiceMetadata.EditUri.AbsoluteUri;
              SQLiteHelper.BindParameter(stmt5, 5, (object) str6);
              SQLiteHelper.BindParameter(stmt5, 6, (object) DateTime.UtcNow);
              for (int index = 0; index < mapping.PrimaryKeys.Length; ++index)
              {
                object obj = mapping.PrimaryKeys[index].GetValue((object) entity);
                SQLiteHelper.BindParameter(stmt5, index + 7, obj);
              }
              int num4 = (int) stmt5.Step();
              stmt5.Reset();
              stmt5.ClearBindings();
            }
          }
          using (ISQLiteStatement sqLiteStatement = connection.Prepare("Commit Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          stmt3.Dispose();
          stmt4.Dispose();
          stmt2.Dispose();
          stmt1.Dispose();
          stmt5.Dispose();
        }
        catch (Exception ex)
        {
          using (ISQLiteStatement sqLiteStatement = connection.Prepare("Rollback Transaction"))
          {
            int num = (int) sqLiteStatement.Step();
          }
          throw;
        }
        this.CreateTriggers(ty, connection);
      }
    }

    internal IEnumerable<SQLiteOfflineEntity> GetChanges(
      OfflineSchema schema,
      DateTime lastModifiedDate)
    {
      List<SQLiteOfflineEntity> changes = new List<SQLiteOfflineEntity>();
      using (SQLiteConnection sqLiteConnection = new SQLiteConnection(this.localFilePath))
      {
        try
        {
          foreach (Type collection in schema.Collections)
          {
            TableMapping mapping = this.manager.GetMapping(collection);
            string selectChanges = SQLiteConstants.SelectChanges;
            List<string> stringList = new List<string>();
            string str1 = string.Empty;
            foreach (TableMapping.Column column in mapping.Columns)
            {
              if (!column.IsPK)
                stringList.Add("[s].[" + column.Name + "]");
              if (column.IsPK)
              {
                stringList.Add("[t].[" + column.Name + "]");
                str1 = column.Name;
              }
            }
            string str2 = string.Join(",\n", stringList.ToArray());
            string sql = string.Format(selectChanges, (object) mapping.TableName, (object) str1, (object) str2);
            using (ISQLiteStatement stmt = sqLiteConnection.Prepare(sql))
            {
              try
              {
                SQLiteHelper.BindParameter(stmt, 1, (object) lastModifiedDate);
                TableMapping.Column[] columnArray = new TableMapping.Column[mapping.Columns.Length];
                for (int index = 0; index < columnArray.Length; ++index)
                {
                  string columnName = stmt.ColumnName(index);
                  if (mapping.FindColumn(columnName) != null)
                    columnArray[index] = mapping.FindColumn(columnName);
                }
                while (stmt.Step() == SQLiteResult.ROW)
                {
                  SQLiteOfflineEntity instance = (SQLiteOfflineEntity) Activator.CreateInstance(mapping.MappedType);
                  for (int index = 0; index < columnArray.Length; ++index)
                  {
                    if (columnArray[index] != null)
                    {
                      object val = SQLiteHelper.ReadCol(stmt, index, columnArray[index].ColumnType);
                      columnArray[index].SetValue((object) instance, val);
                    }
                  }
                  int index1 = ((IEnumerable<TableMapping.Column>) mapping.Columns).Count<TableMapping.Column>();
                  instance.ServiceMetadata = new OfflineEntityMetadata();
                  instance.ServiceMetadata.IsTombstone = (bool) SQLiteHelper.ReadCol(stmt, index1, typeof (bool));
                  instance.ServiceMetadata.Id = (string) SQLiteHelper.ReadCol(stmt, index1 + 1, typeof (string));
                  instance.ServiceMetadata.ETag = (string) SQLiteHelper.ReadCol(stmt, index1 + 2, typeof (string));
                  string uriString = (string) SQLiteHelper.ReadCol(stmt, index1 + 3, typeof (string));
                  instance.ServiceMetadata.EditUri = string.IsNullOrEmpty(uriString) ? (Uri) null : new Uri(uriString);
                  changes.Add(instance);
                }
              }
              catch (Exception ex)
              {
                throw;
              }
              finally
              {
                stmt.Reset();
                stmt.ClearBindings();
              }
            }
          }
        }
        catch (Exception ex)
        {
          throw;
        }
      }
      return (IEnumerable<SQLiteOfflineEntity>) changes;
    }

    private static IEnumerable<string> GetOfflineEntityMetadataSQlDecl()
    {
      return (IEnumerable<string>) new List<string>()
      {
        "\"Oem_AbsoluteUri\" varchar(255) ",
        "\"Oem_IsTombstone\" integer ",
        "\"Oem_IsDirty\" integer ",
        "\"Oem_Id\" varchar(255) ",
        "\"Oem_Etag\" varchar(255) ",
        "\"Oem_EditUri\" varchar(255) ",
        "\"Oem_LastModifiedDate\" datetime "
      };
    }

    public static object ReadCol(ISQLiteStatement stmt, int index, Type clrType)
    {
      object obj = stmt[index];
      if (obj == null)
        return (object) null;
      if (clrType == typeof (string))
        return (object) (obj as string);
      if (clrType == typeof (int))
        return (object) Convert.ToInt32(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (bool))
        return (object) ((long) obj == 1L);
      if (clrType == typeof (double))
        return (object) Convert.ToDouble(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (float))
        return (object) Convert.ToSingle(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (DateTime))
        return (object) DateTime.Parse((string) obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (DateTimeOffset))
        return (object) DateTime.Parse((string) obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (TimeSpan))
        return (object) TimeSpan.FromTicks((long) obj);
      if (clrType.GetTypeInfo().IsEnum)
        return (object) Convert.ToInt32(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (long))
        return (object) (long) obj;
      if (clrType == typeof (uint))
        return (object) Convert.ToUInt32(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (Decimal))
        return (object) Convert.ToDecimal(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (byte))
        return (object) Convert.ToByte(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (ushort))
        return (object) Convert.ToUInt16(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (short))
        return (object) Convert.ToInt16(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (sbyte))
        return (object) Convert.ToSByte(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      if (clrType == typeof (byte[]))
        return (object) (byte[]) obj;
      if (clrType == typeof (Guid))
        return (object) new Guid((string) obj);
      throw new NotSupportedException("Don't know how to read " + (object) clrType);
    }

    internal static void BindParameter(ISQLiteStatement stmt, int index, object value)
    {
      int num1;
      switch (value)
      {
        case null:
          stmt.Bind(index, (object) null);
          return;
        case int num2:
          stmt.Bind(index, (object) num2);
          return;
        case string _:
          stmt.Bind(index, (object) (string) value);
          return;
        case byte _:
        case ushort _:
        case sbyte _:
          num1 = 1;
          break;
        default:
          num1 = value is short ? 1 : 0;
          break;
      }
      if (num1 != 0)
      {
        stmt.Bind(index, (object) Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture));
      }
      else
      {
        int num3;
        switch (value)
        {
          case bool flag:
            stmt.Bind(index, (object) (flag ? 1 : 0));
            return;
          case uint _:
            num3 = 1;
            break;
          default:
            num3 = value is long ? 1 : 0;
            break;
        }
        if (num3 != 0)
        {
          stmt.Bind(index, (object) Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture));
        }
        else
        {
          int num4;
          switch (value)
          {
            case float _:
            case double _:
              num4 = 1;
              break;
            default:
              num4 = value is Decimal ? 1 : 0;
              break;
          }
          if (num4 != 0)
          {
            stmt.Bind(index, (object) Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture));
          }
          else
          {
            switch (value)
            {
              case DateTimeOffset dateTimeOffset:
                stmt.Bind(index, (object) dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss"));
                break;
              case TimeSpan timeSpan:
                stmt.Bind(index, (object) timeSpan.Ticks);
                break;
              case DateTime dateTime:
                stmt.Bind(index, (object) dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                break;
              default:
                if (value.GetType().IsEnum)
                {
                  stmt.Bind(index, (object) Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture));
                  break;
                }
                switch (value)
                {
                  case byte[] _:
                    byte[] numArray = (byte[]) value;
                    stmt.Bind(index, (object) numArray);
                    return;
                  case Guid guid:
                    stmt.Bind(index, (object) guid.ToString());
                    return;
                  default:
                    throw new NotSupportedException("Cannot store type: " + (object) value.GetType());
                }
            }
          }
        }
      }
    }

    private struct IndexedColumn
    {
      public int Order;
      public string ColumnName;
    }

    private struct IndexInfo
    {
      public string TableName;
      public bool Unique;
      public List<SQLiteHelper.IndexedColumn> Columns;
    }
  }
}
