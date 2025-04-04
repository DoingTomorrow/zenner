// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.IntoClause
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class IntoClause(IToken token) : HqlSqlWalkerNode(token), IDisplayableNode
  {
    private IQueryable _persister;
    private string _columnSpec = string.Empty;
    private IType[] _types;
    private bool _discriminated;
    private bool _explicitIdInsertion;
    private bool _explicitVersionInsertion;

    public void Initialize(IQueryable persister)
    {
      this._persister = !persister.IsAbstract ? persister : throw new QueryException("cannot insert into abstract class (no table)");
      this.InitializeColumns();
      if (this.Walker.SessionFactoryHelper.HasPhysicalDiscriminatorColumn(persister))
      {
        this._discriminated = true;
        IntoClause intoClause = this;
        intoClause._columnSpec = intoClause._columnSpec + ", " + persister.DiscriminatorColumnName;
      }
      this.ResetText();
    }

    private void ResetText()
    {
      this.Text = "into " + this.TableName + " ( " + this._columnSpec + " )";
    }

    private string TableName => this._persister.GetSubclassTableName(0);

    public IQueryable Queryable => this._persister;

    private string EntityName => this._persister.EntityName;

    public bool IsDiscriminated => this._discriminated;

    public bool IsExplicitIdInsertion => this._explicitIdInsertion;

    public bool IsExplicitVersionInsertion => this._explicitVersionInsertion;

    public void PrependIdColumnSpec()
    {
      this._columnSpec = this._persister.IdentifierColumnNames[0] + ", " + this._columnSpec;
      this.ResetText();
    }

    public void PrependVersionColumnSpec()
    {
      this._columnSpec = this._persister.GetPropertyColumnNames(this._persister.VersionProperty)[0] + ", " + this._columnSpec;
      this.ResetText();
    }

    public void ValidateTypes(SelectClause selectClause)
    {
      IType[] queryReturnTypes = selectClause.QueryReturnTypes;
      if (queryReturnTypes.Length != this._types.Length)
        throw new QueryException("number of select types did not match those for insert");
      for (int index = 0; index < this._types.Length; ++index)
      {
        if (queryReturnTypes[index] == null)
          queryReturnTypes[index] = this._types[index];
        else if (!this.AreCompatible(this._types[index], queryReturnTypes[index]))
          throw new QueryException("insertion type [" + (object) this._types[index] + "] and selection type [" + (object) queryReturnTypes[index] + "] at position " + (object) index + " are not compatible");
      }
    }

    public string GetDisplayText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("IntoClause{");
      stringBuilder.Append("entityName=").Append(this.EntityName);
      stringBuilder.Append(",tableName=").Append(this.TableName);
      stringBuilder.Append(",columns={").Append(this._columnSpec).Append("}");
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    private void InitializeColumns()
    {
      IASTNode firstChild = this.GetFirstChild();
      List<IType> typeList = new List<IType>();
      this.VisitPropertySpecNodes(firstChild.GetFirstChild(), (ICollection<IType>) typeList);
      this._types = typeList.ToArray();
      this._columnSpec = this._columnSpec.Substring(0, this._columnSpec.Length - 2);
    }

    private void VisitPropertySpecNodes(IASTNode propertyNode, ICollection<IType> types)
    {
      if (propertyNode == null)
        return;
      string text = propertyNode.Text;
      if (this.IsSuperclassProperty(text))
        throw new QueryException("INSERT statements cannot refer to superclass/joined properties [" + text + "]");
      if (text == this._persister.IdentifierPropertyName)
        this._explicitIdInsertion = true;
      if (((IEntityPersister) this._persister).IsVersioned && text == this._persister.PropertyNames[this._persister.VersionProperty])
        this._explicitVersionInsertion = true;
      this.RenderColumns(this._persister.ToColumns(text));
      types.Add(this._persister.ToType(text));
      this.VisitPropertySpecNodes(propertyNode.NextSibling, types);
      this.VisitPropertySpecNodes(propertyNode.GetFirstChild(), types);
    }

    private void RenderColumns(string[] columnNames)
    {
      for (int index = 0; index < columnNames.Length; ++index)
      {
        IntoClause intoClause = this;
        intoClause._columnSpec = intoClause._columnSpec + columnNames[index] + ", ";
      }
    }

    private bool IsSuperclassProperty(string propertyName)
    {
      return this._persister.GetSubclassPropertyTableNumber(propertyName) != 0;
    }

    private bool AreCompatible(IType target, IType source)
    {
      if (target.Equals((object) source))
        return true;
      if (!target.ReturnedClass.IsAssignableFrom(source.ReturnedClass))
        return false;
      SqlType[] sqlTypeArray1 = target.SqlTypes((IMapping) this.SessionFactoryHelper.Factory);
      SqlType[] sqlTypeArray2 = source.SqlTypes((IMapping) this.SessionFactoryHelper.Factory);
      if (sqlTypeArray1.Length != sqlTypeArray2.Length)
        return false;
      for (int index = 0; index < sqlTypeArray1.Length; ++index)
      {
        if (!IntoClause.AreSqlTypesCompatible(sqlTypeArray1[index], sqlTypeArray2[index]))
          return false;
      }
      return true;
    }

    private static bool AreSqlTypesCompatible(SqlType target, SqlType source)
    {
      return target.Equals(source);
    }
  }
}
