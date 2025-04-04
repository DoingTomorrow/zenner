// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_TDC_Calibration
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_TDC_Calibration
  {
    internal static Logger S4_TDC_CalibrationLogger = LogManager.GetLogger(nameof (S4_TDC_Calibration));
    private float[,] TDC_CalibrationCurves;
    private short[] TDC_CalibrationCurvesTemp;
    private float[] TDC_CalibrationCurvesFlow;

    public S4_TDC_Calibration()
    {
      this.TDC_CalibrationCurvesTemp = new short[5]
      {
        (short) 5,
        (short) 10,
        (short) 20,
        (short) 30,
        (short) 45
      };
      this.TDC_CalibrationCurvesFlow = new float[18]
      {
        0.008f,
        0.016f,
        0.033f,
        0.065f,
        0.104f,
        0.165f,
        0.31f,
        0.69f,
        1.7f,
        3.7f,
        8.8f,
        14f,
        17.5f,
        25f,
        28f,
        31.25f,
        40f,
        50f
      };
      this.TDC_CalibrationCurves = new float[5, 18]
      {
        {
          0.01f,
          0.02f,
          0.03f,
          0.04f,
          0.05f,
          0.06f,
          0.07f,
          0.08f,
          0.09f,
          0.1f,
          0.11f,
          0.12f,
          0.13f,
          0.14f,
          0.15f,
          0.16f,
          0.17f,
          0.18f
        },
        {
          0.1f,
          0.2f,
          0.3f,
          0.4f,
          0.5f,
          0.6f,
          0.7f,
          0.8f,
          0.9f,
          1f,
          1.1f,
          1.2f,
          1.3f,
          1.4f,
          1.5f,
          1.6f,
          1.7f,
          1.8f
        },
        {
          1f,
          2f,
          3f,
          4f,
          5f,
          6f,
          7f,
          8f,
          9f,
          10f,
          11f,
          12f,
          13f,
          14f,
          15f,
          16f,
          17f,
          18f
        },
        {
          10f,
          20f,
          30f,
          40f,
          50f,
          60f,
          70f,
          80f,
          90f,
          100f,
          110f,
          120f,
          130f,
          140f,
          150f,
          160f,
          170f,
          180f
        },
        {
          100f,
          200f,
          300f,
          400f,
          500f,
          600f,
          700f,
          800f,
          900f,
          1000f,
          1100f,
          1200f,
          1300f,
          1400f,
          1500f,
          1600f,
          1700f,
          1800f
        }
      };
    }

    internal void LoadCalibrationFromMemory(S4_DeviceMemory theMemory)
    {
      uint parameterAddress1 = theMemory.GetParameterAddress(S4_Params.TDC_MapTemp);
      for (int index = 0; index < this.TDC_CalibrationCurvesTemp.GetLength(0); ++index)
      {
        short int16 = BitConverter.ToInt16(theMemory.GetData(parameterAddress1, 2U), 0);
        this.TDC_CalibrationCurvesTemp[index] = int16;
        parameterAddress1 += 2U;
      }
      uint parameterAddress2 = theMemory.GetParameterAddress(S4_Params.TDC_MapFlow);
      for (int index = 0; index < this.TDC_CalibrationCurvesFlow.GetLength(0); ++index)
      {
        float single = BitConverter.ToSingle(theMemory.GetData(parameterAddress2, 4U), 0);
        this.TDC_CalibrationCurvesFlow[index] = single;
        parameterAddress2 += 4U;
      }
      uint parameterAddress3 = theMemory.GetParameterAddress(S4_Params.TDC_MapCoef);
      for (int index1 = 0; index1 < this.TDC_CalibrationCurves.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.TDC_CalibrationCurves.GetLength(1); ++index2)
        {
          float single = BitConverter.ToSingle(theMemory.GetData(parameterAddress3, 4U), 0);
          this.TDC_CalibrationCurves[index1, index2] = single;
          parameterAddress3 += 4U;
        }
      }
    }

    internal void CopyCalibrationToMemory(S4_DeviceMemory theMemory)
    {
      uint parameterAddress1 = theMemory.GetParameterAddress(S4_Params.TDC_MapTemp);
      for (int index = 0; index < this.TDC_CalibrationCurvesTemp.GetLength(0); ++index)
      {
        short num = this.TDC_CalibrationCurvesTemp[index];
        AddressRange setRange = new AddressRange(parameterAddress1, (uint) BitConverter.GetBytes(num).Length);
        theMemory.GarantMemoryAvailable(setRange);
        theMemory.SetData(parameterAddress1, BitConverter.GetBytes(num));
        parameterAddress1 += 2U;
      }
      uint parameterAddress2 = theMemory.GetParameterAddress(S4_Params.TDC_MapFlow);
      for (int index = 0; index < this.TDC_CalibrationCurvesFlow.GetLength(0); ++index)
      {
        float num = this.TDC_CalibrationCurvesFlow[index];
        AddressRange setRange = new AddressRange(parameterAddress1, (uint) BitConverter.GetBytes(num).Length);
        theMemory.GarantMemoryAvailable(setRange);
        theMemory.SetData(parameterAddress2, BitConverter.GetBytes(num));
        parameterAddress2 += 4U;
      }
      float[,,] numArray = new float[this.TDC_CalibrationCurves.GetLength(0), this.TDC_CalibrationCurves.GetLength(1), 2];
      uint parameterAddress3 = theMemory.GetParameterAddress(S4_Params.TDC_MapCoef);
      for (int index1 = 0; index1 < this.TDC_CalibrationCurves.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.TDC_CalibrationCurves.GetLength(1); ++index2)
        {
          float single = BitConverter.ToSingle(theMemory.GetData(parameterAddress3, 4U), 0);
          numArray[index1, index2, 0] = single;
          float calibrationCurve = this.TDC_CalibrationCurves[index1, index2];
          numArray[index1, index2, 1] = calibrationCurve;
          AddressRange setRange = new AddressRange(parameterAddress1, (uint) BitConverter.GetBytes(calibrationCurve).Length);
          theMemory.GarantMemoryAvailable(setRange);
          theMemory.SetData(parameterAddress3, BitConverter.GetBytes(calibrationCurve));
          parameterAddress3 += 4U;
        }
      }
    }

    internal void AdjustCalibration(SortedList<double, double> cf)
    {
      if (cf == null || cf.Count < 1)
        throw new ArgumentException("No calibration values available");
      double[] numArray = new double[this.TDC_CalibrationCurvesFlow.Length];
      for (int index1 = 0; index1 < this.TDC_CalibrationCurvesFlow.Length; ++index1)
      {
        if ((double) this.TDC_CalibrationCurvesFlow[index1] <= cf.Keys[0])
          numArray[index1] = cf.Values[0];
        else if ((double) this.TDC_CalibrationCurvesFlow[index1] >= cf.Keys[cf.Count - 1])
        {
          numArray[index1] = cf.Values[cf.Count - 1];
        }
        else
        {
          for (int index2 = 0; index2 < cf.Count - 1; ++index2)
          {
            if ((double) this.TDC_CalibrationCurvesFlow[index1] >= cf.Keys[index2] && (double) this.TDC_CalibrationCurvesFlow[index1] <= cf.Keys[index2 + 1])
            {
              double num1 = cf.Keys[index2 + 1] - cf.Keys[index2];
              double num2 = (double) this.TDC_CalibrationCurvesFlow[index1] - cf.Keys[index2];
              double num3 = cf.Values[index2];
              double num4 = cf.Values[index2 + 1] - cf.Values[index2];
              numArray[index1] = num3 + num4 / num1 * num2;
            }
          }
        }
      }
      for (int index3 = 0; index3 < this.TDC_CalibrationCurves.GetLength(0); ++index3)
      {
        for (int index4 = 0; index4 < this.TDC_CalibrationCurves.GetLength(1); ++index4)
        {
          float calibrationCurve = this.TDC_CalibrationCurves[index3, index4];
          this.TDC_CalibrationCurves[index3, index4] *= (float) numArray[index4];
          if (S4_TDC_Calibration.S4_TDC_CalibrationLogger.IsTraceEnabled)
            S4_TDC_Calibration.S4_TDC_CalibrationLogger.Trace("Temp:" + ((float) this.TDC_CalibrationCurvesTemp[index3] / 100f).ToString() + "; Flow:" + this.TDC_CalibrationCurvesFlow[index4].ToString() + "; CorFactor:" + numArray[index4].ToString() + "; valBefore:" + calibrationCurve.ToString() + "; valNow:" + this.TDC_CalibrationCurves[index3, index4].ToString());
        }
      }
    }

    internal void CopyZeroOffsetParameters(
      S4_DeviceMemory meterMemory,
      EventHandler<string> strEvent)
    {
      int parameterValue1 = meterMemory.GetParameterValue<int>(S4_Params.zeroOffsetMeasUnit1);
      int parameterValue2 = meterMemory.GetParameterValue<int>(S4_Params.zeroOffsetMeasUnit2);
      int parameterValue3 = meterMemory.GetParameterValue<int>(S4_Params.zeroOffsetUnit1);
      int parameterValue4 = meterMemory.GetParameterValue<int>(S4_Params.zeroOffsetUnit2);
      if (strEvent != null)
      {
        string e = "copy new data...\r\n" + parameterValue3.ToString() + " --> " + parameterValue1.ToString() + "\r\n" + parameterValue4.ToString() + " --> " + parameterValue2.ToString() + "\r\n";
        strEvent((object) this, e);
      }
      meterMemory.SetParameterValue<int>(S4_Params.zeroOffsetUnit1, parameterValue1);
      meterMemory.SetParameterValue<int>(S4_Params.zeroOffsetUnit2, parameterValue2);
    }

    internal string GetCalibrationChanges(S4_DeviceMemory theMemory, string info)
    {
      float[,] numArray1 = (float[,]) this.TDC_CalibrationCurves.Clone();
      short[] numArray2 = (short[]) this.TDC_CalibrationCurvesTemp.Clone();
      float[] numArray3 = (float[]) this.TDC_CalibrationCurvesFlow.Clone();
      this.LoadCalibrationFromMemory(theMemory);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("*** TDC calibration curves changes ***");
      if (info != null)
        stringBuilder.AppendLine(info);
      stringBuilder.AppendLine("[TimeIndex,FlowIndex]: CompareValue -> WorkValue = Change %");
      stringBuilder.AppendLine();
      for (int index1 = 0; index1 < this.TDC_CalibrationCurves.GetLength(0); ++index1)
      {
        stringBuilder.AppendLine("Temperature: " + ((float) this.TDC_CalibrationCurvesTemp[index1] / 100f).ToString());
        for (int index2 = 0; index2 < this.TDC_CalibrationCurves.GetLength(1); ++index2)
        {
          float calibrationCurve = this.TDC_CalibrationCurves[index1, index2];
          float num1 = numArray1[index1, index2];
          float num2 = (float) (100.0 / ((double) num1 / (double) calibrationCurve) - 100.0);
          stringBuilder.AppendLine("[" + index1.ToString("d02") + "," + index2.ToString("d02") + "] " + this.TDC_CalibrationCurvesFlow[index2].ToString("f3").PadLeft(6) + "m\u00B3/h  : " + calibrationCurve.ToString("f5") + "->" + num1.ToString("f5") + " = " + num2.ToString("f2").PadLeft(6) + "%");
        }
        stringBuilder.AppendLine();
      }
      this.TDC_CalibrationCurves = (float[,]) numArray1.Clone();
      this.TDC_CalibrationCurvesTemp = (short[]) numArray2.Clone();
      this.TDC_CalibrationCurvesFlow = (float[]) numArray3.Clone();
      return stringBuilder.ToString();
    }

    internal List<float> GetSmallCalibrationChanges(S4_DeviceMemory theMemory)
    {
      List<float> calibrationChanges = new List<float>();
      float[,] numArray = (float[,]) this.TDC_CalibrationCurves.Clone();
      this.LoadCalibrationFromMemory(theMemory);
      for (int index = 0; index < this.TDC_CalibrationCurves.GetLength(1); ++index)
      {
        float calibrationCurve = this.TDC_CalibrationCurves[0, index];
        float num = (float) (100.0 / ((double) numArray[0, index] / (double) calibrationCurve) - 100.0);
        calibrationChanges.Add(num);
      }
      return calibrationChanges;
    }
  }
}
