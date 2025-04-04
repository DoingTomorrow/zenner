// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.InsertStatement
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  [Serializable]
  public class InsertStatement(IToken token) : AbstractStatement(token)
  {
    public override bool NeedsExecutor => true;

    public override int StatementType => 29;

    public IntoClause IntoClause => (IntoClause) this.GetFirstChild();

    public SelectClause SelectClause => ((QueryNode) this.IntoClause.NextSibling).GetSelectClause();

    public virtual void Validate() => this.IntoClause.ValidateTypes(this.SelectClause);
  }
}
