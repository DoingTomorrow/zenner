// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.OptimisticLock
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class OptimisticLock
  {
    public static readonly OptimisticLock Unset = new OptimisticLock("");
    public static readonly OptimisticLock None = new OptimisticLock("none");
    public static readonly OptimisticLock Version = new OptimisticLock("version");
    public static readonly OptimisticLock Dirty = new OptimisticLock("dirty");
    public static readonly OptimisticLock All = new OptimisticLock("all");
    private readonly string value;

    private OptimisticLock(string value) => this.value = value;

    public override bool Equals(object obj)
    {
      return (object) (obj as OptimisticLock) != null ? this.Equals((OptimisticLock) obj) : base.Equals(obj);
    }

    public bool Equals(OptimisticLock other)
    {
      return object.Equals((object) other.value, (object) this.value);
    }

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(OptimisticLock x, OptimisticLock y) => x.Equals(y);

    public static bool operator !=(OptimisticLock x, OptimisticLock y) => !(x == y);

    public override string ToString() => this.value;

    public static OptimisticLock FromString(string value) => new OptimisticLock(value);
  }
}
