// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.NHibernateToSQLConverter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Impl;
using NHibernate.Linq;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;

#nullable disable
namespace MSS.Business.Utils
{
  public static class NHibernateToSQLConverter
  {
    public static string GetGeneratedSql(System.Linq.IQueryable queryable, ISession session)
    {
      ISessionImplementor sessionImplementor = (ISessionImplementor) session;
      NhLinqExpression nhLinqExpression = new NhLinqExpression(queryable.Expression, (ISessionFactory) sessionImplementor.Factory);
      return new ASTQueryTranslatorFactory().CreateQueryTranslators(nhLinqExpression.Key, (IQueryExpression) nhLinqExpression, (string) null, false, sessionImplementor.EnabledFilters, sessionImplementor.Factory)[0].SQLString;
    }

    public static string GetGeneratedSql(ICriteria criteria)
    {
      CriteriaImpl criteriaImpl = (CriteriaImpl) criteria;
      SessionImpl session = (SessionImpl) criteriaImpl.Session;
      SessionFactoryImpl sessionFactory = (SessionFactoryImpl) session.SessionFactory;
      string[] implementors = sessionFactory.GetImplementors(criteriaImpl.EntityOrClassName);
      return ((object) new CriteriaLoader((IOuterJoinLoadable) sessionFactory.GetEntityPersister(implementors[0]), (ISessionFactoryImplementor) sessionFactory, criteriaImpl, implementors[0], ((AbstractSessionImpl) session).EnabledFilters).SqlString).ToString();
    }

    public static string GetGeneratedSql(IQueryOver queryOver)
    {
      return NHibernateToSQLConverter.GetGeneratedSql(queryOver.UnderlyingCriteria);
    }

    public static string GetGeneratedSql(IQuery query, ISession session)
    {
      ISessionImplementor sessionImplementor = (ISessionImplementor) session;
      string generatedSql = new ASTQueryTranslatorFactory().CreateQueryTranslators(query.QueryString, (string) null, false, sessionImplementor.EnabledFilters, sessionImplementor.Factory)[0].CollectSqlStrings[0];
      foreach (string namedParameter in query.NamedParameters)
        generatedSql = generatedSql.Replace("?", "@" + namedParameter);
      return generatedSql;
    }
  }
}
