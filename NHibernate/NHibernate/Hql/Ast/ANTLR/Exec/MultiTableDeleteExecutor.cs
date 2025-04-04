// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Exec.MultiTableDeleteExecutor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Exec
{
  [CLSCompliant(false)]
  public class MultiTableDeleteExecutor : AbstractStatementExecutor
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (MultiTableDeleteExecutor));
    private readonly NHibernate.Persister.Entity.IQueryable persister;
    private readonly SqlString idInsertSelect;
    private readonly SqlString[] deletes;

    public MultiTableDeleteExecutor(IStatement statement)
      : base(statement, MultiTableDeleteExecutor.log)
    {
      if (!this.Factory.Dialect.SupportsTemporaryTables)
        throw new HibernateException("cannot perform multi-table deletes using dialect not supporting temp tables");
      DeleteStatement deleteStatement = (DeleteStatement) statement;
      FromElement fromElement = deleteStatement.FromClause.GetFromElement();
      string tableAlias = fromElement.TableAlias;
      this.persister = fromElement.Queryable;
      this.idInsertSelect = this.GenerateIdInsertSelect(this.persister, tableAlias, deleteStatement.WhereClause);
      MultiTableDeleteExecutor.log.Debug((object) ("Generated ID-INSERT-SELECT SQL (multi-table delete) : " + (object) this.idInsertSelect));
      string[] tableNameClosure = this.persister.ConstraintOrderedTableNameClosure;
      string[][] keyColumnClosure = this.persister.ContraintOrderedTableKeyColumnClosure;
      string idSubselect = this.GenerateIdSubselect(this.persister);
      this.deletes = new SqlString[tableNameClosure.Length];
      for (int index = tableNameClosure.Length - 1; index >= 0; --index)
      {
        SqlDeleteBuilder sqlDeleteBuilder = new SqlDeleteBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(tableNameClosure[index]).SetWhere("(" + StringHelper.Join(", ", (IEnumerable) keyColumnClosure[index]) + ") IN (" + idSubselect + ")");
        if (this.Factory.Settings.IsCommentsEnabled)
          sqlDeleteBuilder.SetComment("bulk delete");
        this.deletes[index] = sqlDeleteBuilder.ToSqlString();
      }
    }

    public override SqlString[] SqlStatements => this.deletes;

    public override int Execute(QueryParameters parameters, ISessionImplementor session)
    {
      this.CoordinateSharedCacheCleanup(session);
      this.CreateTemporaryTableIfNecessary(this.persister, session);
      try
      {
        IDbCommand dbCommand = (IDbCommand) null;
        int num;
        try
        {
          try
          {
            IList<IParameterSpecification> parameters1 = this.Walker.Parameters;
            List<Parameter> list = this.idInsertSelect.GetParameters().ToList<Parameter>();
            SqlType[] queryParameterTypes = parameters1.GetQueryParameterTypes(list, session.Factory);
            dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.idInsertSelect, queryParameterTypes);
            foreach (IParameterSpecification parameterSpecification in (IEnumerable<IParameterSpecification>) parameters1)
              parameterSpecification.Bind(dbCommand, (IList<Parameter>) list, parameters, session);
            num = session.Batcher.ExecuteNonQuery(dbCommand);
          }
          finally
          {
            if (dbCommand != null)
              session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not insert/select ids for bulk delete", this.idInsertSelect);
        }
        for (int index = 0; index < this.deletes.Length; ++index)
        {
          try
          {
            try
            {
              dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.deletes[index], new SqlType[0]);
              session.Batcher.ExecuteNonQuery(dbCommand);
            }
            finally
            {
              if (dbCommand != null)
                session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
            }
          }
          catch (DbException ex)
          {
            throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "error performing bulk delete", this.deletes[index]);
          }
        }
        return num;
      }
      finally
      {
        this.DropTemporaryTableIfNecessary(this.persister, session);
      }
    }

    protected override NHibernate.Persister.Entity.IQueryable[] AffectedQueryables
    {
      get => new NHibernate.Persister.Entity.IQueryable[1]{ this.persister };
    }
  }
}
