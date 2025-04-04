// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.TupleNewExpressionTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class TupleNewExpressionTransformer : MemberAddingNewExpressionTransformerBase
  {
    protected override bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments)
    {
      return instantiatedType.Namespace == "System" && instantiatedType.Name.StartsWith("Tuple`");
    }

    protected override MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments)
    {
      return arguments.Select<Expression, MemberInfo>((Func<Expression, int, MemberInfo>) ((expr, i) => this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Item" + (object) (i + 1)))).ToArray<MemberInfo>();
    }
  }
}
