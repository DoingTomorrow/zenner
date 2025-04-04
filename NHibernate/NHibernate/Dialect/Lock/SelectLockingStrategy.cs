// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Lock.SelectLockingStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Lock
{
  public class SelectLockingStrategy : ILockingStrategy
  {
    private readonly ILockable lockable;
    private readonly LockMode lockMode;
    private readonly SqlString sql;

    public SelectLockingStrategy(ILockable lockable, LockMode lockMode)
    {
      this.lockable = lockable;
      this.lockMode = lockMode;
      this.sql = this.GenerateLockString();
    }

    private SqlString GenerateLockString()
    {
      ISessionFactoryImplementor factory = this.lockable.Factory;
      SqlSimpleSelectBuilder simpleSelectBuilder = new SqlSimpleSelectBuilder(factory.Dialect, (IMapping) factory).SetLockMode(this.lockMode).SetTableName(this.lockable.RootTableName).AddColumn(this.lockable.RootTableIdentifierColumnNames[0]).SetIdentityColumn(this.lockable.RootTableIdentifierColumnNames, this.lockable.IdentifierType);
      if (((IEntityPersister) this.lockable).IsVersioned)
        simpleSelectBuilder.SetVersionColumn(new string[1]
        {
          this.lockable.VersionColumnName
        }, this.lockable.VersionType);
      if (factory.Settings.IsCommentsEnabled)
        simpleSelectBuilder.SetComment(this.lockMode.ToString() + " lock " + this.lockable.EntityName);
      return simpleSelectBuilder.ToSqlString();
    }

    public void Lock(object id, object version, object obj, ISessionImplementor session)
    {
      ISessionFactoryImplementor factory = session.Factory;
      try
      {
        IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.sql, this.lockable.IdAndVersionSqlTypes);
        IDataReader reader = (IDataReader) null;
        try
        {
          this.lockable.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
          if (((IEntityPersister) this.lockable).IsVersioned)
            this.lockable.VersionType.NullSafeSet(dbCommand, version, this.lockable.IdentifierType.GetColumnSpan((IMapping) factory), session);
          reader = session.Batcher.ExecuteReader(dbCommand);
          try
          {
            if (!reader.Read())
            {
              if (factory.Statistics.IsStatisticsEnabled)
                factory.StatisticsImplementor.OptimisticFailure(this.lockable.EntityName);
              throw new StaleObjectStateException(this.lockable.EntityName, id);
            }
          }
          finally
          {
            reader.Close();
          }
        }
        finally
        {
          session.Batcher.CloseCommand(dbCommand, reader);
        }
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        AdoExceptionContextInfo exceptionContextInfo = new AdoExceptionContextInfo()
        {
          SqlException = ex,
          Message = "could not lock: " + MessageHelper.InfoString((IEntityPersister) this.lockable, id, factory),
          Sql = this.sql.ToString(),
          EntityName = this.lockable.EntityName,
          EntityId = id
        };
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, exceptionContextInfo);
      }
    }
  }
}
