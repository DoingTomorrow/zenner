// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.BatcherConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class BatcherConfiguration : IBatcherConfiguration
  {
    private readonly DbIntegrationConfiguration dbc;

    public BatcherConfiguration(DbIntegrationConfiguration dbc) => this.dbc = dbc;

    public IBatcherConfiguration Through<TBatcher>() where TBatcher : IBatcherFactory
    {
      this.dbc.Configuration.SetProperty("adonet.factory_class", typeof (TBatcher).AssemblyQualifiedName);
      return (IBatcherConfiguration) this;
    }

    public IDbIntegrationConfiguration Each(short batchSize)
    {
      this.dbc.Configuration.SetProperty("adonet.batch_size", batchSize.ToString());
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IBatcherConfiguration OrderingInserts()
    {
      this.dbc.Configuration.SetProperty("order_inserts", true.ToString().ToLowerInvariant());
      return (IBatcherConfiguration) this;
    }

    public IBatcherConfiguration DisablingInsertsOrdering()
    {
      this.dbc.Configuration.SetProperty("order_inserts", false.ToString().ToLowerInvariant());
      return (IBatcherConfiguration) this;
    }
  }
}
