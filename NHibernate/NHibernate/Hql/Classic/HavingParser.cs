// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.HavingParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class HavingParser : WhereParser
  {
    protected override void AppendToken(QueryTranslator q, string token)
    {
      if (token == null || token.Length <= 0)
        return;
      q.AppendHavingToken(new SqlString(token));
    }

    protected override void AppendToken(QueryTranslator q, SqlString token)
    {
      q.AppendHavingToken(token);
    }
  }
}
