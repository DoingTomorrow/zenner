// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.NotFound
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class NotFound
  {
    public static readonly NotFound Unset = new NotFound("");
    public static readonly NotFound Ignore = new NotFound("ignore");
    public static readonly NotFound Exception = new NotFound("exception");
    private readonly string value;

    private NotFound(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as NotFound) != null ? this.Equals((NotFound) obj) : base.Equals(obj);
    }

    public bool Equals(NotFound other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(NotFound x, NotFound y) => x.Equals(y);

    public static bool operator !=(NotFound x, NotFound y) => !(x == y);

    public override string ToString() => this.value;

    public static NotFound FromString(string value) => new NotFound(value);
  }
}
