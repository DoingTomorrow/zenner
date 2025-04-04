// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.VBInformationIsNothingExpressionTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class VBInformationIsNothingExpressionTransformer : 
    IExpressionTransformer<MethodCallExpression>
  {
    private const string c_vbInformationClassName = "Microsoft.VisualBasic.Information";
    private const string c_vbIsNothingMethodName = "IsNothing";

    public ExpressionType[] SupportedExpressionTypes
    {
      get => new ExpressionType[1]{ ExpressionType.Call };
    }

    public Expression Transform(MethodCallExpression expression)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expression), expression);
      return this.IsVBIsNothing(expression.Method) ? (Expression) Expression.Equal(expression.Arguments[0], (Expression) Expression.Constant((object) null)) : (Expression) expression;
    }

    private bool IsVBIsNothing(MethodInfo operatorMethod)
    {
      return operatorMethod.DeclaringType.FullName == "Microsoft.VisualBasic.Information" && operatorMethod.Name == "IsNothing";
    }
  }
}
