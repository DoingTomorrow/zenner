// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.WalkByTestManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.Meters;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class WalkByTestManager
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private ProfileType _profileType;
    private StructuresManager _structuresManager;
    private readonly MeterReceiverManager _receiver;
    private List<ZENNER.CommonLibrary.Entities.Meter> _gmmMeters;
    private List<MeterDTO> _meterDTOsInStructure;
    public EventHandler<MSS.Core.Model.Meters.Meter> OnMeterValuesReceivedHandler;
    public System.EventHandler OnErrorReceivedHandler;
    private static bool _isWalkByTestStarted;

    public WalkByTestManager(
      IRepositoryFactory repositoryFactory,
      Guid selectedStructureNodeId,
      ProfileType profileType)
    {
      this._repositoryFactory = repositoryFactory;
      this._profileType = profileType;
      this._structuresManager = this.GetStructureManagerInstance();
      this._receiver = GmmInterface.Receiver;
      this._receiver.StoreResultsToDatabase = true;
      this._receiver.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError += new EventHandler<Exception>(this.receiver_OnError);
      this._receiver.ConnectionLost += new System.EventHandler(this._receiver_ConnectionLost);
      WalkByTestManager.IsWalkByTestStarted = true;
    }

    ~WalkByTestManager()
    {
      this._receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError -= new EventHandler<Exception>(this.receiver_OnError);
      this._receiver.ConnectionLost += new System.EventHandler(this._receiver_ConnectionLost);
      WalkByTestManager.IsWalkByTestStarted = false;
    }

    private StructuresManager GetStructureManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private ReadingValuesManager GetReadingValuesManagerInstance()
    {
      return new ReadingValuesManager(this._repositoryFactory);
    }

    private void _receiver_ConnectionLost(object sender, EventArgs e)
    {
      this.ShowMessage(MessageTypeEnum.Warning, Resources.MSS_ReceiverManager_ConnectionLost);
    }

    private void _receiver_OnJobCompleted(object sender, Job e)
    {
      this.ShowMessage(MessageTypeEnum.Success, Resources.MSS_Client_ExecuteReadingOrder_Succedded);
    }

    private void receiver_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      MeterDTO meterDTO = this._meterDTOsInStructure.First<MeterDTO>((Func<MeterDTO, bool>) (meter => meter.SerialNumber == e.SerialNumber));
      if (meterDTO.DeviceType != DeviceTypeEnum.Minoprotect3)
        ;
      meterDTO.IsReceived = true;
      MSS.Core.Model.Meters.Meter e1 = this._structuresManager.TransactionalSaveOrUpdateMeter(meterDTO);
      EventHandler<MSS.Core.Model.Meters.Meter> valuesReceivedHandler = this.OnMeterValuesReceivedHandler;
      if (valuesReceivedHandler == null)
        return;
      valuesReceivedHandler((object) this, e1);
    }

    private void receiver_OnError(object sender, Exception e)
    {
      this._receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError -= new EventHandler<Exception>(this.receiver_OnError);
      this._receiver.StopRead();
      WalkByTestManager.IsWalkByTestStarted = false;
      this.ShowMessage(MessageTypeEnum.Warning, e.Message);
      System.EventHandler errorReceivedHandler = this.OnErrorReceivedHandler;
      if (errorReceivedHandler == null)
        return;
      errorReceivedHandler((object) this, (EventArgs) null);
    }

    public bool StartReadingValues(StructureNodeDTO selectedStructureNode, ProfileType profileType)
    {
      this._profileType = profileType;
      this._gmmMeters = new List<ZENNER.CommonLibrary.Entities.Meter>();
      this._gmmMeters = GMMHelper.GetGMMMetersFromStructureNodeDTO(selectedStructureNode, out this._meterDTOsInStructure);
      EquipmentModel defaultEquipment = MSS.Business.Utils.AppContext.Current.DefaultEquipment;
      DeviceModel deviceModel = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice, TransceiverType.Receiver).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (s => s.Name == "Radio3"));
      if (defaultEquipment == null)
        return false;
      if (deviceModel != null)
      {
        List<ProfileType> profileTypes = GmmInterface.DeviceManager.GetProfileTypes(deviceModel, defaultEquipment);
        if (profileTypes != null)
        {
          this._profileType = profileTypes.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (p => p.Name == "WalkBy Radio3"));
          if (this._profileType != null)
          {
            this._receiver.StartRead(deviceModel, this._gmmMeters, defaultEquipment, this._profileType);
            WalkByTestManager.IsWalkByTestStarted = true;
          }
        }
      }
      return true;
    }

    public void StopReadingValues()
    {
      this._receiver.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.receiver_ValueIdentSetReceived);
      this._receiver.OnError -= new EventHandler<Exception>(this.receiver_OnError);
      EventPublisher.Publish<ProgressEvent>(new ProgressEvent()
      {
        Value = 0
      }, (object) this);
      this._receiver.StopRead();
      WalkByTestManager.IsWalkByTestStarted = false;
    }

    private void ShowMessage(MessageTypeEnum messageType, string messageText)
    {
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
      {
        MessageType = messageType,
        MessageText = messageText
      };
      EventPublisher.Publish<ActionSyncFinished>(new ActionSyncFinished()
      {
        Message = message
      }, (object) this);
    }

    public static bool IsWalkByTestStarted
    {
      get => WalkByTestManager._isWalkByTestStarted;
      set
      {
        WalkByTestManager._isWalkByTestStarted = value;
        WalkByTestManager.OnStaticPropertyChanged(nameof (IsWalkByTestStarted));
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

    private static void OnStaticPropertyChanged(string propertyName)
    {
      EventHandler<PropertyChangedEventArgs> staticPropertyChanged = WalkByTestManager.StaticPropertyChanged;
      if (staticPropertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      staticPropertyChanged((object) null, e);
    }
  }
}
