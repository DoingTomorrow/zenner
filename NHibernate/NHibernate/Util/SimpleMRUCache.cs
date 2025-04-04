// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SimpleMRUCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class SimpleMRUCache : IDeserializationCallback
  {
    private const int DefaultStrongRefCount = 128;
    private object _syncRoot;
    private readonly int strongReferenceCount;
    [NonSerialized]
    private LRUMap cache;

    public SimpleMRUCache()
      : this(128)
    {
    }

    public SimpleMRUCache(int strongReferenceCount)
    {
      this.strongReferenceCount = strongReferenceCount;
      this.cache = new LRUMap(strongReferenceCount);
    }

    private object SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      this.cache = new LRUMap(this.strongReferenceCount);
    }

    public object this[object key]
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        lock (this.SyncRoot)
          return this.cache[key];
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Put(object key, object value)
    {
      lock (this.SyncRoot)
        this.cache.Add(key, value);
    }

    public int Count
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        lock (this.SyncRoot)
          return this.cache.Count;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Clear()
    {
      lock (this.SyncRoot)
        this.cache.Clear();
    }
  }
}
