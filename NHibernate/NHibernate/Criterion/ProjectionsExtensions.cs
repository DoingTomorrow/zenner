// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.ProjectionsExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Type;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  public static class ProjectionsExtensions
  {
    public static IProjection WithAlias(this IProjection projection, Expression<Func<object>> alias)
    {
      string propertyExpression = ExpressionProcessor.FindPropertyExpression(alias.Body);
      return Projections.Alias(projection, propertyExpression);
    }

    public static int YearPart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessYearPart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("year", (IType) NHibernateUtil.Int32, projection);
    }

    public static int DayPart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessDayPart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("day", (IType) NHibernateUtil.Int32, projection);
    }

    public static int MonthPart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessMonthPart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("month", (IType) NHibernateUtil.Int32, projection);
    }

    public static int HourPart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessHourPart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("hour", (IType) NHibernateUtil.Int32, projection);
    }

    public static int MinutePart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessMinutePart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("minute", (IType) NHibernateUtil.Int32, projection);
    }

    public static int SecondPart(this DateTime dateTimeProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessSecondPart(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("second", (IType) NHibernateUtil.Int32, projection);
    }

    public static double Sqrt(this double numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static double Sqrt(this int numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static double Sqrt(this long numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static double Sqrt(this Decimal numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static double Sqrt(this byte numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessSqrt(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("sqrt", (IType) NHibernateUtil.Double, projection);
    }

    public static string Lower(this string stringProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessLower(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("lower", (IType) NHibernateUtil.String, projection);
    }

    public static string Upper(this string stringProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessUpper(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("upper", (IType) NHibernateUtil.String, projection);
    }

    public static int Abs(this int numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessIntAbs(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("abs", (IType) NHibernateUtil.Int32, projection);
    }

    public static long Abs(this long numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessInt64Abs(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("abs", (IType) NHibernateUtil.Int64, projection);
    }

    public static double Abs(this double numericProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessDoubleAbs(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("abs", (IType) NHibernateUtil.Double, projection);
    }

    public static string TrimStr(this string stringProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessTrimStr(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("trim", (IType) NHibernateUtil.String, projection);
    }

    public static int StrLength(this string stringProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessStrLength(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("length", (IType) NHibernateUtil.String, projection);
    }

    public static int BitLength(this string stringProperty)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessBitLength(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      return Projections.SqlFunction("bit_length", (IType) NHibernateUtil.String, projection);
    }

    public static string Substr(this string stringProperty, int startIndex, int length)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessSubstr(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      object obj1 = ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      object obj2 = ExpressionProcessor.FindValue(methodCallExpression.Arguments[2]);
      return Projections.SqlFunction("substring", (IType) NHibernateUtil.String, projection, Projections.Constant(obj1), Projections.Constant(obj2));
    }

    public static int CharIndex(this string stringProperty, string theChar, int startLocation)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessCharIndex(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      object obj1 = ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      object obj2 = ExpressionProcessor.FindValue(methodCallExpression.Arguments[2]);
      return Projections.SqlFunction("locate", (IType) NHibernateUtil.String, Projections.Constant(obj1), projection, Projections.Constant(obj2));
    }

    public static T Coalesce<T>(this T objectProperty, T replaceValueIfIsNull)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static T? Coalesce<T>(this T? objectProperty, T replaceValueIfIsNull) where T : struct
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessCoalesce(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      object obj = ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      return Projections.SqlFunction("coalesce", NHibernateUtil.Object, projection, Projections.Constant(obj));
    }

    public static int Mod(this int numericProperty, int divisor)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    internal static IProjection ProcessMod(MethodCallExpression methodCallExpression)
    {
      IProjection projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
      object obj = ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      return Projections.SqlFunction("mod", (IType) NHibernateUtil.Int32, projection, Projections.Constant(obj));
    }
  }
}
