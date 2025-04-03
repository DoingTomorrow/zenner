// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.TranslationRulesViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class TranslationRulesViewModel : ValidationViewModelBase
  {
    private bool _isEditMode;
    private bool _isItemSelected;
    private bool? _isNewRule;
    private StructureNodeDTO _selectedNode;
    private bool _isSubDevice;
    private string _receivedManufacturer;
    private string _receivedMedium;
    private string _receivedGeneration;
    private List<string> _receivedZdfKeys;
    private IWindowFactory _windowFactory;
    private TranslationRule _selectedItem;
    private TranslationRuleCollection _translationRules;
    private string _manufacturer;
    private string _medium;
    private Dictionary<DeviceMediumEnum, string> _deviceMediumList;
    private int? _versionMin;
    private int? _versionMax;
    private string _zdfKey;
    private List<string> _zdfKeyList;
    private bool _isMainChecked;
    private bool _isSubDeviceChecked;
    private double _multiplier;
    private int _ruleOrder;
    private string _specialTranslation;
    private List<string> _specialTranslationsList;
    private int _subDeviceIndex;
    private string _subDeviceZDFKey;
    private bool _isDeleteButtonEnabled;
    private bool _isEditButtonEnabled;
    private bool _isSaveButtonEnabled;
    private bool _isCancelButtonEnabled;
    private bool _isTranslateToGroupEnabled;
    private bool _isValueSettingsGroupEnabled;
    private bool _isSubDeviceGroupEnabled;
    private bool _isZDFKeyEnabled;
    private bool _isDeviceTypeEnabled;
    private string _valueIdentifier;
    private string _unit;
    private string _timepoint;
    private List<string> _timepointList;
    private string _timepointModification;
    private List<string> _timepointModificationList;
    private string _meterType;
    private string _physicalQuantity;
    private string _calculation;
    private string _calculationStart;
    private string _storageInterval;
    private string _creation;
    private int _ruleIndex;
    private ViewModelBase _messageUserControl;
    private bool _isShowAllRulesChecked;

    public TranslationRulesViewModel(
      IWindowFactory windowFactory,
      string manufacturer,
      string medium,
      string generation,
      List<string> zdfKeys,
      StructureNodeDTO selectedNode)
    {
      this._receivedManufacturer = manufacturer;
      this._receivedMedium = medium;
      this._receivedGeneration = generation;
      this._receivedZdfKeys = zdfKeys;
      this._isNewRule = new bool?();
      this._isEditMode = false;
      this._windowFactory = windowFactory;
      this._selectedNode = selectedNode;
      this.InitUI();
      this.IsShowAllRulesChecked = false;
    }

    private void InitUI()
    {
      this._isSubDevice = StructuresHelper.IsMeterWithMeterParent(this._selectedNode);
      int result;
      if (int.TryParse(this._receivedGeneration, out result))
        this._versionMax = this._versionMin = new int?(result);
      this._deviceMediumList = this.InitDeviceMediumList();
      List<string> receivedZdfKeys1 = this._receivedZdfKeys;
      this._zdfKeyList = receivedZdfKeys1 != null ? receivedZdfKeys1.Where<string>((Func<string, bool>) (k => !k.Contains("TIMP"))).ToList<string>() : (List<string>) null;
      this._manufacturer = this._receivedManufacturer;
      this.OnPropertyChanged("Manufacturer");
      this._medium = this._receivedMedium;
      this.OnPropertyChanged("Medium");
      this.IsMainChecked = !this._isSubDevice;
      this.IsSubDeviceChecked = this._isSubDevice;
      List<string> receivedZdfKeys2 = this._receivedZdfKeys;
      this._timepointList = receivedZdfKeys2 != null ? receivedZdfKeys2.Where<string>((Func<string, bool>) (k => k.Contains("TIMP"))).ToList<string>() : (List<string>) null;
      this._timepointList?.Insert(0, "RTIME");
      this._timepointModificationList = this.InitTimepointModificationList();
      List<string> timepointList = this.TimepointList;
      this.Timepoint = timepointList != null ? timepointList.First<string>() : (string) null;
      List<string> modificationList = this.TimepointModificationList;
      this.TimepointModification = modificationList != null ? modificationList.First<string>((Func<string, bool>) (item => item == "None")) : (string) null;
      this._specialTranslationsList = this.InitSpecialTranslationsList();
      this.Multiplier = 1.0;
      this.LoadTranslationRulesList();
      this.SelectedItem = this.TranslationRules.FirstOrDefault<TranslationRule>();
      this._isItemSelected = this.SelectedItem != null;
      if (!this._isItemSelected)
        this.EnableDisableControls();
      this.IsShowAllRulesChecked = true;
    }

    public TranslationRule SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        if (value != null)
        {
          this.LoadTranslationRule(this._selectedItem);
          this._isEditMode = false;
          this._isItemSelected = true;
          this.EnableDisableControls();
        }
        this.OnPropertyChanged(nameof (SelectedItem));
      }
    }

    public TranslationRuleCollection TranslationRules
    {
      get => this._translationRules;
      set
      {
        this._translationRules = value;
        this.OnPropertyChanged(nameof (TranslationRules));
        this.OnPropertyChanged("TranslationRulesCount");
      }
    }

    public string TranslationRulesCount
    {
      get => this._translationRules.Count.ToString() + " " + Resources.MSS_Client_Rules;
    }

    public string Manufacturer
    {
      get => this._manufacturer;
      set
      {
        this._manufacturer = value;
        if (!this._isEditMode)
          this.LoadTranslationRulesList();
        this.OnPropertyChanged(nameof (Manufacturer));
      }
    }

    public string Medium
    {
      get => this._medium;
      set
      {
        this._medium = this.DeviceMediumList.FirstOrDefault<KeyValuePair<DeviceMediumEnum, string>>((Func<KeyValuePair<DeviceMediumEnum, string>, bool>) (item => item.Key.ToString() == value)).Key.ToString();
        if (!this._isEditMode)
          this.LoadTranslationRulesList();
        this.OnPropertyChanged(nameof (Medium));
      }
    }

    public Dictionary<DeviceMediumEnum, string> DeviceMediumList
    {
      get => this._deviceMediumList;
      set
      {
        this._deviceMediumList = value;
        this.OnPropertyChanged(nameof (DeviceMediumList));
      }
    }

    public int? VersionMin
    {
      get => this._versionMin;
      set
      {
        this._versionMin = value;
        if (!this._isEditMode)
          this.LoadTranslationRulesList();
        this.OnPropertyChanged(nameof (VersionMin));
      }
    }

    public int? VersionMax
    {
      get => this._versionMax;
      set
      {
        this._versionMax = value;
        if (!this._isEditMode)
          this.LoadTranslationRulesList();
        this.OnPropertyChanged(nameof (VersionMax));
      }
    }

    [Required(ErrorMessage = "MSS_Client_ZDFKey_Mandatory")]
    public string ZDFKey
    {
      get => this._zdfKey;
      set
      {
        List<string> zdfKeyList = this.ZDFKeyList;
        this._zdfKey = zdfKeyList != null ? zdfKeyList.FirstOrDefault<string>((Func<string, bool>) (item => item == value)) : (string) null;
        this.OnPropertyChanged(nameof (ZDFKey));
      }
    }

    public List<string> ZDFKeyList
    {
      get => this._zdfKeyList;
      set
      {
        this._zdfKeyList = value;
        this.OnPropertyChanged(nameof (ZDFKeyList));
      }
    }

    public bool IsMainChecked
    {
      get => this._isMainChecked;
      set
      {
        this._isMainChecked = value;
        this.OnPropertyChanged(nameof (IsMainChecked));
      }
    }

    public bool IsSubDeviceChecked
    {
      get => this._isSubDeviceChecked;
      set
      {
        this._isSubDeviceChecked = value;
        this.OnPropertyChanged(nameof (IsSubDeviceChecked));
      }
    }

    public double Multiplier
    {
      get => this._multiplier;
      set
      {
        this._multiplier = value;
        this.OnPropertyChanged(nameof (Multiplier));
      }
    }

    public int RuleOrder
    {
      get => this._ruleOrder;
      set
      {
        this._ruleOrder = value;
        this.OnPropertyChanged(nameof (RuleOrder));
      }
    }

    public string SpecialTranslation
    {
      get => this._specialTranslation;
      set
      {
        this._specialTranslation = this.SpecialTranslationsList.FirstOrDefault<string>((Func<string, bool>) (item => item == value));
        this.OnPropertyChanged(nameof (SpecialTranslation));
      }
    }

    public List<string> SpecialTranslationsList
    {
      get => this._specialTranslationsList;
      set
      {
        this._specialTranslationsList = value;
        this.OnPropertyChanged(nameof (SpecialTranslationsList));
      }
    }

    public int SubDeviceIndex
    {
      get => this._subDeviceIndex;
      set
      {
        this._subDeviceIndex = value;
        this.OnPropertyChanged(nameof (SubDeviceIndex));
      }
    }

    public string SubDeviceZDFKey
    {
      get => this._subDeviceZDFKey;
      set
      {
        this._subDeviceZDFKey = value;
        this.OnPropertyChanged(nameof (SubDeviceZDFKey));
      }
    }

    public bool IsDeleteButtonEnabled
    {
      get => this._isDeleteButtonEnabled;
      set
      {
        this._isDeleteButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsDeleteButtonEnabled));
      }
    }

    public bool IsEditButtonEnabled
    {
      get => this._isEditButtonEnabled;
      set
      {
        this._isEditButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsEditButtonEnabled));
      }
    }

    public bool IsSaveButtonEnabled
    {
      get => this._isSaveButtonEnabled;
      set
      {
        this._isSaveButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsSaveButtonEnabled));
      }
    }

    public bool IsCancelButtonEnabled
    {
      get => this._isCancelButtonEnabled;
      set
      {
        this._isCancelButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsCancelButtonEnabled));
      }
    }

    public bool IsTranslateToGroupEnabled
    {
      get => this._isTranslateToGroupEnabled;
      set
      {
        this._isTranslateToGroupEnabled = value;
        this.OnPropertyChanged(nameof (IsTranslateToGroupEnabled));
      }
    }

    public bool IsValueSettingsGroupEnabled
    {
      get => this._isValueSettingsGroupEnabled;
      set
      {
        this._isValueSettingsGroupEnabled = value;
        this.OnPropertyChanged(nameof (IsValueSettingsGroupEnabled));
      }
    }

    public bool IsSubDeviceGroupEnabled
    {
      get => this._isSubDeviceGroupEnabled;
      set
      {
        this._isSubDeviceGroupEnabled = value;
        this.OnPropertyChanged(nameof (IsSubDeviceGroupEnabled));
      }
    }

    public bool IsZDFKeyEnabled
    {
      get => this._isZDFKeyEnabled;
      set
      {
        this._isZDFKeyEnabled = value;
        this.OnPropertyChanged(nameof (IsZDFKeyEnabled));
      }
    }

    public bool IsDeviceTypeEnabled
    {
      get => this._isDeviceTypeEnabled;
      set
      {
        this._isDeviceTypeEnabled = value;
        this.OnPropertyChanged(nameof (IsDeviceTypeEnabled));
      }
    }

    public IEnumerable<string> MeterTypeEnumerable => ValueIdentHelper.GetMeterTypeEnumerable();

    public IEnumerable<string> PhysicalQuantitiesEnumerable
    {
      get => ValueIdentHelper.GetPhysicalQuantitiesEnumerable();
    }

    public IEnumerable<string> CalculationEnumerable => ValueIdentHelper.GetCalculationEnumerable();

    public IEnumerable<string> CalculationStartEnumerable
    {
      get => ValueIdentHelper.GetCalculationStartEnumerable();
    }

    public IEnumerable<string> StorageIntervalEnumerable
    {
      get => ValueIdentHelper.GetStorageIntervalEnumerable();
    }

    public IEnumerable<string> CreationEnumerable => ValueIdentHelper.GetCreationEnumerable();

    public string ValueIdentifier
    {
      get => this._valueIdentifier;
      set
      {
        this._valueIdentifier = value;
        this.OnPropertyChanged(nameof (ValueIdentifier));
      }
    }

    public string Unit
    {
      get => this._unit;
      set
      {
        this._unit = value;
        this.OnPropertyChanged(nameof (Unit));
      }
    }

    public string Timepoint
    {
      get => this._timepoint;
      set
      {
        List<string> timepointList = this.TimepointList;
        this._timepoint = timepointList != null ? timepointList.FirstOrDefault<string>((Func<string, bool>) (item => item == value)) : (string) null;
        this.OnPropertyChanged(nameof (Timepoint));
      }
    }

    public List<string> TimepointList
    {
      get => this._timepointList;
      set
      {
        this._timepointList = value;
        this.OnPropertyChanged(nameof (TimepointList));
      }
    }

    public string TimepointModification
    {
      get => this._timepointModification;
      set
      {
        List<string> modificationList = this.TimepointModificationList;
        this._timepointModification = modificationList != null ? modificationList.FirstOrDefault<string>((Func<string, bool>) (item => value == null ? item == "None" : item == value)) : (string) null;
        this.OnPropertyChanged(nameof (TimepointModification));
      }
    }

    public List<string> TimepointModificationList
    {
      get => this._timepointModificationList;
      set
      {
        this._timepointModificationList = value;
        this.OnPropertyChanged(nameof (TimepointModificationList));
      }
    }

    [Required(ErrorMessage = "MSS_Client_MeterType_Mandatory")]
    public string MeterType
    {
      get => this._meterType;
      set
      {
        this._meterType = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    [Required(ErrorMessage = "MSS_Client_PhysicalQuantity_Mandatory")]
    public string PhysicalQuantity
    {
      get => this._physicalQuantity;
      set
      {
        this._physicalQuantity = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    [Required(ErrorMessage = "MSS_Client_Calculation_Mandatory")]
    public string Calculation
    {
      get => this._calculation;
      set
      {
        this._calculation = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    [Required(ErrorMessage = "MSS_Client_CalculationStart_Mandatory")]
    public string CalculationStart
    {
      get => this._calculationStart;
      set
      {
        this._calculationStart = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    [Required(ErrorMessage = "MSS_Client_StorageInterval_Mandatory")]
    public string StorageInterval
    {
      get => this._storageInterval;
      set
      {
        this._storageInterval = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    [Required(ErrorMessage = "MSS_Client_Creation_Mandatory")]
    public string Creation
    {
      get => this._creation;
      set
      {
        this._creation = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public int RuleIndex
    {
      get => this._ruleIndex;
      set
      {
        this._ruleIndex = value;
        this._valueIdentifier = ValueIdentHelper.GetValueId(this.PhysicalQuantity, this.MeterType, this.Calculation, this.CalculationStart, this.StorageInterval, this.Creation, this._ruleIndex);
        this.OnPropertyChanged("ValueIdentifier");
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public bool IsShowAllRulesChecked
    {
      get => this._isShowAllRulesChecked;
      set
      {
        this._isShowAllRulesChecked = value;
        this.OnPropertyChanged(nameof (IsShowAllRulesChecked));
      }
    }

    private Dictionary<DeviceMediumEnum, string> InitDeviceMediumList()
    {
      return EnumHelper.GetEnumTranslationsDictionary<DeviceMediumEnum>().OrderBy<KeyValuePair<DeviceMediumEnum, string>, string>((Func<KeyValuePair<DeviceMediumEnum, string>, string>) (item => item.Value)).ToDictionary<KeyValuePair<DeviceMediumEnum, string>, DeviceMediumEnum, string>((Func<KeyValuePair<DeviceMediumEnum, string>, DeviceMediumEnum>) (t => t.Key), (Func<KeyValuePair<DeviceMediumEnum, string>, string>) (t => t.Value));
    }

    private List<string> InitTimepointModificationList()
    {
      return Enum.GetValues(typeof (SpecialStorageTimeTranslation)).Cast<SpecialStorageTimeTranslation>().Select<SpecialStorageTimeTranslation, string>((Func<SpecialStorageTimeTranslation, string>) (item => item.ToString())).ToList<string>();
    }

    private List<string> InitSpecialTranslationsList()
    {
      return Enum.GetValues(typeof (SpecialTranslationsEnum)).Cast<SpecialTranslationsEnum>().Select<SpecialTranslationsEnum, string>((Func<SpecialTranslationsEnum, string>) (item => item.ToString())).ToList<string>();
    }

    private void LoadTranslationRule(TranslationRule rule)
    {
      this._manufacturer = rule.Manufacturer;
      this.OnPropertyChanged("Manufacturer");
      this._medium = rule.Medium != "" ? this.DeviceMediumList.FirstOrDefault<KeyValuePair<DeviceMediumEnum, string>>((Func<KeyValuePair<DeviceMediumEnum, string>, bool>) (item => item.Key.ToString() == rule.Medium)).Key.ToString() : this.DeviceMediumList[DeviceMediumEnum.OTHER];
      this.OnPropertyChanged("Medium");
      this._versionMin = new int?(rule.VersionMin);
      this.OnPropertyChanged("VersionMin");
      this._versionMax = new int?(rule.VersionMax);
      this.OnPropertyChanged("VersionMax");
      List<string> zdfKeyList = this.ZDFKeyList;
      this.ZDFKey = zdfKeyList != null ? zdfKeyList.FirstOrDefault<string>((Func<string, bool>) (item => item == rule.MBusZDF)) : (string) null;
      this.ValueIdentifier = rule.ValueIdent.ToString();
      if (this.IsMainChecked)
      {
        long valueId = long.Parse(this._valueIdentifier);
        this._meterType = this.MeterTypeEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId).ToString()));
        this.OnPropertyChanged("MeterType");
        this._physicalQuantity = this.PhysicalQuantitiesEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId).ToString()));
        this.OnPropertyChanged("PhysicalQuantity");
        this._calculation = this.CalculationEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId).ToString()));
        this.OnPropertyChanged("Calculation");
        this._calculationStart = this.CalculationStartEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId).ToString()));
        this.OnPropertyChanged("CalculationStart");
        this._storageInterval = this.StorageIntervalEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId).ToString()));
        this.OnPropertyChanged("StorageInterval");
        this._creation = this.CreationEnumerable.FirstOrDefault<string>((Func<string, bool>) (item => item == ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId).ToString()));
        this.OnPropertyChanged("Creation");
        this._ruleIndex = (int) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Index>(valueId);
        this.OnPropertyChanged("RuleIndex");
        this.Timepoint = rule.StorageTimeParam;
        this.TimepointModification = rule.StorageTimeTranslation.ToString();
        this.Multiplier = rule.Multiplier;
        this.RuleOrder = rule.RuleOrder;
      }
      if (!this.IsSubDeviceChecked)
        return;
      this.SpecialTranslation = rule.SpecialTranslation.ToString();
      this.SubDeviceIndex = rule.SubDeviceIndex;
      this.SubDeviceZDFKey = rule.SubDeviceAttributeIdentifier;
    }

    private void EnableDisableControls()
    {
      this.IsTranslateToGroupEnabled = this._isEditMode;
      this.IsValueSettingsGroupEnabled = this._isEditMode;
      this.IsSubDeviceGroupEnabled = this._isEditMode;
      this.IsZDFKeyEnabled = this._isEditMode;
      this.IsDeviceTypeEnabled = this._isEditMode;
      this.IsDeleteButtonEnabled = !this._isEditMode && this._isItemSelected;
      this.IsEditButtonEnabled = !this._isEditMode && this._isItemSelected;
      this.IsSaveButtonEnabled = this._isEditMode;
      this.IsCancelButtonEnabled = this._isEditMode;
    }

    private void LoadTranslationRulesList()
    {
      string manufacturer = this._manufacturer ?? "";
      string medium = "";
      if (!string.IsNullOrEmpty(this._medium))
        medium = Enum.GetValues(typeof (DeviceMediumEnum)).Cast<DeviceMediumEnum>().ToList<DeviceMediumEnum>().FirstOrDefault<DeviceMediumEnum>((Func<DeviceMediumEnum, bool>) (item => item.ToString() == this._medium)).ToString();
      this.TranslationRules = TranslationRulesManager.Instance.LoadRules(manufacturer, medium, this._versionMin, this._versionMax);
    }

    private void ClearFields()
    {
      this._selectedItem = (TranslationRule) null;
      this.OnPropertyChanged("SelectedItem");
      this._isItemSelected = false;
      this.ZDFKey = (string) null;
      this._meterType = (string) null;
      this.OnPropertyChanged("MeterType");
      this._physicalQuantity = (string) null;
      this.OnPropertyChanged("PhysicalQuantity");
      this._calculation = (string) null;
      this.OnPropertyChanged("Calculation");
      this._calculationStart = (string) null;
      this.OnPropertyChanged("CalculationStart");
      this._storageInterval = (string) null;
      this.OnPropertyChanged("StorageInterval");
      this._creation = (string) null;
      this.OnPropertyChanged("Creation");
      this._ruleIndex = 0;
      this.OnPropertyChanged("RuleIndex");
      this.ValueIdentifier = "0";
      this.Unit = (string) null;
      this.Timepoint = (string) null;
      this.TimepointModification = (string) null;
      this.Multiplier = 1.0;
      this.RuleOrder = 0;
      this.SpecialTranslation = (string) null;
      this.SubDeviceIndex = 1;
      this.SubDeviceZDFKey = (string) null;
    }

    private TranslationRule GetRuleFromUI()
    {
      TranslationRule ruleFromUi = new TranslationRule();
      ruleFromUi.Manufacturer = this.Manufacturer ?? "";
      Dictionary<string, string> enumElements = EnumHelper.GetEnumElements<DeviceMediumEnum>();
      ruleFromUi.Medium = enumElements.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (item => item.Key == this.Medium)).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (item => item.Key)).FirstOrDefault<string>();
      TranslationRule translationRule1 = ruleFromUi;
      int? nullable = this.VersionMin;
      int num1 = nullable ?? 0;
      translationRule1.VersionMin = num1;
      TranslationRule translationRule2 = ruleFromUi;
      nullable = this.VersionMax;
      int num2 = nullable ?? 0;
      translationRule2.VersionMax = num2;
      ruleFromUi.MBusZDF = this.ZDFKey;
      ruleFromUi.ValueIdent = this.IsMainChecked ? long.Parse(this.ValueIdentifier) : 0L;
      ruleFromUi.StorageTimeParam = this.IsMainChecked ? this.Timepoint : (string) null;
      if (this.IsMainChecked)
      {
        TranslationRule translationRule3 = ruleFromUi;
        Type enumType = typeof (SpecialStorageTimeTranslation);
        string str = this.TimepointModification;
        if (str == null)
        {
          List<string> modificationList = this.TimepointModificationList;
          str = modificationList != null ? modificationList.First<string>((Func<string, bool>) (item => item == "None")) : (string) null;
        }
        int num3 = (int) Enum.Parse(enumType, str);
        translationRule3.StorageTimeTranslation = (SpecialStorageTimeTranslation) num3;
        ruleFromUi.SubDeviceIndex = 0;
        ruleFromUi.Multiplier = this.Multiplier;
        ruleFromUi.RuleOrder = this.RuleOrder;
      }
      if (this.IsSubDeviceChecked)
      {
        ruleFromUi.SpecialTranslation = (ZR_ClassLibrary.SpecialTranslation) Enum.Parse(typeof (ZR_ClassLibrary.SpecialTranslation), this.SpecialTranslation);
        ruleFromUi.SubDeviceIndex = this.SubDeviceIndex;
        ruleFromUi.SubDeviceAttributeIdentifier = this.SubDeviceZDFKey;
      }
      return ruleFromUi;
    }

    public ICommand NewRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._isEditMode = true;
          this.SelectedItem = (TranslationRule) null;
          this._isItemSelected = false;
          this.EnableDisableControls();
          this._isNewRule = new bool?(true);
        }));
      }
    }

    public ICommand DeleteRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_TranslationRules_DeleteRuleDialog), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) true)));
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          bool flag = TranslationRulesManager.Instance.DeleteRule(this.SelectedItem);
          this._isEditMode = false;
          this.ClearFields();
          this.InitUI();
          this.SelectedItem = this.TranslationRules.FirstOrDefault<TranslationRule>();
          this._isItemSelected = this.SelectedItem != null;
          if (!this._isItemSelected)
            this.EnableDisableControls();
          if (!flag)
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_TranslationRules_UnableToDeleteRule), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
        }));
      }
    }

    public ICommand ClearRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.ClearFields();
          this._isNewRule = new bool?();
        }));
      }
    }

    public ICommand EditRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._isEditMode = true;
          this.EnableDisableControls();
          this._isNewRule = new bool?(false);
        }));
      }
    }

    public ICommand ShowAllRules
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? nullable = parameter is CheckBox checkBox2 ? checkBox2.IsChecked : new bool?();
          if (!nullable.HasValue || !nullable.Value)
            return;
          string receivedManufacturer = this._receivedManufacturer;
          string receivedMedium = this._receivedMedium;
          this._receivedManufacturer = "";
          this._receivedMedium = "UNKNOWN";
          this.InitUI();
          this._receivedManufacturer = receivedManufacturer;
          this._receivedMedium = receivedMedium;
        }));
      }
    }

    public ICommand SaveRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.Medium == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_TranslationRules_MediumMandatory);
          }
          else
          {
            bool flag = false;
            this._isEditMode = false;
            this.EnableDisableControls();
            if (this.IsMainChecked)
            {
              if (this._isNewRule.HasValue && this.IsValid)
                flag = this._isNewRule.Value ? TranslationRulesManager.Instance.CreateRule(this.GetRuleFromUI()) : TranslationRulesManager.Instance.UpdateRule(this.SelectedItem, this.GetRuleFromUI());
            }
            else if (this.IsSubDeviceChecked)
              flag = !string.IsNullOrEmpty(this._zdfKey) && !string.IsNullOrEmpty(this._specialTranslation) && (this._isNewRule.Value ? TranslationRulesManager.Instance.CreateRule(this.GetRuleFromUI()) : TranslationRulesManager.Instance.UpdateRule(this.SelectedItem, this.GetRuleFromUI()));
            this.MessageUserControl = flag ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_TranslationRule_Save_Successful) : MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_TranslationRule_Save_Error);
            this._selectedItem = (TranslationRule) null;
            this.OnPropertyChanged("SelectedItem");
            this._isNewRule = new bool?();
            this.ClearFields();
            this.LoadTranslationRulesList();
            this.IsShowAllRulesChecked = false;
          }
        }));
      }
    }

    public ICommand CancelRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._isEditMode = false;
          this.EnableDisableControls();
          this.ClearFields();
        }));
      }
    }
  }
}
