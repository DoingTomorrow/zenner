// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.QueryParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure
{
  public class QueryParser : IQueryParser
  {
    private readonly ExpressionTreeParser _expressionTreeParser;

    public static QueryParser CreateDefault()
    {
      ExpressionTransformerRegistry transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
      return new QueryParser(new ExpressionTreeParser((INodeTypeProvider) ExpressionTreeParser.CreateDefaultNodeTypeProvider(), (IExpressionTreeProcessor) ExpressionTreeParser.CreateDefaultProcessor((IExpressionTranformationProvider) transformerRegistry)));
    }

    public QueryParser(ExpressionTreeParser expressionTreeParser)
    {
      ArgumentUtility.CheckNotNull<ExpressionTreeParser>(nameof (expressionTreeParser), expressionTreeParser);
      this._expressionTreeParser = expressionTreeParser;
    }

    public ExpressionTreeParser ExpressionTreeParser => this._expressionTreeParser;

    public INodeTypeProvider NodeTypeProvider => this._expressionTreeParser.NodeTypeProvider;

    public IExpressionTreeProcessor Processor => this._expressionTreeParser.Processor;

    public QueryModel GetParsedQuery(Expression expressionTreeRoot)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTreeRoot), expressionTreeRoot);
      return this.ApplyAllNodes(this._expressionTreeParser.ParseTree(expressionTreeRoot), new ClauseGenerationContext(this._expressionTreeParser.NodeTypeProvider));
    }

    private QueryModel ApplyAllNodes(
      IExpressionNode node,
      ClauseGenerationContext clauseGenerationContext)
    {
      QueryModel queryModel = (QueryModel) null;
      if (node.Source != null)
        queryModel = this.ApplyAllNodes(node.Source, clauseGenerationContext);
      return node.Apply(queryModel, clauseGenerationContext);
    }
  }
}
