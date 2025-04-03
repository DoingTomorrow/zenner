// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.WarningWithListBoxViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class WarningWithListBoxViewModel : ViewModelBase
  {
    public WarningWithListBoxViewModel(List<string> existingItems, string warningMessage)
    {
      this.WindowHeight = existingItems.Count > 0 ? "410" : "280";
      this.WarningMessage = warningMessage;
      this.ExistingItems = existingItems;
    }

    public string WarningMessage { get; set; }

    public string WindowHeight { get; set; }

    public List<string> ExistingItems { get; set; }

    public ICommand YesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.OnRequestClose(true);
        });
      }
    }

    public ICommand NoCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.OnRequestClose(false);
        });
      }
    }
  }
}
