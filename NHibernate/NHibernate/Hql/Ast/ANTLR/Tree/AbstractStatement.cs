// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AbstractStatement
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
  public abstract class AbstractStatement(IToken token) : 
    HqlSqlWalkerNode(token),
    IDisplayableNode,
    IStatement
  {
    public abstract bool NeedsExecutor { get; }

    public abstract int StatementType { get; }

    public string GetDisplayText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Walker.QuerySpaces.Count > 0)
      {
        stringBuilder.Append(" querySpaces (");
        bool flag = true;
        foreach (string querySpace in this.Walker.QuerySpaces)
        {
          if (!flag)
            stringBuilder.Append(',');
          stringBuilder.Append(querySpace);
          flag = false;
        }
        stringBuilder.Append(")");
      }
      return stringBuilder.ToString();
    }
  }
}
