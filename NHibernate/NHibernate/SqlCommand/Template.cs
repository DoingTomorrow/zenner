// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.Template
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Dialect.Function;
using NHibernate.Util;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public sealed class Template
  {
    private static readonly ISet Keywords = (ISet) new HashedSet();
    private static readonly ISet BeforeTableKeywords = (ISet) new HashedSet();
    private static readonly ISet FunctionKeywords = (ISet) new HashedSet();
    public static readonly string Placeholder = "$PlaceHolder$";

    static Template()
    {
      Template.Keywords.Add((object) "and");
      Template.Keywords.Add((object) "or");
      Template.Keywords.Add((object) "not");
      Template.Keywords.Add((object) "like");
      Template.Keywords.Add((object) "is");
      Template.Keywords.Add((object) "in");
      Template.Keywords.Add((object) "between");
      Template.Keywords.Add((object) "null");
      Template.Keywords.Add((object) "select");
      Template.Keywords.Add((object) "distinct");
      Template.Keywords.Add((object) "from");
      Template.Keywords.Add((object) "join");
      Template.Keywords.Add((object) "inner");
      Template.Keywords.Add((object) "outer");
      Template.Keywords.Add((object) "left");
      Template.Keywords.Add((object) "right");
      Template.Keywords.Add((object) "on");
      Template.Keywords.Add((object) "where");
      Template.Keywords.Add((object) "having");
      Template.Keywords.Add((object) "group");
      Template.Keywords.Add((object) "order");
      Template.Keywords.Add((object) "by");
      Template.Keywords.Add((object) "desc");
      Template.Keywords.Add((object) "asc");
      Template.Keywords.Add((object) "limit");
      Template.Keywords.Add((object) "any");
      Template.Keywords.Add((object) "some");
      Template.Keywords.Add((object) "exists");
      Template.Keywords.Add((object) "all");
      Template.BeforeTableKeywords.Add((object) "from");
      Template.BeforeTableKeywords.Add((object) "join");
      Template.FunctionKeywords.Add((object) "as");
      Template.FunctionKeywords.Add((object) "leading");
      Template.FunctionKeywords.Add((object) "trailing");
      Template.FunctionKeywords.Add((object) "from");
      Template.FunctionKeywords.Add((object) "case");
      Template.FunctionKeywords.Add((object) "when");
      Template.FunctionKeywords.Add((object) "then");
      Template.FunctionKeywords.Add((object) "else");
      Template.FunctionKeywords.Add((object) "end");
    }

    private Template()
    {
    }

    public static string RenderWhereStringTemplate(
      string sqlWhereString,
      NHibernate.Dialect.Dialect dialect,
      SQLFunctionRegistry functionRegistry)
    {
      return Template.RenderWhereStringTemplate(sqlWhereString, Template.Placeholder, dialect, functionRegistry);
    }

    public static string RenderWhereStringTemplate(
      string sqlWhereString,
      string placeholder,
      NHibernate.Dialect.Dialect dialect,
      SQLFunctionRegistry functionRegistry)
    {
      string delim = new StringBuilder().Append("=><!+-*/()',|&`").Append(" \n\r\f\t").Append(dialect.OpenQuote).Append(dialect.CloseQuote).ToString();
      StringTokenizer stringTokenizer = new StringTokenizer(sqlWhereString, delim, true);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      IEnumerator<string> enumerator = stringTokenizer.GetEnumerator();
      bool flag6 = enumerator.MoveNext();
      string current = flag6 ? enumerator.Current : (string) null;
      string str1 = string.Empty;
      while (flag6)
      {
        string token = current;
        string str2 = token.ToLowerInvariant();
        flag6 = enumerator.MoveNext();
        current = flag6 ? enumerator.Current : (string) null;
        bool flag7 = false;
        if (!flag2 && "'".Equals(token))
        {
          flag1 = !flag1;
          flag7 = true;
        }
        if (!flag1)
        {
          bool flag8;
          if ("`".Equals(token))
          {
            flag8 = !flag2;
            token = str2 = flag8 ? dialect.OpenQuote.ToString() : dialect.CloseQuote.ToString();
            flag2 = flag8;
            flag7 = true;
          }
          else if (!flag2 && (int) dialect.OpenQuote == (int) token[0])
          {
            flag8 = true;
            flag2 = true;
            flag7 = true;
          }
          else if (flag2 && (int) dialect.CloseQuote == (int) token[0])
          {
            flag2 = false;
            flag7 = true;
            flag8 = false;
          }
          else
            flag8 = false;
          if (flag8 && !flag4 && !str1.EndsWith("."))
            stringBuilder.Append(placeholder).Append('.');
        }
        if (flag1 || flag2 || flag7 || char.IsWhiteSpace(token[0]))
          stringBuilder.Append(token);
        else if (flag3)
        {
          stringBuilder.Append(token);
          flag3 = false;
          flag5 = true;
        }
        else if (flag5)
        {
          if (!"as".Equals(str2))
            flag5 = false;
          stringBuilder.Append(token);
        }
        else if (Template.IsNamedParameter(token))
          stringBuilder.Append(token);
        else if (Template.IsIdentifier(token, dialect) && !Template.IsFunctionOrKeyword(str2, current, dialect, functionRegistry))
        {
          stringBuilder.Append(placeholder).Append('.').Append(token);
        }
        else
        {
          if (Template.BeforeTableKeywords.Contains((object) str2))
          {
            flag3 = true;
            flag4 = true;
          }
          else if (flag4 && ",".Equals(str2))
            flag3 = true;
          stringBuilder.Append(token);
        }
        if (flag4 && Template.Keywords.Contains((object) str2) && !Template.BeforeTableKeywords.Contains((object) str2))
          flag4 = false;
        str1 = token;
      }
      return stringBuilder.ToString();
    }

    public static string RenderOrderByStringTemplate(
      string sqlOrderByString,
      NHibernate.Dialect.Dialect dialect,
      SQLFunctionRegistry functionRegistry)
    {
      string delim = new StringBuilder().Append("=><!+-*/()',|&`").Append(" \n\r\f\t").Append(dialect.OpenQuote).Append(dialect.CloseQuote).ToString();
      StringTokenizer stringTokenizer = new StringTokenizer(sqlOrderByString, delim, true);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag1 = false;
      bool flag2 = false;
      IEnumerator<string> enumerator = stringTokenizer.GetEnumerator();
      bool flag3 = enumerator.MoveNext();
      string current = flag3 ? enumerator.Current : (string) null;
      while (flag3)
      {
        string token = current;
        string lcToken = token.ToLowerInvariant();
        flag3 = enumerator.MoveNext();
        current = flag3 ? enumerator.Current : (string) null;
        bool flag4 = false;
        if (!flag2 && "'".Equals(token))
        {
          flag1 = !flag1;
          flag4 = true;
        }
        if (!flag1)
        {
          bool flag5;
          if ("`".Equals(token))
          {
            flag5 = !flag2;
            token = lcToken = flag5 ? dialect.OpenQuote.ToString() : dialect.CloseQuote.ToString();
            flag2 = flag5;
            flag4 = true;
          }
          else if (!flag2 && (int) dialect.OpenQuote == (int) token[0])
          {
            flag5 = true;
            flag2 = true;
            flag4 = true;
          }
          else if (flag2 && (int) dialect.CloseQuote == (int) token[0])
          {
            flag2 = false;
            flag4 = true;
            flag5 = false;
          }
          else
            flag5 = false;
          if (flag5)
            stringBuilder.Append(Template.Placeholder).Append('.');
        }
        if (flag1 || flag2 || flag4 || char.IsWhiteSpace(token[0]))
          stringBuilder.Append(token);
        else if (Template.IsIdentifier(token, dialect) && !Template.IsFunctionOrKeyword(lcToken, current, dialect, functionRegistry))
          stringBuilder.Append(Template.Placeholder).Append('.').Append(token);
        else
          stringBuilder.Append(token);
      }
      return stringBuilder.ToString();
    }

    private static bool IsNamedParameter(string token) => token.StartsWith(":");

    private static bool IsFunctionOrKeyword(
      string lcToken,
      string nextToken,
      NHibernate.Dialect.Dialect dialect,
      SQLFunctionRegistry functionRegistry)
    {
      return "(".Equals(nextToken) || Template.Keywords.Contains((object) lcToken) || functionRegistry.HasFunction(lcToken) || dialect.Keywords.Contains(lcToken) || dialect.IsKnownToken(lcToken, nextToken) || Template.FunctionKeywords.Contains((object) lcToken);
    }

    private static bool IsIdentifier(string token, NHibernate.Dialect.Dialect dialect)
    {
      if (token[0] == '`')
        return true;
      return char.IsLetter(token[0]) && token.IndexOf('.') < 0;
    }
  }
}
