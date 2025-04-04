// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.PolymorphicQuerySourceDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  internal class PolymorphicQuerySourceDetector
  {
    private readonly ISessionFactoryImplementor _sfi;
    private readonly Dictionary<IASTNode, IASTNode[]> _map = new Dictionary<IASTNode, IASTNode[]>();
    private readonly SessionFactoryHelper _sessionFactoryHelper;

    public PolymorphicQuerySourceDetector(ISessionFactoryImplementor sfi)
    {
      this._sfi = sfi;
      this._sessionFactoryHelper = new SessionFactoryHelper(sfi);
    }

    public Dictionary<IASTNode, IASTNode[]> Process(IASTNode tree)
    {
      foreach (IASTNode locateQuerySource in (IEnumerable<IASTNode>) new QuerySourceDetector(tree).LocateQuerySources())
      {
        string className = PolymorphicQuerySourceDetector.GetClassName(locateQuerySource);
        string[] implementors = this._sfi.GetImplementors(className);
        this.AddImplementorsToMap(locateQuerySource, className, implementors);
      }
      return this._map;
    }

    private void AddImplementorsToMap(
      IASTNode querySource,
      string className,
      string[] implementors)
    {
      if (implementors.Length == 1 && implementors[0] == className)
        return;
      this._map.Add(querySource, ((IEnumerable<string>) implementors).Select<string, IASTNode>((Func<string, IASTNode>) (implementor => PolymorphicQuerySourceDetector.MakeIdent(querySource, implementor))).ToArray<IASTNode>());
    }

    private static string GetClassName(IASTNode querySource)
    {
      switch (querySource.Type)
      {
        case 15:
          return PolymorphicQuerySourceDetector.BuildPath(querySource);
        case 125:
          return querySource.Text;
        default:
          throw new NotSupportedException();
      }
    }

    private static IASTNode MakeIdent(IASTNode source, string text)
    {
      IASTNode astNode = source.DupNode();
      astNode.Type = 125;
      astNode.Text = text;
      return astNode;
    }

    private static string BuildPath(IASTNode node)
    {
      StringBuilder sb = new StringBuilder();
      PolymorphicQuerySourceDetector.BuildPath(node, sb);
      return sb.ToString();
    }

    private static void BuildPath(IASTNode node, StringBuilder sb)
    {
      if (node.Type == 15)
      {
        PolymorphicQuerySourceDetector.BuildPath(node.GetChild(0), sb);
        sb.Append('.');
        sb.Append(node.GetChild(1).Text);
      }
      else
        sb.Append(node.Text);
    }
  }
}
