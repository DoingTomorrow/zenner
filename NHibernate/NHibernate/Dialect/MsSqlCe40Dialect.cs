// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSqlCe40Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSqlCe40Dialect : MsSqlCeDialect
  {
    public MsSqlCe40Dialect()
    {
      this.RegisterFunction("concat", (ISQLFunction) new VarArgsSQLFunction((IType) NHibernateUtil.String, "(", "+", ")"));
    }

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(queryString);
      if (queryString.IndexOfCaseInsensitive(" ORDER BY ") < 0)
        sqlStringBuilder.Add(" ORDER BY GETDATE()");
      sqlStringBuilder.Add(" OFFSET ");
      if (offset == null)
        sqlStringBuilder.Add("0");
      else
        sqlStringBuilder.Add(offset);
      sqlStringBuilder.Add(" ROWS");
      if (limit != null)
      {
        sqlStringBuilder.Add(" FETCH NEXT ");
        sqlStringBuilder.Add(limit);
        sqlStringBuilder.Add(" ROWS ONLY");
      }
      return sqlStringBuilder.ToSqlString();
    }
  }
}
