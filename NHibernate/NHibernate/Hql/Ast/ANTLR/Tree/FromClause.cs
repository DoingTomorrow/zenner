// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.FromClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using Iesi.Collections.Generic;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class FromClause(IToken token) : HqlSqlWalkerNode(token), IDisplayableNode
  {
    private const int RootLevel = 1;
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (FromClause));
    private int _level = 1;
    private int _fromElementCounter;
    private readonly NullableDictionary<string, FromElement> _fromElementByClassAlias = new NullableDictionary<string, FromElement>();
    private readonly Dictionary<string, FromElement> _fromElementByTableAlias = new Dictionary<string, FromElement>();
    private readonly NullableDictionary<string, FromElement> _fromElementsByPath = new NullableDictionary<string, FromElement>();
    private readonly List<FromElement> _fromElements = new List<FromElement>();
    private readonly NullableDictionary<string, FromElement> _collectionJoinFromElementsByPath = new NullableDictionary<string, FromElement>();
    private FromClause _parentFromClause;
    private ISet<FromClause> _childFromClauses;

    public void SetParentFromClause(FromClause parentFromClause)
    {
      this._parentFromClause = parentFromClause;
      if (parentFromClause == null)
        return;
      this._level = parentFromClause.Level + 1;
      parentFromClause.AddChild(this);
    }

    public FromClause ParentFromClause => this._parentFromClause;

    public IList<IASTNode> GetExplicitFromElements()
    {
      return ASTUtil.CollectChildren((IASTNode) this, new FilterPredicate(FromClause.ExplicitFromPredicate));
    }

    public IList<IASTNode> GetCollectionFetches()
    {
      return ASTUtil.CollectChildren((IASTNode) this, new FilterPredicate(FromClause.CollectionFetchPredicate));
    }

    public FromElement FindCollectionJoin(string path)
    {
      return this._collectionJoinFromElementsByPath[path];
    }

    public bool IsFromElementAlias(string possibleAlias)
    {
      bool flag = this.ContainsClassAlias(possibleAlias);
      if (!flag && this._parentFromClause != null)
        flag = this._parentFromClause.IsFromElementAlias(possibleAlias);
      return flag;
    }

    public bool ContainsClassAlias(string alias)
    {
      bool flag = this._fromElementByClassAlias.ContainsKey(alias);
      if (!flag && this.SessionFactoryHelper.IsStrictJPAQLComplianceEnabled)
        flag = this.FindIntendedAliasedFromElementBasedOnCrazyJPARequirements(alias) != null;
      return flag;
    }

    public bool ContainsTableAlias(string alias)
    {
      return this._fromElementByTableAlias.ContainsKey(alias);
    }

    public void AddJoinByPathMap(string path, FromElement destination)
    {
      if (FromClause.Log.IsDebugEnabled)
        FromClause.Log.Debug((object) ("addJoinByPathMap() : " + path + " -> " + (object) destination));
      this._fromElementsByPath.Add(path, destination);
    }

    public void AddCollectionJoinFromElementByPath(string path, FromElement destination)
    {
      if (FromClause.Log.IsDebugEnabled)
        FromClause.Log.Debug((object) ("addCollectionJoinFromElementByPath() : " + path + " -> " + (object) destination));
      this._collectionJoinFromElementsByPath.Add(path, destination);
    }

    private void AddChild(FromClause fromClause)
    {
      if (this._childFromClauses == null)
        this._childFromClauses = (ISet<FromClause>) new HashedSet<FromClause>();
      this._childFromClauses.Add(fromClause);
    }

    public FromElement AddFromElement(string path, IASTNode alias)
    {
      string text = alias == null ? (string) null : alias.Text;
      this.CheckForDuplicateClassAlias(text);
      return new FromElementFactory(this, (FromElement) null, path, text, (string[]) null, false).AddFromElement();
    }

    public FromElement GetFromElement(string aliasOrClassName)
    {
      FromElement fromElement;
      this._fromElementByClassAlias.TryGetValue(aliasOrClassName, out fromElement);
      if (fromElement == null && this.SessionFactoryHelper.IsStrictJPAQLComplianceEnabled)
        fromElement = this.FindIntendedAliasedFromElementBasedOnCrazyJPARequirements(aliasOrClassName);
      if (fromElement == null && this._parentFromClause != null)
        fromElement = this._parentFromClause.GetFromElement(aliasOrClassName);
      return fromElement;
    }

    public IList<IASTNode> GetFromElements()
    {
      return ASTUtil.CollectChildren((IASTNode) this, new FilterPredicate(FromClause.FromElementPredicate));
    }

    public IList<IASTNode> GetProjectionList()
    {
      return ASTUtil.CollectChildren((IASTNode) this, new FilterPredicate(FromClause.ProjectionListPredicate));
    }

    public FromElement GetFromElement() => (FromElement) this.GetFromElements()[0];

    public void AddDuplicateAlias(string alias, FromElement element)
    {
      if (alias == null)
        return;
      this._fromElementByClassAlias.Add(alias, element);
    }

    public FromElement FindJoinByPath(string path)
    {
      FromElement joinByPath = this.FindJoinByPathLocal(path);
      if (joinByPath == null && this._parentFromClause != null)
        joinByPath = this._parentFromClause.FindJoinByPath(path);
      return joinByPath;
    }

    private int Level => this._level;

    public bool IsSubQuery => this._parentFromClause != null;

    public string GetDisplayText()
    {
      return "FromClause{level=" + (object) this._level + ", fromElementCounter=" + (object) this._fromElementCounter + ", fromElements=" + (object) this._fromElements.Count + ", fromElementByClassAlias=" + (object) this._fromElementByClassAlias.Keys + ", fromElementByTableAlias=" + (object) this._fromElementByTableAlias.Keys + ", fromElementsByPath=" + (object) this._fromElementsByPath.Keys + ", collectionJoinFromElementsByPath=" + (object) this._collectionJoinFromElementsByPath.Keys + "}";
    }

    private void CheckForDuplicateClassAlias(string classAlias)
    {
      if (classAlias != null && this._fromElementByClassAlias.ContainsKey(classAlias))
        throw new QueryException("Duplicate definition of alias '" + classAlias + "'");
    }

    private static bool ProjectionListPredicate(IASTNode node)
    {
      return node is FromElement fromElement && fromElement.InProjectionList;
    }

    private static bool FromElementPredicate(IASTNode node)
    {
      return node is FromElement fromElement && fromElement.IsFromOrJoinFragment;
    }

    private static bool ExplicitFromPredicate(IASTNode node)
    {
      return node is FromElement fromElement && !fromElement.IsImplied;
    }

    private static bool CollectionFetchPredicate(IASTNode node)
    {
      return node is FromElement fromElement && fromElement.IsFetch && fromElement.QueryableCollection != null;
    }

    private FromElement FindIntendedAliasedFromElementBasedOnCrazyJPARequirements(
      string specifiedAlias)
    {
      foreach (KeyValuePair<string, FromElement> elementByClassAlia in this._fromElementByClassAlias)
      {
        if (elementByClassAlia.Key.ToLowerInvariant() == specifiedAlias.ToLowerInvariant())
          return elementByClassAlia.Value;
      }
      return (FromElement) null;
    }

    public void RegisterFromElement(FromElement element)
    {
      this._fromElements.Add(element);
      string classAlias = element.ClassAlias;
      if (classAlias != null)
        this._fromElementByClassAlias.Add(classAlias, element);
      string tableAlias = element.TableAlias;
      if (tableAlias == null)
        return;
      this._fromElementByTableAlias[tableAlias] = element;
    }

    private FromElement FindJoinByPathLocal(string path) => this._fromElementsByPath[path];

    public override string ToString() => "FromClause{level=" + (object) this._level + "}";

    public virtual void Resolve()
    {
      IEnumerator<IASTNode> enumerator = new ASTIterator(this.GetFirstChild()).GetEnumerator();
      HashedSet<IASTNode> hashedSet = new HashedSet<IASTNode>();
      while (enumerator.MoveNext())
        hashedSet.Add(enumerator.Current);
      foreach (FromElement fromElement in this._fromElements)
      {
        if (!hashedSet.Contains((IASTNode) fromElement))
          throw new SemanticException("Element not in AST: " + (object) fromElement);
      }
    }
  }
}
