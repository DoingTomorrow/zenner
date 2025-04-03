// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ParseTree
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class ParseTree : BaseTree
  {
    public object payload;
    public List<IToken> hiddenTokens;

    public ParseTree(object label) => this.payload = label;

    public override string Text
    {
      get => this.ToString();
      set
      {
      }
    }

    public override int TokenStartIndex
    {
      get => 0;
      set
      {
      }
    }

    public override int TokenStopIndex
    {
      get => 0;
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

    public override ITree DupNode() => (ITree) null;

    public override string ToString()
    {
      if (!(this.payload is IToken))
        return this.payload.ToString();
      IToken payload = (IToken) this.payload;
      return payload.Type == -1 ? "<EOF>" : payload.Text;
    }

    public virtual string ToStringWithHiddenTokens()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.hiddenTokens != null)
      {
        for (int index = 0; index < this.hiddenTokens.Count; ++index)
        {
          IToken hiddenToken = this.hiddenTokens[index];
          stringBuilder.Append(hiddenToken.Text);
        }
      }
      string str = this.ToString();
      if (!str.Equals("<EOF>"))
        stringBuilder.Append(str);
      return stringBuilder.ToString();
    }

    public virtual string ToInputString()
    {
      StringBuilder buf = new StringBuilder();
      this.ToStringLeaves(buf);
      return buf.ToString();
    }

    protected virtual void ToStringLeaves(StringBuilder buf)
    {
      if (this.payload is IToken)
      {
        buf.Append(this.ToStringWithHiddenTokens());
      }
      else
      {
        for (int index = 0; this.Children != null && index < this.Children.Count; ++index)
          ((ParseTree) this.Children[index]).ToStringLeaves(buf);
      }
    }
  }
}
