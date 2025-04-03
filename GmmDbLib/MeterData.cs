// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterData
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

#nullable disable
namespace GmmDbLib
{
  public static class MeterData
  {
    public static List<BaseTables.MeterDataRow> LoadMeterData(BaseDbConnection db, int meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID < 0)
        throw new IndexOutOfRangeException(nameof (meterID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterData WHERE MeterID=@MeterID;", newConnection);
        BaseTables.MeterDataDataTable source = new BaseTables.MeterDataDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@MeterID", meterID);
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<BaseTables.MeterDataRow>) null : source.ToList<BaseTables.MeterDataRow>();
      }
    }

    public static BaseTables.MeterDataRow GetMeterData(
      BaseDbConnection db,
      int meterID,
      DateTime timepoint)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID < 0)
        throw new IndexOutOfRangeException(nameof (meterID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterData WHERE MeterID=@MeterID AND TimePoint=@TimePoint;", newConnection);
        BaseTables.MeterDataDataTable source = new BaseTables.MeterDataDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@MeterID", meterID);
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@TimePoint", timepoint);
        return dataAdapter.Fill((DataTable) source) == 0 ? (BaseTables.MeterDataRow) null : source.ToList<BaseTables.MeterDataRow>()[0];
      }
    }

    public static DateTime? LoadLastBackupTimepoint(BaseDbConnection db, int meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID < 0)
        throw new IndexOutOfRangeException(nameof (meterID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT TOP 1 TimePoint FROM MeterData WHERE MeterID=@MeterID AND ((PValueID > 50000 AND PValueID < 54095) OR PValueID = 1) ORDER BY TimePoint DESC;";
        DbUtil.AddParameter((IDbCommand) command, "@MeterID", meterID);
        return command.ExecuteScalar() as DateTime?;
      }
    }

    public static BaseTables.MeterDataRow LoadLastBackup(BaseDbConnection db, int meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID < 0)
        throw new IndexOutOfRangeException(nameof (meterID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT TOP 1 [MeterID] \r\n                                                         ,[TimePoint]\r\n                                                         ,[PValueID]\r\n                                                         ,[PValue]\r\n                                                         ,[PValueBinary]\r\n                                                         ,[SyncStatus] \r\n                FROM MeterData WHERE MeterID=@MeterID AND ((PValueID > 50000 AND PValueID < 54095) OR PValueID = 1) ORDER BY TimePoint DESC;", newConnection);
        BaseTables.MeterDataDataTable source = new BaseTables.MeterDataDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@MeterID", meterID);
        return dataAdapter.Fill((DataTable) source) == 0 ? (BaseTables.MeterDataRow) null : source.ToList<BaseTables.MeterDataRow>()[0];
      }
    }

    public static DateTime InsertData(
      BaseDbConnection db,
      int meterID,
      MeterData.Special special,
      byte[] zippedBuffer)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID <= 0)
        throw new ArgumentException(nameof (meterID));
      if (zippedBuffer == null)
        throw new ArgumentNullException(nameof (zippedBuffer));
      DateTime utcNow = DateTime.UtcNow;
      DateTime dateTime = utcNow.AddMilliseconds((double) (utcNow.Millisecond * -1));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction transaction = newConnection.BeginTransaction();
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterData", newConnection, transaction);
        BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
        BaseTables.MeterDataRow row = meterDataDataTable.NewMeterDataRow();
        row.MeterID = meterID;
        row.TimePoint = dateTime;
        row.PValueID = (int) special;
        row.PValue = special.ToString();
        row.PValueBinary = zippedBuffer;
        meterDataDataTable.AddMeterDataRow(row);
        dataAdapter.Update((DataTable) meterDataDataTable);
        transaction.Commit();
        return dateTime;
      }
    }

    public static List<BaseTables.MeterDataRow> LoadLastBackups(
      BaseDbConnection db,
      List<int> meterID_List)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID_List == null)
        throw new ArgumentNullException(nameof (meterID_List));
      List<BaseTables.MeterDataRow> meterDataRowList = new List<BaseTables.MeterDataRow>();
      foreach (int meterId in meterID_List)
      {
        BaseTables.MeterDataRow meterDataRow = MeterData.LoadLastBackup(db, meterId);
        if (meterDataRow != null)
          meterDataRowList.Add(meterDataRow);
      }
      return meterDataRowList;
    }

    public enum Special
    {
      EdcEncabulator = 60000, // 0x0000EA60
    }
  }
}
