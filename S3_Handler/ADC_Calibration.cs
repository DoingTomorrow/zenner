// Decompiled with JetBrains decompiler
// Type: S3_Handler.ADC_Calibration
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class ADC_Calibration
  {
    private S3_HandlerFunctions MyFunctions;
    internal ADC_Values adc_Values;
    internal SortedList<string, S3_Parameter> adc_Parameter;
    internal SortedList<ADC_CalibrationSteps, List<ADC_Values>> adcValueLists;
    private int adc_ParameterReadStartAdr;
    private int adc_ParameterReadNumberOfBytes;

    public ADC_Calibration(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeCalibration();
    }

    private void InitializeCalibration()
    {
      this.adc_Parameter = new SortedList<string, S3_Parameter>();
      S3_Parameter s3Parameter1 = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Adc_Counter_Value_VL.ToString()];
      this.adc_Parameter.Add(s3Parameter1.Name, s3Parameter1);
      S3_Parameter s3Parameter2 = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Adc_Counter_Value_RL.ToString()];
      this.adc_Parameter.Add(s3Parameter2.Name, s3Parameter2);
      S3_Parameter s3Parameter3 = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Adc_Counter_Value_Ref1.ToString()];
      this.adc_Parameter.Add(s3Parameter3.Name, s3Parameter3);
      S3_Parameter s3Parameter4 = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Adc_Counter_Value_Ref2.ToString()];
      this.adc_Parameter.Add(s3Parameter4.Name, s3Parameter4);
      S3_Parameter s3Parameter5 = this.MyFunctions.MyMeters.ConnectedMeter.MyParameters.ParameterByName[S3_ParameterNames.Adc_Counter_V_Batt.ToString()];
      this.adc_Parameter.Add(s3Parameter5.Name, s3Parameter5);
      this.adc_ParameterReadStartAdr = (int) ushort.MaxValue;
      int num1 = 0;
      foreach (KeyValuePair<string, S3_Parameter> keyValuePair in this.adc_Parameter)
      {
        if (keyValuePair.Value.BlockStartAddress < this.adc_ParameterReadStartAdr)
          this.adc_ParameterReadStartAdr = keyValuePair.Value.BlockStartAddress;
        int num2 = keyValuePair.Value.BlockStartAddress + keyValuePair.Value.ByteSize;
        if (num2 > num1)
          num1 = num2;
      }
      this.adc_ParameterReadNumberOfBytes = num1 - this.adc_ParameterReadStartAdr;
      this.adc_Values = new ADC_Values(this.adc_Parameter, 0.0f, 0.0f);
    }

    internal bool SendAdcTestActivate()
    {
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.DeviceTestModeActivateTemperatureTest);
      bool flag = this.MyFunctions.MyCommands.AdcTestActivate();
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.DeviceTestModeActivateTemperatureTest);
      return flag;
    }

    internal bool ReadAdcValues()
    {
      if (!this.MyFunctions.MyMeters.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.adc_ParameterReadStartAdr, this.adc_ParameterReadNumberOfBytes))
        return false;
      this.adc_Values.SetValues(this.adc_Parameter, 0.0f, 0.0f);
      return true;
    }

    internal bool RunAdcCalibration(
      ADC_CalibrationSteps calibrationStep,
      float flowTemp,
      float returnTemp,
      int loops)
    {
      if (calibrationStep == ADC_CalibrationSteps.StartCalibration)
        this.InitializeCalibration();
      bool flag = false;
      try
      {
        this.MyFunctions.MyMeters.MeterJobStart(MeterJob.ADC_Calibration);
        if (calibrationStep == ADC_CalibrationSteps.StartCalibration)
          this.adcValueLists = new SortedList<ADC_CalibrationSteps, List<ADC_Values>>();
        List<ADC_Values> adcValuesList = new List<ADC_Values>();
        this.adcValueLists.Add(calibrationStep, adcValuesList);
        this.MyFunctions.BreakRequest = false;
        flag = this.MyFunctions.MyCommands.AdcTestActivate();
        if (flag)
        {
          int num = 0;
          for (int MessageInt = 0; MessageInt < loops; ++MessageInt)
          {
            flag = this.RunSingleADC_Cycle(1f);
            if (flag)
            {
              ADC_Values adcValues = new ADC_Values(this.adc_Parameter, flowTemp, returnTemp);
              if (adcValues.Adc_Counter_Value_Ref1 == 0 || adcValues.Adc_Counter_Value_Ref2 == 0 || adcValues.Adc_Counter_Value_RL == 0 || adcValues.Adc_Counter_Value_VL == 0)
              {
                ++num;
                if (num > 5)
                {
                  flag = false;
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration");
                  break;
                }
                if (MessageInt >= 0)
                  --MessageInt;
              }
              else
              {
                adcValuesList.Add(adcValues);
                this.adc_Values.flowTemp = (double) flowTemp;
                this.adc_Values.returnTemp = (double) returnTemp;
                this.MyFunctions.SendMessage(MessageInt, GMM_EventArgs.MessageType.TestStepDone);
                if (this.MyFunctions.BreakRequest)
                  break;
              }
            }
            else
              break;
          }
        }
        flag &= this.MyFunctions.MyCommands.TestDone(272769346L);
        if (flag && calibrationStep == ADC_CalibrationSteps.EndCalibration)
          flag = this.calculateNewAdcParameter();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Error on RunAdcCalibration");
      }
      finally
      {
        this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.ADC_Calibration);
      }
      return flag;
    }

    private bool calculateNewAdcParameter()
    {
      if (this.adcValueLists.Count != 3 || !this.adcValueLists.ContainsKey(ADC_CalibrationSteps.StartCalibration) || !this.adcValueLists.ContainsKey(ADC_CalibrationSteps.SecondPoint) || !this.adcValueLists.ContainsKey(ADC_CalibrationSteps.EndCalibration))
        return false;
      int counterValueRef1 = this.adcValueLists[ADC_CalibrationSteps.StartCalibration][1].Adc_Counter_Value_Ref1;
      float[,] numArray1 = new float[5, 5];
      float[,] numArray2 = new float[3, 6];
      float[,] numArray3 = new float[3, 3];
      float[,] numArray4 = new float[3, 3];
      float[,] numArray5 = new float[3, 3];
      float[] numArray6 = new float[3];
      float[] numArray7 = new float[3];
      float[] numArray8 = new float[4];
      float[] numArray9 = new float[4];
      float[] numArray10 = new float[3];
      float[] numArray11 = new float[3];
      ADC_CalibrationSteps key1 = ADC_CalibrationSteps.StartCalibration;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        if (index1 == 0)
          key1 = ADC_CalibrationSteps.StartCalibration;
        if (index1 == 1)
          key1 = ADC_CalibrationSteps.SecondPoint;
        if (index1 == 2)
          key1 = ADC_CalibrationSteps.EndCalibration;
        int count = this.adcValueLists[ADC_CalibrationSteps.StartCalibration].Count;
        for (int index2 = 0; index2 < 6; ++index2)
        {
          float num1 = 0.0f;
          for (int index3 = 0; index3 < count; ++index3)
          {
            if (index2 == 0)
              num1 += (float) this.adcValueLists[key1][index3].Adc_Counter_Value_Ref1;
            if (index2 == 1)
              num1 += (float) this.adcValueLists[key1][index3].Adc_Counter_Value_Ref2;
            if (index2 == 2)
              num1 += (float) this.adcValueLists[key1][index3].Adc_Counter_Value_VL;
            if (index2 == 3)
              num1 += (float) this.adcValueLists[key1][index3].Adc_Counter_Value_RL;
            if (index2 == 4)
              num1 += (float) this.adcValueLists[key1][index3].flowTemp;
            if (index2 == 5)
              num1 += (float) this.adcValueLists[key1][index3].returnTemp;
          }
          float num2 = num1 / (float) count;
          numArray2[index1, index2] = num2;
        }
      }
      for (int index = 0; index < 3; ++index)
      {
        if (!this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.IsWR4)
        {
          if ((double) numArray2[index, 2] <= (double) numArray2[index, 0])
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter VL <= Counter Ref1");
            return false;
          }
          if ((double) numArray2[index, 3] <= (double) numArray2[index, 0])
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter RL <= Counter Ref1");
            return false;
          }
          if ((double) numArray2[index, 2] >= (double) numArray2[index, 1])
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter VL >= Counter Ref2");
            return false;
          }
          if ((double) numArray2[index, 3] >= (double) numArray2[index, 1])
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter RL >= Counter Ref2");
            return false;
          }
        }
        if ((double) numArray2[index, 0] == 0.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter Ref1 = 0");
          return false;
        }
        if ((double) numArray2[index, 1] == 0.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter Ref2 = 0");
          return false;
        }
        if ((double) numArray2[index, 0] == 16777214.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter Ref1 = 2^24-1");
          return false;
        }
        if ((double) numArray2[index, 1] == 16777214.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter Ref2 = 2^24-1");
          return false;
        }
        if ((double) numArray2[index, 0] == (double) numArray2[index, 1])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration, Counter Ref1 = Counter Ref2");
          return false;
        }
      }
      for (int index = 0; index < 3; ++index)
      {
        numArray10[index] = numArray2[index, 2] - numArray2[index, 0];
        numArray11[index] = numArray2[index, 3] - numArray2[index, 0];
        numArray6[index] = numArray2[index, 4] * 100f;
        numArray7[index] = numArray2[index, 5] * 100f;
      }
      int num3;
      for (num3 = 0; num3 < 2; ++num3)
      {
        for (int index4 = 0; index4 < 3; ++index4)
        {
          for (int index5 = 0; index5 < 3; ++index5)
          {
            numArray3[index4, index5] = 0.0f;
            numArray4[index4, index5] = 0.0f;
            numArray5[index4, index5] = 0.0f;
          }
        }
        for (int index = 0; index < 3; ++index)
        {
          if (num3 == 0)
          {
            numArray4[index, 0] = 1f;
            numArray4[index, 1] = numArray10[index];
            numArray4[index, 2] = numArray10[index] * numArray10[index];
          }
          else
          {
            numArray4[index, 0] = 1f;
            numArray4[index, 1] = numArray11[index];
            numArray4[index, 2] = numArray11[index] * numArray11[index];
          }
        }
        numArray5[0, 0] = (float) ((double) numArray4[1, 1] * (double) numArray4[2, 2] - (double) numArray4[1, 2] * (double) numArray4[2, 1]);
        numArray5[0, 1] = (float) ((double) numArray4[0, 2] * (double) numArray4[2, 1] - (double) numArray4[0, 1] * (double) numArray4[2, 2]);
        numArray5[0, 2] = (float) ((double) numArray4[0, 1] * (double) numArray4[1, 2] - (double) numArray4[0, 2] * (double) numArray4[1, 1]);
        numArray5[1, 0] = (float) ((double) numArray4[1, 2] * (double) numArray4[2, 0] - (double) numArray4[1, 0] * (double) numArray4[2, 2]);
        numArray5[1, 1] = (float) ((double) numArray4[0, 0] * (double) numArray4[2, 2] - (double) numArray4[0, 2] * (double) numArray4[2, 0]);
        numArray5[1, 2] = (float) ((double) numArray4[0, 2] * (double) numArray4[1, 0] - (double) numArray4[0, 0] * (double) numArray4[1, 2]);
        numArray5[2, 0] = (float) ((double) numArray4[1, 0] * (double) numArray4[2, 1] - (double) numArray4[1, 1] * (double) numArray4[2, 0]);
        numArray5[2, 1] = (float) ((double) numArray4[0, 1] * (double) numArray4[2, 0] - (double) numArray4[0, 0] * (double) numArray4[2, 1]);
        numArray5[2, 2] = (float) ((double) numArray4[0, 0] * (double) numArray4[1, 1] - (double) numArray4[0, 1] * (double) numArray4[1, 0]);
        float num4 = (float) ((double) numArray4[0, 0] * (double) numArray4[1, 1] * (double) numArray4[2, 2] + (double) numArray4[0, 1] * (double) numArray4[1, 2] * (double) numArray4[2, 0] + (double) numArray4[0, 2] * (double) numArray4[1, 0] * (double) numArray4[2, 1] - (double) numArray4[2, 0] * (double) numArray4[1, 1] * (double) numArray4[0, 2] - (double) numArray4[2, 1] * (double) numArray4[1, 2] * (double) numArray4[0, 0] - (double) numArray4[2, 2] * (double) numArray4[1, 0] * (double) numArray4[0, 1]);
        if ((double) num4 == 0.0)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on RunAdcCalibration");
          return false;
        }
        numArray5[0, 0] = numArray5[0, 0] / num4;
        numArray5[0, 1] = numArray5[0, 1] / num4;
        numArray5[0, 2] = numArray5[0, 2] / num4;
        numArray5[1, 0] = numArray5[1, 0] / num4;
        numArray5[1, 1] = numArray5[1, 1] / num4;
        numArray5[1, 2] = numArray5[1, 2] / num4;
        numArray5[2, 0] = numArray5[2, 0] / num4;
        numArray5[2, 1] = numArray5[2, 1] / num4;
        numArray5[2, 2] = numArray5[2, 2] / num4;
        for (int index6 = 0; index6 < 3; ++index6)
        {
          for (int index7 = 0; index7 < 3; ++index7)
          {
            for (int index8 = 0; index8 < 3; ++index8)
              numArray3[index6, index7] = numArray3[index6, index7] + numArray4[index6, index8] * numArray5[index8, index7];
          }
        }
        for (int index9 = 0; index9 < 3; ++index9)
        {
          for (int index10 = 0; index10 < 3; ++index10)
          {
            if (num3 == 0)
              numArray8[index9] = numArray8[index9] + numArray5[index9, index10] * numArray6[index10];
            else
              numArray9[index9] = numArray9[index9] + numArray5[index9, index10] * numArray7[index10];
          }
        }
        if (num3 == 0)
          numArray8[3] = (float) (((double) numArray2[0, 1] + (double) numArray2[1, 1] + (double) numArray2[2, 1] - ((double) numArray2[0, 0] + (double) numArray2[1, 0] + (double) numArray2[2, 0])) / 3.0);
        else
          numArray9[3] = (float) (((double) numArray2[0, 1] + (double) numArray2[1, 1] + (double) numArray2[2, 1] - ((double) numArray2[0, 0] + (double) numArray2[1, 0] + (double) numArray2[2, 0])) / 3.0);
      }
      int num5 = num3 + 1;
      ParameterList parameters = this.MyFunctions.MyMeters.WorkMeter.MyParameters;
      int num6 = parameters.ParameterByName[S3_ParameterNames.Con_cal_tf_a0_0.ToString()].SetFloatValue(numArray8[0]) ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName1 = parameters.ParameterByName;
      S3_ParameterNames s3ParameterNames = S3_ParameterNames.Con_cal_tf_a0_1;
      string key2 = s3ParameterNames.ToString();
      int num7 = parameterByName1[key2].SetFloatValue(numArray9[0]) ? 1 : 0;
      int num8 = (num6 | num7) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName2 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_0;
      string key3 = s3ParameterNames.ToString();
      int num9 = parameterByName2[key3].SetFloatValue(numArray8[1]) ? 1 : 0;
      int num10 = (num8 | num9) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName3 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_1;
      string key4 = s3ParameterNames.ToString();
      int num11 = parameterByName3[key4].SetFloatValue(numArray9[1]) ? 1 : 0;
      int num12 = (num10 | num11) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName4 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_0;
      string key5 = s3ParameterNames.ToString();
      int num13 = parameterByName4[key5].SetFloatValue(numArray8[2]) ? 1 : 0;
      int num14 = (num12 | num13) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName5 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_1;
      string key6 = s3ParameterNames.ToString();
      int num15 = parameterByName5[key6].SetFloatValue(numArray9[2]) ? 1 : 0;
      int num16 = (num14 | num15) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName6 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_0;
      string key7 = s3ParameterNames.ToString();
      int num17 = parameterByName6[key7].SetFloatValue(numArray8[3]) ? 1 : 0;
      int num18 = (num16 | num17) != 0 ? 1 : 0;
      SortedList<string, S3_Parameter> parameterByName7 = parameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_adc_diff_ref2_ref1_1;
      string key8 = s3ParameterNames.ToString();
      int num19 = parameterByName7[key8].SetFloatValue(numArray9[3]) ? 1 : 0;
      return (num18 | num19) != 0;
    }

    internal bool RunSingleADC_Cycle(Decimal simulatedVolumeInQm)
    {
      return this.RunSingleADC_Cycle((float) (simulatedVolumeInQm / this.MyFunctions.MyMeters.WorkMeter.MyMeterScaling.volumeLcdToQmFactor));
    }

    private bool RunSingleADC_Cycle(float simulatedVolume)
    {
      return this.MyFunctions.MyCommands.AdcTestCycleWithSimulatedVolume(simulatedVolume) && this.ReadAdcValues();
    }

    internal void CalibrateConnectedSensors(
      TemperatureSensorParameters flowSensor,
      TemperatureSensorParameters returnSensor)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      double[] numArray1 = new double[3];
      double[] numArray2 = new double[3];
      double[] numArray3 = new double[3];
      double[] numArray4 = new double[3];
      double[] numArray5 = new double[3];
      double[] numArray6 = new double[3];
      double[] numArray7 = new double[3];
      double[] numArray8 = new double[3];
      double[] numArray9 = new double[3];
      double[] numArray10 = new double[3];
      double[,] numArray11 = new double[3, 3];
      double[,] numArray12 = new double[3, 3];
      double[,] numArray13 = new double[3, 3];
      if (flowSensor.NominalResistorValue != returnSensor.NominalResistorValue)
        throw new Exception("R0 FlowSensor unequal R0 Returnsensor");
      double num1;
      switch (flowSensor.NominalResistorValue)
      {
        case 100:
          num1 = 100.0;
          break;
        case 500:
          num1 = 500.0;
          break;
        case 1000:
          num1 = 1000.0;
          break;
        default:
          throw new Exception("Invalied R0, R0 unequal 1000 / 500 / 100 Ohm");
      }
      double num2 = 0.0039083;
      double num3 = -5.775E-07;
      S3_ParameterNames s3ParameterNames;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        if (index1 == 0)
        {
          numArray6[0] = flowSensor.T1Set;
          numArray6[1] = flowSensor.T2Set;
          numArray6[2] = flowSensor.T3Set;
          for (int index2 = 0; index2 < 3; ++index2)
            numArray5[index2] = this.tempToResistor(flowSensor.R0, flowSensor.A, flowSensor.B, numArray6[index2]);
          double[] numArray14 = numArray9;
          SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a0_0;
          string key1 = s3ParameterNames.ToString();
          double floatValue1 = (double) parameterByName1[key1].GetFloatValue();
          numArray14[0] = floatValue1;
          double[] numArray15 = numArray9;
          SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_0;
          string key2 = s3ParameterNames.ToString();
          double floatValue2 = (double) parameterByName2[key2].GetFloatValue();
          numArray15[1] = floatValue2;
          double[] numArray16 = numArray9;
          SortedList<string, S3_Parameter> parameterByName3 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_0;
          string key3 = s3ParameterNames.ToString();
          double floatValue3 = (double) parameterByName3[key3].GetFloatValue();
          numArray16[2] = floatValue3;
          numArray1[0] = numArray9[0];
          numArray1[1] = numArray9[1];
          numArray1[2] = numArray9[2];
        }
        else
        {
          numArray6[0] = returnSensor.T1Set;
          numArray6[1] = returnSensor.T2Set;
          numArray6[2] = returnSensor.T3Set;
          for (int index3 = 0; index3 < 3; ++index3)
            numArray5[index3] = this.tempToResistor(returnSensor.R0, returnSensor.A, returnSensor.B, numArray6[index3]);
          double[] numArray17 = numArray9;
          SortedList<string, S3_Parameter> parameterByName4 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a0_1;
          string key4 = s3ParameterNames.ToString();
          double floatValue4 = (double) parameterByName4[key4].GetFloatValue();
          numArray17[0] = floatValue4;
          double[] numArray18 = numArray9;
          SortedList<string, S3_Parameter> parameterByName5 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_1;
          string key5 = s3ParameterNames.ToString();
          double floatValue5 = (double) parameterByName5[key5].GetFloatValue();
          numArray18[1] = floatValue5;
          double[] numArray19 = numArray9;
          SortedList<string, S3_Parameter> parameterByName6 = workMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_1;
          string key6 = s3ParameterNames.ToString();
          double floatValue6 = (double) parameterByName6[key6].GetFloatValue();
          numArray19[2] = floatValue6;
          numArray2[0] = numArray9[0];
          numArray2[1] = numArray9[1];
          numArray2[2] = numArray9[2];
        }
        for (int index4 = 0; index4 < 3; ++index4)
          numArray7[index4] = this.qGleichung(num3 * num1, num2 * num1, num1 - numArray5[index4]);
        for (int index5 = 0; index5 < 3; ++index5)
          numArray8[index5] = this.qGleichung(numArray9[2], numArray9[1], numArray9[0] - 100.0 * numArray7[index5]);
        for (int index6 = 0; index6 < 3; ++index6)
        {
          for (int index7 = 0; index7 < 3; ++index7)
          {
            numArray11[index6, index7] = 0.0;
            numArray12[index6, index7] = 0.0;
            numArray13[index6, index7] = 0.0;
          }
        }
        for (int index8 = 0; index8 < 3; ++index8)
        {
          numArray12[index8, 0] = 1.0;
          numArray12[index8, 1] = numArray8[index8];
          numArray12[index8, 2] = numArray8[index8] * numArray8[index8];
        }
        numArray13[0, 0] = numArray12[1, 1] * numArray12[2, 2] - numArray12[1, 2] * numArray12[2, 1];
        numArray13[0, 1] = numArray12[0, 2] * numArray12[2, 1] - numArray12[0, 1] * numArray12[2, 2];
        numArray13[0, 2] = numArray12[0, 1] * numArray12[1, 2] - numArray12[0, 2] * numArray12[1, 1];
        numArray13[1, 0] = numArray12[1, 2] * numArray12[2, 0] - numArray12[1, 0] * numArray12[2, 2];
        numArray13[1, 1] = numArray12[0, 0] * numArray12[2, 2] - numArray12[0, 2] * numArray12[2, 0];
        numArray13[1, 2] = numArray12[0, 2] * numArray12[1, 0] - numArray12[0, 0] * numArray12[1, 2];
        numArray13[2, 0] = numArray12[1, 0] * numArray12[2, 1] - numArray12[1, 1] * numArray12[2, 0];
        numArray13[2, 1] = numArray12[0, 1] * numArray12[2, 0] - numArray12[0, 0] * numArray12[2, 1];
        numArray13[2, 2] = numArray12[0, 0] * numArray12[1, 1] - numArray12[0, 1] * numArray12[1, 0];
        double num4 = numArray12[0, 0] * numArray12[1, 1] * numArray12[2, 2] + numArray12[0, 1] * numArray12[1, 2] * numArray12[2, 0] + numArray12[0, 2] * numArray12[1, 0] * numArray12[2, 1] - numArray12[2, 0] * numArray12[1, 1] * numArray12[0, 2] - numArray12[2, 1] * numArray12[1, 2] * numArray12[0, 0] - numArray12[2, 2] * numArray12[1, 0] * numArray12[0, 1];
        if (num4 == 0.0)
          throw new Exception("Error on RunAdcCalibration");
        numArray13[0, 0] = numArray13[0, 0] / num4;
        numArray13[0, 1] = numArray13[0, 1] / num4;
        numArray13[0, 2] = numArray13[0, 2] / num4;
        numArray13[1, 0] = numArray13[1, 0] / num4;
        numArray13[1, 1] = numArray13[1, 1] / num4;
        numArray13[1, 2] = numArray13[1, 2] / num4;
        numArray13[2, 0] = numArray13[2, 0] / num4;
        numArray13[2, 1] = numArray13[2, 1] / num4;
        numArray13[2, 2] = numArray13[2, 2] / num4;
        for (int index9 = 0; index9 < 3; ++index9)
        {
          for (int index10 = 0; index10 < 3; ++index10)
          {
            for (int index11 = 0; index11 < 3; ++index11)
              numArray11[index9, index10] = numArray11[index9, index10] + numArray12[index9, index11] * numArray13[index11, index10];
          }
        }
        for (int index12 = 0; index12 < 3; ++index12)
        {
          numArray10[index12] = 0.0;
          for (int index13 = 0; index13 < 3; ++index13)
            numArray10[index12] = numArray10[index12] + numArray13[index12, index13] * numArray6[index13] * 100.0;
        }
        if (index1 == 0)
        {
          for (int index14 = 0; index14 < 3; ++index14)
            numArray3[index14] = numArray10[index14];
        }
        else
        {
          for (int index15 = 0; index15 < 3; ++index15)
            numArray4[index15] = numArray10[index15];
        }
      }
      SortedList<string, S3_Parameter> parameterByName7 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a0_0;
      string key7 = s3ParameterNames.ToString();
      parameterByName7[key7].SetFloatValue((float) numArray3[0]);
      SortedList<string, S3_Parameter> parameterByName8 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_0;
      string key8 = s3ParameterNames.ToString();
      parameterByName8[key8].SetFloatValue((float) numArray3[1]);
      SortedList<string, S3_Parameter> parameterByName9 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_0;
      string key9 = s3ParameterNames.ToString();
      parameterByName9[key9].SetFloatValue((float) numArray3[2]);
      SortedList<string, S3_Parameter> parameterByName10 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a0_1;
      string key10 = s3ParameterNames.ToString();
      parameterByName10[key10].SetFloatValue((float) numArray4[0]);
      SortedList<string, S3_Parameter> parameterByName11 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a1_1;
      string key11 = s3ParameterNames.ToString();
      parameterByName11[key11].SetFloatValue((float) numArray4[1]);
      SortedList<string, S3_Parameter> parameterByName12 = workMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_cal_tf_a2_1;
      string key12 = s3ParameterNames.ToString();
      parameterByName12[key12].SetFloatValue((float) numArray4[2]);
    }

    internal double qGleichung(double a, double b, double c)
    {
      double num = Math.Sqrt(b * b - 4.0 * a * c);
      return (-b + num) / (2.0 * a);
    }

    internal double tempToResistor(double R0, double A, double B, double t)
    {
      return R0 * (1.0 + A * t + B * t * t);
    }

    internal SortedList<string, string> GetDiagnosticValues()
    {
      SortedList<string, string> diagnosticValues = new SortedList<string, string>();
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      S3_Meter connectedMeter = this.MyFunctions.MyMeters.ConnectedMeter;
      string key1 = S3_ParameterNames.Con_cal_tf_a0_0.ToString();
      diagnosticValues.Add(key1 + " r", connectedMeter.MyParameters.ParameterByName[key1].GetFloatValue().ToString());
      string key2 = S3_ParameterNames.Con_cal_tf_a1_0.ToString();
      diagnosticValues.Add(key2 + " r", connectedMeter.MyParameters.ParameterByName[key2].GetFloatValue().ToString());
      string key3 = S3_ParameterNames.Con_cal_tf_a2_0.ToString();
      diagnosticValues.Add(key3 + " r", connectedMeter.MyParameters.ParameterByName[key3].GetFloatValue().ToString());
      string key4 = S3_ParameterNames.Con_cal_tf_a0_0.ToString();
      diagnosticValues.Add(key4 + " w", workMeter.MyParameters.ParameterByName[key4].GetFloatValue().ToString());
      string key5 = S3_ParameterNames.Con_cal_tf_a1_0.ToString();
      diagnosticValues.Add(key5 + " w", workMeter.MyParameters.ParameterByName[key5].GetFloatValue().ToString());
      string key6 = S3_ParameterNames.Con_cal_tf_a2_0.ToString();
      diagnosticValues.Add(key6 + " w", workMeter.MyParameters.ParameterByName[key6].GetFloatValue().ToString());
      string key7 = S3_ParameterNames.Con_cal_tf_a0_1.ToString();
      diagnosticValues.Add(key7 + " r", connectedMeter.MyParameters.ParameterByName[key7].GetFloatValue().ToString());
      string key8 = S3_ParameterNames.Con_cal_tf_a1_1.ToString();
      diagnosticValues.Add(key8 + " r", connectedMeter.MyParameters.ParameterByName[key8].GetFloatValue().ToString());
      string key9 = S3_ParameterNames.Con_cal_tf_a2_1.ToString();
      diagnosticValues.Add(key9 + " r", connectedMeter.MyParameters.ParameterByName[key9].GetFloatValue().ToString());
      string key10 = S3_ParameterNames.Con_cal_tf_a0_1.ToString();
      diagnosticValues.Add(key10 + " w", workMeter.MyParameters.ParameterByName[key10].GetFloatValue().ToString());
      string key11 = S3_ParameterNames.Con_cal_tf_a1_1.ToString();
      diagnosticValues.Add(key11 + " w", workMeter.MyParameters.ParameterByName[key11].GetFloatValue().ToString());
      string key12 = S3_ParameterNames.Con_cal_tf_a2_1.ToString();
      diagnosticValues.Add(key12 + " w", workMeter.MyParameters.ParameterByName[key12].GetFloatValue().ToString());
      return diagnosticValues;
    }

    public void CalibrateByConfigurator(CalibrationValues cv)
    {
      if (cv.CalFlowTempMaxGrad < 80.0)
        throw new Exception("Max flow temperatur has to be >= 80°C");
      if (cv.CalReturnTempMaxGrad < 80.0)
        throw new Exception("Max return temperatur has to be >= 80°C");
      if (cv.CalFlowTempMinGrad > 30.0)
        throw new Exception("Min flow temperatur has to be <= 30°C");
      if (cv.CalReturnTempMinGrad > 30.0)
        throw new Exception("Min return temperatur has to be <= 30°C");
      if (cv.CalFlowTempMiddleGrad > cv.CalFlowTempMaxGrad - 20.0)
        throw new Exception("Middle flow temperatur has to be 20°c <= then max temperature");
      if (cv.CalFlowTempMiddleGrad < cv.CalFlowTempMinGrad + 20.0)
        throw new Exception("Middle flow temperatur has to be 20°c >= then min temperature");
      if (cv.CalReturnTempMiddleGrad > cv.CalReturnTempMaxGrad - 20.0)
        throw new Exception("Middle return temperatur has to be 20°c <= then max temperature");
      if (cv.CalReturnTempMiddleGrad < cv.CalReturnTempMinGrad + 20.0)
        throw new Exception("Middle return temperatur has to be 20°c >= then min temperature");
      if (cv.CalFlowTempMinErrorPercent > 5.0 || cv.CalFlowTempMinErrorPercent < -5.0)
        throw new Exception("Min flow temperatur error has to be between -5% and +5% ");
      if (cv.CalFlowTempMiddleErrorPercent > 5.0 || cv.CalFlowTempMiddleErrorPercent < -5.0)
        throw new Exception("Middle flow temperatur error has to be between -5% and +5% ");
      if (cv.CalFlowTempMaxErrorPercent > 5.0 || cv.CalFlowTempMaxErrorPercent < -5.0)
        throw new Exception("Max flow temperatur error has to be between -5% and +5% ");
      if (cv.CalReturnTempMinErrorPercent > 5.0 || cv.CalReturnTempMinErrorPercent < -5.0)
        throw new Exception("Min return temperatur error has to be between -5% and +5% ");
      if (cv.CalReturnTempMiddleErrorPercent > 5.0 || cv.CalReturnTempMiddleErrorPercent < -5.0)
        throw new Exception("Middle return temperatur error has to be between -5% and +5% ");
      if (cv.CalReturnTempMaxErrorPercent > 5.0 || cv.CalReturnTempMaxErrorPercent < -5.0)
        throw new Exception("Max return temperatur error has to be between -5% and +5% ");
      throw new NotImplementedException("Temperatur calibration by configurator not yet implemented");
    }
  }
}
