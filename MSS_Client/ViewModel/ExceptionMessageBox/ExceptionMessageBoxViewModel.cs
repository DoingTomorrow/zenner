// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.ExceptionMessageBox.ExceptionMessageBoxViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.ExceptionMessageBox
{
  public class ExceptionMessageBoxViewModel : ViewModelBase
  {
    public ExceptionMessageBoxViewModel()
    {
      this.ShowButtonYes = false;
      this.ShowButtonOK = false;
      this.ShowButtonNo = false;
    }

    public ExceptionMessageBoxViewModel(string errorMessage, bool isUnhandledException)
    {
      this.GetErrorMessage = errorMessage;
      this.ConfirmationText = isUnhandledException ? CultureResources.GetValue("MSS_Client_UnhandledException_Confirmation_Message") : CultureResources.GetValue("MSS_Client_Exception_Confirmation_Message");
      this.ExceptionDialogTitle = isUnhandledException ? CultureResources.GetValue("MSS_Client_UnhandledException_Title") : CultureResources.GetValue("MSS_Client_Exception_Title");
      if (isUnhandledException)
      {
        this.ShowButtonOK = true;
        this.ShowButtonYes = false;
        this.ShowButtonNo = false;
      }
      else
      {
        this.ShowButtonOK = false;
        this.ShowButtonYes = true;
        this.ShowButtonNo = true;
      }
      this.IsCustomMessageDialog = false;
    }

    public ExceptionMessageBoxViewModel(string messageTitle, string messageBody)
    {
      this.GetErrorMessage = messageBody;
      this.ExceptionDialogTitle = messageTitle;
      this.ShowButtonOK = false;
      this.ShowButtonYes = true;
      this.ShowButtonNo = true;
      this.IsYesButtonClicked = false;
      this.IsCustomMessageDialog = true;
    }

    public string ConfirmationText { get; set; }

    public string ExceptionDialogTitle { get; set; }

    public string GetErrorMessage { get; set; }

    public bool ShowButtonNo { get; set; }

    public bool ShowButtonOK { get; set; }

    public bool ShowButtonYes { get; set; }

    public ICommand ContinueCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsYesButtonClicked |= this.IsCustomMessageDialog;
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => this.OnRequestClose(false)));
    }

    private bool IsCustomMessageDialog { get; }

    public bool IsYesButtonClicked { get; set; }
  }
}
