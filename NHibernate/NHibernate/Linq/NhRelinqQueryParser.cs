// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NhRelinqQueryParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ExpressionTransformers;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using Remotion.Linq.Parsing.Structure;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public static class NhRelinqQueryParser
  {
    private static readonly QueryParser QueryParser;

    static NhRelinqQueryParser()
    {
      ExpressionTransformerRegistry transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
      transformerRegistry.Register<BinaryExpression>((IExpressionTransformer<BinaryExpression>) new RemoveCharToIntConversion());
      transformerRegistry.Register<UnaryExpression>((IExpressionTransformer<UnaryExpression>) new RemoveRedundantCast());
      transformerRegistry.Register<BinaryExpression>((IExpressionTransformer<BinaryExpression>) new SimplifyCompareTransformer());
      NhRelinqQueryParser.QueryParser = new QueryParser(new ExpressionTreeParser((INodeTypeProvider) new NHibernateNodeTypeProvider(), (IExpressionTreeProcessor) ExpressionTreeParser.CreateDefaultProcessor((IExpressionTranformationProvider) transformerRegistry)));
    }

    public static QueryModel Parse(Expression expression)
    {
      return NhRelinqQueryParser.QueryParser.GetParsedQuery(expression);
    }
  }
}
