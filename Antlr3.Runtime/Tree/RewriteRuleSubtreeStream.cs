// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleSubtreeStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteRuleSubtreeStream : RewriteRuleElementStream
  {
    public RewriteRuleSubtreeStream(ITreeAdaptor adaptor, string elementDescription)
      : base(adaptor, elementDescription)
    {
    }

    public RewriteRuleSubtreeStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      object oneElement)
      : base(adaptor, elementDescription, oneElement)
    {
    }

    public RewriteRuleSubtreeStream(
      ITreeAdaptor adaptor,
      string elementDescription,
      IList elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    public virtual object NextNode()
    {
      int count = this.Count;
      if (this.dirty || this.cursor >= count && count == 1)
        return this.adaptor.DupNode(this.NextCore());
      object obj = this.NextCore();
      while (this.adaptor.IsNil(obj) && this.adaptor.GetChildCount(obj) == 1)
        obj = this.adaptor.GetChild(obj, 0);
      return this.adaptor.DupNode(obj);
    }

    protected override object Dup(object el) => this.adaptor.DupTree(el);
  }
}
