// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.ITree
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace Antlr.Runtime.Tree
{
  internal interface ITree
  {
    int ChildCount { get; }

    ITree Parent { get; set; }

    bool HasAncestor(int ttype);

    ITree GetAncestor(int ttype);

    IList GetAncestors();

    int ChildIndex { get; set; }

    void FreshenParentAndChildIndexes();

    bool IsNil { get; }

    int Type { get; }

    string Text { get; }

    int Line { get; }

    int CharPositionInLine { get; }

    ITree GetChild(int i);

    void AddChild(ITree t);

    void SetChild(int i, ITree t);

    object DeleteChild(int i);

    void ReplaceChildren(int startChildIndex, int stopChildIndex, object t);

    int TokenStartIndex { get; set; }

    int TokenStopIndex { get; set; }

    ITree DupNode();

    string ToStringTree();

    string ToString();
  }
}
