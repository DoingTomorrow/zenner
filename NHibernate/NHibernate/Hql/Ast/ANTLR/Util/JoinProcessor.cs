// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.JoinProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class JoinProcessor
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (JoinProcessor));
    private readonly HqlSqlWalker _walker;
    private readonly SyntheticAndFactory _syntheticAndFactory;

    public JoinProcessor(HqlSqlWalker walker)
    {
      this._walker = walker;
      this._syntheticAndFactory = new SyntheticAndFactory(walker);
    }

    public static JoinType ToHibernateJoinType(int astJoinType)
    {
      switch (astJoinType)
      {
        case 23:
          return JoinType.FullJoin;
        case 28:
          return JoinType.InnerJoin;
        case 139:
          return JoinType.LeftOuterJoin;
        case 140:
          return JoinType.RightOuterJoin;
        default:
          throw new AssertionFailure("undefined join type " + (object) astJoinType);
      }
    }

    public void ProcessJoins(QueryNode query)
    {
      FromClause fromClause = query.FromClause;
      IList<IASTNode> astNodeList;
      if (DotNode.UseThetaStyleImplicitJoins)
      {
        astNodeList = (IList<IASTNode>) new List<IASTNode>();
        IList<IASTNode> fromElements = fromClause.GetFromElements();
        for (int index = fromElements.Count - 1; index >= 0; --index)
          astNodeList.Add(fromElements[index]);
      }
      else
        astNodeList = fromClause.GetFromElements();
      foreach (FromElement fromElement in (IEnumerable<IASTNode>) astNodeList)
      {
        JoinSequence joinSequence = fromElement.JoinSequence;
        joinSequence.SetSelector((JoinSequence.ISelector) new JoinProcessor.JoinSequenceSelector(this._walker, fromClause, fromElement));
        this.AddJoinNodes(query, joinSequence, fromElement);
      }
    }

    private void AddJoinNodes(QueryNode query, JoinSequence join, FromElement fromElement)
    {
      JoinFragment joinFragment = join.ToJoinFragment(this._walker.EnabledFilters, fromElement.UseFromFragment || fromElement.IsDereferencedBySuperclassOrSubclassProperty, fromElement.WithClauseFragment, fromElement.WithClauseJoinAlias);
      SqlString fromFragmentString = joinFragment.ToFromFragmentString;
      SqlString whereFragmentString = joinFragment.ToWhereFragmentString;
      if (fromElement.Type == 137 && (join.IsThetaStyle || StringHelper.IsNotEmpty(whereFragmentString)))
      {
        fromElement.Type = 135;
        fromElement.JoinSequence.SetUseThetaStyle(true);
      }
      if (fromElement.UseFromFragment)
      {
        SqlString sqlFragment = JoinProcessor.ProcessFromFragment(fromFragmentString, join).Trim();
        if (JoinProcessor.log.IsDebugEnabled)
          JoinProcessor.log.Debug((object) ("Using FROM fragment [" + (object) sqlFragment + "]"));
        JoinProcessor.ProcessDynamicFilterParameters(sqlFragment, (IParameterContainer) fromElement, this._walker);
      }
      this._syntheticAndFactory.AddWhereFragment(joinFragment, whereFragmentString, query, fromElement, this._walker);
    }

    private static SqlString ProcessFromFragment(SqlString frag, JoinSequence join)
    {
      SqlString sqlString = frag.Trim();
      if (sqlString.StartsWithCaseInsensitive(", "))
        sqlString = sqlString.Substring(2);
      return sqlString;
    }

    public static void ProcessDynamicFilterParameters(
      SqlString sqlFragment,
      IParameterContainer container,
      HqlSqlWalker walker)
    {
      if (walker.EnabledFilters.Count == 0 && !JoinProcessor.HasDynamicFilterParam(sqlFragment) && !JoinProcessor.HasCollectionFilterParam(sqlFragment))
        return;
      container.Text = sqlFragment.ToString();
    }

    private static bool HasDynamicFilterParam(SqlString sqlFragment)
    {
      return sqlFragment.IndexOfCaseInsensitive(":") < 0;
    }

    private static bool HasCollectionFilterParam(SqlString sqlFragment)
    {
      return sqlFragment.IndexOfCaseInsensitive("?") < 0;
    }

    private class JoinSequenceSelector : JoinSequence.ISelector
    {
      private FromClause _fromClause;
      private FromElement _fromElement;
      private HqlSqlWalker _walker;

      internal JoinSequenceSelector(
        HqlSqlWalker walker,
        FromClause fromClause,
        FromElement fromElement)
      {
        this._walker = walker;
        this._fromClause = fromClause;
        this._fromElement = fromElement;
      }

      public bool IncludeSubclasses(string alias)
      {
        bool flag = this._fromClause.ContainsTableAlias(alias);
        if (this._fromElement.IsDereferencedBySubclassProperty)
        {
          JoinProcessor.log.Info((object) ("forcing inclusion of extra joins [alias=" + alias + ", containsTableAlias=" + (object) flag + "]"));
          return true;
        }
        bool isShallowQuery = this._walker.IsShallowQuery;
        bool includeSubclasses = this._fromElement.IncludeSubclasses;
        bool isSubQuery = this._fromClause.IsSubQuery;
        return includeSubclasses && flag && !isSubQuery && !isShallowQuery;
      }
    }
  }
}
