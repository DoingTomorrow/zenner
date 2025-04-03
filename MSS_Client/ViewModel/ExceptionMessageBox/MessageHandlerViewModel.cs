// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.ExceptionMessageBox.MessageHandlerViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.DTO.MessageHandler;
using MVVM.ViewModel;

#nullable disable
namespace MSS_Client.ViewModel.ExceptionMessageBox
{
  public class MessageHandlerViewModel : ViewModelBase
  {
    private string _message;
    private bool isSuccessMessage;
    private int _opacity;

    public MessageHandlerViewModel(string message, MessageTypeEnum messageType)
    {
      this.ContentMessage = message;
      switch (messageType)
      {
        case MessageTypeEnum.Success:
          this.IsSuccessMessage = true;
          this.IsValidationMessage = false;
          this.IsWarningMessage = false;
          break;
        case MessageTypeEnum.Warning:
          this.IsSuccessMessage = false;
          this.IsWarningMessage = true;
          this.IsValidationMessage = false;
          break;
        case MessageTypeEnum.Validation:
          this.IsSuccessMessage = false;
          this.IsWarningMessage = false;
          this.IsValidationMessage = true;
          break;
      }
    }

    public string ContentMessage
    {
      get => this._message;
      set
      {
        this._message = value;
        this.OnPropertyChanged(nameof (ContentMessage));
      }
    }

    public bool IsSuccessMessage
    {
      get => this.isSuccessMessage;
      set
      {
        this.isSuccessMessage = value;
        this.OnPropertyChanged(nameof (IsSuccessMessage));
      }
    }

    public bool IsWarningMessage { get; set; }

    public bool IsValidationMessage { get; set; }

    public int Opacity
    {
      get => this._opacity;
      set
      {
        this._opacity = value;
        this.OnPropertyChanged(nameof (Opacity));
      }
    }
  }
}
