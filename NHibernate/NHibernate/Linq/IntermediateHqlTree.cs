// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.IntermediateHqlTree
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class IntermediateHqlTree
  {
    private readonly bool _isRoot;
    private readonly List<Action<IQuery, IDictionary<string, Tuple<object, IType>>>> _additionalCriteria = new List<Action<IQuery, IDictionary<string, Tuple<object, IType>>>>();
    private readonly List<LambdaExpression> _listTransformers = new List<LambdaExpression>();
    private readonly List<LambdaExpression> _itemTransformers = new List<LambdaExpression>();
    private readonly List<LambdaExpression> _postExecuteTransformers = new List<LambdaExpression>();
    private bool _hasDistinctRootOperator;
    private HqlExpression _skipCount;
    private HqlExpression _takeCount;
    private HqlHaving _hqlHaving;
    private HqlTreeNode _root;
    private HqlOrderBy _orderBy;

    public HqlTreeNode Root
    {
      get
      {
        this.ExecuteAddHavingClause(this._hqlHaving);
        this.ExecuteAddOrderBy((HqlTreeNode) this._orderBy);
        this.ExecuteAddSkipClause(this._skipCount);
        this.ExecuteAddTakeClause(this._takeCount);
        return this._root;
      }
    }

    public HqlTreeBuilder TreeBuilder { get; private set; }

    public IntermediateHqlTree(bool root)
    {
      this._isRoot = root;
      this.TreeBuilder = new HqlTreeBuilder();
      this._root = (HqlTreeNode) this.TreeBuilder.Query(this.TreeBuilder.SelectFrom(this.TreeBuilder.From()));
    }

    public ExpressionToHqlTranslationResults GetTranslation()
    {
      if (this._isRoot)
        this.DetectOuterExists();
      return new ExpressionToHqlTranslationResults(this.Root, (IList<LambdaExpression>) this._itemTransformers, (IList<LambdaExpression>) this._listTransformers, (IList<LambdaExpression>) this._postExecuteTransformers, this._additionalCriteria);
    }

    public void AddDistinctRootOperator()
    {
      if (this._hasDistinctRootOperator)
        return;
      this._listTransformers.Add((LambdaExpression) (l => new DistinctRootEntityResultTransformer().TransformList(l.ToList<object>())));
      this._hasDistinctRootOperator = true;
    }

    public void AddItemTransformer(LambdaExpression transformer)
    {
      this._itemTransformers.Add(transformer);
    }

    public void AddFromClause(HqlTreeNode from)
    {
      this._root.NodesPreOrder.OfType<HqlFrom>().First<HqlFrom>().AddChild(from);
    }

    public void AddSelectClause(HqlTreeNode select)
    {
      this._root.NodesPreOrder.OfType<HqlSelectFrom>().First<HqlSelectFrom>().AddChild(select);
    }

    public void AddGroupByClause(HqlGroupBy groupBy)
    {
      this._root.As<HqlQuery>().AddChild((HqlTreeNode) groupBy);
    }

    public void AddOrderByClause(HqlExpression orderBy, HqlDirectionStatement direction)
    {
      if (this._orderBy == null)
        this._orderBy = this.TreeBuilder.OrderBy();
      this._orderBy.AddChild((HqlTreeNode) orderBy);
      this._orderBy.AddChild((HqlTreeNode) direction);
    }

    public void AddSkipClause(HqlExpression toSkip) => this._skipCount = toSkip;

    public void AddTakeClause(HqlExpression toTake) => this._takeCount = toTake;

    private void ExecuteAddOrderBy(HqlTreeNode orderBy)
    {
      if (orderBy == null || this._root.NodesPreOrder.Any<HqlTreeNode>((Func<HqlTreeNode, bool>) (x => x == orderBy)))
        return;
      this._root.As<HqlQuery>().AddChild(orderBy);
    }

    private void ExecuteAddTakeClause(HqlExpression toTake)
    {
      if (toTake == null)
        return;
      HqlQuery hqlQuery = this._root.NodesPreOrder.OfType<HqlQuery>().First<HqlQuery>();
      if (hqlQuery.Children.OfType<HqlTake>().FirstOrDefault<HqlTake>() != null)
        return;
      HqlTake child = this.TreeBuilder.Take(toTake);
      hqlQuery.AddChild((HqlTreeNode) child);
    }

    private void ExecuteAddSkipClause(HqlExpression toSkip)
    {
      if (toSkip == null)
        return;
      HqlQuery hqlQuery = this._root.NodesPreOrder.OfType<HqlQuery>().First<HqlQuery>();
      if (hqlQuery.Children.OfType<HqlSkip>().FirstOrDefault<HqlSkip>() != null)
        return;
      HqlSkip child = this.TreeBuilder.Skip(toSkip);
      hqlQuery.AddChild((HqlTreeNode) child);
    }

    private void ExecuteAddHavingClause(HqlHaving hqlHaving)
    {
      if (hqlHaving == null || this._root.NodesPreOrder.OfType<HqlHaving>().Any<HqlHaving>())
        return;
      this._root.As<HqlQuery>().AddChild((HqlTreeNode) hqlHaving);
    }

    public void AddWhereClause(HqlBooleanExpression where)
    {
      HqlWhere hqlWhere = this._root.NodesPreOrder.OfType<HqlWhere>().FirstOrDefault<HqlWhere>();
      if (hqlWhere == null)
      {
        HqlWhere child = this.TreeBuilder.Where((HqlExpression) where);
        this._root.As<HqlQuery>().AddChild((HqlTreeNode) child);
      }
      else
      {
        HqlBooleanExpression lhs = (HqlBooleanExpression) hqlWhere.Children.Single<HqlTreeNode>();
        hqlWhere.ClearChildren();
        hqlWhere.AddChild((HqlTreeNode) this.TreeBuilder.BooleanAnd(lhs, where));
      }
    }

    public void AddHavingClause(HqlBooleanExpression where)
    {
      if (this._hqlHaving == null)
      {
        this._hqlHaving = this.TreeBuilder.Having((HqlExpression) where);
      }
      else
      {
        HqlBooleanExpression lhs = (HqlBooleanExpression) this._hqlHaving.Children.Single<HqlTreeNode>();
        this._hqlHaving.ClearChildren();
        this._hqlHaving.AddChild((HqlTreeNode) this.TreeBuilder.BooleanAnd(lhs, where));
      }
    }

    private void DetectOuterExists()
    {
      if (!(this._root is HqlExists))
        return;
      this._takeCount = (HqlExpression) this.TreeBuilder.Constant((object) 1);
      this._root = this.Root.Children.First<HqlTreeNode>();
      this._listTransformers.Add((LambdaExpression) (l => l.Any<object>()));
    }

    public void AddAdditionalCriteria(
      Action<IQuery, IDictionary<string, Tuple<object, IType>>> criteria)
    {
      this._additionalCriteria.Add(criteria);
    }

    public void AddPostExecuteTransformer(LambdaExpression lambda)
    {
      this._postExecuteTransformers.Add(lambda);
    }

    public void AddListTransformer(LambdaExpression lambda) => this._listTransformers.Add(lambda);

    public void SetRoot(HqlTreeNode newRoot) => this._root = newRoot;
  }
}
