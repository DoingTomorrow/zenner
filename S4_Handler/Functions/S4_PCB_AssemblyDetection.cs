// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_PCB_AssemblyDetection
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;
using System.Text;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_PCB_AssemblyDetection
  {
    private static double[] RatioRangeStartValues = new double[6]
    {
      1.154,
      1.461,
      1.993,
      3.07,
      7.233,
      double.MaxValue
    };
    internal ushort ADC_V_Bat_Value;
    internal ushort ADC_VCC_Bat_Value;
    internal double Ratio;
    internal int AssemblyID;
    internal ushort ADC_RangeMin;
    internal ushort ADC_RangeMax;
    internal int ADC_RangeDisplacement;
    internal double RangeDisplacementPercent = double.NaN;
    internal StringBuilder LoadInfo = new StringBuilder();
    internal bool AssamblyDetectionOk = false;
    internal bool SpecialAssamblyDetectionRedistors = false;

    internal S4_PCB_AssemblyDetection(S4_DeviceMemory deviceMemory)
    {
      this.LoadInfo.AppendLine("*** Assembly detection ***");
      this.LoadInfo.AppendLine();
      this.AssemblyID = 0;
      try
      {
        if (deviceMemory == null)
          this.LoadInfo.AppendLine("Data not available");
        else if (!deviceMemory.IsParameterInMap(S4_Params.ADC_HardwareIdentifier))
          this.LoadInfo.AppendLine("Firmware dosn't support assambly detection");
        else if (!deviceMemory.IsParameterAvailable(S4_Params.ADC_HardwareIdentifier))
        {
          this.LoadInfo.AppendLine("Hardware detection data not read");
        }
        else
        {
          uint parameterValue = deviceMemory.GetParameterValue<uint>(S4_Params.ADC_HardwareIdentifier);
          this.ADC_V_Bat_Value = (ushort) parameterValue;
          this.ADC_VCC_Bat_Value = (ushort) (parameterValue >> 16);
          if (this.ADC_VCC_Bat_Value < (ushort) 1024 || this.ADC_VCC_Bat_Value > (ushort) 4088)
            this.LoadInfo.AppendLine("ADC_VCC_Bat out of range");
          else if (this.ADC_V_Bat_Value < (ushort) 8 || this.ADC_V_Bat_Value > (ushort) 4088)
          {
            this.LoadInfo.AppendLine("ADC_V_Bat out of range");
            this.AssemblyID = 15;
          }
          else
          {
            this.Ratio = (double) this.ADC_VCC_Bat_Value / (double) this.ADC_V_Bat_Value;
            double num1 = 3.0;
            if (this.ADC_VCC_Bat_Value > (ushort) 2834)
            {
              this.LoadInfo.AppendLine("Voltage divider 100K/330K detected");
              this.SpecialAssamblyDetectionRedistors = true;
              if (this.ADC_VCC_Bat_Value <= (ushort) 3150)
              {
                this.LoadInfo.AppendLine("Voltage at sample time <= 3V");
              }
              else
              {
                num1 = 0.000732421875 * (double) this.ADC_VCC_Bat_Value / 330.0 * 430.0;
                this.LoadInfo.AppendLine("Voltage at sample time = " + num1.ToString("0.00"));
              }
              double num2 = num1 / 3.6;
              this.ADC_RangeMin = ushort.MaxValue;
              this.ADC_RangeMax = (ushort) 0;
              ushort num3 = 4095;
              for (int index = 0; index < S4_PCB_AssemblyDetection.RatioRangeStartValues.Length; ++index)
              {
                ushort num4 = (ushort) ((double) this.ADC_VCC_Bat_Value / S4_PCB_AssemblyDetection.RatioRangeStartValues[index]);
                ushort num5 = (ushort) ((uint) num3 - (uint) num4);
                if ((int) num5 < (int) this.ADC_RangeMin)
                  this.ADC_RangeMin = num5;
                if ((int) num5 > (int) this.ADC_RangeMax)
                  this.ADC_RangeMax = num5;
                num3 = num4;
              }
              for (int index = 0; index < S4_PCB_AssemblyDetection.RatioRangeStartValues.Length; ++index)
              {
                if (this.Ratio < S4_PCB_AssemblyDetection.RatioRangeStartValues[index])
                {
                  this.AssemblyID = index;
                  if (index == 0)
                  {
                    this.ADC_RangeMin = (ushort) ((double) this.ADC_VCC_Bat_Value / S4_PCB_AssemblyDetection.RatioRangeStartValues[index] * num2);
                    this.ADC_RangeMax = (ushort) 4095;
                  }
                  else if (index == S4_PCB_AssemblyDetection.RatioRangeStartValues.Length - 1)
                  {
                    this.ADC_RangeMin = (ushort) 0;
                    this.ADC_RangeMax = (ushort) ((double) this.ADC_VCC_Bat_Value / S4_PCB_AssemblyDetection.RatioRangeStartValues[index - 1] * num2);
                  }
                  else
                  {
                    this.ADC_RangeMin = (ushort) ((double) this.ADC_VCC_Bat_Value / S4_PCB_AssemblyDetection.RatioRangeStartValues[index] * num2);
                    this.ADC_RangeMax = (ushort) ((double) this.ADC_VCC_Bat_Value / S4_PCB_AssemblyDetection.RatioRangeStartValues[index - 1] * num2);
                  }
                  ushort num6 = (ushort) (((int) this.ADC_RangeMin + (int) this.ADC_RangeMax) / 2);
                  ushort num7 = (ushort) ((uint) this.ADC_RangeMax - (uint) this.ADC_RangeMin);
                  this.ADC_RangeDisplacement = (int) this.ADC_V_Bat_Value - (int) num6;
                  this.RangeDisplacementPercent = 100.0 / (double) ((int) num7 / 2) * (double) this.ADC_RangeDisplacement;
                }
              }
            }
            else
            {
              this.LoadInfo.AppendLine("Voltage divider 100K/100K detected");
              if (this.ADC_VCC_Bat_Value <= (ushort) 2055)
                this.LoadInfo.AppendLine("Voltage at sample time <= 3V");
              else
                this.LoadInfo.AppendLine("Voltage at sample time = " + (0.000732421875 * (double) this.ADC_VCC_Bat_Value / 100.0 * 200.0).ToString("0.00"));
              this.ADC_RangeDisplacement = (int) this.ADC_V_Bat_Value - (int) this.ADC_VCC_Bat_Value;
              this.RangeDisplacementPercent = 0.5 * (double) this.ADC_RangeDisplacement;
            }
            this.AssamblyDetectionOk = true;
          }
        }
      }
      catch (Exception ex)
      {
        this.LoadInfo.AppendLine();
        this.LoadInfo.AppendLine("Exception:");
        this.LoadInfo.AppendLine(ex.Message);
        this.LoadInfo.AppendLine();
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.LoadInfo.ToString());
      stringBuilder.AppendLine("AssemblyID: " + this.AssemblyID.ToString("x"));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("ADC_V_Bat_Value: " + this.ADC_V_Bat_Value.ToString());
      stringBuilder.AppendLine("ADC_VCC_Bat_Value: " + this.ADC_VCC_Bat_Value.ToString());
      stringBuilder.AppendLine();
      if (this.AssamblyDetectionOk)
      {
        stringBuilder.AppendLine("Ratio: " + this.Ratio.ToString("0.000"));
        if (this.SpecialAssamblyDetectionRedistors)
        {
          stringBuilder.AppendLine("ADC_RangeMin: " + this.ADC_RangeMin.ToString());
          stringBuilder.AppendLine("ADC_RangeMax: " + this.ADC_RangeMax.ToString());
          stringBuilder.AppendLine("ADC_RangeDisplacement: " + this.ADC_RangeDisplacement.ToString());
        }
        stringBuilder.AppendLine("RangeDisplacementPercent: " + this.RangeDisplacementPercent.ToString("0.00") + "%");
      }
      return stringBuilder.ToString();
    }
  }
}
