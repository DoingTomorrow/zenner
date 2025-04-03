// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.CreateEditLocationViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DTO.Clients;
using MSS.DTO.MessageHandler;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class CreateEditLocationViewModel : ValidationViewModelBase
  {
    private readonly IRepository<Scenario> _scenarioRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly StructureNodeDTO _selectedNode;
    private string _name;
    private string _description;
    private string _cityTextValue = string.Empty;
    private string _streetTextValue = string.Empty;
    private string _descriptionValue = string.Empty;
    private string _zipCodeValue = string.Empty;
    private string _buildingNumerValue = string.Empty;
    private DateTime? _dueDateValue = new DateTime?(DateTime.Now);
    private Guid? _scenarioIdGuid;
    private int _generationEnumObjId;
    private bool _isMBusGenerationNotSelected;
    private bool _isWalkByWithoutDueDateNotSelected;
    private IEnumerable<ScenarioDTO> _scenarios;
    private ViewModelBase _messageUserControl;

    [Inject]
    public CreateEditLocationViewModel(
      bool isExistingEntity,
      StructureNodeDTO node,
      IRepositoryFactory repositoryFactory,
      List<string> locationNumberList)
    {
      this.IsExistingEntity = isExistingEntity;
      this.LocationDialogTitle = isExistingEntity ? CultureResources.GetValue("MSS_Client_Structures_EditLocation_Title") : CultureResources.GetValue("MSS_Client_Structures_CreateLocation_Title");
      this._selectedNode = node;
      this.EvaluationFactorVisibility = LicenseHelper.LicenseIsDisplayEvaluationFactor();
      this._repositoryFactory = repositoryFactory;
      this._scenarioRepository = repositoryFactory.GetRepository<Scenario>();
      this.LocationNumberList = locationNumberList;
      if (isExistingEntity && node.Entity != null)
      {
        LocationDTO entity = node.Entity as LocationDTO;
        this.LocationId = entity.Id;
        this.CityTextValue = entity.City;
        this.StreetTextValue = entity.Street;
        this.BuildingNumberValue = entity.BuildingNr;
        this.DescriptionValue = entity.Description;
        this.DueDateValue = entity.DueDate;
        this.GenerationEnumObjId = (int) entity.Generation;
        this.ZipCodeValue = entity.ZipCode;
        this.ScenarioId = entity.Scenario != null ? new Guid?(entity.Scenario.Id) : new Guid?();
        this.HasMaster = entity.HasMaster;
        this.ScaleEnumObjId = (int) entity.Scale;
        this.LocationNumberList.Remove(this.BuildingNumberValue);
      }
      this.Name = node.Name;
      this.Description = node.Description;
    }

    private LocationManager GetLocationManagerInstance()
    {
      return new LocationManager(this._repositoryFactory);
    }

    public bool IsExistingEntity { get; set; }

    public string LocationDialogTitle { get; set; }

    public bool IsAddEntityButtonVisible { get; set; }

    public bool IsEditEntityButtonVisible { get; set; }

    public bool EvaluationFactorVisibility { get; set; }

    public List<string> LocationNumberList { get; set; }

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

    [Required(ErrorMessage = "MSS_Client_Structures_CityErrorToolTip")]
    public string CityTextValue
    {
      get => this._cityTextValue;
      set
      {
        this._cityTextValue = value;
        this.Description = CreateEditLocationViewModel.ConcatenatedDescriptionValue(this.CityTextValue, this.StreetTextValue, this.ZipCodeValue);
        this.OnPropertyChanged(nameof (CityTextValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Structures_StreetErrorToolTip")]
    public string StreetTextValue
    {
      get => this._streetTextValue;
      set
      {
        this._streetTextValue = value;
        this.Description = CreateEditLocationViewModel.ConcatenatedDescriptionValue(this.CityTextValue, this.StreetTextValue, this.ZipCodeValue);
        this.OnPropertyChanged(nameof (StreetTextValue));
      }
    }

    public string DescriptionValue
    {
      get => this._descriptionValue;
      set => this._descriptionValue = value;
    }

    [Required(ErrorMessage = "MSS_Client_Structures_ZipCodeErrorToolTip")]
    public string ZipCodeValue
    {
      get => this._zipCodeValue;
      set
      {
        this._zipCodeValue = value;
        this.Description = CreateEditLocationViewModel.ConcatenatedDescriptionValue(this.CityTextValue, this.StreetTextValue, this.ZipCodeValue);
        this.OnPropertyChanged(nameof (ZipCodeValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Structures_BuildingNrErrorToolTip")]
    public string BuildingNumberValue
    {
      get => this._buildingNumerValue;
      set
      {
        this._buildingNumerValue = value;
        this.Name = this._buildingNumerValue;
        this.OnPropertyChanged(nameof (BuildingNumberValue));
      }
    }

    public DateTime? DueDateValue
    {
      get => this._dueDateValue;
      set
      {
        this._dueDateValue = value;
        this.OnPropertyChanged(nameof (DueDateValue));
      }
    }

    public Guid? ScenarioId
    {
      get => this._scenarioIdGuid;
      set
      {
        this._scenarioIdGuid = value;
        if (!this._scenarioIdGuid.HasValue && this.IsMBusGenerationNotSelected)
          this._scenarioIdGuid = this._scenarioRepository.GetAll().FirstOrDefault<Scenario>()?.Id;
        if (!this._scenarioIdGuid.HasValue || this.GenerationEnumObjId != GenerationEnum.Radio3.GetCelestaCodes().Max())
          return;
        switch (this._scenarioRepository.GetById((object) this._scenarioIdGuid).Code)
        {
          case 0:
          case 1:
          case 2:
          case 4:
            this.IsWalkByWithoutDueDateNotSelected = true;
            this.HasMaster = new bool?(true);
            this.OnPropertyChanged("HasMaster");
            break;
          case 6:
          case 8:
            this.IsWalkByWithoutDueDateNotSelected = false;
            this.HasMaster = new bool?(false);
            this.OnPropertyChanged("HasMaster");
            break;
          default:
            this.HasMaster = new bool?(false);
            this.OnPropertyChanged("HasMaster");
            break;
        }
      }
    }

    public int GenerationEnumObjId
    {
      get => this._generationEnumObjId;
      set
      {
        this._generationEnumObjId = value;
        if (this._generationEnumObjId == GenerationEnum.Radio2.GetCelestaCodes().Max())
        {
          this.HasMaster = new bool?(false);
          this._scenarios = this.GetScenarioDTOList().Take<ScenarioDTO>(1);
        }
        if (this._generationEnumObjId == GenerationEnum.Radio3.GetCelestaCodes().Max())
          this._scenarios = this.GetScenarioDTOList();
        if (this._generationEnumObjId == 4 || this._generationEnumObjId == 5)
        {
          this.IsMBusGenerationNotSelected = false;
          this.IsWalkByWithoutDueDateNotSelected = true;
        }
        else
          this.IsMBusGenerationNotSelected = true;
        if (this._scenarios != null && (!this.ScenarioId.HasValue || !this._scenarios.ToList<ScenarioDTO>().Any<ScenarioDTO>((Func<ScenarioDTO, bool>) (item =>
        {
          Guid id = item.Id;
          Guid? scenarioId = this.ScenarioId;
          return scenarioId.HasValue && id == scenarioId.GetValueOrDefault();
        }))))
        {
          ScenarioDTO scenarioDto = this._scenarios.FirstOrDefault<ScenarioDTO>();
          if (scenarioDto != null)
            this.ScenarioId = new Guid?(scenarioDto.Id);
        }
        this.OnPropertyChanged("HasMaster");
        this.OnPropertyChanged("GetScenarios");
        this.OnPropertyChanged("ScenarioId");
      }
    }

    public int GenerationValue { get; set; }

    private Guid LocationId { get; set; }

    public int ScaleEnumObjId { get; set; }

    public bool? HasMaster { get; set; }

    public IEnumerable<EnumObj> GetGenerations
    {
      get
      {
        IEnumerable<EnumObj> source = this.GetLocationManagerInstance().GetGenerations();
        if (!GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.MBus).Any<DeviceModel>())
          source = source.Where<EnumObj>((Func<EnumObj, bool>) (item => item.IdEnum != 4 && item.IdEnum != 5));
        return source;
      }
    }

    public bool IsMBusGenerationNotSelected
    {
      get => this._isMBusGenerationNotSelected;
      set
      {
        this._isMBusGenerationNotSelected = value;
        this.OnPropertyChanged(nameof (IsMBusGenerationNotSelected));
      }
    }

    public bool IsWalkByWithoutDueDateNotSelected
    {
      get => this._isWalkByWithoutDueDateNotSelected;
      set
      {
        this._isWalkByWithoutDueDateNotSelected = value;
        this.OnPropertyChanged(nameof (IsWalkByWithoutDueDateNotSelected));
      }
    }

    public IEnumerable<EnumObj> GetScales => this.GetLocationManagerInstance().GetScales();

    public IEnumerable<ScenarioDTO> GetScenarios
    {
      get => this._scenarios;
      set => this._scenarios = value;
    }

    public System.Windows.Input.ICommand SaveLocationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsExistingEntity)
            this.AddLocation();
          else
            this.EditLocation();
        });
      }
    }

    private void AddLocation()
    {
      this.ValidateProperty("GenerationEnumObjId");
      this.GenerationValue = this.GenerationEnumObjId;
      if (this.IsValid)
      {
        MSS.Core.Model.Structures.Location location = new MSS.Core.Model.Structures.Location()
        {
          BuildingNr = this._buildingNumerValue,
          Scenario = this.IsMBusGenerationNotSelected ? this._scenarioRepository.GetById((object) this.ScenarioId) : (Scenario) null,
          City = this._cityTextValue,
          Description = this._descriptionValue,
          DueDate = this.IsWalkByWithoutDueDateNotSelected ? this._dueDateValue : new DateTime?(),
          Generation = (GenerationEnum) this.GenerationEnumObjId,
          Street = this._streetTextValue,
          ZipCode = this._zipCodeValue,
          HasMaster = this.IsMBusGenerationNotSelected ? this.HasMaster : new bool?(),
          Scale = (ScaleEnum) this.ScaleEnumObjId
        };
        MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Success,
          MessageText = MessageCodes.Success_Save.GetStringValue()
        };
        EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
        {
          Location = location,
          Node = this._selectedNode,
          Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
          Message = message,
          Name = this.Name,
          Description = this.Description
        }, (IViewModel) this);
        this.OnRequestClose(true);
      }
      else
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Validation,
          MessageText = MessageCodes.ValidationError.GetStringValue()
        }.MessageText);
    }

    private void EditLocation()
    {
      this.ValidateProperty("GenerationEnumObjId");
      this.ValidateProperty("ScenarioId");
      this.ValidateProperty("BuildingNumberValue");
      this.GenerationValue = this.GenerationEnumObjId;
      if (this.IsValid)
      {
        MSS.Core.Model.Structures.Location source = new MSS.Core.Model.Structures.Location()
        {
          Id = ((LocationDTO) this._selectedNode.Entity).Id,
          BuildingNr = this._buildingNumerValue,
          City = this._cityTextValue,
          Description = this._descriptionValue,
          DueDate = this.IsWalkByWithoutDueDateNotSelected ? this._dueDateValue : new DateTime?(),
          Generation = (GenerationEnum) this.GenerationEnumObjId,
          Street = this._streetTextValue,
          ZipCode = this._zipCodeValue,
          Scenario = this.IsMBusGenerationNotSelected ? this._scenarioRepository.GetById((object) this.ScenarioId) : (Scenario) null,
          HasMaster = this.IsMBusGenerationNotSelected ? this.HasMaster : new bool?(),
          Scale = (ScaleEnum) this.ScaleEnumObjId
        };
        LocationDTO destination = new LocationDTO();
        Mapper.Map<MSS.Core.Model.Structures.Location, LocationDTO>(source, destination);
        StructureNodeDTO selectedNode = this._selectedNode;
        selectedNode.Entity = (object) destination;
        selectedNode.Description = CreateEditLocationViewModel.ConcatenatedDescriptionValue(source.City, source.Street, source.ZipCode);
        selectedNode.Name = source.BuildingNr;
        MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Success,
          MessageText = MessageCodes.Success_Save.GetStringValue()
        };
        EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
        {
          Location = source,
          Node = selectedNode,
          Guid = this._selectedNode.RootNode != this._selectedNode ? this._selectedNode.RootNode.Id : this._selectedNode.Id,
          Message = message,
          Name = this.Name,
          Description = this.Description
        }, (IViewModel) this);
        this.OnRequestClose(true);
      }
      else
        this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
        {
          MessageType = MessageTypeEnum.Validation,
          MessageText = MessageCodes.ValidationError.GetStringValue()
        }.MessageText);
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

    private IEnumerable<ScenarioDTO> GetScenarioDTOList()
    {
      ObservableCollection<ScenarioDTO> scenarioDTOList = new ObservableCollection<ScenarioDTO>();
      TypeHelperExtensionMethods.ForEach<Scenario>((IEnumerable<Scenario>) this._scenarioRepository.GetAll(), (Action<Scenario>) (r => scenarioDTOList.Add(new ScenarioDTO()
      {
        Id = r.Id,
        Code = r.Code,
        Name = string.IsNullOrEmpty(CultureResources.GetValue("Scenario_Type_" + (object) r.Code)) ? "Scenario_Type_" + (object) r.Code : CultureResources.GetValue("Scenario_Type_" + (object) r.Code)
      })));
      return (IEnumerable<ScenarioDTO>) scenarioDTOList.OrderBy<ScenarioDTO, int>((Func<ScenarioDTO, int>) (x => x.Code)).ToList<ScenarioDTO>();
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<int>((Expression<Func<int>>) (() => this.GenerationEnumObjId));
      string propertyName2 = this.GetPropertyName<Guid?>((Expression<Func<Guid?>>) (() => this.ScenarioId));
      this.GetPropertyName<DateTime?>((Expression<Func<DateTime?>>) (() => this.DueDateValue));
      string propertyName3 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.BuildingNumberValue));
      List<string> source = new List<string>();
      if (propertyName == propertyName1)
      {
        if (this.GenerationEnumObjId != 0)
          return source.ToList<string>();
        source.Add(Resources.MSS_Client_Structures_GenerationErrorToolTip);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (propertyName == propertyName2)
      {
        Guid? scenarioId = this.ScenarioId;
        Guid empty = Guid.Empty;
        if (!scenarioId.HasValue || scenarioId.HasValue && scenarioId.GetValueOrDefault() != empty)
          return source.ToList<string>();
        source.Add(Resources.MSS_Client_Structures_ScenarioErrorToolTip);
        this.IsValid = false;
        return source.ToList<string>();
      }
      if (propertyName != propertyName3)
        return source;
      if (this.BuildingNumberValue.Length < 10)
      {
        source.Add(Resources.MSS_Client_Structures_BuildingNrIncompleteErrorToolTip);
        this.IsValid = false;
      }
      if (!this.LocationNumberList.Contains(this.BuildingNumberValue))
        return source;
      source.Add(Resources.MSS_LOCATIONS_UNIQUE_LOCATION_BUILDING_NUMBER);
      this.IsValid = false;
      return source;
    }

    private static string ConcatenatedDescriptionValue(string city, string street, string zip)
    {
      string str = string.Empty;
      if (city.Trim() != string.Empty)
        str = str + city.Trim() + " , ";
      if (street.Trim() != string.Empty)
        str = str + street.Trim() + " , ";
      if (zip.Trim() != string.Empty)
        str = str + zip.Trim() + " , ";
      return str.Length > 0 ? str.Remove(str.Length - 2, 2) : str;
    }
  }
}
