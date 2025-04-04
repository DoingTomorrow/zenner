// Decompiled with JetBrains decompiler
// Type: NHibernate.DebugHelpers.DictionaryProxy`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace NHibernate.DebugHelpers
{
  public class DictionaryProxy<K, V>
  {
    private readonly IDictionary<K, V> dic;

    public DictionaryProxy(IDictionary<K, V> dic) => this.dic = dic;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public KeyValuePair<K, V>[] Items
    {
      get
      {
        KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[this.dic.Count];
        this.dic.CopyTo(array, 0);
        return array;
      }
    }
  }
}
