// Decompiled with JetBrains decompiler
// Type: GmmDbLib.ServiceTaskResult
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
  public static class ServiceTaskResult
  {
    public static List<DriverTables.ServiceTaskResultRow> LoadServiceTaskResult(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM ServiceTaskResult;", newConnection);
        DriverTables.ServiceTaskResultDataTable source = new DriverTables.ServiceTaskResultDataTable();
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.ServiceTaskResultRow>) null : source.ToList<DriverTables.ServiceTaskResultRow>();
      }
    }

    public static List<DriverTables.ServiceTaskResultRow> LoadServiceTaskResultByJobID(
      BaseDbConnection db,
      Guid jobID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM ServiceTaskResult WHERE JobID=@JobID;", newConnection);
        DriverTables.ServiceTaskResultDataTable source = new DriverTables.ServiceTaskResultDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@JobID", jobID);
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.ServiceTaskResultRow>) null : source.ToList<DriverTables.ServiceTaskResultRow>();
      }
    }

    public static List<DriverTables.ServiceTaskResultRow> LoadServiceTaskResultByMethodName(
      BaseDbConnection db,
      string methodName)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof (methodName));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM ServiceTaskResult WHERE MethodName=@MethodName;", newConnection);
        DriverTables.ServiceTaskResultDataTable source = new DriverTables.ServiceTaskResultDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@MethodName", methodName);
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.ServiceTaskResultRow>) null : source.ToList<DriverTables.ServiceTaskResultRow>();
      }
    }

    public static List<Guid> LoadJobIdOfServiceTaskResult(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT DISTINCT JobID FROM ServiceTaskResult;";
        List<Guid> guidList = new List<Guid>();
        using (DbDataReader dbDataReader = command.ExecuteReader())
        {
          while (dbDataReader.Read())
          {
            Guid result;
            if (dbDataReader["JobID"] != DBNull.Value && Guid.TryParse(dbDataReader["JobID"].ToString(), out result))
              guidList.Add(result);
          }
        }
        return guidList;
      }
    }

    public static List<string> LoadMethodNameOfServiceTaskResult(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT DISTINCT MethodName FROM ServiceTaskResult;";
        List<string> stringList = new List<string>();
        using (DbDataReader dbDataReader = command.ExecuteReader())
        {
          while (dbDataReader.Read())
            stringList.Add(dbDataReader["MethodName"].ToString());
        }
        return stringList;
      }
    }

    public static bool DeleteServiceTaskResult(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM ServiceTaskResult;";
        return command.ExecuteNonQuery() > 0;
      }
    }

    public static DriverTables.ServiceTaskResultRow SaveServiceTaskResult(
      BaseDbConnection db,
      DateTime timepoint,
      string serialNumber,
      Guid jobID,
      Guid meterID,
      string methodName,
      string resultType,
      string resultObject,
      byte[] rawdata)
    {
      if (string.IsNullOrEmpty(serialNumber))
        throw new ArgumentNullException(nameof (serialNumber));
      if (string.IsNullOrEmpty(methodName))
        throw new ArgumentNullException(nameof (methodName));
      if (string.IsNullOrEmpty(resultType))
        throw new ArgumentNullException(nameof (resultType));
      if (string.IsNullOrEmpty(resultObject))
        throw new ArgumentNullException(nameof (resultObject));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM ServiceTaskResult;", newConnection, out DbCommandBuilder _);
        DriverTables.ServiceTaskResultDataTable taskResultDataTable = new DriverTables.ServiceTaskResultDataTable();
        DriverTables.ServiceTaskResultRow row = taskResultDataTable.NewServiceTaskResultRow();
        row.TimePoint = new DateTime(timepoint.Year, timepoint.Month, timepoint.Day, timepoint.Hour, timepoint.Minute, timepoint.Second);
        row.SerialNumber = serialNumber;
        row.JobID = jobID;
        row.MeterID = meterID;
        row.MethodName = methodName;
        row.ResultType = resultType;
        row.ResultObject = resultObject;
        if (rawdata != null)
          row.RawData = DbUtil.ByteArrayToHexString(rawdata);
        taskResultDataTable.AddServiceTaskResultRow(row);
        if (dataAdapter.Update((DataTable) taskResultDataTable) != 1)
          throw new Exception("Can not add service task result!");
        return row;
      }
    }

    public static bool DeleteServiceTaskResultByJobID(BaseDbConnection db, Guid jobID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM ServiceTaskResult WHERE JobID=@JobID;";
        DbUtil.AddParameter((IDbCommand) command, "@JobID", jobID);
        return command.ExecuteNonQuery() > 0;
      }
    }
  }
}
