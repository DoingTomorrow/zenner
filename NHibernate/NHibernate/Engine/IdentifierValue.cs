// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.IdentifierValue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Engine
{
  public class IdentifierValue
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (IdentifierValue));
    private readonly object value;
    public static readonly IdentifierValue SaveAny = (IdentifierValue) new IdentifierValue.SaveAnyClass();
    public static readonly IdentifierValue SaveNone = (IdentifierValue) new IdentifierValue.SaveNoneClass();
    public static readonly IdentifierValue SaveNull = (IdentifierValue) new IdentifierValue.SaveNullClass();
    public static readonly IdentifierValue Undefined = (IdentifierValue) new IdentifierValue.UndefinedClass();

    protected IdentifierValue() => this.value = (object) null;

    public IdentifierValue(object value) => this.value = value;

    public virtual bool? IsUnsaved(object id)
    {
      if (IdentifierValue.log.IsDebugEnabled)
        IdentifierValue.log.Debug((object) ("unsaved-value: " + this.value));
      return new bool?(id == null || id.Equals(this.value));
    }

    public virtual object GetDefaultValue(object currentValue) => this.value;

    private class SaveAnyClass : IdentifierValue
    {
      public override bool? IsUnsaved(object id)
      {
        IdentifierValue.log.Debug((object) "unsaved-value strategy ANY");
        return new bool?(true);
      }

      public override object GetDefaultValue(object currentValue) => currentValue;
    }

    private class SaveNoneClass : IdentifierValue
    {
      public override bool? IsUnsaved(object id)
      {
        IdentifierValue.log.Debug((object) "unsaved-value strategy NONE");
        return new bool?(false);
      }

      public override object GetDefaultValue(object currentValue) => currentValue;
    }

    private class SaveNullClass : IdentifierValue
    {
      public override bool? IsUnsaved(object id)
      {
        IdentifierValue.log.Debug((object) "unsaved-value strategy NULL");
        return new bool?(id == null);
      }

      public override object GetDefaultValue(object currentValue) => (object) null;
    }

    public class UndefinedClass : IdentifierValue
    {
      public override bool? IsUnsaved(object id)
      {
        IdentifierValue.log.Debug((object) "id unsaved-value strategy UNDEFINED");
        return new bool?();
      }

      public override object GetDefaultValue(object currentValue) => (object) null;
    }
  }
}
