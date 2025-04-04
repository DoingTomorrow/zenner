// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupJoin.GroupJoinAggregateDetectionVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Expressions;
using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupJoin
{
  internal class GroupJoinAggregateDetectionVisitor : NhExpressionTreeVisitor
  {
    private readonly HashSet<GroupJoinClause> _groupJoinClauses;
    private readonly GroupJoinAggregateDetectionVisitor.StackFlag _inAggregate = new GroupJoinAggregateDetectionVisitor.StackFlag();
    private readonly GroupJoinAggregateDetectionVisitor.StackFlag _parentExpressionProcessed = new GroupJoinAggregateDetectionVisitor.StackFlag();
    private readonly List<Expression> _nonAggregatingExpressions = new List<Expression>();
    private readonly List<GroupJoinClause> _nonAggregatingGroupJoins = new List<GroupJoinClause>();
    private readonly List<GroupJoinClause> _aggregatingGroupJoins = new List<GroupJoinClause>();

    private GroupJoinAggregateDetectionVisitor(IEnumerable<GroupJoinClause> groupJoinClause)
    {
      this._groupJoinClauses = new HashSet<GroupJoinClause>(groupJoinClause);
    }

    public static IsAggregatingResults Visit(
      IEnumerable<GroupJoinClause> groupJoinClause,
      Expression selectExpression)
    {
      GroupJoinAggregateDetectionVisitor detectionVisitor = new GroupJoinAggregateDetectionVisitor(groupJoinClause);
      detectionVisitor.VisitExpression(selectExpression);
      return new IsAggregatingResults()
      {
        NonAggregatingClauses = detectionVisitor._nonAggregatingGroupJoins,
        AggregatingClauses = detectionVisitor._aggregatingGroupJoins,
        NonAggregatingExpressions = detectionVisitor._nonAggregatingExpressions
      };
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      this.VisitExpression(expression.QueryModel.SelectClause.Selector);
      return (Expression) expression;
    }

    protected override Expression VisitNhAggregate(NhAggregatedExpression expression)
    {
      using (this._inAggregate.SetFlag())
        return base.VisitNhAggregate(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      if (this._inAggregate.FlagIsFalse && this._parentExpressionProcessed.FlagIsFalse)
        this._nonAggregatingExpressions.Add((Expression) expression);
      using (this._parentExpressionProcessed.SetFlag())
        return base.VisitMemberExpression(expression);
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      FromClauseBase referencedQuerySource = (FromClauseBase) expression.ReferencedQuerySource;
      if (referencedQuerySource.FromExpression is QuerySourceReferenceExpression)
      {
        QuerySourceReferenceExpression fromExpression = (QuerySourceReferenceExpression) referencedQuerySource.FromExpression;
        if (this._groupJoinClauses.Contains(fromExpression.ReferencedQuerySource as GroupJoinClause))
        {
          if (this._inAggregate.FlagIsFalse)
            this._nonAggregatingGroupJoins.Add((GroupJoinClause) fromExpression.ReferencedQuerySource);
          else
            this._aggregatingGroupJoins.Add((GroupJoinClause) fromExpression.ReferencedQuerySource);
        }
      }
      return base.VisitQuerySourceReferenceExpression(expression);
    }

    internal class StackFlag
    {
      public bool FlagIsTrue { get; private set; }

      public bool FlagIsFalse => !this.FlagIsTrue;

      public IDisposable SetFlag()
      {
        return (IDisposable) new GroupJoinAggregateDetectionVisitor.StackFlag.StackFlagDisposable(this);
      }

      internal class StackFlagDisposable : IDisposable
      {
        private readonly GroupJoinAggregateDetectionVisitor.StackFlag _parent;
        private readonly bool _old;

        public StackFlagDisposable(
          GroupJoinAggregateDetectionVisitor.StackFlag parent)
        {
          this._parent = parent;
          this._old = parent.FlagIsTrue;
          parent.FlagIsTrue = true;
        }

        public void Dispose() => this._parent.FlagIsTrue = this._old;
      }
    }
  }
}
