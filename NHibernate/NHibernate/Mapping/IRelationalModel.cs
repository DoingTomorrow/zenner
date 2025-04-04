// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IRelationalModel
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;

#nullable disable
namespace NHibernate.Mapping
{
  public interface IRelationalModel
  {
    string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema);

    string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog, string defaultSchema);
  }
}
