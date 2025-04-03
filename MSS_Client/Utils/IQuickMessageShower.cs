// Decompiled with JetBrains decompiler
// Type: MSS_Client.Utils.IQuickMessageShower
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MVVM.ViewModel;

#nullable disable
namespace MSS_Client.Utils
{
  public interface IQuickMessageShower
  {
    ViewModelBase ShowSuccessMessage(string successMessage);

    ViewModelBase ShowWarningMessage(string warningMessage);

    ViewModelBase ShowValidationMessage(string validationMessage);

    ViewModelBase Show(MSS.DTO.Message.Message message);
  }
}
