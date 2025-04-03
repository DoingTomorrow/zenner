// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.AntlrRuntime_BaseTreeDebugView
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System.Diagnostics;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal sealed class AntlrRuntime_BaseTreeDebugView
  {
    private readonly BaseTree _tree;

    public AntlrRuntime_BaseTreeDebugView(BaseTree tree) => this._tree = tree;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public ITree[] Children
    {
      get
      {
        if (this._tree == null || this._tree.Children == null)
          return (ITree[]) null;
        ITree[] array = new ITree[this._tree.Children.Count];
        this._tree.Children.CopyTo(array, 0);
        return array;
      }
    }
  }
}
