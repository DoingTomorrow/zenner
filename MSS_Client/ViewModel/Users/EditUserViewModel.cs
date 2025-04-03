// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.EditUserViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.Events;
using MSS.Business.Languages;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.UsersManagement;
using MSS.Core.Utils;
using MSS.DTO.Users;
using MSS.Interfaces;
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
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  internal class EditUserViewModel : ValidationViewModelBase
  {
    private readonly Guid _userId;
    private string _firstNameTextValue = string.Empty;
    private string _lastNameTextValue = string.Empty;
    private string _usernameTextValue = string.Empty;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Country> _countryRepository;
    private ObservableCollection<RoleDTO> _roleCollection;
    private string _officeTextValue;
    private ObservableCollection<Language> _languages;

    [Inject]
    public EditUserViewModel(UserDTO um, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._countryRepository = repositoryFactory.GetRepository<Country>();
      this._userId = um.Id;
      this.FirstNameTextValue = um.FirstName;
      this.LastNameTextValue = um.LastName;
      this.UsernameTextValue = um.Username;
      this.OfficeTextValue = um.Office;
      this.SelectedCountryId = um.CountryId;
      this.SelectedLanguage = this.LanguageList.FirstOrDefault<Language>((Func<Language, bool>) (l => l.Name.ToString() == um.Language));
      this.InvalidPassword = false;
      IList<UserRole> userRoleList = repositoryFactory.GetRepository<UserRole>().SearchFor((Expression<Func<UserRole, bool>>) (ur => ur.User.Id == um.Id));
      ObservableCollection<MSS.Core.Model.UsersManagement.Role> rolsList = new ObservableCollection<MSS.Core.Model.UsersManagement.Role>();
      TypeHelperExtensionMethods.ForEach<UserRole>((IEnumerable<UserRole>) userRoleList, (Action<UserRole>) (ur => rolsList.Add(ur.Role)));
      Mapper.CreateMap<MSS.Core.Model.UsersManagement.Role, RoleDTO>();
      this.RoleList = new ObservableCollection<RoleDTO>();
      this.RoleList = Mapper.Map<ObservableCollection<MSS.Core.Model.UsersManagement.Role>, ObservableCollection<RoleDTO>>(rolsList);
      this.RoleCollection = this.RolesCollection();
    }

    private ObservableCollection<RoleDTO> RolesCollection()
    {
      IEnumerable<RoleDTO> roleDtos = this.GetRoles.Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default", StringComparison.CurrentCulture)));
      ObservableCollection<RoleDTO> observableCollection = new ObservableCollection<RoleDTO>();
      foreach (RoleDTO roleDto in roleDtos)
        observableCollection.Add(roleDto);
      return observableCollection;
    }

    [RequiredCollection("MSS_Client_UserControl_Dialog_RoleErrorToolTip")]
    public ObservableCollection<RoleDTO> RoleList { get; set; }

    public RoleDTO Role
    {
      set => this.OnPropertyChanged("RoleList");
    }

    public ObservableCollection<RoleDTO> RoleCollection
    {
      get => this._roleCollection;
      set
      {
        this._roleCollection = this.RolesCollection();
        this.OnPropertyChanged(nameof (RoleCollection));
      }
    }

    public IEnumerable<RoleDTO> GetRoles
    {
      get
      {
        return (IEnumerable<RoleDTO>) this.GetRoleManagerInsance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !this.RoleList.Select<RoleDTO, Guid>((Func<RoleDTO, Guid>) (y => y.Id)).ToList<Guid>().Contains(x.Id))).ToList<RoleDTO>();
      }
    }

    [Required(ErrorMessage = "MSS_Client_UserControl_Dialog_FirstNameValidationMessage")]
    public string FirstNameTextValue
    {
      get => this._firstNameTextValue;
      set
      {
        this._firstNameTextValue = value;
        this.OnPropertyChanged(nameof (FirstNameTextValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_UserControl_Dialog_LastNameValidationMessage")]
    public string LastNameTextValue
    {
      get => this._lastNameTextValue;
      set
      {
        this._lastNameTextValue = value;
        this.OnPropertyChanged(nameof (LastNameTextValue));
      }
    }

    [Required(ErrorMessage = "MSS_Client_UserControl_Dialog_UsernameValidationMessage")]
    public string UsernameTextValue
    {
      get => this._usernameTextValue;
      set
      {
        this._usernameTextValue = value;
        this.OnPropertyChanged(nameof (UsernameTextValue));
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

    public string OfficeTextValue
    {
      get => this._officeTextValue;
      set
      {
        this._officeTextValue = value;
        this.OnPropertyChanged(nameof (OfficeTextValue));
      }
    }

    public Language SelectedLanguage { get; set; }

    public bool InvalidPassword { get; set; }

    public ObservableCollection<Language> LanguageList
    {
      get
      {
        if (this._languages != null)
          return this._languages;
        ObservableCollection<Language> observableCollection = new ObservableCollection<Language>();
        observableCollection.Add(new Language(LangEnum.English, "pack://application:,,,/Styles;component/Images/Universal/english.png"));
        observableCollection.Add(new Language(LangEnum.German, "pack://application:,,,/Styles;component/Images/Universal/german.png"));
        this._languages = observableCollection;
        return this._languages;
      }
    }

    public ICommand EditUserInfoCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (this.InvalidPassword || !this.RoleList.Any<RoleDTO>())
            return;
          PasswordBox passwordBox = parameter as PasswordBox;
          UsersManager userManagerInstance = this.GetUserManagerInstance();
          if (!this.IsValid)
            return;
          userManagerInstance.EditUser(new UserEditDTO()
          {
            Id = this._userId,
            FirstName = this._firstNameTextValue,
            LastName = this._lastNameTextValue,
            Username = this._usernameTextValue,
            Office = this._officeTextValue,
            CountryId = this.SelectedCountryId,
            Language = this.SelectedLanguage.Name.ToString()
          }, this.RoleList, passwordBox?.Password);
          EventPublisher.Publish<GridShouldBeUpdated>(new GridShouldBeUpdated(), (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);

    private RoleManager GetRoleManagerInsance() => new RoleManager(this._repositoryFactory);
  }
}
