// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.SoftLimitMRUCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class SoftLimitMRUCache : IDeserializationCallback
  {
    private const int DefaultStrongRefCount = 128;
    private object _syncRoot;
    private readonly int strongReferenceCount;
    [NonSerialized]
    private readonly IDictionary softReferenceCache = (IDictionary) new WeakHashtable();
    [NonSerialized]
    private LRUMap strongReferenceCache;

    public SoftLimitMRUCache(int strongReferenceCount)
    {
      this.strongReferenceCount = strongReferenceCount;
      this.strongReferenceCache = new LRUMap(strongReferenceCount);
    }

    public SoftLimitMRUCache()
      : this(128)
    {
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
      this.strongReferenceCache = new LRUMap(this.strongReferenceCount);
    }

    public object this[object key]
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        lock (this.SyncRoot)
        {
          object obj = this.softReferenceCache[key];
          if (obj != null)
            this.strongReferenceCache.Add(key, obj);
          return obj;
        }
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Put(object key, object value)
    {
      lock (this.SyncRoot)
      {
        this.softReferenceCache[key] = value;
        this.strongReferenceCache[key] = value;
      }
    }

    public int Count
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        lock (this.SyncRoot)
          return this.strongReferenceCache.Count;
      }
    }

    public int SoftCount
    {
      [MethodImpl(MethodImplOptions.Synchronized)] get
      {
        lock (this.SyncRoot)
          return this.softReferenceCache.Count;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Clear()
    {
      lock (this.SyncRoot)
      {
        this.strongReferenceCache.Clear();
        this.softReferenceCache.Clear();
      }
    }
  }
}
