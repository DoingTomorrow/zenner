// Decompiled with JetBrains decompiler
// Type: S3_Handler.MeterScaling
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class MeterScaling
  {
    private static Logger S3_ScalingLogger = LogManager.GetLogger(nameof (S3_ScalingLogger));
    private S3_Meter MyMeter;
    internal ResolutionData energyResolutionInfo;
    internal Decimal energyLcdToMWhFactor;
    internal Decimal energyLcdToBaseUnitFactor;
    internal ResolutionData volumeResolutionInfo;
    internal Decimal volumeLcdToQmFactor;
    internal Decimal volumePulsValueLiterPerImpuls;
    internal InputData[] inpData = new InputData[3];
    internal static Dictionary<string, MeterScaling.SubUnits> C5EnergyResolutions;
    internal static Dictionary<string, MeterScaling.SubUnits> C5VolumeResolutions;

    internal MeterScaling(S3_Meter MyMeter)
    {
      this.MyMeter = MyMeter;
      if (this.MyMeter.MyIdentification.IsInput1Available)
        this.inpData[0] = new InputData();
      if (this.MyMeter.MyIdentification.IsInput2Available)
        this.inpData[1] = new InputData();
      if (this.MyMeter.MyIdentification.IsInput3Available)
        this.inpData[2] = new InputData();
      if (MeterScaling.C5EnergyResolutions != null)
        return;
      lock (MeterScaling.S3_ScalingLogger)
      {
        if (MeterScaling.C5EnergyResolutions == null)
        {
          MeterScaling.C5EnergyResolutions = new Dictionary<string, MeterScaling.SubUnits>();
          MeterScaling.C5EnergyResolutions.Add("0.0kWh", new MeterScaling.SubUnits("0.00kW", "0.0kW", "0.0000Wh", 6));
          MeterScaling.C5EnergyResolutions.Add("0kWh", new MeterScaling.SubUnits("0.0kW", "0kW", "0.000Wh", 6));
          MeterScaling.C5EnergyResolutions.Add("0.000MWh", new MeterScaling.SubUnits("0.0kW", "0kW", "0.000Wh", 6));
          MeterScaling.C5EnergyResolutions.Add("0.00MWh", new MeterScaling.SubUnits("0kW", "0.00MW", "0.00Wh", 6));
          MeterScaling.C5EnergyResolutions.Add("0.000GJ", new MeterScaling.SubUnits("0.0000GJ/h", "0.000GJ/h", "0.0000000GJ", 4));
          MeterScaling.C5EnergyResolutions.Add("0.00GJ", new MeterScaling.SubUnits("0.000GJ/h", "0.00GJ/h", "0.0000000GJ", 5));
          MeterScaling.C5VolumeResolutions = new Dictionary<string, MeterScaling.SubUnits>();
          MeterScaling.C5VolumeResolutions.Add("0L", new MeterScaling.SubUnits("0L/h", "0L/h", "0.000L", 3));
          MeterScaling.C5VolumeResolutions.Add("0.000m\u00B3", new MeterScaling.SubUnits("0.000m\u00B3/h", "0.000m\u00B3/h", "0.000L", 3));
          if (MyMeter.MyIdentification.IsWR4)
          {
            MeterScaling.C5VolumeResolutions.Add("0.00m\u00B3", new MeterScaling.SubUnits("0.00m\u00B3/h", "0.00m\u00B3/h", "0.00L", 3));
            MeterScaling.C5VolumeResolutions.Add("0.0m\u00B3", new MeterScaling.SubUnits("0.0m\u00B3/h", "0.0m\u00B3/h", "0.0L", 3));
            MeterScaling.C5VolumeResolutions.Add("0m\u00B3", new MeterScaling.SubUnits("0m\u00B3/h", "0m\u00B3/h", "0.000m\u00B3", 3));
          }
        }
      }
    }

    internal MeterScaling Clone(S3_Meter cloneMeter)
    {
      MeterScaling meterScaling = new MeterScaling(cloneMeter);
      meterScaling.energyResolutionInfo = this.energyResolutionInfo;
      meterScaling.energyLcdToMWhFactor = this.energyLcdToMWhFactor;
      meterScaling.energyLcdToBaseUnitFactor = this.energyLcdToBaseUnitFactor;
      meterScaling.volumeResolutionInfo = this.volumeResolutionInfo;
      meterScaling.volumeLcdToQmFactor = this.volumeLcdToQmFactor;
      meterScaling.volumePulsValueLiterPerImpuls = this.volumePulsValueLiterPerImpuls;
      for (int index = 0; index < this.inpData.Length; ++index)
      {
        if (this.inpData[index] != null)
          meterScaling.inpData[index] = new InputData(this.inpData[index]);
      }
      return meterScaling;
    }

    internal string[] GetAllowedEnergyResolutions()
    {
      string[] array = new string[MeterScaling.C5EnergyResolutions.Count];
      MeterScaling.C5EnergyResolutions.Keys.CopyTo(array, 0);
      return array;
    }

    internal string[] GetAllowedVolumeResolutions()
    {
      string[] array = new string[MeterScaling.C5VolumeResolutions.Count];
      MeterScaling.C5VolumeResolutions.Keys.CopyTo(array, 0);
      return array;
    }

    internal bool ReadSettingsFromParameter()
    {
      try
      {
        byte byteValue1 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.energyUnitIndex.ToString()].GetByteValue();
        this.energyResolutionInfo = MeterUnits.resolutionDataFromResolutionString[LcdUnitsC2.resolutionStringFromResolutionId[byteValue1]];
        this.energyLcdToBaseUnitFactor = this.energyResolutionInfo.baseUnitFactor / this.energyResolutionInfo.displayFactor;
        this.energyLcdToMWhFactor = !(this.energyResolutionInfo.baseUnitString == "GJ") ? this.energyLcdToBaseUnitFactor : this.energyLcdToBaseUnitFactor / 3.6M;
        byte byteValue2 = this.MyMeter.MyParameters.ParameterByName["volumeUnitIndex"].GetByteValue();
        this.volumeResolutionInfo = MeterUnits.resolutionDataFromResolutionString[LcdUnitsC2.resolutionStringFromResolutionId[byteValue2]];
        this.volumeLcdToQmFactor = this.volumeResolutionInfo.baseUnitFactor / this.volumeResolutionInfo.displayFactor;
        float floatValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_VolFactor.ToString()].GetFloatValue();
        this.volumePulsValueLiterPerImpuls = Convert.ToDecimal((double) floatValue);
        this.volumePulsValueLiterPerImpuls *= this.volumeResolutionInfo.baseUnitFactor / this.volumeResolutionInfo.displayFactor * 1000M;
        if ((double) (float) (this.volumePulsValueLiterPerImpuls / this.volumeResolutionInfo.baseUnitFactor * this.volumeResolutionInfo.displayFactor / 1000M) != (double) floatValue)
          throw new Exception("Con_VolFactor rounding error");
        for (int index = 0; index < 3; ++index)
        {
          if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputUnitIndex[index]) && this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputFactor[index]) && this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_Meter.inputDevisor[index]) && this.inpData[index] != null)
          {
            InputData inputData = this.inpData[index];
            byte byteValue3 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputUnitIndex[index]].GetByteValue();
            inputData.inputUnitInfo = MeterUnits.resolutionDataFromResolutionString[LcdUnitsC2.resolutionStringFromResolutionId[byteValue3]];
            S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[index]];
            inputData.impulsValueFactor = s3Parameter1.GetUshortValue();
            S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputDevisor[index]];
            inputData.impulsValueDivisor = s3Parameter2.GetUshortValue();
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription("Exception");
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "ReadSettingsFromParameter error", MeterScaling.S3_ScalingLogger);
      }
    }

    internal bool WriteSettingsToParameter()
    {
      this.MyMeter.MyParameters.ParameterByName["volumeUnitIndex"].SetByteValue(LcdUnitsC2.resolutionIdFromResolutionString[this.volumeResolutionInfo.resolutionString]);
      float NewValue1 = -1f;
      S3_ParameterNames s3ParameterNames;
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.VolInputSetup.ToString()))
      {
        SortedList<string, S3_Parameter> parameterByName = this.MyMeter.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.VolInputSetup;
        string key = s3ParameterNames.ToString();
        if (parameterByName[key].GetByteValue() == (byte) 14)
          NewValue1 = (float) (1M / this.volumeResolutionInfo.baseUnitFactor * this.volumeResolutionInfo.displayFactor / 1000000M);
      }
      if ((double) NewValue1 < 0.0)
        NewValue1 = (float) (this.volumePulsValueLiterPerImpuls / this.volumeResolutionInfo.baseUnitFactor * this.volumeResolutionInfo.displayFactor / 1000M);
      SortedList<string, S3_Parameter> parameterByName1 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_VolFactor;
      string key1 = s3ParameterNames.ToString();
      parameterByName1[key1].SetFloatValue(NewValue1);
      Decimal num1 = 1000M * this.volumeResolutionInfo.baseUnitFactor / this.volumeResolutionInfo.displayFactor;
      SortedList<string, S3_Parameter> parameterByName2 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.energyUnitIndex;
      string key2 = s3ParameterNames.ToString();
      parameterByName2[key2].SetByteValue(LcdUnitsC2.resolutionIdFromResolutionString[this.energyResolutionInfo.resolutionString]);
      Decimal num2 = this.energyResolutionInfo.displayFactor / this.energyResolutionInfo.baseUnitFactor / 1000M;
      float NewValue2;
      float NewValue3;
      if (this.energyResolutionInfo.baseUnitString == "GJ")
      {
        NewValue2 = (float) (num2 * 3.6M * num1);
        NewValue3 = (float) (num2 * 3.6M * 10M);
      }
      else
      {
        NewValue2 = (float) (num2 * num1);
        NewValue3 = (float) (num2 * 10M);
      }
      SortedList<string, S3_Parameter> parameterByName3 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_water_glycol_value_e20;
      string key3 = s3ParameterNames.ToString();
      if (parameterByName3.ContainsKey(key3))
      {
        float[] numArray = new float[15]
        {
          1f,
          0.955f,
          0.94f,
          0.925f,
          0.909f,
          0.893f,
          0.877f,
          0.86f,
          0.998f,
          0.985f,
          0.971f,
          0.956f,
          0.939f,
          0.919f,
          0.899f
        };
        byte? nullable = new byte?();
        SortedList<string, S3_Parameter> parameterByName4 = this.MyMeter.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
        string key4 = s3ParameterNames.ToString();
        if (parameterByName4.ContainsKey(key4))
        {
          SortedList<string, S3_Parameter> parameterByName5 = this.MyMeter.MyParameters.ParameterByName;
          s3ParameterNames = S3_ParameterNames.Heap_GlycolTableIndex;
          string key5 = s3ParameterNames.ToString();
          nullable = new byte?(parameterByName5[key5].GetByteValue());
        }
        SortedList<string, S3_Parameter> parameterByName6 = this.MyMeter.MyParameters.ParameterByName;
        s3ParameterNames = S3_ParameterNames.Con_water_glycol_value_e20;
        string key6 = s3ParameterNames.ToString();
        int blockStartAddress = parameterByName6[key6].BlockStartAddress;
        for (int index = 0; index < 15; ++index)
          this.MyMeter.MyDeviceMemory.SetFloatValue(blockStartAddress + index * 4, NewValue2 * numArray[index]);
        if (this.MyMeter.MyResources.IsResourceAvailable("GlycolActive") && nullable.HasValue)
          NewValue2 = this.MyMeter.MyDeviceMemory.GetFloatValue(blockStartAddress + (int) nullable.Value * 4);
      }
      SortedList<string, S3_Parameter> parameterByName7 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_EnergyFactor;
      string key7 = s3ParameterNames.ToString();
      parameterByName7[key7].SetFloatValue(NewValue2);
      SortedList<string, S3_Parameter> parameterByName8 = this.MyMeter.MyParameters.ParameterByName;
      s3ParameterNames = S3_ParameterNames.Con_CalPowerFactor;
      string key8 = s3ParameterNames.ToString();
      parameterByName8[key8].SetFloatValue(NewValue3);
      for (int index = 0; index < 3; ++index)
      {
        if (this.inpData[index] != null)
        {
          InputData inputData = this.inpData[index];
          byte NewValue4 = LcdUnitsC2.resolutionIdFromResolutionString[inputData.inputResolutionString];
          this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputUnitIndex[index]].SetByteValue(NewValue4);
          this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[index]].SetUshortValue(inputData.impulsValueFactor);
          this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputDevisor[index]].SetUshortValue(inputData.impulsValueDivisor);
        }
      }
      return this.WriteParameterDependencies();
    }

    internal bool WriteParameterDependencies()
    {
      byte approvalRevision = this.MyMeter.MyFunctions.MyDatabase.GetApprovalRevision((int) this.MyMeter.MyIdentification.HardwareTypeId);
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["ApprovalRevison"];
      if (this.MyMeter.IsWriteProtected)
      {
        byte byteValue = s3Parameter1.GetByteValue();
        if ((int) approvalRevision != (int) byteValue)
          MeterScaling.S3_ScalingLogger.Warn("ApprovalRevison in Gerät: " + byteValue.ToString() + " \nApprovalRevison in Datenbank: " + approvalRevision.ToString() + " \nHardwareTypeId: " + ((int) this.MyMeter.MyIdentification.HardwareTypeId).ToString() + " \nEs gibt C5 Geräte im Feld die eine falsche ApprovalRevision haben. Nachträglich ist das Problem behoben worden. Das hat aber dazu geführt, dass beim Lesen versucht GMM die richtige ApprovalRevision in das Gerät zu schreiben. Das geht aber nicht da der Parameter ApprovalRevisionin in einem geschütztem Bereich liegt. Es wurde entschieden genau diese Änderung der ApprovalRevision nicht durchzuführen. Alle C5 Geräte die eine falsche ApprovalRevision haben, sollen weiterhin die falschen Wert haben. Das beeinträchtigt nicht die Hauptfunktionalität des Gerätes.");
      }
      else
        s3Parameter1.SetByteValue(approvalRevision);
      ushort ushortValue1 = this.MyMeter.MyParameters.ParameterByName["Device_Setup"].GetUshortValue();
      bool flag1 = false;
      if (((uint) ushortValue1 & 256U) > 0U)
        flag1 = true;
      bool flag2 = false;
      if (((uint) ushortValue1 & 512U) > 0U)
        flag2 = true;
      ushort ushortValue2 = this.MyMeter.MyParameters.ParameterByName["Device_Setup_2"].GetUshortValue();
      bool flag3 = false;
      if (((uint) ushortValue2 & 1U) > 0U)
        flag3 = true;
      ushort num1 = 3840;
      if (flag1)
        num1 = !flag2 ? (!flag3 ? (ushort) 1024 : (ushort) 3072) : (ushort) 3328;
      else if (flag2)
        num1 = !flag3 ? (ushort) 2560 : (ushort) 2816;
      if (!this.MyMeter.IsWriteProtected)
      {
        S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["Con_Medium_Generation"];
        ushort NewValue = (ushort) ((uint) s3Parameter2.GetUshortValue() & (uint) byte.MaxValue | (uint) num1);
        if (!s3Parameter2.SetUshortValue(NewValue))
          return false;
      }
      S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["SerDev0_Medium_Generation"];
      ushort NewValue1 = (ushort) ((uint) s3Parameter3.GetUshortValue() & (uint) byte.MaxValue | (uint) num1);
      if (!s3Parameter3.SetUshortValue(NewValue1))
        return false;
      this.GarantInputBaseSettings(this.MyMeter.MyTransmitParameterManager.AreVirtualDevicesUsed());
      if (!this.MyMeter.IsWriteProtected)
      {
        NotProtectedRange childMemoryBlock = (NotProtectedRange) this.MyMeter.MyDeviceMemory.BlockWriteProtTable.childMemoryBlocks[0];
        if (this.MyMeter.MyParameters.AddressLables.ContainsKey("CSTACK") && this.MyMeter.MyParameters.AddressLables.ContainsKey("SERIE3_CONFIG_HEAP"))
        {
          int addressLable1 = this.MyMeter.MyParameters.AddressLables["SERIE3_CONFIG_HEAP"];
          int addressLable2 = this.MyMeter.MyParameters.AddressLables["CSTACK"];
          int num2 = (int) childMemoryBlock.NotProtectedAddress + (int) childMemoryBlock.NotProtectedLength;
          if (num2 > addressLable1 && num2 < addressLable2)
            childMemoryBlock.memNotProtectedLength = (ushort) ((uint) addressLable2 - (uint) childMemoryBlock.NotProtectedAddress);
        }
      }
      return true;
    }

    internal bool GarantInputBaseSettings(bool virtualDevicesUsed)
    {
      ushort num = (ushort) ((uint) this.MyMeter.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"].GetUshortValue() & (uint) byte.MaxValue);
      for (int inputIndex = 0; inputIndex < 3; ++inputIndex)
      {
        if (this.MyMeter.IsInputAvailable(inputIndex))
        {
          ushort ushortValue = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[inputIndex]].GetUshortValue();
          if (ushortValue == (ushort) 0)
          {
            S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]];
            ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() & 65280U | (uint) num);
            s3Parameter.SetUshortValue(NewValue);
          }
          uint NewValue1 = S3_DeviceIdentification.virtualDeviceOffSerialNumber[inputIndex];
          if (!virtualDevicesUsed)
          {
            this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].SetUintValue(NewValue1);
            this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]].SetUshortValue((ushort) 254);
          }
          else
          {
            S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]];
            S3_ParameterNames s3ParameterNames;
            if (ushortValue == (ushort) 0)
            {
              SortedList<string, S3_Parameter> parameterByName = this.MyMeter.MyParameters.ParameterByName;
              s3ParameterNames = S3_ParameterNames.SerDev0_IdentNo;
              string key = s3ParameterNames.ToString();
              uint uintValue = parameterByName[key].GetUintValue();
              this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].SetUintValue(uintValue);
            }
            else if ((int) this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].GetUintValue() == (int) NewValue1)
            {
              SortedList<string, S3_Parameter> parameterByName1 = this.MyMeter.MyParameters.ParameterByName;
              s3ParameterNames = S3_ParameterNames.SerDev0_IdentNo;
              string key1 = s3ParameterNames.ToString();
              uint NewValue2 = (parameterByName1[key1].GetUintValue() & 16777215U) + (uint) (16777216 * (inputIndex + 1));
              this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].SetUintValue(NewValue2);
              SortedList<string, S3_Parameter> parameterByName2 = this.MyMeter.MyParameters.ParameterByName;
              s3ParameterNames = S3_ParameterNames.SerDev0_SelectedList_PrimaryAddress;
              string key2 = s3ParameterNames.ToString();
              ushort NewValue3 = (ushort) ((uint) (ushort) ((uint) parameterByName2[key2].GetUshortValue() & (uint) byte.MaxValue) + (uint) (ushort) (inputIndex + 1));
              if (NewValue3 > (ushort) 250)
                NewValue3 -= (ushort) 250;
              this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]].SetUshortValue(NewValue3);
            }
          }
        }
      }
      return true;
    }

    internal bool GarantFinalConfigurations()
    {
      int index1 = this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.DeviceSetupNp.ToString());
      if (index1 >= 0)
      {
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName.Values[index1];
        byte byteValue = s3Parameter.GetByteValue();
        byte num = !this.MyMeter.MyResources.IsResourceAvailable("MassAvailable") ? (byte) ((uint) byteValue & 4294967294U) : (byte) ((uint) byteValue | 1U);
        byte NewValue = !this.MyMeter.MyResources.IsResourceAvailable("VolumeBackFlowAvailable") && !this.MyMeter.MyResources.IsResourceAvailable("IUF") ? (byte) ((uint) num & 4294967293U) : (byte) ((uint) num | 2U);
        s3Parameter.SetByteValue(NewValue);
      }
      int index2 = this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Con_Energy_VIF.ToString());
      if (index2 >= 0)
      {
        this.MyMeter.MyParameters.ParameterByName.Values[index2].SetUshortValue((ushort) this.energyResolutionInfo.mbusVIF);
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Volume_VIF.ToString()].SetUshortValue((ushort) this.volumeResolutionInfo.mbusVIF);
      }
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.VolInputSetup.ToString()))
      {
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_CalFlowFactor.ToString()];
        if (this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.VolInputSetup.ToString()].GetByteValue() == (byte) 14)
        {
          ResolutionData resolutionData = MeterUnits.GetResolutionData(MeterScaling.C5VolumeResolutions[this.volumeResolutionInfo.resolutionString].DifferentialUnit);
          Decimal NewValue = resolutionData.displayFactor / resolutionData.baseUnitFactor;
          s3Parameter.SetFloatValue((float) NewValue);
        }
        else
          s3Parameter.SetFloatValue(1f);
      }
      return true;
    }

    internal bool SetEnergyResolution(string resolution)
    {
      if (!MeterUnits.resolutionDataFromResolutionString.ContainsKey(resolution))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set the energy reslolution! The M-Bus VIF is not defined by " + resolution, MeterScaling.S3_ScalingLogger);
      this.energyResolutionInfo = MeterUnits.resolutionDataFromResolutionString[resolution];
      return true;
    }

    internal bool SetVolumeResolution(string resolution)
    {
      this.volumeResolutionInfo = MeterUnits.resolutionDataFromResolutionString[resolution];
      return true;
    }

    internal bool SetVolumePulsValue(string literPerImpuls)
    {
      if (Util.TryParse(literPerImpuls, out this.volumePulsValueLiterPerImpuls))
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal puls value");
      return false;
    }

    internal bool SetInputPulsValue(string impulsDisplayValueString, int inputIndex)
    {
      Decimal result;
      if (!Util.TryParse(impulsDisplayValueString, out result))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal input puls value input: " + inputIndex.ToString(), MeterScaling.S3_ScalingLogger);
      this.inpData[inputIndex].impulsValueDecimal = result * this.inpData[inputIndex].inputUnitInfo.DisplayPulsValue_To_MeterPulsValue_Factor;
      return true;
    }

    internal bool SetInputResolution(string resolution, int inputIndex)
    {
      InputData inputData = this.inpData[inputIndex];
      Decimal num = inputData.impulsValueDecimal * inputData.inputUnitInfo.MeterPulsValue_To_DisplayPulsValue_Factor;
      inputData.inputUnitInfo = MeterUnits.resolutionDataFromResolutionString[resolution];
      inputData.impulsValueDecimal = num / inputData.inputUnitInfo.MeterPulsValue_To_DisplayPulsValue_Factor;
      return true;
    }

    internal bool FrameCodeGetUnit(
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      out string unitInfo)
    {
      unitInfo = "Not a unit";
      string oldFrameString;
      if (!LcdUnitsC2.GetFrameType(ref frameCodeStorage, frameCodeStartOffset, out oldFrameString, out LcdUnitsC2FrameType _, out int _))
      {
        unitInfo = "Illegal unit";
        return false;
      }
      unitInfo = oldFrameString;
      return true;
    }

    internal bool FrameCodeAdjustUnit(
      ref byte[] frameCodeStorage,
      int frameCodeStartOffset,
      string functionFlags,
      out string unitInfo)
    {
      unitInfo = "Not a unit";
      string oldFrameString;
      LcdUnitsC2FrameType lcdUnitC2FrameType;
      int virtualDeviceNumber;
      if (!LcdUnitsC2.GetFrameType(ref frameCodeStorage, frameCodeStartOffset, out oldFrameString, out lcdUnitC2FrameType, out virtualDeviceNumber))
      {
        unitInfo = "Illegal unit";
        return false;
      }
      int newShift = -1;
      string unitString = (string) null;
      switch (virtualDeviceNumber)
      {
        case 1:
          unitString = this.inpData[0].inputResolutionString;
          break;
        case 2:
          unitString = this.inpData[1].inputResolutionString;
          break;
        case 3:
          unitString = this.inpData[2] != null ? this.inpData[2].inputResolutionString : throw new Exception("Imput 3 frame scanling not possible. Input not available!");
          break;
        default:
          switch (lcdUnitC2FrameType)
          {
            case LcdUnitsC2FrameType.EnergyFrame:
              if (MeterScaling.C5EnergyResolutions.ContainsKey(oldFrameString))
              {
                unitString = this.energyResolutionInfo.resolutionString;
                break;
              }
              unitString = MeterScaling.C5EnergyResolutions[this.energyResolutionInfo.resolutionString].HighResolution;
              newShift = MeterScaling.C5EnergyResolutions[this.energyResolutionInfo.resolutionString].HighResolutionShift;
              break;
            case LcdUnitsC2FrameType.VolumeFrame:
              if (MeterScaling.C5VolumeResolutions.ContainsKey(oldFrameString))
              {
                unitString = this.volumeResolutionInfo.resolutionString;
                break;
              }
              unitString = MeterScaling.C5VolumeResolutions[this.volumeResolutionInfo.resolutionString].HighResolution;
              newShift = MeterScaling.C5VolumeResolutions[this.volumeResolutionInfo.resolutionString].HighResolutionShift;
              break;
            case LcdUnitsC2FrameType.PowerFrame:
              unitString = !functionFlags.Contains(";" + FunctionFlags.MaxPowerFrame.ToString() + ";") ? MeterScaling.C5EnergyResolutions[this.energyResolutionInfo.resolutionString].DifferentialUnit : MeterScaling.C5EnergyResolutions[this.energyResolutionInfo.resolutionString].DifferentialUnitMax;
              break;
            case LcdUnitsC2FrameType.FlowFrame:
              unitString = !functionFlags.Contains(";" + FunctionFlags.MaxFlowFrame.ToString() + ";") ? MeterScaling.C5VolumeResolutions[this.volumeResolutionInfo.resolutionString].DifferentialUnit : MeterScaling.C5VolumeResolutions[this.volumeResolutionInfo.resolutionString].DifferentialUnitMax;
              break;
          }
          break;
      }
      if (unitString == null)
        return true;
      unitInfo = unitString;
      if (unitString == oldFrameString)
        return true;
      bool useVisibleAfterPointFrame = false;
      if (functionFlags.Contains(";" + FunctionFlags.AfterPointFrameOn.ToString() + ";"))
        useVisibleAfterPointFrame = true;
      if (!LcdUnitsC2.SetInterpreterFrameToUnit(unitString, ref frameCodeStorage, frameCodeStartOffset, useVisibleAfterPointFrame))
        return false;
      bool flag = true;
      if (newShift >= 0)
      {
        string shiftInfo;
        flag = LcdUnitsC2.SetHightResolutionShift((byte) newShift, ref frameCodeStorage, frameCodeStartOffset, out shiftInfo);
        unitInfo += shiftInfo;
      }
      return flag;
    }

    internal bool MBusDifVifAdjustUnit(
      byte[] buffer,
      int DifVifOffset,
      string parameterName,
      out byte[] newDifVif,
      out string theUnit)
    {
      theUnit = "";
      newDifVif = (byte[]) null;
      LoggerChanal loggerChanal = (LoggerChanal) null;
      S3_Parameter chanalParameter;
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(parameterName))
      {
        chanalParameter = this.MyMeter.MyParameters.ParameterByName[parameterName];
      }
      else
      {
        loggerChanal = this.MyMeter.MyLoggerManager.GetLoggerChanal(parameterName);
        if (loggerChanal == null)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown DifVif parameter detected! Value: " + parameterName, MeterScaling.S3_ScalingLogger);
        chanalParameter = loggerChanal.chanalParameter;
      }
      if (chanalParameter == null)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Parameter referenz not found for parameter: " + parameterName, MeterScaling.S3_ScalingLogger);
      theUnit = chanalParameter.GetUnit();
      if (theUnit == "")
        return true;
      if (!MeterUnits.resolutionDataFromResolutionString.ContainsKey(theUnit))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not adjust the unit! The M-Bus VIF is not defined for: " + parameterName + " Unit: " + theUnit, MeterScaling.S3_ScalingLogger);
      int mbusVif = MeterUnits.resolutionDataFromResolutionString[theUnit].mbusVIF;
      MBusDifVif mbusDifVif = new MBusDifVif(MBusDifVif.DifVifOptions.DifSizeUnchangabel);
      mbusDifVif.LoadDifVif(buffer, DifVifOffset);
      if (!mbusDifVif.IsDate_16bit && !mbusDifVif.IsDateTime_32bit)
      {
        if (MeterScaling.S3_ScalingLogger.IsTraceEnabled)
          MeterScaling.S3_ScalingLogger.Trace("Scale VIF to unit " + theUnit + " on address 0x" + DifVifOffset.ToString("x04"));
        mbusDifVif.SetScalingVif(mbusVif);
      }
      S3P_Device_Setup s3PDeviceSetup = new S3P_Device_Setup(this.MyMeter);
      if (s3PDeviceSetup.Cooling && !s3PDeviceSetup.Heating && mbusDifVif.TarifNumber == 1)
      {
        if (loggerChanal == null && mbusDifVif.StorageNumber == 0)
        {
          mbusDifVif.DifSizeUnchangable = false;
          mbusDifVif.TarifNumber = 0;
        }
        else
          mbusDifVif.TarifNumber = 0;
      }
      newDifVif = mbusDifVif.DifVifArray;
      return true;
    }

    internal enum DeviceSetupNp_Bits : byte
    {
      NP_DEVICE_SETUP_CALC_MASS = 1,
      NP_DEVICE_SETUP_MEAS_BACKFLOW = 2,
      NP_DEVICE_SETUP_RADIO_AES_ENABLE = 128, // 0x80
    }

    internal class SubUnits
    {
      internal string DifferentialUnit;
      internal string DifferentialUnitMax;
      internal string HighResolution;
      internal int HighResolutionShift;

      internal SubUnits(
        string DifferentialUnit,
        string DifferentialUnitMax,
        string HighResolution,
        int HighResolutionShift)
      {
        this.DifferentialUnit = DifferentialUnit;
        this.DifferentialUnitMax = DifferentialUnitMax;
        this.HighResolution = HighResolution;
        this.HighResolutionShift = HighResolutionShift;
      }
    }
  }
}
