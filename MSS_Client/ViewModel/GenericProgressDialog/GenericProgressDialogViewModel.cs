// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.GenericProgressDialog.GenericProgressDialogViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.GenericProgressDialog
{
  public class GenericProgressDialogViewModel : ViewModelBase
  {
    private string _progress;

    public GenericProgressDialogViewModel(string progressDialogTitle, string progressDialogMessage)
    {
      this.ProgressDialogTitle = progressDialogTitle;
      this.ProgressDialogMessage = progressDialogMessage;
    }

    public string ProgressDialogTitle { get; set; }

    public string ProgressDialogMessage
    {
      get => this._progress;
      set
      {
        this._progress = value;
        this.OnPropertyChanged(nameof (ProgressDialogMessage));
      }
    }

    public ICommand CancelButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this.ProgressDialogMessage = Resources.EXPORT_ROLLING_BACK;
          this.OnRequestCancel();
        }));
      }
    }
  }
}
