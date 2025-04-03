// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.AsyncObservableCollection`1
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace ZENNER.CommonLibrary
{
  [Serializable]
  public class AsyncObservableCollection<T> : ObservableCollection<T>
  {
    private SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    public AsyncObservableCollection()
    {
    }

    public AsyncObservableCollection(IEnumerable<T> list)
      : base(list)
    {
    }

    public void AddRange(IEnumerable<T> items)
    {
      this.CheckReentrancy();
      foreach (T obj in items)
        this.Items.Add(obj);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this.synchronizationContext == null)
        this.synchronizationContext = SynchronizationContext.Current;
      if (SynchronizationContext.Current == this.synchronizationContext)
        this.RaiseCollectionChanged((object) e);
      else
        this.synchronizationContext.Post(new SendOrPostCallback(this.RaiseCollectionChanged), (object) e);
    }

    private void RaiseCollectionChanged(object param)
    {
      using (this.BlockReentrancy())
        base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.synchronizationContext == null)
        this.synchronizationContext = SynchronizationContext.Current;
      if (SynchronizationContext.Current == this.synchronizationContext)
        this.RaisePropertyChanged((object) e);
      else
        this.synchronizationContext.Post(new SendOrPostCallback(this.RaisePropertyChanged), (object) e);
    }

    private void RaisePropertyChanged(object param)
    {
      using (this.BlockReentrancy())
        base.OnPropertyChanged((PropertyChangedEventArgs) param);
    }
  }
}
