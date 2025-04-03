// Decompiled with JetBrains decompiler
// Type: HandlerLib.RadioTestParameters
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class RadioTestParameters
  {
    public RadioTestByDevice.RadioTestDevice TestDevice { get; private set; }

    public ushort SyncWord { get; private set; }

    public double TestFrequency { get; private set; }

    public int AverageRSSITx { get; set; }

    public int AverageRSSIRx { get; set; }

    public RadioTestParameters()
    {
      this.TestDevice = RadioTestByDevice.RadioTestDevice.MinoConnect;
      this.TestFrequency = 868.3;
      this.SyncWord = (ushort) 37331;
    }

    public RadioTestParameters(
      RadioTestByDevice.RadioTestDevice testDevice,
      double testFrequency,
      ushort radioSyncWord)
    {
      this.TestDevice = testDevice;
      this.TestFrequency = testFrequency;
      this.SyncWord = radioSyncWord;
    }

    public override string ToString()
    {
      return "Test device: " + this.TestDevice.ToString() + "\r Frequency: " + this.TestFrequency.ToString() + "\r SyncWord: " + this.SyncWord.ToString() + "\r AvgRSSITx: " + this.AverageRSSITx.ToString() + "\r AvgRSSIRx: " + this.AverageRSSIRx.ToString();
    }
  }
}
