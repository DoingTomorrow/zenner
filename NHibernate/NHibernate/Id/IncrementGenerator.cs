// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.IncrementGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace NHibernate.Id
{
  public class IncrementGenerator : IIdentifierGenerator, IConfigurable
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (IncrementGenerator));
    private long next;
    private string sql;
    private System.Type returnClass;

    public void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      string str1;
      if (!parms.TryGetValue("tables", out str1))
        parms.TryGetValue(PersistentIdGeneratorParmsNames.Tables, out str1);
      string[] strArray = str1.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
      string str2;
      if (!parms.TryGetValue("column", out str2))
        parms.TryGetValue(PersistentIdGeneratorParmsNames.PK, out str2);
      this.returnClass = type.ReturnedClass;
      string schema;
      parms.TryGetValue(PersistentIdGeneratorParmsNames.Schema, out schema);
      string catalog;
      parms.TryGetValue(PersistentIdGeneratorParmsNames.Catalog, out catalog);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray.Length > 1)
          stringBuilder.Append("select ").Append(str2).Append(" from ");
        stringBuilder.Append(dialect.Qualify(catalog, schema, strArray[index]));
        if (index < strArray.Length - 1)
          stringBuilder.Append(" union ");
      }
      if (strArray.Length > 1)
      {
        stringBuilder.Insert(0, "( ").Append(" ) ids_");
        str2 = "ids_." + str2;
      }
      this.sql = "select max(" + str2 + ") from " + (object) stringBuilder;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public object Generate(ISessionImplementor session, object obj)
    {
      if (this.sql != null)
        this.GetNext(session);
      return IdentifierGeneratorFactory.CreateNumber(this.next++, this.returnClass);
    }

    private void GetNext(ISessionImplementor session)
    {
      IncrementGenerator.log.Debug((object) ("fetching initial value: " + this.sql));
      try
      {
        IDbConnection connection = session.Factory.ConnectionProvider.GetConnection();
        IDbCommand command = connection.CreateCommand();
        command.CommandText = this.sql;
        command.CommandType = CommandType.Text;
        try
        {
          IDataReader dataReader = command.ExecuteReader();
          try
          {
            this.next = !dataReader.Read() ? 1L : (!dataReader.IsDBNull(0) ? Convert.ToInt64(dataReader.GetValue(0)) + 1L : 1L);
            this.sql = (string) null;
            IncrementGenerator.log.Debug((object) ("first free id: " + (object) this.next));
          }
          finally
          {
            dataReader.Close();
          }
        }
        finally
        {
          session.Factory.ConnectionProvider.CloseConnection(connection);
        }
      }
      catch (DbException ex)
      {
        IncrementGenerator.log.Error((object) "could not get increment value", (Exception) ex);
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not fetch initial value for increment generator");
      }
    }
  }
}
