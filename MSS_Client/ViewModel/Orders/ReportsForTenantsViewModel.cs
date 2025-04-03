// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ReportsForTenantsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MinomatHandler;
using MSS.Business.DTO;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Controls.CheckableComboBox;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ReportsForTenantsViewModel : ViewModelBase
  {
    private StructureNodeDTO _rootNode;
    private List<TenantInfo> _fullTenantInfos;
    private List<MeterInfo> _fullMetersInfoCollection;
    private IWindowFactory _windowFactory;
    private IRepositoryFactory _repositoryFactory;
    private IRepository<MinomatMeter> _minomatMeterRepository;
    private IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private List<string> _selectedAddresses = new List<string>();
    private List<string> _selectedFloors = new List<string>();
    private List<string> _selectedPositions = new List<string>();
    private List<string> _selectedMinomats = new List<string>();
    private List<string> _selectedStates = new List<string>();
    private Dictionary<Guid, RoutingTable> _minomatMasterRoutingTables = new Dictionary<Guid, RoutingTable>();
    private List<MinomatRadioDetails> _masterRadioDetails;
    private List<Tuple<Guid, string>> _meterGuidsWithMinomatSN = new List<Tuple<Guid, string>>();
    private string _selectAllText = Resources.MSS_Client_SelectAll;
    private ObservableCollection<TenantInfo> _filteredTenantsInfoCollection;
    private ObservableCollection<MinomatMasterInfo> _fullMinomatMastersInfoCollection;
    private ObservableCollection<MinomatSlaveInfo> _minomatSlavesInfoCollection;
    private ObservableCollection<MeterInfo> _filteredMetersInfoCollection;
    private int _totalDevices;
    private bool _isShowDataCollectorsForMeterEnabled;
    private string _registeredDevicesImageLocation;
    private string _registeredDevicesPercentage;
    private string _selectedAddressesText;
    private string _selectedFloorsText;
    private string _selectedPositionsText;
    private string _selectedMinomatsText;
    private string _selectedStatesText;
    private string _busyContent;
    private bool _showProgressCircle;
    private TenantInfo _selectedTenantInfo;
    private StructureNodeDTO _tenantsInfoCollection_SelectedMeter;
    private StructureNodeDTO _tenantsInfoCollection_SelectedSubMeter;
    private bool _isTenantReportTabSelected;
    private bool _isNetworkReportTabSelected;
    private bool _isDeviceReportTabSelected;
    private bool _isSlavesGridVisible;
    private MinomatMasterInfo _minomatMastersSelectedItem;
    private bool _isReadRoutingTableButtonEnabled;
    private bool _isShowRoutingTableButtonEnabled;

    public ReportsForTenantsViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      StructureNodeDTO rootNode)
    {
      this._rootNode = rootNode;
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._minomatMeterRepository = this._repositoryFactory.GetRepository<MinomatMeter>();
      this._minomatRadioDetailsRepository = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
      this._isTenantReportTabSelected = true;
      this._isNetworkReportTabSelected = false;
      this._isDeviceReportTabSelected = false;
      this.OnPropertyChanged(nameof (IsTenantReportTabSelected));
      this.OnPropertyChanged(nameof (IsNetworkReportTabSelected));
      this.OnPropertyChanged(nameof (IsDeviceReportTabSelected));
      this.LoadTenantReportTab();
    }

    private void LoadTenantReportTab()
    {
      this.LoadTenantsInfoCollection();
      this.InitAddressList();
      this.InitFloorList();
      this.InitPositionList();
      this.LoadNumberLabelsAndImages();
      this.IsShowDataCollectorsForMeterEnabled = false;
    }

    private void LoadNetworkReportTab()
    {
      this.IsReadRoutingTableButtonEnabled = false;
      this.IsShowRoutingTableButtonEnabled = false;
      this.IsSlavesGridVisible = false;
      ObservableCollection<StructureNodeDTO> minomats = new ObservableCollection<StructureNodeDTO>();
      this.GetListOfMinomats(this._rootNode, ref minomats);
      this._fullMinomatMastersInfoCollection = new ObservableCollection<MinomatMasterInfo>();
      this._fullMinomatMastersInfoCollection = this.GetMinomatInfos(minomats);
      List<Guid> listOfMinomatMasterIds = this._fullMinomatMastersInfoCollection.Select<MinomatMasterInfo, Guid>((Func<MinomatMasterInfo, Guid>) (item => item.MinomatMaster.Id)).ToList<Guid>();
      if (listOfMinomatMasterIds.Any<Guid>())
        this._masterRadioDetails = this._minomatRadioDetailsRepository.SearchForInMemoryOrDb((Expression<Func<MinomatRadioDetails, bool>>) (item => listOfMinomatMasterIds.Contains(item.Minomat.Id)), (Func<MinomatRadioDetails, bool>) (item => listOfMinomatMasterIds.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
      this.OnPropertyChanged("FullMinomatMastersInfoCollection");
    }

    private void GetListOfMinomats(
      StructureNodeDTO rootNode,
      ref ObservableCollection<StructureNodeDTO> minomats)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        if (subNode.Entity != null && subNode.Entity is MinomatSerializableDTO)
          minomats.Add(subNode);
        this.GetListOfMinomats(subNode, ref minomats);
      }
    }

    private ObservableCollection<MinomatMasterInfo> GetMinomatInfos(
      ObservableCollection<StructureNodeDTO> listOfMinomats)
    {
      ObservableCollection<MinomatMasterInfo> source = new ObservableCollection<MinomatMasterInfo>();
      List<Guid?> minomatIds = listOfMinomats.Select<StructureNodeDTO, Guid?>((Func<StructureNodeDTO, Guid?>) (item => !(item.Entity is MinomatSerializableDTO entity1) ? new Guid?() : new Guid?(entity1.Id))).ToList<Guid?>();
      List<MinomatRadioDetails> list = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => minomatIds.Contains((Guid?) item.Minomat.Id))).ToList<MinomatRadioDetails>();
      foreach (StructureNodeDTO listOfMinomat in (Collection<StructureNodeDTO>) listOfMinomats)
      {
        MinomatSerializableDTO currentMinomat = listOfMinomat.Entity as MinomatSerializableDTO;
        MinomatRadioDetails minomatRadioDetails = list.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == currentMinomat.Id));
        string str1 = "";
        string str2 = "";
        if (!string.IsNullOrEmpty(minomatRadioDetails?.Location))
        {
          string[] strArray = minomatRadioDetails.Location.Split('/');
          str1 = strArray[0];
          str2 = strArray[1];
        }
        if (currentMinomat.IsMaster)
        {
          MinomatMasterInfo minomatMasterInfo = new MinomatMasterInfo()
          {
            MinomatMaster = currentMinomat,
            Address = (this._rootNode.Entity is LocationDTO entity2 ? entity2.Street : (string) null) + " " + (this._rootNode.Entity is LocationDTO entity3 ? entity3.BuildingNr : (string) null),
            Floor = str1 + (!string.IsNullOrEmpty(str1) ? ". " : "") + str2,
            Status = this._minomatMasterRoutingTables.ContainsKey(currentMinomat.Id) ? Resources.MSS_Client_ReadStatus : Resources.MSS_Client_TenantReports_DCStatus,
            Date = DateTime.MinValue,
            MinomatSlavesList = new List<MinomatSlaveInfo>()
          };
          source.Add(minomatMasterInfo);
        }
        else
        {
          MinomatMasterInfo masterForCurrentSlave = source.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item =>
          {
            Guid id = item.MinomatMaster.Id;
            Guid? minomatMasterId = currentMinomat.MinomatMasterId;
            return minomatMasterId.HasValue && id == minomatMasterId.GetValueOrDefault();
          }));
          if (masterForCurrentSlave != null)
            masterForCurrentSlave.MinomatSlavesList.Add(new MinomatSlaveInfo()
            {
              MinomatSlave = currentMinomat,
              Address = (this._rootNode.Entity is LocationDTO entity4 ? entity4.Street : (string) null) + " " + (this._rootNode.Entity is LocationDTO entity5 ? entity5.BuildingNr : (string) null),
              Floor = str1 + (!string.IsNullOrEmpty(str1) ? ". " : "") + str2,
              Status = "",
              NodeId = minomatRadioDetails != null ? minomatRadioDetails.NodeId : "",
              ParentId = list.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == masterForCurrentSlave.MinomatMaster.Id))?.NodeId,
              ImageLocation = "",
              HopCount = "",
              RSSI = ""
            });
        }
      }
      foreach (MinomatMasterInfo minomatMasterInfo in (Collection<MinomatMasterInfo>) source)
      {
        minomatMasterInfo.ReceivedSlavesNumber = minomatMasterInfo.MinomatSlavesList.Count<MinomatSlaveInfo>((Func<MinomatSlaveInfo, bool>) (item => item.MinomatSlave.Registered));
        minomatMasterInfo.ReceivedSlavesString = minomatMasterInfo.ReceivedSlavesNumber.ToString() + " / " + (object) minomatMasterInfo.MinomatSlavesList.Count;
        this.UpdateMasterAndSlaveImagesAndSlaveStatuses(minomatMasterInfo);
      }
      return source;
    }

    private void UpdateMasterAndSlaveImagesAndSlaveStatuses(MinomatMasterInfo minomatMasterInfo)
    {
      minomatMasterInfo.ImageLocation = minomatMasterInfo.ReceivedSlavesNumber == minomatMasterInfo.MinomatSlavesList.Count ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
      RoutingTable source;
      this._minomatMasterRoutingTables.TryGetValue(minomatMasterInfo.MinomatMaster.Id, out source);
      foreach (MinomatSlaveInfo minomatSlaves in minomatMasterInfo.MinomatSlavesList)
      {
        MinomatSlaveInfo minomatSlaveInfo = minomatSlaves;
        if ((source != null ? source.FirstOrDefault<RoutingRow>((Func<RoutingRow, bool>) (x => x.NodeId.ToString() == minomatSlaveInfo.NodeId)) : (RoutingRow) null) != null)
        {
          minomatSlaveInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
          minomatSlaveInfo.Status = Resources.MSS_Client_StatusConnected;
        }
        else
        {
          minomatSlaveInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
          minomatSlaveInfo.Status = Resources.MSS_Client_StatusNotConnected;
        }
      }
    }

    private void LoadDeviceReportTab()
    {
      this._fullMetersInfoCollection = new List<MeterInfo>();
      List<string> source = new List<string>();
      foreach (TenantInfo fullTenantInfo in this._fullTenantInfos)
      {
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) fullTenantInfo.SubNodes)
          this.GetMeters(fullTenantInfo, subNode, ref this._fullMetersInfoCollection);
      }
      List<Guid> meterIds = this._fullMetersInfoCollection.Select<MeterInfo, Guid>((Func<MeterInfo, Guid>) (m => m.Meter.Id)).ToList<Guid>();
      List<MinomatMeter> list1 = this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => meterIds.Contains(item.Meter.Id) && item.Status != new MeterStatusEnum?())).ToList<MinomatMeter>();
      foreach (MeterInfo fullMetersInfo in this._fullMetersInfoCollection)
      {
        MeterInfo meterInfo = fullMetersInfo;
        List<MinomatMeter> list2 = list1.Where<MinomatMeter>((Func<MinomatMeter, bool>) (item => item.Meter.Id == meterInfo.Meter.Id)).ToList<MinomatMeter>();
        if (list2 != null && list2.Any<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
        {
          MeterStatusEnum? status = item.Status;
          MeterStatusEnum meterStatusEnum = MeterStatusEnum.Registered;
          return status.GetValueOrDefault() == meterStatusEnum && status.HasValue;
        })))
        {
          meterInfo.MinomatGsmId = list2.First<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
          {
            MeterStatusEnum? status = item.Status;
            MeterStatusEnum meterStatusEnum = MeterStatusEnum.Registered;
            return status.GetValueOrDefault() == meterStatusEnum && status.HasValue;
          })).Minomat.RadioId;
          meterInfo.Status = list2.First<MinomatMeter>((Func<MinomatMeter, bool>) (item =>
          {
            MeterStatusEnum? status = item.Status;
            MeterStatusEnum meterStatusEnum = MeterStatusEnum.Registered;
            return status.GetValueOrDefault() == meterStatusEnum && status.HasValue;
          })).Status;
        }
        else
        {
          meterInfo.MinomatGsmId = list2.Any<MinomatMeter>() ? list2.First<MinomatMeter>().Minomat.RadioId : "";
          meterInfo.Status = list2.Any<MinomatMeter>() ? list2.First<MinomatMeter>().Status : new MeterStatusEnum?();
        }
        MeterInfo meterInfo1 = meterInfo;
        int num;
        if (meterInfo.Status.HasValue)
        {
          MeterStatusEnum? status = meterInfo.Status;
          MeterStatusEnum meterStatusEnum = MeterStatusEnum.Registered;
          num = status.GetValueOrDefault() == meterStatusEnum ? (status.HasValue ? 1 : 0) : 0;
        }
        else
          num = 0;
        meterInfo1.Registered = num != 0;
        foreach (MinomatMeter minomatMeter1 in list2)
        {
          MinomatMeter minomatMeter = minomatMeter1;
          if (minomatMeter != null && !minomatMeter.Minomat.IsDeactivated && !source.Contains(minomatMeter.Minomat.RadioId))
            source.Add(minomatMeter.Minomat.RadioId);
          if (!this._meterGuidsWithMinomatSN.Any<Tuple<Guid, string>>((Func<Tuple<Guid, string>, bool>) (item => item.Item1 == minomatMeter.Meter.Id && item.Item2 == minomatMeter.Minomat.RadioId)))
            this._meterGuidsWithMinomatSN.Add(new Tuple<Guid, string>(minomatMeter.Meter.Id, minomatMeter.Minomat.RadioId));
        }
      }
      this.FilteredMetersInfoCollection = new ObservableCollection<MeterInfo>();
      foreach (MeterInfo fullMetersInfo in this._fullMetersInfoCollection)
        this.FilteredMetersInfoCollection.Add(fullMetersInfo);
      this.InitMinomatsList(source.Distinct<string>().ToList<string>());
      this.InitStatesList();
    }

    private void GetMeters(
      TenantInfo tenantInfo,
      StructureNodeDTO node,
      ref List<MeterInfo> meterInfos)
    {
      if (node.Entity is MeterDTO)
        meterInfos.Add(new MeterInfo()
        {
          Meter = node.Entity as MeterDTO,
          TenantName = tenantInfo.Tenant.Name,
          Address = tenantInfo.Address,
          FloorPosition = tenantInfo.FloorPosition
        });
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
        this.GetMeters(tenantInfo, subNode, ref meterInfos);
    }

    private void LoadNumberLabelsAndImages()
    {
      int totalDevices = 0;
      int registeredDevices = 0;
      this._fullTenantInfos.ForEach((Action<TenantInfo>) (tenant =>
      {
        totalDevices += tenant.NoOfDevices;
        registeredDevices += tenant.RegisteredDevices;
      }));
      this.TotalDevices = totalDevices;
      double num = totalDevices != 0 ? (double) registeredDevices / (double) totalDevices * 100.0 : 0.0;
      this.RegisteredDevicesPercentage = totalDevices != 0 ? string.Format("{0:0.00}%", (object) num) : "";
      this.RegisteredDevicesImageLocation = num < 0.0 || num >= 85.0 ? (num < 85.0 || num >= 95.0 ? "pack://application:,,,/Styles;component/Images/Settings/light-green.png" : "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png") : "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
    }

    private void LoadTenantsInfoCollection()
    {
      this._filteredTenantsInfoCollection = (ObservableCollection<TenantInfo>) new RadObservableCollection<TenantInfo>();
      this._fullTenantInfos = new List<TenantInfo>();
      List<MinomatMeter> metersForAllMeters = this.GetMinomatMetersForAllMeters();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._rootNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Tenant")
        {
          TenantDTO entity1 = subNode.Entity as TenantDTO;
          TenantInfo tenantInfo = new TenantInfo()
          {
            SubNodes = new ObservableCollection<StructureNodeDTO>(),
            Tenant = entity1,
            Address = (this._rootNode.Entity is LocationDTO entity2 ? entity2.Street : (string) null) + " " + (this._rootNode.Entity is LocationDTO entity3 ? entity3.BuildingNr : (string) null)
          };
          tenantInfo.FloorPosition = tenantInfo?.Tenant?.FloorNr + "." + tenantInfo?.Tenant?.FloorName + " / " + tenantInfo?.Tenant?.ApartmentNr + " " + tenantInfo?.Tenant?.Direction;
          int noOfDevices = 0;
          int registeredDevices = 0;
          int openDevices = 0;
          this.SetMeterDetailsForCurrentTenant(subNode, tenantInfo.SubNodes, metersForAllMeters, ref noOfDevices, ref openDevices, ref registeredDevices);
          tenantInfo.NoOfDevices = noOfDevices;
          tenantInfo.RegisteredDevices = registeredDevices;
          tenantInfo.OpenDevices = openDevices;
          tenantInfo.OpenDevicesString = openDevices.ToString() + "/" + (object) noOfDevices;
          if (noOfDevices > 0)
            tenantInfo.ImageLocation = registeredDevices != noOfDevices ? "pack://application:,,,/Styles;component/Images/Settings/light-red.png" : "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
          this._filteredTenantsInfoCollection.Add(tenantInfo);
          this._fullTenantInfos.Add(tenantInfo);
        }
      }
    }

    private void SetMeterDetailsForCurrentTenant(
      StructureNodeDTO node,
      ObservableCollection<StructureNodeDTO> collectionToAddSubnodeDetailsTo,
      List<MinomatMeter> minomatMetersForAllMeters,
      ref int noOfDevices,
      ref int openDevices,
      ref int registeredDevices)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        StructureNodeDTO currentSubnode = subNode;
        if (currentSubnode.NodeType.Name == "Meter")
        {
          ++noOfDevices;
          if (collectionToAddSubnodeDetailsTo == null)
            collectionToAddSubnodeDetailsTo = new ObservableCollection<StructureNodeDTO>();
          if (currentSubnode.ParentNode.NodeType.Name != "Meter")
            collectionToAddSubnodeDetailsTo.Add(currentSubnode);
          MeterDTO currentMeter = currentSubnode.Entity as MeterDTO;
          if (currentMeter != null)
          {
            int num1 = 0;
            int num2 = 0;
            List<MinomatMeter> list = minomatMetersForAllMeters.Where<MinomatMeter>((Func<MinomatMeter, bool>) (item => item.Meter.Id == currentMeter.Id)).ToList<MinomatMeter>();
            if (list != null && list.Any<MinomatMeter>())
            {
              num2 += list.Count<MinomatMeter>((Func<MinomatMeter, bool>) (item => item.Status.HasValue && item.Status.Value == MeterStatusEnum.Registered));
              num1 += list.Count;
            }
            if (num2 > 0)
              ++registeredDevices;
            StructureNodeDTO structureNodeDto = currentSubnode;
            BitmapImage bitmapImageFromFiles;
            if (num2 <= 0)
              bitmapImageFromFiles = ImageHelper.Instance.GetBitmapImageFromFiles(new string[1]
              {
                "pack://application:,,,/Styles;component/Images/Settings/light-red.png"
              });
            else
              bitmapImageFromFiles = ImageHelper.Instance.GetBitmapImageFromFiles(new string[1]
              {
                "pack://application:,,,/Styles;component/Images/Settings/light-green.png"
              });
            structureNodeDto.Image = bitmapImageFromFiles;
            if (num1 == 0)
              ++openDevices;
            if (list != null)
              currentMeter.MinomatMeters = list;
          }
        }
        this.SetMeterDetailsForCurrentTenant(currentSubnode, collectionToAddSubnodeDetailsTo.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Id == currentSubnode.Id))?.SubNodes, minomatMetersForAllMeters, ref noOfDevices, ref openDevices, ref registeredDevices);
      }
    }

    private List<MinomatMeter> GetMinomatMetersForAllMeters()
    {
      List<Guid> meterIds = new List<Guid>();
      this.WalkStructureAndGetMeterIds(this._rootNode, ref meterIds);
      return this._minomatMeterRepository.Where((Expression<Func<MinomatMeter, bool>>) (item => meterIds.Contains(item.Meter.Id))).ToList<MinomatMeter>();
    }

    private void WalkStructureAndGetMeterIds(StructureNodeDTO rootNode, ref List<Guid> meterIds)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        if (subNode.Entity is MeterDTO entity && entity.Id != Guid.Empty)
          meterIds.Add(entity.Id);
        this.WalkStructureAndGetMeterIds(subNode, ref meterIds);
      }
    }

    public List<CheckableComboBoxItem> AddressList { get; private set; }

    private void InitAddressList()
    {
      this.AddressList = new List<CheckableComboBoxItem>();
      List<string> list = this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Address)).Distinct<string>().ToList<string>();
      list.Insert(0, this._selectAllText);
      foreach (string str in list)
        this.AddressList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
      this.OnPropertyChanged("AddressList");
      this._selectedAddresses.Clear();
      this.AddressList.ForEach((Action<CheckableComboBoxItem>) (item =>
      {
        item.IsChecked = true;
        this._selectedAddresses.Add(item.Text);
      }));
      this.SelectedAddressesText = this._selectAllText;
    }

    public List<CheckableComboBoxItem> FloorList { get; private set; }

    private void InitFloorList()
    {
      this.FloorList = new List<CheckableComboBoxItem>();
      List<string> list = this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Tenant.FloorNr + "." + item.Tenant.FloorName)).Distinct<string>().ToList<string>();
      list.Insert(0, this._selectAllText);
      foreach (string str in list)
        this.FloorList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
      this.OnPropertyChanged("FloorList");
      this._selectedFloors.Clear();
      this.FloorList.ForEach((Action<CheckableComboBoxItem>) (item =>
      {
        item.IsChecked = true;
        this._selectedFloors.Add(item.Text);
      }));
      this.SelectedFloorsText = this._selectAllText;
    }

    public List<CheckableComboBoxItem> PositionList { get; private set; }

    private void InitPositionList()
    {
      this.PositionList = new List<CheckableComboBoxItem>();
      List<string> list = this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Tenant.ApartmentNr + " " + item.Tenant.Direction)).Distinct<string>().ToList<string>();
      list.Insert(0, this._selectAllText);
      foreach (string str in list)
        this.PositionList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
      this.OnPropertyChanged("PositionList");
      this._selectedPositions.Clear();
      this.PositionList.ForEach((Action<CheckableComboBoxItem>) (item =>
      {
        item.IsChecked = true;
        this._selectedPositions.Add(item.Text);
      }));
      this.SelectedPositionsText = this._selectAllText;
    }

    public List<CheckableComboBoxItem> MinomatsList { get; private set; }

    private void InitMinomatsList(List<string> minomatSerialNumbers)
    {
      this.MinomatsList = new List<CheckableComboBoxItem>();
      minomatSerialNumbers.Sort();
      minomatSerialNumbers.Insert(0, this._selectAllText);
      foreach (string minomatSerialNumber in minomatSerialNumbers)
        this.MinomatsList.Add(new CheckableComboBoxItem()
        {
          Text = minomatSerialNumber
        });
      this.OnPropertyChanged("MinomatsList");
      this._selectedMinomats.Clear();
      this.MinomatsList.ForEach((Action<CheckableComboBoxItem>) (item =>
      {
        item.IsChecked = true;
        this._selectedMinomats.Add(item.Text);
      }));
      this.SelectedMinomatsText = this._selectAllText;
    }

    public List<CheckableComboBoxItem> StatesList { get; private set; }

    private void InitStatesList()
    {
      this.StatesList = new List<CheckableComboBoxItem>();
      this.StatesList.Add(new CheckableComboBoxItem()
      {
        Text = Resources.MSS_MeterStatus_Registered
      });
      this.StatesList.Add(new CheckableComboBoxItem()
      {
        Text = Resources.MSS_MeterStatus_NotRegistered
      });
      this.StatesList.Sort((Comparison<CheckableComboBoxItem>) ((item1, item2) => item2.Text.CompareTo(item1.Text)));
      this.StatesList.Insert(0, new CheckableComboBoxItem()
      {
        Text = this._selectAllText
      });
      this.OnPropertyChanged("StatesList");
      this._selectedStates.Clear();
      this.StatesList.ForEach((Action<CheckableComboBoxItem>) (item =>
      {
        item.IsChecked = true;
        this._selectedStates.Add(item.Text);
      }));
      this.SelectedStatesText = this._selectAllText;
    }

    private void FilterTenantInfoCollection()
    {
      bool flag1 = this._selectedAddresses.Count == this.AddressList.Count;
      bool flag2 = this._selectedFloors.Count == this.FloorList.Count;
      bool flag3 = this._selectedPositions.Count == this.PositionList.Count;
      this._filteredTenantsInfoCollection = new ObservableCollection<TenantInfo>();
      foreach (TenantInfo fullTenantInfo in this._fullTenantInfos)
      {
        if ((flag1 || this._selectedAddresses.Contains(fullTenantInfo.Address)) & (flag2 || this._selectedFloors.Contains(fullTenantInfo.Tenant?.FloorNr + "." + fullTenantInfo.Tenant?.FloorName)) & (flag3 || this._selectedPositions.Contains(fullTenantInfo.Tenant?.ApartmentNr + " " + fullTenantInfo.Tenant?.Direction)))
          this._filteredTenantsInfoCollection.Add(fullTenantInfo);
      }
      this.OnPropertyChanged("FilteredTenantsInfoCollection");
    }

    private void FilterMeterInfoCollection()
    {
      bool flag1 = this._selectedMinomats.Count == this.MinomatsList.Count;
      bool flag2 = this._selectedStates.Count == this.StatesList.Count;
      this._filteredMetersInfoCollection = new ObservableCollection<MeterInfo>();
      foreach (MeterInfo fullMetersInfo in this._fullMetersInfoCollection)
      {
        MeterInfo meterInfo = fullMetersInfo;
        if ((flag1 || this._meterGuidsWithMinomatSN.Where<Tuple<Guid, string>>((Func<Tuple<Guid, string>, bool>) (item => item.Item1 == meterInfo.Meter.Id)).ToList<Tuple<Guid, string>>().Any<Tuple<Guid, string>>((Func<Tuple<Guid, string>, bool>) (element => this._selectedMinomats.Contains(element.Item2)))) & (flag2 || this.IsMeterStateInSelectedStates(meterInfo)))
          this._filteredMetersInfoCollection.Add(meterInfo);
      }
      this.OnPropertyChanged("FilteredMetersInfoCollection");
    }

    private bool IsMeterStateInSelectedStates(MeterInfo meterInfo)
    {
      return this._selectedStates.Contains(Resources.MSS_MeterStatus_Registered) && meterInfo.Registered || this._selectedStates.Contains(Resources.MSS_MeterStatus_NotRegistered) && !meterInfo.Registered;
    }

    public ICommand OnCheckChangedAddressCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.AddressList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedAddresses.Clear();
              this.AddressList.ForEach((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedAddresses.Add(item.Text);
              }));
              this.SelectedAddressesText = checkableComboBoxItem2.Text;
            }
            else
            {
              this._selectedAddresses.Clear();
              this.AddressList.ForEach((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
              this.SelectedAddressesText = string.Empty;
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.AddressList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.AddressList.Count - 1 && !this.AddressList[0].IsChecked;
            this._selectedAddresses = this.AddressList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
            this.SelectedAddressesText = this._selectedAddresses.Any<string>() ? (this._selectedAddresses.Count<string>() == this.AddressList.Count ? this._selectAllText : string.Join("; ", (IEnumerable<string>) this._selectedAddresses)) : string.Empty;
          }
          this.FilterTenantInfoCollection();
        }));
      }
    }

    public ICommand OnCheckChangedFloorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.FloorList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedFloors.Clear();
              this.FloorList.ForEach((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedFloors.Add(item.Text);
              }));
              this.SelectedFloorsText = checkableComboBoxItem2.Text;
            }
            else
            {
              this._selectedFloors.Clear();
              this.FloorList.ForEach((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
              this.SelectedFloorsText = string.Empty;
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.FloorList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.FloorList.Count - 1 && !this.FloorList[0].IsChecked;
            this._selectedFloors = this.FloorList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
            this.SelectedFloorsText = this._selectedFloors.Any<string>() ? (this._selectedFloors.Count<string>() == this.FloorList.Count ? this._selectAllText : string.Join("; ", (IEnumerable<string>) this._selectedFloors)) : string.Empty;
          }
          this.FilterTenantInfoCollection();
        }));
      }
    }

    public ICommand OnCheckChangedPositionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.PositionList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedPositions.Clear();
              this.PositionList.ForEach((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedPositions.Add(item.Text);
              }));
              this.SelectedPositionsText = checkableComboBoxItem2.Text;
            }
            else
            {
              this._selectedPositions.Clear();
              this.PositionList.ForEach((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
              this.SelectedPositionsText = string.Empty;
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.PositionList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.PositionList.Count - 1 && !this.PositionList[0].IsChecked;
            this._selectedPositions = this.PositionList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
            this.SelectedPositionsText = this._selectedPositions.Any<string>() ? (this._selectedPositions.Count<string>() == this.PositionList.Count ? this._selectAllText : string.Join("; ", (IEnumerable<string>) this._selectedPositions)) : string.Empty;
          }
          this.FilterTenantInfoCollection();
        }));
      }
    }

    public ICommand OnCheckChangedMinomatCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.MinomatsList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedMinomats.Clear();
              this.MinomatsList.ForEach((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedMinomats.Add(item.Text);
              }));
              this.SelectedMinomatsText = checkableComboBoxItem2.Text;
            }
            else
            {
              this._selectedMinomats.Clear();
              this.MinomatsList.ForEach((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
              this.SelectedMinomatsText = string.Empty;
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.MinomatsList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.MinomatsList.Count - 1 && !this.MinomatsList[0].IsChecked;
            this._selectedMinomats = this.MinomatsList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
            this.SelectedMinomatsText = this._selectedMinomats.Any<string>() ? (this._selectedMinomats.Count<string>() == this.MinomatsList.Count ? this._selectAllText : string.Join("; ", (IEnumerable<string>) this._selectedMinomats)) : string.Empty;
          }
          this.FilterMeterInfoCollection();
        }));
      }
    }

    public ICommand OnCheckChangedStateCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CheckableComboBoxItem checkableComboBoxItem1 = parameter as CheckableComboBoxItem;
          CheckableComboBoxItem checkableComboBoxItem2 = this.StatesList.FirstOrDefault<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.Text == this._selectAllText));
          if (checkableComboBoxItem1 == checkableComboBoxItem2)
          {
            if (checkableComboBoxItem2.IsChecked)
            {
              this._selectedStates.Clear();
              this.StatesList.ForEach((Action<CheckableComboBoxItem>) (item =>
              {
                item.IsChecked = true;
                this._selectedStates.Add(item.Text);
              }));
              this.SelectedStatesText = checkableComboBoxItem2.Text;
            }
            else
            {
              this._selectedStates.Clear();
              this.StatesList.ForEach((Action<CheckableComboBoxItem>) (item => item.IsChecked = false));
              this.SelectedStatesText = string.Empty;
            }
          }
          else
          {
            checkableComboBoxItem2.IsChecked = this.StatesList.Count<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)) == this.StatesList.Count - 1 && !this.StatesList[0].IsChecked;
            this._selectedStates = this.StatesList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
            this.SelectedStatesText = this._selectedStates.Any<string>() ? (this._selectedStates.Count<string>() == this.StatesList.Count ? this._selectAllText : string.Join("; ", (IEnumerable<string>) this._selectedStates)) : string.Empty;
          }
          this.FilterMeterInfoCollection();
        }));
      }
    }

    public ICommand ShowDataCollectorsForMeter
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          StructureNodeDTO structureNodeDto = this._tenantsInfoCollection_SelectedMeter ?? this._tenantsInfoCollection_SelectedSubMeter;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ShowDataCollectorsForMeterViewModel>((IParameter) new ConstructorArgument("selectedMeter", (object) structureNodeDto), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)));
        }));
      }
    }

    public ICommand ReadRoutingTableCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.MSS_Client_ReadingRoutingTable;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.GetRoutingTable(this.MinomatMastersSelectedItem.MinomatMaster, out RoutingTable _);
              if (gmmConfiguratorResult.IsSuccess)
              {
                RoutingTable routingTable;
                this._minomatMasterRoutingTables.TryGetValue(this.MinomatMastersSelectedItem.MinomatMaster.Id, out routingTable);
                if (routingTable == null)
                  this._minomatMasterRoutingTables.Add(this.MinomatMastersSelectedItem.MinomatMaster.Id, routingTable);
                else
                  this._minomatMasterRoutingTables[this.MinomatMastersSelectedItem.MinomatMaster.Id] = routingTable;
                int numberOfReceivedSlaves = 0;
                this.FullMinomatMastersInfoCollection.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item => item.MinomatMaster.Id == this.MinomatMastersSelectedItem.MinomatMaster.Id))?.MinomatSlavesList.ForEach((Action<MinomatSlaveInfo>) (slave =>
                {
                  RoutingRow routingRow = routingTable.FirstOrDefault<RoutingRow>((Func<RoutingRow, bool>) (x => x.NodeId.ToString() == slave.NodeId));
                  if (routingRow == null)
                    return;
                  slave.HopCount = ((int) routingRow.HopCount).ToString();
                  slave.RSSI = routingRow.RSSI_dBm.ToString() + " dBm";
                  ++numberOfReceivedSlaves;
                }));
                Guid selectedMasterId = this._minomatMastersSelectedItem.MinomatMaster.Id;
                MinomatMasterInfo minomatMasterInfo = this.FullMinomatMastersInfoCollection.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item => item.MinomatMaster.Id == selectedMasterId));
                if (minomatMasterInfo != null)
                {
                  minomatMasterInfo.ReceivedSlavesNumber = numberOfReceivedSlaves;
                  int count = minomatMasterInfo.MinomatSlavesList != null ? minomatMasterInfo.MinomatSlavesList.Count : 0;
                  minomatMasterInfo.ReceivedSlavesString = numberOfReceivedSlaves.ToString() + " / " + (object) count;
                  minomatMasterInfo.Status = Resources.MSS_Client_ReadStatus;
                  this.UpdateMasterAndSlaveImagesAndSlaveStatuses(minomatMasterInfo);
                }
                this.MinomatMastersSelectedItem = this.FullMinomatMastersInfoCollection.FirstOrDefault<MinomatMasterInfo>((Func<MinomatMasterInfo, bool>) (item => item.MinomatMaster.Id == selectedMasterId));
                this.IsShowRoutingTableButtonEnabled = this._minomatMastersSelectedItem != null && this._minomatMasterRoutingTables.ContainsKey(this._minomatMastersSelectedItem.MinomatMaster.Id);
              }
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              this.ShowProgressCircle = false;
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, ex.Message, false)))));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public ICommand ShowRoutingTableCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ShowRoutingTableViewModel>((IParameter) new ConstructorArgument("minomatMastersInfoCollection", (object) this._fullMinomatMastersInfoCollection), (IParameter) new ConstructorArgument("selectedMinomatMasterGuid", (object) this.MinomatMastersSelectedItem.MinomatMaster.Id), (IParameter) new ConstructorArgument("minomatMasterRoutingTables", (object) this._minomatMasterRoutingTables), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory)))));
      }
    }

    public ObservableCollection<TenantInfo> FilteredTenantsInfoCollection
    {
      get => this._filteredTenantsInfoCollection;
      set
      {
        this._filteredTenantsInfoCollection = value;
        this.OnPropertyChanged(nameof (FilteredTenantsInfoCollection));
      }
    }

    public ObservableCollection<MinomatMasterInfo> FullMinomatMastersInfoCollection
    {
      get => this._fullMinomatMastersInfoCollection;
      set
      {
        this._fullMinomatMastersInfoCollection = value;
        this.OnPropertyChanged(nameof (FullMinomatMastersInfoCollection));
      }
    }

    public ObservableCollection<MinomatSlaveInfo> MinomatSlavesInfoCollection
    {
      get => this._minomatSlavesInfoCollection;
      set
      {
        this._minomatSlavesInfoCollection = value;
        this.OnPropertyChanged(nameof (MinomatSlavesInfoCollection));
      }
    }

    public ObservableCollection<MeterInfo> FilteredMetersInfoCollection
    {
      get => this._filteredMetersInfoCollection;
      set
      {
        this._filteredMetersInfoCollection = value;
        this.OnPropertyChanged(nameof (FilteredMetersInfoCollection));
      }
    }

    public int TotalDevices
    {
      get => this._totalDevices;
      set
      {
        this._totalDevices = value;
        this.OnPropertyChanged(nameof (TotalDevices));
      }
    }

    public bool IsShowDataCollectorsForMeterEnabled
    {
      get => this._isShowDataCollectorsForMeterEnabled;
      set
      {
        this._isShowDataCollectorsForMeterEnabled = value;
        this.OnPropertyChanged(nameof (IsShowDataCollectorsForMeterEnabled));
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

    public string RegisteredDevicesPercentage
    {
      get => this._registeredDevicesPercentage;
      set
      {
        this._registeredDevicesPercentage = value;
        this.OnPropertyChanged(nameof (RegisteredDevicesPercentage));
      }
    }

    public string SelectedAddressesText
    {
      get => this._selectedAddressesText;
      set
      {
        this._selectedAddressesText = value;
        this.OnPropertyChanged(nameof (SelectedAddressesText));
      }
    }

    public string SelectedFloorsText
    {
      get => this._selectedFloorsText;
      set
      {
        this._selectedFloorsText = value;
        this.OnPropertyChanged(nameof (SelectedFloorsText));
      }
    }

    public string SelectedPositionsText
    {
      get => this._selectedPositionsText;
      set
      {
        this._selectedPositionsText = value;
        this.OnPropertyChanged(nameof (SelectedPositionsText));
      }
    }

    public string SelectedMinomatsText
    {
      get => this._selectedMinomatsText;
      set
      {
        this._selectedMinomatsText = value;
        this.OnPropertyChanged(nameof (SelectedMinomatsText));
      }
    }

    public string SelectedStatesText
    {
      get => this._selectedStatesText;
      set
      {
        this._selectedStatesText = value;
        this.OnPropertyChanged(nameof (SelectedStatesText));
      }
    }

    public string BusyContent
    {
      get => this._busyContent;
      set
      {
        this._busyContent = value;
        this.OnPropertyChanged(nameof (BusyContent));
      }
    }

    public bool ShowProgressCircle
    {
      get => this._showProgressCircle;
      set
      {
        this._showProgressCircle = value;
        this.OnPropertyChanged(nameof (ShowProgressCircle));
      }
    }

    public TenantInfo SelectedTenantInfo
    {
      get => this._selectedTenantInfo;
      set
      {
        this._selectedTenantInfo = value;
        this.OnPropertyChanged(nameof (SelectedTenantInfo));
        this.IsShowDataCollectorsForMeterEnabled = false;
      }
    }

    public StructureNodeDTO TenantsInfoCollection_SelectedMeter
    {
      get => this._tenantsInfoCollection_SelectedMeter;
      set
      {
        this._tenantsInfoCollection_SelectedMeter = value;
        this.OnPropertyChanged(nameof (TenantsInfoCollection_SelectedMeter));
        this._selectedTenantInfo = (TenantInfo) null;
        this.OnPropertyChanged("SelectedTenantInfo");
        this._tenantsInfoCollection_SelectedSubMeter = (StructureNodeDTO) null;
        this.OnPropertyChanged("TenantsInfoCollection_SelectedSubMeter");
        this.IsShowDataCollectorsForMeterEnabled = value != null && value.NodeType.Name == "Meter";
      }
    }

    public StructureNodeDTO TenantsInfoCollection_SelectedSubMeter
    {
      get => this._tenantsInfoCollection_SelectedSubMeter;
      set
      {
        this._tenantsInfoCollection_SelectedSubMeter = value;
        this.OnPropertyChanged(nameof (TenantsInfoCollection_SelectedSubMeter));
        this._selectedTenantInfo = (TenantInfo) null;
        this.OnPropertyChanged("SelectedTenantInfo");
        this._tenantsInfoCollection_SelectedMeter = (StructureNodeDTO) null;
        this.OnPropertyChanged("TenantsInfoCollection_SelectedMeter");
        this.IsShowDataCollectorsForMeterEnabled = value != null && value.NodeType.Name == "Meter";
      }
    }

    public bool IsTenantReportTabSelected
    {
      get => this._isTenantReportTabSelected;
      set
      {
        this._isTenantReportTabSelected = value;
        if (value)
          this.LoadTenantReportTab();
        this.OnPropertyChanged(nameof (IsTenantReportTabSelected));
      }
    }

    public bool IsNetworkReportTabSelected
    {
      get => this._isNetworkReportTabSelected;
      set
      {
        this._isNetworkReportTabSelected = value;
        if (value)
          this.LoadNetworkReportTab();
        this.OnPropertyChanged(nameof (IsNetworkReportTabSelected));
      }
    }

    public bool IsDeviceReportTabSelected
    {
      get => this._isDeviceReportTabSelected;
      set
      {
        this._isDeviceReportTabSelected = value;
        if (value)
          this.LoadDeviceReportTab();
        this.OnPropertyChanged(nameof (IsDeviceReportTabSelected));
      }
    }

    public bool IsSlavesGridVisible
    {
      get => this._isSlavesGridVisible;
      set
      {
        this._isSlavesGridVisible = value;
        this.OnPropertyChanged(nameof (IsSlavesGridVisible));
      }
    }

    public MinomatMasterInfo MinomatMastersSelectedItem
    {
      get => this._minomatMastersSelectedItem;
      set
      {
        this._minomatMastersSelectedItem = value;
        this.MinomatSlavesInfoCollection = (ObservableCollection<MinomatSlaveInfo>) null;
        if (value != null)
        {
          this.MinomatSlavesInfoCollection = new ObservableCollection<MinomatSlaveInfo>();
          foreach (MinomatSlaveInfo minomatSlaves in value.MinomatSlavesList)
            this.MinomatSlavesInfoCollection.Add(minomatSlaves);
        }
        this.IsSlavesGridVisible = value != null;
        MinomatRadioDetails minomatRadioDetails = this._masterRadioDetails == null || value == null ? (MinomatRadioDetails) null : this._masterRadioDetails.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == this._minomatMastersSelectedItem.MinomatMaster.Id));
        this.IsReadRoutingTableButtonEnabled = value != null && minomatRadioDetails != null && minomatRadioDetails.LastStartOn.HasValue;
        this.IsShowRoutingTableButtonEnabled = value != null && this._minomatMasterRoutingTables.ContainsKey(this._minomatMastersSelectedItem.MinomatMaster.Id);
        this.OnPropertyChanged(nameof (MinomatMastersSelectedItem));
      }
    }

    public bool IsReadRoutingTableButtonEnabled
    {
      get => this._isReadRoutingTableButtonEnabled;
      set
      {
        this._isReadRoutingTableButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsReadRoutingTableButtonEnabled));
      }
    }

    public bool IsShowRoutingTableButtonEnabled
    {
      get => this._isShowRoutingTableButtonEnabled;
      set
      {
        this._isShowRoutingTableButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsShowRoutingTableButtonEnabled));
      }
    }
  }
}
