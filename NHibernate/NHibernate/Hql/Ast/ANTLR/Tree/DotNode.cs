// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.DotNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class DotNode(IToken token) : FromReferenceNode(token)
  {
    private const int DerefUnknown = 0;
    private const int DerefEntity = 1;
    private const int DerefComponent = 2;
    private const int DerefCollection = 3;
    private const int DerefPrimitive = 4;
    private const int DerefIdentifier = 5;
    private const int DerefJavaConstant = 6;
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (DotNode));
    public static bool UseThetaStyleImplicitJoins;
    public static bool REGRESSION_STYLE_JOIN_SUPPRESSION;
    private string _path;
    private int _dereferenceType;
    private string _propertyName;
    private string _propertyPath;
    private string[] _columns;
    private bool _fetch;
    private FromElement _impliedJoin;
    private JoinType _joinType;

    public JoinType JoinType
    {
      set => this._joinType = value;
    }

    public bool Fetch
    {
      set => this._fetch = value;
    }

    public override FromElement GetImpliedJoin() => this._impliedJoin;

    public override string Path
    {
      get
      {
        if (this._path == null)
        {
          FromReferenceNode lhs = this.GetLhs();
          if (lhs == null)
          {
            this._path = this.Text;
          }
          else
          {
            SqlNode nextSibling = (SqlNode) lhs.NextSibling;
            this._path = lhs.Path + "." + nextSibling.OriginalText;
          }
        }
        return this._path;
      }
    }

    public string PropertyPath
    {
      get => this._propertyPath;
      set => this._propertyPath = value;
    }

    public override void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateScalarColumns(this.Walker.ASTFactory, (IASTNode) this, this.GetColumns(), i);
    }

    public override void ResolveIndex(IASTNode parent)
    {
      if (this.IsResolved)
        return;
      this.DereferenceCollection((CollectionType) this.PrepareLhs(), true, true, (string) null);
    }

    public override void ResolveFirstChild()
    {
      FromReferenceNode firstChild = (FromReferenceNode) this.GetFirstChild();
      string text = ((ASTNode) this.GetChild(1)).Text;
      this._propertyName = text;
      if (this._propertyPath == null)
        this._propertyPath = text;
      firstChild.Resolve(true, true, (string) null, (IASTNode) this);
      this.FromElement = firstChild.FromElement;
      DotNode.CheckSubclassOrSuperclassPropertyReference((AbstractSelectExpression) firstChild, text);
    }

    public override void ResolveInFunctionCall(bool generateJoin, bool implicitJoin)
    {
      if (this.IsResolved)
        return;
      IType type = this.PrepareLhs();
      if (type != null && type.IsCollectionType)
      {
        this.ResolveIndex((IASTNode) null);
      }
      else
      {
        this.ResolveFirstChild();
        this.Resolve(generateJoin, implicitJoin);
      }
    }

    public override void Resolve(
      bool generateJoin,
      bool implicitJoin,
      string classAlias,
      IASTNode parent)
    {
      if (this.IsResolved)
        return;
      IType type = this.PrepareLhs();
      if (type == null)
      {
        if (parent != null)
          return;
        this.Walker.LiteralProcessor.LookupConstant(this);
      }
      else
      {
        if (type.IsComponentType)
        {
          this.CheckLhsIsNotCollection();
          this.DereferenceComponent(parent);
          this.InitText();
        }
        else if (type.IsEntityType)
        {
          this.CheckLhsIsNotCollection();
          this.DereferenceEntity((EntityType) type, implicitJoin, classAlias, generateJoin, parent);
          this.InitText();
        }
        else if (type.IsCollectionType)
        {
          this.CheckLhsIsNotCollection();
          this.DereferenceCollection((CollectionType) type, implicitJoin, false, classAlias);
        }
        else
        {
          if (!CollectionProperties.IsAnyCollectionProperty(this._propertyName))
            this.CheckLhsIsNotCollection();
          this._dereferenceType = 4;
          this.InitText();
        }
        this.IsResolved = true;
      }
    }

    public FromReferenceNode GetLhs()
    {
      return (FromReferenceNode) this.GetChild(0) ?? throw new InvalidOperationException("DOT node with no left-hand-side!");
    }

    private IType GetDataType()
    {
      if (this.DataType == null)
      {
        FromElement fromElement = this.GetLhs().FromElement;
        if (fromElement == null)
          return (IType) null;
        IType propertyType = fromElement.GetPropertyType(this._propertyName, this._propertyPath);
        if (DotNode.Log.IsDebugEnabled)
          DotNode.Log.Debug((object) ("getDataType() : " + this._propertyPath + " -> " + (object) propertyType));
        this.DataType = propertyType;
      }
      return this.DataType;
    }

    public void SetResolvedConstant(string text)
    {
      this._path = text;
      this._dereferenceType = 6;
      this.IsResolved = true;
    }

    private static QueryException BuildIllegalCollectionDereferenceException(
      string propertyName,
      IASTNode lhs)
    {
      return new QueryException("illegal attempt to dereference collection [" + ASTUtil.GetPathText(lhs) + "] with element property reference [" + propertyName + "]");
    }

    private void DereferenceCollection(
      CollectionType collectionType,
      bool implicitJoin,
      bool indexed,
      string classAlias)
    {
      this._dereferenceType = 3;
      string role = collectionType.Role;
      IASTNode nextSibling = this.NextSibling;
      bool flag = nextSibling != null && CollectionProperties.IsAnyCollectionProperty(nextSibling.Text);
      if (flag)
        indexed = true;
      IQueryableCollection queryableCollection = this.SessionFactoryHelper.RequireQueryableCollection(role);
      string path = this.Path;
      FromClause currentFromClause = this.Walker.CurrentFromClause;
      if (this.Walker.StatementType != 45 && indexed && classAlias == null)
        this._columns = this.FromElement.ToColumns(this.GetLhs().FromElement.Queryable.TableName, this._propertyPath, false, true);
      FromElement collection = new FromElementFactory(currentFromClause, this.GetLhs().FromElement, path, classAlias, this.GetColumns(), implicitJoin).CreateCollection(queryableCollection, role, this._joinType, this._fetch, indexed);
      if (DotNode.Log.IsDebugEnabled)
        DotNode.Log.Debug((object) ("dereferenceCollection() : Created new FROM element for " + path + " : " + (object) collection));
      this.SetImpliedJoin(collection);
      this.FromElement = collection;
      if (flag)
      {
        collection.Text = "";
        collection.UseWhereFragment = false;
      }
      if (!implicitJoin)
      {
        IEntityPersister entityPersister = collection.EntityPersister;
        if (entityPersister != null)
          this.Walker.AddQuerySpaces(entityPersister.QuerySpaces);
      }
      this.Walker.AddQuerySpaces(queryableCollection.CollectionSpaces);
    }

    private void DereferenceEntity(
      EntityType entityType,
      bool implicitJoin,
      string classAlias,
      bool generateJoin,
      IASTNode parent)
    {
      this.CheckForCorrelatedSubquery("dereferenceEntity");
      DotNode dotParent = (DotNode) null;
      string propertyName = this._propertyName;
      bool flag;
      if (DotNode.IsDotNode(parent))
      {
        dotParent = (DotNode) parent;
        propertyName = dotParent._propertyName;
        flag = generateJoin && !this.IsReferenceToPrimaryKey(dotParent._propertyName, entityType);
      }
      else
        flag = this.Walker.IsSelectStatement ? (!DotNode.REGRESSION_STYLE_JOIN_SUPPRESSION ? generateJoin || this.Walker.IsInSelect || this.Walker.IsInFrom : generateJoin && (!this.Walker.IsInSelect || !this.Walker.IsShallowQuery)) : this.Walker.CurrentStatementType == 45 && this.Walker.IsInFrom;
      if (flag)
        this.DereferenceEntityJoin(classAlias, entityType, implicitJoin, parent);
      else
        this.DereferenceEntityIdentifier(propertyName, dotParent);
    }

    private void DereferenceEntityIdentifier(string propertyName, DotNode dotParent)
    {
      if (DotNode.Log.IsDebugEnabled)
        DotNode.Log.Debug((object) ("dereferenceShortcut() : property " + propertyName + " in " + this.FromElement.ClassName + " does not require a join."));
      this.InitText();
      this.SetPropertyNameAndPath((IASTNode) dotParent);
      if (dotParent == null)
        return;
      dotParent._dereferenceType = 5;
      dotParent.Text = this.Text;
      dotParent._columns = this.GetColumns();
    }

    private void DereferenceEntityJoin(
      string classAlias,
      EntityType propertyType,
      bool impliedJoin,
      IASTNode parent)
    {
      this._dereferenceType = 1;
      if (DotNode.Log.IsDebugEnabled)
        DotNode.Log.Debug((object) ("dereferenceEntityJoin() : generating join for " + this._propertyName + " in " + this.FromElement.ClassName + " " + (classAlias == null ? "{no alias}" : "(" + classAlias + ")") + " parent = " + ASTUtil.GetDebugstring(parent)));
      string associatedEntityName = propertyType.GetAssociatedEntityName();
      string name = this.AliasGenerator.CreateName(associatedEntityName);
      string[] columns = this.GetColumns();
      string path = this.Path;
      if (impliedJoin && this.Walker.IsInFrom)
        this._joinType = this.Walker.ImpliedJoinType;
      FromClause currentFromClause = this.Walker.CurrentFromClause;
      FromElement fromElement = currentFromClause.FindJoinByPath(path);
      if (fromElement == null || (!fromElement.IsImplied || fromElement.FromClause != currentFromClause) && !this.AreSame(classAlias, fromElement.ClassAlias))
      {
        JoinSequence joinSequence = this.SessionFactoryHelper.CreateJoinSequence(impliedJoin, (IAssociationType) propertyType, name, this._joinType, columns);
        fromElement = new FromElementFactory(currentFromClause, this.GetLhs().FromElement, path, classAlias, columns, impliedJoin).CreateEntityJoin(associatedEntityName, name, joinSequence, this._fetch, this.Walker.IsInFrom, propertyType);
      }
      else
        currentFromClause.AddDuplicateAlias(classAlias, fromElement);
      this.SetImpliedJoin(fromElement);
      this.Walker.AddQuerySpaces(fromElement.EntityPersister.QuerySpaces);
      this.FromElement = fromElement;
    }

    private bool AreSame(string alias1, string alias2)
    {
      return !StringHelper.IsEmpty(alias1) && !StringHelper.IsEmpty(alias2) && alias1.Equals(alias2);
    }

    private void SetImpliedJoin(FromElement elem)
    {
      this._impliedJoin = elem;
      if (this.GetFirstChild().Type != 15)
        return;
      DotNode firstChild = (DotNode) this.GetFirstChild();
      if (firstChild.GetImpliedJoin() == null)
        return;
      this._impliedJoin = firstChild.GetImpliedJoin();
    }

    private bool IsReferenceToPrimaryKey(string propertyName, EntityType owningType)
    {
      IEntityPersister entityPersister = this.SessionFactoryHelper.Factory.GetEntityPersister(owningType.GetAssociatedEntityName());
      if (entityPersister.EntityMetamodel.HasNonIdentifierPropertyNamedId)
        return propertyName == entityPersister.IdentifierPropertyName && owningType.IsReferenceToPrimaryKey;
      if (EntityPersister.EntityID == propertyName)
        return owningType.IsReferenceToPrimaryKey;
      string uniqueKeyPropertyName = this.SessionFactoryHelper.GetIdentifierOrUniqueKeyPropertyName(owningType);
      return uniqueKeyPropertyName != null && uniqueKeyPropertyName == propertyName && owningType.IsReferenceToPrimaryKey;
    }

    private void CheckForCorrelatedSubquery(string methodName)
    {
      if (!this.IsCorrelatedSubselect || !DotNode.Log.IsDebugEnabled)
        return;
      DotNode.Log.Debug((object) (methodName + "() : correlated subquery"));
    }

    private bool IsCorrelatedSubselect
    {
      get => this.Walker.IsSubQuery && this.FromElement.FromClause != this.Walker.CurrentFromClause;
    }

    private void CheckLhsIsNotCollection()
    {
      FromReferenceNode lhs = this.GetLhs();
      if (lhs.DataType != null && lhs.DataType.IsCollectionType)
        throw DotNode.BuildIllegalCollectionDereferenceException(this._propertyName, (IASTNode) lhs);
    }

    private IType PrepareLhs()
    {
      this.GetLhs().PrepareForDot(this._propertyName);
      return this.GetDataType();
    }

    private void DereferenceComponent(IASTNode parent)
    {
      this._dereferenceType = 2;
      this.SetPropertyNameAndPath(parent);
    }

    private void SetPropertyNameAndPath(IASTNode parent)
    {
      if (DotNode.IsDotNode(parent))
      {
        DotNode dotNode = (DotNode) parent;
        this._propertyName = dotNode.GetChild(1).Text;
        this._propertyPath = this._propertyPath + "." + this._propertyName;
        dotNode._propertyPath = this._propertyPath;
        if (!DotNode.Log.IsDebugEnabled)
          return;
        DotNode.Log.Debug((object) ("Unresolved property path is now '" + dotNode._propertyPath + "'"));
      }
      else
      {
        if (!DotNode.Log.IsDebugEnabled)
          return;
        DotNode.Log.Debug((object) ("terminal propertyPath = [" + this._propertyPath + "]"));
      }
    }

    private void InitText()
    {
      string[] columns = this.GetColumns();
      string str = StringHelper.Join(", ", (IEnumerable) columns);
      if (columns.Length > 1 && this.Walker.IsComparativeExpressionClause)
        str = "(" + str + ")";
      this.Text = str;
    }

    private string[] GetColumns()
    {
      if (this._columns == null)
        this._columns = this.FromElement.ToColumns(this.GetLhs().FromElement.TableAlias, this._propertyPath, false);
      return this._columns;
    }

    private static void CheckSubclassOrSuperclassPropertyReference(
      AbstractSelectExpression lhs,
      string propertyName)
    {
      if (lhs == null || lhs is IndexNode)
        return;
      lhs.FromElement?.HandlePropertyBeingDereferenced(lhs.DataType, propertyName);
    }

    private static bool IsDotNode(IASTNode n) => n != null && n.Type == 15;

    public void ResolveSelectExpression()
    {
      if (this.Walker.IsShallowQuery || this.Walker.CurrentFromClause.IsSubQuery)
      {
        this.Resolve(false, true);
      }
      else
      {
        this.Resolve(true, false);
        if (this.GetDataType().IsEntityType)
        {
          FromElement fromElement = this.FromElement;
          fromElement.IncludeSubclasses = true;
          if (DotNode.UseThetaStyleImplicitJoins)
          {
            fromElement.JoinSequence.SetUseThetaStyle(true);
            FromElement origin = fromElement.Origin;
            if (origin != null)
              ASTUtil.MakeSiblingOfParent((IASTNode) origin, (IASTNode) fromElement);
          }
        }
      }
      for (FromReferenceNode lhs = this.GetLhs(); lhs != null; lhs = (FromReferenceNode) lhs.GetChild(0))
        DotNode.CheckSubclassOrSuperclassPropertyReference((AbstractSelectExpression) lhs, lhs.NextSibling.Text);
    }
  }
}
