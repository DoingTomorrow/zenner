// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreePatternParser
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreePatternParser
  {
    protected TreePatternLexer tokenizer;
    protected int ttype;
    protected TreeWizard wizard;
    protected ITreeAdaptor adaptor;

    public TreePatternParser(TreePatternLexer tokenizer, TreeWizard wizard, ITreeAdaptor adaptor)
    {
      this.tokenizer = tokenizer;
      this.wizard = wizard;
      this.adaptor = adaptor;
      this.ttype = tokenizer.NextToken();
    }

    public virtual object Pattern()
    {
      if (this.ttype == 1)
        return this.ParseTree();
      if (this.ttype != 3)
        return (object) null;
      object node = this.ParseNode();
      return this.ttype == -1 ? node : (object) null;
    }

    public virtual object ParseTree()
    {
      this.ttype = this.ttype == 1 ? this.tokenizer.NextToken() : throw new InvalidOperationException("No beginning.");
      object node1 = this.ParseNode();
      if (node1 == null)
        return (object) null;
      while (this.ttype == 1 || this.ttype == 3 || this.ttype == 5 || this.ttype == 7)
      {
        if (this.ttype == 1)
        {
          object tree = this.ParseTree();
          this.adaptor.AddChild(node1, tree);
        }
        else
        {
          object node2 = this.ParseNode();
          if (node2 == null)
            return (object) null;
          this.adaptor.AddChild(node1, node2);
        }
      }
      this.ttype = this.ttype == 2 ? this.tokenizer.NextToken() : throw new InvalidOperationException("No end.");
      return node1;
    }

    public virtual object ParseNode()
    {
      string str1 = (string) null;
      if (this.ttype == 5)
      {
        this.ttype = this.tokenizer.NextToken();
        if (this.ttype != 3)
          return (object) null;
        str1 = this.tokenizer.sval.ToString();
        this.ttype = this.tokenizer.NextToken();
        if (this.ttype != 6)
          return (object) null;
        this.ttype = this.tokenizer.NextToken();
      }
      if (this.ttype == 7)
      {
        this.ttype = this.tokenizer.NextToken();
        TreeWizard.TreePattern node = (TreeWizard.TreePattern) new TreeWizard.WildcardTreePattern((IToken) new CommonToken(0, "."));
        if (str1 != null)
          node.label = str1;
        return (object) node;
      }
      if (this.ttype != 3)
        return (object) null;
      string tokenName = this.tokenizer.sval.ToString();
      this.ttype = this.tokenizer.NextToken();
      if (tokenName.Equals("nil"))
        return this.adaptor.Nil();
      string text = tokenName;
      string str2 = (string) null;
      if (this.ttype == 4)
      {
        str2 = this.tokenizer.sval.ToString();
        text = str2;
        this.ttype = this.tokenizer.NextToken();
      }
      int tokenType = this.wizard.GetTokenType(tokenName);
      if (tokenType == 0)
        return (object) null;
      object node1 = this.adaptor.Create(tokenType, text);
      if (str1 != null && node1.GetType() == typeof (TreeWizard.TreePattern))
        ((TreeWizard.TreePattern) node1).label = str1;
      if (str2 != null && node1.GetType() == typeof (TreeWizard.TreePattern))
        ((TreeWizard.TreePattern) node1).hasTextArg = true;
      return node1;
    }
  }
}
