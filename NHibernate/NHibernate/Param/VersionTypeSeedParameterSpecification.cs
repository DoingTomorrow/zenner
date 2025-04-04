// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.VersionTypeSeedParameterSpecification
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
  public class VersionTypeSeedParameterSpecification : IParameterSpecification
  {
    private const string IdBackTrack = "<nhv_seed_nh>";
    private readonly string[] idForBackTracks = new string[1]
    {
      "<nhv_seed_nh>"
    };
    private readonly IVersionType type;

    public VersionTypeSeedParameterSpecification(IVersionType type) => this.type = type;

    public void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      int index = sqlQueryParametersList.GetEffectiveParameterLocations("<nhv_seed_nh>").Single<int>();
      this.type.NullSafeSet(command, this.type.Seed(session), index, session);
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      throw new NotSupportedException("Not supported for multiquery loader.");
    }

    public IType ExpectedType
    {
      get => (IType) this.type;
      set
      {
      }
    }

    public string RenderDisplayInfo() => "version-seed, type=" + (object) this.type;

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      return (IEnumerable<string>) this.idForBackTracks;
    }
  }
}
