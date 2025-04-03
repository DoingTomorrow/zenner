// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteRuleNodeStream
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  public class RewriteRuleNodeStream : RewriteRuleElementStream
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

    public RewriteRuleNodeStream(ITreeAdaptor adaptor, string elementDescription, IList elements)
      : base(adaptor, elementDescription, elements)
    {
    }

    public virtual object NextNode() => this.NextCore();

    protected override object ToTree(object el) => this.adaptor.DupNode(el);

    protected override object Dup(object el)
    {
      throw new NotSupportedException("dup can't be called for a node stream.");
    }
  }
}
