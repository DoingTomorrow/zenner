// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.PositionalParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Param
{
  public class PositionalParameterSpecification : AbstractExplicitParameterSpecification
  {
    private const string PositionalParameterIdTemplate = "<pos{0}_span{1}>";
    private readonly int hqlPosition;

    public PositionalParameterSpecification(int sourceLine, int sourceColumn, int hqlPosition)
      : base(sourceLine, sourceColumn)
    {
      this.hqlPosition = hqlPosition;
    }

    public int HqlPosition => this.hqlPosition;

    public override string RenderDisplayInfo()
    {
      return "ordinal=" + (object) this.hqlPosition + ", expectedType=" + (object) this.ExpectedType;
    }

    public override IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      int paremeterSpan = this.GetParemeterSpan(sessionFactory);
      for (int i = 0; i < paremeterSpan; ++i)
        yield return string.Format("<pos{0}_span{1}>", (object) this.hqlPosition, (object) i);
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
      IType expectedType = this.ExpectedType;
      object positionalParameterValue = queryParameters.PositionalParameterValues[this.hqlPosition];
      string backTrackId = this.GetIdsForBackTrack((IMapping) session.Factory).First<string>();
      foreach (int parameterLocation in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
        expectedType.NullSafeSet(command, this.GetPagingValue(positionalParameterValue, session.Factory.Dialect, queryParameters), parameterLocation + singleSqlParametersOffset, session);
    }

    public override int GetSkipValue(QueryParameters queryParameters)
    {
      return (int) queryParameters.PositionalParameterValues[this.hqlPosition];
    }

    public override void SetEffectiveType(QueryParameters queryParameters)
    {
      this.ExpectedType = queryParameters.PositionalParameterTypes[this.hqlPosition];
    }

    public override bool Equals(object obj) => this.Equals(obj as PositionalParameterSpecification);

    public bool Equals(PositionalParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || other.hqlPosition == this.hqlPosition;
    }

    public override int GetHashCode() => this.hqlPosition ^ 751;
  }
}
