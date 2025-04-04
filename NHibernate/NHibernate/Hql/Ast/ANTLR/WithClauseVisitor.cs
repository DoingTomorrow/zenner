// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.WithClauseVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Param;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class WithClauseVisitor : IVisitationStrategy
  {
    private readonly FromElement _joinFragment;
    private FromElement _referencedFromElement;
    private string _joinAlias;

    public WithClauseVisitor(FromElement fromElement) => this._joinFragment = fromElement;

    public void Visit(IASTNode node)
    {
      switch (node)
      {
        case DotNode _:
          DotNode dotNode = (DotNode) node;
          FromElement fromElement = dotNode.FromElement;
          if (this._referencedFromElement != null)
          {
            if (fromElement == this._referencedFromElement)
              break;
            throw new HibernateException("with-clause referenced two different from-clause elements");
          }
          this._referencedFromElement = fromElement;
          this._joinAlias = WithClauseVisitor.ExtractAppliedAlias((IASTNode) dotNode);
          if (!(this._joinAlias != this._referencedFromElement.TableAlias))
            break;
          throw new InvalidWithClauseException("with clause can only reference columns in the driving table");
        case ParameterNode _:
          this.ApplyParameterSpecification(((ParameterNode) node).HqlParameterSpecification);
          break;
        case IParameterContainer _:
          this.ApplyParameterSpecifications((IParameterContainer) node);
          break;
      }
    }

    private void ApplyParameterSpecifications(IParameterContainer parameterContainer)
    {
      if (!parameterContainer.HasEmbeddedParameters)
        return;
      foreach (IParameterSpecification embeddedParameter in parameterContainer.GetEmbeddedParameters())
        this.ApplyParameterSpecification(embeddedParameter);
    }

    private void ApplyParameterSpecification(IParameterSpecification paramSpec)
    {
      this._joinFragment.AddEmbeddedParameter(paramSpec);
    }

    private static string ExtractAppliedAlias(IASTNode dotNode)
    {
      return dotNode.Text.Substring(0, dotNode.Text.IndexOf('.'));
    }

    public FromElement GetReferencedFromElement() => this._referencedFromElement;

    public string GetJoinAlias() => this._joinAlias;
  }
}
