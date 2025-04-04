// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Debug.DebugTokenStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime.Debug
{
  internal class DebugTokenStream : IIntStream, ITokenStream
  {
    protected internal IDebugEventListener dbg;
    public ITokenStream input;
    protected internal bool initialStreamState = true;
    protected int lastMarker;

    public DebugTokenStream(ITokenStream input, IDebugEventListener dbg)
    {
      this.input = input;
      this.DebugListener = dbg;
      input.LT(1);
    }

    public virtual IDebugEventListener DebugListener
    {
      set => this.dbg = value;
    }

    public virtual void Consume()
    {
      if (this.initialStreamState)
        this.ConsumeInitialHiddenTokens();
      int num1 = this.input.Index();
      IToken t = this.input.LT(1);
      this.input.Consume();
      int num2 = this.input.Index();
      this.dbg.ConsumeToken(t);
      if (num2 <= num1 + 1)
        return;
      for (int i = num1 + 1; i < num2; ++i)
        this.dbg.ConsumeHiddenToken(this.input.Get(i));
    }

    protected internal virtual void ConsumeInitialHiddenTokens()
    {
      int num = this.input.Index();
      for (int i = 0; i < num; ++i)
        this.dbg.ConsumeHiddenToken(this.input.Get(i));
      this.initialStreamState = false;
    }

    public virtual IToken LT(int i)
    {
      if (this.initialStreamState)
        this.ConsumeInitialHiddenTokens();
      this.dbg.LT(i, this.input.LT(i));
      return this.input.LT(i);
    }

    public virtual int LA(int i)
    {
      if (this.initialStreamState)
        this.ConsumeInitialHiddenTokens();
      this.dbg.LT(i, this.input.LT(i));
      return this.input.LA(i);
    }

    public virtual IToken Get(int i) => this.input.Get(i);

    public virtual int Mark()
    {
      this.lastMarker = this.input.Mark();
      this.dbg.Mark(this.lastMarker);
      return this.lastMarker;
    }

    public virtual int Index() => this.input.Index();

    public virtual void Rewind(int marker)
    {
      this.dbg.Rewind(marker);
      this.input.Rewind(marker);
    }

    public virtual void Rewind()
    {
      this.dbg.Rewind();
      this.input.Rewind(this.lastMarker);
    }

    public virtual void Release(int marker)
    {
    }

    public virtual void Seek(int index) => this.input.Seek(index);

    [Obsolete("Please use property Count instead.")]
    public virtual int Size() => this.Count;

    public virtual int Count => this.input.Count;

    public virtual ITokenSource TokenSource => this.input.TokenSource;

    public virtual string SourceName => this.TokenSource.SourceName;

    public override string ToString() => this.input.ToString();

    public virtual string ToString(int start, int stop) => this.input.ToString(start, stop);

    public virtual string ToString(IToken start, IToken stop) => this.input.ToString(start, stop);
  }
}
