// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlSqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class HqlSqlGenerator
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (HqlSqlGenerator));
    private readonly IASTNode _ast;
    private readonly ISessionFactoryImplementor _sfi;
    private SqlString _sql;
    private IList<IParameterSpecification> _parameters;

    public HqlSqlGenerator(IStatement ast, ISessionFactoryImplementor sfi)
    {
      this._ast = (IASTNode) ast;
      this._sfi = sfi;
    }

    public SqlString Sql => this._sql;

    public IList<IParameterSpecification> CollectionParameters => this._parameters;

    public SqlString Generate()
    {
      if (this._sql == null)
      {
        SqlGenerator sqlGenerator = new SqlGenerator(this._sfi, (ITreeNodeStream) new CommonTreeNodeStream((object) this._ast));
        try
        {
          sqlGenerator.statement();
          this._sql = sqlGenerator.GetSQL();
          if (HqlSqlGenerator.log.IsDebugEnabled)
            HqlSqlGenerator.log.Debug((object) ("SQL: " + (object) this._sql));
        }
        finally
        {
          sqlGenerator.ParseErrorHandler.ThrowQueryException();
        }
        this._parameters = sqlGenerator.GetCollectedParameters();
      }
      return this._sql;
    }
  }
}
