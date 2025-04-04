// Decompiled with JetBrains decompiler
// Type: NHibernate.Param.AbstractExplicitParameterSpecification
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Param
{
  public abstract class AbstractExplicitParameterSpecification : 
    IPageableParameterSpecification,
    IExplicitParameterSpecification,
    IParameterSpecification
  {
    private readonly int sourceColumn;
    private readonly int sourceLine;
    private bool isSkipParameter;
    private bool isTakeParameter;
    private IPageableParameterSpecification skipParameter;

    protected AbstractExplicitParameterSpecification(int sourceLine, int sourceColumn)
    {
      this.sourceLine = sourceLine;
      this.sourceColumn = sourceColumn;
    }

    public int SourceLine => this.sourceLine;

    public int SourceColumn => this.sourceColumn;

    public IType ExpectedType { get; set; }

    public abstract string RenderDisplayInfo();

    public abstract IEnumerable<string> GetIdsForBackTrack(IMapping sessionFactory);

    public abstract void Bind(
      IDbCommand command,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session);

    public abstract void Bind(
      IDbCommand command,
      IList<Parameter> multiSqlQueryParametersList,
      int singleSqlParametersOffset,
      IList<Parameter> sqlQueryParametersList,
      QueryParameters queryParameters,
      ISessionImplementor session);

    public abstract void SetEffectiveType(QueryParameters queryParameters);

    public abstract int GetSkipValue(QueryParameters queryParameters);

    public void IsSkipParameter() => this.isSkipParameter = true;

    public void IsTakeParameterWithSkipParameter(IPageableParameterSpecification skipParameter)
    {
      this.isTakeParameter = true;
      this.skipParameter = skipParameter;
    }

    protected int GetParemeterSpan(IMapping sessionFactory)
    {
      if (sessionFactory == null)
        throw new ArgumentNullException(nameof (sessionFactory));
      if (this.ExpectedType == null)
        return 1;
      int columnSpan = this.ExpectedType.GetColumnSpan(sessionFactory);
      return columnSpan != 0 ? columnSpan : 1;
    }

    protected object GetPagingValue(object value, NHibernate.Dialect.Dialect dialect, QueryParameters queryParameters)
    {
      if (this.isTakeParameter)
      {
        int offset = 0;
        if (this.skipParameter != null)
          offset = this.skipParameter.GetSkipValue(queryParameters);
        return (object) dialect.GetLimitValue(offset, (int) value);
      }
      return this.isSkipParameter ? (object) dialect.GetOffsetValue((int) value) : value;
    }
  }
}
