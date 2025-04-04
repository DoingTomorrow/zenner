// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.SelectParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Hql.Util;
using NHibernate.Type;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class SelectParser : IParser
  {
    private bool readyForAliasOrExpression;
    private bool first;
    private bool afterNew;
    private bool insideNew;
    private System.Type holderClass;
    private readonly SelectPathExpressionParser pathExpressionParser = new SelectPathExpressionParser();
    private FunctionStack funcStack;
    private int parenCount;
    private static readonly Regex pathExpressionRegEx = new Regex("\\A[A-Za-z_][A-Za-z_0-9]*[.][A-Za-z_][A-Za-z_0-9]*\\z", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex stringCostantRegEx = new Regex("\\A'('{2})*([^'\\r\\n]*)('{2})*([^'\\r\\n]*)('{2})*'\\z", RegexOptions.Compiled | RegexOptions.Singleline);

    public SelectParser() => this.pathExpressionParser.UseThetaStyleJoin = true;

    public void Token(string token, QueryTranslator q)
    {
      SessionFactoryHelper sessionFactoryHelper = new SessionFactoryHelper(q.Factory);
      string lowerInvariant = token.ToLowerInvariant();
      if (this.first)
      {
        this.first = false;
        if ("distinct".Equals(lowerInvariant))
        {
          q.Distinct = true;
          return;
        }
        if ("all".Equals(lowerInvariant))
        {
          q.Distinct = false;
          return;
        }
      }
      if (this.afterNew)
      {
        this.afterNew = false;
        this.holderClass = sessionFactoryHelper.GetImportedClass(token);
        q.HolderClass = this.holderClass != null ? this.holderClass : throw new QueryException("class not found: " + token);
        this.insideNew = true;
      }
      else if (token.Equals(","))
      {
        if (this.readyForAliasOrExpression)
          throw new QueryException("alias or expression expected in SELECT");
        q.AppendScalarSelectToken(", ");
        this.readyForAliasOrExpression = true;
      }
      else if ("new".Equals(lowerInvariant))
      {
        this.afterNew = true;
        this.readyForAliasOrExpression = false;
      }
      else if ("(".Equals(token))
      {
        ++this.parenCount;
        if (!this.funcStack.HasFunctions && this.holderClass != null && !this.readyForAliasOrExpression)
        {
          this.readyForAliasOrExpression = true;
        }
        else
        {
          if (!this.funcStack.HasFunctions)
            throw new QueryException("HQL function expected before '(' in SELECT clause.");
          q.AppendScalarSelectToken(token);
        }
        this.readyForAliasOrExpression = true;
      }
      else if (")".Equals(token))
      {
        --this.parenCount;
        if (this.parenCount < 0)
          throw new QueryException("'(' expected before ')' in SELECT clause.");
        if (this.insideNew && !this.funcStack.HasFunctions && !this.readyForAliasOrExpression)
        {
          this.insideNew = false;
        }
        else
        {
          if (!this.funcStack.HasFunctions)
            return;
          q.AppendScalarSelectToken(token);
          IType returnType = this.funcStack.GetReturnType();
          this.funcStack.Pop();
          this.readyForAliasOrExpression = false;
          if (this.funcStack.HasFunctions)
            return;
          q.AddSelectScalar(returnType);
        }
      }
      else if (SelectParser.IsHQLFunction(lowerInvariant, q) && token == q.Unalias(token))
      {
        if (!this.readyForAliasOrExpression && !this.funcStack.HasFunctions)
          throw new QueryException("',' expected before function in SELECT: " + token);
        if (this.funcStack.HasFunctions && this.funcStack.FunctionGrammar.IsKnownArgument(lowerInvariant))
        {
          q.AppendScalarSelectToken(token);
        }
        else
        {
          this.funcStack.Push(SelectParser.GetFunction(lowerInvariant, q));
          q.AppendScalarSelectToken(token);
          if (this.funcStack.SqlFunction.HasArguments || this.funcStack.SqlFunction.HasParenthesesIfNoArguments)
            return;
          q.AddSelectScalar(this.funcStack.GetReturnType());
          this.funcStack.Pop();
          this.readyForAliasOrExpression = this.funcStack.HasFunctions;
        }
      }
      else if (this.funcStack.HasFunctions)
      {
        bool flag = false;
        int num = this.parenCount + (this.insideNew ? -1 : 0);
        if (!this.readyForAliasOrExpression)
        {
          if (num != this.funcStack.NestedFunctionCount)
            throw new QueryException("'(' expected after HQL function in SELECT");
        }
        try
        {
          ParserHelper.Parse((IParser) this.funcStack.PathExpressionParser, q.Unalias(token), ".", q);
        }
        catch (QueryException ex)
        {
          if (SelectParser.IsPathExpression(token))
            throw;
          else
            flag = true;
        }
        if (token.StartsWith(":"))
        {
          string name = token.Substring(1);
          q.AppendScalarSelectParameter(name);
        }
        else if (token.Equals("?"))
          q.AppendScalarSelectParameter();
        else if (flag)
        {
          q.AppendScalarSelectToken(token);
        }
        else
        {
          if (this.funcStack.PathExpressionParser.IsCollectionValued)
            q.AddCollection(this.funcStack.PathExpressionParser.CollectionName, this.funcStack.PathExpressionParser.CollectionRole);
          q.AppendScalarSelectToken(this.funcStack.PathExpressionParser.WhereColumn);
          this.funcStack.PathExpressionParser.AddAssociation(q);
        }
        this.readyForAliasOrExpression = false;
      }
      else
      {
        if (!this.readyForAliasOrExpression)
          throw new QueryException("',' expected in SELECT before:" + token);
        try
        {
          ParserHelper.Parse((IParser) this.pathExpressionParser, q.Unalias(token), ".", q);
          if (this.pathExpressionParser.IsCollectionValued)
            q.AddCollection(this.pathExpressionParser.CollectionName, this.pathExpressionParser.CollectionRole);
          else if (this.pathExpressionParser.WhereColumnType.IsEntityType)
            q.AddSelectClass(this.pathExpressionParser.SelectName);
          q.AppendScalarSelectTokens(this.pathExpressionParser.WhereColumns);
          q.AddSelectScalar(this.pathExpressionParser.WhereColumnType);
          this.pathExpressionParser.AddAssociation(q);
        }
        catch (QueryException ex)
        {
          if (SelectParser.IsStringCostant(token))
          {
            q.AppendScalarSelectToken(token);
            q.AddSelectScalar((IType) NHibernateUtil.String);
          }
          else if (SelectParser.IsIntegerConstant(token))
          {
            q.AppendScalarSelectToken(token);
            q.AddSelectScalar(SelectParser.GetIntegerConstantType(token));
          }
          else if (SelectParser.IsFloatingPointConstant(token))
          {
            q.AppendScalarSelectToken(token);
            q.AddSelectScalar(SelectParser.GetFloatingPointConstantType());
          }
          else if (token.StartsWith(":"))
          {
            string name = token.Substring(1);
            q.AppendScalarSelectParameter(name);
          }
          else if (token.Equals("?"))
            q.AppendScalarSelectParameter();
          else
            throw;
        }
        this.readyForAliasOrExpression = false;
      }
    }

    private static bool IsPathExpression(string token)
    {
      return SelectParser.pathExpressionRegEx.IsMatch(token);
    }

    private static bool IsStringCostant(string token)
    {
      return SelectParser.stringCostantRegEx.IsMatch(token);
    }

    private static bool IsIntegerConstant(string token)
    {
      return long.TryParse(token, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out long _);
    }

    private static bool IsFloatingPointConstant(string token)
    {
      return double.TryParse(token, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out double _);
    }

    private static IType GetIntegerConstantType(string token)
    {
      return int.TryParse(token, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out int _) ? (IType) NHibernateUtil.Int32 : (IType) NHibernateUtil.Int64;
    }

    private static IType GetFloatingPointConstantType() => (IType) NHibernateUtil.Double;

    private static bool IsHQLFunction(string funcName, QueryTranslator q)
    {
      return q.Factory.SQLFunctionRegistry.HasFunction(funcName);
    }

    private static ISQLFunction GetFunction(string name, QueryTranslator q)
    {
      return q.Factory.SQLFunctionRegistry.FindSQLFunction(name);
    }

    public void Start(QueryTranslator q)
    {
      this.readyForAliasOrExpression = true;
      this.first = true;
      this.afterNew = false;
      this.holderClass = (System.Type) null;
      this.parenCount = 0;
      this.funcStack = new FunctionStack((IMapping) q.Factory);
    }

    public void End(QueryTranslator q)
    {
      if (this.parenCount > 0 || this.funcStack.HasFunctions)
        throw new QueryException("close paren missed in SELECT");
    }
  }
}
