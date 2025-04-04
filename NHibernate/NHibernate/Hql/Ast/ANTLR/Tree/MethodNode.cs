// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.MethodNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Dialect.Function;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class MethodNode(IToken token) : AbstractSelectExpression(token), ISelectExpression
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (MethodNode));
    private string[] _selectColumns;
    private string _methodName;
    private bool _inSelect;
    private FromElement _fromElement;
    private ISQLFunction _function;

    public virtual void Resolve(bool inSelect)
    {
      IASTNode child = this.GetChild(0);
      this.InitializeMethodNode(child, inSelect);
      IASTNode nextSibling = child.NextSibling;
      if (nextSibling != null && nextSibling.ChildCount == 1 && this.IsCollectionPropertyMethod)
        this.CollectionProperty(nextSibling.GetChild(0), child);
      else
        this.DialectFunction(nextSibling);
    }

    public override void SetScalarColumnText(int i)
    {
      if (this._selectColumns == null)
        ColumnHelper.GenerateSingleScalarColumn(this.Walker.ASTFactory, (IASTNode) this, i);
      else
        ColumnHelper.GenerateScalarColumns(this.Walker.ASTFactory, (IASTNode) this, this._selectColumns, i);
    }

    public void InitializeMethodNode(IASTNode name, bool inSelect)
    {
      name.Type = 148;
      this._methodName = name.Text.ToLowerInvariant();
      this._inSelect = inSelect;
    }

    public bool IsCollectionPropertyMethod
    {
      get => CollectionProperties.IsAnyCollectionProperty(this._methodName);
    }

    public ISQLFunction SQLFunction => this._function;

    public override FromElement FromElement
    {
      get => this._fromElement;
      set => base.FromElement = value;
    }

    public override bool IsScalar => true;

    public void ResolveCollectionProperty(IASTNode expr)
    {
      string normalizedPropertyName = CollectionProperties.GetNormalizedPropertyName(this._methodName);
      FromReferenceNode collectionNode = expr is FromReferenceNode ? (FromReferenceNode) expr : throw new SemanticException("Unexpected expression " + (object) expr + " found for collection function " + normalizedPropertyName);
      if ("elements" == normalizedPropertyName)
      {
        this.HandleElements(collectionNode, normalizedPropertyName);
      }
      else
      {
        this._fromElement = collectionNode.FromElement;
        this.DataType = this._fromElement.GetPropertyType(normalizedPropertyName, normalizedPropertyName);
        this._selectColumns = this._fromElement.ToColumns(this._fromElement.TableAlias, normalizedPropertyName, this._inSelect);
      }
      if (collectionNode is DotNode)
        MethodNode.PrepareAnyImplicitJoins((DotNode) collectionNode);
      if (!this._inSelect)
      {
        this._fromElement.Text = "";
        this._fromElement.UseWhereFragment = false;
      }
      this.PrepareSelectColumns(this._selectColumns);
      this.Text = this._selectColumns[0];
      this.Type = 143;
    }

    public string GetDisplayText()
    {
      return "{method=" + this._methodName + ",selectColumns=" + (object) this._selectColumns + ",fromElement=" + this._fromElement.TableAlias + "}";
    }

    protected virtual void PrepareSelectColumns(string[] columns)
    {
    }

    private void CollectionProperty(IASTNode path, IASTNode name)
    {
      SqlNode expr = path != null ? (SqlNode) path : throw new SemanticException("Collection function " + name.Text + " has no path!");
      IType dataType = expr.DataType;
      if (MethodNode.Log.IsDebugEnabled)
        MethodNode.Log.Debug((object) ("collectionProperty() :  name=" + (object) name + " type=" + (object) dataType));
      this.ResolveCollectionProperty((IASTNode) expr);
    }

    private static void PrepareAnyImplicitJoins(DotNode dotNode)
    {
      if (!(dotNode.GetLhs() is DotNode))
        return;
      DotNode lhs = (DotNode) dotNode.GetLhs();
      FromElement fromElement = lhs.FromElement;
      if (fromElement != null && "" == fromElement.Text)
      {
        string str = fromElement.Queryable.TableName + " " + fromElement.TableAlias;
        fromElement.Text = str;
      }
      MethodNode.PrepareAnyImplicitJoins(lhs);
    }

    private void HandleElements(FromReferenceNode collectionNode, string propertyName)
    {
      FromElement fromElement = collectionNode.FromElement;
      IQueryableCollection queryableCollection = fromElement.QueryableCollection;
      string str = collectionNode.Path + "[]." + propertyName;
      MethodNode.Log.Debug((object) ("Creating elements for " + str));
      this._fromElement = fromElement;
      if (!fromElement.IsCollectionOfValuesOrComponents)
        this.Walker.AddQuerySpaces(queryableCollection.ElementPersister.QuerySpaces);
      this.DataType = queryableCollection.ElementType;
      this._selectColumns = fromElement.ToColumns(this._fromElement.TableAlias, propertyName, this._inSelect);
    }

    private void DialectFunction(IASTNode exprList)
    {
      this._function = this.SessionFactoryHelper.FindSQLFunction(this._methodName);
      if (this._function == null)
        return;
      IASTNode first = (IASTNode) null;
      if (exprList != null)
        first = this._methodName == "iif" ? exprList.GetChild(1) : exprList.GetChild(0);
      this.DataType = this.SessionFactoryHelper.FindFunctionReturnType(this._methodName, first);
    }
  }
}
