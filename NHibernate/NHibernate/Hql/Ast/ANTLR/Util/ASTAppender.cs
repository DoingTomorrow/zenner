// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.ASTAppender
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class ASTAppender
  {
    private readonly IASTFactory factory;
    private readonly IASTNode parent;

    public ASTAppender(IASTFactory factory, IASTNode parent)
    {
      this.factory = factory;
      this.parent = parent;
    }

    public IASTNode Append(int type, string text, bool appendIfEmpty)
    {
      return text != null && (appendIfEmpty || text.Length > 0) ? this.parent.AddChild(this.factory.CreateNode(type, text)) : (IASTNode) null;
    }
  }
}
