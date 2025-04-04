// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ParameterNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ParameterNode(IToken token) : 
    HqlSqlWalkerNode(token),
    IDisplayableNode,
    IExpectedTypeAwareNode,
    ISelectExpression
  {
    private string _alias;
    private IParameterSpecification _parameterSpecification;

    public IParameterSpecification HqlParameterSpecification
    {
      get => this._parameterSpecification;
      set => this._parameterSpecification = value;
    }

    public string GetDisplayText()
    {
      return "{" + (this._parameterSpecification == null ? "???" : this._parameterSpecification.RenderDisplayInfo()) + "}";
    }

    public IType ExpectedType
    {
      get
      {
        return this.HqlParameterSpecification != null ? this.HqlParameterSpecification.ExpectedType : (IType) null;
      }
      set
      {
        this.HqlParameterSpecification.ExpectedType = value;
        this.DataType = value;
      }
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      int columnSpan;
      if (this.ExpectedType == null || (columnSpan = this.ExpectedType.GetColumnSpan((IMapping) sessionFactory)) <= 1)
        return new SqlString(Parameter.Placeholder);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      sqlStringBuilder.Add("(");
      sqlStringBuilder.AddParameter();
      for (int index = 1; index < columnSpan; ++index)
      {
        sqlStringBuilder.Add(",");
        sqlStringBuilder.AddParameter();
      }
      sqlStringBuilder.Add(")");
      return sqlStringBuilder.ToSqlString();
    }

    public void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }

    public FromElement FromElement => (FromElement) null;

    public bool IsConstructor => false;

    public bool IsReturnableEntity => false;

    public bool IsScalar => this.DataType != null && !this.DataType.IsAssociationType;

    public string Alias
    {
      get => this._alias;
      set => this._alias = value;
    }
  }
}
