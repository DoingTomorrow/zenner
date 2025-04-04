// Decompiled with JetBrains decompiler
// Type: M8_Handler.Temperature
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

#nullable disable
namespace M8_Handler
{
  public class Temperature
  {
    public double Environment { get; set; }

    public double Radiator { get; set; }

    public double Remote { get; set; }

    public override string ToString()
    {
      return string.Format("Environment: {0}, Radiator: {1}, Remote: {2}", (object) this.Environment, (object) this.Radiator, (object) this.Remote);
    }
  }
}
