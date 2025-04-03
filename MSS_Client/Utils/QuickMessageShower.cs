// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.QuickMessageShower
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Client.UI.Desktop.View.ExceptionMessageBox;
using MSS.DTO.MessageHandler;
using MSS_Client.ViewModel.ExceptionMessageBox;
using MVVM.ViewModel;

#nullable disable
namespace MSS_Client.Utils
{
  public class QuickMessageShower : IQuickMessageShower
  {
    public ViewModelBase ShowSuccessMessage(string successMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(successMessage, MessageTypeEnum.Success);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public ViewModelBase ShowWarningMessage(string warningMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(warningMessage, MessageTypeEnum.Warning);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public ViewModelBase ShowValidationMessage(string validationMessage)
    {
      MessageHandlerUserControl handlerUserControl = new MessageHandlerUserControl();
      handlerUserControl.DataContext = (object) new MessageHandlerViewModel(validationMessage, MessageTypeEnum.Validation);
      return (ViewModelBase) handlerUserControl.DataContext;
    }

    public ViewModelBase Show(MSS.DTO.Message.Message message)
    {
      MessageTypeEnum? messageType = message?.MessageType;
      if (messageType.HasValue)
      {
        switch (messageType.GetValueOrDefault())
        {
          case MessageTypeEnum.Success:
            return this.ShowSuccessMessage(message.MessageText);
          case MessageTypeEnum.Warning:
            return this.ShowWarningMessage(message.MessageText);
          case MessageTypeEnum.Validation:
            return this.ShowValidationMessage(message.MessageText);
        }
      }
      return (ViewModelBase) null;
    }
  }
}
