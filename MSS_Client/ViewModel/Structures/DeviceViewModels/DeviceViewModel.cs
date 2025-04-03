// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.DeviceViewModels.DeviceViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Client.UI.Common.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using MSS_Client.ViewModel.Structures.Helpers.StructureHelpers;
using MVVM.Commands;
using MVVM.Converters;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using NLog;
using Styles.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ZENNER;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Structures.DeviceViewModels
{
  public class DeviceViewModel : ValidationViewModelBase
  {
    protected IWindowFactory _windowFactory;
    protected StructureNodeDTO _selectedNode;
    protected IRepository<RoomType> _roomTypeRepository;
    protected IRepository<MeasureUnit> _measureUnitsRepository;
    protected IRepository<Channel> _channelRepository;
    protected IRepository<ConnectedDeviceType> _connectedDeviceTypeRepository;
    protected IRepositoryFactory _repositoryFactory;
    protected ConfiguratorManager configManager;
    protected bool isSaveAndCreateNewFirstTimePressed;
    protected IEnumerable<string> _licenseDeviceTypes;
    private static Logger logger = LogManager.GetCurrentClassLogger();
    protected StructureBehaviour structureBehaviour;
    protected StructureTypeEnum? _structureType;
    private const string DEVICE_GROUP_WATER_METERS = "Water meters";
    private bool _isExistingMeter;
    private string _meterDialogTitle;
    private bool _isDeviceInfoVisible;
    private bool _isMeterReplacementVisible;
    private bool _areReadWriteButtonsVisibile;
    private bool _areTabletButtonsEnabled;
    private ReplacementMeterDropDownValue _meterToReplaceWith;
    private bool _isReadingEnabled;
    private bool _isReadingEnabledVisible;
    private double? _impulses;
    private DeviceGroup _selectedDeviceGroup;
    private DeviceModel _selectedDeviceType;
    private BitmapImage _deviceModelImage;
    private string _name;
    private string _description;
    private string _city;
    private string _street;
    private string _houseNumber;
    private string _houseNumberSupplement;
    private string _apartmentNumber;
    private string _zipCode;
    private string _firstName;
    private string _lastName;
    private string _location;
    private string _radioSerialNumber;
    private string _serialNo = string.Empty;
    private string _shortDeviceNo;
    private string _completeDeviceId;
    private bool _isBusy;
    private Dictionary<DeviceMediumEnum, string> _mediumCollection;
    private Dictionary<MeasurementRangeEnum, string> _measurementRangeCollection;
    private Guid? _selectedDeviceTypeId;
    private Guid? _selectedStartValueUnitId;
    private string _startValue;
    private double? _evaluationFactor;
    private Guid? _selectedImpulsUnitId;
    private Guid? _selectedChannelId;
    public IEnumerable<ConnectedDeviceTypeDTO> _connectedDeviceTypeCollection;
    private Guid? _selectedConnectedDeviceTypeId;
    private List<ReplacedMeterDTO> _meterReplacementHistoryCollection;
    private string _pageSize = string.Empty;
    private string _AES;
    private int? _primaryAddress;
    private string _manufacturer;
    private string _medium;
    private string _inputNumber;
    private string _generation;
    private ViewModelBase _messageUserControl;
    private bool _isSerialNumberEnabled;
    private bool _isDeviceGroupEnabled;
    private bool _isDeviceModelEnabled;
    private Dictionary<string, string> _deviceInfoCollection;
    private string _measurementRange;
    private bool _isEditMeterButtonVisible;
    private bool _isShortDeviceNoVisible;
    private bool _isShortDeviceNoEnabled;
    private bool _isCompletDeviceIdVisible;
    private bool _isRoomVisible;
    private bool _isStartValueVisible;
    private bool _isEvaluationFactorVisible;
    private bool _isImpulsValueVisible;
    private bool _isImpulsUnitVisible;
    private bool _isChannelVisible;
    private bool _isConnectedDeviceTypeVisible;
    private bool _isAESVisible;
    private bool _isPrimaryAddressVisible;
    private bool _isManufacturerVisible;
    private bool _isMediumVisible;
    private bool _isInputNumberVisible;
    private bool _isMeasurementRangeVisible;
    private bool _isGenerationVisible;
    public bool _isConfigValuesVisible;
    public bool _isConfigChannel1Visible;
    public bool _isConfigChannel2Visible;
    private bool _isConfigChannel3Visible;
    private List<ConfigurationPerChannel> _configValuesCollection;
    private bool _isReadButtonEnabled;
    private bool _isWriteButtonEnabled;

    [Inject]
    public DeviceViewModel(
      DeviceStateEnum deviceState,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      List<string> serialNumberList)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._roomTypeRepository = repositoryFactory.GetRepository<RoomType>();
      this._measureUnitsRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._channelRepository = repositoryFactory.GetRepository<Channel>();
      this._connectedDeviceTypeRepository = repositoryFactory.GetRepository<ConnectedDeviceType>();
      this._licenseDeviceTypes = LicenseHelper.GetDeviceTypes();
      this.configManager = GmmInterface.ConfiguratorManager;
      this._selectedNode = node;
      this._structureType = node.StructureType;
      this.IsWriteButtonEnabled = false;
      this.SerialNumberList = new List<string>((IEnumerable<string>) serialNumberList);
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this.MeterReplacementHistoryCollection = this.GetMeterReplacementHistoryCollection();
      this.SelectedNodeType = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name);
      this.structureBehaviour = StructureFactory.GetStructureBehaviour(node.RootNode.StructureType);
      this.IsSerialNumberEnabled = node.ParentNode != null && node.ParentNode.NodeType.Name == "Meter";
      this.InitializeMediumCollection();
      this.InitializeMeasurementRangeCollection();
      MeterDTO dto = node.Entity as MeterDTO;
      if (dto != null && node.Entity != null && dto.Medium.HasValue)
      {
        MeterDTO entity = (MeterDTO) node.Entity;
        DeviceMediumEnum? medium = entity.Medium;
        if (medium.HasValue)
        {
          medium = entity.Medium;
          this.Medium = medium.Value.ToString();
        }
      }
      this.AreTabletButtonsEnabled = true;
      this.IsConfigValuesVisible = false;
      this.IsConfigChannel1Visible = false;
      this.IsConfigChannel2Visible = false;
      this.IsConfigChannel3Visible = false;
      if (dto != null && node.NodeType.Name == "RadioMeter")
      {
        if (dto.MbusRadioMeter == null)
        {
          MbusRadioMeter mbusRadioMeter = this._repositoryFactory.GetRepository<MbusRadioMeter>().FirstOrDefault((Expression<Func<MbusRadioMeter, bool>>) (item => item.Meter.Id == dto.Id));
          dto.MbusRadioMeter = mbusRadioMeter;
        }
        if (dto.MbusRadioMeter != null)
          this.InitializeMeterMbusRadioDetails(dto.MbusRadioMeter);
      }
      if (dto != null && node.NodeType.Name == "Meter")
      {
        MeterRadioDetails meterRadioDetails1;
        if (dto.MeterRadioDetails == null || dto.MeterRadioDetails.Count <= 0)
          meterRadioDetails1 = this._repositoryFactory.GetRepository<MeterRadioDetails>().FirstOrDefault((Expression<Func<MeterRadioDetails, bool>>) (m => m.Meter.Id == dto.Id));
        else
          meterRadioDetails1 = dto.MeterRadioDetails.ElementAt<MeterRadioDetails>(0);
        MeterRadioDetails meterRadioDetails2 = meterRadioDetails1;
        if (meterRadioDetails2 != null)
          this.InitializeRadioMeterDetails(meterRadioDetails2);
      }
      this.StructureType = this._selectedNode.StructureType;
      StructureTypeEnum? structureType = this.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      this.IsReadingEnabledVisible = structureType.GetValueOrDefault() != structureTypeEnum || !structureType.HasValue;
      this.IsMeterBeingReplaced = false;
    }

    public bool IsExistingMeter
    {
      get => this._isExistingMeter;
      set => this._isExistingMeter = value;
    }

    public string MeterDialogTitle
    {
      get => this._meterDialogTitle;
      set
      {
        this._meterDialogTitle = value;
        this.OnPropertyChanged(nameof (MeterDialogTitle));
      }
    }

    public StructureTypeEnum? StructureType { get; set; }

    public bool IsDeviceInfoVisible
    {
      get => this._isDeviceInfoVisible;
      set
      {
        this._isDeviceInfoVisible = value;
        this.OnPropertyChanged(nameof (IsDeviceInfoVisible));
      }
    }

    public bool IsMeterReplacementVisible
    {
      get => this._isMeterReplacementVisible;
      set
      {
        this._isMeterReplacementVisible = value;
        this.OnPropertyChanged(nameof (IsMeterReplacementVisible));
      }
    }

    public bool AreReadWriteButtonsVisibile
    {
      get => this._areReadWriteButtonsVisibile;
      set
      {
        this._areReadWriteButtonsVisibile = value;
        this.OnPropertyChanged(nameof (AreReadWriteButtonsVisibile));
      }
    }

    public bool AreTabletButtonsEnabled
    {
      get => this._areTabletButtonsEnabled;
      set
      {
        this._areTabletButtonsEnabled = value;
        this.OnPropertyChanged(nameof (AreTabletButtonsEnabled));
      }
    }

    public bool IsMeterBeingReplaced { get; set; }

    public ReplacementMeterDropDownValue MeterToReplaceWith
    {
      get => this._meterToReplaceWith;
      set
      {
        this._meterToReplaceWith = value;
        this.OnPropertyChanged(nameof (MeterToReplaceWith));
      }
    }

    public bool IsReadingEnabled
    {
      get => this._isReadingEnabled;
      set
      {
        this._isReadingEnabled = value;
        this.OnPropertyChanged(nameof (IsReadingEnabled));
      }
    }

    public bool IsReadingEnabledVisible
    {
      get => this._isReadingEnabledVisible;
      set
      {
        this._isReadingEnabledVisible = value;
        this.OnPropertyChanged(nameof (IsReadingEnabledVisible));
      }
    }

    public double? Impulses
    {
      get => this._impulses;
      set
      {
        this._impulses = value;
        this.OnPropertyChanged(nameof (Impulses));
      }
    }

    public StructureNodeTypeEnum SelectedNodeType { get; set; }

    public IEnumerable<DeviceGroup> DeviceGroupCollection
    {
      get
      {
        List<DeviceGroup> deviceGroups = GmmInterface.DeviceManager.GetDeviceGroups();
        List<DeviceGroup> deviceGroupCollection = new List<DeviceGroup>();
        StructureTypeEnum? structureType = this._selectedNode.RootNode.StructureType;
        if (structureType.HasValue)
        {
          switch (structureType.GetValueOrDefault())
          {
            case StructureTypeEnum.Physical:
            case StructureTypeEnum.Logical:
              deviceGroupCollection = deviceGroups.Where<DeviceGroup>((Func<DeviceGroup, bool>) (deviceGroup => GmmInterface.DeviceManager.GetDeviceModels(deviceGroup).Any<DeviceModel>((Func<DeviceModel, bool>) (d => !d.Parameters.ContainsKey(ConnectionProfileParameter.SystemDevice) && d.Name != "Smoke detectors")))).OrderBy<DeviceGroup, string>((Func<DeviceGroup, string>) (d => d.Name)).ToList<DeviceGroup>();
              break;
            case StructureTypeEnum.Fixed:
              deviceGroupCollection = deviceGroups.Where<DeviceGroup>((Func<DeviceGroup, bool>) (deviceGroup => GmmInterface.DeviceManager.GetDeviceModels(deviceGroup).Any<DeviceModel>((Func<DeviceModel, bool>) (d => d.Parameters.ContainsKey(ConnectionProfileParameter.Radio3))))).OrderBy<DeviceGroup, string>((Func<DeviceGroup, string>) (d => d.Name)).ToList<DeviceGroup>();
              break;
          }
        }
        return (IEnumerable<DeviceGroup>) deviceGroupCollection;
      }
    }

    public List<DeviceModel> DeviceTypeCollection { get; set; }

    private void RefreshDeviceTypesCollection()
    {
      DeviceGroup selectedDeviceGroup = this.SelectedDeviceGroup;
      if (selectedDeviceGroup == null)
        this.DeviceTypeCollection = (List<DeviceModel>) null;
      List<DeviceModel> source = new List<DeviceModel>();
      StructureTypeEnum? structureType = this._selectedNode.RootNode.StructureType;
      if (structureType.HasValue)
      {
        switch (structureType.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
          case StructureTypeEnum.Logical:
            source = GmmInterface.DeviceManager.GetDeviceModels(selectedDeviceGroup).Where<DeviceModel>((Func<DeviceModel, bool>) (d => !d.Parameters.ContainsKey(ConnectionProfileParameter.SystemDevice))).OrderBy<DeviceModel, string>((Func<DeviceModel, string>) (d => d.Name)).ToList<DeviceModel>();
            break;
          case StructureTypeEnum.Fixed:
            source = GmmInterface.DeviceManager.GetDeviceModels(selectedDeviceGroup).Where<DeviceModel>((Func<DeviceModel, bool>) (d => d.Parameters.ContainsKey(ConnectionProfileParameter.Radio3))).OrderBy<DeviceModel, string>((Func<DeviceModel, string>) (d => d.Name)).ToList<DeviceModel>();
            break;
        }
      }
      this.DeviceTypeCollection = source.Where<DeviceModel>((Func<DeviceModel, bool>) (deviceModel => this._licenseDeviceTypes.Contains<string>(deviceModel.Name))).ToList<DeviceModel>();
    }

    public DeviceGroup SelectedDeviceGroup
    {
      get => this._selectedDeviceGroup;
      set
      {
        this._selectedDeviceGroup = value;
        this.RefreshDeviceTypesCollection();
        this.RefreshConnectedDeviceTypeCollection();
        this.OnPropertyChanged("DeviceTypeCollection");
        this.OnPropertyChanged(nameof (SelectedDeviceGroup));
      }
    }

    public DeviceModel SelectedDeviceType
    {
      get => this._selectedDeviceType;
      set
      {
        this._selectedDeviceType = value;
        if (this._selectedDeviceType != null)
        {
          this.DeviceModelImage = this._selectedDeviceType.Image500x500;
          if (this._selectedDeviceType.Parameters.ContainsKey(ConnectionProfileParameter.MBus) || this._selectedDeviceType.Parameters.ContainsKey(ConnectionProfileParameter.wMBus))
            this.IsDeviceInfoVisible = true;
        }
        else
          this.DeviceModelImage = (BitmapImage) null;
        this.OnPropertyChanged("DeviceTypeEnumBySelectedDeviceModel");
        this.OnPropertyChanged(nameof (SelectedDeviceType));
        this.InitializeMeasurementRangeCollection();
        this.ResetControls();
        if (this._selectedDeviceType != null)
        {
          this.IsReadButtonEnabled = true;
        }
        else
        {
          this.IsReadButtonEnabled = false;
          this.IsWriteButtonEnabled = false;
        }
        this.OnPropertyChanged("IsShortDeviceNoVisible");
        this.OnPropertyChanged("IsCompletDeviceIdVisible");
        this.OnPropertyChanged("IsRoomVisible");
        this.OnPropertyChanged("IsStartValueVisible");
        this.OnPropertyChanged("IsEvaluationFactorVisible");
        this.OnPropertyChanged("IsImpulsValueVisible");
        this.OnPropertyChanged("IsImpulsUnitVisible");
        this.OnPropertyChanged("IsChannelVisible");
        this.OnPropertyChanged("IsConnectedDeviceTypeVisible");
        this.OnPropertyChanged("IsAESVisible");
        this.OnPropertyChanged("IsPrimaryAddressVisible");
        this.OnPropertyChanged("IsManufacturerVisible");
        this.OnPropertyChanged("IsMediumVisible");
        this.OnPropertyChanged("IsInputNumberVisible");
        this.OnPropertyChanged("IsGenerationVisible");
        this.OnPropertyChanged("IsMeasurementRangeVisible");
        UserDeviceTypeSettings deviceTypeSettings = this._repositoryFactory.GetRepository<UserDeviceTypeSettings>().FirstOrDefault((Expression<Func<UserDeviceTypeSettings, bool>>) (x => (int?) x.DeviceType == (int?) this.DeviceTypeEnumBySelectedDeviceModel));
        if (!this.IsExistingMeter && deviceTypeSettings != null)
          this.SelectedStartValueUnitId = new Guid?(deviceTypeSettings.DisplayUnit != null ? deviceTypeSettings.DisplayUnit.Id : Guid.Empty);
        this.OnPropertyChanged("SelectedStartValueUnitId");
        this.IsSerialNumberEnabled = true;
        if (!this._isSerialNumberEnabled)
          this.SerialNo = string.Empty;
        this.SetDefaultValuesForWaterMetersDeviceGroup();
      }
    }

    public BitmapImage DeviceModelImage
    {
      get => this._deviceModelImage;
      set
      {
        this._deviceModelImage = value;
        this.OnPropertyChanged(nameof (DeviceModelImage));
      }
    }

    public DeviceTypeEnum? DeviceTypeEnumBySelectedDeviceModel
    {
      get
      {
        return this.SelectedDeviceType != null ? StructuresHelper.GetDeviceTypeEnumByDeviceModelName(this.SelectedDeviceType.Name) : new DeviceTypeEnum?();
      }
    }

    public string Name
    {
      get => this._name;
      set
      {
        this._name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public string Description
    {
      get => this._description;
      set
      {
        this._description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }

    public string City
    {
      get => this._city;
      set
      {
        this._city = value;
        this.OnPropertyChanged(nameof (City));
      }
    }

    public string Street
    {
      get => this._street;
      set
      {
        this._street = value;
        this.OnPropertyChanged(nameof (Street));
      }
    }

    public string HouseNumber
    {
      get => this._houseNumber;
      set
      {
        this._houseNumber = value;
        this.OnPropertyChanged(nameof (HouseNumber));
      }
    }

    public string HouseNumberSupplement
    {
      get => this._houseNumberSupplement;
      set
      {
        this._houseNumberSupplement = value;
        this.OnPropertyChanged(nameof (HouseNumberSupplement));
      }
    }

    public string ApartmentNumber
    {
      get => this._apartmentNumber;
      set
      {
        this._apartmentNumber = value;
        this.OnPropertyChanged(nameof (ApartmentNumber));
      }
    }

    public string ZipCode
    {
      get => this._zipCode;
      set
      {
        this._zipCode = value;
        this.OnPropertyChanged(nameof (ZipCode));
      }
    }

    public string FirstName
    {
      get => this._firstName;
      set
      {
        this._firstName = value;
        this.OnPropertyChanged(nameof (FirstName));
      }
    }

    public string LastName
    {
      get => this._lastName;
      set
      {
        this._lastName = value;
        this.OnPropertyChanged(nameof (LastName));
      }
    }

    public string Location
    {
      get => this._location;
      set
      {
        this._location = value;
        this.OnPropertyChanged(nameof (Location));
      }
    }

    public string RadioSerialNumber
    {
      get => this._radioSerialNumber;
      set
      {
        this._radioSerialNumber = value;
        this.OnPropertyChanged(nameof (RadioSerialNumber));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Structures_SerialNumberErrorToolTip")]
    public string SerialNo
    {
      get => this._serialNo;
      set
      {
        this._serialNo = value;
        StructureTypeEnum? structureType = this._selectedNode.RootNode.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
          this.structureBehaviour.UpdateName(this);
        this.AnteriorSerialNumber = this._serialNo;
        this.OnPropertyChanged(nameof (SerialNo));
      }
    }

    public string AnteriorSerialNumber { get; set; }

    public List<string> SerialNumberList { get; set; }

    public string ShortDeviceNo
    {
      get => this._shortDeviceNo;
      set
      {
        this._shortDeviceNo = value;
        this.OnPropertyChanged(nameof (ShortDeviceNo));
      }
    }

    public string CompleteDeviceId
    {
      get => this._completeDeviceId;
      set
      {
        this._completeDeviceId = value;
        if (this._structureType.HasValue && this._structureType.Value == StructureTypeEnum.Fixed && this._selectedDeviceType != null)
        {
          DeviceTypeEnum? byDeviceModelName = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(this.SelectedDeviceType.Name);
          DeviceTypeEnum? nullable = byDeviceModelName;
          DeviceTypeEnum deviceTypeEnum1 = DeviceTypeEnum.MinotelContactRadio3;
          int num1;
          if ((nullable.GetValueOrDefault() == deviceTypeEnum1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = byDeviceModelName;
            DeviceTypeEnum deviceTypeEnum2 = DeviceTypeEnum.MinomessMicroRadio3;
            if ((nullable.GetValueOrDefault() == deviceTypeEnum2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
            {
              nullable = byDeviceModelName;
              DeviceTypeEnum deviceTypeEnum3 = DeviceTypeEnum.C5Radio;
              num1 = nullable.GetValueOrDefault() == deviceTypeEnum3 ? (nullable.HasValue ? 1 : 0) : 0;
              goto label_5;
            }
          }
          num1 = 1;
label_5:
          if (num1 != 0)
            this.ShortDeviceNo = value == null || value.Length < 4 ? value : value.Substring(value.Length - 4);
          nullable = byDeviceModelName;
          DeviceTypeEnum deviceTypeEnum4 = DeviceTypeEnum.TemperatureSensor;
          int num2;
          if ((nullable.GetValueOrDefault() == deviceTypeEnum4 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = byDeviceModelName;
            DeviceTypeEnum deviceTypeEnum5 = DeviceTypeEnum.HumiditySensor;
            num2 = nullable.GetValueOrDefault() == deviceTypeEnum5 ? (nullable.HasValue ? 1 : 0) : 0;
          }
          else
            num2 = 1;
          if (num2 != 0)
          {
            this.ShortDeviceNo = value == null || value.Length < 4 ? value : value.Substring(value.Length - 4);
            this.SerialNo = value;
          }
        }
        this.OnPropertyChanged(nameof (CompleteDeviceId));
      }
    }

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        if (this._isBusy == value)
          return;
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public IEnumerable<RoomTypeDTO> RoomTypeCollection
    {
      get
      {
        ObservableCollection<RoomTypeDTO> roomTypeDTOList = new ObservableCollection<RoomTypeDTO>();
        TypeHelperExtensionMethods.ForEach<RoomType>((IEnumerable<RoomType>) this._roomTypeRepository.GetAll().OrderBy<RoomType, string>((Func<RoomType, string>) (rt => rt.Code)), (Action<RoomType>) (r => roomTypeDTOList.Add(new RoomTypeDTO()
        {
          Id = r.Id,
          Code = r.Code,
          Name = string.IsNullOrEmpty(CultureResources.GetValue(r.Code)) ? r.Code : CultureResources.GetValue(r.Code)
        })));
        return (IEnumerable<RoomTypeDTO>) roomTypeDTOList.OrderBy<RoomTypeDTO, string>((Func<RoomTypeDTO, string>) (x => x.Name)).ToList<RoomTypeDTO>();
      }
    }

    public Dictionary<DeviceMediumEnum, string> MediumCollection => this._mediumCollection;

    public Dictionary<MeasurementRangeEnum, string> MeasurementRangeCollection
    {
      get => this._measurementRangeCollection;
      set
      {
        this._measurementRangeCollection = value;
        this.OnPropertyChanged(nameof (MeasurementRangeCollection));
      }
    }

    public Guid? SelectedRoomTypeId
    {
      get => this._selectedDeviceTypeId;
      set
      {
        this._selectedDeviceTypeId = value;
        this.OnPropertyChanged(nameof (SelectedRoomTypeId));
      }
    }

    public Guid? SelectedStartValueUnitId
    {
      get => this._selectedStartValueUnitId;
      set
      {
        this._selectedStartValueUnitId = value;
        this.OnPropertyChanged(nameof (SelectedStartValueUnitId));
      }
    }

    public string StartValue
    {
      get => this._startValue;
      set
      {
        this._startValue = value;
        this.OnPropertyChanged(nameof (StartValue));
      }
    }

    public IEnumerable<MeasureUnitDTO> StartValueUnitCollection
    {
      get => MeasureUnitsHelper.GetMeasureUnits(this._measureUnitsRepository);
    }

    public double? EvaluationFactor
    {
      get => this._evaluationFactor;
      set
      {
        this._evaluationFactor = value;
        this.OnPropertyChanged(nameof (EvaluationFactor));
      }
    }

    public IEnumerable<MeasureUnitDTO> ImpulsUnitCollection
    {
      get => MeasureUnitsHelper.GetMeasureUnits(this._measureUnitsRepository);
    }

    public Guid? SelectedImpulsUnitId
    {
      get => this._selectedImpulsUnitId;
      set
      {
        this._selectedImpulsUnitId = value;
        this.OnPropertyChanged(nameof (SelectedImpulsUnitId));
      }
    }

    public IEnumerable<ChannelDTO> ChannelCollection
    {
      get
      {
        ObservableCollection<ChannelDTO> channelDTOList = new ObservableCollection<ChannelDTO>();
        TypeHelperExtensionMethods.ForEach<Channel>((IEnumerable<Channel>) this._channelRepository.GetAll().OrderBy<Channel, string>((Func<Channel, string>) (c => c.Code)), (Action<Channel>) (r => channelDTOList.Add(new ChannelDTO()
        {
          Id = r.Id,
          Code = r.Code,
          Name = string.IsNullOrEmpty(CultureResources.GetValue("Channel_" + r.Code)) ? "Channel_" + r.Code : CultureResources.GetValue("Channel_" + r.Code)
        })));
        return (IEnumerable<ChannelDTO>) channelDTOList.OrderBy<ChannelDTO, string>((Func<ChannelDTO, string>) (x => x.Name)).ToList<ChannelDTO>();
      }
    }

    public Guid? SelectedChannelId
    {
      get => this._selectedChannelId;
      set
      {
        this._selectedChannelId = value;
        this.OnPropertyChanged(nameof (SelectedChannelId));
      }
    }

    public IEnumerable<ConnectedDeviceTypeDTO> ConnectedDeviceTypeCollection
    {
      get => this._connectedDeviceTypeCollection;
      set
      {
        this._connectedDeviceTypeCollection = value;
        this.OnPropertyChanged(nameof (ConnectedDeviceTypeCollection));
      }
    }

    private void RefreshConnectedDeviceTypeCollection()
    {
      List<ConnectedDeviceTypeDTO> connectedDeviceTypeDTOList = new List<ConnectedDeviceTypeDTO>();
      TypeHelperExtensionMethods.ForEach<ConnectedDeviceType>((IEnumerable<ConnectedDeviceType>) this._connectedDeviceTypeRepository.GetAll().OrderBy<ConnectedDeviceType, string>((Func<ConnectedDeviceType, string>) (c => c.Code)), (Action<ConnectedDeviceType>) (r => connectedDeviceTypeDTOList.Add(new ConnectedDeviceTypeDTO()
      {
        Id = r.Id,
        Code = r.Code,
        Name = string.IsNullOrEmpty(CultureResources.GetValue("Connected_Device_Type_" + r.Code)) ? "Connected_Device_Type_" + r.Code : CultureResources.GetValue("Connected_Device_Type_" + r.Code)
      })));
      if (this.SelectedDeviceGroup != null && this.SelectedDeviceGroup.Name == "Water meters")
        connectedDeviceTypeDTOList = connectedDeviceTypeDTOList.Where<ConnectedDeviceTypeDTO>((Func<ConnectedDeviceTypeDTO, bool>) (i => i.Name == "Warm water" || i.Name == "Cold water")).ToList<ConnectedDeviceTypeDTO>();
      this.ConnectedDeviceTypeCollection = (IEnumerable<ConnectedDeviceTypeDTO>) connectedDeviceTypeDTOList.OrderBy<ConnectedDeviceTypeDTO, string>((Func<ConnectedDeviceTypeDTO, string>) (x => x.Name)).ToList<ConnectedDeviceTypeDTO>();
    }

    public Guid? SelectedConnectedDeviceTypeId
    {
      get => this._selectedConnectedDeviceTypeId;
      set
      {
        this._selectedConnectedDeviceTypeId = value;
        this.OnPropertyChanged(nameof (SelectedConnectedDeviceTypeId));
      }
    }

    public bool IsSaveAndCreateNewVisible { get; set; }

    public bool IsReplaceMeterButtonVisible { get; set; }

    public List<ReplacedMeterDTO> MeterReplacementHistoryCollection
    {
      get => this._meterReplacementHistoryCollection;
      set
      {
        this._meterReplacementHistoryCollection = value;
        this.OnPropertyChanged(nameof (MeterReplacementHistoryCollection));
      }
    }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    public string AES
    {
      get => this._AES;
      set
      {
        this._AES = value;
        this.OnPropertyChanged(nameof (AES));
      }
    }

    public int? PrimaryAddress
    {
      get => this._primaryAddress;
      set
      {
        this._primaryAddress = value;
        this.OnPropertyChanged(nameof (PrimaryAddress));
      }
    }

    public string Manufacturer
    {
      get => this._manufacturer;
      set
      {
        this._manufacturer = value;
        this.OnPropertyChanged(nameof (Manufacturer));
      }
    }

    public string Medium
    {
      get => this._medium;
      set
      {
        this._medium = this._mediumCollection.FirstOrDefault<KeyValuePair<DeviceMediumEnum, string>>((Func<KeyValuePair<DeviceMediumEnum, string>, bool>) (item => item.Key.ToString() == value)).Key.ToString();
        this.OnPropertyChanged(nameof (Medium));
      }
    }

    public string InputNumber
    {
      get => this._inputNumber;
      set
      {
        this._inputNumber = value;
        this.OnPropertyChanged(nameof (InputNumber));
      }
    }

    public string Generation
    {
      get => this._generation;
      set
      {
        this._generation = value;
        this.OnPropertyChanged(nameof (Generation));
      }
    }

    public List<MeterReplacementHistorySerializableDTO> MeterReplacementHistoryList { get; set; }

    public Guid? ReplacedMeterId { get; set; }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public bool IsSerialNumberEnabled
    {
      get => this._isSerialNumberEnabled;
      set
      {
        StructureTypeEnum? structureType = this._selectedNode.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
        {
          DeviceTypeEnum? nullable1 = new DeviceTypeEnum?();
          if (this._selectedDeviceType != null)
            nullable1 = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(this.SelectedDeviceType.Name);
          DeviceTypeEnum? nullable2;
          int num;
          if (nullable1.HasValue)
          {
            nullable2 = nullable1;
            DeviceTypeEnum deviceTypeEnum1 = DeviceTypeEnum.TemperatureSensor;
            if ((nullable2.GetValueOrDefault() == deviceTypeEnum1 ? (nullable2.HasValue ? 1 : 0) : 0) == 0)
            {
              nullable2 = nullable1;
              DeviceTypeEnum deviceTypeEnum2 = DeviceTypeEnum.HumiditySensor;
              if ((nullable2.GetValueOrDefault() == deviceTypeEnum2 ? (nullable2.HasValue ? 1 : 0) : 0) == 0)
                goto label_6;
            }
            num = 0;
            goto label_11;
          }
label_6:
          if (nullable1.HasValue)
          {
            nullable2 = nullable1;
            DeviceTypeEnum deviceTypeEnum = DeviceTypeEnum.C5Radio;
            if ((nullable2.GetValueOrDefault() == deviceTypeEnum ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
            {
              num = 1;
              goto label_11;
            }
          }
          num = !CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory") & value ? 1 : 0;
label_11:
          this._isSerialNumberEnabled = num != 0;
        }
        else
          this._isSerialNumberEnabled = value;
        this.OnPropertyChanged(nameof (IsSerialNumberEnabled));
      }
    }

    public bool IsDeviceGroupEnabled
    {
      get => this._isDeviceGroupEnabled;
      set
      {
        this._isDeviceGroupEnabled = value;
        this.OnPropertyChanged(nameof (IsDeviceGroupEnabled));
      }
    }

    public bool IsDeviceModelEnabled
    {
      get => this._isDeviceModelEnabled;
      set
      {
        this._isDeviceModelEnabled = value;
        this.OnPropertyChanged(nameof (IsDeviceModelEnabled));
      }
    }

    public Dictionary<string, string> DeviceInfoCollection
    {
      get => this._deviceInfoCollection;
      set
      {
        this._deviceInfoCollection = value;
        this.OnPropertyChanged(nameof (DeviceInfoCollection));
      }
    }

    public bool IsMeterGeneralTabSelected { get; set; }

    public bool IsRadioMeterGeneralTabSelected { get; set; }

    public bool IsMeterGeneralTabVisible { get; set; }

    public bool IsRadioMeterGeneralTabVisible { get; set; }

    public bool IsSpecificTabSelected { get; set; }

    public string MeasurementRange
    {
      get => this._measurementRange;
      set
      {
        this._measurementRange = value;
        this.OnPropertyChanged(nameof (MeasurementRange));
      }
    }

    public bool IsAddMeterButtonVisible { get; set; }

    public bool IsEditMeterButtonVisible
    {
      get => this._isEditMeterButtonVisible;
      set
      {
        this._isEditMeterButtonVisible = value && this.IsSaveAndReadAndWriteVisible;
        this.OnPropertyChanged(nameof (IsEditMeterButtonVisible));
      }
    }

    public bool IsSaveAndReadAndWriteVisible
    {
      get
      {
        return this._selectedNode.StructureType.HasValue && this._selectedNode.StructureType.Value != StructureTypeEnum.Fixed;
      }
    }

    public bool IsShortDeviceNoVisible
    {
      get
      {
        this._isShortDeviceNoVisible = false;
        foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).ShortDeviceNo))))
        {
          if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.DeviceTypeEnumBySelectedDeviceModel))
          {
            this._isShortDeviceNoVisible = true;
            break;
          }
        }
        if (this._isShortDeviceNoVisible)
        {
          bool flag = true;
          if (this._structureType.HasValue && this._structureType.Value == StructureTypeEnum.Fixed && this._selectedDeviceType != null)
          {
            DeviceTypeEnum? byDeviceModelName = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(this.SelectedDeviceType.Name);
            DeviceTypeEnum? nullable = byDeviceModelName;
            DeviceTypeEnum deviceTypeEnum1 = DeviceTypeEnum.MinotelContactRadio3;
            int num;
            if ((nullable.GetValueOrDefault() == deviceTypeEnum1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
            {
              nullable = byDeviceModelName;
              DeviceTypeEnum deviceTypeEnum2 = DeviceTypeEnum.MinomessMicroRadio3;
              if ((nullable.GetValueOrDefault() == deviceTypeEnum2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
              {
                nullable = byDeviceModelName;
                DeviceTypeEnum deviceTypeEnum3 = DeviceTypeEnum.C5Radio;
                if ((nullable.GetValueOrDefault() == deviceTypeEnum3 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                {
                  nullable = byDeviceModelName;
                  DeviceTypeEnum deviceTypeEnum4 = DeviceTypeEnum.TemperatureSensor;
                  if ((nullable.GetValueOrDefault() == deviceTypeEnum4 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                  {
                    nullable = byDeviceModelName;
                    DeviceTypeEnum deviceTypeEnum5 = DeviceTypeEnum.HumiditySensor;
                    num = nullable.GetValueOrDefault() == deviceTypeEnum5 ? (nullable.HasValue ? 1 : 0) : 0;
                    goto label_16;
                  }
                }
              }
            }
            num = 1;
label_16:
            if (num != 0)
              flag = false;
          }
          this.IsShortDeviceNoEnabled = flag;
        }
        else
          this.IsShortDeviceNoEnabled = true;
        return this._isShortDeviceNoVisible;
      }
      set => this._isShortDeviceNoVisible = value;
    }

    public bool IsShortDeviceNoEnabled
    {
      get => this._isShortDeviceNoEnabled;
      set
      {
        this._isShortDeviceNoEnabled = value;
        this.OnPropertyChanged(nameof (IsShortDeviceNoEnabled));
      }
    }

    public bool IsCompletDeviceIdVisible
    {
      get
      {
        this._isCompletDeviceIdVisible = false;
        this._isCompletDeviceIdVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).CompletDevice)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isCompletDeviceIdVisible;
      }
      set => this._isCompletDeviceIdVisible = value;
    }

    public bool IsRoomVisible
    {
      get
      {
        StructureTypeEnum? structureType = this._structureType;
        StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Physical;
        int num;
        if ((structureType.GetValueOrDefault() == structureTypeEnum1 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
        {
          structureType = this._structureType;
          StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Logical;
          num = structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0;
        }
        else
          num = 1;
        if (num != 0)
          return false;
        this._isRoomVisible = false;
        this._isRoomVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<RoomType>((Expression<Func<RoomType>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Room)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isRoomVisible;
      }
      set => this._isRoomVisible = value;
    }

    public bool IsStartValueVisible
    {
      get
      {
        this._isStartValueVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<double?>((Expression<Func<double?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).StartValue)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isStartValueVisible;
      }
      set => this._isStartValueVisible = value;
    }

    public bool IsEvaluationFactorVisible
    {
      get
      {
        this._isEvaluationFactorVisible = false;
        foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<double?>((Expression<Func<double?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).EvaluationFactor))))
        {
          if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.DeviceTypeEnumBySelectedDeviceModel) && this._selectedNode.RootNode.Entity is LocationDTO entity && entity.Scale == ScaleEnum.P && LicenseHelper.LicenseIsDisplayEvaluationFactor())
          {
            this._isEvaluationFactorVisible = true;
            break;
          }
        }
        return this._isEvaluationFactorVisible;
      }
      set => this._isEvaluationFactorVisible = value;
    }

    public bool IsImpulsValueVisible
    {
      get
      {
        this._isImpulsValueVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<double?>((Expression<Func<double?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).ImpulsValue)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isImpulsValueVisible;
      }
      set => this._isImpulsValueVisible = value;
    }

    public bool IsImpulsUnitVisible
    {
      get
      {
        this._isImpulsUnitVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<MeasureUnit>((Expression<Func<MeasureUnit>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).ImpulsUnit)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isImpulsUnitVisible;
      }
      set => this._isImpulsValueVisible = value;
    }

    public bool IsChannelVisible
    {
      get
      {
        this._isChannelVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<Channel>((Expression<Func<Channel>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Channel)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isChannelVisible;
      }
      set => this._isChannelVisible = value;
    }

    public bool IsConnectedDeviceTypeVisible
    {
      get
      {
        this._isConnectedDeviceTypeVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<ConnectedDeviceType>((Expression<Func<ConnectedDeviceType>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).ConnectedDeviceType)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isConnectedDeviceTypeVisible;
      }
      set => this._isConnectedDeviceTypeVisible = value;
    }

    public bool IsAESVisible
    {
      get
      {
        this._isAESVisible = false;
        foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).AES))))
        {
          if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.DeviceTypeEnumBySelectedDeviceModel))
          {
            this._isAESVisible = true;
            break;
          }
        }
        return this._isAESVisible;
      }
      set => this._isAESVisible = value;
    }

    public bool IsPrimaryAddressVisible
    {
      get
      {
        this._isPrimaryAddressVisible = false;
        if (this._selectedNode.NodeType.Name != "RadioMeter")
        {
          foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<int?>((Expression<Func<int?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).PrimaryAddress))))
          {
            if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.DeviceTypeEnumBySelectedDeviceModel))
            {
              this._isPrimaryAddressVisible = true;
              break;
            }
          }
        }
        return this._isPrimaryAddressVisible;
      }
      set => this._isPrimaryAddressVisible = value;
    }

    public bool IsManufacturerVisible
    {
      get
      {
        this._isManufacturerVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Manufacturer)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isManufacturerVisible;
      }
      set => this._isManufacturerVisible = value;
    }

    public bool IsMediumVisible
    {
      get
      {
        this._isMediumVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<DeviceMediumEnum?>((Expression<Func<DeviceMediumEnum?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Medium)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isMediumVisible;
      }
      set => this._isMediumVisible = value;
    }

    public bool IsInputNumberVisible
    {
      get
      {
        this._isInputNumberVisible = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).InputNumber)), this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isInputNumberVisible;
      }
      set => this._isInputNumberVisible = value;
    }

    public bool IsMeasurementRangeVisible
    {
      get
      {
        string propertyName = DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MeterRadioDetails)).dgMessbereich));
        this._isMeasurementRangeVisible = this._selectedNode.NodeType.Name == "Meter" && DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MeterRadioDetails), propertyName, this.DeviceTypeEnumBySelectedDeviceModel);
        return this._isMeasurementRangeVisible;
      }
      set => this._isMeasurementRangeVisible = value;
    }

    public bool IsGenerationVisible
    {
      get
      {
        this._isGenerationVisible = false;
        foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<string>((Expression<Func<string>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Generation))))
        {
          if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.DeviceTypeEnumBySelectedDeviceModel))
          {
            this._isGenerationVisible = true;
            break;
          }
        }
        return this._isGenerationVisible;
      }
      set => this._isGenerationVisible = value;
    }

    public System.Windows.Input.ICommand AddDeviceCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          List<string> source = this.ValidateFieldsBeforeSave();
          if (!this.IsValid)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
          }
          else
          {
            MeterDTO meterDto = this.CreateMeterDTO();
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            };
            EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
            {
              MeterDTO = meterDto,
              Node = this.isSaveAndCreateNewFirstTimePressed ? this._selectedNode : this.CreateSiblingStructureNodeDTO(this._selectedNode),
              Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
              Message = message,
              Name = this.Name,
              Description = this.Description
            }, (IViewModel) this);
            this.OnRequestClose(true);
          }
        }));
      }
    }

    public System.Windows.Input.ICommand EditDeviceCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (async _ =>
        {
          List<string> errorMessages = this.ValidateFieldsBeforeSave();
          if (!this.IsValid)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !errorMessages.Any<string>() || string.IsNullOrEmpty(errorMessages.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : errorMessages.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
          }
          else
          {
            StructureTypeEnum? structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
            if ((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 && CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"))
            {
              DeviceTypeEnum? selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
              if (selectedDeviceModel.HasValue)
              {
                switch (selectedDeviceModel.GetValueOrDefault())
                {
                  case DeviceTypeEnum.C5Radio:
                  case DeviceTypeEnum.M7:
                  case DeviceTypeEnum.MinomessMicroRadio3:
                  case DeviceTypeEnum.MinotelContactRadio3:
                    if (!this.AreReadWriteButtonsVisibile)
                    {
                      this.IsBusy = true;
                      await Task.Run((Action) (() =>
                      {
                        if (!this.ConfigureDevice())
                          return;
                        this.SaveMeterData();
                      }));
                      this.IsBusy = false;
                      goto label_10;
                    }
                    else
                      goto label_10;
                }
              }
              this.SaveMeterData();
            }
            else
              this.SaveMeterData();
label_10:;
          }
        }));
      }
    }

    private void SaveMeterData()
    {
      Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
      MeterDTO meterDto = this.CreateMeterDTO();
      if (this.isSaveAndCreateNewFirstTimePressed && this._selectedNode.Entity is MeterDTO entity)
        meterDto.Id = entity.Id;
      this.SetPreviousDeviceGroupAndModel();
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
      {
        MessageType = MessageTypeEnum.Success,
        MessageText = MessageCodes.Success_Save.GetStringValue()
      };
      this.UpdateMeterImageAfterEdit(meterDto.SerialNumber);
      EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
      {
        MeterDTO = meterDto,
        Node = this.isSaveAndCreateNewFirstTimePressed ? this._selectedNode : this.CreateSiblingStructureNodeDTO(this._selectedNode),
        Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
        Message = message,
        Name = this._name,
        Description = this._description
      }, (IViewModel) this);
      Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
    }

    public System.Windows.Input.ICommand CreateEditRuleCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          Dictionary<string, string> deviceInfoCollection = this.DeviceInfoCollection;
          List<string> list = deviceInfoCollection != null ? deviceInfoCollection.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (d => d.Key)).ToList<string>() : (List<string>) null;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<TranslationRulesViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("manufacturer", (object) this._manufacturer), (IParameter) new ConstructorArgument("medium", (object) this._medium), (IParameter) new ConstructorArgument("generation", (object) this._generation), (IParameter) new ConstructorArgument("zdfKeys", (object) list), (IParameter) new ConstructorArgument("selectedNode", (object) this._selectedNode)));
        }));
      }
    }

    public System.Windows.Input.ICommand SaveAndCreateNewCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (async parameter =>
        {
          this.IsValid = true;
          List<string> errorMessages = new List<string>();
          errorMessages.AddRange((IEnumerable<string>) this.ValidateFieldsBeforeSave());
          if (!this.IsValid)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !errorMessages.Any<string>() || string.IsNullOrEmpty(errorMessages.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : errorMessages.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
          }
          else
          {
            StructureTypeEnum? structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
            if ((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 && CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory") && !this.AreReadWriteButtonsVisibile)
            {
              this.IsBusy = true;
              if (this._selectedNode.RootNode.Entity is LocationDTO location2)
              {
                if (location2.Scenario != null)
                  location2.Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) location2.Scenario.Id);
                ParamsReturnedUsingIrExpando result;
                ref ParamsReturnedUsingIrExpando local = ref result;
                result = await DeviceConfigurator.WriteParametersForDevice(MSS.Business.Utils.AppContext.Current.DefaultEquipment, location2, this.CreateMeterDTO());
                if (result.IsSuccess)
                {
                  if (!string.IsNullOrEmpty(result.RadioId))
                    this.SerialNo = result.RadioId;
                }
                else
                {
                  errorMessages.Add(result.Message);
                  Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(result.Message)));
                }
              }
              this.IsBusy = false;
              location2 = (LocationDTO) null;
            }
            errorMessages.AddRange((IEnumerable<string>) this.ValidateProperty("SerialNo"));
            if (this.IsValid && errorMessages.Count == 0)
            {
              this.SaveMeterDataAndCreateNew();
            }
            else
            {
              MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Validation,
                MessageText = string.Join("\n", errorMessages.ToArray())
              };
              DeviceViewModel.logger.Log(NLog.LogLevel.Error, message.MessageText);
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
            }
          }
        }));
      }
    }

    private void UpdateMeterImageAfterEdit(string serialNumber)
    {
      StructureTypeEnum? structureType = this._selectedNode.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Physical;
      if (structureType.GetValueOrDefault() != structureTypeEnum || !structureType.HasValue || this.SerialNumberList.Contains(serialNumber))
        return;
      this._selectedNode.Image = new BitmapImage(new Uri(this._selectedNode.NodeType.IconPath));
    }

    private void SaveMeterDataAndCreateNew()
    {
      this.ConfigValuesCollection = new List<ConfigurationPerChannel>();
      this.IsConfigValuesVisible = false;
      MeterDTO meterDto = this.CreateMeterDTO();
      if (!this.IsAddMeterButtonVisible && this._selectedNode.Entity is MeterDTO entity)
        meterDto.Id = entity.Id;
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
      message.MessageType = MessageTypeEnum.Success;
      message.MessageText = MessageCodes.Success_Save.GetStringValue();
      this.UpdateMeterImageAfterEdit(meterDto.SerialNumber);
      EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
      {
        MeterDTO = meterDto,
        Node = this.isSaveAndCreateNewFirstTimePressed ? this._selectedNode : this.CreateSiblingStructureNodeDTO(this._selectedNode),
        Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
        Message = message,
        Name = this.Name,
        Description = this.Description
      }, (IViewModel) this);
      if (!this.SerialNumberList.Contains(meterDto.SerialNumber))
        this.SerialNumberList.Add(meterDto.SerialNumber);
      this.IsAddMeterButtonVisible = true;
      this.MeterDialogTitle = this._selectedNode.NodeType.Name == "Meter" ? CultureResources.GetValue("MSS_Client_Structures_CreateDevice_Title") : CultureResources.GetValue("MSS_Client_Structures_CreateRadioDevice_Title");
      this.isSaveAndCreateNewFirstTimePressed = false;
      this.SetPreviousDeviceGroupAndModel();
      this.ResetControls();
      this.Name = this._selectedNode.NodeType.Name == "Meter" ? CultureResources.GetValue("MSS_StructureNode_Meter") : CultureResources.GetValue("MSS_StructureNode_RadioMeter");
      this.Description = (string) null;
      this.SerialNo = string.Empty;
      this.IsReplaced = false;
      this.IsSerialNumberEnabled = true;
      this.IsDeviceGroupEnabled = true;
      this.IsDeviceModelEnabled = true;
      this.MeterReplacementHistoryList = (List<MeterReplacementHistorySerializableDTO>) null;
      this.ReplacedMeterId = new Guid?();
      this.MeterReplacementHistoryCollection = (List<ReplacedMeterDTO>) null;
      Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(message.MessageText)));
    }

    public System.Windows.Input.ICommand ReplaceMeterCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          List<string> source = this.ValidateFieldsBeforeSave();
          this.IsMeterBeingReplaced = true;
          if (!this.IsValid || this.IsMeterBeingReplaced && this.MeterToReplaceWith == null)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
            this.IsMeterBeingReplaced = false;
          }
          else
          {
            StructureTypeEnum? structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Physical;
            MeterDTO meterDto = (structureType.GetValueOrDefault() == structureTypeEnum1 ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? this.MeterToReplaceWith.MeterStructureNode.Entity as MeterDTO : this.CreateMeterDTO();
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
            message.MessageType = MessageTypeEnum.Success;
            message.MessageText = MessageCodes.Success_Save.GetStringValue();
            ReplaceDeviceEvent eventToPublish = new ReplaceDeviceEvent();
            eventToPublish.CurrentMeterDTO = meterDto;
            eventToPublish.ReplacedMeter = this._selectedNode;
            eventToPublish.Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id;
            eventToPublish.Message = message;
            eventToPublish.Name = this._selectedNode.Name;
            ReplaceDeviceEvent replaceDeviceEvent1 = eventToPublish;
            structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
            string str = (structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? this.MeterToReplaceWith.MeterStructureNode.Description : this.Description;
            replaceDeviceEvent1.Description = str;
            ReplaceDeviceEvent replaceDeviceEvent2 = eventToPublish;
            structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum3 = StructureTypeEnum.Physical;
            MSS.Core.Model.Structures.StructureNodeType structureNodeType;
            if ((structureType.GetValueOrDefault() == structureTypeEnum3 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
              structureNodeType = this._repositoryFactory.GetRepository<MSS.Core.Model.Structures.StructureNodeType>().FirstOrDefault((Expression<Func<MSS.Core.Model.Structures.StructureNodeType, bool>>) (item => item.Name == "Meter"));
            else
              structureNodeType = this.MeterToReplaceWith.MeterStructureNode.NodeType;
            replaceDeviceEvent2.CurrentMeterNodeType = structureNodeType;
            ReplaceDeviceEvent replaceDeviceEvent3 = eventToPublish;
            structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum4 = StructureTypeEnum.Physical;
            ObservableCollection<StructureNodeDTO> subNodes = (structureType.GetValueOrDefault() == structureTypeEnum4 ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? this.MeterToReplaceWith.MeterStructureNode.SubNodes : (ObservableCollection<StructureNodeDTO>) null;
            replaceDeviceEvent3.SubNodes = subNodes;
            ReplaceDeviceEvent replaceDeviceEvent4 = eventToPublish;
            structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum5 = StructureTypeEnum.Physical;
            List<Note> assignedNotes = (structureType.GetValueOrDefault() == structureTypeEnum5 ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? this.MeterToReplaceWith.MeterStructureNode.AssignedNotes : (List<Note>) null;
            replaceDeviceEvent4.AssignedNotes = assignedNotes;
            ReplaceDeviceEvent replaceDeviceEvent5 = eventToPublish;
            structureType = this._selectedNode.StructureType;
            StructureTypeEnum structureTypeEnum6 = StructureTypeEnum.Physical;
            List<byte[]> assignedPicture = (structureType.GetValueOrDefault() == structureTypeEnum6 ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? this.MeterToReplaceWith.MeterStructureNode.AssignedPicture : (List<byte[]>) null;
            replaceDeviceEvent5.AssignedPicture = assignedPicture;
            eventToPublish.MeterToReplaceWith = this.MeterToReplaceWith.MeterStructureNode;
            EventPublisher.Publish<ReplaceDeviceEvent>(eventToPublish, (IViewModel) this);
            this.IsMeterBeingReplaced = false;
            this.OnRequestClose(true);
          }
        }));
      }
    }

    public ProfileType ProfileType
    {
      get
      {
        List<ProfileType> profileTypes = GmmInterface.DeviceManager.GetProfileTypes(this.SelectedDeviceType, MSS.Business.Utils.AppContext.Current.DefaultEquipment, TransceiverType.Reader);
        return profileTypes == null || !profileTypes.Any<ProfileType>() ? (ProfileType) null : profileTypes[0];
      }
    }

    public bool IsConfigValuesVisible
    {
      get => this._isConfigValuesVisible;
      set
      {
        this._isConfigValuesVisible = value;
        this.OnPropertyChanged(nameof (IsConfigValuesVisible));
      }
    }

    public bool IsConfigChannel1Visible
    {
      get => this._isConfigChannel1Visible;
      set
      {
        this._isConfigChannel1Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel1Visible));
      }
    }

    public bool IsConfigChannel2Visible
    {
      get => this._isConfigChannel2Visible;
      set
      {
        this._isConfigChannel2Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel2Visible));
      }
    }

    public bool IsConfigChannel3Visible
    {
      get => this._isConfigChannel3Visible;
      set
      {
        this._isConfigChannel3Visible = value;
        this.OnPropertyChanged(nameof (IsConfigChannel3Visible));
      }
    }

    public List<ConfigurationPerChannel> ConfigValuesCollection
    {
      get => this._configValuesCollection;
      set
      {
        this._configValuesCollection = value;
        this.OnPropertyChanged(nameof (ConfigValuesCollection));
      }
    }

    public bool IsConfigured { get; set; }

    public bool IsReplaced { get; set; }

    public bool IsReadButtonEnabled
    {
      get => this._isReadButtonEnabled;
      set
      {
        this._isReadButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsReadButtonEnabled));
      }
    }

    public bool IsWriteButtonEnabled
    {
      get => this._isWriteButtonEnabled;
      set
      {
        this._isWriteButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsWriteButtonEnabled));
      }
    }

    public List<Config> DynamicGridTag { get; set; }

    public List<Config> Channel1DynamicGridTag { get; set; }

    public List<Config> Channel2DynamicGridTag { get; set; }

    public List<Config> Channel3DynamicGridTag { get; set; }

    public System.Windows.Input.ICommand ReadConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          List<string> source = this.ValidateFieldsBeforeSave();
          if (!this.IsValid || this.IsMeterBeingReplaced && this.MeterToReplaceWith == null)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
          }
          else
          {
            this.IsBusy = true;
            this.AreTabletButtonsEnabled = false;
            this.IsConfigValuesVisible = false;
            this.IsConfigChannel1Visible = false;
            this.IsConfigChannel2Visible = false;
            this.IsConfigChannel3Visible = false;
            ZENNER.CommonLibrary.Entities.Meter meter = new ZENNER.CommonLibrary.Entities.Meter()
            {
              ID = Guid.NewGuid(),
              DeviceModel = this.SelectedDeviceType,
              SerialNumber = this.SerialNo
            };
            try
            {
              this.ConfigValuesCollection = new DeviceConfigurationParameterCollector((IConfiguratorManager) new ConfiguratorManagerWrapper(), (IConfigurationValuesToListConfigConverter) new ConfigurationValuesToListConfigConverter()).Collect(GmmInterface.DeviceManager.GetConnectionAdjuster(meter.DeviceModel, MSS.Business.Utils.AppContext.Current.DefaultEquipment, this.ProfileType));
              this.AjustIsConfigGeneralVisibleValues(this.ConfigValuesCollection.Count);
              if (this.ConfigValuesCollection.Count > 0)
              {
                StructureTypeEnum? structureType = this._selectedNode.StructureType;
                StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
                if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
                {
                  foreach (ConfigurationPerChannel configValues in this.ConfigValuesCollection)
                  {
                    foreach (Config configValue in configValues.ConfigValues)
                    {
                      if (this._selectedNode.RootNode.Entity is LocationDTO entity2)
                      {
                        switch (configValue.PropertyName)
                        {
                          case "Radio protocol":
                            Scenario scenario = entity2.Scenario;
                            configValue.PropertyValue = "Scenario" + (object) scenario.Code;
                            configValue.IsReadOnly = true;
                            break;
                          case "Due date":
                            configValue.PropertyValue = entity2.DueDate.ToString();
                            configValue.IsReadOnly = true;
                            break;
                          case "EnumName_ZR_ClassLibrary.OverrideID_HCA_Scale":
                            configValue.PropertyValue = entity2.Scale.GetGMMScale();
                            configValue.IsReadOnly = true;
                            break;
                          case "Manipulation":
                            configValue.PropertyValue = "false";
                            configValue.IsReadOnly = true;
                            break;
                        }
                      }
                    }
                  }
                }
                this.IsConfigured = false;
                this.IsWriteButtonEnabled = true;
                EventPublisher.Publish<MeterConfigurationEvent>(new MeterConfigurationEvent()
                {
                  ConfigValuesPerChannelList = this.ConfigValuesCollection
                }, (IViewModel) this);
                this.IsConfigValuesVisible = true;
                Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation)));
              }
            }
            catch (Exception ex)
            {
              DeviceViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false);
            }
            finally
            {
              this.IsBusy = false;
              this.AreTabletButtonsEnabled = true;
            }
          }
        }));
      }
    }

    public System.Windows.Input.ICommand WriteConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          List<string> source = this.ValidateFieldsBeforeSave();
          if (!this.IsValid || this.IsMeterBeingReplaced && this.MeterToReplaceWith == null)
          {
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
            };
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
          }
          else
          {
            try
            {
              this.IsBusy = true;
              this.AreTabletButtonsEnabled = false;
              if (this.SetConfigParamsForTabs(parameter))
                return;
              this.configManager.WriteDevice();
              this.IsWriteButtonEnabled = false;
              this.IsConfigured = true;
              this.IsConfigValuesVisible = false;
              this.IsConfigChannel1Visible = false;
              this.IsConfigChannel2Visible = false;
              this.IsConfigChannel3Visible = false;
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation)));
            }
            catch (Exception ex)
            {
              DeviceViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false);
            }
            finally
            {
              this.IsBusy = false;
              this.AreTabletButtonsEnabled = true;
            }
          }
        }));
      }
    }

    private void ReadDeviceData()
    {
      this.IsValid = true;
      List<string> source = this.ValidateFieldsBeforeSave();
      if (!this.IsValid || this.IsMeterBeingReplaced && this.MeterToReplaceWith == null)
      {
        MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Validation,
          MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
        };
        Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
      }
      else
      {
        this.IsConfigValuesVisible = false;
        this.IsConfigChannel1Visible = false;
        this.IsConfigChannel2Visible = false;
        this.IsConfigChannel3Visible = false;
        ZENNER.CommonLibrary.Entities.Meter meter = new ZENNER.CommonLibrary.Entities.Meter()
        {
          ID = Guid.NewGuid(),
          DeviceModel = this.SelectedDeviceType,
          SerialNumber = this.SerialNo
        };
        if (this.ProfileType == null)
          throw new Exception(Resources.MSS_Client_InvalidProfileType);
        this.ConfigValuesCollection = new DeviceConfigurationParameterCollector((IConfiguratorManager) new ConfiguratorManagerWrapper(), (IConfigurationValuesToListConfigConverter) new ConfigurationValuesToListConfigConverter()).Collect(GmmInterface.DeviceManager.GetConnectionAdjuster(meter.DeviceModel, MSS.Business.Utils.AppContext.Current.DefaultEquipment, this.ProfileType));
        this.AjustIsConfigGeneralVisibleValues(this.ConfigValuesCollection.Count);
        if (this.ConfigValuesCollection.Count > 0)
        {
          StructureTypeEnum? structureType = this._selectedNode.StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
          if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
          {
            foreach (ConfigurationPerChannel configValues in this.ConfigValuesCollection)
            {
              foreach (Config configValue in configValues.ConfigValues)
              {
                if (this._selectedNode.RootNode.Entity is LocationDTO entity)
                {
                  switch (configValue.PropertyName)
                  {
                    case "Radio protocol":
                      Scenario scenario = entity.Scenario;
                      configValue.PropertyValue = "Scenario" + (object) scenario.Code;
                      configValue.IsReadOnly = true;
                      break;
                    case "Due date":
                      configValue.PropertyValue = entity.DueDate.ToString();
                      configValue.IsReadOnly = true;
                      break;
                    case "EnumName_ZR_ClassLibrary.OverrideID_HCA_Scale":
                      configValue.PropertyValue = entity.Scale.GetGMMScale();
                      configValue.IsReadOnly = true;
                      break;
                    case "Manipulation":
                      configValue.PropertyValue = "false";
                      configValue.IsReadOnly = true;
                      break;
                  }
                }
              }
            }
          }
          this.IsConfigured = false;
          this.IsWriteButtonEnabled = true;
          EventPublisher.Publish<MeterConfigurationEvent>(new MeterConfigurationEvent()
          {
            ConfigValuesPerChannelList = this.ConfigValuesCollection
          }, (IViewModel) this);
          this.IsConfigValuesVisible = true;
          Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation)));
        }
      }
    }

    private void WriteDeviceData(object parameter)
    {
      List<string> source = this.ValidateFieldsBeforeSave();
      if (!this.IsValid || this.IsMeterBeingReplaced && this.MeterToReplaceWith == null)
      {
        MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Validation,
          MessageText = !source.Any<string>() || string.IsNullOrEmpty(source.First<string>()) ? MessageCodes.ValidationError.GetStringValue() : source.First<string>()
        };
        Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(message.MessageText)));
      }
      else
      {
        try
        {
          bool result = false;
          Application.Current.Dispatcher.Invoke((Action) (() => result = this.SetConfigParamsForTabs(parameter)));
          if (result)
            return;
          this.configManager.WriteDevice();
          this.IsConfigured = true;
          Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation)));
        }
        catch (Exception ex)
        {
          DeviceViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
          Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false)));
        }
      }
    }

    public bool ConfigureDevice()
    {
      LocationDTO locationObj = this._selectedNode.RootNode.Entity as LocationDTO;
      bool flag = locationObj != null;
      try
      {
        if (CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"))
        {
          this.ReadDeviceData();
          if (this.ConfigValuesCollection != null && this.ConfigValuesCollection.Count > 0)
            this.SerialNo = this.ConfigValuesCollection[0].ConfigValues.FirstOrDefault<Config>((Func<Config, bool>) (p => p.PropertyName == "Serial number"))?.PropertyValue;
          Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            try
            {
              LocationDTO locationDto = locationObj;
              string str;
              if (locationDto == null)
              {
                str = (string) null;
              }
              else
              {
                DateTime? dueDate = locationDto.DueDate;
                ref DateTime? local = ref dueDate;
                str = local.HasValue ? local.GetValueOrDefault().ToString((IFormatProvider) CultureInfo.InvariantCulture) : (string) null;
              }
              string StringValue = str;
              // ISSUE: reference to a compiler-generated field
              int code = this._repositoryFactory.GetRepository<Scenario>().FirstOrDefault((Expression<Func<Scenario, bool>>) (item => item.Id == this.locationObj.Scenario.Id)).Code;
              SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.configManager.GetConfigurationParameters(0);
              if (configurationParameters == null)
                return;
              configurationParameters[OverrideID.DueDate].SetValueFromStringWin(StringValue);
              configurationParameters[OverrideID.RadioProtocol].SetValueFromStringWin(string.Format("Scenario{0}", (object) code));
              this.configManager.SetConfigurationParameters(configurationParameters);
            }
            catch (Exception ex)
            {
              MSSUIHelper.ShowWarningDialog(this._windowFactory, "Exception", ex.Message, false);
            }
          }));
          this.configManager.WriteDevice();
          this.ReadDeviceData();
        }
        return true;
      }
      catch (Exception ex)
      {
        DeviceViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
        Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, "Exception", ex.Message, false)));
        return false;
      }
    }

    private void AjustIsConfigGeneralVisibleValues(int count)
    {
      if (count > 0)
        this.IsConfigValuesVisible = true;
      if (count > 1)
        this.IsConfigChannel1Visible = true;
      if (count > 2)
        this.IsConfigChannel2Visible = true;
      if (count <= 3)
        return;
      this.IsConfigChannel3Visible = true;
    }

    private bool SetConfigParamsForTabs(object parameter)
    {
      if (!(parameter is CustomMultiBindingConverter.FindCommandParameters commandParameters))
        return true;
      SortedList<OverrideID, ConfigurationParameter> parameter1 = this.SetConfigurationParameters(this.DynamicGridTag, commandParameters.Property0);
      if (parameter1.Count == 0)
        return true;
      this.configManager.SetConfigurationParameters(parameter1);
      if (commandParameters.Property1 is UIElementCollection property1 && property1.Count > 0)
      {
        SortedList<OverrideID, ConfigurationParameter> parameter2 = this.SetConfigurationParameters(this.Channel1DynamicGridTag, commandParameters.Property1);
        if (parameter2 == null)
          return true;
        this.configManager.SetConfigurationParameters(parameter2, 1);
      }
      if (commandParameters.Property2 is UIElementCollection property2 && property2.Count > 0)
      {
        SortedList<OverrideID, ConfigurationParameter> parameter3 = this.SetConfigurationParameters(this.Channel2DynamicGridTag, commandParameters.Property2);
        if (parameter3 == null)
          return true;
        this.configManager.SetConfigurationParameters(parameter3, 2);
      }
      if (commandParameters.Property3 is UIElementCollection property3 && property3.Count > 0)
      {
        SortedList<OverrideID, ConfigurationParameter> parameter4 = this.SetConfigurationParameters(this.Channel3DynamicGridTag, commandParameters.Property3);
        if (parameter4 == null)
          return true;
        this.configManager.SetConfigurationParameters(parameter4, 3);
      }
      return false;
    }

    private SortedList<OverrideID, ConfigurationParameter> SetConfigurationParameters(
      List<Config> dynamicGridTag,
      object parameter = null)
    {
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      if (parameter != null)
      {
        GridControl grid = (parameter as UIElementCollection)[0] as GridControl;
        dynamicGridTag.UpdateCheckBoxInDynamicGrid(grid);
      }
      if (dynamicGridTag == null)
      {
        MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_Configuration_ReadShouldBeDoneFirst, false);
        return sortedList;
      }
      foreach (Config config in dynamicGridTag)
      {
        KeyValuePair<OverrideID, ConfigurationParameter> parameter1 = (KeyValuePair<OverrideID, ConfigurationParameter>) config.Parameter;
        if (parameter1.Key != OverrideID.AESKey || !(config.PropertyValue == "ZENNER DEFAULT KEY"))
        {
          if (config.PropertyValue == "")
          {
            sortedList.Add(parameter1.Key, parameter1.Value);
          }
          else
          {
            if (parameter1.Value.ParameterValue == null || parameter1.Value.ParameterValue.ToString() != config.PropertyValue)
              parameter1.Value.SetValueFromStringWin(config.PropertyValue);
            sortedList.Add(parameter1.Key, parameter1.Value);
          }
        }
      }
      return sortedList;
    }

    protected void GetPreviousDeviceGroupAndModel()
    {
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
      ApplicationParameter previousSelectedDeviceGroup = parametersManagement.GetAppParam("PreviousSelectedDeviceGroup");
      if (previousSelectedDeviceGroup.Value != null)
        this.SelectedDeviceGroup = this.DeviceGroupCollection.FirstOrDefault<DeviceGroup>((Func<DeviceGroup, bool>) (g => g.Name == previousSelectedDeviceGroup.Value));
      ApplicationParameter previousSelectedDeviceModel = parametersManagement.GetAppParam("PreviousSelectedDeviceModel");
      if (previousSelectedDeviceModel.Value == null || this.DeviceTypeCollection == null)
        return;
      this.SelectedDeviceType = this.DeviceTypeCollection.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (g => g.Name == previousSelectedDeviceModel.Value));
    }

    private void SetPreviousDeviceGroupAndModel()
    {
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
      ApplicationParameter appParam1 = parametersManagement.GetAppParam("PreviousSelectedDeviceGroup");
      appParam1.Value = this.SelectedDeviceGroup.Name;
      parametersManagement.Update(appParam1);
      ApplicationParameter appParam2 = parametersManagement.GetAppParam("PreviousSelectedDeviceModel");
      appParam2.Value = this.SelectedDeviceType.Name;
      parametersManagement.Update(appParam2);
    }

    private StructureNodeDTO CreateSiblingStructureNodeDTO(StructureNodeDTO selectedNode)
    {
      StructureNodeDTO newMeter = new StructureNodeDTO()
      {
        ParentNode = selectedNode.ParentNode,
        RootNode = selectedNode.RootNode,
        IsNewNode = true,
        StructureType = selectedNode.StructureType,
        NodeType = selectedNode.NodeType,
        Image = selectedNode.Image
      };
      StructureNodeDTO parent = selectedNode.ParentNode;
      Application.Current.Dispatcher.Invoke((Action) (() => parent.SubNodes.Add(newMeter)));
      return newMeter;
    }

    private void ResetControls()
    {
      this.ShortDeviceNo = string.Empty;
      this.CompleteDeviceId = string.Empty;
      this.SelectedRoomTypeId = new Guid?();
      this.StartValue = (string) null;
      this.EvaluationFactor = new double?();
      this.SelectedChannelId = new Guid?();
      this.SelectedConnectedDeviceTypeId = new Guid?();
      this.AES = string.Empty;
      this.PrimaryAddress = new int?();
      this.Manufacturer = string.Empty;
      this.Medium = string.Empty;
      this.Generation = string.Empty;
      this.SelectedChannelId = new Guid?();
      this.Impulses = new double?();
      this.SelectedImpulsUnitId = new Guid?();
      this.StartValue = (string) null;
      this.SelectedStartValueUnitId = new Guid?();
      this.InputNumber = string.Empty;
      this.SetDefaultValuesForWaterMetersDeviceGroup();
    }

    private MeterDTO CreateMeterDTO()
    {
      double result = 0.0;
      bool flag = double.TryParse(!string.IsNullOrWhiteSpace(this.StartValue) ? this.StartValue.Replace(',', '.') : this.StartValue, out result);
      MeterDTO meterDto1 = new MeterDTO();
      meterDto1.SerialNumber = this.SerialNo;
      meterDto1.ShortDeviceNo = this.IsShortDeviceNoVisible ? this.ShortDeviceNo : (string) null;
      meterDto1.CompletDevice = this.IsCompletDeviceIdVisible ? this.CompleteDeviceId : (string) null;
      MeterDTO meterDto2 = meterDto1;
      Guid? nullable;
      RoomType roomType;
      if (!this.IsRoomVisible)
      {
        roomType = (RoomType) null;
      }
      else
      {
        nullable = this.SelectedRoomTypeId;
        roomType = nullable.HasValue ? this._roomTypeRepository.GetById((object) this.SelectedRoomTypeId) : (RoomType) null;
      }
      meterDto2.Room = roomType;
      meterDto1.StartValue = ((!this.IsStartValueVisible ? 0 : (!string.IsNullOrWhiteSpace(this.StartValue) ? 1 : 0)) & (flag ? 1 : 0)) != 0 ? new double?(result) : new double?();
      meterDto1.EvaluationFactor = this.IsEvaluationFactorVisible ? this.EvaluationFactor : new double?();
      MeterDTO meterDto3 = meterDto1;
      MeasureUnit measureUnit;
      if (!this.IsStartValueVisible)
      {
        measureUnit = (MeasureUnit) null;
      }
      else
      {
        nullable = this.SelectedStartValueUnitId;
        measureUnit = nullable.HasValue ? this._measureUnitsRepository.GetById((object) this.SelectedStartValueUnitId) : (MeasureUnit) null;
      }
      meterDto3.ReadingUnit = measureUnit;
      meterDto1.ImpulsValue = this.IsImpulsUnitVisible ? this.Impulses : new double?();
      meterDto1.ImpulsUnit = this.IsImpulsUnitVisible ? this._measureUnitsRepository.GetById((object) this.SelectedImpulsUnitId) : (MeasureUnit) null;
      MeterDTO meterDto4 = meterDto1;
      Channel channel;
      if (!this.IsChannelVisible)
      {
        channel = (Channel) null;
      }
      else
      {
        nullable = this.SelectedChannelId;
        channel = nullable.HasValue ? this._channelRepository.GetById((object) this.SelectedChannelId) : (Channel) null;
      }
      meterDto4.Channel = channel;
      MeterDTO meterDto5 = meterDto1;
      ConnectedDeviceType connectedDeviceType;
      if (!this.IsConnectedDeviceTypeVisible)
      {
        connectedDeviceType = (ConnectedDeviceType) null;
      }
      else
      {
        nullable = this.SelectedConnectedDeviceTypeId;
        connectedDeviceType = nullable.HasValue ? this._connectedDeviceTypeRepository.GetById((object) this.SelectedConnectedDeviceTypeId) : (ConnectedDeviceType) null;
      }
      meterDto5.ConnectedDeviceType = connectedDeviceType;
      meterDto1.GMMParameters = this.ConfigValuesCollection == null || this.ConfigValuesCollection.Count == 0 ? (byte[]) null : ConfigurationHelper.SerializeConfigurationParameters(this.ConfigValuesCollection);
      meterDto1.ConfigDate = this.ConfigValuesCollection == null || this.ConfigValuesCollection.Count == 0 ? new DateTime?() : new DateTime?(DateTime.Now);
      meterDto1.IsConfigured = new bool?(this.IsConfigured);
      meterDto1.IsReplaced = this.IsReplaced;
      meterDto1.AES = this.AES;
      meterDto1.PrimaryAddress = this.PrimaryAddress;
      meterDto1.Manufacturer = this.Manufacturer;
      meterDto1.Medium = this.IsMediumVisible ? new DeviceMediumEnum?((DeviceMediumEnum) Enum.Parse(typeof (DeviceMediumEnum), this.Medium)) : new DeviceMediumEnum?();
      meterDto1.InputNumber = this.IsInputNumberVisible ? this.InputNumber : (string) null;
      meterDto1.Generation = this.Generation;
      meterDto1.MeterReplacementHistoryList = this.MeterReplacementHistoryList;
      meterDto1.ReplacedMeterId = this.ReplacedMeterId;
      meterDto1.ReadingEnabled = this.IsReadingEnabled;
      MeterDTO meterDto6 = meterDto1;
      if (this._selectedNode.NodeType.Name == "RadioMeter")
        meterDto6.MbusRadioMeter = new MbusRadioMeter()
        {
          City = this.City,
          Street = this.Street,
          HouseNumber = this.HouseNumber,
          HouseNumberSupplement = this.HouseNumberSupplement,
          ApartmentNumber = this.ApartmentNumber,
          ZipCode = this.ZipCode,
          FirstName = this.FirstName,
          LastName = this.LastName,
          Location = this.Location,
          RadioSerialNumber = this.RadioSerialNumber
        };
      if (this.DeviceTypeEnumBySelectedDeviceModel.HasValue)
        meterDto6.DeviceType = this.DeviceTypeEnumBySelectedDeviceModel.Value;
      if (this.IsMeasurementRangeVisible)
      {
        meterDto6.MeterRadioDetails = new List<MeterRadioDetails>();
        meterDto6.MeterRadioDetails.Add(new MeterRadioDetails()
        {
          dgMessbereich = this.MeasurementRange
        });
      }
      return meterDto6;
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<DeviceGroup>((Expression<Func<DeviceGroup>>) (() => this.SelectedDeviceGroup));
      string propertyName2 = this.GetPropertyName<DeviceModel>((Expression<Func<DeviceModel>>) (() => this.SelectedDeviceType));
      string propertyName3 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.SerialNo));
      string propertyName4 = this.GetPropertyName<Guid?>((Expression<Func<Guid?>>) (() => this.SelectedChannelId));
      string propertyName5 = this.GetPropertyName<double?>((Expression<Func<double?>>) (() => this.Impulses));
      string propertyName6 = this.GetPropertyName<Guid?>((Expression<Func<Guid?>>) (() => this.SelectedImpulsUnitId));
      string propertyName7 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.StartValue));
      string propertyName8 = this.GetPropertyName<Guid?>((Expression<Func<Guid?>>) (() => this.SelectedStartValueUnitId));
      List<string> source = new List<string>();
      if (propertyName == propertyName1)
      {
        if (this.SelectedDeviceGroup == null)
        {
          source.Add(Resources.MSS_Client_Structures_DeviceGroupErrorToolTip);
          this.IsValid = false;
        }
        return source.ToList<string>();
      }
      if (propertyName == propertyName2)
      {
        if (this.SelectedDeviceType == null)
        {
          source.Add(Resources.MSS_Client_Structures_DeviceModelErrorToolTip);
          this.IsValid = false;
        }
        return source.ToList<string>();
      }
      if (propertyName == propertyName3)
      {
        if (!string.IsNullOrEmpty(this.SerialNo) && !new Regex("^[a-zA-Z0-9]*$").IsMatch(this.SerialNo))
        {
          source.Add(Resources.MSS_METERS_ALPHANUMERIC_SERIAL_NUMBER);
          this.IsValid = false;
          return source.ToList<string>();
        }
        if (this.SerialNumberList.Contains(this.SerialNo))
        {
          source.Add(Resources.MSS_METERS_UNIQUE_SERIAL_NUMBER);
          this.IsValid = false;
          return source.ToList<string>();
        }
      }
      if (propertyName == propertyName4 && this.DeviceTypeEnumBySelectedDeviceModel.HasValue && this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinotelContactRadio3 && !this._selectedChannelId.HasValue)
      {
        source.Add(Resources.MSS_METERS_CHANNEL_MANDATORY);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (propertyName == propertyName5 && this.DeviceTypeEnumBySelectedDeviceModel.HasValue && (this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinotelContactRadio3 || this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinomessMicroRadio3) && !this._impulses.HasValue)
      {
        source.Add(Resources.MSS_Client_ImpulsesIsMandatory);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (propertyName == propertyName6 && this.DeviceTypeEnumBySelectedDeviceModel.HasValue && (this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinotelContactRadio3 || this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinomessMicroRadio3) && !this._selectedImpulsUnitId.HasValue)
      {
        source.Add(Resources.MSS_Client_ImpulseUnitIsMandatory);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (propertyName == propertyName7 && this.DeviceTypeEnumBySelectedDeviceModel.HasValue && (this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinotelContactRadio3 || this.DeviceTypeEnumBySelectedDeviceModel.Value == DeviceTypeEnum.MinomessMicroRadio3) && this._startValue == null)
      {
        source.Add(Resources.MSS_Client_StartValueIsMandatory);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (!(propertyName == propertyName8) || !this.DeviceTypeEnumBySelectedDeviceModel.HasValue || this.DeviceTypeEnumBySelectedDeviceModel.Value != DeviceTypeEnum.MinotelContactRadio3 && this.DeviceTypeEnumBySelectedDeviceModel.Value != DeviceTypeEnum.MinomessMicroRadio3 || this._selectedStartValueUnitId.HasValue)
        return source;
      source.Add(Resources.MSS_Client_StartValueUnitIsMandatory);
      this.IsValid = false;
      return source.ToList<string>();
    }

    protected List<ReplacedMeterDTO> GetMeterReplacementHistoryCollection()
    {
      this._meterReplacementHistoryCollection = new List<ReplacedMeterDTO>();
      if (this._selectedNode.Entity is MeterDTO entity)
      {
        int key = 0;
        if (entity.MeterReplacementHistoryList != null)
        {
          foreach (MeterReplacementHistorySerializableDTO replacementHistory1 in entity.MeterReplacementHistoryList)
          {
            this._meterReplacementHistoryCollection.Add(new ReplacedMeterDTO()
            {
              Id = key,
              DeviceType = replacementHistory1.ReplacedMeterDeviceType,
              ReplacementDate = replacementHistory1.ReplacementDate,
              SerialNumber = replacementHistory1.ReplacedMeterSerialNumber
            });
            ++key;
            if (entity.Id == replacementHistory1.CurrentMeterId)
            {
              System.Tuple<List<ReplacedMeterDTO>, int> replacementHistory2 = this.GetMeterReplacementHistory(this._meterReplacementHistoryCollection, replacementHistory1.ReplacedMeterId, key);
              this._meterReplacementHistoryCollection = replacementHistory2.Item1;
              key = replacementHistory2.Item2;
            }
          }
        }
        else
          this._meterReplacementHistoryCollection = this.GetMeterReplacementHistory(this._meterReplacementHistoryCollection, entity.Id, key).Item1;
      }
      return this._meterReplacementHistoryCollection;
    }

    private System.Tuple<List<ReplacedMeterDTO>, int> GetMeterReplacementHistory(
      List<ReplacedMeterDTO> meterReplacementHistoryCollection,
      Guid meterId,
      int key)
    {
      IRepository<MeterReplacementHistory> repository = this._repositoryFactory.GetRepository<MeterReplacementHistory>();
      Expression<Func<MeterReplacementHistory, bool>> predicate = (Expression<Func<MeterReplacementHistory, bool>>) (m => m.CurrentMeter.Id == meterId);
      foreach (MeterReplacementHistory replacementHistory in repository.Where(predicate).ToList<MeterReplacementHistory>())
      {
        ReplacedMeterDTO replacedMeterDto = new ReplacedMeterDTO()
        {
          Id = key,
          DeviceType = replacementHistory.ReplacedMeter.DeviceType,
          ReplacementDate = replacementHistory.ReplacementDate,
          SerialNumber = replacementHistory.ReplacedMeter.SerialNumber
        };
        meterReplacementHistoryCollection.Add(replacedMeterDto);
        ++key;
      }
      return new System.Tuple<List<ReplacedMeterDTO>, int>(meterReplacementHistoryCollection, key);
    }

    private void InitializeMediumCollection()
    {
      this._mediumCollection = EnumHelper.GetEnumTranslationsDictionary<DeviceMediumEnum>();
    }

    private void InitializeMeasurementRangeCollection()
    {
      this.MeasurementRangeCollection = EnumHelper.GetEnumTranslationsDictionary<MeasurementRangeEnum>().ToDictionary<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum, string>((Func<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum>) (kv => kv.Key), (Func<KeyValuePair<MeasurementRangeEnum, string>, string>) (kv => kv.Value));
      DeviceTypeEnum? selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
      int num1;
      if (selectedDeviceModel.HasValue)
      {
        selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
        num1 = selectedDeviceModel.Value == DeviceTypeEnum.MinotelContactRadio3 ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
      {
        this.MeasurementRangeCollection = this.MeasurementRangeCollection.Where<KeyValuePair<MeasurementRangeEnum, string>>((Func<KeyValuePair<MeasurementRangeEnum, string>, bool>) (d => this.GetMeasurementRangesForMinotelContactRadioR3().Contains(d.Key))).ToDictionary<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum, string>((Func<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum>) (kv => kv.Key), (Func<KeyValuePair<MeasurementRangeEnum, string>, string>) (kv => kv.Value));
      }
      else
      {
        selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
        int num2;
        if (selectedDeviceModel.HasValue)
        {
          selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
          if (selectedDeviceModel.Value != DeviceTypeEnum.EDCRadio)
          {
            selectedDeviceModel = this.DeviceTypeEnumBySelectedDeviceModel;
            num2 = selectedDeviceModel.Value == DeviceTypeEnum.MinomessMicroRadio3 ? 1 : 0;
          }
          else
            num2 = 1;
        }
        else
          num2 = 0;
        if (num2 != 0)
          this.MeasurementRangeCollection = this.MeasurementRangeCollection.Where<KeyValuePair<MeasurementRangeEnum, string>>((Func<KeyValuePair<MeasurementRangeEnum, string>, bool>) (d => this.GetMeasurementRangesForWaterMeters().Contains(d.Key))).ToDictionary<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum, string>((Func<KeyValuePair<MeasurementRangeEnum, string>, MeasurementRangeEnum>) (kv => kv.Key), (Func<KeyValuePair<MeasurementRangeEnum, string>, string>) (kv => kv.Value));
      }
      MeasurementRangeEnum result;
      bool flag = Enum.TryParse<MeasurementRangeEnum>(this.MeasurementRange, true, out result);
      if (string.IsNullOrWhiteSpace(this.MeasurementRange) || flag && this.MeasurementRangeCollection.ContainsKey(result))
        return;
      this.MeasurementRangeCollection[MeasurementRangeEnum.Other] = this.MeasurementRange ?? string.Empty;
      this.MeasurementRange = MeasurementRangeEnum.Other.ToString();
    }

    private List<MeasurementRangeEnum> GetMeasurementRangesForMinotelContactRadioR3()
    {
      return new List<MeasurementRangeEnum>()
      {
        MeasurementRangeEnum.MC0,
        MeasurementRangeEnum.MC1,
        MeasurementRangeEnum.MC2,
        MeasurementRangeEnum.MC3
      };
    }

    private List<MeasurementRangeEnum> GetMeasurementRangesForWaterMeters()
    {
      return new List<MeasurementRangeEnum>()
      {
        MeasurementRangeEnum.MB0,
        MeasurementRangeEnum.MB1
      };
    }

    private void InitializeMeterMbusRadioDetails(MbusRadioMeter meterMbusRadioDetails)
    {
      if (meterMbusRadioDetails == null)
        return;
      this.City = meterMbusRadioDetails.City;
      this.Street = meterMbusRadioDetails.Street;
      this.HouseNumber = meterMbusRadioDetails.HouseNumber;
      this.HouseNumberSupplement = meterMbusRadioDetails.HouseNumberSupplement;
      this.ApartmentNumber = meterMbusRadioDetails.ApartmentNumber;
      this.ZipCode = meterMbusRadioDetails.ZipCode;
      this.FirstName = meterMbusRadioDetails.FirstName;
      this.LastName = meterMbusRadioDetails.LastName;
      this.Location = meterMbusRadioDetails.Location;
      this.RadioSerialNumber = meterMbusRadioDetails.RadioSerialNumber;
    }

    private void InitializeRadioMeterDetails(MeterRadioDetails meterRadioDetails)
    {
      this.MeasurementRange = meterRadioDetails != null ? meterRadioDetails.dgMessbereich : string.Empty;
    }

    protected string GetTenantTitleString()
    {
      TenantDTO tenantParent = this.GetTenantParent(this._selectedNode);
      return "         " + Resources.MSS_StructureNode_Tenant + ":  " + (object) tenantParent?.TenantNr + " - " + tenantParent?.Name;
    }

    private TenantDTO GetTenantParent(StructureNodeDTO node)
    {
      StructureNodeDTO structureNodeDto = node;
      while (structureNodeDto.NodeType.Name != "Tenant" && structureNodeDto.ParentNode != null && structureNodeDto.ParentNode.Id != Guid.Empty)
        structureNodeDto = structureNodeDto.ParentNode;
      return structureNodeDto.Entity as TenantDTO;
    }

    private List<string> ValidateFieldsBeforeSave()
    {
      return new List<string>()
      {
        this.ValidateProperty("SelectedDeviceGroup").FirstOrDefault<string>(),
        this.ValidateProperty("SelectedDeviceType").FirstOrDefault<string>(),
        this.ValidateProperty("SelectedChannelId").FirstOrDefault<string>(),
        this.ValidateProperty("Impulses").FirstOrDefault<string>(),
        this.ValidateProperty("SelectedImpulsUnitId").FirstOrDefault<string>(),
        this.ValidateProperty("StartValue").FirstOrDefault<string>(),
        this.ValidateProperty("SelectedStartValueUnitId").FirstOrDefault<string>()
      }.Where<string>((Func<string, bool>) (item => !string.IsNullOrEmpty(item))).ToList<string>();
    }

    private void SetDefaultValuesForWaterMetersDeviceGroup()
    {
      if (this.SelectedDeviceGroup == null || !(this.SelectedDeviceGroup.Name == "Water meters"))
        return;
      if (this.IsImpulsValueVisible && !this.Impulses.HasValue)
        this.Impulses = new double?(1.0);
      Guid? nullable;
      int num1;
      if (this.IsImpulsUnitVisible)
      {
        nullable = this.SelectedImpulsUnitId;
        num1 = !nullable.HasValue ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
      {
        MeasureUnitDTO measureUnitDto = this.ImpulsUnitCollection.FirstOrDefault<MeasureUnitDTO>((Func<MeasureUnitDTO, bool>) (i => i.Name == "l"));
        if (measureUnitDto != null)
        {
          this.SelectedImpulsUnitId = new Guid?(measureUnitDto.Id);
        }
        else
        {
          nullable = new Guid?();
          this.SelectedImpulsUnitId = nullable;
        }
      }
      int num2;
      if (this.IsStartValueVisible)
      {
        nullable = this.SelectedStartValueUnitId;
        if (nullable.HasValue)
        {
          nullable = this.SelectedStartValueUnitId;
          num2 = nullable.Value == Guid.Empty ? 1 : 0;
        }
        else
          num2 = 1;
      }
      else
        num2 = 0;
      if (num2 != 0)
      {
        MeasureUnitDTO measureUnitDto = this.StartValueUnitCollection.FirstOrDefault<MeasureUnitDTO>((Func<MeasureUnitDTO, bool>) (i => i.Name == "m\u00B3"));
        if (measureUnitDto != null)
        {
          this.SelectedStartValueUnitId = new Guid?(measureUnitDto.Id);
        }
        else
        {
          nullable = new Guid?();
          this.SelectedStartValueUnitId = nullable;
        }
      }
    }
  }
}
