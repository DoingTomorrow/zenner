// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.CollectionFilterKeyParameterSpecification
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
  public class CollectionFilterKeyParameterSpecification : IParameterSpecification
  {
    private const string CollectionFilterParameterIdTemplate = "<collfilter{0}{1}_{2}>";
    private readonly string collectionRole;
    private readonly IType keyType;
    private readonly int queryParameterPosition;

    public CollectionFilterKeyParameterSpecification(
      string collectionRole,
      IType keyType,
      int queryParameterPosition)
    {
      this.collectionRole = collectionRole;
      this.keyType = keyType;
      this.queryParameterPosition = queryParameterPosition;
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      IType keyType = this.keyType;
      object positionalParameterValue = queryParameters.PositionalParameterValues[this.queryParameterPosition];
      string backTrackId = this.GetIdsForBackTrack((IMapping) session.Factory).First<string>();
      int num = sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId).Single<int>();
      keyType.NullSafeSet(command, positionalParameterValue, num + singleSqlParametersOffset, session);
    }

    public IType ExpectedType
    {
      get => this.keyType;
      set => throw new InvalidOperationException();
    }

    public string RenderDisplayInfo() => "collection-filter-key=" + this.collectionRole;

    public IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory)
    {
      int paremeterSpan = this.keyType.GetColumnSpan(sessionFactory);
      for (int i = 0; i < paremeterSpan; ++i)
        yield return string.Format("<collfilter{0}{1}_{2}>", (object) this.collectionRole, (object) this.queryParameterPosition, (object) i);
    }

    public void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      this.Bind(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
    }

    public override bool Equals(object obj)
    {
      return base.Equals((object) (obj as CollectionFilterKeyParameterSpecification));
    }

    public bool Equals(CollectionFilterKeyParameterSpecification other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || other.queryParameterPosition == this.queryParameterPosition;
    }

    public override int GetHashCode() => this.queryParameterPosition ^ 877;
  }
}
