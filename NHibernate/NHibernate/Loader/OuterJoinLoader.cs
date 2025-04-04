// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.OuterJoinLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader
{
  public abstract class OuterJoinLoader : BasicLoader
  {
    private ILoadable[] persisters;
    private ICollectionPersister[] collectionPersisters;
    private int[] collectionOwners;
    private string[] aliases;
    private LockMode[] lockModeArray;
    private int[] owners;
    private EntityType[] ownerAssociationTypes;
    private SqlString sql;
    private string[] suffixes;
    private string[] collectionSuffixes;
    private readonly IDictionary<string, IFilter> enabledFilters;

    protected OuterJoinLoader(
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
      : base(factory)
    {
      this.enabledFilters = enabledFilters;
    }

    protected NHibernate.Dialect.Dialect Dialect => this.Factory.Dialect;

    protected override int[] Owners => this.owners;

    protected override EntityType[] OwnerAssociationTypes => this.ownerAssociationTypes;

    public IDictionary<string, IFilter> EnabledFilters => this.enabledFilters;

    protected override int[] CollectionOwners => this.collectionOwners;

    protected override string[] Suffixes => this.suffixes;

    protected override string[] CollectionSuffixes => this.collectionSuffixes;

    public override SqlString SqlString => this.sql;

    public override ILoadable[] EntityPersisters => this.persisters;

    public override LockMode[] GetLockModes(IDictionary<string, LockMode> lockModes)
    {
      return this.lockModeArray;
    }

    protected override string[] Aliases => this.aliases;

    protected override ICollectionPersister[] CollectionPersisters => this.collectionPersisters;

    protected void InitFromWalker(JoinWalker walker)
    {
      this.persisters = walker.Persisters;
      this.collectionPersisters = walker.CollectionPersisters;
      this.ownerAssociationTypes = walker.OwnerAssociationTypes;
      this.lockModeArray = walker.LockModeArray;
      this.suffixes = walker.Suffixes;
      this.collectionSuffixes = walker.CollectionSuffixes;
      this.owners = walker.Owners;
      this.collectionOwners = walker.CollectionOwners;
      this.sql = walker.SqlString.Compact();
      this.aliases = walker.Aliases;
    }
  }
}
