// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_FirmwareVersionManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using GmmDbLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_FirmwareVersionManager
  {
    private static List<S4_FirmwareVersionManager.MeterInfoID_Map> MeterInfoID_Replacements;

    static S4_FirmwareVersionManager()
    {
      try
      {
        BaseDbConnection baseDbConnection = DbBasis.PrimaryDB.BaseDbConnection;
        Schema.MeterInfoDataTable meterInfoDataTable = new Schema.MeterInfoDataTable();
        using (DbConnection newConnection = baseDbConnection.GetNewConnection())
        {
          string selectSql = "SELECT * FROM MeterInfo  WHERE MeterInfoID < 10000 AND PPSArtikelNr LIKE 'IUW_%' AND DefaultFunctionNr LIKE 'Firmware%'";
          baseDbConnection.GetDataAdapter(selectSql, newConnection).Fill((DataTable) meterInfoDataTable);
          S4_FirmwareVersionManager.MeterInfoID_Replacements = new List<S4_FirmwareVersionManager.MeterInfoID_Map>();
          foreach (Schema.MeterInfoRow meterInfoRow in (TypedTableBase<Schema.MeterInfoRow>) meterInfoDataTable)
          {
            S4_FirmwareVersionManager.MeterInfoID_Map meterInfoIdMap = new S4_FirmwareVersionManager.MeterInfoID_Map();
            meterInfoIdMap.MeterInfoID = meterInfoRow.MeterInfoID;
            string[] strArray = meterInfoRow.DefaultFunctionNr.Replace("Firmware>=", "").Replace("->MeterInfoID", "").Split('=');
            FirmwareVersion firmwareVersion = new FirmwareVersion(strArray[0]);
            meterInfoIdMap.GreaterEqual_FirmwareVersion = firmwareVersion.Version;
            meterInfoIdMap.ReplaceMeterInfoID = int.Parse(strArray[1]);
            S4_FirmwareVersionManager.MeterInfoID_Replacements.Add(meterInfoIdMap);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("S4_FirmwareVersionManager init error", ex);
      }
    }

    internal static string GetRequiredTypeCreationString(
      uint firmwareVersion,
      string typeCreationString,
      int loopCounter = 0)
    {
      if (typeCreationString == null)
        throw new ArgumentException("typeCreationString not defined");
      ++loopCounter;
      if (loopCounter > 5)
        throw new Exception("GetRequiredTypeCreationString loop detected");
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = typeCreationString;
      char[] chArray = new char[1]{ '|' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append('|');
        string[] strArray = str2.Split('=');
        int oldMeterInfoID = int.Parse(strArray[0]);
        S4_FirmwareVersionManager.MeterInfoID_Map meterInfoIdMap = S4_FirmwareVersionManager.MeterInfoID_Replacements.FirstOrDefault<S4_FirmwareVersionManager.MeterInfoID_Map>((System.Func<S4_FirmwareVersionManager.MeterInfoID_Map, bool>) (x => x.MeterInfoID == oldMeterInfoID && firmwareVersion >= x.GreaterEqual_FirmwareVersion));
        if (meterInfoIdMap == null)
        {
          stringBuilder.Append(str2);
        }
        else
        {
          stringBuilder.Append(meterInfoIdMap.ReplaceMeterInfoID.ToString());
          if (strArray.Length > 1)
            stringBuilder.Append("=" + strArray[1]);
        }
      }
      string typeCreationString1 = stringBuilder.ToString();
      if (typeCreationString1 != typeCreationString)
        typeCreationString1 = S4_FirmwareVersionManager.GetRequiredTypeCreationString(firmwareVersion, typeCreationString1, loopCounter);
      return typeCreationString1;
    }

    internal static int GetRequiredMeterInfoID(
      uint firmwareVersion,
      int currentMeterInfoID,
      int loopCounter = 0)
    {
      ++loopCounter;
      if (loopCounter > 5)
        throw new Exception("GetRequiredMeterInfoID loop detected");
      S4_FirmwareVersionManager.MeterInfoID_Map meterInfoIdMap = S4_FirmwareVersionManager.MeterInfoID_Replacements.FirstOrDefault<S4_FirmwareVersionManager.MeterInfoID_Map>((System.Func<S4_FirmwareVersionManager.MeterInfoID_Map, bool>) (x => x.MeterInfoID == currentMeterInfoID && firmwareVersion >= x.GreaterEqual_FirmwareVersion));
      return meterInfoIdMap == null ? currentMeterInfoID : S4_FirmwareVersionManager.GetRequiredMeterInfoID(firmwareVersion, meterInfoIdMap.ReplaceMeterInfoID, loopCounter);
    }

    internal class MeterInfoID_Map
    {
      internal int MeterInfoID;
      internal int ReplaceMeterInfoID;
      internal uint GreaterEqual_FirmwareVersion;
    }
  }
}
