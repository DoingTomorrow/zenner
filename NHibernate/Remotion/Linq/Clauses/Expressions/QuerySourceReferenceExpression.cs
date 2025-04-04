// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.QuerySourceReferenceExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.Expressions
{
  public class QuerySourceReferenceExpression : Expression
  {
    public const ExpressionType ExpressionType = (ExpressionType) 100001;

    public QuerySourceReferenceExpression(IQuerySource querySource)
      : base((ExpressionType) 100001, ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource).ItemType)
    {
      this.ReferencedQuerySource = querySource;
    }

    public IQuerySource ReferencedQuerySource { get; private set; }

    public override bool Equals(object obj)
    {
      return obj is QuerySourceReferenceExpression referenceExpression && this.ReferencedQuerySource == referenceExpression.ReferencedQuerySource;
    }

    public override int GetHashCode() => this.ReferencedQuerySource.GetHashCode();
  }
}
