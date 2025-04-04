// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MethodInfoBinding
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings
{
  public class MethodInfoBinding(MethodInfo boundMember, Expression associatedExpression) : 
    MemberBinding((MemberInfo) boundMember, associatedExpression)
  {
    public override bool MatchesReadAccess(MemberInfo readMember)
    {
      return readMember == this.BoundMember || readMember is PropertyInfo propertyInfo && propertyInfo.CanRead && propertyInfo.GetGetMethod() == this.BoundMember;
    }
  }
}
