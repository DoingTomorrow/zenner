// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.Sql.NativeSQLQuerySpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query.Sql
{
  public class NativeSQLQuerySpecification
  {
    private readonly string queryString;
    private readonly INativeSQLQueryReturn[] sqlQueryReturns;
    private readonly ISet<string> querySpaces;
    private readonly int hashCode;

    public NativeSQLQuerySpecification(
      string queryString,
      INativeSQLQueryReturn[] sqlQueryReturns,
      ICollection<string> querySpaces)
    {
      this.queryString = queryString;
      this.sqlQueryReturns = sqlQueryReturns;
      if (querySpaces == null)
      {
        this.querySpaces = (ISet<string>) new HashedSet<string>();
      }
      else
      {
        ISet<string> set = (ISet<string>) new HashedSet<string>();
        set.AddAll(querySpaces);
        this.querySpaces = set;
      }
      int num = 29 * queryString.GetHashCode() + CollectionHelper.GetHashCode<string>((IEnumerable<string>) this.querySpaces);
      if (this.sqlQueryReturns != null)
        num = 29 * num + CollectionHelper.GetHashCode<INativeSQLQueryReturn>((IEnumerable<INativeSQLQueryReturn>) this.sqlQueryReturns);
      this.hashCode = num;
    }

    public string QueryString => this.queryString;

    public INativeSQLQueryReturn[] SqlQueryReturns => this.sqlQueryReturns;

    public ISet<string> QuerySpaces => this.querySpaces;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is NativeSQLQuerySpecification querySpecification && this.hashCode == querySpecification.hashCode;
    }

    public override int GetHashCode() => this.hashCode;
  }
}
