// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_DifVif_Parameter
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using MBusLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  public class S4_DifVif_Parameter
  {
    internal const uint VIF_DefaultVolume = 255;
    internal const uint VIF_DefaultFlow = 511;
    internal const uint VIF_Temperature = 89;
    internal const uint VIF_Status = 6141;
    internal const uint VIF_DateAndTime = 109;
    internal const int Int32_Invalid = -2147483648;
    internal const byte SpacingControlMonthLogger = 52;
    internal const byte SpacingValueMonthLogger = 254;
    internal static SortedList<_UNIT_SCALE_, BaseUnitScaleValues> BaseUnitScale = new SortedList<_UNIT_SCALE_, BaseUnitScaleValues>();
    internal static SortedList<_UNIT_SCALE_, VolumeUnitBaseDefines> AllVolumeUnitBaseDefines;
    internal string Name = "?";
    internal string Info;
    internal DIF_Function DifFunction;
    internal int StorageNumber;
    internal int Tarif;
    internal _UNIT_SCALE_ UnitScale;
    internal _VAR_TYPE_ VarType;
    private bool IsCompactProfile = false;
    internal byte LVAR;
    internal byte SpacingControl;
    internal byte SpacingValue;

    internal static List<S4_DifVif_Parameter> PreDefParams { get; private set; }

    private S4_DifVif_Parameter()
    {
    }

    internal S4_DifVif_Parameter(
      string name,
      _UNIT_SCALE_ unitScale,
      int storageNumber,
      int tarif,
      DIF_Function difFunction,
      _VAR_TYPE_ varType,
      int? compactProfileMonth = null)
    {
      this.Name = name;
      this.UnitScale = unitScale;
      this.StorageNumber = storageNumber;
      this.Tarif = tarif;
      this.DifFunction = difFunction;
      this.VarType = varType;
      if (!compactProfileMonth.HasValue)
        return;
      this.IsCompactProfile = true;
      if (this.VarType != _VAR_TYPE_.VAR_LEN)
        throw new Exception("Compact profile needs VAR_LEN DIF");
      this.LVAR = (byte) (compactProfileMonth.Value * 4 + 2);
      this.SpacingControl = (byte) 52;
      this.SpacingValue = (byte) 254;
    }

    internal List<byte> GetDifVifBytes()
    {
      List<byte> difVifBytes = new List<byte>();
      int storageNumber = this.StorageNumber;
      int tarif = this.Tarif;
      byte num1 = (byte) ((uint) (byte) ((uint) (byte) (this.VarType & _VAR_TYPE_.SPECIAL_FUNC) | (uint) (byte) ((uint) (this.DifFunction & DIF_Function.Err) << 4)) | (uint) (byte) ((storageNumber & 1) << 6));
      int num2 = storageNumber >> 1;
      if (num2 > 0 || tarif > 0)
      {
        byte num3 = (byte) ((uint) num1 | 128U);
        difVifBytes.Add(num3);
        do
        {
          byte num4 = (byte) ((uint) (byte) (num2 & 15) | (uint) (byte) ((tarif & 3) << 4));
          num2 >>= 4;
          tarif >>= 2;
          if (num2 > 0 || tarif > 0)
            num4 |= (byte) 128;
          difVifBytes.Add(num4);
        }
        while (num2 > 0 || tarif > 0);
      }
      else
        difVifBytes.Add(num1);
      uint vifValue = S4_DifVif_Parameter.BaseUnitScale[this.UnitScale].VIF_value;
      while (true)
      {
        byte num5 = (byte) (vifValue & (uint) byte.MaxValue);
        difVifBytes.Add(num5);
        if (num5 >= (byte) 128)
          vifValue >>= 8;
        else
          break;
      }
      if (this.IsCompactProfile)
      {
        difVifBytes[difVifBytes.Count - 1] |= (byte) 128;
        difVifBytes.Add((byte) 19);
        difVifBytes.Add(this.LVAR);
        difVifBytes.Add(this.SpacingControl);
        difVifBytes.Add(this.SpacingValue);
      }
      return difVifBytes;
    }

    internal string GetNameAndValueFromProtocol(byte[] protocol, ref int offset)
    {
      return this.Name + ": " + this.GetValueFromProtocol(protocol, ref offset);
    }

    internal string GetValueFromProtocol(byte[] protocol, ref int offset)
    {
      string valueFromProtocol = "Value scaling not found";
      switch (this.VarType)
      {
        case _VAR_TYPE_.INT8_T:
          byte intValue1 = protocol[offset];
          ++offset;
          valueFromProtocol = this.ScaleValue((long) intValue1);
          break;
        case _VAR_TYPE_.INT16_T:
          if (this.UnitScale == _UNIT_SCALE_.U_date)
          {
            DateTime? dateMbusCp16TypeG = MBusUtil.ConvertToDate_MBus_CP16_TypeG(protocol, offset);
            valueFromProtocol = !dateMbusCp16TypeG.HasValue ? (((uint) protocol[offset] & 128U) <= 0U ? "Illegal Date MBus code type G: 0x" + BitConverter.ToInt16(protocol, offset).ToString("x04") : "invalid") : dateMbusCp16TypeG.Value.ToShortDateString();
          }
          else
          {
            short int16 = BitConverter.ToInt16(protocol, offset);
            valueFromProtocol = S4_DifVif_Parameter.GetPhysicalBase(this.UnitScale) != VIF_PhyicalBase.Temperature || int16 != short.MinValue ? this.ScaleValue((long) int16) : "invalid";
          }
          offset += 2;
          break;
        case _VAR_TYPE_.INT24_T:
          byte[] dst1 = new byte[4];
          Buffer.BlockCopy((Array) protocol, offset, (Array) dst1, 0, 3);
          offset += 3;
          dst1[3] = ((uint) dst1[2] & 128U) <= 0U ? (byte) 0 : byte.MaxValue;
          valueFromProtocol = this.ScaleValue((long) BitConverter.ToInt32(dst1, 0));
          break;
        case _VAR_TYPE_.INT32_T:
          if (this.UnitScale == _UNIT_SCALE_.U_dateTime)
          {
            DateTime? timeMbusCp32TypeF = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(protocol, offset);
            valueFromProtocol = !timeMbusCp32TypeF.HasValue ? (((uint) protocol[offset] & 128U) <= 0U ? "Illegal DateTime MBus code type F: 0x" + BitConverter.ToInt32(protocol, offset).ToString("x08") : "invalid") : timeMbusCp32TypeF.Value.ToShortDateString() + " " + timeMbusCp32TypeF.Value.ToShortTimeString();
          }
          else
          {
            int int32 = BitConverter.ToInt32(protocol, offset);
            valueFromProtocol = int32 != int.MinValue ? this.ScaleValue((long) int32) : "invalid";
          }
          offset += 4;
          break;
        case _VAR_TYPE_.FLOAT_T:
          float single = BitConverter.ToSingle(protocol, offset);
          offset += 4;
          valueFromProtocol = this.ScaleValue((double) single);
          break;
        case _VAR_TYPE_.INT48_T:
          byte[] dst2 = new byte[8];
          Buffer.BlockCopy((Array) protocol, offset, (Array) dst2, 0, 6);
          offset += 6;
          if (((uint) dst2[5] & 128U) > 0U)
          {
            dst2[6] = byte.MaxValue;
            dst2[7] = byte.MaxValue;
          }
          else
          {
            dst2[6] = (byte) 0;
            dst2[7] = (byte) 0;
          }
          valueFromProtocol = this.ScaleValue(BitConverter.ToInt64(dst2, 0));
          break;
        case _VAR_TYPE_.INT64_T:
          long int64 = BitConverter.ToInt64(protocol, offset);
          offset += 8;
          valueFromProtocol = this.ScaleValue(int64);
          break;
        case _VAR_TYPE_.VAR_LEN:
          int num = ((int) this.LVAR - 2) / 4;
          if (num < 1)
          {
            valueFromProtocol = "No logger values defined";
            break;
          }
          if (num * 4 + 2 != (int) this.LVAR)
          {
            valueFromProtocol = "Logger LVAR parameter not aligned";
            break;
          }
          if (this.SpacingControl != (byte) 52)
          {
            valueFromProtocol = "Logger SpacingControl not supported";
            break;
          }
          if (this.SpacingValue != (byte) 254)
          {
            valueFromProtocol = "Logger SpacingValue not supported";
            break;
          }
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < num; ++index)
          {
            if (index > 0)
              stringBuilder.Append(';');
            stringBuilder.Append("[" + index.ToString() + "]");
            int intValue2 = ByteArrayScanner.ScanInt32(protocol, ref offset);
            if (intValue2 == int.MinValue)
              stringBuilder.Append("invalid");
            else
              stringBuilder.Append(this.ScaleValue((long) intValue2));
          }
          valueFromProtocol = stringBuilder.ToString();
          break;
      }
      return valueFromProtocol;
    }

    public string ScaleValue(long intValue)
    {
      switch (S4_DifVif_Parameter.GetPhysicalBase(this.UnitScale))
      {
        case VIF_PhyicalBase.Volume:
        case VIF_PhyicalBase.Flow:
        case VIF_PhyicalBase.Temperature:
          return this.ScaleValue((double) intValue);
        case VIF_PhyicalBase.FunctionalState:
          return new S4_FunctionalState((ushort) intValue).ToString();
        case VIF_PhyicalBase.SystemState:
          return new S4_SystemState((uint) intValue).ToString();
        default:
          return "PhysicalBase not implemented. Poor value: " + intValue.ToString() + " = 0x" + intValue.ToString("x");
      }
    }

    public string ScaleValue(double doubleValue)
    {
      switch (this.UnitScale)
      {
        case _UNIT_SCALE_.U0_001L:
          doubleValue /= 1000000.0;
          return doubleValue.ToString("f6") + "m\u00B3";
        case _UNIT_SCALE_.U0_01L:
          doubleValue /= 100000.0;
          return doubleValue.ToString("f5") + "m\u00B3";
        case _UNIT_SCALE_.U0_1L:
          doubleValue /= 10000.0;
          return doubleValue.ToString("f4") + "m\u00B3";
        case _UNIT_SCALE_.U0_001qm:
          doubleValue /= 1000.0;
          return doubleValue.ToString("f3") + "m\u00B3";
        case _UNIT_SCALE_.U0_01qm:
          doubleValue /= 100.0;
          return doubleValue.ToString("f2") + "m\u00B3";
        case _UNIT_SCALE_.U0_1qm:
          doubleValue /= 10.0;
          return doubleValue.ToString("f1") + "m\u00B3";
        case _UNIT_SCALE_.U1_0qm:
          return doubleValue.ToString("f0") + "m\u00B3";
        case _UNIT_SCALE_.U10_0qm:
          doubleValue *= 10.0;
          return doubleValue.ToString("f0") + "m\u00B3";
        case _UNIT_SCALE_.U100_0qm:
          doubleValue *= 100.0;
          return doubleValue.ToString("f0") + "m\u00B3";
        case _UNIT_SCALE_.U0_000001USgal:
          doubleValue /= 1000000.0;
          return doubleValue.ToString("f6") + "US GAL";
        case _UNIT_SCALE_.U0_00001USgal:
          doubleValue /= 100000.0;
          return doubleValue.ToString("f5") + "US GAL";
        case _UNIT_SCALE_.U0_0001USgal:
          doubleValue /= 10000.0;
          return doubleValue.ToString("f4") + "US GAL";
        case _UNIT_SCALE_.U0_001USgal:
          doubleValue /= 1000.0;
          return doubleValue.ToString("f3") + "US GAL";
        case _UNIT_SCALE_.U0_01USgal:
          doubleValue /= 100.0;
          return doubleValue.ToString("f2") + "US GAL";
        case _UNIT_SCALE_.U0_1USgal:
          doubleValue /= 10.0;
          return doubleValue.ToString("f1") + "US GAL";
        case _UNIT_SCALE_.U1_0USgal:
          return doubleValue.ToString("f0") + "US GAL";
        case _UNIT_SCALE_.U10_0USgal:
          doubleValue *= 10.0;
          return doubleValue.ToString("f0") + "US GAL";
        case _UNIT_SCALE_.U100_0USgal:
          doubleValue *= 100.0;
          return doubleValue.ToString("f0") + "US GAL";
        case _UNIT_SCALE_.U0_001qmh:
          doubleValue /= 1000.0;
          return doubleValue.ToString("f3") + "m\u00B3/h";
        case _UNIT_SCALE_.U0_01qmh:
          doubleValue /= 100.0;
          return doubleValue.ToString("f2") + "m\u00B3/h";
        case _UNIT_SCALE_.U0_1qmh:
          doubleValue /= 10.0;
          return doubleValue.ToString("f1") + "m\u00B3/h";
        case _UNIT_SCALE_.U1_0qmh:
          return doubleValue.ToString("f0") + "m\u00B3/h";
        case _UNIT_SCALE_.U0_001gpm:
          doubleValue /= 1000.0;
          return doubleValue.ToString("f3") + "US GAL/min";
        case _UNIT_SCALE_.U0_01gpm:
          doubleValue /= 100.0;
          return doubleValue.ToString("f2") + "US GAL/min";
        case _UNIT_SCALE_.U0_1gpm:
          doubleValue /= 10.0;
          return doubleValue.ToString("f1") + "US GAL/min";
        case _UNIT_SCALE_.U1_0gpm:
          return doubleValue.ToString("f0") + "US GAL/min";
        case _UNIT_SCALE_.U0_01cels:
          return (doubleValue / 100.0).ToString("f2") + "°C";
        default:
          return "Value scaling not implemented. Unscaled value: " + doubleValue.ToString();
      }
    }

    public override string ToString() => this.ToStringPadNameRight(0);

    public string ToStringPadNameRight(int padSize)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Name.PadRight(padSize));
      stringBuilder.Append("; UnitScale:" + this.UnitScale.ToString());
      stringBuilder.Append("; VarType:" + this.VarType.ToString());
      stringBuilder.Append("; Storage:" + this.StorageNumber.ToString());
      stringBuilder.Append("; Tarif:" + this.Tarif.ToString());
      stringBuilder.Append("; Function:" + this.DifFunction.ToString());
      List<byte> difVifBytes = this.GetDifVifBytes();
      stringBuilder.Append("; DifVif:" + Util.ByteArrayToHexString(difVifBytes.ToArray()));
      if (this.Info != null)
        stringBuilder.Append("; " + this.Info);
      return stringBuilder.ToString();
    }

    static S4_DifVif_Parameter()
    {
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001L, "0mL", 16U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01L, "0.00L", 17U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1L, "0.0000m\u00B3", 18U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001qm, "0.000m\u00B3", 19U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01qm, "0.00m\u00B3", 20U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1qm, "0.0m\u00B3", 21U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0qm, "0m\u00B3", 22U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U10_0qm, "0m\u00B3x10", 23U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U100_0qm, "0m\u00B3x100", 4347U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_000001USgal, "0.000000US_GAL", 15760U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_00001USgal, "0.00000US_GAL", 15761U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_0001USgal, "0.0000US_GAL", 15762U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001USgal, "0.000US_GAL", 15763U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01USgal, "0.00US_GAL", 15764U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1USgal, "0.0US_GAL", 15765U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0USgal, "0US_GAL", 15766U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U10_0USgal, "0US_GALx10", 15767U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U100_0USgal, "0US_GALx100", 4034811U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_000001cft, "0.000000ft\u00B3", 7381243U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_00001cft, "0.00000ft\u00B3", 7446779U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_0001cft, "0.0000ft\u00B3", 7512315U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001cft, "0.000ft\u00B3", 7577851U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01cft, "0.00ft\u00B3", 7643387U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1cft, "0.0ft\u00B3", 8699U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0cft, "0ft\u00B3", 8443U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U10_0cft, "0ft\u00B3x10", 7839995U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U100_0cft, "0ft\u00B3x100", 8233467U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_000001qmh, "0.000L/h", 56U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_00001qmh, "0.00L/h", 57U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_0001qmh, "0.0000m\u00B3/h", 58U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001qmh, "0.000m\u00B3/h", 59U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01qmh, "0.00m\u00B3/h", 60U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1qmh, "0.0m\u00B3/h", 61U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0qmh, "0m\u00B3/h", 62U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U10_0qmh, "0m\u00B3/hx10", 63U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001gpm, "0.000GAL/Minute", 15809U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01gpm, "0.00GAL/Minute", 15810U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1gpm, "0.0GAL/Minute", 15811U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0gpm, "0GAL/Minute", 15812U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001cfm, "0.000ft\u00B3/Minute", 569614587U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01cfm, "0.00ft\u00B3/Minute", 569680123U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1cfm, "0.0ft\u00B3/Minute", 2204155U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0cfm, "0ft\u00B3/Minute", 2203899U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_dateTime, "Device clock date and time", 109U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_date, "Device clock date", 108U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_time, "Device clock time", 109U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.NotDef, "Not defined scale", 0U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_segment_test, "Segment test", 0U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_system_current, "Test current", 0U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_identification, "Identification", 0U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_specialflow, "Special flow", 0U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_sys_state, "System state", 6141U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U_functional_State, "Function state", 6141U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_001cels, "0.000°C", 88U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_01cels, "0.00°C", 89U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U0_1cels, "0.0°C", 90U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.U1_0cels, "0°C", 91U);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.DefaultVolUnit, "Default volume unit", (uint) byte.MaxValue);
      S4_DifVif_Parameter.AddBaseUnitScale(_UNIT_SCALE_.DefaultFlowUnit, "Default flow unit", 511U);
      S4_DifVif_Parameter.AllVolumeUnitBaseDefines = new SortedList<_UNIT_SCALE_, VolumeUnitBaseDefines>();
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_001L, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_1L, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_001qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_01qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_1qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U1_0qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U10_0qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U100_0qm, _UNIT_SCALE_.U0_001qmh);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_000001USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_00001USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_0001USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_001USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_01USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_1USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U1_0USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U10_0USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U100_0USgal, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_000001cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_00001cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_0001cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_001cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_01cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U0_1cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U1_0cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U10_0cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.U100_0cft, _UNIT_SCALE_.U0_01gpm);
      S4_DifVif_Parameter.AddVolumeUnitBaseDefine(_UNIT_SCALE_.DefaultVolUnit, _UNIT_SCALE_.DefaultFlowUnit);
      S4_DifVif_Parameter.PreDefParams = new List<S4_DifVif_Parameter>();
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, device clock", _UNIT_SCALE_.U_date, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date and time, device clock", _UNIT_SCALE_.U_dateTime, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date and time to seconds, device clock", _UNIT_SCALE_.U_dateTime, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT48_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume", _UNIT_SCALE_.DefaultVolUnit, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, flow direction", _UNIT_SCALE_.DefaultVolUnit, 0, 1, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, return direction", _UNIT_SCALE_.DefaultVolUnit, 0, 2, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Flow", _UNIT_SCALE_.DefaultFlowUnit, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature", _UNIT_SCALE_.U0_01cels, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("System state", _UNIT_SCALE_.U_sys_state, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State", _UNIT_SCALE_.U_functional_State, 0, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, last key date", _UNIT_SCALE_.U_date, 1, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last key date", _UNIT_SCALE_.DefaultVolUnit, 1, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last key date", _UNIT_SCALE_.DefaultVolUnit, 2, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last key date", _UNIT_SCALE_.DefaultVolUnit, 3, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last key date", _UNIT_SCALE_.U0_01cels, 1, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last key date", _UNIT_SCALE_.U0_01cels, 2, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last  key date", _UNIT_SCALE_.U0_01cels, 3, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last key date", _UNIT_SCALE_.U_functional_State, 1, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last key date", _UNIT_SCALE_.U_functional_State, 2, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last key date", _UNIT_SCALE_.U_functional_State, 3, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, last month", _UNIT_SCALE_.U_date, 4, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last month", _UNIT_SCALE_.DefaultVolUnit, 4, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last month", _UNIT_SCALE_.DefaultVolUnit, 5, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last month", _UNIT_SCALE_.DefaultVolUnit, 6, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last month", _UNIT_SCALE_.U0_01cels, 4, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last month", _UNIT_SCALE_.U0_01cels, 5, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last month", _UNIT_SCALE_.U0_01cels, 6, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last month", _UNIT_SCALE_.U_functional_State, 4, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last month", _UNIT_SCALE_.U_functional_State, 5, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last month", _UNIT_SCALE_.U_functional_State, 6, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, middle of last month", _UNIT_SCALE_.U_date, 8, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, middle of last month", _UNIT_SCALE_.DefaultVolUnit, 8, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, middle of second last month", _UNIT_SCALE_.DefaultVolUnit, 9, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, middle of third last month", _UNIT_SCALE_.DefaultVolUnit, 10, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, middle of last month", _UNIT_SCALE_.U0_01cels, 8, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, middle of second last month", _UNIT_SCALE_.U0_01cels, 9, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, middle of third last month", _UNIT_SCALE_.U0_01cels, 10, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, middle of last month", _UNIT_SCALE_.U_functional_State, 8, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, middle of second last month", _UNIT_SCALE_.U_functional_State, 9, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, middle of third last month", _UNIT_SCALE_.U_functional_State, 10, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, last day", _UNIT_SCALE_.U_date, 12, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last day", _UNIT_SCALE_.DefaultVolUnit, 12, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last day", _UNIT_SCALE_.DefaultVolUnit, 13, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last day", _UNIT_SCALE_.DefaultVolUnit, 14, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last day", _UNIT_SCALE_.U0_01cels, 12, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last day", _UNIT_SCALE_.U0_01cels, 13, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last day", _UNIT_SCALE_.U0_01cels, 14, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last day", _UNIT_SCALE_.U_functional_State, 12, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last day", _UNIT_SCALE_.U_functional_State, 13, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last day", _UNIT_SCALE_.U_functional_State, 14, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date and time, last 6 hour interval", _UNIT_SCALE_.U_dateTime, 16, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last 6 hour interval", _UNIT_SCALE_.DefaultVolUnit, 16, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last 6 hour interval", _UNIT_SCALE_.DefaultVolUnit, 17, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last 6 hour interval", _UNIT_SCALE_.DefaultVolUnit, 18, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last 6 hour interval", _UNIT_SCALE_.U0_01cels, 16, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last 6 hour interval", _UNIT_SCALE_.U0_01cels, 17, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last 6 hour interval", _UNIT_SCALE_.U0_01cels, 18, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last 6 hour interval", _UNIT_SCALE_.U_functional_State, 16, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last 6 hour interval", _UNIT_SCALE_.U_functional_State, 17, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last 6 hour interval", _UNIT_SCALE_.U_functional_State, 18, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date and time, last hour", _UNIT_SCALE_.U_dateTime, 20, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last hour", _UNIT_SCALE_.DefaultVolUnit, 20, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last hour", _UNIT_SCALE_.DefaultVolUnit, 21, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last hour", _UNIT_SCALE_.DefaultVolUnit, 22, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last hour", _UNIT_SCALE_.U0_01cels, 20, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last hour", _UNIT_SCALE_.U0_01cels, 21, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last hour", _UNIT_SCALE_.U0_01cels, 22, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last hour", _UNIT_SCALE_.U_functional_State, 20, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last hour", _UNIT_SCALE_.U_functional_State, 21, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last hour", _UNIT_SCALE_.U_functional_State, 22, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date and time, last quarter hour", _UNIT_SCALE_.U_dateTime, 24, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, last quarter hour", _UNIT_SCALE_.DefaultVolUnit, 24, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, second last quarter hour", _UNIT_SCALE_.DefaultVolUnit, 25, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, third last quarter hour", _UNIT_SCALE_.DefaultVolUnit, 26, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, last quarter hour", _UNIT_SCALE_.U0_01cels, 24, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, second last quarter hour", _UNIT_SCALE_.U0_01cels, 25, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Temperature, third last quarter hour", _UNIT_SCALE_.U0_01cels, 26, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, last quarter hour", _UNIT_SCALE_.U_functional_State, 24, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, second last quarter hour", _UNIT_SCALE_.U_functional_State, 25, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("State, third last quarter hour", _UNIT_SCALE_.U_functional_State, 26, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, last month", _UNIT_SCALE_.U_date, 30, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, last month", _UNIT_SCALE_.DefaultVolUnit, 30, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, additional 11 month, compact", _UNIT_SCALE_.DefaultVolUnit, 30, 0, DIF_Function.Inst, _VAR_TYPE_.VAR_LEN, new int?(11)));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, last 12 month", _UNIT_SCALE_.DefaultVolUnit, 30, 0, DIF_Function.Inst, _VAR_TYPE_.VAR_LEN, new int?(12)));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, middle of last month", _UNIT_SCALE_.U_date, 31, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, middle of last month", _UNIT_SCALE_.DefaultVolUnit, 31, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, additional 11 middle month, compact", _UNIT_SCALE_.DefaultVolUnit, 31, 0, DIF_Function.Inst, _VAR_TYPE_.VAR_LEN, new int?(11)));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry", _UNIT_SCALE_.U_date, 32, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry", _UNIT_SCALE_.DefaultVolUnit, 32, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry 2", _UNIT_SCALE_.U_date, 33, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry 2", _UNIT_SCALE_.DefaultVolUnit, 33, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry 3", _UNIT_SCALE_.U_date, 34, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry 3", _UNIT_SCALE_.DefaultVolUnit, 34, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry 4", _UNIT_SCALE_.U_date, 35, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry 4", _UNIT_SCALE_.DefaultVolUnit, 35, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry 5", _UNIT_SCALE_.U_date, 36, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry 5", _UNIT_SCALE_.DefaultVolUnit, 36, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Date, logger, entry 6", _UNIT_SCALE_.U_date, 37, 0, DIF_Function.Inst, _VAR_TYPE_.INT16_T));
      S4_DifVif_Parameter.PreDefParams.Add(new S4_DifVif_Parameter("Volume, logger, entry 6", _UNIT_SCALE_.DefaultVolUnit, 37, 0, DIF_Function.Inst, _VAR_TYPE_.INT32_T));
    }

    private static void AddBaseUnitScale(
      _UNIT_SCALE_ unitScale,
      string scaleViewString,
      uint vif_value)
    {
      S4_DifVif_Parameter.BaseUnitScale.Add(unitScale, new BaseUnitScaleValues(unitScale, scaleViewString, vif_value));
    }

    private static void AddVolumeUnitBaseDefine(_UNIT_SCALE_ scaleUnit, _UNIT_SCALE_ flow_scaleUnit)
    {
      VolumeUnitBaseDefines volumeUnitBaseDefines = new VolumeUnitBaseDefines()
      {
        volumeInfo = new _MBUS_INFO_TYPE_(),
        flowInfo = new _MBUS_INFO_TYPE_(),
        scaleUnit = scaleUnit
      };
      volumeUnitBaseDefines.volumeInfo.scaleUnit = scaleUnit;
      volumeUnitBaseDefines.volumeInfo.difFunction = DIF_Function.Inst;
      volumeUnitBaseDefines.volumeInfo.destType = _VAR_TYPE_.INT32_T;
      volumeUnitBaseDefines.volumeInfo.tarif = (byte) 0;
      volumeUnitBaseDefines.volumeInfo.storeNum = (ushort) 0;
      volumeUnitBaseDefines.volumeInfo.selectedVif = S4_DifVif_Parameter.BaseUnitScale[scaleUnit].VIF_value;
      volumeUnitBaseDefines.flowInfo.scaleUnit = flow_scaleUnit;
      volumeUnitBaseDefines.flowInfo.difFunction = DIF_Function.Inst;
      volumeUnitBaseDefines.flowInfo.destType = _VAR_TYPE_.INT32_T;
      volumeUnitBaseDefines.flowInfo.tarif = (byte) 0;
      volumeUnitBaseDefines.flowInfo.storeNum = (ushort) 0;
      volumeUnitBaseDefines.flowInfo.selectedVif = S4_DifVif_Parameter.BaseUnitScale[flow_scaleUnit].VIF_value;
      S4_DifVif_Parameter.AllVolumeUnitBaseDefines.Add(scaleUnit, volumeUnitBaseDefines);
    }

    internal static string GetParameterName(_MBUS_INFO_TYPE_ mbusInfo)
    {
      try
      {
        List<S4_DifVif_Parameter> all1 = S4_DifVif_Parameter.PreDefParams.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.StorageNumber == (int) mbusInfo.storeNum && x.Tarif == (int) mbusInfo.tarif && x.DifFunction == mbusInfo.difFunction));
        if (all1.Count == 0)
          return "?";
        VIF_PhyicalBase physicalBase = S4_DifVif_Parameter.GetPhysicalBase(mbusInfo);
        List<S4_DifVif_Parameter> all2 = all1.FindAll((Predicate<S4_DifVif_Parameter>) (x => S4_DifVif_Parameter.GetPhysicalBase(S4_DifVif_Parameter.BaseUnitScale[x.UnitScale].VIF_value) == physicalBase));
        if (all2.Count == 1)
          return all2[0].Name;
        List<S4_DifVif_Parameter> all3 = all2.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.VarType == mbusInfo.destType));
        return all3.Count == 1 ? all3[0].Name : "?";
      }
      catch
      {
        return "?";
      }
    }

    internal static VIF_PhyicalBase GetPhysicalBase(_MBUS_INFO_TYPE_ mMusInfo)
    {
      return S4_DifVif_Parameter.GetPhysicalBase(mMusInfo.GetBaseVif());
    }

    internal static VIF_PhyicalBase GetPhysicalBase(uint vif)
    {
      switch (vif)
      {
        case 89:
          return VIF_PhyicalBase.Temperature;
        case (uint) byte.MaxValue:
          return VIF_PhyicalBase.Volume;
        case 511:
          return VIF_PhyicalBase.Flow;
        case 6141:
          return VIF_PhyicalBase.FunctionalState;
        default:
          switch (vif)
          {
            case 4347:
              return VIF_PhyicalBase.Volume;
            case 4603:
              return VIF_PhyicalBase.Volume;
            case 8443:
              return VIF_PhyicalBase.Volume;
            case 8699:
              return VIF_PhyicalBase.Volume;
            default:
              if (((int) vif & 120) == 16)
                return VIF_PhyicalBase.Volume;
              if (((int) vif & 120) == 56)
                return VIF_PhyicalBase.Flow;
              if (((int) vif & 120) == 88)
                return VIF_PhyicalBase.Temperature;
              if (vif == 108U || vif == 109U)
                return VIF_PhyicalBase.DateAndTime;
              throw new Exception("Unknown physical VIF base." + vif.ToString("x08"));
          }
      }
    }

    internal static VIF_PhyicalBase GetPhysicalBase(_UNIT_SCALE_ unitScale)
    {
      if (unitScale < ~_UNIT_SCALE_.NotSet || unitScale == _UNIT_SCALE_.NotDef)
        throw new Exception("Unit scale not defined");
      if (unitScale == _UNIT_SCALE_.DefaultVolUnit || unitScale < _UNIT_SCALE_.U0_000001qmh)
        return VIF_PhyicalBase.Volume;
      if (unitScale == _UNIT_SCALE_.DefaultFlowUnit || unitScale < _UNIT_SCALE_.U_dateTime)
        return VIF_PhyicalBase.Flow;
      if (unitScale >= _UNIT_SCALE_.U0_001cels && unitScale <= _UNIT_SCALE_.U1_0cels)
        return VIF_PhyicalBase.Temperature;
      switch (unitScale)
      {
        case _UNIT_SCALE_.U_dateTime:
          return VIF_PhyicalBase.DateAndTime;
        case _UNIT_SCALE_.U_time:
          return VIF_PhyicalBase.DateAndTime;
        case _UNIT_SCALE_.U_date:
          return VIF_PhyicalBase.DateAndTime;
        case _UNIT_SCALE_.U_sys_state:
          return VIF_PhyicalBase.SystemState;
        case _UNIT_SCALE_.U_functional_State:
          return VIF_PhyicalBase.FunctionalState;
        default:
          throw new Exception("Unknown physical _UNIT_SCALE_ base: " + unitScale.ToString());
      }
    }

    internal static byte[] GetSelectDataForSensusGroup()
    {
      return S4_DifVif_Parameter.PrepareSelectData(new List<S4_DifVif_Parameter>()
      {
        S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Volume"))
      });
    }

    internal static byte[] PrepareSelectData(List<S4_DifVif_Parameter> parameterList)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) parameterList.Count);
      foreach (S4_DifVif_Parameter parameter in parameterList)
        byteList.AddRange((IEnumerable<byte>) parameter.GetDifVifBytes());
      return byteList.ToArray();
    }

    internal static List<S4_DifVif_Parameter> GetParametersFromNfcProtocolData(byte[] protocolData)
    {
      List<S4_DifVif_Parameter> fromNfcProtocolData = new List<S4_DifVif_Parameter>();
      int offset = 1;
      while (offset < protocolData.Length)
        fromNfcProtocolData.Add(S4_DifVif_Parameter.GetParametersFromDifVifBytes(protocolData, ref offset));
      return fromNfcProtocolData;
    }

    internal static S4_DifVif_Parameter GetParametersFromDifVifBytes(byte[] data, ref int offset)
    {
      S4_DifVif_Parameter theParam = new S4_DifVif_Parameter();
      if (data.Length > offset)
      {
        theParam.VarType = (_VAR_TYPE_) ((int) data[offset] & 15);
        theParam.DifFunction = (DIF_Function) (((int) data[offset] & 48) >> 4);
        theParam.StorageNumber = ((int) data[offset] & 64) >> 6;
        theParam.Tarif = 0;
        int num1 = 1;
        int num2 = 0;
        while (((uint) data[offset] & 128U) > 0U)
        {
          ++offset;
          if (data.Length > offset)
          {
            theParam.StorageNumber |= ((int) data[offset] & 15) << num1;
            theParam.Tarif |= ((int) data[offset] & 48) >> 4 << num2;
            num1 += 4;
            num2 += 2;
          }
          else
            goto label_36;
        }
        ++offset;
        if (data.Length > offset)
        {
          uint num3 = (uint) data[offset];
          int num4 = 8;
          while (((uint) data[offset] & 128U) > 0U)
          {
            ++offset;
            if (data.Length > offset)
            {
              num3 |= (uint) data[offset] << num4;
              num4 += 8;
            }
            else
              goto label_36;
          }
          ++offset;
          uint num5 = num3;
          if (num3 > (uint) ushort.MaxValue && ((int) num3 & (int) short.MinValue) == 1277952)
          {
            num5 &= 4278222847U;
            theParam.IsCompactProfile = true;
            theParam.LVAR = data[offset++];
            theParam.SpacingControl = data[offset++];
            theParam.SpacingValue = data[offset++];
          }
          else if (num3 > (uint) byte.MaxValue && ((int) num3 & (int) sbyte.MinValue) == 4992)
          {
            num5 &= 4294901887U;
            theParam.IsCompactProfile = true;
            theParam.LVAR = data[offset++];
            theParam.SpacingControl = data[offset++];
            theParam.SpacingValue = data[offset++];
          }
          theParam.UnitScale = _UNIT_SCALE_.NotSet;
          foreach (BaseUnitScaleValues baseUnitScaleValues in (IEnumerable<BaseUnitScaleValues>) S4_DifVif_Parameter.BaseUnitScale.Values)
          {
            if ((int) baseUnitScaleValues.VIF_value == (int) num5)
            {
              switch (num5)
              {
                case 109:
                  if (theParam.VarType == _VAR_TYPE_.INT32_T)
                  {
                    theParam.UnitScale = _UNIT_SCALE_.U_dateTime;
                    break;
                  }
                  if (theParam.VarType == _VAR_TYPE_.INT24_T)
                  {
                    theParam.UnitScale = _UNIT_SCALE_.U_time;
                    break;
                  }
                  continue;
                case 6141:
                  if (theParam.VarType == _VAR_TYPE_.INT16_T)
                  {
                    theParam.UnitScale = _UNIT_SCALE_.U_functional_State;
                    break;
                  }
                  if (theParam.VarType == _VAR_TYPE_.INT32_T)
                  {
                    theParam.UnitScale = _UNIT_SCALE_.U_sys_state;
                    break;
                  }
                  continue;
                default:
                  theParam.UnitScale = baseUnitScaleValues.ScaleUnit;
                  break;
              }
            }
          }
          if (theParam.UnitScale == _UNIT_SCALE_.NotSet)
            throw new Exception("VIF value not found: 0x" + num3.ToString("x08"));
          S4_DifVif_Parameter.AddParameterName(theParam);
          return theParam;
        }
      }
label_36:
      throw new Exception("Missing data bytes by parsing DifVif parameters on offset: " + offset.ToString());
    }

    internal static void AddParameterName(S4_DifVif_Parameter theParam)
    {
      List<S4_DifVif_Parameter> all1 = S4_DifVif_Parameter.PreDefParams.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.VarType == theParam.VarType && x.DifFunction == theParam.DifFunction && x.StorageNumber == theParam.StorageNumber && x.Tarif == theParam.Tarif && (int) x.LVAR == (int) theParam.LVAR && x.UnitScale == theParam.UnitScale));
      if (all1.Count == 1)
      {
        theParam.Name = all1[0].Name;
      }
      else
      {
        List<S4_DifVif_Parameter> all2 = S4_DifVif_Parameter.PreDefParams.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.VarType == theParam.VarType && x.DifFunction == theParam.DifFunction && x.StorageNumber == theParam.StorageNumber && x.Tarif == theParam.Tarif && (int) x.LVAR == (int) theParam.LVAR && S4_DifVif_Parameter.GetPhysicalBase(x.UnitScale) == S4_DifVif_Parameter.GetPhysicalBase(theParam.UnitScale)));
        if (all2.Count == 1)
        {
          theParam.Name = all2[0].Name;
        }
        else
        {
          List<S4_DifVif_Parameter> all3 = S4_DifVif_Parameter.PreDefParams.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.DifFunction == theParam.DifFunction && x.StorageNumber == theParam.StorageNumber && x.Tarif == theParam.Tarif && (int) x.LVAR == (int) theParam.LVAR && x.UnitScale == theParam.UnitScale));
          if (all3.Count == 1)
          {
            theParam.Name = all3[0].Name;
            theParam.Info = "Type convertet from " + all3[0].VarType.ToString() + " to " + theParam.VarType.ToString();
          }
          else
          {
            List<S4_DifVif_Parameter> all4 = S4_DifVif_Parameter.PreDefParams.FindAll((Predicate<S4_DifVif_Parameter>) (x => x.DifFunction == theParam.DifFunction && x.StorageNumber == theParam.StorageNumber && x.Tarif == theParam.Tarif && (int) x.LVAR == (int) theParam.LVAR && S4_DifVif_Parameter.GetPhysicalBase(x.UnitScale) == S4_DifVif_Parameter.GetPhysicalBase(theParam.UnitScale)));
            if (all4.Count != 1)
              return;
            theParam.Name = all4[0].Name;
            theParam.Info = "Type convertet from " + all4[0].VarType.ToString() + " to " + theParam.VarType.ToString();
          }
        }
      }
    }
  }
}
