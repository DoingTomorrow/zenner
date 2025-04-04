// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.CommonErrorNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class CommonErrorNode : CommonTree
  {
    public IIntStream input;
    public IToken start;
    public IToken stop;
    [NonSerialized]
    public RecognitionException trappedException;

    public CommonErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
    {
      if (stop == null || stop.TokenIndex < start.TokenIndex && stop.Type != Antlr.Runtime.Token.EOF)
        stop = start;
      this.input = (IIntStream) input;
      this.start = start;
      this.stop = stop;
      this.trappedException = e;
    }

    public override bool IsNil => false;

    public override int Type => 0;

    public override string Text
    {
      get
      {
        string text;
        if (this.start is IToken)
        {
          int tokenIndex = this.start.TokenIndex;
          int stop = this.stop.TokenIndex;
          if (this.stop.Type == Antlr.Runtime.Token.EOF)
            stop = this.input.Count;
          text = ((ITokenStream) this.input).ToString(tokenIndex, stop);
        }
        else
          text = !(this.start is ITree) ? "<unknown>" : ((ITreeNodeStream) this.input).ToString((object) this.start, (object) this.stop);
        return text;
      }
    }

    public override string ToString()
    {
      if (this.trappedException is MissingTokenException)
        return "<missing type: " + (object) ((MissingTokenException) this.trappedException).MissingType + ">";
      if (this.trappedException is UnwantedTokenException)
        return "<extraneous: " + (object) ((UnwantedTokenException) this.trappedException).UnexpectedToken + ", resync=" + this.Text + ">";
      if (this.trappedException is MismatchedTokenException)
        return "<mismatched token: " + (object) this.trappedException.Token + ", resync=" + this.Text + ">";
      if (!(this.trappedException is NoViableAltException))
        return "<error: " + this.Text + ">";
      return "<unexpected: " + (object) this.trappedException.Token + ", resync=" + this.Text + ">";
    }
  }
}
