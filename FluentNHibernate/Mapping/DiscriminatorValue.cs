// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.DiscriminatorValue
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class DiscriminatorValue
  {
    public static readonly DiscriminatorValue Null = new DiscriminatorValue("null");
    public static readonly DiscriminatorValue NotNull = new DiscriminatorValue("not null");
    private readonly string outputValue;

    private DiscriminatorValue(string outputValue) => this.outputValue = outputValue;

    public override string ToString() => this.outputValue;
  }
}
