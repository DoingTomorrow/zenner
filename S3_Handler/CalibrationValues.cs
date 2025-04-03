// Decompiled with JetBrains decompiler
// Type: S3_Handler.CalibrationValues
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;

#nullable disable
namespace S3_Handler
{
  internal class CalibrationValues
  {
    internal double CalVolMaxFlowLiterPerHour = double.NaN;
    internal double CalVolMaxErrorPercent = double.NaN;
    internal double CalVolNominalFlowLiterPerHour = double.NaN;
    internal double CalVolNominalErrorPercent = double.NaN;
    internal double CalVolMinFlowLiterPerHour = double.NaN;
    internal double CalVolMinErrorPercent = double.NaN;
    internal double CalFlowTempMinGrad = double.NaN;
    internal double CalFlowTempMinErrorPercent = double.NaN;
    internal double CalFlowTempMiddleGrad = double.NaN;
    internal double CalFlowTempMiddleErrorPercent = double.NaN;
    internal double CalFlowTempMaxGrad = double.NaN;
    internal double CalFlowTempMaxErrorPercent = double.NaN;
    internal double CalReturnTempMinGrad = double.NaN;
    internal double CalReturnTempMinErrorPercent = double.NaN;
    internal double CalReturnTempMiddleGrad = double.NaN;
    internal double CalReturnTempMiddleErrorPercent = double.NaN;
    internal double CalReturnTempMaxGrad = double.NaN;
    internal double CalReturnTempMaxErrorPercent = double.NaN;
    internal bool PreparedForOnePointVolumeCalibration = false;
    internal bool PreparedForThreePointVolumeCalibration = false;
    internal bool PreparedForTemperatureCalibration = false;

    internal bool IsCalibrationOk()
    {
      bool flag = false;
      if (!double.IsNaN(this.CalVolMaxFlowLiterPerHour) || !double.IsNaN(this.CalVolMaxErrorPercent) || !double.IsNaN(this.CalVolNominalFlowLiterPerHour) || !double.IsNaN(this.CalVolNominalErrorPercent) || !double.IsNaN(this.CalVolMinFlowLiterPerHour) || !double.IsNaN(this.CalVolMinErrorPercent) || !double.IsNaN(this.CalFlowTempMinGrad) || !double.IsNaN(this.CalFlowTempMinErrorPercent) || !double.IsNaN(this.CalFlowTempMiddleGrad) || !double.IsNaN(this.CalFlowTempMiddleErrorPercent) || !double.IsNaN(this.CalFlowTempMaxGrad) || !double.IsNaN(this.CalFlowTempMaxErrorPercent) || !double.IsNaN(this.CalReturnTempMinGrad) || !double.IsNaN(this.CalReturnTempMinErrorPercent) || !double.IsNaN(this.CalReturnTempMiddleGrad) || !double.IsNaN(this.CalReturnTempMiddleErrorPercent) || !double.IsNaN(this.CalReturnTempMaxGrad) || !double.IsNaN(this.CalReturnTempMaxErrorPercent))
      {
        if (!double.IsNaN(this.CalVolMaxFlowLiterPerHour) || !double.IsNaN(this.CalVolMaxErrorPercent) || !double.IsNaN(this.CalVolNominalFlowLiterPerHour) || !double.IsNaN(this.CalVolNominalErrorPercent) || !double.IsNaN(this.CalVolMinFlowLiterPerHour) || !double.IsNaN(this.CalVolMinErrorPercent))
        {
          if (!double.IsNaN(this.CalVolMaxFlowLiterPerHour) && !double.IsNaN(this.CalVolMaxErrorPercent) && double.IsNaN(this.CalVolNominalFlowLiterPerHour) && double.IsNaN(this.CalVolNominalErrorPercent) && double.IsNaN(this.CalVolMinFlowLiterPerHour) && double.IsNaN(this.CalVolMinErrorPercent))
          {
            if (!this.IsPercentOk(this.CalVolMaxErrorPercent))
              throw new Exception("Volume calibration out of range");
            this.PreparedForOnePointVolumeCalibration = true;
            flag = true;
          }
          else
          {
            if (double.IsNaN(this.CalVolMaxFlowLiterPerHour) || double.IsNaN(this.CalVolMaxErrorPercent) || double.IsNaN(this.CalVolNominalFlowLiterPerHour) || double.IsNaN(this.CalVolNominalErrorPercent) || double.IsNaN(this.CalVolMinFlowLiterPerHour) || double.IsNaN(this.CalVolMinErrorPercent))
              throw new Exception("Invalied number of volume calibration values");
            if (!this.IsPercentOk(this.CalVolMaxErrorPercent) || !this.IsPercentOk(this.CalVolMaxErrorPercent) || !this.IsPercentOk(this.CalVolMinErrorPercent))
              throw new Exception("Volume calibration percent out of range");
            if (this.CalVolMaxFlowLiterPerHour < this.CalVolNominalFlowLiterPerHour * 1.3 || this.CalVolNominalFlowLiterPerHour < this.CalVolMinFlowLiterPerHour * 1.3)
              throw new Exception("Volume calibration flow values not in line");
            this.PreparedForThreePointVolumeCalibration = true;
            flag = true;
          }
        }
        if (!double.IsNaN(this.CalFlowTempMinGrad) || !double.IsNaN(this.CalFlowTempMinErrorPercent) || !double.IsNaN(this.CalFlowTempMiddleGrad) || !double.IsNaN(this.CalFlowTempMiddleErrorPercent) || !double.IsNaN(this.CalFlowTempMaxGrad) || !double.IsNaN(this.CalFlowTempMaxErrorPercent) || !double.IsNaN(this.CalReturnTempMinGrad) || !double.IsNaN(this.CalReturnTempMinErrorPercent) || !double.IsNaN(this.CalReturnTempMiddleGrad) || !double.IsNaN(this.CalReturnTempMiddleErrorPercent) || !double.IsNaN(this.CalReturnTempMaxGrad) || !double.IsNaN(this.CalReturnTempMaxErrorPercent))
        {
          if (double.IsNaN(this.CalFlowTempMinGrad) || double.IsNaN(this.CalFlowTempMinErrorPercent) || double.IsNaN(this.CalFlowTempMiddleGrad) || double.IsNaN(this.CalFlowTempMiddleErrorPercent) || double.IsNaN(this.CalFlowTempMaxGrad) || double.IsNaN(this.CalFlowTempMaxErrorPercent) || double.IsNaN(this.CalReturnTempMinGrad) || double.IsNaN(this.CalReturnTempMinErrorPercent) || double.IsNaN(this.CalReturnTempMiddleGrad) || double.IsNaN(this.CalReturnTempMiddleErrorPercent) || double.IsNaN(this.CalReturnTempMaxGrad) || double.IsNaN(this.CalReturnTempMaxErrorPercent))
            throw new Exception("Invalied number of temperature calibration values");
          if (!this.IsPercentOk(this.CalFlowTempMinErrorPercent) || !this.IsPercentOk(this.CalFlowTempMiddleErrorPercent) || !this.IsPercentOk(this.CalFlowTempMaxErrorPercent))
            throw new Exception("Temperature calibration out of range");
          this.PreparedForTemperatureCalibration = true;
          flag = true;
        }
      }
      return flag;
    }

    private bool IsPercentOk(double percentValue) => percentValue >= -50.0 && percentValue <= 200.0;
  }
}
