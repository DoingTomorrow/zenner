// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.MappingExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public static class MappingExtensions
  {
    public static EntityMode ToEntityMode(this HbmTuplizerEntitymode source)
    {
      switch (source)
      {
        case HbmTuplizerEntitymode.Poco:
          return EntityMode.Poco;
        case HbmTuplizerEntitymode.Xml:
          return EntityMode.Xml;
        case HbmTuplizerEntitymode.DynamicMap:
          return EntityMode.Map;
        default:
          throw new ArgumentOutOfRangeException(nameof (source));
      }
    }
  }
}
