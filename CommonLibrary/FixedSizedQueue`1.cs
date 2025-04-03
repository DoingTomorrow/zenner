// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.FixedSizedQueue`1
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.Collections.Concurrent;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class FixedSizedQueue<T>
  {
    private readonly object lockObj = new object();
    private ConcurrentQueue<T> q = new ConcurrentQueue<T>();

    public int Limit { get; set; }

    public void Enqueue(T obj)
    {
      this.q.Enqueue(obj);
      lock (this.lockObj)
      {
        do
          ;
        while (this.q.Count > this.Limit && this.q.TryDequeue(out T _));
      }
    }

    public T First()
    {
      T result;
      return this.q.TryDequeue(out result) ? result : default (T);
    }
  }
}
