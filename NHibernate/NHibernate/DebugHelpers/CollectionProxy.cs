// Decompiled with JetBrains decompiler
// Type: NHibernate.DebugHelpers.CollectionProxy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace NHibernate.DebugHelpers
{
  public class CollectionProxy
  {
    private readonly ICollection set;

    public CollectionProxy(ICollection dic) => this.set = dic;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public object[] Items
    {
      get
      {
        object[] items = new object[this.set.Count];
        this.set.CopyTo((Array) items, 0);
        return items;
      }
    }
  }
}
