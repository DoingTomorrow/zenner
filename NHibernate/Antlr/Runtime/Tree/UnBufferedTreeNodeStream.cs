// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.UnBufferedTreeNodeStream
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
  internal class UnBufferedTreeNodeStream : IIntStream, ITreeNodeStream
  {
    public const int INITIAL_LOOKAHEAD_BUFFER_SIZE = 5;
    private ITree currentEnumerationNode;
    protected bool uniqueNavigationNodes;
    protected internal object root;
    protected ITokenStream tokens;
    private ITreeAdaptor adaptor;
    protected internal StackList nodeStack = new StackList();
    protected internal StackList indexStack = new StackList();
    protected internal object currentNode;
    protected internal object previousNode;
    protected internal int currentChildIndex;
    protected int absoluteNodeIndex;
    protected internal object[] lookahead = new object[5];
    protected internal int head;
    protected internal int tail;
    protected IList markers;
    protected int markDepth;
    protected int lastMarker;
    protected object down;
    protected object up;
    protected object eof;

    public UnBufferedTreeNodeStream(object tree)
      : this((ITreeAdaptor) new CommonTreeAdaptor(), tree)
    {
    }

    public UnBufferedTreeNodeStream(ITreeAdaptor adaptor, object tree)
    {
      this.root = tree;
      this.adaptor = adaptor;
      this.Reset();
      this.down = adaptor.Create(2, "DOWN");
      this.up = adaptor.Create(3, "UP");
      this.eof = adaptor.Create(Token.EOF, "EOF");
    }

    public virtual object TreeSource => this.root;

    public virtual void Reset()
    {
      this.currentNode = this.root;
      this.previousNode = (object) null;
      this.currentChildIndex = -1;
      this.absoluteNodeIndex = -1;
      this.head = this.tail = 0;
    }

    public virtual bool MoveNext()
    {
      if (this.currentNode == null)
      {
        this.AddLookahead(this.eof);
        this.currentEnumerationNode = (ITree) null;
        return false;
      }
      if (this.currentChildIndex == -1)
      {
        this.currentEnumerationNode = (ITree) this.handleRootNode();
        return true;
      }
      if (this.currentChildIndex < this.adaptor.GetChildCount(this.currentNode))
      {
        this.currentEnumerationNode = (ITree) this.VisitChild(this.currentChildIndex);
        return true;
      }
      this.WalkBackToMostRecentNodeWithUnvisitedChildren();
      if (this.currentNode == null)
        return false;
      this.currentEnumerationNode = (ITree) this.VisitChild(this.currentChildIndex);
      return true;
    }

    public virtual object Current => (object) this.currentEnumerationNode;

    public virtual object Get(int i) => throw new NotSupportedException("stream is unbuffered");

    public virtual object LT(int k)
    {
      if (k == -1)
        return this.previousNode;
      if (k < 0)
        throw new ArgumentNullException("tree node streams cannot look backwards more than 1 node", nameof (k));
      if (k == 0)
        return (object) Antlr.Runtime.Tree.Tree.INVALID_NODE;
      this.fill(k);
      return this.lookahead[(this.head + k - 1) % this.lookahead.Length];
    }

    protected internal virtual void fill(int k)
    {
      int lookaheadSize = this.LookaheadSize;
      for (int index = 1; index <= k - lookaheadSize; ++index)
        this.MoveNext();
    }

    protected internal virtual void AddLookahead(object node)
    {
      this.lookahead[this.tail] = node;
      this.tail = (this.tail + 1) % this.lookahead.Length;
      if (this.tail != this.head)
        return;
      object[] destinationArray = new object[2 * this.lookahead.Length];
      int num = this.lookahead.Length - this.head;
      Array.Copy((Array) this.lookahead, this.head, (Array) destinationArray, 0, num);
      Array.Copy((Array) this.lookahead, 0, (Array) destinationArray, num, this.tail);
      this.lookahead = destinationArray;
      this.head = 0;
      this.tail += num;
    }

    public virtual void Consume()
    {
      this.fill(1);
      ++this.absoluteNodeIndex;
      this.previousNode = this.lookahead[this.head];
      this.head = (this.head + 1) % this.lookahead.Length;
    }

    public virtual int LA(int i)
    {
      object t = (object) (ITree) this.LT(i);
      return t == null ? 0 : this.adaptor.GetNodeType(t);
    }

    public virtual int Mark()
    {
      if (this.markers == null)
      {
        this.markers = (IList) new ArrayList();
        this.markers.Add((object) null);
      }
      ++this.markDepth;
      UnBufferedTreeNodeStream.TreeWalkState treeWalkState;
      if (this.markDepth >= this.markers.Count)
      {
        treeWalkState = new UnBufferedTreeNodeStream.TreeWalkState();
        this.markers.Add((object) treeWalkState);
      }
      else
        treeWalkState = (UnBufferedTreeNodeStream.TreeWalkState) this.markers[this.markDepth];
      treeWalkState.absoluteNodeIndex = this.absoluteNodeIndex;
      treeWalkState.currentChildIndex = this.currentChildIndex;
      treeWalkState.currentNode = this.currentNode;
      treeWalkState.previousNode = this.previousNode;
      treeWalkState.nodeStackSize = this.nodeStack.Count;
      treeWalkState.indexStackSize = this.indexStack.Count;
      int lookaheadSize = this.LookaheadSize;
      int index = 0;
      treeWalkState.lookahead = new object[lookaheadSize];
      int k = 1;
      while (k <= lookaheadSize)
      {
        treeWalkState.lookahead[index] = this.LT(k);
        ++k;
        ++index;
      }
      this.lastMarker = this.markDepth;
      return this.markDepth;
    }

    public virtual void Release(int marker)
    {
      this.markDepth = marker;
      --this.markDepth;
    }

    public virtual void Rewind(int marker)
    {
      if (this.markers == null)
        return;
      UnBufferedTreeNodeStream.TreeWalkState marker1 = (UnBufferedTreeNodeStream.TreeWalkState) this.markers[marker];
      this.absoluteNodeIndex = marker1.absoluteNodeIndex;
      this.currentChildIndex = marker1.currentChildIndex;
      this.currentNode = marker1.currentNode;
      this.previousNode = marker1.previousNode;
      this.nodeStack.Capacity = marker1.nodeStackSize;
      this.indexStack.Capacity = marker1.indexStackSize;
      for (this.head = this.tail = 0; this.tail < marker1.lookahead.Length; ++this.tail)
        this.lookahead[this.tail] = marker1.lookahead[this.tail];
      this.Release(marker);
    }

    public void Rewind() => this.Rewind(this.lastMarker);

    public virtual void Seek(int index)
    {
      if (index < this.Index())
        throw new ArgumentOutOfRangeException("can't seek backwards in node stream", nameof (index));
      while (this.Index() < index)
        this.Consume();
    }

    public virtual int Index() => this.absoluteNodeIndex + 1;

    [Obsolete("Please use property Count instead.")]
    public virtual int Size() => this.Count;

    public virtual int Count => new CommonTreeNodeStream(this.root).Count;

    protected internal virtual object handleRootNode()
    {
      object obj = this.currentNode;
      this.currentChildIndex = 0;
      if (this.adaptor.IsNil(obj))
      {
        obj = this.VisitChild(this.currentChildIndex);
      }
      else
      {
        this.AddLookahead(obj);
        if (this.adaptor.GetChildCount(this.currentNode) == 0)
          this.currentNode = (object) null;
      }
      return obj;
    }

    protected internal virtual object VisitChild(int child)
    {
      this.nodeStack.Push(this.currentNode);
      this.indexStack.Push((object) child);
      if (child == 0 && !this.adaptor.IsNil(this.currentNode))
        this.AddNavigationNode(2);
      this.currentNode = this.adaptor.GetChild(this.currentNode, child);
      this.currentChildIndex = 0;
      object currentNode = this.currentNode;
      this.AddLookahead(currentNode);
      this.WalkBackToMostRecentNodeWithUnvisitedChildren();
      return currentNode;
    }

    protected internal virtual void AddNavigationNode(int ttype)
    {
      this.AddLookahead(ttype != 2 ? (!this.HasUniqueNavigationNodes ? this.up : this.adaptor.Create(3, "UP")) : (!this.HasUniqueNavigationNodes ? this.down : this.adaptor.Create(2, "DOWN")));
    }

    protected internal virtual void WalkBackToMostRecentNodeWithUnvisitedChildren()
    {
      while (this.currentNode != null && this.currentChildIndex >= this.adaptor.GetChildCount(this.currentNode))
      {
        this.currentNode = this.nodeStack.Pop();
        if (this.currentNode == null)
          break;
        this.currentChildIndex = (int) this.indexStack.Pop();
        ++this.currentChildIndex;
        if (this.currentChildIndex >= this.adaptor.GetChildCount(this.currentNode))
        {
          if (!this.adaptor.IsNil(this.currentNode))
            this.AddNavigationNode(3);
          if (this.currentNode == this.root)
            this.currentNode = (object) null;
        }
      }
    }

    public ITreeAdaptor TreeAdaptor => this.adaptor;

    public string SourceName => this.TokenStream.SourceName;

    public ITokenStream TokenStream
    {
      get => this.tokens;
      set => this.tokens = value;
    }

    public bool HasUniqueNavigationNodes
    {
      get => this.uniqueNavigationNodes;
      set => this.uniqueNavigationNodes = value;
    }

    public void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
    {
      throw new NotSupportedException("can't do stream rewrites yet");
    }

    public override string ToString() => this.ToString(this.root, (object) null);

    protected int LookaheadSize
    {
      get
      {
        return this.tail < this.head ? this.lookahead.Length - this.head + this.tail : this.tail - this.head;
      }
    }

    public virtual string ToString(object start, object stop)
    {
      if (start == null)
        return (string) null;
      if (this.tokens != null)
      {
        int tokenStartIndex = this.adaptor.GetTokenStartIndex(start);
        this.adaptor.GetTokenStopIndex(stop);
        int stop1 = stop == null || this.adaptor.GetNodeType(stop) != 3 ? this.Count - 1 : this.adaptor.GetTokenStopIndex(start);
        return this.tokens.ToString(tokenStartIndex, stop1);
      }
      StringBuilder buf = new StringBuilder();
      this.ToStringWork(start, stop, buf);
      return buf.ToString();
    }

    protected internal virtual void ToStringWork(object p, object stop, StringBuilder buf)
    {
      if (!this.adaptor.IsNil(p))
      {
        string str = this.adaptor.GetNodeText(p) ?? " " + (object) this.adaptor.GetNodeType(p);
        buf.Append(str);
      }
      if (p == stop)
        return;
      int childCount = this.adaptor.GetChildCount(p);
      if (childCount > 0 && !this.adaptor.IsNil(p))
      {
        buf.Append(" ");
        buf.Append(2);
      }
      for (int i = 0; i < childCount; ++i)
        this.ToStringWork(this.adaptor.GetChild(p, i), stop, buf);
      if (childCount <= 0 || this.adaptor.IsNil(p))
        return;
      buf.Append(" ");
      buf.Append(3);
    }

    protected class TreeWalkState
    {
      protected internal int currentChildIndex;
      protected internal int absoluteNodeIndex;
      protected internal object currentNode;
      protected internal object previousNode;
      protected internal int nodeStackSize;
      protected internal int indexStackSize;
      protected internal object[] lookahead;
    }
  }
}
