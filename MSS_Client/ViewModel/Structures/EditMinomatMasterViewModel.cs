// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditMinomatMasterViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.MessageHandler;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Data;
using ZENNER;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  internal class EditMinomatMasterViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly StructureNodeDTO _structureNode;
    private readonly IWindowFactory _windowFactory;
    private readonly string _orderNumber;
    private readonly IRepository<Country> _countryRepository;
    private readonly IRepository<Minomat> _minomatRepository;
    private readonly IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private Minomat _selectedMinomat;
    private MinomatRadioDetails _selectedMinomatRadioDetails;
    private readonly bool _useMasterpool;
    private string _title;
    private bool _isBusy;
    private string _radioId;
    private int _netId;
    private int _nodeId;
    private string _entrance;
    private static List<string> _entrancesList;
    private string _floorNameValue = string.Empty;
    private string _floorNrValue = string.Empty;
    private string _apartmentNrValue = string.Empty;
    private int? _selectedDirectionId;
    private int? _selectedFloorNameId;
    private string _description;
    private int _channel;
    private Provider _provider;
    private string _simPin;
    private string _accessPoint;
    private string _userId;
    private string _userPassword;
    private int? _polling;
    private string _hostandPort;
    private string _url;
    private ViewModelBase _messageUserControl;
    private string _simCardNumber;

    public EditMinomatMasterViewModel(
      IRepositoryFactory repositoryFactory,
      StructureNodeDTO node,
      IWindowFactory windowFactory,
      string orderNumber)
    {
      this._structureNode = node;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._orderNumber = orderNumber;
      this._countryRepository = repositoryFactory.GetRepository<Country>();
      this._minomatRepository = this._repositoryFactory.GetRepository<Minomat>();
      this._minomatRadioDetailsRepository = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
      ApplicationParameter applicationParameter = MSS.Business.Utils.AppContext.Current.Parameters.FirstOrDefault<ApplicationParameter>((Func<ApplicationParameter, bool>) (p => p.Parameter == "MinomatUseMasterpool"));
      this._useMasterpool = applicationParameter != null && bool.Parse(applicationParameter.Value);
      Task.Run((Action) (() =>
      {
        this.IsBusy = true;
        this.InitializeView();
        this.IsBusy = false;
      })).ContinueWith((Action<Task>) (t =>
      {
        if (t.IsFaulted && t.Exception != null)
          throw t.Exception;
      }));
    }

    private void InitializeView()
    {
      this.EntrancesList = new StructureEntrancesManager().GetStructureEntrances(this._structureNode, this._repositoryFactory);
      MinomatSerializableDTO selectedMinomatDto = this._structureNode.Entity as MinomatSerializableDTO;
      if (selectedMinomatDto != null)
      {
        this.Title = Resources.MSS_EditMinomatMaster;
        this._selectedMinomat = Mapper.Map<MinomatSerializableDTO, Minomat>(selectedMinomatDto);
        this._selectedMinomatRadioDetails = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (x => x.Minomat.Id == selectedMinomatDto.Id)).FirstOrDefault<MinomatRadioDetails>();
        if (!string.IsNullOrEmpty(this._selectedMinomatRadioDetails?.Location))
        {
          string[] positionInfo = this._selectedMinomatRadioDetails.Location.Split('/');
          this.FloorNrValue = positionInfo[0];
          this.FloorNameValue = positionInfo[1];
          if (this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (fl => fl.FloorNameEnum.ToString() == positionInfo[1])) != null)
          {
            FloorNameDTO floorNameDto = this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (fl => fl.FloorNameEnum.ToString() == positionInfo[1]));
            if (floorNameDto != null)
              this.SelectedFloorNameId = new int?(floorNameDto.Id);
          }
          this.ApartmentNrValue = positionInfo[2];
          DirectionDTO directionDto = this.GetDirections.FirstOrDefault<DirectionDTO>((Func<DirectionDTO, bool>) (d => d.DirectionEnum.ToString() == positionInfo[3]));
          if (directionDto != null)
            this.SelectedDirectionId = new int?(directionDto.Id);
          this.Entrance = this.EntrancesList.FirstOrDefault<string>((Func<string, bool>) (item => item == this._selectedMinomatRadioDetails.Entrance));
          this.Description = this._selectedMinomatRadioDetails?.Description;
          int parsedValue;
          bool result = int.TryParse(this._selectedMinomatRadioDetails?.NetId, out parsedValue);
          if (result)
            this.NetId = parsedValue;
          result = int.TryParse(this._selectedMinomatRadioDetails?.NodeId, out parsedValue);
          this.NodeId = result ? parsedValue : 1;
          result = int.TryParse(this._selectedMinomatRadioDetails?.Channel, out parsedValue);
          this.Channel = this.GetListOfChannels.FirstOrDefault<int>((Func<int, bool>) (item => !result ? item == 0 : item == parsedValue));
        }
        this._radioId = this._selectedMinomat.RadioId;
        this.SelectedProvider = this.GetListofProviders.FirstOrDefault<Provider>((Func<Provider, bool>) (item =>
        {
          Guid id1 = item.Id;
          Guid? id2 = this._selectedMinomat?.Provider?.Id;
          return id2.HasValue && id1 == id2.GetValueOrDefault();
        }));
        this.SimCardNumber = this._selectedMinomat.SimCardNumber;
        this.Polling = new int?(this._selectedMinomat.Polling);
        this.HostAndPort = this._selectedMinomat.HostAndPort;
        this.Url = this._selectedMinomat.Url;
        Country country = this._selectedMinomat.Country;
        this.SelectedCountryId = country != null ? country.Id : Guid.Empty;
      }
      else
      {
        this.Title = Resources.MSS_AddMinomatMaster;
        this._selectedMinomatRadioDetails = new MinomatRadioDetails();
        this._selectedMinomat = new Minomat();
        this._selectedMinomat.IsMaster = true;
        IList<Provider> all = this._repositoryFactory.GetRepository<Provider>().GetAll();
        this._selectedMinomat.Provider = all.Count == 1 ? all.First<Provider>() : (Provider) null;
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
        this.Url = parametersManagement.GetAppParam("Url").Value;
        this.HostAndPort = parametersManagement.GetAppParam("HostAndPort").Value;
        this.Polling = new int?(Convert.ToInt32(parametersManagement.GetAppParam("Polling").Value));
        this.NetId = this.GenerateNetId();
        this.NodeId = 1;
        Country country = MSS.Business.Utils.AppContext.Current.LoggedUser.Country;
        this.SelectedCountryId = country != null ? country.Id : Guid.Empty;
      }
    }

    public System.Windows.Input.ICommand EditMinomatMasterCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) delegate
        {
          try
          {
            if (this.IsValid)
            {
              Minomat minomat1;
              if (this.IsNewMinomat())
              {
                minomat1 = this._minomatRepository.SearchWithFetch<Scenario>((Expression<Func<Minomat, bool>>) (item => item.RadioId == this.RadioId && item.Status == StatusMinomatEnum.New.ToString() && !item.IsDeactivated), (Expression<Func<Minomat, Scenario>>) (m => m.Scenario)).FirstOrDefault<Minomat>();
              }
              else
              {
                minomat1 = this._minomatRepository.SearchWithFetch<Scenario>((Expression<Func<Minomat, bool>>) (m => m.Id == ((MinomatSerializableDTO) this._structureNode.Entity).Id), (Expression<Func<Minomat, Scenario>>) (m => m.Scenario)).FirstOrDefault<Minomat>();
                if (minomat1 != null)
                  minomat1.RadioDetails = this._selectedMinomatRadioDetails;
              }
              if (minomat1 != null)
              {
                this._selectedMinomat = minomat1;
                this.CopyNewMinomatSettings(false);
              }
              else
                this.CopyNewMinomatSettings(true);
              if (this._useMasterpool)
              {
                if (minomat1 == null && this.IsNewMinomat())
                {
                  this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_MinomatMaster_MinomatNotFound);
                  return;
                }
              }
              else
              {
                MinomatSerializableDTO minomat2 = Mapper.Map<Minomat, MinomatSerializableDTO>(this._selectedMinomat);
                using (ServiceClient serviceClient = new ServiceClient(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>("ServerIp")))
                {
                  if (!serviceClient.SaveMinomatOnServer(minomat2))
                  {
                    this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_MinomatMaster_MinomatSaveOnServer);
                    return;
                  }
                }
              }
              GMMMinomatConfigurator gmmConfigurator = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
              gmmConfigurator.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
              this.IsBusy = true;
              Task.Run<GMMMinomatConfiguratorResult>((Func<GMMMinomatConfiguratorResult>) (() => gmmConfigurator.SetupMinomat(this._selectedMinomat, this._orderNumber))).ContinueWith((Action<Task<GMMMinomatConfiguratorResult>>) (p =>
              {
                GMMMinomatConfiguratorResult gmmConfiguratorResult = p.Result;
                if (gmmConfiguratorResult.IsSuccess)
                {
                  this._selectedMinomat.Status = StatusMinomatEnum.BuiltIn.ToString();
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  try
                  {
                    this._minomatRepository.TransactionalUpdate(this._selectedMinomat);
                    this._minomatRadioDetailsRepository.TransactionalInsertOrUpdate(this._selectedMinomatRadioDetails, (Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == this._selectedMinomat.Id), (Func<MinomatRadioDetails, bool>) (item => item.Minomat.Id == this._selectedMinomat.Id));
                    session.Transaction.Commit();
                  }
                  catch (Exception ex)
                  {
                    session.Transaction.Rollback();
                    Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
                  }
                  if (this._structureNode.Entity == null)
                    this._structureNode.Entity = (object) new MinomatSerializableDTO();
                  Mapper.CreateMap<Minomat, MinomatSerializableDTO>();
                  Mapper.Map<Minomat, object>(this._selectedMinomat, this._structureNode.Entity);
                  EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
                  {
                    Minomat = this._selectedMinomat,
                    Node = this._structureNode,
                    Guid = this._structureNode.RootNode != this._structureNode ? this._structureNode.RootNode.Id : this._structureNode.Id,
                    Message = new MSS.DTO.Message.Message()
                    {
                      MessageType = MessageTypeEnum.Success,
                      MessageText = MessageCodes.Success_Save.GetStringValue()
                    },
                    Name = this.RadioId,
                    Description = this._structureNode.Description
                  }, (IViewModel) this);
                  Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
                }
                else
                {
                  if (this.IsNewMinomat())
                  {
                    this._selectedMinomat = new Minomat();
                    this._selectedMinomatRadioDetails = (MinomatRadioDetails) null;
                  }
                  Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
                }
                this.IsBusy = false;
              }));
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_ValidationError);
          }
          catch (Exception ex)
          {
            Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
          }
        });
      }
    }

    public RadObservableCollection<Provider> GetListofProviders
    {
      get
      {
        RadObservableCollection<Provider> getListofProviders = new RadObservableCollection<Provider>();
        IOrderedEnumerable<Provider> orderedEnumerable = this._repositoryFactory.GetRepository<Provider>().GetAll().OrderBy<Provider, string>((Func<Provider, string>) (p => p.ProviderName));
        if (orderedEnumerable.Any<Provider>())
          getListofProviders = new RadObservableCollection<Provider>((IEnumerable<Provider>) orderedEnumerable);
        return getListofProviders;
      }
    }

    public RadObservableCollection<int> GetListOfChannels
    {
      get
      {
        RadObservableCollection<int> getListOfChannels = new RadObservableCollection<int>();
        getListOfChannels.Add(0);
        getListOfChannels.Add(1);
        return getListOfChannels;
      }
    }

    public IEnumerable<FloorNameDTO> GetFloorNames => FloorHelper.GetFloorNames();

    public ObservableCollection<DirectionDTO> GetDirections
    {
      get => new ObservableCollection<DirectionDTO>(FloorHelper.GetDirections());
    }

    private void CopyNewMinomatSettings(bool isNew)
    {
      if (this._selectedMinomatRadioDetails == null)
        this._selectedMinomatRadioDetails = new MinomatRadioDetails();
      if (this._selectedMinomatRadioDetails.Minomat == null)
      {
        this._selectedMinomatRadioDetails.Minomat = this._selectedMinomat;
        this._selectedMinomat.RadioDetails = this._selectedMinomatRadioDetails;
      }
      this._selectedMinomatRadioDetails.DueDate = (DateTime?) ((LocationDTO) this._structureNode.RootNode?.Entity)?.DueDate;
      this._selectedMinomat.RadioId = this.RadioId;
      MinomatRadioDetails minomatRadioDetails1 = this._selectedMinomatRadioDetails;
      object[] objArray = new object[7];
      objArray[0] = (object) this.FloorNrValue;
      objArray[1] = (object) "/";
      int? nullable1 = this.SelectedFloorNameId;
      int num1 = 1;
      objArray[2] = (object) (FloorNamesEnum) (nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() - num1) : new int?()).Value;
      objArray[3] = (object) "/";
      objArray[4] = (object) this.ApartmentNrValue;
      objArray[5] = (object) "/";
      int? selectedDirectionId = this.SelectedDirectionId;
      int num2 = 1;
      int? nullable2;
      if (!selectedDirectionId.HasValue)
      {
        nullable1 = new int?();
        nullable2 = nullable1;
      }
      else
        nullable2 = new int?(selectedDirectionId.GetValueOrDefault() - num2);
      nullable1 = nullable2;
      objArray[6] = (object) (DirectionsEnum) nullable1.Value;
      string str1 = string.Concat(objArray);
      minomatRadioDetails1.Location = str1;
      this._selectedMinomatRadioDetails.Entrance = this.Entrance;
      this._selectedMinomatRadioDetails.Description = this.Description;
      this._selectedMinomatRadioDetails.NetId = this.NetId.ToString();
      MinomatRadioDetails minomatRadioDetails2 = this._selectedMinomatRadioDetails;
      int num3 = this.NodeId;
      string str2 = num3.ToString();
      minomatRadioDetails2.NodeId = str2;
      MinomatRadioDetails minomatRadioDetails3 = this._selectedMinomatRadioDetails;
      num3 = this.Channel;
      string str3 = num3.ToString();
      minomatRadioDetails3.Channel = str3;
      this._selectedMinomat.Provider = this.SelectedProvider;
      this._selectedMinomat.SimPin = this.SimPin;
      this._selectedMinomat.AccessPoint = this.AccessPoint;
      this._selectedMinomat.UserId = this.UserId;
      this._selectedMinomat.UserPassword = this.UserPassword;
      this._selectedMinomat.SimCardNumber = this.SimCardNumber ?? "";
      Minomat selectedMinomat = this._selectedMinomat;
      nullable1 = this.Polling;
      int num4;
      if (!nullable1.HasValue)
      {
        num4 = 0;
      }
      else
      {
        nullable1 = this.Polling;
        num4 = nullable1.Value;
      }
      selectedMinomat.Polling = num4;
      this._selectedMinomat.HostAndPort = this.HostAndPort;
      this._selectedMinomat.Url = this.Url;
      this._selectedMinomat.Country = this.CountryCollection.FirstOrDefault<Country>((Func<Country, bool>) (item => item.Id == this.SelectedCountryId));
      if (!this._selectedMinomatRadioDetails.StatusDevices.HasValue)
        this._selectedMinomatRadioDetails.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.Open);
      if (!this._selectedMinomatRadioDetails.StatusNetwork.HasValue)
        this._selectedMinomatRadioDetails.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Open);
      if (isNew)
      {
        this._selectedMinomat.Challenge = MeterListenerManager.CreateRandomChallengeKey().ToString();
        this._selectedMinomat.GsmId = this.RadioId;
        this._selectedMinomat.SessionKey = MeterListenerManager.CreateRandomSessionKey().ToString();
        this._selectedMinomat.CreatedOn = new DateTime?(DateTime.Now);
        this._selectedMinomat.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
        this._selectedMinomat.Status = StatusMinomatEnum.New.ToString();
      }
      this._selectedMinomat.LastUpdatedBy = "";
      Guid? id = ((LocationDTO) this._structureNode.RootNode?.Entity)?.Scenario?.Id;
      if (!id.HasValue)
        return;
      this._selectedMinomat.Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) id);
    }

    private int GenerateNetId()
    {
      List<int> intList = new List<int>();
      if (this._structureNode?.ParentNode?.NodeType?.Name == "Location")
      {
        foreach (StructureNodeDTO structureNodeDto in this._structureNode.ParentNode.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item =>
        {
          if (!(item.NodeType.Name == "MinomatMaster") || item.Entity == null)
            return false;
          Guid? id = ((MinomatSerializableDTO) item.Entity)?.Id;
          Guid empty = Guid.Empty;
          if (!id.HasValue)
            return true;
          return id.HasValue && id.GetValueOrDefault() != empty;
        })).ToList<StructureNodeDTO>())
        {
          StructureNodeDTO currentMaster = structureNodeDto;
          intList.Add(this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => item.Minomat.Id == currentMaster.Id)).Select<MinomatRadioDetails, int>((Expression<Func<MinomatRadioDetails, int>>) (item => int.Parse(item.NetId))).FirstOrDefault<int>());
        }
      }
      Random random = new Random();
      int netId = random.Next(0, 250);
      if (intList.Count > 0)
      {
        while (intList.Contains(netId))
          netId = random.Next(0, 250);
      }
      return netId;
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      this.IsValid = true;
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.RadioId));
      string propertyName2 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.SimCardNumber));
      if (propertyName == propertyName1)
      {
        ICollection<string> validationErrors;
        this.ValidateMasterRadioId(this.RadioId, out validationErrors);
        if (validationErrors.Count > 0)
          this.IsValid = false;
        return validationErrors.ToList<string>();
      }
      if (!(propertyName == propertyName2) || string.IsNullOrEmpty(this._simCardNumber))
        return new List<string>();
      ICollection<string> validationErrors1;
      this.ValidateSimCardNumber(this._simCardNumber, out validationErrors1);
      if (validationErrors1.Count > 0)
        this.IsValid = false;
      return validationErrors1.ToList<string>();
    }

    public bool ValidateMasterRadioId(
      string masterRadioId,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      List<Minomat> list = this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.RadioId == masterRadioId && !x.IsDeactivated)).ToList<Minomat>();
      if (this._useMasterpool && list.Any<Minomat>((Func<Minomat, bool>) (item => item.Status != StatusMinomatEnum.New.ToString())) && this._selectedMinomat.Id == Guid.Empty)
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      else if (list.Any<Minomat>())
      {
        foreach (Minomat minomat in list)
        {
          if ((!(minomat.RadioId == this._selectedMinomat.RadioId) || !(minomat.Id == this._selectedMinomat.Id)) && this._selectedMinomat.Id != Guid.Empty)
          {
            validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
            break;
          }
        }
      }
      return validationErrors.Count == 0;
    }

    public bool ValidateSimCardNumber(
      string simCardNumber,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (!string.IsNullOrEmpty(simCardNumber) && simCardNumber.Length != 30)
        validationErrors.Add(Resources.MSS_MinomatMaster_SimCardNumber_TooFewDigits);
      return validationErrors.Count == 0;
    }

    private bool IsNewMinomat() => this._structureNode.Entity == null;

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
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

    [Required(ErrorMessage = "MSS_MINOMAT_MASTER_RADIO_ID")]
    [Range(47100000, 48099999, ErrorMessage = "MSS_MINOMATS_RADIO_ID_BETWEEN")]
    public string RadioId
    {
      get => this._radioId;
      set
      {
        this._radioId = value;
        this.OnPropertyChanged(nameof (RadioId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMATS_NET_ID_REQUIRED")]
    [Range(0, 249, ErrorMessage = "MSS_MINOMATS_NET_ID_OUT_OF_RANGE")]
    public int NetId
    {
      get => this._netId;
      set
      {
        this._netId = value;
        this.OnPropertyChanged(nameof (NetId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMATS_NODE_ID_REQUIRED")]
    [Range(1, 1, ErrorMessage = "MSS_MINOMATS_NODE_ID_MUST_BE_1")]
    public int NodeId
    {
      get => this._nodeId;
      set
      {
        this._nodeId = value;
        this.OnPropertyChanged(nameof (NodeId));
      }
    }

    [Required(ErrorMessage = "MSS_Client_EntranceRequired")]
    public string Entrance
    {
      get => this._entrance;
      set
      {
        this._entrance = value;
        this.OnPropertyChanged(nameof (Entrance));
      }
    }

    public List<string> EntrancesList
    {
      get => EditMinomatMasterViewModel._entrancesList;
      set => EditMinomatMasterViewModel._entrancesList = value;
    }

    public string FloorNameValue
    {
      get => this._floorNameValue;
      set
      {
        this._floorNameValue = value;
        this.OnPropertyChanged(nameof (FloorNameValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_FloorNoRequired")]
    public string FloorNrValue
    {
      get => this._floorNrValue;
      set
      {
        this._floorNrValue = value;
        this.OnPropertyChanged(nameof (FloorNrValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_ApartmentNoRequired")]
    public string ApartmentNrValue
    {
      get => this._apartmentNrValue;
      set
      {
        this._apartmentNrValue = value;
        this.OnPropertyChanged(nameof (ApartmentNrValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_DirectionRequired")]
    public int? SelectedDirectionId
    {
      get => this._selectedDirectionId;
      set
      {
        this._selectedDirectionId = value;
        this.OnPropertyChanged(nameof (SelectedDirectionId));
      }
    }

    [Required(ErrorMessage = "MSS_Client_FloorNameRequired")]
    public int? SelectedFloorNameId
    {
      get => this._selectedFloorNameId;
      set
      {
        this._selectedFloorNameId = value;
        this.OnPropertyChanged(nameof (SelectedFloorNameId));
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

    [Required(ErrorMessage = "MSS_MINOMAT_CHANNEL_REQUIRED")]
    public int Channel
    {
      get => this._channel;
      set
      {
        this._channel = value;
        this.OnPropertyChanged(nameof (Channel));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMATS_PROVIDER")]
    public Provider SelectedProvider
    {
      get => this._provider;
      set
      {
        this._provider = value;
        if (this._provider != null)
        {
          this.SimPin = this._provider.SimPin;
          this.AccessPoint = this._provider.AccessPoint;
          this.UserId = this._provider.UserId;
          this.UserPassword = this._provider.UserPassword;
        }
        this.OnPropertyChanged(nameof (SelectedProvider));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_SIMPIN")]
    public string SimPin
    {
      get => this._simPin;
      set
      {
        this._simPin = value;
        this.OnPropertyChanged(nameof (SimPin));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_ACCESSPOINT")]
    public string AccessPoint
    {
      get => this._accessPoint;
      set
      {
        this._accessPoint = value;
        this.OnPropertyChanged(nameof (AccessPoint));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_USERID")]
    public string UserId
    {
      get => this._userId;
      set
      {
        this._userId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_USERPASS")]
    public string UserPassword
    {
      get => this._userPassword;
      set
      {
        this._userPassword = value;
        this.OnPropertyChanged(nameof (UserPassword));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_POLLING_REQUIRED")]
    public int? Polling
    {
      get => this._polling;
      set
      {
        this._polling = value;
        this.OnPropertyChanged(nameof (Polling));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_HOSTANDPORT_REQUIRED")]
    public string HostAndPort
    {
      get => this._hostandPort;
      set
      {
        this._hostandPort = value;
        this.OnPropertyChanged(nameof (HostAndPort));
      }
    }

    public string Url
    {
      get => this._url;
      set
      {
        this._url = value;
        this.OnPropertyChanged(nameof (Url));
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

    public IEnumerable<Country> CountryCollection
    {
      get
      {
        return (IEnumerable<Country>) this._countryRepository.GetAll().OrderBy<Country, string>((Func<Country, string>) (x => x.Name)).ToList<Country>();
      }
    }

    public Guid SelectedCountryId { get; set; }

    public string SimCardNumber
    {
      get => this._simCardNumber;
      set
      {
        this._simCardNumber = value;
        this.OnPropertyChanged(nameof (SimCardNumber));
      }
    }
  }
}
