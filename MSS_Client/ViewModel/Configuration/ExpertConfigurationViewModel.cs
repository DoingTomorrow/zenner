// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Configuration.ExpertConfigurationViewModel
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
  public class ExpertConfigurationViewModel : ViewModelBase
  {
    private ConnectionAdjuster _connectionAdjuster;

    public ExpertConfigurationViewModel(ConnectionAdjuster connectionAdjuster)
    {
      this.SelectedConnectionAdjuster = connectionAdjuster;
      this.ChangeableParametersDynamicGridTag = this.SelectedConnectionAdjuster.SetupParameters != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this.SelectedConnectionAdjuster.SetupParameters) : new List<Config>();
    }

    public ConnectionAdjuster SelectedConnectionAdjuster
    {
      get => this._connectionAdjuster;
      set
      {
        this._connectionAdjuster = value;
        this.OnPropertyChanged(nameof (SelectedConnectionAdjuster));
      }
    }

    public List<Config> ChangeableParametersDynamicGridTag { get; set; }

    public ICommand SaveChangeableParametersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.SelectedConnectionAdjuster = new ConnectionAdjuster(this.SelectedConnectionAdjuster.ConnectionProfileID, this.SelectedConnectionAdjuster.Name, GMMHelper.ReplaceValuesInChangeableParameters(this.SelectedConnectionAdjuster.SetupParameters, this.ChangeableParametersDynamicGridTag));
          this.OnRequestClose(true);
        }));
      }
    }
  }
}
