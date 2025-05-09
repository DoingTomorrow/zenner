﻿// Decompiled with JetBrains decompiler
// Type: SQLite.TableQuery`1
// Assembly: SQLite.SyncClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E253D644-5E08-4C51-8B60-033C30AC87B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SQLite.SyncClient.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#nullable disable
namespace SQLite
{
  public class TableQuery<T> : BaseTableQuery, IEnumerable<T>, IEnumerable
  {
    private Expression _where;
    private List<BaseTableQuery.Ordering> _orderBys;
    private int? _limit;
    private int? _offset;
    private BaseTableQuery _joinInner;
    private Expression _joinInnerKeySelector;
    private BaseTableQuery _joinOuter;
    private Expression _joinOuterKeySelector;
    private Expression _joinSelector;
    private Expression _selector;
    private bool _deferred;

    public SQLiteConnection Connection { get; private set; }

    public TableMapping Table { get; private set; }

    private TableQuery(SQLiteConnection conn, TableMapping table)
    {
      this.Connection = conn;
      this.Table = table;
    }

    public TableQuery(SQLiteConnection conn)
    {
      this.Connection = conn;
      this.Table = this.Connection.GetMapping(typeof (T));
    }

    public TableQuery<U> Clone<U>()
    {
      TableQuery<U> tableQuery = new TableQuery<U>(this.Connection, this.Table);
      tableQuery._where = this._where;
      tableQuery._deferred = this._deferred;
      if (this._orderBys != null)
        tableQuery._orderBys = new List<BaseTableQuery.Ordering>((IEnumerable<BaseTableQuery.Ordering>) this._orderBys);
      tableQuery._limit = this._limit;
      tableQuery._offset = this._offset;
      tableQuery._joinInner = this._joinInner;
      tableQuery._joinInnerKeySelector = this._joinInnerKeySelector;
      tableQuery._joinOuter = this._joinOuter;
      tableQuery._joinOuterKeySelector = this._joinOuterKeySelector;
      tableQuery._joinSelector = this._joinSelector;
      tableQuery._selector = this._selector;
      return tableQuery;
    }

    public TableQuery<T> Where(Expression<Func<T, bool>> predExpr)
    {
      Expression pred = predExpr.NodeType == ExpressionType.Lambda ? predExpr.Body : throw new NotSupportedException("Must be a predicate");
      TableQuery<T> tableQuery = this.Clone<T>();
      tableQuery.AddWhere(pred);
      return tableQuery;
    }

    public TableQuery<T> Take(int n)
    {
      TableQuery<T> tableQuery = this.Clone<T>();
      tableQuery._limit = new int?(n);
      return tableQuery;
    }

    public TableQuery<T> Skip(int n)
    {
      TableQuery<T> tableQuery = this.Clone<T>();
      tableQuery._offset = new int?(n);
      return tableQuery;
    }

    public T ElementAt(int index) => this.Skip(index).Take(1).First();

    public TableQuery<T> Deferred()
    {
      TableQuery<T> tableQuery = this.Clone<T>();
      tableQuery._deferred = true;
      return tableQuery;
    }

    public TableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
    {
      return this.AddOrderBy<U>(orderExpr, true);
    }

    public TableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
    {
      return this.AddOrderBy<U>(orderExpr, false);
    }

    public TableQuery<T> ThenBy<U>(Expression<Func<T, U>> orderExpr)
    {
      return this.AddOrderBy<U>(orderExpr, true);
    }

    public TableQuery<T> ThenByDescending<U>(Expression<Func<T, U>> orderExpr)
    {
      return this.AddOrderBy<U>(orderExpr, false);
    }

    private TableQuery<T> AddOrderBy<U>(Expression<Func<T, U>> orderExpr, bool asc)
    {
      LambdaExpression lambdaExpression = orderExpr.NodeType == ExpressionType.Lambda ? (LambdaExpression) orderExpr : throw new NotSupportedException("Must be a predicate");
      MemberExpression memberExpression = !(lambdaExpression.Body is UnaryExpression body) || body.NodeType != ExpressionType.Convert ? lambdaExpression.Body as MemberExpression : body.Operand as MemberExpression;
      if (memberExpression == null || memberExpression.Expression.NodeType != ExpressionType.Parameter)
        throw new NotSupportedException("Order By does not support: " + (object) orderExpr);
      TableQuery<T> tableQuery = this.Clone<T>();
      if (tableQuery._orderBys == null)
        tableQuery._orderBys = new List<BaseTableQuery.Ordering>();
      tableQuery._orderBys.Add(new BaseTableQuery.Ordering()
      {
        ColumnName = this.Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name,
        Ascending = asc
      });
      return tableQuery;
    }

    private void AddWhere(Expression pred)
    {
      if (this._where == null)
        this._where = pred;
      else
        this._where = (Expression) Expression.AndAlso(this._where, pred);
    }

    public TableQuery<TResult> Join<TInner, TKey, TResult>(
      TableQuery<TInner> inner,
      Expression<Func<T, TKey>> outerKeySelector,
      Expression<Func<TInner, TKey>> innerKeySelector,
      Expression<Func<T, TInner, TResult>> resultSelector)
    {
      return new TableQuery<TResult>(this.Connection, this.Connection.GetMapping(typeof (TResult)))
      {
        _joinOuter = (BaseTableQuery) this,
        _joinOuterKeySelector = (Expression) outerKeySelector,
        _joinInner = (BaseTableQuery) inner,
        _joinInnerKeySelector = (Expression) innerKeySelector,
        _joinSelector = (Expression) resultSelector
      };
    }

    public TableQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
    {
      TableQuery<TResult> tableQuery = this.Clone<TResult>();
      tableQuery._selector = (Expression) selector;
      return tableQuery;
    }

    private SQLiteCommand GenerateCommand(string selectionList)
    {
      if (this._joinInner != null && this._joinOuter != null)
        throw new NotSupportedException("Joins are not supported.");
      string cmdText = "select " + selectionList + " from \"" + this.Table.TableName + "\"";
      List<object> queryArgs = new List<object>();
      if (this._where != null)
      {
        TableQuery<T>.CompileResult compileResult = this.CompileExpr(this._where, queryArgs);
        cmdText = cmdText + " where " + compileResult.CommandText;
      }
      if (this._orderBys != null && this._orderBys.Count > 0)
      {
        string str = string.Join(", ", this._orderBys.Select<BaseTableQuery.Ordering, string>((Func<BaseTableQuery.Ordering, string>) (o => "\"" + o.ColumnName + "\"" + (o.Ascending ? "" : " desc"))).ToArray<string>());
        cmdText = cmdText + " order by " + str;
      }
      if (this._limit.HasValue)
        cmdText = cmdText + " limit " + (object) this._limit.Value;
      if (this._offset.HasValue)
      {
        if (!this._limit.HasValue)
          cmdText += " limit -1 ";
        cmdText = cmdText + " offset " + (object) this._offset.Value;
      }
      return this.Connection.CreateCommand(cmdText, queryArgs.ToArray());
    }

    private TableQuery<T>.CompileResult CompileExpr(Expression expr, List<object> queryArgs)
    {
      if (expr == null)
        throw new NotSupportedException("Expression is NULL");
      if (expr is BinaryExpression)
      {
        BinaryExpression binaryExpression = (BinaryExpression) expr;
        TableQuery<T>.CompileResult parameter1 = this.CompileExpr(binaryExpression.Left, queryArgs);
        TableQuery<T>.CompileResult parameter2 = this.CompileExpr(binaryExpression.Right, queryArgs);
        string str;
        if (parameter1.CommandText == "?" && parameter1.Value == null)
          str = this.CompileNullBinaryExpression(binaryExpression, parameter2);
        else if (parameter2.CommandText == "?" && parameter2.Value == null)
          str = this.CompileNullBinaryExpression(binaryExpression, parameter1);
        else
          str = "(" + parameter1.CommandText + " " + this.GetSqlName((Expression) binaryExpression) + " " + parameter2.CommandText + ")";
        return new TableQuery<T>.CompileResult()
        {
          CommandText = str
        };
      }
      if (expr.NodeType == ExpressionType.Call)
      {
        MethodCallExpression methodCallExpression = (MethodCallExpression) expr;
        TableQuery<T>.CompileResult[] source = new TableQuery<T>.CompileResult[methodCallExpression.Arguments.Count];
        TableQuery<T>.CompileResult compileResult = methodCallExpression.Object != null ? this.CompileExpr(methodCallExpression.Object, queryArgs) : (TableQuery<T>.CompileResult) null;
        for (int index = 0; index < source.Length; ++index)
          source[index] = this.CompileExpr(methodCallExpression.Arguments[index], queryArgs);
        string str;
        if (methodCallExpression.Method.Name == "Like" && source.Length == 2)
          str = "(" + source[0].CommandText + " like " + source[1].CommandText + ")";
        else if (methodCallExpression.Method.Name == "Contains" && source.Length == 2)
          str = "(" + source[1].CommandText + " in " + source[0].CommandText + ")";
        else if (methodCallExpression.Method.Name == "Contains" && source.Length == 1)
        {
          if (methodCallExpression.Object != null && methodCallExpression.Object.Type == typeof (string))
            str = "(" + compileResult.CommandText + " like ('%' || " + source[0].CommandText + " || '%'))";
          else
            str = "(" + source[0].CommandText + " in " + compileResult.CommandText + ")";
        }
        else if (methodCallExpression.Method.Name == "StartsWith" && source.Length == 1)
          str = "(" + compileResult.CommandText + " like (" + source[0].CommandText + " || '%'))";
        else if (methodCallExpression.Method.Name == "EndsWith" && source.Length == 1)
          str = "(" + compileResult.CommandText + " like ('%' || " + source[0].CommandText + "))";
        else if (methodCallExpression.Method.Name == "Equals" && source.Length == 1)
          str = "(" + compileResult.CommandText + " = (" + source[0].CommandText + "))";
        else
          str = !(methodCallExpression.Method.Name == "ToLower") ? (!(methodCallExpression.Method.Name == "ToUpper") ? methodCallExpression.Method.Name.ToLower() + "(" + string.Join(",", ((IEnumerable<TableQuery<T>.CompileResult>) source).Select<TableQuery<T>.CompileResult, string>((Func<TableQuery<T>.CompileResult, string>) (a => a.CommandText)).ToArray<string>()) + ")" : "(upper(" + compileResult.CommandText + "))") : "(lower(" + compileResult.CommandText + "))";
        return new TableQuery<T>.CompileResult()
        {
          CommandText = str
        };
      }
      if (expr.NodeType == ExpressionType.Constant)
      {
        ConstantExpression constantExpression = (ConstantExpression) expr;
        queryArgs.Add(constantExpression.Value);
        return new TableQuery<T>.CompileResult()
        {
          CommandText = "?",
          Value = constantExpression.Value
        };
      }
      if (expr.NodeType == ExpressionType.Convert)
      {
        UnaryExpression unaryExpression = (UnaryExpression) expr;
        Type type = unaryExpression.Type;
        TableQuery<T>.CompileResult compileResult = this.CompileExpr(unaryExpression.Operand, queryArgs);
        return new TableQuery<T>.CompileResult()
        {
          CommandText = compileResult.CommandText,
          Value = compileResult.Value != null ? TableQuery<T>.ConvertTo(compileResult.Value, type) : (object) null
        };
      }
      MemberExpression memberExpression = expr.NodeType == ExpressionType.MemberAccess ? (MemberExpression) expr : throw new NotSupportedException("Cannot compile: " + expr.NodeType.ToString());
      if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
      {
        string name = this.Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name;
        return new TableQuery<T>.CompileResult()
        {
          CommandText = "\"" + name + "\""
        };
      }
      object obj1 = (object) null;
      if (memberExpression.Expression != null)
      {
        TableQuery<T>.CompileResult compileResult = this.CompileExpr(memberExpression.Expression, queryArgs);
        if (compileResult.Value == null)
          throw new NotSupportedException("Member access failed to compile expression");
        if (compileResult.CommandText == "?")
          queryArgs.RemoveAt(queryArgs.Count - 1);
        obj1 = compileResult.Value;
      }
      object obj2;
      if (memberExpression.Member.MemberType == MemberTypes.Property)
      {
        obj2 = ((PropertyInfo) memberExpression.Member).GetValue(obj1, (object[]) null);
      }
      else
      {
        if (memberExpression.Member.MemberType != MemberTypes.Field)
          throw new NotSupportedException("MemberExpr: " + (object) memberExpression.Member.MemberType);
        obj2 = ((FieldInfo) memberExpression.Member).GetValue(obj1);
      }
      if (obj2 != null && obj2 is IEnumerable && !(obj2 is string) && !(obj2 is IEnumerable<byte>))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("(");
        string str = "";
        foreach (object obj3 in (IEnumerable) obj2)
        {
          queryArgs.Add(obj3);
          stringBuilder.Append(str);
          stringBuilder.Append("?");
          str = ",";
        }
        stringBuilder.Append(")");
        return new TableQuery<T>.CompileResult()
        {
          CommandText = stringBuilder.ToString(),
          Value = obj2
        };
      }
      queryArgs.Add(obj2);
      return new TableQuery<T>.CompileResult()
      {
        CommandText = "?",
        Value = obj2
      };
    }

    private static object ConvertTo(object obj, Type t)
    {
      Type underlyingType = Nullable.GetUnderlyingType(t);
      if (!(underlyingType != (Type) null))
        return Convert.ChangeType(obj, t);
      return obj == null ? (object) null : Convert.ChangeType(obj, underlyingType);
    }

    private string CompileNullBinaryExpression(
      BinaryExpression expression,
      TableQuery<T>.CompileResult parameter)
    {
      if (expression.NodeType == ExpressionType.Equal)
        return "(" + parameter.CommandText + " is ?)";
      if (expression.NodeType == ExpressionType.NotEqual)
        return "(" + parameter.CommandText + " is not ?)";
      throw new NotSupportedException("Cannot compile Null-BinaryExpression with type " + expression.NodeType.ToString());
    }

    private string GetSqlName(Expression expr)
    {
      ExpressionType nodeType = expr.NodeType;
      switch (nodeType)
      {
        case ExpressionType.And:
          return "&";
        case ExpressionType.AndAlso:
          return "and";
        case ExpressionType.Equal:
          return "=";
        case ExpressionType.GreaterThan:
          return ">";
        case ExpressionType.GreaterThanOrEqual:
          return ">=";
        case ExpressionType.LessThan:
          return "<";
        case ExpressionType.LessThanOrEqual:
          return "<=";
        case ExpressionType.NotEqual:
          return "!=";
        case ExpressionType.Or:
          return "|";
        case ExpressionType.OrElse:
          return "or";
        default:
          throw new NotSupportedException("Cannot get SQL for: " + (object) nodeType);
      }
    }

    public int Count() => this.GenerateCommand("count(*)").ExecuteScalar<int>();

    public int Count(Expression<Func<T, bool>> predExpr) => this.Where(predExpr).Count();

    public IEnumerator<T> GetEnumerator()
    {
      return !this._deferred ? (IEnumerator<T>) this.GenerateCommand("*").ExecuteQuery<T>().GetEnumerator() : this.GenerateCommand("*").ExecuteDeferredQuery<T>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public T First() => this.Take(1).ToList<T>().First<T>();

    public T FirstOrDefault() => this.Take(1).ToList<T>().FirstOrDefault<T>();

    private class CompileResult
    {
      public string CommandText { get; set; }

      public object Value { get; set; }
    }
  }
}
