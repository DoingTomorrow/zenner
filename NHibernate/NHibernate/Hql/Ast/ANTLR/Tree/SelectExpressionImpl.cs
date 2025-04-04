// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.SelectExpressionImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class SelectExpressionImpl(IToken token) : FromReferenceNode(token), ISelectExpression
  {
    public override void ResolveIndex(IASTNode parent) => throw new InvalidOperationException();

    public override void SetScalarColumnText(int i)
    {
      this.Text = this.FromElement.RenderScalarIdentifierSelect(i);
    }

    public override void Resolve(
      bool generateJoin,
      bool implicitJoin,
      string classAlias,
      IASTNode parent)
    {
    }
  }
}
