// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ReWriters.MoveOrderByToEndRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;

#nullable disable
namespace NHibernate.Linq.ReWriters
{
  public class MoveOrderByToEndRewriter
  {
    public static void ReWrite(QueryModel queryModel)
    {
      int count = queryModel.BodyClauses.Count;
      for (int index = 0; index < count; ++index)
      {
        if (queryModel.BodyClauses[index] is OrderByClause)
        {
          IBodyClause bodyClause = queryModel.BodyClauses[index];
          queryModel.BodyClauses.RemoveAt(index);
          queryModel.BodyClauses.Add(bodyClause);
          --index;
          --count;
        }
      }
    }
  }
}
