// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.Access
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class Access
  {
    public static readonly Access Unset = new Access("");
    public static readonly Access Field = new Access("field");
    public static readonly Access BackField = new Access("backfield");
    public static readonly Access Property = new Access("property");
    public static readonly Access ReadOnlyProperty = new Access("nosetter");
    public static readonly Access NoOp = new Access("noop");
    public static readonly Access None = new Access("none");
    private readonly string value;

    public static Access CamelCaseField() => Access.CamelCaseField(CamelCasePrefix.None);

    public static Access CamelCaseField(CamelCasePrefix prefix)
    {
      return new Access("field", NamingStrategy.FromString("camelcase" + (object) prefix));
    }

    public static Access LowerCaseField() => Access.LowerCaseField(LowerCasePrefix.None);

    public static Access LowerCaseField(LowerCasePrefix prefix)
    {
      return new Access("field", NamingStrategy.FromString("lowercase" + (object) prefix));
    }

    public static Access PascalCaseField(PascalCasePrefix prefix)
    {
      return new Access("field", NamingStrategy.FromString("pascalcase" + (object) prefix));
    }

    public static Access ReadOnlyPropertyThroughCamelCaseField()
    {
      return Access.ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix.None);
    }

    public static Access ReadOnlyPropertyThroughCamelCaseField(CamelCasePrefix prefix)
    {
      return new Access("nosetter", NamingStrategy.FromString("camelcase" + (object) prefix));
    }

    public static Access ReadOnlyPropertyThroughLowerCaseField()
    {
      return Access.ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix.None);
    }

    public static Access ReadOnlyPropertyThroughLowerCaseField(LowerCasePrefix prefix)
    {
      return new Access("nosetter", NamingStrategy.FromString("lowercase" + (object) prefix));
    }

    public static Access ReadOnlyPropertyThroughPascalCaseField(PascalCasePrefix prefix)
    {
      return new Access("nosetter", NamingStrategy.FromString("pascalcase" + (object) prefix));
    }

    public static Access ReadOnlyPropertyWithField(NamingStrategy namingStrategy)
    {
      return new Access("nosetter." + (object) namingStrategy);
    }

    public static Access Using(string value) => new Access(value);

    public static Access Using(Type accessorType)
    {
      return Access.Using(accessorType.AssemblyQualifiedName);
    }

    public static Access Using<T>() => Access.Using(typeof (T));

    private Access(string value) => this.value = value;

    private Access(string value, NamingStrategy strategy)
    {
      this.value = value + "." + (object) strategy;
    }

    public override bool Equals(object obj)
    {
      return (object) (obj as Access) != null ? this.Equals((Access) obj) : base.Equals(obj);
    }

    public bool Equals(Access other) => object.Equals((object) other.value, (object) this.value);

    public override int GetHashCode() => this.value == null ? 0 : this.value.GetHashCode();

    public static bool operator ==(Access x, Access y) => x.Equals(y);

    public static bool operator !=(Access x, Access y) => !(x == y);

    public override string ToString() => this.value;

    public static Access FromString(string value) => new Access(value);
  }
}
