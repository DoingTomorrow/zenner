// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.UpdateStatement
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  [Serializable]
  public class UpdateStatement(IToken token) : AbstractRestrictableStatement(token)
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (UpdateStatement));

    public override bool NeedsExecutor => true;

    public override int StatementType => 53;

    public IASTNode SetClause => ASTUtil.FindTypeInChildren((IASTNode) this, 46);

    protected override IInternalLogger GetLog() => UpdateStatement.Log;

    protected override int GetWhereClauseParentTokenType() => 46;
  }
}
