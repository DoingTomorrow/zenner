// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.BooleanLiteralNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class BooleanLiteralNode(IToken token) : LiteralNode(token), IExpectedTypeAwareNode
  {
    private IType _expectedType;

    public override IType DataType
    {
      get => this._expectedType ?? (IType) NHibernateUtil.Boolean;
      set => base.DataType = value;
    }

    private BooleanType GetTypeInternal() => (BooleanType) this.DataType;

    private bool GetValue() => this.Type == 51;

    public IType ExpectedType
    {
      get => this._expectedType;
      set => this._expectedType = value;
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      try
      {
        return new SqlString(this.GetTypeInternal().ObjectToSQLString((object) this.GetValue(), sessionFactory.Dialect));
      }
      catch (Exception ex)
      {
        throw new QueryException("Unable to render boolean literal value", ex);
      }
    }
  }
}
