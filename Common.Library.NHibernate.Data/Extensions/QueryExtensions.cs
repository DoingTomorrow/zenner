// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.QueryExtensions
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public static class QueryExtensions
  {
    public const int ALL_RECORDS = -1;

    public static IQuery AddOrder(this IQuery query, OrderClauseInfo orderClause)
    {
      Dictionary<string, string> aliasesColumns = QueryExtensions.ExtractAliasesColumns(query);
      foreach (string key in aliasesColumns.Keys)
      {
        if (orderClause.PropertyName.Equals(key))
          orderClause.PropertyName = aliasesColumns[key];
      }
      int byClausePosition;
      try
      {
        byClausePosition = QueryExtensions.FindOuterOrderByClausePosition(query.QueryString);
      }
      catch (Exception ex)
      {
        throw new SqlParseException("Invalid order by clause specification. It appears that multiple ORDER BY clauses are on the root level. Please check the query and inner message.");
      }
      string str1 = query.QueryString;
      string str2 = string.Format("{0} {1}", (object) orderClause.PropertyName, orderClause.Direction.Equals((object) OrderDirection.Asc) ? (object) "asc" : (object) "desc");
      if (byClausePosition > 0)
        str1 = query.QueryString.Insert(byClausePosition + 8, string.Format("{0}, ", (object) str2));
      string queryString = str1 + string.Format(" order by {0}", (object) str2);
      return HibernateManager.DataSessionFactory.GetCurrentSession().CreateQuery(queryString);
    }

    public static IQuery AddWhere(this IQuery query, IWhereClauseInfo whereClause)
    {
      Dictionary<string, string> aliasesColumns = QueryExtensions.ExtractAliasesColumns(query);
      string str = QueryExtensions.ExtractClauseString(QueryExtensions.ProcessWhereClauseExpression(whereClause, aliasesColumns));
      foreach (string key in aliasesColumns.Keys)
      {
        int num = str.IndexOf(key);
        char[] source = new char[3]{ '(', ' ', ',' };
        for (; num > 0; num = str.IndexOf(key, num + 1))
        {
          if (((IEnumerable<char>) source).Contains<char>(str[num - 1]))
            str = str.Substring(0, num - 1) + aliasesColumns[key] + str.Substring(num + key.Length, str.Length - key.Length - num);
        }
      }
      int whereClausePosition;
      try
      {
        whereClausePosition = QueryExtensions.FindOuterWhereClausePosition(query.QueryString);
      }
      catch (Exception ex)
      {
        throw new SqlParseException("Invalid where clause specification. It appears that multiple WHERE clauses are on the root level. Please check the query and inner message.");
      }
      string queryString1 = query.QueryString;
      string queryString2;
      if (whereClausePosition > 0)
      {
        queryString2 = query.QueryString.Insert(whereClausePosition + 5, string.Format(" ({0}) and ", (object) str));
      }
      else
      {
        int startIndex = query.QueryString.LastIndexOf("order by", StringComparison.CurrentCultureIgnoreCase);
        queryString2 = startIndex <= 0 ? queryString1 + string.Format(" where ({0})", (object) str) : query.QueryString.Insert(startIndex, string.Format("where ({0}) ", (object) str));
      }
      return HibernateManager.DataSessionFactory.GetCurrentSession().CreateQuery(queryString2);
    }

    private static string ExtractClauseString(ICriterion whereClause)
    {
      IEnumerable<string> distinctWhereClauses = StringsProcessor.ExtractDistinctWhereClauses(whereClause);
      string str = string.Empty;
      foreach (string clause in distinctWhereClauses)
        str = clause.Equals("and", StringComparison.CurrentCultureIgnoreCase) || clause.Equals("or", StringComparison.CurrentCultureIgnoreCase) ? str + clause + (object) ' ' : str + StringsProcessor.AdjustWhereClauseOperands(clause) + (object) ' ';
      return string.Format("({0})", (object) str);
    }

    public static ICriterion ProcessWhereClauseExpression(
      IWhereClauseInfo clause,
      Dictionary<string, string> aliasesList)
    {
      switch (clause)
      {
        case SimpleWhereClauseInfo _:
          SimpleWhereClauseInfo simpleWhereClauseInfo = clause as SimpleWhereClauseInfo;
          return QueryExtensions.TranslateExpressionOperation(aliasesList[simpleWhereClauseInfo.PropertyName] ?? simpleWhereClauseInfo.PropertyName, simpleWhereClauseInfo.Operator, simpleWhereClauseInfo.Value);
        case CompositeWhereClauseInfo _:
          CompositeWhereClauseInfo compositeWhereClauseInfo = clause as CompositeWhereClauseInfo;
          if (compositeWhereClauseInfo.LogicalOperator.Equals("or", StringComparison.CurrentCultureIgnoreCase))
            return (ICriterion) Restrictions.Or(QueryExtensions.ProcessWhereClauseExpression(compositeWhereClauseInfo.LeftHandSideClause, aliasesList), QueryExtensions.ProcessWhereClauseExpression(compositeWhereClauseInfo.RightHandSideClause, aliasesList));
          if (compositeWhereClauseInfo.LogicalOperator.Equals("and", StringComparison.CurrentCultureIgnoreCase))
            return (ICriterion) Restrictions.And(QueryExtensions.ProcessWhereClauseExpression(compositeWhereClauseInfo.LeftHandSideClause, aliasesList), QueryExtensions.ProcessWhereClauseExpression(compositeWhereClauseInfo.RightHandSideClause, aliasesList));
          break;
      }
      return (ICriterion) null;
    }

    public static ICriterion TranslateExpressionOperation(
      string property,
      HQLOperator operation,
      object value)
    {
      ICriterion criterion = (ICriterion) null;
      switch (operation)
      {
        case HQLOperator.IsLessThan:
          criterion = (ICriterion) Restrictions.Lt(property, value).IgnoreCase();
          break;
        case HQLOperator.IsLessThanOrEqualTo:
          criterion = (ICriterion) Restrictions.Le(property, value).IgnoreCase();
          break;
        case HQLOperator.IsEqualTo:
          criterion = (ICriterion) Restrictions.Eq(property, value).IgnoreCase();
          break;
        case HQLOperator.IsNotEqualTo:
          criterion = (ICriterion) Restrictions.Not((ICriterion) Restrictions.Eq(property, value).IgnoreCase());
          break;
        case HQLOperator.IsGreaterThanOrEqualTo:
          criterion = (ICriterion) Restrictions.Ge(property, value).IgnoreCase();
          break;
        case HQLOperator.IsGreaterThan:
          criterion = (ICriterion) Restrictions.Gt(property, value).IgnoreCase();
          break;
        case HQLOperator.StartsWith:
          criterion = (ICriterion) Restrictions.InsensitiveLike(property, value.ToString(), MatchMode.Start);
          break;
        case HQLOperator.EndsWith:
          criterion = (ICriterion) Restrictions.InsensitiveLike(property, value.ToString(), MatchMode.End);
          break;
        case HQLOperator.Contains:
          criterion = (ICriterion) Restrictions.InsensitiveLike(property, value.ToString(), MatchMode.Anywhere);
          break;
        case HQLOperator.IsContainedIn:
          criterion = (ICriterion) Restrictions.In(property, value as object[]);
          break;
        case HQLOperator.IsNull:
          criterion = (ICriterion) Restrictions.IsNull(property);
          break;
        case HQLOperator.IsNotNull:
          criterion = (ICriterion) Restrictions.IsNotNull(property);
          break;
        case HQLOperator.Wildcards:
          criterion = (ICriterion) Restrictions.InsensitiveLike(property, (object) value.ToString().Replace("*", "%").Replace("?", "_"));
          break;
      }
      return criterion;
    }

    private static string FormatValueAsShortDateFunction(DateTime value)
    {
      return string.Format("todate('{0}','YYYY-MM-DD')", (object) value.ToString("yyyy-MM-dd"));
    }

    public static bool HasColumnsSelect(this IQuery query)
    {
      if (query.QueryString.IndexOf("select", StringComparison.InvariantCultureIgnoreCase) != 0)
        return false;
      int num = 0;
      string[] strArray = query.QueryString.Split(' ');
      for (int index = 1; index < strArray.Length; ++index)
      {
        if (strArray[index] != string.Empty)
          ++num;
        if (num == 2)
          return !strArray[index].Equals("from", StringComparison.InvariantCultureIgnoreCase);
      }
      return false;
    }

    public static Dictionary<string, string> ExtractAliasesColumns(IQuery query)
    {
      Dictionary<string, string> aliasesColumns = new Dictionary<string, string>();
      ReplaceProcessedString replaceProcessedString = StringsProcessor.CreateReplaceProcessedString(query.QueryString.Replace("\n", string.Empty).Replace("\r\n", string.Empty), "#{0}#");
      List<string> list = Enumerable.Where<string>(((IEnumerable<string>) replaceProcessedString.ProcessedString.Split(' ', ',')).AsEnumerable<string>(), (Func<string, bool>) (s => s != " ")).ToList<string>();
      foreach (string returnAlias in query.ReturnAliases)
      {
        string columnAlias = returnAlias;
        int index = list.FindIndex(0, (Predicate<string>) (s => s.Equals(columnAlias)));
        if (index > 2)
        {
          string replacements = list[index - 2];
          if (replaceProcessedString.ReplacementsDictionary.ContainsKey(replacements))
            replacements = replaceProcessedString.ReplacementsDictionary[replacements];
          if (!aliasesColumns.ContainsKey(columnAlias))
            aliasesColumns.Add(columnAlias, replacements);
        }
      }
      return aliasesColumns;
    }

    private static int FindOuterOrderByClausePosition(string queryString)
    {
      int num = StringsProcessor.SearchWordPositionOuterLevel("order by", queryString);
      return num > 0 && queryString.LastIndexOf("from", StringComparison.CurrentCultureIgnoreCase) < num ? num : -1;
    }

    private static int FindOuterWhereClausePosition(string queryString)
    {
      int num = StringsProcessor.SearchWordPositionOuterLevel("where", queryString);
      return num > 0 && StringsProcessor.SearchWordPositionOuterLevel("from", queryString) < num ? num : -1;
    }
  }
}
