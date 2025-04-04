// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.QuerySkipParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Param
{
  public class QuerySkipParameterSpecification : IParameterSpecification
  {
    private readonly string[] idTrack;
    private readonly string limitParametersNameForThisQuery = "<nhskip" + Guid.NewGuid().ToString("N");
    private readonly IType type = (IType) NHibernateUtil.Int32;

    public QuerySkipParameterSpecification()
    {
      this.idTrack = new string[1]
      {
        this.limitParametersNameForThisQuery
      };
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      this.Bind(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      int[] array = multiSqlQueryParametersList.GetEffectiveParameterLocations(this.limitParametersNameForThisQuery).ToArray<int>();
      if (array.Length <= 0)
        return;
      int firstRow = queryParameters.RowSelection.FirstRow;
      int index = array[0];
      this.type.NullSafeSet(command, (object) firstRow, index, session);
    }

    public IType ExpectedType
    {
      get => this.type;
      set => throw new InvalidOperationException();
    }

    public string RenderDisplayInfo() => "query-skip";

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      return (IEnumerable<string>) this.idTrack;
    }

    public override bool Equals(object obj) => this.Equals(obj as QuerySkipParameterSpecification);

    public bool Equals(QuerySkipParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.limitParametersNameForThisQuery, (object) this.limitParametersNameForThisQuery);
    }

    public override int GetHashCode() => this.limitParametersNameForThisQuery.GetHashCode();
  }
}
