// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.FetchReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Loader.Custom
{
  public abstract class FetchReturn : NonScalarReturn
  {
    private readonly NonScalarReturn owner;
    private readonly string ownerProperty;

    public FetchReturn(
      NonScalarReturn owner,
      string ownerProperty,
      string alias,
      LockMode lockMode)
      : base(alias, lockMode)
    {
      this.owner = owner;
      this.ownerProperty = ownerProperty;
    }

    public NonScalarReturn Owner => this.owner;

    public string OwnerProperty => this.ownerProperty;
  }
}
