// Decompiled with JetBrains decompiler
// Type: NHibernate.Connection.ConnectionProviderFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Connection
{
  public sealed class ConnectionProviderFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ConnectionProviderFactory));

    private ConnectionProviderFactory()
    {
      throw new InvalidOperationException("ConnectionProviderFactory can not be instantiated.");
    }

    public static IConnectionProvider NewConnectionProvider(IDictionary<string, string> settings)
    {
      string name;
      IConnectionProvider connectionProvider;
      if (settings.TryGetValue("connection.provider", out name))
      {
        try
        {
          ConnectionProviderFactory.log.Info((object) ("Initializing connection provider: " + name));
          connectionProvider = (IConnectionProvider) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
        }
        catch (Exception ex)
        {
          ConnectionProviderFactory.log.Fatal((object) "Could not instantiate connection provider", ex);
          throw new HibernateException("Could not instantiate connection provider: " + name, ex);
        }
      }
      else if (settings.ContainsKey("connection.connection_string") || settings.ContainsKey("connection.connection_string_name"))
      {
        connectionProvider = (IConnectionProvider) new DriverConnectionProvider();
      }
      else
      {
        ConnectionProviderFactory.log.Info((object) "No connection provider specified, UserSuppliedConnectionProvider will be used.");
        connectionProvider = (IConnectionProvider) new UserSuppliedConnectionProvider();
      }
      connectionProvider.Configure(settings);
      return connectionProvider;
    }
  }
}
