// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.Fetch
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class Fetch
  {
    public static readonly Fetch Unset = new Fetch("");
    public static readonly Fetch Select = new Fetch("select");
    public static readonly Fetch Join = new Fetch("join");
    public static readonly Fetch Subselect = new Fetch("subselect");
    private readonly string value;

    private Fetch(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as Fetch) != null ? this.Equals((Fetch) obj) : base.Equals(obj);
    }

    public bool Equals(Fetch other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Fetch x, Fetch y) => x.Equals(y);

    public static bool operator !=(Fetch x, Fetch y) => !(x == y);

    public override string ToString() => this.value;

    public static Fetch FromString(string value) => new Fetch(value);
  }
}
