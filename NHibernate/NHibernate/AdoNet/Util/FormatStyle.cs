// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.Util.FormatStyle
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.AdoNet.Util
{
  public class FormatStyle
  {
    public static readonly FormatStyle Basic = new FormatStyle("basic", (IFormatter) new BasicFormatter());
    public static readonly FormatStyle Ddl = new FormatStyle("ddl", (IFormatter) new DdlFormatter());
    public static readonly FormatStyle None = new FormatStyle("none", (IFormatter) new FormatStyle.NoFormatImpl());

    private FormatStyle(string name, IFormatter formatter)
    {
      this.Name = name;
      this.Formatter = formatter;
    }

    public string Name { get; private set; }

    public IFormatter Formatter { get; private set; }

    public override bool Equals(object obj) => this.Equals(obj as FormatStyle);

    public bool Equals(FormatStyle other)
    {
      if (other == null)
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.Name, (object) this.Name);
    }

    public override int GetHashCode() => this.Name == null ? 0 : this.Name.GetHashCode();

    private class NoFormatImpl : IFormatter
    {
      public string Format(string source) => source;
    }
  }
}
