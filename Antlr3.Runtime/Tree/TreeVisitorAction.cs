// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.TreeVisitorAction
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Misc;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public class TreeVisitorAction : ITreeVisitorAction
  {
    private readonly Func<object, object> _preAction;
    private readonly Func<object, object> _postAction;

    public TreeVisitorAction(Func<object, object> preAction, Func<object, object> postAction)
    {
      this._preAction = preAction;
      this._postAction = postAction;
    }

    public object Pre(object t) => this._preAction != null ? this._preAction(t) : t;

    public object Post(object t) => this._postAction != null ? this._postAction(t) : t;
  }
}
