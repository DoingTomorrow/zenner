// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ExecuteInstallationOrderViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MinomatHandler;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Structures;
using MVVM.Commands;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  internal class ExecuteInstallationOrderViewModel : StructureViewModelBase
  {
    private readonly string _orderNumber;
    private readonly StructureNodeDTO _parentForSelectedNode;
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private IRepository<MinomatMeter> _minomatMeterRepository;
    private IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private IRepository<Minomat> _minomatRepository;
    private StructureNodeDTO _locationNode;
    private int _numberOfReadMeters;
    private int _numberOfMetersInStructure;
    private List<StructureNodeDTO> _metersInStructure = new List<StructureNodeDTO>();
    private WalkByTestManager walkByTestManager;
    private string _locationInfo = "";
    private bool _isNotLockedItem;
    private bool _isAddTenant;
    private bool _isAddMeter;
    private bool _isEditTenantOrMeter;
    private bool _isDeleteTenantOrMeter;
    private bool _isAddMaster;
    private bool _isAddSlave;
    private bool _isEditMasterOrSlave;
    private bool _isDeleteMasterOrSlave;
    private bool _isNetworkSetupEnabled;
    private StructureNodeDTO _selectedTenantStructureNode;
    private StructureNodeDTO _selectedMeterDtoStructureNode;
    private StructureNodeDTO _selectedSubMeterStructureNode;
    private MinomatStructure _selectedMasterStructureNode;
    private MinomatStructure _selectedSlaveStructureNode;
    private bool _isBusy;
    private bool _isPasteActive;
    private bool _isExpanded;
    private string _registeredDevicesPercentage;
    private string _registeredDevicesImageLocation;
    private bool _isShortDeviceNoVisible;

    [Inject]
    public ExecuteInstallationOrderViewModel(
      string orderNumber,
      StructureNodeDTO selectedNode,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._locationNode = selectedNode;
      this._orderNumber = orderNumber;
      this._parentForSelectedNode = selectedNode.ParentNode;
      this._nodesCollection.Add(selectedNode);
      this._minomatMeterRepository = this._repositoryFactory.GetRepository<MinomatMeter>();
      this._minomatRepository = this._repositoryFactory.GetRepository<Minomat>();
      this._minomatRadioDetailsRepository = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
      this.IsNotLockedItem = true;
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      Mapper.CreateMap<Minomat, MinomatSerializableDTO>();
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(this.UpdateStructure));
      this.StructureForSelectedNode = ExecuteInstallationOrderViewModel.GetStructureNodeCollection(new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) this._nodesCollection), (Func<StructureNodeDTO, bool>) (x => x.NodeType.Name != StructureNodeTypeEnum.MinomatMaster.ToString() && x.NodeType.Name != StructureNodeTypeEnum.MinomatSlave.ToString()));
      this.StructureForSelectedNode = new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) this.StructureForSelectedNode.OrderBy<StructureNodeDTO, int>((Func<StructureNodeDTO, int>) (structure => !(structure.Entity is TenantDTO entity) ? structure.OrderNr : entity.TenantNr)));
      this.CalculateNoOfDevicesForTenants(this.StructureForSelectedNode);
      this.SetLocationDetails(selectedNode);
      ObservableCollection<StructureNodeDTO> structureNodeCollection = ExecuteInstallationOrderViewModel.GetStructureNodeCollection(new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) this._nodesCollection), (Func<StructureNodeDTO, bool>) (x => x.NodeType.Name == StructureNodeTypeEnum.MinomatMaster.ToString() || x.NodeType.Name == StructureNodeTypeEnum.MinomatSlave.ToString()));
      List<Guid> minomatIds = new List<Guid>();
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) structureNodeCollection, (Action<StructureNodeDTO>) (minomatMaster =>
      {
        minomatIds.Add((minomatMaster.Entity as MinomatSerializableDTO).Id);
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) minomatMaster.SubNodes, (Action<StructureNodeDTO>) (minomatSlave => minomatIds.Add((minomatSlave.Entity as MinomatSerializableDTO).Id)));
      }));
      List<MinomatRadioDetails> list = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => minomatIds.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
      this.StructureForMinomats = new ObservableCollection<MinomatStructure>();
      foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) structureNodeCollection)
      {
        StructureNodeDTO currentMinomatMaster = structureNodeDto;
        ObservableCollection<MinomatStructure> observableCollection = (ObservableCollection<MinomatStructure>) null;
        if (currentMinomatMaster.SubNodes != null && currentMinomatMaster.SubNodes.Any<StructureNodeDTO>())
          observableCollection = new ObservableCollection<MinomatStructure>();
        MinomatRadioDetails minomatRadioDetails1 = list.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == (currentMinomatMaster.Entity as MinomatSerializableDTO).Id));
        MinomatStructure minomatStructure1 = new MinomatStructure();
        minomatStructure1.MinomatStructureNode = currentMinomatMaster;
        minomatStructure1.Location = minomatRadioDetails1?.Location;
        minomatStructure1.Description = minomatRadioDetails1?.Description;
        MinomatStructure minomatStructure2 = minomatStructure1;
        string str1;
        if (minomatRadioDetails1 == null)
        {
          str1 = (string) null;
        }
        else
        {
          MinomatStatusDevicesEnum? statusDevices = minomatRadioDetails1.StatusDevices;
          ref MinomatStatusDevicesEnum? local = ref statusDevices;
          str1 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
        }
        minomatStructure2.StatusDevices = str1;
        MinomatStructure minomatStructure3 = minomatStructure1;
        string str2;
        if (minomatRadioDetails1 == null)
        {
          str2 = (string) null;
        }
        else
        {
          MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails1.StatusNetwork;
          ref MinomatStatusNetworkEnum? local = ref statusNetwork;
          str2 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
        }
        minomatStructure3.StatusNetwork = str2;
        minomatStructure1.GSMStatus = minomatRadioDetails1 == null ? string.Empty : this.GetGSMTestReceptionString(minomatRadioDetails1.GSMStatus);
        minomatStructure1.MinomatStructureSubNodes = observableCollection;
        MinomatStructure minomatStructure4 = minomatStructure1;
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) currentMinomatMaster.SubNodes)
        {
          StructureNodeDTO currentMinomatSlave = subNode;
          MinomatRadioDetails minomatRadioDetails2 = list.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == (currentMinomatSlave.Entity as MinomatSerializableDTO).Id));
          ObservableCollection<MinomatStructure> structureSubNodes = minomatStructure4.MinomatStructureSubNodes;
          MinomatStructure minomatStructure5 = new MinomatStructure();
          minomatStructure5.MinomatStructureNode = currentMinomatSlave;
          minomatStructure5.Location = minomatRadioDetails2?.Location;
          minomatStructure5.Description = minomatRadioDetails2?.Description;
          MinomatStructure minomatStructure6 = minomatStructure5;
          string str3;
          if (minomatRadioDetails2 == null)
          {
            str3 = (string) null;
          }
          else
          {
            MinomatStatusDevicesEnum? statusDevices = minomatRadioDetails2.StatusDevices;
            ref MinomatStatusDevicesEnum? local = ref statusDevices;
            str3 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
          }
          minomatStructure6.StatusDevices = str3;
          MinomatStructure minomatStructure7 = minomatStructure5;
          string str4;
          if (minomatRadioDetails2 == null)
          {
            str4 = (string) null;
          }
          else
          {
            MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails2.StatusNetwork;
            ref MinomatStatusNetworkEnum? local = ref statusNetwork;
            str4 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
          }
          minomatStructure7.StatusNetwork = str4;
          minomatStructure5.GSMStatus = minomatRadioDetails2 == null ? string.Empty : this.GetGSMTestReceptionString(minomatRadioDetails2.GSMStatus);
          minomatStructure5.MinomatStructureSubNodes = (ObservableCollection<MinomatStructure>) null;
          MinomatStructure minomatStructure8 = minomatStructure5;
          structureSubNodes.Add(minomatStructure8);
        }
        this.StructureForMinomats.Add(minomatStructure4);
      }
      this.CalculateReceivedAndRegisteredPercentages();
      this.SetRegisteredDevicesForMinomats();
      this.ResetWalkByButtons();
      StructureTypeEnum? structureType = this._locationNode.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      this.IsShortDeviceNoVisible = structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue;
    }

    public string LocationInfo
    {
      get => this._locationInfo;
      set
      {
        this._locationInfo = value;
        this.OnPropertyChanged(nameof (LocationInfo));
      }
    }

    public bool IsNotLockedItem
    {
      get => this._isNotLockedItem;
      set
      {
        this._isNotLockedItem = value;
        this.OnPropertyChanged(nameof (IsNotLockedItem));
      }
    }

    public bool IsAddTenant
    {
      get => this._isAddTenant;
      set
      {
        this._isAddTenant = value;
        this.OnPropertyChanged(nameof (IsAddTenant));
      }
    }

    public bool IsAddMeter
    {
      get => this._isAddMeter;
      set
      {
        this._isAddMeter = value;
        this.OnPropertyChanged(nameof (IsAddMeter));
      }
    }

    public bool IsEditTenantOrMeter
    {
      get => this._isEditTenantOrMeter;
      set
      {
        this._isEditTenantOrMeter = value;
        this.OnPropertyChanged(nameof (IsEditTenantOrMeter));
      }
    }

    public bool IsDeleteTenantOrMeter
    {
      get => this._isDeleteTenantOrMeter;
      set
      {
        this._isDeleteTenantOrMeter = value;
        this.OnPropertyChanged(nameof (IsDeleteTenantOrMeter));
      }
    }

    public bool IsAddMaster
    {
      get => this._isAddMaster;
      set
      {
        this._isAddMaster = value;
        this.OnPropertyChanged(nameof (IsAddMaster));
      }
    }

    public bool IsAddSlave
    {
      get => this._isAddSlave;
      set
      {
        this._isAddSlave = value;
        this.OnPropertyChanged(nameof (IsAddSlave));
      }
    }

    public bool IsEditMasterOrSlave
    {
      get => this._isEditMasterOrSlave;
      set
      {
        this._isEditMasterOrSlave = value;
        this.OnPropertyChanged(nameof (IsEditMasterOrSlave));
      }
    }

    public bool IsDeleteMasterOrSlave
    {
      get => this._isDeleteMasterOrSlave;
      set
      {
        this._isDeleteMasterOrSlave = value;
        this.OnPropertyChanged(nameof (IsDeleteMasterOrSlave));
      }
    }

    public bool IsNetworkSetupEnabled
    {
      get => this._isNetworkSetupEnabled;
      set
      {
        this._isNetworkSetupEnabled = value;
        this.OnPropertyChanged(nameof (IsNetworkSetupEnabled));
      }
    }

    public StructureNodeDTO SelectedTenantStructureNode
    {
      get => this._selectedTenantStructureNode;
      set
      {
        if (this.SelectedMeterStructureNode != null)
          this.SelectedMeterStructureNode = (StructureNodeDTO) null;
        if (this.SelectedSubMeterStructureNode != null)
          this.SelectedSubMeterStructureNode = (StructureNodeDTO) null;
        this._selectedTenantStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedTenantStructureNode));
      }
    }

    public StructureNodeDTO SelectedMeterStructureNode
    {
      get => this._selectedMeterDtoStructureNode;
      set
      {
        if (this._selectedMeterDtoStructureNode != null)
        {
          this._selectedMeterDtoStructureNode = (StructureNodeDTO) null;
          this.OnPropertyChanged(nameof (SelectedMeterStructureNode));
        }
        if (this._selectedTenantStructureNode != null)
          this.SelectedTenantStructureNode = (StructureNodeDTO) null;
        if (this.SelectedSubMeterStructureNode != null)
          this.SelectedSubMeterStructureNode = (StructureNodeDTO) null;
        this._selectedMeterDtoStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedMeterStructureNode));
      }
    }

    public StructureNodeDTO SelectedSubMeterStructureNode
    {
      get => this._selectedSubMeterStructureNode;
      set
      {
        if (this._selectedMeterDtoStructureNode != null)
        {
          this._selectedMeterDtoStructureNode = (StructureNodeDTO) null;
          this.OnPropertyChanged("SelectedMeterStructureNode");
        }
        if (this._selectedSubMeterStructureNode != null)
        {
          this._selectedSubMeterStructureNode = (StructureNodeDTO) null;
          this.OnPropertyChanged(nameof (SelectedSubMeterStructureNode));
        }
        if (this._selectedTenantStructureNode != null)
        {
          this._selectedTenantStructureNode = (StructureNodeDTO) null;
          this.OnPropertyChanged("SelectedTenantStructureNode");
        }
        this._selectedSubMeterStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedSubMeterStructureNode));
      }
    }

    public MinomatStructure SelectedMasterStructureNode
    {
      get => this._selectedMasterStructureNode;
      set
      {
        if (this.SelectedSlaveStructureNode != null)
          this.SelectedSlaveStructureNode = (MinomatStructure) null;
        this._selectedMasterStructureNode = value;
        this.IsNetworkSetupEnabled = value != null && value.StatusNetwork != MinomatStatusNetworkEnum.NetworkOptimization.GetStringValue();
        this.OnPropertyChanged(nameof (SelectedMasterStructureNode));
      }
    }

    public MinomatStructure SelectedSlaveStructureNode
    {
      get => this._selectedSlaveStructureNode;
      set
      {
        if (this._selectedSlaveStructureNode != null)
        {
          this._selectedSlaveStructureNode = (MinomatStructure) null;
          this.OnPropertyChanged(nameof (SelectedSlaveStructureNode));
        }
        this.SelectedMasterStructureNode = (MinomatStructure) null;
        this._selectedSlaveStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedSlaveStructureNode));
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

    public bool IsPasteActive
    {
      get => this._isPasteActive;
      set
      {
        this._isPasteActive = value;
        this.OnPropertyChanged(nameof (IsPasteActive));
      }
    }

    public bool IsExpanded
    {
      get => this._isExpanded;
      set
      {
        this._isExpanded = value;
        if (this._isExpanded == value)
          return;
        this._isExpanded = value;
        this.OnPropertyChanged(nameof (IsExpanded));
      }
    }

    public string RegisteredDevicesPercentage
    {
      get => this._registeredDevicesPercentage;
      set
      {
        this._registeredDevicesPercentage = value;
        this.OnPropertyChanged(nameof (RegisteredDevicesPercentage));
      }
    }

    public string RegisteredDevicesImageLocation
    {
      get => this._registeredDevicesImageLocation;
      set
      {
        this._registeredDevicesImageLocation = value;
        this.OnPropertyChanged(nameof (RegisteredDevicesImageLocation));
      }
    }

    public ObservableCollection<StructureNodeDTO> StructureForSelectedNode { get; set; }

    public ObservableCollection<MinomatStructure> StructureForMinomats { get; set; }

    public bool IsShortDeviceNoVisible
    {
      get => this._isShortDeviceNoVisible;
      set
      {
        this._isShortDeviceNoVisible = value;
        this.OnPropertyChanged(nameof (IsShortDeviceNoVisible));
      }
    }

    public ICommand EditTenantOrMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO node = this.SelectedTenantStructureNode ?? this.SelectedMeterStructureNode ?? this.SelectedSubMeterStructureNode;
          if (node == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_OPERATION);
            this.IsEditTenantOrMeter = false;
          }
          else
          {
            this.SetParentAndRootNode(node);
            this.IsEditTenantOrMeter = false;
            this.EditSelectedStructureNode(node);
            this.CalculateReceivedAndRegisteredPercentages();
          }
        }));
      }
    }

    public ICommand EditMasterOrSlaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MinomatStructure selectedItem = this.SelectedMasterStructureNode ?? this.SelectedSlaveStructureNode;
          if (selectedItem == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_OPERATION);
            this.IsEditMasterOrSlave = false;
          }
          else
          {
            string registeredDevices = (selectedItem.MinomatStructureNode.Entity as MinomatSerializableDTO).NrOfRegisteredDevices;
            this.SetParentAndRootNode(selectedItem.MinomatStructureNode);
            this.IsEditMasterOrSlave = false;
            this.EditSelectedStructureNode(selectedItem.MinomatStructureNode, orderNumber: this._orderNumber);
            this._repositoryFactory.GetSession().Clear();
            Minomat source = this._minomatRepository.Where((Expression<Func<Minomat, bool>>) (item => item.Id == (selectedItem.MinomatStructureNode.Entity as MinomatSerializableDTO).Id)).FirstOrDefault<Minomat>();
            if (source == null)
              return;
            MinomatSerializableDTO updatedMinomatEntity = Mapper.Map<Minomat, MinomatSerializableDTO>(source);
            MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == updatedMinomatEntity.Id)).FirstOrDefault<MinomatRadioDetails>();
            updatedMinomatEntity.NrOfRegisteredDevices = registeredDevices;
            selectedItem.MinomatStructureNode.Entity = (object) updatedMinomatEntity;
            selectedItem.Location = minomatRadioDetails?.Location;
            selectedItem.Description = minomatRadioDetails?.Description;
            MinomatStructure minomatStructure1 = selectedItem;
            string str1;
            if (minomatRadioDetails == null)
            {
              str1 = (string) null;
            }
            else
            {
              MinomatStatusDevicesEnum? statusDevices = minomatRadioDetails.StatusDevices;
              ref MinomatStatusDevicesEnum? local = ref statusDevices;
              str1 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
            }
            minomatStructure1.StatusDevices = str1;
            MinomatStructure minomatStructure2 = selectedItem;
            string str2;
            if (minomatRadioDetails == null)
            {
              str2 = (string) null;
            }
            else
            {
              MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
              ref MinomatStatusNetworkEnum? local = ref statusNetwork;
              str2 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
            }
            minomatStructure2.StatusNetwork = str2;
            selectedItem.GSMStatus = minomatRadioDetails == null ? string.Empty : this.GetGSMTestReceptionString(minomatRadioDetails.GSMStatus);
          }
        }));
      }
    }

    public ICommand AddTenantCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO secondLevelNode = this.GetSecondLevelNode(StructureNodeTypeEnum.Tenant.ToString());
          this.SetParentAndRootNode(secondLevelNode);
          this._nodesCollection[0].SubNodes.Add(secondLevelNode);
          this.IsAddTenant = false;
          bool? nullable = this.EditSelectedStructureNode(secondLevelNode, orderNumber: this._orderNumber);
          if (nullable.HasValue && nullable.Value)
          {
            this.StructureForSelectedNode.Insert(0, secondLevelNode);
            this.SelectedTenantStructureNode = secondLevelNode;
          }
          else
            this._nodesCollection[0].SubNodes.Remove(secondLevelNode);
        }));
      }
    }

    public ICommand AddMasterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO node = this.GetSecondLevelNode(StructureNodeTypeEnum.MinomatMaster.ToString());
          this._nodesCollection[0].SubNodes.Add(node);
          this.IsAddMaster = false;
          bool? nullable = this.EditSelectedStructureNode(node, orderNumber: this._orderNumber);
          if (nullable.HasValue && nullable.Value)
          {
            MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (node.Entity as MinomatSerializableDTO).Id));
            ObservableCollection<MinomatStructure> structureForMinomats = this.StructureForMinomats;
            MinomatStructure minomatStructure1 = new MinomatStructure();
            minomatStructure1.MinomatStructureNode = node;
            minomatStructure1.Location = minomatRadioDetails?.Location;
            minomatStructure1.Description = minomatRadioDetails?.Description;
            MinomatStructure minomatStructure2 = minomatStructure1;
            string str1;
            if (minomatRadioDetails == null)
            {
              str1 = (string) null;
            }
            else
            {
              MinomatStatusDevicesEnum? statusDevices = minomatRadioDetails.StatusDevices;
              ref MinomatStatusDevicesEnum? local = ref statusDevices;
              str1 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
            }
            minomatStructure2.StatusDevices = str1;
            MinomatStructure minomatStructure3 = minomatStructure1;
            string str2;
            if (minomatRadioDetails == null)
            {
              str2 = (string) null;
            }
            else
            {
              MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
              ref MinomatStatusNetworkEnum? local = ref statusNetwork;
              str2 = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
            }
            minomatStructure3.StatusNetwork = str2;
            minomatStructure1.GSMStatus = minomatRadioDetails == null ? string.Empty : this.GetGSMTestReceptionString(minomatRadioDetails.GSMStatus);
            MinomatStructure minomatStructure4 = minomatStructure1;
            structureForMinomats.Insert(0, minomatStructure4);
            this.SelectedMasterStructureNode = this.StructureForMinomats.FirstOrDefault<MinomatStructure>((Func<MinomatStructure, bool>) (item => item.MinomatStructureNode == node));
          }
          else
            this._nodesCollection[0].SubNodes.Remove(node);
        }));
      }
    }

    public ICommand AddMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO parentNode = this.SelectedTenantStructureNode ?? this.SelectedMeterStructureNode;
          if (parentNode == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_ADD_METER);
            this.IsAddMeter = false;
          }
          else
          {
            StructureNodeDTO thirdLevelNode = this.GetThirdLevelNode(parentNode, StructureNodeTypeEnum.Meter.ToString());
            parentNode.SubNodes.Insert(0, thirdLevelNode);
            this.SetParentAndRootNode(thirdLevelNode);
            this.EditSelectedStructureNode(thirdLevelNode);
            this.CalculateReceivedAndRegisteredPercentages();
            this.IsAddMeter = false;
            StructureNodeDTO tenantParent = this.GetTenantParent(thirdLevelNode);
            StructureNodeDTO structureNodeDto = tenantParent.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter" && item.Id == Guid.Empty));
            if (structureNodeDto != null)
              tenantParent.SubNodes.Remove(structureNodeDto);
            if (tenantParent != null)
              (tenantParent.Entity as TenantDTO).NoOfDevices = new int?(tenantParent.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
            this.CalculateReceivedAndRegisteredPercentages();
          }
        }));
      }
    }

    public ICommand AddSlaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MinomatStructure masterStructureNode = this.SelectedMasterStructureNode;
          if (masterStructureNode == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_ADD_SLAVE);
            this.IsAddSlave = false;
          }
          else
          {
            StructureNodeDTO node = this.GetThirdLevelNode(masterStructureNode.MinomatStructureNode, StructureNodeTypeEnum.MinomatSlave.ToString());
            masterStructureNode.MinomatStructureNode.SubNodes.Insert(0, node);
            if (masterStructureNode.MinomatStructureSubNodes == null)
              masterStructureNode.MinomatStructureSubNodes = new ObservableCollection<MinomatStructure>();
            MinomatStructure minomatStructure = new MinomatStructure()
            {
              MinomatStructureNode = node,
              Location = (string) null,
              Description = (string) null,
              MinomatStructureSubNodes = (ObservableCollection<MinomatStructure>) null,
              StatusDevices = (string) null,
              StatusNetwork = (string) null,
              GSMStatus = (string) null
            };
            masterStructureNode.MinomatStructureSubNodes.Add(minomatStructure);
            this.SelectedSlaveStructureNode = minomatStructure;
            this.IsAddSlave = false;
            bool? nullable = this.EditSelectedStructureNode(node, orderNumber: this._orderNumber);
            if (!nullable.HasValue || !nullable.Value)
            {
              masterStructureNode.MinomatStructureNode.SubNodes.Remove(node);
              masterStructureNode.MinomatStructureSubNodes.Remove(minomatStructure);
            }
            else
            {
              MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (node.Entity as MinomatSerializableDTO).Id));
              if (minomatRadioDetails != null)
              {
                this.SelectedSlaveStructureNode.Location = minomatRadioDetails.Location;
                this.SelectedSlaveStructureNode.Description = minomatRadioDetails.Description;
                MinomatStructure slaveStructureNode1 = this.SelectedSlaveStructureNode;
                MinomatStatusDevicesEnum? statusDevices = minomatRadioDetails.StatusDevices;
                ref MinomatStatusDevicesEnum? local1 = ref statusDevices;
                string stringValue1 = local1.HasValue ? local1.GetValueOrDefault().GetStringValue() : (string) null;
                slaveStructureNode1.StatusDevices = stringValue1;
                MinomatStructure slaveStructureNode2 = this.SelectedSlaveStructureNode;
                MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
                ref MinomatStatusNetworkEnum? local2 = ref statusNetwork;
                string stringValue2 = local2.HasValue ? local2.GetValueOrDefault().GetStringValue() : (string) null;
                slaveStructureNode2.StatusNetwork = stringValue2;
                this.SelectedSlaveStructureNode.GSMStatus = this.GetGSMTestReceptionString(minomatRadioDetails.GSMStatus);
              }
            }
          }
        }));
      }
    }

    public ICommand DeleteTenantOrMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_MessageCodes_Delete, Resources.MSS_MessageCodes_DeleteGeneric, true);
          if (!nullable.HasValue || !nullable.Value)
            return;
          new StructuresManager(this._repositoryFactory).DeleteStructureNodeAndDescendants(this.SelectedMeterStructureNode ?? this.SelectedTenantStructureNode ?? this.SelectedSubMeterStructureNode);
          this.RemoveSelectedItemFromList(this.SelectedMeterStructureNode ?? this.SelectedTenantStructureNode ?? this.SelectedSubMeterStructureNode, this.StructureForSelectedNode);
          this.CalculateNoOfDevicesForTenants(this.StructureForSelectedNode);
          this.CalculateReceivedAndRegisteredPercentages();
        }));
      }
    }

    public ICommand DeleteMasterOrSlaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_MessageCodes_Delete, Resources.MSS_MessageCodes_DeleteGeneric, true);
          if (!nullable.HasValue || !nullable.Value)
            return;
          this.DeleteAssociatedMinomatMeters(this.SelectedMasterStructureNode?.MinomatStructureNode ?? this.SelectedSlaveStructureNode?.MinomatStructureNode);
          new StructuresManager(this._repositoryFactory).DeleteStructureNodeAndDescendants(this.SelectedMasterStructureNode?.MinomatStructureNode ?? this.SelectedSlaveStructureNode?.MinomatStructureNode);
          this.IsDeleteMasterOrSlave = false;
          Guid selectedNodeId = this.SelectedMasterStructureNode != null ? this.SelectedMasterStructureNode.MinomatStructureNode.Id : this.SelectedSlaveStructureNode.MinomatStructureNode.Id;
          this.RemoveSelectedItemFromList(this.SelectedMasterStructureNode?.MinomatStructureNode ?? this.SelectedSlaveStructureNode?.MinomatStructureNode, this.StructureForMinomats);
          this.RemoveSelectedNodeFromStructureById(selectedNodeId, this.StructureForSelectedNode[0].RootNode.SubNodes);
          if (this.SelectedMasterStructureNode != null)
            this.StructureForMinomats.Remove(this.SelectedMasterStructureNode);
          else if (this.SelectedSlaveStructureNode != null)
            this.StructureForMinomats.FirstOrDefault<MinomatStructure>((Func<MinomatStructure, bool>) (item => item.MinomatStructureSubNodes != null && item.MinomatStructureSubNodes.Contains(this.SelectedSlaveStructureNode)))?.MinomatStructureSubNodes.Remove(this.SelectedSlaveStructureNode);
          this.CalculateReceivedAndRegisteredPercentages();
        }));
      }
    }

    public ICommand RegisterDeviceUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto = this.SelectedMasterStructureNode?.MinomatStructureNode ?? this.SelectedSlaveStructureNode?.MinomatStructureNode;
          MinomatStructure selectedMinomatStructureItem = this.SelectedMasterStructureNode ?? this.SelectedSlaveStructureNode;
          MinomatSerializableDTO selectedItemDto = structureNodeDto.Entity as MinomatSerializableDTO;
          if (structureNodeDto == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_OPERATION);
            this.IsEditMasterOrSlave = false;
          }
          else
          {
            List<StructureNodeDTO> listOfMeters = new List<StructureNodeDTO>();
            this.GetMetersInStructure(this._locationNode, ref listOfMeters);
            if (listOfMeters.Count <= 300)
            {
              this.IsBusy = true;
              Task.Run<GMMMinomatConfiguratorResult>((Func<GMMMinomatConfiguratorResult>) (() => this.SaveAndRegisterMinomatMeters(listOfMeters.Select<StructureNodeDTO, MeterDTO>((Func<StructureNodeDTO, MeterDTO>) (meterStructureNodeDTO => meterStructureNodeDTO.Entity as MeterDTO)).ToList<MeterDTO>(), selectedItemDto))).ContinueWith((Action<Task<GMMMinomatConfiguratorResult>>) (p =>
              {
                this.IsBusy = false;
                GMMMinomatConfiguratorResult gmmConfiguratorResult = p.Result;
                if (gmmConfiguratorResult.IsSuccess)
                {
                  Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_ExecuteInstallationOrder_MeterRegistrationSuccessful)));
                  this.UpdateSelectedMinomat(selectedMinomatStructureItem);
                  this.CalculateReceivedAndRegisteredPercentages();
                }
                else
                  Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              }));
            }
            else
            {
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ManuallyAssignMetersViewModel>((IParameter) new ConstructorArgument("node", (object) structureNodeDto), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)));
              this.UpdateSelectedMinomat(selectedMinomatStructureItem);
              this.CalculateReceivedAndRegisteredPercentages();
            }
          }
        }));
      }
    }

    public ICommand NetworkSetupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (p =>
        {
          MinomatStructure selectedItem = this.SelectedMasterStructureNode ?? this.SelectedSlaveStructureNode;
          if (selectedItem == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_OPERATION);
            this.IsEditMasterOrSlave = false;
          }
          else
          {
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<NetworkSetupViewModel>((IParameter) new ConstructorArgument("masterNode", (object) selectedItem.MinomatStructureNode)));
            List<Guid> minomatIds = new List<Guid>();
            minomatIds.Add((selectedItem.MinomatStructureNode.Entity as MinomatSerializableDTO).Id);
            ObservableCollection<MinomatStructure> structureSubNodes = selectedItem.MinomatStructureSubNodes;
            if (structureSubNodes != null)
              TypeHelperExtensionMethods.ForEach<MinomatStructure>((IEnumerable<MinomatStructure>) structureSubNodes, (Action<MinomatStructure>) (minomatSlave => minomatIds.Add((minomatSlave.MinomatStructureNode.Entity as MinomatSerializableDTO).Id)));
            this._repositoryFactory.GetSession().Clear();
            List<MinomatRadioDetails> minomatRadioDetailsList = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => minomatIds.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
            minomatIds.ForEach((Action<Guid>) (id =>
            {
              MinomatStructure currentMinomat = id == (selectedItem.MinomatStructureNode.Entity as MinomatSerializableDTO).Id ? selectedItem : selectedItem.MinomatStructureSubNodes.FirstOrDefault<MinomatStructure>((Func<MinomatStructure, bool>) (item => (item.MinomatStructureNode.Entity as MinomatSerializableDTO).Id == id));
              if (currentMinomat == null)
                return;
              MinomatRadioDetails minomatRadioDetails = minomatRadioDetailsList.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == (currentMinomat.MinomatStructureNode.Entity as MinomatSerializableDTO).Id));
              if (minomatRadioDetails != null)
              {
                MinomatStructure minomatStructure = currentMinomat;
                string str;
                if (minomatRadioDetails == null)
                {
                  str = (string) null;
                }
                else
                {
                  MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
                  ref MinomatStatusNetworkEnum? local = ref statusNetwork;
                  str = local.HasValue ? local.GetValueOrDefault().GetStringValue() : (string) null;
                }
                minomatStructure.StatusNetwork = str;
                if ((currentMinomat.MinomatStructureNode.Entity as MinomatSerializableDTO).IsMaster)
                  currentMinomat.GSMStatus = this.GetGSMTestReceptionString((GSMTestReceptionState?) minomatRadioDetails?.GSMStatus);
              }
            }));
            this.IsNetworkSetupEnabled = this._selectedMasterStructureNode != null && this._selectedMasterStructureNode.StatusNetwork != MinomatStatusNetworkEnum.NetworkOptimization.GetStringValue();
          }
        }));
      }
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand OkCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand RepairModeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MinomatStructure minomatStructure = this.SelectedMasterStructureNode ?? this.SelectedSlaveStructureNode;
          StructureNodeDTO selectedMinomatNode = minomatStructure.MinomatStructureNode;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<RepairModeViewModel>("RepairModeViewModel", (IParameter) new ConstructorArgument("minomatNode", (object) selectedMinomatNode), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)));
          MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (selectedMinomatNode.Entity as MinomatSerializableDTO).Id));
          if (minomatRadioDetails == null)
            return;
          minomatStructure.StatusNetwork = ((Enum) (ValueType) minomatRadioDetails.StatusNetwork).GetStringValue();
          minomatStructure.StatusDevices = ((Enum) (ValueType) minomatRadioDetails.StatusDevices).GetStringValue();
        }));
      }
    }

    public ICommand StartWalkByCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          StructureNodeDTO structureNodeDto = this.SelectedTenantStructureNode ?? this.SelectedMeterStructureNode;
          this.walkByTestManager = new WalkByTestManager(this._repositoryFactory, Guid.Empty, (ProfileType) null);
          this.walkByTestManager.OnMeterValuesReceivedHandler += new EventHandler<MSS.Core.Model.Meters.Meter>(this.OnMeterValuesReceivedHandler);
          this.walkByTestManager.OnErrorReceivedHandler += new EventHandler(this.OnErrorReceivedHandler);
          this.walkByTestManager.StartReadingValues(structureNodeDto, new ProfileType());
          this.IsWalkByTestButtonEnabled = false;
          this.IsStopWalkByTestButtonEnabled = true;
          WalkByTestManager.IsWalkByTestStarted = true;
          this.GetMetersInStructure(structureNodeDto);
          this._numberOfReadMeters = 0;
          this._numberOfMetersInStructure = this._metersInStructure.Count<StructureNodeDTO>();
        }));
      }
    }

    public ICommand StopWalkByCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.walkByTestManager.OnMeterValuesReceivedHandler -= new EventHandler<MSS.Core.Model.Meters.Meter>(this.OnMeterValuesReceivedHandler);
          this.walkByTestManager.OnErrorReceivedHandler -= new EventHandler(this.OnErrorReceivedHandler);
          this.walkByTestManager.StopReadingValues();
          this.ResetWalkByButtons();
        }));
      }
    }

    public ICommand EditLocationCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditLocationViewModel>((IParameter) new ConstructorArgument("isExistingEntity", (object) true), (IParameter) new ConstructorArgument("node", (object) this._locationNode), (IParameter) new ConstructorArgument("locationNumberList", (object) this.locationNumberList)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            return;
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand ReportsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ReportsForTenantsViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("rootNode", (object) this.StructureForSelectedNode[0].RootNode)))));
      }
    }

    public ICommand CloseOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_CloseOrderQuestion), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) true)));
          if (!(newModalDialog.HasValue & newModalDialog.Value))
            return;
          ISession session = this._repositoryFactory.GetSession();
          session.BeginTransaction();
          IRepository<Order> repository = this._repositoryFactory.GetRepository<Order>();
          Order entity = repository.FirstOrDefault((Expression<Func<Order, bool>>) (item => item.InstallationNumber == this._orderNumber));
          if (entity != null)
          {
            entity.Status = StatusOrderEnum.Closed;
            repository.TransactionalUpdate(entity);
          }
          session.Transaction.Commit();
          Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
        }));
      }
    }

    public ICommand ShowRegisteredDevicesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          StructureNodeDTO structureNodeDto = this.SelectedMasterStructureNode?.MinomatStructureNode ?? this.SelectedSlaveStructureNode?.MinomatStructureNode;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<RegisteredDevicesForMinomatViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("selectedMinomatStructureNode", (object) structureNodeDto), (IParameter) new ConstructorArgument("rootNode", (object) this._locationNode)));
        }));
      }
    }

    private void OnMeterValuesReceivedHandler(object sender, MSS.Core.Model.Meters.Meter meter)
    {
      MeterDTO savedMeterDTO = Mapper.Map<MSS.Core.Model.Meters.Meter, MeterDTO>(meter);
      foreach (StructureNodeDTO structureNodeDto in this._metersInStructure)
      {
        StructureNodeDTO node = structureNodeDto;
        if (node.NodeType.Name == "Meter" && ((MeterDTO) node.Entity).Id == meter.Id && ((MeterDTO) node.Entity).SerialNumber == meter.SerialNumber)
        {
          ++this._numberOfReadMeters;
          Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            node.Entity = (object) savedMeterDTO;
            node.Image = node.SetImageNode();
          }));
        }
      }
      if (this._numberOfReadMeters != this._numberOfMetersInStructure)
        return;
      this.walkByTestManager.StopReadingValues();
      this.ResetWalkByButtons();
    }

    private void OnErrorReceivedHandler(object sender, object e) => this.ResetWalkByButtons();

    private void GetMetersInStructure(StructureNodeDTO selectedNode)
    {
      this._metersInStructure.Clear();
      if (selectedNode.NodeType.Name == "Meter")
        this._metersInStructure.Add(selectedNode);
      this.WalkStructure(selectedNode);
    }

    private void WalkStructure(StructureNodeDTO selectedNode)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) selectedNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter")
          this._metersInStructure.Add(subNode);
        this.WalkStructure(subNode);
      }
    }

    private static ObservableCollection<StructureNodeDTO> GetStructureNodeCollection(
      ObservableCollection<StructureNodeDTO> locationNode,
      Func<StructureNodeDTO, bool> condition)
    {
      StructureNodeDTO structureNodeDto = locationNode.FirstOrDefault<StructureNodeDTO>();
      return structureNodeDto != null ? new ObservableCollection<StructureNodeDTO>(structureNodeDto.SubNodes.Where<StructureNodeDTO>(condition)) : (ObservableCollection<StructureNodeDTO>) null;
    }

    private void SetParentAndRootNode(StructureNodeDTO node)
    {
      if (node.ParentNode != null || node.RootNode != null || this._parentForSelectedNode == null)
        return;
      StructureNodeDTO structureNodeDto = this._parentForSelectedNode.RootNode != this._parentForSelectedNode ? this._parentForSelectedNode.RootNode : this._parentForSelectedNode;
      node.ParentNode = this._parentForSelectedNode;
      node.RootNode = structureNodeDto;
    }

    private StructureNodeDTO GetSecondLevelNode(string nodeName)
    {
      StructureNodeType nodeType = this._repositoryFactory.GetRepository<StructureNodeType>().FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (x => x.Name == nodeName));
      return new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed)
      {
        ParentNode = this._nodesCollection[0],
        RootNode = this._nodesCollection[0]
      };
    }

    private void RemoveSelectedItemFromList(
      StructureNodeDTO structureNode,
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      if (structureNode == null)
      {
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_ADD_METER);
        this.IsDeleteTenantOrMeter = false;
      }
      else
      {
        this.RemoveSelectedNodeFromStructure(structureNode, nodeCollection);
        this.RemoveSerialNumberFromUniquenessList(structureNode);
        this.IsDeleteTenantOrMeter = false;
      }
    }

    private void RemoveSelectedItemFromList(
      StructureNodeDTO structureNode,
      ObservableCollection<MinomatStructure> minomatStructureCollection)
    {
      if (structureNode == null)
      {
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_ADD_METER);
        this.IsDeleteTenantOrMeter = false;
      }
      else
      {
        this.RemoveSelectedNodeFromMinomatStructureCollection(structureNode, minomatStructureCollection);
        this.RemoveSerialNumberFromUniquenessList(structureNode);
        this.IsDeleteTenantOrMeter = false;
      }
    }

    private void RemoveSelectedNodeFromMinomatStructureCollection(
      StructureNodeDTO selectedNode,
      ObservableCollection<MinomatStructure> minomatCollection)
    {
      foreach (MinomatStructure minomat in (Collection<MinomatStructure>) minomatCollection)
      {
        if (minomat.MinomatStructureNode == selectedNode)
        {
          minomatCollection.Remove(minomat);
          break;
        }
        if (minomat.MinomatStructureNode.SubNodes.Any<StructureNodeDTO>())
        {
          foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) minomat.MinomatStructureNode.SubNodes)
          {
            StructureNodeDTO subnode = subNode;
            MinomatStructure minomatStructure = minomatCollection.FirstOrDefault<MinomatStructure>((Func<MinomatStructure, bool>) (item => item.MinomatStructureNode.Id == subnode.Id));
            if (minomatStructure != null)
              minomatCollection.Remove(minomatStructure);
          }
        }
      }
    }

    private StructureNodeDTO GetThirdLevelNode(StructureNodeDTO parentNode, string nodeName)
    {
      StructureNodeType nodeType = this._repositoryFactory.GetRepository<StructureNodeType>().FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (x => x.Name == nodeName));
      return new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed)
      {
        ParentNode = parentNode,
        RootNode = parentNode.RootNode
      };
    }

    protected void UpdateStructure(ActionStructureAndEntitiesUpdate update)
    {
      this._node = update.Node;
      this._updatedLocation = update.Location;
      this._updateMeterDTO = update.MeterDTO;
      this._updatedTenant = update.Tenant;
      this._name = update.Name;
      this._description = update.Description;
      if (update.Location != null)
      {
        this.SetLocationDetails(update.Node);
        this._locationNode = update.Node;
      }
      if (this._updateMeterDTO != null)
        this._node.Entity = (object) this._updateMeterDTO;
      if (this._updatedTenant != null)
        this._node.Entity = (object) Mapper.Map<Tenant, TenantDTO>(this._updatedTenant);
      if (this._node != null)
      {
        this._node.Name = this._name;
        this._node.Description = this._description;
      }
      new StructuresManager(this._repositoryFactory).TransactionalUpdateStructure((IList<StructureNodeDTO>) new StructureNodeDTO[1]
      {
        this._node
      }, StructureTypeEnum.Fixed, (StructureNodeEquipmentSettings) null);
    }

    private void ResetWalkByButtons()
    {
      WalkByTestManager.IsWalkByTestStarted = false;
      this.IsWalkByTestButtonEnabled = true;
      this.IsStopWalkByTestButtonEnabled = false;
    }

    private GMMMinomatConfiguratorResult SaveAndRegisterMinomatMeters(
      List<MeterDTO> meterList,
      MinomatSerializableDTO selectedMinomat)
    {
      bool flag = true;
      ISession session = this._repositoryFactory.GetSession();
      try
      {
        Minomat minomat = Mapper.Map<MinomatSerializableDTO, Minomat>(selectedMinomat);
        GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(selectedMinomat.IsMaster, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
        instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
        List<Guid> meterIds = meterList.Select<MeterDTO, Guid>((Func<MeterDTO, Guid>) (item => item.Id)).ToList<Guid>();
        List<Guid> alreadyRegisteredIds = this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => item.Minomat.Id == minomat.Id && meterIds.Contains(item.Meter.Id))).Select<MinomatMeter, Guid>((Expression<Func<MinomatMeter, Guid>>) (item => item.Meter.Id)).ToList<Guid>();
        List<MeterDTO> list = meterList.Where<MeterDTO>((Func<MeterDTO, bool>) (item => !alreadyRegisteredIds.Contains(item.Id))).ToList<MeterDTO>();
        GMMMinomatConfiguratorResult configuratorResult = instance.RegisterDevicesOnMinomat(list, minomat);
        if (configuratorResult.IsSuccess)
        {
          session.BeginTransaction();
          Minomat byId = this._minomatRepository.GetById((object) minomat.Id);
          foreach (MeterDTO meterDto in list)
            this._minomatMeterRepository.TransactionalInsert(new MinomatMeter()
            {
              SignalStrength = 0,
              Status = new MeterStatusEnum?(MeterStatusEnum.Registered),
              Meter = this._meterRepository.GetById((object) meterDto.Id),
              Minomat = byId
            });
          MinomatRadioDetails entity = this._repositoryFactory.GetRepository<MinomatRadioDetails>().FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (r => r.Minomat.Id == minomat.Id));
          if (entity != null)
          {
            entity.NrOfRegisteredDevices = meterList.Count.ToString();
            entity.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.DevicesRegistered);
            this._repositoryFactory.GetRepository<MinomatRadioDetails>().TransactionalUpdate(entity);
          }
          session.Transaction.Commit();
        }
        return configuratorResult;
      }
      catch (Exception ex)
      {
        MSS.Business.Errors.MessageHandler.LogException(ex);
        flag = false;
        if (session.IsOpen && session.Transaction.IsActive)
          session.Transaction.Rollback();
        return new GMMMinomatConfiguratorResult()
        {
          IsSuccess = false,
          Message = ex.Message
        };
      }
    }

    private void SetLocationDetails(StructureNodeDTO selectedNode)
    {
      LocationDTO entity = selectedNode.Entity as LocationDTO;
      string str = Resources.MSS_ExecuteInstallationOrder_Location;
      if (entity != null)
        str = str + entity.BuildingNr + " - ";
      if (!string.IsNullOrEmpty(selectedNode.Description))
        str += selectedNode.Description;
      this.LocationInfo = str;
    }

    public override ObservableCollection<StructureNodeDTO> GetStructureCollection()
    {
      return this._nodesCollection;
    }

    private void GetMetersInStructure(
      StructureNodeDTO root,
      ref List<StructureNodeDTO> listOfMeters)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) root.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter")
          listOfMeters.Add(subNode);
        this.GetMetersInStructure(subNode, ref listOfMeters);
      }
    }

    private void DeleteAssociatedMinomatMeters(StructureNodeDTO minomatToDelete)
    {
      if (minomatToDelete.Entity == null || !(minomatToDelete.Entity is MinomatSerializableDTO) || !(minomatToDelete.Entity is MinomatSerializableDTO))
        return;
      MinomatSerializableDTO entity1 = minomatToDelete.Entity as MinomatSerializableDTO;
      List<Guid> minomatIds = new List<Guid>();
      minomatIds.Add(entity1.Id);
      if (entity1.IsMaster)
        this.GetMinomatsInStructure(minomatToDelete, ref minomatIds);
      List<MinomatMeter> list = this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => minomatIds.Contains(item.Minomat.Id))).ToList<MinomatMeter>();
      if (list != null && list.Any<MinomatMeter>())
      {
        ISession session = this._repositoryFactory.GetSession();
        session.BeginTransaction();
        foreach (MinomatMeter entity2 in list)
          this._minomatMeterRepository.TransactionalDelete(entity2);
        session.Transaction.Commit();
      }
    }

    private void GetMinomatsInStructure(StructureNodeDTO root, ref List<Guid> listOfMinomatIds)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) root.SubNodes)
      {
        if (subNode.Entity != null && subNode.Entity is MinomatSerializableDTO)
        {
          MinomatSerializableDTO entity = subNode.Entity as MinomatSerializableDTO;
          if (entity.Id != Guid.Empty)
            listOfMinomatIds.Add(entity.Id);
        }
        this.GetMinomatsInStructure(subNode, ref listOfMinomatIds);
      }
    }

    private void SetRegisteredDevicesForMinomats()
    {
      List<Guid> minomatIds = new List<Guid>();
      foreach (MinomatStructure structureForMinomat in (Collection<MinomatStructure>) this.StructureForMinomats)
      {
        minomatIds.Add((structureForMinomat.MinomatStructureNode.Entity as MinomatSerializableDTO).Id);
        this.GetMasterAndSlaveIds(ref minomatIds, structureForMinomat.MinomatStructureNode.SubNodes);
      }
      List<MinomatMeter> list = this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => minomatIds.Contains(item.Minomat.Id))).ToList<MinomatMeter>();
      foreach (MinomatStructure structureForMinomat in (Collection<MinomatStructure>) this.StructureForMinomats)
      {
        MinomatStructure currentMinomatStructure = structureForMinomat;
        int num = list.Count<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
        {
          Guid id = item.Minomat.Id;
          Guid? nullable = currentMinomatStructure.MinomatStructureNode.Entity is MinomatSerializableDTO entity2 ? new Guid?(entity2.Id) : new Guid?();
          return nullable.HasValue && id == nullable.GetValueOrDefault();
        }));
        (currentMinomatStructure.MinomatStructureNode.Entity as MinomatSerializableDTO).NrOfRegisteredDevices = num > 0 ? num.ToString() : "0";
        this.SetRegisteredDevicesFromList(list, currentMinomatStructure.MinomatStructureNode.SubNodes);
      }
    }

    private void GetMasterAndSlaveIds(
      ref List<Guid> minomatIds,
      ObservableCollection<StructureNodeDTO> structure)
    {
      foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) structure)
      {
        if (structureNodeDto.Entity is MinomatSerializableDTO && structureNodeDto.Entity is MinomatSerializableDTO entity && entity.Id != Guid.Empty)
          minomatIds.Add(entity.Id);
        this.GetMasterAndSlaveIds(ref minomatIds, structureNodeDto.SubNodes);
      }
    }

    private void SetRegisteredDevicesFromList(
      List<MinomatMeter> registeredDevicesForMinomats,
      ObservableCollection<StructureNodeDTO> structureToUpdate)
    {
      foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) structureToUpdate)
      {
        StructureNodeDTO node = structureNodeDto;
        if (node.Entity is MinomatSerializableDTO)
        {
          int num = registeredDevicesForMinomats.Count<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
          {
            Guid id = item.Minomat.Id;
            Guid? nullable = node.Entity is MinomatSerializableDTO entity2 ? new Guid?(entity2.Id) : new Guid?();
            return nullable.HasValue && id == nullable.GetValueOrDefault();
          }));
          (node.Entity as MinomatSerializableDTO).NrOfRegisteredDevices = num > 0 ? num.ToString() : "0";
        }
        this.SetRegisteredDevicesFromList(registeredDevicesForMinomats, node.SubNodes);
      }
    }

    private void CalculateNoOfDevicesForTenants(
      ObservableCollection<StructureNodeDTO> nodesCollection)
    {
      foreach (StructureNodeDTO nodes in (Collection<StructureNodeDTO>) nodesCollection)
      {
        if (nodes.Entity is TenantDTO entity)
        {
          int? nullable = new int?(nodes.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          entity.NoOfDevices = nullable;
        }
        this.CalculateNoOfDevicesForTenants(nodes.SubNodes);
      }
    }

    private StructureNodeDTO GetTenantParent(StructureNodeDTO node)
    {
      StructureNodeDTO tenantParent = node;
      while (tenantParent.ParentNode != null && tenantParent.ParentNode.Id != Guid.Empty && tenantParent.NodeType.Name != "Tenant")
        tenantParent = tenantParent.ParentNode;
      return tenantParent;
    }

    private void UpdateSelectedMinomat(MinomatStructure selectedMinomat)
    {
      Guid minomatEntityId = (selectedMinomat.MinomatStructureNode.Entity as MinomatSerializableDTO).Id;
      this._minomatRepository.Refresh((object) minomatEntityId);
      MinomatSerializableDTO minomatSerializableDto = Mapper.Map<Minomat, MinomatSerializableDTO>(this._minomatRepository.FirstOrDefault((Expression<Func<Minomat, bool>>) (item => item.Id == minomatEntityId)));
      this._repositoryFactory.GetSession().Clear();
      int num = this._repositoryFactory.GetRepository<MinomatMeter>().Where((Expression<Func<MinomatMeter, bool>>) (item => item.Minomat.Id == minomatEntityId)).ToList<MinomatMeter>().Count<MinomatMeter>();
      minomatSerializableDto.NrOfRegisteredDevices = num > 0 ? num.ToString() : "0";
      if (string.IsNullOrEmpty(selectedMinomat.StatusDevices) || selectedMinomat.StatusDevices == MinomatStatusDevicesEnum.Open.GetStringValue())
      {
        MinomatRadioDetails entity = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (selectedMinomat.MinomatStructureNode.Entity as MinomatSerializableDTO).Id));
        if (entity != null)
        {
          if (num == 0 && string.IsNullOrEmpty(selectedMinomat.StatusDevices))
            entity.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.Open);
          else if (num > 0)
            entity.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.DevicesRegistered);
          this._minomatRadioDetailsRepository.Update(entity);
          selectedMinomat.StatusDevices = ((Enum) (ValueType) entity.StatusDevices).GetStringValue();
        }
      }
      selectedMinomat.MinomatStructureNode.Entity = (object) minomatSerializableDto;
    }

    private void CalculateReceivedAndRegisteredPercentages()
    {
      int num1 = 0;
      int num2 = 0;
      List<StructureNodeDTO> list1 = this._locationNode.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Entity is TenantDTO)).ToList<StructureNodeDTO>();
      List<Guid> meterIdsList = new List<Guid>();
      List<Guid> minomatIdsList = new List<Guid>();
      foreach (StructureNodeDTO structureNodeDto in list1)
        meterIdsList.AddRange((IEnumerable<Guid>) structureNodeDto.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Entity is MeterDTO)).Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (item => (item.Entity as MeterDTO).Id)).ToList<Guid>());
      this._locationNode.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Entity is MinomatSerializableDTO)).ToList<StructureNodeDTO>().ForEach((Action<StructureNodeDTO>) (minomatMaster =>
      {
        minomatIdsList.Add((minomatMaster.Entity as MinomatSerializableDTO).Id);
        minomatIdsList.AddRange((IEnumerable<Guid>) minomatMaster.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Entity is MinomatSerializableDTO)).Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (item => (item.Entity as MinomatSerializableDTO).Id)).ToList<Guid>());
      }));
      List<MinomatMeter> list2 = this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => meterIdsList.Contains(item.Meter.Id) && minomatIdsList.Contains(item.Minomat.Id))).ToList<MinomatMeter>();
      int num3 = num1 + meterIdsList.Count;
      foreach (Guid guid in meterIdsList)
      {
        Guid currentMeterId = guid;
        IEnumerable<MinomatMeter> source = list2.Where<MinomatMeter>((Func<MinomatMeter, bool>) (item => item.Meter.Id == currentMeterId));
        if (source.Any<MinomatMeter>() && source.Any<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
        {
          MeterStatusEnum? status = item.Status;
          MeterStatusEnum meterStatusEnum = MeterStatusEnum.Registered;
          return status.GetValueOrDefault() == meterStatusEnum && status.HasValue;
        })))
          ++num2;
      }
      double num4 = num3 != 0 ? (double) num2 / (double) num3 * 100.0 : 0.0;
      this.RegisteredDevicesPercentage = num3 != 0 ? string.Format("{0:0.00}%", (object) num4) : "";
      this.RegisteredDevicesImageLocation = num4 < 0.0 || num4 >= 85.0 ? (num4 < 85.0 || num4 >= 95.0 ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png") : "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
    }

    private string GetGSMTestReceptionString(GSMTestReceptionState? gsmState)
    {
      if (!gsmState.HasValue)
        return Resources.MSS_Client_GSMState_Empty_NotStarted;
      GSMTestReceptionState? nullable = gsmState;
      if (nullable.HasValue)
      {
        switch (nullable.GetValueOrDefault())
        {
          case GSMTestReceptionState.NotStartet:
            return Resources.MSS_Client_GSMTestReceptionState_NotStarted;
          case GSMTestReceptionState.NotComplete:
            return Resources.MSS_Client_GSMTestReceptionState_NotComplete;
          case GSMTestReceptionState.Successful:
            return Resources.MSS_Client_GSMTestReceptionState_Successful;
          case GSMTestReceptionState.Failed:
            return Resources.MSS_Client_GSMTestReceptionState_Failed;
        }
      }
      return Resources.MSS_Client_GSMState_Empty_NotStarted;
    }
  }
}
