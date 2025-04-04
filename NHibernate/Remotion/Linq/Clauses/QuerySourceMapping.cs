// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.QuerySourceMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public class QuerySourceMapping
  {
    private readonly Dictionary<IQuerySource, Expression> _lookup = new Dictionary<IQuerySource, Expression>();

    public bool ContainsMapping(IQuerySource querySource)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      return this._lookup.ContainsKey(querySource);
    }

    public void AddMapping(IQuerySource querySource, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      try
      {
        this._lookup.Add(querySource, expression);
      }
      catch (ArgumentException ex)
      {
        throw new InvalidOperationException("Query source has already been associated with an expression.");
      }
    }

    public void ReplaceMapping(IQuerySource querySource, Expression expression)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (!this.ContainsMapping(querySource))
        throw new InvalidOperationException("Query source has not been associated with an expression, cannot replace its mapping.");
      this._lookup[querySource] = expression;
    }

    public Expression GetExpression(IQuerySource querySource)
    {
      ArgumentUtility.CheckNotNull<IQuerySource>(nameof (querySource), querySource);
      try
      {
        return this._lookup[querySource];
      }
      catch (KeyNotFoundException ex)
      {
        throw new KeyNotFoundException("Query source has not been associated with an expression.");
      }
    }
  }
}
