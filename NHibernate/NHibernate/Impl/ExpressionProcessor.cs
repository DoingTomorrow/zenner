// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.ExpressionProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Impl
{
  public static class ExpressionProcessor
  {
    private static readonly IDictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>> _simpleExpressionCreators = (IDictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>>) new Dictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>>();
    private static readonly IDictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>> _propertyExpressionCreators;
    private static readonly IDictionary<LambdaSubqueryType, IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>> _subqueryExpressionCreatorTypes;
    private static readonly IDictionary<string, Func<MethodCallExpression, ICriterion>> _customMethodCallProcessors;
    private static readonly IDictionary<string, Func<MethodCallExpression, IProjection>> _customProjectionProcessors;

    static ExpressionProcessor()
    {
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.Equal] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Eq);
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.NotEqual] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Ne);
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.GreaterThan] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Gt);
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.GreaterThanOrEqual] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Ge);
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.LessThan] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Lt);
      ExpressionProcessor._simpleExpressionCreators[ExpressionType.LessThanOrEqual] = new Func<ExpressionProcessor.ProjectionInfo, object, ICriterion>(ExpressionProcessor.Le);
      ExpressionProcessor._propertyExpressionCreators = (IDictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>>) new Dictionary<ExpressionType, Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>>();
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.Equal] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.EqProperty), new Func<string, IProjection, ICriterion>(Restrictions.EqProperty), new Func<IProjection, string, ICriterion>(Restrictions.EqProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.EqProperty)));
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.NotEqual] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.NotEqProperty), new Func<string, IProjection, ICriterion>(Restrictions.NotEqProperty), new Func<IProjection, string, ICriterion>(Restrictions.NotEqProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.NotEqProperty)));
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.GreaterThan] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.GtProperty), new Func<string, IProjection, ICriterion>(Restrictions.GtProperty), new Func<IProjection, string, ICriterion>(Restrictions.GtProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.GtProperty)));
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.GreaterThanOrEqual] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.GeProperty), new Func<string, IProjection, ICriterion>(Restrictions.GeProperty), new Func<IProjection, string, ICriterion>(Restrictions.GeProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.GeProperty)));
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.LessThan] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.LtProperty), new Func<string, IProjection, ICriterion>(Restrictions.LtProperty), new Func<IProjection, string, ICriterion>(Restrictions.LtProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.LtProperty)));
      ExpressionProcessor._propertyExpressionCreators[ExpressionType.LessThanOrEqual] = (Func<ExpressionProcessor.ProjectionInfo, ExpressionProcessor.ProjectionInfo, ICriterion>) ((lhs, rhs) => lhs.CreateCriterion(rhs, new Func<string, string, ICriterion>(Restrictions.LeProperty), new Func<string, IProjection, ICriterion>(Restrictions.LeProperty), new Func<IProjection, string, ICriterion>(Restrictions.LeProperty), new Func<IProjection, IProjection, ICriterion>(Restrictions.LeProperty)));
      ExpressionProcessor._subqueryExpressionCreatorTypes = (IDictionary<LambdaSubqueryType, IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>>) new Dictionary<LambdaSubqueryType, IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>>();
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact] = (IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>) new Dictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>();
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All] = (IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>) new Dictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>();
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Some] = (IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>) new Dictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>>();
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.Equal] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEq);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.NotEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyNe);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.GreaterThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGt);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.GreaterThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGe);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.LessThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLt);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Exact][ExpressionType.LessThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLe);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All][ExpressionType.Equal] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyEqAll);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All][ExpressionType.GreaterThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtAll);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All][ExpressionType.GreaterThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeAll);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All][ExpressionType.LessThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtAll);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.All][ExpressionType.LessThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeAll);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Some][ExpressionType.GreaterThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGtSome);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Some][ExpressionType.GreaterThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyGeSome);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Some][ExpressionType.LessThan] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLtSome);
      ExpressionProcessor._subqueryExpressionCreatorTypes[LambdaSubqueryType.Some][ExpressionType.LessThanOrEqual] = new Func<string, DetachedCriteria, AbstractCriterion>(Subqueries.PropertyLeSome);
      ExpressionProcessor._customMethodCallProcessors = (IDictionary<string, Func<MethodCallExpression, ICriterion>>) new Dictionary<string, Func<MethodCallExpression, ICriterion>>();
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => "".IsLike("")), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsLike));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => "".IsLike("", default (MatchMode))), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsLikeMatchMode));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => "".IsLike("", default (MatchMode), new char?())), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsLikeMatchModeEscapeChar));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => "".IsInsensitiveLike("")), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsInsensitiveLike));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => "".IsInsensitiveLike("", default (MatchMode))), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsInsensitiveLikeMatchMode));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => RestrictionExtensions.IsIn((object) null, new object[]
      {
      })), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsInArray));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => ((object) null).IsIn(new List<object>())), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsInCollection));
      ExpressionProcessor.RegisterCustomMethodCall((Expression<Func<bool>>) (() => ((object) null).IsBetween((object) null).And((object) null)), new Func<MethodCallExpression, ICriterion>(RestrictionExtensions.ProcessIsBetween));
      ExpressionProcessor._customProjectionProcessors = (IDictionary<string, Func<MethodCallExpression, IProjection>>) new Dictionary<string, Func<MethodCallExpression, IProjection>>();
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().YearPart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessYearPart));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().DayPart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessDayPart));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().MonthPart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessMonthPart));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().HourPart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessHourPart));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().MinutePart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessMinutePart));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => new DateTime().SecondPart()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSecondPart));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => 0.Sqrt()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSqrt));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => 0.0.Sqrt()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSqrt));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => 0M.Sqrt()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSqrt));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => (byte) 0.Sqrt()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSqrt));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => 0L.Sqrt()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSqrt));
      ExpressionProcessor.RegisterCustomProjection<string>((Expression<Func<string>>) (() => string.Empty.Lower()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessLower));
      ExpressionProcessor.RegisterCustomProjection<string>((Expression<Func<string>>) (() => string.Empty.Upper()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessUpper));
      ExpressionProcessor.RegisterCustomProjection<string>((Expression<Func<string>>) (() => string.Empty.TrimStr()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessTrimStr));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => string.Empty.StrLength()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessStrLength));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => string.Empty.BitLength()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessBitLength));
      ExpressionProcessor.RegisterCustomProjection<string>((Expression<Func<string>>) (() => string.Empty.Substr(0, 0)), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessSubstr));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => string.Empty.CharIndex(string.Empty, 0)), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessCharIndex));
      ExpressionProcessor.RegisterCustomProjection<DBNull>((Expression<Func<DBNull>>) (() => default (DBNull).Coalesce<DBNull>(default (DBNull))), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessCoalesce));
      ExpressionProcessor.RegisterCustomProjection<int?>((Expression<Func<int?>>) (() => new int?().Coalesce<int>(0)), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessCoalesce));
      ExpressionProcessor.RegisterCustomProjection<string>((Expression<Func<string>>) (() => Projections.Concat(default (string[]))), new Func<MethodCallExpression, IProjection>(Projections.ProcessConcat));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => 0.Mod(0)), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessMod));
      ExpressionProcessor.RegisterCustomProjection<int>((Expression<Func<int>>) (() => 0.Abs()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessIntAbs));
      ExpressionProcessor.RegisterCustomProjection<double>((Expression<Func<double>>) (() => 0.0.Abs()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessDoubleAbs));
      ExpressionProcessor.RegisterCustomProjection<long>((Expression<Func<long>>) (() => 0L.Abs()), new Func<MethodCallExpression, IProjection>(ProjectionsExtensions.ProcessInt64Abs));
    }

    private static ICriterion Eq(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Eq), new Func<IProjection, object, ICriterion>(Restrictions.Eq), value);
    }

    private static ICriterion Ne(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return (ICriterion) Restrictions.Not(property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Eq), new Func<IProjection, object, ICriterion>(Restrictions.Eq), value));
    }

    private static ICriterion Gt(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Gt), new Func<IProjection, object, ICriterion>(Restrictions.Gt), value);
    }

    private static ICriterion Ge(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Ge), new Func<IProjection, object, ICriterion>(Restrictions.Ge), value);
    }

    private static ICriterion Lt(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Lt), new Func<IProjection, object, ICriterion>(Restrictions.Lt), value);
    }

    private static ICriterion Le(ExpressionProcessor.ProjectionInfo property, object value)
    {
      return property.CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Le), new Func<IProjection, object, ICriterion>(Restrictions.Le), value);
    }

    public static object FindValue(System.Linq.Expressions.Expression expression)
    {
      return System.Linq.Expressions.Expression.Lambda(expression).Compile().DynamicInvoke();
    }

    public static ExpressionProcessor.ProjectionInfo FindMemberProjection(System.Linq.Expressions.Expression expression)
    {
      if (!ExpressionProcessor.IsMemberExpression(expression))
        return ExpressionProcessor.ProjectionInfo.ForProjection(Projections.Constant(ExpressionProcessor.FindValue(expression)));
      switch (expression)
      {
        case UnaryExpression _:
          UnaryExpression unaryExpression = (UnaryExpression) expression;
          return ExpressionProcessor.IsConversion(unaryExpression.NodeType) ? ExpressionProcessor.FindMemberProjection(unaryExpression.Operand) : throw new Exception("Cannot interpret member from " + expression.ToString());
        case MethodCallExpression _:
          MethodCallExpression methodCallExpression = (MethodCallExpression) expression;
          string key = ExpressionProcessor.Signature(methodCallExpression.Method);
          if (ExpressionProcessor._customProjectionProcessors.ContainsKey(key))
            return ExpressionProcessor.ProjectionInfo.ForProjection(ExpressionProcessor._customProjectionProcessors[key](methodCallExpression));
          break;
      }
      return ExpressionProcessor.ProjectionInfo.ForProperty(ExpressionProcessor.FindMemberExpression(expression));
    }

    public static string FindMemberExpression(System.Linq.Expressions.Expression expression)
    {
      switch (expression)
      {
        case MemberExpression _:
          MemberExpression memberExpression = (MemberExpression) expression;
          if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess || memberExpression.Expression.NodeType == ExpressionType.Call)
            return ExpressionProcessor.IsNullableOfT(memberExpression.Member.DeclaringType) && memberExpression.Member.Name == "Value" ? ExpressionProcessor.FindMemberExpression(memberExpression.Expression) : ExpressionProcessor.FindMemberExpression(memberExpression.Expression) + "." + memberExpression.Member.Name;
          if (!ExpressionProcessor.IsConversion(memberExpression.Expression.NodeType))
            return memberExpression.Member.Name;
          return (ExpressionProcessor.FindMemberExpression(memberExpression.Expression) + "." + memberExpression.Member.Name).TrimStart('.');
        case UnaryExpression _:
          UnaryExpression unaryExpression = (UnaryExpression) expression;
          return ExpressionProcessor.IsConversion(unaryExpression.NodeType) ? ExpressionProcessor.FindMemberExpression(unaryExpression.Operand) : throw new Exception("Cannot interpret member from " + expression.ToString());
        case MethodCallExpression _:
          MethodCallExpression methodCallExpression = (MethodCallExpression) expression;
          if (methodCallExpression.Method.Name == "GetType")
            return ExpressionProcessor.ClassMember(methodCallExpression.Object);
          if (methodCallExpression.Method.Name == "get_Item")
            return ExpressionProcessor.FindMemberExpression(methodCallExpression.Object);
          if (methodCallExpression.Method.Name == "First")
            return ExpressionProcessor.FindMemberExpression(methodCallExpression.Arguments[0]);
          throw new Exception("Unrecognised method call in expression " + expression.ToString());
        case ParameterExpression _:
          return "";
        default:
          throw new Exception("Could not determine member from " + expression.ToString());
      }
    }

    public static string FindPropertyExpression(System.Linq.Expressions.Expression expression)
    {
      string memberExpression = ExpressionProcessor.FindMemberExpression(expression);
      int startIndex = memberExpression.LastIndexOf('.') + 1;
      return startIndex <= 0 ? memberExpression : memberExpression.Substring(startIndex);
    }

    public static DetachedCriteria FindDetachedCriteria(System.Linq.Expressions.Expression expression)
    {
      if (!(expression is MethodCallExpression methodCallExpression))
        throw new Exception("right operand should be detachedQueryInstance.As<T>() - " + expression.ToString());
      return ((QueryOver) System.Linq.Expressions.Expression.Lambda(methodCallExpression.Object).Compile().DynamicInvoke()).DetachedCriteria;
    }

    private static bool EvaluatesToNull(System.Linq.Expressions.Expression expression)
    {
      return System.Linq.Expressions.Expression.Lambda(expression).Compile().DynamicInvoke() == null;
    }

    private static Type FindMemberType(System.Linq.Expressions.Expression expression)
    {
      switch (expression)
      {
        case MemberExpression _:
          return expression.Type;
        case UnaryExpression _:
          UnaryExpression unaryExpression = (UnaryExpression) expression;
          return ExpressionProcessor.IsConversion(unaryExpression.NodeType) ? ExpressionProcessor.FindMemberType(unaryExpression.Operand) : throw new Exception("Cannot interpret member from " + expression.ToString());
        case MethodCallExpression _:
          return ((MethodCallExpression) expression).Method.ReturnType;
        default:
          throw new Exception("Could not determine member type from " + expression.ToString());
      }
    }

    private static bool IsMemberExpression(System.Linq.Expressions.Expression expression)
    {
      switch (expression)
      {
        case ParameterExpression _:
          return true;
        case MemberExpression _:
          MemberExpression memberExpression = (MemberExpression) expression;
          if (memberExpression.Expression == null)
            return false;
          return ExpressionProcessor.IsMemberExpression(memberExpression.Expression) || ExpressionProcessor.EvaluatesToNull(memberExpression.Expression);
        case UnaryExpression _:
          UnaryExpression unaryExpression = (UnaryExpression) expression;
          return ExpressionProcessor.IsConversion(unaryExpression.NodeType) ? ExpressionProcessor.IsMemberExpression(unaryExpression.Operand) : throw new Exception("Cannot interpret member from " + expression.ToString());
        case MethodCallExpression _:
          MethodCallExpression methodCallExpression = (MethodCallExpression) expression;
          string key = ExpressionProcessor.Signature(methodCallExpression.Method);
          if (ExpressionProcessor._customProjectionProcessors.ContainsKey(key))
            return true;
          if (methodCallExpression.Method.Name == "First")
            return ExpressionProcessor.IsMemberExpression(methodCallExpression.Arguments[0]) || ExpressionProcessor.EvaluatesToNull(methodCallExpression.Arguments[0]);
          if (methodCallExpression.Method.Name == "GetType" || methodCallExpression.Method.Name == "get_Item")
            return ExpressionProcessor.IsMemberExpression(methodCallExpression.Object) || ExpressionProcessor.EvaluatesToNull(methodCallExpression.Object);
          break;
      }
      return false;
    }

    private static bool IsConversion(ExpressionType expressionType)
    {
      return expressionType == ExpressionType.Convert || expressionType == ExpressionType.ConvertChecked;
    }

    private static object ConvertType(object value, Type type)
    {
      if (value == null)
        return (object) null;
      if (type.IsAssignableFrom(value.GetType()))
        return value;
      if (ExpressionProcessor.IsNullableOfT(type))
        type = Nullable.GetUnderlyingType(type);
      if (type.IsEnum)
        return Enum.ToObject(type, value);
      return type.IsPrimitive ? Convert.ChangeType(value, type) : throw new Exception("Cannot convert '" + value.ToString() + "' to " + type.ToString());
    }

    private static bool IsNullableOfT(Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
    }

    private static ICriterion ProcessSimpleExpression(BinaryExpression be)
    {
      return be.Left.NodeType == ExpressionType.Call && ((MethodCallExpression) be.Left).Method.Name == "CompareString" ? ExpressionProcessor.ProcessVisualBasicStringComparison(be) : ExpressionProcessor.ProcessSimpleExpression(be.Left, be.Right, be.NodeType);
    }

    private static ICriterion ProcessSimpleExpression(
      System.Linq.Expressions.Expression left,
      System.Linq.Expressions.Expression right,
      ExpressionType nodeType)
    {
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(left);
      Type memberType = ExpressionProcessor.FindMemberType(left);
      object obj = ExpressionProcessor.ConvertType(ExpressionProcessor.FindValue(right), memberType);
      if (obj == null)
        return ExpressionProcessor.ProcessSimpleNullExpression(memberProjection, nodeType);
      if (!ExpressionProcessor._simpleExpressionCreators.ContainsKey(nodeType))
        throw new Exception("Unhandled simple expression type: " + (object) nodeType);
      return ExpressionProcessor._simpleExpressionCreators[nodeType](memberProjection, obj);
    }

    private static ICriterion ProcessVisualBasicStringComparison(BinaryExpression be)
    {
      MethodCallExpression left = (MethodCallExpression) be.Left;
      return ExpressionProcessor.IsMemberExpression(left.Arguments[1]) ? ExpressionProcessor.ProcessMemberExpression(left.Arguments[0], left.Arguments[1], be.NodeType) : ExpressionProcessor.ProcessSimpleExpression(left.Arguments[0], left.Arguments[1], be.NodeType);
    }

    private static ICriterion ProcessSimpleNullExpression(
      ExpressionProcessor.ProjectionInfo property,
      ExpressionType expressionType)
    {
      if (expressionType == ExpressionType.Equal)
        return property.CreateCriterion(new Func<string, ICriterion>(Restrictions.IsNull), new Func<IProjection, ICriterion>(Restrictions.IsNull));
      if (expressionType == ExpressionType.NotEqual)
        return (ICriterion) Restrictions.Not(property.CreateCriterion(new Func<string, ICriterion>(Restrictions.IsNull), new Func<IProjection, ICriterion>(Restrictions.IsNull)));
      throw new Exception("Cannot supply null value to operator " + (object) expressionType);
    }

    private static ICriterion ProcessMemberExpression(BinaryExpression be)
    {
      return ExpressionProcessor.ProcessMemberExpression(be.Left, be.Right, be.NodeType);
    }

    private static ICriterion ProcessMemberExpression(
      System.Linq.Expressions.Expression left,
      System.Linq.Expressions.Expression right,
      ExpressionType nodeType)
    {
      ExpressionProcessor.ProjectionInfo memberProjection1 = ExpressionProcessor.FindMemberProjection(left);
      ExpressionProcessor.ProjectionInfo memberProjection2 = ExpressionProcessor.FindMemberProjection(right);
      if (!ExpressionProcessor._propertyExpressionCreators.ContainsKey(nodeType))
        throw new Exception("Unhandled property expression type: " + (object) nodeType);
      return ExpressionProcessor._propertyExpressionCreators[nodeType](memberProjection1, memberProjection2);
    }

    private static ICriterion ProcessAndExpression(BinaryExpression expression)
    {
      return (ICriterion) Restrictions.And(ExpressionProcessor.ProcessExpression(expression.Left), ExpressionProcessor.ProcessExpression(expression.Right));
    }

    private static ICriterion ProcessOrExpression(BinaryExpression expression)
    {
      return (ICriterion) Restrictions.Or(ExpressionProcessor.ProcessExpression(expression.Left), ExpressionProcessor.ProcessExpression(expression.Right));
    }

    private static ICriterion ProcessBinaryExpression(BinaryExpression expression)
    {
      switch (expression.NodeType)
      {
        case ExpressionType.AndAlso:
          return ExpressionProcessor.ProcessAndExpression(expression);
        case ExpressionType.Equal:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.NotEqual:
          return ExpressionProcessor.IsMemberExpression(expression.Right) ? ExpressionProcessor.ProcessMemberExpression(expression) : ExpressionProcessor.ProcessSimpleExpression(expression);
        case ExpressionType.OrElse:
          return ExpressionProcessor.ProcessOrExpression(expression);
        default:
          throw new Exception("Unhandled binary expression: " + (object) expression.NodeType + ", " + expression.ToString());
      }
    }

    private static ICriterion ProcessBooleanExpression(System.Linq.Expressions.Expression expression)
    {
      switch (expression)
      {
        case MemberExpression _:
          return (ICriterion) Restrictions.Eq(ExpressionProcessor.FindMemberExpression(expression), (object) true);
        case UnaryExpression _:
          UnaryExpression unaryExpression = (UnaryExpression) expression;
          if (unaryExpression.NodeType != ExpressionType.Not)
            throw new Exception("Cannot interpret member from " + expression.ToString());
          return ExpressionProcessor.IsMemberExpression(unaryExpression.Operand) ? (ICriterion) Restrictions.Eq(ExpressionProcessor.FindMemberExpression(unaryExpression.Operand), (object) false) : (ICriterion) Restrictions.Not(ExpressionProcessor.ProcessExpression(unaryExpression.Operand));
        case MethodCallExpression _:
          return ExpressionProcessor.ProcessCustomMethodCall((MethodCallExpression) expression);
        case TypeBinaryExpression _:
          TypeBinaryExpression binaryExpression = (TypeBinaryExpression) expression;
          return (ICriterion) Restrictions.Eq(ExpressionProcessor.ClassMember(binaryExpression.Expression), (object) binaryExpression.TypeOperand.FullName);
        default:
          throw new Exception("Could not determine member type from " + (object) expression.NodeType + ", " + expression.ToString() + ", " + (object) expression.GetType());
      }
    }

    private static string ClassMember(System.Linq.Expressions.Expression expression)
    {
      return expression.NodeType == ExpressionType.MemberAccess ? ExpressionProcessor.FindMemberExpression(expression) + ".class" : "class";
    }

    public static string Signature(MethodInfo methodInfo)
    {
      while (methodInfo.IsGenericMethod && !methodInfo.IsGenericMethodDefinition)
        methodInfo = methodInfo.GetGenericMethodDefinition();
      return methodInfo.DeclaringType.FullName + ":" + methodInfo.ToString();
    }

    private static ICriterion ProcessCustomMethodCall(MethodCallExpression methodCallExpression)
    {
      string key = ExpressionProcessor.Signature(methodCallExpression.Method);
      if (!ExpressionProcessor._customMethodCallProcessors.ContainsKey(key))
        throw new Exception("Unrecognised method call: " + key);
      return ExpressionProcessor._customMethodCallProcessors[key](methodCallExpression);
    }

    private static ICriterion ProcessExpression(System.Linq.Expressions.Expression expression)
    {
      return expression is BinaryExpression ? ExpressionProcessor.ProcessBinaryExpression((BinaryExpression) expression) : ExpressionProcessor.ProcessBooleanExpression(expression);
    }

    private static ICriterion ProcessLambdaExpression(LambdaExpression expression)
    {
      return ExpressionProcessor.ProcessExpression(expression.Body);
    }

    public static ICriterion ProcessExpression<T>(Expression<Func<T, bool>> expression)
    {
      return ExpressionProcessor.ProcessLambdaExpression((LambdaExpression) expression);
    }

    public static ICriterion ProcessExpression(Expression<Func<bool>> expression)
    {
      return ExpressionProcessor.ProcessLambdaExpression((LambdaExpression) expression);
    }

    public static Order ProcessOrder<T>(
      Expression<Func<T, object>> expression,
      Func<string, Order> orderDelegate)
    {
      string memberExpression = ExpressionProcessor.FindMemberExpression(expression.Body);
      return orderDelegate(memberExpression);
    }

    public static Order ProcessOrder(
      Expression<Func<object>> expression,
      Func<string, Order> orderDelegate)
    {
      string memberExpression = ExpressionProcessor.FindMemberExpression(expression.Body);
      return orderDelegate(memberExpression);
    }

    public static Order ProcessOrder(LambdaExpression expression, Func<string, Order> orderDelegate)
    {
      string propertyExpression = ExpressionProcessor.FindPropertyExpression(expression.Body);
      return orderDelegate(propertyExpression);
    }

    public static Order ProcessOrder(
      LambdaExpression expression,
      Func<string, Order> orderStringDelegate,
      Func<IProjection, Order> orderProjectionDelegate)
    {
      return ExpressionProcessor.FindMemberProjection(expression.Body).CreateOrder(orderStringDelegate, orderProjectionDelegate);
    }

    private static AbstractCriterion ProcessSubqueryExpression(
      LambdaSubqueryType subqueryType,
      BinaryExpression be)
    {
      string memberExpression = ExpressionProcessor.FindMemberExpression(be.Left);
      DetachedCriteria detachedCriteria = ExpressionProcessor.FindDetachedCriteria(be.Right);
      IDictionary<ExpressionType, Func<string, DetachedCriteria, AbstractCriterion>> expressionCreatorType = ExpressionProcessor._subqueryExpressionCreatorTypes[subqueryType];
      if (!expressionCreatorType.ContainsKey(be.NodeType))
        throw new Exception("Unhandled subquery expression type: " + (object) subqueryType + "," + (object) be.NodeType);
      return expressionCreatorType[be.NodeType](memberExpression, detachedCriteria);
    }

    public static AbstractCriterion ProcessSubquery<T>(
      LambdaSubqueryType subqueryType,
      Expression<Func<T, bool>> expression)
    {
      BinaryExpression body = (BinaryExpression) expression.Body;
      return ExpressionProcessor.ProcessSubqueryExpression(subqueryType, body);
    }

    public static AbstractCriterion ProcessSubquery(
      LambdaSubqueryType subqueryType,
      Expression<Func<bool>> expression)
    {
      BinaryExpression body = (BinaryExpression) expression.Body;
      return ExpressionProcessor.ProcessSubqueryExpression(subqueryType, body);
    }

    public static void RegisterCustomMethodCall(
      Expression<Func<bool>> function,
      Func<MethodCallExpression, ICriterion> functionProcessor)
    {
      string key = ExpressionProcessor.Signature(((MethodCallExpression) function.Body).Method);
      ExpressionProcessor._customMethodCallProcessors.Add(key, functionProcessor);
    }

    public static void RegisterCustomProjection<T>(
      Expression<Func<T>> function,
      Func<MethodCallExpression, IProjection> functionProcessor)
    {
      string key = ExpressionProcessor.Signature(((MethodCallExpression) function.Body).Method);
      ExpressionProcessor._customProjectionProcessors.Add(key, functionProcessor);
    }

    public class ProjectionInfo
    {
      private string _property;
      private IProjection _projection;

      protected ProjectionInfo()
      {
      }

      public static ExpressionProcessor.ProjectionInfo ForProperty(string property)
      {
        return new ExpressionProcessor.ProjectionInfo()
        {
          _property = property
        };
      }

      public static ExpressionProcessor.ProjectionInfo ForProjection(IProjection projection)
      {
        return new ExpressionProcessor.ProjectionInfo()
        {
          _projection = projection
        };
      }

      public IProjection AsProjection()
      {
        return this._projection ?? (IProjection) Projections.Property(this._property);
      }

      public ICriterion CreateCriterion(
        Func<string, ICriterion> stringFunc,
        Func<IProjection, ICriterion> projectionFunc)
      {
        return this._property == null ? projectionFunc(this._projection) : stringFunc(this._property);
      }

      public ICriterion CreateCriterion(
        Func<string, object, ICriterion> stringFunc,
        Func<IProjection, object, ICriterion> projectionFunc,
        object value)
      {
        return this._property == null ? projectionFunc(this._projection, value) : stringFunc(this._property, value);
      }

      public ICriterion CreateCriterion(
        ExpressionProcessor.ProjectionInfo rhs,
        Func<string, string, ICriterion> ssFunc,
        Func<string, IProjection, ICriterion> spFunc,
        Func<IProjection, string, ICriterion> psFunc,
        Func<IProjection, IProjection, ICriterion> ppFunc)
      {
        if (this._property != null && rhs._property != null)
          return ssFunc(this._property, rhs._property);
        if (this._property != null)
          return spFunc(this._property, rhs._projection);
        return rhs._property != null ? psFunc(this._projection, rhs._property) : ppFunc(this._projection, rhs._projection);
      }

      public T Create<T>(Func<string, T> stringFunc, Func<IProjection, T> projectionFunc)
      {
        return this._property == null ? projectionFunc(this._projection) : stringFunc(this._property);
      }

      public Order CreateOrder(
        Func<string, Order> orderStringDelegate,
        Func<IProjection, Order> orderProjectionDelegate)
      {
        return this._property == null ? orderProjectionDelegate(this._projection) : orderStringDelegate(this._property);
      }

      public string AsProperty()
      {
        if (this._property != null)
          return this._property;
        return this._projection is PropertyProjection ? ((PropertyProjection) this._projection).PropertyName : throw new Exception("Cannot determine property for " + this._projection.ToString());
      }
    }
  }
}
