// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.GroupByParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class GroupByParser : IParser
  {
    private readonly PathExpressionParser pathExpressionParser = new PathExpressionParser();

    public void Token(string token, QueryTranslator q)
    {
      if (q.IsName(StringHelper.Root(token)))
      {
        ParserHelper.Parse((IParser) this.pathExpressionParser, q.Unalias(token), ".", q);
        q.AppendGroupByToken(this.pathExpressionParser.WhereColumn);
        this.pathExpressionParser.AddAssociation(q);
      }
      else if (token.StartsWith(":"))
      {
        string name = token.Substring(1);
        q.AppendGroupByParameter(name);
      }
      else if (token.Equals("?"))
        q.AppendGroupByParameter();
      else
        q.AppendGroupByToken(token);
    }

    public void Start(QueryTranslator q)
    {
    }

    public void End(QueryTranslator q)
    {
    }

    public GroupByParser() => this.pathExpressionParser.UseThetaStyleJoin = true;
  }
}
