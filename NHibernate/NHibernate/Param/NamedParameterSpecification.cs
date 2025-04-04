// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.NamedParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Param
{
  public class NamedParameterSpecification : AbstractExplicitParameterSpecification
  {
    private const string NamedParameterIdTemplate = "<nnh{0}_span{1}>";
    private readonly string name;

    public NamedParameterSpecification(int sourceLine, int sourceColumn, string name)
      : base(sourceLine, sourceColumn)
    {
      this.name = name;
    }

    public string Name => this.name;

    public override string RenderDisplayInfo()
    {
      return this.ExpectedType == null ? string.Format("name={0}, expectedType={1}", (object) this.name, (object) "Unknow") : string.Format("name={0}, expectedType={1}", (object) this.name, (object) this.ExpectedType);
    }

    public override IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      int paremeterSpan = this.GetParemeterSpan(sessionFactory);
      for (int i = 0; i < paremeterSpan; ++i)
        yield return string.Format("<nnh{0}_span{1}>", (object) this.name, (object) i);
    }

    public override void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      this.Bind(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
    }

    public override void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      TypedValue namedParameter = queryParameters.NamedParameters[this.name];
      string backTrackId = this.GetIdsForBackTrack((IMapping) session.Factory).First<string>();
      foreach (int parameterLocation in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
        this.ExpectedType.NullSafeSet(command, this.GetPagingValue(namedParameter.Value, session.Factory.Dialect, queryParameters), parameterLocation + singleSqlParametersOffset, session);
    }

    public override int GetSkipValue(QueryParameters queryParameters)
    {
      return (int) queryParameters.NamedParameters[this.name].Value;
    }

    public override void SetEffectiveType(QueryParameters queryParameters)
    {
      this.ExpectedType = queryParameters.NamedParameters[this.name].Type;
    }

    public override bool Equals(object obj) => this.Equals(obj as NamedParameterSpecification);

    public bool Equals(NamedParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.name, (object) this.name);
    }

    public override int GetHashCode() => this.name.GetHashCode() ^ 211;
  }
}
