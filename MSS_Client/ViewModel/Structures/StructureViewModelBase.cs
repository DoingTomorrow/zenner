// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.StructureViewModelBase
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.DataCollectors;
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
using MSS_Client.ViewModel.Structures.DeviceViewModels;
using MSS_Client.ViewModel.Structures.Helpers.DeviceHelpers;
using MVVM.ViewModel;
using NHibernate.Criterion;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Xml;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public abstract class StructureViewModelBase : ViewModelBase
  {
    protected readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    protected readonly IRepository<MSS.Core.Model.Meters.Meter> _meterRepository;
    protected readonly IRepository<MeterReplacementHistory> _meterReplacementHistoryRepository;
    protected readonly IRepository<StructureNode> _structureNodeRepository;
    protected readonly IRepository<StructureNodeLinks> _structureNodeLinkRepository;
    protected readonly IRepositoryFactory _repositoryFactory;
    protected readonly IWindowFactory _windowFactory;
    protected MeterDTO _updateMeterDTO;
    protected MSS.Core.Model.Structures.Location _updatedLocation;
    protected Tenant _updatedTenant;
    protected Minomat _updatedMinomat;
    protected string _description;
    protected string _name;
    protected StructureNodeDTO _node;
    protected readonly List<StructureNodeDTO> _replacedMeterList = new List<StructureNodeDTO>();
    protected List<string> serialNumberList = new List<string>();
    protected List<string> locationNumberList = new List<string>();
    protected static List<string> deviceModelsInLicense = new List<string>();
    protected HashSet<string> _serialNumbersOfMetersThatAreMissingTranslationRules;
    private StructureNodeDTO _selectedItem;
    private bool _isChangeDeviceModelParametersEnabled;
    private bool _isNodeSelected;
    private bool _isRootItemSelected;
    private bool _isReplacebleMeterSelected;
    private string _devicesFoundLabel;
    private ViewModelBase _messageUserControl;
    private List<ZENNER.CommonLibrary.Entities.Meter> _physicalSubMeters = new List<ZENNER.CommonLibrary.Entities.Meter>();
    private List<ZENNER.CommonLibrary.Entities.Meter> _fixedSubMeters = new List<ZENNER.CommonLibrary.Entities.Meter>();
    private bool _isStartMBusScanButtonEnabled;
    private bool _isStopMBusScanButtonEnabled;
    private bool _isWalkByTestButtonEnabled;
    private bool _isStopWalkByTestButtonEnabled;
    private bool _isRadioSelected;
    private bool _isMeterSelected;

    protected StructureNodeEquipmentSettings StructureEquipmentSettings { get; set; }

    [Inject]
    protected StructureViewModelBase(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._meterRepository = repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeLinkRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._meterReplacementHistoryRepository = repositoryFactory.GetRepository<MeterReplacementHistory>();
      this.serialNumberList = this._repositoryFactory.GetSession().CreateCriteria<MSS.Core.Model.Meters.Meter>().SetProjection((IProjection) Projections.Property("SerialNumber")).Add((ICriterion) Restrictions.Eq("IsDeactivated", (object) false)).List<string>().ToList<string>();
      this.locationNumberList = this._repositoryFactory.GetSession().CreateCriteria<MSS.Core.Model.Structures.Location>().SetProjection((IProjection) Projections.Property("BuildingNr")).Add((ICriterion) Restrictions.Eq("IsDeactivated", (object) false)).List<string>().ToList<string>();
      EventPublisher.Register<ExpandoObject>((Action<ExpandoObject>) (s => this.SetStartAndStopScanVisibility()));
      this._serialNumbersOfMetersThatAreMissingTranslationRules = new HashSet<string>();
    }

    protected StructureViewModelBase()
    {
    }

    protected StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    public StructureNodeDTO SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        int num1;
        if (this._selectedItem != null)
        {
          string name1 = this._selectedItem.NodeType.Name;
          StructureNodeTypeEnum structureNodeTypeEnum = StructureNodeTypeEnum.Meter;
          string str1 = structureNodeTypeEnum.ToString();
          if (!(name1 == str1))
          {
            string name2 = this._selectedItem.NodeType.Name;
            structureNodeTypeEnum = StructureNodeTypeEnum.RadioMeter;
            string str2 = structureNodeTypeEnum.ToString();
            num1 = name2 == str2 ? 1 : 0;
          }
          else
            num1 = 1;
        }
        else
          num1 = 0;
        this.IsMeterSelected = num1 != 0;
        this.IsNodeSelected = this._selectedItem != null;
        this.IsRadioSelected = this._selectedItem != null && this._selectedItem.NodeType.Name == "Radio";
        if (this._selectedItem != null)
        {
          this.IsRootItemSelected = this._selectedItem.RootNode == this._selectedItem;
          this.IsReplacebleMeterSelected = this.SetIsReplacementMeterEnabled(this._selectedItem);
        }
        else
          this.IsRootItemSelected = false;
        int num2;
        if (this._selectedItem != null)
        {
          StructureTypeEnum? structureType = this._selectedItem.StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Physical;
          if ((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0)
          {
            num2 = this._selectedItem.NodeType.Name == "Meter" ? 1 : 0;
            goto label_12;
          }
        }
        num2 = 0;
label_12:
        this.IsChangeDeviceModelParametersEnabled = num2 != 0;
        this.SetStartAndStopScanVisibility();
      }
    }

    public bool IsChangeDeviceModelParametersEnabled
    {
      get => this._isChangeDeviceModelParametersEnabled;
      set
      {
        this._isChangeDeviceModelParametersEnabled = value;
        this.OnPropertyChanged(nameof (IsChangeDeviceModelParametersEnabled));
      }
    }

    private bool SetIsReplacementMeterEnabled(StructureNodeDTO _selectedItem)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (_selectedItem.Entity != null)
      {
        flag1 = true;
        if (_selectedItem.Entity is MeterDTO entity)
          flag2 |= entity.Id != Guid.Empty;
      }
      return ((!(_selectedItem.NodeType.Name == StructureNodeTypeEnum.Meter.ToString()) ? 0 : (_selectedItem.Id != Guid.Empty ? 1 : 0)) & (flag1 ? 1 : 0) & (flag2 ? 1 : 0)) != 0;
    }

    private void SetStartAndStopScanVisibility()
    {
      if (this._selectedItem == null)
        return;
      IList<StructureNodeType> structureNodeTypeList = (IList<StructureNodeType>) new List<StructureNodeType>();
      StructureTypeEnum? structureType = this._selectedItem.StructureType;
      if (structureType.HasValue)
      {
        switch (structureType.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
            structureNodeTypeList = this._structureNodeTypeRepository.SearchFor((Expression<Func<StructureNodeType, bool>>) (s => s.Name == StructureNodeTypeEnum.COMServer.ToString() || s.Name == StructureNodeTypeEnum.Converter.ToString() || s.Name == StructureNodeTypeEnum.Repeater.ToString() || s.Name == StructureNodeTypeEnum.Radio.ToString()));
            break;
          case StructureTypeEnum.Fixed:
            structureNodeTypeList = this._structureNodeTypeRepository.SearchFor((Expression<Func<StructureNodeType, bool>>) (s => s.Name == StructureNodeTypeEnum.Location.ToString() || s.Name == StructureNodeTypeEnum.Tenant.ToString() || s.Name == StructureNodeTypeEnum.MinomatMaster.ToString() || s.Name == StructureNodeTypeEnum.MinomatSlave.ToString()));
            break;
        }
      }
      List<Guid> nodeTypeIds = new List<Guid>();
      TypeHelperExtensionMethods.ForEach<StructureNodeType>((IEnumerable<StructureNodeType>) structureNodeTypeList, (Action<StructureNodeType>) (n => nodeTypeIds.Add(n.Id)));
      this.IsStartMBusScanButtonEnabled = !ScanMinoConnectManager.IsScanningStarted && !WalkByTestManager.IsWalkByTestStarted && nodeTypeIds.Contains(this._selectedItem.NodeType.Id) && this.StructureEquipmentSettings?.EquipmentName != null && this._selectedItem != null;
      this.IsStopMBusScanButtonEnabled = ScanMinoConnectManager.IsScanningStarted;
      this.IsWalkByTestButtonEnabled = !ScanMinoConnectManager.IsScanningStarted && !WalkByTestManager.IsWalkByTestStarted && nodeTypeIds.Contains(this._selectedItem.NodeType.Id);
      this.IsStopWalkByTestButtonEnabled = WalkByTestManager.IsWalkByTestStarted;
    }

    public bool IsNodeSelected
    {
      get => this._isNodeSelected;
      set
      {
        this._isNodeSelected = value;
        this.OnPropertyChanged(nameof (IsNodeSelected));
      }
    }

    public bool IsRootItemSelected
    {
      get => this._isRootItemSelected;
      set
      {
        this._isRootItemSelected = value;
        this.OnPropertyChanged(nameof (IsRootItemSelected));
      }
    }

    public bool IsReplacebleMeterSelected
    {
      get => this._isReplacebleMeterSelected;
      set
      {
        this._isReplacebleMeterSelected = value;
        this.OnPropertyChanged(nameof (IsReplacebleMeterSelected));
      }
    }

    public string DevicesFoundLabel
    {
      get => this._devicesFoundLabel;
      set
      {
        this._devicesFoundLabel = value;
        this.OnPropertyChanged(nameof (DevicesFoundLabel));
      }
    }

    public virtual ObservableCollection<StructureNodeDTO> GetStructureCollection()
    {
      return new ObservableCollection<StructureNodeDTO>();
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

    protected void ReplaceDevice(ReplaceDeviceEvent update)
    {
      MeterDTO currentMeterDto = update.CurrentMeterDTO;
      StructureNodeDTO _replacedMeter = update.ReplacedMeter;
      this._replacedMeterList.Add(_replacedMeter);
      ObservableCollection<StructureNodeDTO> structureCollection = this.GetStructureCollection();
      if (currentMeterDto != null)
      {
        if (_replacedMeter.Entity is MeterDTO entity)
        {
          List<MeterReplacementHistorySerializableDTO> replacementHistoryList = entity.MeterReplacementHistoryList;
          List<MeterReplacementHistorySerializableDTO> historySerializableDtoList = new List<MeterReplacementHistorySerializableDTO>();
          replacementHistoryList?.ForEach(new Action<MeterReplacementHistorySerializableDTO>(historySerializableDtoList.Add));
          historySerializableDtoList.Add(new MeterReplacementHistorySerializableDTO()
          {
            CurrentMeterId = currentMeterDto.Id,
            ReplacedByUserId = MSS.Business.Utils.AppContext.Current.LoggedUser.Id,
            ReplacedMeterId = entity.Id,
            ReplacedMeterDeviceType = entity.DeviceType,
            ReplacedMeterSerialNumber = entity.SerialNumber,
            ReplacementDate = DateTime.Now
          });
          currentMeterDto.MeterReplacementHistoryList = historySerializableDtoList;
          currentMeterDto.ReplacedMeterId = new Guid?(entity.Id);
          currentMeterDto.IsReplaced = true;
        }
        StructureNodeDTO newNodeDTO = new StructureNodeDTO()
        {
          Entity = (object) currentMeterDto,
          ParentNode = _replacedMeter.ParentNode,
          RootNode = _replacedMeter.RootNode,
          Name = update.Name,
          NodeType = update.CurrentMeterNodeType,
          Description = update.Description,
          IsNewNode = true,
          StructureType = _replacedMeter.StructureType,
          OrderNr = _replacedMeter.OrderNr,
          StartDate = _replacedMeter.StartDate,
          EndDate = new DateTime?(),
          IsExpanded = true,
          AssignedNotes = update.AssignedNotes,
          AssignedPicture = update.AssignedPicture,
          SubNodes = update.SubNodes
        };
        newNodeDTO.Image = StructuresHelper.GetImageForNode(newNodeDTO, newNodeDTO.Entity != null);
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) newNodeDTO.SubNodes)
        {
          subNode.ParentNode = newNodeDTO;
          subNode.RootNode = newNodeDTO.RootNode;
          subNode.StructureType = newNodeDTO.StructureType;
        }
        if (structureCollection.Any<StructureNodeDTO>() && structureCollection[0].NodeType == _replacedMeter.NodeType)
        {
          int index = structureCollection.FindIndex<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.OrderNr == newNodeDTO.OrderNr));
          structureCollection.Insert(index, newNodeDTO);
        }
        else
        {
          StructureNodeDTO parentNode = _replacedMeter.ParentNode;
          int index = parentNode.SubNodes.FindIndex<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.OrderNr == newNodeDTO.OrderNr));
          parentNode.SubNodes.Insert(index, newNodeDTO);
        }
      }
      this.RemoveSelectedNodeFromStructure(_replacedMeter, structureCollection);
      this.serialNumberList.Remove(_replacedMeter.Entity is MeterDTO entity1 ? entity1.SerialNumber : (string) null);
      MSS.Core.Model.Meters.Meter entity2 = this._meterRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (item => item.Id == (_replacedMeter.Entity as MeterDTO).Id));
      if (entity2 != null)
      {
        entity2.IsDeactivated = true;
        this._meterRepository.Update(entity2);
      }
      if (update.MeterToReplaceWith != null)
      {
        StructureNodeLinks entity3 = this._structureNodeLinkRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (item => item.Node.Id == update.MeterToReplaceWith.Id));
        if (entity3 != null)
        {
          entity3.EndDate = new DateTime?(DateTime.Now);
          this._structureNodeLinkRepository.Update(entity3);
        }
        StructureNode entity4 = this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (item => item.Id == update.MeterToReplaceWith.Id));
        if (entity4 != null)
        {
          entity4.EndDate = new DateTime?(DateTime.Now);
          this._structureNodeRepository.Update(entity4);
        }
        this.RemoveSelectedNodeFromStructure(update.MeterToReplaceWith, structureCollection);
      }
      if (currentMeterDto != null && !this.serialNumberList.Contains(currentMeterDto.SerialNumber))
        this.serialNumberList.Add(currentMeterDto.SerialNumber);
      this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(update.Message.MessageText);
    }

    protected void AddDevicesToPhysicalStructure(ZENNER.CommonLibrary.Entities.Meter obj, string systemName)
    {
      bool hasRadioAsParent = this.HasRadioAsParent();
      StructureNodeType nodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (n => n.Name == (hasRadioAsParent ? "RadioMeter" : "Meter")));
      bool isInCurrentStructure = false;
      MeterMBusStateEnum meterMbusState = this.GetMeterMBusState(obj.SerialNumber, this.MBusSelectedItem, this._meterRepository, this._structureNodeRepository, this._structureNodeLinkRepository, out isInCurrentStructure);
      switch (meterMbusState)
      {
        case MeterMBusStateEnum.New:
        case MeterMBusStateEnum.ExistingMeterInAnotherStructure:
          this.AddMeterToPhysicalStructure(obj, meterMbusState, nodeType, isInCurrentStructure);
          break;
        case MeterMBusStateEnum.Update:
        case MeterMBusStateEnum.ExistingMeterInAnotherStructureAndCurrentStructure:
          if (!(systemName != "wM-Bus") || !(systemName != "Radio2") || !(systemName != "Radio3"))
            break;
          List<StructureNodeDTO> list = StructuresHelper.Descendants(this.MBusSelectedItem).ToList<StructureNodeDTO>();
          StructureNodeDTO node1 = (StructureNodeDTO) null;
          if (list != null)
            node1 = list.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (node =>
            {
              if (node.NodeType.Name == "Meter" || node.NodeType.Name == "RadioMeter")
              {
                Guid? nullable = node.Entity is MeterDTO entity3 ? new Guid?(entity3.Id) : new Guid?();
                Guid empty = Guid.Empty;
                if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1) != 0)
                  return (node.Entity is MeterDTO entity4 ? entity4.SerialNumber : (string) null) == obj.SerialNumber;
              }
              return false;
            }));
          if (node1 != null)
          {
            MeterDTO entity5 = node1.Entity as MeterDTO;
            entity5.DeviceType = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(obj.DeviceModel.Name).Value;
            entity5.MBusStateEnum = new MeterMBusStateEnum?(meterMbusState);
            node1.Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterMbusState), node1.NodeType, entity5.IsConfigured, entity5.SerialNumber, node1.StructureType, isInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules);
            this.AddAdditionalInfoToMeterDTO(obj, ref entity5);
            node1.IsMeterNonEditable = this.IsItemNonEditable(node1);
          }
          else
          {
            MeterMBusStateEnum meterState = meterMbusState == MeterMBusStateEnum.Update ? MeterMBusStateEnum.New : meterMbusState;
            this.AddMeterToPhysicalStructure(obj, meterState, nodeType, isInCurrentStructure);
          }
          break;
      }
    }

    private void AddMeterToPhysicalStructure(
      ZENNER.CommonLibrary.Entities.Meter meter,
      MeterMBusStateEnum meterState,
      StructureNodeType nodeType,
      bool meterWithSameSerialNumberExistsInCurrentStructure)
    {
      MeterDTO meterDTO = new MeterDTO()
      {
        SerialNumber = meter.SerialNumber,
        DeviceType = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(meter.DeviceModel.Name).Value,
        MBusStateEnum = new MeterMBusStateEnum?(meterState),
        ReadingEnabled = true
      };
      this.AddAdditionalInfoToMeterDTO(meter, ref meterDTO);
      if (meter.AdditionalInfo != null && meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.MainDeviceSecondaryAddress))
      {
        this._physicalSubMeters.Add(meter);
      }
      else
      {
        StructureNodeDTO node = new StructureNodeDTO()
        {
          IsNewNode = true,
          NodeType = nodeType,
          ParentNode = this.MBusSelectedItem,
          RootNode = this.MBusSelectedItem.RootNode,
          Entity = (object) meterDTO,
          Name = nodeType.Name,
          Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterState), nodeType, meterDTO.IsConfigured, meterDTO.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Physical), meterWithSameSerialNumberExistsInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules),
          StructureType = this.MBusSelectedItem.StructureType
        };
        node.IsMeterNonEditable = this.IsItemNonEditable(node);
        this.MBusSelectedItem.SubNodes.Add(node);
      }
      foreach (ZENNER.CommonLibrary.Entities.Meter meter1 in new List<ZENNER.CommonLibrary.Entities.Meter>((IEnumerable<ZENNER.CommonLibrary.Entities.Meter>) this._physicalSubMeters))
      {
        int secondaryAddr;
        if (int.TryParse(meter1.AdditionalInfo[AdditionalInfoKey.MainDeviceSecondaryAddress], out secondaryAddr))
        {
          StructureNodeDTO structureNodeDto = this.MBusSelectedItem.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (x => x.Entity is MeterDTO && ((MeterDTO) x.Entity).SerialNumber == secondaryAddr.ToString()));
          if (structureNodeDto != null)
          {
            if (structureNodeDto.Entity != null)
              meterDTO.Manufacturer = ((MeterDTO) structureNodeDto.Entity).Manufacturer;
            StructureNodeDTO node = new StructureNodeDTO()
            {
              IsNewNode = true,
              NodeType = nodeType,
              ParentNode = this.MBusSelectedItem,
              RootNode = this.MBusSelectedItem.RootNode,
              Entity = (object) meterDTO,
              Name = nodeType.Name,
              Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterState), nodeType, meterDTO.IsConfigured, meterDTO.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Physical), meterWithSameSerialNumberExistsInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules),
              StructureType = this.MBusSelectedItem.StructureType
            };
            node.IsMeterNonEditable = this.IsItemNonEditable(node);
            structureNodeDto.SubNodes.Add(node);
            this._physicalSubMeters.Remove(meter1);
          }
        }
      }
      this.serialNumberList.Add(meter.SerialNumber);
    }

    private void AddAdditionalInfoToMeterDTO(ZENNER.CommonLibrary.Entities.Meter meter, ref MeterDTO meterDTO)
    {
      if (meter.AdditionalInfo == null || meter.AdditionalInfo.Count <= 0)
        return;
      if (meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.Manufacturer))
        meterDTO.Manufacturer = meter.AdditionalInfo[AdditionalInfoKey.Manufacturer];
      if (meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.Medium))
      {
        string str = meter.AdditionalInfo[AdditionalInfoKey.Medium];
        DeviceMediumEnum result;
        if (Enum.IsDefined(typeof (DeviceMediumEnum), (object) str) && Enum.TryParse<DeviceMediumEnum>(str, true, out result))
        {
          bool flag = DeviceTypeVisibilityHelper.IsPropertyVisible(typeof (MSS.Core.Model.Meters.Meter), DeviceTypeVisibilityHelper.GetPropertyName<DeviceMediumEnum?>((Expression<Func<DeviceMediumEnum?>>) (() => System.Linq.Expressions.Expression.New(typeof (MSS.Core.Model.Meters.Meter)).Medium)), new DeviceTypeEnum?((DeviceTypeEnum) result));
          meterDTO.Medium = flag ? new DeviceMediumEnum?(result) : new DeviceMediumEnum?();
        }
      }
      if (meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.InputNumber))
        meterDTO.InputNumber = meter.AdditionalInfo[AdditionalInfoKey.InputNumber];
      int result1;
      if (meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.PrimaryAddress) && int.TryParse(meter.AdditionalInfo[AdditionalInfoKey.PrimaryAddress], out result1))
        meterDTO.PrimaryAddress = new int?(result1);
      if (meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.Version))
        meterDTO.Generation = meter.AdditionalInfo[AdditionalInfoKey.Version];
      meterDTO.GMMAdditionalInfo = meter.AdditionalInfoString;
    }

    protected void AddDevicesToFixedStructure(ZENNER.CommonLibrary.Entities.Meter obj)
    {
      if (!GMMHelper.GetDeviceModelNameList(this.MBusSelectedItem.StructureType).Contains(obj.DeviceModel.Name))
        return;
      ZENNER.CommonLibrary.Entities.Meter meter = obj;
      bool hasRadioAsParent = this.HasRadioAsParent();
      StructureNodeType nodeType1 = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (n => n.Name == (hasRadioAsParent ? "RadioMeter" : "Meter")));
      bool isInCurrentStructure = false;
      MeterMBusStateEnum meterMbusState = this.GetMeterMBusState(meter.SerialNumber, this.MBusSelectedItem, this._meterRepository, this._structureNodeRepository, this._structureNodeLinkRepository, out isInCurrentStructure);
      switch (meterMbusState)
      {
        case MeterMBusStateEnum.New:
        case MeterMBusStateEnum.ExistingMeterInAnotherStructure:
          if (!this.CurrentStructureContainsSerialNumber(this.MBusSelectedItem, obj.SerialNumber))
          {
            if (this.MBusSelectedItem.NodeType.Name == "Location")
            {
              if (!this.MBusSelectedItem.SubNodes.Any<StructureNodeDTO>())
              {
                StructureNodeType nodeType2 = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (n => n.Name == "Tenant"));
                StructureNodeDTO structureNodeDto = new StructureNodeDTO()
                {
                  IsNewNode = true,
                  NodeType = nodeType2,
                  ParentNode = this.MBusSelectedItem,
                  RootNode = this.MBusSelectedItem.RootNode,
                  Entity = (object) null,
                  Name = "Tenant",
                  Image = StructuresHelper.GetImageForNode(nodeType2, (object) null, false),
                  StructureType = this.MBusSelectedItem.StructureType
                };
                this.MBusSelectedItem.SubNodes.Add(structureNodeDto);
                this.MBusSelectedItem = structureNodeDto;
              }
              else
                this.MBusSelectedItem = this.MBusSelectedItem.SubNodes.First<StructureNodeDTO>();
            }
            MeterDTO meterDTO = new MeterDTO()
            {
              SerialNumber = meter.SerialNumber,
              DeviceType = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(meter.DeviceModel.Name).Value,
              MBusStateEnum = new MeterMBusStateEnum?(meterMbusState),
              ReadingEnabled = true
            };
            this.AddAdditionalInfoToMeterDTO(meter, ref meterDTO);
            if (meter.AdditionalInfo != null && meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.MainDeviceSecondaryAddress))
              this._fixedSubMeters.Add(meter);
            else
              this.MBusSelectedItem.SubNodes.Add(new StructureNodeDTO()
              {
                IsNewNode = true,
                NodeType = nodeType1,
                ParentNode = this.MBusSelectedItem,
                RootNode = this.MBusSelectedItem.RootNode,
                Entity = (object) meterDTO,
                Name = nodeType1.Name,
                Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterMbusState), nodeType1, meterDTO.IsConfigured, meterDTO.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Fixed), isInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules),
                StructureType = this.MBusSelectedItem.StructureType
              });
            using (List<ZENNER.CommonLibrary.Entities.Meter>.Enumerator enumerator = new List<ZENNER.CommonLibrary.Entities.Meter>((IEnumerable<ZENNER.CommonLibrary.Entities.Meter>) this._fixedSubMeters).GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                ZENNER.CommonLibrary.Entities.Meter current = enumerator.Current;
                int secondaryAddr;
                if (int.TryParse(current.AdditionalInfo[AdditionalInfoKey.MainDeviceSecondaryAddress], out secondaryAddr))
                {
                  StructureNodeDTO structureNodeDto1 = this.MBusSelectedItem.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (x => x.Entity is MeterDTO && ((MeterDTO) x.Entity).SerialNumber == secondaryAddr.ToString()));
                  if (structureNodeDto1 != null)
                  {
                    if (structureNodeDto1.Entity != null)
                      meterDTO.Manufacturer = ((MeterDTO) structureNodeDto1.Entity).Manufacturer;
                    StructureNodeDTO structureNodeDto2 = new StructureNodeDTO()
                    {
                      IsNewNode = true,
                      NodeType = nodeType1,
                      ParentNode = this.MBusSelectedItem,
                      RootNode = this.MBusSelectedItem.RootNode,
                      Entity = (object) meterDTO,
                      Name = nodeType1.Name,
                      Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterMbusState), nodeType1, meterDTO.IsConfigured, meterDTO.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Fixed), isInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules),
                      StructureType = this.MBusSelectedItem.StructureType
                    };
                    structureNodeDto1.SubNodes.Add(structureNodeDto2);
                    this._fixedSubMeters.Remove(current);
                  }
                }
              }
              break;
            }
          }
          else
            break;
        case MeterMBusStateEnum.Update:
          MSS.Core.Model.Meters.Meter meter1 = this._meterRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (m => m.SerialNumber == meter.SerialNumber && !m.IsDeactivated));
          using (IEnumerator<StructureNodeDTO> enumerator = StructuresHelper.Descendants(this.MBusSelectedItem).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              StructureNodeDTO current = enumerator.Current;
              if ((current.NodeType.Name == "Meter" || current.NodeType.Name == "RadioMeter") && current.Entity is MeterDTO entity && meter1 != null && entity.Id == meter1.Id)
              {
                entity.DeviceType = StructuresHelper.GetDeviceTypeEnumByDeviceModelName(meter.DeviceModel.Name).Value;
                entity.MBusStateEnum = new MeterMBusStateEnum?(meterMbusState);
                current.Image = StructuresHelper.GetImageForMeterNode(new MeterMBusStateEnum?(meterMbusState), nodeType1, entity.IsConfigured, entity.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Fixed), isInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules);
                this.AddAdditionalInfoToMeterDTO(meter, ref entity);
              }
            }
            break;
          }
      }
    }

    private void SetRadioMetersPropertyWhenImportingFromFile(
      string propertyName,
      int columnIndex,
      int radioMeterIndex,
      List<string[]> radioMetersList,
      MeterDTO meterDTO)
    {
      switch (propertyName)
      {
        case "Medium":
          try
          {
            meterDTO.Medium = string.IsNullOrWhiteSpace(radioMetersList[radioMeterIndex][columnIndex]) ? new DeviceMediumEnum?() : new DeviceMediumEnum?((DeviceMediumEnum) Enum.Parse(typeof (DeviceMediumEnum), radioMetersList[radioMeterIndex][columnIndex].ToUpper()));
            break;
          }
          catch (Exception ex)
          {
            MSS.Business.Errors.MessageHandler.LogException(ex);
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_ImportCSV_RadioMeter_MediumError);
            throw new Exception(MessageCodes.Error_ImportRadioMeters_MediumFormat.ToString());
          }
        case "ReadingEnabled":
          try
          {
            meterDTO.ReadingEnabled = columnIndex < 0 || string.IsNullOrWhiteSpace(radioMetersList[radioMeterIndex][columnIndex]) || Convert.ToBoolean(radioMetersList[radioMeterIndex][columnIndex]);
            break;
          }
          catch (Exception ex)
          {
            MSS.Business.Errors.MessageHandler.LogException(ex);
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_ImportCSV_RadioMeter_ReadingEnabledError);
            throw new Exception(MessageCodes.Error_ImportRadioMeters_ReadingEnabledFormat.ToString());
          }
        default:
          typeof (MbusRadioMeter).GetProperty(propertyName)?.SetValue((object) meterDTO.MbusRadioMeter, (object) radioMetersList[radioMeterIndex][columnIndex]);
          break;
      }
    }

    protected void ImportRadioMeters(string filePath)
    {
      try
      {
        List<string[]> radioMetersList = new CSVManager().ReadStructureFromFile(filePath, new string[1]
        {
          ";"
        });
        if (radioMetersList.Count <= 1)
          return;
        List<string> list = ((IEnumerable<string>) radioMetersList[0]).ToList<string>();
        int columnIndex1 = list.IndexOf("City");
        int columnIndex2 = list.IndexOf("Street");
        int columnIndex3 = list.IndexOf("House number");
        int columnIndex4 = list.IndexOf("House number supplement");
        int columnIndex5 = list.IndexOf("Apartment number");
        int columnIndex6 = list.IndexOf("ZIP code");
        int columnIndex7 = list.IndexOf("First name");
        int columnIndex8 = list.IndexOf("Last name");
        int columnIndex9 = list.IndexOf("Location");
        int columnIndex10 = list.IndexOf("Radio serial number");
        int columnIndex11 = list.IndexOf("Medium");
        int columnIndex12 = list.IndexOf("Reading enabled");
        if (columnIndex1 < 0 || columnIndex2 < 0 || columnIndex3 < 0 || columnIndex4 < 0 || columnIndex5 < 0 || columnIndex6 < 0 || columnIndex7 < 0 || columnIndex8 < 0 || columnIndex9 < 0 || columnIndex10 < 0 || columnIndex11 < 0)
        {
          MSS.Business.Errors.MessageHandler.LogDebug("Not all the expected columns found in CSV file to import");
          throw new BaseApplicationException(Resources.MSS_Client_ImportCSV_OpenFileError);
        }
        StructureNodeType nodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (item => item.Name == "RadioMeter"));
        for (int index = 1; index < radioMetersList.Count; ++index)
        {
          bool isInCurrentStructure = false;
          MeterMBusStateEnum meterMbusState = this.GetMeterMBusState(radioMetersList[index][columnIndex10], this.SelectedItem, this._meterRepository, this._structureNodeRepository, this._structureNodeLinkRepository, out isInCurrentStructure);
          MeterDTO meterDTO = new MeterDTO();
          meterDTO.MbusRadioMeter = new MbusRadioMeter();
          this.SetRadioMetersPropertyWhenImportingFromFile("City", columnIndex1, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("Street", columnIndex2, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("HouseNumber", columnIndex3, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("HouseNumberSupplement", columnIndex4, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("ApartmentNumber", columnIndex5, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("ZipCode", columnIndex6, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("FirstName", columnIndex7, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("LastName", columnIndex8, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("Location", columnIndex9, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("RadioSerialNumber", columnIndex10, index, radioMetersList, meterDTO);
          meterDTO.SerialNumber = meterDTO.MbusRadioMeter.RadioSerialNumber;
          meterDTO.MBusStateEnum = new MeterMBusStateEnum?(meterMbusState == MeterMBusStateEnum.Update ? MeterMBusStateEnum.New : meterMbusState);
          meterDTO.DeviceType = DeviceTypeEnum.GenericWMBus;
          this.SetRadioMetersPropertyWhenImportingFromFile("Medium", columnIndex11, index, radioMetersList, meterDTO);
          this.SetRadioMetersPropertyWhenImportingFromFile("ReadingEnabled", columnIndex12, index, radioMetersList, meterDTO);
          if (!string.IsNullOrWhiteSpace(meterDTO.SerialNumber))
            this.serialNumberList.Add(meterDTO.SerialNumber);
          StructureNodeDTO node = new StructureNodeDTO()
          {
            IsNewNode = true,
            NodeType = nodeType,
            ParentNode = this.SelectedItem,
            RootNode = this.SelectedItem.RootNode,
            Entity = (object) meterDTO,
            Name = "RadioMeter",
            Image = StructuresHelper.GetImageForMeterNode(meterDTO.MBusStateEnum, nodeType, meterDTO.IsConfigured, meterDTO.SerialNumber, new StructureTypeEnum?(StructureTypeEnum.Physical), isInCurrentStructure, this._serialNumbersOfMetersThatAreMissingTranslationRules),
            StructureType = this.SelectedItem.StructureType
          };
          node.IsMeterNonEditable = this.IsItemNonEditable(node);
          this.SelectedItem.SubNodes.Add(node);
        }
      }
      catch (BaseApplicationException ex)
      {
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(ex.Message);
      }
      catch (Exception ex)
      {
        string message1 = ex.Message;
        MessageCodes messageCodes = MessageCodes.Error_ImportRadioMeters_MediumFormat;
        string str1 = messageCodes.ToString();
        int num;
        if (message1 != str1)
        {
          string message2 = ex.Message;
          messageCodes = MessageCodes.Error_ImportRadioMeters_ReadingEnabledFormat;
          string str2 = messageCodes.ToString();
          num = message2 != str2 ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
          return;
        MSS.Business.Errors.MessageHandler.LogException(ex);
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(ex.Message);
      }
    }

    protected Dictionary<string, string> ImportAesKeysFromDeliveryNoteXml(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        return (Dictionary<string, string>) null;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(filePath);
      XmlNode documentElement = (XmlNode) xmlDocument.DocumentElement;
      if (documentElement?.ChildNodes != null)
      {
        foreach (XmlNode childNode1 in documentElement.ChildNodes)
        {
          string key = (string) null;
          string str = (string) null;
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            if (childNode2.LocalName == "SerialNo.Meter")
              key = childNode2.InnerText;
            if (childNode2.LocalName == "AES")
              str = childNode2?.Attributes["AesKey"]?.InnerText;
          }
          if (key != null && !dictionary.ContainsKey(key))
            dictionary.Add(key, str);
        }
      }
      return dictionary;
    }

    protected Dictionary<string, string> ImportAesKeysFromDeliveryNoteCsv(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        return (Dictionary<string, string>) null;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      List<string[]> source = new CSVManager().ReadStructureFromFile(filePath, new string[1]
      {
        ";"
      });
      if (source != null && source.Count > 1)
      {
        int index1 = ((IEnumerable<string>) source[0]).FindIndex<string>((Func<string, bool>) (item => item == "SerialNo. Meter"));
        int index2 = ((IEnumerable<string>) source[0]).FindIndex<string>((Func<string, bool>) (item => item == "AESKey"));
        foreach (string[] strArray in source.Skip<string[]>(1))
        {
          string key = strArray[index1];
          string str = strArray[index2];
          if (!string.IsNullOrWhiteSpace(key))
            dictionary.Add(key, str);
        }
      }
      return dictionary;
    }

    private bool UniqueMeter(ZENNER.CommonLibrary.Entities.Meter meter)
    {
      List<StructureNodeDTO> meters = StructuresHelper.GetMeters(new ObservableCollection<StructureNodeDTO>(StructuresHelper.Descendants(this.MBusSelectedItem.RootNode)));
      string inputNumber = string.Empty;
      string empty = string.Empty;
      if (meter.AdditionalInfo != null && meter.AdditionalInfo.Count > 0 && meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.InputNumber))
        inputNumber = meter.AdditionalInfo[AdditionalInfoKey.InputNumber];
      if (meter.AdditionalInfo != null && meter.AdditionalInfo.Count > 0 && meter.AdditionalInfo.ContainsKey(AdditionalInfoKey.MainDeviceSecondaryAddress))
        empty = meter.AdditionalInfo[AdditionalInfoKey.MainDeviceSecondaryAddress];
      foreach (StructureNodeDTO structureNodeDto in meters)
      {
        if (structureNodeDto.Entity is MeterDTO entity)
        {
          if (!string.IsNullOrEmpty(empty))
          {
            if (entity.SerialNumber == empty && structureNodeDto.SubNodes != null && structureNodeDto.SubNodes.Select<StructureNodeDTO, MeterDTO>((Func<StructureNodeDTO, MeterDTO>) (childNode => childNode.Entity as MeterDTO)).Any<MeterDTO>((Func<MeterDTO, bool>) (childEntity => childEntity != null && childEntity.InputNumber == inputNumber)))
              return false;
          }
          else if (entity.SerialNumber == meter.SerialNumber)
            return false;
        }
      }
      return true;
    }

    public bool IsStartMBusScanButtonEnabled
    {
      get => this._isStartMBusScanButtonEnabled;
      set
      {
        this._isStartMBusScanButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsStartMBusScanButtonEnabled));
      }
    }

    public bool IsStopMBusScanButtonEnabled
    {
      get => this._isStopMBusScanButtonEnabled;
      set
      {
        this._isStopMBusScanButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsStopMBusScanButtonEnabled));
      }
    }

    public bool IsWalkByTestButtonEnabled
    {
      get => this._isWalkByTestButtonEnabled;
      set
      {
        this._isWalkByTestButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsWalkByTestButtonEnabled));
      }
    }

    public bool IsStopWalkByTestButtonEnabled
    {
      get => this._isStopWalkByTestButtonEnabled;
      set
      {
        this._isStopWalkByTestButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsStopWalkByTestButtonEnabled));
      }
    }

    public bool IsRadioSelected
    {
      get => this._isRadioSelected;
      set
      {
        this._isRadioSelected = value;
        this.OnPropertyChanged(nameof (IsRadioSelected));
      }
    }

    public bool IsMeterSelected
    {
      get => this._isMeterSelected;
      set
      {
        this._isMeterSelected = value;
        this.OnPropertyChanged(nameof (IsMeterSelected));
      }
    }

    protected StructureNodeDTO MBusSelectedItem { get; set; }

    protected MeterMBusStateEnum GetMeterMBusState(
      string serialNumber,
      StructureNodeDTO selectedNode,
      IRepository<MSS.Core.Model.Meters.Meter> meterRepository,
      IRepository<StructureNode> structureNodeRepository,
      IRepository<StructureNodeLinks> structureNodeLinkRepository,
      out bool isInCurrentStructure)
    {
      isInCurrentStructure = false;
      if (selectedNode.SubNodes != null)
      {
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) selectedNode.SubNodes)
        {
          if (subNode.Entity is MeterDTO entity1 && entity1.SerialNumber == serialNumber)
            isInCurrentStructure = true;
          if (subNode.SubNodes != null && subNode.SubNodes.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.Entity is MeterDTO entity2 && entity2.SerialNumber == serialNumber)))
            isInCurrentStructure = true;
        }
      }
      IList<MSS.Core.Model.Meters.Meter> meterList;
      if (meterRepository == null)
        meterList = (IList<MSS.Core.Model.Meters.Meter>) null;
      else
        meterList = meterRepository.SearchFor((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (m => m.SerialNumber == serialNumber && !m.IsDeactivated));
      IList<MSS.Core.Model.Meters.Meter> source = meterList;
      if (source != null && source.Any<MSS.Core.Model.Meters.Meter>())
      {
        MSS.Core.Model.Meters.Meter existingMeter = source.First<MSS.Core.Model.Meters.Meter>();
        StructureNode node = structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (n => n.EntityId == existingMeter.Id && n.EndDate == new DateTime?()));
        if (node != null)
        {
          StructureNodeLinks structureNodeLinks = structureNodeLinkRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (l => l.Node.Id == node.Id && l.EndDate == new DateTime?()));
          if (structureNodeLinks != null)
            return structureNodeLinks.RootNode.Id == selectedNode.RootNode.Id ? MeterMBusStateEnum.Update : (isInCurrentStructure ? MeterMBusStateEnum.ExistingMeterInAnotherStructureAndCurrentStructure : MeterMBusStateEnum.ExistingMeterInAnotherStructure);
        }
      }
      return isInCurrentStructure ? MeterMBusStateEnum.Update : MeterMBusStateEnum.New;
    }

    protected bool ReconstructCollectionWithoutInvalidMBusScannerMeters(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      List<StructureNodeDTO> source = new List<StructureNodeDTO>();
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        StructureNodeTypeEnum structureNodeTypeEnum = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), node.NodeType.Name, true);
        if ((structureNodeTypeEnum == StructureNodeTypeEnum.Meter || structureNodeTypeEnum == StructureNodeTypeEnum.RadioMeter) && node.Entity is MeterDTO entity)
        {
          MeterMBusStateEnum? mbusStateEnum = entity.MBusStateEnum;
          MeterMBusStateEnum meterMbusStateEnum1 = MeterMBusStateEnum.ExistingMeterInAnotherStructure;
          int num;
          if ((mbusStateEnum.GetValueOrDefault() == meterMbusStateEnum1 ? (mbusStateEnum.HasValue ? 1 : 0) : 0) == 0)
          {
            mbusStateEnum = entity.MBusStateEnum;
            MeterMBusStateEnum meterMbusStateEnum2 = MeterMBusStateEnum.ExistingMeterInAnotherStructureAndCurrentStructure;
            num = mbusStateEnum.GetValueOrDefault() == meterMbusStateEnum2 ? (mbusStateEnum.HasValue ? 1 : 0) : 0;
          }
          else
            num = 1;
          if (num != 0)
            source.Add(node);
        }
      }
      if (!source.Any<StructureNodeDTO>())
        return true;
      bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Warning_Title, Resources.MSS_Warning_InvalidMetersWillbeRemoved_Message, true);
      if (!nullable.HasValue || !nullable.Value)
        return false;
      foreach (StructureNodeDTO structureNodeDto in source)
        nodeCollection.Remove(structureNodeDto);
      return true;
    }

    protected void ShowActionSyncFinished(ActionSyncFinished obj)
    {
      switch (obj.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(obj.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          this.IsStartMBusScanButtonEnabled = true;
          this.IsStopMBusScanButtonEnabled = false;
          this.IsWalkByTestButtonEnabled = true;
          this.IsStopWalkByTestButtonEnabled = false;
          ScanMinoConnectManager.IsScanningStarted = false;
          MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Warning_Title, obj.Message.MessageText, false);
          break;
      }
    }

    protected bool ContinueActionIfMBusIsStarted(ScanMinoConnectManager scanMinoConnectManager)
    {
      bool flag = false;
      if (ScanMinoConnectManager.IsScanningStarted)
      {
        bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.Warning_MBusScanning_Running_Title, Resources.Warning_MBusScanning_Running_Message, true);
        if (nullable.HasValue && nullable.Value)
        {
          scanMinoConnectManager.StopScan();
          this.IsStartMBusScanButtonEnabled = true;
          this.IsStopMBusScanButtonEnabled = false;
          this.IsWalkByTestButtonEnabled = true;
          this.IsStopWalkByTestButtonEnabled = false;
          ScanMinoConnectManager.IsScanningStarted = false;
          flag = true;
        }
      }
      else
        flag = true;
      return flag;
    }

    protected void UpdateEntities(ActionStructureAndEntitiesUpdate update)
    {
      this._updateMeterDTO = update.MeterDTO;
      this._node = update.Node;
      this._updatedLocation = update.Location;
      this._updatedTenant = update.Tenant;
      this._updatedMinomat = update.Minomat;
      this._name = update.Name;
      this._description = update.Description;
      if (update.Location != null)
      {
        if (!this.locationNumberList.Contains(update.Location.BuildingNr))
          this.locationNumberList.Add(update.Location.BuildingNr);
        this.UpdateRootNodeLocation(update.Location);
      }
      if (update.MeterDTO != null && !this.serialNumberList.Contains(update.MeterDTO.SerialNumber))
        this.serialNumberList.Add(update.MeterDTO.SerialNumber);
      if (update.Minomat != null)
        update.Node.Entity.SafeCast<MinomatSerializableDTO>().NetworkStatus = MinomatNetworkStatusEnum.New;
      if (this._updateMeterDTO != null)
      {
        this._node.Entity = (object) this._updateMeterDTO;
        ObservableCollection<StructureNodeDTO> structureCollection = this.GetStructureCollection();
        if (structureCollection.Any<StructureNodeDTO>() && structureCollection[0].NodeType == this._node.NodeType && !structureCollection.Contains(this._node))
          structureCollection.Add(this._node);
      }
      if (this._node != null)
      {
        this._node.Name = this._name;
        this._node.Description = this._description;
      }
      if (this._node != null)
      {
        if (this is EditPhysicalStructureViewModel)
          this.UpdateDevicesFoundLabel();
        else if (this is CreatePhysicalStructureViewModel)
          this.UpdateDevicesFoundLabel();
      }
      switch (update.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(update.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
          break;
      }
    }

    private void UpdateRootNodeLocation(MSS.Core.Model.Structures.Location location)
    {
      LocationDTO locationEntity = new LocationDTO()
      {
        BuildingNr = location.BuildingNr,
        City = location.City,
        Description = location.City,
        DueDate = location.DueDate,
        Generation = location.Generation,
        HasMaster = location.HasMaster,
        Id = location.Id,
        IsDeactivated = location.IsDeactivated,
        LastChangedOn = location.LastChangedOn,
        Scale = location.Scale,
        Scenario = location.Scenario,
        Status = location.Status,
        Street = location.Street,
        ZipCode = location.ZipCode
      };
      StructureNodeDTO structureNodeDto = (StructureNodeDTO) null;
      string propertyName = string.Empty;
      switch (this)
      {
        case EditFixedStructureViewModel _:
          structureNodeDto = ((EditFixedStructureViewModel) this).StructureForSelectedNode.ElementAt<StructureNodeDTO>(0);
          this.UpdateRootNodeForFixedStructureNodes(((EditFixedStructureViewModel) this).StructureForSelectedNode.ElementAt<StructureNodeDTO>(0).SubNodes, structureNodeDto);
          propertyName = "StructureForSelectedNode";
          break;
        case CreateFixedStructureViewModel _:
          structureNodeDto = ((CreateFixedStructureViewModel) this).FixedStructureNodeCollection.ElementAt<StructureNodeDTO>(0);
          this.UpdateRootNodeForFixedStructureNodes(((CreateFixedStructureViewModel) this).FixedStructureNodeCollection.ElementAt<StructureNodeDTO>(0).SubNodes, structureNodeDto);
          propertyName = "FixedStructureNodeCollection";
          break;
      }
      this.UpdateLocationEntity(structureNodeDto, locationEntity);
      this.OnPropertyChanged(propertyName);
    }

    private void UpdateLocationEntity(StructureNodeDTO locationNode, LocationDTO locationEntity)
    {
      if (locationNode == null)
        return;
      locationNode.Entity = (object) locationEntity;
    }

    private void UpdateRootNodeForFixedStructureNodes(
      ObservableCollection<StructureNodeDTO> subnodes,
      StructureNodeDTO rootNode)
    {
      if (subnodes == null)
        return;
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) subnodes, (Action<StructureNodeDTO>) (t =>
      {
        t.RootNode = rootNode;
        t.ParentNode = rootNode;
        if (t.SubNodes == null || t.SubNodes.Count <= 0)
          return;
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) t.SubNodes, (Action<StructureNodeDTO>) (m => m.RootNode = rootNode));
      }));
    }

    protected bool? EditSelectedStructureNode(
      StructureNodeDTO node,
      bool isReplacedMeter = false,
      string orderNumber = null)
    {
      ObservableCollection<StructureNodeDTO> structureCollection = this.GetStructureCollection();
      bool? nullable = new bool?();
      string name = node.NodeType?.Name;
      if (name == null)
        return new bool?(false);
      bool flag = node.Entity != null;
      switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), name, true))
      {
        case StructureNodeTypeEnum.Location:
          nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditLocationViewModel>((IParameter) new ConstructorArgument("isExistingEntity", (object) flag), (IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("locationNumberList", (object) this.locationNumberList)));
          if (!nullable.HasValue || !nullable.Value)
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          if (this._updatedLocation != null)
          {
            LocationDTO locationDto = Mapper.Map<MSS.Core.Model.Structures.Location, LocationDTO>(this._updatedLocation);
            node.Entity = (object) locationDto;
            break;
          }
          break;
        case StructureNodeTypeEnum.Tenant:
          List<int> localTenantNrs = StructureViewModelBase.GetLocalTenantNrs((IEnumerable<StructureNodeDTO>) structureCollection);
          if (node.Entity != null)
            localTenantNrs.Remove((node.Entity as TenantDTO).TenantNr);
          nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditTenantViewModel>((IParameter) new ConstructorArgument("isExistingEntity", (object) flag), (IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("localStructureTenantNrs", (object) localTenantNrs)));
          if (!nullable.HasValue || !nullable.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            break;
          }
          if (this._updatedTenant != null)
          {
            TenantDTO tenantDto = Mapper.Map<Tenant, TenantDTO>(this._updatedTenant);
            node.Entity = (object) tenantDto;
          }
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          DeviceStateEnum deviceStateEnum = !isReplacedMeter ? (!flag ? DeviceStateEnum.Create : DeviceStateEnum.Edit) : DeviceStateEnum.Replace;
          DeviceViewModel deviceViewModel = (DeviceViewModel) null;
          switch (deviceStateEnum)
          {
            case DeviceStateEnum.Create:
              deviceViewModel = (DeviceViewModel) DIConfigurator.GetConfigurator().Get<CreateDeviceViewModel>((IParameter) new ConstructorArgument("deviceState", (object) DeviceStateEnum.Create), (IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("serialNumberList", (object) this.serialNumberList));
              break;
            case DeviceStateEnum.Edit:
              this.serialNumberList.Remove(node.Entity is MeterDTO entity ? entity.SerialNumber : (string) null);
              deviceViewModel = (DeviceViewModel) DIConfigurator.GetConfigurator().Get<EditDeviceViewModel>((IParameter) new ConstructorArgument("deviceState", (object) DeviceStateEnum.Edit), (IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("serialNumberList", (object) this.serialNumberList));
              break;
            case DeviceStateEnum.Replace:
              deviceViewModel = (DeviceViewModel) DIConfigurator.GetConfigurator().Get<ReplaceDeviceViewModel>((IParameter) new ConstructorArgument("deviceState", (object) DeviceStateEnum.Replace), (IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("serialNumberList", (object) this.serialNumberList));
              break;
          }
          nullable = this._windowFactory.CreateNewModalDialog((IViewModel) deviceViewModel, new object[1]
          {
            (object) node
          });
          if (!nullable.HasValue || !nullable.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            break;
          }
          break;
        case StructureNodeTypeEnum.MinomatMaster:
          StructureTypeEnum? structureType1 = node.StructureType;
          StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
          if (structureType1.GetValueOrDefault() == structureTypeEnum1 && structureType1.HasValue)
          {
            nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditMinomatMasterViewModel>((IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument(nameof (orderNumber), (object) orderNumber)));
            if (!nullable.HasValue || !nullable.Value)
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            else if (this._updatedMinomat != null)
            {
              MinomatSerializableDTO minomatSerializableDto = Mapper.Map<Minomat, MinomatSerializableDTO>(this._updatedMinomat);
              if (this._updatedMinomat.Country != null)
                minomatSerializableDto.CountryId = this._updatedMinomat.Country.Id;
              node.Entity = (object) minomatSerializableDto;
            }
            break;
          }
          break;
        case StructureNodeTypeEnum.MinomatSlave:
          StructureTypeEnum? structureType2 = node.StructureType;
          StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Fixed;
          if (structureType2.GetValueOrDefault() == structureTypeEnum2 && structureType2.HasValue)
          {
            nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditMinomatSlaveViewModel>((IParameter) new ConstructorArgument(nameof (node), (object) node), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument(nameof (orderNumber), (object) orderNumber)));
            if (!nullable.HasValue || !nullable.Value)
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            else if (this._updatedMinomat != null)
            {
              MinomatSerializableDTO minomatSerializableDto = Mapper.Map<Minomat, MinomatSerializableDTO>(this._updatedMinomat);
              if (this._updatedMinomat.Country != null)
                minomatSerializableDto.CountryId = this._updatedMinomat.Country.Id;
              node.Entity = (object) minomatSerializableDto;
            }
            break;
          }
          break;
        default:
          nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditGenericEntityViewModel>((IParameter) new ConstructorArgument(nameof (node), (object) node)));
          if (!nullable.HasValue || !nullable.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            break;
          }
          break;
      }
      return nullable;
    }

    private static List<int> GetLocalTenantNrs(IEnumerable<StructureNodeDTO> structureCollection)
    {
      List<int> localTenantNrs = new List<int>();
      foreach (StructureNodeDTO structure in structureCollection)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(structure.RootNode))
        {
          if (descendant.Entity != null)
          {
            string name = descendant.NodeType?.Name;
            if (name != null && (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), name, true) == StructureNodeTypeEnum.Tenant)
            {
              int tenantNr = (descendant.Entity as TenantDTO).TenantNr;
              localTenantNrs.Add(tenantNr);
            }
          }
        }
      }
      return localTenantNrs;
    }

    private static List<string> GetLocalMinomatsNrs(
      IEnumerable<StructureNodeDTO> structureCollection)
    {
      List<string> localMinomatsNrs = new List<string>();
      foreach (StructureNodeDTO structure in structureCollection)
      {
        foreach (StructureNodeDTO descendant in StructuresHelper.Descendants(structure.RootNode))
        {
          if (descendant.Entity != null)
          {
            string name = descendant.NodeType?.Name;
            if (name != null)
            {
              switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), name, true))
              {
                case StructureNodeTypeEnum.MinomatMaster:
                case StructureNodeTypeEnum.MinomatSlave:
                  string radioId = (descendant.Entity as MinomatSerializableDTO).RadioId;
                  localMinomatsNrs.Add(radioId);
                  break;
              }
            }
          }
        }
      }
      return localMinomatsNrs;
    }

    protected ObservableCollection<StructureNodeDTO> ReconstructNodeCollection(
      ObservableCollection<StructureNodeDTO> actualNodeCollection,
      StructureNodeDTO parentForSelectedNode)
    {
      foreach (StructureNodeDTO node in actualNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.ParentNode == null && s.RootNode == s)))
      {
        StructureNodeDTO rootNode;
        if (parentForSelectedNode == null)
        {
          rootNode = node;
          node.ParentNode = (StructureNodeDTO) null;
          node.RootNode = rootNode;
        }
        else
        {
          rootNode = parentForSelectedNode.RootNode != parentForSelectedNode ? parentForSelectedNode.RootNode : parentForSelectedNode;
          node.ParentNode = parentForSelectedNode;
          node.RootNode = rootNode;
        }
        StructureViewModelBase.SetRootForChildren(node, rootNode);
      }
      foreach (StructureNodeDTO structureNodeDto1 in actualNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.ParentNode == null && s.RootNode == null)))
      {
        if (parentForSelectedNode != null)
        {
          StructureNodeDTO structureNodeDto2 = parentForSelectedNode.RootNode != parentForSelectedNode ? parentForSelectedNode.RootNode : parentForSelectedNode;
          structureNodeDto1.ParentNode = parentForSelectedNode;
          structureNodeDto1.RootNode = structureNodeDto2;
        }
      }
      foreach (StructureNodeDTO structureNodeDto3 in actualNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s =>
      {
        StructureTypeEnum? structureType = s.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Logical;
        return (structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 && s.RootNode == null;
      })))
      {
        if (parentForSelectedNode != null)
        {
          StructureNodeDTO structureNodeDto4 = parentForSelectedNode.RootNode != parentForSelectedNode ? parentForSelectedNode.RootNode : parentForSelectedNode;
          structureNodeDto3.ParentNode = parentForSelectedNode;
          structureNodeDto3.RootNode = structureNodeDto4;
        }
      }
      return actualNodeCollection;
    }

    private static void SetRootForChildren(StructureNodeDTO node, StructureNodeDTO rootNode)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        subNode.RootNode = rootNode;
        StructureViewModelBase.SetRootForChildren(subNode, rootNode);
      }
    }

    protected bool? ShowWarningWithStructuresToDeleteDialog(
      ObservableCollection<StructureNodeDTO> physicalStructureNodeDTOToDelete,
      ObservableCollection<StructureNodeDTO> logicalStructureNodeDTOToDelete)
    {
      return this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) physicalStructureNodeDTOToDelete), (IParameter) new ConstructorArgument("otherAffectedStructures", (object) logicalStructureNodeDTOToDelete)));
    }

    public void RemoveSelectedNodeFromStructure(
      StructureNodeDTO selectedNode,
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        if (node == selectedNode)
        {
          nodeCollection.Remove(node);
          break;
        }
        this.RemoveSelectedNodeFromStructure(selectedNode, node.SubNodes);
      }
    }

    public void RemoveSelectedNodeFromStructureById(
      Guid selectedNodeId,
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        if (selectedNodeId != Guid.Empty && node.Id == selectedNodeId)
        {
          nodeCollection.Remove(node);
          break;
        }
        this.RemoveSelectedNodeFromStructureById(selectedNodeId, node.SubNodes);
      }
    }

    protected void RemoveSerialNumberFromUniquenessList(StructureNodeDTO selectedNode)
    {
      if (!(selectedNode.Entity is MeterDTO entity))
        return;
      this.serialNumberList.Remove(entity.SerialNumber);
    }

    protected void UpdateSerialNumberListForReadingOrder(bool updatedForReadingOrder)
    {
      if (!updatedForReadingOrder)
        return;
      foreach (StructureNodeDTO meter in StructuresHelper.GetMeters(this.GetStructureCollection()))
      {
        if (meter.Entity is MeterDTO entity)
        {
          if (!this.serialNumberList.Contains(entity.SerialNumber))
            this.serialNumberList.Add(entity.SerialNumber);
          List<MeterReplacementHistorySerializableDTO> replacementHistoryList = entity.MeterReplacementHistoryList;
          if (replacementHistoryList != null && replacementHistoryList.Count > 0)
            replacementHistoryList.ForEach((Action<MeterReplacementHistorySerializableDTO>) (h =>
            {
              if (this.serialNumberList.Contains(h.ReplacedMeterSerialNumber))
                return;
              this.serialNumberList.Add(h.ReplacedMeterSerialNumber);
            }));
        }
      }
    }

    private bool HasRadioAsParent()
    {
      if (this.SelectedItem == null)
        throw new Exception(CultureResources.GetValue("MSS_Client_Select_Node"));
      if (this.SelectedItem.NodeType.Name == "Radio")
        return true;
      for (StructureNodeDTO structureNodeDto = this.SelectedItem; structureNodeDto.ParentNode != null; structureNodeDto = structureNodeDto.ParentNode)
      {
        if (structureNodeDto.ParentNode.NodeType.Name == "Radio")
          return true;
      }
      return false;
    }

    private bool CurrentStructureContainsSerialNumber(
      StructureNodeDTO nodeInCurrentStructure,
      string serialNumber)
    {
      StructureNodeDTO rootNode = nodeInCurrentStructure.RootNode;
      List<string> serialNumberList = new List<string>();
      this.WalkTreeAndUpdateSerialNumbersList(rootNode, ref serialNumberList);
      return serialNumberList.Contains(serialNumber);
    }

    private void WalkTreeAndUpdateSerialNumbersList(
      StructureNodeDTO node,
      ref List<string> serialNumberList)
    {
      if (node.Entity != null && node.Entity is MeterDTO)
      {
        string serialNumber = (node.Entity as MeterDTO).SerialNumber;
        if (!serialNumberList.Contains(serialNumber))
          serialNumberList.Add(serialNumber);
      }
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
        this.WalkTreeAndUpdateSerialNumbersList(subNode, ref serialNumberList);
    }

    protected bool IsItemNonEditable(StructureNodeDTO node)
    {
      bool flag = false;
      StructureTypeEnum? structureType = node.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Physical;
      if ((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 && (node.NodeType.Name == "Meter" || node.NodeType.Name == "RadioMeter"))
        flag = !(node.Entity is MeterDTO entity) || !StructureViewModelBase.deviceModelsInLicense.Contains(entity.DeviceType.GetGMMDeviceModelName());
      return flag;
    }

    protected void CalculateNoOfDevicesForAllTenants(StructureNodeDTO root)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) root.SubNodes)
      {
        if (subNode.NodeType.Name == "Tenant" && subNode.Entity is TenantDTO entity)
        {
          int? nullable = new int?(subNode.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          entity.NoOfDevices = nullable;
        }
      }
    }

    protected void CalculateNoOfDevicesForTenantParent(StructureNodeDTO meterNode)
    {
      StructureNodeDTO structureNodeDto = meterNode;
      while (structureNodeDto.ParentNode != null && structureNodeDto != structureNodeDto.RootNode && structureNodeDto.NodeType.Name != "Tenant")
        structureNodeDto = structureNodeDto.ParentNode;
      if (!(structureNodeDto.NodeType.Name == "Tenant"))
        return;
      if (!(structureNodeDto.Entity is TenantDTO tenantDto))
        tenantDto = new TenantDTO();
      tenantDto.NoOfDevices = new int?(structureNodeDto.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
    }

    public void OnMissingTranslationRule(object sender, string e)
    {
      this._serialNumbersOfMetersThatAreMissingTranslationRules.Add(e);
    }

    protected void RemoveSerialNumberOfMeterThatIsMissingTranslationRulesFromList(
      string serialNumber)
    {
      this._serialNumbersOfMetersThatAreMissingTranslationRules.Remove(serialNumber);
    }

    protected void WalkStructure(StructureNodeDTO node, ref int meters)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter" || subNode.NodeType.Name == "RadioMeter")
          ++meters;
        this.WalkStructure(subNode, ref meters);
      }
    }

    protected void GetMetersInStructure(StructureNodeDTO node, out int meters)
    {
      meters = 0;
      if (node == null)
        return;
      if (node.NodeType.Name == "Meter" || node.NodeType.Name == "RadioMeter")
        ++meters;
      this.WalkStructure(node, ref meters);
    }

    protected virtual void UpdateDevicesFoundLabel() => throw new NotImplementedException();

    protected Dictionary<string, string> GetMetersWithAesKeysFromDeliveryNote(
      int filterIndex,
      string fileName)
    {
      Dictionary<string, string> fromDeliveryNote = (Dictionary<string, string>) null;
      switch (filterIndex)
      {
        case 1:
          fromDeliveryNote = this.ImportAesKeysFromDeliveryNoteCsv(fileName);
          break;
        case 2:
          fromDeliveryNote = this.ImportAesKeysFromDeliveryNoteXml(fileName);
          break;
      }
      return fromDeliveryNote;
    }

    protected void ImportDeliveryNote(
      ObservableCollection<StructureNodeDTO> structureForSelectedNode)
    {
      if (structureForSelectedNode == null || structureForSelectedNode.Count == 0)
        return;
      try
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Filter = "CSV Document|*.csv|XML Document|*.xml";
        openFileDialog1.Title = Resources.MSS_Client_ImportDeliveryNoteFromFile;
        openFileDialog1.RestoreDirectory = true;
        OpenFileDialog openFileDialog2 = openFileDialog1;
        bool? nullable = openFileDialog2.ShowDialog();
        if (!nullable.HasValue || !nullable.Value)
        {
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
        }
        else
        {
          if (openFileDialog2.FileName == string.Empty)
            return;
          Dictionary<string, string> fromDeliveryNote = this.GetMetersWithAesKeysFromDeliveryNote(openFileDialog2.FilterIndex, openFileDialog2.FileName);
          if (fromDeliveryNote != null && fromDeliveryNote.Any<KeyValuePair<string, string>>())
          {
            List<StructureNodeDTO> radioMeters = StructuresHelper.GetRadioMeters(structureForSelectedNode);
            foreach (KeyValuePair<string, string> keyValuePair in fromDeliveryNote)
            {
              KeyValuePair<string, string> meterWithKey = keyValuePair;
              StructureNodeDTO structureNodeDto1 = radioMeters.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Entity != null && item.Entity is MeterDTO && (item.Entity as MeterDTO).SerialNumber == meterWithKey.Key));
              if (structureNodeDto1 != null && structureNodeDto1.Entity is MeterDTO)
              {
                (structureNodeDto1.Entity as MeterDTO).AES = meterWithKey.Value;
              }
              else
              {
                MeterDTO meterDto = new MeterDTO();
                meterDto.SerialNumber = meterWithKey.Key;
                meterDto.AES = meterWithKey.Value;
                meterDto.DeviceType = DeviceTypeEnum.GenericWMBus;
                StructureNodeDTO structureNodeDto2 = new StructureNodeDTO();
                structureNodeDto2.Name = "RadioMeter";
                structureNodeDto2.Description = "";
                structureNodeDto2.NodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (item => item.Name == "RadioMeter"));
                structureNodeDto2.Entity = (object) meterDto;
                structureNodeDto2.StructureType = new StructureTypeEnum?(StructureTypeEnum.Physical);
                structureNodeDto2.ParentNode = this.SelectedItem;
                StructureNodeDTO node = structureNodeDto2;
                node.Image = StructuresHelper.GetImageForNode(node, true);
                node.ParentNode.SubNodes.Add(node);
                node.IsMeterNonEditable = this.IsItemNonEditable(node);
              }
            }
            if (this is EditPhysicalStructureViewModel)
              this.UpdateDevicesFoundLabel();
            else if (this is CreatePhysicalStructureViewModel)
              this.UpdateDevicesFoundLabel();
          }
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
        }
      }
      catch (Exception ex)
      {
        MSS.Business.Errors.MessageHandler.LogException(ex, MessageCodes.Error);
        this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) (ex.Message + Environment.NewLine + Resources.MSS_Client_CheckFileFormat)), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
      }
    }
  }
}
