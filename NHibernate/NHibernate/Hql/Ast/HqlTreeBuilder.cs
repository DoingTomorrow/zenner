// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlTreeBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlTreeBuilder
  {
    private readonly IASTFactory _factory;

    public HqlTreeBuilder()
    {
      this._factory = (IASTFactory) new ASTFactory((ITreeAdaptor) new ASTTreeAdaptor());
    }

    public HqlQuery Query() => new HqlQuery(this._factory, new HqlStatement[0]);

    public HqlQuery Query(HqlSelectFrom selectFrom)
    {
      return new HqlQuery(this._factory, new HqlStatement[1]
      {
        (HqlStatement) selectFrom
      });
    }

    public HqlQuery Query(HqlSelectFrom selectFrom, HqlWhere where)
    {
      return new HqlQuery(this._factory, new HqlStatement[2]
      {
        (HqlStatement) selectFrom,
        (HqlStatement) where
      });
    }

    public HqlTreeNode Query(HqlSelectFrom selectFrom, HqlWhere where, HqlOrderBy orderBy)
    {
      return (HqlTreeNode) new HqlQuery(this._factory, new HqlStatement[3]
      {
        (HqlStatement) selectFrom,
        (HqlStatement) where,
        (HqlStatement) orderBy
      });
    }

    public HqlSelectFrom SelectFrom() => new HqlSelectFrom(this._factory, new HqlTreeNode[0]);

    public HqlSelectFrom SelectFrom(HqlSelect select)
    {
      return new HqlSelectFrom(this._factory, new HqlTreeNode[1]
      {
        (HqlTreeNode) select
      });
    }

    public HqlSelectFrom SelectFrom(HqlFrom from, HqlSelect select)
    {
      return new HqlSelectFrom(this._factory, new HqlTreeNode[2]
      {
        (HqlTreeNode) from,
        (HqlTreeNode) select
      });
    }

    public HqlSelectFrom SelectFrom(HqlFrom from)
    {
      return new HqlSelectFrom(this._factory, new HqlTreeNode[1]
      {
        (HqlTreeNode) from
      });
    }

    public HqlFrom From(HqlRange range)
    {
      return new HqlFrom(this._factory, new HqlTreeNode[1]
      {
        (HqlTreeNode) range
      });
    }

    public HqlFrom From() => new HqlFrom(this._factory, new HqlTreeNode[0]);

    public HqlRange Range(HqlIdent ident)
    {
      return new HqlRange(this._factory, new HqlTreeNode[1]
      {
        (HqlTreeNode) ident
      });
    }

    public HqlRange Range(HqlTreeNode ident, HqlAlias alias)
    {
      return new HqlRange(this._factory, new HqlTreeNode[2]
      {
        ident,
        (HqlTreeNode) alias
      });
    }

    public HqlIdent Ident(string ident) => new HqlIdent(this._factory, ident);

    public HqlIdent Ident(Type type) => new HqlIdent(this._factory, type);

    public HqlAlias Alias(string alias) => new HqlAlias(this._factory, alias);

    public HqlEquality Equality(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlEquality(this._factory, lhs, rhs);
    }

    public HqlBooleanAnd BooleanAnd(HqlBooleanExpression lhs, HqlBooleanExpression rhs)
    {
      return new HqlBooleanAnd(this._factory, lhs, rhs);
    }

    public HqlBooleanOr BooleanOr(HqlBooleanExpression lhs, HqlBooleanExpression rhs)
    {
      return new HqlBooleanOr(this._factory, lhs, rhs);
    }

    public HqlAdd Add(HqlExpression lhs, HqlExpression rhs) => new HqlAdd(this._factory, lhs, rhs);

    public HqlSubtract Subtract(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlSubtract(this._factory, lhs, rhs);
    }

    public HqlMultiplty Multiply(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlMultiplty(this._factory, lhs, rhs);
    }

    public HqlDivide Divide(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlDivide(this._factory, lhs, rhs);
    }

    public HqlDot Dot(HqlExpression lhs, HqlExpression rhs) => new HqlDot(this._factory, lhs, rhs);

    public HqlParameter Parameter(string name) => new HqlParameter(this._factory, name);

    public HqlWhere Where(HqlExpression expression) => new HqlWhere(this._factory, expression);

    public HqlHaving Having(HqlExpression expression) => new HqlHaving(this._factory, expression);

    public HqlConstant Constant(object value)
    {
      if (value == null)
        return (HqlConstant) new HqlNull(this._factory);
      switch (Type.GetTypeCode(value.GetType()))
      {
        case TypeCode.Boolean:
          return !(bool) value ? (HqlConstant) this.False() : (HqlConstant) this.True();
        case TypeCode.Char:
        case TypeCode.String:
          return (HqlConstant) new HqlStringConstant(this._factory, "'" + value + "'");
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
          return (HqlConstant) new HqlIntegerConstant(this._factory, value.ToString());
        case TypeCode.Single:
          return (HqlConstant) new HqlFloatConstant(this._factory, value.ToString());
        case TypeCode.Double:
          return (HqlConstant) new HqlDoubleConstant(this._factory, value.ToString());
        case TypeCode.Decimal:
          return (HqlConstant) new HqlDecimalConstant(this._factory, value.ToString());
        case TypeCode.DateTime:
          return (HqlConstant) new HqlStringConstant(this._factory, "'" + (object) (DateTime) value + "'");
        default:
          throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));
      }
    }

    public HqlOrderBy OrderBy() => new HqlOrderBy(this._factory);

    public HqlSkip Skip(HqlExpression parameter) => new HqlSkip(this._factory, parameter);

    public HqlTake Take(HqlExpression parameter) => new HqlTake(this._factory, parameter);

    public HqlSelect Select(HqlExpression expression)
    {
      return new HqlSelect(this._factory, new HqlExpression[1]
      {
        expression
      });
    }

    public HqlSelect Select(params HqlExpression[] expression)
    {
      return new HqlSelect(this._factory, expression);
    }

    public HqlSelect Select(IEnumerable<HqlExpression> expressions)
    {
      return new HqlSelect(this._factory, expressions.ToArray<HqlExpression>());
    }

    public HqlCase Case(HqlWhen[] whenClauses)
    {
      return new HqlCase(this._factory, whenClauses, (HqlExpression) null);
    }

    public HqlCase Case(HqlWhen[] whenClauses, HqlExpression ifFalse)
    {
      return new HqlCase(this._factory, whenClauses, ifFalse);
    }

    public HqlWhen When(HqlExpression predicate, HqlExpression ifTrue)
    {
      return new HqlWhen(this._factory, predicate, ifTrue);
    }

    public HqlElse Else(HqlExpression ifFalse) => new HqlElse(this._factory, ifFalse);

    public HqlInequality Inequality(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlInequality(this._factory, lhs, rhs);
    }

    public HqlLessThan LessThan(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlLessThan(this._factory, lhs, rhs);
    }

    public HqlLessThanOrEqual LessThanOrEqual(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlLessThanOrEqual(this._factory, lhs, rhs);
    }

    public HqlGreaterThan GreaterThan(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlGreaterThan(this._factory, lhs, rhs);
    }

    public HqlGreaterThanOrEqual GreaterThanOrEqual(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlGreaterThanOrEqual(this._factory, lhs, rhs);
    }

    public HqlCount Count() => new HqlCount(this._factory);

    public HqlCount Count(HqlExpression child) => new HqlCount(this._factory, child);

    public HqlRowStar RowStar() => new HqlRowStar(this._factory);

    public HqlCast Cast(HqlExpression expression, Type type)
    {
      return new HqlCast(this._factory, expression, type);
    }

    public HqlBitwiseNot BitwiseNot() => new HqlBitwiseNot(this._factory);

    public HqlBooleanNot BooleanNot(HqlBooleanExpression operand)
    {
      return new HqlBooleanNot(this._factory, operand);
    }

    public HqlAverage Average(HqlExpression expression)
    {
      return new HqlAverage(this._factory, expression);
    }

    public HqlSum Sum(HqlExpression expression) => new HqlSum(this._factory, expression);

    public HqlMin Min(HqlExpression expression) => new HqlMin(this._factory, expression);

    public HqlMax Max(HqlExpression expression) => new HqlMax(this._factory, expression);

    public HqlJoin Join(HqlExpression expression, HqlAlias alias)
    {
      return new HqlJoin(this._factory, expression, alias);
    }

    public HqlAny Any() => new HqlAny(this._factory);

    public HqlExists Exists(HqlQuery query) => new HqlExists(this._factory, query);

    public HqlElements Elements() => new HqlElements(this._factory);

    public HqlDistinct Distinct() => new HqlDistinct(this._factory);

    public HqlDirectionAscending Ascending() => new HqlDirectionAscending(this._factory);

    public HqlDirectionDescending Descending() => new HqlDirectionDescending(this._factory);

    public HqlGroupBy GroupBy(params HqlExpression[] expressions)
    {
      return new HqlGroupBy(this._factory, expressions);
    }

    public HqlAll All() => new HqlAll(this._factory);

    public HqlLike Like(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlLike(this._factory, lhs, rhs);
    }

    public HqlConcat Concat(params HqlExpression[] args) => new HqlConcat(this._factory, args);

    public HqlMethodCall MethodCall(string methodName, IEnumerable<HqlExpression> parameters)
    {
      return new HqlMethodCall(this._factory, methodName, parameters);
    }

    public HqlMethodCall MethodCall(string methodName, params HqlExpression[] parameters)
    {
      return new HqlMethodCall(this._factory, methodName, (IEnumerable<HqlExpression>) parameters);
    }

    public HqlBooleanMethodCall BooleanMethodCall(
      string methodName,
      IEnumerable<HqlExpression> parameters)
    {
      return new HqlBooleanMethodCall(this._factory, methodName, parameters);
    }

    public HqlDistinctHolder DistinctHolder(params HqlTreeNode[] children)
    {
      return new HqlDistinctHolder(this._factory, children);
    }

    public HqlExpressionSubTreeHolder ExpressionSubTreeHolder(params HqlTreeNode[] children)
    {
      return new HqlExpressionSubTreeHolder(this._factory, children);
    }

    public HqlExpressionSubTreeHolder ExpressionSubTreeHolder(IEnumerable<HqlTreeNode> children)
    {
      return new HqlExpressionSubTreeHolder(this._factory, children);
    }

    public HqlIsNull IsNull(HqlExpression lhs) => new HqlIsNull(this._factory, lhs);

    public HqlIsNotNull IsNotNull(HqlExpression lhs) => new HqlIsNotNull(this._factory, lhs);

    public HqlTreeNode ExpressionList(IEnumerable<HqlExpression> expressions)
    {
      return (HqlTreeNode) new HqlExpressionList(this._factory, expressions);
    }

    public HqlStar Star() => new HqlStar(this._factory);

    public HqlTrue True() => new HqlTrue(this._factory);

    public HqlFalse False() => new HqlFalse(this._factory);

    public HqlIn In(HqlExpression itemExpression, HqlTreeNode source)
    {
      return new HqlIn(this._factory, itemExpression, source);
    }

    public HqlLeftJoin LeftJoin(HqlExpression expression, HqlAlias alias)
    {
      return new HqlLeftJoin(this._factory, expression, alias);
    }

    public HqlFetchJoin FetchJoin(HqlExpression expression, HqlAlias alias)
    {
      return new HqlFetchJoin(this._factory, expression, alias);
    }

    public HqlLeftFetchJoin LeftFetchJoin(HqlExpression expression, HqlAlias alias)
    {
      return new HqlLeftFetchJoin(this._factory, expression, alias);
    }

    public HqlClass Class() => new HqlClass(this._factory);

    public HqlBitwiseAnd BitwiseAnd(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlBitwiseAnd(this._factory, lhs, rhs);
    }

    public HqlBitwiseOr BitwiseOr(HqlExpression lhs, HqlExpression rhs)
    {
      return new HqlBitwiseOr(this._factory, lhs, rhs);
    }

    public HqlTreeNode Coalesce(HqlExpression lhs, HqlExpression rhs)
    {
      return (HqlTreeNode) new HqlCoalesce(this._factory, lhs, rhs);
    }

    public HqlTreeNode DictionaryItem(HqlExpression dictionary, HqlExpression index)
    {
      return (HqlTreeNode) new HqlDictionaryIndex(this._factory, dictionary, index);
    }

    public HqlTreeNode Indices(HqlExpression dictionary)
    {
      return (HqlTreeNode) new HqlIndices(this._factory, dictionary);
    }
  }
}
