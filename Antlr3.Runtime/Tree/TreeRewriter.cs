// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeRewriter
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;
using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreeRewriter : TreeParser
  {
    protected bool showTransformations;
    protected ITokenStream originalTokenStream;
    protected ITreeAdaptor originalAdaptor;
    private Func<IAstRuleReturnScope> topdown_func;
    private Func<IAstRuleReturnScope> bottomup_func;

    public TreeRewriter(ITreeNodeStream input)
      : this(input, new RecognizerSharedState())
    {
    }

    public TreeRewriter(ITreeNodeStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.originalAdaptor = input.TreeAdaptor;
      this.originalTokenStream = input.TokenStream;
      this.topdown_func = (Func<IAstRuleReturnScope>) (() => this.Topdown());
      this.bottomup_func = (Func<IAstRuleReturnScope>) (() => this.Bottomup());
    }

    public virtual object ApplyOnce(object t, Func<IAstRuleReturnScope> whichRule)
    {
      if (t == null)
        return (object) null;
      try
      {
        this.SetState(new RecognizerSharedState());
        this.SetTreeNodeStream((ITreeNodeStream) new CommonTreeNodeStream(this.originalAdaptor, t));
        ((CommonTreeNodeStream) this.input).TokenStream = this.originalTokenStream;
        this.BacktrackingLevel = 1;
        IAstRuleReturnScope astRuleReturnScope = whichRule();
        this.BacktrackingLevel = 0;
        if (this.Failed)
          return t;
        if (this.showTransformations && astRuleReturnScope != null && !t.Equals(astRuleReturnScope.Tree) && astRuleReturnScope.Tree != null)
          this.ReportTransformation(t, astRuleReturnScope.Tree);
        return astRuleReturnScope != null && astRuleReturnScope.Tree != null ? astRuleReturnScope.Tree : t;
      }
      catch (RecognitionException ex)
      {
      }
      return t;
    }

    public virtual object ApplyRepeatedly(object t, Func<IAstRuleReturnScope> whichRule)
    {
      bool flag = true;
      while (flag)
      {
        object obj = this.ApplyOnce(t, whichRule);
        flag = !t.Equals(obj);
        t = obj;
      }
      return t;
    }

    public virtual object Downup(object t) => this.Downup(t, false);

    public virtual object Downup(object t, bool showTransformations)
    {
      this.showTransformations = showTransformations;
      t = new TreeVisitor((ITreeAdaptor) new CommonTreeAdaptor()).Visit(t, (Func<object, object>) (o => this.ApplyOnce(o, this.topdown_func)), (Func<object, object>) (o => this.ApplyRepeatedly(o, this.bottomup_func)));
      return t;
    }

    protected virtual IAstRuleReturnScope Topdown() => (IAstRuleReturnScope) null;

    protected virtual IAstRuleReturnScope Bottomup() => (IAstRuleReturnScope) null;

    protected virtual void ReportTransformation(object oldTree, object newTree)
    {
      ITree tree1 = oldTree as ITree;
      ITree tree2 = newTree as ITree;
      Console.WriteLine("{0} -> {1}", tree1 != null ? (object) tree1.ToStringTree() : (object) "??", tree2 != null ? (object) tree2.ToStringTree() : (object) "??");
    }
  }
}
