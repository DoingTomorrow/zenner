// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleNodeStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class RewriteRuleNodeStream : RewriteRuleElementStream<object>
  {
    public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription)
      : base(adaptor, elementDescription)
    {
    }

    public RewriteRuleNodeStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      object oneElement)
      : base(adaptor, elementDescription, oneElement)
    {
    }

    public RewriteRuleNodeStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IList<object> elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    [Obsolete("This constructor is for internal use only and might be phased out soon. Use instead the one with IList<T>.")]
    public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    public object NextNode() => this._Next();

    protected override object ToTree(object el) => this.adaptor.DupNode(el);
  }
}
