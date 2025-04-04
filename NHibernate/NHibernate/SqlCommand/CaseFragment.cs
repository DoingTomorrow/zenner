// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.CaseFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.SqlCommand
{
  public abstract class CaseFragment
  {
    protected internal readonly NHibernate.Dialect.Dialect dialect;
    protected internal string returnColumnName;
    protected internal IDictionary<string, string> cases = (IDictionary<string, string>) new LinkedHashMap<string, string>();

    protected CaseFragment(NHibernate.Dialect.Dialect dialect) => this.dialect = dialect;

    public virtual CaseFragment SetReturnColumnName(string returnColumnName)
    {
      this.returnColumnName = returnColumnName;
      return this;
    }

    public virtual CaseFragment SetReturnColumnName(string returnColumnName, string suffix)
    {
      return this.SetReturnColumnName(new Alias(suffix).ToAliasString(returnColumnName, this.dialect));
    }

    public virtual CaseFragment AddWhenColumnNotNull(string alias, string columnName, string value)
    {
      this.cases[StringHelper.Qualify(alias, columnName)] = value;
      return this;
    }

    public abstract string ToSqlStringFragment();
  }
}
