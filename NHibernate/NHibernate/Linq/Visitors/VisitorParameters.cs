// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.VisitorParameters
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Param;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class VisitorParameters
  {
    public ISessionFactoryImplementor SessionFactory { get; private set; }

    public IDictionary<ConstantExpression, NamedParameter> ConstantToParameterMap { get; private set; }

    public List<NamedParameterDescriptor> RequiredHqlParameters { get; private set; }

    public QuerySourceNamer QuerySourceNamer { get; set; }

    public VisitorParameters(
      ISessionFactoryImplementor sessionFactory,
      IDictionary<ConstantExpression, NamedParameter> constantToParameterMap,
      List<NamedParameterDescriptor> requiredHqlParameters,
      QuerySourceNamer querySourceNamer)
    {
      this.SessionFactory = sessionFactory;
      this.ConstantToParameterMap = constantToParameterMap;
      this.RequiredHqlParameters = requiredHqlParameters;
      this.QuerySourceNamer = querySourceNamer;
    }
  }
}
