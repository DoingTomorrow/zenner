// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Lock.UpdateLockingStrategy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Lock
{
  public class UpdateLockingStrategy : ILockingStrategy
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (UpdateLockingStrategy));
    private readonly ILockable lockable;
    private readonly LockMode lockMode;
    private readonly SqlString sql;

    public UpdateLockingStrategy(ILockable lockable, LockMode lockMode)
    {
      this.lockable = lockable;
      this.lockMode = lockMode;
      if (lockMode.LessThan(LockMode.Upgrade))
        throw new HibernateException("[" + (object) lockMode + "] not valid for update statement");
      if (!((IEntityPersister) lockable).IsVersioned)
      {
        UpdateLockingStrategy.log.Warn((object) ("write locks via update not supported for non-versioned entities [" + lockable.EntityName + "]"));
        this.sql = (SqlString) null;
      }
      else
        this.sql = this.GenerateLockString();
    }

    private SqlString GenerateLockString()
    {
      ISessionFactoryImplementor factory = this.lockable.Factory;
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(factory.Dialect, (IMapping) factory);
      sqlUpdateBuilder.SetTableName(this.lockable.RootTableName);
      sqlUpdateBuilder.SetIdentityColumn(this.lockable.RootTableIdentifierColumnNames, this.lockable.IdentifierType);
      sqlUpdateBuilder.SetVersionColumn(new string[1]
      {
        this.lockable.VersionColumnName
      }, this.lockable.VersionType);
      sqlUpdateBuilder.AddColumns(new string[1]
      {
        this.lockable.VersionColumnName
      }, (bool[]) null, (IType) this.lockable.VersionType);
      if (factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment(this.lockMode.ToString() + " lock " + this.lockable.EntityName);
      return sqlUpdateBuilder.ToSqlString();
    }

    public void Lock(object id, object version, object obj, ISessionImplementor session)
    {
      if (!((IEntityPersister) this.lockable).IsVersioned)
        throw new HibernateException("write locks via update not supported for non-versioned entities [" + this.lockable.EntityName + "]");
      ISessionFactoryImplementor factory = session.Factory;
      try
      {
        IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.sql, this.lockable.IdAndVersionSqlTypes);
        try
        {
          this.lockable.VersionType.NullSafeSet(dbCommand, version, 1, session);
          int index1 = 2;
          this.lockable.IdentifierType.NullSafeSet(dbCommand, id, index1, session);
          int index2 = index1 + this.lockable.IdentifierType.GetColumnSpan((IMapping) factory);
          if (((IEntityPersister) this.lockable).IsVersioned)
            this.lockable.VersionType.NullSafeSet(dbCommand, version, index2, session);
          if (session.Batcher.ExecuteNonQuery(dbCommand) < 0)
          {
            factory.StatisticsImplementor.OptimisticFailure(this.lockable.EntityName);
            throw new StaleObjectStateException(this.lockable.EntityName, id);
          }
        }
        finally
        {
          session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
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
