// Decompiled with JetBrains decompiler
// Type: GmmDbLib.Device
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using GmmDbLib.TableManagers;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace GmmDbLib
{
  public static class Device
  {
    private static bool isRunning;

    public static event EventHandler<BaseTables.MeterRow> MeterFound;

    public static event EventHandler SearchDone;

    public static DateTime? Save(
      BaseDbConnection db,
      int meterID,
      int meterInfoID,
      uint hardwareTypeID_OR_firmwareVersion,
      string serialNr,
      string orderNr,
      byte[] deviceMemory,
      bool isSaveVersionOld)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (meterID <= 0)
        throw new ArgumentException("MeterID <= 0 not allowed for backup");
      if (hardwareTypeID_OR_firmwareVersion == 0U)
        throw new ArgumentException("hardwareTypeID is zero");
      if (string.IsNullOrEmpty(serialNr))
        throw new ArgumentNullException("serialNr is incorrect");
      if (deviceMemory == null)
        throw new ArgumentNullException("deviceMemory is NULL");
      if (serialNr == null)
        serialNr = string.Empty;
      DateTime utcNow = DateTime.UtcNow;
      DateTime dateTime = utcNow.AddMilliseconds((double) (utcNow.Millisecond * -1));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction transaction = newConnection.BeginTransaction();
        DbDataAdapter dataAdapter1 = db.GetDataAdapter("SELECT * FROM Meter WHERE MeterId = " + meterID.ToString(), newConnection, transaction);
        BaseTables.MeterDataTable meterDataTable = new BaseTables.MeterDataTable();
        dataAdapter1.Fill((DataTable) meterDataTable);
        if (meterDataTable.Rows.Count != 1)
        {
          ZRGlobalID.CheckID(newConnection, transaction, "Meter", "MeterID", meterID);
          BaseTables.MeterRow meterRow = meterDataTable.NewMeterRow();
          meterRow.MeterID = meterID;
          meterRow.MeterInfoID = meterInfoID;
          meterRow.ProductionDate = dateTime;
          meterRow.SerialNr = serialNr;
          meterRow.OrderNr = orderNr;
          meterDataTable.AddMeterRow(meterRow);
          MeterChanges.UpdateMeterRowChanges(db, meterRow, dataAdapter1);
        }
        else
        {
          meterDataTable[0].MeterInfoID = meterInfoID;
          meterDataTable[0].SerialNr = serialNr;
          if (!string.IsNullOrEmpty(orderNr))
            meterDataTable[0].OrderNr = orderNr;
          MeterChanges.UpdateMeterRowChanges(db, meterDataTable[0], dataAdapter1);
        }
        DbDataAdapter dataAdapter2 = db.GetDataAdapter("SELECT * FROM MeterData", newConnection, transaction);
        BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
        BaseTables.MeterDataRow row = meterDataDataTable.NewMeterDataRow();
        row.MeterID = meterID;
        row.PValue = hardwareTypeID_OR_firmwareVersion.ToString();
        row.TimePoint = dateTime;
        if (isSaveVersionOld)
        {
          row.PValueID = 1;
        }
        else
        {
          FirmwareVersion firmwareVersion = new FirmwareVersion(hardwareTypeID_OR_firmwareVersion);
          row.PValueID = 50000 + (int) firmwareVersion.Type;
        }
        row.PValueBinary = deviceMemory;
        meterDataDataTable.AddMeterDataRow(row);
        dataAdapter2.Update((DataTable) meterDataDataTable);
        transaction.Commit();
        return new DateTime?(dateTime);
      }
    }

    public static void StartSearch(BaseDbConnection db, DeviceSearchFilter filter, bool useFilter = true)
    {
      if (Device.isRunning)
        return;
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (filter == null)
        throw new ArgumentNullException(nameof (filter));
      Device.isRunning = true;
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          using (DbConnection newConnection = db.GetNewConnection())
          {
            newConnection.Open();
            DbCommand command = newConnection.CreateCommand();
            if (useFilter)
            {
              if (!filter.IsOldVersion)
              {
                command.CommandText = "SELECT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr\r\n                                                FROM Meter AS m INNER JOIN MeterData AS md ON m.MeterID = md.MeterID\r\n                                                WHERE md.PValueID = @PValueID";
                DbUtil.AddParameter((IDbCommand) command, "PValueID", (int) (50000 + filter.FwType));
                if (!string.IsNullOrEmpty(filter.Serialnumber))
                {
                  command.CommandText += " AND m.SerialNr LIKE @SerialNr";
                  DbUtil.AddParameter((IDbCommand) command, "SerialNr", filter.Serialnumber);
                }
                if (!string.IsNullOrEmpty(filter.MeterID))
                {
                  command.CommandText += " AND m.MeterID=@MeterID";
                  DbUtil.AddParameter((IDbCommand) command, "MeterID", filter.MeterID);
                }
                if (!string.IsNullOrEmpty(filter.OrderNumber))
                {
                  command.CommandText += " AND m.OrderNr=@OrderNr";
                  DbUtil.AddParameter((IDbCommand) command, "OrderNr", filter.OrderNumber);
                }
                DateTime dateTime;
                DateTime? nullable;
                if (filter.ProductionStartDate.HasValue && filter.ProductionEndDate.HasValue)
                {
                  command.CommandText += " AND (m.ProductionDate BETWEEN @ProductionStartDate AND @ProductionEndDate)";
                  DbCommand cmd1 = command;
                  dateTime = filter.ProductionStartDate.Value;
                  DateTime universalTime1 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd1, "ProductionStartDate", universalTime1);
                  DbCommand cmd2 = command;
                  nullable = filter.ProductionEndDate;
                  dateTime = nullable.Value;
                  DateTime universalTime2 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd2, "ProductionEndDate", universalTime2);
                }
                nullable = filter.ApprovalStartDate;
                int num;
                if (nullable.HasValue)
                {
                  nullable = filter.ApprovalEndDate;
                  num = nullable.HasValue ? 1 : 0;
                }
                else
                  num = 0;
                if (num != 0)
                {
                  command.CommandText += " AND (m.ApprovalDate BETWEEN @ApprovalStartDate AND @ApprovalEndDate)";
                  DbCommand cmd3 = command;
                  nullable = filter.ApprovalStartDate;
                  dateTime = nullable.Value;
                  DateTime universalTime3 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd3, "ApprovalStartDate", universalTime3);
                  DbCommand cmd4 = command;
                  nullable = filter.ApprovalEndDate;
                  dateTime = nullable.Value;
                  DateTime universalTime4 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd4, "ApprovalEndDate", universalTime4);
                }
                command.CommandText += " GROUP BY m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr;";
              }
              else
              {
                command.CommandText = db.ConnectionInfo.DbType != MeterDbTypes.Access ? "SELECT DISTINCT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr \r\n                                            FROM Meter as m, MeterData as md, HardwareType as ht   \r\n                                            WHERE md.PValueID = 1 AND m.MeterID=md.MeterID AND md.PValue=CAST(ht.HardwareTypeID AS varchar)" : "SELECT DISTINCT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr \r\n                                            FROM Meter as m, MeterData as md, HardwareType as ht   \r\n                                            WHERE md.PValueID = 1 AND m.MeterID=md.MeterID AND CINT(TRIM(md.PValue))=ht.HardwareTypeID";
                if (!string.IsNullOrEmpty(filter.HardwareName))
                {
                  command.CommandText += " AND ht.HardwareName = @HardwareName";
                  DbUtil.AddParameter((IDbCommand) command, "HardwareName", filter.HardwareName);
                }
                if (!string.IsNullOrEmpty(filter.Serialnumber))
                {
                  command.CommandText += " AND m.SerialNr  LIKE @SerialNr";
                  DbUtil.AddParameter((IDbCommand) command, "SerialNr", filter.Serialnumber);
                }
                if (!string.IsNullOrEmpty(filter.OrderNumber))
                {
                  command.CommandText += " AND m.OrderNr=@OrderNr";
                  DbUtil.AddParameter((IDbCommand) command, "OrderNr", filter.OrderNumber);
                }
                if (!string.IsNullOrEmpty(filter.MeterID))
                {
                  command.CommandText += " AND m.MeterID=@MeterID";
                  DbUtil.AddParameter((IDbCommand) command, "MeterID", filter.MeterID);
                }
                DateTime dateTime;
                DateTime? nullable;
                if (filter.ProductionStartDate.HasValue && filter.ProductionEndDate.HasValue)
                {
                  command.CommandText += " AND (m.ProductionDate BETWEEN @ProductionStartDate AND @ProductionEndDate)";
                  DbCommand cmd5 = command;
                  dateTime = filter.ProductionStartDate.Value;
                  DateTime universalTime5 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd5, "ProductionStartDate", universalTime5);
                  DbCommand cmd6 = command;
                  nullable = filter.ProductionEndDate;
                  dateTime = nullable.Value;
                  DateTime universalTime6 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd6, "ProductionEndDate", universalTime6);
                }
                nullable = filter.ApprovalStartDate;
                int num;
                if (nullable.HasValue)
                {
                  nullable = filter.ApprovalEndDate;
                  num = nullable.HasValue ? 1 : 0;
                }
                else
                  num = 0;
                if (num != 0)
                {
                  command.CommandText += " AND (m.ApprovalDate BETWEEN @ApprovalStartDate AND @ApprovalEndDate)";
                  DbCommand cmd7 = command;
                  nullable = filter.ApprovalStartDate;
                  dateTime = nullable.Value;
                  DateTime universalTime7 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd7, "ApprovalStartDate", universalTime7);
                  DbCommand cmd8 = command;
                  nullable = filter.ApprovalEndDate;
                  dateTime = nullable.Value;
                  DateTime universalTime8 = dateTime.ToUniversalTime();
                  DbUtil.AddParameter((IDbCommand) cmd8, "ApprovalEndDate", universalTime8);
                }
              }
            }
            else
            {
              command.CommandText = "SELECT DISTINCT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr\r\n                                                FROM Meter AS m INNER JOIN MeterData AS md ON m.MeterID = md.MeterID\r\n                                                WHERE ";
              if (!string.IsNullOrEmpty(filter.MeterID))
              {
                command.CommandText += " m.MeterID=@MeterID";
                DbUtil.AddParameter((IDbCommand) command, "MeterID", filter.MeterID);
              }
            }
            DbDataReader dbDataReader = command.ExecuteReader();
            if (!dbDataReader.HasRows)
              return;
            BaseTables.MeterDataTable meterDataTable = new BaseTables.MeterDataTable();
            while (dbDataReader.Read() && Device.isRunning)
            {
              BaseTables.MeterRow e = meterDataTable.NewMeterRow();
              foreach (DataColumn column in (InternalDataCollectionBase) meterDataTable.Columns)
                e[column.ColumnName] = dbDataReader[column.ColumnName];
              if (Device.MeterFound != null)
                Device.MeterFound((object) null, e);
            }
          }
        }
        finally
        {
          Device.isRunning = false;
          if (Device.SearchDone != null)
            Device.SearchDone((object) null, (EventArgs) null);
        }
      }));
    }

    public static void CancelSearch() => Device.isRunning = false;
  }
}
