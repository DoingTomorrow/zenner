// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.OnDelete
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class OnDelete
  {
    public static readonly OnDelete Unset = new OnDelete("");
    public static readonly OnDelete Cascade = new OnDelete("cascade");
    public static readonly OnDelete NoAction = new OnDelete("noaction");
    private readonly string value;

    private OnDelete(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as OnDelete) != null ? this.Equals((OnDelete) obj) : base.Equals(obj);
    }

    public bool Equals(OnDelete other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(OnDelete x, OnDelete y) => x.Equals(y);

    public static bool operator !=(OnDelete x, OnDelete y) => !(x == y);

    public override string ToString() => this.value;

    public static OnDelete FromString(string value) => new OnDelete(value);
  }
}
