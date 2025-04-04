// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.WhereParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Hql.Util;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class WhereParser : IParser
  {
    private readonly PathExpressionParser pathExpressionParser = new PathExpressionParser();
    private static readonly ISet<string> expressionTerminators = (ISet<string>) new HashedSet<string>();
    private static readonly ISet<string> expressionOpeners = (ISet<string>) new HashedSet<string>();
    private static readonly ISet<string> booleanOperators = (ISet<string>) new HashedSet<string>();
    private static readonly Dictionary<string, string> negations = new Dictionary<string, string>();
    private static readonly ISet<string> specialFunctions = (ISet<string>) new HashedSet<string>();
    private bool betweenSpecialCase;
    private bool negated;
    private bool inSubselect;
    private int bracketsSinceSelect;
    private StringBuilder subselect;
    private bool isInSpecialFunctionClause;
    private int specialFunctionParenCount;
    private bool expectingPathContinuation;
    private int expectingIndex;
    private readonly List<bool> nots = new List<bool>();
    private readonly List<SqlStringBuilder> joins = new List<SqlStringBuilder>();
    private readonly List<bool> booleanTests = new List<bool>();

    public WhereParser() => this.pathExpressionParser.UseThetaStyleJoin = true;

    static WhereParser()
    {
      WhereParser.expressionTerminators.Add("and");
      WhereParser.expressionTerminators.Add("or");
      WhereParser.expressionTerminators.Add(")");
      WhereParser.expressionOpeners.Add("and");
      WhereParser.expressionOpeners.Add("or");
      WhereParser.expressionOpeners.Add("(");
      WhereParser.booleanOperators.Add("<");
      WhereParser.booleanOperators.Add("=");
      WhereParser.booleanOperators.Add(">");
      WhereParser.booleanOperators.Add("#");
      WhereParser.booleanOperators.Add("~");
      WhereParser.booleanOperators.Add("like");
      WhereParser.booleanOperators.Add("ilike");
      WhereParser.booleanOperators.Add("is");
      WhereParser.booleanOperators.Add("in");
      WhereParser.booleanOperators.Add("any");
      WhereParser.booleanOperators.Add("some");
      WhereParser.booleanOperators.Add("all");
      WhereParser.booleanOperators.Add("exists");
      WhereParser.booleanOperators.Add("between");
      WhereParser.booleanOperators.Add("<=");
      WhereParser.booleanOperators.Add(">=");
      WhereParser.booleanOperators.Add("=>");
      WhereParser.booleanOperators.Add("=<");
      WhereParser.booleanOperators.Add("!=");
      WhereParser.booleanOperators.Add("<>");
      WhereParser.booleanOperators.Add("!#");
      WhereParser.booleanOperators.Add("!~");
      WhereParser.booleanOperators.Add("!<");
      WhereParser.booleanOperators.Add("!>");
      WhereParser.booleanOperators.Add("is not");
      WhereParser.booleanOperators.Add("not like");
      WhereParser.booleanOperators.Add("not ilike");
      WhereParser.booleanOperators.Add("not in");
      WhereParser.booleanOperators.Add("not between");
      WhereParser.booleanOperators.Add("not exists");
      WhereParser.negations.Add("and", "or");
      WhereParser.negations.Add("or", "and");
      WhereParser.negations.Add("<", ">=");
      WhereParser.negations.Add("=", "<>");
      WhereParser.negations.Add(">", "<=");
      WhereParser.negations.Add("#", "!#");
      WhereParser.negations.Add("~", "!~");
      WhereParser.negations.Add("like", "not like");
      WhereParser.negations.Add("ilike", "not ilike");
      WhereParser.negations.Add("is", "is not");
      WhereParser.negations.Add("in", "not in");
      WhereParser.negations.Add("exists", "not exists");
      WhereParser.negations.Add("between", "not between");
      WhereParser.negations.Add("<=", ">");
      WhereParser.negations.Add(">=", "<");
      WhereParser.negations.Add("=>", "<");
      WhereParser.negations.Add("=<", ">");
      WhereParser.negations.Add("!=", "=");
      WhereParser.negations.Add("<>", "=");
      WhereParser.negations.Add("!#", "#");
      WhereParser.negations.Add("!~", "~");
      WhereParser.negations.Add("!<", ">=");
      WhereParser.negations.Add("!>", "<=");
      WhereParser.negations.Add("is not", "is");
      WhereParser.negations.Add("not like", "like");
      WhereParser.negations.Add("not in", "in");
      WhereParser.negations.Add("not between", "between");
      WhereParser.negations.Add("not exists", "exists");
      WhereParser.specialFunctions.Add("trim");
      WhereParser.specialFunctions.Add("extract");
    }

    private string GetElementName(PathExpressionParser.CollectionElement element, QueryTranslator q)
    {
      if (element.IsOneToMany)
        return element.Alias;
      IType type = element.Type;
      if (type.IsEntityType)
        return this.pathExpressionParser.ContinueFromManyToMany(((EntityType) type).GetAssociatedEntityName(), element.ElementColumns, q);
      throw new QueryException("illegally dereferenced collection element");
    }

    public void Token(string token, QueryTranslator q)
    {
      string lowerInvariant = token.ToLowerInvariant();
      if (token.Equals("[") && !this.expectingPathContinuation)
      {
        this.expectingPathContinuation = false;
        if (this.expectingIndex == 0)
          throw new QueryException("unexpected [");
      }
      else if (token.Equals("]"))
      {
        --this.expectingIndex;
        this.expectingPathContinuation = true;
      }
      else
      {
        if (this.expectingPathContinuation && this.ContinuePathExpression(token, q))
          return;
        if (!this.inSubselect)
        {
          switch (lowerInvariant)
          {
            case "select":
              this.inSubselect = true;
              this.subselect = new StringBuilder(20);
              break;
            case "from":
              if (this.isInSpecialFunctionClause)
                break;
              goto case "select";
          }
        }
        if (this.inSubselect && token.Equals(")"))
        {
          --this.bracketsSinceSelect;
          if (this.bracketsSinceSelect == -1)
          {
            QueryTranslator queryTranslator = new QueryTranslator(q.Factory, this.subselect.ToString(), q.EnabledFilters);
            try
            {
              queryTranslator.Compile(q);
            }
            catch (MappingException ex)
            {
              throw new QueryException("MappingException occurred compiling subquery", (Exception) ex);
            }
            this.AppendToken(q, queryTranslator.SqlString);
            this.inSubselect = false;
            this.bracketsSinceSelect = 0;
          }
        }
        if (this.inSubselect)
        {
          if (token.Equals("("))
            ++this.bracketsSinceSelect;
          this.subselect.Append(token).Append(' ');
        }
        else
        {
          this.SpecialCasesBefore(lowerInvariant);
          if (!this.betweenSpecialCase && WhereParser.expressionTerminators.Contains(lowerInvariant))
            this.CloseExpression(q, lowerInvariant);
          if (WhereParser.booleanOperators.Contains(lowerInvariant))
          {
            this.booleanTests.RemoveAt(this.booleanTests.Count - 1);
            this.booleanTests.Add(true);
          }
          if (lowerInvariant.Equals("not"))
          {
            this.nots[this.nots.Count - 1] = !this.nots[this.nots.Count - 1];
            this.negated = !this.negated;
          }
          else
          {
            if (!this.isInSpecialFunctionClause && WhereParser.specialFunctions.Contains(lowerInvariant))
              this.isInSpecialFunctionClause = true;
            if (this.isInSpecialFunctionClause && token.Equals("("))
              ++this.specialFunctionParenCount;
            if (this.isInSpecialFunctionClause && token.Equals(")"))
            {
              --this.specialFunctionParenCount;
              this.isInSpecialFunctionClause = this.specialFunctionParenCount > 0;
            }
            this.DoToken(token, q);
            if (!this.betweenSpecialCase && WhereParser.expressionOpeners.Contains(lowerInvariant))
              this.OpenExpression(q, lowerInvariant);
            this.SpecialCasesAfter(lowerInvariant);
          }
        }
      }
    }

    public void Start(QueryTranslator q) => this.Token("(", q);

    public void End(QueryTranslator q)
    {
      if (this.expectingPathContinuation)
      {
        this.expectingPathContinuation = false;
        PathExpressionParser.CollectionElement ce = this.pathExpressionParser.LastCollectionElement();
        if (ce.ElementColumns.Length != 1)
          throw new QueryException("path expression ended in composite collection element");
        this.AppendToken(q, ce.ElementColumns[0]);
        this.AddToCurrentJoin(ce);
      }
      this.Token(")", q);
    }

    private void CloseExpression(QueryTranslator q, string lcToken)
    {
      bool booleanTest = this.booleanTests[this.booleanTests.Count - 1];
      this.booleanTests.RemoveAt(this.booleanTests.Count - 1);
      if (booleanTest)
      {
        if (this.booleanTests.Count > 0)
          this.booleanTests[this.booleanTests.Count - 1] = true;
        SqlStringBuilder join = this.joins[this.joins.Count - 1];
        this.joins.RemoveAt(this.joins.Count - 1);
        this.AppendToken(q, join.ToSqlString());
      }
      else
      {
        SqlStringBuilder join = this.joins[this.joins.Count - 1];
        this.joins.RemoveAt(this.joins.Count - 1);
        if (this.joins.Count == 0)
          this.AppendToken(q, join.ToSqlString());
        else
          this.joins[this.joins.Count - 1].Add(join.ToSqlString());
      }
      bool not = this.nots[this.nots.Count - 1];
      this.nots.RemoveAt(this.nots.Count - 1);
      if (not)
        this.negated = !this.negated;
      if (")".Equals(lcToken))
        return;
      this.AppendToken(q, ")");
    }

    private void OpenExpression(QueryTranslator q, string lcToken)
    {
      this.nots.Add(false);
      this.booleanTests.Add(false);
      this.joins.Add(new SqlStringBuilder());
      if ("(".Equals(lcToken))
        return;
      this.AppendToken(q, "(");
    }

    private void Preprocess(string token, QueryTranslator q)
    {
      string[] strArray = StringHelper.Split(".", token, true);
      if (strArray.Length <= 5 || !"elements".Equals(strArray[strArray.Length - 1]) && !"indices".Equals(strArray[strArray.Length - 1]))
        return;
      this.pathExpressionParser.Start(q);
      for (int index = 0; index < strArray.Length - 3; ++index)
        this.pathExpressionParser.Token(strArray[index], q);
      this.pathExpressionParser.Token((string) null, q);
      this.pathExpressionParser.End(q);
      this.AddJoin(this.pathExpressionParser.WhereJoin, q);
      this.pathExpressionParser.IgnoreInitialJoin();
    }

    private void DoPathExpression(string token, QueryTranslator q)
    {
      this.Preprocess(token, q);
      StringTokenizer stringTokenizer = new StringTokenizer(token, ".", true);
      this.pathExpressionParser.Start(q);
      foreach (string token1 in stringTokenizer)
        this.pathExpressionParser.Token(token1, q);
      this.pathExpressionParser.End(q);
      if (this.pathExpressionParser.IsCollectionValued)
      {
        this.OpenExpression(q, string.Empty);
        this.AppendToken(q, this.pathExpressionParser.GetCollectionSubquery(q.EnabledFilters));
        this.CloseExpression(q, string.Empty);
        q.AddQuerySpaces(q.GetCollectionPersister(this.pathExpressionParser.CollectionRole).CollectionSpaces);
      }
      else if (this.pathExpressionParser.IsExpectingCollectionIndex)
      {
        ++this.expectingIndex;
      }
      else
      {
        this.AddJoin(this.pathExpressionParser.WhereJoin, q);
        this.AppendToken(q, this.pathExpressionParser.WhereColumn);
      }
    }

    private void AddJoin(JoinSequence joinSequence, QueryTranslator q)
    {
      q.AddFromJoinOnly(this.pathExpressionParser.Name, joinSequence);
      try
      {
        this.AddToCurrentJoin(joinSequence.ToJoinFragment(q.EnabledFilters, true).ToWhereFragmentString);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
    }

    private void DoToken(string token, QueryTranslator q)
    {
      SessionFactoryHelper sessionFactoryHelper = new SessionFactoryHelper(q.Factory);
      if (q.IsName(StringHelper.Root(token)))
        this.DoPathExpression(q.Unalias(token), q);
      else if (token.StartsWith(":"))
      {
        string name = token.Substring(1);
        this.AppendToken(q, q.GetNamedParameter(name));
      }
      else if (token.Equals("?"))
      {
        this.AppendToken(q, q.GetPositionalParameter());
      }
      else
      {
        IQueryable persisterUsingImports = q.GetPersisterUsingImports(token);
        if (persisterUsingImports != null)
        {
          string discriminatorSqlValue = persisterUsingImports.DiscriminatorSQLValue;
          if (InFragment.Null == discriminatorSqlValue || InFragment.NotNull == discriminatorSqlValue)
            throw new QueryException("subclass test not allowed for null or not null discriminator");
          this.AppendToken(q, discriminatorSqlValue);
        }
        else
        {
          string fieldName = (string) null;
          System.Type type1 = (System.Type) null;
          int num = token.IndexOf('.');
          if (num > -1)
          {
            fieldName = StringHelper.Unqualify(token);
            string className = StringHelper.Qualifier(token);
            type1 = sessionFactoryHelper.GetImportedClass(className);
          }
          if (num > -1 && type1 != null)
          {
            object constantValue;
            if ((constantValue = ReflectHelper.GetConstantValue(type1, fieldName)) != null)
            {
              IType type2;
              try
              {
                type2 = TypeFactory.HeuristicType(constantValue.GetType().AssemblyQualifiedName);
              }
              catch (MappingException ex)
              {
                throw new QueryException((Exception) ex);
              }
              if (type2 == null)
                throw new QueryException(string.Format("Could not determin the type of: {0}", (object) token));
              try
              {
                this.AppendToken(q, ((ILiteralType) type2).ObjectToSQLString(constantValue, q.Factory.Dialect));
                return;
              }
              catch (Exception ex)
              {
                throw new QueryException("Could not format constant value to SQL literal: " + token, ex);
              }
            }
          }
          string token1 = (string) null;
          if (this.negated)
            WhereParser.negations.TryGetValue(token.ToLowerInvariant(), out token1);
          if (token1 != null && (!this.betweenSpecialCase || !"or".Equals(token1)))
            this.AppendToken(q, token1);
          else
            this.AppendToken(q, token);
        }
      }
    }

    private void AddToCurrentJoin(SqlString sql) => this.joins[this.joins.Count - 1].Add(sql);

    private void AddToCurrentJoin(PathExpressionParser.CollectionElement ce)
    {
      try
      {
        this.AddToCurrentJoin(ce.JoinSequence.ToJoinFragment().ToWhereFragmentString + ce.IndexValue.ToSqlString());
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
    }

    private void SpecialCasesBefore(string lcToken)
    {
      if (!"between".Equals(lcToken) && !"not between".Equals(lcToken))
        return;
      this.betweenSpecialCase = true;
    }

    private void SpecialCasesAfter(string lcToken)
    {
      if (!this.betweenSpecialCase || !"and".Equals(lcToken))
        return;
      this.betweenSpecialCase = false;
    }

    protected virtual void AppendToken(QueryTranslator q, string token)
    {
      if (this.expectingIndex > 0)
      {
        this.pathExpressionParser.SetLastCollectionElementIndexValue(new SqlString(token));
      }
      else
      {
        if (string.IsNullOrEmpty(token))
          return;
        q.AppendWhereToken(new SqlString(token));
      }
    }

    protected virtual void AppendToken(QueryTranslator q, SqlString token)
    {
      if (this.expectingIndex > 0)
        this.pathExpressionParser.SetLastCollectionElementIndexValue(token);
      else
        q.AppendWhereToken(token);
    }

    private bool ContinuePathExpression(string token, QueryTranslator q)
    {
      this.expectingPathContinuation = false;
      PathExpressionParser.CollectionElement collectionElement = this.pathExpressionParser.LastCollectionElement();
      if (token.StartsWith("."))
      {
        this.DoPathExpression(this.GetElementName(collectionElement, q) + token, q);
        this.AddToCurrentJoin(collectionElement);
        return true;
      }
      if (collectionElement.ElementColumns.Length != 1)
        throw new QueryException("path expression ended in composite collection element");
      this.AppendToken(q, collectionElement.ElementColumns[0]);
      this.AddToCurrentJoin(collectionElement);
      return false;
    }
  }
}
