// Decompiled with JetBrains decompiler
// Type: DeviceCollector.WFParameterEntry
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

#nullable disable
namespace DeviceCollector
{
  public class WFParameterEntry
  {
    public WaveFlowDevice.ParameterNames ParameterName;
    public string Entry;

    public WFParameterEntry(WaveFlowDevice.ParameterNames TheParameterName, string TheEntry)
    {
      this.ParameterName = TheParameterName;
      this.Entry = TheEntry;
    }
  }
}
