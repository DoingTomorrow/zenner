// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.PagingProvider`1
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Business.Utils
{
  public class PagingProvider<T1>
  {
    private readonly int _count;
    private readonly ISessionFactory _sessionFactory;
    private readonly Expression<Func<T1, bool>> _searchExpression;

    public PagingProvider(
      ISessionFactory nhSessionfactory,
      Expression<Func<T1, bool>> searchExpression)
    {
      this._sessionFactory = nhSessionfactory;
      this._searchExpression = searchExpression;
      using (IStatelessSession statelessSession = this._sessionFactory.OpenStatelessSession())
        this._count = LinqExtensionMethods.Query<T1>(statelessSession).Count<T1>(this._searchExpression);
    }

    public PagingProvider(ISessionFactory nhSessionfactory)
    {
      this._sessionFactory = nhSessionfactory;
      using (IStatelessSession statelessSession = this._sessionFactory.OpenStatelessSession())
        this._count = LinqExtensionMethods.Query<T1>(statelessSession).Count<T1>();
    }

    public int FetchCount() => this._count;

    public IList<T1> FetchRange(int startIndex, int pageSize)
    {
      List<T1> objList;
      using (IStatelessSession statelessSession = this._sessionFactory.OpenStatelessSession())
        objList = this._searchExpression == null ? LinqExtensionMethods.Query<T1>(statelessSession).Take<T1>(pageSize).Skip<T1>(startIndex - 1).ToList<T1>() : LinqExtensionMethods.Query<T1>(statelessSession).Where<T1>(this._searchExpression).Take<T1>(pageSize).Skip<T1>(startIndex - 1).ToList<T1>();
      return (IList<T1>) objList;
    }
  }
}
