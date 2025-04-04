// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.MemberAddingNewExpressionTransformerBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public abstract class MemberAddingNewExpressionTransformerBase : 
    IExpressionTransformer<NewExpression>
  {
    protected abstract bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments);

    protected abstract MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments);

    public ExpressionType[] SupportedExpressionTypes
    {
      get => new ExpressionType[1]{ ExpressionType.New };
    }

    public Expression Transform(NewExpression expression)
    {
      ArgumentUtility.CheckNotNull<NewExpression>(nameof (expression), expression);
      if (expression.Members != null || !this.CanAddMembers(expression.Type, expression.Arguments))
        return (Expression) expression;
      MemberInfo[] members = this.GetMembers(expression.Constructor, expression.Arguments);
      return (Expression) Expression.New(expression.Constructor, ExpressionTreeVisitor.AdjustArgumentsForNewExpression((IList<Expression>) expression.Arguments, (IList<MemberInfo>) members), members);
    }

    protected MemberInfo GetMemberForNewExpression(Type instantiatedType, string propertyName)
    {
      return (MemberInfo) instantiatedType.GetProperty(propertyName).GetGetMethod();
    }
  }
}
