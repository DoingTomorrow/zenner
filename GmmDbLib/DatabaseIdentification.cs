// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DatabaseIdentification
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public static class DatabaseIdentification
  {
    public static string GetValue(BaseDbConnection db, string key)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (string.IsNullOrEmpty(key))
        throw new ArgumentNullException(nameof (key));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName=@InfoName;";
        DbUtil.AddParameter((IDbCommand) command, "@InfoName", key);
        return command.ExecuteScalar() as string;
      }
    }

    public static string GetDatabaseLocationName(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        return DatabaseIdentification.GetDatabaseLocationName(newConnection, (DbTransaction) null);
      }
    }

    public static string GetDatabaseLocationName(DbConnection connection, DbTransaction transaction)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      DbCommand cmd = connection.State == ConnectionState.Open ? connection.CreateCommand() : throw new Exception("Connection can not be closed!");
      cmd.Transaction = transaction;
      cmd.CommandText = "SELECT InfoData FROM DatabaseIdentification WHERE InfoName=@InfoName;";
      DbUtil.AddParameter((IDbCommand) cmd, "@InfoName", "DatabaseLocationName");
      object obj = cmd.ExecuteScalar();
      return obj != null && obj != DBNull.Value ? Convert.ToString(obj) : (string) null;
    }
  }
}
