// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.Cascade
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class Cascade
  {
    public static readonly Cascade Unset = new Cascade("");
    public static readonly Cascade All = new Cascade("all");
    public static readonly Cascade AllDeleteOrphan = new Cascade("all-delete-orphan");
    public static readonly Cascade None = new Cascade("none");
    public static readonly Cascade SaveUpdate = new Cascade("save-update");
    public static readonly Cascade Delete = new Cascade("delete");
    public static readonly Cascade Merge = new Cascade("merge");
    private readonly string value;

    private Cascade(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as Cascade) != null ? this.Equals((Cascade) obj) : base.Equals(obj);
    }

    public bool Equals(Cascade other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Cascade x, Cascade y) => x.Equals(y);

    public static bool operator !=(Cascade x, Cascade y) => !(x == y);

    public override string ToString() => this.value;

    public static Cascade FromString(string value) => new Cascade(value);
  }
}
