// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.HqlParseEngine
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Util;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  public class HqlParseEngine
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (HqlParseEngine));
    private readonly string _hql;
    private CommonTokenStream _tokens;
    private readonly bool _filter;
    private readonly ISessionFactoryImplementor _sfi;

    public HqlParseEngine(string hql, bool filter, ISessionFactoryImplementor sfi)
    {
      this._hql = hql;
      this._filter = filter;
      this._sfi = sfi;
    }

    public IASTNode Parse()
    {
      this._tokens = new CommonTokenStream((ITokenSource) new HqlLexer((ICharStream) new CaseInsensitiveStringStream(this._hql)));
      HqlParser hqlParser = new HqlParser((ITokenStream) this._tokens)
      {
        TreeAdaptor = (ITreeAdaptor) new ASTTreeAdaptor(),
        Filter = this._filter
      };
      if (HqlParseEngine.log.IsDebugEnabled)
        HqlParseEngine.log.Debug((object) ("parse() - HQL: " + this._hql));
      try
      {
        IASTNode tree = (IASTNode) hqlParser.statement().Tree;
        new NodeTraverser((IVisitationStrategy) new HqlParseEngine.ConstantConverter(this._sfi)).TraverseDepthFirst(tree);
        return tree;
      }
      finally
      {
        hqlParser.ParseErrorHandler.ThrowQueryException();
      }
    }

    private class ConstantConverter : IVisitationStrategy
    {
      private IASTNode _dotRoot;
      private readonly ISessionFactoryImplementor _sfi;

      public ConstantConverter(ISessionFactoryImplementor sfi) => this._sfi = sfi;

      public void Visit(IASTNode node)
      {
        if (this._dotRoot != null)
        {
          if (ASTUtil.IsSubtreeChild(this._dotRoot, node))
            return;
          this._dotRoot = (IASTNode) null;
        }
        if (this._dotRoot != null || node.Type != 15)
          return;
        this._dotRoot = node;
        this.HandleDotStructure(this._dotRoot);
      }

      private void HandleDotStructure(IASTNode dotStructureRoot)
      {
        string pathText = ASTUtil.GetPathText(dotStructureRoot);
        if (ReflectHelper.GetConstantValue(pathText, this._sfi) == null)
          return;
        dotStructureRoot.ClearChildren();
        dotStructureRoot.Type = 100;
        dotStructureRoot.Text = pathText;
      }
    }
  }
}
