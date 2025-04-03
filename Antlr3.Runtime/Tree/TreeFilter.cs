// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeFilter
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreeFilter : TreeParser
  {
    protected ITokenStream originalTokenStream;
    protected ITreeAdaptor originalAdaptor;

    public TreeFilter(ITreeNodeStream input)
      : this(input, new RecognizerSharedState())
    {
    }

    public TreeFilter(ITreeNodeStream input, RecognizerSharedState state)
      : base(input, state)
    {
      this.originalAdaptor = input.TreeAdaptor;
      this.originalTokenStream = input.TokenStream;
    }

    public virtual void ApplyOnce(object t, Action whichRule)
    {
      if (t == null)
        return;
      try
      {
        this.SetState(new RecognizerSharedState());
        this.SetTreeNodeStream((ITreeNodeStream) new CommonTreeNodeStream(this.originalAdaptor, t));
        ((CommonTreeNodeStream) this.input).TokenStream = this.originalTokenStream;
        this.BacktrackingLevel = 1;
        whichRule();
        this.BacktrackingLevel = 0;
      }
      catch (RecognitionException ex)
      {
      }
    }

    public virtual void Downup(object t)
    {
      TreeVisitor treeVisitor = new TreeVisitor((ITreeAdaptor) new CommonTreeAdaptor());
      Func<object, object> preAction = (Func<object, object>) (o =>
      {
        this.ApplyOnce(o, new Action(this.Topdown));
        return o;
      });
      Func<object, object> postAction = (Func<object, object>) (o =>
      {
        this.ApplyOnce(o, new Action(this.Bottomup));
        return o;
      });
      treeVisitor.Visit(t, preAction, postAction);
    }

    protected virtual void Topdown()
    {
    }

    protected virtual void Bottomup()
    {
    }
  }
}
