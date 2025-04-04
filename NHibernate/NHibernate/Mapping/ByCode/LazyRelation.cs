// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.LazyRelation
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class LazyRelation
  {
    public static LazyRelation Proxy = (LazyRelation) new LazyRelation.LazyProxy();
    public static LazyRelation NoProxy = (LazyRelation) new LazyRelation.LazyNoProxy();
    public static LazyRelation NoLazy = (LazyRelation) new LazyRelation.NoLazyRelation();

    public abstract HbmLaziness ToHbm();

    private class LazyNoProxy : LazyRelation
    {
      public override HbmLaziness ToHbm() => HbmLaziness.NoProxy;
    }

    private class LazyProxy : LazyRelation
    {
      public override HbmLaziness ToHbm() => HbmLaziness.Proxy;
    }

    private class NoLazyRelation : LazyRelation
    {
      public override HbmLaziness ToHbm() => HbmLaziness.False;
    }
  }
}
