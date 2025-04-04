// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.IJoinable
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface IJoinable
  {
    string Name { get; }

    string[] KeyColumnNames { get; }

    bool IsCollection { get; }

    string TableName { get; }

    string SelectFragment(
      IJoinable rhs,
      string rhsAlias,
      string lhsAlias,
      string currentEntitySuffix,
      string currentCollectionSuffix,
      bool includeCollectionColumns);

    SqlString WhereJoinFragment(string alias, bool innerJoin, bool includeSubclasses);

    SqlString FromJoinFragment(string alias, bool innerJoin, bool includeSubclasses);

    string FilterFragment(string alias, IDictionary<string, IFilter> enabledFilters);

    string OneToManyFilterFragment(string alias);

    bool ConsumesEntityAlias();

    bool ConsumesCollectionAlias();
  }
}
