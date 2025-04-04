// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SchemaExport
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet.Util;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class SchemaExport
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SchemaExport));
    private bool wasInitialized;
    private readonly Configuration cfg;
    private readonly IDictionary<string, string> configProperties;
    private string[] createSQL;
    private NHibernate.Dialect.Dialect dialect;
    private string[] dropSQL;
    private IFormatter formatter;
    private string delimiter;
    private string outputFile;

    public SchemaExport(Configuration cfg)
      : this(cfg, cfg.Properties)
    {
    }

    public SchemaExport(Configuration cfg, IDictionary<string, string> configProperties)
    {
      this.cfg = cfg;
      this.configProperties = configProperties;
    }

    private void Initialize()
    {
      if (this.wasInitialized)
        return;
      if (PropertiesHelper.GetString("hbm2ddl.keywords", this.configProperties, "not-defined").ToLowerInvariant() == Hbm2DDLKeyWords.AutoQuote)
        SchemaMetadataUpdater.QuoteTableAndColumns(this.cfg);
      this.dialect = NHibernate.Dialect.Dialect.GetDialect(this.configProperties);
      this.dropSQL = this.cfg.GenerateDropSchemaScript(this.dialect);
      this.createSQL = this.cfg.GenerateSchemaCreationScript(this.dialect);
      this.formatter = (PropertiesHelper.GetBoolean("format_sql", this.configProperties, true) ? FormatStyle.Ddl : FormatStyle.None).Formatter;
      this.wasInitialized = true;
    }

    public SchemaExport SetOutputFile(string filename)
    {
      this.outputFile = filename;
      return this;
    }

    public SchemaExport SetDelimiter(string delimiter)
    {
      this.delimiter = delimiter;
      return this;
    }

    public void Create(bool script, bool export) => this.Execute(script, export, false);

    public void Create(Action<string> scriptAction, bool export)
    {
      this.Execute(scriptAction, export, false);
    }

    public void Drop(bool script, bool export) => this.Execute(script, export, true);

    private void Execute(
      Action<string> scriptAction,
      bool export,
      bool throwOnError,
      TextWriter exportOutput,
      IDbCommand statement,
      string sql)
    {
      this.Initialize();
      try
      {
        string message = this.formatter.Format(sql);
        if (this.delimiter != null)
          message += this.delimiter;
        if (scriptAction != null)
          scriptAction(message);
        SchemaExport.log.Debug((object) message);
        exportOutput?.WriteLine(message);
        if (!export)
          return;
        this.ExecuteSql(statement, sql);
      }
      catch (Exception ex)
      {
        SchemaExport.log.Warn((object) ("Unsuccessful: " + sql));
        SchemaExport.log.Warn((object) ex.Message);
        if (!throwOnError)
          return;
        throw;
      }
    }

    private void ExecuteSql(IDbCommand cmd, string sql)
    {
      if (this.dialect.SupportsSqlBatches)
      {
        foreach (string str in (ScriptSplitter) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(typeof (ScriptSplitter), (object) sql))
        {
          SchemaExport.log.DebugFormat("SQL Batch: {0}", (object) str);
          cmd.CommandText = str;
          cmd.CommandType = CommandType.Text;
          cmd.ExecuteNonQuery();
        }
      }
      else
      {
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
      }
    }

    public void Execute(
      bool script,
      bool export,
      bool justDrop,
      IDbConnection connection,
      TextWriter exportOutput)
    {
      if (script)
        this.Execute(new Action<string>(Console.WriteLine), export, justDrop, connection, exportOutput);
      else
        this.Execute((Action<string>) null, export, justDrop, connection, exportOutput);
    }

    public void Execute(
      Action<string> scriptAction,
      bool export,
      bool justDrop,
      IDbConnection connection,
      TextWriter exportOutput)
    {
      this.Initialize();
      IDbCommand statement = (IDbCommand) null;
      if (export && connection == null)
        throw new ArgumentNullException(nameof (connection), "When export is set to true, you need to pass a non null connection");
      if (export)
        statement = connection.CreateCommand();
      try
      {
        for (int index = 0; index < this.dropSQL.Length; ++index)
          this.Execute(scriptAction, export, false, exportOutput, statement, this.dropSQL[index]);
        if (justDrop)
          return;
        for (int index = 0; index < this.createSQL.Length; ++index)
          this.Execute(scriptAction, export, true, exportOutput, statement, this.createSQL[index]);
      }
      finally
      {
        try
        {
          statement?.Dispose();
        }
        catch (Exception ex)
        {
          SchemaExport.log.Error((object) ("Could not close connection: " + ex.Message), ex);
        }
        if (exportOutput != null)
        {
          try
          {
            exportOutput.Close();
          }
          catch (Exception ex)
          {
            SchemaExport.log.Error((object) ("Error closing output file " + this.outputFile + ": " + ex.Message), ex);
          }
        }
      }
    }

    public void Execute(bool script, bool export, bool justDrop)
    {
      if (script)
        this.Execute(new Action<string>(Console.WriteLine), export, justDrop);
      else
        this.Execute((Action<string>) null, export, justDrop);
    }

    public void Execute(Action<string> scriptAction, bool export, bool justDrop)
    {
      this.Initialize();
      IDbConnection dbConnection = (IDbConnection) null;
      StreamWriter exportOutput = (StreamWriter) null;
      IConnectionProvider connectionProvider = (IConnectionProvider) null;
      Dictionary<string, string> settings = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> defaultProperty in (IEnumerable<KeyValuePair<string, string>>) this.dialect.DefaultProperties)
        settings[defaultProperty.Key] = defaultProperty.Value;
      if (this.configProperties != null)
      {
        foreach (KeyValuePair<string, string> configProperty in (IEnumerable<KeyValuePair<string, string>>) this.configProperties)
          settings[configProperty.Key] = configProperty.Value;
      }
      try
      {
        if (this.outputFile != null)
          exportOutput = new StreamWriter(this.outputFile);
        if (export)
        {
          connectionProvider = ConnectionProviderFactory.NewConnectionProvider((IDictionary<string, string>) settings);
          dbConnection = connectionProvider.GetConnection();
        }
        this.Execute(scriptAction, export, justDrop, dbConnection, (TextWriter) exportOutput);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        SchemaExport.log.Error((object) ex.Message, ex);
        throw new HibernateException(ex.Message, ex);
      }
      finally
      {
        if (dbConnection != null)
        {
          connectionProvider.CloseConnection(dbConnection);
          connectionProvider.Dispose();
        }
      }
    }
  }
}
