// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.CollectionSubqueryFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql
{
  public class CollectionSubqueryFactory
  {
    public static string CreateCollectionSubquery(
      JoinSequence joinSequence,
      IDictionary<string, IFilter> enabledFilters,
      string[] columns)
    {
      try
      {
        JoinFragment joinFragment = joinSequence.ToJoinFragment(enabledFilters, true);
        return new StringBuilder().Append("select ").Append(StringHelper.Join(", ", (IEnumerable) columns)).Append(" from ").Append((object) joinFragment.ToFromFragmentString.Substring(2)).Append(" where ").Append((object) joinFragment.ToWhereFragmentString.Substring(5)).ToString();
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
    }
  }
}
