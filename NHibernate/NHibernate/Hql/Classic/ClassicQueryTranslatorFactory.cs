// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.ClassicQueryTranslatorFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class ClassicQueryTranslatorFactory : IQueryTranslatorFactory
  {
    public IQueryTranslator[] CreateQueryTranslators(
      string queryString,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> filters,
      ISessionFactoryImplementor factory)
    {
      QueryTranslator[] array = ((IEnumerable<string>) QuerySplitter.ConcreteQueries(queryString, factory)).Select<string, QueryTranslator>((Func<string, QueryTranslator>) (hql => new QueryTranslator(queryString, hql, filters, factory))).ToArray<QueryTranslator>();
      foreach (QueryTranslator queryTranslator in array)
      {
        if (collectionRole == null)
          queryTranslator.Compile(factory.Settings.QuerySubstitutions, shallow);
        else
          queryTranslator.Compile(collectionRole, factory.Settings.QuerySubstitutions, shallow);
      }
      return (IQueryTranslator[]) array;
    }
  }
}
