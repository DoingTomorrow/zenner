// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.BooleanToCaseConvertor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public static class BooleanToCaseConvertor
  {
    public static IEnumerable<HqlExpression> Convert(IEnumerable<HqlExpression> hqlTreeNodes)
    {
      return hqlTreeNodes.Select<HqlExpression, HqlExpression>((Func<HqlExpression, HqlExpression>) (node => BooleanToCaseConvertor.ConvertBooleanToCase(node)));
    }

    public static HqlExpression ConvertBooleanToCase(HqlExpression node)
    {
      if (!(node is HqlBooleanExpression))
        return node;
      HqlTreeBuilder hqlTreeBuilder = new HqlTreeBuilder();
      return (HqlExpression) hqlTreeBuilder.Case(new HqlWhen[1]
      {
        hqlTreeBuilder.When(node, (HqlExpression) hqlTreeBuilder.True())
      }, (HqlExpression) hqlTreeBuilder.False());
    }
  }
}
