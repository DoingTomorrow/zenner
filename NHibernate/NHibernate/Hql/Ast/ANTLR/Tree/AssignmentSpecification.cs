// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AssignmentSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class AssignmentSpecification
  {
    private readonly IASTNode _eq;
    private readonly ISessionFactoryImplementor _factory;
    private readonly ISet<string> _tableNames;
    private readonly IParameterSpecification[] _hqlParameters;
    private SqlString _sqlAssignmentString;

    public AssignmentSpecification(IASTNode eq, IQueryable persister)
    {
      this._eq = eq.Type == 102 ? eq : throw new QueryException("assignment in set-clause not associated with equals");
      this._factory = persister.Factory;
      DotNode firstChild;
      try
      {
        firstChild = (DotNode) eq.GetFirstChild();
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException(string.Format("Left side of assigment should be a case sensitive property or a field (depending on mapping); found '{0}'", (object) eq.GetFirstChild()), (Exception) ex);
      }
      SqlNode nextSibling = (SqlNode) firstChild.NextSibling;
      AssignmentSpecification.ValidateLhs((FromReferenceNode) firstChild);
      string propertyPath = firstChild.PropertyPath;
      HashedSet<string> basisSet = new HashedSet<string>();
      if (persister is UnionSubclassEntityPersister)
        basisSet.AddAll((ICollection<string>) persister.ConstraintOrderedTableNameClosure);
      else
        basisSet.Add(persister.GetSubclassTableName(persister.GetSubclassPropertyTableNumber(propertyPath)));
      this._tableNames = (ISet<string>) new ImmutableSet<string>((ISet<string>) basisSet);
      if (nextSibling == null)
        this._hqlParameters = new IParameterSpecification[0];
      else if (AssignmentSpecification.IsParam((IASTNode) nextSibling))
      {
        this._hqlParameters = new IParameterSpecification[1]
        {
          ((ParameterNode) nextSibling).HqlParameterSpecification
        };
      }
      else
      {
        IList<IASTNode> astNodeList = ASTUtil.CollectChildren((IASTNode) nextSibling, new FilterPredicate(AssignmentSpecification.IsParam));
        this._hqlParameters = new IParameterSpecification[astNodeList.Count];
        int num = 0;
        foreach (ParameterNode parameterNode in (IEnumerable<IASTNode>) astNodeList)
          this._hqlParameters[num++] = parameterNode.HqlParameterSpecification;
      }
    }

    public bool AffectsTable(string tableName) => this._tableNames.Contains(tableName);

    private static bool IsParam(IASTNode node) => node.Type == 106 || node.Type == 149;

    private static void ValidateLhs(FromReferenceNode lhs)
    {
      if (!lhs.IsResolved)
        throw new NotSupportedException("cannot validate assignablity of unresolved node");
      if (lhs.DataType.IsCollectionType)
        throw new QueryException("collections not assignable in update statements");
      if (lhs.DataType.IsComponentType)
        throw new QueryException("Components currently not assignable in update statements");
      int num = lhs.DataType.IsEntityType ? 1 : 0;
      if (lhs.GetImpliedJoin() != null || lhs.FromElement.IsImplied)
        throw new QueryException("Implied join paths are not assignable in update statements");
    }

    public IParameterSpecification[] Parameters => this._hqlParameters;

    public SqlString SqlAssignmentFragment
    {
      get
      {
        if (this._sqlAssignmentString == null)
        {
          try
          {
            SqlGenerator sqlGenerator = new SqlGenerator(this._factory, (ITreeNodeStream) new CommonTreeNodeStream((object) this._eq));
            sqlGenerator.comparisonExpr(false);
            this._sqlAssignmentString = sqlGenerator.GetSQL();
          }
          catch (Exception ex)
          {
            throw new QueryException("cannot interpret set-clause assignment", ex);
          }
        }
        return this._sqlAssignmentString;
      }
    }
  }
}
