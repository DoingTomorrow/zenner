// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.ParametersBackTrackExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Param
{
  public static class ParametersBackTrackExtensions
  {
    public static IEnumerable<int> GetEffectiveParameterLocations(
      this IList<Parameter> sqlParameters,
      string backTrackId)
    {
      for (int i = 0; i < sqlParameters.Count; ++i)
      {
        if (backTrackId.Equals(sqlParameters[i].BackTrack))
          yield return i;
      }
    }

    public static SqlType[] GetQueryParameterTypes(
      this IEnumerable<IParameterSpecification> parameterSpecs,
      List<Parameter> sqlQueryParametersList,
      ISessionFactoryImplementor factory)
    {
      return parameterSpecs.Select(specification => new
      {
        specification = specification,
        firstParameterId = specification.GetIdsForBackTrack((IMapping) factory).First<string>()
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier4 = _param1,
        effectiveParameterLocations = sqlQueryParametersList.GetEffectiveParameterLocations(_param1.firstParameterId)
      }).SelectMany(_param0 => _param0.effectiveParameterLocations, (_param0, location) => new
      {
        Location = location,
        Type = _param0.\u003C\u003Eh__TransparentIdentifier4.specification.ExpectedType
      }).GroupBy(typeLocation => typeLocation.Location).OrderBy<IGrouping<int, \u003C\u003Ef__AnonymousTyped<int, IType>>, int>(locations => locations.Key).Select<IGrouping<int, \u003C\u003Ef__AnonymousTyped<int, IType>>, IType>(locations => locations.First().Type).SelectMany<IType, SqlType>((Func<IType, IEnumerable<SqlType>>) (t => (IEnumerable<SqlType>) t.SqlTypes((IMapping) factory))).ToArray<SqlType>();
    }

    public static void ResetEffectiveExpectedType(
      this IEnumerable<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters)
    {
      foreach (IExplicitParameterSpecification parameterSpecification in parameterSpecs.OfType<IExplicitParameterSpecification>())
        parameterSpecification.SetEffectiveType(queryParameters);
    }
  }
}
