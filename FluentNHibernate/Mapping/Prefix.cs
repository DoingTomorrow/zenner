// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.Prefix
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class Prefix
  {
    public static readonly Prefix None = new Prefix("");
    public static readonly Prefix Underscore = new Prefix("-underscore");
    public static readonly Prefix m = new Prefix("-m");
    public static readonly Prefix mUnderscore = new Prefix("-m-underscore");
    private readonly string value;

    private Prefix(string value) => this.value = value;

    public string Value => this.value;
  }
}
