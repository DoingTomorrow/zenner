// Decompiled with JetBrains decompiler
// Type: S4_Handler.UltrasonicTestResults
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using S4_Handler.Functions;
using System.Text;

#nullable disable
namespace S4_Handler
{
  public class UltrasonicTestResults
  {
    public double Temperature;
    public bool TemperatureMeasuringOK;
    public uint TemperatureReferenceCounts;
    public uint TemperatureSensorCounts;
    public double ResonatorCalibration;
    public double Receiver1Amplitude;
    public double Receiver2Amplitude;
    public double UltrasonicUpTime;
    public double UltrasonicDownTime;
    public double UltrasonicTimeDiff;
    public bool SecondUltransoncSystemInstalled;
    public double SUS_ResonatorCalibration;
    public double SUS_Receiver1Amplitude;
    public double SUS_Receiver2Amplitude;
    public double SUS_UltrasonicUpTime;
    public double SUS_UltrasonicDownTime;
    public double SUS_UltrasonicTimeDiff;
    public TdcLevelTestData LevelData;
    public TdcLevelTestData SUS_LevelData;
    public ushort[] LevelOffsets = new ushort[6];
    public uint[] CountValues = new uint[6];

    public UltrasonicTestResults(UltrasonicState us)
    {
      this.Temperature = 0.0;
      this.TemperatureReferenceCounts = 0U;
      this.TemperatureSensorCounts = 0U;
      this.TemperatureMeasuringOK = false;
      this.ResonatorCalibration = 0.0;
      this.Receiver1Amplitude = 0.0;
      this.Receiver2Amplitude = 0.0;
      if (us.TransducerPair2State != null)
      {
        this.SecondUltransoncSystemInstalled = true;
        this.UltrasonicUpTime = (double) us.TransducerPair2State.UpCounts;
        this.UltrasonicDownTime = (double) us.TransducerPair2State.DownCounts;
        this.UltrasonicTimeDiff = (double) us.TransducerPair2State.DiffCounts;
        this.SUS_UltrasonicUpTime = (double) us.TransducerPair1State.UpCounts;
        this.SUS_UltrasonicDownTime = (double) us.TransducerPair1State.DownCounts;
        this.SUS_UltrasonicTimeDiff = (double) us.TransducerPair1State.DiffCounts;
      }
      else
      {
        this.SecondUltransoncSystemInstalled = false;
        this.UltrasonicUpTime = (double) us.TransducerPair1State.UpCounts;
        this.UltrasonicDownTime = (double) us.TransducerPair1State.DownCounts;
        this.UltrasonicTimeDiff = (double) us.TransducerPair1State.DiffCounts;
      }
    }

    public override string ToString()
    {
      int totalWidth1 = 5;
      int totalWidth2 = 11;
      int totalWidth3 = 6;
      int totalWidth4 = 8;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Ultrasonic test results");
      stringBuilder1.AppendLine();
      stringBuilder1.AppendLine("------------------ Temperature ------------------");
      if (this.TemperatureMeasuringOK)
        stringBuilder1.AppendLine("Temperature: " + this.Temperature.ToString());
      else
        stringBuilder1.AppendLine("Temperature measurement error !! Used temperature: " + this.Temperature.ToString());
      stringBuilder1.AppendLine("TemperatureReferenceCounts: " + this.TemperatureReferenceCounts.ToString());
      stringBuilder1.AppendLine("TemperatureSensorCounts: " + this.TemperatureSensorCounts.ToString());
      int levelOffset;
      if (this.SecondUltransoncSystemInstalled)
      {
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("---------------- Transducer pairs ---------------");
        stringBuilder1.AppendLine("    Pair1/Receiver1           Pair2/Receiver1    ");
        stringBuilder1.AppendLine("                red ┌───────┐ green              ");
        stringBuilder1.AppendLine("                    │  LCD  |                    ");
        stringBuilder1.AppendLine("             yellow └───────┘ blue               ");
        stringBuilder1.AppendLine("    Pair2/Receiver2           Pair1/Receiver2    ");
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("--------------- Transducer pair 1 ---------------");
        stringBuilder1.AppendLine("SUS_ResonatorCalibration: " + this.SUS_ResonatorCalibration.ToString());
        stringBuilder1.AppendLine("SUS_Receiver1Amplitude: " + this.SUS_Receiver1Amplitude.ToString());
        stringBuilder1.AppendLine("SUS_Receiver2Amplitude: " + this.SUS_Receiver2Amplitude.ToString());
        stringBuilder1.AppendLine("SUS_UltrasonicUpTime: " + this.SUS_UltrasonicUpTime.ToString());
        stringBuilder1.AppendLine("SUS_UltrasonicDownTime: " + this.SUS_UltrasonicDownTime.ToString());
        stringBuilder1.AppendLine("SUS_UltrasonicTimeDiff: " + this.SUS_UltrasonicTimeDiff.ToString());
        if (this.SUS_LevelData != null)
        {
          stringBuilder1.AppendLine("SUS_measCounter: " + this.SUS_LevelData.measCounter.ToString());
          stringBuilder1.AppendLine("SUS_offsetOldUp: " + this.SUS_LevelData.offsetOldUp.ToString());
          stringBuilder1.AppendLine("SUS_offsetOldDn: " + this.SUS_LevelData.offsetOldDn.ToString());
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine("---------- Transducer pair 1 data ------------");
          stringBuilder1.Append("Level".PadLeft(totalWidth1));
          stringBuilder1.Append("cntDn".PadLeft(totalWidth2));
          stringBuilder1.Append("cntDnDiff".PadLeft(totalWidth2));
          stringBuilder1.Append("wvrDn".PadLeft(totalWidth3));
          stringBuilder1.Append("AmplDn".PadLeft(totalWidth4));
          stringBuilder1.Append("cntUp".PadLeft(totalWidth2));
          stringBuilder1.Append("cntUpDiff".PadLeft(totalWidth2));
          stringBuilder1.Append("wvrUp".PadLeft(totalWidth3));
          stringBuilder1.Append("AmplUp".PadLeft(totalWidth4));
          stringBuilder1.AppendLine();
          for (int index = 0; index < 6; ++index)
          {
            stringBuilder1.Append(this.LevelOffsets[index].ToString().PadLeft(totalWidth1));
            int num = (int) this.SUS_LevelData.cntDn[index] - (int) this.SUS_LevelData.cntDn[0];
            stringBuilder1.Append(this.SUS_LevelData.cntDn[index].ToString().PadLeft(totalWidth2));
            stringBuilder1.Append(num.ToString().PadLeft(totalWidth2));
            stringBuilder1.Append(this.SUS_LevelData.wvrDn[index].ToString().PadLeft(totalWidth3));
            StringBuilder stringBuilder2 = stringBuilder1;
            levelOffset = S4_TDC_Internals.GetLevelOffset(this.LevelOffsets[index], (uint) this.SUS_LevelData.wvrDn[index]);
            string str1 = levelOffset.ToString().PadLeft(totalWidth4);
            stringBuilder2.Append(str1);
            num = (int) this.SUS_LevelData.cntUp[index] - (int) this.SUS_LevelData.cntUp[0];
            stringBuilder1.Append(this.SUS_LevelData.cntUp[index].ToString().PadLeft(totalWidth2));
            stringBuilder1.Append(num.ToString().PadLeft(totalWidth2));
            stringBuilder1.Append(this.SUS_LevelData.wvrUp[index].ToString().PadLeft(totalWidth3));
            StringBuilder stringBuilder3 = stringBuilder1;
            levelOffset = S4_TDC_Internals.GetLevelOffset(this.LevelOffsets[index], (uint) this.SUS_LevelData.wvrUp[index]);
            string str2 = levelOffset.ToString().PadLeft(totalWidth4);
            stringBuilder3.Append(str2);
            stringBuilder1.AppendLine();
          }
        }
      }
      if (this.SecondUltransoncSystemInstalled)
      {
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("--------------- Transducer pair 2 ---------------");
      }
      else
      {
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine("---------------_ Transducer pair ----------------");
      }
      stringBuilder1.AppendLine("ResonatorCalibration: " + this.ResonatorCalibration.ToString());
      stringBuilder1.AppendLine("Receiver1Amplitude: " + this.Receiver1Amplitude.ToString());
      stringBuilder1.AppendLine("Receiver2Amplitude: " + this.Receiver2Amplitude.ToString());
      stringBuilder1.AppendLine("UltrasonicUpTime: " + this.UltrasonicUpTime.ToString());
      stringBuilder1.AppendLine("UltrasonicDownTime: " + this.UltrasonicDownTime.ToString());
      stringBuilder1.AppendLine("UltrasonicTimeDiff: " + this.UltrasonicTimeDiff.ToString());
      if (this.LevelData != null)
      {
        stringBuilder1.AppendLine("measCounter: " + this.LevelData.measCounter.ToString());
        stringBuilder1.AppendLine("offsetOldUp: " + this.LevelData.offsetOldUp.ToString());
        stringBuilder1.AppendLine("offsetOldDn: " + this.LevelData.offsetOldDn.ToString());
        if (this.SecondUltransoncSystemInstalled)
        {
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine("---------- Transducer pair 2 data ------------");
        }
        else
        {
          stringBuilder1.AppendLine();
          stringBuilder1.AppendLine("----------- Transducer pair data -------------");
        }
        stringBuilder1.Append("Level".PadLeft(totalWidth1));
        stringBuilder1.Append("cntDn".PadLeft(totalWidth2));
        stringBuilder1.Append("cntDnDiff".PadLeft(totalWidth2));
        stringBuilder1.Append("wvrDn".PadLeft(totalWidth3));
        stringBuilder1.Append("AmplDn".PadLeft(totalWidth4));
        stringBuilder1.Append("cntUp".PadLeft(totalWidth2));
        stringBuilder1.Append("cntUpDiff".PadLeft(totalWidth2));
        stringBuilder1.Append("wvrUp".PadLeft(totalWidth3));
        stringBuilder1.Append("AmplUp".PadLeft(totalWidth4));
        stringBuilder1.AppendLine();
        for (int index = 0; index < 6; ++index)
        {
          stringBuilder1.Append(this.LevelOffsets[index].ToString().PadLeft(totalWidth1));
          int num = (int) this.LevelData.cntDn[index] - (int) this.LevelData.cntDn[0];
          stringBuilder1.Append(this.LevelData.cntDn[index].ToString().PadLeft(totalWidth2));
          stringBuilder1.Append(num.ToString().PadLeft(totalWidth2));
          stringBuilder1.Append(this.LevelData.wvrDn[index].ToString().PadLeft(totalWidth3));
          StringBuilder stringBuilder4 = stringBuilder1;
          levelOffset = S4_TDC_Internals.GetLevelOffset(this.LevelOffsets[index], (uint) this.LevelData.wvrDn[index]);
          string str3 = levelOffset.ToString().PadLeft(totalWidth4);
          stringBuilder4.Append(str3);
          num = (int) this.LevelData.cntUp[index] - (int) this.LevelData.cntUp[0];
          stringBuilder1.Append(this.LevelData.cntUp[index].ToString().PadLeft(totalWidth2));
          stringBuilder1.Append(num.ToString().PadLeft(totalWidth2));
          stringBuilder1.Append(this.LevelData.wvrUp[index].ToString().PadLeft(totalWidth3));
          StringBuilder stringBuilder5 = stringBuilder1;
          levelOffset = S4_TDC_Internals.GetLevelOffset(this.LevelOffsets[index], (uint) this.LevelData.wvrUp[index]);
          string str4 = levelOffset.ToString().PadLeft(totalWidth4);
          stringBuilder5.Append(str4);
          stringBuilder1.AppendLine();
        }
      }
      return stringBuilder1.ToString();
    }
  }
}
