// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleElementStream`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal abstract class RewriteRuleElementStream<T>
  {
    protected int cursor;
    protected T singleElement;
    protected IList<T> elements;
    protected bool dirty;
    protected string elementDescription;
    protected ITreeAdaptor adaptor;

    public RewriteRuleElementStream(ITreeAdaptor adaptor, string elementDescription)
    {
      this.elementDescription = elementDescription;
      this.adaptor = adaptor;
    }

    public RewriteRuleElementStream(ITreeAdaptor adaptor, string elementDescription, T oneElement)
      : this(adaptor, elementDescription)
    {
      this.Add(oneElement);
    }

    public RewriteRuleElementStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IList<T> elements)
      : this(adaptor, elementDescription)
    {
      this.singleElement = default (T);
      this.elements = elements;
    }

    [Obsolete("This constructor is for internal use only and might be phased out soon. Use instead the one with IList<T>.")]
    public RewriteRuleElementStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IList elements)
      : this(adaptor, elementDescription)
    {
      this.singleElement = default (T);
      this.elements = (IList<T>) new List<T>();
      if (elements == null)
        return;
      foreach (T element in (IEnumerable) elements)
        this.elements.Add(element);
    }

    public void Add(T el)
    {
      if ((object) el == null)
        return;
      if (this.elements != null)
        this.elements.Add(el);
      else if ((object) this.singleElement == null)
      {
        this.singleElement = el;
      }
      else
      {
        this.elements = (IList<T>) new List<T>(5);
        this.elements.Add(this.singleElement);
        this.singleElement = default (T);
        this.elements.Add(el);
      }
    }

    public virtual void Reset()
    {
      this.cursor = 0;
      this.dirty = true;
    }

    public bool HasNext()
    {
      if ((object) this.singleElement != null && this.cursor < 1)
        return true;
      return this.elements != null && this.cursor < this.elements.Count;
    }

    public virtual object NextTree() => this._Next();

    protected object _Next()
    {
      int count = this.Count;
      if (count == 0)
        throw new RewriteEmptyStreamException(this.elementDescription);
      if (this.cursor >= count)
      {
        if (count == 1)
          return this.ToTree(this.singleElement);
        throw new RewriteCardinalityException(this.elementDescription);
      }
      if ((object) this.singleElement == null)
        return this.ToTree(this.elements[this.cursor++]);
      ++this.cursor;
      return this.ToTree(this.singleElement);
    }

    protected virtual object ToTree(T el) => (object) el;

    public int Count
    {
      get
      {
        if ((object) this.singleElement != null)
          return 1;
        return this.elements != null ? this.elements.Count : 0;
      }
    }

    [Obsolete("Please use property Count instead.")]
    public int Size() => this.Count;

    public string Description => this.elementDescription;
  }
}
