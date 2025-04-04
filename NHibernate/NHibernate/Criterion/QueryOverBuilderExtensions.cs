// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.QueryOverBuilderExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion.Lambda;

#nullable disable
namespace NHibernate.Criterion
{
  public static class QueryOverBuilderExtensions
  {
    public static QueryOver<TRoot, TSubType> Default<TRoot, TSubType>(
      this QueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Default;
    }

    public static QueryOver<TRoot, TSubType> Eager<TRoot, TSubType>(
      this QueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Eager;
    }

    public static QueryOver<TRoot, TSubType> Lazy<TRoot, TSubType>(
      this QueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Lazy;
    }

    public static IQueryOver<TRoot, TSubType> Default<TRoot, TSubType>(
      this IQueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Default;
    }

    public static IQueryOver<TRoot, TSubType> Eager<TRoot, TSubType>(
      this IQueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Eager;
    }

    public static IQueryOver<TRoot, TSubType> Lazy<TRoot, TSubType>(
      this IQueryOverFetchBuilder<TRoot, TSubType> builder)
    {
      return builder.Lazy;
    }

    public static QueryOver<TRoot, TSubType> Force<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Force;
    }

    public static QueryOver<TRoot, TSubType> None<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.None;
    }

    public static QueryOver<TRoot, TSubType> Read<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Read;
    }

    public static QueryOver<TRoot, TSubType> Upgrade<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Upgrade;
    }

    public static QueryOver<TRoot, TSubType> UpgradeNoWait<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.UpgradeNoWait;
    }

    public static QueryOver<TRoot, TSubType> Write<TRoot, TSubType>(
      this QueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Write;
    }

    public static IQueryOver<TRoot, TSubType> Force<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Force;
    }

    public static IQueryOver<TRoot, TSubType> None<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.None;
    }

    public static IQueryOver<TRoot, TSubType> Read<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Read;
    }

    public static IQueryOver<TRoot, TSubType> Upgrade<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Upgrade;
    }

    public static IQueryOver<TRoot, TSubType> UpgradeNoWait<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.UpgradeNoWait;
    }

    public static IQueryOver<TRoot, TSubType> Write<TRoot, TSubType>(
      this IQueryOverLockBuilder<TRoot, TSubType> builder)
    {
      return builder.Write;
    }

    public static QueryOver<TRoot, TSubType> Asc<TRoot, TSubType>(
      this QueryOverOrderBuilder<TRoot, TSubType> builder)
    {
      return builder.Asc;
    }

    public static QueryOver<TRoot, TSubType> Desc<TRoot, TSubType>(
      this QueryOverOrderBuilder<TRoot, TSubType> builder)
    {
      return builder.Desc;
    }

    public static IQueryOver<TRoot, TSubType> Asc<TRoot, TSubType>(
      this IQueryOverOrderBuilder<TRoot, TSubType> builder)
    {
      return builder.Asc;
    }

    public static IQueryOver<TRoot, TSubType> Desc<TRoot, TSubType>(
      this IQueryOverOrderBuilder<TRoot, TSubType> builder)
    {
      return builder.Desc;
    }

    public static QueryOver<TRoot, TSubType> IsEmpty<TRoot, TSubType>(
      this QueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsEmpty;
    }

    public static QueryOver<TRoot, TSubType> IsNotEmpty<TRoot, TSubType>(
      this QueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNotEmpty;
    }

    public static QueryOver<TRoot, TSubType> IsNotNull<TRoot, TSubType>(
      this QueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNotNull;
    }

    public static QueryOver<TRoot, TSubType> IsNull<TRoot, TSubType>(
      this QueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNull;
    }

    public static IQueryOver<TRoot, TSubType> IsEmpty<TRoot, TSubType>(
      this IQueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsEmpty;
    }

    public static IQueryOver<TRoot, TSubType> IsNotEmpty<TRoot, TSubType>(
      this IQueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNotEmpty;
    }

    public static IQueryOver<TRoot, TSubType> IsNotNull<TRoot, TSubType>(
      this IQueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNotNull;
    }

    public static IQueryOver<TRoot, TSubType> IsNull<TRoot, TSubType>(
      this IQueryOverRestrictionBuilder<TRoot, TSubType> builder)
    {
      return builder.IsNull;
    }
  }
}
