// Decompiled with JetBrains decompiler
// Type: NHibernate.Connection.ConnectionProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Driver;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

#nullable disable
namespace NHibernate.Connection
{
  public abstract class ConnectionProvider : IConnectionProvider, IDisposable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ConnectionProvider));
    private string connString;
    private IDriver driver;
    private bool _isAlreadyDisposed;

    public virtual void CloseConnection(IDbConnection conn)
    {
      ConnectionProvider.log.Debug((object) "Closing connection");
      try
      {
        conn.Close();
      }
      catch (Exception ex)
      {
        throw new ADOException("Could not close " + (object) conn.GetType() + " connection", ex);
      }
    }

    public virtual void Configure(IDictionary<string, string> settings)
    {
      ConnectionProvider.log.Info((object) "Configuring ConnectionProvider");
      if (!settings.TryGetValue("connection.connection_string", out this.connString))
        this.connString = this.GetNamedConnectionString(settings);
      if (this.connString == null)
        throw new HibernateException("Could not find connection string setting (set connection.connection_string or connection.connection_string_name property)");
      this.ConfigureDriver(settings);
    }

    protected virtual string GetNamedConnectionString(IDictionary<string, string> settings)
    {
      string name;
      if (!settings.TryGetValue("connection.connection_string_name", out name))
        return (string) null;
      return (ConfigurationManager.ConnectionStrings[name] ?? throw new HibernateException(string.Format("Could not find named connection string {0}", (object) name))).ConnectionString;
    }

    protected virtual void ConfigureDriver(IDictionary<string, string> settings)
    {
      string name;
      if (!settings.TryGetValue("connection.driver_class", out name))
        throw new HibernateException("The connection.driver_class must be specified in the NHibernate configuration section.");
      try
      {
        this.driver = (IDriver) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name));
        this.driver.Configure(settings);
      }
      catch (Exception ex)
      {
        throw new HibernateException("Could not create the driver from " + name + ".", ex);
      }
    }

    protected virtual string ConnectionString => this.connString;

    public IDriver Driver => this.driver;

    public abstract IDbConnection GetConnection();

    ~ConnectionProvider() => this.Dispose(false);

    public void Dispose() => this.Dispose(true);

    protected virtual void Dispose(bool isDisposing)
    {
      if (this._isAlreadyDisposed)
        return;
      if (isDisposing)
        ConnectionProvider.log.Debug((object) "Disposing of ConnectionProvider.");
      this._isAlreadyDisposed = true;
      GC.SuppressFinalize((object) this);
    }
  }
}
