// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.ConnectionConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Connection;
using NHibernate.Driver;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class ConnectionConfiguration : IConnectionConfiguration
  {
    private readonly DbIntegrationConfiguration dbc;

    public ConnectionConfiguration(DbIntegrationConfiguration dbc) => this.dbc = dbc;

    public IConnectionConfiguration Through<TProvider>() where TProvider : IConnectionProvider
    {
      this.dbc.Configuration.SetProperty("connection.provider", typeof (TProvider).AssemblyQualifiedName);
      return (IConnectionConfiguration) this;
    }

    public IConnectionConfiguration By<TDriver>() where TDriver : IDriver
    {
      this.dbc.Configuration.SetProperty("connection.driver_class", typeof (TDriver).AssemblyQualifiedName);
      return (IConnectionConfiguration) this;
    }

    public IConnectionConfiguration With(IsolationLevel level)
    {
      this.dbc.Configuration.SetProperty("connection.isolation", level.ToString());
      return (IConnectionConfiguration) this;
    }

    public IConnectionConfiguration Releasing(ConnectionReleaseMode releaseMode)
    {
      this.dbc.Configuration.SetProperty("connection.release_mode", ConnectionReleaseModeParser.ToString(releaseMode));
      return (IConnectionConfiguration) this;
    }

    public IDbIntegrationConfiguration Using(string connectionString)
    {
      this.dbc.Configuration.SetProperty("connection.connection_string", connectionString);
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration Using(DbConnectionStringBuilder connectionStringBuilder)
    {
      this.dbc.Configuration.SetProperty("connection.connection_string", connectionStringBuilder.ConnectionString);
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration ByAppConfing(string connectionStringName)
    {
      this.dbc.Configuration.SetProperty("connection.connection_string_name", connectionStringName);
      return (IDbIntegrationConfiguration) this.dbc;
    }
  }
}
