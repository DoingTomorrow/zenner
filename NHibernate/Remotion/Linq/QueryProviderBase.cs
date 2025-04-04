// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryProviderBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq
{
  public abstract class QueryProviderBase : IQueryProvider
  {
    private static readonly MethodInfo s_genericCreateQueryMethod = ((IEnumerable<MethodInfo>) typeof (QueryProviderBase).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "CreateQuery" && m.IsGenericMethod)).Single<MethodInfo>();
    private readonly IQueryParser _queryParser;
    private readonly IQueryExecutor _executor;

    protected QueryProviderBase(IQueryParser queryParser, IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser);
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      this._queryParser = queryParser;
      this._executor = executor;
    }

    public IQueryParser QueryParser => this._queryParser;

    public IQueryExecutor Executor => this._executor;

    [Obsolete("This property has been replaced by the QueryParser property. Use QueryParser instead. (1.13.92)", true)]
    public ExpressionTreeParser ExpressionTreeParser => throw new NotImplementedException();

    public IQueryable CreateQuery(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      Type typeOfIenumerable = ReflectionUtility.GetItemTypeOfIEnumerable(expression.Type, nameof (expression));
      return (IQueryable) QueryProviderBase.s_genericCreateQueryMethod.MakeGenericMethod(typeOfIenumerable).Invoke((object) this, new object[1]
      {
        (object) expression
      });
    }

    public abstract IQueryable<T> CreateQuery<T>(Expression expression);

    public virtual IStreamedData Execute(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this.GenerateQueryModel(expression).Execute(this.Executor);
    }

    TResult IQueryProvider.Execute<TResult>(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return (TResult) this.Execute(expression).Value;
    }

    object IQueryProvider.Execute(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this.Execute(expression).Value;
    }

    public virtual QueryModel GenerateQueryModel(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return this._queryParser.GetParsedQuery(expression);
    }
  }
}
