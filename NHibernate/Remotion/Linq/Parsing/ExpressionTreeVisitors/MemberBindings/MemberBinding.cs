// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings.MemberBinding
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.MemberBindings
{
  public abstract class MemberBinding
  {
    private readonly MemberInfo _boundMember;
    private readonly Expression _associatedExpression;

    public static MemberBinding Bind(MemberInfo boundMember, Expression associatedExpression)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (boundMember), boundMember);
      ArgumentUtility.CheckNotNull<Expression>(nameof (associatedExpression), associatedExpression);
      switch (boundMember)
      {
        case MethodInfo boundMember1:
          return (MemberBinding) new MethodInfoBinding(boundMember1, associatedExpression);
        case PropertyInfo boundMember2:
          return (MemberBinding) new PropertyInfoBinding(boundMember2, associatedExpression);
        default:
          return (MemberBinding) new FieldInfoBinding((FieldInfo) boundMember, associatedExpression);
      }
    }

    public MemberInfo BoundMember => this._boundMember;

    public Expression AssociatedExpression => this._associatedExpression;

    public MemberBinding(MemberInfo boundMember, Expression associatedExpression)
    {
      ArgumentUtility.CheckNotNull<MemberInfo>(nameof (boundMember), boundMember);
      ArgumentUtility.CheckNotNull<Expression>(nameof (associatedExpression), associatedExpression);
      this._boundMember = boundMember;
      this._associatedExpression = associatedExpression;
    }

    public abstract bool MatchesReadAccess(MemberInfo member);
  }
}
