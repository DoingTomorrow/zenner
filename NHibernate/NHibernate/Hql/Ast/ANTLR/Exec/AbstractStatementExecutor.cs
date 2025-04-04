// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Exec.AbstractStatementExecutor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Action;
using NHibernate.AdoNet.Util;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Exec
{
  [CLSCompliant(false)]
  public abstract class AbstractStatementExecutor : IStatementExecutor
  {
    private readonly IInternalLogger log;

    protected AbstractStatementExecutor(IStatement statement, IInternalLogger log)
    {
      this.Statement = statement;
      this.Walker = statement.Walker;
      this.log = log;
    }

    protected HqlSqlWalker Walker { get; private set; }

    protected IStatement Statement { get; private set; }

    public abstract SqlString[] SqlStatements { get; }

    public abstract int Execute(QueryParameters parameters, ISessionImplementor session);

    protected abstract IQueryable[] AffectedQueryables { get; }

    protected ISessionFactoryImplementor Factory => this.Walker.SessionFactoryHelper.Factory;

    protected virtual void CoordinateSharedCacheCleanup(ISessionImplementor session)
    {
      BulkOperationCleanupAction cleanupAction = new BulkOperationCleanupAction(session, this.AffectedQueryables);
      cleanupAction.Init();
      if (!session.IsEventSource)
        return;
      ((IEventSource) session).ActionQueue.AddAction(cleanupAction);
    }

    protected SqlString GenerateIdInsertSelect(
      IQueryable persister,
      string tableAlias,
      IASTNode whereClause)
    {
      SqlSelectBuilder select = new SqlSelectBuilder(this.Factory);
      SelectFragment selectFragment = new SelectFragment(this.Factory.Dialect).AddColumns(tableAlias, persister.IdentifierColumnNames, persister.IdentifierColumnNames);
      select.SetSelectClause(selectFragment.ToFragmentString().Substring(2));
      string tableName = persister.TableName;
      SqlString sqlString1 = persister.FromJoinFragment(tableAlias, true, false);
      SqlString sqlString2 = persister.WhereJoinFragment(tableAlias, true, false);
      select.SetFromClause(tableName + (object) ' ' + tableAlias + (object) sqlString1);
      SqlString sqlString3;
      if (sqlString2 == null)
      {
        sqlString3 = SqlString.Empty;
      }
      else
      {
        sqlString3 = sqlString2.Trim();
        if (sqlString3.StartsWithCaseInsensitive("and "))
          sqlString3 = sqlString3.Substring(4);
      }
      SqlString sqlString4 = SqlString.Empty;
      if (whereClause.ChildCount != 0)
      {
        try
        {
          SqlGenerator sqlGenerator = new SqlGenerator(this.Factory, (ITreeNodeStream) new CommonTreeNodeStream((object) whereClause));
          sqlGenerator.whereClause();
          sqlString4 = sqlGenerator.GetSQL().Substring(7);
        }
        catch (RecognitionException ex)
        {
          throw new HibernateException("Unable to generate id select for DML operation", (Exception) ex);
        }
        if (sqlString3.Length > 0)
          sqlString3.Append(" and ");
      }
      select.SetWhereClause(sqlString3 + sqlString4);
      InsertSelect insertSelect = new InsertSelect();
      if (this.Factory.Settings.IsCommentsEnabled)
        insertSelect.SetComment("insert-select for " + persister.EntityName + " ids");
      insertSelect.SetTableName(persister.TemporaryIdTableName);
      insertSelect.SetSelect(select);
      return insertSelect.ToSqlString();
    }

    protected string GenerateIdSubselect(IQueryable persister)
    {
      return "select " + StringHelper.Join(", ", (IEnumerable) persister.IdentifierColumnNames) + " from " + persister.TemporaryIdTableName;
    }

    protected virtual void CreateTemporaryTableIfNecessary(
      IQueryable persister,
      ISessionImplementor session)
    {
      IIsolatedWork work = (IIsolatedWork) new AbstractStatementExecutor.TmpIdTableCreationIsolatedWork(persister, this.log, session);
      if (this.ShouldIsolateTemporaryTableDDL())
      {
        if (this.Factory.Settings.IsDataDefinitionInTransactionSupported)
          Isolater.DoIsolatedWork(work, session);
        else
          Isolater.DoNonTransactedWork(work, session);
      }
      else
      {
        work.DoWork(session.ConnectionManager.GetConnection(), (IDbTransaction) null);
        session.ConnectionManager.AfterStatement();
      }
    }

    protected virtual bool ShouldIsolateTemporaryTableDDL()
    {
      bool? nullable = this.Factory.Dialect.PerformTemporaryTableDDLInIsolation();
      return nullable.HasValue ? nullable.Value : this.Factory.Settings.IsDataDefinitionImplicitCommit;
    }

    protected virtual void DropTemporaryTableIfNecessary(
      IQueryable persister,
      ISessionImplementor session)
    {
      if (this.Factory.Dialect.DropTemporaryTableAfterUse())
      {
        IIsolatedWork work = (IIsolatedWork) new AbstractStatementExecutor.TmpIdTableDropIsolatedWork(persister, this.log, session);
        if (this.ShouldIsolateTemporaryTableDDL())
        {
          if (this.Factory.Settings.IsDataDefinitionInTransactionSupported)
            Isolater.DoIsolatedWork(work, session);
          else
            Isolater.DoNonTransactedWork(work, session);
        }
        else
        {
          work.DoWork(session.ConnectionManager.GetConnection(), (IDbTransaction) null);
          session.ConnectionManager.AfterStatement();
        }
      }
      else
      {
        IDbCommand cmd = (IDbCommand) null;
        try
        {
          SqlString sql = new SqlString("delete from " + persister.TemporaryIdTableName);
          cmd = session.Batcher.PrepareCommand(CommandType.Text, sql, new SqlType[0]);
          session.Batcher.ExecuteNonQuery(cmd);
        }
        catch (Exception ex)
        {
          this.log.Warn((object) ("unable to cleanup temporary id table after use [" + (object) ex + "]"));
        }
        finally
        {
          if (cmd != null)
          {
            try
            {
              session.Batcher.CloseCommand(cmd, (IDataReader) null);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
    }

    private class TmpIdTableCreationIsolatedWork : IIsolatedWork
    {
      private readonly IQueryable persister;
      private readonly IInternalLogger log;
      private readonly ISessionImplementor session;

      public TmpIdTableCreationIsolatedWork(
        IQueryable persister,
        IInternalLogger log,
        ISessionImplementor session)
      {
        this.persister = persister;
        this.log = log;
        this.session = session;
      }

      public void DoWork(IDbConnection connection, IDbTransaction transaction)
      {
        IDbCommand command = (IDbCommand) null;
        try
        {
          command = this.session.ConnectionManager.CreateCommand();
          command.CommandText = this.persister.TemporaryIdTableDDL;
          command.ExecuteNonQuery();
          this.session.Factory.Settings.SqlStatementLogger.LogCommand(command, FormatStyle.Ddl);
        }
        catch (Exception ex)
        {
          this.log.Debug((object) ("unable to create temporary id table [" + ex.Message + "]"));
        }
        finally
        {
          if (command != null)
          {
            try
            {
              command.Dispose();
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
    }

    private class TmpIdTableDropIsolatedWork : IIsolatedWork
    {
      private readonly IQueryable persister;
      private readonly IInternalLogger log;
      private readonly ISessionImplementor session;

      public TmpIdTableDropIsolatedWork(
        IQueryable persister,
        IInternalLogger log,
        ISessionImplementor session)
      {
        this.persister = persister;
        this.log = log;
        this.session = session;
      }

      public void DoWork(IDbConnection connection, IDbTransaction transaction)
      {
        IDbCommand command = (IDbCommand) null;
        try
        {
          command = this.session.ConnectionManager.CreateCommand();
          command.CommandText = "drop table " + this.persister.TemporaryIdTableName;
          command.ExecuteNonQuery();
          this.session.Factory.Settings.SqlStatementLogger.LogCommand(command, FormatStyle.Ddl);
        }
        catch (Exception ex)
        {
          this.log.Warn((object) ("unable to drop temporary id table after use [" + ex.Message + "]"));
        }
        finally
        {
          if (command != null)
          {
            try
            {
              command.Dispose();
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
    }
  }
}
