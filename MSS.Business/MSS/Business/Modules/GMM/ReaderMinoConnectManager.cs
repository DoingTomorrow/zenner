// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.ReaderMinoConnectManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Events;
using MSS.Business.Modules.GMMWrapper;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DTO.MessageHandler;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class ReaderMinoConnectManager : BaseMinoConnectManager
  {
    private readonly MeterReaderManager _reader;
    private ObservableCollection<ExecuteOrderStructureNode> _meters;
    private IDeviceManager _deviceManager;
    private const string IR_MINOMAT_V4 = "IR Minomat V4";
    private List<string> metersSerialNumberRead;
    private List<string> metersSerialNumberWithoutValues = new List<string>();
    public EventHandler<string> OnMissingTranslationRule;
    public System.EventHandler OnReadingFinished;
    public EventHandler<string> OnError;
    public System.EventHandler OnBatterieLow;

    public ReaderMinoConnectManager(
      IRepositoryFactory repositoryFactory,
      IDeviceManager deviceManager)
    {
      this._deviceManager = deviceManager;
      this._reader = GmmInterface.Reader;
      this._reader.DontCloseConnectionAfterRead = true;
      this._reader.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.Reader_ValueIdentSetReceived);
      this._reader.OnError += new EventHandler<Exception>(this.Reader_OnError);
      this._reader.OnProgress += new EventHandler<int>(this._reader_OnProgress);
      this._reader.OnReadFinished += new EventHandler<ReadSettings>(this._reader_OnReadFinished);
      this._reader.StoreResultsToDatabase = true;
      this._reader.BatterieLow += new System.EventHandler(this.Reader_BatterieLow);
      TranslationRulesManager.Instance.MissedTranslationRules += new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
      this._repositoryFactory = repositoryFactory;
      this._orderRepository = this._repositoryFactory.GetRepository<Order>();
    }

    private void ReinitializeParams(
      ObservableCollection<ExecuteOrderStructureNode> meterList,
      OrderDTO order,
      ProfileType profileType)
    {
      this._meters = meterList;
      this._orderId = order.Id;
      this._profileType = profileType;
    }

    ~ReaderMinoConnectManager()
    {
      this._reader.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.Reader_ValueIdentSetReceived);
      this._reader.OnError -= new EventHandler<Exception>(this.Reader_OnError);
      this._reader.OnProgress -= new EventHandler<int>(this._reader_OnProgress);
      this._reader.OnReadFinished -= new EventHandler<ReadSettings>(this._reader_OnReadFinished);
      this._reader.BatterieLow -= new System.EventHandler(this.Reader_BatterieLow);
      this._reader.CloseConnection();
      TranslationRulesManager.Instance.MissedTranslationRules -= new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
    }

    private void TranslationRulesManager_MissedTranslationRules(object sender, string e)
    {
      string parameter = ParameterService.GetParameter(e, "SID");
      if (this.metersSerialNumberRead.Contains(parameter))
        return;
      this.metersSerialNumberRead.Add(parameter);
      this.SaveErrorMessage(string.Format(Resources.MSS_ReaderManager_MissingTranslationRule, (object) parameter), MessageLevelsEnum.Warning, parameter);
      EventPublisher.Publish<ErrorDuringReading>(new ErrorDuringReading()
      {
        SerialNumber = parameter,
        ErrorMessage = ReadingValueStatusEnum.MissingTranslationRules.ToString()
      }, (object) this);
      EventHandler<string> missingTranslationRule = this.OnMissingTranslationRule;
      if (missingTranslationRule != null)
        missingTranslationRule((object) this, string.Format(Resources.MSS_ReaderManager_MissingTranslationRule, (object) parameter));
    }

    private void _reader_OnProgress(object sender, int e) => this.UpdateProgressBar(e);

    private void _reader_OnReadFinished(object sender, ReadSettings e)
    {
      this.StopReadingValues();
      System.EventHandler onReadingFinished = this.OnReadingFinished;
      if (onReadingFinished == null)
        return;
      onReadingFinished((object) this, (EventArgs) null);
    }

    private void UpdateProgressBar(int progress)
    {
      if (this._meters.Count <= 0)
        return;
      if (this._profileType.Name == "IR Minomat V4" && this._meters.Count<ExecuteOrderStructureNode>() > 1 && this.metersSerialNumberRead.Count > 0)
      {
        int num = this.metersSerialNumberRead.Count * 100 / this._meters.Count;
        EventPublisher.Publish<ProgressEvent>(new ProgressEvent()
        {
          Value = Convert.ToInt32(num)
        }, (object) this);
      }
      else
        EventPublisher.Publish<ProgressEvent>(new ProgressEvent()
        {
          Value = progress
        }, (object) this);
    }

    private void Reader_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      if (e.AvailableValues == null || e.AvailableValues.Count == 0)
        return;
      ExecuteOrderStructureNode orderStructureNode = this._meters.FirstOrDefault<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (m => m.SerialNumber == e.SerialNumber));
      Guid guid = Guid.Empty;
      if (orderStructureNode != null)
        guid = orderStructureNode.MeterId;
      bool flag = this.SaveReadingValues(e.SerialNumber);
      if (flag)
      {
        EventPublisher.Publish<StructureBytesUpdated>(new StructureBytesUpdated()
        {
          SerialNumberRead = e.SerialNumber,
          AnyReadingValuesRead = flag,
          IsConfigured = true
        }, (object) this);
        if (this.metersSerialNumberWithoutValues.Contains(e.SerialNumber))
          this.metersSerialNumberWithoutValues.Remove(e.SerialNumber);
      }
      else if (!this.metersSerialNumberWithoutValues.Contains(e.SerialNumber))
        this.metersSerialNumberWithoutValues.Add(e.SerialNumber);
      if (this.metersSerialNumberRead.Contains(e.SerialNumber))
        return;
      this.metersSerialNumberRead.Add(e.SerialNumber);
      this.UpdateProgressBar(this.metersSerialNumberRead.Count);
    }

    private void Reader_OnError(object sender, Exception e)
    {
      if (e == null)
        return;
      if (e is InvalidMeterException)
      {
        string serialNumber = (e as InvalidMeterException).Meter.SerialNumber;
        string str = string.Format(Resources.MSS_InvalidMeter, (object) serialNumber);
        if (serialNumber != string.Empty)
          this.SaveErrorMessage(str, MessageLevelsEnum.Error, serialNumber);
        if (!this.metersSerialNumberRead.Contains(serialNumber))
        {
          this.metersSerialNumberRead.Add(serialNumber);
          EventPublisher.Publish<ErrorDuringReading>(new ErrorDuringReading()
          {
            SerialNumber = serialNumber
          }, (object) this);
          EventHandler<string> onError = this.OnError;
          if (onError != null)
            onError(sender, str);
        }
      }
      if (e is FailedToReadException)
      {
        string serialnumber = (e as FailedToReadException).Serialnumber;
        string str = string.Format(Resources.MSS_FailedToRead, (object) serialnumber);
        if (serialnumber != string.Empty)
          this.SaveErrorMessage(str, MessageLevelsEnum.Error, serialnumber);
        if (!this.metersSerialNumberRead.Contains(serialnumber) || this.metersSerialNumberWithoutValues.Contains(serialnumber))
        {
          if (!this.metersSerialNumberRead.Contains(serialnumber))
            this.metersSerialNumberRead.Add(serialnumber);
          EventPublisher.Publish<ErrorDuringReading>(new ErrorDuringReading()
          {
            SerialNumber = serialnumber
          }, (object) this);
          EventHandler<string> onError = this.OnError;
          if (onError != null)
            onError(sender, str);
        }
      }
    }

    private void Reader_BatterieLow(object sender, EventArgs e)
    {
      this.ShowMessage(MessageTypeEnum.Warning, Resources.MSS_MinoConnect_Battery_Low);
      System.EventHandler onBatterieLow = this.OnBatterieLow;
      if (onBatterieLow == null)
        return;
      onBatterieLow((object) this, e);
    }

    public bool StartReadingValues(
      ObservableCollection<ExecuteOrderStructureNode> meterList,
      OrderDTO order,
      ProfileType profileName,
      EquipmentModel equipment,
      string scanParams = null)
    {
      return this.StartReadingValues(meterList, order, profileName, equipment, (Dictionary<Guid, StructureNodeEquipmentSettings>) null, scanParams);
    }

    public bool StartReadingValues(
      ObservableCollection<ExecuteOrderStructureNode> meterList,
      OrderDTO order,
      ProfileType profileName,
      EquipmentModel equipment,
      Dictionary<Guid, StructureNodeEquipmentSettings> equipmentSettingsForMeters,
      string scanParams = null)
    {
      if (meterList.Count == 0)
        return false;
      this.ReinitializeParams(meterList, order, profileName);
      List<ZENNER.CommonLibrary.Entities.Meter> meters = GMMHelper.GetMeters(this._meters, this.GetGMMManagerInstance().GetFilterListForOrder(this._orderId), order.StructureType, scanParams);
      if (this._profileType != null)
      {
        this.metersSerialNumberRead = new List<string>();
        foreach (ZENNER.CommonLibrary.Entities.Meter meterZ in meters)
        {
          this.UpdateEquipmentSettings(meterZ, meterList, equipmentSettingsForMeters);
          this.UpdateConnectionAdjuster(meterZ, profileName, equipment);
          this._reader.ReadMeter(meters, equipment, this._profileType);
        }
      }
      return true;
    }

    public void UpdateEquipmentSettings(
      ZENNER.CommonLibrary.Entities.Meter meterZ,
      ObservableCollection<ExecuteOrderStructureNode> meterList,
      Dictionary<Guid, StructureNodeEquipmentSettings> equipmentSettingsForMeters)
    {
      if (equipmentSettingsForMeters == null || equipmentSettingsForMeters.Count == 0)
        return;
      Guid? meterId = meterList.FirstOrDefault<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (item => item.SerialNumber == meterZ.SerialNumber))?.MeterId;
      StructureNodeEquipmentSettings settingsForMeter = !meterId.HasValue || !equipmentSettingsForMeters.ContainsKey(meterId.Value) ? (StructureNodeEquipmentSettings) null : equipmentSettingsForMeters[meterId.Value];
      if (settingsForMeter == null)
        return;
      meterZ.DeviceModel = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateDeviceModelWithSavedParams(meterZ.DeviceModel, settingsForMeter.DeviceModelReadingParams);
    }

    public void UpdateConnectionAdjuster(
      ZENNER.CommonLibrary.Entities.Meter meterZ,
      ProfileType profileName,
      EquipmentModel equipment)
    {
      meterZ.ConnectionAdjuster = this._deviceManager.GetConnectionAdjuster(meterZ.DeviceModel, equipment, profileName);
    }

    public void StopReadingValues()
    {
      this._reader.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.Reader_ValueIdentSetReceived);
      this._reader.OnError -= new EventHandler<Exception>(this.Reader_OnError);
      this._reader.OnProgress -= new EventHandler<int>(this._reader_OnProgress);
      this._reader.OnReadFinished -= new EventHandler<ReadSettings>(this._reader_OnReadFinished);
      this._reader.CancelRead();
    }

    protected GMMManager GetGMMManagerInstance() => new GMMManager(this._repositoryFactory);

    public void CloseConnection() => this._reader.CloseConnection();
  }
}
