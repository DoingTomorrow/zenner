// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.Include
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class Include
  {
    public static readonly Include Unset = new Include("");
    public static readonly Include All = new Include("all");
    public static readonly Include NonLazy = new Include("non-lazy");
    private readonly string value;

    public static Include Custom(string value) => new Include(value);

    private Include(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as Include) != null ? this.Equals((Include) obj) : base.Equals(obj);
    }

    public bool Equals(Include other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Include x, Include y) => x.Equals(y);

    public static bool operator !=(Include x, Include y) => !(x == y);

    public override string ToString() => this.value;

    public static Include FromString(string value) => new Include(value);
  }
}
