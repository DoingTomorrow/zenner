// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SchemaMetadataUpdater
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public static class SchemaMetadataUpdater
  {
    public static void Update(ISessionFactory sessionFactory)
    {
      ISessionFactoryImplementor factoryImplementor = (ISessionFactoryImplementor) sessionFactory;
      NHibernate.Dialect.Dialect dialect = factoryImplementor.Dialect;
      SuppliedConnectionProviderConnectionHelper connectionHelper = new SuppliedConnectionProviderConnectionHelper(factoryImplementor.ConnectionProvider);
      factoryImplementor.Dialect.Keywords.AddAll((ICollection<string>) SchemaMetadataUpdater.GetReservedWords(dialect, (IConnectionHelper) connectionHelper));
    }

    public static void QuoteTableAndColumns(Configuration configuration)
    {
      ISet<string> reservedWords = SchemaMetadataUpdater.GetReservedWords(configuration.GetDerivedProperties());
      foreach (PersistentClass classMapping in (IEnumerable<PersistentClass>) configuration.ClassMappings)
        SchemaMetadataUpdater.QuoteTable(classMapping.Table, (ICollection<string>) reservedWords);
      foreach (Collection collectionMapping in (IEnumerable<Collection>) configuration.CollectionMappings)
        SchemaMetadataUpdater.QuoteTable(collectionMapping.Table, (ICollection<string>) reservedWords);
    }

    private static ISet<string> GetReservedWords(IDictionary<string, string> cfgProperties)
    {
      return SchemaMetadataUpdater.GetReservedWords(NHibernate.Dialect.Dialect.GetDialect(cfgProperties), (IConnectionHelper) new ManagedProviderConnectionHelper(cfgProperties));
    }

    private static ISet<string> GetReservedWords(
      NHibernate.Dialect.Dialect dialect,
      IConnectionHelper connectionHelper)
    {
      ISet<string> reservedWords = (ISet<string>) new HashedSet<string>();
      connectionHelper.Prepare();
      try
      {
        foreach (string reservedWord in (IEnumerable<string>) dialect.GetDataBaseSchema(connectionHelper.Connection).GetReservedWords())
          reservedWords.Add(reservedWord.ToLowerInvariant());
      }
      finally
      {
        connectionHelper.Release();
      }
      return reservedWords;
    }

    private static void QuoteTable(Table table, ICollection<string> reservedDb)
    {
      if (!table.IsQuoted && reservedDb.Contains(table.Name.ToLowerInvariant()))
        table.Name = SchemaMetadataUpdater.GetNhQuoted(table.Name);
      foreach (Column column in table.ColumnIterator)
      {
        if (!column.IsQuoted && reservedDb.Contains(column.Name.ToLowerInvariant()))
          column.Name = SchemaMetadataUpdater.GetNhQuoted(column.Name);
      }
    }

    private static string GetNhQuoted(string name) => "`" + name + "`";
  }
}
