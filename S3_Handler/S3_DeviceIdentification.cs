// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DeviceIdentification
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using HandlerLib;
using System;
using System.Globalization;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_DeviceIdentification : S3_DeviceId
  {
    private S3_Meter MyMeter;
    internal static uint[] virtualDeviceOffSerialNumber = new uint[3]
    {
      2863311530U,
      3149642683U,
      3435973836U
    };
    private static ushort[] InputDefaultMediumAndGeneration = new ushort[3]
    {
      (ushort) 5632,
      (ushort) 1536,
      (ushort) 1792
    };

    internal S3_DeviceIdentification(S3_Meter MyMeter) => this.MyMeter = MyMeter;

    internal S3_DeviceIdentification Clone(S3_Meter TheCloneMeter)
    {
      S3_DeviceIdentification TheClone = new S3_DeviceIdentification(TheCloneMeter);
      this.Write_S3_DeviceId_CloneData((S3_DeviceId) TheClone);
      return TheClone;
    }

    internal S3_DeviceId ReadHardwareIdentification()
    {
      int index = this.MyMeter.MyParameters.AddressLables.IndexOfKey("SERIE3_PROTECTED_CONFIG");
      if (index >= 0)
        this.MyMeter.MyDeviceMemory.flashStartAddress = this.MyMeter.MyParameters.AddressLables.Values[index];
      if (!this.MyMeter.MyDeviceMemory.ReadDataFromConnectedDevice(this.MyMeter.MyDeviceMemory.flashStartAddress, 512))
        return (S3_DeviceId) null;
      this.MyMeter.MyDeviceMemory.flashNextReadAddress = this.MyMeter.MyDeviceMemory.flashStartAddress + 512;
      this.MyMeter.CheckIdentificationChecksum();
      this.LoadHardwareIdFromParameter();
      return this.Clone();
    }

    internal void AddIdsFromVersion(ReadVersionData versionData)
    {
      uint version = versionData.Version;
      uint num = versionData.HardwareIdentification & 4095U;
      this.FirmwareVersion = version;
      this.HardwareMask = num;
      this.FirmwareVersionString = ParameterService.GetVersionString((long) this.FirmwareVersion, 8);
      this.Signature = versionData.FirmwareSignature;
      this.BuildRevision = versionData.BuildRevision;
      this.MBusMedium = versionData.MBusMedium;
    }

    internal void AddIdsFromTypeData(
      uint FirmwareVersion,
      uint HardwareMask,
      uint HardwareTypeId,
      uint mapId)
    {
      if (this.FirmwareVersion > 0U)
      {
        this.MapId = (uint) (this.MyMeter.MyFunctions.MyDatabase.HardwareAndFirmwareInfos.GetHardwareAndFirmwareInfo((int) HardwareMask, (int) this.FirmwareVersion) ?? throw new Exception("Hardware - firmware combination not found! Database error?")).MapID;
      }
      else
      {
        this.FirmwareVersion = FirmwareVersion;
        this.MapId = mapId;
      }
      this.HardwareMask = HardwareMask;
      this.HardwareTypeId = HardwareTypeId;
      this.FirmwareVersionString = ParameterService.GetVersionString((long) this.FirmwareVersion, 8);
    }

    internal void LoadDeviceIdFromParameter()
    {
      this.LoadHardwareIdFromParameter();
      this.LoadTypeIdFromParameter();
    }

    internal void LoadHardwareIdFromParameter()
    {
      this.MeterId = this.MyMeter.MyParameters.ParameterByName["Con_MeterId"].GetUintValue();
      this.SerialNumber = this.MyMeter.MyParameters.ParameterByName["Con_SerialNumber"].GetUintValue();
      this.ApprovalRevison = this.MyMeter.MyParameters.ParameterByName["ApprovalRevison"].GetByteValue();
      ushort ushortValue1 = this.MyMeter.MyParameters.ParameterByName["Con_Manufacturer"].GetUshortValue();
      ushort ushortValue2 = this.MyMeter.MyParameters.ParameterByName["Con_Medium_Generation"].GetUshortValue();
      char mediumGeneration = S3_DeviceIdentification.GetObisMediumFromMBusMediumGeneration(ushortValue2);
      string str = ((byte) ushortValue2).ToString("X02");
      string manufacturer = MBusDevice.GetManufacturer((short) ushortValue1);
      this.FullSerialNumber = mediumGeneration.ToString() + manufacturer + str + this.SerialNumber.ToString("X08");
    }

    internal static char GetObisMediumFromMBusMediumGeneration(ushort mediumGeneration)
    {
      char mediumGeneration1 = '?';
      MBusDeviceType mbusDeviceType = (MBusDeviceType) ((uint) mediumGeneration >> 8);
      int num;
      switch (mbusDeviceType)
      {
        case MBusDeviceType.HEAT_OUTLET:
        case MBusDeviceType.HEAT_INLET:
        case MBusDeviceType.HEAT_AND_COOL:
          num = 1;
          break;
        default:
          num = mbusDeviceType == MBusDeviceType.HEAT_AND_COOL ? 1 : 0;
          break;
      }
      if (num != 0)
        mediumGeneration1 = '6';
      else if (mbusDeviceType == MBusDeviceType.COOL_INLET || mbusDeviceType == MBusDeviceType.COOL_OUTLET)
        mediumGeneration1 = '5';
      return mediumGeneration1;
    }

    internal void LoadTypeIdFromParameter()
    {
      uint uintValue1 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].GetUintValue();
      if (uintValue1 > 0U)
      {
        this.HardwareTypeId = uintValue1;
        this.HardwareMask = (uint) this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].GetUshortValue();
      }
      this.MyMeter.MyFunctions.MyDatabase.AddHardwareInfosFromHardwareTypeId((S3_DeviceId) this);
      this.MeterInfoId = this.MyMeter.MyParameters.ParameterByName["Con_MeterInfoId"].GetUintValue();
      this.BaseTypeId = this.MyMeter.MyParameters.ParameterByName["Con_BaseTypeId"].GetUintValue();
      this.MeterTypeId = this.MyMeter.MyParameters.ParameterByName["Con_MeterTypeId"].GetUintValue();
      this.SAP_MaterialNumber = this.MyMeter.MyParameters.ParameterByName["Con_SAP_MaterialNumber"].GetUintValue();
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()))
      {
        uint uintValue2 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].GetUintValue();
        if (uintValue2 > 0U)
          this.SAP_ProductionOrderNumber = uintValue2.ToString();
      }
      else
        this.SAP_ProductionOrderNumber = Utility.ZeroTerminatedAsciiStringToString(this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_ProductionOrderNumber.ToString()].GetByteArray(20));
      this.TypeCreationString = this.MyMeter.TypeCreationString;
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.VolumeMeterIdentification.ToString()))
        this.VolumeMeterIdentification = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.VolumeMeterIdentification.ToString()].GetUintValue();
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_RadioId.ToString()))
      {
        this.SerDev0_RadioId_Vol = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_RadioId.ToString()].GetUintValue();
        this.SerDev0_RadioId_Heat = this.SerDev0_RadioId_Vol + 1U;
        this.SerDev0_RadioId_Cool = this.SerDev0_RadioId_Vol + 2U;
      }
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev1_RadioId.ToString()))
        this.SerDev1_RadioId = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev1_RadioId.ToString()].GetUintValue();
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev2_RadioId.ToString()))
        this.SerDev2_RadioId = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev2_RadioId.ToString()].GetUintValue();
      if (!this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev3_RadioId.ToString()))
        return;
      this.SerDev3_RadioId = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev3_RadioId.ToString()].GetUintValue();
    }

    internal void GarantTypeIdConsistent()
    {
      if (this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].GetUintValue() == 0U)
      {
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].SetUintValue(this.HardwareTypeId);
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].SetUshortValue((ushort) this.HardwareMask);
      }
      this.MyMeter.MyFunctions.MyDatabase.AddHardwareInfosFromHardwareTypeId((S3_DeviceId) this);
    }

    internal void GarantCloneRules(S3_Meter preCloneMeter)
    {
      if (this.MyMeter.MyFunctions.SuppressCloneOptimization)
        return;
      ushort MeasurementWatchdogFunctionNumber = 1112;
      if ((preCloneMeter.MyIdentification.FirmwareVersion == 84025349U || preCloneMeter.MyIdentification.FirmwareVersion == 84029445U) && preCloneMeter.MyResources.IsResourceAvailable("MBus") && this.MyMeter.MyFunctionManager.GetFunction(MeasurementWatchdogFunctionNumber) == null)
      {
        int index = this.MyMeter.MyFunctions.MyDatabase.CachedFunctionList.FindIndex((Predicate<Function>) (f => f.FunctionNumber == (int) MeasurementWatchdogFunctionNumber));
        if (index < 0)
          return;
        int count = this.MyMeter.MyFunctionManager.MyFunctionTable.childMemoryBlocks[1].childMemoryBlocks.Count;
        this.MyMeter.MyFunctionManager.AddFunction(1, this.MyMeter.MyFunctions.MyDatabase.CachedFunctionList[index], count);
      }
    }

    internal S3_DeviceId GetDeviceId() => this.Clone();

    internal string GetDeviceIdInfo()
    {
      StringBuilder stringBuilder = new StringBuilder(5000);
      if (this.IdentificationCheckState != 0)
        stringBuilder.AppendLine("Identification checksum: " + this.IdentificationCheckState.ToString());
      if (this.MyMeter.TypeCreationString != null)
        stringBuilder.AppendLine("TypeCreationString: " + this.MyMeter.TypeCreationString);
      if (this.MyMeter.MeterInfoDescription != null)
        stringBuilder.AppendLine("Type description: " + this.MyMeter.MeterInfoDescription);
      if (this.FullSerialNumber != null)
        stringBuilder.AppendLine("SerialNumber: " + this.FullSerialNumber);
      else
        stringBuilder.AppendLine("SerialNumber: " + this.SerialNumber.ToString("x08"));
      stringBuilder.Append("Firmware: " + this.FirmwareVersionString);
      if (this.Signature > (ushort) 0)
        stringBuilder.Append(" ; Signature: 0x" + this.Signature.ToString("x04"));
      if (this.BuildRevision > 0U)
        stringBuilder.Append(" ; BuildRevision: " + this.BuildRevision.ToString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("ApprovalRevison: " + this.ApprovalRevison.ToString());
      stringBuilder.AppendLine("Hardware: (0x" + this.HardwareMask.ToString("x04") + ") " + ParameterService.GetHardwareString(this.HardwareMask));
      stringBuilder.AppendLine("Inputs: " + this.GetInputString());
      stringBuilder.AppendLine("SAP_MaterialNumber: " + this.SAP_MaterialNumber.ToString());
      stringBuilder.AppendLine("SAP_ProductionOrderNumber: " + this.SAP_ProductionOrderNumber);
      stringBuilder.AppendLine("MeterId: " + this.MeterId.ToString());
      stringBuilder.AppendLine("MeterInfoId: " + this.MeterInfoId.ToString());
      stringBuilder.AppendLine("HardwareTypeId: " + this.HardwareTypeId.ToString());
      stringBuilder.AppendLine("MeterTypeId: " + this.MeterTypeId.ToString());
      stringBuilder.AppendLine("BaseTypeId: " + this.BaseTypeId.ToString());
      if (this.VolumeMeterIdentification > 0U)
        stringBuilder.AppendLine("VolumeMeterId: " + this.VolumeMeterIdentification.ToString("x04"));
      return stringBuilder.ToString();
    }

    internal string GetDeviceIdInfoForConfigurator()
    {
      StringBuilder stringBuilder = new StringBuilder(5000);
      if (this.IdentificationCheckState != 0)
        stringBuilder.AppendLine("Identification checksum: " + this.IdentificationCheckState.ToString());
      if (this.MyMeter.IsWriteProtected)
        stringBuilder.AppendLine("**** Write protected ****");
      if (this.MyMeter.MeterInfoDescription != null)
        stringBuilder.AppendLine("Type description: " + this.MyMeter.MeterInfoDescription);
      if (this.FullSerialNumber != null)
        stringBuilder.AppendLine("SerialNumber: " + this.FullSerialNumber);
      else
        stringBuilder.AppendLine("SerialNumber: " + this.SerialNumber.ToString("x08"));
      stringBuilder.AppendLine("Firmware: " + this.FirmwareVersionString);
      stringBuilder.AppendLine("ApprovalRevison: " + this.ApprovalRevison.ToString());
      stringBuilder.AppendLine("Hardware: " + ParameterService.GetHardwareString(this.HardwareMask));
      stringBuilder.AppendLine("SAP_MaterialNumber: " + this.SAP_MaterialNumber.ToString());
      stringBuilder.AppendLine("SAP_ProductionOrderNumber: " + this.SAP_ProductionOrderNumber);
      stringBuilder.AppendLine("MeterTypeId: " + this.MeterTypeId.ToString());
      stringBuilder.AppendLine("BaseTypeId: " + this.BaseTypeId.ToString());
      return stringBuilder.ToString();
    }

    internal ParameterService.HardwareMaskElements GetVolumeMeterType()
    {
      return ParameterService.GetVolumeHardware(this.HardwareMask);
    }

    internal bool CorrectInputIds(S3_DeviceId DeviceId)
    {
      try
      {
        if ((int) this.FirmwareVersion != (int) this.FirmwareVersion || this.FirmwareVersionString != this.FirmwareVersionString || (int) this.HardwareMask != (int) this.HardwareMask)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal deviceId");
          return false;
        }
        this.SetInputIDsDepentOnConfiguration(uint.Parse(DeviceId.FullSerialNumber.Substring(6), NumberStyles.HexNumber));
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_Meter.S3_MeterLogger);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal serial number", S3_Meter.S3_MeterLogger);
        return false;
      }
    }

    internal bool SetDeviceId(S3_DeviceId newDeviceId)
    {
      try
      {
        if ((int) this.FirmwareVersion != (int) newDeviceId.FirmwareVersion || this.FirmwareVersionString != newDeviceId.FirmwareVersionString || (int) this.HardwareMask != (int) newDeviceId.HardwareMask)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal deviceId");
          return false;
        }
        if (newDeviceId.FullSerialNumber == null)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Serial number not available", S3_Meter.S3_MeterLogger);
        if (newDeviceId.FullSerialNumber.Length != 14)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal serial number length!", S3_Meter.S3_MeterLogger);
        uint num1 = uint.Parse(newDeviceId.FullSerialNumber.Substring(6), NumberStyles.HexNumber);
        byte num2 = byte.Parse(newDeviceId.FullSerialNumber.Substring(4, 2), NumberStyles.HexNumber);
        ushort manufacturerCode = MBusDevice.GetManufacturerCode(newDeviceId.FullSerialNumber.Substring(1, 3));
        ushort ushortValue = this.MyMeter.MyParameters.ParameterByName["Con_Medium_Generation"].GetUshortValue();
        if (newDeviceId.FullSerialNumber[0] != '6')
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal serial number: Medium error!", S3_Meter.S3_MeterLogger);
        ushort NewValue = (ushort) (((uint) ushortValue & 65280U) + (uint) num2);
        this.MyMeter.MyParameters.ParameterByName["Con_Medium_Generation"].SetUshortValue(NewValue);
        this.MyMeter.MyParameters.ParameterByName["SerDev0_Medium_Generation"].SetUshortValue(NewValue);
        if (newDeviceId.IsInput1Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev1_Medium_Generation"].SetUshortValue((ushort) (((uint) this.MyMeter.MyParameters.ParameterByName["SerDev1_Medium_Generation"].GetUshortValue() & 65280U) + (uint) num2));
        if (newDeviceId.IsInput2Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev2_Medium_Generation"].SetUshortValue((ushort) (((uint) this.MyMeter.MyParameters.ParameterByName["SerDev2_Medium_Generation"].GetUshortValue() & 65280U) + (uint) num2));
        if (newDeviceId.IsInput3Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev3_Medium_Generation"].SetUshortValue((ushort) (((uint) this.MyMeter.MyParameters.ParameterByName["SerDev3_Medium_Generation"].GetUshortValue() & 65280U) + (uint) num2));
        this.MyMeter.MyParameters.ParameterByName["Con_Manufacturer"].SetUshortValue(manufacturerCode);
        this.MyMeter.MyParameters.ParameterByName["SerDev0_Manufacturer"].SetUshortValue(manufacturerCode);
        if (newDeviceId.IsInput1Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev1_Manufacturer"].SetUshortValue(manufacturerCode);
        if (newDeviceId.IsInput2Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev2_Manufacturer"].SetUshortValue(manufacturerCode);
        if (newDeviceId.IsInput3Available)
          this.MyMeter.MyParameters.ParameterByName["SerDev3_Manufacturer"].SetUshortValue(manufacturerCode);
        this.MyMeter.MyParameters.ParameterByName["Con_MeterId"].SetUintValue(newDeviceId.MeterId);
        this.MyMeter.MyParameters.ParameterByName["Con_MeterInfoId"].SetUintValue(newDeviceId.MeterInfoId);
        this.MyMeter.MyParameters.ParameterByName["Con_BaseTypeId"].SetUintValue(newDeviceId.BaseTypeId);
        this.MyMeter.MyParameters.ParameterByName["Con_MeterTypeId"].SetUintValue(newDeviceId.MeterTypeId);
        this.MyMeter.MyParameters.ParameterByName["Con_SAP_MaterialNumber"].SetUintValue(newDeviceId.SAP_MaterialNumber);
        if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()))
        {
          uint result;
          if (uint.TryParse(newDeviceId.SAP_ProductionOrderNumber, out result) && result > 0U)
          {
            this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].SetUintValue(result);
          }
          else
          {
            this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].SetUintValue(0U);
            this.SAP_ProductionOrderNumber = newDeviceId.SAP_ProductionOrderNumber;
          }
        }
        else
        {
          byte[] terminatedAsciiString = Utility.StringToZeroTerminatedAsciiString(newDeviceId.SAP_ProductionOrderNumber, 20);
          this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_ProductionOrderNumber.ToString()].SetByteArray(terminatedAsciiString);
        }
        this.MyMeter.MyParameters.ParameterByName["Con_SerialNumber"].SetUintValue(num1);
        this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].SetUintValue(num1);
        this.SetRadioIDsDepentOnConfiguration(num1);
        this.LoadDeviceIdFromParameter();
        this.SetInputIDsDepentOnConfiguration(num1);
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), S3_Meter.S3_MeterLogger);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal serial number", S3_Meter.S3_MeterLogger);
        return false;
      }
    }

    private bool CheckRadio3IDs(uint RadioID)
    {
      RadioTransmitter radio = this.MyMeter.MyTransmitParameterManager.Transmitter.Radio;
      if (radio.childMemoryBlocks == null)
        return true;
      foreach (S3_MemoryBlock childMemoryBlock in radio.childMemoryBlocks)
      {
        if (childMemoryBlock is RadioListHeader)
        {
          RadioListHeader radioListHeader = childMemoryBlock as RadioListHeader;
          if ((radioListHeader.Mode == RADIO_MODE.Radio3_Sz0 || radioListHeader.Mode == RADIO_MODE.Radio3 || radioListHeader.Mode == RADIO_MODE.Radio3_Sz5) && ((int) RadioID & -16777216) != 1929379840)
            return false;
        }
      }
      return true;
    }

    internal bool GarantInputIdentity(int inputIndex)
    {
      if (!this.MyMeter.IsInputAvailable(inputIndex))
        return true;
      uint uintValue1 = this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].GetUintValue();
      ushort ushortValue1 = this.MyMeter.MyParameters.ParameterByName["SerDev0_Manufacturer"].GetUshortValue();
      ushort ushortValue2 = this.MyMeter.MyParameters.ParameterByName["SerDev0_Medium_Generation"].GetUshortValue();
      ushort NewValue = (ushort) ((uint) this.MyMeter.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"].GetUshortValue() & (uint) byte.MaxValue);
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]];
      S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusManufacturer[inputIndex]];
      S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusMediumGeneration[inputIndex]];
      S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]];
      uint uintValue2 = s3Parameter1.GetUintValue();
      if (s3Parameter4.GetByteValue() == (byte) 254 && (int) uintValue2 == (int) S3_DeviceIdentification.virtualDeviceOffSerialNumber[inputIndex])
        return true;
      if (this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[inputIndex]].GetUshortValue() == (ushort) 0)
      {
        if (!s3Parameter1.SetUintValue(uintValue1) || !s3Parameter2.SetUshortValue(ushortValue1) || !s3Parameter3.SetUshortValue(ushortValue2) || !s3Parameter4.SetUshortValue(NewValue))
          return false;
      }
      else
      {
        string str = "Input" + (inputIndex + 1).ToString();
        uint uintValue3 = s3Parameter1.GetUintValue();
        if ((int) uintValue3 == (int) uintValue1)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str + " serial number is identical to heat meter serial number", S3_Meter.S3_MeterLogger);
        byte ushortValue3 = (byte) s3Parameter4.GetUshortValue();
        if ((int) ushortValue3 == (int) NewValue)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str + " mbus address is identical to heat meter mbus address", S3_Meter.S3_MeterLogger);
        for (int inputIndex1 = 0; inputIndex1 < 3; ++inputIndex1)
        {
          if (inputIndex1 != inputIndex && this.MyMeter.IsInputAvailable(inputIndex1) && this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputFactor[inputIndex1]].GetUshortValue() != (ushort) 0)
          {
            uint uintValue4 = this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex1]].GetUintValue();
            if ((int) uintValue3 == (int) uintValue4)
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str + " serial number is identical to input" + (inputIndex1 + 1).ToString() + " serial number", S3_Meter.S3_MeterLogger);
            byte ushortValue4 = (byte) this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex1]].GetUshortValue();
            if ((int) ushortValue3 == (int) ushortValue4)
              return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str + " mbus address is identical to input" + (inputIndex1 + 1).ToString() + " mbus address", S3_Meter.S3_MeterLogger);
          }
        }
      }
      return true;
    }

    internal bool ResetInputIdentity(int inputIndex)
    {
      uint uintValue = this.MyMeter.MyParameters.ParameterByName["SerDev0_IdentNo"].GetUintValue();
      uint NewValue1 = (uint) ((ulong) (uintValue & 16777215U) + (ulong) (16777216 * (inputIndex + 1)));
      for (int index = 0; index < 3; ++index)
      {
        if (index != inputIndex)
        {
          if ((int) this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[index]].GetUintValue() == (int) NewValue1)
          {
            ++NewValue1;
            index = -1;
          }
          else if ((int) NewValue1 == (int) uintValue)
          {
            ++NewValue1;
            index = -1;
          }
        }
      }
      if (!this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusIdentNumber[inputIndex]].SetUintValue(NewValue1))
        return false;
      ushort ushortValue1 = this.MyMeter.MyParameters.ParameterByName["SerDev0_Manufacturer"].GetUshortValue();
      if (!this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusManufacturer[inputIndex]].SetUshortValue(ushortValue1))
        return false;
      ushort ushortValue2 = this.MyMeter.MyParameters.ParameterByName["SerDev0_Medium_Generation"].GetUshortValue();
      ushort NewValue2 = (ushort) ((uint) S3_DeviceIdentification.InputDefaultMediumAndGeneration[inputIndex] | (uint) ushortValue2 & (uint) byte.MaxValue);
      if (!this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusMediumGeneration[inputIndex]].SetUshortValue(NewValue2))
        return false;
      ushort num = (ushort) ((uint) this.MyMeter.MyParameters.ParameterByName["SerDev0_SelectedList_PrimaryAddress"].GetUshortValue() & (uint) byte.MaxValue);
      ushort NewValue3 = (ushort) ((int) num + 1 + inputIndex);
      for (int index = 0; index < 3; ++index)
      {
        if (index != inputIndex)
        {
          if ((int) this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[index]].GetUshortValue() == (int) NewValue3)
          {
            ++NewValue3;
            index = -1;
          }
          else if ((int) NewValue3 == (int) num)
          {
            ++NewValue3;
            index = -1;
          }
          else if (NewValue3 > (ushort) 250)
          {
            NewValue3 = (ushort) 0;
            index = -1;
          }
        }
      }
      return this.MyMeter.MyParameters.ParameterByName[S3_Meter.inputMBusSelectedListAndAddress[inputIndex]].SetUshortValue(NewValue3);
    }

    internal void UpdateRadioIDsByResources()
    {
      if (this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.Radio.ToString()))
      {
        if (!this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.Cooling_Active.ToString()))
          this.MyMeter.MyIdentification.SerDev0_RadioId_Cool = 0U;
        if (this.MyMeter.MyIdentification.IsInput1Available && !this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.IO_1_Input.ToString()))
          this.MyMeter.MyIdentification.SerDev1_RadioId = 0U;
        if (this.MyMeter.MyIdentification.IsInput2Available && !this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.IO_2_Input.ToString()))
          this.MyMeter.MyIdentification.SerDev2_RadioId = 0U;
        if (!this.MyMeter.MyIdentification.IsInput3Available || this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.IO_2_Input.ToString()))
          return;
        this.MyMeter.MyIdentification.SerDev3_RadioId = 0U;
      }
      else
      {
        this.MyMeter.MyIdentification.SerDev0_RadioId_Vol = 0U;
        this.MyMeter.MyIdentification.SerDev0_RadioId_Heat = 0U;
        this.MyMeter.MyIdentification.SerDev0_RadioId_Cool = 0U;
        if (this.MyMeter.MyIdentification.IsInput1Available)
          this.MyMeter.MyIdentification.SerDev1_RadioId = 0U;
        if (this.MyMeter.MyIdentification.IsInput2Available)
          this.MyMeter.MyIdentification.SerDev2_RadioId = 0U;
        if (this.MyMeter.MyIdentification.IsInput3Available)
          this.MyMeter.MyIdentification.SerDev3_RadioId = 0U;
      }
    }

    internal void SetRadioIDsDepentOnConfiguration(uint serialNumberHeatMeter)
    {
      uint num = (uint) (((int) (serialNumberHeatMeter << 4) & 16777215) + 1929379840);
      this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_RadioId.ToString()].SetUintValue(num);
      if (this.IsInput1Available)
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev1_RadioId.ToString()].SetUintValue(num + 3U);
      if (this.IsInput2Available)
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev2_RadioId.ToString()].SetUintValue(num + 4U);
      if (!this.IsWR4 && this.IsInput3Available)
        this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev3_RadioId.ToString()].SetUintValue(num + 5U);
      if (this.MyMeter.MyResources.IsResourceAvailable(S3_MeterResources.Radio.ToString()) && !this.CheckRadio3IDs(num))
        throw new Exception("Radio3_IDs not valid!!!");
    }

    internal void SetInputIDsDepentOnConfiguration(uint serialNumberHeatMeter)
    {
      bool virtualDevicesUsed = this.MyMeter.MyTransmitParameterManager.AreVirtualDevicesUsed();
      if (virtualDevicesUsed)
      {
        serialNumberHeatMeter &= 16777215U;
        serialNumberHeatMeter += 16777216U;
        this.MyMeter.MyParameters.ParameterByName["SerDev1_IdentNo"].SetUintValue(serialNumberHeatMeter);
        serialNumberHeatMeter += 16777216U;
        this.MyMeter.MyParameters.ParameterByName["SerDev2_IdentNo"].SetUintValue(serialNumberHeatMeter);
        if (!this.MyMeter.MyIdentification.IsWR4)
        {
          serialNumberHeatMeter += 16777216U;
          this.MyMeter.MyParameters.ParameterByName["SerDev3_IdentNo"].SetUintValue(serialNumberHeatMeter);
        }
      }
      this.MyMeter.MyMeterScaling.GarantInputBaseSettings(virtualDevicesUsed);
    }
  }
}
