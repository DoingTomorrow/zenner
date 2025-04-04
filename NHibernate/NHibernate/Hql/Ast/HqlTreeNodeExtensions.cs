// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlTreeNodeExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Hql.Ast
{
  public static class HqlTreeNodeExtensions
  {
    public static HqlExpression AsExpression(this HqlTreeNode node) => (HqlExpression) node;

    public static HqlBooleanExpression AsBooleanExpression(this HqlTreeNode node)
    {
      return node is HqlDot ? (HqlBooleanExpression) new HqlBooleanDot(node.Factory, (HqlDot) node) : (HqlBooleanExpression) node;
    }
  }
}
