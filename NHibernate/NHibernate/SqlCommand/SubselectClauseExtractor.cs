// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SubselectClauseExtractor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SubselectClauseExtractor
  {
    private const string FromClauseToken = " from ";
    private const string OrderByToken = "order by";
    private int lastOrderByIndex;
    private int lastOrderByPartIndex;
    private int parenNestCount;
    private SqlString sql;
    private SqlStringBuilder builder;

    public SubselectClauseExtractor(SqlString sql)
    {
      this.builder = new SqlStringBuilder(sql.Count);
      this.sql = sql;
      this.lastOrderByIndex = -1;
      this.lastOrderByPartIndex = -1;
    }

    private bool ProcessPartBeforeFrom(object part)
    {
      if (!(part is string part1))
        return false;
      int fromClauseInPart = this.FindFromClauseInPart(part1);
      if (fromClauseInPart < 0)
        return false;
      this.AddPart((object) part1.Substring(fromClauseInPart));
      return true;
    }

    public SqlString GetSqlString()
    {
      IEnumerator enumerator = (IEnumerator) this.sql.GetEnumerator();
      this.parenNestCount = 0;
      do
        ;
      while (enumerator.MoveNext() && !this.ProcessPartBeforeFrom(enumerator.Current));
      while (enumerator.MoveNext())
        this.AddPart(enumerator.Current);
      this.RemoveLastOrderByClause();
      return this.builder.ToSqlString();
    }

    public static bool HasOrderBy(SqlString subselect)
    {
      SubselectClauseExtractor subselectClauseExtractor = new SubselectClauseExtractor(subselect);
      subselectClauseExtractor.GetSqlString();
      return subselectClauseExtractor.lastOrderByPartIndex >= 0;
    }

    private int FindFromClauseInPart(string part)
    {
      int startIndex = 0;
      int num = StringHelper.IndexOfCaseInsensitive(part, " from ");
      for (int index = 0; index < part.Length && (this.parenNestCount != 0 || index <= num); ++index)
      {
        if (part[index] == '(')
          ++this.parenNestCount;
        else if (part[index] == ')')
        {
          --this.parenNestCount;
          if (this.parenNestCount == 0)
          {
            startIndex = index + 1;
            num = StringHelper.IndexOfCaseInsensitive(part, " from ", startIndex);
          }
        }
      }
      if (startIndex == 0)
        num = StringHelper.IndexOfCaseInsensitive(part, " from ");
      return this.parenNestCount > 0 ? -1 : num;
    }

    private void AddPart(object part)
    {
      this.builder.AddObject(part);
      this.CheckLastPartForOrderByClause();
    }

    private void CheckLastPartForOrderByClause()
    {
      object obj = this.builder[this.builder.Count - 1];
      if (obj == Parameter.Placeholder)
        return;
      string str = obj as string;
      int num = StringHelper.LastIndexOfCaseInsensitive(str, "order by");
      if (num >= 0)
      {
        this.lastOrderByPartIndex = this.builder.Count - 1;
        this.lastOrderByIndex = num;
      }
      this.IgnoreOrderByInSubselect(str);
    }

    private void IgnoreOrderByInSubselect(string partString)
    {
      int index = StringHelper.LastIndexOfCaseInsensitive(partString, ")");
      if (index < 0 || !this.ParenIsAfterLastOrderBy(index))
        return;
      this.lastOrderByPartIndex = -1;
      this.lastOrderByIndex = -1;
    }

    private bool ParenIsAfterLastOrderBy(int index)
    {
      return this.builder.Count - 1 > this.lastOrderByPartIndex || index > this.lastOrderByIndex;
    }

    private void RemoveLastOrderByClause()
    {
      if (this.lastOrderByPartIndex < 0)
        return;
      while (this.builder.Count > this.lastOrderByPartIndex + 1)
        this.builder.RemoveAt(this.builder.Count - 1);
      if (!(this.builder[this.builder.Count - 1] is string str))
        return;
      this.builder[this.builder.Count - 1] = (object) str.Substring(0, this.lastOrderByIndex);
    }
  }
}
