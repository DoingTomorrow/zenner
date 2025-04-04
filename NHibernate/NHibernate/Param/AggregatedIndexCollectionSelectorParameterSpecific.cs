// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.AggregatedIndexCollectionSelectorParameterSpecifications
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.Param
{
  public class AggregatedIndexCollectionSelectorParameterSpecifications : IParameterSpecification
  {
    private readonly IList<IParameterSpecification> _paramSpecs;

    public AggregatedIndexCollectionSelectorParameterSpecifications(
      IList<IParameterSpecification> paramSpecs)
    {
      this._paramSpecs = paramSpecs;
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      throw new NotImplementedException();
    }

    public IType ExpectedType
    {
      get => (IType) null;
      set
      {
      }
    }

    public string RenderDisplayInfo() => "index-selector [" + this.CollectDisplayInfo() + "]";

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      throw new NotImplementedException();
    }

    private string CollectDisplayInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (IParameterSpecification paramSpec in (IEnumerable<IParameterSpecification>) this._paramSpecs)
        stringBuilder.Append(paramSpec.RenderDisplayInfo());
      return stringBuilder.ToString();
    }
  }
}
