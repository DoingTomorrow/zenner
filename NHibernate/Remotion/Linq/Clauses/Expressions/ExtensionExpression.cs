// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.Expressions.ExtensionExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.Expressions
{
  public abstract class ExtensionExpression(Type type, ExpressionType nodeType) : Expression(nodeType, ArgumentUtility.CheckNotNull<Type>("", type))
  {
    public const ExpressionType DefaultExtensionExpressionNodeType = (ExpressionType) 150000;

    protected ExtensionExpression(Type type)
      : this(ArgumentUtility.CheckNotNull<Type>("", type), (ExpressionType) 150000)
    {
    }

    public virtual bool CanReduce => false;

    public virtual Expression Reduce()
    {
      if (this.CanReduce)
        throw new InvalidOperationException("Reducible nodes must override the Reduce method.");
      return (Expression) this;
    }

    public Expression ReduceAndCheck()
    {
      if (!this.CanReduce)
        throw new InvalidOperationException("Reduce and check can only be called on reducible nodes.");
      Expression expression = this.Reduce();
      if (expression == null)
        throw new InvalidOperationException("Reduce cannot return null.");
      if (expression == this)
        throw new InvalidOperationException("Reduce cannot return the original expression.");
      return this.Type.IsAssignableFrom(expression.Type) ? expression : throw new InvalidOperationException("Reduce must produce an expression of a compatible type.");
    }

    public virtual Expression Accept(ExpressionTreeVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeVisitor>(nameof (visitor), visitor);
      return visitor.VisitExtensionExpression(this);
    }

    protected internal abstract Expression VisitChildren(ExpressionTreeVisitor visitor);
  }
}
