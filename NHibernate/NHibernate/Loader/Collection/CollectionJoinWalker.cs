// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.CollectionJoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Collection
{
  public abstract class CollectionJoinWalker : JoinWalker
  {
    public CollectionJoinWalker(
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
    }

    protected SqlStringBuilder WhereString(
      string alias,
      string[] columnNames,
      SqlString subselect,
      int batchSize)
    {
      if (subselect == null)
        return this.WhereString(alias, columnNames, batchSize);
      string sql = StringHelper.Join(", ", (IEnumerable) StringHelper.Qualify(alias, columnNames));
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (columnNames.Length > 1)
        sqlStringBuilder.Add("(").Add(sql).Add(")");
      else
        sqlStringBuilder.Add(sql);
      sqlStringBuilder.Add(" in ").Add("(").Add(subselect).Add(")");
      return sqlStringBuilder;
    }
  }
}
