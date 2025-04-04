// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors.TransformingExpressionTreeProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors
{
  public class TransformingExpressionTreeProcessor : IExpressionTreeProcessor
  {
    private readonly IExpressionTranformationProvider _provider;

    public TransformingExpressionTreeProcessor(IExpressionTranformationProvider provider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (provider), provider);
      this._provider = provider;
    }

    public IExpressionTranformationProvider Provider => this._provider;

    public Expression Process(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return TransformingExpressionTreeVisitor.Transform(expressionTree, this._provider);
    }
  }
}
