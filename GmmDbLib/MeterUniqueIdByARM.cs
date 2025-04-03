// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MeterUniqueIdByARM
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using GmmDbLib.TableManagers;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace GmmDbLib
{
  public class MeterUniqueIdByARM
  {
    private BaseDbConnection MyDb;

    public MeterUniqueIdByARM(BaseDbConnection dbBaseConnection) => this.MyDb = dbBaseConnection;

    public bool ManageMeterID(byte[] UniqueArmID, ref uint? MeterID, bool checkOnly = false)
    {
      uint num = UniqueArmID != null && UniqueArmID.Length == 12 ? BitConverter.ToUInt32(UniqueArmID, 0) : throw new Exception("UniqueArmID not available");
      uint uint32_1 = BitConverter.ToUInt32(UniqueArmID, 4);
      uint uint32_2 = BitConverter.ToUInt32(UniqueArmID, 8);
      using (DbConnection newConnection = this.MyDb.GetNewConnection())
      {
        newConnection.Open();
        DbTransaction transaction = newConnection.BeginTransaction();
        string selectSql = "SELECT * FROM MeterUniqueIdByARM WHERE UniqueIdPart1 = " + num.ToString() + " AND UniqueIdPart2 = " + uint32_1.ToString() + " AND UniqueIdPart3 = " + uint32_2.ToString();
        BaseTables.MeterUniqueIdByARMDataTable idByArmDataTable = new BaseTables.MeterUniqueIdByARMDataTable();
        DbDataAdapter dataAdapter1 = this.MyDb.GetDataAdapter(selectSql, newConnection, transaction, out DbCommandBuilder _);
        dataAdapter1.Fill((DataTable) idByArmDataTable);
        if (idByArmDataTable.Count == 1)
        {
          if (MeterID.HasValue && (int) MeterID.Value != (int) idByArmDataTable[0].MeterID)
            throw new NotSupportedException("Change of MeterID not jet supported");
          MeterID = new uint?(idByArmDataTable[0].MeterID);
          return false;
        }
        if (checkOnly)
        {
          MeterID = new uint?();
          return true;
        }
        BaseTables.MeterDataTable meterDataTable = new BaseTables.MeterDataTable();
        if (MeterID.HasValue)
        {
          this.MyDb.GetDataAdapter("SELECT * FROM Meter WHERE MeterID = " + MeterID.Value.ToString(), newConnection, transaction).Fill((DataTable) meterDataTable);
          if (meterDataTable.Count == 1)
            throw new Exception("MeterID exists in data base but UniqueIdByARM not available! Possible critical database change.");
          throw new NotSupportedException("MeterID defined from device but not in the data base. This change of data base is not supported jet.");
        }
        MeterID = new uint?((uint) this.MyDb.GetNewId("Meter"));
        BaseTables.MeterUniqueIdByARMRow row1 = idByArmDataTable.NewMeterUniqueIdByARMRow();
        row1.UniqueIdPart1 = num;
        row1.UniqueIdPart2 = uint32_1;
        row1.UniqueIdPart3 = uint32_2;
        row1.MeterID = MeterID.Value;
        row1.CreateDate = DateTime.Now.ToUniversalTime();
        idByArmDataTable.AddMeterUniqueIdByARMRow(row1);
        dataAdapter1.Update((DataTable) idByArmDataTable);
        BaseTables.MeterDataTable changedMeterRowsTable = new BaseTables.MeterDataTable();
        BaseTables.MeterRow row2 = changedMeterRowsTable.NewMeterRow();
        row2.ProductionDate = row1.CreateDate;
        row2.MeterID = (int) row1.MeterID;
        changedMeterRowsTable.AddMeterRow(row2);
        DbDataAdapter dataAdapter2 = this.MyDb.GetDataAdapter("SELECT * FROM Meter WHERE 1=0", newConnection, transaction, out DbCommandBuilder _);
        MeterChanges.UpdateMeterRowChanges(this.MyDb, changedMeterRowsTable, dataAdapter2);
        transaction.Commit();
        return true;
      }
    }
  }
}
