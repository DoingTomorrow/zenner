// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.CollectionFetchMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class CollectionFetchMode
  {
    public static CollectionFetchMode Select = (CollectionFetchMode) new CollectionFetchMode.SelectFetchMode();
    public static CollectionFetchMode Join = (CollectionFetchMode) new CollectionFetchMode.JoinFetchMode();
    public static CollectionFetchMode Subselect = (CollectionFetchMode) new CollectionFetchMode.SubselectFetchMode();

    internal abstract HbmCollectionFetchMode ToHbm();

    private class JoinFetchMode : CollectionFetchMode
    {
      internal override HbmCollectionFetchMode ToHbm() => HbmCollectionFetchMode.Join;
    }

    private class SelectFetchMode : CollectionFetchMode
    {
      internal override HbmCollectionFetchMode ToHbm() => HbmCollectionFetchMode.Select;
    }

    private class SubselectFetchMode : CollectionFetchMode
    {
      internal override HbmCollectionFetchMode ToHbm() => HbmCollectionFetchMode.Subselect;
    }
  }
}
