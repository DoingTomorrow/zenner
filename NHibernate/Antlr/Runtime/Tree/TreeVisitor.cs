// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal class TreeVisitor
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
      int childCount = this.adaptor.GetChildCount(t);
      for (int i = 0; i < childCount; ++i)
      {
        object child1 = this.Visit(this.adaptor.GetChild(t, i), action);
        object child2 = this.adaptor.GetChild(t, i);
        if (child1 != child2)
          this.adaptor.SetChild(t, i, child1);
      }
      if (action != null && !flag)
        t = action.Post(t);
      return t;
    }
  }
}
