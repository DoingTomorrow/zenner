// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.VersionValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Id;
using System;

#nullable disable
namespace NHibernate.Engine
{
  public class VersionValue
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (VersionValue));
    private readonly object value;
    public static VersionValue VersionSaveNull = (VersionValue) new VersionValue.VersionSaveNullClass();
    public static VersionValue VersionUndefined = (VersionValue) new VersionValue.VersionUndefinedClass();
    public static VersionValue VersionNegative = (VersionValue) new VersionValue.VersionNegativeClass();

    protected VersionValue() => this.value = (object) null;

    public VersionValue(object value) => this.value = value;

    public virtual bool? IsUnsaved(object version)
    {
      if (VersionValue.log.IsDebugEnabled)
        VersionValue.log.Debug((object) ("unsaved-value: " + this.value));
      return new bool?(version == null || version.Equals(this.value));
    }

    public virtual object GetDefaultValue(object currentValue) => this.value;

    private class VersionSaveNullClass : VersionValue
    {
      public override bool? IsUnsaved(object version)
      {
        VersionValue.log.Debug((object) "version unsaved-value strategy NULL");
        return new bool?(version == null);
      }

      public override object GetDefaultValue(object currentValue) => (object) null;
    }

    private class VersionUndefinedClass : VersionValue
    {
      public override bool? IsUnsaved(object version)
      {
        VersionValue.log.Debug((object) "version unsaved-value strategy UNDEFINED");
        return version == null ? new bool?(true) : new bool?();
      }

      public override object GetDefaultValue(object currentValue) => currentValue;
    }

    private class VersionNegativeClass : VersionValue
    {
      public override bool? IsUnsaved(object version)
      {
        VersionValue.log.Debug((object) "version unsaved-value strategy NEGATIVE");
        switch (version)
        {
          case short _:
          case int _:
          case long _:
            return new bool?(Convert.ToInt64(version) < 0L);
          default:
            throw new MappingException("unsaved-value strategy NEGATIVE may only be used with short, int and long types");
        }
      }

      public override object GetDefaultValue(object currentValue)
      {
        return IdentifierGeneratorFactory.CreateNumber(-1L, currentValue.GetType());
      }
    }
  }
}
