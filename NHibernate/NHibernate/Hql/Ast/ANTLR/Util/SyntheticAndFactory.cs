// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.SyntheticAndFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class SyntheticAndFactory
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SyntheticAndFactory));
    private readonly HqlSqlWalker _hqlSqlWalker;
    private IASTNode _filters;
    private IASTNode _thetaJoins;

    public SyntheticAndFactory(HqlSqlWalker hqlSqlWalker) => this._hqlSqlWalker = hqlSqlWalker;

    public void AddWhereFragment(
      JoinFragment joinFragment,
      SqlString whereFragment,
      QueryNode query,
      FromElement fromElement,
      HqlSqlWalker hqlSqlWalker)
    {
      if (whereFragment == null || !fromElement.UseWhereFragment && !joinFragment.HasThetaJoins)
        return;
      whereFragment = whereFragment.Trim();
      if (StringHelper.IsEmpty(whereFragment.ToString()))
        return;
      if (whereFragment.StartsWithCaseInsensitive("and"))
        whereFragment = whereFragment.Substring(4);
      SyntheticAndFactory.log.Debug((object) ("Using unprocessed WHERE-fragment [" + (object) whereFragment + "]"));
      SqlFragment sqlFragment = (SqlFragment) this.Create(143, whereFragment.ToString());
      sqlFragment.SetJoinFragment(joinFragment);
      sqlFragment.FromElement = fromElement;
      if (fromElement.IndexCollectionSelectorParamSpec != null)
      {
        sqlFragment.AddEmbeddedParameter(fromElement.IndexCollectionSelectorParamSpec);
        fromElement.IndexCollectionSelectorParamSpec = (IParameterSpecification) null;
      }
      if (hqlSqlWalker.IsFilter() && whereFragment.ToString().IndexOf("?") >= 0)
      {
        IType keyType = hqlSqlWalker.SessionFactoryHelper.RequireQueryableCollection(hqlSqlWalker.CollectionFilterRole).KeyType;
        CollectionFilterKeyParameterSpecification specification = new CollectionFilterKeyParameterSpecification(hqlSqlWalker.CollectionFilterRole, keyType, 0);
        sqlFragment.AddEmbeddedParameter((IParameterSpecification) specification);
      }
      JoinProcessor.ProcessDynamicFilterParameters(whereFragment, (IParameterContainer) sqlFragment, hqlSqlWalker);
      SyntheticAndFactory.log.Debug((object) ("Using processed WHERE-fragment [" + sqlFragment.Text + "]"));
      if (sqlFragment.FromElement.IsFilter || sqlFragment.HasFilterCondition)
      {
        if (this._filters == null)
        {
          IASTNode whereClause = query.WhereClause;
          this._filters = this.Create(147, "{filter conditions}");
          whereClause.InsertChild(0, this._filters);
        }
        this._filters.AddChild((IASTNode) sqlFragment);
      }
      else
      {
        if (this._thetaJoins == null)
        {
          IASTNode whereClause = query.WhereClause;
          this._thetaJoins = this.Create(146, "{theta joins}");
          if (this._filters == null)
            whereClause.InsertChild(0, this._thetaJoins);
          else
            this._filters.AddSibling(this._thetaJoins);
        }
        this._thetaJoins.AddChild((IASTNode) sqlFragment);
      }
    }

    private IASTNode Create(int tokenType, string text)
    {
      return this._hqlSqlWalker.ASTFactory.CreateNode(tokenType, text);
    }

    public virtual void AddDiscriminatorWhereFragment(
      IRestrictableStatement statement,
      IQueryable persister,
      IDictionary<string, IFilter> enabledFilters,
      string alias)
    {
      string template = persister.FilterFragment(alias, enabledFilters).Trim();
      if (string.Empty.Equals(template))
        return;
      if (template.StartsWith("and"))
        template = template.Substring(4);
      IASTNode newChild1 = this.Create(143, StringHelper.Replace(template, persister.GenerateFilterConditionAlias(alias) + ".", ""));
      if (statement.WhereClause.ChildCount == 0)
      {
        statement.WhereClause.SetFirstChild(newChild1);
      }
      else
      {
        IASTNode newChild2 = this.Create(6, "{and}");
        IASTNode firstChild = statement.WhereClause.GetFirstChild();
        newChild2.SetFirstChild(newChild1);
        newChild2.AddChild(firstChild);
        statement.WhereClause.SetFirstChild(newChild2);
      }
    }
  }
}
