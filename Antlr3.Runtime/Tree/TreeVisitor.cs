// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeVisitor
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreeVisitor
  {
    protected ITreeAdaptor adaptor;

    public TreeVisitor(ITreeAdaptor adaptor) => this.adaptor = adaptor;

    public TreeVisitor()
      : this((ITreeAdaptor) new CommonTreeAdaptor())
    {
    }

    public object Visit(object t, ITreeVisitorAction action)
    {
      bool flag = this.adaptor.IsNil(t);
      if (action != null && !flag)
        t = action.Pre(t);
      for (int i = 0; i < this.adaptor.GetChildCount(t); ++i)
        this.Visit(this.adaptor.GetChild(t, i), action);
      if (action != null && !flag)
        t = action.Post(t);
      return t;
    }

    public object Visit(object t, Func<object, object> preAction, Func<object, object> postAction)
    {
      return this.Visit(t, (ITreeVisitorAction) new TreeVisitorAction(preAction, postAction));
    }
  }
}
