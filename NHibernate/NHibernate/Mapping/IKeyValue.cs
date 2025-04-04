// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.IKeyValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Id;

#nullable disable
namespace NHibernate.Mapping
{
  public interface IKeyValue : IValue
  {
    void CreateForeignKeyOfEntity(string entityName);

    bool IsCascadeDeleteEnabled { get; }

    bool IsIdentityColumn(NHibernate.Dialect.Dialect dialect);

    string NullValue { get; }

    bool IsUpdateable { get; }

    IIdentifierGenerator CreateIdentifierGenerator(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema,
      RootClass rootClass);
  }
}
