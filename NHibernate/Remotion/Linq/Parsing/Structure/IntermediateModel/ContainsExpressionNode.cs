// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ContainsExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class ContainsExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<bool>((Expression<Func<bool>>) (() => default (IQueryable<object>).Contains<object>((object) null))),
      MethodCallExpressionNodeBase.GetSupportedMethod<bool>((Expression<Func<bool>>) (() => default (IEnumerable<object>).Contains<object>((object) null)))
    };
    public static readonly NameBasedRegistrationInfo[] SupportedMethodNames = new NameBasedRegistrationInfo[1]
    {
      new NameBasedRegistrationInfo("Contains", (Func<MethodInfo, bool>) (mi =>
      {
        if (mi.DeclaringType == typeof (string) || !typeof (IEnumerable).IsAssignableFrom(mi.DeclaringType) || typeof (IDictionary).IsAssignableFrom(mi.DeclaringType))
          return false;
        if (mi.IsStatic && mi.GetParameters().Length == 2)
          return true;
        return !mi.IsStatic && mi.GetParameters().Length == 1;
      }))
    };

    public ContainsExpressionNode(MethodCallExpressionParseInfo parseInfo, Expression item)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (item), item);
      this.Item = item;
    }

    public Expression Item { get; private set; }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new ContainsResultOperator(this.Item);
    }
  }
}
