// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSync.Managers.ServerPartialSyncManager
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Business.Errors;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.DTO.Sync;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace MSS.PartialSync.PartialSync.Managers
{
  public class ServerPartialSyncManager : AbstractPartialSyncManager
  {
    private static List<T> ConvertObjectsToList<T>(IList objects)
    {
      if (objects == null || objects.Count == 0)
        return (List<T>) null;
      List<T> list = new List<T>();
      for (int index = 0; index < objects.Count; ++index)
      {
        T obj = (T) objects[index];
        if ((object) obj != null)
          list.Add(obj);
      }
      return list;
    }

    private DataTable ConvertListToStringListDataTable(List<Guid> existingRootNodes)
    {
      DataTable table = new DataTable();
      table.Columns.Add("Item", typeof (string));
      existingRootNodes.ForEach((Action<Guid>) (x => table.Rows.Add((object) x.ToString())));
      return table;
    }

    public SerializedSyncResponse GetServerChangeSet(
      Guid userId,
      DateTime lastSuccessfulDownload,
      List<Guid> existingRootNodes,
      List<Guid> existingOrders,
      bool userMasterPool)
    {
      if (lastSuccessfulDownload == DateTime.MinValue)
        lastSuccessfulDownload = new DateTime(2000, 1, 1);
      SerializedSyncResponse response = new SerializedSyncResponse();
      try
      {
        DataTable stringListDataTable1 = this.ConvertListToStringListDataTable(existingRootNodes);
        DataTable stringListDataTable2 = this.ConvertListToStringListDataTable(existingOrders);
        ISession session = this.RepositoryFactory.GetSession();
        ISQLQuery sqlQuery = session.CreateSQLQuery("exec GetServerChangeSet ? , ? , ?,@existingRootNodes = :data, @existingOrders = :data2");
        sqlQuery.SetParameter<string>(0, userId.ToString());
        sqlQuery.SetParameter<DateTime>(1, lastSuccessfulDownload);
        sqlQuery.SetParameter<bool>(2, userMasterPool);
        sqlQuery.SetStructuredStringList("data", stringListDataTable1);
        sqlQuery.SetStructuredStringList("data2", stringListDataTable2);
        sqlQuery.AddEntity(typeof (Order));
        IMultiQuery multiQuery = session.CreateMultiQuery().Add((IQuery) sqlQuery).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (OrderUser))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (OrderMessage))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (StructureNode))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (StructureNodeLinks))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (StructureNodeEquipmentSettings))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (Note))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (Meter))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MeterRadioDetails))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MeterReplacementHistory))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MbusRadioMeter))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MSS.Core.Model.DataCollectors.Minomat))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MinomatRadioDetails))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MinomatMeter))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (Tenant))).Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (Location)));
        if (userMasterPool)
          multiQuery.Add((IQuery) session.CreateSQLQuery(" ").AddEntity(typeof (MSS.Core.Model.DataCollectors.Minomat)));
        IList list = multiQuery.List();
        response.SerializedOrders = this.Serialize<Order, OrderSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<Order>((IList) list[0]));
        response.SerializedOrderUser = this.Serialize<OrderUser, OrderUserSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<OrderUser>((IList) list[1]));
        response.SerializedOrderMessages = this.Serialize<OrderMessage, OrderMessagesSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<OrderMessage>((IList) list[2]));
        response.SerializedStructureNode = this.Serialize<StructureNode, StructureNodeSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<StructureNode>((IList) list[3]));
        response.SerializedStructureNodeLinks = this.Serialize<StructureNodeLinks, StructureNodeLinksSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<StructureNodeLinks>((IList) list[4]));
        response.SerializedStructureNodeEquipmentSettings = this.Serialize<StructureNodeEquipmentSettings, StructureNodeEquipmentSettingsSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<StructureNodeEquipmentSettings>((IList) list[5]));
        response.SerializedNote = this.Serialize<Note, NoteSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<Note>((IList) list[6]));
        response.SerializedMeter = this.Serialize<Meter, MeterSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<Meter>((IList) list[7]));
        response.SerializedMeterRadioDetails = this.Serialize<MeterRadioDetails, MeterRadioDetailsSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MeterRadioDetails>((IList) list[8]));
        response.SerializedMeterReplacementHistory = this.Serialize<MeterReplacementHistory, MeterReplacementHistorySerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MeterReplacementHistory>((IList) list[9]));
        response.SerializedMeterMBusRadio = this.Serialize<MbusRadioMeter, MeterMBusRadioSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MbusRadioMeter>((IList) list[10]));
        response.SerializedMinomat = this.Serialize<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MSS.Core.Model.DataCollectors.Minomat>((IList) list[11]));
        response.SerializedMinomatRadioDetails = this.Serialize<MinomatRadioDetails, MinomatRadioDetailsSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MinomatRadioDetails>((IList) list[12]));
        response.SerializedMinomatMeters = this.Serialize<MinomatMeter, MinomatMetersSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MinomatMeter>((IList) list[13]));
        response.SerializedTenant = this.Serialize<Tenant, TenantSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<Tenant>((IList) list[14]));
        response.SerializedLocation = this.Serialize<Location, LocationSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<Location>((IList) list[15]));
        if (userMasterPool)
          response.SerializedMinomatPool = this.Serialize<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableSync>(ServerPartialSyncManager.ConvertObjectsToList<MSS.Core.Model.DataCollectors.Minomat>((IList) list[16]));
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return (SerializedSyncResponse) null;
      }
      this.LogChangeset(response);
      return response;
    }

    public SerializedSyncResponse Download(
      Guid userId,
      DateTime lastSuccessfulDownload = default (DateTime),
      List<Guid> existingRootNodes = null,
      List<Guid> existingOrders = null,
      bool userMaterPool = false)
    {
      return this.GetServerChangeSet(userId, lastSuccessfulDownload, existingRootNodes, existingOrders, userMaterPool);
    }

    public bool Upload(SerializedSyncResponse changeset)
    {
      MessageHandler.LogInfo("PARTIAL SYNC: Started upload operation on the server");
      MessageHandler.LogInfo("PARTIAL SYNC: Started to insert changes on the server");
      bool flag = this.InsertChanges(changeset);
      MessageHandler.LogInfo("PARTIAL SYNC: Finished inserting changes on the server");
      if (changeset.SerializedOrders != null)
      {
        MessageHandler.LogInfo("PARTIAL SYNC: Started to unlock closed orders on the server");
        List<Guid> list = changeset.SerializedOrders.Where<OrderSerializableSync>((System.Func<OrderSerializableSync, bool>) (_ => _.Status == StatusOrderEnum.Closed)).Select<OrderSerializableSync, Guid>((System.Func<OrderSerializableSync, Guid>) (_ => _.Id)).ToList<Guid>();
        if (list.Any<Guid>())
          this.SetLockByForOrders(list);
        MessageHandler.LogInfo("PARTIAL SYNC: Finished to unlock closed orders on the server");
      }
      else
        MessageHandler.LogInfo("PARTIAL SYNC: No orders to unlock");
      return flag;
    }
  }
}
