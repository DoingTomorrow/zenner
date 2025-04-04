// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Exec.BasicExecutor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Exec
{
  [CLSCompliant(false)]
  public class BasicExecutor : AbstractStatementExecutor
  {
    private readonly NHibernate.Persister.Entity.IQueryable persister;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (BasicExecutor));
    private readonly SqlString sql;

    public BasicExecutor(IStatement statement, NHibernate.Persister.Entity.IQueryable persister)
      : base(statement, BasicExecutor.log)
    {
      this.persister = persister;
      try
      {
        SqlGenerator sqlGenerator = new SqlGenerator(this.Factory, (ITreeNodeStream) new CommonTreeNodeStream((object) statement));
        sqlGenerator.statement();
        this.sql = sqlGenerator.GetSQL();
        sqlGenerator.ParseErrorHandler.ThrowQueryException();
        this.Parameters = sqlGenerator.GetCollectedParameters();
      }
      catch (RecognitionException ex)
      {
        throw QuerySyntaxException.Convert(ex);
      }
    }

    private IList<IParameterSpecification> Parameters { get; set; }

    public override SqlString[] SqlStatements
    {
      get => new SqlString[1]{ this.sql };
    }

    public override int Execute(QueryParameters parameters, ISessionImplementor session)
    {
      this.CoordinateSharedCacheCleanup(session);
      IDbCommand dbCommand = (IDbCommand) null;
      RowSelection rowSelection = parameters.RowSelection;
      try
      {
        try
        {
          this.CheckParametersExpectedType(parameters);
          List<Parameter> list = this.sql.GetParameters().ToList<Parameter>();
          SqlType[] queryParameterTypes = this.Parameters.GetQueryParameterTypes(list, session.Factory);
          dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.sql, queryParameterTypes);
          foreach (IParameterSpecification parameter in (IEnumerable<IParameterSpecification>) this.Parameters)
            parameter.Bind(dbCommand, (IList<Parameter>) list, parameters, session);
          if (rowSelection != null && rowSelection.Timeout != RowSelection.NoValue)
            dbCommand.CommandTimeout = rowSelection.Timeout;
          return session.Batcher.ExecuteNonQuery(dbCommand);
        }
        finally
        {
          if (dbCommand != null)
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, (Exception) ex, "could not execute update query", this.sql);
      }
    }

    private void CheckParametersExpectedType(QueryParameters parameters)
    {
      foreach (IParameterSpecification parameter in (IEnumerable<IParameterSpecification>) this.Parameters)
      {
        if (parameter.ExpectedType == null)
        {
          if (parameter is NamedParameterSpecification parameterSpecification2)
          {
            TypedValue typedValue;
            if (parameters.NamedParameters.TryGetValue(parameterSpecification2.Name, out typedValue))
              parameter.ExpectedType = typedValue.Type;
          }
          else if (parameter is PositionalParameterSpecification parameterSpecification1)
            parameter.ExpectedType = parameters.PositionalParameterTypes[parameterSpecification1.HqlPosition];
        }
      }
    }

    protected override NHibernate.Persister.Entity.IQueryable[] AffectedQueryables
    {
      get => new NHibernate.Persister.Entity.IQueryable[1]{ this.persister };
    }
  }
}
