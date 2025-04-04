// Decompiled with JetBrains decompiler
// Type: S4_Handler.FlyingTestData
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  public class FlyingTestData
  {
    public float flowStart;
    public float flowStop;
    public float nfcTimeStart;
    public float nfcTimeStop;
    public uint tdcMeasCounter;
    public float nfcTotalTime;
    public float tdcTotalTime;
    public float flyingTestVol;
    public float flyingTestResultVol;

    public override string ToString()
    {
      return "" + "Flow-Start:\t " + this.flowStart.ToString() + "\r\nFlow-Stop:\t " + this.flowStop.ToString() + "\r\nNFC_Time-Start:\t " + this.nfcTimeStart.ToString() + "\r\nNFC_Time-Stop:\t " + this.nfcTimeStop.ToString() + "\r\nMeas_Counter:\t " + this.tdcMeasCounter.ToString() + "\r\nNFC_Time-Total:\t " + this.nfcTotalTime.ToString() + "\r\nTDC_Time-Total:\t " + this.tdcTotalTime.ToString() + "\r\nTest_Volume:\t " + this.flyingTestVol.ToString() + "\r\nTest_ResultVolume:\t " + this.flyingTestResultVol.ToString() + "\r\n";
    }
  }
}
