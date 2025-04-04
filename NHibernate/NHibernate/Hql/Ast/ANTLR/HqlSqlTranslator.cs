// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlSqlTranslator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class HqlSqlTranslator
  {
    private readonly IASTNode _inputAst;
    private readonly QueryTranslatorImpl _qti;
    private readonly ISessionFactoryImplementor _sfi;
    private readonly IDictionary<string, string> _tokenReplacements;
    private readonly string _collectionRole;
    private IStatement _resultAst;

    public HqlSqlTranslator(
      IASTNode ast,
      QueryTranslatorImpl qti,
      ISessionFactoryImplementor sfi,
      IDictionary<string, string> tokenReplacements,
      string collectionRole)
    {
      this._inputAst = ast;
      this._qti = qti;
      this._sfi = sfi;
      this._tokenReplacements = tokenReplacements;
      this._collectionRole = collectionRole;
    }

    public IStatement SqlStatement => this._resultAst;

    public IStatement Translate()
    {
      if (this._resultAst == null)
      {
        HqlSqlWalker walker = new HqlSqlWalker(this._qti, this._sfi, (ITreeNodeStream) new HqlSqlWalkerTreeNodeStream((object) this._inputAst), this._tokenReplacements, this._collectionRole);
        walker.TreeAdaptor = (ITreeAdaptor) new HqlSqlWalkerTreeAdaptor((object) walker);
        try
        {
          this._resultAst = (IStatement) walker.statement().Tree;
        }
        finally
        {
          walker.ParseErrorHandler.ThrowQueryException();
        }
      }
      return this._resultAst;
    }
  }
}
