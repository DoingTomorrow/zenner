// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Configuration.DeviceModelChangeableParametersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Configuration
{
  public class DeviceModelChangeableParametersViewModel : ViewModelBase
  {
    private DeviceModel _selectedDeviceModel;

    public DeviceModelChangeableParametersViewModel(DeviceModel selectedDeviceModel)
    {
      this.SelectedDeviceModel = selectedDeviceModel;
      this.ChangeableParametersDynamicGridTag = selectedDeviceModel?.ChangeableParameters != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(selectedDeviceModel?.ChangeableParameters) : new List<Config>();
    }

    public DeviceModel SelectedDeviceModel
    {
      get => this._selectedDeviceModel;
      set
      {
        this._selectedDeviceModel = value;
        this.OnPropertyChanged(nameof (SelectedDeviceModel));
      }
    }

    public List<Config> ChangeableParametersDynamicGridTag { get; set; }

    public ICommand SaveChangeableParametersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.SelectedDeviceModel.ChangeableParameters = GMMHelper.ReplaceValuesInChangeableParameters(this.SelectedDeviceModel.ChangeableParameters, this.ChangeableParametersDynamicGridTag);
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
