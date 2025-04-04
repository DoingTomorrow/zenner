// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.RootReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class RootReturn : NonScalarReturn
  {
    private readonly string entityName;
    private readonly IEntityAliases entityAliases;

    public RootReturn(
      string alias,
      string entityName,
      IEntityAliases entityAliases,
      LockMode lockMode)
      : base(alias, lockMode)
    {
      this.entityName = entityName;
      this.entityAliases = entityAliases;
    }

    public string EntityName => this.entityName;

    public IEntityAliases EntityAliases => this.entityAliases;
  }
}
