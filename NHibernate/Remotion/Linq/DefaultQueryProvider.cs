// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.DefaultQueryProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq
{
  public class DefaultQueryProvider : QueryProviderBase
  {
    private readonly Type _queryableType;

    public DefaultQueryProvider(
      Type queryableType,
      IQueryParser queryParser,
      IQueryExecutor executor)
      : base(ArgumentUtility.CheckNotNull<IQueryParser>(nameof (queryParser), queryParser), ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor))
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (queryableType), queryableType);
      this.CheckQueryableType(queryableType);
      this._queryableType = queryableType;
    }

    private void CheckQueryableType(Type queryableType)
    {
      ArgumentUtility.CheckTypeIsAssignableFrom(nameof (queryableType), queryableType, typeof (IQueryable));
      int num = queryableType.IsGenericTypeDefinition ? queryableType.GetGenericArguments().Length : throw new ArgumentTypeException(string.Format("Expected the generic type definition of an implementation of IQueryable<T>, but was '{0}'.", (object) queryableType.FullName), nameof (queryableType), typeof (IQueryable<>), queryableType);
      if (num != 1)
        throw new ArgumentTypeException(string.Format("Expected the generic type definition of an implementation of IQueryable<T> with exactly one type argument, but found {0} arguments.", (object) num), nameof (queryableType), typeof (IQueryable<>), queryableType);
    }

    public Type QueryableType => this._queryableType;

    public override IQueryable<T> CreateQuery<T>(Expression expression)
    {
      return (IQueryable<T>) Activator.CreateInstance(this.QueryableType.MakeGenericType(typeof (T)), (object) this, (object) expression);
    }
  }
}
