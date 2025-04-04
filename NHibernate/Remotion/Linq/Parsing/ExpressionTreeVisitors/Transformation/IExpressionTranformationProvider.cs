// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.IExpressionTranformationProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation
{
  public interface IExpressionTranformationProvider
  {
    IEnumerable<ExpressionTransformation> GetTransformations(Expression expression);
  }
}
