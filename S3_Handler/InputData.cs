// Decompiled with JetBrains decompiler
// Type: S3_Handler.InputData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class InputData
  {
    internal InputOutputFunctions inOutFunction;
    internal ResolutionData inputUnitInfo;
    internal ushort impulsValueDivisor;
    internal ushort impulsValueFactor;

    internal string inputResolutionString => this.inputUnitInfo.resolutionString;

    internal Decimal impulsValueDecimal
    {
      get => (Decimal) this.impulsValueFactor / (Decimal) this.impulsValueDivisor;
      set
      {
        double d = Convert.ToDouble(value);
        this.impulsValueDivisor = (ushort) 1;
        while (true)
        {
          double num = Math.Floor(d);
          if (d - num >= 1E-05 && d <= 6500.0 && this.impulsValueDivisor <= (ushort) 6500)
          {
            d *= 10.0;
            this.impulsValueDivisor *= (ushort) 10;
          }
          else
            break;
        }
        this.impulsValueFactor = (ushort) d;
      }
    }

    internal InputData()
    {
    }

    internal InputData(InputData dataToCopy)
    {
      this.inOutFunction = dataToCopy.inOutFunction;
      this.inputUnitInfo = dataToCopy.inputUnitInfo;
      this.impulsValueDivisor = dataToCopy.impulsValueDivisor;
      this.impulsValueFactor = dataToCopy.impulsValueFactor;
    }
  }
}
