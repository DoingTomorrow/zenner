// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IdentNode
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
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class IdentNode(IToken token) : FromReferenceNode(token), ISelectExpression
  {
    private const int Unknown = 0;
    private const int PropertyRef = 1;
    private const int ComponentRef = 2;
    private bool _nakedPropertyRef;

    public override IType DataType
    {
      get
      {
        IType dataType = base.DataType;
        if (dataType != null)
          return dataType;
        FromElement fromElement = this.FromElement;
        if (fromElement != null)
          return fromElement.DataType;
        return this.Walker.SessionFactoryHelper.FindSQLFunction(this.Text)?.ReturnType((IType) null, (IMapping) null);
      }
      set => base.DataType = value;
    }

    public override void SetScalarColumnText(int i)
    {
      if (this._nakedPropertyRef)
      {
        ColumnHelper.GenerateSingleScalarColumn(this.Walker.ASTFactory, (IASTNode) this, i);
      }
      else
      {
        FromElement fromElement = this.FromElement;
        if (fromElement != null)
          this.Text = fromElement.RenderScalarIdentifierSelect(i);
        else
          ColumnHelper.GenerateSingleScalarColumn(this.Walker.ASTFactory, (IASTNode) this, i);
      }
    }

    public override void ResolveIndex(IASTNode parent)
    {
      if (!this.IsResolved || !this._nakedPropertyRef)
        throw new InvalidOperationException();
      string originalText = this.OriginalText;
      string role = this.DataType.IsCollectionType ? ((CollectionType) this.DataType).Role : throw new SemanticException("Collection expected; [" + originalText + "] does not refer to a collection property");
      IQueryableCollection queryableCollection = this.SessionFactoryHelper.RequireQueryableCollection(role);
      string tableAlias = this.FromElement.TableAlias;
      this.FromElement = new FromElementFactory(this.Walker.CurrentFromClause, this.FromElement, originalText, (string) null, this.FromElement.ToColumns(tableAlias, originalText, false), true).CreateCollection(queryableCollection, role, JoinType.InnerJoin, false, true);
      this.Walker.AddQuerySpaces(queryableCollection.CollectionSpaces);
    }

    public override void Resolve(
      bool generateJoin,
      bool implicitJoin,
      string classAlias,
      IASTNode parent)
    {
      if (this.IsResolved)
        return;
      if (this.Walker.CurrentFromClause.IsFromElementAlias(this.Text))
      {
        if (this.ResolveAsAlias())
          this.IsResolved = true;
      }
      else if (parent != null && parent.Type == 15)
      {
        DotNode parent1 = (DotNode) parent;
        if (parent.GetFirstChild() == this)
        {
          if (this.ResolveAsNakedComponentPropertyRefLhs(parent1))
            this.IsResolved = true;
        }
        else if (this.ResolveAsNakedComponentPropertyRefRhs(parent1))
          this.IsResolved = true;
      }
      else
      {
        switch (this.ResolveAsNakedPropertyRef())
        {
          case 1:
            this.IsResolved = true;
            break;
          case 2:
            return;
        }
      }
      if (this.IsResolved)
        return;
      try
      {
        this.Walker.LiteralProcessor.ProcessConstant((SqlNode) this, false);
      }
      catch (Exception ex)
      {
      }
    }

    private int ResolveAsNakedPropertyRef()
    {
      FromElement fromElement = this.LocateSingleFromElement();
      if (fromElement == null)
        return 0;
      IQueryable queryable = fromElement.Queryable;
      if (queryable == null)
        return 0;
      IType nakedPropertyType = this.GetNakedPropertyType(fromElement);
      if (nakedPropertyType == null)
        return 0;
      if (nakedPropertyType.IsComponentType || nakedPropertyType.IsAssociationType)
        return 2;
      this.FromElement = fromElement;
      string text = this.Text;
      string[] objects = this.Walker.IsSelectStatement ? queryable.ToColumns(fromElement.TableAlias, text) : queryable.ToColumns(text);
      string str = StringHelper.Join(", ", (IEnumerable) objects);
      this.Text = objects.Length == 1 ? str : "(" + str + ")";
      this.Type = 143;
      this.DataType = nakedPropertyType;
      this._nakedPropertyRef = true;
      return 1;
    }

    private bool ResolveAsNakedComponentPropertyRefLhs(DotNode parent)
    {
      FromElement fromElement = this.LocateSingleFromElement();
      if (fromElement == null)
        return false;
      IType nakedPropertyType = this.GetNakedPropertyType(fromElement);
      if (nakedPropertyType == null)
        throw new QueryException("Unable to resolve path [" + parent.Path + "], unexpected token [" + this.OriginalText + "]");
      if (!nakedPropertyType.IsComponentType)
        throw new QueryException("Property '" + this.OriginalText + "' is not a component.  Use an alias to reference associations or collections.");
      string propertyPath = this.Text + "." + this.NextSibling.Text;
      IType propertyType;
      try
      {
        propertyType = fromElement.GetPropertyType(this.Text, propertyPath);
      }
      catch (Exception ex)
      {
        return false;
      }
      this.FromElement = fromElement;
      parent.PropertyPath = propertyPath;
      parent.DataType = propertyType;
      return true;
    }

    private bool ResolveAsNakedComponentPropertyRefRhs(DotNode parent)
    {
      FromElement fromElement = this.LocateSingleFromElement();
      if (fromElement == null)
        return false;
      string propertyPath = parent.GetLhs().Text + "." + this.Text;
      IType propertyType;
      try
      {
        propertyType = fromElement.GetPropertyType(this.Text, propertyPath);
      }
      catch (Exception ex)
      {
        return false;
      }
      this.FromElement = fromElement;
      this.DataType = propertyType;
      this._nakedPropertyRef = true;
      return true;
    }

    private IType GetNakedPropertyType(FromElement fromElement)
    {
      if (fromElement == null)
        return (IType) null;
      string originalText = this.OriginalText;
      IType nakedPropertyType = (IType) null;
      try
      {
        nakedPropertyType = fromElement.GetPropertyType(originalText, originalText);
      }
      catch (Exception ex)
      {
      }
      return nakedPropertyType;
    }

    private FromElement LocateSingleFromElement()
    {
      IList<IASTNode> fromElements = this.Walker.CurrentFromClause.GetFromElements();
      if (fromElements == null || fromElements.Count != 1)
        return (FromElement) null;
      FromElement fromElement = (FromElement) fromElements[0];
      return fromElement.ClassAlias != null ? (FromElement) null : fromElement;
    }

    private bool ResolveAsAlias()
    {
      FromElement fromElement = this.Walker.CurrentFromClause.GetFromElement(this.Text);
      if (fromElement == null)
        return false;
      this.FromElement = fromElement;
      this.Text = fromElement.GetIdentityColumn();
      this.Type = 141;
      return true;
    }
  }
}
