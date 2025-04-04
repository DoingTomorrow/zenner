// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.LRUMap
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Util
{
  [Serializable]
  public class LRUMap : SequencedHashMap
  {
    private int maximumSize;

    public LRUMap()
      : this(100)
    {
    }

    public LRUMap(int capacity)
      : base(capacity)
    {
      this.maximumSize = capacity;
    }

    public override object this[object key]
    {
      get
      {
        object obj = base[key];
        if (obj == null)
          return (object) null;
        this.Remove(key);
        base.Add(key, obj);
        return obj;
      }
      set => this.Add(key, value);
    }

    public override void Add(object key, object value)
    {
      if (this.Count >= this.maximumSize && !this.ContainsKey(key))
        this.RemoveLRU();
      base[key] = value;
    }

    private void RemoveLRU()
    {
      object firstKey = this.FirstKey;
      object obj = base[firstKey];
      this.Remove(firstKey);
      this.ProcessRemovedLRU(firstKey, obj);
    }

    protected void ProcessRemovedLRU(object key, object value)
    {
    }

    public int MaximumSize
    {
      get => this.maximumSize;
      set
      {
        this.maximumSize = value;
        while (this.Count > this.maximumSize)
          this.RemoveLRU();
      }
    }
  }
}
