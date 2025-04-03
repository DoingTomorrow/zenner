// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ITree
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System.Collections.Generic;

#nullable disable
namespace Antlr.Runtime.Tree
{
  public interface ITree
  {
    ITree GetChild(int i);

    int ChildCount { get; }

    ITree Parent { get; set; }

    bool HasAncestor(int ttype);

    ITree GetAncestor(int ttype);

    IList<ITree> GetAncestors();

    int ChildIndex { get; set; }

    void FreshenParentAndChildIndexes();

    void AddChild(ITree t);

    void SetChild(int i, ITree t);

    object DeleteChild(int i);

    void ReplaceChildren(int startChildIndex, int stopChildIndex, object t);

    bool IsNil { get; }

    int TokenStartIndex { get; set; }

    int TokenStopIndex { get; set; }

    ITree DupNode();

    int Type { get; }

    string Text { get; }

    int Line { get; }

    int CharPositionInLine { get; }

    string ToStringTree();

    string ToString();
  }
}
