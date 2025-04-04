// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.SequenceGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Id
{
  public class SequenceGenerator : 
    IPersistentIdentifierGenerator,
    IIdentifierGenerator,
    IConfigurable
  {
    public const string Sequence = "sequence";
    public const string Parameters = "parameters";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SequenceGenerator));
    private string sequenceName;
    private IType identifierType;
    private SqlString sql;
    private string parameters;

    public string SequenceName => this.sequenceName;

    public virtual void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string str1 = PropertiesHelper.GetString("sequence", parms, "hibernate_sequence");
      bool flag1 = StringHelper.IsBackticksEnclosed(str1);
      bool flag2 = str1.IndexOf('.') > 0;
      if (flag2)
      {
        string str2 = StringHelper.Qualifier(str1);
        string tableName = StringHelper.PurgeBackticksEnclosing(StringHelper.Unqualify(str1));
        this.sequenceName = str2 + (object) '.' + (flag1 ? (object) dialect.QuoteForTableName(tableName) : (object) tableName);
      }
      else
      {
        string tableName = StringHelper.PurgeBackticksEnclosing(str1);
        this.sequenceName = flag1 ? dialect.QuoteForTableName(tableName) : tableName;
      }
      parms.TryGetValue("parameters", out this.parameters);
      string schema;
      parms.TryGetValue(PersistentIdGeneratorParmsNames.Schema, out schema);
      string catalog;
      parms.TryGetValue(PersistentIdGeneratorParmsNames.Catalog, out catalog);
      if (!flag2)
        this.sequenceName = dialect.Qualify(catalog, schema, this.sequenceName);
      this.identifierType = type;
      this.sql = new SqlString(dialect.GetSequenceNextValString(this.sequenceName));
    }

    public virtual object Generate(ISessionImplementor session, object obj)
    {
      try
      {
        IDbCommand cmd = session.Batcher.PrepareCommand(CommandType.Text, this.sql, SqlTypeFactory.NoTypes);
        IDataReader dataReader = (IDataReader) null;
        try
        {
          dataReader = session.Batcher.ExecuteReader(cmd);
          try
          {
            dataReader.Read();
            object obj1 = IdentifierGeneratorFactory.Get(dataReader, this.identifierType, session);
            if (SequenceGenerator.log.IsDebugEnabled)
              SequenceGenerator.log.Debug((object) ("Sequence identifier generated: " + obj1));
            return obj1;
          }
          finally
          {
            dataReader.Close();
          }
        }
        finally
        {
          session.Batcher.CloseCommand(cmd, dataReader);
        }
      }
      catch (DbException ex)
      {
        SequenceGenerator.log.Error((object) "error generating sequence", (Exception) ex);
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not get next sequence value");
      }
    }

    public string[] SqlCreateStrings(NHibernate.Dialect.Dialect dialect)
    {
      string createSequenceString = dialect.GetCreateSequenceString(this.sequenceName);
      string str = (string) null;
      if (this.parameters != null)
        str = ' '.ToString() + this.parameters;
      return new string[1]{ createSequenceString + str };
    }

    public string[] SqlDropString(NHibernate.Dialect.Dialect dialect)
    {
      return new string[1]
      {
        dialect.GetDropSequenceString(this.sequenceName)
      };
    }

    public string GeneratorKey() => this.sequenceName;
  }
}
