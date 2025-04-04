// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericSortedSetType`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GenericSortedSetType<T> : GenericSetType<T>
  {
    private readonly IComparer<T> comparer;

    public GenericSortedSetType(string role, string propertyRef, IComparer<T> comparer)
      : base(role, propertyRef)
    {
      this.comparer = comparer;
    }

    public IComparer<T> Comparer => this.comparer;

    public override object Instantiate(int anticipatedSize)
    {
      return (object) new SortedSet<T>(this.comparer);
    }
  }
}
