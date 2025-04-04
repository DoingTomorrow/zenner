// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.ExpressionTransformerRegistry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Collections;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation
{
  public class ExpressionTransformerRegistry : IExpressionTranformationProvider
  {
    private readonly MultiDictionary<ExpressionType, ExpressionTransformation> _transformations = new MultiDictionary<ExpressionType, ExpressionTransformation>();
    private readonly List<ExpressionTransformation> _genericTransformations = new List<ExpressionTransformation>();

    public static ExpressionTransformerRegistry CreateDefault()
    {
      ExpressionTransformerRegistry transformerRegistry = new ExpressionTransformerRegistry();
      transformerRegistry.Register<BinaryExpression>((IExpressionTransformer<BinaryExpression>) new VBCompareStringExpressionTransformer());
      transformerRegistry.Register<MethodCallExpression>((IExpressionTransformer<MethodCallExpression>) new VBInformationIsNothingExpressionTransformer());
      transformerRegistry.Register<InvocationExpression>((IExpressionTransformer<InvocationExpression>) new InvocationOfLambdaExpressionTransformer());
      transformerRegistry.Register<MemberExpression>((IExpressionTransformer<MemberExpression>) new NullableValueTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new KeyValuePairNewExpressionTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new DictionaryEntryNewExpressionTransformer());
      transformerRegistry.Register<NewExpression>((IExpressionTransformer<NewExpression>) new TupleNewExpressionTransformer());
      return transformerRegistry;
    }

    public int RegisteredTransformerCount => this._transformations.CountValues();

    public ExpressionTransformation[] GetAllTransformations(ExpressionType expressionType)
    {
      return this._transformations[expressionType].ToArray<ExpressionTransformation>();
    }

    public IEnumerable<ExpressionTransformation> GetTransformations(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      IList<ExpressionTransformation> first;
      this._transformations.TryGetValue(expression.NodeType, out first);
      return first != null ? first.Concat<ExpressionTransformation>((IEnumerable<ExpressionTransformation>) this._genericTransformations) : (IEnumerable<ExpressionTransformation>) this._genericTransformations;
    }

    public void Register<T>(IExpressionTransformer<T> transformer) where T : Expression
    {
      ArgumentUtility.CheckNotNull<IExpressionTransformer<T>>(nameof (transformer), transformer);
      ExpressionTransformation expressionTransformation = (ExpressionTransformation) (expr => ExpressionTransformerRegistry.TransformExpression<T>(expr, transformer));
      if (transformer.SupportedExpressionTypes == null)
      {
        if (typeof (T) != typeof (Expression))
          throw new ArgumentException(string.Format("Cannot register an IExpressionTransformer<{0}> as a generic transformer. Generic transformers must implement IExpressionTransformer<Expression>.", (object) typeof (T).Name), nameof (transformer));
        this._genericTransformations.Add(expressionTransformation);
      }
      else
      {
        foreach (ExpressionType supportedExpressionType in transformer.SupportedExpressionTypes)
          this._transformations.Add(supportedExpressionType, expressionTransformation);
      }
    }

    private static Expression TransformExpression<T>(
      Expression expression,
      IExpressionTransformer<T> transformer)
      where T : Expression
    {
      T expression1;
      try
      {
        expression1 = (T) expression;
      }
      catch (InvalidCastException ex)
      {
        throw new InvalidOperationException(string.Format("A '{0}' with node type '{1}' cannot be handled by the IExpressionTransformer<{2}>. The transformer was probably registered for a wrong ExpressionType.", (object) expression.GetType().Name, (object) expression.NodeType, (object) typeof (T).Name), (Exception) ex);
      }
      return transformer.Transform(expression1);
    }
  }
}
