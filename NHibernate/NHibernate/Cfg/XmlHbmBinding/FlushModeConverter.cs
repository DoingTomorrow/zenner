// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.FlushModeConverter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  internal static class FlushModeConverter
  {
    public static FlushMode GetFlushMode(HbmQuery querySchema)
    {
      return FlushModeConverter.GetFlushMode(querySchema.flushmodeSpecified, querySchema.flushmode);
    }

    public static FlushMode GetFlushMode(HbmSqlQuery querySchema)
    {
      return FlushModeConverter.GetFlushMode(querySchema.flushmodeSpecified, querySchema.flushmode);
    }

    private static FlushMode GetFlushMode(bool flushModeSpecified, HbmFlushMode flushMode)
    {
      if (!flushModeSpecified)
        return FlushMode.Unspecified;
      switch (flushMode)
      {
        case HbmFlushMode.Auto:
          return FlushMode.Auto;
        case HbmFlushMode.Never:
          return FlushMode.Never;
        case HbmFlushMode.Always:
          return FlushMode.Always;
        default:
          throw new ArgumentOutOfRangeException(nameof (flushMode));
      }
    }
  }
}
