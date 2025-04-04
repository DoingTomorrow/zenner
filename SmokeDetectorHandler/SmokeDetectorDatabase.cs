// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.SmokeDetectorDatabase
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using GmmDbLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  internal static class SmokeDetectorDatabase
  {
    private static Logger logger = LogManager.GetLogger(nameof (SmokeDetectorDatabase));
    public const int METER_HARDWARE_ID_SMOKE = 101;
    private static bool isCanceled;

    public static event EventHandlerEx<Meter> OnMeterFound;

    public static event System.EventHandler OnDone;

    internal static MeterData LoadMeterData(uint meterId, DateTime timePoint)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterData WHERE MeterID=@MeterID AND TimePoint=@TimePoint;";
          MeterDatabase.AddParameter(cmd, "@MeterID", (double) meterId);
          MeterDatabase.AddParameter(cmd, "@TimePoint", timePoint);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null || !dataReader.Read())
            return (MeterData) null;
          MeterData meterData = new MeterData()
          {
            MeterID = Convert.ToInt32(dataReader["MeterID"]),
            TimePoint = Convert.ToDateTime(dataReader["TimePoint"]),
            HardwareTypeID = Convert.ToInt32(dataReader["PValue"]),
            Buffer = dataReader["PValueBinary"] == DBNull.Value || dataReader["PValueBinary"] == null ? (byte[]) null : (byte[]) dataReader["PValueBinary"]
          };
          if (dataReader.Read())
            throw new Exception("INTERNAL ERROR: The meter data is no unique for tis time point! MeterID: " + meterId.ToString() + ", Timepoint: " + timePoint.ToString());
          return meterData;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        SmokeDetectorDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (MeterData) null;
      }
    }

    internal static List<MeterData> LoadMeterData(int meterID)
    {
      return SmokeDetectorDatabase.LoadMeterData(meterID, new int?());
    }

    internal static List<MeterData> LoadMeterData(int meterID, int? maxRows)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.CommandText = "SELECT * FROM MeterData WHERE MeterID=@MeterID ORDER BY TimePoint DESC;";
          MeterDatabase.AddParameter(cmd, "@MeterID", meterID);
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<MeterData>) null;
          List<MeterData> meterDataList = new List<MeterData>();
          int? nullable1 = maxRows;
          while (dataReader.Read())
          {
            int? nullable2 = nullable1;
            int num = 0;
            if (nullable2.GetValueOrDefault() <= num & nullable2.HasValue)
              return meterDataList;
            nullable2 = nullable1;
            nullable1 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() - 1) : new int?();
            if (dataReader["PValue"] != DBNull.Value && dataReader["PValueBinary"] != DBNull.Value)
            {
              int int32 = Convert.ToInt32(dataReader["PValue"]);
              byte[] numArray = (byte[]) dataReader["PValueBinary"];
              meterDataList.Add(new MeterData()
              {
                MeterID = Convert.ToInt32(dataReader["MeterID"]),
                TimePoint = Convert.ToDateTime(dataReader["TimePoint"]),
                HardwareTypeID = int32,
                Buffer = numArray
              });
            }
          }
          return meterDataList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        SmokeDetectorDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<MeterData>) null;
      }
    }

    public static Meter GetMeter(uint meterID) => MeterDatabase.GetMeter((int) meterID);

    public static List<Meter> LoadMeter()
    {
      return SmokeDetectorDatabase.LoadMeter(new DateTime?(), new DateTime?());
    }

    public static List<Meter> LoadMeter(DateTime? start, DateTime? end)
    {
      try
      {
        using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
          cmd.Parameters.Clear();
          if (start.HasValue && end.HasValue)
          {
            cmd.CommandText = "SELECT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr FROM Meter as m, MeterInfo as mi WHERE mi.MeterHardwareID = " + 101.ToString() + " AND m.MeterInfoID=mi.MeterInfoID AND m.ProductionDate >= @start AND m.ProductionDate <= @end;";
            MeterDatabase.AddParameter(cmd, "@start", start);
            MeterDatabase.AddParameter(cmd, "@end", end);
          }
          else
            cmd.CommandText = "SELECT * FROM Meter;";
          IDataReader dataReader = cmd.ExecuteReader();
          if (dataReader == null)
            return (List<Meter>) null;
          List<Meter> meterList = new List<Meter>();
          while (dataReader.Read())
          {
            int int32_1 = Convert.ToInt32(dataReader["MeterID"]);
            if (int32_1 != 0)
            {
              int int32_2 = Convert.ToInt32(dataReader["MeterInfoID"]);
              string str = dataReader["SerialNr"].ToString();
              List<MeterData> meterDataList = SmokeDetectorDatabase.LoadMeterData(int32_1, new int?(1));
              if (meterDataList != null && meterDataList.Count != 0)
              {
                DateTime? nullable1 = new DateTime?();
                if (dataReader["ProductionDate"] != DBNull.Value)
                  nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
                DateTime? nullable2 = new DateTime?();
                if (dataReader["ApprovalDate"] != DBNull.Value)
                  nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
                meterList.Add(new Meter()
                {
                  MeterID = int32_1,
                  MeterInfoID = int32_2,
                  SerialNr = str,
                  ProductionDate = nullable1,
                  ApprovalDate = nullable2,
                  OrderNr = dataReader["OrderNr"].ToString()
                });
              }
            }
          }
          return meterList;
        }
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        SmokeDetectorDatabase.logger.ErrorException(str, ex);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        return (List<Meter>) null;
      }
    }

    internal static void CancelSearch() => SmokeDetectorDatabase.isCanceled = true;

    internal static void StartLoadMeterFilterByRadioTransmitIntervalLessAs(
      DateTime? start,
      DateTime? end,
      ushort interval)
    {
      SmokeDetectorDatabase.isCanceled = false;
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
          {
            dbConnection.Open();
            IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
            cmd.Parameters.Clear();
            if (start.HasValue && end.HasValue)
            {
              cmd.CommandText = "SELECT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr FROM Meter as m, MeterInfo as mi WHERE mi.HardwareTypeID <> 194 AND mi.MeterHardwareID = " + 101.ToString() + " AND m.MeterInfoID=mi.MeterInfoID AND m.ProductionDate >= @start AND m.ProductionDate <= @end;";
              MeterDatabase.AddParameter(cmd, "@start", start);
              MeterDatabase.AddParameter(cmd, "@end", end);
            }
            else
              cmd.CommandText = "SELECT * FROM Meter;";
            IDataReader dataReader = cmd.ExecuteReader();
            if (dataReader == null)
              return;
            while (dataReader.Read() && !SmokeDetectorDatabase.isCanceled)
            {
              int int32_1 = Convert.ToInt32(dataReader["MeterID"]);
              if (int32_1 != 0)
              {
                int int32_2 = Convert.ToInt32(dataReader["MeterInfoID"]);
                string str = dataReader["SerialNr"].ToString();
                List<MeterData> meterDataList = SmokeDetectorDatabase.LoadMeterData(int32_1, new int?(1));
                if (meterDataList != null && meterDataList.Count != 0)
                {
                  bool flag = false;
                  foreach (MeterData meterData in meterDataList)
                  {
                    MinoprotectIII minoprotectIii = MinoprotectIII.Unzip(meterData.Buffer);
                    if (minoprotectIii != null && (int) minoprotectIii.Parameter.RadioTransmitInterval < (int) interval)
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (flag)
                  {
                    DateTime? nullable1 = new DateTime?();
                    if (dataReader["ProductionDate"] != DBNull.Value)
                      nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
                    DateTime? nullable2 = new DateTime?();
                    if (dataReader["ApprovalDate"] != DBNull.Value)
                      nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
                    if (SmokeDetectorDatabase.OnMeterFound != null)
                      SmokeDetectorDatabase.OnMeterFound((object) null, new Meter()
                      {
                        MeterID = int32_1,
                        MeterInfoID = int32_2,
                        SerialNr = str,
                        ProductionDate = nullable1,
                        ApprovalDate = nullable2,
                        OrderNr = dataReader["OrderNr"].ToString()
                      });
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          SmokeDetectorDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        }
        if (SmokeDetectorDatabase.OnDone == null)
          return;
        SmokeDetectorDatabase.OnDone((object) null, (EventArgs) null);
      }));
    }

    internal static void StartLoadMeterFilterBySerialnumber(string serialnumber)
    {
      if (string.IsNullOrEmpty(serialnumber))
        throw new ArgumentNullException(nameof (serialnumber));
      SmokeDetectorDatabase.isCanceled = false;
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          using (IDbConnection dbConnection = DbBasis.PrimaryDB.GetDbConnection())
          {
            dbConnection.Open();
            IDbCommand cmd = DbBasis.PrimaryDB.DbCommand(dbConnection);
            cmd.CommandText = "SELECT m.MeterID, m.MeterInfoID, m.SerialNr, m.ProductionDate, m.ApprovalDate, m.OrderNr FROM Meter as m, MeterInfo as mi WHERE mi.HardwareTypeID <> 194 AND mi.MeterHardwareID = " + 101.ToString() + " AND m.MeterInfoID=mi.MeterInfoID AND m.SerialNr = @SerialNr;";
            MeterDatabase.AddParameter(cmd, "@SerialNr", serialnumber);
            IDataReader dataReader = cmd.ExecuteReader();
            if (dataReader == null)
              return;
            while (dataReader.Read() && !SmokeDetectorDatabase.isCanceled)
            {
              int int32_1 = Convert.ToInt32(dataReader["MeterID"]);
              if (int32_1 != 0)
              {
                int int32_2 = Convert.ToInt32(dataReader["MeterInfoID"]);
                string str = dataReader["SerialNr"].ToString();
                DateTime? nullable1 = new DateTime?();
                if (dataReader["ProductionDate"] != DBNull.Value)
                  nullable1 = new DateTime?(Convert.ToDateTime(dataReader["ProductionDate"]));
                DateTime? nullable2 = new DateTime?();
                if (dataReader["ApprovalDate"] != DBNull.Value)
                  nullable2 = new DateTime?(Convert.ToDateTime(dataReader["ApprovalDate"]));
                if (SmokeDetectorDatabase.OnMeterFound != null)
                  SmokeDetectorDatabase.OnMeterFound((object) null, new Meter()
                  {
                    MeterID = int32_1,
                    MeterInfoID = int32_2,
                    SerialNr = str,
                    ProductionDate = nullable1,
                    ApprovalDate = nullable2,
                    OrderNr = dataReader["OrderNr"].ToString()
                  });
              }
            }
          }
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          SmokeDetectorDatabase.logger.ErrorException(str, ex);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.DatabaseError, str);
        }
        if (SmokeDetectorDatabase.OnDone == null)
          return;
        SmokeDetectorDatabase.OnDone((object) null, (EventArgs) null);
      }));
    }
  }
}
