// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionParseInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public struct MethodCallExpressionParseInfo
  {
    public MethodCallExpressionParseInfo(
      string associatedIdentifier,
      IExpressionNode source,
      MethodCallExpression parsedExpression)
      : this()
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (associatedIdentifier), associatedIdentifier);
      ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (source), source);
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (parsedExpression), parsedExpression);
      this.AssociatedIdentifier = associatedIdentifier;
      this.Source = source;
      this.ParsedExpression = parsedExpression;
    }

    public string AssociatedIdentifier { get; private set; }

    public IExpressionNode Source { get; private set; }

    public MethodCallExpression ParsedExpression { get; private set; }
  }
}
