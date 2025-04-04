// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.ASTIterator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class ASTIterator : IEnumerable<IASTNode>, IEnumerable
  {
    private readonly Stack<IASTNode> _stack = new Stack<IASTNode>();
    private IASTNode _current;

    public ASTIterator(IASTNode tree) => this._current = tree;

    public IEnumerator<IASTNode> GetEnumerator()
    {
      this.Down();
      while (this._current != null)
      {
        yield return this._current;
        this._current = this._current.NextSibling;
        if (this._current == null)
        {
          if (this._stack.Count > 0)
            this._current = this._stack.Pop();
        }
        else
          this.Down();
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private void Down()
    {
      for (; this._current != null && this._current.ChildCount > 0; this._current = this._current.GetChild(0))
        this._stack.Push(this._current);
    }
  }
}
