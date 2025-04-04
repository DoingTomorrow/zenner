// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.AbstractSelectingDelegate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Id.Insert
{
  public abstract class AbstractSelectingDelegate : IInsertGeneratedIdentifierDelegate
  {
    private readonly IPostInsertIdentityPersister persister;

    protected internal AbstractSelectingDelegate(IPostInsertIdentityPersister persister)
    {
      this.persister = persister;
    }

    public abstract IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert();

    public object PerformInsert(
      SqlCommandInfo insertSQL,
      ISessionImplementor session,
      IBinder binder)
    {
      try
      {
        IDbCommand dbCommand = session.Batcher.PrepareCommand(insertSQL.CommandType, insertSQL.Text, insertSQL.ParameterTypes);
        try
        {
          binder.BindValues(dbCommand);
          session.Batcher.ExecuteNonQuery(dbCommand);
        }
        finally
        {
          session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not insert: " + this.persister.GetInfoString(), insertSQL.Text);
      }
      SqlString selectSql = this.SelectSQL;
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, selectSql, this.ParametersTypes);
          try
          {
            this.BindParameters(session, dbCommand, binder.Entity);
            IDataReader dataReader = session.Batcher.ExecuteReader(dbCommand);
            try
            {
              return this.GetResult(session, dataReader, binder.Entity);
            }
            finally
            {
              session.Batcher.CloseReader(dataReader);
            }
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not retrieve generated id after insert: " + this.persister.GetInfoString(), insertSQL.Text);
        }
      }
    }

    protected internal abstract SqlString SelectSQL { get; }

    protected internal abstract object GetResult(
      ISessionImplementor session,
      IDataReader rs,
      object entity);

    protected internal virtual void BindParameters(
      ISessionImplementor session,
      IDbCommand ps,
      object entity)
    {
    }

    protected internal virtual SqlType[] ParametersTypes => SqlTypeFactory.NoTypes;
  }
}
