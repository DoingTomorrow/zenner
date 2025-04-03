// Decompiled with JetBrains decompiler
// Type: GMM_Handler.OverrideParameter
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using NLog;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class OverrideParameter : ConfigurationParameter
  {
    private static Logger logger = LogManager.GetLogger(nameof (OverrideParameter));
    public static OverrideParameter.OverrideParameterTypeIdent[] OverrideParameterInfoList;
    public static OverrideParameter.BaseConfigStruct[] BaseConfigTable;
    public bool AtFunctionTabel;

    public ushort ByteSize
    {
      get => OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].ByteSize;
    }

    public bool IsStructureParameter
    {
      get => OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].IsStruct;
    }

    public MeterResources NeadedRessource
    {
      get => OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].NeadedResource;
    }

    public ulong Value
    {
      set
      {
        this.SetValueFromULong(value);
        this.TrueDivisor = 0M;
      }
      get => this.GetValueAsULong();
    }

    public OverrideParameter(OverrideParameter ParameterToCopy)
      : base((ConfigurationParameter) ParameterToCopy)
    {
      this.AtFunctionTabel = ParameterToCopy.AtFunctionTabel;
    }

    public OverrideParameter(OverrideID TheID)
      : base(TheID)
    {
      this.AtFunctionTabel = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].AtFunctionTable;
      this.HasWritePermission = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].HasWritePermission;
      this.SetValueFromStringDb(OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].DefaultValue);
    }

    public OverrideParameter(OverrideID TheID, ulong LongValue)
      : base(TheID)
    {
      this.Value = LongValue;
      this.AtFunctionTabel = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].AtFunctionTable;
      this.HasWritePermission = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].HasWritePermission;
    }

    public OverrideParameter(OverrideID TheID, DateTime dateTimeValue)
      : base(TheID)
    {
      this.Value = (ulong) dateTimeValue.Year;
      this.AtFunctionTabel = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].AtFunctionTable;
      this.HasWritePermission = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].HasWritePermission;
    }

    public OverrideParameter(OverrideID TheID, ulong LongValue, Decimal ScaleFactor)
      : base(TheID)
    {
      this.TrueDivisor = ScaleFactor;
      this.Value = LongValue;
      this.AtFunctionTabel = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].AtFunctionTable;
      this.HasWritePermission = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].HasWritePermission;
    }

    public OverrideParameter(OverrideID TheID, string StringValue, bool DbString)
      : base(TheID, StringValue, DbString)
    {
      this.AtFunctionTabel = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].AtFunctionTable;
      this.HasWritePermission = OverrideParameter.OverrideParameterInfoList[(int) this.ParameterID].HasWritePermission;
    }

    public OverrideParameter Clone() => new OverrideParameter(this);

    public static SortedList GetNewOverridesList()
    {
      SortedList newOverridesList = new SortedList();
      OverrideParameter overrideParameter1 = new OverrideParameter(OverrideID.BaseConfig);
      newOverridesList.Add((object) overrideParameter1.ParameterID, (object) overrideParameter1);
      OverrideParameter overrideParameter2 = new OverrideParameter(OverrideID.EnergyResolution);
      newOverridesList.Add((object) overrideParameter2.ParameterID, (object) overrideParameter2);
      OverrideParameter overrideParameter3 = new OverrideParameter(OverrideID.VolumeResolution);
      newOverridesList.Add((object) overrideParameter3.ParameterID, (object) overrideParameter3);
      OverrideParameter overrideParameter4 = new OverrideParameter(OverrideID.Input1Unit);
      newOverridesList.Add((object) overrideParameter4.ParameterID, (object) overrideParameter4);
      OverrideParameter overrideParameter5 = new OverrideParameter(OverrideID.Input2Unit);
      newOverridesList.Add((object) overrideParameter5.ParameterID, (object) overrideParameter5);
      return newOverridesList;
    }

    public static SortedList GetOverridesListClone(SortedList OverridesList)
    {
      SortedList overridesListClone = new SortedList();
      for (int index = 0; index < OverridesList.Count; ++index)
      {
        OverrideParameter overrideParameter = ((OverrideParameter) OverridesList.GetByIndex(index)).Clone();
        overridesListClone.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
      }
      return overridesListClone;
    }

    public static void ChangeOrAddOverrideParameter(
      SortedList TheList,
      OverrideParameter TheOverrideParameter)
    {
      int index = TheList.IndexOfKey((object) TheOverrideParameter.ParameterID);
      if (index < 0)
        TheList.Add((object) TheOverrideParameter.ParameterID, (object) TheOverrideParameter);
      else
        TheList.SetByIndex(index, (object) TheOverrideParameter);
    }

    public static void DeleteOverrideParameter(SortedList TheList, OverrideID TheId)
    {
      int index = TheList.IndexOfKey((object) TheId);
      if (index < 0)
        return;
      TheList.RemoveAt(index);
    }

    public bool LoadDataFromByteArray(byte[] TheArray, ref int Offset)
    {
      if (this.ByteSize < (ushort) 1)
        return false;
      ulong num = 0;
      for (int index = 0; index < (int) this.ByteSize; ++index)
        num += (ulong) TheArray[Offset++] << index * 8;
      this.Value = num;
      this.AtFunctionTabel = true;
      return true;
    }

    public ulong GetValueAsULong()
    {
      ulong valueAsUlong;
      switch (this.ParameterID)
      {
        case OverrideID.WarmerPipe:
        case OverrideID.ChangeOver:
          valueAsUlong = !(bool) this.ParameterValue ? 0UL : 1UL;
          break;
        case OverrideID.CustomID:
        case OverrideID.MBusAddress:
        case OverrideID.MeterID:
        case OverrideID.BaseTypeID:
        case OverrideID.FactoryTypeID:
        case OverrideID.Baudrate:
        case OverrideID.CycleTimeFast:
        case OverrideID.CycleTimeStandard:
          valueAsUlong = (ulong) this.ParameterValue;
          break;
        case OverrideID.ReadingDate:
          valueAsUlong = (ulong) ZR_Calendar.Cal_GetMeterTime((DateTime) this.ParameterValue);
          break;
        case OverrideID.MBusIdentificationNo:
        case OverrideID.SerialNumber:
        case OverrideID.Input1IdNumber:
        case OverrideID.Input2IdNumber:
          valueAsUlong = ulong.Parse((string) this.ParameterValue, NumberStyles.HexNumber);
          break;
        case OverrideID.EnergyResolution:
          valueAsUlong = (ulong) MeterMath.GetEnergyUnitOverwriteID((string) this.ParameterValue);
          break;
        case OverrideID.VolumeResolution:
          valueAsUlong = (ulong) MeterMath.GetVolumeUnitIndex((string) this.ParameterValue);
          break;
        case OverrideID.VolumePulsValue:
        case OverrideID.Input1PulsValue:
        case OverrideID.Input2PulsValue:
          int BCDValue;
          if (!OverrideParameter.DoubleToPackedBCD((double) this.ParameterValue, out BCDValue))
            throw new ArgumentException("Illegal ulong puls value");
          valueAsUlong = (ulong) BCDValue;
          break;
        case OverrideID.Input1Unit:
        case OverrideID.Input2Unit:
          valueAsUlong = (ulong) MeterMath.GetInputUnitIndex((string) this.ParameterValue);
          break;
        case OverrideID.Output1Function:
        case OverrideID.Output2Function:
          valueAsUlong = (ulong) (int) Enum.Parse(typeof (ConfigurationParameter.OutputFunctions), (string) this.ParameterValue, true);
          break;
        case OverrideID.BaseConfig:
          valueAsUlong = (ulong) (int) this.ParameterValue;
          break;
        case OverrideID.EnergyActualValue:
        case OverrideID.EnergyDueDateValue:
        case OverrideID.EnergyDueDateLastValue:
        case OverrideID.VolumeActualValue:
        case OverrideID.VolumeDueDateValue:
        case OverrideID.VolumeDueDateLastValue:
        case OverrideID.Input1ActualValue:
        case OverrideID.Input1DueDateValue:
        case OverrideID.Input1DueDateLastValue:
        case OverrideID.Input2ActualValue:
        case OverrideID.Input2DueDateValue:
        case OverrideID.Input2DueDateLastValue:
        case OverrideID.CEnergyActualValue:
        case OverrideID.CEnergyDueDateValue:
        case OverrideID.CEnergyDueDateLastValue:
        case OverrideID.TarifEnergy0:
        case OverrideID.TarifEnergy1:
          valueAsUlong = this.GetULongParameterValue();
          break;
        case OverrideID.ModuleType:
          ulong num1 = 0;
          string parameterValue1 = (string) this.ParameterValue;
          char[] chArray1 = new char[1]{ ';' };
          foreach (string str1 in parameterValue1.Split(chArray1))
          {
            string str2 = str1.Trim();
            if (str2.Length != 0)
              num1 += (ulong) (ModuleTypeValues) Enum.Parse(typeof (ModuleTypeValues), str2, true);
          }
          valueAsUlong = num1;
          break;
        case OverrideID.IO_Functions:
          ulong num2 = 0;
          string parameterValue2 = (string) this.ParameterValue;
          char[] chArray2 = new char[1]{ ';' };
          foreach (string str in parameterValue2.Split(chArray2))
            num2 += (ulong) (InOutFunctions) Enum.Parse(typeof (InOutFunctions), str, true);
          valueAsUlong = num2;
          break;
        case OverrideID.Input1Type:
        case OverrideID.Input2Type:
          valueAsUlong = (ulong) this.ParameterValue;
          break;
        case OverrideID.FixedTempSetup:
          valueAsUlong = (ulong) (FixedTempSetup) this.ParameterValue;
          break;
        case OverrideID.FixedTempValue:
        case OverrideID.MinTempDiffPlusTemp:
        case OverrideID.MinTempDiffMinusTemp:
        case OverrideID.TarifRefTemp:
        case OverrideID.HeatThresholdTemp:
          valueAsUlong = (ulong) (long) ((Decimal) this.ParameterValue * 100M);
          break;
        case OverrideID.MimTempDiffSetup:
          valueAsUlong = (ulong) (MinimalTempDiffSetup) this.ParameterValue;
          break;
        case OverrideID.TarifFunction:
          valueAsUlong = (ulong) (TarifSetup) this.ParameterValue;
          break;
        case OverrideID.EndOfBattery:
          valueAsUlong = (ulong) ((DateTime) this.ParameterValue).Year;
          break;
        case OverrideID.EndOfCalibration:
          valueAsUlong = (ulong) (int) this.ParameterValue;
          break;
        case OverrideID.CycleTimeDynamic:
          valueAsUlong = (ulong) (CycleTimeChangeMethode) this.ParameterValue;
          break;
        case OverrideID.Medium:
          valueAsUlong = (ulong) (MBusDeviceType) this.ParameterValue;
          break;
        default:
          throw new ArgumentException("Parameter without ulong type");
      }
      return valueAsUlong;
    }

    public void SetValueFromULong(ulong ULongValue)
    {
      string message1;
      try
      {
        OverrideParameter.logger.Trace("SetValueFromUlong: " + ULongValue.ToString() + " ParameterID: " + this.ParameterID.ToString());
        switch (this.ParameterID)
        {
          case OverrideID.WarmerPipe:
          case OverrideID.ChangeOver:
            if (ULongValue == 0UL)
            {
              this.ParameterValue = (object) false;
              return;
            }
            this.ParameterValue = (object) true;
            return;
          case OverrideID.CustomID:
          case OverrideID.MBusAddress:
          case OverrideID.MeterID:
          case OverrideID.BaseTypeID:
          case OverrideID.FactoryTypeID:
          case OverrideID.Baudrate:
          case OverrideID.CycleTimeFast:
          case OverrideID.CycleTimeStandard:
            this.ParameterValue = (object) ULongValue;
            return;
          case OverrideID.ReadingDate:
            this.ParameterValue = (object) ZR_Calendar.Cal_GetDateTime((uint) ULongValue);
            return;
          case OverrideID.MBusIdentificationNo:
          case OverrideID.SerialNumber:
          case OverrideID.Input1IdNumber:
          case OverrideID.Input2IdNumber:
            string StringValue;
            if (!OverrideParameter.PackedBCD_ToString((int) ULongValue, out StringValue))
              throw new ArgumentException("Illegal override string value");
            this.ParameterValue = (object) StringValue;
            return;
          case OverrideID.EnergyResolution:
            this.ParameterValue = (object) MeterMath.GetEnergyUnitOfID((int) ULongValue);
            return;
          case OverrideID.VolumeResolution:
            this.ParameterValue = (object) MeterMath.GetVolumeUnitOfID((int) ULongValue);
            return;
          case OverrideID.VolumePulsValue:
          case OverrideID.Input1PulsValue:
          case OverrideID.Input2PulsValue:
            double num;
            if (!OverrideParameter.PackedBCD_ToDouble((int) ULongValue, out num))
              throw new ArgumentException("Illegal ulong puls value");
            this.ParameterValue = (object) num;
            return;
          case OverrideID.Input1Unit:
          case OverrideID.Input2Unit:
            this.ParameterValue = (object) MeterMath.GetInputUnitOfID((int) ULongValue);
            return;
          case OverrideID.Output1Function:
          case OverrideID.Output2Function:
            this.ParameterValue = (object) (ConfigurationParameter.OutputFunctions) ULongValue;
            return;
          case OverrideID.BaseConfig:
            this.ParameterValue = (object) (ConfigurationParameter.BaseConfigSettings) ULongValue;
            return;
          case OverrideID.EnergyActualValue:
          case OverrideID.EnergyDueDateValue:
          case OverrideID.EnergyDueDateLastValue:
          case OverrideID.VolumeActualValue:
          case OverrideID.VolumeDueDateValue:
          case OverrideID.VolumeDueDateLastValue:
          case OverrideID.Input1ActualValue:
          case OverrideID.Input1DueDateValue:
          case OverrideID.Input1DueDateLastValue:
          case OverrideID.Input2ActualValue:
          case OverrideID.Input2DueDateValue:
          case OverrideID.Input2DueDateLastValue:
          case OverrideID.CEnergyActualValue:
          case OverrideID.CEnergyDueDateValue:
          case OverrideID.CEnergyDueDateLastValue:
          case OverrideID.TarifEnergy0:
          case OverrideID.TarifEnergy1:
            this.ParameterValue = (object) this.GetTrueDecimalValue(ULongValue, this.TrueDivisor);
            return;
          case OverrideID.ModuleType:
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string str1 = ((ModuleTypeValues) ((int) ULongValue & 48)).ToString();
            string str2 = str1;
            ModuleTypeValues moduleTypeValues = ModuleTypeValues.NoValue;
            string str3 = moduleTypeValues.ToString();
            if (str2 != str3)
              empty1 += str1;
            moduleTypeValues = (ModuleTypeValues) ((int) ULongValue & 3);
            string str4 = moduleTypeValues.ToString();
            string str5 = str4;
            moduleTypeValues = ModuleTypeValues.NoValue;
            string str6 = moduleTypeValues.ToString();
            if (str5 != str6)
            {
              if (empty1.Length > 0)
                empty1 += ";";
              empty1 += str4;
            }
            moduleTypeValues = (ModuleTypeValues) ((int) ULongValue & 12);
            string str7 = moduleTypeValues.ToString();
            string str8 = str7;
            moduleTypeValues = ModuleTypeValues.NoValue;
            string str9 = moduleTypeValues.ToString();
            if (str8 != str9)
            {
              if (empty1.Length > 0)
                empty1 += ";";
              empty1 += str7;
            }
            this.ParameterValue = (object) empty1;
            return;
          case OverrideID.IO_Functions:
            this.ParameterValue = (object) (((InOutFunctions) ((long) ULongValue & 15L)).ToString() + ";" + ((InOutFunctions) ((long) ULongValue & 240L)).ToString());
            return;
          case OverrideID.Input1Type:
          case OverrideID.Input2Type:
            this.ParameterValue = (object) ULongValue;
            return;
          case OverrideID.FixedTempSetup:
            this.ParameterValue = (object) (FixedTempSetup) ULongValue;
            return;
          case OverrideID.FixedTempValue:
          case OverrideID.MinTempDiffPlusTemp:
          case OverrideID.MinTempDiffMinusTemp:
          case OverrideID.TarifRefTemp:
          case OverrideID.HeatThresholdTemp:
            this.ParameterValue = (object) ((Decimal) (short) ULongValue / 100M);
            return;
          case OverrideID.MimTempDiffSetup:
            this.ParameterValue = (object) (MinimalTempDiffSetup) ULongValue;
            return;
          case OverrideID.TarifFunction:
            this.ParameterValue = (object) (TarifSetup) ULongValue;
            return;
          case OverrideID.EndOfBattery:
            this.ParameterValue = (object) new DateTime((int) ULongValue, 1, 1);
            return;
          case OverrideID.EndOfCalibration:
            this.ParameterValue = (object) (int) ULongValue;
            return;
          case OverrideID.CycleTimeDynamic:
            this.ParameterValue = (object) (CycleTimeChangeMethode) ULongValue;
            return;
          case OverrideID.Medium:
            this.ParameterValue = (object) (MBusDeviceType) ULongValue;
            return;
          default:
            throw new ArgumentException("Parameter without ulong type");
        }
      }
      catch (Exception ex)
      {
        message1 = ex.Message;
      }
      string message2 = "Illegal override parameter value " + ULongValue.ToString() + " ParameterID: " + this.ParameterID.ToString();
      OverrideParameter.logger.Trace("SetValueFromULong: " + message2);
      if (message1 == null)
        throw new ArgumentException(message2);
      throw new ArgumentException(message2 + ZR_Constants.SystemNewLine + message1);
    }

    public static bool OverrideIdAtString(string TheString, OverrideID TheID)
    {
      return TheString.IndexOf(" " + ((int) TheID).ToString() + " ") >= 0;
    }

    public static void CopyOverrideParameter(
      SortedList DestinationList,
      SortedList SourceList,
      OverrideID TheID)
    {
      DestinationList.Remove((object) TheID);
      OverrideParameter source = (OverrideParameter) SourceList[(object) TheID];
      if (source == null)
        return;
      DestinationList.Add((object) TheID, (object) source);
    }

    public static void CopyIOFunctionOverrideParameter(
      SortedList DestinationList,
      SortedList SourceList,
      ulong IO_Mask)
    {
      OverrideParameter source = (OverrideParameter) SourceList[(object) OverrideID.IO_Functions];
      OverrideParameter destination = (OverrideParameter) DestinationList[(object) OverrideID.IO_Functions];
      DestinationList.Remove((object) OverrideID.IO_Functions);
      if (source == null)
        return;
      destination.Value = (ulong) ((long) destination.Value & ~(long) IO_Mask | (long) source.Value & (long) IO_Mask);
      DestinationList.Add((object) OverrideID.IO_Functions, (object) destination);
    }

    public static void ClearProtectedValues(SortedList AllOverrides)
    {
      for (int index = 0; index < OverrideParameter.OverrideParameterInfoList.Length; ++index)
      {
        OverrideParameter.OverrideParameterTypeIdent overrideParameterInfo = OverrideParameter.OverrideParameterInfoList[index];
        if (overrideParameterInfo.ValueType == OverrideParameter.ValueAccessType.Protected)
        {
          AllOverrides.Remove((object) overrideParameterInfo.OverrideTypeID);
          OverrideParameter overrideParameter = new OverrideParameter(overrideParameterInfo.OverrideTypeID);
          AllOverrides.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        }
      }
    }

    public static void ClearNotProtectedValues(SortedList AllOverrides)
    {
      for (int index = 0; index < OverrideParameter.OverrideParameterInfoList.Length; ++index)
      {
        OverrideParameter.OverrideParameterTypeIdent overrideParameterInfo = OverrideParameter.OverrideParameterInfoList[index];
        if (overrideParameterInfo.ValueType == OverrideParameter.ValueAccessType.NotProtected)
        {
          AllOverrides.Remove((object) overrideParameterInfo.OverrideTypeID);
          OverrideParameter overrideParameter = new OverrideParameter(overrideParameterInfo.OverrideTypeID);
          AllOverrides.Add((object) overrideParameter.ParameterID, (object) overrideParameter);
        }
      }
    }

    public ulong GetParameterValue(Decimal NewFactor)
    {
      if (this.TrueDivisor == 0M)
        return (ulong) ((Decimal) this.ParameterValue * NewFactor);
      return NewFactor != this.TrueDivisor ? (ulong) ((Decimal) this.ParameterValue * NewFactor) : (ulong) ((Decimal) this.ParameterValue * this.TrueDivisor);
    }

    public ulong GetULongParameterValue()
    {
      return this.TrueDivisor == 0M ? (ulong) (Decimal) this.ParameterValue : (ulong) ((Decimal) this.ParameterValue * this.TrueDivisor);
    }

    internal string GetTrueStringValue()
    {
      long parameterValue = (long) this.ParameterValue;
      Decimal trueDivisor = this.TrueDivisor;
      try
      {
        if (trueDivisor == 1M)
          return parameterValue.ToString();
        Decimal num1 = 1M / trueDivisor;
        string str = num1.ToString();
        if (str.StartsWith("0" + SystemValues.ZRDezimalSeparator))
        {
          int num2 = 2;
          while (num2 < str.Length && str[num2] == '0')
            ++num2;
          if (num2 != str.Length)
          {
            num1 = Decimal.Parse(str.Substring(0, num2) + "1");
            Decimal num3 = (Decimal) parameterValue / trueDivisor;
            for (int index = 0; index < 100; ++index)
            {
              string s = num3.ToString();
              int num4 = s.IndexOf(SystemValues.ZRDezimalSeparator);
              if (num4 >= 0)
              {
                int length = num4 + num2;
                if (length < s.Length)
                  s = s.Substring(0, length);
              }
              long num5 = (long) (Decimal.Parse(s) * trueDivisor);
              if (num5 == parameterValue)
              {
                this.ParameterValue = (object) num5;
                return s;
              }
              num3 += num1;
            }
          }
        }
      }
      catch
      {
      }
      throw new ArgumentException("True string convertion error");
    }

    internal Decimal GetTrueDecimalValue(ulong LongValue, Decimal TheDevisor)
    {
      try
      {
        if (TheDevisor == 1M)
          return (Decimal) LongValue;
        string str = (1M / TheDevisor).ToString();
        if (str.StartsWith("0" + SystemValues.ZRDezimalSeparator))
        {
          int num1 = 2;
          while (num1 < str.Length && str[num1] == '0')
            ++num1;
          if (num1 != str.Length)
          {
            Decimal num2 = Decimal.Parse(str.Substring(0, num1) + "1");
            Decimal num3 = (Decimal) LongValue / TheDevisor;
            for (int index = 0; index < 100; ++index)
            {
              string s = num3.ToString();
              int num4 = s.IndexOf(SystemValues.ZRDezimalSeparator);
              if (num4 >= 0)
              {
                int length = num4 + num1;
                if (length < s.Length)
                  s = s.Substring(0, length);
              }
              Decimal trueDecimalValue = Decimal.Parse(s);
              if ((long) (ulong) (trueDecimalValue * TheDevisor) == (long) LongValue)
              {
                this.ParameterValue = (object) trueDecimalValue;
                return trueDecimalValue;
              }
              num3 += num2;
            }
          }
        }
      }
      catch
      {
      }
      OverrideParameter.logger.Error("GetTrueDecimalVal: " + LongValue.ToString() + " Divisor: " + TheDevisor.ToString());
      throw new ArgumentException("True string convertion error " + LongValue.ToString() + " Divisor: " + TheDevisor.ToString());
    }

    internal static bool PackedBCD_ToDouble(int BCDValue, out double Value)
    {
      Value = double.NaN;
      string StringValue;
      if (!OverrideParameter.PackedBCD_ToString(BCDValue, out StringValue))
        return false;
      Value = double.Parse(StringValue);
      return true;
    }

    internal static bool PackedBCD_ToString(int BCDValue, out string StringValue)
    {
      StringValue = string.Empty;
      StringBuilder stringBuilder = new StringBuilder(20);
      bool flag = false;
      for (int index = 0; index < 8; ++index)
      {
        int num = BCDValue >> (7 - index) * 4 & 15;
        if (num < 10)
        {
          stringBuilder.Append(num.ToString());
        }
        else
        {
          if (num != 15 || flag)
            return false;
          flag = true;
          if (stringBuilder.Length == 0)
            stringBuilder.Append('0');
          stringBuilder.Append(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }
      }
      StringValue = stringBuilder.ToString();
      return true;
    }

    internal static bool DoubleToPackedBCD(double Value, out int BCDValue)
    {
      BCDValue = 0;
      short num1 = 0;
      Decimal d1 = (Decimal) Value;
      while (d1 >= 1M)
      {
        ++num1;
        d1 /= 10M;
      }
      Decimal num2 = Decimal.Round(d1, 7);
      for (short index = 0; index < (short) 8; ++index)
      {
        if (num1 == (short) 0)
        {
          BCDValue = (BCDValue << 4) + 15;
        }
        else
        {
          Decimal d2 = num2 * 10M;
          Decimal num3 = Math.Truncate(d2);
          num2 = d2 - num3;
          BCDValue = (BCDValue << 4) + (int) num3;
        }
        --num1;
      }
      return true;
    }

    public static OverrideParameter.BaseConfigStruct GetBaseConfigStruct(string StringValueWin)
    {
      for (int index = 0; index < OverrideParameter.BaseConfigTable.Length; ++index)
      {
        OverrideParameter.BaseConfigStruct baseConfigStruct = OverrideParameter.BaseConfigTable[index];
        if (baseConfigStruct.TheConfigID.ToString() == StringValueWin)
          return baseConfigStruct;
      }
      return (OverrideParameter.BaseConfigStruct) null;
    }

    static OverrideParameter()
    {
      OverrideParameter.OverrideParameterTypeIdent[] parameterTypeIdentArray = new OverrideParameter.OverrideParameterTypeIdent[83];
      parameterTypeIdentArray[0] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Unknown, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, string.Empty);
      parameterTypeIdentArray[1] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.WarmerPipe, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, ConfigurationParameter.WormerPipeValues.RETURN.ToString());
      parameterTypeIdentArray[2] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CustomID, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "0");
      parameterTypeIdentArray[3] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ReadingDate, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "01.01");
      parameterTypeIdentArray[4] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MBusIdentificationNo, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "0");
      parameterTypeIdentArray[5] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EnergyResolution, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, true, true, false, MeterResources.NoResource, "0.000MWh");
      parameterTypeIdentArray[6] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ChangeOver, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, ConfigurationParameter.ChangeOverValues.Heating.ToString());
      parameterTypeIdentArray[7] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.VolumeResolution, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, true, true, false, MeterResources.NoResource, "0.000m\u00B3");
      parameterTypeIdentArray[8] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.VolumePulsValue, (ushort) 4, OverrideParameter.ValueAccessType.NoValue, false, true, false, MeterResources.NoResource, 0.01.ToString());
      parameterTypeIdentArray[9] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1PulsValue, (ushort) 4, OverrideParameter.ValueAccessType.NoValue, false, true, true, MeterResources.Inp1On, 0.01.ToString());
      parameterTypeIdentArray[10] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1Unit, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, true, true, true, MeterResources.Inp1On, "0.000m\u00B3");
      parameterTypeIdentArray[11] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2PulsValue, (ushort) 4, OverrideParameter.ValueAccessType.NoValue, false, true, true, MeterResources.Inp2On, 0.01.ToString());
      parameterTypeIdentArray[12] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2Unit, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, true, true, true, MeterResources.Inp2On, "0.000m\u00B3");
      parameterTypeIdentArray[13] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Output1Function, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Out1On, ConfigurationParameter.OutputFunctions.KEINE.ToString());
      parameterTypeIdentArray[14] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Output2Function, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Out2On, ConfigurationParameter.OutputFunctions.KEINE.ToString());
      parameterTypeIdentArray[15] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.BaseConfig, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, true, true, false, MeterResources.NoResource, ConfigurationParameter.BaseConfigSettings.HSrH.ToString());
      parameterTypeIdentArray[16] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MBusAddress, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "0");
      parameterTypeIdentArray[17] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.SerialNumber, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[18] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MeterID, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[19] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.BaseTypeID, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[20] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EnergyActualValue, (ushort) 0, OverrideParameter.ValueAccessType.Protected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[21] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EnergyDueDateValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[22] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EnergyDueDateLastValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[23] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.VolumeActualValue, (ushort) 0, OverrideParameter.ValueAccessType.Protected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[24] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.VolumeDueDateValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[25] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.VolumeDueDateLastValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[26] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1ActualValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp1On, "0");
      parameterTypeIdentArray[27] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1DueDateValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp1On, "0");
      parameterTypeIdentArray[28] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1DueDateLastValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp1On, "0");
      parameterTypeIdentArray[29] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2ActualValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp2On, "0");
      parameterTypeIdentArray[30] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2DueDateValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp2On, "0");
      parameterTypeIdentArray[31] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2DueDateLastValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, true, MeterResources.Inp2On, "0");
      parameterTypeIdentArray[32] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CEnergyActualValue, (ushort) 0, OverrideParameter.ValueAccessType.Protected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[33] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CEnergyDueDateValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[34] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CEnergyDueDateLastValue, (ushort) 0, OverrideParameter.ValueAccessType.NotProtected, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[35] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.FactoryTypeID, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[36] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ModuleType, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, false, true, false, MeterResources.NoResource, ModuleTypeValues.NoValue.ToString());
      InOutFunctions inOutFunctions = InOutFunctions.IO1_Off;
      string str1 = inOutFunctions.ToString();
      inOutFunctions = InOutFunctions.IO2_Off;
      string str2 = inOutFunctions.ToString();
      parameterTypeIdentArray[37] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.IO_Functions, (ushort) 1, OverrideParameter.ValueAccessType.NoValue, false, true, false, MeterResources.NoResource, str1 + ";" + str2);
      parameterTypeIdentArray[38] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1IdNumber, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Inp1On, "0");
      parameterTypeIdentArray[39] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2IdNumber, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Inp2On, "0");
      parameterTypeIdentArray[40] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input1Type, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Inp1Type, "0");
      parameterTypeIdentArray[41] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Input2Type, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.Inp1Type, "0");
      parameterTypeIdentArray[42] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MenuOverride, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[43] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MBusListOverride, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[44] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ClearNotProtectedValues, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "0");
      parameterTypeIdentArray[45] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ClearProtectedValues, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[46] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Baudrate, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "2400");
      parameterTypeIdentArray[47] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.FixedTempSetup, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "OFF");
      parameterTypeIdentArray[48] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.FixedTempValue, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[49] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MimTempDiffSetup, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "OFF");
      parameterTypeIdentArray[50] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MinTempDiffPlusTemp, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "1");
      parameterTypeIdentArray[51] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.MinTempDiffMinusTemp, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "1");
      parameterTypeIdentArray[52] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.TarifFunction, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "OFF");
      parameterTypeIdentArray[53] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.TarifRefTemp, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[54] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.TarifEnergy0, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[55] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.TarifEnergy1, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[56] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CycleTimeFast, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "10");
      parameterTypeIdentArray[57] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CycleTimeStandard, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "30");
      parameterTypeIdentArray[58] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HeatThresholdTemp, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "18");
      parameterTypeIdentArray[59] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EndOfBattery, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "1980");
      parameterTypeIdentArray[60] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.EndOfCalibration, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, true, MeterResources.NoResource, "1980");
      parameterTypeIdentArray[61] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.CycleTimeDynamic, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "OFF");
      parameterTypeIdentArray[62] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DeviceClock, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[63] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_Factor_Weighting, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[64] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_SensorMode, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[65] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_Scale, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[66] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_Factor_CH, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[67] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_Factor_CHR, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[68] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.HCA_ActualValue, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[69] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.RadioSendInterval, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[70] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.LastErrorDate, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[71] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.SleepMode, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[72] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.NumberOfSubDevices, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[73] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DeviceName, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[74] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.ErrorDate, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[75] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Medium, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[76] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.FirmwareVersion, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[77] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DiagnosticString, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[78] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DeviceHasError, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[79] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.Protected, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[80] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DeviceUnit, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[81] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DaKonSerialNumber, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      parameterTypeIdentArray[82] = new OverrideParameter.OverrideParameterTypeIdent(OverrideID.DaKonRegisterNumber, (ushort) 0, OverrideParameter.ValueAccessType.NoValue, false, false, false, MeterResources.NoResource, "0");
      OverrideParameter.OverrideParameterInfoList = parameterTypeIdentArray;
      OverrideParameter.BaseConfigTable = new OverrideParameter.BaseConfigStruct[16]
      {
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.HSrL, false, false, false, true, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.HSrH, false, false, false, false, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.HdrL, false, false, false, true, true, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.HEnL, false, false, false, true, false, true),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.CSrL, false, true, false, true, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.CSrH, false, true, false, false, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.CdrL, false, true, false, true, true, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.CEnL, false, true, false, true, false, true),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.OSrL, false, false, true, true, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.OSrH, false, false, true, false, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.OdrL, false, false, true, true, true, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.OEnL, false, false, true, true, false, true),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.FSrL, true, false, false, true, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.FSrH, true, false, false, false, false, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.FdrL, true, false, false, true, true, false),
        new OverrideParameter.BaseConfigStruct(ConfigurationParameter.BaseConfigSettings.FEnL, true, false, false, true, false, true)
      };
    }

    public enum ValueAccessType
    {
      NoValue,
      Protected,
      NotProtected,
    }

    public struct OverrideParameterTypeIdent
    {
      public OverrideID OverrideTypeID;
      public ushort ByteSize;
      public bool IsStruct;
      public OverrideParameter.ValueAccessType ValueType;
      public bool AtFunctionTable;
      public string DefaultValue;
      public MeterResources NeadedResource;
      public bool HasWritePermission;

      internal OverrideParameterTypeIdent(
        OverrideID OverrideTypeID_In,
        ushort ByteSizeIn,
        OverrideParameter.ValueAccessType ValueTypeIn,
        bool IsStructIn,
        bool AtFunctionTableIn,
        bool HasWritePermissionIn,
        MeterResources NeadedResourceIn,
        string DefaultValueIn)
      {
        this.OverrideTypeID = OverrideTypeID_In;
        this.ByteSize = ByteSizeIn;
        this.ValueType = ValueTypeIn;
        this.IsStruct = IsStructIn;
        this.HasWritePermission = HasWritePermissionIn;
        this.AtFunctionTable = AtFunctionTableIn;
        this.NeadedResource = NeadedResourceIn;
        this.DefaultValue = DefaultValueIn;
      }
    }

    public class BaseConfigStruct
    {
      public ConfigurationParameter.BaseConfigSettings TheConfigID;
      public bool Cooling;
      public bool HeatAndCooling;
      public bool EnergyOff;
      public bool FrequenzLimitTo1Hz;
      public bool DoubleReed;
      public bool Encoder;

      public BaseConfigStruct(
        ConfigurationParameter.BaseConfigSettings TheId,
        bool Cool,
        bool HeatAndCool,
        bool EnOff,
        bool Freq1Hz,
        bool DReed,
        bool Enc)
      {
        this.TheConfigID = TheId;
        this.Cooling = Cool;
        this.HeatAndCooling = HeatAndCool;
        this.EnergyOff = EnOff;
        this.FrequenzLimitTo1Hz = Freq1Hz;
        this.DoubleReed = DReed;
        this.Encoder = Enc;
      }
    }
  }
}
