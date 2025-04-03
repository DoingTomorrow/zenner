// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.TypeElementCollection`1
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class TypeElementCollection<TElement> : 
    ICollection<TElement>,
    IEnumerable<TElement>,
    IEnumerable
    where TElement : MetaTypeElement, IEquatable<TElement>
  {
    private readonly ICollection<TElement> items = (ICollection<TElement>) new List<TElement>();

    public IEnumerator<TElement> GetEnumerator() => this.items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(TElement item)
    {
      if (!item.CanBeImplementedExplicitly)
      {
        this.items.Add(item);
      }
      else
      {
        if (this.Contains(item))
        {
          item.SwitchToExplicitImplementation();
          if (this.Contains(item))
            throw new ProxyGenerationException("Duplicate element: " + item.ToString());
        }
        this.items.Add(item);
      }
    }

    void ICollection<TElement>.Clear() => throw new NotSupportedException();

    public bool Contains(TElement item)
    {
      foreach (TElement element in (IEnumerable<TElement>) this.items)
      {
        if (element.Equals(item))
          return true;
      }
      return false;
    }

    void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
    {
      throw new NotSupportedException();
    }

    bool ICollection<TElement>.Remove(TElement item) => throw new NotSupportedException();

    public int Count => this.items.Count;

    bool ICollection<TElement>.IsReadOnly => false;
  }
}
