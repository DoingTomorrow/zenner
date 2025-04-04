// Decompiled with JetBrains decompiler
// Type: NHibernate.DebugHelpers.CollectionProxy`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace NHibernate.DebugHelpers
{
  public class CollectionProxy<T>
  {
    private readonly ICollection<T> set;

    public CollectionProxy(ICollection<T> dic) => this.set = dic;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items
    {
      get
      {
        T[] array = new T[this.set.Count];
        this.set.CopyTo(array, 0);
        return array;
      }
    }
  }
}
