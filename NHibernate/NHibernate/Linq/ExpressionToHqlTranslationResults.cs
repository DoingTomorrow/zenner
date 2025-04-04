// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ExpressionToHqlTranslationResults
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class ExpressionToHqlTranslationResults
  {
    public HqlTreeNode Statement { get; private set; }

    public ResultTransformer ResultTransformer { get; private set; }

    public Delegate PostExecuteTransformer { get; private set; }

    public List<Action<IQuery, IDictionary<string, Tuple<object, IType>>>> AdditionalCriteria { get; private set; }

    public ExpressionToHqlTranslationResults(
      HqlTreeNode statement,
      IList<LambdaExpression> itemTransformers,
      IList<LambdaExpression> listTransformers,
      IList<LambdaExpression> postExecuteTransformers,
      List<Action<IQuery, IDictionary<string, Tuple<object, IType>>>> additionalCriteria)
    {
      this.Statement = statement;
      this.PostExecuteTransformer = ExpressionToHqlTranslationResults.MergeLambdasAndCompile(postExecuteTransformers);
      Delegate itemTransformation = ExpressionToHqlTranslationResults.MergeLambdasAndCompile(itemTransformers);
      Delegate listTransformation = ExpressionToHqlTranslationResults.MergeLambdasAndCompile(listTransformers);
      if ((object) itemTransformation != null || (object) listTransformation != null)
        this.ResultTransformer = new ResultTransformer(itemTransformation, listTransformation);
      this.AdditionalCriteria = additionalCriteria;
    }

    private static Delegate MergeLambdasAndCompile(IList<LambdaExpression> transformations)
    {
      if (transformations == null || transformations.Count == 0)
        return (Delegate) null;
      LambdaExpression lambdaExpression = transformations[0];
      for (int index = 1; index < transformations.Count; ++index)
        lambdaExpression = Expression.Lambda((Expression) Expression.Invoke((Expression) transformations[index], lambdaExpression.Body), lambdaExpression.Parameters.ToArray<ParameterExpression>());
      return lambdaExpression.Compile();
    }
  }
}
