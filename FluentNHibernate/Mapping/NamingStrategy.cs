// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.NamingStrategy
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class NamingStrategy
  {
    public static readonly NamingStrategy LowerCase = new NamingStrategy("lowercase");
    public static readonly NamingStrategy LowerCaseUnderscore = new NamingStrategy("lowercase-underscore");
    public static readonly NamingStrategy PascalCase = new NamingStrategy("pascalcase");
    public static readonly NamingStrategy PascalCaseM = new NamingStrategy("pascalcase-m");
    public static readonly NamingStrategy PascalCaseMUnderscore = new NamingStrategy("pascalcase-m-underscore");
    public static readonly NamingStrategy PascalCaseUnderscore = new NamingStrategy("pascalcase-underscore");
    public static readonly NamingStrategy CamelCase = new NamingStrategy("camelcase");
    public static readonly NamingStrategy CamelCaseUnderscore = new NamingStrategy("camelcase-underscore");
    public static readonly NamingStrategy Unknown = new NamingStrategy("[unknown]");
    private readonly string strategy;

    private NamingStrategy(string strategy) => this.strategy = strategy;

    public override bool Equals(object obj)
    {
      return obj is NamingStrategy ? this.Equals((NamingStrategy) obj) : base.Equals(obj);
    }

    public bool Equals(NamingStrategy other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.strategy, (object) this.strategy);
    }

    public override int GetHashCode() => this.strategy == null ? 0 : this.strategy.GetHashCode();

    public override string ToString() => this.strategy;

    public static NamingStrategy FromString(string strategy) => new NamingStrategy(strategy);
  }
}
