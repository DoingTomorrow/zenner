// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.Dialects.ExtendedOracle10gDialect
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Type;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions.Dialects
{
  public class ExtendedOracle10gDialect : Oracle10gDialect
  {
    public ExtendedOracle10gDialect()
    {
      this.RegisterFunction("toDate", (ISQLFunction) new StandardSQLFunction("to_date", (IType) NHibernateUtil.Date));
      this.RegisterFunction("countAll", (ISQLFunction) new NoArgSQLFunction("count(*)", (IType) NHibernateUtil.Int32, false));
    }

    public override bool IsKnownToken(string currentToken, string nextToken)
    {
      return base.IsKnownToken(currentToken, nextToken);
    }
  }
}
