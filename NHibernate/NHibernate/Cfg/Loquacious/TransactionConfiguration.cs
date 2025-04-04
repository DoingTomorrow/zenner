// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.TransactionConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Transaction;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class TransactionConfiguration : ITransactionConfiguration
  {
    private readonly DbIntegrationConfiguration dbc;

    public TransactionConfiguration(DbIntegrationConfiguration dbc) => this.dbc = dbc;

    public IDbIntegrationConfiguration Through<TFactory>() where TFactory : ITransactionFactory
    {
      this.dbc.Configuration.SetProperty("transaction.factory_class", typeof (TFactory).AssemblyQualifiedName);
      return (IDbIntegrationConfiguration) this.dbc;
    }
  }
}
