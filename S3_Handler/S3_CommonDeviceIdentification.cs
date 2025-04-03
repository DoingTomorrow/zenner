// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_CommonDeviceIdentification
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_CommonDeviceIdentification : DeviceIdentification
  {
    private S3_Meter TheMeter;

    internal S3_CommonDeviceIdentification(S3_Meter theMeter)
    {
      this.TheMeter = theMeter;
      S3_DeviceIdentification identification = theMeter.MyIdentification;
      this.firmwareVersion = new uint?(identification.FirmwareVersion);
      if (!string.IsNullOrEmpty(identification.FullSerialNumber))
        this.obisMedium = new char?(identification.FullSerialNumber[0]);
      this.signatur = new ushort?(identification.Signature);
      this.svnRevision = new uint?(identification.BuildRevision);
    }

    public override uint? HardwareID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_HardwareAndRestrictions.ToString()].GetUintValue() & 4095U);
      }
      set => throw new Exception("Write to HardwareID not defined");
    }

    public override uint? MeterID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_MeterId.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterId.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterId.ToString()].SetUintValue(value.Value);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override uint? ID_BCD
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_IdentNo.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_IdentNo.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_IdentNo.ToString()].SetUintValue(value.Value);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override uint? FD_ID_BCD
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SerialNumber.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SerialNumber.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SerialNumber.ToString()].SetUintValue(value.Value);
      }
    }

    public override ushort? Manufacturer
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_Manufacturer.ToString()) ? new ushort?() : new ushort?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Manufacturer.ToString()].GetUshortValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Manufacturer.ToString()].SetUshortValue(value.Value);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override ushort? FD_Manufacturer
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_Manufacturer.ToString()) ? new ushort?() : new ushort?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Manufacturer.ToString()].GetUshortValue());
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_Manufacturer.ToString()))
          return;
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Manufacturer.ToString()].SetUshortValue(value.Value);
      }
    }

    public override byte? Medium
    {
      get
      {
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_Medium_Generation.ToString()) ? new byte?((byte) ((uint) this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()].GetUshortValue() >> 8)) : new byte?(this.TheMeter.MyIdentification.MBusMedium);
      }
      set
      {
        ushort NewValue = (ushort) ((int) this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()].GetUshortValue() & (int) byte.MaxValue | (int) value.Value << 8);
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()].SetUshortValue(NewValue);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override byte? FD_Medium
    {
      get
      {
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_Medium_Generation.ToString()) ? new byte?((byte) ((uint) this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].GetUshortValue() >> 8)) : new byte?();
      }
      set
      {
        ushort NewValue = (ushort) ((int) this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].GetUshortValue() & (int) byte.MaxValue | (int) value.Value << 8);
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].SetUshortValue(NewValue);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override byte? Generation
    {
      get
      {
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_Medium_Generation.ToString()) ? new byte?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()].GetByteValue()) : new byte?();
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()].SetByteValue(value.Value);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override byte? FD_Generation
    {
      get
      {
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_Medium_Generation.ToString()) ? new byte?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].GetByteValue()) : new byte?();
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].SetByteValue(value.Value);
        this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      }
    }

    public override char? ObisMedium
    {
      get
      {
        if (this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.SerDev0_Medium_Generation.ToString()))
        {
          S3_Parameter s3Parameter = this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.SerDev0_Medium_Generation.ToString()];
          if (s3Parameter.IsCacheInitialised())
            return new char?(S3_DeviceIdentification.GetObisMediumFromMBusMediumGeneration(s3Parameter.GetUshortValue()));
        }
        return !string.IsNullOrEmpty(this.TheMeter.MyIdentification.FullSerialNumber) ? new char?(this.TheMeter.MyIdentification.FullSerialNumber[0]) : new char?('?');
      }
      set
      {
      }
    }

    public override char? FD_ObisMedium
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_Obis_Medium.ToString()) ? new char?(S3_DeviceIdentification.GetObisMediumFromMBusMediumGeneration(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_Medium_Generation.ToString()].GetUshortValue())) : new char?((char) this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_Obis_Medium.ToString()].GetByteValue());
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_Obis_Medium.ToString()))
          return;
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_Obis_Medium.ToString()].SetByteValue((byte) value.Value);
      }
    }

    public override byte[] AES_Key
    {
      get => this.TheMeter.GetEncryptionKey();
      set => this.TheMeter.SetEncryptionKey(value);
    }

    public override uint? HardwareTypeID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_HardwareTypeId.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_HardwareTypeId.ToString()].SetUintValue(value.Value);
      }
    }

    public override uint? MeterInfoID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_MeterInfoId.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterInfoId.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterInfoId.ToString()].SetUintValue(value.Value);
      }
    }

    public override uint? MeterTypeID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_MeterTypeId.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterTypeId.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_MeterTypeId.ToString()].SetUintValue(value.Value);
      }
    }

    public override uint? BaseTypeID
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_BaseTypeId.ToString()) ? new uint?() : new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_BaseTypeId.ToString()].GetUintValue());
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_BaseTypeId.ToString()].SetUintValue(value.Value);
      }
    }

    public override uint? SAP_MaterialNumber
    {
      get
      {
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SAP_MaterialNumber.ToString()) ? new uint?(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_MaterialNumber.ToString()].GetUintValue()) : new uint?();
      }
      set
      {
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_MaterialNumber.ToString()].SetUintValue(value.Value);
      }
    }

    public override string SAP_ProductionOrderNumber
    {
      get
      {
        if (this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()))
        {
          uint uintValue = this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].GetUintValue();
          return uintValue > 0U ? uintValue.ToString() : this.TheMeter.SavedOrderNumberString;
        }
        return this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_ProductionOrderNumber.ToString()) ? Utility.ZeroTerminatedAsciiStringToString(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_ProductionOrderNumber.ToString()].GetByteArray(20)) : (string) null;
      }
      set
      {
        if (this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()))
        {
          uint result;
          if (uint.TryParse(value, out result))
          {
            this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].SetUintValue(result);
          }
          else
          {
            this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_SAP_ProductionOrderNumber.ToString()].SetUintValue(0U);
            this.TheMeter.SavedOrderNumberString = value;
          }
        }
        else
        {
          if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.Con_ProductionOrderNumber.ToString()))
            throw new Exception("Firmware dosen't contains ProductionOrderNumber");
          byte[] terminatedAsciiString = Utility.StringToZeroTerminatedAsciiString(value, 20);
          this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_ProductionOrderNumber.ToString()].SetByteArray(terminatedAsciiString);
        }
      }
    }

    public override byte[] PrintedSerialNumber
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.PrintedSerialNumber.ToString()) ? Utility.StringToZeroTerminatedAsciiString(this.FD_FullSerialNumber, 15) : this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.PrintedSerialNumber.ToString()].GetByteArray(20);
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.PrintedSerialNumber.ToString()))
          return;
        if (value.Length > 19)
          throw new Exception("PrintedSerialNumber to long");
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.PrintedSerialNumber.ToString()].SetByteArray(value);
      }
    }

    public override ulong? LoRa_DevEUI
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfg_lora_deveui_0.ToString()) ? new ulong?() : new ulong?(this.TheMeter.MyDeviceMemory.GetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_lora_deveui_0.ToString()].BlockStartAddress, 8));
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfg_lora_deveui_0.ToString()))
          return;
        this.TheMeter.MyDeviceMemory.SetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_lora_deveui_0.ToString()].BlockStartAddress, 8, value.Value);
      }
    }

    public override ulong? FD_LoRa_DevEUI
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_LoRa_DevEUI.ToString()) ? new ulong?() : new ulong?(this.TheMeter.MyDeviceMemory.GetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_DevEUI.ToString()].BlockStartAddress, 8));
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfg_lora_deveui_0.ToString()))
          return;
        this.TheMeter.MyDeviceMemory.SetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_DevEUI.ToString()].BlockStartAddress, 8, value.Value);
      }
    }

    public override ulong? LoRa_JoinEUI
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfg_lora_appeui_0.ToString()) ? new ulong?() : new ulong?(this.TheMeter.MyDeviceMemory.GetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_lora_appeui_0.ToString()].BlockStartAddress, 8));
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfg_lora_appeui_0.ToString()))
          return;
        this.TheMeter.MyDeviceMemory.SetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfg_lora_appeui_0.ToString()].BlockStartAddress, 8, value.Value);
      }
    }

    public override ulong? FD_LoRa_JoinEUI
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_LoRa_AppEUI.ToString()) ? new ulong?() : new ulong?(this.TheMeter.MyDeviceMemory.GetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_AppEUI.ToString()].BlockStartAddress, 8));
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_LoRa_AppEUI.ToString()))
          return;
        this.TheMeter.MyDeviceMemory.SetUlongValue(this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_AppEUI.ToString()].BlockStartAddress, 8, value.Value);
      }
    }

    public override byte[] LoRa_AppKey
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfb_lora_AppKey_0.ToString()) ? (byte[]) null : this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfb_lora_AppKey_0.ToString()].GetByteArray(16);
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.cfb_lora_AppKey_0.ToString()))
          return;
        if (value.Length != 16)
          throw new ArgumentException("LoRa_AppKey length not 16");
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.cfb_lora_AppKey_0.ToString()].SetByteArray(value);
      }
    }

    public override byte[] FD_LoRa_AppKey
    {
      get
      {
        return !this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_LoRa_AppKey.ToString()) ? (byte[]) null : this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_AppKey.ToString()].GetByteArray(16);
      }
      set
      {
        if (!this.TheMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.FD_LoRa_AppKey.ToString()))
          return;
        if (value.Length != 16)
          throw new ArgumentException("FD_LoRa_AppKey length not 16");
        this.TheMeter.MyParameters.ParameterByName[S3_ParameterNames.FD_LoRa_AppKey.ToString()].SetByteArray(value);
      }
    }

    public string GetDeviceIdDescription()
    {
      S3_DeviceIdentification identification = this.TheMeter.MyIdentification;
      StringBuilder stringBuilder = new StringBuilder();
      if (this.TheMeter.IsWriteProtected)
        stringBuilder.Append("Protected; ");
      else
        stringBuilder.Append("!!! Not protected !!!; ");
      DateTime generatedTimeStamp = this.TheMeter.MeterObjectGeneratedTimeStamp;
      if (true)
        stringBuilder.AppendLine("Last changed time: " + this.TheMeter.MeterObjectGeneratedTimeStamp.ToString("dd.MM.yyyy HH.mm.ss"));
      stringBuilder.Append("SerialNumber: ");
      if (this.FullSerialNumber != null)
        stringBuilder.Append(this.FullSerialNumber);
      stringBuilder.Append(" ;  FD: ");
      if (this.FD_FullSerialNumber != null)
        stringBuilder.Append(this.FD_FullSerialNumber);
      stringBuilder.Append(" ;  Printed: ");
      if (this.PrintedSerialNumberAsString != null)
        stringBuilder.Append(this.PrintedSerialNumberAsString);
      string mediumAsText = this.GetMediumAsText();
      if (!string.IsNullOrEmpty(mediumAsText))
        stringBuilder.Append(" ; Medium:" + mediumAsText);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Identification checksum: " + this.TheMeter.MyIdentification.IdentificationCheckState.ToString());
      if (this.TheMeter.TypeCreationString != null)
        stringBuilder.AppendLine("TypeCreationString: " + this.TheMeter.TypeCreationString);
      if (this.TheMeter.MeterInfoDescription != null)
        stringBuilder.AppendLine("MeterInfo description: " + this.TheMeter.MeterInfoDescription);
      if (this.TheMeter.MeterTypeDescription != null)
        stringBuilder.AppendLine("MeterType description: " + this.TheMeter.MeterTypeDescription);
      stringBuilder.Append("Firmware: " + this.GetFirmwareVersionString());
      if (identification.Signature > (ushort) 0)
        stringBuilder.Append(" ; Signature: 0x" + identification.Signature.ToString("x04"));
      if (identification.BuildRevision > 0U)
        stringBuilder.Append(" ; BuildRevision: " + identification.BuildRevision.ToString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("ApprovalRevison: " + identification.ApprovalRevison.ToString());
      stringBuilder.AppendLine("Hardware: (0x" + identification.HardwareMask.ToString("x04") + ") " + ParameterService.GetHardwareString(identification.HardwareMask));
      string inputString = identification.GetInputString();
      if (inputString != null && inputString != "-")
        stringBuilder.AppendLine("Inputs: " + identification.GetInputString());
      if (this.SAP_MaterialNumber.HasValue)
        stringBuilder.Append("SAP_MaterialNumber: " + this.SAP_MaterialNumber.ToString());
      if (this.SAP_ProductionOrderNumber != null)
        stringBuilder.AppendLine(" ;  SAP_ProductionOrderNumber: " + this.SAP_ProductionOrderNumber);
      if (this.MeterID.HasValue)
        stringBuilder.Append("MeterId: " + this.MeterID.ToString());
      if (this.MeterInfoID.HasValue)
        stringBuilder.AppendLine(" ;  MeterInfoId: " + this.MeterInfoID.ToString());
      if (this.HardwareTypeID.HasValue)
        stringBuilder.Append("HardwareTypeId: " + this.HardwareTypeID.ToString());
      if (this.MeterTypeID.HasValue)
        stringBuilder.Append(" ;  MeterTypeId: " + this.MeterTypeID.ToString());
      if (this.BaseTypeID.HasValue)
        stringBuilder.AppendLine(" ;  BaseTypeId: " + this.BaseTypeID.ToString());
      if (identification.VolumeMeterIdentification > 0U)
        stringBuilder.AppendLine("VolumeMeterId: " + identification.VolumeMeterIdentification.ToString("x04"));
      return stringBuilder.ToString();
    }

    public override void Set_FD_Values()
    {
      base.Set_FD_Values();
      this.TheMeter.MyIdentification.LoadHardwareIdFromParameter();
      this.TheMeter.MyIdentification.SetRadioIDsDepentOnConfiguration(this.ID_BCD.Value);
      this.TheMeter.MyIdentification.SetInputIDsDepentOnConfiguration(this.ID_BCD.Value);
    }
  }
}
