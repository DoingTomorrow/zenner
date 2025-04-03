// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.QueryPropertyAliasesList
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class QueryPropertyAliasesList
  {
    private List<QueryPropertyAlias> propertyAliases = new List<QueryPropertyAlias>();

    protected QueryPropertyAliasesList()
    {
    }

    public static QueryPropertyAliasesList Create() => new QueryPropertyAliasesList();

    public QueryPropertyAliasesList Add(QueryPropertyAlias propertyAlias)
    {
      this.propertyAliases.Add(propertyAlias);
      return this;
    }

    public string GetPropertyForAlias(string alias)
    {
      QueryPropertyAlias queryPropertyAlias;
      return (queryPropertyAlias = Enumerable.SingleOrDefault<QueryPropertyAlias>((IEnumerable<QueryPropertyAlias>) this.propertyAliases, (Func<QueryPropertyAlias, bool>) (p => p.Alias.Equals(alias)))) != null ? queryPropertyAlias.PropertyName : (string) null;
    }
  }
}
