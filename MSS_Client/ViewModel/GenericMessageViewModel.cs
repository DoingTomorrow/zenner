// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.GenericMessageViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel
{
  public class GenericMessageViewModel : ViewModelBase
  {
    private string _title;
    private string _message;
    private bool _isCancelButtonVisible;

    public GenericMessageViewModel(string title, string message, bool isCancelButtonVisible)
    {
      this._title = title;
      this._message = message;
      this.IsCancelButtonVisible = isCancelButtonVisible;
    }

    public string Title
    {
      get => this._title;
      set => this._title = value;
    }

    public string Message
    {
      get => this._message;
      set => this._message = value;
    }

    public bool IsCancelButtonVisible
    {
      get => this._isCancelButtonVisible;
      set
      {
        this._isCancelButtonVisible = value;
        this.OnPropertyChanged(nameof (IsCancelButtonVisible));
      }
    }

    public ICommand OkButtonCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.OnRequestClose(true);
        });
      }
    }
  }
}
