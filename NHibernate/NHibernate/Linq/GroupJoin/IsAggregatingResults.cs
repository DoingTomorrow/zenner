// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.GroupJoin.IsAggregatingResults
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.GroupJoin
{
  public class IsAggregatingResults
  {
    public List<GroupJoinClause> NonAggregatingClauses { get; set; }

    public List<GroupJoinClause> AggregatingClauses { get; set; }

    public List<Expression> NonAggregatingExpressions { get; set; }
  }
}
