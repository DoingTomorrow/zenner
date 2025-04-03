// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.CommonErrorNode
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class CommonErrorNode : CommonTree
  {
    public IIntStream input;
    public IToken start;
    public IToken stop;
    public RecognitionException trappedException;

    public CommonErrorNode(ITokenStream input, IToken start, IToken stop, RecognitionException e)
    {
      if (stop == null || stop.TokenIndex < start.TokenIndex && stop.Type != -1)
        stop = start;
      this.input = (IIntStream) input;
      this.start = start;
      this.stop = stop;
      this.trappedException = e;
    }

    public override bool IsNil => false;

    public override string Text
    {
      get
      {
        string text;
        if (this.start != null)
        {
          int tokenIndex = this.start.TokenIndex;
          int stop = this.stop.TokenIndex;
          if (this.stop.Type == -1)
            stop = this.input.Count;
          text = ((ITokenStream) this.input).ToString(tokenIndex, stop);
        }
        else
          text = !(this.start is ITree) ? "<unknown>" : ((ITreeNodeStream) this.input).ToString((object) this.start, (object) this.stop);
        return text;
      }
      set
      {
      }
    }

    public override int Type
    {
      get => 0;
      set
      {
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
