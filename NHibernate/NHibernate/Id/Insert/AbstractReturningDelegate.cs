// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.Insert.AbstractReturningDelegate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Id.Insert
{
  public abstract class AbstractReturningDelegate : IInsertGeneratedIdentifierDelegate
  {
    private readonly IPostInsertIdentityPersister persister;

    protected AbstractReturningDelegate(IPostInsertIdentityPersister persister)
    {
      this.persister = persister;
    }

    protected IPostInsertIdentityPersister Persister => this.persister;

    public abstract IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert();

    public object PerformInsert(
      SqlCommandInfo insertSQL,
      ISessionImplementor session,
      IBinder binder)
    {
      try
      {
        IDbCommand dbCommand = this.Prepare(insertSQL, session);
        try
        {
          binder.BindValues(dbCommand);
          return this.ExecuteAndExtract(dbCommand, session);
        }
        finally
        {
          this.ReleaseStatement(dbCommand, session);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not insert: " + this.persister.GetInfoString(), insertSQL.Text);
      }
    }

    protected internal virtual void ReleaseStatement(IDbCommand insert, ISessionImplementor session)
    {
      session.Batcher.CloseCommand(insert, (IDataReader) null);
    }

    protected internal abstract IDbCommand Prepare(
      SqlCommandInfo insertSQL,
      ISessionImplementor session);

    public abstract object ExecuteAndExtract(IDbCommand insert, ISessionImplementor session);
  }
}
