// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.IdentitySet
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Util
{
  public class IdentitySet : Set
  {
    private IDictionary map;
    private static readonly object DumpValue = new object();

    public IdentitySet() => this.map = IdentityMap.Instantiate(10);

    public override bool Add(object o)
    {
      object obj = this.map[o];
      this.map[o] = IdentitySet.DumpValue;
      return obj == null;
    }

    public override bool AddAll(ICollection c)
    {
      bool flag = false;
      foreach (object o in (IEnumerable) c)
        flag |= this.Add(o);
      return flag;
    }

    public override void Clear() => this.map.Clear();

    public override bool Contains(object o) => this.map[o] == IdentitySet.DumpValue;

    public override bool ContainsAll(ICollection c)
    {
      foreach (object key in (IEnumerable) c)
      {
        if (!this.map.Contains(key))
          return false;
      }
      return true;
    }

    public override bool Remove(object o)
    {
      object obj = this.map[o];
      this.map.Remove(o);
      return obj == IdentitySet.DumpValue;
    }

    public override bool RemoveAll(ICollection c)
    {
      bool flag = false;
      foreach (object o in (IEnumerable) c)
      {
        flag |= this.Contains(o);
        this.Remove(o);
      }
      return flag;
    }

    public override bool RetainAll(ICollection c) => throw new NotSupportedException();

    public override void CopyTo(Array array, int index) => this.map.CopyTo(array, index);

    public override IEnumerator GetEnumerator() => (IEnumerator) this.map.GetEnumerator();

    public override bool IsEmpty => this.map.Count == 0;

    public override int Count => this.map.Count;

    public override bool IsSynchronized => false;

    public override object SyncRoot => (object) this;
  }
}
