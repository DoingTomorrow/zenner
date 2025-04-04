// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.NonScalarReturn
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Loader.Custom
{
  public abstract class NonScalarReturn : IReturn
  {
    private readonly string alias;
    private readonly LockMode lockMode;

    public NonScalarReturn(string alias, LockMode lockMode)
    {
      this.alias = alias;
      if (string.IsNullOrEmpty(alias))
        throw new HibernateException("alias must be specified");
      this.lockMode = lockMode;
    }

    public string Alias => this.alias;

    public LockMode LockMode => this.lockMode;
  }
}
