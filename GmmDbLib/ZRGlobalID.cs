// Decompiled with JetBrains decompiler
// Type: GmmDbLib.ZRGlobalID
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public static class ZRGlobalID
  {
    public static void CheckID(BaseDbConnection db, string tableName, string fieldName, int id)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        ZRGlobalID.CheckID(newConnection, (DbTransaction) null, tableName, fieldName, id);
      }
    }

    public static void CheckID(
      DbConnection connection,
      DbTransaction transaction,
      string tableName,
      string fieldName,
      int id)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (connection.State != ConnectionState.Open)
        throw new Exception("Connection can not be closed!");
      if (string.IsNullOrEmpty(tableName))
        throw new ArgumentNullException(nameof (tableName));
      if (string.IsNullOrEmpty(fieldName))
        throw new ArgumentNullException(nameof (fieldName));
      string databaseLocationName = DatabaseIdentification.GetDatabaseLocationName(connection, transaction);
      if (string.IsNullOrEmpty(databaseLocationName))
        throw new Exception("Can not determine the DatabaseLocationName from table DatabaseIdentification!");
      DbCommand command = connection.CreateCommand();
      command.Transaction = transaction;
      command.CommandText = "SELECT ZRNextNr, ZRFirstNr, ZRLastNr FROM ZRGlobalID WHERE ZRTableName=@ZRTableName AND ZRFieldName=@ZRFieldName AND DatabaseLocationName=@DatabaseLocationName;";
      DbUtil.AddParameter((IDbCommand) command, "@ZRTableName", tableName);
      DbUtil.AddParameter((IDbCommand) command, "@ZRFieldName", fieldName);
      DbUtil.AddParameter((IDbCommand) command, "@DatabaseLocationName", databaseLocationName);
      using (DbDataReader dbDataReader = command.ExecuteReader())
      {
        int num = dbDataReader.Read() ? dbDataReader.GetInt32(0) : throw new Exception("Can not find the difinition of id range (see table ZRGlobalID)! ZRTableName=" + tableName + ", ZRFieldName=" + fieldName + ", DatabaseLocationName=" + databaseLocationName);
        int int32_1 = dbDataReader.GetInt32(1);
        int int32_2 = dbDataReader.GetInt32(2);
        if (int32_1 > int32_2)
          throw new ArgumentOutOfRangeException("firstNr > lastNr");
        if (num < int32_1)
          throw new ArgumentOutOfRangeException("nextNr < firstNr");
        if (int32_2 < num)
          throw new ArgumentOutOfRangeException("lastNr > nextNr");
        if (id > num && id < int32_2)
          throw new ArgumentOutOfRangeException("The MeterID has invalid range (see database ZRGlobalID table). The MeterID of device is " + id.ToString() + " but into database is FIRST: " + int32_1.ToString() + " LAST: " + int32_2.ToString() + " NEXT: " + num.ToString());
      }
    }
  }
}
