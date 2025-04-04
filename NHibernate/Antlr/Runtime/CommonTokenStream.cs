// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.CommonTokenStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Collections;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime
{
  internal class CommonTokenStream : IIntStream, ITokenStream
  {
    protected ITokenSource tokenSource;
    protected IList tokens;
    protected IDictionary channelOverrideMap;
    protected HashList discardSet;
    protected int channel;
    protected bool discardOffChannelTokens;
    protected int lastMarker;
    protected int p = -1;

    public CommonTokenStream()
    {
      this.channel = 0;
      this.tokens = (IList) new ArrayList(500);
    }

    public CommonTokenStream(ITokenSource tokenSource)
      : this()
    {
      this.tokenSource = tokenSource;
    }

    public CommonTokenStream(ITokenSource tokenSource, int channel)
      : this(tokenSource)
    {
      this.channel = channel;
    }

    public virtual IToken LT(int k)
    {
      if (this.p == -1)
        this.FillBuffer();
      if (k == 0)
        return (IToken) null;
      if (k < 0)
        return this.LB(-k);
      if (this.p + k - 1 >= this.tokens.Count)
        return Token.EOF_TOKEN;
      int index1 = this.p;
      for (int index2 = 1; index2 < k; ++index2)
        index1 = this.SkipOffTokenChannels(index1 + 1);
      return index1 >= this.tokens.Count ? Token.EOF_TOKEN : (IToken) this.tokens[index1];
    }

    public virtual IToken Get(int i) => (IToken) this.tokens[i];

    public virtual ITokenSource TokenSource
    {
      get => this.tokenSource;
      set
      {
        this.tokenSource = value;
        this.tokens.Clear();
        this.p = -1;
        this.channel = 0;
      }
    }

    public virtual string SourceName => this.TokenSource.SourceName;

    public virtual string ToString(int start, int stop)
    {
      if (start < 0 || stop < 0)
        return (string) null;
      if (this.p == -1)
        this.FillBuffer();
      if (stop >= this.tokens.Count)
        stop = this.tokens.Count - 1;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = start; index <= stop; ++index)
      {
        IToken token = (IToken) this.tokens[index];
        stringBuilder.Append(token.Text);
      }
      return stringBuilder.ToString();
    }

    public virtual string ToString(IToken start, IToken stop)
    {
      return start != null && stop != null ? this.ToString(start.TokenIndex, stop.TokenIndex) : (string) null;
    }

    public virtual void Consume()
    {
      if (this.p >= this.tokens.Count)
        return;
      ++this.p;
      this.p = this.SkipOffTokenChannels(this.p);
    }

    public virtual int LA(int i) => this.LT(i).Type;

    public virtual int Mark()
    {
      if (this.p == -1)
        this.FillBuffer();
      this.lastMarker = this.Index();
      return this.lastMarker;
    }

    public virtual int Index() => this.p;

    public virtual void Rewind(int marker) => this.Seek(marker);

    public virtual void Rewind() => this.Seek(this.lastMarker);

    public virtual void Reset()
    {
      this.p = 0;
      this.lastMarker = 0;
    }

    public virtual void Release(int marker)
    {
    }

    public virtual void Seek(int index) => this.p = index;

    [Obsolete("Please use the property Count instead.")]
    public virtual int Size() => this.Count;

    public virtual int Count => this.tokens.Count;

    protected virtual void FillBuffer()
    {
      int num = 0;
      for (IToken token = this.tokenSource.NextToken(); token != null && token.Type != -1; token = this.tokenSource.NextToken())
      {
        bool flag = false;
        if (this.channelOverrideMap != null)
        {
          object channelOverride = this.channelOverrideMap[(object) token.Type];
          if (channelOverride != null)
            token.Channel = (int) channelOverride;
        }
        if (this.discardSet != null && this.discardSet.Contains((object) token.Type.ToString()))
          flag = true;
        else if (this.discardOffChannelTokens && token.Channel != this.channel)
          flag = true;
        if (!flag)
        {
          token.TokenIndex = num;
          this.tokens.Add((object) token);
          ++num;
        }
      }
      this.p = 0;
      this.p = this.SkipOffTokenChannels(this.p);
    }

    protected virtual int SkipOffTokenChannels(int i)
    {
      int count = this.tokens.Count;
      while (i < count && ((IToken) this.tokens[i]).Channel != this.channel)
        ++i;
      return i;
    }

    protected virtual int SkipOffTokenChannelsReverse(int i)
    {
      while (i >= 0 && ((IToken) this.tokens[i]).Channel != this.channel)
        --i;
      return i;
    }

    public virtual void SetTokenTypeChannel(int ttype, int channel)
    {
      if (this.channelOverrideMap == null)
        this.channelOverrideMap = (IDictionary) new Hashtable();
      this.channelOverrideMap[(object) ttype] = (object) channel;
    }

    public virtual void DiscardTokenType(int ttype)
    {
      if (this.discardSet == null)
        this.discardSet = new HashList();
      this.discardSet.Add((object) ttype.ToString(), (object) ttype);
    }

    public virtual void DiscardOffChannelTokens(bool discardOffChannelTokens)
    {
      this.discardOffChannelTokens = discardOffChannelTokens;
    }

    public virtual IList GetTokens()
    {
      if (this.p == -1)
        this.FillBuffer();
      return this.tokens;
    }

    public virtual IList GetTokens(int start, int stop)
    {
      return this.GetTokens(start, stop, (BitSet) null);
    }

    public virtual IList GetTokens(int start, int stop, BitSet types)
    {
      if (this.p == -1)
        this.FillBuffer();
      if (stop >= this.tokens.Count)
        stop = this.tokens.Count - 1;
      if (start < 0)
        start = 0;
      if (start > stop)
        return (IList) null;
      IList tokens = (IList) new ArrayList();
      for (int index = start; index <= stop; ++index)
      {
        IToken token = (IToken) this.tokens[index];
        if (types == null || types.Member(token.Type))
          tokens.Add((object) token);
      }
      if (tokens.Count == 0)
        tokens = (IList) null;
      return tokens;
    }

    public virtual IList GetTokens(int start, int stop, IList types)
    {
      return this.GetTokens(start, stop, new BitSet(types));
    }

    public virtual IList GetTokens(int start, int stop, int ttype)
    {
      return this.GetTokens(start, stop, BitSet.Of(ttype));
    }

    protected virtual IToken LB(int k)
    {
      if (this.p == -1)
        this.FillBuffer();
      if (k == 0)
        return (IToken) null;
      if (this.p - k < 0)
        return (IToken) null;
      int index1 = this.p;
      for (int index2 = 1; index2 <= k; ++index2)
        index1 = this.SkipOffTokenChannelsReverse(index1 - 1);
      return index1 < 0 ? (IToken) null : (IToken) this.tokens[index1];
    }

    public override string ToString()
    {
      if (this.p == -1)
        this.FillBuffer();
      return this.ToString(0, this.tokens.Count - 1);
    }
  }
}
