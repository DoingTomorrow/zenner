// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.NetworkSetupViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MinomatHandler;
using MSS.Business.DTO;
using MSS.Business.Modules.GMM;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class NetworkSetupViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private StructureNodeDTO _selectedMaster;
    private ObservableCollection<NetworkSetupSlaveInfo> _slavesList;
    private string _title;
    private string _busyContent;
    private bool _showProgressCircle;
    private ViewModelBase _messageUserControl;
    private string _lastMinomatMasterStartedOn;
    private string _fixedNetworkSetup_GSMStatus;
    private DateTime? _fixedNetworkSetup_GSMStatusDate;
    private bool _isStartMasterEnabled;
    private DateTime? _lastStartOn;
    private DateTime? _minomatMasterLastStartOn;
    private bool _isNetworkOptimizationEnabled;

    public NetworkSetupViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      StructureNodeDTO masterNode)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._minomatRadioDetailsRepository = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
      this.Title = Resources.MSS_Client_Orders_ExecuteInstallationOrder_NetworkSetup;
      this.IsNetworkOptimizationEnabled = false;
      if (!(masterNode.NodeType.Name == "MinomatMaster"))
        return;
      this.Title = this.Title + " - " + Resources.MSS_MinomatMaster_Master;
      this._selectedMaster = masterNode;
      MinomatRadioDetails minomatRadioDetails = this._minomatRadioDetailsRepository.SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (this._selectedMaster.Entity as MinomatSerializableDTO).Id)).FirstOrDefault<MinomatRadioDetails>();
      this._lastMinomatMasterStartedOn = Resources.NetworkSetup_LastStartOn;
      if (minomatRadioDetails != null)
        this._lastMinomatMasterStartedOn = this._lastMinomatMasterStartedOn + " " + (object) minomatRadioDetails.LastStartOn;
      this.MinomatMasterLastStartOn = (DateTime?) minomatRadioDetails?.LastStartOn;
      this.FixedNetworkSetup_GSMStatus = minomatRadioDetails != null ? minomatRadioDetails.GSMStatus.ToString() : "";
      this.FixedNetworkSetup_GSMStatusDate = (DateTime?) minomatRadioDetails?.GSMStatusDate;
      int num;
      if (minomatRadioDetails != null)
      {
        MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
        MinomatStatusNetworkEnum statusNetworkEnum = MinomatStatusNetworkEnum.SetupStarted;
        num = statusNetwork.GetValueOrDefault() == statusNetworkEnum ? (statusNetwork.HasValue ? 1 : 0) : 0;
      }
      else
        num = 0;
      this.IsNetworkOptimizationEnabled = num != 0;
      this.InitSlavesList();
      this._isStartMasterEnabled = this.CanMasterBeStarted();
      this._showProgressCircle = false;
    }

    public ICommand RegisterSlavesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.FixedNetworkSetup_RegisterSlavesInProgress;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              List<Guid> slaveIds = this._slavesList.Select<NetworkSetupSlaveInfo, Guid>((Func<NetworkSetupSlaveInfo, Guid>) (item => (item.Slave.Entity as MinomatSerializableDTO).Id)).ToList<Guid>();
              List<MinomatRadioDetails> minomatRadioDetailsForStartedSlaves = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => !item.Minomat.IsDeactivated && slaveIds.Contains(item.Minomat.Id) && ((int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Open || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Registered || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Received || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.SetupStarted))).ToList<MinomatRadioDetails>();
              List<Guid> openOrRegisteredOrReceivedSlavesIds = minomatRadioDetailsForStartedSlaves.Select<MinomatRadioDetails, Guid>((Func<MinomatRadioDetails, Guid>) (item => item.Minomat.Id)).ToList<Guid>();
              Dictionary<string, MinomatSerializableDTO> openOrRegisteredOrReceivedSlaves = this._slavesList.Where<NetworkSetupSlaveInfo>((Func<NetworkSetupSlaveInfo, bool>) (item => openOrRegisteredOrReceivedSlavesIds.Contains((item.Slave.Entity as MinomatSerializableDTO).Id))).ToDictionary<NetworkSetupSlaveInfo, string, MinomatSerializableDTO>((Func<NetworkSetupSlaveInfo, string>) (s => s.NodeId), (Func<NetworkSetupSlaveInfo, MinomatSerializableDTO>) (s => (MinomatSerializableDTO) s.Slave.Entity));
              if (openOrRegisteredOrReceivedSlaves.Count > 0)
              {
                GMMMinomatConfiguratorResult canSlavesBeRegisteredOnMaster;
                Dictionary<string, GMMMinomatConfiguratorResult> dictionary = instance.RegisterSlavesOnMinomat(openOrRegisteredOrReceivedSlaves, this._selectedMaster.Entity as MinomatSerializableDTO, out canSlavesBeRegisteredOnMaster);
                if (canSlavesBeRegisteredOnMaster.IsSuccess)
                {
                  TypeHelperExtensionMethods.ForEach<KeyValuePair<string, GMMMinomatConfiguratorResult>>((IEnumerable<KeyValuePair<string, GMMMinomatConfiguratorResult>>) dictionary, (Action<KeyValuePair<string, GMMMinomatConfiguratorResult>>) (gmmConfigResult =>
                  {
                    if (!gmmConfigResult.Value.IsSuccess)
                      return;
                    MinomatSerializableDTO startedSlave = openOrRegisteredOrReceivedSlaves[gmmConfigResult.Key];
                    MinomatRadioDetails minomatRadioDetails3 = minomatRadioDetailsForStartedSlaves.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == startedSlave.Id));
                    MinomatStatusNetworkEnum? statusNetwork;
                    int num3;
                    if (minomatRadioDetails3 != null)
                    {
                      statusNetwork = minomatRadioDetails3.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum8 = MinomatStatusNetworkEnum.Open;
                      if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum8 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                      {
                        statusNetwork = minomatRadioDetails3.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum9 = MinomatStatusNetworkEnum.Registered;
                        num3 = statusNetwork.GetValueOrDefault() == statusNetworkEnum9 ? (statusNetwork.HasValue ? 1 : 0) : 0;
                      }
                      else
                        num3 = 1;
                    }
                    else
                      num3 = 0;
                    if (num3 != 0)
                    {
                      MinomatRadioDetails minomatRadioDetails4 = minomatRadioDetailsForStartedSlaves.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == startedSlave.Id));
                      minomatRadioDetails4.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Registered);
                      minomatRadioDetails4.LastRegisteredOn = new DateTime?(DateTime.Now);
                    }
                    else
                    {
                      int num4;
                      if (minomatRadioDetails3 != null)
                      {
                        statusNetwork = minomatRadioDetails3.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum10 = MinomatStatusNetworkEnum.Received;
                        if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum10 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                        {
                          statusNetwork = minomatRadioDetails3.StatusNetwork;
                          MinomatStatusNetworkEnum statusNetworkEnum11 = MinomatStatusNetworkEnum.SetupStarted;
                          num4 = statusNetwork.GetValueOrDefault() == statusNetworkEnum11 ? (statusNetwork.HasValue ? 1 : 0) : 0;
                        }
                        else
                          num4 = 1;
                      }
                      else
                        num4 = 0;
                      if (num4 != 0)
                        minomatRadioDetailsForStartedSlaves.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == startedSlave.Id)).LastRegisteredOn = new DateTime?(DateTime.Now);
                    }
                  }));
                  List<MinomatRadioDetails> list = minomatRadioDetailsForStartedSlaves.Where<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item =>
                  {
                    if (item.Minomat.IsDeactivated)
                      return false;
                    MinomatStatusNetworkEnum? statusNetwork = item.StatusNetwork;
                    MinomatStatusNetworkEnum statusNetworkEnum12 = MinomatStatusNetworkEnum.Registered;
                    if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum12 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                    {
                      statusNetwork = item.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum13 = MinomatStatusNetworkEnum.Received;
                      if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum13 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                      {
                        statusNetwork = item.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum14 = MinomatStatusNetworkEnum.SetupStarted;
                        return statusNetwork.GetValueOrDefault() == statusNetworkEnum14 && statusNetwork.HasValue;
                      }
                    }
                    return true;
                  })).ToList<MinomatRadioDetails>();
                  bool flag = list.Count == this._slavesList.Count;
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  this._minomatRadioDetailsRepository.TransactionalUpdateMany((IEnumerable<MinomatRadioDetails>) minomatRadioDetailsForStartedSlaves);
                  session.Transaction.Commit();
                  foreach (MinomatRadioDetails minomatRadioDetails in minomatRadioDetailsForStartedSlaves)
                  {
                    MinomatRadioDetails minomatRadioDetailsForSlave = minomatRadioDetails;
                    NetworkSetupSlaveInfo networkSetupSlaveInfo = this.SlavesList.FirstOrDefault<NetworkSetupSlaveInfo>((Func<NetworkSetupSlaveInfo, bool>) (item => (item.Slave.Entity as MinomatSerializableDTO).Id == minomatRadioDetailsForSlave.Minomat.Id));
                    if (networkSetupSlaveInfo != null)
                      networkSetupSlaveInfo.LastRegisteredOn = minomatRadioDetailsForSlave.LastRegisteredOn;
                  }
                  this.IsStartMasterEnabled = this.CanMasterBeStarted();
                  if (flag)
                  {
                    Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage)));
                  }
                  else
                  {
                    List<Guid> registeredSlavesIds = list.Select<MinomatRadioDetails, Guid>((Func<MinomatRadioDetails, Guid>) (item => item.Minomat.Id)).ToList<Guid>();
                    string unregisteredSlavesString = string.Join(",", (IEnumerable<string>) this._slavesList.Where<NetworkSetupSlaveInfo>((Func<NetworkSetupSlaveInfo, bool>) (item => !registeredSlavesIds.Contains((item.Slave.Entity as MinomatSerializableDTO).Id))).ToList<NetworkSetupSlaveInfo>().Select<NetworkSetupSlaveInfo, string>((Func<NetworkSetupSlaveInfo, string>) (item => !(item.Slave.Entity is MinomatSerializableDTO entity2) ? (string) null : entity2.RadioId)).ToList<string>());
                    Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, Resources.MSS_Client_SlavesNotRegistered + unregisteredSlavesString, false)));
                  }
                }
                else
                  Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, canSlavesBeRegisteredOnMaster.Message, false)));
                this.ShowProgressCircle = false;
              }
              else
                Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_ExportOrder_Warning_Title, Resources.MSS_Client_NoAvailableSlavesToRegister, false)));
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public ICommand StartGsmTestCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.MSS_Client_GSMTestInProgress;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.StartMasterGSMTestReception(this._selectedMaster.Entity as MinomatSerializableDTO);
              if (gmmConfiguratorResult.IsSuccess)
              {
                int result;
                if (int.TryParse(new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).GetAppParam("GSMTestWaitingTime").Value, out result))
                {
                  Thread.Sleep(result * 1000);
                  GSMTestReceptionState testReceptionState = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory")).ReadMasterGSMTestReception(this._selectedMaster.Entity as MinomatSerializableDTO);
                  MinomatRadioDetails entity = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (this._selectedMaster.Entity as MinomatSerializableDTO).Id));
                  entity.GSMStatus = new GSMTestReceptionState?(testReceptionState);
                  entity.GSMStatusDate = new DateTime?(DateTime.Now);
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  this._minomatRadioDetailsRepository.TransactionalUpdate(entity);
                  session.Transaction.Commit();
                  this.FixedNetworkSetup_GSMStatus = entity.GSMStatus.ToString();
                  this.FixedNetworkSetup_GSMStatusDate = entity.GSMStatusDate;
                  this.ShowProgressCircle = false;
                  string message = Resources.MSS_Client_GSMTestResult_EndedWithState + testReceptionState.ToString();
                  Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Client_GSMTestResult_Title, message, false)));
                }
                else
                  Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Client_GSMTestResult_Title, Resources.MSS_Client_GSMTestResult_NotCorrectlyConfigured, false)));
              }
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public ICommand CheckRoutingTableCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.MSS_Client_CheckingRoutingTable;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              List<string> minomatsNotFoundInRoutingTable = new List<string>();
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.GetRoutingTable(this._selectedMaster.Entity as MinomatSerializableDTO, out RoutingTable _);
              if (gmmConfiguratorResult.IsSuccess)
              {
                List<Guid> slaveIds = this.SlavesList.Select<NetworkSetupSlaveInfo, Guid>((Func<NetworkSetupSlaveInfo, Guid>) (item => (item.Slave.Entity as MinomatSerializableDTO).Id)).ToList<Guid>();
                List<MinomatRadioDetails> minomatRadioDetailsForSlaves = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => slaveIds.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
                Dictionary<Guid, DateTime?> minomatRadioDetailsIds_FoundInRoutingTable = new Dictionary<Guid, DateTime?>();
                Dictionary<Guid, DateTime?> minomatRadioDetailsIds_NotInRoutingTable = new Dictionary<Guid, DateTime?>();
                this.SlavesList.ToList<NetworkSetupSlaveInfo>().ForEach((Action<NetworkSetupSlaveInfo>) (slave =>
                {
                  RoutingRow routingRow = routingTable.FirstOrDefault<RoutingRow>((Func<RoutingRow, bool>) (x => x.NodeId.ToString() == slave.NodeId));
                  MinomatRadioDetails minomatRadioDetails = minomatRadioDetailsForSlaves.FirstOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == (slave.Slave.Entity as MinomatSerializableDTO).Id));
                  MinomatStatusNetworkEnum? statusNetwork;
                  int num3;
                  if (routingRow != null && minomatRadioDetails != null)
                  {
                    statusNetwork = minomatRadioDetails.StatusNetwork;
                    MinomatStatusNetworkEnum statusNetworkEnum6 = MinomatStatusNetworkEnum.Registered;
                    if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum6 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                    {
                      statusNetwork = minomatRadioDetails.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum7 = MinomatStatusNetworkEnum.Received;
                      if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum7 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                      {
                        statusNetwork = minomatRadioDetails.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum8 = MinomatStatusNetworkEnum.SetupStarted;
                        num3 = statusNetwork.GetValueOrDefault() == statusNetworkEnum8 ? (statusNetwork.HasValue ? 1 : 0) : 0;
                        goto label_6;
                      }
                    }
                    num3 = 1;
                  }
                  else
                    num3 = 0;
label_6:
                  if (num3 != 0)
                  {
                    slave.SignalStrength = routingRow.RSSI_dBm.ToString() + " dBm";
                    slave.HasErrors = new bool?(false);
                    slave.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
                    if (!(slave.Slave.Entity is MinomatSerializableDTO entity2) || !(entity2.Id != Guid.Empty))
                      return;
                    minomatRadioDetailsIds_FoundInRoutingTable.Add((slave.Slave.Entity as MinomatSerializableDTO).Id, new DateTime?(DateTime.Now));
                  }
                  else
                  {
                    slave.SignalStrength = "";
                    slave.HasErrors = new bool?(true);
                    int num4;
                    if (minomatRadioDetails != null)
                    {
                      statusNetwork = minomatRadioDetails.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum9 = MinomatStatusNetworkEnum.Open;
                      if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum9 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                      {
                        statusNetwork = minomatRadioDetails.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum10 = MinomatStatusNetworkEnum.Registered;
                        num4 = statusNetwork.GetValueOrDefault() == statusNetworkEnum10 ? (statusNetwork.HasValue ? 1 : 0) : 0;
                      }
                      else
                        num4 = 1;
                    }
                    else
                      num4 = 0;
                    if (num4 != 0)
                    {
                      slave.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
                      minomatRadioDetailsIds_NotInRoutingTable.Add((slave.Slave.Entity as MinomatSerializableDTO).Id, new DateTime?(DateTime.Now));
                    }
                  }
                }));
                if (minomatRadioDetailsIds_FoundInRoutingTable.Any<KeyValuePair<Guid, DateTime?>>())
                {
                  List<Guid> minomatKeysToGet_Found = minomatRadioDetailsIds_FoundInRoutingTable.Keys.ToList<Guid>();
                  List<Guid> minomatKeysToGet_NotFound = minomatRadioDetailsIds_NotInRoutingTable.Keys.ToList<Guid>();
                  List<MinomatRadioDetails> list3 = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => minomatKeysToGet_Found.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
                  List<MinomatRadioDetails> list4 = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => minomatKeysToGet_NotFound.Contains(item.Minomat.Id))).ToList<MinomatRadioDetails>();
                  foreach (MinomatRadioDetails minomatRadioDetails in list3)
                  {
                    MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails.StatusNetwork;
                    MinomatStatusNetworkEnum statusNetworkEnum = MinomatStatusNetworkEnum.Registered;
                    if (statusNetwork.GetValueOrDefault() == statusNetworkEnum && statusNetwork.HasValue)
                      minomatRadioDetails.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Received);
                  }
                  foreach (MinomatRadioDetails minomatRadioDetails3 in list4)
                  {
                    MinomatStatusNetworkEnum? statusNetwork = minomatRadioDetails3.StatusNetwork;
                    MinomatStatusNetworkEnum statusNetworkEnum13 = MinomatStatusNetworkEnum.Open;
                    if (statusNetwork.GetValueOrDefault() != statusNetworkEnum13 || !statusNetwork.HasValue)
                    {
                      MinomatRadioDetails minomatRadioDetails4 = minomatRadioDetails3;
                      statusNetwork = minomatRadioDetails3.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum14 = MinomatStatusNetworkEnum.Received;
                      MinomatStatusNetworkEnum? nullable = (statusNetwork.GetValueOrDefault() == statusNetworkEnum14 ? (statusNetwork.HasValue ? 1 : 0) : 0) != 0 ? new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Registered) : minomatRadioDetails3.StatusNetwork;
                      minomatRadioDetails4.StatusNetwork = nullable;
                    }
                  }
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  this._minomatRadioDetailsRepository.TransactionalUpdateMany((IEnumerable<MinomatRadioDetails>) list3);
                  this._minomatRadioDetailsRepository.TransactionalUpdateMany((IEnumerable<MinomatRadioDetails>) list4);
                  session.Transaction.Commit();
                  if (list4 != null && list4.Any<MinomatRadioDetails>())
                    minomatsNotFoundInRoutingTable = list4.Select<MinomatRadioDetails, string>((Func<MinomatRadioDetails, string>) (item => item.Minomat.RadioId)).ToList<string>();
                }
                int num = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => slaveIds.Contains(item.Minomat.Id) && ((int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Registered || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Received || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.SetupStarted))).Count<MinomatRadioDetails>();
                this.IsStartMasterEnabled = slaveIds.Count == 0 || slaveIds.Count == num;
                Application.Current.Dispatcher.Invoke((Action) (() =>
                {
                  if (minomatsNotFoundInRoutingTable.Any<string>())
                    MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, Resources.MSS_Client_SlavesNotFoundInRoutingTable + string.Join(", ", (IEnumerable<string>) minomatsNotFoundInRoutingTable), false);
                  else
                    this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
                }));
              }
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              this.ShowProgressCircle = false;
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public ICommand MinomatMasterStartCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.FixedNetworkSetup_StartingMinomatMaster;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.StartMinomatMaster(this._selectedMaster.Entity as MinomatSerializableDTO);
              if (gmmConfiguratorResult.IsSuccess)
                Application.Current.Dispatcher.Invoke((Action) (() =>
                {
                  this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
                  DateTime now = DateTime.Now;
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  try
                  {
                    MinomatRadioDetails entity = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (this._selectedMaster.Entity as MinomatSerializableDTO).Id));
                    MinomatStatusNetworkEnum? statusNetwork;
                    int num3;
                    if (entity != null)
                    {
                      statusNetwork = entity.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum3 = MinomatStatusNetworkEnum.Open;
                      if ((statusNetwork.GetValueOrDefault() == statusNetworkEnum3 ? (statusNetwork.HasValue ? 1 : 0) : 0) == 0)
                      {
                        statusNetwork = entity.StatusNetwork;
                        MinomatStatusNetworkEnum statusNetworkEnum4 = MinomatStatusNetworkEnum.Received;
                        num3 = statusNetwork.GetValueOrDefault() == statusNetworkEnum4 ? (statusNetwork.HasValue ? 1 : 0) : 0;
                      }
                      else
                        num3 = 1;
                    }
                    else
                      num3 = 0;
                    if (num3 != 0)
                    {
                      entity.LastStartOn = new DateTime?(now);
                      entity.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.SetupStarted);
                      this._minomatRadioDetailsRepository.TransactionalUpdate(entity);
                    }
                    else if (this._selectedMaster.Entity is MinomatSerializableDTO)
                      this._minomatRadioDetailsRepository.TransactionalInsert(new MinomatRadioDetails()
                      {
                        Minomat = {
                          Id = (this._selectedMaster.Entity as MinomatSerializableDTO).Id
                        },
                        LastStartOn = new DateTime?(now)
                      });
                    session.Transaction.Commit();
                    this.LastMinomatMasterStartedOn = Resources.NetworkSetup_LastStartOn + " " + (object) now;
                    this.MinomatMasterLastStartOn = new DateTime?(now);
                    int num4;
                    if (entity != null)
                    {
                      statusNetwork = entity.StatusNetwork;
                      MinomatStatusNetworkEnum statusNetworkEnum = MinomatStatusNetworkEnum.SetupStarted;
                      num4 = statusNetwork.GetValueOrDefault() == statusNetworkEnum ? (statusNetwork.HasValue ? 1 : 0) : 0;
                    }
                    else
                      num4 = 0;
                    this.IsNetworkOptimizationEnabled = num4 != 0;
                  }
                  catch (Exception ex)
                  {
                    session.Transaction.Rollback();
                    Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
                  }
                }));
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              this.ShowProgressCircle = false;
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public ICommand MinomatSlaveStartCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          NetworkSetupSlaveInfo selectedSlave = parameter as NetworkSetupSlaveInfo;
          MinomatRadioDetails currentMinomatRadioDetails = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (selectedSlave.Slave.Entity as MinomatSerializableDTO).Id));
          MinomatStatusNetworkEnum? statusNetwork = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == (this._selectedMaster.Entity as MinomatSerializableDTO).Id)).StatusNetwork;
          MinomatStatusNetworkEnum statusNetworkEnum = MinomatStatusNetworkEnum.SetupStarted;
          if (statusNetwork.GetValueOrDefault() == statusNetworkEnum && statusNetwork.HasValue)
          {
            this.BusyContent = Resources.FixedNetworkSetup_StartingMinomatSlave;
            this.ShowProgressCircle = true;
            ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
            {
              try
              {
                GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
                instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
                GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.StartMinomatSlave(selectedSlave.Slave.Entity as MinomatSerializableDTO);
                if (gmmConfiguratorResult.IsSuccess)
                  Application.Current.Dispatcher.Invoke((Action) (() =>
                  {
                    this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
                    DateTime now = DateTime.Now;
                    ISession session = this._repositoryFactory.GetSession();
                    session.BeginTransaction();
                    try
                    {
                      if (currentMinomatRadioDetails != null)
                      {
                        currentMinomatRadioDetails.LastStartOn = new DateTime?(now);
                        currentMinomatRadioDetails.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.SetupStarted);
                        this._minomatRadioDetailsRepository.TransactionalUpdate(currentMinomatRadioDetails);
                      }
                      else
                        this._minomatRadioDetailsRepository.TransactionalInsert(new MinomatRadioDetails()
                        {
                          Minomat = Mapper.Map<MinomatSerializableDTO, Minomat>(selectedSlave.Slave.Entity as MinomatSerializableDTO),
                          LastStartOn = new DateTime?(now)
                        });
                      session.Transaction.Commit();
                      selectedSlave.LastStartOn = new DateTime?(now);
                    }
                    catch (Exception ex)
                    {
                      session.Transaction.Rollback();
                      Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
                    }
                  }));
                else
                  Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
                this.ShowProgressCircle = false;
              }
              catch (Exception ex)
              {
                Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
              }
              finally
              {
                this.ShowProgressCircle = false;
              }
            }));
          }
          else
            Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, Resources.MSS_Client_MasterMustBeStarted, false)));
        }));
      }
    }

    public ICommand NetworkOptimizationCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.BusyContent = Resources.MSS_Client_NetworkOptimizationInProgress;
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            try
            {
              GMMMinomatConfigurator instance = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              instance.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              GMMMinomatConfiguratorResult gmmConfiguratorResult = instance.NetworkOptimization(this._selectedMaster.Entity as MinomatSerializableDTO);
              if (gmmConfiguratorResult.IsSuccess)
              {
                Application.Current.Dispatcher.Invoke((Action) (() =>
                {
                  this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
                  this.LastMinomatMasterStartedOn = Resources.NetworkSetup_LastStartOn + " " + (object) DateTime.Now;
                }));
                ISession session = this._repositoryFactory.GetSession();
                session.BeginTransaction();
                IRepository<Minomat> repository = this._repositoryFactory.GetRepository<Minomat>();
                Minomat minomatToUpdate = repository.FirstOrDefault((Expression<Func<Minomat, bool>>) (item => !item.IsDeactivated && item.Id == (this._selectedMaster.Entity as MinomatSerializableDTO).Id));
                MinomatRadioDetails entity = this._minomatRadioDetailsRepository.FirstOrDefault((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == minomatToUpdate.Id));
                int num;
                if (minomatToUpdate != null && entity != null)
                {
                  MinomatStatusNetworkEnum? statusNetwork = entity.StatusNetwork;
                  MinomatStatusNetworkEnum statusNetworkEnum = MinomatStatusNetworkEnum.SetupStarted;
                  num = statusNetwork.GetValueOrDefault() == statusNetworkEnum ? (statusNetwork.HasValue ? 1 : 0) : 0;
                }
                else
                  num = 0;
                if (num != 0)
                {
                  minomatToUpdate.Status = StatusMinomatEnum.NetworkOptimizationStarted.ToString();
                  repository.TransactionalUpdate(minomatToUpdate);
                  entity.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.NetworkOptimization);
                  this._minomatRadioDetailsRepository.TransactionalUpdate(entity);
                }
                session.Transaction.Commit();
                Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
              }
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              this.ShowProgressCircle = false;
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
            }
            finally
            {
              this.ShowProgressCircle = false;
            }
          }));
        }));
      }
    }

    public StructureNodeDTO SelectedMaster => this._selectedMaster;

    public ObservableCollection<NetworkSetupSlaveInfo> SlavesList => this._slavesList;

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
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

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public string LastMinomatMasterStartedOn
    {
      get => this._lastMinomatMasterStartedOn;
      set
      {
        this._lastMinomatMasterStartedOn = value;
        this.OnPropertyChanged(nameof (LastMinomatMasterStartedOn));
      }
    }

    public string FixedNetworkSetup_GSMStatus
    {
      get => this._fixedNetworkSetup_GSMStatus;
      set
      {
        this._fixedNetworkSetup_GSMStatus = value;
        this.OnPropertyChanged(nameof (FixedNetworkSetup_GSMStatus));
      }
    }

    public DateTime? FixedNetworkSetup_GSMStatusDate
    {
      get => this._fixedNetworkSetup_GSMStatusDate;
      set
      {
        this._fixedNetworkSetup_GSMStatusDate = value;
        this.OnPropertyChanged(nameof (FixedNetworkSetup_GSMStatusDate));
      }
    }

    public bool IsStartMasterEnabled
    {
      get => this._isStartMasterEnabled;
      set
      {
        this._isStartMasterEnabled = value;
        this.OnPropertyChanged(nameof (IsStartMasterEnabled));
      }
    }

    public DateTime? LastStartOn
    {
      get => this._lastStartOn;
      set
      {
        this._lastStartOn = value;
        this.OnPropertyChanged(nameof (LastStartOn));
      }
    }

    public DateTime? MinomatMasterLastStartOn
    {
      get => this._minomatMasterLastStartOn;
      set
      {
        this._minomatMasterLastStartOn = value;
        this.OnPropertyChanged(nameof (MinomatMasterLastStartOn));
      }
    }

    public bool IsNetworkOptimizationEnabled
    {
      get => this._isNetworkOptimizationEnabled;
      set
      {
        this._isNetworkOptimizationEnabled = value;
        this.OnPropertyChanged(nameof (IsNetworkOptimizationEnabled));
      }
    }

    private void InitSlavesList()
    {
      this._slavesList = new ObservableCollection<NetworkSetupSlaveInfo>();
      List<Guid?> slaveIds = this._selectedMaster.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "MinomatSlave")).Select<StructureNodeDTO, Guid?>((Func<StructureNodeDTO, Guid?>) (item => !(item.Entity is MinomatSerializableDTO entity) ? new Guid?() : new Guid?(entity.Id))).ToList<Guid?>();
      List<MinomatRadioDetails> list = this._repositoryFactory.GetRepository<MinomatRadioDetails>().Where((Expression<Func<MinomatRadioDetails, bool>>) (item => slaveIds.Contains((Guid?) item.Minomat.Id))).ToList<MinomatRadioDetails>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._selectedMaster.SubNodes)
      {
        StructureNodeDTO subnode = subNode;
        if (subnode.NodeType.Name == "MinomatSlave")
        {
          NetworkSetupSlaveInfo networkSetupSlaveInfo = new NetworkSetupSlaveInfo()
          {
            Slave = subnode,
            HasErrors = new bool?()
          };
          MinomatRadioDetails minomatRadioDetails = list.LastOrDefault<MinomatRadioDetails>((Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == ((MinomatSerializableDTO) subnode.Entity).Id));
          if (minomatRadioDetails != null)
          {
            networkSetupSlaveInfo.NodeId = minomatRadioDetails.NodeId;
            networkSetupSlaveInfo.LastStartOn = minomatRadioDetails.LastStartOn;
            networkSetupSlaveInfo.LastRegisteredOn = minomatRadioDetails.LastRegisteredOn;
          }
          networkSetupSlaveInfo.ImageLocation = "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
          this._slavesList.Add(networkSetupSlaveInfo);
        }
      }
    }

    private bool CanMasterBeStarted()
    {
      List<Guid?> slavesIds = this._slavesList.Select<NetworkSetupSlaveInfo, Guid?>((Func<NetworkSetupSlaveInfo, Guid?>) (item => !(item.Slave.Entity is MinomatSerializableDTO entity) ? new Guid?() : new Guid?(entity.Id))).ToList<Guid?>();
      int num = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => slavesIds.Contains((Guid?) item.Minomat.Id) && ((int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Registered || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.Received || (int?) item.StatusNetwork == (int?) MinomatStatusNetworkEnum.SetupStarted))).Count<MinomatRadioDetails>();
      return slavesIds.Count == 0 || slavesIds.Count == num;
    }
  }
}
