// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.ImpliedFromElement
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class ImpliedFromElement(IToken token) : FromElement(token)
  {
    private bool _impliedInFromClause;
    private bool _inProjectionList;

    public override bool IsImplied => true;

    public override bool IsImpliedInFromClause => this._impliedInFromClause;

    public override void SetImpliedInFromClause(bool flag) => this._impliedInFromClause = flag;

    public override bool IncludeSubclasses
    {
      get => false;
      set => base.IncludeSubclasses = value;
    }

    public override bool InProjectionList
    {
      get => this._inProjectionList && this.IsFromOrJoinFragment;
      set => this._inProjectionList = value;
    }

    public override string GetDisplayText()
    {
      StringBuilder buf = new StringBuilder();
      buf.Append("ImpliedFromElement{");
      this.AppendDisplayText(buf);
      buf.Append("}");
      return buf.ToString();
    }
  }
}
