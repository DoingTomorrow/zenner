// Decompiled with JetBrains decompiler
// Type: S3_Handler.MapSelector
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib.MapManagement;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  internal class MapSelector
  {
    internal static Logger S3_MapSelectorLogger = LogManager.GetLogger("S3_MapSelector");

    internal static List<KeyValuePair<string, int>> GetMap(
      uint FirmwareVersion,
      uint MapId,
      S3_DatabaseAccess databaseAccess)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      List<KeyValuePair<string, int>> map = new List<KeyValuePair<string, int>>();
      byte[] byteArray = (byte[]) null;
      string str = "MapDefClass";
      Type[] types = Assembly.GetExecutingAssembly().GetTypes();
      Type type1 = (Type) null;
      uint num = 0;
      foreach (Type type2 in types)
      {
        if (type2.Name.StartsWith(str))
        {
          string s = type2.Name.Substring(str.Length);
          if (s.Length == 8)
          {
            uint result;
            if (uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
            {
              if ((int) result == (int) FirmwareVersion)
              {
                byteArray = ((MapDefClassBase) Activator.CreateInstance(type2)).FullByteList;
                MapSelector.S3_MapSelectorLogger.Debug("Map bytes loaded: " + type2.Name);
                break;
              }
              if (((int) result & -61441) == ((int) FirmwareVersion & -61441) && (type1 == (Type) null || num < result))
              {
                num = result;
                type1 = type2;
              }
            }
          }
          else
          {
            int result;
            if (s.Length == 2 && int.TryParse(s, out result) && (long) result == (long) MapId)
            {
              byteArray = ((MapDefClassBase) Activator.CreateInstance(type2)).FullByteList;
              break;
            }
          }
        }
      }
      if (byteArray != null || type1 != (Type) null)
      {
        if (byteArray == null)
          byteArray = ((MapDefClassBase) Activator.CreateInstance(type1)).FullByteList;
        int readIndex = 0;
        if (LoadDataFromMapClass.GetByteForced(byteArray, ref readIndex) > (byte) 0)
          throw new FormatException("Illegal MapDef class format");
        string key = "";
        while (readIndex < byteArray.Length)
        {
          try
          {
            key = LoadDataFromMapClass.GetStringForced(byteArray, ref readIndex);
            int intForced = LoadDataFromMapClass.GetIntForced(byteArray, ref readIndex);
            map.Add(new KeyValuePair<string, int>(key, intForced));
          }
          catch (Exception ex)
          {
            throw new FormatException("Illegal parameter config class. Error near parameter:" + key + Environment.NewLine + ex.Message);
          }
        }
      }
      else
      {
        Schema.MapDefDataTable Table = new Schema.MapDefDataTable();
        databaseAccess.GetDataTableBySQLCommand("SELECT * FROM MapDef WHERE MapID = " + MapId.ToString(), (DataTable) Table);
        if (Table.Count == 0)
          throw new ArgumentException("Unknown MapID: " + MapId.ToString());
        for (int index = 0; index < Table.Count; ++index)
          map.Add(new KeyValuePair<string, int>(Table[index].ConstName, Table[index].cValue));
      }
      stopwatch.Stop();
      MapSelector.S3_MapSelectorLogger.Debug("Map loaded. Time[ms]: " + stopwatch.ElapsedMilliseconds.ToString());
      return map;
    }
  }
}
