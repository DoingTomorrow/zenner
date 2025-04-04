// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.Query`1
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace ExpressionSerialization
{
  public class Query<T> : 
    IQueryable<T>,
    IEnumerable<T>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable<T>,
    IOrderedQueryable
  {
    private IQueryProvider provider;
    private Expression expression;

    public Query()
      : this(((IEnumerable<T>) new T[1]).AsQueryable<T>().Provider)
    {
    }

    public Query(IQueryProvider provider)
    {
      this.provider = provider;
      this.expression = (Expression) Expression.Constant((object) this);
    }

    public Query(IQueryProvider provider, Expression expression)
    {
      if (provider == null)
        throw new ArgumentNullException(nameof (provider));
      if (expression == null)
        throw new ArgumentNullException(nameof (expression));
      if (!typeof (IQueryable<T>).IsAssignableFrom(expression.Type) && !typeof (IEnumerable<T>).IsAssignableFrom(expression.Type))
        throw new ArgumentOutOfRangeException(nameof (expression));
      this.provider = provider;
      this.expression = expression;
    }

    Expression IQueryable.Expression => this.expression;

    Type IQueryable.ElementType => typeof (T);

    IQueryProvider IQueryable.Provider => this.provider;

    public IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>) this.provider.Execute(this.expression)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable) this.provider.Execute(this.expression)).GetEnumerator();
    }

    public override string ToString() => this.GetType().FullName;
  }
}
