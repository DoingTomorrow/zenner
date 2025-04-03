// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleTokenStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteRuleTokenStream : RewriteRuleElementStream
  {
    public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription)
      : base(adaptor, elementDescription)
    {
    }

    public RewriteRuleTokenStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      object oneElement)
      : base(adaptor, elementDescription, oneElement)
    {
    }

    public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    public virtual object NextNode() => this.adaptor.Create((IToken) this.NextCore());

    public virtual IToken NextToken() => (IToken) this.NextCore();

    protected override object ToTree(object el) => el;

    protected override object Dup(object el)
    {
      throw new NotSupportedException("dup can't be called for a token stream.");
    }
  }
}
