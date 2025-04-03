// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.AbstractDictionaryAdapter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public abstract class AbstractDictionaryAdapter : IDictionary, ICollection, IEnumerable
  {
    public void Add(object key, object value) => throw new NotSupportedException();

    public void Clear() => throw new NotSupportedException();

    public abstract bool Contains(object key);

    public IDictionaryEnumerator GetEnumerator() => throw new NotSupportedException();

    public bool IsFixedSize => throw new NotSupportedException();

    public abstract bool IsReadOnly { get; }

    public ICollection Keys => throw new NotSupportedException();

    public void Remove(object key) => throw new NotSupportedException();

    public ICollection Values => throw new NotSupportedException();

    public abstract object this[object key] { get; set; }

    public void CopyTo(Array array, int index) => throw new NotSupportedException();

    public int Count => throw new NotSupportedException();

    public virtual bool IsSynchronized => false;

    public virtual object SyncRoot => (object) this;

    IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
  }
}
