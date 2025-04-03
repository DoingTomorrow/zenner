// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.CommonTokenStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class CommonTokenStream : BufferedTokenStream
  {
    private int _channel;

    public CommonTokenStream()
    {
    }

    public CommonTokenStream(ITokenSource tokenSource)
      : this(tokenSource, 0)
    {
    }

    public CommonTokenStream(ITokenSource tokenSource, int channel)
      : base(tokenSource)
    {
      this._channel = channel;
    }

    public int Channel => this._channel;

    public override ITokenSource TokenSource
    {
      get => base.TokenSource;
      set
      {
        base.TokenSource = value;
        this._channel = 0;
      }
    }

    public override void Consume()
    {
      if (this._p == -1)
        this.Setup();
      ++this._p;
      this._p = this.SkipOffTokenChannels(this._p);
    }

    protected override IToken LB(int k)
    {
      if (k == 0 || this._p - k < 0)
        return (IToken) null;
      int index1 = this._p;
      for (int index2 = 1; index2 <= k; ++index2)
        index1 = this.SkipOffTokenChannelsReverse(index1 - 1);
      return index1 < 0 ? (IToken) null : this._tokens[index1];
    }

    public override IToken LT(int k)
    {
      if (this._p == -1)
        this.Setup();
      if (k == 0)
        return (IToken) null;
      if (k < 0)
        return this.LB(-k);
      int index1 = this._p;
      for (int index2 = 1; index2 < k; ++index2)
        index1 = this.SkipOffTokenChannels(index1 + 1);
      if (index1 > this.Range)
        this.Range = index1;
      return this._tokens[index1];
    }

    protected virtual int SkipOffTokenChannels(int i)
    {
      this.Sync(i);
      while (this._tokens[i].Channel != this._channel)
      {
        ++i;
        this.Sync(i);
      }
      return i;
    }

    protected virtual int SkipOffTokenChannelsReverse(int i)
    {
      while (i >= 0 && this._tokens[i].Channel != this._channel)
        --i;
      return i;
    }

    public override void Reset()
    {
      base.Reset();
      this._p = this.SkipOffTokenChannels(0);
    }

    protected override void Setup()
    {
      this._p = 0;
      this._p = this.SkipOffTokenChannels(this._p);
    }
  }
}
