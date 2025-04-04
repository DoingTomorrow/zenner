// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SchemaUpdate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Cfg;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class SchemaUpdate
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SchemaUpdate));
    private readonly Configuration configuration;
    private readonly IConnectionHelper connectionHelper;
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly List<Exception> exceptions;
    private IFormatter formatter;

    public SchemaUpdate(Configuration cfg)
      : this(cfg, cfg.Properties)
    {
    }

    public SchemaUpdate(Configuration cfg, IDictionary<string, string> configProperties)
    {
      this.configuration = cfg;
      this.dialect = NHibernate.Dialect.Dialect.GetDialect(configProperties);
      Dictionary<string, string> cfgProperties = new Dictionary<string, string>(this.dialect.DefaultProperties);
      foreach (KeyValuePair<string, string> configProperty in (IEnumerable<KeyValuePair<string, string>>) configProperties)
        cfgProperties[configProperty.Key] = configProperty.Value;
      this.connectionHelper = (IConnectionHelper) new ManagedProviderConnectionHelper((IDictionary<string, string>) cfgProperties);
      this.exceptions = new List<Exception>();
      this.formatter = (PropertiesHelper.GetBoolean("format_sql", configProperties, true) ? FormatStyle.Ddl : FormatStyle.None).Formatter;
    }

    public SchemaUpdate(Configuration cfg, Settings settings)
    {
      this.configuration = cfg;
      this.dialect = settings.Dialect;
      this.connectionHelper = (IConnectionHelper) new SuppliedConnectionProviderConnectionHelper(settings.ConnectionProvider);
      this.exceptions = new List<Exception>();
      this.formatter = (settings.SqlStatementLogger.FormatSql ? FormatStyle.Ddl : FormatStyle.None).Formatter;
    }

    public IList<Exception> Exceptions => (IList<Exception>) this.exceptions;

    public static void Main(string[] args)
    {
      try
      {
        Configuration cfg = new Configuration();
        bool script = true;
        bool doUpdate = true;
        for (int index = 0; index < args.Length; ++index)
        {
          if (args[index].StartsWith("--"))
          {
            if (args[index].Equals("--quiet"))
            {
              script = false;
            }
            else
            {
              if (args[index].StartsWith("--properties="))
                throw new NotSupportedException("No properties file for .NET, use app.config instead");
              if (args[index].StartsWith("--config="))
                cfg.Configure(args[index].Substring(9));
              else if (args[index].StartsWith("--text"))
                doUpdate = false;
              else if (args[index].StartsWith("--naming="))
                cfg.SetNamingStrategy((INamingStrategy) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(args[index].Substring(9))));
            }
          }
          else
            cfg.AddFile(args[index]);
        }
        new SchemaUpdate(cfg).Execute(script, doUpdate);
      }
      catch (Exception ex)
      {
        SchemaUpdate.log.Error((object) "Error running schema update", ex);
        Console.WriteLine((object) ex);
      }
    }

    public void Execute(bool script, bool doUpdate)
    {
      if (script)
        this.Execute(new Action<string>(Console.WriteLine), doUpdate);
      else
        this.Execute((Action<string>) null, doUpdate);
    }

    public void Execute(Action<string> scriptAction, bool doUpdate)
    {
      SchemaUpdate.log.Info((object) "Running hbm2ddl schema update");
      if (PropertiesHelper.GetString("hbm2ddl.keywords", this.configuration.Properties, "not-defined").ToLowerInvariant() == Hbm2DDLKeyWords.AutoQuote)
        SchemaMetadataUpdater.QuoteTableAndColumns(this.configuration);
      IDbCommand dbCommand = (IDbCommand) null;
      this.exceptions.Clear();
      try
      {
        DatabaseMetadata databaseMetadata;
        try
        {
          SchemaUpdate.log.Info((object) "fetching database metadata");
          this.connectionHelper.Prepare();
          DbConnection connection = this.connectionHelper.Connection;
          databaseMetadata = new DatabaseMetadata(connection, this.dialect);
          dbCommand = (IDbCommand) connection.CreateCommand();
        }
        catch (Exception ex)
        {
          this.exceptions.Add(ex);
          SchemaUpdate.log.Error((object) "could not get database metadata", ex);
          throw;
        }
        SchemaUpdate.log.Info((object) "updating schema");
        foreach (string str1 in this.configuration.GenerateSchemaUpdateScript(this.dialect, databaseMetadata))
        {
          string str2 = this.formatter.Format(str1);
          try
          {
            if (scriptAction != null)
              scriptAction(str2);
            if (doUpdate)
            {
              SchemaUpdate.log.Debug((object) str1);
              dbCommand.CommandText = str1;
              dbCommand.ExecuteNonQuery();
            }
          }
          catch (Exception ex)
          {
            this.exceptions.Add(ex);
            SchemaUpdate.log.Error((object) ("Unsuccessful: " + str1), ex);
          }
        }
        SchemaUpdate.log.Info((object) "schema update complete");
      }
      catch (Exception ex)
      {
        this.exceptions.Add(ex);
        SchemaUpdate.log.Error((object) "could not complete schema update", ex);
      }
      finally
      {
        try
        {
          dbCommand?.Dispose();
          this.connectionHelper.Release();
        }
        catch (Exception ex)
        {
          this.exceptions.Add(ex);
          SchemaUpdate.log.Error((object) "Error closing connection", ex);
        }
      }
    }
  }
}
