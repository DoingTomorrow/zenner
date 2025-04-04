// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.Util.DdlFormatter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.AdoNet.Util
{
  public class DdlFormatter : IFormatter
  {
    private const string Indent1 = "\n    ";
    private const string Indent2 = "\n      ";
    private const string Indent3 = "\n        ";

    public virtual string Format(string sql)
    {
      if (sql.ToLowerInvariant().StartsWith("create table"))
        return this.FormatCreateTable(sql);
      if (sql.ToLowerInvariant().StartsWith("alter table"))
        return this.FormatAlterTable(sql);
      return sql.ToLowerInvariant().StartsWith("comment on") ? this.FormatCommentOn(sql) : "\n    " + sql;
    }

    protected virtual string FormatCommentOn(string sql)
    {
      StringBuilder stringBuilder = new StringBuilder(60).Append("\n    ");
      IEnumerator<string> enumerator = new StringTokenizer(sql, " '[]\"", true).GetEnumerator();
      bool flag = false;
      while (enumerator.MoveNext())
      {
        string current = enumerator.Current;
        stringBuilder.Append(current);
        if (DdlFormatter.IsQuote(current))
          flag = !flag;
        else if (!flag && "is".Equals(current))
          stringBuilder.Append("\n      ");
      }
      return stringBuilder.ToString();
    }

    protected virtual string FormatAlterTable(string sql)
    {
      StringBuilder stringBuilder = new StringBuilder(60).Append("\n    ");
      IEnumerator<string> enumerator = new StringTokenizer(sql, " (,)'[]\"", true).GetEnumerator();
      bool flag = false;
      while (enumerator.MoveNext())
      {
        string current = enumerator.Current;
        if (DdlFormatter.IsQuote(current))
          flag = !flag;
        else if (!flag && DdlFormatter.IsBreak(current))
          stringBuilder.Append("\n        ");
        stringBuilder.Append(current);
      }
      return stringBuilder.ToString();
    }

    protected virtual string FormatCreateTable(string sql)
    {
      StringBuilder stringBuilder = new StringBuilder(60).Append("\n    ");
      IEnumerator<string> enumerator = new StringTokenizer(sql, "(,)'[]\"", true).GetEnumerator();
      int num = 0;
      bool flag = false;
      while (enumerator.MoveNext())
      {
        string current = enumerator.Current;
        if (DdlFormatter.IsQuote(current))
        {
          flag = !flag;
          stringBuilder.Append(current);
        }
        else if (flag)
        {
          stringBuilder.Append(current);
        }
        else
        {
          if (")".Equals(current))
          {
            --num;
            if (num == 0)
              stringBuilder.Append("\n    ");
          }
          stringBuilder.Append(current);
          if (",".Equals(current) && num == 1)
            stringBuilder.Append("\n      ");
          if ("(".Equals(current))
          {
            ++num;
            if (num == 1)
              stringBuilder.Append("\n        ");
          }
        }
      }
      return stringBuilder.ToString();
    }

    private static bool IsBreak(string token)
    {
      return "drop".Equals(token) || "add".Equals(token) || "references".Equals(token) || "foreign".Equals(token) || "on".Equals(token);
    }

    private static bool IsQuote(string token)
    {
      return "\"".Equals(token) || "`".Equals(token) || "]".Equals(token) || "[".Equals(token) || "'".Equals(token);
    }
  }
}
