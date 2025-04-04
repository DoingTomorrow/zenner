// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.FromParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class FromParser : IParser
  {
    private readonly PathExpressionParser peParser = (PathExpressionParser) new FromPathExpressionParser();
    private string entityName;
    private string alias;
    private bool afterIn;
    private bool afterAs;
    private bool afterClass;
    private bool expectingJoin;
    private bool expectingIn;
    private bool expectingAs;
    private bool afterJoinType;
    private bool afterFetch;
    private JoinType joinType = JoinType.None;
    private static readonly Dictionary<string, JoinType> joinTypes = new Dictionary<string, JoinType>();

    static FromParser()
    {
      FromParser.joinTypes.Add("left", JoinType.LeftOuterJoin);
      FromParser.joinTypes.Add("right", JoinType.RightOuterJoin);
      FromParser.joinTypes.Add("full", JoinType.FullJoin);
      FromParser.joinTypes.Add("inner", JoinType.InnerJoin);
    }

    public void Token(string token, QueryTranslator q)
    {
      string lowerInvariant = token.ToLowerInvariant();
      if (lowerInvariant.Equals(","))
      {
        this.expectingJoin = this.expectingJoin | this.expectingAs ? false : throw new QueryException("unexpected token: ,");
        this.expectingAs = false;
      }
      else if ("join".Equals(lowerInvariant))
      {
        if (!this.afterJoinType)
        {
          if (!(this.expectingJoin | this.expectingAs))
            throw new QueryException("unexpected token: join");
          this.joinType = JoinType.InnerJoin;
          this.expectingJoin = false;
          this.expectingAs = false;
        }
        else
          this.afterJoinType = false;
      }
      else if ("fetch".Equals(lowerInvariant))
      {
        if (q.IsShallowQuery)
          throw new QueryException("fetch may not be used with scroll() or iterate()");
        if (this.joinType == JoinType.None)
          throw new QueryException("unexpected token: fetch");
        if (this.joinType == JoinType.FullJoin || this.joinType == JoinType.RightOuterJoin)
          throw new QueryException("fetch may only be used with inner join or left outer join");
        this.afterFetch = true;
      }
      else if ("outer".Equals(lowerInvariant))
      {
        if (!this.afterJoinType || this.joinType != JoinType.LeftOuterJoin && this.joinType != JoinType.RightOuterJoin)
          throw new QueryException("unexpected token: outer");
      }
      else if (FromParser.joinTypes.ContainsKey(lowerInvariant))
      {
        if (!(this.expectingJoin | this.expectingAs))
          throw new QueryException("unexpected token: " + token);
        this.joinType = FromParser.joinTypes[lowerInvariant];
        this.afterJoinType = true;
        this.expectingJoin = false;
        this.expectingAs = false;
      }
      else if ("class".Equals(lowerInvariant))
      {
        if (!this.afterIn)
          throw new QueryException("unexpected token: class");
        if (this.joinType != JoinType.None)
          throw new QueryException("outer or full join must be followed by path expression");
        this.afterClass = true;
      }
      else if ("in".Equals(lowerInvariant))
      {
        if (!this.expectingIn)
          throw new QueryException("unexpected token: in");
        this.afterIn = true;
        this.expectingIn = false;
      }
      else if ("as".Equals(lowerInvariant))
      {
        if (!this.expectingAs)
          throw new QueryException("unexpected token: as");
        this.afterAs = true;
        this.expectingAs = false;
      }
      else
      {
        if (this.afterJoinType)
          throw new QueryException("join expected: " + token);
        if (this.expectingJoin)
          throw new QueryException("unexpected token: " + token);
        if (this.expectingIn)
          throw new QueryException("in expected: " + token);
        if (this.afterAs || this.expectingAs)
        {
          if (this.entityName == null)
            throw new QueryException("unexpected: as " + token);
          q.SetAliasName(token, this.entityName);
          this.afterAs = false;
          this.expectingJoin = true;
          this.expectingAs = false;
          this.entityName = (string) null;
        }
        else if (this.afterIn)
        {
          if (this.alias == null)
            throw new QueryException("alias not specified for: " + token);
          if (this.joinType != JoinType.None)
            throw new QueryException("outer or full join must be followed by path expressions");
          if (this.afterClass)
          {
            q.AddFromClass(this.alias, q.GetPersisterUsingImports(token) ?? throw new QueryException("persister not found: " + token));
          }
          else
          {
            this.peParser.JoinType = JoinType.InnerJoin;
            this.peParser.UseThetaStyleJoin = true;
            ParserHelper.Parse((IParser) this.peParser, q.Unalias(token), ".", q);
            string name = this.peParser.IsCollectionValued ? this.peParser.AddFromCollection(q) : throw new QueryException("pathe expression did not resolve to collection: " + token);
            q.SetAliasName(this.alias, name);
          }
          this.alias = (string) null;
          this.afterIn = false;
          this.afterClass = false;
          this.expectingJoin = true;
        }
        else
        {
          IQueryable persisterUsingImports = q.GetPersisterUsingImports(token);
          if (persisterUsingImports != null)
          {
            if (this.joinType != JoinType.None)
              throw new QueryException("outer or full join must be followed by path expression");
            this.entityName = q.CreateNameFor(persisterUsingImports.EntityName);
            q.AddFromClass(this.entityName, persisterUsingImports);
            this.expectingAs = true;
          }
          else if (token.IndexOf('.') < 0)
          {
            this.alias = token;
            this.expectingIn = true;
          }
          else
          {
            this.peParser.JoinType = this.joinType == JoinType.None ? JoinType.InnerJoin : this.joinType;
            this.peParser.UseThetaStyleJoin = q.IsSubquery;
            ParserHelper.Parse((IParser) this.peParser, q.Unalias(token), ".", q);
            this.entityName = this.peParser.AddFromAssociation(q);
            this.joinType = JoinType.None;
            this.peParser.JoinType = JoinType.InnerJoin;
            if (this.afterFetch)
            {
              this.peParser.Fetch(q, this.entityName);
              this.afterFetch = false;
            }
            this.expectingAs = true;
          }
        }
      }
    }

    public virtual void Start(QueryTranslator q)
    {
      this.entityName = (string) null;
      this.alias = (string) null;
      this.afterIn = false;
      this.afterAs = false;
      this.afterClass = false;
      this.expectingJoin = false;
      this.expectingIn = false;
      this.expectingAs = false;
      this.joinType = JoinType.None;
    }

    public virtual void End(QueryTranslator q)
    {
      if (this.alias != null && this.expectingIn)
        throw new QueryException("in expected: <end-of-text> (possibly an invalid or unmapped class name was used in the query)");
    }
  }
}
