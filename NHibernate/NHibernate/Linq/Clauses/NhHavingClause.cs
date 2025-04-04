// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Clauses.NhHavingClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Clauses
{
  public class NhHavingClause(Expression predicate) : WhereClause(predicate)
  {
    public override string ToString()
    {
      return "having " + FormattingExpressionTreeVisitor.Format(this.Predicate);
    }
  }
}
