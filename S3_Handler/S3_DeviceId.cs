// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DeviceId
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class S3_DeviceId
  {
    public S3_DeviceId.IdentificationCheckStates IdentificationCheckState = S3_DeviceId.IdentificationCheckStates.NotChecked;
    public uint FirmwareVersion;
    public string FirmwareVersionString;
    public uint HardwareMask;
    public uint HardwareTypeId;
    public uint MeterId;
    public uint MeterInfoId;
    public uint BaseTypeId;
    public uint MeterTypeId;
    public uint MapId;
    public uint SAP_MaterialNumber;
    public string SAP_ProductionOrderNumber;
    public uint SerialNumber;
    public string FullSerialNumber;
    public byte MBusMedium;
    public byte ApprovalRevison;
    public ushort Signature;
    public uint BuildRevision;
    public uint SerDev0_RadioId_Vol;
    public uint SerDev0_RadioId_Heat;
    public uint SerDev0_RadioId_Cool;
    public uint SerDev1_RadioId;
    public uint SerDev2_RadioId;
    public uint SerDev3_RadioId;
    public SortedList<string, int> HardwareResources;
    public string HardwareOptions;
    public string TypeCreationString;
    public uint VolumeMeterIdentification;

    public bool IdentificationCheckedAndOk
    {
      get => this.IdentificationCheckState == S3_DeviceId.IdentificationCheckStates.ok;
    }

    public bool IsWR4 => (this.HardwareMask & 512U) > 0U;

    public bool IsUltrasonic => !this.IsWR4 && (this.HardwareMask & 192U) > 0U;

    public bool IsUltrasonicDirect => !this.IsWR4 && (this.HardwareMask & 128U) > 0U;

    public int PT_Type
    {
      get
      {
        if (!this.IsWR4)
          return 1000;
        switch (this.HardwareMask & 240U)
        {
          case 16:
            return 100;
          case 32:
            return 500;
          case 64:
            return 1000;
          default:
            return -1;
        }
      }
    }

    public bool IsLoRa => (this.HardwareMask & 8U) > 0U;

    public bool IsInput1Available
    {
      get
      {
        if (this.IsWR4)
          return !this.IsLoRa;
        return this.FirmwareVersion < 117510179U;
      }
    }

    public bool IsInput2Available
    {
      get
      {
        if (this.IsWR4)
          return !this.IsLoRa;
        return this.FirmwareVersion < 117510179U;
      }
    }

    public bool IsInput3Available => !this.IsWR4 && this.FirmwareVersion < 117510179U;

    internal S3_DeviceId Clone()
    {
      S3_DeviceId TheClone = new S3_DeviceId();
      this.Write_S3_DeviceId_CloneData(TheClone);
      return TheClone;
    }

    internal void Write_S3_DeviceId_CloneData(S3_DeviceId TheClone)
    {
      TheClone.IdentificationCheckState = this.IdentificationCheckState;
      TheClone.FirmwareVersion = this.FirmwareVersion;
      TheClone.FirmwareVersionString = this.FirmwareVersionString;
      TheClone.HardwareMask = this.HardwareMask;
      TheClone.HardwareTypeId = this.HardwareTypeId;
      TheClone.MeterId = this.MeterId;
      TheClone.MeterInfoId = this.MeterInfoId;
      TheClone.MeterTypeId = this.MeterTypeId;
      TheClone.BaseTypeId = this.BaseTypeId;
      TheClone.MapId = this.MapId;
      TheClone.SAP_MaterialNumber = this.SAP_MaterialNumber;
      TheClone.SAP_ProductionOrderNumber = this.SAP_ProductionOrderNumber;
      TheClone.SerialNumber = this.SerialNumber;
      TheClone.FullSerialNumber = this.FullSerialNumber;
      TheClone.MBusMedium = this.MBusMedium;
      TheClone.ApprovalRevison = this.ApprovalRevison;
      TheClone.HardwareResources = this.HardwareResources;
      TheClone.HardwareOptions = this.HardwareOptions;
      TheClone.Signature = this.Signature;
      TheClone.BuildRevision = this.BuildRevision;
      TheClone.TypeCreationString = this.TypeCreationString;
      TheClone.VolumeMeterIdentification = this.VolumeMeterIdentification;
      TheClone.SerDev0_RadioId_Vol = this.SerDev0_RadioId_Vol;
      TheClone.SerDev0_RadioId_Heat = this.SerDev0_RadioId_Heat;
      TheClone.SerDev0_RadioId_Cool = this.SerDev0_RadioId_Cool;
      TheClone.SerDev1_RadioId = this.SerDev1_RadioId;
      TheClone.SerDev2_RadioId = this.SerDev2_RadioId;
      TheClone.SerDev3_RadioId = this.SerDev3_RadioId;
    }

    internal void PrintId(StringBuilder printText)
    {
      printText.AppendLine("Hardware: ......... " + ParameterService.GetHardwareString(this.HardwareMask));
      printText.AppendLine("FirmwareVersion: .. " + this.FirmwareVersionString);
      printText.AppendLine("ApprovalRevison: .. " + this.ApprovalRevison.ToString());
      printText.AppendLine("SerialNumber: ..... " + this.FullSerialNumber);
      printText.AppendLine("SAP_Number: ....... " + this.SAP_MaterialNumber.ToString());
      printText.AppendLine("MeterId: .......... " + this.MeterId.ToString());
      printText.AppendLine("MeterInfoId: ...... " + this.MeterInfoId.ToString());
      printText.AppendLine("BaseTypeId: ....... " + this.BaseTypeId.ToString());
      printText.AppendLine("TypeCreationString: " + this.TypeCreationString);
    }

    internal void PrintNotStandardId(StringBuilder printText)
    {
      printText.AppendLine("ApprovalRevison: .. " + this.ApprovalRevison.ToString());
      printText.AppendLine("TypeCreationString: " + this.TypeCreationString);
    }

    internal string GetInputString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.IsInput1Available)
        stringBuilder.Append("1");
      if (this.IsInput2Available)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(";");
        stringBuilder.Append("2");
      }
      if (this.IsInput3Available)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(";");
        stringBuilder.Append("3");
      }
      if (stringBuilder.Length == 0)
        stringBuilder.Append("-");
      return stringBuilder.ToString();
    }

    public enum IdentificationCheckStates
    {
      NotChecked,
      ok,
      ChecksumError,
    }
  }
}
