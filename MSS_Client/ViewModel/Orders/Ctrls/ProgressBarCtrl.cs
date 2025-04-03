// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.Ctrls.ProgressBarCtrl
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Interfaces;
using MVVM.ViewModel;
using System;

#nullable disable
namespace MSS_Client.ViewModel.Orders.Ctrls
{
  public class ProgressBarCtrl : ValidationViewModelBase
  {
    private bool _progressBarVisibility;
    private int _progressBarValue;
    private int _progressBarTotal;
    private string _progressDialogMessage = string.Empty;
    private IProgressBarUpdater _progressBarUpdater;

    public ProgressBarCtrl()
    {
      EventPublisher.Register<ProgressEvent>(new Action<ProgressEvent>(this.UpdateOnHarwareEvent));
      EventPublisher.Register<ProgressBarItemDone>(new Action<ProgressBarItemDone>(this.UpdateOnItemDone));
    }

    public void Start(IProgressBarUpdater progressBarUpdater)
    {
      this._progressBarUpdater = progressBarUpdater;
      this._progressBarUpdater?.Start(this);
    }

    public virtual void UpdateOnItemDone(ProgressBarItemDone obj)
    {
      this._progressBarUpdater?.UpdateOnItemDone(obj);
    }

    public virtual void UpdateOnHarwareEvent(ProgressEvent obj)
    {
      this._progressBarUpdater?.UpdateOnHarwareEvent(obj);
    }

    public virtual void ResetProgressBar()
    {
      this.ProgressDialogMessage = string.Empty;
      this.ProgressBarVisibility = false;
      this.ProgressBarTotal = 0;
      this.ProgressBarValue = 0;
      this._progressBarUpdater = (IProgressBarUpdater) null;
      EventPublisher.Publish<ProgressFinished>(new ProgressFinished(), (IViewModel) this);
    }

    public bool ProgressBarVisibility
    {
      get => this._progressBarVisibility;
      set
      {
        this._progressBarVisibility = value;
        this.OnPropertyChanged(nameof (ProgressBarVisibility));
      }
    }

    public int ProgressBarValue
    {
      get => this._progressBarValue;
      set
      {
        this._progressBarValue = value;
        this.OnPropertyChanged(nameof (ProgressBarValue));
      }
    }

    public int ProgressBarTotal
    {
      get => this._progressBarTotal;
      set
      {
        this._progressBarTotal = value;
        this.OnPropertyChanged(nameof (ProgressBarTotal));
      }
    }

    public string ProgressDialogMessage
    {
      get => this._progressDialogMessage;
      set
      {
        this._progressDialogMessage = value;
        this.OnPropertyChanged(nameof (ProgressDialogMessage));
      }
    }
  }
}
