// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.ProfileTypeViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  public class ProfileTypeViewModel : ViewModelBase
  {
    private List<ProfileType> _profileTypesCollection;
    private ProfileType _selectedProfileType;

    public ProfileTypeViewModel(
      Meter meter,
      ProfileType selectedProfileType,
      EquipmentModel equipment)
    {
      this.ProfileTypesCollection = GmmInterface.DeviceManager.GetProfileTypes(meter.DeviceModel, equipment);
      this.InitializeUI(selectedProfileType);
    }

    public ProfileTypeViewModel(
      IEnumerable<ProfileType> profileTypeCollection,
      ProfileType selectedProfileType)
    {
      this.ProfileTypesCollection = profileTypeCollection.ToList<ProfileType>();
      this.InitializeUI(selectedProfileType);
    }

    private void InitializeUI(ProfileType selectedProfileType)
    {
      if (selectedProfileType == null)
        return;
      this._selectedProfileType = this.ProfileTypesCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (item => item.Name == selectedProfileType.Name));
      if (this._selectedProfileType != null)
        this._selectedProfileType.ChangeableParameters = selectedProfileType.ChangeableParameters;
      this.OnPropertyChanged("SelectedProfileType");
      this.ProfileTypesDynamicGridTag = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this._selectedProfileType?.ChangeableParameters);
    }

    public List<ProfileType> ProfileTypesCollection
    {
      get => this._profileTypesCollection;
      set
      {
        this._profileTypesCollection = value;
        this.OnPropertyChanged(nameof (ProfileTypesCollection));
      }
    }

    public ProfileType SelectedProfileType
    {
      get => this._selectedProfileType;
      set
      {
        this._selectedProfileType = value;
        this.OnPropertyChanged(nameof (SelectedProfileType));
        this.UpdateDynamicGrid();
      }
    }

    public List<Config> ProfileTypesDynamicGridTag { get; set; }

    private void UpdateDynamicGrid()
    {
      List<Config> configList = (List<Config>) null;
      if (this.SelectedProfileType != null)
        configList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this.SelectedProfileType.ChangeableParameters);
      EventPublisher.Publish<ProfileTypeChangedEvent>(new ProfileTypeChangedEvent()
      {
        ProfileTypeValues = configList
      }, (IViewModel) this);
    }

    public System.Windows.Input.ICommand SaveProfileTypeCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.SelectedProfileType.ChangeableParameters = GMMHelper.ReplaceValuesInChangeableParameters(this.SelectedProfileType.ChangeableParameters, this.ProfileTypesDynamicGridTag);
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
