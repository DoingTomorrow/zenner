// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSync.Managers.AbstractPartialSyncManager
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using AutoMapper;
using MSS.Business.Errors;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.Data.Repository;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.DTO.Sync;
using MSS.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

#nullable disable
namespace MSS.PartialSync.PartialSync.Managers
{
  public abstract class AbstractPartialSyncManager
  {
    protected const string PARTIAL_SYNC_LOG = "PARTIAL SYNC: ";
    private IRepositoryFactory _repositoryFactory;
    private IRepositoryFactory _partialSyncRepositoryFactory;

    protected IRepositoryFactory RepositoryFactory
    {
      get
      {
        return this._repositoryFactory ?? (this._repositoryFactory = new RepositoryFactoryCreator().CreateNewRepositoryFactory());
      }
    }

    protected IRepositoryFactory PartialSyncRepositoryFactory
    {
      get
      {
        return this._partialSyncRepositoryFactory ?? (this._partialSyncRepositoryFactory = new RepositoryFactoryCreator().CreateNewPartialSyncRepositoryFactory());
      }
    }

    protected List<TD> Deserialize<TS, TD>(List<TS> values)
    {
      if (values == null || values.Count == 0)
        return (List<TD>) null;
      List<TD> entity = new List<TD>();
      Mapper.CreateMap<TS, TD>();
      values.ForEach((Action<TS>) (x => entity.Add(Mapper.Map<TS, TD>(x))));
      return entity;
    }

    protected List<TD> Serialize<TS, TD>(List<TS> values)
    {
      if (values == null || values.Count == 0)
        return (List<TD>) null;
      List<TD> serializedEntity = new List<TD>();
      Mapper.CreateMap<TS, TD>();
      values.ForEach((Action<TS>) (x => serializedEntity.Add(Mapper.Map<TS, TD>(x))));
      return serializedEntity;
    }

    internal void LogChangeset(SerializedSyncResponse response)
    {
      if (response.SerializedOrders != null)
        MessageHandler.LogInfo("PARTIAL SYNC: The following order were identified:" + string.Join<Guid>(",", response.SerializedOrders.Select<OrderSerializableSync, Guid>((Func<OrderSerializableSync, Guid>) (_ => _.Id))));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("PARTIAL SYNC: Selected items: ");
      if (response.SerializedOrders != null)
        stringBuilder.Append("No. of Orders = " + (object) response.SerializedOrders.Count + ", ");
      else
        stringBuilder.Append("No. of Orders = 0, ");
      if (response.SerializedOrderUser != null)
        stringBuilder.Append("No. of OrdersUser = " + (object) response.SerializedOrderUser.Count + ", ");
      else
        stringBuilder.Append("No. of OrdersUser = 0, ");
      if (response.SerializedOrderMessages != null)
        stringBuilder.Append("No. of OrderMessages = " + (object) response.SerializedOrderMessages.Count + ", ");
      else
        stringBuilder.Append("No. of OrderMessages = 0, ");
      if (response.SerializedStructureNode != null)
        stringBuilder.Append("No. of StructureNode = " + (object) response.SerializedStructureNode.Count + ", ");
      else
        stringBuilder.Append("No. of StructureNode = 0, ");
      if (response.SerializedStructureNodeLinks != null)
        stringBuilder.Append("No. of StructureNodeLinks = " + (object) response.SerializedStructureNodeLinks.Count + ", ");
      else
        stringBuilder.Append("No. of StructureNodeLinks = 0, ");
      if (response.SerializedStructureNodeEquipmentSettings != null)
        stringBuilder.Append("No. of StructureNodeEquipmentSettings = " + (object) response.SerializedStructureNodeEquipmentSettings.Count + ", ");
      else
        stringBuilder.Append("No. of StructureNodeEquipmentSettings = 0, ");
      if (response.SerializedMeter != null)
        stringBuilder.Append("No. of Meter = " + (object) response.SerializedMeter.Count + ", ");
      else
        stringBuilder.Append("No. of Meter = 0, ");
      if (response.SerializedMeterRadioDetails != null)
        stringBuilder.Append("No. of MeterRadioDetails = " + (object) response.SerializedMeterRadioDetails.Count + ", ");
      else
        stringBuilder.Append("No. of MeterRadioDetails = 0, ");
      if (response.SerializedMeterReplacementHistory != null)
        stringBuilder.Append("No. of MeterReplacementHistory = " + (object) response.SerializedMeterReplacementHistory.Count + ", ");
      else
        stringBuilder.Append("No. of MeterReplacementHistory = 0, ");
      if (response.SerializedMeterMBusRadio != null)
        stringBuilder.Append("No. of MeterMBusRadio = " + (object) response.SerializedMeterMBusRadio.Count + ", ");
      else
        stringBuilder.Append("No. of MeterMBusRadio = 0, ");
      if (response.SerializedNote != null)
        stringBuilder.Append("No. of Note = " + (object) response.SerializedNote.Count + ", ");
      else
        stringBuilder.Append("No. of Note = 0, ");
      if (response.SerializedMinomat != null)
        stringBuilder.Append("No. of Minomat = " + (object) response.SerializedMinomat.Count + ", ");
      else
        stringBuilder.Append("No. of Minomat = 0, ");
      if (response.SerializedMinomatRadioDetails != null)
        stringBuilder.Append("No. of MinomatRadioDetails = " + (object) response.SerializedMinomatRadioDetails.Count + ", ");
      else
        stringBuilder.Append("No. of MinomatRadioDetails = 0, ");
      if (response.SerializedMinomatMeters != null)
        stringBuilder.Append("No. of MinomatMeters = " + (object) response.SerializedMinomatMeters.Count + ", ");
      else
        stringBuilder.Append("No. of MinomatMeters = 0, ");
      if (response.SerializedTenant != null)
        stringBuilder.Append("No. of Tenant = " + (object) response.SerializedTenant.Count + ", ");
      else
        stringBuilder.Append("No. of Tenant = 0, ");
      if (response.SerializedLocation != null)
        stringBuilder.Append("No. of Location = " + (object) response.SerializedLocation.Count + ", ");
      else
        stringBuilder.Append("No. of Location = 0, ");
      if (response.SerializedMinomatPool != null)
        stringBuilder.Append("No. of Minomats in MinomatPool = " + (object) response.SerializedMinomatPool.Count + ", ");
      else
        stringBuilder.Append("No. of Minomats in MinomatPool = 0 ");
      MessageHandler.LogInfo(stringBuilder.ToString());
    }

    public bool InsertChanges(SerializedSyncResponse changes)
    {
      try
      {
        this.PartialSyncRepositoryFactory.GetSession().BeginTransaction();
        List<StructureNodeType> nodeTypes = this.PartialSyncRepositoryFactory.GetRepository<StructureNodeType>().GetAll().ToList<StructureNodeType>();
        List<RoomType> roomTypes = this.PartialSyncRepositoryFactory.GetRepository<RoomType>().GetAll().ToList<RoomType>();
        List<MeasureUnit> measureUnits = this.PartialSyncRepositoryFactory.GetRepository<MeasureUnit>().GetAll().ToList<MeasureUnit>();
        List<Channel> channels = this.PartialSyncRepositoryFactory.GetRepository<Channel>().GetAll().ToList<Channel>();
        List<ConnectedDeviceType> connectedDeviceTypes = this.PartialSyncRepositoryFactory.GetRepository<ConnectedDeviceType>().GetAll().ToList<ConnectedDeviceType>();
        List<Provider> providerList = this.PartialSyncRepositoryFactory.GetRepository<Provider>().GetAll().ToList<Provider>();
        List<Scenario> scenarioList = this.PartialSyncRepositoryFactory.GetRepository<Scenario>().GetAll().ToList<Scenario>();
        List<Country> countryList = this.PartialSyncRepositoryFactory.GetRepository<Country>().GetAll().ToList<Country>();
        List<Order> orderList1 = this.Deserialize<OrderSerializableSync, Order>(changes.SerializedOrders);
        if (orderList1 != null)
        {
          Guid?[] filterGuids = changes.SerializedOrders.Where<OrderSerializableSync>((Func<OrderSerializableSync, bool>) (x => x.FilterId.HasValue)).Select<OrderSerializableSync, Guid?>((Func<OrderSerializableSync, Guid?>) (x => x.FilterId)).Distinct<Guid?>().ToArray<Guid?>();
          List<MSS.Core.Model.DataFilters.Filter> filters = this.PartialSyncRepositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().SearchForInMemoryOrDb((Expression<Func<MSS.Core.Model.DataFilters.Filter, bool>>) (x => filterGuids.Contains<Guid?>((Guid?) x.Id)), (Func<MSS.Core.Model.DataFilters.Filter, bool>) (x => ((IEnumerable<Guid?>) filterGuids).Contains<Guid?>(new Guid?(x.Id)))).ToList<MSS.Core.Model.DataFilters.Filter>();
          Dictionary<Guid, Order> OrderDictionary = new Dictionary<Guid, Order>();
          orderList1.ForEach((Action<Order>) (x =>
          {
            OrderSerializableSync currentLink = changes.SerializedOrders.FirstOrDefault<OrderSerializableSync>((Func<OrderSerializableSync, bool>) (y => y.Id == x.Id));
            if (currentLink == null)
              return;
            if (filters.Count > 0 && currentLink.FilterId.HasValue)
              x.Filter = filters.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (y =>
              {
                Guid id = y.Id;
                Guid? filterId = currentLink.FilterId;
                return filterId.HasValue && id == filterId.GetValueOrDefault();
              }));
            OrderDictionary.Add(x.Id, x);
          }));
          Guid[] orderGuids = OrderDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<Order>().TransactionalInsertOrUpdateList(OrderDictionary, (Expression<Func<Order, bool>>) (y => orderGuids.Contains<Guid>(y.Id)), (Func<Order, bool>) (y => ((IEnumerable<Guid>) orderGuids).Contains<Guid>(y.Id)));
        }
        List<OrderUser> orderUserList = this.Deserialize<OrderUserSerializableSync, OrderUser>(changes.SerializedOrderUser);
        if (orderUserList != null)
        {
          OrderUserSerializableSync orderUser = changes.SerializedOrderUser.FirstOrDefault<OrderUserSerializableSync>();
          User userFromOrder = this.PartialSyncRepositoryFactory.GetRepository<User>().FirstOrDefaultCacheSearch((Expression<Func<User, bool>>) (y => y.Id == orderUser.UserId), (Func<User, bool>) (y => y.Id == orderUser.UserId));
          Guid[] orderGuids = changes.SerializedOrderUser.Select<OrderUserSerializableSync, Guid>((Func<OrderUserSerializableSync, Guid>) (x => x.OrderId)).Distinct<Guid>().ToArray<Guid>();
          List<Order> orders = this.PartialSyncRepositoryFactory.GetRepository<Order>().SearchForInMemoryOrDb((Expression<Func<Order, bool>>) (x => orderGuids.Contains<Guid>(x.Id)), (Func<Order, bool>) (x => ((IEnumerable<Guid>) orderGuids).Contains<Guid>(x.Id))).ToList<Order>();
          Dictionary<Guid, OrderUser> OrderUserDictionary = new Dictionary<Guid, OrderUser>();
          orderUserList.ForEach((Action<OrderUser>) (x =>
          {
            OrderUserSerializableSync currentLink = changes.SerializedOrderUser.FirstOrDefault<OrderUserSerializableSync>((Func<OrderUserSerializableSync, bool>) (y => y.Id == x.Id));
            if (currentLink == null)
              return;
            x.User = userFromOrder;
            x.Order = orders.FirstOrDefault<Order>((Func<Order, bool>) (y => y.Id == currentLink.OrderId));
            OrderUserDictionary.Add(x.Id, x);
          }));
          Guid[] orderUserGuids = OrderUserDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<OrderUser>().TransactionalInsertOrUpdateList(OrderUserDictionary, (Expression<Func<OrderUser, bool>>) (y => orderUserGuids.Contains<Guid>(y.Id)), (Func<OrderUser, bool>) (y => ((IEnumerable<Guid>) orderUserGuids).Contains<Guid>(y.Id)));
        }
        List<StructureNode> structureNodeList1 = this.Deserialize<StructureNodeSerializableSync, StructureNode>(changes.SerializedStructureNode);
        if (structureNodeList1 != null)
        {
          Dictionary<Guid, StructureNode> strunctureNodeDictionary = new Dictionary<Guid, StructureNode>();
          structureNodeList1.ForEach((Action<StructureNode>) (x =>
          {
            StructureNodeSerializableSync currentLink = changes.SerializedStructureNode.FirstOrDefault<StructureNodeSerializableSync>((Func<StructureNodeSerializableSync, bool>) (y => y.Id == x.Id));
            if (currentLink == null)
              return;
            if (nodeTypes.Count > 0)
              x.NodeType = nodeTypes.FirstOrDefault<StructureNodeType>((Func<StructureNodeType, bool>) (y => y.Id == currentLink.NodeTypeId));
            strunctureNodeDictionary.Add(x.Id, x);
          }));
          Guid[] strunctureNodeGuids = strunctureNodeDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<StructureNode>().TransactionalInsertOrUpdateList(strunctureNodeDictionary, (Expression<Func<StructureNode, bool>>) (y => strunctureNodeGuids.Contains<Guid>(y.Id)), (Func<StructureNode, bool>) (y => ((IEnumerable<Guid>) strunctureNodeGuids).Contains<Guid>(y.Id)));
        }
        List<StructureNodeLinks> structureNodeLinksList = this.Deserialize<StructureNodeLinksSerializableSync, StructureNodeLinks>(changes.SerializedStructureNodeLinks);
        if (structureNodeLinksList != null)
        {
          Guid[] structureNodesGuidArray = changes.SerializedStructureNodeLinks.Select<StructureNodeLinksSerializableSync, Guid>((Func<StructureNodeLinksSerializableSync, Guid>) (x => x.NodeId)).Distinct<Guid>().Union<Guid>(changes.SerializedStructureNodeLinks.Select<StructureNodeLinksSerializableSync, Guid>((Func<StructureNodeLinksSerializableSync, Guid>) (x => x.RootNodeId)).Distinct<Guid>()).ToArray<Guid>();
          List<StructureNode> structureNodeList = this.PartialSyncRepositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (x => structureNodesGuidArray.Contains<Guid>(x.Id)), (Func<StructureNode, bool>) (x => ((IEnumerable<Guid>) structureNodesGuidArray).Contains<Guid>(x.Id))).ToList<StructureNode>();
          Dictionary<Guid, StructureNodeLinks> strunctureNodeLinkDictionary = new Dictionary<Guid, StructureNodeLinks>();
          structureNodeLinksList.ForEach((Action<StructureNodeLinks>) (x =>
          {
            StructureNodeLinksSerializableSync currentLink = changes.SerializedStructureNodeLinks.FirstOrDefault<StructureNodeLinksSerializableSync>((Func<StructureNodeLinksSerializableSync, bool>) (y => y.Id == x.Id));
            if (currentLink == null)
              return;
            x.Node = structureNodeList.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (y => y.Id == currentLink.NodeId));
            x.RootNode = structureNodeList.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (y => y.Id == currentLink.RootNodeId));
            strunctureNodeLinkDictionary.Add(x.Id, x);
          }));
          Guid[] strunctureNodeLinkGuids = strunctureNodeLinkDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<StructureNodeLinks>().TransactionalInsertOrUpdateList(strunctureNodeLinkDictionary, (Expression<Func<StructureNodeLinks, bool>>) (y => strunctureNodeLinkGuids.Contains<Guid>(y.Id)), (Func<StructureNodeLinks, bool>) (y => ((IEnumerable<Guid>) strunctureNodeLinkGuids).Contains<Guid>(y.Id)));
        }
        List<StructureNodeEquipmentSettings> equipmentSettingsList = this.Deserialize<StructureNodeEquipmentSettingsSerializableSync, StructureNodeEquipmentSettings>(changes.SerializedStructureNodeEquipmentSettings);
        if (equipmentSettingsList != null)
        {
          Guid[] structureNodesGuids = changes.SerializedStructureNodeEquipmentSettings.Select<StructureNodeEquipmentSettingsSerializableSync, Guid>((Func<StructureNodeEquipmentSettingsSerializableSync, Guid>) (x => x.StructureNodeId)).Distinct<Guid>().ToArray<Guid>();
          List<StructureNode> structureNodeList = this.PartialSyncRepositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (x => structureNodesGuids.Contains<Guid>(x.Id)), (Func<StructureNode, bool>) (x => ((IEnumerable<Guid>) structureNodesGuids).Contains<Guid>(x.Id))).ToList<StructureNode>();
          Dictionary<Guid, StructureNodeEquipmentSettings> strunctureNodeEquipmentDictionary = new Dictionary<Guid, StructureNodeEquipmentSettings>();
          equipmentSettingsList.ForEach((Action<StructureNodeEquipmentSettings>) (x =>
          {
            StructureNodeEquipmentSettingsSerializableSync currentLink = changes.SerializedStructureNodeEquipmentSettings.FirstOrDefault<StructureNodeEquipmentSettingsSerializableSync>((Func<StructureNodeEquipmentSettingsSerializableSync, bool>) (y => y.Id == x.Id));
            if (currentLink == null)
              return;
            x.StructureNode = structureNodeList.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (y => y.Id == currentLink.StructureNodeId));
            strunctureNodeEquipmentDictionary.Add(x.Id, x);
          }));
          Guid[] strunctureNodeEquipmentGuids = strunctureNodeEquipmentDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<StructureNodeEquipmentSettings>().TransactionalInsertOrUpdateList(strunctureNodeEquipmentDictionary, (Expression<Func<StructureNodeEquipmentSettings, bool>>) (y => strunctureNodeEquipmentGuids.Contains<Guid>(y.Id)), (Func<StructureNodeEquipmentSettings, bool>) (y => ((IEnumerable<Guid>) strunctureNodeEquipmentGuids).Contains<Guid>(y.Id)));
        }
        List<Note> noteList = this.Deserialize<NoteSerializableSync, Note>(changes.SerializedNote);
        if (noteList != null)
        {
          Guid?[] structureNodesGuids = changes.SerializedNote.Select<NoteSerializableSync, Guid?>((Func<NoteSerializableSync, Guid?>) (x => x.StructureNodeId)).Distinct<Guid?>().ToArray<Guid?>();
          List<StructureNode> structureNodeList = this.PartialSyncRepositoryFactory.GetRepository<StructureNode>().SearchForInMemoryOrDb((Expression<Func<StructureNode, bool>>) (x => structureNodesGuids.Contains<Guid?>((Guid?) x.Id)), (Func<StructureNode, bool>) (x => ((IEnumerable<Guid?>) structureNodesGuids).Contains<Guid?>(new Guid?(x.Id)))).ToList<StructureNode>();
          Guid?[] noteTypeGuids = changes.SerializedNote.Select<NoteSerializableSync, Guid?>((Func<NoteSerializableSync, Guid?>) (x => x.NoteTypeId)).Distinct<Guid?>().ToArray<Guid?>();
          List<NoteType> noteTypeList = this.PartialSyncRepositoryFactory.GetRepository<NoteType>().SearchForInMemoryOrDb((Expression<Func<NoteType, bool>>) (x => noteTypeGuids.Contains<Guid?>((Guid?) x.Id)), (Func<NoteType, bool>) (x => ((IEnumerable<Guid?>) noteTypeGuids).Contains<Guid?>(new Guid?(x.Id)))).ToList<NoteType>();
          Dictionary<Guid, Note> noteDictionary = new Dictionary<Guid, Note>();
          noteList.ForEach((Action<Note>) (x =>
          {
            NoteSerializableSync current = changes.SerializedNote.FirstOrDefault<NoteSerializableSync>((Func<NoteSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.NoteType = noteTypeList.FirstOrDefault<NoteType>((Func<NoteType, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? noteTypeId = current.NoteTypeId;
              return noteTypeId.HasValue && id == noteTypeId.GetValueOrDefault();
            }));
            x.StructureNode = structureNodeList.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? structureNodeId = current.StructureNodeId;
              return structureNodeId.HasValue && id == structureNodeId.GetValueOrDefault();
            }));
            noteDictionary.Add(x.Id, x);
          }));
          Guid[] noteGuids = noteDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<Note>().TransactionalInsertOrUpdateList(noteDictionary, (Expression<Func<Note, bool>>) (y => noteGuids.Contains<Guid>(y.Id)), (Func<Note, bool>) (y => ((IEnumerable<Guid>) noteGuids).Contains<Guid>(y.Id)));
        }
        List<Meter> meterList1 = this.Deserialize<MeterSerializableSync, Meter>(changes.SerializedMeter);
        Dictionary<Guid, Meter> meterDictionary = new Dictionary<Guid, Meter>();
        meterList1?.ForEach((Action<Meter>) (x =>
        {
          MeterSerializableSync current = changes.SerializedMeter.FirstOrDefault<MeterSerializableSync>((Func<MeterSerializableSync, bool>) (y => y.Id == x.Id));
          if (current == null)
            return;
          x.Room = roomTypes.FirstOrDefault<RoomType>((Func<RoomType, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? roomId = current.RoomId;
            return roomId.HasValue && id == roomId.GetValueOrDefault();
          }));
          x.ReadingUnit = measureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? readingUnitId = current.ReadingUnitId;
            return readingUnitId.HasValue && id == readingUnitId.GetValueOrDefault();
          }));
          x.ImpulsUnit = measureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? impulsUnitId = current.ImpulsUnitId;
            return impulsUnitId.HasValue && id == impulsUnitId.GetValueOrDefault();
          }));
          x.Channel = channels.FirstOrDefault<Channel>((Func<Channel, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? channelId = current.ChannelId;
            return channelId.HasValue && id == channelId.GetValueOrDefault();
          }));
          x.ConnectedDeviceType = connectedDeviceTypes.FirstOrDefault<ConnectedDeviceType>((Func<ConnectedDeviceType, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? connectedDeviceTypeId = current.ConnectedDeviceTypeId;
            return connectedDeviceTypeId.HasValue && id == connectedDeviceTypeId.GetValueOrDefault();
          }));
          meterDictionary.Add(x.Id, x);
        }));
        if (meterDictionary.Any<KeyValuePair<Guid, Meter>>())
        {
          Guid[] metersToInsertGuids = meterDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<Meter>().TransactionalInsertOrUpdateList(meterDictionary, (Expression<Func<Meter, bool>>) (y => metersToInsertGuids.Contains<Guid>(y.Id)), (Func<Meter, bool>) (y => ((IEnumerable<Guid>) metersToInsertGuids).Contains<Guid>(y.Id)));
        }
        List<MeterRadioDetails> meterRadioDetailsList = this.Deserialize<MeterRadioDetailsSerializableSync, MeterRadioDetails>(changes.SerializedMeterRadioDetails);
        if (meterRadioDetailsList != null)
        {
          Guid?[] meterGuids = changes.SerializedMeterRadioDetails.Select<MeterRadioDetailsSerializableSync, Guid?>((Func<MeterRadioDetailsSerializableSync, Guid?>) (x => x.MeterId)).Distinct<Guid?>().ToArray<Guid?>();
          List<Meter> meterList = this.PartialSyncRepositoryFactory.GetRepository<Meter>().SearchForInMemoryOrDb((Expression<Func<Meter, bool>>) (x => meterGuids.Contains<Guid?>((Guid?) x.Id)), (Func<Meter, bool>) (x => ((IEnumerable<Guid?>) meterGuids).Contains<Guid?>(new Guid?(x.Id)))).ToList<Meter>();
          Dictionary<Guid, MeterRadioDetails> meterRadioDetailsDictionary = new Dictionary<Guid, MeterRadioDetails>();
          meterRadioDetailsList.ForEach((Action<MeterRadioDetails>) (x =>
          {
            MeterRadioDetailsSerializableSync current = changes.SerializedMeterRadioDetails.FirstOrDefault<MeterRadioDetailsSerializableSync>((Func<MeterRadioDetailsSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.Meter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? meterId = current.MeterId;
              return meterId.HasValue && id == meterId.GetValueOrDefault();
            }));
            meterRadioDetailsDictionary.Add(x.Id, x);
          }));
          Guid[] metersRadioDetailsGuids = meterRadioDetailsDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MeterRadioDetails>().TransactionalInsertOrUpdateList(meterRadioDetailsDictionary, (Expression<Func<MeterRadioDetails, bool>>) (y => metersRadioDetailsGuids.Contains<Guid>(y.Id)), (Func<MeterRadioDetails, bool>) (y => ((IEnumerable<Guid>) metersRadioDetailsGuids).Contains<Guid>(y.Id)));
        }
        List<MeterReplacementHistory> replacementHistoryList = this.Deserialize<MeterReplacementHistorySerializableSync, MeterReplacementHistory>(changes.SerializedMeterReplacementHistory);
        if (replacementHistoryList != null)
        {
          Guid?[] meterGuidsArray = changes.SerializedMeterReplacementHistory.Select<MeterReplacementHistorySerializableSync, Guid?>((Func<MeterReplacementHistorySerializableSync, Guid?>) (x => x.CurrentMeterId)).Distinct<Guid?>().Union<Guid?>(changes.SerializedMeterReplacementHistory.Select<MeterReplacementHistorySerializableSync, Guid?>((Func<MeterReplacementHistorySerializableSync, Guid?>) (x => x.ReplacedMeterId)).Distinct<Guid?>()).ToArray<Guid?>();
          List<Meter> meterList = this.PartialSyncRepositoryFactory.GetRepository<Meter>().SearchForInMemoryOrDb((Expression<Func<Meter, bool>>) (x => meterGuidsArray.Contains<Guid?>((Guid?) x.Id)), (Func<Meter, bool>) (x => ((IEnumerable<Guid?>) meterGuidsArray).Contains<Guid?>(new Guid?(x.Id)))).ToList<Meter>();
          Guid?[] users = changes.SerializedMeterReplacementHistory.Select<MeterReplacementHistorySerializableSync, Guid?>((Func<MeterReplacementHistorySerializableSync, Guid?>) (x => x.ReplacedByUserId)).Distinct<Guid?>().ToArray<Guid?>();
          List<User> userList = this.PartialSyncRepositoryFactory.GetRepository<User>().SearchForInMemoryOrDb((Expression<Func<User, bool>>) (x => users.Contains<Guid?>((Guid?) x.Id)), (Func<User, bool>) (x => ((IEnumerable<Guid?>) users).Contains<Guid?>(new Guid?(x.Id)))).ToList<User>();
          Dictionary<Guid, MeterReplacementHistory> meterReplacementHistoryDictionary = new Dictionary<Guid, MeterReplacementHistory>();
          replacementHistoryList.ForEach((Action<MeterReplacementHistory>) (x =>
          {
            MeterReplacementHistorySerializableSync current = changes.SerializedMeterReplacementHistory.FirstOrDefault<MeterReplacementHistorySerializableSync>((Func<MeterReplacementHistorySerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.CurrentMeter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? currentMeterId = current.CurrentMeterId;
              return currentMeterId.HasValue && id == currentMeterId.GetValueOrDefault();
            }));
            x.ReplacedMeter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? replacedMeterId = current.ReplacedMeterId;
              return replacedMeterId.HasValue && id == replacedMeterId.GetValueOrDefault();
            }));
            x.ReplacedByUser = userList.FirstOrDefault<User>((Func<User, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? replacedByUserId = current.ReplacedByUserId;
              return replacedByUserId.HasValue && id == replacedByUserId.GetValueOrDefault();
            }));
            meterReplacementHistoryDictionary.Add(x.Id, x);
          }));
          Guid[] meterReplacementHistoryGuids = meterReplacementHistoryDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MeterReplacementHistory>().TransactionalInsertOrUpdateList(meterReplacementHistoryDictionary, (Expression<Func<MeterReplacementHistory, bool>>) (y => meterReplacementHistoryGuids.Contains<Guid>(y.Id)), (Func<MeterReplacementHistory, bool>) (y => ((IEnumerable<Guid>) meterReplacementHistoryGuids).Contains<Guid>(y.Id)));
        }
        List<MbusRadioMeter> mbusRadioMeterList = this.Deserialize<MeterMBusRadioSerializableSync, MbusRadioMeter>(changes.SerializedMeterMBusRadio);
        if (mbusRadioMeterList != null)
        {
          Guid?[] meters = changes.SerializedMeterMBusRadio.Select<MeterMBusRadioSerializableSync, Guid?>((Func<MeterMBusRadioSerializableSync, Guid?>) (x => x.MeterId)).Distinct<Guid?>().ToArray<Guid?>();
          List<Meter> meterList = this.PartialSyncRepositoryFactory.GetRepository<Meter>().SearchForInMemoryOrDb((Expression<Func<Meter, bool>>) (x => meters.Contains<Guid?>((Guid?) x.Id)), (Func<Meter, bool>) (x => ((IEnumerable<Guid?>) meters).Contains<Guid?>(new Guid?(x.Id)))).ToList<Meter>();
          Dictionary<Guid, MbusRadioMeter> mbusRadioMeterDictionary = new Dictionary<Guid, MbusRadioMeter>();
          mbusRadioMeterList.ForEach((Action<MbusRadioMeter>) (x =>
          {
            MeterMBusRadioSerializableSync current = changes.SerializedMeterMBusRadio.FirstOrDefault<MeterMBusRadioSerializableSync>((Func<MeterMBusRadioSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.Meter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? meterId = current.MeterId;
              return meterId.HasValue && id == meterId.GetValueOrDefault();
            }));
            mbusRadioMeterDictionary.Add(x.Id, x);
          }));
          Guid[] mbusRadioMeterGuids = mbusRadioMeterDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MbusRadioMeter>().TransactionalInsertOrUpdateList(mbusRadioMeterDictionary, (Expression<Func<MbusRadioMeter, bool>>) (y => mbusRadioMeterGuids.Contains<Guid>(y.Id)), (Func<MbusRadioMeter, bool>) (y => ((IEnumerable<Guid>) mbusRadioMeterGuids).Contains<Guid>(y.Id)));
        }
        List<OrderMessage> orderMessageList = this.Deserialize<OrderMessagesSerializableSync, OrderMessage>(changes.SerializedOrderMessages);
        if (orderMessageList != null)
        {
          Guid[] meters = changes.SerializedOrderMessages.Select<OrderMessagesSerializableSync, Guid>((Func<OrderMessagesSerializableSync, Guid>) (x => x.MeterId)).Distinct<Guid>().ToArray<Guid>();
          List<Meter> meterList = this.PartialSyncRepositoryFactory.GetRepository<Meter>().SearchForInMemoryOrDb((Expression<Func<Meter, bool>>) (x => meters.Contains<Guid>(x.Id)), (Func<Meter, bool>) (x => ((IEnumerable<Guid>) meters).Contains<Guid>(x.Id))).ToList<Meter>();
          Guid[] orders = changes.SerializedOrderMessages.Select<OrderMessagesSerializableSync, Guid>((Func<OrderMessagesSerializableSync, Guid>) (x => x.OrderId)).Distinct<Guid>().ToArray<Guid>();
          List<Order> orderList = this.PartialSyncRepositoryFactory.GetRepository<Order>().SearchForInMemoryOrDb((Expression<Func<Order, bool>>) (x => orders.Contains<Guid>(x.Id)), (Func<Order, bool>) (x => ((IEnumerable<Guid>) orders).Contains<Guid>(x.Id))).ToList<Order>();
          Dictionary<Guid, OrderMessage> orderMessageDictionary = new Dictionary<Guid, OrderMessage>();
          orderMessageList.ForEach((Action<OrderMessage>) (x =>
          {
            OrderMessagesSerializableSync current = changes.SerializedOrderMessages.FirstOrDefault<OrderMessagesSerializableSync>((Func<OrderMessagesSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.Meter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y => y.Id == current.MeterId));
            x.Order = orderList.FirstOrDefault<Order>((Func<Order, bool>) (y => y.Id == current.OrderId));
            orderMessageDictionary.Add(x.Id, x);
          }));
          Guid[] orderMessageGuids = orderMessageDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<OrderMessage>().TransactionalInsertOrUpdateList(orderMessageDictionary, (Expression<Func<OrderMessage, bool>>) (y => orderMessageGuids.Contains<Guid>(y.Id)), (Func<OrderMessage, bool>) (y => ((IEnumerable<Guid>) orderMessageGuids).Contains<Guid>(y.Id)));
        }
        List<MSS.Core.Model.DataCollectors.Minomat> minomatList1 = this.Deserialize<MinomatSerializableSync, MSS.Core.Model.DataCollectors.Minomat>(changes.SerializedMinomat);
        Dictionary<Guid, MSS.Core.Model.DataCollectors.Minomat> minomatDictionary = new Dictionary<Guid, MSS.Core.Model.DataCollectors.Minomat>();
        minomatList1?.ForEach((Action<MSS.Core.Model.DataCollectors.Minomat>) (x =>
        {
          MinomatSerializableSync current = changes.SerializedMinomat.FirstOrDefault<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (y => y.Id == x.Id));
          if (current == null)
            return;
          x.Scenario = scenarioList.FirstOrDefault<Scenario>((Func<Scenario, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? scenarioId = current.ScenarioId;
            return scenarioId.HasValue && id == scenarioId.GetValueOrDefault();
          }));
          x.Provider = providerList.FirstOrDefault<Provider>((Func<Provider, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? providerId = current.ProviderId;
            return providerId.HasValue && id == providerId.GetValueOrDefault();
          }));
          x.Country = countryList.FirstOrDefault<Country>((Func<Country, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? countryId = current.CountryId;
            return countryId.HasValue && id == countryId.GetValueOrDefault();
          }));
          minomatDictionary.Add(x.Id, x);
        }));
        if (minomatDictionary.Any<KeyValuePair<Guid, MSS.Core.Model.DataCollectors.Minomat>>())
        {
          Guid[] minomatGuids = minomatDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().TransactionalInsertOrUpdateList(minomatDictionary, (Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (y => minomatGuids.Contains<Guid>(y.Id)), (Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (y => ((IEnumerable<Guid>) minomatGuids).Contains<Guid>(y.Id)));
        }
        List<MinomatRadioDetails> minomatRadioDetailsList = this.Deserialize<MinomatRadioDetailsSerializableSync, MinomatRadioDetails>(changes.SerializedMinomatRadioDetails);
        Dictionary<Guid, MinomatRadioDetails> minomatRadioDetailsDictionary = new Dictionary<Guid, MinomatRadioDetails>();
        if (minomatRadioDetailsList != null)
        {
          Guid?[] minomats = changes.SerializedMinomatRadioDetails.Select<MinomatRadioDetailsSerializableSync, Guid?>((Func<MinomatRadioDetailsSerializableSync, Guid?>) (x => x.MinomatId)).Distinct<Guid?>().ToArray<Guid?>();
          List<MSS.Core.Model.DataCollectors.Minomat> minomatList = this.PartialSyncRepositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchForInMemoryOrDb((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => minomats.Contains<Guid?>((Guid?) x.Id)), (Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (x => ((IEnumerable<Guid?>) minomats).Contains<Guid?>(new Guid?(x.Id)))).ToList<MSS.Core.Model.DataCollectors.Minomat>();
          minomatRadioDetailsList.ForEach((Action<MinomatRadioDetails>) (x =>
          {
            MinomatRadioDetailsSerializableSync current = changes.SerializedMinomatRadioDetails.FirstOrDefault<MinomatRadioDetailsSerializableSync>((Func<MinomatRadioDetailsSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.Minomat = minomatList.FirstOrDefault<MSS.Core.Model.DataCollectors.Minomat>((Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? minomatId = current.MinomatId;
              return minomatId.HasValue && id == minomatId.GetValueOrDefault();
            }));
            minomatRadioDetailsDictionary.Add(x.Id, x);
          }));
          Guid[] minomatRadioDetailsGuids = minomatRadioDetailsDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MinomatRadioDetails>().TransactionalInsertOrUpdateList(minomatRadioDetailsDictionary, (Expression<Func<MinomatRadioDetails, bool>>) (y => minomatRadioDetailsGuids.Contains<Guid>(y.Id)), (Func<MinomatRadioDetails, bool>) (y => ((IEnumerable<Guid>) minomatRadioDetailsGuids).Contains<Guid>(y.Id)));
        }
        List<MinomatMeter> minomatMeterList = this.Deserialize<MinomatMetersSerializableSync, MinomatMeter>(changes.SerializedMinomatMeters);
        if (minomatMeterList != null)
        {
          Guid?[] minomats = changes.SerializedMinomatMeters.Select<MinomatMetersSerializableSync, Guid?>((Func<MinomatMetersSerializableSync, Guid?>) (x => x.MinomatId)).Distinct<Guid?>().ToArray<Guid?>();
          List<MSS.Core.Model.DataCollectors.Minomat> minomatList = this.PartialSyncRepositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchForInMemoryOrDb((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => minomats.Contains<Guid?>((Guid?) x.Id)), (Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (x => ((IEnumerable<Guid?>) minomats).Contains<Guid?>(new Guid?(x.Id)))).ToList<MSS.Core.Model.DataCollectors.Minomat>();
          Guid?[] meters = changes.SerializedMinomatMeters.Select<MinomatMetersSerializableSync, Guid?>((Func<MinomatMetersSerializableSync, Guid?>) (x => x.MeterId)).Distinct<Guid?>().ToArray<Guid?>();
          List<Meter> meterList = this.PartialSyncRepositoryFactory.GetRepository<Meter>().SearchForInMemoryOrDb((Expression<Func<Meter, bool>>) (x => meters.Contains<Guid?>((Guid?) x.Id)), (Func<Meter, bool>) (x => ((IEnumerable<Guid?>) meters).Contains<Guid?>(new Guid?(x.Id)))).ToList<Meter>();
          Dictionary<Guid, MinomatMeter> minomatMeterDictionary = new Dictionary<Guid, MinomatMeter>();
          minomatMeterList.ForEach((Action<MinomatMeter>) (x =>
          {
            MinomatMetersSerializableSync current = changes.SerializedMinomatMeters.FirstOrDefault<MinomatMetersSerializableSync>((Func<MinomatMetersSerializableSync, bool>) (y => y.Id == x.Id));
            if (current == null)
              return;
            x.Minomat = minomatList.FirstOrDefault<MSS.Core.Model.DataCollectors.Minomat>((Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? minomatId = current.MinomatId;
              return minomatId.HasValue && id == minomatId.GetValueOrDefault();
            }));
            x.Meter = meterList.FirstOrDefault<Meter>((Func<Meter, bool>) (y =>
            {
              Guid id = y.Id;
              Guid? meterId = current.MeterId;
              return meterId.HasValue && id == meterId.GetValueOrDefault();
            }));
            minomatMeterDictionary.Add(x.Id, x);
          }));
          Guid[] minomatMeterGuids = minomatMeterDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MinomatMeter>().TransactionalInsertOrUpdateList(minomatMeterDictionary, (Expression<Func<MinomatMeter, bool>>) (y => minomatMeterGuids.Contains<Guid>(y.Id)), (Func<MinomatMeter, bool>) (y => ((IEnumerable<Guid>) minomatMeterGuids).Contains<Guid>(y.Id)));
        }
        List<Tenant> tenantList = this.Deserialize<TenantSerializableSync, Tenant>(changes.SerializedTenant);
        Dictionary<Guid, Tenant> tenantDictionary = new Dictionary<Guid, Tenant>();
        tenantList?.ForEach((Action<Tenant>) (x => tenantDictionary.Add(x.Id, x)));
        if (tenantDictionary.Any<KeyValuePair<Guid, Tenant>>())
        {
          Guid[] tenantGuids = tenantDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<Tenant>().TransactionalInsertOrUpdateList(tenantDictionary, (Expression<Func<Tenant, bool>>) (y => tenantGuids.Contains<Guid>(y.Id)), (Func<Tenant, bool>) (y => ((IEnumerable<Guid>) tenantGuids).Contains<Guid>(y.Id)));
        }
        List<Location> locationList = this.Deserialize<LocationSerializableSync, Location>(changes.SerializedLocation);
        Dictionary<Guid, Location> locationDictionary = new Dictionary<Guid, Location>();
        locationList?.ForEach((Action<Location>) (x =>
        {
          LocationSerializableSync current = changes.SerializedLocation.FirstOrDefault<LocationSerializableSync>((Func<LocationSerializableSync, bool>) (y => y.Id == x.Id));
          if (current == null)
            return;
          x.Scenario = scenarioList.FirstOrDefault<Scenario>((Func<Scenario, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? scenarioId = current.ScenarioId;
            return scenarioId.HasValue && id == scenarioId.GetValueOrDefault();
          }));
          x.Country = countryList.FirstOrDefault<Country>((Func<Country, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? countryId = current.CountryId;
            return countryId.HasValue && id == countryId.GetValueOrDefault();
          }));
          locationDictionary.Add(x.Id, x);
        }));
        if (locationDictionary.Any<KeyValuePair<Guid, Location>>())
        {
          Guid[] locationGuids = locationDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<Location>().TransactionalInsertOrUpdateList(locationDictionary, (Expression<Func<Location, bool>>) (y => locationGuids.Contains<Guid>(y.Id)), (Func<Location, bool>) (y => ((IEnumerable<Guid>) locationGuids).Contains<Guid>(y.Id)));
        }
        List<MSS.Core.Model.DataCollectors.Minomat> minomatList2 = this.Deserialize<MinomatSerializableSync, MSS.Core.Model.DataCollectors.Minomat>(changes.SerializedMinomatPool);
        Dictionary<Guid, MSS.Core.Model.DataCollectors.Minomat> minomatPoolDictionary = new Dictionary<Guid, MSS.Core.Model.DataCollectors.Minomat>();
        minomatList2?.ForEach((Action<MSS.Core.Model.DataCollectors.Minomat>) (x =>
        {
          MinomatSerializableSync current = changes.SerializedMinomatPool.FirstOrDefault<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (y => y.Id == x.Id));
          if (current == null)
            return;
          x.Scenario = scenarioList.FirstOrDefault<Scenario>((Func<Scenario, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? scenarioId = current.ScenarioId;
            return scenarioId.HasValue && id == scenarioId.GetValueOrDefault();
          }));
          x.Provider = providerList.FirstOrDefault<Provider>((Func<Provider, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? providerId = current.ProviderId;
            return providerId.HasValue && id == providerId.GetValueOrDefault();
          }));
          x.Country = countryList.FirstOrDefault<Country>((Func<Country, bool>) (y =>
          {
            Guid id = y.Id;
            Guid? countryId = current.CountryId;
            return countryId.HasValue && id == countryId.GetValueOrDefault();
          }));
          minomatPoolDictionary.Add(x.Id, x);
        }));
        if (minomatPoolDictionary.Any<KeyValuePair<Guid, MSS.Core.Model.DataCollectors.Minomat>>())
        {
          Guid[] minomatPoolGuids = minomatPoolDictionary.Keys.ToArray<Guid>();
          this.PartialSyncRepositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().TransactionalInsertOrUpdateList(minomatPoolDictionary, (Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (y => minomatPoolGuids.Contains<Guid>(y.Id)), (Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (y => ((IEnumerable<Guid>) minomatPoolGuids).Contains<Guid>(y.Id)));
        }
        this.PartialSyncRepositoryFactory.GetSession().Transaction.Commit();
        return true;
      }
      catch (Exception ex)
      {
        this.PartialSyncRepositoryFactory.GetSession().Transaction.Rollback();
        MessageHandler.LogException(ex);
        return false;
      }
    }

    public bool SetLockByForOrders(List<Guid> orderToUpdateIdList, Guid? lockByUser = null)
    {
      try
      {
        using (ITransaction transaction = this.RepositoryFactory.GetSession().BeginTransaction())
        {
          try
          {
            IRepository<Order> ordersRepository = this.RepositoryFactory.GetRepository<Order>();
            ordersRepository.Where((Expression<Func<Order, bool>>) (_ => orderToUpdateIdList.Contains(_.Id))).ToList<Order>()?.ForEach((Action<Order>) (x =>
            {
              x.LockedBy = lockByUser;
              ordersRepository.TransactionalUpdate(x);
            }));
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            MessageHandler.LogException(ex);
            return false;
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return false;
      }
    }
  }
}
