// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.SelectClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class SelectClause(IToken token) : SelectExpressionList(token)
  {
    private const string JoinFetchWithoutOwnerExceptionMsg = "Query specified join fetching, but the owner of the fetched association was not present in the select list [{0}]";
    private bool _prepared;
    private bool _scalarSelect;
    private List<FromElement> _collectionFromElements;
    private IType[] _queryReturnTypes;
    private string[][] _columnNames;
    private readonly List<FromElement> _fromElementsForLoad = new List<FromElement>();
    private ConstructorNode _constructorNode;
    private string[] _aliases;
    public static bool VERSION2_SQL;

    public void InitializeDerivedSelectClause(FromClause fromClause)
    {
      if (this._prepared)
        throw new InvalidOperationException("SelectClause was already prepared!");
      IList<IASTNode> projectionList = fromClause.GetProjectionList();
      ASTAppender astAppender = new ASTAppender(this.ASTFactory, (IASTNode) this);
      int count = projectionList.Count;
      List<IType> typeList = new List<IType>(count);
      List<IType> queryReturnTypeList = new List<IType>(count);
      int k = 0;
      foreach (FromElement fromElement in (IEnumerable<IASTNode>) projectionList)
      {
        IType selectType = fromElement.SelectType;
        this.AddCollectionFromElement(fromElement);
        if (selectType != null && !fromElement.IsCollectionOfValuesOrComponents)
        {
          if (!fromElement.IsFetch)
            queryReturnTypeList.Add(selectType);
          this._fromElementsForLoad.Add(fromElement);
          typeList.Add(selectType);
          string text = fromElement.RenderIdentifierSelect(count, k);
          SelectExpressionImpl selectExpressionImpl = (SelectExpressionImpl) astAppender.Append(145, text, false);
          if (selectExpressionImpl != null)
            selectExpressionImpl.FromElement = fromElement;
        }
        ++k;
      }
      ISelectExpression[] selectExpressionArray = this.CollectSelectExpressions();
      if (this.Walker.IsShallowQuery)
        SelectClause.RenderScalarSelects(selectExpressionArray, fromClause);
      else
        this.RenderNonScalarSelects(selectExpressionArray, fromClause);
      this.FinishInitialization(queryReturnTypeList);
    }

    public void InitializeExplicitSelectClause(FromClause fromClause)
    {
      if (this._prepared)
        throw new InvalidOperationException("SelectClause was already prepared!");
      List<IType> queryReturnTypeList = new List<IType>();
      ISelectExpression[] selectExpressionArray = this.CollectSelectExpressions();
      for (int index1 = 0; index1 < selectExpressionArray.Length; ++index1)
      {
        ISelectExpression selectExpression = selectExpressionArray[index1];
        if (selectExpression.IsConstructor)
        {
          this._constructorNode = (ConstructorNode) selectExpression;
          IList<IType> argumentTypeList = this._constructorNode.ConstructorArgumentTypeList;
          queryReturnTypeList.AddRange((IEnumerable<IType>) argumentTypeList);
          this._scalarSelect = true;
          for (int index2 = 1; index2 < this._constructorNode.ChildCount; ++index2)
          {
            if (this._constructorNode.GetChild(index2) is ISelectExpression child && SelectClause.IsReturnableEntity(child))
              this._fromElementsForLoad.Add(child.FromElement);
          }
        }
        else
        {
          IType dataType = selectExpression.DataType;
          if (dataType == null && !(selectExpression is ParameterNode))
            throw new QueryException("No data type for node: " + selectExpression.GetType().Name + " " + new ASTPrinter().ShowAsString((IASTNode) selectExpression, ""));
          if (selectExpression.IsScalar)
            this._scalarSelect = true;
          if (SelectClause.IsReturnableEntity(selectExpression))
            this._fromElementsForLoad.Add(selectExpression.FromElement);
          queryReturnTypeList.Add(dataType);
        }
      }
      this.InitAliases(selectExpressionArray);
      if (!this.Walker.IsShallowQuery)
      {
        IList<IASTNode> projectionList = fromClause.GetProjectionList();
        ASTAppender astAppender = new ASTAppender(this.ASTFactory, (IASTNode) this);
        int count = projectionList.Count;
        int k = 0;
        foreach (FromElement fromElement1 in (IEnumerable<IASTNode>) projectionList)
        {
          if (fromElement1.IsFetch)
          {
            FromElement fromElement2;
            if (fromElement1.RealOrigin == null)
              fromElement2 = fromElement1.Origin != null ? fromElement1.Origin : throw new QueryException("Unable to determine origin of join fetch [" + fromElement1.GetDisplayText() + "]");
            else
              fromElement2 = fromElement1.RealOrigin;
            if (!this._fromElementsForLoad.Contains(fromElement2))
            {
              fromElement1.Parent.RemoveChild((IASTNode) fromElement1);
            }
            else
            {
              IType selectType = fromElement1.SelectType;
              this.AddCollectionFromElement(fromElement1);
              if (selectType != null && !fromElement1.IsCollectionOfValuesOrComponents)
              {
                fromElement1.IncludeSubclasses = true;
                this._fromElementsForLoad.Add(fromElement1);
                string text = fromElement1.RenderIdentifierSelect(count, k);
                SelectExpressionImpl selectExpressionImpl = (SelectExpressionImpl) astAppender.Append(145, text, false);
                if (selectExpressionImpl != null)
                  selectExpressionImpl.FromElement = fromElement1;
              }
            }
          }
          ++k;
        }
        this.RenderNonScalarSelects(this.CollectSelectExpressions(true), fromClause);
      }
      if (this._scalarSelect || this.Walker.IsShallowQuery)
        SelectClause.RenderScalarSelects(selectExpressionArray, fromClause);
      this.FinishInitialization(queryReturnTypeList);
    }

    public IList<FromElement> FromElementsForLoad => (IList<FromElement>) this._fromElementsForLoad;

    public bool IsScalarSelect => this._scalarSelect;

    public bool IsDistinct => this.ChildCount > 0 && this.GetChild(0).Type == 16;

    public string[][] ColumnNames => this._columnNames;

    public ConstructorInfo Constructor
    {
      get
      {
        return this._constructorNode != null ? this._constructorNode.Constructor : (ConstructorInfo) null;
      }
    }

    public bool IsMap => this._constructorNode != null && this._constructorNode.IsMap;

    public bool IsList => this._constructorNode != null && this._constructorNode.IsList;

    public string[] QueryReturnAliases => this._aliases;

    public IList<FromElement> CollectionFromElements
    {
      get => (IList<FromElement>) this._collectionFromElements;
    }

    public IType[] QueryReturnTypes => this._queryReturnTypes;

    protected internal override IASTNode GetFirstSelectExpression()
    {
      foreach (IASTNode selectExpression in (ASTNode) this)
      {
        if (selectExpression.Type != 16 && selectExpression.Type != 4)
          return selectExpression;
      }
      return (IASTNode) null;
    }

    private static bool IsReturnableEntity(ISelectExpression selectExpression)
    {
      FromElement fromElement = selectExpression.FromElement;
      return (fromElement == null || !fromElement.IsFetch && !fromElement.IsCollectionOfValuesOrComponents) && selectExpression.IsReturnableEntity;
    }

    private void InitAliases(ISelectExpression[] selectExpressions)
    {
      if (this._constructorNode == null)
      {
        this._aliases = new string[selectExpressions.Length];
        for (int index = 0; index < selectExpressions.Length; ++index)
        {
          string alias = selectExpressions[index].Alias;
          this._aliases[index] = alias ?? index.ToString();
        }
      }
      else
        this._aliases = this._constructorNode.GetAliases();
    }

    private void RenderNonScalarSelects(
      ISelectExpression[] selectExpressions,
      FromClause currentFromClause)
    {
      ASTAppender appender = new ASTAppender(this.ASTFactory, (IASTNode) this);
      int length = selectExpressions.Length;
      int nonscalarSize = 0;
      for (int index = 0; index < length; ++index)
      {
        if (!selectExpressions[index].IsScalar)
          ++nonscalarSize;
      }
      int j = 0;
      for (int index = 0; index < length; ++index)
      {
        if (!selectExpressions[index].IsScalar)
        {
          ISelectExpression selectExpression = selectExpressions[index];
          FromElement fromElement = selectExpression.FromElement;
          if (fromElement != null)
          {
            this.RenderNonScalarIdentifiers(fromElement, nonscalarSize, j, selectExpression, appender);
            ++j;
          }
        }
      }
      if (currentFromClause.IsSubQuery)
        return;
      int k = 0;
      for (int index = 0; index < length; ++index)
      {
        if (!selectExpressions[index].IsScalar)
        {
          FromElement fromElement = selectExpressions[index].FromElement;
          if (fromElement != null)
          {
            SelectClause.RenderNonScalarProperties(appender, fromElement, nonscalarSize, k);
            ++k;
          }
        }
      }
    }

    private void RenderNonScalarIdentifiers(
      FromElement fromElement,
      int nonscalarSize,
      int j,
      ISelectExpression expr,
      ASTAppender appender)
    {
      string text = fromElement.RenderIdentifierSelect(nonscalarSize, j);
      if (fromElement.FromClause.IsSubQuery)
        return;
      if (!this._scalarSelect && !this.Walker.IsShallowQuery)
        expr.Text = text;
      else
        appender.Append(143, text, false);
    }

    private static void RenderNonScalarProperties(
      ASTAppender appender,
      FromElement fromElement,
      int nonscalarSize,
      int k)
    {
      string text1 = fromElement.RenderPropertySelect(nonscalarSize, k);
      appender.Append(143, text1, false);
      if (fromElement.QueryableCollection != null && fromElement.IsFetch)
      {
        string text2 = fromElement.RenderCollectionSelectFragment(nonscalarSize, k);
        appender.Append(143, text2, false);
      }
      foreach (FromElement fromElement1 in new ASTIterator((IASTNode) fromElement))
      {
        if (fromElement1.IsCollectionOfValuesOrComponents && fromElement1.IsFetch)
        {
          string text3 = fromElement1.RenderValueCollectionSelectFragment(nonscalarSize, nonscalarSize + k);
          appender.Append(143, text3, false);
        }
      }
    }

    private static void RenderScalarSelects(ISelectExpression[] se, FromClause currentFromClause)
    {
      if (currentFromClause.IsSubQuery)
        return;
      for (int i = 0; i < se.Length; ++i)
        se[i].SetScalarColumnText(i);
    }

    private void AddCollectionFromElement(FromElement fromElement)
    {
      if (!fromElement.IsFetch || !fromElement.CollectionJoin && fromElement.QueryableCollection == null)
        return;
      string str;
      if (this._collectionFromElements == null)
      {
        this._collectionFromElements = new List<FromElement>();
        str = SelectClause.VERSION2_SQL ? "__" : "0__";
      }
      else
        str = this._collectionFromElements.Count.ToString() + "__";
      this._collectionFromElements.Add(fromElement);
      fromElement.CollectionSuffix = str;
    }

    private void FinishInitialization(List<IType> queryReturnTypeList)
    {
      this._queryReturnTypes = queryReturnTypeList.ToArray();
      this.InitializeColumnNames();
      this._prepared = true;
    }

    private void InitializeColumnNames()
    {
      this._columnNames = this.SessionFactoryHelper.GenerateColumnNames(this._queryReturnTypes);
    }
  }
}
