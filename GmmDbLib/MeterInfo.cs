// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterInfo
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  [Serializable]
  public sealed class MeterInfo
  {
    public int MeterInfoID { get; private set; }

    public int MeterHardwareID { get; private set; }

    public int MeterTypeID { get; private set; }

    public string PPSArtikelNr { get; private set; }

    public string DefaultFunctionNr { get; private set; }

    public string Description { get; private set; }

    public int HardwareTypeID { get; private set; }

    public MeterInfo DeepCopy() => this.MemberwiseClone() as MeterInfo;

    public static List<MeterInfo> LoadMeterInfo(string hardwareName)
    {
      return MeterInfo.LoadMeterInfo(DbBasis.PrimaryDB.BaseDbConnection, hardwareName);
    }

    public static List<MeterInfo> LoadMeterInfo(BaseDbConnection db, string hardwareName)
    {
      return MeterInfo.LoadMeterInfo(DbBasis.PrimaryDB.BaseDbConnection, new string[1]
      {
        hardwareName
      });
    }

    public static List<MeterInfo> LoadMeterInfo(string[] hardwareName)
    {
      return MeterInfo.LoadMeterInfo(DbBasis.PrimaryDB.BaseDbConnection, hardwareName);
    }

    public static List<MeterInfo> LoadMeterInfo(BaseDbConnection db, string[] hardwareName)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT m.* FROM MeterInfo AS m, HardwareType AS h WHERE h.HardwareTypeID=m.HardwareTypeID AND (";
        for (int index = 0; index < hardwareName.Length; ++index)
        {
          DbCommand dbCommand = command;
          dbCommand.CommandText = dbCommand.CommandText + "h.HardwareName = @HardwareName" + index.ToString();
          DbUtil.AddParameter((IDbCommand) command, "HardwareName" + index.ToString(), hardwareName[index]);
          if (index + 1 < hardwareName.Length)
            command.CommandText += " OR ";
        }
        command.CommandText += ") ORDER BY m.MeterInfoID DESC;";
        List<MeterInfo> meterInfoList = new List<MeterInfo>();
        using (DbDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
            meterInfoList.Add(new MeterInfo()
            {
              MeterInfoID = reader.GetInt32("MeterInfoID"),
              MeterHardwareID = reader.GetInt32("MeterHardwareID"),
              MeterTypeID = reader.GetInt32("MeterTypeID"),
              PPSArtikelNr = reader.GetString("PPSArtikelNr"),
              DefaultFunctionNr = reader.SafeGetString("DefaultFunctionNr"),
              Description = reader.SafeGetString("Description"),
              HardwareTypeID = reader.GetInt32("HardwareTypeID")
            });
        }
        return meterInfoList;
      }
    }

    public static MeterInfo GetMeterInfo(int meterInfoID)
    {
      return MeterInfo.GetMeterInfo(DbBasis.PrimaryDB.BaseDbConnection, meterInfoID);
    }

    public static MeterInfo GetMeterInfo(BaseDbConnection db, int meterInfoID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT * FROM MeterInfo WHERE MeterInfoID=@MeterInfoID;";
        DbUtil.AddParameter((IDbCommand) command, "MeterInfoID", meterInfoID);
        using (DbDataReader reader = command.ExecuteReader())
        {
          if (reader.Read())
            return new MeterInfo()
            {
              MeterInfoID = reader.GetInt32("MeterInfoID"),
              MeterHardwareID = reader.GetInt32("MeterHardwareID"),
              MeterTypeID = reader.GetInt32("MeterTypeID"),
              PPSArtikelNr = reader.GetString("PPSArtikelNr"),
              DefaultFunctionNr = reader.SafeGetString("DefaultFunctionNr"),
              Description = reader.SafeGetString("Description"),
              HardwareTypeID = reader.GetInt32("HardwareTypeID")
            };
        }
        return (MeterInfo) null;
      }
    }

    public static void AddMeterInfo(
      DbCommand cmd,
      int meterInfoId,
      int meterHardwareID,
      int meterTypeId,
      string sapNumber,
      string defaultFunctionNumber,
      string description,
      int hardwareTypeID)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "INSERT INTO MeterInfo (MeterInfoID, MeterHardwareID, MeterTypeID, PPSArtikelNr, DefaultFunctionNr, Description, HardwareTypeID) VALUES (@MeterInfoID, @MeterHardwareID, @MeterTypeID, @PPSArtikelNr, @DefaultFunctionNr, @Description, @HardwareTypeID)";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterInfoID", meterInfoId);
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterHardwareID", meterHardwareID);
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      DbUtil.AddParameter((IDbCommand) cmd, "@PPSArtikelNr", sapNumber);
      DbUtil.AddParameter((IDbCommand) cmd, "@DefaultFunctionNr", defaultFunctionNumber);
      DbUtil.AddParameter((IDbCommand) cmd, "@Description", description);
      DbUtil.AddParameter((IDbCommand) cmd, "@HardwareTypeID", hardwareTypeID);
      cmd.ExecuteNonQuery();
    }

    public static void UpdateMeterInfo(
      DbCommand cmd,
      int meterInfoId,
      string sapNumber,
      string description,
      int hardwareTypeID)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "UPDATE MeterInfo SET PPSArtikelNr=@PPSArtikelNr, Description=@Description, HardwareTypeID=@HardwareTypeID WHERE MeterInfoID=@MeterInfoID";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@PPSArtikelNr", sapNumber);
      DbUtil.AddParameter((IDbCommand) cmd, "@Description", description);
      DbUtil.AddParameter((IDbCommand) cmd, "@HardwareTypeID", hardwareTypeID);
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterInfoID", meterInfoId);
      cmd.ExecuteNonQuery();
    }

    public static void DeleteMeterInfo(DbCommand cmd, int meterInfoId)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "DELETE FROM MeterInfo WHERE MeterInfoID=@MeterInfoID";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterInfoID", meterInfoId);
      cmd.ExecuteNonQuery();
    }
  }
}
