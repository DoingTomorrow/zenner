// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.DbIntegrationConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class DbIntegrationConfiguration : IDbIntegrationConfiguration
  {
    private readonly Configuration configuration;

    public DbIntegrationConfiguration(Configuration configuration)
    {
      this.configuration = configuration;
      this.Connected = (IConnectionConfiguration) new ConnectionConfiguration(this);
      this.BatchingQueries = (IBatcherConfiguration) new BatcherConfiguration(this);
      this.Transactions = (ITransactionConfiguration) new TransactionConfiguration(this);
      this.CreateCommands = (ICommandsConfiguration) new CommandsConfiguration(this);
      this.Schema = (IDbSchemaIntegrationConfiguration) new DbSchemaIntegrationConfiguration(this);
    }

    public Configuration Configuration => this.configuration;

    public IDbIntegrationConfiguration Using<TDialect>() where TDialect : NHibernate.Dialect.Dialect
    {
      this.configuration.SetProperty("dialect", typeof (TDialect).AssemblyQualifiedName);
      return (IDbIntegrationConfiguration) this;
    }

    public IDbIntegrationConfiguration DisableKeywordsAutoImport()
    {
      this.configuration.SetProperty("hbm2ddl.keywords", "none");
      return (IDbIntegrationConfiguration) this;
    }

    public IDbIntegrationConfiguration AutoQuoteKeywords()
    {
      this.configuration.SetProperty("hbm2ddl.keywords", "auto-quote");
      return (IDbIntegrationConfiguration) this;
    }

    public IDbIntegrationConfiguration LogSqlInConsole()
    {
      this.configuration.SetProperty("show_sql", "true");
      return (IDbIntegrationConfiguration) this;
    }

    [Obsolete("This method will be removed in a future version as logged SQL formatting is disabled by default. To enable SQL formatting, use EnableLogFormattedSql.")]
    public IDbIntegrationConfiguration DisableLogFormatedSql()
    {
      this.configuration.SetProperty("format_sql", "false");
      return (IDbIntegrationConfiguration) this;
    }

    public IDbIntegrationConfiguration EnableLogFormattedSql()
    {
      this.configuration.SetProperty("format_sql", "true");
      return (IDbIntegrationConfiguration) this;
    }

    public IConnectionConfiguration Connected { get; private set; }

    public IBatcherConfiguration BatchingQueries { get; private set; }

    public ITransactionConfiguration Transactions { get; private set; }

    public ICommandsConfiguration CreateCommands { get; private set; }

    public IDbSchemaIntegrationConfiguration Schema { get; private set; }
  }
}
