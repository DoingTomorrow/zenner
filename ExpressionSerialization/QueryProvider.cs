// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.QueryProvider
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace ExpressionSerialization
{
  public abstract class QueryProvider : IQueryProvider
  {
    IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
    {
      return (IQueryable<S>) new Query<S>((IQueryProvider) this, expression);
    }

    IQueryable IQueryProvider.CreateQuery(Expression expression)
    {
      Type elementType = TypeResolver.GetElementType(expression.Type);
      try
      {
        return (IQueryable) Activator.CreateInstance(typeof (Query<>).MakeGenericType(elementType), (object) this, (object) expression);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    S IQueryProvider.Execute<S>(Expression expression) => (S) this.Execute(expression);

    object IQueryProvider.Execute(Expression expression) => this.Execute(expression);

    public abstract string GetQueryText(Expression expression);

    public abstract object Execute(Expression expression);
  }
}
