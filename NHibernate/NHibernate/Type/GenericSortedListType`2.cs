// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericSortedListType`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GenericSortedListType<TKey, TValue> : GenericMapType<TKey, TValue>
  {
    private readonly IComparer<TKey> comparer;

    public GenericSortedListType(string role, string propertyRef, IComparer<TKey> comparer)
      : base(role, propertyRef)
    {
      this.comparer = comparer;
    }

    public IComparer<TKey> Comparer => this.comparer;

    public override object Instantiate(int anticipatedSize)
    {
      return (object) new SortedList<TKey, TValue>(this.comparer);
    }
  }
}
