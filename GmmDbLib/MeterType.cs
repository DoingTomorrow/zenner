// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterType
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  [Serializable]
  public sealed class MeterType
  {
    public int MeterTypeID { get; private set; }

    public string MTypeTableName { get; private set; }

    public string Typename { get; private set; }

    public DateTime? GenerateDate { get; private set; }

    public string Description { get; private set; }

    public MeterType DeepCopy() => this.MemberwiseClone() as MeterType;

    public static MeterType GetMeterType(int meterTypeID)
    {
      return MeterType.GetMeterType(DbBasis.PrimaryDB.BaseDbConnection, meterTypeID);
    }

    public static MeterType GetMeterType(BaseDbConnection db, int meterTypeID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT * FROM MeterType WHERE MeterTypeID=@MeterTypeID;";
        DbUtil.AddParameter((IDbCommand) command, "@MeterTypeID", meterTypeID);
        using (DbDataReader reader = command.ExecuteReader())
        {
          if (reader.Read())
            return new MeterType()
            {
              MeterTypeID = reader.GetInt32("MeterTypeID"),
              MTypeTableName = reader.GetString("MTypeTableName"),
              Typename = reader.GetString("Typename"),
              GenerateDate = reader.SafeGetDateTime("GenerateDate"),
              Description = reader.SafeGetString("Description")
            };
        }
        return (MeterType) null;
      }
    }

    public static void AddMeterType(
      DbCommand cmd,
      int meterTypeId,
      string mTypeTableName,
      string hardwareName,
      DateTime generateDate,
      string description)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "INSERT INTO MeterType (MeterTypeID, MTypeTableName, Typename, GenerateDate, Description) VALUES (@MeterTypeID, @MTypeTableName, @Typename, @GenerateDate, @Description)";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      DbUtil.AddParameter((IDbCommand) cmd, "@MTypeTableName", mTypeTableName);
      DbUtil.AddParameter((IDbCommand) cmd, "@Typename", hardwareName);
      DbUtil.AddParameter((IDbCommand) cmd, "@GenerateDate", generateDate);
      DbUtil.AddParameter((IDbCommand) cmd, "@Description", description);
      cmd.ExecuteNonQuery();
    }

    public static void UpdateMeterType(DbCommand cmd, int meterTypeId, DateTime generateDate)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "UPDATE MeterType SET GenerateDate=@GenerateDate WHERE MeterTypeID=@MeterTypeID;";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@GenerateDate", generateDate);
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      cmd.ExecuteNonQuery();
    }

    public static void DeleteMeterType(DbCommand cmd, int meterTypeId)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "DELETE FROM MeterType WHERE MeterTypeID=@MeterTypeID;";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      cmd.ExecuteNonQuery();
    }
  }
}
