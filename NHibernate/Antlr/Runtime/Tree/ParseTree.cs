// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ParseTree
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class ParseTree : BaseTree
  {
    public object payload;
    public IList hiddenTokens;

    public ParseTree(object label) => this.payload = label;

    public override int Type => 0;

    public override string Text => this.ToString();

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

    public override ITree DupNode() => (ITree) null;

    public override string ToString()
    {
      if (!(this.payload is IToken))
        return this.payload.ToString();
      IToken payload = (IToken) this.payload;
      return payload.Type == Token.EOF ? "<EOF>" : payload.Text;
    }

    public string ToStringWithHiddenTokens()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.hiddenTokens != null)
      {
        for (int index = 0; index < this.hiddenTokens.Count; ++index)
        {
          IToken hiddenToken = (IToken) this.hiddenTokens[index];
          stringBuilder.Append(hiddenToken.Text);
        }
      }
      string str = this.ToString();
      if (str != "<EOF>")
        stringBuilder.Append(str);
      return stringBuilder.ToString();
    }

    public string ToInputString()
    {
      StringBuilder buf = new StringBuilder();
      this._ToStringLeaves(buf);
      return buf.ToString();
    }

    public void _ToStringLeaves(StringBuilder buf)
    {
      if (this.payload is IToken)
      {
        buf.Append(this.ToStringWithHiddenTokens());
      }
      else
      {
        for (int index = 0; this.children != null && index < this.children.Count; ++index)
          ((ParseTree) this.children[index])._ToStringLeaves(buf);
      }
    }
  }
}
