// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.DeviceReaderManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using GmmDbLib.DataSets;
using MSS.Business.Modules.OrdersManagement;
using MSS.Core.Model.Meters;
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
  public class DeviceReaderManager
  {
    private readonly MeterReaderManager _reader;
    private readonly IRepositoryFactory _repositoryFactory;
    public EventHandler<List<MeterReadingValue>> OnMeterValuesReceivedHandler;
    public EventHandler<Exception> OnErrorReceivedHandler;
    public EventHandler<string> OnMissingTranslationRule;
    public System.EventHandler OnReadingFinished;
    private static bool _isDeviceReadingStarted;

    public DeviceReaderManager(IRepositoryFactory repositoryFactory, bool storeResultsToDatabase = false)
    {
      this._repositoryFactory = repositoryFactory;
      this._reader = GmmInterface.Reader;
      this._reader.StoreResultsToDatabase = storeResultsToDatabase;
      this._reader.ValueIdentSetReceived += new EventHandler<ValueIdentSet>(this.reader_ValueIdentSetReceived);
      this._reader.OnError += new EventHandler<Exception>(this.reader_OnError);
      DeviceReaderManager.IsDeviceReadingStarted = true;
      TranslationRulesManager.Instance.MissedTranslationRules += new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
    }

    ~DeviceReaderManager() => this.StopReadingValues();

    private void reader_ValueIdentSetReceived(object sender, ValueIdentSet e)
    {
      this.StopReadingValues();
      List<MeterReadingValue> readingValues = ReadingValuesHelper.ConvertValueIdentToReadingValues(e);
      EventHandler<List<MeterReadingValue>> valuesReceivedHandler = this.OnMeterValuesReceivedHandler;
      if (valuesReceivedHandler == null)
        return;
      valuesReceivedHandler((object) this, readingValues);
    }

    private void reader_OnError(object sender, Exception e)
    {
      this.StopReadingValues();
      EventHandler<Exception> errorReceivedHandler = this.OnErrorReceivedHandler;
      if (errorReceivedHandler == null)
        return;
      errorReceivedHandler(sender, e);
    }

    private void TranslationRulesManager_MissedTranslationRules(object sender, string e)
    {
      EventHandler<string> missingTranslationRule = this.OnMissingTranslationRule;
      if (missingTranslationRule == null)
        return;
      missingTranslationRule((object) this, string.Format(Resources.MSS_ReaderManager_MissingTranslationRule, (object) e));
    }

    public void StartReadingValues(
      ZENNER.CommonLibrary.Entities.Meter meter,
      EquipmentModel equipmentModel,
      ProfileType profileType)
    {
      this._reader.ReadMeter(meter, equipmentModel, profileType);
      DeviceReaderManager.IsDeviceReadingStarted = true;
    }

    public void StopReadingValues()
    {
      this._reader.ValueIdentSetReceived -= new EventHandler<ValueIdentSet>(this.reader_ValueIdentSetReceived);
      this._reader.OnError -= new EventHandler<Exception>(this.reader_OnError);
      this._reader.CloseConnection();
      DeviceReaderManager.IsDeviceReadingStarted = false;
      TranslationRulesManager.Instance.MissedTranslationRules -= new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
    }

    public static bool IsDeviceReadingStarted
    {
      get => DeviceReaderManager._isDeviceReadingStarted;
      set
      {
        DeviceReaderManager._isDeviceReadingStarted = value;
        DeviceReaderManager.OnStaticPropertyChanged("IsWalkByTestStarted");
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

    private static void OnStaticPropertyChanged(string propertyName)
    {
      EventHandler<PropertyChangedEventArgs> staticPropertyChanged = DeviceReaderManager.StaticPropertyChanged;
      if (staticPropertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      staticPropertyChanged((object) null, e);
    }

    private MeterReadingValue ConvertReadingValues(
      string serialNumber,
      MSS.Core.Model.Meters.Meter mssMeter,
      List<MeasureUnit> meaureUnits,
      DriverTables.MeterValuesMSSRow gmmMeterValue)
    {
      MeterReadingValue meterReadingValue = new MeterReadingValue();
      meterReadingValue.CreatedOn = DateTime.Now;
      meterReadingValue.Date = gmmMeterValue.TimePoint;
      meterReadingValue.MeterSerialNumber = serialNumber;
      meterReadingValue.Value = gmmMeterValue.Value;
      meterReadingValue.ValueId = Convert.ToInt64(ValueIdent.GetValueIdent(gmmMeterValue.ValueIdentIndex, gmmMeterValue.PhysicalQuantity, gmmMeterValue.MeterType, gmmMeterValue.Calculation, gmmMeterValue.CalculationStart, gmmMeterValue.StorageInterval, gmmMeterValue.Creation));
      meterReadingValue.MeterId = mssMeter != null ? mssMeter.Id : Guid.Empty;
      string unitName = ValueIdent.GetUnit(Convert.ToInt64(meterReadingValue.ValueId));
      MeasureUnit measureUnit = meaureUnits.FirstOrDefault<MeasureUnit>((Func<MeasureUnit, bool>) (m => m.Code == unitName));
      if (measureUnit == null && !string.IsNullOrEmpty(unitName))
      {
        measureUnit = new MeasureUnit() { Code = unitName };
        meaureUnits.Add(measureUnit);
      }
      if (measureUnit != null)
        meterReadingValue.Unit = measureUnit;
      long valueId = meterReadingValue.ValueId;
      ValueIdent.ValueIdPart_StorageInterval partStorageInterval = ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId);
      meterReadingValue.StorageInterval = (long) partStorageInterval;
      meterReadingValue.PhysicalQuantity = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId);
      meterReadingValue.MeterType = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId);
      meterReadingValue.CalculationStart = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId);
      meterReadingValue.Creation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId);
      meterReadingValue.Calculation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId);
      return meterReadingValue;
    }
  }
}
