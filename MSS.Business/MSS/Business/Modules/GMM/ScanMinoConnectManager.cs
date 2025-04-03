// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.GMM.ScanMinoConnectManager
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Modules.GMMWrapper;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Structures;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.Business.Modules.GMM
{
  public class ScanMinoConnectManager : BaseMinoConnectManager
  {
    private readonly ScannerManager _scanner;
    private readonly IRepository<ApplicationParameter> _appParamRepository;
    private readonly StructureNodeEquipmentSettings _structureNodeEquipmentSettings;
    public EventHandler<string> OnMissingTranslationRule;
    public System.EventHandler OnBatterieLow;
    private string _systemParam;
    private readonly IDeviceManager _deviceManager;
    private static bool _isScanningStarted;

    public event EventHandler<int> OnProgressChanged;

    public event EventHandler<string> OnProgressMessage;

    public event EventHandler<ZENNER.CommonLibrary.Entities.Meter> OnMeterFound;

    public string SystemName => this._systemParam;

    public ScanMinoConnectManager(
      IRepositoryFactory repositoryFactory,
      StructureNodeEquipmentSettings structureNodeEquipmentSettings,
      IDeviceManager deviceManager)
    {
      this._scanner = GmmInterface.ScannerManager;
      this._deviceManager = deviceManager;
      this._structureNodeEquipmentSettings = structureNodeEquipmentSettings;
      this._appParamRepository = repositoryFactory.GetRepository<ApplicationParameter>();
      this._scanner.OnMeterFound += new EventHandler<ZENNER.CommonLibrary.Entities.Meter>(this._scanner_OnMeterFound);
      this._scanner.OnError += new EventHandler<Exception>(this._scanner_OnError);
      this._scanner.OnProgress += new EventHandler<int>(this._scanner_OnProgress);
      this._scanner.OnProgressMessage += new EventHandler<string>(this._scanner_OnProgressMessage);
      this._scanner.BatterieLow += new System.EventHandler(this.BatterieLow);
      TranslationRulesManager.Instance.MissedTranslationRules += new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
    }

    private void BatterieLow(object sender, EventArgs e)
    {
      this.ShowMessage(MessageTypeEnum.Warning, Resources.MSS_MinoConnect_Battery_Low);
      System.EventHandler onBatterieLow = this.OnBatterieLow;
      if (onBatterieLow == null)
        return;
      onBatterieLow((object) this, e);
    }

    public ScanMinoConnectManager()
    {
    }

    ~ScanMinoConnectManager()
    {
      if (this._scanner == null)
        return;
      this._scanner.OnMeterFound -= new EventHandler<ZENNER.CommonLibrary.Entities.Meter>(this._scanner_OnMeterFound);
      this._scanner.OnError -= new EventHandler<Exception>(this._scanner_OnError);
      this._scanner.OnProgress -= new EventHandler<int>(this._scanner_OnProgress);
      this._scanner.OnProgressMessage -= new EventHandler<string>(this._scanner_OnProgressMessage);
      TranslationRulesManager.Instance.MissedTranslationRules -= new EventHandlerEx<string>(this.TranslationRulesManager_MissedTranslationRules);
      this._scanner.CancelScan();
    }

    private void _scanner_OnProgressMessage(object sender, string e)
    {
      EventHandler<string> onProgressMessage = this.OnProgressMessage;
      if (onProgressMessage == null)
        return;
      onProgressMessage((object) this, e);
    }

    private void _scanner_OnProgress(object sender, int e)
    {
      EventHandler<int> onProgressChanged = this.OnProgressChanged;
      if (e == 100 && string.IsNullOrEmpty(this._systemParam) && this._systemParam == "M-Bus")
        MSS.Business.Errors.MessageHandler.LogDebug("M-Bus scanning - 100% - automatically stop scanning");
      if (onProgressChanged == null)
        return;
      onProgressChanged((object) this, e);
    }

    private void _scanner_OnError(object sender, Exception e)
    {
      MSS.Business.Errors.MessageHandler.LogException(e);
      this._scanner.OnMeterFound -= new EventHandler<ZENNER.CommonLibrary.Entities.Meter>(this._scanner_OnMeterFound);
      this._scanner.OnError -= new EventHandler<Exception>(this._scanner_OnError);
      this._scanner.CancelScan();
      this.ShowMessage(MessageTypeEnum.Warning, e.Message);
    }

    private void _scanner_OnMeterFound(object sender, ZENNER.CommonLibrary.Entities.Meter e)
    {
      MSS.Business.Errors.MessageHandler.LogDebug(string.Format("Scanning - Meter found: {0}, {1}", (object) (e.SerialNumber ?? string.Empty), (object) (e.DeviceModel.Name ?? string.Empty)));
      EventHandler<ZENNER.CommonLibrary.Entities.Meter> onMeterFound = this.OnMeterFound;
      if (onMeterFound == null)
        return;
      onMeterFound((object) this, e);
    }

    public bool StartScan()
    {
      MSS.Business.Errors.MessageHandler.LogDebug("Start scanning");
      EquipmentModel equipment = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(this._deviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (d => d.Name == this._structureNodeEquipmentSettings.EquipmentName)), this._structureNodeEquipmentSettings.EquipmentParams);
      if (equipment == null)
        return false;
      string scanMode = this._structureNodeEquipmentSettings.ScanMode;
      if (string.IsNullOrEmpty(scanMode))
      {
        MSS.Business.Errors.MessageHandler.LogDebug("Scanning - scanMode is not set");
        return false;
      }
      this._appParamRepository.Refresh((object) this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "System")).Id);
      this._systemParam = this._structureNodeEquipmentSettings.SystemName;
      if (string.IsNullOrEmpty(this._systemParam))
      {
        MSS.Business.Errors.MessageHandler.LogDebug("Scanning - systemParam is not set");
        return false;
      }
      DeviceModel system = this._deviceManager.GetDeviceModels(DeviceModelTags.SystemDevice).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (e => e.Name == this._systemParam));
      ProfileType profileType = this.LoadProfileType(equipment, system, scanMode);
      this._scanner.BeginScan(equipment, system, profileType);
      return true;
    }

    private ProfileType LoadProfileType(
      EquipmentModel equipment,
      DeviceModel system,
      string scanModeName)
    {
      ProfileType profileType = this._deviceManager.GetProfileTypes(system, equipment).FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (e => e.Name == scanModeName));
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(profileType, this._structureNodeEquipmentSettings.ScanParams);
      return profileType;
    }

    public void StopScan()
    {
      this._scanner.OnMeterFound -= new EventHandler<ZENNER.CommonLibrary.Entities.Meter>(this._scanner_OnMeterFound);
      this._scanner.OnError -= new EventHandler<Exception>(this._scanner_OnError);
      this._scanner.OnProgress -= new EventHandler<int>(this._scanner_OnProgress);
      this._scanner.OnProgressMessage -= new EventHandler<string>(this._scanner_OnProgressMessage);
      this._scanner.CancelScan();
    }

    public static bool IsScanningStarted
    {
      get => ScanMinoConnectManager._isScanningStarted;
      set
      {
        ScanMinoConnectManager._isScanningStarted = value;
        ScanMinoConnectManager.OnStaticPropertyChanged(nameof (IsScanningStarted));
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

    private static void OnStaticPropertyChanged(string propertyName)
    {
      EventHandler<PropertyChangedEventArgs> staticPropertyChanged = ScanMinoConnectManager.StaticPropertyChanged;
      if (staticPropertyChanged == null)
        return;
      PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
      staticPropertyChanged((object) null, e);
    }

    private void TranslationRulesManager_MissedTranslationRules(object sender, string e)
    {
      string parameter = ParameterService.GetParameter(e, "SID");
      EventHandler<string> missingTranslationRule = this.OnMissingTranslationRule;
      if (missingTranslationRule == null)
        return;
      missingTranslationRule((object) this, parameter);
    }
  }
}
