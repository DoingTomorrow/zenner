// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.KeyValuePairsEnumerator`2
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class KeyValuePairsEnumerator<TKey, TValue> : IEnumerator
  {
    private IEnumerator<KeyValuePair<TKey, TValue>> m_collection;

    public KeyValuePairsEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
    {
      this.m_collection = enumerator;
    }

    public void Reset() => this.m_collection.Reset();

    public bool MoveNext() => this.m_collection.MoveNext();

    public object Current => (object) this.m_collection.Current.Value;
  }
}
