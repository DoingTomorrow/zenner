// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.Generated
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class Generated
  {
    public static readonly Generated Unset = new Generated("");
    public static readonly Generated Never = new Generated("never");
    public static readonly Generated Insert = new Generated("insert");
    public static readonly Generated Always = new Generated("always");
    private readonly string value;

    private Generated(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as Generated) != null ? this.Equals((Generated) obj) : base.Equals(obj);
    }

    public bool Equals(Generated other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Generated x, Generated y) => x.Equals(y);

    public static bool operator !=(Generated x, Generated y) => !(x == y);

    public override string ToString() => this.value;

    public static Generated FromString(string value) => new Generated(value);
  }
}
