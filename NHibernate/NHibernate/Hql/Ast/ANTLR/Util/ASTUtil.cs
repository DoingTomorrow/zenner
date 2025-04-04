// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.ASTUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public static class ASTUtil
  {
    public static void MakeSiblingOfParent(IASTNode parent, IASTNode child)
    {
      parent.RemoveChild(child);
      parent.AddSibling(child);
    }

    public static string GetPathText(IASTNode n)
    {
      StringBuilder buf = new StringBuilder();
      ASTUtil.GetPathText(buf, n);
      return buf.ToString();
    }

    private static void GetPathText(StringBuilder buf, IASTNode n)
    {
      IASTNode child = n.GetChild(0);
      if (child != null)
        ASTUtil.GetPathText(buf, child);
      buf.Append(n.Text);
      if (child == null || n.ChildCount <= 1)
        return;
      ASTUtil.GetPathText(buf, n.GetChild(1));
    }

    public static string GetDebugstring(IASTNode n)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[ ");
      stringBuilder.Append(n == null ? "{null}" : n.ToStringTree());
      stringBuilder.Append(" ]");
      return stringBuilder.ToString();
    }

    public static bool IsSubtreeChild(IASTNode fixture, IASTNode test)
    {
      for (int index = 0; index < fixture.ChildCount; ++index)
      {
        IASTNode child = fixture.GetChild(index);
        if (child == test || child.ChildCount > 0 && ASTUtil.IsSubtreeChild(child, test))
          return true;
      }
      return false;
    }

    public static IASTNode FindTypeInChildren(IASTNode parent, int type)
    {
      for (int index = 0; index < parent.ChildCount; ++index)
      {
        if (parent.GetChild(index).Type == type)
          return parent.GetChild(index);
      }
      return (IASTNode) null;
    }

    public static IList<IASTNode> CollectChildren(IASTNode root, FilterPredicate predicate)
    {
      return new CollectingNodeVisitor(predicate).Collect(root);
    }
  }
}
