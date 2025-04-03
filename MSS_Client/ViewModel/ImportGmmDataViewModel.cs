// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.ImportGmmDataViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Core.Model.ApplicationParamenters;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel
{
  public class ImportGmmDataViewModel : ViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private bool _isDontShowAgainChecked;

    public ImportGmmDataViewModel(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
    }

    public bool IsDontShowAgainChecked
    {
      get => this._isDontShowAgainChecked;
      set
      {
        this._isDontShowAgainChecked = value;
        this.OnPropertyChanged(nameof (IsDontShowAgainChecked));
      }
    }

    public ICommand YesButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (this.IsDontShowAgainChecked)
          {
            MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
            ApplicationParameter appParam = parametersManagement.GetAppParam("DoNotShowGmmImportScreenAtStartup");
            appParam.Value = "true";
            parametersManagement.Update(appParam);
          }
          this.OnRequestClose(true);
        });
      }
    }

    public ICommand NoButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (this.IsDontShowAgainChecked)
          {
            MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
            ApplicationParameter appParam = parametersManagement.GetAppParam("DoNotShowGmmImportScreenAtStartup");
            appParam.Value = "true";
            parametersManagement.Update(appParam);
          }
          this.OnRequestClose(false);
        });
      }
    }
  }
}
