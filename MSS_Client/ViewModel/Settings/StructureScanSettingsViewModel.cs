// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.StructureScanSettingsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.Utils;
using MSS.Core.Model.Structures;
using MSS.Interfaces;
using MSS_Client.ViewModel.Settings.Selector;
using MVVM.Commands;
using MVVM.Converters;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  public class StructureScanSettingsViewModel : ValidationViewModelBase
  {
    private readonly StructureNodeEquipmentSettings _equipmentSettings;
    private EquipmentSelector _equipmentSelector;
    private bool _isValid;
    private bool _scanParametersVisibility;
    private object[] _selectionChanged;
    private List<DeviceModel> _systemCollection;
    private DeviceModel _selectedSystem;
    private ProfileType _selectedScanMode;
    private List<Config> _tagScanConfigValues;

    public EquipmentSelector EquipmentSelectorProperty
    {
      get => this._equipmentSelector;
      set
      {
        this._equipmentSelector = value;
        this.OnPropertyChanged(nameof (EquipmentSelectorProperty));
      }
    }

    [Inject]
    public StructureScanSettingsViewModel(StructureNodeEquipmentSettings equipmentSettings)
    {
      this._equipmentSettings = equipmentSettings;
      this._equipmentSelector = !string.IsNullOrEmpty(this._equipmentSettings.EquipmentName) ? new EquipmentSelector(this._equipmentSettings.EquipmentName, this._equipmentSettings.EquipmentParams) : new EquipmentSelector(MSS.Business.Utils.AppContext.Current.DefaultEquipment);
      this.SystemCollection = this.SelectedEquipmentModel != null ? GmmInterface.DeviceManager.GetDeviceModels(this.SelectedEquipmentModel, new DeviceModelTags?(DeviceModelTags.SystemDevice)) : GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice);
      string system = this._equipmentSettings?.SystemName;
      if (!string.IsNullOrEmpty(system))
        this.SelectedSystem = this.SystemCollection.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (e => e.Name == system));
      if (this.SelectedSystem != null && this.SelectedEquipmentModel != null)
        this.ScanModeCollection = GmmInterface.DeviceManager.GetProfileTypes(this.SelectedSystem, this.SelectedEquipmentModel, new ProfileTypeTags?(ProfileTypeTags.Scanning)).OrderBy<ProfileType, string>((Func<ProfileType, string>) (p => p.Name)).ToList<ProfileType>();
      string scanModeName = equipmentSettings?.ScanMode;
      if (string.IsNullOrEmpty(scanModeName) || this.ScanModeCollection == null)
        return;
      this.SelectedScanMode = this.ScanModeCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (e => e.Name == scanModeName));
      this.ScanConfigsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.DeserializeChangeableParams(equipmentSettings.ScanParams);
      this.ScanParametersVisibility = this.ScanConfigsList != null;
    }

    public bool ScanParametersVisibility
    {
      get => this._scanParametersVisibility;
      set
      {
        this._scanParametersVisibility = value;
        this.OnPropertyChanged(nameof (ScanParametersVisibility));
      }
    }

    [Required(ErrorMessage = "EQUIPMENT_GROUP_SELECTION_REQUIRED")]
    public EquipmentGroup SelectedEquipmentGroup
    {
      get => this.EquipmentSelectorProperty.SelectedEquipmentGroup;
      set
      {
        this.EquipmentSelectorProperty.SelectedEquipmentGroup = value;
        this.OnPropertyChanged("SystemCollection");
        this.OnPropertyChanged("ScanModeCollection");
      }
    }

    [Required(ErrorMessage = "EQUIPMENT_MODEL_SELECTION_REQUIRED")]
    public EquipmentModel SelectedEquipmentModel
    {
      get => this.EquipmentSelectorProperty.SelectedEquipmentModel;
      set
      {
        this.SelectedSystem = (DeviceModel) null;
        if (MSS.Business.Utils.AppContext.Current.DefaultEquipment == null || value != null)
          this.SystemCollection = GmmInterface.DeviceManager.GetDeviceModels(value, new DeviceModelTags?(DeviceModelTags.SystemDevice));
        this.EquipmentSelectorProperty.SelectedEquipmentModel = value;
      }
    }

    public object[] SelectionChanged
    {
      get => this._selectionChanged;
      set
      {
        if (this._selectionChanged == value)
          return;
        this._selectionChanged = value;
        this.OnPropertyChanged(nameof (SelectionChanged));
      }
    }

    public List<DeviceModel> SystemCollection
    {
      get => this._systemCollection;
      set
      {
        this._systemCollection = value;
        this.OnPropertyChanged(nameof (SystemCollection));
      }
    }

    [Required(ErrorMessage = "SYSTEM_MODEL_SELECTION_REQUIRED")]
    public DeviceModel SelectedSystem
    {
      get => this._selectedSystem;
      set
      {
        this._selectedSystem = value;
        this.ScanParametersVisibility = false;
        if (this.SelectedSystem != null && this.SelectedEquipmentModel != null)
          this.ScanModeCollection = GmmInterface.DeviceManager.GetProfileTypes(this.SelectedSystem, this.SelectedEquipmentModel, new ProfileTypeTags?(ProfileTypeTags.Scanning)).OrderBy<ProfileType, string>((Func<ProfileType, string>) (p => p.Name)).ToList<ProfileType>();
        else
          this.SelectedScanMode = (ProfileType) null;
        this.OnPropertyChanged(nameof (SelectedSystem));
        this.OnPropertyChanged("ScanModeCollection");
      }
    }

    public List<ProfileType> ScanModeCollection { get; set; }

    [Required(ErrorMessage = "SCAN_MODE_SELECTION_REQUIRED")]
    public ProfileType SelectedScanMode
    {
      get => this._selectedScanMode;
      set
      {
        this._selectedScanMode = value;
        this.ScanConfigsList = (List<Config>) null;
        this.ScanParametersVisibility = false;
        if (this._selectedScanMode != null && this._selectedScanMode?.ChangeableParameters != null)
        {
          this.ScanParametersVisibility = true;
          this.ScanConfigsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.CreateScanConfigsList(this._selectedScanMode.ChangeableParameters, new List<string>()
          {
            this.EquipmentSelectorProperty.PortLabel
          });
        }
        EventPublisher.Publish<ScanModeConfigEvent>(new ScanModeConfigEvent()
        {
          ScanModeConfigValues = this.ScanConfigsList
        }, (IViewModel) this);
        this.OnPropertyChanged(nameof (SelectedScanMode));
      }
    }

    public List<Config> ScanConfigsList { get; set; }

    public List<Config> TagScanConfigValues
    {
      get => this._tagScanConfigValues;
      set
      {
        this._tagScanConfigValues = value;
        this.OnPropertyChanged(nameof (TagScanConfigValues));
      }
    }

    public System.Windows.Input.ICommand SaveScanSettingsCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._isValid = true;
          this.ValidateProperty("SelectedEquipmentModel");
          this.ValidateProperty("SelectedSystem");
          this.ValidateProperty("SelectedScanMode");
          if (!this._isValid)
            return;
          MSS.Business.Utils.AppContext.Current.DefaultEquipment = this.SelectedEquipmentModel;
          CustomMultiBindingConverter.FindCommandParameters commandParameters = parameter as CustomMultiBindingConverter.FindCommandParameters;
          this._equipmentSettings.EquipmentName = this.SelectedEquipmentModel != null ? this.SelectedEquipmentModel.Name : string.Empty;
          this._equipmentSettings.EquipmentParams = this.SelectedEquipmentModel != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializeEquipementParams(this.SelectedEquipmentModel) : string.Empty;
          this._equipmentSettings.SystemName = this.SelectedSystem != null ? this.SelectedSystem.Name : string.Empty;
          DynamicGridControl.SetConfigurationParameters(commandParameters.Property1, this.TagScanConfigValues);
          ChangeableParameter changeableParam = this.SelectedEquipmentModel?.ChangeableParameters.Find((Predicate<ChangeableParameter>) (param => param.Key == this.EquipmentSelectorProperty.PortLabel));
          ChangeableParameter changeableParameter = this.SelectedScanMode == null || this.SelectedScanMode.ChangeableParameters == null ? (ChangeableParameter) null : this.SelectedScanMode.ChangeableParameters.Find((Predicate<ChangeableParameter>) (param => param.Key == this.EquipmentSelectorProperty.PortLabel));
          if (changeableParam != null && changeableParameter != null)
          {
            changeableParameter.Value = changeableParam.Value;
            this.TagScanConfigValues.Add(MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigFromChangeableParameters(changeableParam));
          }
          this._equipmentSettings.ScanMode = this.SelectedScanMode != null ? this.SelectedScanMode.Name : string.Empty;
          this._equipmentSettings.ScanParams = this.SelectedScanMode != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(this.TagScanConfigValues, this.SelectedScanMode.ChangeableParameters) : string.Empty;
          EventPublisher.Publish<ExpandoObject>(new ExpandoObject(), (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<EquipmentModel>((Expression<Func<EquipmentModel>>) (() => this.SelectedEquipmentModel));
      if (propertyName == propertyName1 && this.SelectedEquipmentModel == null)
      {
        this._isValid = false;
        return base.ValidateProperty(propertyName);
      }
      string propertyName2 = this.GetPropertyName<DeviceModel>((Expression<Func<DeviceModel>>) (() => this.SelectedSystem));
      if (propertyName == propertyName2 && this.SelectedSystem == null)
      {
        this._isValid = false;
        return base.ValidateProperty(propertyName);
      }
      string propertyName3 = this.GetPropertyName<ProfileType>((Expression<Func<ProfileType>>) (() => this.SelectedScanMode));
      if (!(propertyName == propertyName3) || this.SelectedScanMode != null)
        return base.ValidateProperty(propertyName);
      this._isValid = false;
      return base.ValidateProperty(propertyName);
    }
  }
}
