// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SchemaValidator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class SchemaValidator
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SchemaValidator));
    private readonly Configuration configuration;
    private readonly IConnectionHelper connectionHelper;
    private readonly NHibernate.Dialect.Dialect dialect;

    public SchemaValidator(Configuration cfg)
      : this(cfg, cfg.Properties)
    {
    }

    public SchemaValidator(Configuration cfg, IDictionary<string, string> connectionProperties)
    {
      this.configuration = cfg;
      this.dialect = NHibernate.Dialect.Dialect.GetDialect(connectionProperties);
      IDictionary<string, string> cfgProperties = (IDictionary<string, string>) new Dictionary<string, string>(this.dialect.DefaultProperties);
      foreach (KeyValuePair<string, string> connectionProperty in (IEnumerable<KeyValuePair<string, string>>) connectionProperties)
        cfgProperties[connectionProperty.Key] = connectionProperty.Value;
      this.connectionHelper = (IConnectionHelper) new ManagedProviderConnectionHelper(cfgProperties);
    }

    public SchemaValidator(Configuration cfg, Settings settings)
    {
      this.configuration = cfg;
      this.dialect = settings.Dialect;
      this.connectionHelper = (IConnectionHelper) new SuppliedConnectionProviderConnectionHelper(settings.ConnectionProvider);
    }

    public static void Main(string[] args)
    {
      try
      {
        Configuration cfg = new Configuration();
        for (int index = 0; index < args.Length; ++index)
        {
          if (args[index].StartsWith("--"))
          {
            if (args[index].StartsWith("--config="))
              cfg.Configure(args[index].Substring(9));
            else if (args[index].StartsWith("--naming="))
              cfg.SetNamingStrategy((INamingStrategy) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(args[index].Substring(9))));
          }
          else
            cfg.AddFile(args[index]);
        }
        new SchemaValidator(cfg).Validate();
      }
      catch (Exception ex)
      {
        SchemaValidator.log.Error((object) "Error running schema update", ex);
        Console.WriteLine((object) ex);
      }
    }

    public void Validate()
    {
      SchemaValidator.log.Info((object) "Running schema validator");
      try
      {
        DatabaseMetadata databaseMetadata;
        try
        {
          SchemaValidator.log.Info((object) "fetching database metadata");
          this.connectionHelper.Prepare();
          databaseMetadata = new DatabaseMetadata(this.connectionHelper.Connection, this.dialect, false);
        }
        catch (Exception ex)
        {
          SchemaValidator.log.Error((object) "could not get database metadata", ex);
          throw;
        }
        this.configuration.ValidateSchema(this.dialect, databaseMetadata);
      }
      catch (Exception ex)
      {
        SchemaValidator.log.Error((object) "could not complete schema validation", ex);
        throw;
      }
      finally
      {
        try
        {
          this.connectionHelper.Release();
        }
        catch (Exception ex)
        {
          SchemaValidator.log.Error((object) "Error closing connection", ex);
        }
      }
    }
  }
}
