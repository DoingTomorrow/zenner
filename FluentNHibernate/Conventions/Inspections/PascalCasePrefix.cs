// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.PascalCasePrefix
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class PascalCasePrefix : Prefix
  {
    public static readonly PascalCasePrefix M = new PascalCasePrefix("-m");
    public static readonly PascalCasePrefix Underscore = new PascalCasePrefix("-underscore");
    public static readonly PascalCasePrefix MUnderscore = new PascalCasePrefix("-m-underscore");

    protected PascalCasePrefix(string value)
      : base(value)
    {
    }
  }
}
