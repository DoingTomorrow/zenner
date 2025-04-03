// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.CreateEditTenantViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.DTO.MessageHandler;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class CreateEditTenantViewModel : ValidationViewModelBase
  {
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
    private readonly StructureNodeDTO _selectedNode;
    private readonly List<int> _localStructureTenantNrs;
    private bool _isExistingEntity;
    private string _tenantDialogTitle;
    private string _name;
    private string _description;
    private bool _isAddEntityButtonVisible;
    private bool _isEditEntityButtonVisible;
    private int _tenantNr;
    private string _nameValue = string.Empty;
    private static List<string> _entrancesList;
    private string _entrance;
    private string _floorNameValue = string.Empty;
    private string _floorNrValue = string.Empty;
    private string _apartmentNrValue = string.Empty;
    private string _descriptionValue = string.Empty;
    private string _customerTenantNo = string.Empty;
    private ViewModelBase _messageUserControl;

    [Inject]
    public CreateEditTenantViewModel(
      bool isExistingEntity,
      StructureNodeDTO node,
      List<int> localStructureTenantNrs,
      IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._isExistingEntity = isExistingEntity;
      this._tenantDialogTitle = isExistingEntity ? CultureResources.GetValue("MSS_Client_Structures_EditTenant_Title") : CultureResources.GetValue("MSS_Client_Structures_CreateTenant_Title");
      this._isAddEntityButtonVisible = !isExistingEntity;
      this._isEditEntityButtonVisible = isExistingEntity;
      this._selectedNode = node;
      this._localStructureTenantNrs = localStructureTenantNrs;
      this.EntrancesList = this.GetStructureEntrances(this._selectedNode);
      if (isExistingEntity)
      {
        TenantDTO tenantDTO = node.Entity as TenantDTO;
        if (tenantDTO != null)
        {
          this.TenantGuid = tenantDTO.Id;
          this.TenantNr = tenantDTO.TenantNr;
          this.NameValue = tenantDTO.Name;
          FloorNameDTO floorNameDto = this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (d => d.FloorNameEnum.ToString() == tenantDTO.FloorName));
          if (floorNameDto != null)
            this.SelectedFloorNameId = floorNameDto.Id;
          this.FloorNrValue = tenantDTO.FloorNr;
          this.ApartmentNrValue = tenantDTO.ApartmentNr;
          DirectionDTO directionDto = this.GetDirections.FirstOrDefault<DirectionDTO>((Func<DirectionDTO, bool>) (d => d.DirectionEnum.ToString() == tenantDTO.Direction));
          if (directionDto != null)
            this.SelectedDirectionId = directionDto.Id;
          this.DescriptionValue = tenantDTO.Description;
          this.CustomerTenantNo = tenantDTO.CustomerTenantNo;
          this.Entrance = tenantDTO.Entrance != null ? this.EntrancesList.FirstOrDefault<string>((Func<string, bool>) (item => item != null && item.Equals(tenantDTO.Entrance))) : (string) null;
        }
      }
      else
        this.TenantNr = this.GetNextTenantNr();
      this.Name = node.Name;
      this.Description = node.Description;
    }

    private int GetNextTenantNr()
    {
      return this._localStructureTenantNrs.Concat<int>((IEnumerable<int>) new int[1]).Max() + 1;
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
        stringList = this._repositoryFactory.GetRepository<MinomatRadioDetails>().SearchFor((Expression<Func<MinomatRadioDetails, bool>>) (x => minomatGuidList.Contains(x.Minomat.Id))).Select<MinomatRadioDetails, string>((Func<MinomatRadioDetails, string>) (x => x.Entrance)).Distinct<string>().ToList<string>();
      List<string> second = stringList;
      return first.Union<string>((IEnumerable<string>) second).OrderBy<string, string>((Func<string, string>) (x => x)).ToList<string>();
    }

    private TenantManager GetTenantManagerInstance() => new TenantManager(this._repositoryFactory);

    public bool IsExistingEntity
    {
      get => this._isExistingEntity;
      set => this._isExistingEntity = value;
    }

    public string TenantDialogTitle
    {
      get => this._tenantDialogTitle;
      set => this._tenantDialogTitle = value;
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

    public bool IsAddEntityButtonVisible
    {
      get => this._isAddEntityButtonVisible;
      set => this._isAddEntityButtonVisible = value;
    }

    public bool IsEditEntityButtonVisible
    {
      get => this._isEditEntityButtonVisible;
      set => this._isEditEntityButtonVisible = value;
    }

    public IEnumerable<FloorNameDTO> GetFloorNames => FloorHelper.GetFloorNames();

    public ObservableCollection<DirectionDTO> GetDirections
    {
      get => new ObservableCollection<DirectionDTO>(FloorHelper.GetDirections());
    }

    private Guid TenantGuid { get; set; }

    [Required(ErrorMessage = "MSS_Client_Structures_CreateTenant_TenantNrValidation")]
    public int TenantNr
    {
      get => this._tenantNr;
      set
      {
        this._tenantNr = value;
        this.Name = this._tenantNr.ToString() + " - " + this._nameValue;
        this.OnPropertyChanged(nameof (TenantNr));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Structures_CreateTenant_NameValidation")]
    public string NameValue
    {
      get => this._nameValue;
      set
      {
        this._nameValue = value;
        this.Name = this._tenantNr.ToString() + " - " + this._nameValue;
        this.OnPropertyChanged(nameof (NameValue));
      }
    }

    public List<string> EntrancesList
    {
      get => CreateEditTenantViewModel._entrancesList;
      set => CreateEditTenantViewModel._entrancesList = value;
    }

    public string Entrance
    {
      get => this._entrance;
      set
      {
        this._entrance = value;
        this.OnPropertyChanged(nameof (Entrance));
      }
    }

    public string NewEntrance
    {
      set
      {
        if (this.Entrance != null || string.IsNullOrEmpty(value))
          return;
        CreateEditTenantViewModel._entrancesList.Add(value);
        this.Entrance = value;
      }
    }

    public string FloorNameValue
    {
      get => this._floorNameValue;
      set => this._floorNameValue = value;
    }

    public string FloorNrValue
    {
      get => this._floorNrValue;
      set => this._floorNrValue = value;
    }

    public string ApartmentNrValue
    {
      get => this._apartmentNrValue;
      set => this._apartmentNrValue = value;
    }

    public int SelectedDirectionId { get; set; }

    public int SelectedFloorNameId { get; set; }

    public string DescriptionValue
    {
      get => this._descriptionValue;
      set => this._descriptionValue = value;
    }

    public string CustomerTenantNo
    {
      get => this._customerTenantNo;
      set => this._customerTenantNo = value;
    }

    public ActionStructureAndEntitiesUpdate StructureUpdateAction { get; set; }

    public ICommand AddTenantCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.ValidateProperty("TenantNr");
          if (this.IsValid)
          {
            DirectionDTO directionDto = this.GetDirections.FirstOrDefault<DirectionDTO>((Func<DirectionDTO, bool>) (d => d.Id == this.SelectedDirectionId));
            FloorNameDTO floorNameDto = this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (f => f.Id == this.SelectedFloorNameId));
            Tenant tenant = new Tenant()
            {
              TenantNr = this._tenantNr,
              Name = this._nameValue,
              FloorName = floorNameDto?.FloorNameEnum.ToString(),
              FloorNr = this._floorNrValue,
              Description = this._descriptionValue,
              Direction = directionDto?.DirectionEnum.ToString(),
              ApartmentNr = this._apartmentNrValue,
              CustomerTenantNo = this._customerTenantNo,
              Entrance = this._entrance
            };
            if (CreateEditTenantViewModel._entrancesList != null && !CreateEditTenantViewModel._entrancesList.Contains(this._entrance))
              CreateEditTenantViewModel._entrancesList.Add(this._entrance);
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            };
            this.StructureUpdateAction = new ActionStructureAndEntitiesUpdate()
            {
              Tenant = tenant,
              Node = this._selectedNode,
              Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
              Message = message,
              Name = this.Name,
              Description = this.Description
            };
            EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(this.StructureUpdateAction, (IViewModel) this);
            this.OnRequestClose(true);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = MessageCodes.ValidationError.GetStringValue()
            }.MessageText);
        });
      }
    }

    public ICommand EditTenantCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.ValidateProperty("TenantNr");
          if (this.IsValid)
          {
            DirectionDTO directionDto = this.GetDirections.FirstOrDefault<DirectionDTO>((Func<DirectionDTO, bool>) (d => d.Id == this.SelectedDirectionId));
            FloorNameDTO floorNameDto = this.GetFloorNames.FirstOrDefault<FloorNameDTO>((Func<FloorNameDTO, bool>) (f => f.Id == this.SelectedFloorNameId));
            Tenant tenant1 = new Tenant();
            Tenant tenant2 = tenant1;
            TenantDTO entity = (TenantDTO) this._selectedNode.Entity;
            Guid guid = entity != null ? entity.Id : Guid.Empty;
            tenant2.Id = guid;
            tenant1.TenantNr = this._tenantNr;
            tenant1.Name = this._nameValue;
            tenant1.FloorName = floorNameDto?.FloorNameEnum.ToString();
            tenant1.FloorNr = this._floorNrValue;
            tenant1.Description = this._descriptionValue;
            tenant1.Direction = directionDto?.DirectionEnum.ToString();
            tenant1.ApartmentNr = this._apartmentNrValue;
            tenant1.CustomerTenantNo = this._customerTenantNo;
            tenant1.Entrance = this._entrance;
            Tenant source = tenant1;
            if (this._selectedNode.Entity == null)
            {
              TenantDTO destination = new TenantDTO();
              Mapper.CreateMap(typeof (Tenant), typeof (TenantDTO));
              Mapper.Map<Tenant, TenantDTO>(source, destination);
              this._selectedNode.Entity = (object) destination;
            }
            MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            };
            this.StructureUpdateAction = new ActionStructureAndEntitiesUpdate()
            {
              Tenant = source,
              Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
              Node = this._selectedNode,
              Message = message,
              Name = this.Name,
              Description = this.Description
            };
            EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(this.StructureUpdateAction, (IViewModel) this);
            this.OnRequestClose(true);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = MessageCodes.ValidationError.GetStringValue()
            }.MessageText);
        });
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

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<int>((Expression<Func<int>>) (() => this.TenantNr));
      if (propertyName != propertyName1)
        return new List<string>();
      ICollection<string> validationErrors;
      this.GetTenantManagerInstance().ValidateTenantNr(this.TenantNr, this._localStructureTenantNrs, this.TenantGuid, out validationErrors);
      this.IsValid &= validationErrors.Count <= 0;
      return validationErrors.ToList<string>();
    }
  }
}
