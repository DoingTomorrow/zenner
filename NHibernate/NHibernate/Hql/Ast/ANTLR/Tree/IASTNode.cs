// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IASTNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public interface IASTNode : IEnumerable<IASTNode>, IEnumerable
  {
    bool IsNil { get; }

    int Type { get; set; }

    string Text { get; set; }

    int Line { get; }

    int CharPositionInLine { get; }

    int ChildCount { get; }

    int ChildIndex { get; }

    IASTNode Parent { get; set; }

    IASTNode NextSibling { get; set; }

    IASTNode GetChild(int index);

    IASTNode GetFirstChild();

    void SetFirstChild(IASTNode newChild);

    void SetChild(int index, IASTNode newChild);

    IASTNode AddChild(IASTNode childNode);

    IASTNode InsertChild(int index, IASTNode child);

    IASTNode AddSibling(IASTNode newSibling);

    void ClearChildren();

    void RemoveChild(IASTNode child);

    void AddChildren(IEnumerable<IASTNode> children);

    void AddChildren(params IASTNode[] children);

    IASTNode DupNode();

    IToken Token { get; }

    string ToStringTree();
  }
}
