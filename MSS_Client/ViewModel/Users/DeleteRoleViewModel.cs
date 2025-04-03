// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.DeleteRoleViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  internal class DeleteRoleViewModel : ViewModelBase
  {
    private readonly Guid _roleId;
    private string _roleName = string.Empty;
    private ObservableCollection<OperationDTO> _roleOperationList = new ObservableCollection<OperationDTO>();
    private readonly IRepositoryFactory _repositoryFactory;
    private RoleDTO _role;

    [Inject]
    public DeleteRoleViewModel(RoleDTO role, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._role = role;
      this._roleId = role.Id;
      this.RoleName = role.Name;
      this.ConfirmRoleDeleteDialog = string.Format(Resources.MSS_Client_UserControl_Dialog_DeleteRoleDialogConfirmation, (object) this.RoleName);
      foreach (OperationDTO operationDto in this.GetRoleManagerInstance().GetRoleOperations().Where<RoleOperation>((Func<RoleOperation, bool>) (roleOperation => roleOperation.Role.Id == role.Id)).Select<RoleOperation, OperationDTO>((Func<RoleOperation, OperationDTO>) (roleOperation => new OperationDTO()
      {
        Id = roleOperation.Operation.Id,
        Name = roleOperation.Operation.Name
      })))
        this.RoleOperationList.Add(operationDto);
    }

    public string RoleName
    {
      get => this._roleName;
      set => this._roleName = value;
    }

    public string ConfirmRoleDeleteDialog { get; set; }

    public ObservableCollection<OperationDTO> RoleOperationList
    {
      get => this._roleOperationList;
      set
      {
        this._roleOperationList = value;
        this.OnPropertyChanged(nameof (RoleOperationList));
      }
    }

    public ICommand DeleteRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool flag = this.GetRoleManagerInstance().DeleteRole(this._roleId);
          EventPublisher.Publish<DeleteEntityEvent>(new DeleteEntityEvent()
          {
            WasEntityDeleted = flag,
            ObjectToDelete = (object) this._role,
            Type = typeof (Role)
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);
  }
}
