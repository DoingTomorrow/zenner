// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.ColumnHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public class ColumnHelper
  {
    public static void GenerateSingleScalarColumn(IASTFactory factory, IASTNode node, int i)
    {
      node.AddSibling(factory.CreateNode(144, " as " + NameGenerator.ScalarName(i, 0)));
    }

    public static void GenerateScalarColumns(
      IASTFactory factory,
      IASTNode node,
      string[] sqlColumns,
      int i)
    {
      if (sqlColumns.Length == 1)
      {
        ColumnHelper.GenerateSingleScalarColumn(factory, node, i);
      }
      else
      {
        node.Text = sqlColumns[0];
        for (int y = 0; y < sqlColumns.Length; ++y)
        {
          if (y > 0)
            node = node.AddSibling(factory.CreateNode(143, sqlColumns[y]));
          node = node.AddSibling(factory.CreateNode(144, " as " + NameGenerator.ScalarName(i, y)));
        }
      }
    }
  }
}
