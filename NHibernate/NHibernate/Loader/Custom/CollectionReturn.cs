// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.CollectionReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class CollectionReturn : NonScalarReturn
  {
    private readonly string ownerEntityName;
    private readonly string ownerProperty;
    private readonly ICollectionAliases collectionAliases;
    private readonly IEntityAliases elementEntityAliases;

    public CollectionReturn(
      string alias,
      string ownerEntityName,
      string ownerProperty,
      ICollectionAliases collectionAliases,
      IEntityAliases elementEntityAliases,
      LockMode lockMode)
      : base(alias, lockMode)
    {
      this.ownerEntityName = ownerEntityName;
      this.ownerProperty = ownerProperty;
      this.collectionAliases = collectionAliases;
      this.elementEntityAliases = elementEntityAliases;
    }

    public string OwnerEntityName => this.ownerEntityName;

    public string OwnerProperty => this.ownerProperty;

    public ICollectionAliases CollectionAliases => this.collectionAliases;

    public IEntityAliases ElementEntityAliases => this.elementEntityAliases;
  }
}
