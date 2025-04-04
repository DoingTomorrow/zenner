// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.FetchKind
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class FetchKind
  {
    public static FetchKind Select = (FetchKind) new FetchKind.SelectFetchMode();
    public static FetchKind Join = (FetchKind) new FetchKind.JoinFetchMode();

    internal abstract HbmFetchMode ToHbm();

    internal abstract HbmJoinFetch ToHbmJoinFetch();

    private class JoinFetchMode : FetchKind
    {
      internal override HbmFetchMode ToHbm() => HbmFetchMode.Join;

      internal override HbmJoinFetch ToHbmJoinFetch() => HbmJoinFetch.Join;
    }

    private class SelectFetchMode : FetchKind
    {
      internal override HbmFetchMode ToHbm() => HbmFetchMode.Select;

      internal override HbmJoinFetch ToHbmJoinFetch() => HbmJoinFetch.Select;
    }
  }
}
