// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.IParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Param
{
  public interface IParameterSpecification
  {
    void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session);

    void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session);

    IType ExpectedType { get; set; }

    string RenderDisplayInfo();

    IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory);
  }
}
