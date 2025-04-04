// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryableBase`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq
{
  public abstract class QueryableBase<T> : 
    IOrderedQueryable<T>,
    IQueryable<T>,
    IEnumerable<T>,
    IOrderedQueryable,
    IQueryable,
    IEnumerable
  {
    private readonly IQueryProvider _queryProvider;

    protected QueryableBase(IQueryParser queryParser, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser);
      this._queryProvider = (IQueryProvider) new DefaultQueryProvider(this.GetType().GetGenericTypeDefinition(), queryParser, executor);
      this.Expression = (Expression) Expression.Constant((object) this);
    }

    protected QueryableBase(IQueryProvider provider)
    {
      ArgumentUtility.CheckNotNull<IQueryProvider>(nameof (provider), provider);
      this._queryProvider = provider;
      this.Expression = (Expression) Expression.Constant((object) this);
    }

    protected QueryableBase(IQueryProvider provider, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQueryProvider>(nameof (provider), provider);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (!typeof (IEnumerable<T>).IsAssignableFrom(expression.Type))
        throw new ArgumentTypeException(nameof (expression), typeof (IEnumerable<T>), expression.Type);
      this._queryProvider = provider;
      this.Expression = expression;
    }

    public Expression Expression { get; private set; }

    public IQueryProvider Provider => this._queryProvider;

    public Type ElementType => typeof (T);

    public IEnumerator<T> GetEnumerator()
    {
      return this._queryProvider.Execute<IEnumerable<T>>(this.Expression).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this._queryProvider.Execute<IEnumerable>(this.Expression).GetEnumerator();
    }
  }
}
