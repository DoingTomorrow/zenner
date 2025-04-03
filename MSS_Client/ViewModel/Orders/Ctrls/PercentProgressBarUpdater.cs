// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.Ctrls.PercentProgressBarUpdater
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Localisation;

#nullable disable
namespace MSS_Client.ViewModel.Orders.Ctrls
{
  public class PercentProgressBarUpdater : IProgressBarUpdater
  {
    private ProgressBarCtrl _progressBarCtrl;

    public void Start(ProgressBarCtrl progressBarCtrl)
    {
      progressBarCtrl.ProgressBarTotal = 100;
      progressBarCtrl.ProgressBarVisibility = true;
      progressBarCtrl.ProgressDialogMessage = Resources.ExecuteReadingOrder_ReadMeters;
      this._progressBarCtrl = progressBarCtrl;
    }

    public void UpdateOnHarwareEvent(ProgressEvent obj)
    {
      int num = obj.Value;
      if (num == 100)
      {
        this._progressBarCtrl.ResetProgressBar();
      }
      else
      {
        this._progressBarCtrl.ProgressBarValue = num;
        this._progressBarCtrl.ProgressDialogMessage = Resources.ExecuteReadingOrder_ReadMeters + " " + (object) num + "%";
      }
    }

    public void UpdateOnItemDone(ProgressBarItemDone proccesedItem)
    {
      this._progressBarCtrl.ResetProgressBar();
    }
  }
}
