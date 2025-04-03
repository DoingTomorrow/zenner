// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.TestGSMViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  internal class TestGSMViewModel : ViewModelBase
  {
    private bool _showProgressCircle;
    private string _testResult;

    public ICommand StartGsmTestCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.ShowProgressCircle = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (state =>
          {
            Thread.Sleep(4000);
            this.ShowProgressCircle = false;
            Application.Current.Dispatcher.Invoke((Action) (() => this.TestResult = Resources.MSS_Client_SuccessMessage));
          }));
        }));
      }
    }

    public bool ShowProgressCircle
    {
      get => this._showProgressCircle;
      set
      {
        this._showProgressCircle = value;
        this.OnPropertyChanged(nameof (ShowProgressCircle));
      }
    }

    public string TestResult
    {
      get => this._testResult;
      set
      {
        this._testResult = value;
        this.OnPropertyChanged(nameof (TestResult));
      }
    }
  }
}
