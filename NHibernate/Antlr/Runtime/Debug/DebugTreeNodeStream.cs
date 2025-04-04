// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugTreeNodeStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugTreeNodeStream : IIntStream, ITreeNodeStream
  {
    protected IDebugEventListener dbg;
    protected ITreeAdaptor adaptor;
    protected ITreeNodeStream input;
    protected bool initialStreamState = true;
    protected int lastMarker;

    public DebugTreeNodeStream(ITreeNodeStream input, IDebugEventListener dbg)
    {
      this.input = input;
      this.adaptor = input.TreeAdaptor;
      this.input.HasUniqueNavigationNodes = true;
      this.SetDebugListener(dbg);
    }

    public void SetDebugListener(IDebugEventListener dbg) => this.dbg = dbg;

    public ITokenStream TokenStream => this.input.TokenStream;

    public string SourceName => this.TokenStream.SourceName;

    public ITreeAdaptor TreeAdaptor => this.adaptor;

    public void Consume()
    {
      object t = this.input.LT(1);
      this.input.Consume();
      this.dbg.ConsumeNode(t);
    }

    public object Get(int i) => this.input.Get(i);

    public object LT(int i)
    {
      object t = this.input.LT(i);
      this.dbg.LT(i, t);
      return t;
    }

    public int LA(int i)
    {
      object t = this.input.LT(i);
      int nodeType = this.adaptor.GetNodeType(t);
      this.dbg.LT(i, t);
      return nodeType;
    }

    public int Mark()
    {
      this.lastMarker = this.input.Mark();
      this.dbg.Mark(this.lastMarker);
      return this.lastMarker;
    }

    public int Index() => this.input.Index();

    public void Rewind(int marker)
    {
      this.dbg.Rewind(marker);
      this.input.Rewind(marker);
    }

    public void Rewind()
    {
      this.dbg.Rewind();
      this.input.Rewind(this.lastMarker);
    }

    public void Release(int marker)
    {
    }

    public void Seek(int index) => this.input.Seek(index);

    [Obsolete("Please use property Count instead.")]
    public int Size() => this.Count;

    public int Count => this.input.Count;

    public object TreeSource => (object) this.input;

    public virtual bool HasUniqueNavigationNodes
    {
      set => this.input.HasUniqueNavigationNodes = value;
    }

    public void ReplaceChildren(object parent, int startChildIndex, int stopChildIndex, object t)
    {
      this.input.ReplaceChildren(parent, startChildIndex, stopChildIndex, t);
    }

    public string ToString(object start, object stop) => this.input.ToString(start, stop);
  }
}
