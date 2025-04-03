// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.GenericMessageConfirmationViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Interfaces;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel
{
  public abstract class GenericMessageConfirmationViewModel : ViewModelBase
  {
    public string DialogTitle { get; set; }

    public string DialogMessage { get; set; }

    public string OKButtonLabel { get; set; }

    public string CancelButtonLabel { get; set; }

    public abstract bool ExecuteOkButtonAction();

    public ICommand OKButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          bool flag = this.ExecuteOkButtonAction();
          EventPublisher.Publish<ActionFinishedBooleanResponse>(new ActionFinishedBooleanResponse()
          {
            IsSuccessfull = flag
          }, (IViewModel) this);
          this.OnRequestClose(true);
        });
      }
    }
  }
}
