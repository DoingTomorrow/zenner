// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonTreeNodeStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Collections;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class CommonTreeNodeStream : IIntStream, ITreeNodeStream, IEnumerable
  {
    public const int DEFAULT_INITIAL_BUFFER_SIZE = 100;
    public const int INITIAL_CALL_STACK_SIZE = 10;
    protected object down;
    protected object up;
    protected object eof;
    protected IList nodes;
    protected internal object root;
    protected ITokenStream tokens;
    private ITreeAdaptor adaptor;
    protected bool uniqueNavigationNodes;
    protected int p = -1;
    protected int lastMarker;
    protected StackList calls;

    public CommonTreeNodeStream(object tree)
      : this((ITreeAdaptor) new CommonTreeAdaptor(), tree)
    {
    }

    public CommonTreeNodeStream(ITreeAdaptor adaptor, object tree)
      : this(adaptor, tree, 100)
    {
    }

    public CommonTreeNodeStream(ITreeAdaptor adaptor, object tree, int initialBufferSize)
    {
      this.root = tree;
      this.adaptor = adaptor;
      this.nodes = (IList) new ArrayList(initialBufferSize);
      this.down = adaptor.Create(2, "DOWN");
      this.up = adaptor.Create(3, "UP");
      this.eof = adaptor.Create(Token.EOF, "EOF");
    }

    public IEnumerator GetEnumerator()
    {
      if (this.p == -1)
        this.FillBuffer();
      return (IEnumerator) new CommonTreeNodeStream.CommonTreeNodeStreamEnumerator(this);
    }

    protected void FillBuffer()
    {
      this.FillBuffer(this.root);
      this.p = 0;
    }

    public void FillBuffer(object t)
    {
      bool flag = this.adaptor.IsNil(t);
      if (!flag)
        this.nodes.Add(t);
      int childCount = this.adaptor.GetChildCount(t);
      if (!flag && childCount > 0)
        this.AddNavigationNode(2);
      for (int i = 0; i < childCount; ++i)
        this.FillBuffer(this.adaptor.GetChild(t, i));
      if (flag || childCount <= 0)
        return;
      this.AddNavigationNode(3);
    }

    protected int GetNodeIndex(object node)
    {
      if (this.p == -1)
        this.FillBuffer();
      for (int index = 0; index < this.nodes.Count; ++index)
      {
        if (this.nodes[index] == node)
          return index;
      }
      return -1;
    }

    protected void AddNavigationNode(int ttype)
    {
      this.nodes.Add(ttype != 2 ? (!this.HasUniqueNavigationNodes ? this.up : this.adaptor.Create(3, "UP")) : (!this.HasUniqueNavigationNodes ? this.down : this.adaptor.Create(2, "DOWN")));
    }

    public object Get(int i)
    {
      if (this.p == -1)
        this.FillBuffer();
      return this.nodes[i];
    }

    public object LT(int k)
    {
      if (this.p == -1)
        this.FillBuffer();
      if (k == 0)
        return (object) null;
      if (k < 0)
        return this.LB(-k);
      return this.p + k - 1 >= this.nodes.Count ? this.eof : this.nodes[this.p + k - 1];
    }

    public virtual object CurrentSymbol => this.LT(1);

    protected object LB(int k)
    {
      if (k == 0)
        return (object) null;
      return this.p - k < 0 ? (object) null : this.nodes[this.p - k];
    }

    public virtual object TreeSource => this.root;

    public virtual string SourceName => this.TokenStream.SourceName;

    public virtual ITokenStream TokenStream
    {
      get => this.tokens;
      set => this.tokens = value;
    }

    public ITreeAdaptor TreeAdaptor
    {
      get => this.adaptor;
      set => this.adaptor = value;
    }

    public bool HasUniqueNavigationNodes
    {
      get => this.uniqueNavigationNodes;
      set => this.uniqueNavigationNodes = value;
    }

    public void Push(int index)
    {
      if (this.calls == null)
        this.calls = new StackList();
      this.calls.Push((object) this.p);
      this.Seek(index);
    }

    public int Pop()
    {
      int index = (int) this.calls.Pop();
      this.Seek(index);
      return index;
    }

    public void Reset()
    {
      this.p = -1;
      this.lastMarker = 0;
      if (this.calls == null)
        return;
      this.calls.Clear();
    }

    public void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
    {
      if (parent == null)
        return;
      this.adaptor.ReplaceChildren(parent, startChildIndex, stopChildIndex, t);
    }

    public virtual void Consume()
    {
      if (this.p == -1)
        this.FillBuffer();
      ++this.p;
    }

    public virtual int LA(int i) => this.adaptor.GetNodeType(this.LT(i));

    public virtual int Mark()
    {
      if (this.p == -1)
        this.FillBuffer();
      this.lastMarker = this.Index();
      return this.lastMarker;
    }

    public virtual void Release(int marker)
    {
    }

    public virtual void Rewind(int marker) => this.Seek(marker);

    public void Rewind() => this.Seek(this.lastMarker);

    public virtual void Seek(int index)
    {
      if (this.p == -1)
        this.FillBuffer();
      this.p = index;
    }

    public virtual int Index() => this.p;

    [Obsolete("Please use property Count instead.")]
    public virtual int Size() => this.Count;

    public virtual int Count
    {
      get
      {
        if (this.p == -1)
          this.FillBuffer();
        return this.nodes.Count;
      }
    }

    public override string ToString()
    {
      if (this.p == -1)
        this.FillBuffer();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.nodes.Count; ++index)
      {
        object node = this.nodes[index];
        stringBuilder.Append(" ");
        stringBuilder.Append(this.adaptor.GetNodeType(node));
      }
      return stringBuilder.ToString();
    }

    public string ToTokenString(int start, int stop)
    {
      if (this.p == -1)
        this.FillBuffer();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = start; index < this.nodes.Count && index <= stop; ++index)
      {
        object node = this.nodes[index];
        stringBuilder.Append(" ");
        stringBuilder.Append((object) this.adaptor.GetToken(node));
      }
      return stringBuilder.ToString();
    }

    public virtual string ToString(object start, object stop)
    {
      Console.Out.WriteLine(nameof (ToString));
      if (start == null || stop == null)
        return (string) null;
      if (this.p == -1)
        this.FillBuffer();
      if (start is CommonTree)
        Console.Out.Write("ToString: " + (object) ((CommonTree) start).Token + ", ");
      else
        Console.Out.WriteLine(start);
      if (stop is CommonTree)
        Console.Out.WriteLine((object) ((CommonTree) stop).Token);
      else
        Console.Out.WriteLine(stop);
      if (this.tokens != null)
      {
        int tokenStartIndex = this.adaptor.GetTokenStartIndex(start);
        int stop1 = this.adaptor.GetTokenStopIndex(stop);
        if (this.adaptor.GetNodeType(stop) == 3)
          stop1 = this.adaptor.GetTokenStopIndex(start);
        else if (this.adaptor.GetNodeType(stop) == Token.EOF)
          stop1 = this.Count - 2;
        return this.tokens.ToString(tokenStartIndex, stop1);
      }
      int index = 0;
      while (index < this.nodes.Count && this.nodes[index] != start)
        ++index;
      StringBuilder stringBuilder = new StringBuilder();
      for (object node = this.nodes[index]; node != stop; node = this.nodes[index])
      {
        string str = this.adaptor.GetNodeText(node) ?? " " + (object) this.adaptor.GetNodeType(node);
        stringBuilder.Append(str);
        ++index;
      }
      string str1 = this.adaptor.GetNodeText(stop) ?? " " + (object) this.adaptor.GetNodeType(stop);
      stringBuilder.Append(str1);
      return stringBuilder.ToString();
    }

    protected sealed class CommonTreeNodeStreamEnumerator : IEnumerator
    {
      private CommonTreeNodeStream _nodeStream;
      private int _index;
      private object _currentItem;

      internal CommonTreeNodeStreamEnumerator()
      {
      }

      internal CommonTreeNodeStreamEnumerator(CommonTreeNodeStream nodeStream)
      {
        this._nodeStream = nodeStream;
        this.Reset();
      }

      public void Reset()
      {
        this._index = 0;
        this._currentItem = (object) null;
      }

      public object Current
      {
        get
        {
          return this._currentItem != null ? this._currentItem : throw new InvalidOperationException("Enumeration has either not started or has already finished.");
        }
      }

      public bool MoveNext()
      {
        if (this._index >= this._nodeStream.nodes.Count)
        {
          int index = this._index;
          ++this._index;
          if (index < this._nodeStream.nodes.Count)
            this._currentItem = this._nodeStream.nodes[index];
          this._currentItem = this._nodeStream.eof;
          return true;
        }
        this._currentItem = (object) null;
        return false;
      }
    }
  }
}
