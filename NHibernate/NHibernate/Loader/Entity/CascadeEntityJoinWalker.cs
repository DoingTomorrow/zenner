// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.CascadeEntityJoinWalker
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
  public class CascadeEntityJoinWalker : AbstractEntityJoinWalker
  {
    private readonly CascadingAction cascadeAction;

    public CascadeEntityJoinWalker(
      IOuterJoinLoadable persister,
      CascadingAction action,
      ISessionFactoryImplementor factory)
      : base(persister, factory, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>())
    {
      this.cascadeAction = action;
      this.InitAll(this.WhereString(this.Alias, persister.IdentifierColumnNames, 1).Add(persister.FilterFragment(this.Alias, (IDictionary<string, IFilter>) new CollectionHelper.EmptyMapClass<string, IFilter>())).ToSqlString(), SqlString.Empty, LockMode.Read);
    }

    protected override bool IsJoinedFetchEnabled(
      IAssociationType type,
      FetchMode config,
      CascadeStyle cascadeStyle)
    {
      if (!type.IsEntityType && !type.IsCollectionType)
        return false;
      return cascadeStyle == null || cascadeStyle.DoCascade(this.cascadeAction);
    }

    protected override bool IsTooManyCollections
    {
      get => JoinWalker.CountCollectionPersisters(this.associations) > 0;
    }

    public override string Comment => "load " + this.Persister.EntityName;
  }
}
