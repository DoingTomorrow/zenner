// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.ClauseParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class ClauseParser : IParser
  {
    private IParser child;
    private IList<string> selectTokens;
    private bool cacheSelectTokens;
    private bool byExpected;
    private int parenCount;

    public virtual void Token(string token, QueryTranslator q)
    {
      string lowerInvariant = token.ToLowerInvariant();
      switch (token)
      {
        case "(":
          ++this.parenCount;
          break;
        case ")":
          --this.parenCount;
          break;
      }
      if (this.byExpected && !"by".Equals(lowerInvariant))
        throw new QueryException("BY expected after GROUP or ORDER: " + token);
      bool flag = this.parenCount == 0;
      if (flag)
      {
        if ("select".Equals(lowerInvariant))
        {
          this.selectTokens = (IList<string>) new List<string>();
          this.cacheSelectTokens = true;
        }
        else if ("from".Equals(lowerInvariant))
        {
          this.child = (IParser) new FromParser();
          this.child.Start(q);
          this.cacheSelectTokens = false;
        }
        else if ("where".Equals(lowerInvariant))
        {
          this.EndChild(q);
          this.child = (IParser) new WhereParser();
          this.child.Start(q);
        }
        else if ("order".Equals(lowerInvariant))
        {
          this.EndChild(q);
          this.child = (IParser) new OrderByParser();
          this.byExpected = true;
        }
        else if ("having".Equals(lowerInvariant))
        {
          this.EndChild(q);
          this.child = (IParser) new HavingParser();
          this.child.Start(q);
        }
        else if ("group".Equals(lowerInvariant))
        {
          this.EndChild(q);
          this.child = (IParser) new GroupByParser();
          this.byExpected = true;
        }
        else if ("by".Equals(lowerInvariant))
        {
          if (!this.byExpected)
            throw new QueryException("GROUP or ORDER expected before BY");
          this.child.Start(q);
          this.byExpected = false;
        }
        else
          flag = false;
      }
      if (flag)
        return;
      if (this.cacheSelectTokens)
      {
        this.selectTokens.Add(token);
      }
      else
      {
        if (this.child == null)
          throw new QueryException("query must begin with SELECT or FROM: " + token);
        this.child.Token(token, q);
      }
    }

    private void EndChild(QueryTranslator q)
    {
      if (this.child == null)
        this.cacheSelectTokens = false;
      else
        this.child.End(q);
    }

    public virtual void Start(QueryTranslator q)
    {
    }

    public virtual void End(QueryTranslator q)
    {
      this.EndChild(q);
      if (this.selectTokens != null)
      {
        this.child = (IParser) new SelectParser();
        this.child.Start(q);
        foreach (string selectToken in (IEnumerable<string>) this.selectTokens)
          this.Token(selectToken, q);
        this.child.End(q);
      }
      this.byExpected = false;
      this.parenCount = 0;
      this.cacheSelectTokens = false;
    }
  }
}
