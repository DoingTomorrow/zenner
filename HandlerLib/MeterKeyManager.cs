// Decompiled with JetBrains decompiler
// Type: HandlerLib.MeterKeyManager
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public static class MeterKeyManager
  {
    public static uint GetMeterKey(BaseDbConnection db, uint MeterID)
    {
      using (DbConnection newConnection = db.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM MeterData");
        stringBuilder.Append(" WHERE MeterID = " + MeterID.ToString());
        stringBuilder.Append(" AND PValueId = " + 217.ToString());
        DbDataAdapter dataAdapter = db.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
        BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
        dataAdapter.Fill((DataTable) meterDataDataTable);
        if (meterDataDataTable.Count > 1)
          throw new Exception("More then one MeterKey available for MeterID:" + MeterID.ToString());
        if (meterDataDataTable.Count == 0)
          throw new Exception("MeterKey not available for MeterID:" + MeterID.ToString());
        if (meterDataDataTable[0].IsPValueNull())
          throw new Exception("MeterKey null found for MeterID:" + MeterID.ToString());
        uint result;
        if (!uint.TryParse(meterDataDataTable[0].PValue, out result))
          throw new Exception("MeterKey format error for MeterID:" + MeterID.ToString());
        if (result < (uint) byte.MaxValue || result > 4294967040U)
          throw new Exception("MeterKey value error for MeterID:" + MeterID.ToString());
        return result ^ MeterID;
      }
    }

    public static uint GetOrCreateMeterKey(BaseDbConnection db, uint MeterID)
    {
      if (MeterID == 0U)
        throw new Exception("MeterID == 0 not allowed for protection.");
      using (DbConnection newConnection = db.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM MeterData");
        stringBuilder.Append(" WHERE MeterID = " + MeterID.ToString());
        stringBuilder.Append(" AND PValueId = " + 217.ToString());
        DbDataAdapter dataAdapter = db.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
        BaseTables.MeterDataDataTable meterDataDataTable = new BaseTables.MeterDataDataTable();
        dataAdapter.Fill((DataTable) meterDataDataTable);
        if (meterDataDataTable.Count > 1)
          throw new Exception("More than one MeterKey available for MeterID:" + MeterID.ToString());
        uint result;
        if (meterDataDataTable.Count == 1)
        {
          if (meterDataDataTable[0].IsPValueNull())
            throw new Exception("MeterKey null found for MeterID:" + MeterID.ToString());
          if (!uint.TryParse(meterDataDataTable[0].PValue, out result))
            throw new Exception("MeterKey format error for MeterID:" + MeterID.ToString());
          if (result < (uint) byte.MaxValue || result > 4294967040U)
            throw new Exception("MeterKey value error for MeterID:" + MeterID.ToString());
        }
        else
        {
          Random random = new Random(DateTime.Now.Millisecond);
          uint num;
          do
          {
            num = (uint) random.Next();
          }
          while (((int) num & 2130706432) == 0 || ((int) num & (int) byte.MaxValue) == 0);
          result = num & (uint) int.MaxValue;
          BaseTables.MeterDataRow row = meterDataDataTable.NewMeterDataRow();
          row.MeterID = (int) MeterID;
          row.PValueID = 217;
          row.TimePoint = DateTime.Now.ToUniversalTime();
          row.PValue = result.ToString();
          meterDataDataTable.AddMeterDataRow(row);
          dataAdapter.Update((DataTable) meterDataDataTable);
        }
        return result ^ MeterID;
      }
    }
  }
}
