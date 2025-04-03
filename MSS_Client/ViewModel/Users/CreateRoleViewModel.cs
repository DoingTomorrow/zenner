// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Users.CreateRoleViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace MSS_Client.ViewModel.Users
{
  public class CreateRoleViewModel : ValidationViewModelBase
  {
    private Visibility _roleNameErrorImageVisibility = Visibility.Hidden;
    private string _roleName = string.Empty;
    private Brush _roleNameBrushValue = (Brush) Brushes.LightGray;
    private readonly IRepositoryFactory _repositoryFactory;
    private ObservableCollection<OperationDTO> _roleOperationList;
    private OperationDTO _roleOperation;

    [Inject]
    public CreateRoleViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this.RoleOperationList = new ObservableCollection<OperationDTO>();
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

    public OperationDTO RoleOperation
    {
      set
      {
        this._roleOperation = value;
        this.OnPropertyChanged("RoleOperationList");
      }
    }

    public ObservableCollection<OperationDTO> GetOperations
    {
      get
      {
        IEnumerable<OperationDTO> operationsDto = this.GetRoleManagerInstance().GetOperationsDTO();
        ObservableCollection<OperationDTO> getOperations = new ObservableCollection<OperationDTO>();
        foreach (OperationDTO operationDto in operationsDto)
          getOperations.Add(operationDto);
        return getOperations;
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

    public Visibility RoleNameErrorImageVisibility
    {
      get => this._roleNameErrorImageVisibility;
      set
      {
        this._roleNameErrorImageVisibility = value;
        this.OnPropertyChanged(nameof (RoleNameErrorImageVisibility));
      }
    }

    public Brush RoleNameBrushValue
    {
      get => this._roleNameBrushValue;
      set
      {
        this._roleNameBrushValue = value;
        this.OnPropertyChanged(nameof (RoleNameBrushValue));
      }
    }

    public ICommand AddRoleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.ValidateProperty("RoleName");
          if (!this.IsValid)
            return;
          this.GetRoleManagerInstance().CreateRole(new RoleDTO()
          {
            IsStandard = false,
            Name = this.RoleName
          }, this.RoleOperationList.ToEnumerable<OperationDTO>());
          this.OnRequestClose(true);
        });
      }
    }

    private RoleManager GetRoleManagerInstance() => new RoleManager(this._repositoryFactory);

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
