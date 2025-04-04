// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Exec.MultiTableUpdateExecutor
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
  public class MultiTableUpdateExecutor : AbstractStatementExecutor
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (MultiTableDeleteExecutor));
    private readonly NHibernate.Persister.Entity.IQueryable persister;
    private readonly SqlString idInsertSelect;
    private readonly SqlString[] updates;
    private readonly IParameterSpecification[][] hqlParameters;

    public MultiTableUpdateExecutor(IStatement statement)
      : base(statement, MultiTableUpdateExecutor.log)
    {
      if (!this.Factory.Dialect.SupportsTemporaryTables)
        throw new HibernateException("cannot perform multi-table updates using dialect not supporting temp tables");
      UpdateStatement updateStatement = (UpdateStatement) statement;
      FromElement fromElement = updateStatement.FromClause.GetFromElement();
      string tableAlias = fromElement.TableAlias;
      this.persister = fromElement.Queryable;
      this.idInsertSelect = this.GenerateIdInsertSelect(this.persister, tableAlias, updateStatement.WhereClause);
      MultiTableUpdateExecutor.log.Debug((object) ("Generated ID-INSERT-SELECT SQL (multi-table update) : " + (object) this.idInsertSelect));
      string[] tableNameClosure = this.persister.ConstraintOrderedTableNameClosure;
      string[][] keyColumnClosure = this.persister.ContraintOrderedTableKeyColumnClosure;
      string idSubselect = this.GenerateIdSubselect(this.persister);
      IList<AssignmentSpecification> assignmentSpecifications = this.Walker.AssignmentSpecifications;
      this.updates = new SqlString[tableNameClosure.Length];
      this.hqlParameters = new IParameterSpecification[tableNameClosure.Length][];
      for (int index1 = 0; index1 < tableNameClosure.Length; ++index1)
      {
        bool flag = false;
        List<IParameterSpecification> parameterSpecificationList = new List<IParameterSpecification>();
        SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(tableNameClosure[index1]).SetWhere(string.Format("({0}) IN ({1})", (object) StringHelper.Join(", ", (IEnumerable) keyColumnClosure[index1]), (object) idSubselect));
        if (this.Factory.Settings.IsCommentsEnabled)
          sqlUpdateBuilder.SetComment("bulk update");
        foreach (AssignmentSpecification assignmentSpecification in (IEnumerable<AssignmentSpecification>) assignmentSpecifications)
        {
          if (assignmentSpecification.AffectsTable(tableNameClosure[index1]))
          {
            flag = true;
            sqlUpdateBuilder.AppendAssignmentFragment(assignmentSpecification.SqlAssignmentFragment);
            if (assignmentSpecification.Parameters != null)
            {
              for (int index2 = 0; index2 < assignmentSpecification.Parameters.Length; ++index2)
                parameterSpecificationList.Add(assignmentSpecification.Parameters[index2]);
            }
          }
        }
        if (flag)
        {
          this.updates[index1] = sqlUpdateBuilder.ToSqlString();
          this.hqlParameters[index1] = parameterSpecificationList.ToArray();
        }
      }
    }

    public override SqlString[] SqlStatements => this.updates;

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
            int parametersInSetClause = this.Walker.NumberOfParametersInSetClause;
            IList<IParameterSpecification> parameters1 = this.Walker.Parameters;
            List<IParameterSpecification> range = new List<IParameterSpecification>((IEnumerable<IParameterSpecification>) parameters1).GetRange(parametersInSetClause, parameters1.Count - parametersInSetClause);
            List<Parameter> list = this.idInsertSelect.GetParameters().ToList<Parameter>();
            SqlType[] queryParameterTypes = range.GetQueryParameterTypes(list, session.Factory);
            dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.idInsertSelect, queryParameterTypes);
            foreach (IParameterSpecification parameterSpecification in range)
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
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not insert/select ids for bulk update", this.idInsertSelect);
        }
        for (int index = 0; index < this.updates.Length; ++index)
        {
          if (this.updates[index] != null)
          {
            try
            {
              try
              {
                List<Parameter> list = this.updates[index].GetParameters().ToList<Parameter>();
                IParameterSpecification[] hqlParameter = this.hqlParameters[index];
                SqlType[] queryParameterTypes = ((IEnumerable<IParameterSpecification>) hqlParameter).GetQueryParameterTypes(list, session.Factory);
                dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.updates[index], queryParameterTypes);
                foreach (IParameterSpecification parameterSpecification in hqlParameter)
                  parameterSpecification.Bind(dbCommand, (IList<Parameter>) list, parameters, session);
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
              throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "error performing bulk update", this.updates[index]);
            }
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
