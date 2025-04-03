// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.GenericProgressDialog.DeterminateProgressDialogViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.GenericProgressDialog
{
  public class DeterminateProgressDialogViewModel : ViewModelBase
  {
    private int _progressBarValue;
    private bool _areCancelButtonsVisible;

    public DeterminateProgressDialogViewModel(
      string progressDialogTitle,
      string progressDialogMessage,
      bool areCancelButtonsVisible = true)
    {
      this.ProgressDialogTitle = progressDialogTitle;
      this.ProgressDialogMessage = progressDialogMessage;
      EventPublisher.Register<MSS.Business.Events.ProgressBarValueChanged>(new Action<MSS.Business.Events.ProgressBarValueChanged>(this.ProgressBarValueChanged));
    }

    public string ProgressDialogTitle { get; set; }

    public string ProgressDialogMessage { get; set; }

    public int ProgressBarValue
    {
      get => this._progressBarValue;
      set
      {
        this._progressBarValue = value;
        this.OnPropertyChanged(nameof (ProgressBarValue));
      }
    }

    public bool AreCancelButtonsVisible
    {
      get => this._areCancelButtonsVisible;
      set
      {
        this._areCancelButtonsVisible = value;
        this.OnPropertyChanged(nameof (AreCancelButtonsVisible));
      }
    }

    public ICommand CancelButtonCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.OnRequestCancel()));
    }

    private void ProgressBarValueChanged(MSS.Business.Events.ProgressBarValueChanged update)
    {
      this.ProgressBarValue = update.Value;
    }
  }
}
