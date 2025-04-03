// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.EditRoleViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Utils;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  internal class EditRoleViewModel : ValidationViewModelBase
  {
    private readonly Guid _roleId;
    private ObservableCollection<OperationDTO> _operationsList;
    private ObservableCollection<OperationDTO> _roleOperationList = new ObservableCollection<OperationDTO>();
    private readonly IRepositoryFactory _repositoryFactory;
    private string _roleName;
    private OperationDTO _roleOperation;

    [Inject]
    public EditRoleViewModel(RoleDTO role, IRepositoryFactory repositoryFactory)
    {
      this._roleId = role.Id;
      this._repositoryFactory = repositoryFactory;
      this.RoleName = role.Name;
      IEnumerable<MSS.Core.Model.UsersManagement.RoleOperation> roleOperations = this.GetRoleManagerInstance().GetRoleOperations();
      this._operationsList = this.OpCollection();
      foreach (MSS.Core.Model.UsersManagement.RoleOperation roleOperation1 in roleOperations)
      {
        MSS.Core.Model.UsersManagement.RoleOperation roleOperation = roleOperation1;
        if (!(roleOperation.Role.Id != role.Id))
        {
          OperationDTO operationDto = this.OperationsList.FirstOrDefault<OperationDTO>((Func<OperationDTO, bool>) (op => op.Id == roleOperation.Operation.Id));
          this.RoleOperationList.Add(operationDto);
          this.OperationsList.Remove(operationDto);
        }
      }
    }

    [Required(ErrorMessage = "MSS_Client_UserControl_RoleNameValidation")]
    public string RoleName
    {
      get => this._roleName;
      set
      {
        this._roleName = value;
        this.OnPropertyChanged(nameof (RoleName));
      }
    }

    [RequiredCollection("MSS_Client_UserControl_RoleOperationValidation")]
    public ObservableCollection<OperationDTO> RoleOperationList
    {
      get => this._roleOperationList;
      set
      {
        this._roleOperationList = value;
        this.OnPropertyChanged(nameof (RoleOperationList));
      }
    }

    public ObservableCollection<OperationDTO> OperationsList
    {
      get => this._operationsList;
      set
      {
        this._operationsList = value;
        this.OnPropertyChanged(nameof (OperationsList));
      }
    }

    public OperationDTO RoleOperation
    {
      get => this._roleOperation;
      set
      {
        this._roleOperation = value;
        this.OnPropertyChanged("RoleOperationList");
      }
    }

    public ICommand EditRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (!this.IsValid)
            return;
          this.GetRoleManagerInstance().EditRole(new RoleDTO()
          {
            Id = this._roleId,
            IsStandard = false,
            Name = this.RoleName
          }, this.RoleOperationList.ToEnumerable<OperationDTO>());
          EventPublisher.Publish<GridShouldBeUpdated>(new GridShouldBeUpdated(), (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);

    private ObservableCollection<OperationDTO> OpCollection()
    {
      IEnumerable<OperationDTO> operationsDto = this.GetRoleManagerInstance().GetOperationsDTO();
      ObservableCollection<OperationDTO> observableCollection = new ObservableCollection<OperationDTO>();
      foreach (OperationDTO operationDto in operationsDto)
        observableCollection.Add(operationDto);
      return observableCollection;
    }

    public bool ValidateRoleName(string rolename, out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (rolename.StartsWith("default", StringComparison.CurrentCulture))
        validationErrors.Add(Resources.MSS_Client_UserControl_Dialog_Username_Default);
      return validationErrors.Count == 0;
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.RoleName));
      if (!(propertyName == propertyName1))
        return new List<string>();
      ICollection<string> validationErrors;
      this.ValidateRoleName(this.RoleName, out validationErrors);
      this.IsValid &= !validationErrors.Any<string>();
      return validationErrors.ToList<string>();
    }
  }
}
