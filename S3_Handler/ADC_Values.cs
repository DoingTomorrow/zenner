// Decompiled with JetBrains decompiler
// Type: S3_Handler.ADC_Values
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Collections.Generic;

#nullable disable
namespace S3_Handler
{
  internal class ADC_Values
  {
    internal int Adc_Counter_Value_VL;
    internal int Adc_Counter_Value_RL;
    internal int Adc_Counter_Value_Ref1;
    internal int Adc_Counter_Value_Ref2;
    internal ushort Adc_Counter_V_Batt;
    internal double flowTemp;
    internal double returnTemp;

    internal ADC_Values(
      SortedList<string, S3_Parameter> adc_Parameter,
      float flowTemp,
      float returnTemp)
    {
      this.SetValues(adc_Parameter, flowTemp, returnTemp);
    }

    internal void SetValues(
      SortedList<string, S3_Parameter> adc_Parameter,
      float flowTemp,
      float returnTemp)
    {
      this.flowTemp = (double) flowTemp;
      this.returnTemp = (double) returnTemp;
      this.Adc_Counter_Value_VL = adc_Parameter[S3_ParameterNames.Adc_Counter_Value_VL.ToString()].GetIntValue();
      this.Adc_Counter_Value_RL = adc_Parameter[S3_ParameterNames.Adc_Counter_Value_RL.ToString()].GetIntValue();
      this.Adc_Counter_Value_Ref1 = adc_Parameter[S3_ParameterNames.Adc_Counter_Value_Ref1.ToString()].GetIntValue();
      this.Adc_Counter_Value_Ref2 = adc_Parameter[S3_ParameterNames.Adc_Counter_Value_Ref2.ToString()].GetIntValue();
      this.Adc_Counter_V_Batt = adc_Parameter[S3_ParameterNames.Adc_Counter_V_Batt.ToString()].GetUshortValue();
    }
  }
}
