// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.IQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Event;
using NHibernate.Hql;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  public interface IQueryPlan
  {
    ParameterMetadata ParameterMetadata { get; }

    ISet<string> QuerySpaces { get; }

    IQueryTranslator[] Translators { get; }

    ReturnMetadata ReturnMetadata { get; }

    void PerformList(
      QueryParameters queryParameters,
      ISessionImplementor statelessSessionImpl,
      IList results);

    int PerformExecuteUpdate(
      QueryParameters queryParameters,
      ISessionImplementor statelessSessionImpl);

    IEnumerable<T> PerformIterate<T>(QueryParameters queryParameters, IEventSource session);

    IEnumerable PerformIterate(QueryParameters queryParameters, IEventSource session);
  }
}
