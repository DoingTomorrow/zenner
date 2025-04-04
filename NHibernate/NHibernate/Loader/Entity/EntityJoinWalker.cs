// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.EntityJoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public class EntityJoinWalker : AbstractEntityJoinWalker
  {
    private readonly LockMode lockMode;

    public EntityJoinWalker(
      IOuterJoinLoadable persister,
      string[] uniqueKey,
      int batchSize,
      LockMode lockMode,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(persister, factory, enabledFilters)
    {
      this.lockMode = lockMode;
      this.InitAll(this.WhereString(this.Alias, uniqueKey, batchSize).Add(persister.FilterFragment(this.Alias, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>())).ToSqlString(), SqlString.Empty, lockMode);
    }

    protected override string GenerateAliasForColumn(string rootAlias, string column)
    {
      return this.Persister.GenerateTableAliasForColumn(rootAlias, column);
    }

    protected override bool IsJoinedFetchEnabled(
      IAssociationType type,
      FetchMode config,
      CascadeStyle cascadeStyle)
    {
      return !this.lockMode.GreaterThan(LockMode.Read) && base.IsJoinedFetchEnabled(type, config, cascadeStyle);
    }

    public override string Comment => "load " + this.Persister.EntityName;
  }
}
