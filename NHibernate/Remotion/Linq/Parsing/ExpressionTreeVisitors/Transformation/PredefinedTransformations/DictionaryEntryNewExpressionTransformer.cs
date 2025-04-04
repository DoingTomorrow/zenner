// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.DictionaryEntryNewExpressionTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class DictionaryEntryNewExpressionTransformer : MemberAddingNewExpressionTransformerBase
  {
    protected override MemberInfo[] GetMembers(
      ConstructorInfo constructorInfo,
      ReadOnlyCollection<Expression> arguments)
    {
      return new MemberInfo[2]
      {
        this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Key"),
        this.GetMemberForNewExpression(constructorInfo.DeclaringType, "Value")
      };
    }

    protected override bool CanAddMembers(
      Type instantiatedType,
      ReadOnlyCollection<Expression> arguments)
    {
      return instantiatedType == typeof (DictionaryEntry) && arguments.Count == 2;
    }
  }
}
