// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterValuesMSS
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
  public static class MeterValuesMSS
  {
    public static List<DriverTables.MeterValuesMSSRow> LoadMeterValuesMSS(
      BaseDbConnection db,
      Guid meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MeterValuesMSS WHERE MeterID=@MeterID;", newConnection);
        DriverTables.MeterValuesMSSDataTable source = new DriverTables.MeterValuesMSSDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@MeterID", meterID);
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MeterValuesMSSRow>) null : source.ToList<DriverTables.MeterValuesMSSRow>();
      }
    }

    public static bool DeleteMeterValuesMSS(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MeterValuesMSS;";
        return command.ExecuteNonQuery() > 0;
      }
    }

    public static bool DeleteMeterValuesMSS(BaseDbConnection db, Guid meterID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MeterValuesMSS WHERE MeterID=@MeterID;";
        DbUtil.AddParameter((IDbCommand) command, "@MeterID", meterID);
        return command.ExecuteNonQuery() == 1;
      }
    }
  }
}
