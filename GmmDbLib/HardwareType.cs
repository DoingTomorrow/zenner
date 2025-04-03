// Decompiled with JetBrains decompiler
// Type: GmmDbLib.HardwareType
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
  public class HardwareType
  {
    public int HardwareTypeID { get; private set; }

    public int MapID { get; private set; }

    public uint FirmwareVersion { get; private set; }

    public string HardwareName { get; private set; }

    public int HardwareVersion { get; private set; }

    public string HardwareResource { get; private set; }

    public string Description { get; private set; }

    public override string ToString()
    {
      return "ID: " + this.HardwareTypeID.ToString() + ", " + (string.IsNullOrEmpty(this.Description) ? "" : this.Description);
    }

    public static BaseTables.HardwareTypeRow GetHardwareType(
      BaseDbConnection db,
      int hardwareTypeID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (hardwareTypeID <= 0)
        throw new ArgumentException(nameof (hardwareTypeID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM HardwareType WHERE HardwareTypeID=@HardwareTypeID;", newConnection);
        BaseTables.HardwareTypeDataTable source = new BaseTables.HardwareTypeDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@HardwareTypeID", hardwareTypeID);
        return dataAdapter.Fill((DataTable) source) != 1 ? (BaseTables.HardwareTypeRow) null : source.ToList<BaseTables.HardwareTypeRow>()[0];
      }
    }

    public static BaseTables.HardwareTypeRow GetHardwareType(
      BaseDbConnection db,
      string description,
      int firmwareVersion)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (description == null)
        throw new ArgumentNullException(nameof (description));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT HardwareTypeID, MapID, HardwareName, HardwareVersion, HardwareResource FROM HardwareType WHERE Description LIKE @Description AND FirmwareVersion=@FirmwareVersion;", newConnection);
        BaseTables.HardwareTypeDataTable source = new BaseTables.HardwareTypeDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@Description", description);
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@FirmwareVersion", firmwareVersion);
        int num = dataAdapter.Fill((DataTable) source);
        if (num == 0)
          return (BaseTables.HardwareTypeRow) null;
        if (num > 1)
          throw new Exception("It exists more as one hardware type for firmware = 0x" + firmwareVersion.ToString("X4") + " and description = " + description);
        return source.ToList<BaseTables.HardwareTypeRow>()[0];
      }
    }

    public static List<HardwareType> LoadHardwareType(string hardwareName)
    {
      return HardwareType.LoadHardwareType(DbBasis.PrimaryDB.BaseDbConnection, hardwareName);
    }

    public static List<HardwareType> LoadHardwareType(BaseDbConnection db, string hardwareName)
    {
      return HardwareType.LoadHardwareType(DbBasis.PrimaryDB.BaseDbConnection, new string[1]
      {
        hardwareName
      });
    }

    public static List<HardwareType> LoadHardwareType(string[] hardwareName)
    {
      return HardwareType.LoadHardwareType(DbBasis.PrimaryDB.BaseDbConnection, hardwareName);
    }

    public static List<HardwareType> LoadHardwareType(BaseDbConnection db, string[] hardwareName)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT * FROM HardwareType WHERE (";
        for (int index = 0; index < hardwareName.Length; ++index)
        {
          DbCommand dbCommand = command;
          dbCommand.CommandText = dbCommand.CommandText + "HardwareName = @HardwareName" + index.ToString();
          DbUtil.AddParameter((IDbCommand) command, "HardwareName" + index.ToString(), hardwareName[index]);
          if (index + 1 < hardwareName.Length)
            command.CommandText += " OR ";
        }
        command.CommandText += ") ORDER BY HardwareTypeID DESC;";
        List<HardwareType> hardwareTypeList = new List<HardwareType>();
        using (DbDataReader reader = command.ExecuteReader())
        {
          while (reader.Read())
            hardwareTypeList.Add(new HardwareType()
            {
              HardwareTypeID = reader.GetInt32("HardwareTypeID"),
              MapID = reader.GetInt32("MapID"),
              FirmwareVersion = reader.GetUInt32("FirmwareVersion"),
              HardwareName = reader.GetString("HardwareName"),
              HardwareVersion = reader.GetInt32("HardwareVersion"),
              HardwareResource = reader.SafeGetString("HardwareResource"),
              Description = reader.SafeGetString("Description")
            });
        }
        return hardwareTypeList;
      }
    }
  }
}
