// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.CriteriaNamedParameterSpecification
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
  public class CriteriaNamedParameterSpecification : IParameterSpecification
  {
    private const string CriteriaNamedParameterIdTemplate = "<crnh-{0}_span{1}>";
    private readonly string name;

    public CriteriaNamedParameterSpecification(string name, IType expectedType)
    {
      this.name = name;
      this.ExpectedType = expectedType;
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
      TypedValue namedParameter = queryParameters.NamedParameters[this.name];
      string backTrackId = this.GetIdsForBackTrack((IMapping) session.Factory).First<string>();
      foreach (int parameterLocation in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
        this.ExpectedType.NullSafeSet(command, namedParameter.Value, parameterLocation + singleSqlParametersOffset, session);
    }

    public IType ExpectedType { get; set; }

    public string RenderDisplayInfo()
    {
      return this.ExpectedType == null ? string.Format("criteria: parameter-name={0}, expectedType={1}", (object) this.name, (object) "Unknow") : string.Format("criteria: parameter-name={0}, expectedType={1}", (object) this.name, (object) this.ExpectedType);
    }

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      int paremeterSpan = this.GetParemeterSpan(sessionFactory);
      for (int i = 0; i < paremeterSpan; ++i)
        yield return string.Format("<crnh-{0}_span{1}>", (object) this.name, (object) i);
    }

    protected int GetParemeterSpan(IMapping sessionFactory)
    {
      return sessionFactory != null ? this.ExpectedType.GetColumnSpan(sessionFactory) : throw new ArgumentNullException(nameof (sessionFactory));
    }

    public override bool Equals(object obj)
    {
      return base.Equals((object) (obj as CriteriaNamedParameterSpecification));
    }

    public bool Equals(CriteriaNamedParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.name, (object) this.name);
    }

    public override int GetHashCode() => this.name.GetHashCode() ^ 211;
  }
}
