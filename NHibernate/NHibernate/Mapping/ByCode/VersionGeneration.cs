// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.VersionGeneration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class VersionGeneration
  {
    public static VersionGeneration Never = (VersionGeneration) new VersionGeneration.NeverGeneration();
    public static VersionGeneration Always = (VersionGeneration) new VersionGeneration.AlwaysGeneration();

    public abstract HbmVersionGeneration ToHbm();

    private class AlwaysGeneration : VersionGeneration
    {
      public override HbmVersionGeneration ToHbm() => HbmVersionGeneration.Always;
    }

    private class NeverGeneration : VersionGeneration
    {
      public override HbmVersionGeneration ToHbm() => HbmVersionGeneration.Never;
    }
  }
}
