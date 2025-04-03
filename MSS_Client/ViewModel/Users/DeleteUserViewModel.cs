// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.DeleteUserViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  internal class DeleteUserViewModel : ViewModelBase, IViewModel
  {
    private readonly Guid _userId;
    private string _firstNameTextValue = string.Empty;
    private string _lastNameTextValue = string.Empty;
    private string _usernameTextValue = string.Empty;
    private readonly IRepositoryFactory _repositoryFactory;

    [Inject]
    public DeleteUserViewModel(UserDTO um, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._userId = um.Id;
      this.FirstNameTextValue = um.FirstName;
      this.LastNameTextValue = um.LastName;
      this.UsernameTextValue = um.Username;
    }

    public IEnumerable<RoleDTO> GetRoles => this.GetRoleManagerInstance().GetRolesDTO();

    public string FirstNameTextValue
    {
      get => this._firstNameTextValue;
      set => this._firstNameTextValue = value;
    }

    public string LastNameTextValue
    {
      get => this._lastNameTextValue;
      set => this._lastNameTextValue = value;
    }

    public string UsernameTextValue
    {
      get => this._usernameTextValue;
      set => this._usernameTextValue = value;
    }

    public string RoleName => this.GetUserManagerInstance().GetUserRoles(this._userId);

    public ICommand DeleteUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this.GetUserManagerInstance().DeleteUser(this._userId);
          this.OnRequestClose(true);
        }));
      }
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);
  }
}
