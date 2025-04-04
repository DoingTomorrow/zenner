// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.IPersistentIdentifierGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Id
{
  public interface IPersistentIdentifierGenerator : IIdentifierGenerator
  {
    string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect);

    string[] SqlDropString(NHibernate.Dialect.Dialect dialect);

    string GeneratorKey();
  }
}
