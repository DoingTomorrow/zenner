// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.ReceiverMinoConnectManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Orders;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class ReceiverMinoConnectManager : BaseMinoConnectManager
  {
    private readonly MeterReceiverManager _receiver;
    private List<ZENNER.CommonLibrary.Entities.Meter> _gmmMeters;
    private List<MeterSerializableDTO> metersInStructure = new List<MeterSerializableDTO>();
    private List<MeterSerializableDTO> metersRead = new List<MeterSerializableDTO>();
    public EventHandler<string> OnMeterValuesReceivedHandler;
    public System.EventHandler OnJobFinished;

    public ReceiverMinoConnectManager(
      IRepositoryFactory repositoryFactory,
      OrderDTO selectedOrder,
      Guid selectedStructureNodeId,
      ProfileType profileType)
    {
      this._repositoryFactory = repositoryFactory;
      this._profileType = profileType;
      this._orderRepository = this._repositoryFactory.GetRepository<Order>();
      this._orderId = selectedOrder.Id;
      this.metersInStructure = StructuresHelper.DeserializeStructure(selectedOrder.StructureBytes).meterList;
      this._receiver = GmmInterface.Receiver;
      this._receiver.StoreResultsToDatabase = true;
      this._receiver.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError += new EventHandler<Exception>(this.receiver_OnError);
      this._receiver.ConnectionLost += new System.EventHandler(this._receiver_ConnectionLost);
    }

    ~ReceiverMinoConnectManager()
    {
      this._receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError -= new EventHandler<Exception>(this.receiver_OnError);
      this._receiver.ConnectionLost -= new System.EventHandler(this._receiver_ConnectionLost);
    }

    private void _receiver_ConnectionLost(object sender, EventArgs e)
    {
      this.ShowMessage(MessageTypeEnum.Warning, Resources.MSS_ReceiverManager_ConnectionLost);
    }

    private void _receiver_OnJobCompleted(object sender, Job e)
    {
      this.StopReadingValues();
      System.EventHandler onJobFinished = this.OnJobFinished;
      if (onJobFinished != null)
        onJobFinished((object) this, (EventArgs) null);
      this.ShowMessage(MessageTypeEnum.Success, Resources.MSS_Client_ExecuteReadingOrder_Succedded);
    }

    private void receiver_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      if (e.AvailableValues == null || e.AvailableValues.Count == 0)
        return;
      MeterSerializableDTO meterSerializableDto = this.metersInStructure.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.SerialNumber == e.SerialNumber));
      if (this.metersRead.Contains(meterSerializableDto) || meterSerializableDto == null)
        return;
      bool flag = this.SaveReadingValues(e.SerialNumber);
      if (flag)
        EventPublisher.Publish<StructureBytesUpdated>(new StructureBytesUpdated()
        {
          MeterReadByWalkBy = meterSerializableDto.Id,
          SerialNumberRead = e.SerialNumber,
          AnyReadingValuesRead = flag
        }, (object) this);
      if (this.metersRead.Contains(meterSerializableDto))
        return;
      this.metersRead.Add(meterSerializableDto);
    }

    private void receiver_OnError(object sender, Exception e)
    {
      this.StopReadingValues();
      this.ShowMessage(MessageTypeEnum.Warning, e.Message);
    }

    public bool StartReadingValues(
      ObservableCollection<ExecuteOrderStructureNode> meterList,
      OrderDTO selectedOrder,
      ProfileType profileType,
      EquipmentModel equipment)
    {
      this._profileType = profileType;
      this._orderId = selectedOrder.Id;
      List<long> filterListForOrder = new GMMManager(this._repositoryFactory).GetFilterListForOrder(this._orderId);
      this._gmmMeters = GMMHelper.GetMeters(meterList, filterListForOrder, selectedOrder.StructureType);
      DeviceModel system = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice, TransceiverType.Receiver).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (s => GmmInterface.DeviceManager.GetProfileTypes(s, equipment).Any<ProfileType>((Func<ProfileType, bool>) (p => p.Name == this._profileType.Name))));
      if (equipment == null)
        return false;
      if (system != null && this._profileType != null)
      {
        foreach (ZENNER.CommonLibrary.Entities.Meter gmmMeter in this._gmmMeters)
          gmmMeter.ConnectionAdjuster = GmmInterface.DeviceManager.GetConnectionAdjuster(gmmMeter.DeviceModel, equipment, this._profileType);
        this._receiver.StartRead(system, this._gmmMeters, equipment, this._profileType);
        EventPublisher.Publish<ProgressEvent>(new ProgressEvent()
        {
          Value = 1
        }, (object) this);
      }
      return true;
    }

    public void StopReadingValues()
    {
      this._receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError -= new EventHandler<Exception>(this.receiver_OnError);
      EventPublisher.Publish<ProgressEvent>(new ProgressEvent()
      {
        Value = 100
      }, (object) this);
      this._receiver.StopRead();
    }
  }
}
