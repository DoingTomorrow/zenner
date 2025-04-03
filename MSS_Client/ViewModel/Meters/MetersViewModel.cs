// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Meters.MetersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.DTO.MessageHandler;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Meters
{
  public class MetersViewModel : ViewModelBase
  {
    private ViewModelBase _messageUserControl;

    public MetersViewModel()
    {
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
    }

    public ICommand ThowUnhandledExceptionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          throw new Exception("Excetion thrown for test");
        }));
      }
    }

    public ICommand ThowHandledExceptionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          try
          {
            throw new BaseApplicationException(ErrorCodes.GetErrorMessage("MSSError_1"), new object[1]
            {
              (object) ErrorCodes.GetErrorMessage("MSSError_1")
            });
          }
          catch (BaseApplicationException ex)
          {
            MessageHandlingManager.ShowExceptionMessageDialog(ex);
          }
        }));
      }
    }

    public ICommand ShowSuccessMessageCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue())));
      }
    }

    public ICommand ShowCancelMessageCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.User_Duplicate.ToString())));
      }
    }

    public ICommand ShowValidationMessageCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(MessageCodes.Server_Not_Available.ToString())));
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }
  }
}
