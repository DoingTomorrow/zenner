// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.IQueryTranslator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Event;
using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql
{
  public interface IQueryTranslator
  {
    void Compile(IDictionary<string, string> replacements, bool shallow);

    IList List(ISessionImplementor session, QueryParameters queryParameters);

    IEnumerable GetEnumerable(QueryParameters queryParameters, IEventSource session);

    int ExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session);

    ISet<string> QuerySpaces { get; }

    string SQLString { get; }

    IList<string> CollectSqlStrings { get; }

    string QueryString { get; }

    IDictionary<string, IFilter> EnabledFilters { get; }

    IType[] ReturnTypes { get; }

    string[] ReturnAliases { get; }

    string[][] GetColumnNames();

    bool ContainsCollectionFetches { get; }

    bool IsManipulationStatement { get; }

    NHibernate.Loader.Loader Loader { get; }

    IType[] ActualReturnTypes { get; }

    ParameterMetadata BuildParameterMetadata();
  }
}
