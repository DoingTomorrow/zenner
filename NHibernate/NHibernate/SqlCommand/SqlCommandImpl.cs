// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SqlCommandImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SqlCommandImpl : ISqlCommand
  {
    private readonly SqlString query;
    private readonly ICollection<IParameterSpecification> specifications;
    private readonly QueryParameters queryParameters;
    private readonly ISessionFactoryImplementor factory;
    private SqlType[] parameterTypes;
    private List<Parameter> sqlQueryParametersList;

    public SqlCommandImpl(
      SqlString query,
      ICollection<IParameterSpecification> specifications,
      QueryParameters queryParameters,
      ISessionFactoryImplementor factory)
    {
      this.query = query;
      this.specifications = specifications;
      this.queryParameters = queryParameters;
      this.factory = factory;
    }

    public List<Parameter> SqlQueryParametersList
    {
      get
      {
        return this.sqlQueryParametersList ?? (this.sqlQueryParametersList = this.query.GetParameters().ToList<Parameter>());
      }
    }

    public SqlType[] ParameterTypes
    {
      get
      {
        return this.parameterTypes ?? (this.parameterTypes = this.specifications.GetQueryParameterTypes(this.SqlQueryParametersList, this.factory));
      }
    }

    public SqlString Query => this.query;

    public IEnumerable<IParameterSpecification> Specifications
    {
      get => (IEnumerable<IParameterSpecification>) this.specifications;
    }

    public QueryParameters QueryParameters => this.queryParameters;

    public void ResetParametersIndexesForTheCommand(int singleSqlParametersOffset)
    {
      if (singleSqlParametersOffset < 0)
        throw new AssertionFailure("singleSqlParametersOffset < 0 - this indicate a bug in NHibernate ");
      foreach (IParameterSpecification specification in this.Specifications)
      {
        int[] array = this.SqlQueryParametersList.GetEffectiveParameterLocations(specification.GetIdsForBackTrack((IMapping) this.factory).First<string>()).ToArray<int>();
        if (array.Length > 0)
        {
          int num1 = ((IEnumerable<int>) array).First<int>() + singleSqlParametersOffset;
          foreach (int num2 in array)
          {
            int columnSpan = specification.ExpectedType.GetColumnSpan((IMapping) this.factory);
            for (int index = 0; index < columnSpan; ++index)
              this.sqlQueryParametersList[num2 + index].ParameterPosition = new int?(num1 + index);
          }
        }
      }
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> commandQueryParametersList,
      int singleSqlParametersOffset,
      ISessionImplementor session)
    {
      foreach (IParameterSpecification specification in this.Specifications)
        specification.Bind(command, commandQueryParametersList, singleSqlParametersOffset, (IList<Parameter>) this.SqlQueryParametersList, this.QueryParameters, session);
    }

    public void Bind(IDbCommand command, ISessionImplementor session)
    {
      foreach (IParameterSpecification specification in this.Specifications)
        specification.Bind(command, (IList<Parameter>) this.SqlQueryParametersList, this.QueryParameters, session);
    }
  }
}
