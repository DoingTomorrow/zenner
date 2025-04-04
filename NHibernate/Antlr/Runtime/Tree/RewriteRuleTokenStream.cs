// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleTokenStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class RewriteRuleTokenStream : RewriteRuleElementStream<IToken>
  {
    public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription)
      : base(adaptor, elementDescription)
    {
    }

    public RewriteRuleTokenStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IToken oneElement)
      : base(adaptor, elementDescription, oneElement)
    {
    }

    public RewriteRuleTokenStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IList<IToken> elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    [Obsolete("This constructor is for internal use only and might be phased out soon. Use instead the one with IList<T>.")]
    public RewriteRuleTokenStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    public object NextNode() => this.adaptor.Create((IToken) this._Next());

    public IToken NextToken() => (IToken) this._Next();

    protected override object ToTree(IToken el) => (object) el;
  }
}
