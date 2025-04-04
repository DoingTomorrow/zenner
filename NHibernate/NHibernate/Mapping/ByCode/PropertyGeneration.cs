// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.PropertyGeneration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class PropertyGeneration
  {
    public static PropertyGeneration Never = (PropertyGeneration) new PropertyGeneration.NeverPropertyGeneration();
    public static PropertyGeneration Insert = (PropertyGeneration) new PropertyGeneration.InsertPropertyGeneration();
    public static PropertyGeneration Always = (PropertyGeneration) new PropertyGeneration.AlwaysPropertyGeneration();

    internal abstract HbmPropertyGeneration ToHbm();

    public class AlwaysPropertyGeneration : PropertyGeneration
    {
      internal override HbmPropertyGeneration ToHbm() => HbmPropertyGeneration.Always;
    }

    public class InsertPropertyGeneration : PropertyGeneration
    {
      internal override HbmPropertyGeneration ToHbm() => HbmPropertyGeneration.Insert;
    }

    public class NeverPropertyGeneration : PropertyGeneration
    {
      internal override HbmPropertyGeneration ToHbm() => HbmPropertyGeneration.Never;
    }
  }
}
