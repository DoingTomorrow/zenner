// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.IQueryTranslatorFactory2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql
{
  public interface IQueryTranslatorFactory2 : IQueryTranslatorFactory
  {
    IQueryTranslator[] CreateQueryTranslators(
      string queryIdentifier,
      IQueryExpression queryExpression,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> filters,
      ISessionFactoryImplementor factory);
  }
}
