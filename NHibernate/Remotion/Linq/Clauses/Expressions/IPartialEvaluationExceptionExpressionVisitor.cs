// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.IPartialEvaluationExceptionExpressionVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.Expressions
{
  public interface IPartialEvaluationExceptionExpressionVisitor
  {
    Expression VisitPartialEvaluationExceptionExpression(
      PartialEvaluationExceptionExpression partialEvaluationExceptionExpression);
  }
}
