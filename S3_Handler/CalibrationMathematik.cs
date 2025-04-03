// Decompiled with JetBrains decompiler
// Type: S3_Handler.CalibrationMathematik
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  internal class CalibrationMathematik
  {
    private int rows;
    private int columns;

    internal float[] flowValues { get; set; }

    internal float[,] originalVolumeMatrix { get; set; }

    internal float[,] ResultVolumeMatrix { get; private set; }

    internal CalibrationMathematik(int rows, int columns)
    {
      this.rows = rows;
      this.columns = columns;
      this.flowValues = new float[columns];
      this.originalVolumeMatrix = new float[rows, columns];
      this.ResultVolumeMatrix = new float[rows, columns];
    }

    internal bool CalibrateMatrix(
      float Qi,
      float QiErrorInPercent,
      float Q,
      float QErrorInPercent,
      float Qp,
      float QpErrorInPercent)
    {
      if ((double) Qi >= (double) Q)
        throw new Exception("Qi >= Q");
      if ((double) Q >= (double) Qp)
        throw new Exception("Q >= Qp");
      if ((double) Qi >= (double) Qp)
        throw new Exception("Qi >= Qp");
      if ((double) QiErrorInPercent <= -100.0 && (double) QiErrorInPercent >= 100.0)
        throw new ArgumentOutOfRangeException(nameof (QiErrorInPercent), "Value is out of range and more than +-100%");
      if ((double) QErrorInPercent <= -100.0 && (double) QErrorInPercent >= 100.0)
        throw new ArgumentOutOfRangeException(nameof (QErrorInPercent), "Value is out of range and more than +-100%");
      if ((double) QpErrorInPercent <= -100.0 && (double) QpErrorInPercent >= 100.0)
        throw new ArgumentOutOfRangeException(nameof (QpErrorInPercent), "Value is out of range and more than +-100%");
      float num1 = QiErrorInPercent / 100f;
      float num2 = QErrorInPercent / 100f;
      float num3 = QpErrorInPercent / 100f;
      for (int index1 = 0; index1 < this.rows; ++index1)
      {
        for (int index2 = 0; index2 < this.columns; ++index2)
        {
          if ((double) this.originalVolumeMatrix[index1, index2] == double.NaN)
            return false;
          float flowValue = this.flowValues[index2];
          float num4 = 0.0f;
          if ((double) flowValue <= (double) Qi)
            num4 = (float) (1.0 / (1.0 + (double) num1));
          else if ((double) flowValue > (double) Qi && (double) flowValue <= (double) Q)
            num4 = (float) (1.0 / (1.0 + (double) ((float) (((double) num2 - (double) num1) / ((double) Q - (double) Qi) * ((double) flowValue - (double) Qi)) + num1)));
          else if ((double) flowValue > (double) Q && (double) flowValue <= (double) Qp)
            num4 = (float) (1.0 / (1.0 + (double) ((float) (((double) num3 - (double) num2) / ((double) Qp - (double) Q) * ((double) flowValue - (double) Q)) + num2)));
          else if ((double) flowValue > (double) Qp)
            num4 = (float) (1.0 / (1.0 + (double) num3));
          float floatValue = this.originalVolumeMatrix[index1, index2] * num4;
          this.ResultVolumeMatrix[index1, index2] = CalibrationMathematik.CalibrationFloatPrecision(16U, floatValue);
        }
      }
      return true;
    }

    internal static float CalibrationFloatPrecision(uint bitPrecision, float floatValue)
    {
      if (bitPrecision >= 24U)
        throw new ArgumentOutOfRangeException("bitPrecision is out of range !!!");
      uint num1 = 0;
      for (int index = 0; (long) index < (long) (uint) (23 - ((int) bitPrecision - 1)); ++index)
        num1 = (uint) ((int) num1 << 1 | 1);
      uint num2 = ~num1;
      return BitConverter.ToSingle(BitConverter.GetBytes(BitConverter.ToUInt32(BitConverter.GetBytes(floatValue), 0) & num2), 0);
    }
  }
}
