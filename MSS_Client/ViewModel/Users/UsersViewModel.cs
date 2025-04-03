// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.UsersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MSS_Client.Utils.Virtualization;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  public class UsersViewModel : ValidationViewModelBase
  {
    private int _pageSize;
    private readonly IWindowFactory _windowFactory;
    private readonly IRepositoryFactory _repositoryFactory;
    private int _selectedIndex;
    private IEnumerable<UserDTO> _getUsers;
    private bool _isUserTabSelected;
    private IEnumerable<RoleDTO> _getRoles;
    private bool _isUserRolesTabSelected;
    private bool _createRoleVisibility;
    private bool _editRoleVisibility;
    private bool _deleteRoleVisibility;
    private bool _createUserVisibility;
    private bool _editUserVisibility;
    private bool _deleteUserVisibility;
    private ViewModelBase _messageUserControlUsers;
    private ViewModelBase _messageUserControlRoles;

    [Inject]
    public UsersViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._pageSize = Convert.ToInt32(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize)));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<ActionSearch<UserDTO>>(new Action<ActionSearch<UserDTO>>(this.RefreshUsersAfterSearch));
      EventPublisher.Register<ActionSearch<RoleDTO>>(new Action<ActionSearch<RoleDTO>>(this.RefreshRolesAfterSearch));
      EventPublisher.Register<GridShouldBeUpdated>(new Action<GridShouldBeUpdated>(this.RefreshUsersAndRolesAfterSync));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      EventPublisher.Register<DeleteEntityEvent>(new Action<DeleteEntityEvent>(this.DeleteEntityEventHandler));
      this._getUsers = this.GetUserManagerInstance().GetUsersDTO().Where<UserDTO>((Func<UserDTO, bool>) (x => !x.Username.StartsWith("default")));
      this._getRoles = this.GetRoleManagerInstance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default")));
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      this.CreateRoleVisibility = usersManager1.HasRight(OperationEnum.UserRoleCreate.ToString());
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.UserRoleEdit;
      string operation1 = operationEnum.ToString();
      this.EditRoleVisibility = usersManager2.HasRight(operation1);
      UsersManager usersManager3 = usersManager1;
      operationEnum = OperationEnum.UserRoleDelete;
      string operation2 = operationEnum.ToString();
      this.DeleteRoleVisibility = usersManager3.HasRight(operation2);
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.UserCreate;
      string operation3 = operationEnum.ToString();
      this.CreateUserVisibility = usersManager4.HasRight(operation3);
      UsersManager usersManager5 = usersManager1;
      operationEnum = OperationEnum.UserEdit;
      string operation4 = operationEnum.ToString();
      this.EditUserVisibility = usersManager5.HasRight(operation4);
      UsersManager usersManager6 = usersManager1;
      operationEnum = OperationEnum.UserDelete;
      string operation5 = operationEnum.ToString();
      this.DeleteUserVisibility = usersManager6.HasRight(operation5);
    }

    public int SelectedIndex
    {
      get => this._selectedIndex;
      set
      {
        this._selectedIndex = value;
        this.OnPropertyChanged(nameof (SelectedIndex));
      }
    }

    public int PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.UsersUsers:
          this.SelectedIndex = 0;
          break;
        case ApplicationTabsEnum.UsersRoles:
          this.SelectedIndex = 1;
          break;
      }
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      ViewModelBase viewModelBase = (ViewModelBase) null;
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          viewModelBase = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          viewModelBase = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
      if (this.IsUserTabSelected)
        this.MessageUserControlUsers = viewModelBase;
      if (!this.IsUserRolesTabSelected)
        return;
      this.MessageUserControlUsers = viewModelBase;
    }

    private void RefreshUsersAfterSearch(ActionSearch<UserDTO> update)
    {
      bool flag = update.Message == null;
      this._getUsers = update.ObservableCollection == null || update.ObservableCollection.Count != 0 ? (IEnumerable<UserDTO>) update.ObservableCollection : this.GetUsers;
      this.OnPropertyChanged("GetUsers");
      if (flag)
        return;
      if (this.IsUserTabSelected)
        this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (!this.IsUserRolesTabSelected)
        return;
      this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
    }

    private void RefreshRolesAfterSearch(ActionSearch<RoleDTO> update)
    {
      bool flag = update.Message == null;
      this._getRoles = update.ObservableCollection.Count == 0 ? this.GetRoles : (IEnumerable<RoleDTO>) update.ObservableCollection;
      this.OnPropertyChanged("GetRoles");
      if (flag)
        return;
      if (this.IsUserTabSelected)
        this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (!this.IsUserRolesTabSelected)
        return;
      this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
    }

    private void RefreshUsersAndRolesAfterSync(GridShouldBeUpdated args)
    {
      this._repositoryFactory.GetSession().Clear();
      this._getUsers = this.GetUserManagerInstance().GetUsersDTO().Where<UserDTO>((Func<UserDTO, bool>) (x => !x.Username.StartsWith("default")));
      this._getRoles = this.GetRoleManagerInstance().GetRolesDTO();
      this.OnPropertyChanged("GetUsers");
      this.OnPropertyChanged("GetRoles");
    }

    private void DeleteEntityEventHandler(DeleteEntityEvent args)
    {
      if (!args.WasEntityDeleted || !(args.Type == typeof (Role)))
        return;
      IRepository<Role> repository = this._repositoryFactory.GetRepository<Role>();
      if (args.ObjectToDelete is RoleDTO objectToDelete)
        repository.Refresh((object) objectToDelete.Id);
      this.MessageUserControlRoles = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
    }

    public IEnumerable<UserDTO> GetUsers
    {
      get => this._getUsers;
      set
      {
        this._getUsers = value;
        this.OnPropertyChanged(nameof (GetUsers));
      }
    }

    public ICommand CreateUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateUserViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.MessageUserControlUsers = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
            this._getUsers = this.GetUserManagerInstance().GetUsersDTO().Where<UserDTO>((Func<UserDTO, bool>) (x => !x.Username.StartsWith("default")));
            this.OnPropertyChanged("GetUsers");
          }
          else
            this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand EditUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          UserDTO userDto = parameter as UserDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditUserViewModel>((IParameter) new ConstructorArgument("um", (object) userDto)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this._repositoryFactory.GetUserRepository().Refresh((object) userDto.Id);
            this.MessageUserControlUsers = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          else
            this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand DeleteUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          if (!(param is UserDTO user2))
            return;
          if (user2.Id == MSS.Business.Utils.AppContext.Current.LoggedUser.Id)
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_Users_CannotDeleteCurrentlyLoggedInUser), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          else if (new UsersManager(this._repositoryFactory).IsLastSuperuser(user2))
          {
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_Users_CannotDeleteLastAdmin), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          }
          else
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteUserViewModel>((IParameter) new ConstructorArgument("um", (object) user2)));
            if (newModalDialog.HasValue && newModalDialog.Value)
            {
              this._repositoryFactory.GetUserRepository().Refresh((object) user2.Id);
              this.MessageUserControlUsers = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
              this._getUsers = this.GetUserManagerInstance().GetUsersDTO().Where<UserDTO>((Func<UserDTO, bool>) (x => !x.Username.StartsWith("default", StringComparison.CurrentCulture)));
              this.OnPropertyChanged("GetUsers");
            }
            else
              this.MessageUserControlUsers = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          }
        }));
      }
    }

    private UsersManager GetUserManagerInstance() => new UsersManager(this._repositoryFactory);

    private VirtualizationHelper GetVirtualizationHelperInstance()
    {
      return new VirtualizationHelper(this._repositoryFactory);
    }

    public bool IsUserTabSelected
    {
      get => this._isUserTabSelected;
      set
      {
        this._isUserTabSelected = value;
        if (!this._isUserTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.UsersUsers
        }, (IViewModel) this);
      }
    }

    public IEnumerable<RoleDTO> GetRoles
    {
      get => this._getRoles;
      set
      {
        this._getRoles = value;
        this.OnPropertyChanged(nameof (GetRoles));
      }
    }

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);

    public ICommand CreateRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateRoleViewModel>());
          this.MessageUserControlRoles = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getRoles = this.GetRoleManagerInstance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default", StringComparison.CurrentCulture)));
          this.OnPropertyChanged("GetRoles");
        }));
      }
    }

    public ICommand EditRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RoleDTO roleDto = parameter as RoleDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditRoleViewModel>((IParameter) new ConstructorArgument("role", (object) roleDto)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            IRepository<Role> repository = this._repositoryFactory.GetRepository<Role>();
            if (roleDto != null)
              repository.Refresh((object) roleDto.Id);
            this.MessageUserControlRoles = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          else
            this.MessageUserControlRoles = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this._getRoles = this.GetRoleManagerInstance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default", StringComparison.CurrentCulture)));
          this.OnPropertyChanged("GetRoles");
        }));
      }
    }

    public ICommand DeleteRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RoleDTO roleDto = parameter as RoleDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteRoleViewModel>((IParameter) new ConstructorArgument("role", (object) roleDto)));
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            this.MessageUserControlRoles = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this._getRoles = this.GetRoleManagerInstance().GetRolesDTO().Where<RoleDTO>((Func<RoleDTO, bool>) (x => !x.Name.StartsWith("default", StringComparison.CurrentCulture)));
          this.OnPropertyChanged("GetRoles");
        }));
      }
    }

    public ICommand SeeRolePermissionsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RoleDTO roleDto = parameter as RoleDTO;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewRolePermissionsViewModel>((IParameter) new ConstructorArgument("role", (object) roleDto)));
        }));
      }
    }

    public bool IsUserRolesTabSelected
    {
      get => this._isUserRolesTabSelected;
      set
      {
        this._isUserRolesTabSelected = value;
        if (!this._isUserRolesTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.UsersRoles
        }, (IViewModel) this);
      }
    }

    public bool CreateRoleVisibility
    {
      get => this._createRoleVisibility;
      set
      {
        this._createRoleVisibility = value;
        this.OnPropertyChanged(nameof (CreateRoleVisibility));
      }
    }

    public bool EditRoleVisibility
    {
      get => this._editRoleVisibility;
      set
      {
        this._editRoleVisibility = value;
        this.OnPropertyChanged(nameof (EditRoleVisibility));
      }
    }

    public bool DeleteRoleVisibility
    {
      get => this._deleteRoleVisibility;
      set
      {
        this._deleteRoleVisibility = value;
        this.OnPropertyChanged(nameof (DeleteRoleVisibility));
      }
    }

    public bool CreateUserVisibility
    {
      get => this._createUserVisibility;
      set
      {
        this._createUserVisibility = value;
        this.OnPropertyChanged(nameof (CreateUserVisibility));
      }
    }

    public bool EditUserVisibility
    {
      get => this._editUserVisibility;
      set
      {
        this._editUserVisibility = value;
        this.OnPropertyChanged(nameof (EditUserVisibility));
      }
    }

    public bool DeleteUserVisibility
    {
      get => this._deleteUserVisibility;
      set
      {
        this._deleteUserVisibility = value;
        this.OnPropertyChanged(nameof (DeleteUserVisibility));
      }
    }

    public ViewModelBase MessageUserControlUsers
    {
      get => this._messageUserControlUsers;
      set
      {
        this._messageUserControlUsers = value;
        this.OnPropertyChanged(nameof (MessageUserControlUsers));
      }
    }

    public ViewModelBase MessageUserControlRoles
    {
      get => this._messageUserControlRoles;
      set
      {
        this._messageUserControlRoles = value;
        this.OnPropertyChanged(nameof (MessageUserControlRoles));
      }
    }
  }
}
