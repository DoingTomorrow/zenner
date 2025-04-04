// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.NotFoundMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class NotFoundMode
  {
    public static NotFoundMode Ignore = (NotFoundMode) new NotFoundMode.IgnoreNotFoundMode();
    public static NotFoundMode Exception = (NotFoundMode) new NotFoundMode.ExceptionNotFoundMode();

    public abstract HbmNotFoundMode ToHbm();

    private class ExceptionNotFoundMode : NotFoundMode
    {
      public override HbmNotFoundMode ToHbm() => HbmNotFoundMode.Exception;
    }

    private class IgnoreNotFoundMode : NotFoundMode
    {
      public override HbmNotFoundMode ToHbm() => HbmNotFoundMode.Ignore;
    }
  }
}
