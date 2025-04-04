// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.IDbIntegrationConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  public interface IDbIntegrationConfiguration
  {
    IDbIntegrationConfiguration Using<TDialect>() where TDialect : NHibernate.Dialect.Dialect;

    IDbIntegrationConfiguration DisableKeywordsAutoImport();

    IDbIntegrationConfiguration AutoQuoteKeywords();

    IDbIntegrationConfiguration LogSqlInConsole();

    IDbIntegrationConfiguration EnableLogFormattedSql();

    [Obsolete("Please use EnableLogFormattedSql. This method will be removed in a future version.")]
    IDbIntegrationConfiguration DisableLogFormatedSql();

    IConnectionConfiguration Connected { get; }

    IBatcherConfiguration BatchingQueries { get; }

    ITransactionConfiguration Transactions { get; }

    ICommandsConfiguration CreateCommands { get; }

    IDbSchemaIntegrationConfiguration Schema { get; }
  }
}
