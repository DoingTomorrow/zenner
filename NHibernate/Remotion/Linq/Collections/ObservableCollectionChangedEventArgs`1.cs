// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Collections.ObservableCollectionChangedEventArgs`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Remotion.Linq.Collections
{
  public class ObservableCollectionChangedEventArgs<T> : EventArgs
  {
    public int Index { get; set; }

    public T Item { get; set; }

    public ObservableCollectionChangedEventArgs(int index, T item)
    {
      this.Index = index;
      this.Item = item;
    }
  }
}
