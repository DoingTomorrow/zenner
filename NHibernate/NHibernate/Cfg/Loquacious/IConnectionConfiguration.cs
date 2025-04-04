// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.IConnectionConfiguration
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
  public interface IConnectionConfiguration
  {
    IConnectionConfiguration Through<TProvider>() where TProvider : IConnectionProvider;

    IConnectionConfiguration By<TDriver>() where TDriver : IDriver;

    IConnectionConfiguration With(IsolationLevel level);

    IConnectionConfiguration Releasing(ConnectionReleaseMode releaseMode);

    IDbIntegrationConfiguration Using(string connectionString);

    IDbIntegrationConfiguration Using(DbConnectionStringBuilder connectionStringBuilder);

    IDbIntegrationConfiguration ByAppConfing(string connectionStringName);
  }
}
