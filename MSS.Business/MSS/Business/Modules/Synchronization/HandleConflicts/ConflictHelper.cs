// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.HandleConflicts.ConflictHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace MSS.Business.Modules.Synchronization.HandleConflicts
{
  public static class ConflictHelper
  {
    public static Dictionary<Type, DataTable> GetConflicts()
    {
      Dictionary<Type, DataTable> dictionary = new Dictionary<Type, DataTable>();
      foreach (ConflictDetails conflictDetails in MSS.Business.Utils.AppContext.Current.SyncConflicts.Values)
      {
        Type databaseMapping = DatabaseConstants.DatabaseMappings[conflictDetails.ConflictInfo.LocalChange.TableName];
        if (dictionary.ContainsKey(databaseMapping))
        {
          DataTable newDataTable = dictionary[databaseMapping];
          newDataTable.Rows.Add(ConflictHelper.GetNewRow(newDataTable, conflictDetails.ConflictInfo.LocalChange.Rows[0], true));
          newDataTable.Rows.Add(ConflictHelper.GetNewRow(newDataTable, conflictDetails.ConflictInfo.RemoteChange.Rows[0], false));
        }
        else
        {
          DataTable dataTable = conflictDetails.ConflictInfo.LocalChange.Clone();
          ConflictHelper.RemoveUnusedColumns(dataTable);
          ConflictHelper.AddCustomColumns(dataTable);
          dataTable.Rows.Add(ConflictHelper.GetNewRow(dataTable, conflictDetails.ConflictInfo.LocalChange.Rows[0], true));
          dataTable.Rows.Add(ConflictHelper.GetNewRow(dataTable, conflictDetails.ConflictInfo.RemoteChange.Rows[0], false));
          dictionary.Add(databaseMapping, dataTable);
        }
      }
      ConflictHelper.AlterDataTables(dictionary);
      return dictionary;
    }

    private static void AlterDataTables(Dictionary<Type, DataTable> dictionary)
    {
      foreach (Type key in dictionary.Keys)
      {
        int[] numArray = new int[3]{ 3, 5, 7 };
        if (key.Name == "StructureNodeLinks")
        {
          DataTable dataTable = dictionary[key];
          dataTable.Columns.Add("Node", typeof (string)).SetOrdinal(numArray[0]);
          dataTable.Columns.Add("ParentNode", typeof (string)).SetOrdinal(numArray[1]);
          dataTable.Columns.Add("RootNode", typeof (string)).SetOrdinal(numArray[2]);
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            for (int index = 0; index < ((IEnumerable<object>) row.ItemArray).Count<object>(); ++index)
            {
              if (row.ItemArray[index] is Guid && MSS.Business.Utils.AppContext.Current.SyncExtraData.ContainsKey((Guid) row.ItemArray[index]))
              {
                string str = MSS.Business.Utils.AppContext.Current.SyncExtraData[Guid.Parse(row.ItemArray[index].ToString())];
                row[index + 1] = (object) str;
              }
            }
          }
        }
      }
    }

    private static void AddCustomColumns(DataTable dataTable)
    {
      dataTable.Columns.Add("IconUrl", typeof (string)).SetOrdinal(0);
    }

    private static void RemoveUnusedColumns(DataTable dataTable)
    {
      dataTable.Columns.Remove("sync_create_peer_key");
      dataTable.Columns.Remove("sync_create_peer_timestamp");
      dataTable.Columns.Remove("sync_update_peer_key");
      dataTable.Columns.Remove("sync_update_peer_timestamp");
    }

    private static DataRow GetNewRow(DataTable newDataTable, DataRow row, bool isSelected)
    {
      DataRow newRow = newDataTable.NewRow();
      newRow[0] = isSelected ? (object) "pack://application:,,,/Styles;component/Images/Universal/selected_conflict.png" : (object) "pack://application:,,,/Styles;component/Images/Universal/unselected_conflict.png";
      for (int columnIndex = 1; columnIndex < ((IEnumerable<object>) newRow.ItemArray).Count<object>(); ++columnIndex)
        newRow[columnIndex] = row.ItemArray[columnIndex - 1];
      return newRow;
    }
  }
}
