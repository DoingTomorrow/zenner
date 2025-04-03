// Decompiled with JetBrains decompiler
// Type: S3_Handler.TDC_Calibration
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class TDC_Calibration(S3_HandlerFunctions MyFunctions) : VolumeGraphCalibration(MyFunctions)
  {
    private const int ZeroCalibrationRepeatLoops = 14;
    private static string[] cycleParameterVolAndTempCounter = new string[4]
    {
      "volumeCycleTimeCounter",
      "volumeCycleReduceCounter",
      "temperaturCycleTimeCounter",
      "temperaturCycleTimeSlotCounter"
    };
    private static string[] cycleParameterVolCounter = new string[2]
    {
      "volumeCycleTimeCounter",
      "volumeCycleReduceCounter"
    };
    private static string[] cycleParameterTempCounter = new string[2]
    {
      "temperaturCycleTimeCounter",
      "temperaturCycleTimeSlotCounter"
    };
    private static string[] cycleParameterResonatorCounter = new string[1]
    {
      "tdc_cal_resonator_counter"
    };
    private static string[] tdc_n_upx_down_Parameter = new string[8]
    {
      S3_ParameterNames.tdc_n_up0.ToString(),
      S3_ParameterNames.tdc_n_up1.ToString(),
      S3_ParameterNames.tdc_n_up2.ToString(),
      S3_ParameterNames.tdc_n_up3.ToString(),
      S3_ParameterNames.tdc_n_down0.ToString(),
      S3_ParameterNames.tdc_n_down1.ToString(),
      S3_ParameterNames.tdc_n_down2.ToString(),
      S3_ParameterNames.tdc_n_down3.ToString()
    };
    private static string[] tdc_n_pw_Parameter = new string[2]
    {
      S3_ParameterNames.tdc_pw_up.ToString(),
      S3_ParameterNames.tdc_pw_down.ToString()
    };
    private uint[,] Tdc_Counter_Values = new uint[8, 14];
    private int[,] Tdc_Counter_T_Diff_Values = new int[4, 14];
    private int[,] Tdc_Counter_T_Diff_Median_Values = new int[4, 12];
    private int[] Tdc_Counter_T_Diff_Mean_Values = new int[4];
    private int[] Median_Values_Buffer = new int[3];
    private uint[] Tdc_Counter_Mean_Values = new uint[8];
    private float[] Tdc_Char_Curve_Values = new float[20];
    private uint[] TdcImpulseWidhtUp = new uint[14];
    private uint[] TdcImpulseWidhtDown = new uint[14];

    internal TDC_Calibration.CalibrationStateVol GetCalibrationState()
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp.ToString()].GetShortValue() == (short) 0)
        return TDC_Calibration.CalibrationStateVol.none;
      return workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_LowTemp.ToString()].GetShortValue() == (short) 0 ? TDC_Calibration.CalibrationStateVol.zeroFlowCalibrated : TDC_Calibration.CalibrationStateVol.complete;
    }

    internal bool SetCalibrationState(TDC_Calibration.CalibrationStateVol theState, short stateTemp)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      switch (theState)
      {
        case TDC_Calibration.CalibrationStateVol.zeroFlowCalibrated:
          SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
          TDC_Calibration.TdcCalibationStateParameter calibationStateParameter1 = TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_LowTemp;
          string key1 = calibationStateParameter1.ToString();
          if (!parameterByName1[key1].SetShortValue((short) 0))
            return false;
          SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
          calibationStateParameter1 = TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp;
          string key2 = calibationStateParameter1.ToString();
          if (!parameterByName2[key2].SetShortValue(stateTemp))
            return false;
          break;
        case TDC_Calibration.CalibrationStateVol.complete:
          if (!workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_LowTemp.ToString()].SetShortValue((short) 1))
            return false;
          break;
        default:
          SortedList<string, S3_Parameter> parameterByName3 = workMeter.MyParameters.ParameterByName;
          TDC_Calibration.TdcCalibationStateParameter calibationStateParameter2 = TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_LowTemp;
          string key3 = calibationStateParameter2.ToString();
          if (!parameterByName3[key3].SetShortValue((short) 0))
            return false;
          SortedList<string, S3_Parameter> parameterByName4 = workMeter.MyParameters.ParameterByName;
          calibationStateParameter2 = TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp;
          string key4 = calibationStateParameter2.ToString();
          if (!parameterByName4[key4].SetShortValue((short) 0))
            return false;
          break;
      }
      return true;
    }

    internal override bool AdjustVolumeFactor(float ErrorInPercent)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (workMeter == null)
        return false;
      SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
      TDC_Calibration.TdcCalibationParameter calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0;
      string key1 = calibationParameter.ToString();
      if (parameterByName1.ContainsKey(key1) && !this.AdjustTdcVolume(ErrorInPercent))
        return false;
      SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.TDC_MapTemp;
      string key2 = calibationParameter.ToString();
      return !parameterByName2.ContainsKey(key2) || this.AdjustTdcVolumeMatrix(ErrorInPercent);
    }

    private bool AdjustTdcVolume(float ErrorInPercent)
    {
      for (int index = 0; index < 20; ++index)
        this.Tdc_Char_Curve_Values[index] = 0.0f;
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      this.Tdc_Char_Curve_Values[0] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[1] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_1.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[2] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_2.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[3] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_3.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[4] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_4.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[5] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_5.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[6] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_6.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[7] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_7.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[8] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_8.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[9] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_9.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[10] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_10.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[11] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_11.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[12] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_12.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[13] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_13.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[14] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_14.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[15] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_15.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[16] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_16.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[17] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_17.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[18] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_18.ToString()].GetFloatValue();
      float[] tdcCharCurveValues = this.Tdc_Char_Curve_Values;
      SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
      TDC_Calibration.TdcCalibationParameter calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_19;
      string key1 = calibationParameter.ToString();
      double floatValue = (double) parameterByName1[key1].GetFloatValue();
      tdcCharCurveValues[19] = (float) floatValue;
      for (int index = 0; index < 20; ++index)
      {
        if ((double) this.Tdc_Char_Curve_Values[index] == double.NaN)
          return false;
        this.Tdc_Char_Curve_Values[index] = this.Tdc_Char_Curve_Values[index] / (float) (1.0 + (double) ErrorInPercent / 100.0);
        this.Tdc_Char_Curve_Values[index] = CalibrationMathematik.CalibrationFloatPrecision(16U, this.Tdc_Char_Curve_Values[index]);
      }
      SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0;
      string key2 = calibationParameter.ToString();
      parameterByName2[key2].SetFloatValue(this.Tdc_Char_Curve_Values[0]);
      SortedList<string, S3_Parameter> parameterByName3 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_1;
      string key3 = calibationParameter.ToString();
      parameterByName3[key3].SetFloatValue(this.Tdc_Char_Curve_Values[1]);
      SortedList<string, S3_Parameter> parameterByName4 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_2;
      string key4 = calibationParameter.ToString();
      parameterByName4[key4].SetFloatValue(this.Tdc_Char_Curve_Values[2]);
      SortedList<string, S3_Parameter> parameterByName5 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_3;
      string key5 = calibationParameter.ToString();
      parameterByName5[key5].SetFloatValue(this.Tdc_Char_Curve_Values[3]);
      SortedList<string, S3_Parameter> parameterByName6 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_4;
      string key6 = calibationParameter.ToString();
      parameterByName6[key6].SetFloatValue(this.Tdc_Char_Curve_Values[4]);
      SortedList<string, S3_Parameter> parameterByName7 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_5;
      string key7 = calibationParameter.ToString();
      parameterByName7[key7].SetFloatValue(this.Tdc_Char_Curve_Values[5]);
      SortedList<string, S3_Parameter> parameterByName8 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_6;
      string key8 = calibationParameter.ToString();
      parameterByName8[key8].SetFloatValue(this.Tdc_Char_Curve_Values[6]);
      SortedList<string, S3_Parameter> parameterByName9 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_7;
      string key9 = calibationParameter.ToString();
      parameterByName9[key9].SetFloatValue(this.Tdc_Char_Curve_Values[7]);
      SortedList<string, S3_Parameter> parameterByName10 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_8;
      string key10 = calibationParameter.ToString();
      parameterByName10[key10].SetFloatValue(this.Tdc_Char_Curve_Values[8]);
      SortedList<string, S3_Parameter> parameterByName11 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_9;
      string key11 = calibationParameter.ToString();
      parameterByName11[key11].SetFloatValue(this.Tdc_Char_Curve_Values[9]);
      SortedList<string, S3_Parameter> parameterByName12 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_10;
      string key12 = calibationParameter.ToString();
      parameterByName12[key12].SetFloatValue(this.Tdc_Char_Curve_Values[10]);
      SortedList<string, S3_Parameter> parameterByName13 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_11;
      string key13 = calibationParameter.ToString();
      parameterByName13[key13].SetFloatValue(this.Tdc_Char_Curve_Values[11]);
      SortedList<string, S3_Parameter> parameterByName14 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_12;
      string key14 = calibationParameter.ToString();
      parameterByName14[key14].SetFloatValue(this.Tdc_Char_Curve_Values[12]);
      SortedList<string, S3_Parameter> parameterByName15 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_13;
      string key15 = calibationParameter.ToString();
      parameterByName15[key15].SetFloatValue(this.Tdc_Char_Curve_Values[13]);
      SortedList<string, S3_Parameter> parameterByName16 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_14;
      string key16 = calibationParameter.ToString();
      parameterByName16[key16].SetFloatValue(this.Tdc_Char_Curve_Values[14]);
      SortedList<string, S3_Parameter> parameterByName17 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_15;
      string key17 = calibationParameter.ToString();
      parameterByName17[key17].SetFloatValue(this.Tdc_Char_Curve_Values[15]);
      SortedList<string, S3_Parameter> parameterByName18 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_16;
      string key18 = calibationParameter.ToString();
      parameterByName18[key18].SetFloatValue(this.Tdc_Char_Curve_Values[16]);
      SortedList<string, S3_Parameter> parameterByName19 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_17;
      string key19 = calibationParameter.ToString();
      parameterByName19[key19].SetFloatValue(this.Tdc_Char_Curve_Values[17]);
      SortedList<string, S3_Parameter> parameterByName20 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_18;
      string key20 = calibationParameter.ToString();
      parameterByName20[key20].SetFloatValue(this.Tdc_Char_Curve_Values[18]);
      SortedList<string, S3_Parameter> parameterByName21 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_19;
      string key21 = calibationParameter.ToString();
      parameterByName21[key21].SetFloatValue(this.Tdc_Char_Curve_Values[19]);
      return true;
    }

    private bool AdjustTdcVolumeMatrix(float ErrorInPercent)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      S3_Parameter s3Parameter = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.TDC_MapTemp.ToString()];
      if (s3Parameter.Statics.S3_VarType != S3_VariableTypes.TDC_Matrix)
        return false;
      int minValue = (int) s3Parameter.Statics.MinValue;
      int maxValue = (int) s3Parameter.Statics.MaxValue;
      float[,] numArray = new float[minValue, maxValue];
      int Address1 = s3Parameter.BlockStartAddress + minValue * 2 + maxValue * 4;
      for (int index1 = 0; index1 < minValue; ++index1)
      {
        int index2 = 0;
        while (index2 < maxValue)
        {
          numArray[index1, index2] = workMeter.MyDeviceMemory.GetFloatValue(Address1) / (float) (1.0 + (double) ErrorInPercent / 100.0);
          numArray[index1, index2] = CalibrationMathematik.CalibrationFloatPrecision(16U, numArray[index1, index2]);
          ++index2;
          Address1 += 4;
        }
      }
      int Address2 = s3Parameter.BlockStartAddress + minValue * 2 + maxValue * 4;
      for (int index3 = 0; index3 < minValue; ++index3)
      {
        int index4 = 0;
        while (index4 < maxValue)
        {
          workMeter.MyDeviceMemory.SetFloatValue(Address2, numArray[index3, index4]);
          ++index4;
          Address2 += 4;
        }
      }
      return true;
    }

    internal override bool AdjustVolumeFactorQi(float Qi, float Q_UpperLimit, float ErrorInPercent)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (workMeter == null)
        return false;
      SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
      TDC_Calibration.TdcCalibationParameter calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0;
      string key1 = calibationParameter.ToString();
      if (parameterByName1.ContainsKey(key1) && !this.AdjustTdcVolumeQi(Qi, Q_UpperLimit, ErrorInPercent))
        return false;
      SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.TDC_MapTemp;
      string key2 = calibationParameter.ToString();
      return !parameterByName2.ContainsKey(key2) || this.AdjustTdcVolumeMatrixQi(Qi, Q_UpperLimit, ErrorInPercent);
    }

    private bool AdjustTdcVolumeQi(float Qi, float Q_UpperLimit, float ErrorInPercent)
    {
      if ((double) Qi >= (double) Q_UpperLimit)
        throw new Exception("Qi >= Q");
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      float[] numArray = new float[20]
      {
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_0.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_1.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_2.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_3.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_4.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_5.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_6.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_7.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_8.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_9.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_10.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_11.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_12.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_13.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_14.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_15.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_16.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_17.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_18.ToString()].GetFloatValue(),
        workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_19.ToString()].GetFloatValue()
      };
      this.Tdc_Char_Curve_Values[0] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[1] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_1.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[2] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_2.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[3] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_3.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[4] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_4.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[5] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_5.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[6] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_6.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[7] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_7.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[8] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_8.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[9] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_9.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[10] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_10.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[11] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_11.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[12] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_12.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[13] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_13.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[14] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_14.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[15] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_15.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[16] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_16.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[17] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_17.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[18] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_18.ToString()].GetFloatValue();
      this.Tdc_Char_Curve_Values[19] = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_19.ToString()].GetFloatValue();
      float num1 = ErrorInPercent / 100f;
      for (int index = 0; index < 20; ++index)
      {
        if ((double) this.Tdc_Char_Curve_Values[index] == double.NaN)
          return false;
        float num2 = numArray[index];
        float num3 = (float) (1.0 / (1.0 + (double) num1));
        if ((double) num2 < (double) Q_UpperLimit)
        {
          if ((double) num2 > (double) Qi)
            num3 = (float) (1.0 / (1.0 + (double) num1 * ((double) Q_UpperLimit - (double) num2) / ((double) Q_UpperLimit - (double) Qi)));
          this.Tdc_Char_Curve_Values[index] = this.Tdc_Char_Curve_Values[index] * num3;
          this.Tdc_Char_Curve_Values[index] = CalibrationMathematik.CalibrationFloatPrecision(16U, this.Tdc_Char_Curve_Values[index]);
        }
      }
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[0]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_1.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[1]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_2.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[2]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_3.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[3]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_4.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[4]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_5.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[5]);
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_6.ToString()].SetFloatValue(this.Tdc_Char_Curve_Values[6]);
      SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
      TDC_Calibration.TdcCalibationParameter calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_7;
      string key1 = calibationParameter.ToString();
      parameterByName1[key1].SetFloatValue(this.Tdc_Char_Curve_Values[7]);
      SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_8;
      string key2 = calibationParameter.ToString();
      parameterByName2[key2].SetFloatValue(this.Tdc_Char_Curve_Values[8]);
      SortedList<string, S3_Parameter> parameterByName3 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_9;
      string key3 = calibationParameter.ToString();
      parameterByName3[key3].SetFloatValue(this.Tdc_Char_Curve_Values[9]);
      SortedList<string, S3_Parameter> parameterByName4 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_10;
      string key4 = calibationParameter.ToString();
      parameterByName4[key4].SetFloatValue(this.Tdc_Char_Curve_Values[10]);
      SortedList<string, S3_Parameter> parameterByName5 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_11;
      string key5 = calibationParameter.ToString();
      parameterByName5[key5].SetFloatValue(this.Tdc_Char_Curve_Values[11]);
      SortedList<string, S3_Parameter> parameterByName6 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_12;
      string key6 = calibationParameter.ToString();
      parameterByName6[key6].SetFloatValue(this.Tdc_Char_Curve_Values[12]);
      SortedList<string, S3_Parameter> parameterByName7 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_13;
      string key7 = calibationParameter.ToString();
      parameterByName7[key7].SetFloatValue(this.Tdc_Char_Curve_Values[13]);
      SortedList<string, S3_Parameter> parameterByName8 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_14;
      string key8 = calibationParameter.ToString();
      parameterByName8[key8].SetFloatValue(this.Tdc_Char_Curve_Values[14]);
      SortedList<string, S3_Parameter> parameterByName9 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_15;
      string key9 = calibationParameter.ToString();
      parameterByName9[key9].SetFloatValue(this.Tdc_Char_Curve_Values[15]);
      SortedList<string, S3_Parameter> parameterByName10 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_16;
      string key10 = calibationParameter.ToString();
      parameterByName10[key10].SetFloatValue(this.Tdc_Char_Curve_Values[16]);
      SortedList<string, S3_Parameter> parameterByName11 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_17;
      string key11 = calibationParameter.ToString();
      parameterByName11[key11].SetFloatValue(this.Tdc_Char_Curve_Values[17]);
      SortedList<string, S3_Parameter> parameterByName12 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_18;
      string key12 = calibationParameter.ToString();
      parameterByName12[key12].SetFloatValue(this.Tdc_Char_Curve_Values[18]);
      SortedList<string, S3_Parameter> parameterByName13 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_19;
      string key13 = calibationParameter.ToString();
      parameterByName13[key13].SetFloatValue(this.Tdc_Char_Curve_Values[19]);
      return true;
    }

    private bool AdjustTdcVolumeMatrixQi(float Qi, float Q_UpperLimit, float ErrorInPercent)
    {
      if ((double) Qi >= (double) Q_UpperLimit)
        throw new Exception("Qi >= Q");
      if ((double) ErrorInPercent <= -100.0 && (double) ErrorInPercent >= 100.0)
        throw new ArgumentOutOfRangeException(nameof (ErrorInPercent), "Value is out of range and more than +-100%");
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      S3_Parameter s3Parameter = workMeter.MyParameters.ParameterByName[S3_ParameterNames.TDC_MapTemp.ToString()];
      if (s3Parameter.Statics.S3_VarType != S3_VariableTypes.TDC_Matrix)
        return false;
      int minValue = (int) s3Parameter.Statics.MinValue;
      int maxValue = (int) s3Parameter.Statics.MaxValue;
      float[] numArray1 = new float[maxValue];
      float[,] numArray2 = new float[minValue, maxValue];
      int Address1 = s3Parameter.BlockStartAddress + minValue * 2;
      int index1 = 0;
      while (index1 < maxValue)
      {
        numArray1[index1] = workMeter.MyDeviceMemory.GetFloatValue(Address1);
        ++index1;
        Address1 += 4;
      }
      for (int index2 = 0; index2 < minValue; ++index2)
      {
        int index3 = 0;
        while (index3 < maxValue)
        {
          numArray2[index2, index3] = workMeter.MyDeviceMemory.GetFloatValue(Address1);
          ++index3;
          Address1 += 4;
        }
      }
      float num1 = ErrorInPercent / 100f;
      for (int index4 = 0; index4 < minValue; ++index4)
      {
        for (int index5 = 0; index5 < maxValue; ++index5)
        {
          if ((double) numArray2[index4, index5] == double.NaN)
            return false;
          float num2 = numArray1[index5];
          float num3 = (float) (1.0 / (1.0 + (double) num1));
          if ((double) num2 < (double) Q_UpperLimit)
          {
            if ((double) num2 > (double) Qi)
              num3 = (float) (1.0 / (1.0 + (double) num1 * ((double) Q_UpperLimit - (double) num2) / ((double) Q_UpperLimit - (double) Qi)));
            numArray2[index4, index5] *= num3;
            numArray2[index4, index5] = CalibrationMathematik.CalibrationFloatPrecision(16U, numArray2[index4, index5]);
          }
        }
      }
      int Address2 = s3Parameter.BlockStartAddress + minValue * 2 + maxValue * 4;
      for (int index6 = 0; index6 < minValue; ++index6)
      {
        int index7 = 0;
        while (index7 < maxValue)
        {
          workMeter.MyDeviceMemory.SetFloatValue(Address2, numArray2[index6, index7]);
          ++index7;
          Address2 += 4;
        }
      }
      return true;
    }

    internal override bool AdjustVolumeCalibration(
      float Qi,
      float QiErrorInPercent,
      float Q,
      float QErrorInPercent,
      float Qp,
      float QpErrorInPercent)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (workMeter == null)
        return false;
      SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
      TDC_Calibration.TdcCalibationParameter calibationParameter = TDC_Calibration.TdcCalibationParameter.TDC_MapTemp;
      string key1 = calibationParameter.ToString();
      if (parameterByName1.ContainsKey(key1))
      {
        S3_Parameter s3Parameter = workMeter.MyParameters.ParameterByName[S3_ParameterNames.TDC_MapTemp.ToString()];
        if (s3Parameter.Statics.S3_VarType != S3_VariableTypes.TDC_Matrix)
          return false;
        int minValue = (int) s3Parameter.Statics.MinValue;
        int maxValue = (int) s3Parameter.Statics.MaxValue;
        CalibrationMathematik calibrationMathematik = new CalibrationMathematik(minValue, maxValue);
        int Address1 = s3Parameter.BlockStartAddress + minValue * 2;
        int index1 = 0;
        while (index1 < maxValue)
        {
          calibrationMathematik.flowValues[index1] = workMeter.MyDeviceMemory.GetFloatValue(Address1);
          ++index1;
          Address1 += 4;
        }
        for (int index2 = 0; index2 < minValue; ++index2)
        {
          int index3 = 0;
          while (index3 < maxValue)
          {
            calibrationMathematik.originalVolumeMatrix[index2, index3] = workMeter.MyDeviceMemory.GetFloatValue(Address1);
            ++index3;
            Address1 += 4;
          }
        }
        if (!calibrationMathematik.CalibrateMatrix(Qi, QiErrorInPercent, Q, QErrorInPercent, Qp, QpErrorInPercent))
          return false;
        int Address2 = s3Parameter.BlockStartAddress + minValue * 2 + maxValue * 4;
        for (int index4 = 0; index4 < minValue; ++index4)
        {
          int index5 = 0;
          while (index5 < maxValue)
          {
            workMeter.MyDeviceMemory.SetFloatValue(Address2, calibrationMathematik.ResultVolumeMatrix[index4, index5]);
            ++index5;
            Address2 += 4;
          }
        }
      }
      SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
      calibationParameter = TDC_Calibration.TdcCalibationParameter.Con_volcorr_y_value_0;
      string key2 = calibationParameter.ToString();
      if (!parameterByName2.ContainsKey(key2))
        return false;
      S3_Parameter s3Parameter1 = workMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_volcorr_x_value_0.ToString()];
      int rows = 1;
      int columns = 20;
      CalibrationMathematik calibrationMathematik1 = new CalibrationMathematik(rows, columns);
      int blockStartAddress = s3Parameter1.BlockStartAddress;
      int index6 = 0;
      while (index6 < columns)
      {
        calibrationMathematik1.flowValues[index6] = workMeter.MyDeviceMemory.GetFloatValue(blockStartAddress);
        ++index6;
        blockStartAddress += 4;
      }
      for (int index7 = 0; index7 < rows; ++index7)
      {
        int index8 = 0;
        while (index8 < columns)
        {
          calibrationMathematik1.originalVolumeMatrix[index7, index8] = workMeter.MyDeviceMemory.GetFloatValue(blockStartAddress);
          ++index8;
          blockStartAddress += 4;
        }
      }
      if (!calibrationMathematik1.CalibrateMatrix(Qi, QiErrorInPercent, Q, QErrorInPercent, Qp, QpErrorInPercent))
        return false;
      int Address = s3Parameter1.BlockStartAddress + columns * 4;
      for (int index9 = 0; index9 < rows; ++index9)
      {
        int index10 = 0;
        while (index10 < columns)
        {
          workMeter.MyDeviceMemory.SetFloatValue(Address, calibrationMathematik1.ResultVolumeMatrix[index9, index10]);
          ++index10;
          Address += 4;
        }
      }
      return true;
    }

    internal bool CalibrateZeroFlow(
      int numberOfLoops,
      int mediaFilterRange,
      float temperature,
      out TDC_Calibration.CalibrationInfo calibrationInfo)
    {
      if (mediaFilterRange < 1 || mediaFilterRange > numberOfLoops)
        throw new Exception("mediaFilterRange out of limits");
      if ((mediaFilterRange & 1) == 0)
        throw new Exception("equal mediaFilterRange is not allowed");
      calibrationInfo = new TDC_Calibration.CalibrationInfo();
      calibrationInfo.counterValuesUp = new uint[4, numberOfLoops];
      calibrationInfo.counterValuesDown = new uint[4, numberOfLoops];
      calibrationInfo.diffValues = new int[4, numberOfLoops];
      calibrationInfo.meanDiffValues = new int[4];
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (!this.RunTdcLoop(numberOfLoops))
        return false;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < numberOfLoops; ++index2)
        {
          calibrationInfo.counterValuesUp[index1, index2] = this.Tdc_Counter_Values[index1, index2];
          calibrationInfo.counterValuesDown[index1, index2] = this.Tdc_Counter_Values[index1 + 4, index2];
          calibrationInfo.diffValues[index1, index2] = (int) this.Tdc_Counter_Values[index1, index2] - (int) this.Tdc_Counter_Values[index1 + 4, index2];
        }
      }
      List<int> intList = new List<int>(mediaFilterRange);
      int index3 = mediaFilterRange / 2;
      int num = numberOfLoops - mediaFilterRange + 1;
      for (int index4 = 0; index4 < 4; ++index4)
      {
        switch (numberOfLoops)
        {
          case 1:
            calibrationInfo.meanDiffValues[index4] = calibrationInfo.diffValues[index4, 0];
            break;
          case 2:
            calibrationInfo.meanDiffValues[index4] = (calibrationInfo.diffValues[index4, 0] + calibrationInfo.diffValues[index4, 1]) / 2;
            break;
          default:
            calibrationInfo.meanDiffValues[index4] = 0;
            for (int index5 = 0; index5 < num; ++index5)
            {
              intList.Clear();
              for (int index6 = 0; index6 < mediaFilterRange; ++index6)
                intList.Add(calibrationInfo.diffValues[index4, index5 + index6]);
              intList.Sort();
              calibrationInfo.meanDiffValues[index4] += intList[index3];
            }
            calibrationInfo.meanDiffValues[index4] /= num;
            break;
        }
      }
      byte byteValue1 = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_tdc_zero_crossing_t_diff.ToString()].GetByteValue();
      byte byteValue2 = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_tdc_zero_crossing_t_sum.ToString()].GetByteValue();
      byte indez_t_diff_1 = 0;
      byte indez_t_diff_2 = 0;
      byte indez_t_sum_1 = 0;
      byte indez_t_sum_2 = 0;
      switch (byteValue1)
      {
        case 1:
          indez_t_diff_1 = (byte) 0;
          indez_t_diff_2 = (byte) 4;
          break;
        case 2:
          indez_t_diff_1 = (byte) 1;
          indez_t_diff_2 = (byte) 5;
          break;
        case 4:
          indez_t_diff_1 = (byte) 2;
          indez_t_diff_2 = (byte) 6;
          break;
        case 8:
          indez_t_diff_1 = (byte) 3;
          indez_t_diff_2 = (byte) 7;
          break;
      }
      switch (byteValue2)
      {
        case 1:
          indez_t_sum_1 = (byte) 0;
          indez_t_sum_2 = (byte) 4;
          break;
        case 2:
          indez_t_sum_1 = (byte) 1;
          indez_t_sum_2 = (byte) 5;
          break;
        case 4:
          indez_t_sum_1 = (byte) 2;
          indez_t_sum_2 = (byte) 6;
          break;
        case 8:
          indez_t_sum_1 = (byte) 3;
          indez_t_sum_2 = (byte) 7;
          break;
      }
      for (int j = 0; j < 8; ++j)
      {
        this.Tdc_Counter_Mean_Values[j] = 0U;
        if (j == (int) indez_t_diff_1 || j == (int) indez_t_diff_2 || j == (int) indez_t_sum_1 || j == (int) indez_t_sum_2)
        {
          for (int i = 0; i < numberOfLoops; ++i)
          {
            if (this.Tdc_Counter_Values[j, i] == 0U)
              return this.SetCalibrationError(indez_t_diff_1, indez_t_diff_2, indez_t_sum_1, indez_t_sum_2, i, j);
            this.Tdc_Counter_Mean_Values[j] += this.Tdc_Counter_Values[j, i];
          }
          this.Tdc_Counter_Mean_Values[j] = this.Tdc_Counter_Mean_Values[j] / (uint) numberOfLoops;
        }
      }
      if (!workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp.ToString()].SetShortValue((short) ((double) temperature * 100.0)) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_up0.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[0]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_up1.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[1]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_up2.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[2]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_up3.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[3]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_down0.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[4]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_down1.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[5]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_down2.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[6]) || !workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_down3.ToString()].SetUintValue(this.Tdc_Counter_Mean_Values[7]))
        return false;
      float floatValue = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationRamParameter.tdc_sonic_velocity.ToString()].GetFloatValue();
      if (!workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationStateParameter.Con_Tdc_Dfc_HighTemp_sonic_velocity.ToString()].SetFloatValue(floatValue))
        return false;
      switch (byteValue1)
      {
        case 1:
          calibrationInfo.tdcZeroCalibrationValue = calibrationInfo.meanDiffValues[0];
          break;
        case 2:
          calibrationInfo.tdcZeroCalibrationValue = calibrationInfo.meanDiffValues[1];
          break;
        case 4:
          calibrationInfo.tdcZeroCalibrationValue = calibrationInfo.meanDiffValues[2];
          break;
        case 8:
          calibrationInfo.tdcZeroCalibrationValue = calibrationInfo.meanDiffValues[3] / 3;
          break;
        default:
          return false;
      }
      int intValue = workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_tdc_cal_lag.ToString()].GetIntValue();
      workMeter.MyParameters.ParameterByName[TDC_Calibration.TdcCalibationParameter.Con_tdc_cal_lag.ToString()].SetIntValue(calibrationInfo.tdcZeroCalibrationValue);
      calibrationInfo.changeOfTdcZeroCalibrationValue = calibrationInfo.tdcZeroCalibrationValue - intValue;
      return true;
    }

    private bool SetCalibrationError(
      byte indez_t_diff_1,
      byte indez_t_diff_2,
      byte indez_t_sum_1,
      byte indez_t_sum_2,
      int i,
      int j)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Calibration error");
      stringBuilder.AppendLine("indez_t_diff_1 = " + indez_t_diff_1.ToString());
      stringBuilder.AppendLine("indez_t_diff_2 = " + indez_t_diff_2.ToString());
      stringBuilder.AppendLine("indez_t_sum_1 = " + indez_t_sum_1.ToString());
      stringBuilder.AppendLine("indez_t_sum_2 = " + indez_t_sum_2.ToString());
      stringBuilder.AppendLine("i = " + i.ToString());
      stringBuilder.AppendLine("j = " + j.ToString());
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, stringBuilder.ToString());
    }

    internal bool RunTdcLoop(int numberOfLoops)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      this.MyFunctions.BreakRequest = false;
      this.Tdc_Counter_Values = new uint[8, numberOfLoops];
      this.Tdc_Counter_T_Diff_Values = new int[4, numberOfLoops];
      this.Tdc_Counter_T_Diff_Median_Values = new int[4, numberOfLoops - 2];
      this.TdcImpulseWidhtUp = new uint[numberOfLoops];
      this.TdcImpulseWidhtDown = new uint[numberOfLoops];
      DynamicParameterRange theRange1;
      workMeter.MyParameters.GetParameterRange(TDC_Calibration.cycleParameterVolAndTempCounter, out theRange1);
      DynamicParameterRange theRange2;
      workMeter.MyParameters.GetParameterRange(TDC_Calibration.tdc_n_upx_down_Parameter, out theRange2);
      DynamicParameterRange theRange3;
      workMeter.MyParameters.GetParameterRange(TDC_Calibration.tdc_n_pw_Parameter, out theRange3);
      byte byteValue = workMeter.MyParameters.ParameterByName[S3_ParameterNames.volumeCycleTimeCounterInit.ToString()].GetByteValue();
      S3_Parameter s3Parameter1 = workMeter.MyParameters.ParameterByName["radioCycleTimeCounter"];
      s3Parameter1.SetByteValue((byte) 100);
      s3Parameter1.WriteParameterToConnectedDevice();
      S3_Parameter s3Parameter2 = workMeter.MyParameters.ParameterByName["volumeCycleTimeCounter"];
      s3Parameter2.SetByteValue((byte) 1);
      if (workMeter.MyParameters.ParameterByName.ContainsKey("volumeCycleReduceCounter"))
        workMeter.MyParameters.ParameterByName["volumeCycleReduceCounter"].SetByteValue((byte) 0);
      S3_Parameter s3Parameter3 = workMeter.MyParameters.ParameterByName["temperaturCycleTimeCounter"];
      s3Parameter3.SetByteValue((byte) 100);
      S3_Parameter s3Parameter4 = workMeter.MyParameters.ParameterByName["temperaturCycleTimeSlotCounter"];
      s3Parameter4.SetByteValue((byte) 100);
      S3_Parameter s3Parameter5 = workMeter.MyParameters.ParameterByName["tdc_cal_resonator_counter"];
      s3Parameter5.SetByteValue((byte) 0);
      s3Parameter5.WriteParameterToConnectedDevice();
      if (!workMeter.MyDeviceMemory.WriteDataToConnectedDevice(theRange1.minAddress, theRange1.byteSize))
        return false;
      Thread.Sleep(1200);
      s3Parameter5.SetByteValue((byte) 127);
      s3Parameter5.WriteParameterToConnectedDevice();
      for (int index = 0; index < 2; ++index)
      {
        if (!workMeter.MyDeviceMemory.WriteDataToConnectedDevice(theRange1.minAddress, theRange1.byteSize))
          return false;
        Thread.Sleep(1200);
      }
      int num = (int) byteValue * 500;
      for (int index1 = 0; index1 < numberOfLoops; ++index1)
      {
        this.MyFunctions.SendMessage((object) this, 100 / numberOfLoops * index1, GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        if (this.MyFunctions.BreakRequest)
          return false;
        TDC_Calibration.TdcCalibationRamParameter calibationRamParameter;
        if (byteValue > (byte) 0)
        {
          if (!workMeter.MyDeviceMemory.WriteDataToConnectedDevice(theRange1.minAddress, theRange1.byteSize))
            return false;
          Thread.Sleep(1200);
        }
        else
        {
          DateTime now;
          DateTime dateTime1;
          if (index1 == 0)
          {
            uint[] numArray1 = new uint[8];
            if (!workMeter.MyDeviceMemory.ReadDataFromConnectedDevice(theRange2.minAddress, theRange2.byteSize))
              return false;
            uint[] numArray2 = numArray1;
            SortedList<string, S3_Parameter> parameterByName1 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up0;
            string key1 = calibationRamParameter.ToString();
            int uintValue1 = (int) parameterByName1[key1].GetUintValue();
            numArray2[0] = (uint) uintValue1;
            uint[] numArray3 = numArray1;
            SortedList<string, S3_Parameter> parameterByName2 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up1;
            string key2 = calibationRamParameter.ToString();
            int uintValue2 = (int) parameterByName2[key2].GetUintValue();
            numArray3[1] = (uint) uintValue2;
            uint[] numArray4 = numArray1;
            SortedList<string, S3_Parameter> parameterByName3 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up2;
            string key3 = calibationRamParameter.ToString();
            int uintValue3 = (int) parameterByName3[key3].GetUintValue();
            numArray4[2] = (uint) uintValue3;
            uint[] numArray5 = numArray1;
            SortedList<string, S3_Parameter> parameterByName4 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up3;
            string key4 = calibationRamParameter.ToString();
            int uintValue4 = (int) parameterByName4[key4].GetUintValue();
            numArray5[3] = (uint) uintValue4;
            uint[] numArray6 = numArray1;
            SortedList<string, S3_Parameter> parameterByName5 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down0;
            string key5 = calibationRamParameter.ToString();
            int uintValue5 = (int) parameterByName5[key5].GetUintValue();
            numArray6[4] = (uint) uintValue5;
            uint[] numArray7 = numArray1;
            SortedList<string, S3_Parameter> parameterByName6 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down1;
            string key6 = calibationRamParameter.ToString();
            int uintValue6 = (int) parameterByName6[key6].GetUintValue();
            numArray7[5] = (uint) uintValue6;
            uint[] numArray8 = numArray1;
            SortedList<string, S3_Parameter> parameterByName7 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down2;
            string key7 = calibationRamParameter.ToString();
            int uintValue7 = (int) parameterByName7[key7].GetUintValue();
            numArray8[6] = (uint) uintValue7;
            uint[] numArray9 = numArray1;
            SortedList<string, S3_Parameter> parameterByName8 = workMeter.MyParameters.ParameterByName;
            calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down3;
            string key8 = calibationRamParameter.ToString();
            int uintValue8 = (int) parameterByName8[key8].GetUintValue();
            numArray9[7] = (uint) uintValue8;
            uint[] numArray10 = new uint[8];
            now = DateTime.Now;
            DateTime dateTime2 = now.AddSeconds(10.0);
            do
            {
              now = DateTime.Now;
              dateTime1 = now.AddMilliseconds((double) num);
              if (workMeter.MyDeviceMemory.ReadDataFromConnectedDevice(theRange2.minAddress, theRange2.byteSize))
              {
                uint[] numArray11 = numArray10;
                SortedList<string, S3_Parameter> parameterByName9 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up0;
                string key9 = calibationRamParameter.ToString();
                int uintValue9 = (int) parameterByName9[key9].GetUintValue();
                numArray11[0] = (uint) uintValue9;
                uint[] numArray12 = numArray10;
                SortedList<string, S3_Parameter> parameterByName10 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up1;
                string key10 = calibationRamParameter.ToString();
                int uintValue10 = (int) parameterByName10[key10].GetUintValue();
                numArray12[1] = (uint) uintValue10;
                uint[] numArray13 = numArray10;
                SortedList<string, S3_Parameter> parameterByName11 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up2;
                string key11 = calibationRamParameter.ToString();
                int uintValue11 = (int) parameterByName11[key11].GetUintValue();
                numArray13[2] = (uint) uintValue11;
                uint[] numArray14 = numArray10;
                SortedList<string, S3_Parameter> parameterByName12 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up3;
                string key12 = calibationRamParameter.ToString();
                int uintValue12 = (int) parameterByName12[key12].GetUintValue();
                numArray14[3] = (uint) uintValue12;
                uint[] numArray15 = numArray10;
                SortedList<string, S3_Parameter> parameterByName13 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down0;
                string key13 = calibationRamParameter.ToString();
                int uintValue13 = (int) parameterByName13[key13].GetUintValue();
                numArray15[4] = (uint) uintValue13;
                uint[] numArray16 = numArray10;
                SortedList<string, S3_Parameter> parameterByName14 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down1;
                string key14 = calibationRamParameter.ToString();
                int uintValue14 = (int) parameterByName14[key14].GetUintValue();
                numArray16[5] = (uint) uintValue14;
                uint[] numArray17 = numArray10;
                SortedList<string, S3_Parameter> parameterByName15 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down2;
                string key15 = calibationRamParameter.ToString();
                int uintValue15 = (int) parameterByName15[key15].GetUintValue();
                numArray17[6] = (uint) uintValue15;
                uint[] numArray18 = numArray10;
                SortedList<string, S3_Parameter> parameterByName16 = workMeter.MyParameters.ParameterByName;
                calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down3;
                string key16 = calibationRamParameter.ToString();
                int uintValue16 = (int) parameterByName16[key16].GetUintValue();
                numArray18[7] = (uint) uintValue16;
                if ((int) numArray10[0] != (int) numArray1[0] || (int) numArray10[1] != (int) numArray1[1] || (int) numArray10[2] != (int) numArray1[2] || (int) numArray10[3] != (int) numArray1[3] || (int) numArray10[4] != (int) numArray1[4] || (int) numArray10[5] != (int) numArray1[5] || (int) numArray10[6] != (int) numArray1[6] || (int) numArray10[7] != (int) numArray1[7])
                  goto label_26;
              }
              else
                goto label_20;
            }
            while (!(DateTime.Now > dateTime2));
            goto label_23;
label_20:
            return false;
label_23:
            return false;
          }
          now = DateTime.Now;
          dateTime1 = now.AddMilliseconds((double) num);
label_26:
          s3Parameter2.ReadParameterFromConnectedDevice();
          s3Parameter2.GetByteValue();
          Thread.Sleep((int) dateTime1.Subtract(DateTime.Now).TotalMilliseconds);
        }
        if (!workMeter.MyDeviceMemory.ReadDataFromConnectedDevice(theRange2.minAddress, theRange2.byteSize) || !workMeter.MyDeviceMemory.ReadDataFromConnectedDevice(theRange3.minAddress, theRange3.byteSize))
          return false;
        this.TdcImpulseWidhtUp[index1] = (uint) workMeter.MyParameters.ParameterByName[S3_ParameterNames.tdc_pw_up.ToString()].GetByteValue();
        this.TdcImpulseWidhtDown[index1] = (uint) workMeter.MyParameters.ParameterByName[S3_ParameterNames.tdc_pw_down.ToString()].GetByteValue();
        uint[,] tdcCounterValues1 = this.Tdc_Counter_Values;
        int index2 = index1;
        SortedList<string, S3_Parameter> parameterByName17 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up0;
        string key17 = calibationRamParameter.ToString();
        int uintValue17 = (int) parameterByName17[key17].GetUintValue();
        tdcCounterValues1[0, index2] = (uint) uintValue17;
        uint[,] tdcCounterValues2 = this.Tdc_Counter_Values;
        int index3 = index1;
        SortedList<string, S3_Parameter> parameterByName18 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up1;
        string key18 = calibationRamParameter.ToString();
        int uintValue18 = (int) parameterByName18[key18].GetUintValue();
        tdcCounterValues2[1, index3] = (uint) uintValue18;
        uint[,] tdcCounterValues3 = this.Tdc_Counter_Values;
        int index4 = index1;
        SortedList<string, S3_Parameter> parameterByName19 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up2;
        string key19 = calibationRamParameter.ToString();
        int uintValue19 = (int) parameterByName19[key19].GetUintValue();
        tdcCounterValues3[2, index4] = (uint) uintValue19;
        uint[,] tdcCounterValues4 = this.Tdc_Counter_Values;
        int index5 = index1;
        SortedList<string, S3_Parameter> parameterByName20 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_up3;
        string key20 = calibationRamParameter.ToString();
        int uintValue20 = (int) parameterByName20[key20].GetUintValue();
        tdcCounterValues4[3, index5] = (uint) uintValue20;
        uint[,] tdcCounterValues5 = this.Tdc_Counter_Values;
        int index6 = index1;
        SortedList<string, S3_Parameter> parameterByName21 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down0;
        string key21 = calibationRamParameter.ToString();
        int uintValue21 = (int) parameterByName21[key21].GetUintValue();
        tdcCounterValues5[4, index6] = (uint) uintValue21;
        uint[,] tdcCounterValues6 = this.Tdc_Counter_Values;
        int index7 = index1;
        SortedList<string, S3_Parameter> parameterByName22 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down1;
        string key22 = calibationRamParameter.ToString();
        int uintValue22 = (int) parameterByName22[key22].GetUintValue();
        tdcCounterValues6[5, index7] = (uint) uintValue22;
        uint[,] tdcCounterValues7 = this.Tdc_Counter_Values;
        int index8 = index1;
        SortedList<string, S3_Parameter> parameterByName23 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down2;
        string key23 = calibationRamParameter.ToString();
        int uintValue23 = (int) parameterByName23[key23].GetUintValue();
        tdcCounterValues7[6, index8] = (uint) uintValue23;
        uint[,] tdcCounterValues8 = this.Tdc_Counter_Values;
        int index9 = index1;
        SortedList<string, S3_Parameter> parameterByName24 = workMeter.MyParameters.ParameterByName;
        calibationRamParameter = TDC_Calibration.TdcCalibationRamParameter.tdc_n_down3;
        string key24 = calibationRamParameter.ToString();
        int uintValue24 = (int) parameterByName24[key24].GetUintValue();
        tdcCounterValues8[7, index9] = (uint) uintValue24;
      }
      s3Parameter3.SetByteValue((byte) 0);
      s3Parameter4.SetByteValue((byte) 0);
      return workMeter.MyDeviceMemory.WriteDataToConnectedDevice(theRange1.minAddress, theRange1.byteSize);
    }

    internal bool IsUltrasonicVolumeMeterReady(out IufCheckResults checkResultes)
    {
      checkResultes = (IufCheckResults) null;
      ZR_ClassLibMessages.ClearErrors();
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      if (this.RunTdcLoop(2))
      {
        for (int index1 = 0; index1 < 2; ++index1)
        {
          if (this.TdcImpulseWidhtUp[index1] >= 2U && this.TdcImpulseWidhtDown[index1] >= 2U)
          {
            for (int index2 = 0; index2 < 1; ++index2)
            {
              if (this.Tdc_Counter_Values[index2, index1] == 0U || this.Tdc_Counter_Values[index2 + 4, index1] == 0U)
                goto label_10;
            }
          }
          else
            goto label_10;
        }
        checkResultes = new IufCheckResults();
        checkResultes.FirstPulsWidthUp = this.TdcImpulseWidhtUp[0];
        checkResultes.FirstPulsWidthDown = this.TdcImpulseWidhtDown[0];
        checkResultes.TimeCounterUp = this.Tdc_Counter_Values[0, 0];
        checkResultes.TimeCounterDown = this.Tdc_Counter_Values[4, 0];
        checkResultes.UltrasonicState = new S3P_Sta_Status(workMeter).UltrasonicState;
        return true;
      }
label_10:
      return false;
    }

    internal bool GetIUFcalibrationCurve(out SortedList<double, double> calibrationCurve)
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      string str1 = "Con_volcorr_x_value_";
      string str2 = "Con_volcorr_y_value_";
      bool fcalibrationCurve = true;
      calibrationCurve = new SortedList<double, double>();
      for (int index = 0; index < 20; ++index)
      {
        string key1 = str1 + index.ToString();
        string key2 = str2 + index.ToString();
        double floatValue1 = (double) workMeter.MyParameters.ParameterByName[key1].GetFloatValue();
        double floatValue2 = (double) workMeter.MyParameters.ParameterByName[key2].GetFloatValue();
        if (floatValue2 > 100.0 || floatValue2 < 0.01 || floatValue2 == double.NaN)
          fcalibrationCurve = false;
        calibrationCurve.Add(floatValue1, floatValue2);
      }
      return fcalibrationCurve;
    }

    internal enum CalibrationStateVol
    {
      none,
      cold,
      zeroFlowCalibrated,
      complete,
    }

    private enum TdcCalibationStateParameter
    {
      Con_Tdc_Dfc_LowTemp,
      Con_Tdc_Dfc_HighTemp,
      Con_Tdc_Dfc_LowTemp_up0,
      Con_Tdc_Dfc_LowTemp_up1,
      Con_Tdc_Dfc_LowTemp_up2,
      Con_Tdc_Dfc_LowTemp_up3,
      Con_Tdc_Dfc_HighTemp_up0,
      Con_Tdc_Dfc_HighTemp_up1,
      Con_Tdc_Dfc_HighTemp_up2,
      Con_Tdc_Dfc_HighTemp_up3,
      Con_Tdc_Dfc_LowTemp_down0,
      Con_Tdc_Dfc_LowTemp_down1,
      Con_Tdc_Dfc_LowTemp_down2,
      Con_Tdc_Dfc_LowTemp_down3,
      Con_Tdc_Dfc_HighTemp_down0,
      Con_Tdc_Dfc_HighTemp_down1,
      Con_Tdc_Dfc_HighTemp_down2,
      Con_Tdc_Dfc_HighTemp_down3,
      Con_Tdc_Dfc_LowTemp_sonic_velocity,
      Con_Tdc_Dfc_HighTemp_sonic_velocity,
    }

    private enum TdcCalibationParameter
    {
      Con_tdc_zero_crossing_t_diff,
      Con_tdc_zero_crossing_t_sum,
      Con_tdc_cal_sonic_velocity_slope,
      Con_tdc_cal_sonic_velocity_offset,
      Con_tdc_cal_lag,
      Con_volcorr_y_value_0,
      Con_volcorr_y_value_1,
      Con_volcorr_y_value_2,
      Con_volcorr_y_value_3,
      Con_volcorr_y_value_4,
      Con_volcorr_y_value_5,
      Con_volcorr_y_value_6,
      Con_volcorr_y_value_7,
      Con_volcorr_y_value_8,
      Con_volcorr_y_value_9,
      Con_volcorr_y_value_10,
      Con_volcorr_y_value_11,
      Con_volcorr_y_value_12,
      Con_volcorr_y_value_13,
      Con_volcorr_y_value_14,
      Con_volcorr_y_value_15,
      Con_volcorr_y_value_16,
      Con_volcorr_y_value_17,
      Con_volcorr_y_value_18,
      Con_volcorr_y_value_19,
      TDC_MapTemp,
    }

    private enum TdcCalibationRamParameter
    {
      tdc_n_up0,
      tdc_n_up1,
      tdc_n_up2,
      tdc_n_up3,
      tdc_n_down0,
      tdc_n_down1,
      tdc_n_down2,
      tdc_n_down3,
      tdc_sonic_velocity,
    }

    internal class CalibrationInfo
    {
      internal uint[,] counterValuesUp;
      internal uint[,] counterValuesDown;
      internal int tdcZeroCalibrationValue;
      internal int changeOfTdcZeroCalibrationValue;
      internal int[,] diffValues;
      internal int[] meanDiffValues;
    }
  }
}
