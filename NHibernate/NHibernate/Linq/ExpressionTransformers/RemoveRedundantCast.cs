// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ExpressionTransformers.RemoveRedundantCast
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.ExpressionTransformers
{
  public class RemoveRedundantCast : IExpressionTransformer<UnaryExpression>
  {
    private static readonly ExpressionType[] _supportedExpressionTypes = new ExpressionType[3]
    {
      ExpressionType.TypeAs,
      ExpressionType.Convert,
      ExpressionType.ConvertChecked
    };

    public Expression Transform(UnaryExpression expression)
    {
      return expression.Type != typeof (object) && expression.Type.IsAssignableFrom(expression.Operand.Type) && expression.Method == null && !expression.IsLiftedToNull ? expression.Operand : (Expression) expression;
    }

    public ExpressionType[] SupportedExpressionTypes
    {
      get => RemoveRedundantCast._supportedExpressionTypes;
    }
  }
}
