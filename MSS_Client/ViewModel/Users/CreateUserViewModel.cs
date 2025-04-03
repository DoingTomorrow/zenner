// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.CreateUserViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Languages;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.UsersManagement;
using MSS.Core.Utils;
using MSS.DTO.Users;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
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
  internal class CreateUserViewModel : ValidationViewModelBase
  {
    private string _firstNameTextValue = string.Empty;
    private string _lastNameTextValue = string.Empty;
    private string _usernameTextValue = string.Empty;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IRepository<Country> _countryRepository;
    private ObservableCollection<Language> _languages;
    private string _officeTextValue;
    private Language _selectedLanguage;

    [Inject]
    public CreateUserViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._countryRepository = repositoryFactory.GetRepository<Country>();
      this.RoleList = new ObservableCollection<RoleDTO>();
      User loggedUser = MSS.Business.Utils.AppContext.Current.LoggedUser;
      Country country = loggedUser.Country;
      this.SelectedCountryId = country != null ? country.Id : Guid.Empty;
      this.SelectedLanguage = this.LanguageList.FirstOrDefault<Language>();
      this.OfficeTextValue = loggedUser.Office;
      this.InvalidPassword = true;
    }

    [RequiredCollection("MSS_Client_UserControl_Dialog_RoleErrorToolTip")]
    public ObservableCollection<RoleDTO> RoleList { get; set; }

    public RoleDTO Role
    {
      set => this.OnPropertyChanged("RoleList");
    }

    public ObservableCollection<RoleDTO> GetRoles
    {
      get
      {
        ObservableCollection<RoleDTO> getRoles = new ObservableCollection<RoleDTO>();
        foreach (RoleDTO roleDto in this.GetRoleManagerInstance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default"))))
          getRoles.Add(roleDto);
        return getRoles;
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

    public bool InvalidPassword { get; set; }

    public string OfficeTextValue
    {
      get => this._officeTextValue;
      set
      {
        this._officeTextValue = value;
        this.OnPropertyChanged(nameof (OfficeTextValue));
      }
    }

    public Guid RoleId { get; set; }

    public Language SelectedLanguage
    {
      get => this._selectedLanguage;
      set
      {
        if (value != null)
          this._selectedLanguage = value;
        this.OnPropertyChanged(nameof (SelectedLanguage));
      }
    }

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

    public ICommand AddUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.InvalidPassword)
            return;
          PasswordBox passwordBox = _ as PasswordBox;
          UsersManager userManagerInstance = this.GetUserManagerInstance();
          this.ValidateProperty("UsernameTextValue");
          if (!this.IsValid)
            return;
          userManagerInstance.CreateUser(new UserEditDTO()
          {
            FirstName = this._firstNameTextValue,
            LastName = this._lastNameTextValue,
            Username = this._usernameTextValue,
            Office = this._officeTextValue,
            CountryId = this.SelectedCountryId,
            Language = this.SelectedLanguage.Name.ToString()
          }, this.RoleList, passwordBox?.Password);
          this.OnRequestClose(true);
        }));
      }
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.UsernameTextValue));
      if (propertyName != propertyName1)
        return new List<string>();
      ICollection<string> validationErrors;
      this.GetUserManagerInstance().ValidateUsername(this.UsernameTextValue, out validationErrors);
      return validationErrors.ToList<string>();
    }

    public new string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      return propertyExpression.Body is MemberExpression body ? body.Member.Name : (string) null;
    }
  }
}
