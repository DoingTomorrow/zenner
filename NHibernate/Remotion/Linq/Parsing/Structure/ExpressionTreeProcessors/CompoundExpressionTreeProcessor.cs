// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors.CompoundExpressionTreeProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors
{
  public class CompoundExpressionTreeProcessor : IExpressionTreeProcessor
  {
    private readonly List<IExpressionTreeProcessor> _innerProcessors;

    public CompoundExpressionTreeProcessor(
      IEnumerable<IExpressionTreeProcessor> innerProcessors)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<IExpressionTreeProcessor>>(nameof (innerProcessors), innerProcessors);
      this._innerProcessors = new List<IExpressionTreeProcessor>(innerProcessors);
    }

    public IList<IExpressionTreeProcessor> InnerProcessors
    {
      get => (IList<IExpressionTreeProcessor>) this._innerProcessors;
    }

    public Expression Process(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return this._innerProcessors.Aggregate<IExpressionTreeProcessor, Expression>(expressionTree, (Func<Expression, IExpressionTreeProcessor, Expression>) ((expr, processor) => processor.Process(expr)));
    }
  }
}
