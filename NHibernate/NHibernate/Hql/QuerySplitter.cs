// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.QuerySplitter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Hql.Classic;
using NHibernate.Hql.Util;
using NHibernate.Util;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace NHibernate.Hql
{
  public class QuerySplitter
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (QuerySplitter));
    private static readonly ISet beforeClassTokens = (ISet) new HashedSet();
    private static readonly ISet notAfterClassTokens = (ISet) new HashedSet();

    static QuerySplitter()
    {
      QuerySplitter.beforeClassTokens.Add((object) "from");
      QuerySplitter.beforeClassTokens.Add((object) "delete");
      QuerySplitter.beforeClassTokens.Add((object) "update");
      QuerySplitter.beforeClassTokens.Add((object) ",");
      QuerySplitter.notAfterClassTokens.Add((object) "in");
      QuerySplitter.notAfterClassTokens.Add((object) "from");
      QuerySplitter.notAfterClassTokens.Add((object) ")");
    }

    public static string[] ConcreteQueries(string query, ISessionFactoryImplementor factory)
    {
      SessionFactoryHelper sessionFactoryHelper = new SessionFactoryHelper(factory);
      string[] strArray1 = StringHelper.Split(" \n\r\f\t(),", query, true);
      if (strArray1.Length == 0)
        return new string[1]{ query };
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      StringBuilder stringBuilder = new StringBuilder(40);
      int num = 0;
      string o = (string) null;
      int index1 = 0;
      string str1 = (string) null;
      stringBuilder.Append(strArray1[0]);
      StringHelper.EqualsCaseInsensitive("select", strArray1[0]);
      for (int index2 = 1; index2 < strArray1.Length; ++index2)
      {
        if (!ParserHelper.IsWhitespace(strArray1[index2 - 1]))
          o = strArray1[index2 - 1].ToLowerInvariant();
        StringHelper.EqualsCaseInsensitive("from", strArray1[index2]);
        string str2 = strArray1[index2];
        if (!ParserHelper.IsWhitespace(str2) || o == null)
        {
          if (index1 <= index2)
          {
            for (index1 = index2 + 1; index1 < strArray1.Length; ++index1)
            {
              str1 = strArray1[index1].ToLowerInvariant();
              if (!ParserHelper.IsWhitespace(str1))
                break;
            }
          }
          if (o != null && QuerySplitter.beforeClassTokens.Contains((object) o) && (str1 == null || !QuerySplitter.notAfterClassTokens.Contains((object) str1)) || "class".Equals(o))
          {
            Type importedClass = sessionFactoryHelper.GetImportedClass(str2);
            if (importedClass != null)
            {
              string[] implementors = factory.GetImplementors(importedClass.FullName);
              string str3 = "$clazz" + (object) num++ + "$";
              if (implementors != null)
              {
                arrayList1.Add((object) str3);
                arrayList2.Add((object) implementors);
              }
              str2 = str3;
            }
          }
        }
        stringBuilder.Append(str2);
      }
      string[] strArray2 = StringHelper.Multiply(stringBuilder.ToString(), arrayList1.GetEnumerator(), arrayList2.GetEnumerator());
      if (strArray2.Length == 0)
        QuerySplitter.log.Warn((object) ("no persistent classes found for query class: " + query));
      return strArray2;
    }

    private static bool IsPossiblyClassName(string last, string next)
    {
      if ("class".Equals(last))
        return true;
      return QuerySplitter.beforeClassTokens.Contains((object) last) && !QuerySplitter.notAfterClassTokens.Contains((object) next);
    }
  }
}
