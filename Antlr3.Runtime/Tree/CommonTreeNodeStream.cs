// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTreeNodeStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class CommonTreeNodeStream : LookaheadStream<object>, ITreeNodeStream, IIntStream
  {
    public const int DEFAULT_INITIAL_BUFFER_SIZE = 100;
    public const int INITIAL_CALL_STACK_SIZE = 10;
    private readonly object _root;
    protected ITokenStream tokens;
    [NonSerialized]
    private ITreeAdaptor _adaptor;
    private readonly TreeIterator _it;
    private Stack<int> _calls;
    private bool _hasNilRoot;
    private int _level;

    public CommonTreeNodeStream(object tree)
      : this((ITreeAdaptor) new CommonTreeAdaptor(), tree)
    {
    }

    public CommonTreeNodeStream(ITreeAdaptor adaptor, object tree)
    {
      this._root = tree;
      this._adaptor = adaptor;
      this._it = new TreeIterator(adaptor, this._root);
    }

    public virtual string SourceName
    {
      get => this.TokenStream == null ? (string) null : this.TokenStream.SourceName;
    }

    public virtual ITokenStream TokenStream
    {
      get => this.tokens;
      set => this.tokens = value;
    }

    public virtual ITreeAdaptor TreeAdaptor
    {
      get => this._adaptor;
      set => this._adaptor = value;
    }

    public virtual object TreeSource => this._root;

    public virtual bool UniqueNavigationNodes
    {
      get => false;
      set
      {
      }
    }

    public virtual void Reset()
    {
      this.Clear();
      this._it.Reset();
      this._hasNilRoot = false;
      this._level = 0;
      if (this._calls == null)
        return;
      this._calls.Clear();
    }

    public override object NextElement()
    {
      this._it.MoveNext();
      object current1 = this._it.Current;
      if (current1 == this._it.up)
      {
        --this._level;
        if (this._level == 0 && this._hasNilRoot)
        {
          this._it.MoveNext();
          return this._it.Current;
        }
      }
      else if (current1 == this._it.down)
        ++this._level;
      if (this._level == 0 && this.TreeAdaptor.IsNil(current1))
      {
        this._hasNilRoot = true;
        this._it.MoveNext();
        object current2 = this._it.Current;
        ++this._level;
        this._it.MoveNext();
        current1 = this._it.Current;
      }
      return current1;
    }

    public override bool IsEndOfFile(object o) => this.TreeAdaptor.GetType(o) == -1;

    public virtual int LA(int i) => this.TreeAdaptor.GetType(this.LT(i));

    public virtual void Push(int index)
    {
      if (this._calls == null)
        this._calls = new Stack<int>();
      this._calls.Push(this._p);
      this.Seek(index);
    }

    public virtual int Pop()
    {
      int index = this._calls.Pop();
      this.Seek(index);
      return index;
    }

    public virtual void ReplaceChildren(
      object parent,
      int startChildIndex,
      int stopChildIndex,
      object t)
    {
      if (parent == null)
        return;
      this.TreeAdaptor.ReplaceChildren(parent, startChildIndex, stopChildIndex, t);
    }

    public virtual string ToString(object start, object stop) => "n/a";

    public virtual string ToTokenTypeString()
    {
      this.Reset();
      StringBuilder stringBuilder = new StringBuilder();
      for (int type = this.TreeAdaptor.GetType(this.LT(1)); type != -1; type = this.TreeAdaptor.GetType(this.LT(1)))
      {
        stringBuilder.Append(" ");
        stringBuilder.Append(type);
        this.Consume();
      }
      return stringBuilder.ToString();
    }
  }
}
