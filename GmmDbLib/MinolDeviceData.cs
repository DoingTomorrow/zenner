// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MinolDeviceData
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Collections.Generic;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public static class MinolDeviceData
  {
    public static List<int> LoadMapIDOfMinolDeviceData(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT DISTINCT MapID FROM MinolDeviceData;";
        List<int> intList = new List<int>();
        using (DbDataReader dbDataReader = command.ExecuteReader())
        {
          while (dbDataReader.Read())
            intList.Add(Convert.ToInt32(dbDataReader["MapID"]));
        }
        return intList;
      }
    }
  }
}
