// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.Polymorphism
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class Polymorphism
  {
    public static readonly Polymorphism Unset = new Polymorphism("");
    public static readonly Polymorphism Implicit = new Polymorphism("implicit");
    public static readonly Polymorphism Explicit = new Polymorphism("explicit");
    private readonly string value;

    private Polymorphism(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as Polymorphism) != null ? this.Equals((Polymorphism) obj) : base.Equals(obj);
    }

    public bool Equals(Polymorphism other)
    {
      return object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Polymorphism x, Polymorphism y) => x.Equals(y);

    public static bool operator !=(Polymorphism x, Polymorphism y) => !(x == y);

    public override string ToString() => this.value;

    public static Polymorphism FromString(string value) => new Polymorphism(value);
  }
}
