// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.SqlNode
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
  public class SqlNode : ASTNode
  {
    private string _originalText;
    private IType _dataType;

    public SqlNode(IToken token)
      : base(token)
    {
      this._originalText = token.Text;
    }

    public override string Text
    {
      get => base.Text;
      set
      {
        base.Text = value;
        if (string.IsNullOrEmpty(value) || this._originalText != null)
          return;
        this._originalText = value;
      }
    }

    public virtual SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      return new SqlString(this.Text);
    }

    public string OriginalText => this._originalText;

    public virtual IType DataType
    {
      get => this._dataType;
      set => this._dataType = value;
    }
  }
}
