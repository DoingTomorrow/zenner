// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.Expression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public abstract class Expression
  {
    private readonly List<Expression> _children = new List<Expression>();

    protected string ExpressionString { get; private set; }

    public IEnumerable<Expression> Children => (IEnumerable<Expression>) this._children;

    public Expression Next { get; set; }

    public Expression Prev { get; set; }

    public IOperator Operator { get; set; }

    public abstract bool IsGroupedExpression { get; }

    public Expression()
    {
    }

    public Expression(string expression)
    {
      this.ExpressionString = expression;
      this.Operator = (IOperator) null;
    }

    public virtual bool ParentIsLookupFunction { get; set; }

    public virtual bool HasChildren => this._children.Any<Expression>();

    public virtual Expression PrepareForNextChild() => this;

    public virtual Expression AddChild(Expression child)
    {
      if (this._children.Any<Expression>())
      {
        Expression expression = this._children.Last<Expression>();
        child.Prev = expression;
        expression.Next = child;
      }
      this._children.Add(child);
      return child;
    }

    public virtual Expression MergeWithNext()
    {
      Expression expression1 = this;
      if (this.Next == null || this.Operator == null)
        throw new FormatException("Invalid formula syntax. Operator missing expression.");
      Expression expression2 = ExpressionConverter.Instance.FromCompileResult(this.Operator.Apply(this.Compile(), this.Next.Compile()));
      expression2.Operator = this.Next == null ? (IOperator) null : this.Next.Operator;
      expression2.Next = this.Next.Next;
      if (expression2.Next != null)
        expression2.Next.Prev = expression2;
      expression2.Prev = this.Prev;
      if (this.Prev != null)
        this.Prev.Next = expression2;
      return expression2;
    }

    public abstract CompileResult Compile();
  }
}
