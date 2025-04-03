// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.ViewRolePermissionsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  public class ViewRolePermissionsViewModel : ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private List<OperationDTO> _operations;
    private int _pageSize;

    [Inject]
    public ViewRolePermissionsViewModel(RoleDTO role, IRepositoryFactory repositoryFactory)
    {
      this.Title = Resources.MSS_Client_Permissions + " " + role.Name;
      this._pageSize = Convert.ToInt32(MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize)));
      this._repositoryFactory = repositoryFactory;
      this.Operations = this.GetOperationsForRole(role.Id).Select<Operation, OperationDTO>((Func<Operation, OperationDTO>) (o => new OperationDTO()
      {
        Id = o.Id,
        Name = o.Name,
        Description = Resources.ResourceManager.GetString(o.Name)
      })).ToList<OperationDTO>();
    }

    public List<OperationDTO> Operations
    {
      get => this._operations;
      set
      {
        this._operations = value;
        this.OnPropertyChanged(nameof (Operations));
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

    public string Title { get; set; }

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);

    private List<Operation> GetOperationsForRole(Guid roleId)
    {
      return this.GetRoleManagerInstance().GetRoleOperations().Where<RoleOperation>((Func<RoleOperation, bool>) (ro => ro.Role.Id == roleId)).Select<RoleOperation, Operation>((Func<RoleOperation, Operation>) (ro => ro.Operation)).ToList<Operation>();
    }
  }
}
