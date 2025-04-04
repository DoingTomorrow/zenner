// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.CollectionFunction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class CollectionFunction(IToken token) : MethodNode(token), IDisplayableNode
  {
    public override void Resolve(bool inSelect)
    {
      this.InitializeMethodNode((IASTNode) this, inSelect);
      if (!this.IsCollectionPropertyMethod)
        throw new SemanticException(this.Text + " is not a collection property name!");
      this.ResolveCollectionProperty(this.GetChild(0) ?? throw new SemanticException(this.Text + " requires a path!"));
    }

    protected override void PrepareSelectColumns(string[] selectColumns)
    {
      string str = selectColumns[0].Trim();
      if (str.StartsWith("(") && str.EndsWith(")"))
        str = str.Substring(1, str.Length - 2);
      selectColumns[0] = str;
    }
  }
}
