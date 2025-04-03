// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterMSS
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
  public static class MeterMSS
  {
    public static List<DriverTables.MeterMSSRow> LoadMeterMSS(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterMSS;", newConnection);
        DriverTables.MeterMSSDataTable source = new DriverTables.MeterMSSDataTable();
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MeterMSSRow>) null : source.ToList<DriverTables.MeterMSSRow>();
      }
    }

    public static List<DriverTables.MeterMSSRow> GetMeterMSS(
      BaseDbConnection db,
      string serialNumber)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (string.IsNullOrEmpty(serialNumber))
        throw new ArgumentNullException(nameof (serialNumber));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterMSS WHERE SerialNumber=@SerialNumber;", newConnection);
        DriverTables.MeterMSSDataTable source = new DriverTables.MeterMSSDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@SerialNumber", serialNumber);
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MeterMSSRow>) null : source.ToList<DriverTables.MeterMSSRow>();
      }
    }

    public static List<DriverTables.MeterMSSRow> GetMeterMSS(
      BaseDbConnection db,
      DbConnection conn,
      string serialNumber)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (string.IsNullOrEmpty(serialNumber))
        throw new ArgumentNullException(nameof (serialNumber));
      DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterMSS WHERE SerialNumber=@SerialNumber;", conn);
      DriverTables.MeterMSSDataTable source = new DriverTables.MeterMSSDataTable();
      DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@SerialNumber", serialNumber);
      return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MeterMSSRow>) null : source.ToList<DriverTables.MeterMSSRow>();
    }

    public static bool DeleteMeterMSS(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MeterMSS;";
        return command.ExecuteNonQuery() > 0;
      }
    }

    public static bool DeleteMeterMSS(BaseDbConnection db, Guid meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MeterMSS WHERE MeterID=@MeterID;";
        DbUtil.AddParameter((IDbCommand) command, "@MeterID", meterID);
        return command.ExecuteNonQuery() == 1;
      }
    }

    public static DriverTables.MeterMSSRow AddMeterMSS(
      BaseDbConnection db,
      Guid meterID,
      string serialNumber)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (string.IsNullOrEmpty(serialNumber))
        throw new ArgumentNullException(nameof (serialNumber));
      using (DbConnection newConnection = db.GetNewConnection())
        return MeterMSS.AddMeterMSS(db, newConnection, meterID, serialNumber);
    }

    public static DriverTables.MeterMSSRow AddMeterMSS(
      BaseDbConnection db,
      DbConnection conn,
      Guid meterID,
      string serialNumber)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (conn == null)
        throw new ArgumentNullException(nameof (conn));
      if (string.IsNullOrEmpty(serialNumber))
        throw new ArgumentNullException(nameof (serialNumber));
      DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterMSS;", conn, out DbCommandBuilder _);
      DriverTables.MeterMSSDataTable meterMssDataTable = new DriverTables.MeterMSSDataTable();
      DriverTables.MeterMSSRow row = meterMssDataTable.NewMeterMSSRow();
      row.MeterID = meterID;
      row.SerialNumber = serialNumber;
      meterMssDataTable.AddMeterMSSRow(row);
      if (dataAdapter.Update((DataTable) meterMssDataTable) != 1)
        throw new Exception("Can not add meter device!");
      return row;
    }
  }
}
