// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.Laziness
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class Laziness
  {
    public static readonly Laziness False = new Laziness("false");
    public static readonly Laziness Proxy = new Laziness("proxy");
    public static readonly Laziness NoProxy = new Laziness("no-proxy");
    private readonly string value;

    public Laziness(string value) => this.value = value;

    public override bool Equals(object obj) => this.Equals(obj as Laziness);

    public bool Equals(Laziness other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public override string ToString() => this.value;
  }
}
