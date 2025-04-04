// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.CrossJoinDictionaryArrays
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  public static class CrossJoinDictionaryArrays
  {
    public static IList<Dictionary<IASTNode, IASTNode>> PerformCrossJoin(
      IEnumerable<KeyValuePair<IASTNode, IASTNode[]>> input)
    {
      return (IList<Dictionary<IASTNode, IASTNode>>) CrossJoinDictionaryArrays.CrossJoinKeyValuePairList(input).Select<IEnumerable<KeyValuePair<IASTNode, IASTNode>>, Dictionary<IASTNode, IASTNode>>((Func<IEnumerable<KeyValuePair<IASTNode, IASTNode>>, Dictionary<IASTNode, IASTNode>>) (list => list.ToDictionary<KeyValuePair<IASTNode, IASTNode>, IASTNode, IASTNode>((Func<KeyValuePair<IASTNode, IASTNode>, IASTNode>) (kvp => kvp.Key), (Func<KeyValuePair<IASTNode, IASTNode>, IASTNode>) (kvp => kvp.Value)))).ToList<Dictionary<IASTNode, IASTNode>>();
    }

    private static IEnumerable<IEnumerable<KeyValuePair<IASTNode, IASTNode>>> CrossJoinKeyValuePairList(
      IEnumerable<KeyValuePair<IASTNode, IASTNode[]>> input)
    {
      return input.Count<KeyValuePair<IASTNode, IASTNode[]>>() == 1 ? CrossJoinDictionaryArrays.ExpandKeyValuePair(input.First<KeyValuePair<IASTNode, IASTNode[]>>()) : CrossJoinDictionaryArrays.ExpandKeyValuePair(input.First<KeyValuePair<IASTNode, IASTNode[]>>()).SelectMany<IEnumerable<KeyValuePair<IASTNode, IASTNode>>, IEnumerable<KeyValuePair<IASTNode, IASTNode>>, IEnumerable<KeyValuePair<IASTNode, IASTNode>>>((Func<IEnumerable<KeyValuePair<IASTNode, IASTNode>>, IEnumerable<IEnumerable<KeyValuePair<IASTNode, IASTNode>>>>) (headEntry => CrossJoinDictionaryArrays.CrossJoinKeyValuePairList(input.Skip<KeyValuePair<IASTNode, IASTNode[]>>(1))), (Func<IEnumerable<KeyValuePair<IASTNode, IASTNode>>, IEnumerable<KeyValuePair<IASTNode, IASTNode>>, IEnumerable<KeyValuePair<IASTNode, IASTNode>>>) ((headEntry, tailEntry) => headEntry.Union<KeyValuePair<IASTNode, IASTNode>>(tailEntry)));
    }

    private static IEnumerable<IEnumerable<KeyValuePair<IASTNode, IASTNode>>> ExpandKeyValuePair(
      KeyValuePair<IASTNode, IASTNode[]> input)
    {
      return ((IEnumerable<IASTNode>) input.Value).Select<IASTNode, IEnumerable<KeyValuePair<IASTNode, IASTNode>>>((Func<IASTNode, IEnumerable<KeyValuePair<IASTNode, IASTNode>>>) (i => new List<KeyValuePair<IASTNode, IASTNode>>()
      {
        new KeyValuePair<IASTNode, IASTNode>(input.Key, i)
      }.AsEnumerable<KeyValuePair<IASTNode, IASTNode>>()));
    }
  }
}
