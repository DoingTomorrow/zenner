// Decompiled with JetBrains decompiler
// Type: MSS.PartialSync.PartialSync.Managers.ClientPartialSyncManager
// Assembly: MSS.PartialSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CC2E433D-693C-481B-95B5-7303714FC801
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSync.dll

using MSS.Business.Errors;
using MSS.Business.Modules.Cleanup;
using MSS.Business.Modules.WCFRelated;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.DTO.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSync.PartialSync.Managers
{
  public class ClientPartialSyncManager : AbstractPartialSyncManager
  {
    internal bool Download(Guid userId, SerializedSyncResponse currentClientChanges = null)
    {
      MessageHandler.LogInfo("PARTIAL SYNC: Starting download from server.");
      try
      {
        SerializedSyncResponse serializedSyncResponse = (SerializedSyncResponse) null;
        DateTime result1 = new DateTime();
        bool result2 = false;
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this.RepositoryFactory);
        ApplicationParameter appParam1 = parametersManagement.GetAppParam("LastSuccesfullDownload");
        MessageHandler.LogDebug("PARTIAL SYNC: Last download from server date=" + (object) appParam1);
        if (!string.IsNullOrEmpty(appParam1?.Value) && !DateTime.TryParse(appParam1.Value, out result1))
          return false;
        List<Guid> list1 = this.RepositoryFactory.GetRepository<Order>().SearchFor((Expression<Func<Order, bool>>) (x => (int) x.OrderType == 1)).Select<Order, Guid>((Func<Order, Guid>) (x => x.RootStructureNodeId)).Distinct<Guid>().ToList<Guid>();
        List<Guid> list2 = this.RepositoryFactory.GetRepository<Order>().GetAll().Select<Order, Guid>((Func<Order, Guid>) (x => x.Id)).Distinct<Guid>().ToList<Guid>();
        ApplicationParameter appParam2 = parametersManagement.GetAppParam("MinomatUseMasterpool");
        if (!string.IsNullOrEmpty(appParam2?.Value))
          bool.TryParse(appParam2.Value, out result2);
        MessageHandler.LogInfo("PARTIAL SYNC: Starting to request changeset from server.");
        using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
          serializedSyncResponse = serviceClient.DownloadFromServer(userId, result1, list1, list2, result2);
        MessageHandler.LogInfo("PARTIAL SYNC: Finished requesting changeset from server.");
        if (serializedSyncResponse != null)
        {
          if (serializedSyncResponse.SerializedOrders != null && serializedSyncResponse.SerializedOrders.Count > 0)
          {
            MessageHandler.LogInfo("PARTIAL SYNC: Lock all orders in the changeset, after inserting to client.");
            this.SetLockedByOnAllOrdersInResponse(serializedSyncResponse, userId);
          }
          SerializedSyncResponse changes = currentClientChanges == null ? serializedSyncResponse : this.GetValidChangesForClient(currentClientChanges, serializedSyncResponse);
          MessageHandler.LogInfo("PARTIAL SYNC: Starting to insert changeset to client.");
          if (this.InsertChanges(changes))
          {
            if (changes.SerializedOrders != null && changes.SerializedOrders.Count > 0)
            {
              MessageHandler.LogInfo("PARTIAL SYNC: Starting to lock downloaded orders on the server.");
              using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
              {
                List<Guid> list3 = changes.SerializedOrders.Select<OrderSerializableSync, Guid>((Func<OrderSerializableSync, Guid>) (_ => _.Id)).ToList<Guid>();
                if (list3.Any<Guid>())
                  serviceClient.LockDownloadedEntitiesFromServer(list3, userId);
                MessageHandler.LogInfo("PARTIAL SYNC: Finished locking downloaded orders on the server.");
              }
            }
            using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
            {
              string timeFromServer = serviceClient.GetTimeFromServer();
              if (timeFromServer != null)
              {
                MessageHandler.LogInfo("PARTIAL SYNC: Update Last Download On date.");
                ApplicationParameter appParam3 = parametersManagement.GetAppParam("LastSuccesfullDownload");
                appParam3.Value = timeFromServer;
                parametersManagement.Update(appParam3);
              }
            }
          }
        }
        MessageHandler.LogInfo("PARTIAL SYNC: Finished download from server.");
        return true;
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return false;
      }
    }

    private SerializedSyncResponse GetValidChangesForClient(
      SerializedSyncResponse currentClientChanges,
      SerializedSyncResponse serverChanges)
    {
      return new SerializedSyncResponse()
      {
        SerializedOrderMessages = currentClientChanges.SerializedOrderMessages == null || currentClientChanges.SerializedOrderMessages.Count <= 0 ? serverChanges.SerializedOrderMessages : serverChanges.SerializedOrderMessages.Where<OrderMessagesSerializableSync>((Func<OrderMessagesSerializableSync, bool>) (x => !currentClientChanges.SerializedOrderMessages.Any<OrderMessagesSerializableSync>((Func<OrderMessagesSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<OrderMessagesSerializableSync>(),
        SerializedOrderUser = serverChanges.SerializedOrderUser == null || serverChanges.SerializedOrderUser.Count <= 0 || currentClientChanges.SerializedOrderUser == null || currentClientChanges.SerializedOrderUser.Count <= 0 ? serverChanges.SerializedOrderUser : serverChanges.SerializedOrderUser.Where<OrderUserSerializableSync>((Func<OrderUserSerializableSync, bool>) (x => !currentClientChanges.SerializedOrderUser.Any<OrderUserSerializableSync>((Func<OrderUserSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<OrderUserSerializableSync>(),
        SerializedOrders = serverChanges.SerializedOrders == null || serverChanges.SerializedOrders.Count <= 0 || currentClientChanges.SerializedOrders == null || currentClientChanges.SerializedOrders.Count <= 0 ? serverChanges.SerializedOrders : serverChanges.SerializedOrders.Where<OrderSerializableSync>((Func<OrderSerializableSync, bool>) (x => !currentClientChanges.SerializedOrders.Any<OrderSerializableSync>((Func<OrderSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<OrderSerializableSync>(),
        SerializedLocation = serverChanges.SerializedLocation == null || serverChanges.SerializedLocation.Count <= 0 || currentClientChanges.SerializedLocation == null || currentClientChanges.SerializedLocation.Count <= 0 ? serverChanges.SerializedLocation : serverChanges.SerializedLocation.Where<LocationSerializableSync>((Func<LocationSerializableSync, bool>) (x => !currentClientChanges.SerializedLocation.Any<LocationSerializableSync>((Func<LocationSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<LocationSerializableSync>(),
        SerializedTenant = serverChanges.SerializedTenant == null || serverChanges.SerializedTenant.Count <= 0 || currentClientChanges.SerializedTenant == null || currentClientChanges.SerializedTenant.Count <= 0 ? serverChanges.SerializedTenant : serverChanges.SerializedTenant.Where<TenantSerializableSync>((Func<TenantSerializableSync, bool>) (x => !currentClientChanges.SerializedTenant.Any<TenantSerializableSync>((Func<TenantSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<TenantSerializableSync>(),
        SerializedMeter = serverChanges.SerializedMeter == null || serverChanges.SerializedMeter.Count <= 0 || currentClientChanges.SerializedMeter == null || currentClientChanges.SerializedMeter.Count <= 0 ? serverChanges.SerializedMeter : serverChanges.SerializedMeter.Where<MeterSerializableSync>((Func<MeterSerializableSync, bool>) (x => !currentClientChanges.SerializedMeter.Any<MeterSerializableSync>((Func<MeterSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MeterSerializableSync>(),
        SerializedMeterMBusRadio = serverChanges.SerializedMeterMBusRadio == null || serverChanges.SerializedMeterMBusRadio.Count <= 0 || currentClientChanges.SerializedMeterMBusRadio == null || currentClientChanges.SerializedMeterMBusRadio.Count <= 0 ? serverChanges.SerializedMeterMBusRadio : serverChanges.SerializedMeterMBusRadio.Where<MeterMBusRadioSerializableSync>((Func<MeterMBusRadioSerializableSync, bool>) (x => !currentClientChanges.SerializedMeterMBusRadio.Any<MeterMBusRadioSerializableSync>((Func<MeterMBusRadioSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MeterMBusRadioSerializableSync>(),
        SerializedMeterRadioDetails = serverChanges.SerializedMeterRadioDetails == null || serverChanges.SerializedMeterRadioDetails.Count <= 0 || currentClientChanges.SerializedMeterRadioDetails == null || currentClientChanges.SerializedMeterRadioDetails.Count <= 0 ? serverChanges.SerializedMeterRadioDetails : serverChanges.SerializedMeterRadioDetails.Where<MeterRadioDetailsSerializableSync>((Func<MeterRadioDetailsSerializableSync, bool>) (x => !currentClientChanges.SerializedMeterRadioDetails.Any<MeterRadioDetailsSerializableSync>((Func<MeterRadioDetailsSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MeterRadioDetailsSerializableSync>(),
        SerializedMeterReplacementHistory = serverChanges.SerializedMeterReplacementHistory == null || serverChanges.SerializedMeterReplacementHistory.Count <= 0 || currentClientChanges.SerializedMeterReplacementHistory == null || currentClientChanges.SerializedMeterReplacementHistory.Count <= 0 ? serverChanges.SerializedMeterReplacementHistory : serverChanges.SerializedMeterReplacementHistory.Where<MeterReplacementHistorySerializableSync>((Func<MeterReplacementHistorySerializableSync, bool>) (x => !currentClientChanges.SerializedMeterReplacementHistory.Any<MeterReplacementHistorySerializableSync>((Func<MeterReplacementHistorySerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MeterReplacementHistorySerializableSync>(),
        SerializedMinomat = serverChanges.SerializedMinomat == null || serverChanges.SerializedMinomat.Count <= 0 || currentClientChanges.SerializedMinomat == null || currentClientChanges.SerializedMinomat.Count <= 0 ? serverChanges.SerializedMinomat : serverChanges.SerializedMinomat.Where<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (x => !currentClientChanges.SerializedMinomat.Any<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MinomatSerializableSync>(),
        SerializedMinomatMeters = serverChanges.SerializedMinomatMeters == null || serverChanges.SerializedMinomatMeters.Count <= 0 || currentClientChanges.SerializedMinomatMeters == null || currentClientChanges.SerializedMinomatMeters.Count <= 0 ? serverChanges.SerializedMinomatMeters : serverChanges.SerializedMinomatMeters.Where<MinomatMetersSerializableSync>((Func<MinomatMetersSerializableSync, bool>) (x => !currentClientChanges.SerializedMinomatMeters.Any<MinomatMetersSerializableSync>((Func<MinomatMetersSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MinomatMetersSerializableSync>(),
        SerializedMinomatRadioDetails = serverChanges.SerializedMinomatRadioDetails == null || serverChanges.SerializedMinomatRadioDetails.Count <= 0 || currentClientChanges.SerializedMinomatRadioDetails == null || currentClientChanges.SerializedMinomatRadioDetails.Count <= 0 ? serverChanges.SerializedMinomatRadioDetails : serverChanges.SerializedMinomatRadioDetails.Where<MinomatRadioDetailsSerializableSync>((Func<MinomatRadioDetailsSerializableSync, bool>) (x => !currentClientChanges.SerializedMinomatRadioDetails.Any<MinomatRadioDetailsSerializableSync>((Func<MinomatRadioDetailsSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MinomatRadioDetailsSerializableSync>(),
        SerializedNote = serverChanges.SerializedNote == null || serverChanges.SerializedNote.Count <= 0 || currentClientChanges.SerializedNote == null || currentClientChanges.SerializedNote.Count <= 0 ? serverChanges.SerializedNote : serverChanges.SerializedNote.Where<NoteSerializableSync>((Func<NoteSerializableSync, bool>) (x => !currentClientChanges.SerializedNote.Any<NoteSerializableSync>((Func<NoteSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<NoteSerializableSync>(),
        SerializedStructureNode = serverChanges.SerializedStructureNode == null || serverChanges.SerializedStructureNode.Count <= 0 || currentClientChanges.SerializedStructureNode == null || currentClientChanges.SerializedStructureNode.Count <= 0 ? serverChanges.SerializedStructureNode : serverChanges.SerializedStructureNode.Where<StructureNodeSerializableSync>((Func<StructureNodeSerializableSync, bool>) (x => !currentClientChanges.SerializedStructureNode.Any<StructureNodeSerializableSync>((Func<StructureNodeSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<StructureNodeSerializableSync>(),
        SerializedStructureNodeLinks = serverChanges.SerializedStructureNodeLinks == null || serverChanges.SerializedStructureNodeLinks.Count <= 0 || currentClientChanges.SerializedStructureNodeLinks == null || currentClientChanges.SerializedStructureNodeLinks.Count <= 0 ? serverChanges.SerializedStructureNodeLinks : serverChanges.SerializedStructureNodeLinks.Where<StructureNodeLinksSerializableSync>((Func<StructureNodeLinksSerializableSync, bool>) (x => !currentClientChanges.SerializedStructureNodeLinks.Any<StructureNodeLinksSerializableSync>((Func<StructureNodeLinksSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<StructureNodeLinksSerializableSync>(),
        SerializedStructureNodeEquipmentSettings = serverChanges.SerializedStructureNodeEquipmentSettings == null || serverChanges.SerializedStructureNodeEquipmentSettings.Count <= 0 || currentClientChanges.SerializedStructureNodeEquipmentSettings == null || currentClientChanges.SerializedStructureNodeEquipmentSettings.Count <= 0 ? serverChanges.SerializedStructureNodeEquipmentSettings : serverChanges.SerializedStructureNodeEquipmentSettings.Where<StructureNodeEquipmentSettingsSerializableSync>((Func<StructureNodeEquipmentSettingsSerializableSync, bool>) (x => !currentClientChanges.SerializedStructureNodeEquipmentSettings.Any<StructureNodeEquipmentSettingsSerializableSync>((Func<StructureNodeEquipmentSettingsSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<StructureNodeEquipmentSettingsSerializableSync>(),
        SerializedMinomatPool = serverChanges.SerializedMinomatPool == null || serverChanges.SerializedMinomatPool.Count <= 0 || currentClientChanges.SerializedMinomatPool == null || currentClientChanges.SerializedMinomatPool.Count <= 0 ? serverChanges.SerializedMinomatPool : serverChanges.SerializedMinomatPool.Where<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (x => !currentClientChanges.SerializedMinomatPool.Any<MinomatSerializableSync>((Func<MinomatSerializableSync, bool>) (y => y.Id == x.Id)))).ToList<MinomatSerializableSync>()
      };
    }

    public bool Send(Guid userId, SerializedSyncResponse response = null)
    {
      MessageHandler.LogInfo("PARTIAL SYNC: Starting send to server.");
      bool flag = false;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this.RepositoryFactory);
      if (response == null)
      {
        response = this.GetValidChangesFromClient(userId);
        if (response == null)
          return false;
      }
      MessageHandler.LogInfo("PARTIAL SYNC: Starting to insert changeset to server.");
      using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
        flag = serviceClient.UploadToServer(response);
      MessageHandler.LogInfo("PARTIAL SYNC: Finished inserting changeset to server.");
      MessageHandler.LogInfo("PARTIAL SYNC: Update Last Upload On date.");
      if (flag)
      {
        using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
        {
          string timeFromServer = serviceClient.GetTimeFromServer();
          if (timeFromServer != null)
          {
            ApplicationParameter appParam1 = parametersManagement.GetAppParam("LastSuccesfullDownload");
            appParam1.Value = timeFromServer;
            parametersManagement.Update(appParam1);
            ApplicationParameter appParam2 = parametersManagement.GetAppParam("LastSuccesfullUpload");
            appParam2.Value = DateTime.Now.ToString("o");
            parametersManagement.Update(appParam2);
          }
        }
      }
      MessageHandler.LogInfo("PARTIAL SYNC: Finished send to server.");
      return flag;
    }

    private SerializedSyncResponse GetValidChangesFromClient(Guid userId)
    {
      SerializedSyncResponse changesFromClient = (SerializedSyncResponse) null;
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this.RepositoryFactory);
      ApplicationParameter appParam = parametersManagement.GetAppParam("LastSuccesfullUpload");
      MessageHandler.LogDebug("PARTIAL SYNC: Last send to server date=" + (object) appParam);
      if (appParam != null)
      {
        if (string.IsNullOrEmpty(appParam.Value))
        {
          DateTime result;
          if (!DateTime.TryParse(parametersManagement.GetAppParam("LastSuccesfullDownload").Value, out result))
            return (SerializedSyncResponse) null;
          changesFromClient = this.GetClientChangeset(userId, result);
        }
        else
        {
          DateTime result;
          if (!DateTime.TryParse(appParam.Value, out result))
            return (SerializedSyncResponse) null;
          MessageHandler.LogInfo("PARTIAL SYNC: Starting to request changeset from client.");
          changesFromClient = this.GetClientChangeset(userId, result);
          MessageHandler.LogInfo("PARTIAL SYNC: Finished requesting changeset from client.");
        }
        if (changesFromClient == null)
        {
          MessageHandler.LogInfo("PARTIAL SYNC: Some error occured while the changeset from client was retrieved.");
          return (SerializedSyncResponse) null;
        }
      }
      return changesFromClient;
    }

    public void ExecuteInitialSynchronization(Guid userId) => this.Download(userId);

    public bool Synchronize(Guid userId)
    {
      SerializedSyncResponse changesFromClient = this.GetValidChangesFromClient(userId);
      return this.Download(userId, changesFromClient) && this.Send(userId, changesFromClient);
    }

    private void SetLockedByOnAllOrdersInResponse(SerializedSyncResponse response, Guid userId)
    {
      foreach (OrderSerializableSync serializedOrder in response.SerializedOrders)
        serializedOrder.LockedBy = new Guid?(userId);
    }

    public CleanupModel GetOldOrdersCleanupModel(Guid userId, int? oldOrderDefinitionInDays = null)
    {
      CleanupModel cleanupModel = new CleanupModel();
      try
      {
        ApplicationParameter appParam = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this.RepositoryFactory).GetAppParam("LastSuccesfullUpload");
        DateTime comparisonTimePoint = DateTime.MinValue;
        if (!string.IsNullOrEmpty(appParam.Value) && !DateTime.TryParse(appParam.Value, out comparisonTimePoint))
        {
          cleanupModel.InitializeEmpty();
          return cleanupModel;
        }
        Expression<Func<OrderUser, bool>> expr1 = (Expression<Func<OrderUser, bool>>) (x => (int) x.Order.Status == 6 && x.Order.LastChangedOn < (DateTime?) comparisonTimePoint);
        Expression<Func<OrderUser, bool>> predicate = expr1;
        if (oldOrderDefinitionInDays.HasValue)
          predicate = expr1.And<OrderUser>((Expression<Func<OrderUser, bool>>) (x => x.Order.LastChangedOn < (DateTime?) DateTime.Now.AddDays((double) (-1 * oldOrderDefinitionInDays.Value))));
        List<\u003C\u003Ef__AnonymousType0<Guid, Guid, Guid>> list1 = this.RepositoryFactory.GetRepository<OrderUser>().Where(predicate).Select(_ => new
        {
          OrderUserId = _.Id,
          OrderId = _.Order.Id,
          RootStructureNodeId = _.Order.RootStructureNodeId
        }).ToList();
        cleanupModel.IDsOrderUser = list1.Select(_ => _.OrderUserId).ToList<Guid>();
        cleanupModel.IDsOrder = list1.Select(_ => _.OrderId).Distinct<Guid>().ToList<Guid>();
        if (cleanupModel.IDsOrder.Count > 0)
        {
          List<Guid> resultAllStructuresWhichWillRemainAfterCleanup = this.RepositoryFactory.GetRepository<OrderUser>().Where((Expression<Func<OrderUser, bool>>) (x => (int) x.Order.OrderType == 1 && !cleanupModel.IDsOrder.Contains(x.Order.Id))).Select<OrderUser, Guid>((Expression<Func<OrderUser, Guid>>) (_ => _.Order.RootStructureNodeId)).Distinct<Guid>().ToList<Guid>();
          Guid[] rootStructureGuidsToBeRemoved = list1.Select(_ => _.RootStructureNodeId).Distinct<Guid>().Where<Guid>((Func<Guid, bool>) (_ => !resultAllStructuresWhichWillRemainAfterCleanup.Contains(_))).ToArray<Guid>();
          List<StructureNodeLinks> list2 = this.RepositoryFactory.GetRepository<StructureNodeLinks>().Where((Expression<Func<StructureNodeLinks, bool>>) (x => rootStructureGuidsToBeRemoved.Contains<Guid>(x.RootNode.Id))).ToList<StructureNodeLinks>();
          cleanupModel.IDsOrderReadingValues = this.RepositoryFactory.GetRepository<OrderReadingValues>().Where((Expression<Func<OrderReadingValues, bool>>) (_ => cleanupModel.IDsOrder.Contains(_.OrderObj.Id))).Select<OrderReadingValues, Guid>((Expression<Func<OrderReadingValues, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsStructureNodeLinks = list2.Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsStructureNode = list2.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (_ => _.Node != null)).Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (_ => _.Node.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsStructureNodeEquipmentSettings = this.RepositoryFactory.GetRepository<StructureNodeEquipmentSettings>().Where((Expression<Func<StructureNodeEquipmentSettings, bool>>) (_ => cleanupModel.IDsStructureNode.Contains(_.StructureNode.Id))).Select<StructureNodeEquipmentSettings, Guid>((Expression<Func<StructureNodeEquipmentSettings, Guid>>) (_ => _.Id)).ToList<Guid>();
          cleanupModel.IDsNote = this.RepositoryFactory.GetRepository<Note>().Where((Expression<Func<Note, bool>>) (_ => cleanupModel.IDsStructureNode.Contains(_.StructureNode.Id))).Select<Note, Guid>((Expression<Func<Note, Guid>>) (_ => _.Id)).ToList<Guid>();
          cleanupModel.IDsMeter = list2.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (x => x.Node != null && x.Node.EntityName == StructureNodeTypeEnum.Meter.ToString())).Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (x => x.Node.EntityId)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMeterRadioDetails = this.RepositoryFactory.GetRepository<MeterRadioDetails>().Where((Expression<Func<MeterRadioDetails, bool>>) (_ => cleanupModel.IDsMeter.Contains(_.Meter.Id))).Select<MeterRadioDetails, Guid>((Expression<Func<MeterRadioDetails, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMeterReplacementHistory = this.RepositoryFactory.GetRepository<MeterReplacementHistory>().Where((Expression<Func<MeterReplacementHistory, bool>>) (_ => cleanupModel.IDsMeter.Contains(_.CurrentMeter.Id) || cleanupModel.IDsMeter.Contains(_.ReplacedMeter.Id))).Select<MeterReplacementHistory, Guid>((Expression<Func<MeterReplacementHistory, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMbusRadioMeter = this.RepositoryFactory.GetRepository<MbusRadioMeter>().Where((Expression<Func<MbusRadioMeter, bool>>) (_ => cleanupModel.IDsMeter.Contains(_.Meter.Id))).Select<MbusRadioMeter, Guid>((Expression<Func<MbusRadioMeter, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsOrderMessage = this.RepositoryFactory.GetRepository<OrderMessage>().Where((Expression<Func<OrderMessage, bool>>) (_ => cleanupModel.IDsMeter.Contains(_.Meter.Id))).Select<OrderMessage, Guid>((Expression<Func<OrderMessage, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMinomat = list2.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (x =>
          {
            if (x.Node == null)
              return false;
            string entityName1 = x.Node.EntityName;
            StructureNodeTypeEnum structureNodeTypeEnum = StructureNodeTypeEnum.MinomatMaster;
            string str1 = structureNodeTypeEnum.ToString();
            if (entityName1 == str1)
              return true;
            string entityName2 = x.Node.EntityName;
            structureNodeTypeEnum = StructureNodeTypeEnum.MinomatSlave;
            string str2 = structureNodeTypeEnum.ToString();
            return entityName2 == str2;
          })).Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (x => x.Node.EntityId)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMinomatRadioDetails = this.RepositoryFactory.GetRepository<MinomatRadioDetails>().Where((Expression<Func<MinomatRadioDetails, bool>>) (_ => cleanupModel.IDsMinomat.Contains(_.Minomat.Id))).Select<MinomatRadioDetails, Guid>((Expression<Func<MinomatRadioDetails, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMinomatMeter = this.RepositoryFactory.GetRepository<MinomatMeter>().Where((Expression<Func<MinomatMeter, bool>>) (x => cleanupModel.IDsMinomat.Contains(x.Minomat.Id))).Select<MinomatMeter, Guid>((Expression<Func<MinomatMeter, Guid>>) (_ => _.Id)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsTenant = list2.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (x => x.Node != null && x.Node.EntityName == StructureNodeTypeEnum.Tenant.ToString())).Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (x => x.Node.EntityId)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsLocation = list2.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (x => x.Node != null && x.Node.EntityName == StructureNodeTypeEnum.Location.ToString())).Select<StructureNodeLinks, Guid>((Func<StructureNodeLinks, Guid>) (x => x.Node.EntityId)).Distinct<Guid>().ToList<Guid>();
          cleanupModel.IDsMeterReadingValue = this.RepositoryFactory.GetRepository<OrderReadingValues>().Where((Expression<Func<OrderReadingValues, bool>>) (_ => _.OrderObj != default (object) && cleanupModel.IDsOrder.Contains(_.OrderObj.Id) && _.MeterReadingValue != default (object))).Select<OrderReadingValues, Guid>((Expression<Func<OrderReadingValues, Guid>>) (_ => _.MeterReadingValue.Id)).Distinct<Guid>().ToList<Guid>();
        }
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        cleanupModel.InitializeEmpty();
        return cleanupModel;
      }
      MessageHandler.LogDebug("Objects to be deleted:", (object) cleanupModel);
      return cleanupModel;
    }

    public SerializedSyncResponse GetClientChangeset(Guid userId, DateTime lastSuccessfulUpload = default (DateTime))
    {
      SerializedSyncResponse response = new SerializedSyncResponse();
      try
      {
        List<Order> list1 = this.RepositoryFactory.GetRepository<Order>().SearchFor((Expression<Func<Order, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).Distinct<Order>().ToList<Order>();
        response.SerializedOrders = this.Serialize<Order, OrderSerializableSync>(list1);
        List<StructureNodeLinks> list2 = this.RepositoryFactory.GetRepository<StructureNodeLinks>().SearchFor((Expression<Func<StructureNodeLinks, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<StructureNodeLinks>();
        response.SerializedStructureNodeLinks = this.Serialize<StructureNodeLinks, StructureNodeLinksSerializableSync>(list2);
        List<StructureNode> list3 = this.RepositoryFactory.GetRepository<StructureNode>().SearchFor((Expression<Func<StructureNode, bool>>) (y => y.LastChangedOn == new DateTime?() || y.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<StructureNode>();
        response.SerializedStructureNode = this.Serialize<StructureNode, StructureNodeSerializableSync>(list3);
        List<StructureNodeEquipmentSettings> list4 = this.RepositoryFactory.GetRepository<StructureNodeEquipmentSettings>().SearchFor((Expression<Func<StructureNodeEquipmentSettings, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<StructureNodeEquipmentSettings>();
        response.SerializedStructureNodeEquipmentSettings = this.Serialize<StructureNodeEquipmentSettings, StructureNodeEquipmentSettingsSerializableSync>(list4);
        List<Note> list5 = this.RepositoryFactory.GetRepository<Note>().SearchFor((Expression<Func<Note, bool>>) (x => (DateTime?) x.LastChangedOn == new DateTime?() || x.LastChangedOn > lastSuccessfulUpload)).ToList<Note>();
        response.SerializedNote = this.Serialize<Note, NoteSerializableSync>(list5);
        List<Meter> list6 = this.RepositoryFactory.GetRepository<Meter>().SearchFor((Expression<Func<Meter, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<Meter>();
        response.SerializedMeter = this.Serialize<Meter, MeterSerializableSync>(list6);
        List<MeterRadioDetails> list7 = this.RepositoryFactory.GetRepository<MeterRadioDetails>().SearchFor((Expression<Func<MeterRadioDetails, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MeterRadioDetails>();
        response.SerializedMeterRadioDetails = this.Serialize<MeterRadioDetails, MeterRadioDetailsSerializableSync>(list7);
        List<MeterReplacementHistory> list8 = this.RepositoryFactory.GetRepository<MeterReplacementHistory>().SearchFor((Expression<Func<MeterReplacementHistory, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MeterReplacementHistory>();
        response.SerializedMeterReplacementHistory = this.Serialize<MeterReplacementHistory, MeterReplacementHistorySerializableSync>(list8);
        List<MbusRadioMeter> list9 = this.RepositoryFactory.GetRepository<MbusRadioMeter>().SearchFor((Expression<Func<MbusRadioMeter, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MbusRadioMeter>();
        response.SerializedMeterMBusRadio = this.Serialize<MbusRadioMeter, MeterMBusRadioSerializableSync>(list9);
        List<OrderMessage> list10 = this.RepositoryFactory.GetRepository<OrderMessage>().SearchFor((Expression<Func<OrderMessage, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<OrderMessage>();
        response.SerializedOrderMessages = this.Serialize<OrderMessage, OrderMessagesSerializableSync>(list10);
        List<MSS.Core.Model.DataCollectors.Minomat> list11 = this.RepositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MSS.Core.Model.DataCollectors.Minomat>();
        response.SerializedMinomat = this.Serialize<MSS.Core.Model.DataCollectors.Minomat, MinomatSerializableSync>(list11);
        List<MinomatRadioDetails> list12 = this.RepositoryFactory.GetRepository<MinomatRadioDetails>().SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MinomatRadioDetails>();
        response.SerializedMinomatRadioDetails = this.Serialize<MinomatRadioDetails, MinomatRadioDetailsSerializableSync>(list12);
        List<MinomatMeter> list13 = this.RepositoryFactory.GetRepository<MinomatMeter>().SearchFor((Expression<Func<MinomatMeter, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<MinomatMeter>();
        response.SerializedMinomatMeters = this.Serialize<MinomatMeter, MinomatMetersSerializableSync>(list13);
        List<Tenant> list14 = this.RepositoryFactory.GetRepository<Tenant>().SearchFor((Expression<Func<Tenant, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<Tenant>();
        response.SerializedTenant = this.Serialize<Tenant, TenantSerializableSync>(list14);
        List<Location> list15 = this.RepositoryFactory.GetRepository<Location>().SearchFor((Expression<Func<Location, bool>>) (x => x.LastChangedOn == new DateTime?() || x.LastChangedOn > (DateTime?) lastSuccessfulUpload)).ToList<Location>();
        response.SerializedLocation = this.Serialize<Location, LocationSerializableSync>(list15);
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        return (SerializedSyncResponse) null;
      }
      this.LogChangeset(response);
      return response;
    }
  }
}
