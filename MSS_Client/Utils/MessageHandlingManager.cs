// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.MessageHandlingManager
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Client.UI.Desktop.View.ExceptionMessageBox;
using MSS.Core.Events;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS_Client.ViewModel.ExceptionMessageBox;
using MVVM.ViewModel;
using System;

#nullable disable
namespace MSS_Client.Utils
{
  public static class MessageHandlingManager
  {
    public static ViewModelBase ShowSuccessMessage(string successMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(successMessage, MessageTypeEnum.Success);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public static ViewModelBase ShowWarningMessage(string warningMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(warningMessage, MessageTypeEnum.Warning);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public static ViewModelBase ShowValidationMessage(string validationMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(validationMessage, MessageTypeEnum.Validation);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public static void ShowExceptionMessageDialog(BaseApplicationException e)
    {
      MSS.Business.Errors.MessageHandler.LogException((Exception) e);
      ExceptionMessageBoxDialog messageBoxDialog = new ExceptionMessageBoxDialog();
      messageBoxDialog.DataContext = (object) new ExceptionMessageBoxViewModel(e.Message, false);
      messageBoxDialog.ShowDialog();
    }

    public static void ShowExceptionMessageDialog(string message, IWindowFactory windowFactory)
    {
      MSS.Business.Errors.MessageHandler.LogException(message);
      windowFactory.CreateNewModalDialog((IViewModel) new ExceptionMessageBoxViewModel(message, false));
    }

    public static bool ShowCustomMessageDialog(string messageTitle, string messageBody)
    {
      ExceptionMessageBoxViewModel customMessageviewModel = new ExceptionMessageBoxViewModel(messageTitle, messageBody);
      ExceptionMessageBoxDialog messageBoxDialog = new ExceptionMessageBoxDialog();
      messageBoxDialog.Topmost = true;
      ExceptionMessageBoxDialog customMessageDialog = messageBoxDialog;
      EventHandler<RequestCloseEventArgs> handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) null;
      handlerCloseDialog = (EventHandler<RequestCloseEventArgs>) ((sender, eventArgs) =>
      {
        customMessageviewModel.RequestCloseDialog -= handlerCloseDialog;
        customMessageDialog.Close();
      });
      customMessageviewModel.RequestCloseDialog += handlerCloseDialog;
      customMessageDialog.DataContext = (object) customMessageviewModel;
      customMessageDialog.ShowDialog();
      return customMessageviewModel.IsYesButtonClicked;
    }
  }
}
