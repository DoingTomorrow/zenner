// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.SchemaAction
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class SchemaAction
  {
    public static readonly SchemaAction Unset = new SchemaAction("");
    public static readonly SchemaAction Drop = new SchemaAction("drop");
    public static readonly SchemaAction Export = new SchemaAction("export");
    public static readonly SchemaAction None = new SchemaAction("none");
    public static readonly SchemaAction Update = new SchemaAction("update");
    public static readonly SchemaAction Validate = new SchemaAction("validate");
    public static readonly SchemaAction All = new SchemaAction("all");
    private readonly string value;

    private SchemaAction(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as SchemaAction) != null ? this.Equals((SchemaAction) obj) : base.Equals(obj);
    }

    public bool Equals(SchemaAction other)
    {
      return object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(SchemaAction x, SchemaAction y) => x.Equals(y);

    public static bool operator !=(SchemaAction x, SchemaAction y) => !(x == y);

    public override string ToString() => this.value;

    public static SchemaAction FromString(string value) => new SchemaAction(value);

    public static SchemaAction Custom(string customValue) => new SchemaAction(customValue);
  }
}
