// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.SubselectFetch
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Collections;

#nullable disable
namespace NHibernate.Engine
{
  public class SubselectFetch
  {
    private readonly string alias;
    private readonly ILoadable loadable;
    private readonly QueryParameters queryParameters;
    private readonly SqlString queryString;
    private readonly ISet<EntityKey> resultingEntityKeys;

    public SubselectFetch(
      string alias,
      ILoadable loadable,
      QueryParameters queryParameters,
      ISet<EntityKey> resultingEntityKeys)
    {
      this.resultingEntityKeys = resultingEntityKeys;
      this.queryParameters = queryParameters;
      this.loadable = loadable;
      this.alias = alias;
      this.queryString = queryParameters.ProcessedSql.GetSubselectString();
    }

    public QueryParameters QueryParameters => this.queryParameters;

    public ISet<EntityKey> Result => this.resultingEntityKeys;

    public SqlString ToSubselectString(string ukname)
    {
      string[] objects = ukname == null ? StringHelper.Qualify(this.alias, this.loadable.IdentifierColumnNames) : ((IPropertyMapping) this.loadable).ToColumns(this.alias, ukname);
      return new SqlStringBuilder().Add("select ").Add(StringHelper.Join(", ", (IEnumerable) objects)).Add(this.queryString).ToSqlString();
    }

    public override string ToString()
    {
      return "SubselectFetch(" + (object) this.queryString + (object) ')';
    }
  }
}
