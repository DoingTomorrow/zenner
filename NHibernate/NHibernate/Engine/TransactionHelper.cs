// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.TransactionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Transaction;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Engine
{
  public abstract class TransactionHelper
  {
    public abstract object DoWorkInCurrentTransaction(
      ISessionImplementor session,
      IDbConnection conn,
      IDbTransaction transaction);

    public virtual object DoWorkInNewTransaction(ISessionImplementor session)
    {
      TransactionHelper.Work work = new TransactionHelper.Work(session, this);
      Isolater.DoIsolatedWork((IIsolatedWork) work, session);
      return work.generatedValue;
    }

    public class Work : IIsolatedWork
    {
      private readonly ISessionImplementor session;
      private readonly TransactionHelper owner;
      internal object generatedValue;

      public Work(ISessionImplementor session, TransactionHelper owner)
      {
        this.session = session;
        this.owner = owner;
      }

      public void DoWork(IDbConnection connection, IDbTransaction transaction)
      {
        try
        {
          this.generatedValue = this.owner.DoWorkInCurrentTransaction(this.session, connection, transaction);
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.session.Factory.SQLExceptionConverter, (Exception) ex, "could not get or update next value", (SqlString) null);
        }
      }
    }
  }
}
