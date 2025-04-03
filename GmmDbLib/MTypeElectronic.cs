// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MTypeElectronic
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
  public sealed class MTypeElectronic
  {
    public int MeterTypeID { get; private set; }

    public byte[] EEPdata { get; private set; }

    public string TypeCreationString { get; private set; }

    public static MTypeElectronic GetMTypeElectronic(int meterTypeID)
    {
      return MTypeElectronic.GetMTypeElectronic(DbBasis.PrimaryDB.BaseDbConnection, meterTypeID);
    }

    public static MTypeElectronic GetMTypeElectronic(BaseDbConnection db, int meterTypeID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT * FROM MTypeElectronic WHERE MeterTypeID=@MeterTypeID;";
        DbUtil.AddParameter((IDbCommand) command, "@MeterTypeID", meterTypeID);
        using (DbDataReader reader = command.ExecuteReader())
        {
          if (!reader.Read())
            return (MTypeElectronic) null;
          return new MTypeElectronic()
          {
            MeterTypeID = reader.GetInt32("MeterTypeID"),
            EEPdata = reader.GetByteArray("EEPdata"),
            TypeCreationString = reader.SafeGetString("TypeCreationString")
          };
        }
      }
    }

    public static void AddMTypeElectronic(
      DbCommand cmd,
      int meterTypeId,
      byte[] compressedData,
      string typeCreationString)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "INSERT INTO MTypeElectronic (MeterTypeID, EEPdata, TypeCreationString) VALUES (@MeterTypeID, @EEPdata, @TypeCreationString);";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      DbUtil.AddParameter((IDbCommand) cmd, "@EEPdata", compressedData);
      DbUtil.AddParameter((IDbCommand) cmd, "@TypeCreationString", typeCreationString);
      cmd.ExecuteNonQuery();
    }

    public static void UpdateMTypeElectronic(
      DbCommand cmd,
      int meterTypeId,
      byte[] compressedData,
      string typeCreationString)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "UPDATE MTypeElectronic SET EEPdata=@EEPdata, TypeCreationString=@TypeCreationString WHERE MeterTypeID=@MeterTypeID;";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@EEPdata", compressedData);
      DbUtil.AddParameter((IDbCommand) cmd, "@TypeCreationString", typeCreationString);
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      cmd.ExecuteNonQuery();
    }

    public static void DeleteMTypeElectronic(DbCommand cmd, int meterTypeId)
    {
      if (cmd == null)
        throw new ArgumentNullException(nameof (cmd));
      cmd.CommandText = "DELETE FROM MTypeElectronic WHERE MeterTypeID=@MeterTypeID;";
      cmd.Parameters.Clear();
      DbUtil.AddParameter((IDbCommand) cmd, "@MeterTypeID", meterTypeId);
      cmd.ExecuteNonQuery();
    }
  }
}
