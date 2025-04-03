// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ManuallyAssignMetersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Controls.CheckableComboBox;
using MSS_Client.Utils;
using MVVM.Commands;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ManuallyAssignMetersViewModel : MVVM.ViewModel.ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private IWindowFactory _windowFactory;
    private StructureNodeDTO _structureNode;
    private IRepository<MinomatMeter> _minomatMeterRepository;
    private IRepository<Meter> _meterRepository;
    private MinomatSerializableDTO _selectedMinomat;
    private List<TenantInfo> _fullTenantInfos;
    private List<string> _selectedAddresses;
    private List<string> _selectedFloors;
    private List<string> _selectedPositions;
    private int oldNoOfSelectedDevices;
    private string _selectedAddressesText;
    private string _selectedFloorsText;
    private string _selectedPositionsText;
    private string _title;
    private string _noOfSelectedDevicesLabel;
    private ObservableCollection<TenantInfo> _tenantsInfoCollection;
    private bool _showProgressCircle;
    private List<TenantInfo> _selectedItems = new List<TenantInfo>();
    private MVVM.ViewModel.ViewModelBase _messageUserControl;

    public ManuallyAssignMetersViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      StructureNodeDTO node)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._structureNode = node;
      this._minomatMeterRepository = this._repositoryFactory.GetRepository<MinomatMeter>();
      this._meterRepository = this._repositoryFactory.GetRepository<Meter>();
      this._selectedMinomat = node?.Entity as MinomatSerializableDTO;
      this.Title = this._selectedMinomat != null ? Resources.AssignMeters_Expert_AssignDevicesManuallyToMinomat + this._selectedMinomat.RadioId : Resources.AssignMeters_Expert_AssignDevicesManuallyToMinomat;
      this.ResetNumberOfSelectedDevices();
      this.LoadTenantsInfoCollection();
      this.InitAddressList();
      this.InitFloorList();
      this.InitPositionList();
    }

    public ICommand AssignDevicesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (this.SelectedItems.Sum<TenantInfo>((Func<TenantInfo, int>) (item => item.NoOfDevices)) > 300)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), Resources.ManuallyAssignMeters_MoreThan300Selected, false);
          }
          else
          {
            this.ShowProgressCircle = true;
            bool success = false;
            ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
            {
              success = this.SaveAndRegisterMinomatMeters(this.GetMetersFromSelectedTenants());
              this.ShowProgressCircle = false;
              if (success)
              {
                this.ResetNumberOfSelectedDevices();
                this.LoadTenantsInfoCollection();
                this.OnPropertyChanged("TenantsInfoCollection");
              }
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = success ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_ExecuteInstallationOrder_MeterRegistrationSuccessful) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_ExecuteInstallationOrder_MeterRegistrationError)));
            }));
          }
        });
      }
    }

    public ICommand OnCheckChangedAddressCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._selectedAddresses = this.AddressList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
          this.SelectedAddressesText = !this._selectedAddresses.Any<string>() ? "" : string.Join("; ", (IEnumerable<string>) this._selectedAddresses);
          this.FilterTenantInfoCollection();
        });
      }
    }

    public ICommand UpdateDeviceCountCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadGridView radGridView = parameter as RadGridView;
          int num = radGridView.SelectedItems.Sum<object>((Func<object, int>) (item => ((TenantInfo) item).NoOfDevices));
          if (radGridView.SelectedItems.Contains(radGridView.SelectedItem) && this.oldNoOfSelectedDevices >= num)
            num -= ((TenantInfo) radGridView.SelectedItem).NoOfDevices;
          this.oldNoOfSelectedDevices = num;
          this.NoOfSelectedDevicesLabel = num.ToString() + " " + Resources.AssignMeters_Expert_DevicesSelected;
        }));
      }
    }

    public ICommand OnCheckChangedFloorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._selectedFloors = this.FloorList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
          this.SelectedFloorsText = !this._selectedFloors.Any<string>() ? "" : string.Join("; ", (IEnumerable<string>) this._selectedFloors);
          this.FilterTenantInfoCollection();
        });
      }
    }

    public ICommand OnCheckChangedPositionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._selectedPositions = this.PositionList.Where<CheckableComboBoxItem>((Func<CheckableComboBoxItem, bool>) (item => item.IsChecked)).Select<CheckableComboBoxItem, string>((Func<CheckableComboBoxItem, string>) (item => item.Text)).ToList<string>();
          this.SelectedPositionsText = !this._selectedPositions.Any<string>() ? "" : string.Join("; ", (IEnumerable<string>) this._selectedPositions);
          this.FilterTenantInfoCollection();
        });
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

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public string NoOfSelectedDevicesLabel
    {
      get => this._noOfSelectedDevicesLabel;
      set
      {
        this._noOfSelectedDevicesLabel = value;
        this.OnPropertyChanged(nameof (NoOfSelectedDevicesLabel));
      }
    }

    public ObservableCollection<TenantInfo> TenantsInfoCollection
    {
      get => this._tenantsInfoCollection;
      set
      {
        this._tenantsInfoCollection = value;
        this.OnPropertyChanged(nameof (TenantsInfoCollection));
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

    public List<TenantInfo> SelectedItems
    {
      get => this._selectedItems;
      set
      {
        this._selectedItems = value;
        this.OnPropertyChanged(nameof (SelectedItems));
      }
    }

    public MVVM.ViewModel.ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    private void LoadTenantsInfoCollection()
    {
      this._tenantsInfoCollection = (ObservableCollection<TenantInfo>) new RadObservableCollection<TenantInfo>();
      this._fullTenantInfos = new List<TenantInfo>();
      foreach (StructureNodeDTO subNode1 in (Collection<StructureNodeDTO>) this._structureNode.RootNode.SubNodes)
      {
        if (subNode1.NodeType.Name == "Tenant")
        {
          TenantDTO entity1 = subNode1.Entity as TenantDTO;
          TenantInfo tenantInfo = new TenantInfo()
          {
            Tenant = entity1,
            Address = (this._structureNode.RootNode.Entity is LocationDTO entity2 ? entity2.Street : (string) null) + " " + (this._structureNode.RootNode.Entity is LocationDTO entity3 ? entity3.BuildingNr : (string) null)
          };
          tenantInfo.FloorPosition = tenantInfo?.Tenant?.FloorNr + "." + tenantInfo?.Tenant?.FloorName + " / " + tenantInfo?.Tenant?.ApartmentNr + " " + tenantInfo?.Tenant?.Direction;
          int num1 = 0;
          int num2 = 0;
          int num3 = 0;
          int num4 = 0;
          int num5 = 0;
          foreach (StructureNodeDTO subNode2 in (Collection<StructureNodeDTO>) subNode1.SubNodes)
          {
            if (subNode2.NodeType.Name == "Meter")
            {
              MeterDTO currentMeter = subNode2.Entity as MeterDTO;
              if (currentMeter != null)
              {
                MinomatMeter minomatMeter = this._minomatMeterRepository.SearchFor((Expression<Func<MinomatMeter, bool>>) (item => item.Meter.Id == currentMeter.Id)).FirstOrDefault<MinomatMeter>();
                if (minomatMeter != null)
                {
                  if (minomatMeter.Status.HasValue)
                  {
                    MeterStatusEnum? status = minomatMeter.Status;
                    if (status.HasValue)
                    {
                      switch (status.GetValueOrDefault())
                      {
                        case MeterStatusEnum.Received:
                          ++num2;
                          break;
                        case MeterStatusEnum.Assigned:
                          ++num3;
                          break;
                        case MeterStatusEnum.Registered:
                          ++num4;
                          break;
                      }
                    }
                  }
                }
                else
                  ++num5;
              }
              ++num1;
            }
          }
          tenantInfo.NoOfDevices = num1;
          tenantInfo.ReceivedDevices = num2;
          tenantInfo.AssignedDevices = num3;
          tenantInfo.RegisteredDevices = num4;
          tenantInfo.RecAsRegDevices = num2.ToString() + "/" + (object) num3 + "/" + (object) num4;
          tenantInfo.OpenDevices = num5;
          tenantInfo.OpenDevicesString = num5.ToString() + "/" + (object) num1;
          if (num3 == 0)
            tenantInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
          else if (num3 > 0 && num3 < num1)
            tenantInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
          else if (num3 == num1)
            tenantInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
          this._tenantsInfoCollection.Add(tenantInfo);
          this._fullTenantInfos.Add(tenantInfo);
        }
      }
    }

    public List<CheckableComboBoxItem> AddressList { get; private set; }

    private void InitAddressList()
    {
      this.AddressList = new List<CheckableComboBoxItem>();
      foreach (string str in this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Address)).Distinct<string>().ToList<string>())
        this.AddressList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
    }

    public List<CheckableComboBoxItem> FloorList { get; private set; }

    private void InitFloorList()
    {
      this.FloorList = new List<CheckableComboBoxItem>();
      foreach (string str in this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Tenant.FloorNr + "." + item.Tenant.FloorName)).Distinct<string>().ToList<string>())
        this.FloorList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
    }

    public List<CheckableComboBoxItem> PositionList { get; private set; }

    private void InitPositionList()
    {
      this.PositionList = new List<CheckableComboBoxItem>();
      foreach (string str in this._fullTenantInfos.Select<TenantInfo, string>((Func<TenantInfo, string>) (item => item.Tenant.ApartmentNr + " " + item.Tenant.Direction)).Distinct<string>().ToList<string>())
        this.PositionList.Add(new CheckableComboBoxItem()
        {
          Text = str
        });
    }

    private void FilterTenantInfoCollection()
    {
      this._tenantsInfoCollection = new ObservableCollection<TenantInfo>();
      this.ResetNumberOfSelectedDevices();
      foreach (TenantInfo fullTenantInfo in this._fullTenantInfos)
      {
        if ((this._selectedAddresses == null || this._selectedAddresses.Count == 0 || this._selectedAddresses.Contains(fullTenantInfo.Address)) & (this._selectedFloors == null || this._selectedFloors.Count == 0 || this._selectedFloors.Contains(fullTenantInfo.Tenant?.FloorNr + "." + fullTenantInfo.Tenant?.FloorName)) & (this._selectedPositions == null || this._selectedPositions.Count == 0 || this._selectedPositions.Contains(fullTenantInfo.Tenant?.ApartmentNr + " " + fullTenantInfo.Tenant?.Direction)))
          this._tenantsInfoCollection.Add(fullTenantInfo);
      }
      this.OnPropertyChanged("TenantsInfoCollection");
    }

    private List<MeterDTO> GetMetersFromSelectedTenants()
    {
      List<MeterDTO> fromSelectedTenants = new List<MeterDTO>();
      foreach (TenantInfo selectedItem in this.SelectedItems)
      {
        foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._structureNode.RootNode.SubNodes)
        {
          if (subNode.NodeType.Name == "Tenant" && (TenantDTO) subNode.Entity != null && ((TenantDTO) subNode.Entity).Id == selectedItem.Tenant.Id)
          {
            using (IEnumerator<StructureNodeDTO> enumerator = subNode.SubNodes.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                StructureNodeDTO current = enumerator.Current;
                if (current.NodeType.Name == "Meter" && current.Entity != null)
                  fromSelectedTenants.Add(current.Entity as MeterDTO);
              }
              break;
            }
          }
        }
      }
      return fromSelectedTenants;
    }

    private bool SaveAndRegisterMinomatMeters(List<MeterDTO> meterList)
    {
      bool flag = true;
      ISession session = this._repositoryFactory.GetSession();
      try
      {
        Minomat minomat = Mapper.Map<MinomatSerializableDTO, Minomat>((MinomatSerializableDTO) this._structureNode.Entity);
        GMMMinomatConfigurator.GetInstance(minomat.IsMaster, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory")).RegisterDevicesOnMinomat(meterList, minomat);
        session.BeginTransaction();
        foreach (MeterDTO meter in meterList)
          this._minomatMeterRepository.TransactionalInsert(new MinomatMeter()
          {
            SignalStrength = 0,
            Status = new MeterStatusEnum?(MeterStatusEnum.Registered),
            Meter = Mapper.Map<MeterDTO, Meter>(meter),
            Minomat = minomat
          });
        MinomatRadioDetails radioDetails = minomat.RadioDetails;
        radioDetails.NrOfRegisteredDevices = meterList.Count.ToString();
        radioDetails.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.DevicesRegistered);
        this._repositoryFactory.GetRepository<MinomatRadioDetails>().TransactionalUpdate(radioDetails);
        session.Transaction.Commit();
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
        flag = false;
        if (session.IsOpen && session.Transaction.IsActive)
          session.Transaction.Rollback();
      }
      return flag;
    }

    private void ResetNumberOfSelectedDevices()
    {
      this.NoOfSelectedDevicesLabel = "0 " + Resources.AssignMeters_Expert_DevicesSelected;
      this.oldNoOfSelectedDevices = 0;
    }
  }
}
