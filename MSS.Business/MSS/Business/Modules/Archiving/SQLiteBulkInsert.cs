// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Archiving.SQLiteBulkInsert
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

#nullable disable
namespace MSS.Business.Modules.Archiving
{
  public class SQLiteBulkInsert
  {
    private readonly SQLiteConnection m_dbCon;
    private SQLiteCommand m_cmd;
    private SQLiteTransaction m_trans;
    private readonly Dictionary<string, SQLiteParameter> m_parameters = new Dictionary<string, SQLiteParameter>();
    private uint m_counter = 0;
    private readonly string m_beginInsertText;
    private bool m_allowBulkInsert = true;
    private uint m_commitMax = 10000;
    private readonly string m_tableName;
    private const string m_paramDelim = ":";

    public SQLiteBulkInsert(SQLiteConnection dbConnection, string tableName)
    {
      this.m_dbCon = dbConnection;
      this.m_tableName = tableName;
      StringBuilder stringBuilder = new StringBuilder((int) byte.MaxValue);
      stringBuilder.Append("INSERT INTO [");
      stringBuilder.Append(tableName);
      stringBuilder.Append("] (");
      this.m_beginInsertText = stringBuilder.ToString();
    }

    public bool AllowBulkInsert
    {
      get => this.m_allowBulkInsert;
      set => this.m_allowBulkInsert = value;
    }

    public string CommandText
    {
      get
      {
        if (this.m_parameters.Count < 1)
          return string.Empty;
        StringBuilder stringBuilder = new StringBuilder((int) byte.MaxValue);
        stringBuilder.Append(this.m_beginInsertText);
        foreach (string key in this.m_parameters.Keys)
        {
          stringBuilder.Append('[');
          stringBuilder.Append(key);
          stringBuilder.Append(']');
          stringBuilder.Append(", ");
        }
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        stringBuilder.Append(") VALUES (");
        foreach (string key in this.m_parameters.Keys)
        {
          stringBuilder.Append(":");
          stringBuilder.Append(key);
          stringBuilder.Append(", ");
        }
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        stringBuilder.Append(")");
        return stringBuilder.ToString();
      }
    }

    public uint CommitMax
    {
      get => this.m_commitMax;
      set => this.m_commitMax = value;
    }

    public string TableName => this.m_tableName;

    public string ParamDelimiter => ":";

    public void AddParameter(string name, DbType dbType)
    {
      SQLiteParameter sqLiteParameter = new SQLiteParameter(":" + name, dbType);
      this.m_parameters.Add(name, sqLiteParameter);
    }

    public void Flush()
    {
      try
      {
        if (this.m_trans == null)
          return;
        this.m_trans.Commit();
      }
      catch (Exception ex)
      {
        throw new Exception("Could not commit transaction. See InnerException for more details", ex);
      }
      finally
      {
        if (this.m_trans != null)
          this.m_trans.Dispose();
        this.m_trans = (SQLiteTransaction) null;
        this.m_counter = 0U;
      }
    }

    public void Insert(object[] paramValues)
    {
      if (paramValues.Length != this.m_parameters.Count)
        throw new Exception("The values array count must be equal to the count of the number of parameters.");
      ++this.m_counter;
      if (this.m_counter == 1U)
      {
        if (this.m_allowBulkInsert)
          this.m_trans = this.m_dbCon.BeginTransaction();
        this.m_cmd = this.m_dbCon.CreateCommand();
        foreach (SQLiteParameter parameter in this.m_parameters.Values)
          this.m_cmd.Parameters.Add(parameter);
        if (this.m_parameters.Count < 1)
          throw new SQLiteException("You must add at least one parameter.");
        this.m_cmd.CommandText = this.CommandText;
      }
      int index = 0;
      foreach (DbParameter dbParameter in this.m_parameters.Values)
      {
        dbParameter.Value = paramValues[index];
        ++index;
      }
      this.m_cmd.ExecuteNonQuery();
      if ((int) this.m_counter != (int) this.m_commitMax)
        return;
      try
      {
        if (this.m_trans != null)
          this.m_trans.Commit();
      }
      catch (Exception ex)
      {
      }
      finally
      {
        if (this.m_trans != null)
        {
          this.m_trans.Dispose();
          this.m_trans = (SQLiteTransaction) null;
        }
        this.m_counter = 0U;
      }
    }
  }
}
