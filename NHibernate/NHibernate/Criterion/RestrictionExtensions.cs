// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.RestrictionExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Collections;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  public static class RestrictionExtensions
  {
    public static bool IsLike(this string projection, string comparison)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsLike(this string projection, string comparison, MatchMode matchMode)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsLike(
      this string projection,
      string comparison,
      MatchMode matchMode,
      char? escapeChar)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsInsensitiveLike(this string projection, string comparison)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsInsensitiveLike(
      this string projection,
      string comparison,
      MatchMode matchMode)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsIn(this object projection, object[] values)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static bool IsIn(this object projection, ICollection values)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static RestrictionExtensions.RestrictionBetweenBuilder IsBetween(
      this object projection,
      object lo)
    {
      throw new Exception("Not to be used directly - use inside QueryOver expression");
    }

    public static ICriterion ProcessIsLike(MethodCallExpression methodCallExpression)
    {
      return ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).CreateCriterion(new Func<string, object, ICriterion>(Restrictions.Like), new Func<IProjection, object, ICriterion>(Restrictions.Like), ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]));
    }

    public static ICriterion ProcessIsLikeMatchMode(MethodCallExpression methodCallExpression)
    {
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]);
      string value = (string) ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      MatchMode matchMode = (MatchMode) ExpressionProcessor.FindValue(methodCallExpression.Arguments[2]);
      return memberProjection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.Like(s, value, matchMode)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.Like(p, value, matchMode)));
    }

    public static ICriterion ProcessIsLikeMatchModeEscapeChar(
      MethodCallExpression methodCallExpression)
    {
      return (ICriterion) Restrictions.Like(ExpressionProcessor.FindMemberExpression(methodCallExpression.Arguments[0]), (string) ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]), (MatchMode) ExpressionProcessor.FindValue(methodCallExpression.Arguments[2]), (char?) ExpressionProcessor.FindValue(methodCallExpression.Arguments[3]));
    }

    public static ICriterion ProcessIsInsensitiveLike(MethodCallExpression methodCallExpression)
    {
      return ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).CreateCriterion(new Func<string, object, ICriterion>(Restrictions.InsensitiveLike), new Func<IProjection, object, ICriterion>(Restrictions.InsensitiveLike), ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]));
    }

    public static ICriterion ProcessIsInsensitiveLikeMatchMode(
      MethodCallExpression methodCallExpression)
    {
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]);
      string value = (string) ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      MatchMode matchMode = (MatchMode) ExpressionProcessor.FindValue(methodCallExpression.Arguments[2]);
      return memberProjection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.InsensitiveLike(s, value, matchMode)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.InsensitiveLike(p, value, matchMode)));
    }

    public static ICriterion ProcessIsInArray(MethodCallExpression methodCallExpression)
    {
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]);
      object[] values = (object[]) ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      return memberProjection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.In(s, values)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.In(p, values)));
    }

    public static ICriterion ProcessIsInCollection(MethodCallExpression methodCallExpression)
    {
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]);
      ICollection values = (ICollection) ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
      return memberProjection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.In(s, values)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.In(p, values)));
    }

    public static ICriterion ProcessIsBetween(MethodCallExpression methodCallExpression)
    {
      MethodCallExpression methodCallExpression1 = (MethodCallExpression) methodCallExpression.Object;
      ExpressionProcessor.ProjectionInfo memberProjection = ExpressionProcessor.FindMemberProjection(methodCallExpression1.Arguments[0]);
      object lo = ExpressionProcessor.FindValue(methodCallExpression1.Arguments[1]);
      object hi = ExpressionProcessor.FindValue(methodCallExpression.Arguments[0]);
      return memberProjection.Create<ICriterion>((Func<string, ICriterion>) (s => (ICriterion) Restrictions.Between(s, lo, hi)), (Func<IProjection, ICriterion>) (p => (ICriterion) Restrictions.Between(p, lo, hi)));
    }

    public class RestrictionBetweenBuilder
    {
      public bool And(object hi)
      {
        throw new Exception("Not to be used directly - use inside QueryOver expression");
      }
    }
  }
}
