// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Util.PathHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Util
{
  [CLSCompliant(false)]
  public static class PathHelper
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PathHelper));

    public static IASTNode ParsePath(string path, IASTFactory factory)
    {
      string[] strArray = StringHelper.Split(".", path);
      IASTNode n = (IASTNode) null;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string text = strArray[index];
        IASTNode node = factory.CreateNode(125, text);
        if (index == 0)
          n = node;
        else
          n = factory.CreateNode(15, ".", n, node);
      }
      if (PathHelper.log.IsDebugEnabled)
        PathHelper.log.Debug((object) ("parsePath() : " + path + " -> " + ASTUtil.GetDebugstring(n)));
      return n;
    }

    public static string GetAlias(string path) => StringHelper.Root(path);
  }
}
