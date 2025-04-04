// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Hbm2DDLKeyWords
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg
{
  public class Hbm2DDLKeyWords
  {
    private readonly string value;
    public static Hbm2DDLKeyWords None = new Hbm2DDLKeyWords("none");
    public static Hbm2DDLKeyWords Keywords = new Hbm2DDLKeyWords("keywords");
    public static Hbm2DDLKeyWords AutoQuote = new Hbm2DDLKeyWords("auto-quote");

    private Hbm2DDLKeyWords(string value) => this.value = value;

    public override string ToString() => this.value;

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (Hbm2DDLKeyWords) && this.Equals((Hbm2DDLKeyWords) obj);
    }

    public bool Equals(string other) => this.value.Equals(other);

    public bool Equals(Hbm2DDLKeyWords other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(string a, Hbm2DDLKeyWords b)
    {
      return !object.ReferenceEquals((object) null, (object) b) && b.Equals(a);
    }

    public static bool operator ==(Hbm2DDLKeyWords a, string b) => b == a;

    public static bool operator !=(Hbm2DDLKeyWords a, string b) => !(a == b);

    public static bool operator !=(string a, Hbm2DDLKeyWords b) => !(a == b);
  }
}
