// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.SchemaAutoAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg
{
  public class SchemaAutoAction
  {
    private readonly string value;
    public static SchemaAutoAction Recreate = new SchemaAutoAction("create-drop");
    public static SchemaAutoAction Create = new SchemaAutoAction("create");
    public static SchemaAutoAction Update = new SchemaAutoAction("update");
    public static SchemaAutoAction Validate = new SchemaAutoAction("validate");

    private SchemaAutoAction(string value) => this.value = value;

    public override string ToString() => this.value;

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (SchemaAutoAction) && this.Equals((SchemaAutoAction) obj);
    }

    public bool Equals(string other) => this.value.Equals(other);

    public bool Equals(SchemaAutoAction other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(string a, SchemaAutoAction b)
    {
      return !object.ReferenceEquals((object) null, (object) b) && b.Equals(a);
    }

    public static bool operator ==(SchemaAutoAction a, string b) => b == a;

    public static bool operator !=(SchemaAutoAction a, string b) => !(a == b);

    public static bool operator !=(string a, SchemaAutoAction b) => !(a == b);
  }
}
