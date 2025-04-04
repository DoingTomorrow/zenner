// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.WhereJoinDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ReWriters;
using NHibernate.Param;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  internal class WhereJoinDetector : NhExpressionTreeVisitor
  {
    private readonly IIsEntityDecider _isEntityDecider;
    private readonly IJoiner _joiner;
    private readonly Stack<bool> _handled = new Stack<bool>();
    private readonly Stack<WhereJoinDetector.ExpressionValues> _values = new Stack<WhereJoinDetector.ExpressionValues>();
    private int _memberExpressionDepth;

    internal WhereJoinDetector(IIsEntityDecider isEntityDecider, IJoiner joiner)
    {
      this._isEntityDecider = isEntityDecider;
      this._joiner = joiner;
    }

    public void Transform(WhereClause whereClause)
    {
      whereClause.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      WhereJoinDetector.ExpressionValues expressionValues = this._values.Pop();
      foreach (string memberExpression in expressionValues.MemberExpressions)
      {
        if (!expressionValues.GetValues(memberExpression).Contains((object) true))
          this._joiner.MakeInnerIfJoined(memberExpression);
      }
    }

    public override Expression VisitExpression(Expression expression)
    {
      if (expression == null)
        return (Expression) null;
      this._handled.Push(false);
      int count = this._values.Count;
      Expression expression1 = base.VisitExpression(expression);
      if (!this._handled.Pop())
      {
        while (this._values.Count > count)
          this._values.Pop();
        this._values.Push(new WhereJoinDetector.ExpressionValues(PossibleValueSet.CreateAllValues(expression.Type)));
      }
      return expression1;
    }

    protected override Expression VisitBinaryExpression(BinaryExpression expression)
    {
      Expression expression1 = base.VisitBinaryExpression(expression);
      if (expression.NodeType == ExpressionType.AndAlso)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.AndAlso(b)));
      else if (expression.NodeType == ExpressionType.OrElse)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.OrElse(b)));
      else if (expression.NodeType == ExpressionType.NotEqual && WhereJoinDetector.IsNullConstantExpression(expression.Right))
      {
        this._values.Pop();
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.IsNotNull()));
      }
      else if (expression.NodeType == ExpressionType.NotEqual && WhereJoinDetector.IsNullConstantExpression(expression.Left))
      {
        WhereJoinDetector.ExpressionValues expressionValues = this._values.Pop();
        this._values.Pop();
        this._values.Push(expressionValues);
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.IsNotNull()));
      }
      else if (expression.NodeType == ExpressionType.Equal && WhereJoinDetector.IsNullConstantExpression(expression.Right))
      {
        this._values.Pop();
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.IsNull()));
      }
      else if (expression.NodeType == ExpressionType.Equal && WhereJoinDetector.IsNullConstantExpression(expression.Left))
      {
        WhereJoinDetector.ExpressionValues expressionValues = this._values.Pop();
        this._values.Pop();
        this._values.Push(expressionValues);
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.IsNull()));
      }
      else if (expression.NodeType == ExpressionType.Coalesce)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Coalesce(b)));
      else if (expression.NodeType == ExpressionType.Add || expression.NodeType == ExpressionType.AddChecked)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Add(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Divide)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Divide(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Modulo)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Modulo(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Multiply || expression.NodeType == ExpressionType.MultiplyChecked)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Multiply(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Power)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Power(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Subtract || expression.NodeType == ExpressionType.SubtractChecked)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Subtract(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.And)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.And(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Or)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Or(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.ExclusiveOr)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.ExclusiveOr(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.LeftShift)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.LeftShift(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.RightShift)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.RightShift(b, expression.Type)));
      else if (expression.NodeType == ExpressionType.Equal)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.Equal(b)));
      else if (expression.NodeType == ExpressionType.NotEqual)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.NotEqual(b)));
      else if (expression.NodeType == ExpressionType.GreaterThanOrEqual)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.GreaterThanOrEqual(b)));
      else if (expression.NodeType == ExpressionType.GreaterThan)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.GreaterThan(b)));
      else if (expression.NodeType == ExpressionType.LessThan)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.LessThan(b)));
      else if (expression.NodeType == ExpressionType.LessThanOrEqual)
        this.HandleBinaryOperation((Func<PossibleValueSet, PossibleValueSet, PossibleValueSet>) ((a, b) => a.LessThanOrEqual(b)));
      return expression1;
    }

    protected override Expression VisitUnaryExpression(UnaryExpression expression)
    {
      Expression expression1 = base.VisitUnaryExpression(expression);
      if (expression.NodeType == ExpressionType.Not && expression.Type == typeof (bool))
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.Not()));
      else if (expression.NodeType == ExpressionType.Not)
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.BitwiseNot(expression.Type)));
      else if (expression.NodeType == ExpressionType.ArrayLength)
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.ArrayLength(expression.Type)));
      else if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.Convert(expression.Type)));
      else if (expression.NodeType == ExpressionType.Negate || expression.NodeType == ExpressionType.NegateChecked)
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.Negate(expression.Type)));
      else if (expression.NodeType == ExpressionType.UnaryPlus)
        this.HandleUnaryOperation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.UnaryPlus(expression.Type)));
      return expression1;
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      expression.QueryModel.TransformExpressions(new Func<Expression, Expression>(((ExpressionTreeVisitor) this).VisitExpression));
      return (Expression) expression;
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      bool flag = this._isEntityDecider.IsIdentifier(expression.Expression.Type, expression.Member.Name);
      if (!flag)
        ++this._memberExpressionDepth;
      Expression expression1 = base.VisitMemberExpression(expression);
      if (!flag)
        --this._memberExpressionDepth;
      WhereJoinDetector.ExpressionValues values = this._values.Pop().Operation((Func<PossibleValueSet, PossibleValueSet>) (pvs => pvs.MemberAccess(expression.Type)));
      if (this._isEntityDecider.IsEntity(expression.Type))
      {
        string key = ExpressionKeyVisitor.Visit((Expression) expression, (IDictionary<ConstantExpression, NamedParameter>) null);
        if (this._memberExpressionDepth > 0 && this._joiner.CanAddJoin((Expression) expression))
          expression1 = this._joiner.AddJoin(expression1, key);
        values.MemberExpressionValuesIfEmptyOuterJoined[key] = PossibleValueSet.CreateNull(expression.Type);
      }
      this.SetResultValues(values);
      return expression1;
    }

    private static bool IsNullConstantExpression(Expression expression)
    {
      return expression is ConstantExpression constantExpression && constantExpression.Value == null;
    }

    private void SetResultValues(WhereJoinDetector.ExpressionValues values)
    {
      this._handled.Pop();
      this._handled.Push(true);
      this._values.Push(values);
    }

    private void HandleBinaryOperation(
      Func<PossibleValueSet, PossibleValueSet, PossibleValueSet> operation)
    {
      WhereJoinDetector.ExpressionValues mergeWith = this._values.Pop();
      this.SetResultValues(this._values.Pop().Operation(mergeWith, operation));
    }

    private void HandleUnaryOperation(Func<PossibleValueSet, PossibleValueSet> operation)
    {
      this.SetResultValues(this._values.Pop().Operation(operation));
    }

    private class ExpressionValues
    {
      public ExpressionValues(PossibleValueSet valuesIfUnknownMemberExpression)
      {
        this.Values = valuesIfUnknownMemberExpression;
        this.MemberExpressionValuesIfEmptyOuterJoined = new Dictionary<string, PossibleValueSet>();
      }

      private PossibleValueSet Values { get; set; }

      public Dictionary<string, PossibleValueSet> MemberExpressionValuesIfEmptyOuterJoined { get; private set; }

      public PossibleValueSet GetValues(string memberExpression)
      {
        return this.MemberExpressionValuesIfEmptyOuterJoined.ContainsKey(memberExpression) ? this.MemberExpressionValuesIfEmptyOuterJoined[memberExpression] : this.Values;
      }

      public IEnumerable<string> MemberExpressions
      {
        get => (IEnumerable<string>) this.MemberExpressionValuesIfEmptyOuterJoined.Keys;
      }

      public WhereJoinDetector.ExpressionValues Operation(
        WhereJoinDetector.ExpressionValues mergeWith,
        Func<PossibleValueSet, PossibleValueSet, PossibleValueSet> operation)
      {
        WhereJoinDetector.ExpressionValues expressionValues = new WhereJoinDetector.ExpressionValues(operation(this.Values, mergeWith.Values));
        foreach (string str in this.MemberExpressions.Union<string>(mergeWith.MemberExpressions))
        {
          PossibleValueSet values1 = this.GetValues(str);
          PossibleValueSet values2 = mergeWith.GetValues(str);
          expressionValues.MemberExpressionValuesIfEmptyOuterJoined.Add(str, operation(values1, values2));
        }
        return expressionValues;
      }

      public WhereJoinDetector.ExpressionValues Operation(
        Func<PossibleValueSet, PossibleValueSet> operation)
      {
        WhereJoinDetector.ExpressionValues expressionValues = new WhereJoinDetector.ExpressionValues(operation(this.Values));
        foreach (string memberExpression in this.MemberExpressions)
          expressionValues.MemberExpressionValuesIfEmptyOuterJoined.Add(memberExpression, operation(this.GetValues(memberExpression)));
        return expressionValues;
      }
    }
  }
}
