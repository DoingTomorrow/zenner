// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.QueryTranslatorImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Exec;
using NHibernate.Hql.Ast.ANTLR.Loader;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class QueryTranslatorImpl : IFilterTranslator, IQueryTranslator
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (QueryTranslatorImpl));
    private readonly string _queryIdentifier;
    private readonly IASTNode _stageOneAst;
    private readonly ISessionFactoryImplementor _factory;
    private bool _shallowQuery;
    private bool _compiled;
    private IDictionary<string, IFilter> _enabledFilters;
    private QueryLoader _queryLoader;
    private IStatementExecutor _statementExecutor;
    private IStatement _sqlAst;
    private IDictionary<string, string> _tokenReplacements;
    private HqlSqlGenerator _generator;

    public QueryTranslatorImpl(
      string queryIdentifier,
      IASTNode parsedQuery,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
    {
      this._queryIdentifier = queryIdentifier;
      this._stageOneAst = parsedQuery;
      this._compiled = false;
      this._shallowQuery = false;
      this._enabledFilters = enabledFilters;
      this._factory = factory;
    }

    public void Compile(IDictionary<string, string> replacements, bool shallow)
    {
      this.DoCompile(replacements, shallow, (string) null);
    }

    public void Compile(
      string collectionRole,
      IDictionary<string, string> replacements,
      bool shallow)
    {
      this.DoCompile(replacements, shallow, collectionRole);
    }

    public IList List(ISessionImplementor session, QueryParameters queryParameters)
    {
      this.ErrorIfDML();
      QueryNode sqlAst = (QueryNode) this._sqlAst;
      bool flag1 = queryParameters.RowSelection != null && queryParameters.RowSelection.DefinesLimits;
      bool flag2 = (sqlAst.GetSelectClause().IsDistinct || flag1) && this.ContainsCollectionFetches;
      QueryParameters queryParameters1;
      if (flag1 && this.ContainsCollectionFetches)
      {
        QueryTranslatorImpl.log.Warn((object) "firstResult/maxResults specified with collection fetch; applying in memory!");
        RowSelection selection = new RowSelection()
        {
          FetchSize = queryParameters.RowSelection.FetchSize,
          Timeout = queryParameters.RowSelection.Timeout
        };
        queryParameters1 = queryParameters.CreateCopyUsing(selection);
      }
      else
        queryParameters1 = queryParameters;
      IList list = this._queryLoader.List(session, queryParameters1);
      if (flag2)
      {
        int num1 = -1;
        int firstRow = !flag1 || queryParameters.RowSelection.FirstRow == RowSelection.NoValue ? 0 : queryParameters.RowSelection.FirstRow;
        int num2 = !flag1 || queryParameters.RowSelection.MaxRows == RowSelection.NoValue ? -1 : queryParameters.RowSelection.MaxRows;
        int count = list.Count;
        System.Collections.Generic.List<object> objectList = new System.Collections.Generic.List<object>();
        IdentitySet identitySet = new IdentitySet();
        for (int index = 0; index < count; ++index)
        {
          object o = list[index];
          if (identitySet.Add(o))
          {
            ++num1;
            if (num1 >= firstRow)
            {
              objectList.Add(o);
              if (num2 >= 0 && num1 - firstRow >= num2 - 1)
                break;
            }
          }
        }
        list = (IList) objectList;
      }
      return list;
    }

    public IEnumerable GetEnumerable(QueryParameters queryParameters, IEventSource session)
    {
      this.ErrorIfDML();
      return this._queryLoader.GetEnumerable(queryParameters, session);
    }

    public int ExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session)
    {
      this.ErrorIfSelect();
      return this._statementExecutor.Execute(queryParameters, session);
    }

    private void ErrorIfSelect()
    {
      if (!this._sqlAst.NeedsExecutor)
        throw new QueryExecutionRequestException("Not supported for select queries:", this._queryIdentifier);
    }

    public NHibernate.Loader.Loader Loader => (NHibernate.Loader.Loader) this._queryLoader;

    public virtual IType[] ActualReturnTypes => this._queryLoader.ReturnTypes;

    public ParameterMetadata BuildParameterMetadata()
    {
      IList<IParameterSpecification> parameters = this._sqlAst.Walker.Parameters;
      return new ParameterMetadata(parameters.OfType<PositionalParameterSpecification>().Select<PositionalParameterSpecification, OrdinalParameterDescriptor>((Func<PositionalParameterSpecification, OrdinalParameterDescriptor>) (op => new OrdinalParameterDescriptor(op.HqlPosition, op.ExpectedType))), (IDictionary<string, NamedParameterDescriptor>) parameters.OfType<NamedParameterSpecification>().Distinct<NamedParameterSpecification>().Select(np => new
      {
        Name = np.Name,
        Descriptor = new NamedParameterDescriptor(np.Name, np.ExpectedType, false)
      }).ToDictionary(ep => ep.Name, ep => ep.Descriptor));
    }

    public string[][] GetColumnNames()
    {
      this.ErrorIfDML();
      return this._sqlAst.Walker.SelectClause.ColumnNames;
    }

    public ISet<string> QuerySpaces => (ISet<string>) this._sqlAst.Walker.QuerySpaces;

    public string SQLString => this._generator.Sql.ToString();

    public IStatement SqlAST => this._sqlAst;

    public IList<IParameterSpecification> CollectedParameterSpecifications
    {
      get => this._generator.CollectionParameters;
    }

    public SqlString SqlString => this._generator.Sql;

    public string QueryIdentifier => this._queryIdentifier;

    public IList<string> CollectSqlStrings
    {
      get
      {
        System.Collections.Generic.List<string> collectSqlStrings = new System.Collections.Generic.List<string>();
        if (this.IsManipulationStatement)
        {
          foreach (SqlString sqlStatement in this._statementExecutor.SqlStatements)
          {
            if (sqlStatement != null)
              collectSqlStrings.Add(sqlStatement.ToString());
          }
        }
        else
          collectSqlStrings.Add(this._generator.Sql.ToString());
        return (IList<string>) collectSqlStrings;
      }
    }

    public string QueryString => this._queryIdentifier;

    public IDictionary<string, IFilter> EnabledFilters => this._enabledFilters;

    public IType[] ReturnTypes
    {
      get
      {
        this.ErrorIfDML();
        return this._sqlAst.Walker.ReturnTypes;
      }
    }

    public string[] ReturnAliases
    {
      get
      {
        this.ErrorIfDML();
        return this._sqlAst.Walker.ReturnAliases;
      }
    }

    public bool ContainsCollectionFetches
    {
      get
      {
        this.ErrorIfDML();
        IList<IASTNode> collectionFetches = ((AbstractRestrictableStatement) this._sqlAst).FromClause.GetCollectionFetches();
        return collectionFetches != null && collectionFetches.Count > 0;
      }
    }

    public bool IsManipulationStatement => this._sqlAst.NeedsExecutor;

    public bool IsShallowQuery => this._shallowQuery;

    private void DoCompile(
      IDictionary<string, string> replacements,
      bool shallow,
      string collectionRole)
    {
      if (this._compiled)
      {
        if (!QueryTranslatorImpl.log.IsDebugEnabled)
          return;
        QueryTranslatorImpl.log.Debug((object) "compile() : The query is already compiled, skipping...");
      }
      else
      {
        this._tokenReplacements = replacements ?? (IDictionary<string, string>) new Dictionary<string, string>(1);
        this._shallowQuery = shallow;
        try
        {
          this._sqlAst = this.Analyze(collectionRole).SqlStatement;
          if (this._sqlAst.NeedsExecutor)
          {
            this._statementExecutor = QueryTranslatorImpl.BuildAppropriateStatementExecutor(this._sqlAst);
          }
          else
          {
            this._generator = new HqlSqlGenerator(this._sqlAst, this._factory);
            this._generator.Generate();
            this._queryLoader = new QueryLoader(this, this._factory, this._sqlAst.Walker.SelectClause);
          }
          this._compiled = true;
        }
        catch (QueryException ex)
        {
          ex.QueryString = this._queryIdentifier;
          throw;
        }
        catch (RecognitionException ex)
        {
          if (QueryTranslatorImpl.log.IsInfoEnabled)
            QueryTranslatorImpl.log.Info((object) "converted antlr.RecognitionException", (Exception) ex);
          throw QuerySyntaxException.Convert(ex, this._queryIdentifier);
        }
        this._enabledFilters = (IDictionary<string, IFilter>) null;
      }
    }

    private static IStatementExecutor BuildAppropriateStatementExecutor(IStatement statement)
    {
      HqlSqlWalker walker = statement.Walker;
      if (walker.StatementType == 13)
      {
        NHibernate.Persister.Entity.IQueryable queryable = walker.GetFinalFromClause().GetFromElement().Queryable;
        return queryable.IsMultiTable ? (IStatementExecutor) new MultiTableDeleteExecutor(statement) : (IStatementExecutor) new BasicExecutor(statement, queryable);
      }
      if (walker.StatementType == 53)
      {
        NHibernate.Persister.Entity.IQueryable queryable = walker.GetFinalFromClause().GetFromElement().Queryable;
        return queryable.IsMultiTable ? (IStatementExecutor) new MultiTableUpdateExecutor(statement) : (IStatementExecutor) new BasicExecutor(statement, queryable);
      }
      if (walker.StatementType == 29)
        return (IStatementExecutor) new BasicExecutor(statement, ((InsertStatement) statement).IntoClause.Queryable);
      throw new QueryException("Unexpected statement type");
    }

    private HqlSqlTranslator Analyze(string collectionRole)
    {
      HqlSqlTranslator hqlSqlTranslator = new HqlSqlTranslator(this._stageOneAst, this, this._factory, this._tokenReplacements, collectionRole);
      hqlSqlTranslator.Translate();
      return hqlSqlTranslator;
    }

    private void ErrorIfDML()
    {
      if (this._sqlAst.NeedsExecutor)
        throw new QueryExecutionRequestException("Not supported for DML operations", this._queryIdentifier);
    }
  }
}
