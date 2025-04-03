// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.Ctrls.ListProgressBarUpdater
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Localisation;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MSS_Client.ViewModel.Orders.Ctrls
{
  public class ListProgressBarUpdater : IProgressBarUpdater
  {
    private readonly Dictionary<Guid, string> _items;
    private ProgressBarCtrl _progressBarCtrl;

    public ListProgressBarUpdater(Dictionary<Guid, string> items)
    {
      this._items = new Dictionary<Guid, string>((IDictionary<Guid, string>) items);
    }

    public void UpdateOnHarwareEvent(ProgressEvent obj)
    {
    }

    public void UpdateOnItemDone(ProgressBarItemDone proccesedItem)
    {
      this.RemoveItem(proccesedItem.Item);
      if (this._items.Count == 0)
      {
        this.ResetProgressBar();
      }
      else
      {
        this._progressBarCtrl.ProgressBarValue = this._progressBarCtrl.ProgressBarTotal - this._items.Count;
        this._progressBarCtrl.ProgressDialogMessage = Resources.ExecuteReadingOrder_ReadMeters + " " + (object) this._progressBarCtrl.ProgressBarValue + " " + Resources.ExecuteReadingOrder_of + " " + (object) this._progressBarCtrl.ProgressBarTotal;
      }
    }

    private void RemoveItem(string proccesedItem)
    {
      foreach (Guid key in this._items.Where<KeyValuePair<Guid, string>>((Func<KeyValuePair<Guid, string>, bool>) (pair => pair.Value == proccesedItem)).Select<KeyValuePair<Guid, string>, Guid>((Func<KeyValuePair<Guid, string>, Guid>) (pair => pair.Key)).ToList<Guid>())
        this._items.Remove(key);
    }

    public void ResetProgressBar()
    {
      this._items.Clear();
      this._progressBarCtrl.ResetProgressBar();
    }

    public void Start(ProgressBarCtrl progressBarCtrl)
    {
      progressBarCtrl.ProgressBarTotal = this._items.Count;
      progressBarCtrl.ProgressBarVisibility = true;
      progressBarCtrl.ProgressDialogMessage = Resources.ExecuteReadingOrder_ReadMeters;
      this._progressBarCtrl = progressBarCtrl;
    }
  }
}
