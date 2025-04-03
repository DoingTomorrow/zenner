// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.ConfigChangeableParamsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.ViewModel.Settings.Selector;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  public class ConfigChangeableParamsViewModel : ValidationViewModelBase
  {
    private EquipmentSelector _equipmentSelector;
    private readonly IRepositoryFactory _repositoryFactory;

    public EquipmentSelector EquipmentSelectorProperty
    {
      get => this._equipmentSelector;
      set
      {
        this._equipmentSelector = value;
        this.OnPropertyChanged(nameof (EquipmentSelectorProperty));
      }
    }

    [Inject]
    public ConfigChangeableParamsViewModel(
      EquipmentModel equipmentModel,
      IRepositoryFactory repositoryFactory)
    {
      this.ChangeDefaultEquipmentTitle = Resources.MSS_Client_Change_Default_Equipment;
      this._repositoryFactory = repositoryFactory;
      this._equipmentSelector = new EquipmentSelector(equipmentModel);
    }

    public string ChangeDefaultEquipmentTitle { get; set; }

    public ICommand SaveDefaultEquipmentCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (this.EquipmentSelectorProperty.SelectedEquipmentModel == null)
            return;
          MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
          Task<List<Config>> equipmentConfigsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.CreateEquipmentConfigsList(this.EquipmentSelectorProperty.SelectedEquipmentModel);
          parametersManagement.UpdateDefaultEquipment(this.EquipmentSelectorProperty.SelectedEquipmentModel, equipmentConfigsList.Result);
          EventPublisher.Publish<UpdateDefaultEquipment>(new UpdateDefaultEquipment()
          {
            SelectedEquipmentModel = this.EquipmentSelectorProperty.SelectedEquipmentModel,
            ChangeableParameters = equipmentConfigsList.Result
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
