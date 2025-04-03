// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditMinomatSlaveViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  internal class EditMinomatSlaveViewModel : ValidationViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private StructureNodeDTO _structureNode;
    private IWindowFactory _windowFactory;
    private readonly string _orderNumber;
    private IRepository<Minomat> _minomatRepository;
    private readonly IRepository<MinomatRadioDetails> _minomatRadioDetailsRepository;
    private readonly IRepository<Country> _countryRepository;
    private Minomat _currentMinomatSlave;
    private MinomatRadioDetails _currentMinomatSlaveRadioDetails;
    private Minomat _currentMinomatMaster;
    private MinomatRadioDetails _currentMinomatMasterRadioDetails;
    private bool wasSlaveSaved = false;
    private string _title;
    private bool _isBusy;
    private string _slaveRadioId;
    private int _nodeId;
    private int _netId = 0;
    private string _masterRadioId;
    private ViewModelBase _messageUserControl;
    private string _floorNrValue = string.Empty;
    private string _apartmentNrValue = string.Empty;
    private int? _selectedDirectionId;
    private int? _selectedFloorNameId;
    private string _entrance;
    private static List<string> _entrancesList;
    private int _channel;

    public EditMinomatSlaveViewModel(
      IRepositoryFactory repositoryFactory,
      StructureNodeDTO node,
      IWindowFactory windowFactory,
      string orderNumber)
    {
      this._structureNode = node;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._orderNumber = orderNumber;
      this._minomatRepository = this._repositoryFactory.GetRepository<Minomat>();
      this._minomatRadioDetailsRepository = this._repositoryFactory.GetRepository<MinomatRadioDetails>();
      this._countryRepository = repositoryFactory.GetRepository<Country>();
      MinomatSerializableDTO selectedMinomatSlaveDto = node.Entity as MinomatSerializableDTO;
      MinomatSerializableDTO selectedMinomatMasterDto = new MinomatSerializableDTO();
      if (node.ParentNode != null && node.ParentNode.Entity != null)
      {
        selectedMinomatMasterDto = node.ParentNode.Entity as MinomatSerializableDTO;
        if (selectedMinomatMasterDto != null)
        {
          this._currentMinomatMaster = Mapper.Map<MinomatSerializableDTO, Minomat>(selectedMinomatMasterDto);
          this._currentMinomatMasterRadioDetails = repositoryFactory.GetRepository<MinomatRadioDetails>().SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (x => x.Minomat.Id == selectedMinomatMasterDto.Id)).FirstOrDefault<MinomatRadioDetails>();
          this.MasterRadioId = this._currentMinomatMaster.RadioId;
          this.Channel = this._currentMinomatMasterRadioDetails?.Channel != null ? int.Parse(this._currentMinomatMasterRadioDetails?.Channel) : 0;
          if (this._currentMinomatMasterRadioDetails?.NetId != null)
            this.NetId = int.Parse(this._currentMinomatMasterRadioDetails.NetId);
        }
      }
      EditMinomatSlaveViewModel._entrancesList = this.GetStructureEntrances(node);
      if (selectedMinomatSlaveDto != null)
      {
        this.Title = Resources.MSS_EditMinomatSlave;
        this._currentMinomatSlave = Mapper.Map<MinomatSerializableDTO, Minomat>(selectedMinomatSlaveDto);
        this._currentMinomatSlaveRadioDetails = repositoryFactory.GetRepository<MinomatRadioDetails>().SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (x => x.Minomat.Id == selectedMinomatSlaveDto.Id)).FirstOrDefault<MinomatRadioDetails>();
        if (this._currentMinomatSlaveRadioDetails != null)
        {
          if (!string.IsNullOrEmpty(this._currentMinomatSlaveRadioDetails.Location))
          {
            string[] positionInfo = this._currentMinomatSlaveRadioDetails.Location.Split('/');
            this.FloorNrValue = positionInfo[0];
            FloorNameDTO floorNameDto = this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (fl => fl.FloorNameEnum.ToString() == positionInfo[1]));
            if (floorNameDto != null)
              this.SelectedFloorNameId = new int?(floorNameDto.Id);
            this.ApartmentNrValue = positionInfo[2];
            DirectionDTO directionDto = this.GetDirections.FirstOrDefault<DirectionDTO>((Func<DirectionDTO, bool>) (d => d.DirectionEnum.ToString() == positionInfo[3]));
            if (directionDto != null)
              this.SelectedDirectionId = new int?(directionDto.Id);
          }
          this.NodeId = int.Parse(this._currentMinomatSlaveRadioDetails.NodeId);
          this.Description = this._currentMinomatSlaveRadioDetails.Description;
          this.SlaveRadioId = this._currentMinomatSlave.RadioId;
          this.Entrance = this.EntrancesList.FirstOrDefault<string>((Func<string, bool>) (item => item == this._currentMinomatSlaveRadioDetails.Entrance));
          this.Description = this._currentMinomatSlaveRadioDetails.Description;
        }
        Country country = this._currentMinomatSlave.Country;
        this.SelectedCountryId = country != null ? country.Id : Guid.Empty;
      }
      else
      {
        this.Title = Resources.MSS_AddMinomatSlave;
        this._currentMinomatSlave = new Minomat();
        this._currentMinomatSlaveRadioDetails = new MinomatRadioDetails();
        int generatedNodeId;
        bool isSuccessful;
        this.GenerateSlaveNodeId(out generatedNodeId, out isSuccessful);
        if (isSuccessful)
          this.NodeId = generatedNodeId;
        else
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_MinomatSlave_CannotGenerateNodeId);
        Country country = MSS.Business.Utils.AppContext.Current.LoggedUser.Country;
        this.SelectedCountryId = country != null ? country.Id : Guid.Empty;
      }
    }

    public ICommand EditMinomatSlaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (this.IsValid)
          {
            Minomat minomat;
            if (this.IsNewMinomat())
            {
              minomat = this._minomatRepository.SearchFor((Expression<Func<Minomat, bool>>) (item => item.RadioId == this.SlaveRadioId && item.Status == StatusMinomatEnum.New.ToString() && !item.IsDeactivated)).FirstOrDefault<Minomat>();
            }
            else
            {
              minomat = this._minomatRepository.GetById((object) ((MinomatSerializableDTO) this._structureNode.Entity).Id);
              minomat.RadioDetails = this._currentMinomatSlaveRadioDetails;
            }
            if (minomat != null && this.IsNewMinomat())
            {
              bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_MinomatSlave_ExistingMinomatSlave), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_MinomatSlave_SaveNewSettings), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) true)));
              if (newModalDialog.HasValue && newModalDialog.Value)
                minomat = (Minomat) null;
            }
            if (minomat != null)
            {
              this._currentMinomatSlave = minomat;
              this.CopyNewMinomatSettings(false);
            }
            else
            {
              this.CopyNewMinomatSettings(true);
              this._currentMinomatSlave.RadioDetails = this._currentMinomatSlaveRadioDetails;
            }
            GMMMinomatConfigurator gmmConfigurator = GMMMinomatConfigurator.GetInstance(true, CustomerConfiguration.GetPropertyValue<bool>("IsDeviceConnectionMandatory"));
            gmmConfigurator.OnError = (EventHandler<GMMMinomatConfiguratorResult>) ((sender, result) => Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, result.Message, false))));
            this.IsBusy = true;
            Task.Run<GMMMinomatConfiguratorResult>((Func<GMMMinomatConfiguratorResult>) (() => gmmConfigurator.SetupMinomat(this._currentMinomatSlave, this._orderNumber))).ContinueWith((Action<Task<GMMMinomatConfiguratorResult>>) (p =>
            {
              GMMMinomatConfiguratorResult gmmConfiguratorResult = p.Result;
              if (gmmConfiguratorResult.IsSuccess)
              {
                this._currentMinomatSlave.Status = StatusMinomatEnum.BuiltIn.ToString();
                ISession session = this._repositoryFactory.GetSession();
                session.BeginTransaction();
                try
                {
                  this._currentMinomatSlaveRadioDetails.LastConnection = new DateTime?(DateTime.Now);
                  this._minomatRepository.TransactionalUpdate(this._currentMinomatSlave);
                  this._minomatRadioDetailsRepository.TransactionalUpdate(this._currentMinomatSlaveRadioDetails);
                  session.Transaction.Commit();
                  this.wasSlaveSaved = true;
                }
                catch (Exception ex)
                {
                  session.Transaction.Rollback();
                  Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(ex.Message)));
                }
                if (this._structureNode.Entity == null)
                  this._structureNode.Entity = (object) new MinomatSerializableDTO();
                Mapper.CreateMap<Minomat, MinomatSerializableDTO>();
                Mapper.Map<Minomat, object>(this._currentMinomatSlave, this._structureNode.Entity);
                EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
                {
                  Minomat = this._currentMinomatSlave,
                  Node = this._structureNode,
                  Guid = this._structureNode.RootNode != this._structureNode ? this._structureNode.RootNode.Id : this._structureNode.Id,
                  Message = new MSS.DTO.Message.Message()
                  {
                    MessageType = MessageTypeEnum.Success,
                    MessageText = MessageCodes.Success_Save.GetStringValue()
                  },
                  Name = this.SlaveRadioId,
                  Description = this._structureNode.Description
                }, (IViewModel) this);
                Application.Current.Dispatcher.Invoke((Action) (() => this.OnRequestClose(true)));
              }
              else
                Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Error_Title, gmmConfiguratorResult.Message, false)));
              this.IsBusy = false;
            }));
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_ValidationError);
        });
      }
    }

    private void CopyNewMinomatSettings(bool isNew)
    {
      if (this._currentMinomatSlaveRadioDetails == null)
        this._currentMinomatSlaveRadioDetails = new MinomatRadioDetails();
      if (this._currentMinomatSlaveRadioDetails.Minomat == null)
        this._currentMinomatSlaveRadioDetails.Minomat = this._currentMinomatSlave;
      this._currentMinomatSlaveRadioDetails.DueDate = (DateTime?) ((LocationDTO) this._structureNode.RootNode?.Entity)?.DueDate;
      this._currentMinomatSlave.RadioId = this.SlaveRadioId;
      MinomatRadioDetails slaveRadioDetails = this._currentMinomatSlaveRadioDetails;
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
      string str = string.Concat(objArray);
      slaveRadioDetails.Location = str;
      this._currentMinomatSlaveRadioDetails.Entrance = this.Entrance;
      this._currentMinomatSlaveRadioDetails.Description = this.Description;
      this._currentMinomatSlaveRadioDetails.NetId = this.NetId.ToString();
      this._currentMinomatSlaveRadioDetails.NodeId = this.NodeId.ToString();
      this._currentMinomatSlaveRadioDetails.Channel = this.Channel.ToString();
      this._currentMinomatSlave.Provider = this._currentMinomatMaster?.Provider;
      this._currentMinomatSlave.SimPin = this._currentMinomatMaster?.SimPin;
      this._currentMinomatSlave.AccessPoint = this._currentMinomatMaster?.AccessPoint;
      this._currentMinomatSlave.UserId = this._currentMinomatMaster?.UserId;
      this._currentMinomatSlave.UserPassword = this._currentMinomatMaster?.UserPassword;
      this._currentMinomatSlave.SimCardNumber = this._currentMinomatMaster?.SimCardNumber;
      this._currentMinomatSlave.Polling = this._currentMinomatMaster != null ? this._currentMinomatMaster.Polling : 0;
      this._currentMinomatSlave.HostAndPort = this._currentMinomatMaster?.HostAndPort;
      this._currentMinomatSlave.Url = this._currentMinomatMaster?.Url;
      this._currentMinomatSlave.Country = this.CountryCollection.FirstOrDefault<Country>((Func<Country, bool>) (item => item.Id == this.SelectedCountryId));
      if (!this._currentMinomatSlaveRadioDetails.StatusDevices.HasValue)
        this._currentMinomatSlaveRadioDetails.StatusDevices = new MinomatStatusDevicesEnum?(MinomatStatusDevicesEnum.Open);
      if (!this._currentMinomatSlaveRadioDetails.StatusNetwork.HasValue)
        this._currentMinomatSlaveRadioDetails.StatusNetwork = new MinomatStatusNetworkEnum?(MinomatStatusNetworkEnum.Open);
      if (isNew)
      {
        this._currentMinomatSlave.MinomatMasterId = new Guid?(this._currentMinomatMaster != null ? this._currentMinomatMaster.Id : Guid.Empty);
        this._currentMinomatSlave.Challenge = this._currentMinomatMaster?.Challenge;
        this._currentMinomatSlave.GsmId = this.SlaveRadioId;
        this._currentMinomatSlave.SessionKey = this._currentMinomatMaster?.SessionKey;
        this._currentMinomatSlave.CreatedOn = new DateTime?(DateTime.Now);
        this._currentMinomatSlave.CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString();
        this._currentMinomatSlave.Status = StatusMinomatEnum.New.ToString();
      }
      this._currentMinomatSlave.LastUpdatedBy = "";
      Guid? id = ((LocationDTO) this._structureNode.RootNode?.Entity)?.Scenario?.Id;
      if (!id.HasValue)
        return;
      this._currentMinomatSlave.Scenario = this._repositoryFactory.GetRepository<Scenario>().GetById((object) id);
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

    [Required(ErrorMessage = "MSS_MINOMAT_SLAVE_RADIO_ID_REQUIRED")]
    [Range(48100000, 2147483647, ErrorMessage = "MSS_MINOMAT_SLAVE_RADIO_ID_GREATER_THAN")]
    public string SlaveRadioId
    {
      get => this._slaveRadioId;
      set
      {
        this._slaveRadioId = value;
        this.OnPropertyChanged(nameof (SlaveRadioId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_SLAVE_NODE_ID_REQUIRED")]
    [Range(2, 999, ErrorMessage = "MSS_MINOMAT_SLAVE_NODE_ID_OUT_OF_RANGE")]
    public int NodeId
    {
      get => this._nodeId;
      set
      {
        this._nodeId = value;
        this.OnPropertyChanged(nameof (NodeId));
      }
    }

    public int NetId
    {
      get => this._netId;
      set
      {
        this._netId = value;
        this.OnPropertyChanged(nameof (NetId));
      }
    }

    public string MasterRadioId
    {
      get => this._masterRadioId;
      set
      {
        this._masterRadioId = value;
        this.OnPropertyChanged(nameof (MasterRadioId));
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

    public string Description
    {
      get
      {
        return this._currentMinomatSlaveRadioDetails != null ? this._currentMinomatSlaveRadioDetails.Description : "";
      }
      set
      {
        this._currentMinomatSlaveRadioDetails.Description = value;
        this.OnPropertyChanged(nameof (Description));
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
      get => EditMinomatSlaveViewModel._entrancesList;
      set => EditMinomatSlaveViewModel._entrancesList = value;
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

    public IEnumerable<Country> CountryCollection
    {
      get
      {
        return (IEnumerable<Country>) this._countryRepository.GetAll().OrderBy<Country, string>((Func<Country, string>) (x => x.Name)).ToList<Country>();
      }
    }

    public Guid SelectedCountryId { get; set; }

    private void ShowSuccessMessage()
    {
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
      {
        MessageType = MessageTypeEnum.Success,
        MessageText = MessageCodes.Success_Save.GetStringValue()
      };
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.SlaveRadioId));
      if (!(propertyName == propertyName1) || this.wasSlaveSaved)
        return new List<string>();
      ICollection<string> validationErrors;
      this.ValidateSlaveRadioId(this.SlaveRadioId, out validationErrors);
      return validationErrors.ToList<string>();
    }

    public bool ValidateSlaveRadioId(string slaveRadioId, out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      List<Minomat> list = this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.RadioId == slaveRadioId && !x.IsDeactivated)).ToList<Minomat>();
      int num;
      if (list.Any<Minomat>((Func<Minomat, bool>) (item => item.Status != StatusMinomatEnum.New.ToString())))
      {
        Guid id = this._currentMinomatSlave.Id;
        num = this._currentMinomatSlave.Id == Guid.Empty ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      else if (list.Any<Minomat>())
      {
        foreach (Minomat minomat in list)
        {
          if (!(minomat.RadioId == this._currentMinomatSlave.RadioId) || !(minomat.Id == this._currentMinomatSlave.Id))
          {
            validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
            break;
          }
        }
      }
      return validationErrors.Count == 0;
    }

    private List<string> GetStructureEntrances(StructureNodeDTO node)
    {
      List<string> first = new List<string>();
      List<Guid> minomatGuidList = new List<Guid>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.RootNode.SubNodes)
      {
        if (subNode.Entity != null)
        {
          string name = subNode.NodeType?.Name;
          if (name != null)
          {
            switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), name, true))
            {
              case StructureNodeTypeEnum.Tenant:
                if (subNode.Entity is TenantDTO entity1)
                {
                  string entrance = entity1.Entrance;
                  if (entrance != null && !first.Contains(entrance))
                    first.Add(entrance);
                  break;
                }
                break;
              case StructureNodeTypeEnum.MinomatMaster:
                if (subNode.Entity is MinomatSerializableDTO entity3)
                {
                  Guid id1 = entity3.Id;
                  if (id1 != Guid.Empty)
                    minomatGuidList.Add(id1);
                  using (IEnumerator<StructureNodeDTO> enumerator = subNode.SubNodes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      if (enumerator.Current.Entity is MinomatSerializableDTO entity2)
                      {
                        Guid id2 = entity2.Id;
                        if (id2 != Guid.Empty)
                          minomatGuidList.Add(id2);
                      }
                    }
                    break;
                  }
                }
                else
                  break;
            }
          }
        }
      }
      List<string> stringList;
      if (minomatGuidList.Count <= 0)
        stringList = new List<string>();
      else
        stringList = this._minomatRadioDetailsRepository.SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (x => minomatGuidList.Contains(x.Minomat.Id))).Select<MinomatRadioDetails, string>((Func<MinomatRadioDetails, string>) (x => x.Entrance)).Distinct<string>().ToList<string>();
      List<string> second = stringList;
      return first.Union<string>((IEnumerable<string>) second).OrderBy<string, string>((Func<string, string>) (x => x)).ToList<string>();
    }

    public ObservableCollection<DirectionDTO> GetDirections
    {
      get => new ObservableCollection<DirectionDTO>(FloorHelper.GetDirections());
    }

    public IEnumerable<FloorNameDTO> GetFloorNames => FloorHelper.GetFloorNames();

    private void GenerateSlaveNodeId(out int generatedNodeId, out bool isSuccessful)
    {
      generatedNodeId = 0;
      isSuccessful = false;
      List<string> source = new List<string>();
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) this._structureNode.RootNode.SubNodes)
      {
        MinomatSerializableDTO masterSerializableDTO = subNode.Entity as MinomatSerializableDTO;
        if (masterSerializableDTO != null)
        {
          List<Guid> slavesIdsAssignedToMaster = this._minomatRepository.Where((Expression<Func<Minomat, bool>>) (item => item.MinomatMasterId != new Guid?() && item.MinomatMasterId == (Guid?) masterSerializableDTO.Id)).Select<Minomat, Guid>((Expression<Func<Minomat, Guid>>) (item => item.Id)).ToList<Guid>();
          List<string> list = this._minomatRadioDetailsRepository.Where((Expression<Func<MinomatRadioDetails, bool>>) (item => slavesIdsAssignedToMaster.Contains(item.Minomat.Id))).Select<MinomatRadioDetails, string>((Expression<Func<MinomatRadioDetails, string>>) (item => item.NodeId)).ToList<string>();
          source.AddRange((IEnumerable<string>) list);
        }
      }
      if (source.Any<string>())
      {
        for (int index = 2; index <= 999; ++index)
        {
          if (!source.Contains(index.ToString()))
          {
            generatedNodeId = index;
            isSuccessful = true;
            break;
          }
        }
      }
      else
      {
        generatedNodeId = 2;
        isSuccessful = true;
      }
    }

    private bool IsNewMinomat() => this._structureNode.Entity == null;
  }
}
