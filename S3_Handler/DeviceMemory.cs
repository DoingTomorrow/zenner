// Decompiled with JetBrains decompiler
// Type: S3_Handler.DeviceMemory
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class DeviceMemory
  {
    internal static Logger S3_DeviceMemoryLogger = LogManager.GetLogger("S3_DeviceMemory");
    private const byte PackedFormatBasic = 1;
    private const byte PackedFormatVersion = 2;
    private S3_Meter MyMeter;
    internal List<DeviceMemory.ControllerMemoryInfo> ControllerSegmentsByAddress;
    internal SortedList<DeviceMemory.ControllerMemoryTypes, DeviceMemory.ControllerMemoryInfo> ControllerSegmentsByType;
    internal S3_MemoryBlock meterMemory;
    internal S3_MemoryBlock BlockInfoMemory;
    internal S3_MemoryBlock BlockBackupStorage0;
    internal S3_MemoryBlock BlockBackup0;
    internal S3_MemoryBlock BlockBackupRuntimeVars0;
    internal S3_MemoryBlock BlockBackupStorage1;
    internal S3_MemoryBlock BlockBackup1;
    internal S3_MemoryBlock BlockBackupRuntimeVars1;
    internal S3_MemoryBlock BlockRAM;
    internal S3_MemoryBlock BlockLinkedVars;
    internal S3_MemoryBlock BlockBackupInRAM;
    internal S3_MemoryBlock BlockBackupRuntimeVars;
    internal S3_MemoryBlock BlockConfiguratorHeap;
    internal S3_MemoryBlock BlockLoggerRamData;
    internal S3_MemoryBlock BlockHeap;
    internal S3_MemoryBlock BlockFlashBlock0;
    internal S3_MemoryBlock BlockProtectedConfigValues;
    internal WriteProtTable BlockWriteProtTable;
    internal S3_MemoryBlock BlockProtectedDisplayCode;
    internal S3_MemoryBlock BlockFlashBlock1;
    internal S3_MemoryBlock BlockConfigValues;
    internal S3_MemoryBlock BlockFunctionTable;
    internal S3_MemoryBlock BlockDisplayCode;
    internal S3_MemoryBlock BlockResetRuntimeCode;
    internal S3_MemoryBlock BlockMesurementRuntimeCode;
    internal S3_MemoryBlock BlockCycleRuntimeCode;
    internal S3_MemoryBlock BlockMBusRuntimeCode;
    internal S3_MemoryBlock BlockRuntimeConstants;
    internal HandlerInfo BlockHandlerInfo;
    internal S3_MemoryBlock BlockFlashBlock2;
    internal S3_MemoryBlock BlockLoggerTable;
    internal S3_MemoryBlock BlockMBusTable;
    internal S3_MemoryBlock BlockFlashBlock3;
    internal S3_MemoryBlock BlockLoggerData;
    internal int flashStartAddress = 16384;
    internal int flashNextReadAddress;
    internal byte[] MemoryBytes;
    internal bool[] ByteIsDefined;
    internal int minDefinedAddress;
    internal int minUnDefinedAddress;
    private int WriteBlockStartAddress = -1;
    private int WriteBlockEndAddress = -1;
    private bool IsFlashBlock = false;
    private int ramWriteStartAddress = -1;
    private ByteField WriteBuffer = (ByteField) null;

    private DeviceMemory(S3_Meter TheMeter) => this.MyMeter = TheMeter;

    internal DeviceMemory(S3_Meter TheMeter, int DeviceMemorySize)
    {
      this.MyMeter = TheMeter;
      this.DeviceMemorySize = DeviceMemorySize;
      this.BaseConstructor();
    }

    internal DeviceMemory(S3_Meter TheMeter, byte[] PackedByteList)
    {
      uint num1 = 0;
      this.MyMeter = TheMeter;
      int num2 = 0;
      byte[] numArray1 = PackedByteList;
      int index1 = num2;
      int num3 = index1 + 1;
      byte num4 = numArray1[index1];
      switch (num4)
      {
        case 1:
          byte[] numArray2 = PackedByteList;
          int index2 = num3;
          int num5 = index2 + 1;
          int num6 = (int) numArray2[index2];
          byte[] numArray3 = PackedByteList;
          int index3 = num5;
          int num7 = index3 + 1;
          int num8 = (int) numArray3[index3] << 8;
          int num9 = num6 + num8;
          byte[] numArray4 = PackedByteList;
          int index4 = num7;
          int num10 = index4 + 1;
          int num11 = (int) numArray4[index4] << 16;
          int num12 = num9 + num11;
          this.DeviceMemorySize = num12;
          int index5 = 0;
          while (index5 < num12)
          {
            byte packedByte1 = PackedByteList[num10++];
            switch (packedByte1)
            {
              case 0:
                goto label_21;
              case 63:
                int packedByte2 = (int) PackedByteList[num10++];
                if (this.minDefinedAddress == this.minUnDefinedAddress)
                  this.minDefinedAddress = index5;
                while (packedByte2-- > 0)
                {
                  this.MemoryBytes[index5] = (byte) 0;
                  this.ByteIsDefined[index5++] = true;
                }
                this.minUnDefinedAddress = index5;
                continue;
              default:
                int num13 = (int) packedByte1 & 63;
                if (((int) packedByte1 & 128) == 128)
                  num13 = (num13 << 8) + (int) PackedByteList[num10++];
                if (((int) packedByte1 & 64) == 0)
                {
                  index5 += num13;
                  continue;
                }
                if (this.minDefinedAddress == this.minUnDefinedAddress)
                  this.minDefinedAddress = index5;
                while (num13-- > 0)
                {
                  this.MemoryBytes[index5] = PackedByteList[num10++];
                  this.ByteIsDefined[index5++] = true;
                }
                this.minUnDefinedAddress = index5;
                continue;
            }
          }
label_21:
          this.BaseConstructor();
          if (num4 != (byte) 2)
            break;
          TheMeter.MyIdentification = new S3_DeviceIdentification(this.MyMeter);
          TheMeter.MyIdentification.FirmwareVersion = num1;
          break;
        case 2:
          num1 = BitConverter.ToUInt32(PackedByteList, 1);
          num3 += 4;
          goto case 1;
        default:
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown packed memory format");
          goto case 1;
      }
    }

    private void BaseConstructor()
    {
      this.ControllerSegmentsByAddress = new List<DeviceMemory.ControllerMemoryInfo>();
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.Peripherals, DeviceMemory.PhysicalMemoryTypes.Register, 0, 0, 0));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.BootstrapLoader, DeviceMemory.PhysicalMemoryTypes.Flash, 4096, 512, 1));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.InfoMemory, DeviceMemory.PhysicalMemoryTypes.Flash, 6144, 128, 2));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.DeviceDecription, DeviceMemory.PhysicalMemoryTypes.ROM, 6656, 0, 0));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.RAM, DeviceMemory.PhysicalMemoryTypes.RAM, 7168, 0, 0));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.Unused, DeviceMemory.PhysicalMemoryTypes.None, 11264, 0, 0));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.MainFlash, DeviceMemory.PhysicalMemoryTypes.Flash, 16384, 512, 3));
      this.ControllerSegmentsByAddress.Add(new DeviceMemory.ControllerMemoryInfo(DeviceMemory.ControllerMemoryTypes.OutOfMemory, DeviceMemory.PhysicalMemoryTypes.None, 81920, 0, 0));
      for (int index = 0; index < this.ControllerSegmentsByAddress.Count - 1; ++index)
      {
        this.ControllerSegmentsByAddress[index].Size = this.ControllerSegmentsByAddress[index + 1].StartAddress - this.ControllerSegmentsByAddress[index].StartAddress;
        this.ControllerSegmentsByAddress[index].LastAddress = this.ControllerSegmentsByAddress[index + 1].StartAddress - 1;
      }
      this.ControllerSegmentsByAddress[this.ControllerSegmentsByAddress.Count - 1].Size = 0;
      this.ControllerSegmentsByAddress[this.ControllerSegmentsByAddress.Count - 1].LastAddress = this.ControllerSegmentsByAddress[this.ControllerSegmentsByAddress.Count - 1].StartAddress - 1;
      this.ControllerSegmentsByType = new SortedList<DeviceMemory.ControllerMemoryTypes, DeviceMemory.ControllerMemoryInfo>();
      foreach (DeviceMemory.ControllerMemoryInfo controllerMemoryInfo in this.ControllerSegmentsByAddress)
        this.ControllerSegmentsByType.Add(controllerMemoryInfo.ControllerMemoryType, controllerMemoryInfo);
      this.CreateSegments();
    }

    internal void CreateSegments()
    {
      this.meterMemory = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.CompleteMeter);
      this.meterMemory.IsHardLinkedAddress = true;
      this.BlockInfoMemory = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.InfoMemory, this.meterMemory);
      this.BlockBackupStorage0 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.BackupStorage0, this.BlockInfoMemory);
      this.BlockBackup0 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.Backup0, this.BlockBackupStorage0);
      this.BlockBackupRuntimeVars0 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.BackupRuntimeVars0, this.BlockBackupStorage0);
      this.BlockBackupStorage1 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.BackupStorage1, this.BlockInfoMemory);
      this.BlockBackup1 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.Backup1, this.BlockBackupStorage1);
      this.BlockBackupRuntimeVars1 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.BackupRuntimeVars1, this.BlockBackupStorage1);
      this.BlockRAM = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.RAM, this.meterMemory);
      this.BlockLinkedVars = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.LinkedVars, this.BlockRAM);
      this.BlockBackupInRAM = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.Backup, this.BlockRAM);
      this.BlockBackupRuntimeVars = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.BackupRuntimeVars, this.BlockRAM);
      this.BlockConfiguratorHeap = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.RuntimeVars, this.BlockRAM);
      this.BlockLoggerRamData = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.LoggerRamData, this.BlockConfiguratorHeap);
      this.BlockHeap = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.Heap, this.BlockConfiguratorHeap);
      this.BlockFlashBlock0 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.FlashBlock0, this.meterMemory);
      this.BlockFlashBlock0.Alignment = 512;
      this.BlockProtectedConfigValues = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.ProtectedConfigValues, this.BlockFlashBlock0);
      this.BlockWriteProtTable = new WriteProtTable(this.MyMeter, this.BlockFlashBlock0);
      this.BlockProtectedDisplayCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.ProtectedDisplayCode, this.BlockFlashBlock0);
      this.BlockFlashBlock1 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.FlashBlock1, this.meterMemory);
      this.BlockFlashBlock1.Alignment = 512;
      this.BlockConfigValues = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.ConfigValues, this.BlockFlashBlock1);
      this.BlockFunctionTable = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.FunctionTable, this.BlockFlashBlock1);
      this.BlockDisplayCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.DisplayCode, this.BlockFlashBlock1);
      this.BlockResetRuntimeCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.ResetRuntimeCode, this.BlockFlashBlock1);
      this.BlockResetRuntimeCode.Alignment = 1;
      this.BlockCycleRuntimeCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.CycleRuntimeCode, this.BlockFlashBlock1);
      this.BlockCycleRuntimeCode.Alignment = 1;
      this.BlockMesurementRuntimeCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.MesurementRuntimeCode, this.BlockFlashBlock1);
      this.BlockMesurementRuntimeCode.Alignment = 1;
      this.BlockMBusRuntimeCode = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.MBusRuntimeCode, this.BlockFlashBlock1);
      this.BlockMBusRuntimeCode.Alignment = 1;
      this.BlockRuntimeConstants = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.RuntimeConstants, this.BlockFlashBlock1);
      this.BlockHandlerInfo = new HandlerInfo(this.MyMeter, S3_MemorySegment.HandlerInfos, this.BlockFlashBlock1);
      this.BlockHandlerInfo.Alignment = 1;
      this.BlockFlashBlock2 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.FlashBlock2, this.meterMemory);
      this.BlockFlashBlock2.Alignment = 512;
      this.BlockLoggerTable = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.LoggerTable, this.BlockFlashBlock2);
      this.BlockMBusTable = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.TransmitParameterTable, this.BlockFlashBlock2);
      this.BlockFlashBlock3 = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.FlashBlock3, this.meterMemory);
      this.BlockFlashBlock3.Alignment = 512;
      this.BlockLoggerData = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.LoggerData, this.BlockFlashBlock3);
      this.BlockBackupInRAM.IsFixSize = true;
      this.BlockLinkedVars.IsFixSize = true;
      this.BlockBackupStorage1.IsFixSize = true;
      this.BlockBackupStorage0.IsFixSize = true;
      this.BlockRAM.IsFixSize = true;
      this.BlockProtectedConfigValues.IsFixSize = true;
      this.BlockConfigValues.IsFixSize = true;
    }

    internal void CreateClonedSegments(S3_Meter sourceMeter)
    {
      this.meterMemory = new S3_MemoryBlock(this.MyMeter, S3_MemorySegment.CompleteMeter);
      this.meterMemory.IsHardLinkedAddress = true;
      this.BlockInfoMemory = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockInfoMemory, true);
      this.BlockBackupStorage0 = new S3_MemoryBlock(this.MyMeter, this.BlockInfoMemory, sourceMeter.MyDeviceMemory.BlockBackupStorage0, true);
      this.BlockBackup0 = new S3_MemoryBlock(this.MyMeter, this.BlockBackupStorage0, sourceMeter.MyDeviceMemory.BlockBackup0, true);
      this.BlockBackupRuntimeVars0 = new S3_MemoryBlock(this.MyMeter, this.BlockBackupStorage0, sourceMeter.MyDeviceMemory.BlockBackupRuntimeVars0, true);
      this.BlockBackupStorage1 = new S3_MemoryBlock(this.MyMeter, this.BlockInfoMemory, sourceMeter.MyDeviceMemory.BlockBackupStorage1, true);
      this.BlockBackup1 = new S3_MemoryBlock(this.MyMeter, this.BlockBackupStorage1, sourceMeter.MyDeviceMemory.BlockBackup1, true);
      this.BlockBackupRuntimeVars1 = new S3_MemoryBlock(this.MyMeter, this.BlockBackupStorage1, sourceMeter.MyDeviceMemory.BlockBackupRuntimeVars1, true);
      this.BlockRAM = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockRAM, true);
      this.BlockLinkedVars = new S3_MemoryBlock(this.MyMeter, this.BlockRAM, sourceMeter.MyDeviceMemory.BlockLinkedVars, true);
      this.BlockBackupInRAM = new S3_MemoryBlock(this.MyMeter, this.BlockRAM, sourceMeter.MyDeviceMemory.BlockBackupInRAM, true);
      this.BlockBackupRuntimeVars = new S3_MemoryBlock(this.MyMeter, this.BlockRAM, sourceMeter.MyDeviceMemory.BlockBackupRuntimeVars, false);
      this.BlockConfiguratorHeap = new S3_MemoryBlock(this.MyMeter, this.BlockRAM, sourceMeter.MyDeviceMemory.BlockConfiguratorHeap, false);
      this.BlockLoggerRamData = new S3_MemoryBlock(this.MyMeter, this.BlockConfiguratorHeap, sourceMeter.MyDeviceMemory.BlockLoggerRamData, false);
      this.BlockHeap = new S3_MemoryBlock(this.MyMeter, this.BlockConfiguratorHeap, sourceMeter.MyDeviceMemory.BlockHeap, false);
      this.BlockFlashBlock0 = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockFlashBlock0, false);
      this.BlockProtectedConfigValues = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock0, sourceMeter.MyDeviceMemory.BlockProtectedConfigValues, true);
      this.BlockWriteProtTable = new WriteProtTable(this.MyMeter, this.BlockFlashBlock0, (S3_MemoryBlock) sourceMeter.MyDeviceMemory.BlockWriteProtTable);
      this.BlockProtectedDisplayCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock0, sourceMeter.MyDeviceMemory.BlockProtectedDisplayCode, true);
      this.BlockFlashBlock1 = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockFlashBlock1, false);
      this.BlockConfigValues = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockConfigValues, true);
      this.BlockFunctionTable = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockFunctionTable, true);
      this.BlockDisplayCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockDisplayCode, true);
      this.BlockResetRuntimeCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockResetRuntimeCode, false);
      this.BlockResetRuntimeCode.Alignment = 1;
      this.BlockCycleRuntimeCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockCycleRuntimeCode, false);
      this.BlockCycleRuntimeCode.Alignment = 1;
      this.BlockMesurementRuntimeCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockMesurementRuntimeCode, false);
      this.BlockMesurementRuntimeCode.Alignment = 1;
      this.BlockMBusRuntimeCode = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockMBusRuntimeCode, false);
      this.BlockMBusRuntimeCode.Alignment = 1;
      this.BlockRuntimeConstants = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock1, sourceMeter.MyDeviceMemory.BlockRuntimeConstants, false);
      this.BlockHandlerInfo = new HandlerInfo(this.MyMeter, this.BlockFlashBlock1, (S3_MemoryBlock) sourceMeter.MyDeviceMemory.BlockHandlerInfo, true);
      this.BlockHandlerInfo.Alignment = 1;
      this.BlockFlashBlock2 = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockFlashBlock2, false);
      this.BlockLoggerTable = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock2, sourceMeter.MyDeviceMemory.BlockLoggerTable, false);
      this.BlockMBusTable = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock2, sourceMeter.MyDeviceMemory.BlockMBusTable, true);
      this.BlockFlashBlock3 = new S3_MemoryBlock(this.MyMeter, this.meterMemory, sourceMeter.MyDeviceMemory.BlockFlashBlock3, false);
      this.BlockLoggerData = new S3_MemoryBlock(this.MyMeter, this.BlockFlashBlock3, sourceMeter.MyDeviceMemory.BlockLoggerData, false);
      this.BlockBackupInRAM.IsFixSize = true;
      this.BlockLinkedVars.IsFixSize = true;
      this.BlockBackupStorage1.IsFixSize = true;
      this.BlockBackupStorage0.IsFixSize = true;
      this.BlockRAM.IsFixSize = true;
      this.BlockProtectedConfigValues.IsFixSize = true;
      this.BlockConfigValues.IsFixSize = true;
    }

    internal DeviceMemory Clone(S3_Meter CloneMeter)
    {
      DeviceMemory deviceMemory = new DeviceMemory(CloneMeter);
      deviceMemory.ControllerSegmentsByAddress = this.ControllerSegmentsByAddress;
      deviceMemory.ControllerSegmentsByType = this.ControllerSegmentsByType;
      deviceMemory.CreateClonedSegments(this.MyMeter);
      deviceMemory.MemoryBytes = (byte[]) this.MemoryBytes.Clone();
      deviceMemory.ByteIsDefined = (bool[]) this.ByteIsDefined.Clone();
      deviceMemory.minDefinedAddress = this.minDefinedAddress;
      deviceMemory.minUnDefinedAddress = this.minUnDefinedAddress;
      deviceMemory.flashStartAddress = this.flashStartAddress;
      return deviceMemory;
    }

    internal void ClearMemory()
    {
      for (int index = 0; index < this.ByteIsDefined.Length; ++index)
        this.ByteIsDefined[index] = false;
      this.minUnDefinedAddress = 0;
      this.minDefinedAddress = this.ByteIsDefined.Length;
    }

    internal void LoadFromDifferentMemory(DeviceMemory SourceMemory, int Address, int ByteSize)
    {
      for (int index = 0; index < ByteSize; ++index)
      {
        this.MemoryBytes[Address] = SourceMemory.MemoryBytes[Address];
        this.ByteIsDefined[Address] = SourceMemory.ByteIsDefined[Address];
        ++Address;
      }
    }

    internal bool SetBlockAddressesFromParameters()
    {
      try
      {
        DeviceMemory.ControllerMemoryInfo controllerMemoryInfo1 = this.ControllerSegmentsByType[DeviceMemory.ControllerMemoryTypes.InfoMemory];
        this.BlockInfoMemory.BlockStartAddress = controllerMemoryInfo1.StartAddress;
        this.BlockInfoMemory.ByteSize = controllerMemoryInfo1.Size;
        this.BlockInfoMemory.IsHardLinkedAddress = true;
        this.BlockBackupStorage0.BlockStartAddress = this.BlockInfoMemory.BlockStartAddress;
        this.BlockBackupStorage0.ByteSize = this.BlockInfoMemory.ByteSize / 2;
        this.BlockBackupStorage0.IsHardLinkedAddress = true;
        this.BlockBackupStorage1.BlockStartAddress = this.BlockBackupStorage0.BlockStartAddress + this.BlockBackupStorage0.ByteSize;
        this.BlockBackupStorage1.ByteSize = this.BlockBackupStorage0.ByteSize;
        this.BlockBackupStorage1.IsHardLinkedAddress = true;
        DeviceMemory.ControllerMemoryInfo controllerMemoryInfo2 = this.ControllerSegmentsByType[DeviceMemory.ControllerMemoryTypes.RAM];
        this.BlockRAM.BlockStartAddress = controllerMemoryInfo2.StartAddress;
        this.BlockRAM.ByteSize = controllerMemoryInfo2.Size;
        this.BlockRAM.IsHardLinkedAddress = true;
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName["Con_FunctionTableAdr"];
        int ushortValue1 = (int) this.MyMeter.MyParameters.ParameterByName["Con_DispCodeAdrFromProt_0"].GetUshortValue();
        int num1 = !this.MyMeter.MyParameters.AddressLables.ContainsKey("SERIE3_PROTECTED_CONFIG") ? this.MyMeter.MyParameters.ParameterByName["Con_MeterKey"].BlockStartAddress : this.MyMeter.MyParameters.AddressLables["SERIE3_PROTECTED_CONFIG"];
        int blockStartAddress1 = this.MyMeter.MyParameters.ParameterByName["Con_WritePermissionTableFirstSize"].BlockStartAddress;
        int blockStartAddress2 = this.MyMeter.MyParameters.ParameterByName["Bak_TimeBaseSecounds"].BlockStartAddress;
        int num2 = !this.MyMeter.MyParameters.ParameterByName.ContainsKey("Bak_OutOfBackup") ? this.MyMeter.MyParameters.AddressLables["SERIE3_CONFIG_HEAP"] : this.MyMeter.MyParameters.ParameterByName["Bak_OutOfBackup"].BlockStartAddress;
        int num3 = num2 - blockStartAddress2;
        this.BlockLinkedVars.BlockStartAddress = 7168;
        this.BlockLinkedVars.StartAddressOfNextBlock = blockStartAddress2;
        this.BlockLinkedVars.IsHardLinkedAddress = true;
        this.BlockBackupInRAM.BlockStartAddress = blockStartAddress2;
        this.BlockBackupInRAM.StartAddressOfNextBlock = num2;
        this.BlockBackupInRAM.IsHardLinkedAddress = true;
        this.BlockBackupRuntimeVars.BlockStartAddress = num2;
        this.BlockConfiguratorHeap.BlockStartAddress = this.BlockBackupRuntimeVars.StartAddressOfNextBlock;
        this.MyMeter.MyLinker.Link(this.BlockConfiguratorHeap);
        this.BlockBackup0.BlockStartAddress = this.MyMeter.MyParameters.ParameterByName["Bak_Data0"].BlockStartAddress;
        this.BlockBackup0.ByteSize = num3;
        this.BlockBackup0.IsHardLinkedAddress = true;
        this.BlockBackupRuntimeVars0.BlockStartAddress = this.BlockBackup0.StartAddressOfNextBlock;
        this.BlockBackup1.BlockStartAddress = this.MyMeter.MyParameters.ParameterByName["Bak_Data1"].BlockStartAddress;
        this.BlockBackup1.ByteSize = num3;
        this.BlockBackup1.IsHardLinkedAddress = true;
        this.BlockBackupRuntimeVars1.BlockStartAddress = this.BlockBackup1.StartAddressOfNextBlock;
        this.BlockFlashBlock0.BlockStartAddress = num1;
        this.BlockFlashBlock0.IsHardLinkedAddress = true;
        this.BlockProtectedConfigValues.BlockStartAddress = num1;
        this.BlockProtectedConfigValues.StartAddressOfNextBlock = blockStartAddress1;
        this.BlockProtectedConfigValues.IsHardLinkedAddress = true;
        this.BlockWriteProtTable.BlockStartAddress = blockStartAddress1;
        this.BlockWriteProtTable.IsHardLinkedAddress = true;
        this.BlockProtectedDisplayCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_ProtectedDispCodeAdrTest"].GetUshortValue();
        this.BlockFlashBlock1.BlockStartAddress = s3Parameter.BlockStartAddress;
        this.BlockFlashBlock1.StartAddressOfNextBlock = (int) s3Parameter.GetUshortValue();
        this.BlockFlashBlock1.IsHardLinkedAddress = true;
        this.BlockConfigValues.BlockStartAddress = s3Parameter.BlockStartAddress;
        this.BlockConfigValues.StartAddressOfNextBlock = (int) s3Parameter.GetUshortValue();
        this.BlockConfigValues.IsHardLinkedAddress = true;
        this.BlockFunctionTable.BlockStartAddress = (int) s3Parameter.GetUshortValue();
        this.BlockFunctionTable.IsHardLinkedAddress = true;
        this.BlockDisplayCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_DispCodeAdrFromProt_1"].GetUshortValue();
        this.BlockResetRuntimeCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_ResetRuntimeCodeAdr"].GetUshortValue();
        this.BlockCycleRuntimeCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_CycleRuntimeCodeAdr"].GetUshortValue();
        this.BlockMesurementRuntimeCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_MesurementRuntimeCodeAdr"].GetUshortValue();
        this.BlockMBusRuntimeCode.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_MBusRuntimeCodeAdr"].GetUshortValue();
        int ushortValue2 = (int) this.MyMeter.MyParameters.ParameterByName["Con_FirstLoggerAdr"].GetUshortValue();
        this.BlockFlashBlock2.BlockStartAddress = ushortValue2;
        this.BlockFlashBlock2.Alignment = 512;
        if (this.MyMeter.MyFunctions.holdBaseBlockAddresses)
          this.BlockFlashBlock2.IsHardLinkedAddress = true;
        this.BlockLoggerTable.BlockStartAddress = ushortValue2;
        this.BlockMBusTable.BlockStartAddress = (int) this.MyMeter.MyParameters.ParameterByName["Con_TransmitTablePtr"].GetUshortValue();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Changed memory with undefined data");
        return false;
      }
      return true;
    }

    internal bool AreAllRuntimeCodeAddressesOk()
    {
      int ushortValue1 = (int) this.MyMeter.MyParameters.ParameterByName["Con_ResetRuntimeCodeAdr"].GetUshortValue();
      int ushortValue2 = (int) this.MyMeter.MyParameters.ParameterByName["Con_CycleRuntimeCodeAdr"].GetUshortValue();
      int ushortValue3 = (int) this.MyMeter.MyParameters.ParameterByName["Con_MesurementRuntimeCodeAdr"].GetUshortValue();
      int ushortValue4 = (int) this.MyMeter.MyParameters.ParameterByName["Con_MBusRuntimeCodeAdr"].GetUshortValue();
      if (ushortValue1 != 0 && ushortValue1 != this.BlockResetRuntimeCode.BlockStartAddress)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "ResetRuntimeCodeBlockStartAddress not consistent");
      if (ushortValue2 != 0 && ushortValue2 != this.BlockCycleRuntimeCode.BlockStartAddress && this.BlockCycleRuntimeCode.ByteSize == 0 && this.MyMeter.MyDeviceMemory.GetByteValue(ushortValue2) > (byte) 0)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "CycleRuntimeCodeBlockStartAddress not consistent");
      if (ushortValue3 != 0 && ushortValue3 != this.BlockMesurementRuntimeCode.BlockStartAddress)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MesurementRuntimeCodeBlockStartAddress not consistent");
      return ushortValue4 == 0 || ushortValue4 == this.BlockMBusRuntimeCode.BlockStartAddress || ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MBusRuntimeCodeBlockStartAddress not consistent");
    }

    internal bool AddLinkedParametersToBlocks()
    {
      try
      {
        int ParameterIndex = 0;
        this.AddParamterToBlock(this.BlockLinkedVars, ref ParameterIndex);
        this.AddParamterToBlock(this.BlockBackupInRAM, ref ParameterIndex);
        this.AddParamterToBlock(this.BlockProtectedConfigValues, ref ParameterIndex);
        this.AddParamterToBlock(this.BlockConfigValues, ref ParameterIndex);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Error on add linked parameters to memory blocks");
        return false;
      }
      return true;
    }

    private void AddParamterToBlock(S3_MemoryBlock TheBlock, ref int ParameterIndex)
    {
      while (ParameterIndex < this.MyMeter.MyParameters.ParameterByAddress.Count && this.MyMeter.MyParameters.ParameterByAddress.Keys[ParameterIndex] < TheBlock.BlockStartAddress)
        ++ParameterIndex;
      while (ParameterIndex < this.MyMeter.MyParameters.ParameterByAddress.Count && this.MyMeter.MyParameters.ParameterByAddress.Keys[ParameterIndex] < TheBlock.StartAddressOfNextBlock)
      {
        TheBlock.Add(S3_MemorySegment.Parameter, (S3_MemoryBlock) this.MyMeter.MyParameters.ParameterByAddress.Values[ParameterIndex]);
        ++ParameterIndex;
      }
    }

    internal void SetMinMaxAddress(int address, int byteSize)
    {
      if (address < this.minDefinedAddress || this.minDefinedAddress == 0)
        this.minDefinedAddress = address;
      int num = address + byteSize;
      if (num <= this.minUnDefinedAddress)
        return;
      this.minUnDefinedAddress = num;
    }

    internal byte[] GetPackedByteList(uint? FirmwareVersion = null)
    {
      List<byte> byteList = new List<byte>();
      if (!FirmwareVersion.HasValue)
      {
        byteList.Add((byte) 1);
      }
      else
      {
        byteList.Add((byte) 2);
        byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(FirmwareVersion.Value));
      }
      int length = this.MemoryBytes.Length;
      byteList.Add((byte) length);
      int num1 = length >> 8;
      byteList.Add((byte) num1);
      int num2 = num1 >> 8;
      byteList.Add((byte) num2);
      int num3;
      for (int index1 = 0; index1 < this.MemoryBytes.Length; index1 += num3)
      {
        if (this.ByteIsDefined[index1])
        {
          bool flag = true;
          int index2;
          for (index2 = index1; index2 < this.ByteIsDefined.Length && this.ByteIsDefined[index2]; ++index2)
          {
            if (flag && this.MemoryBytes[index2] > (byte) 0)
            {
              if (index2 - index1 <= 2)
                flag = false;
              else
                break;
            }
          }
          num3 = index2 - index1;
          if (num3 > 16383)
            num3 = 16383;
          if (flag)
          {
            if (num3 > (int) byte.MaxValue)
              num3 = (int) byte.MaxValue;
            byteList.Add((byte) 63);
            byteList.Add((byte) num3);
          }
          else
          {
            if (num3 > 62)
            {
              byteList.Add((byte) (num3 >> 8 | 192));
              byteList.Add((byte) num3);
            }
            else
              byteList.Add((byte) (num3 | 64));
            int num4 = index1 + num3;
            int num5 = index1;
            while (num5 < num4)
              byteList.Add(this.MemoryBytes[num5++]);
          }
        }
        else
        {
          int index3 = index1 + 1;
          while (index3 < this.ByteIsDefined.Length && !this.ByteIsDefined[index3])
            ++index3;
          num3 = index3 - index1;
          if (num3 > 16383)
            num3 = 16383;
          if (num3 > 63)
          {
            byteList.Add((byte) (num3 >> 8 | 128));
            byteList.Add((byte) num3);
          }
          else
            byteList.Add((byte) num3);
        }
      }
      byte[] packedByteList = new byte[byteList.Count];
      for (int index = 0; index < packedByteList.Length; ++index)
        packedByteList[index] = byteList[index];
      return packedByteList;
    }

    internal int DeviceMemorySize
    {
      set
      {
        byte[] numArray = new byte[value];
        bool[] flagArray = new bool[value];
        if (this.MemoryBytes != null)
        {
          int minDefinedAddress = this.minDefinedAddress;
          int num = this.minUnDefinedAddress;
          if (num > numArray.Length)
            num = numArray.Length;
          for (int index = minDefinedAddress; index < num; ++index)
          {
            if (this.ByteIsDefined[index])
            {
              flagArray[index] = true;
              numArray[index] = this.MemoryBytes[index];
            }
          }
        }
        else
        {
          this.minDefinedAddress = 0;
          this.minUnDefinedAddress = 0;
        }
        this.MemoryBytes = numArray;
        this.ByteIsDefined = flagArray;
      }
      get => this.MemoryBytes.Length;
    }

    internal int UsedMemorySize => this.minUnDefinedAddress - this.minDefinedAddress;

    internal bool FillMemory(int Address, int NumberOfBytes, byte fillByte)
    {
      if (!this.IsCacheAvailable(Address, NumberOfBytes))
        return false;
      if (this.UsedMemorySize == 0 || Address < this.minDefinedAddress)
        this.minDefinedAddress = Address;
      for (; NumberOfBytes > 0; --NumberOfBytes)
      {
        this.MemoryBytes[Address] = fillByte;
        this.ByteIsDefined[Address] = true;
        ++Address;
      }
      if (Address > this.minUnDefinedAddress)
        this.minUnDefinedAddress = Address;
      return true;
    }

    internal bool SetValue(ulong TheValue, int Address, int NumberOfBytes)
    {
      if (!this.IsCacheAvailable(Address, NumberOfBytes))
        return false;
      if (this.UsedMemorySize == 0 || Address < this.minDefinedAddress)
        this.minDefinedAddress = Address;
      for (; NumberOfBytes > 0; --NumberOfBytes)
      {
        this.MemoryBytes[Address] = (byte) TheValue;
        this.ByteIsDefined[Address] = true;
        ++Address;
        TheValue >>= 8;
      }
      if (Address > this.minUnDefinedAddress)
        this.minUnDefinedAddress = Address;
      return true;
    }

    internal bool ReadDataFromConnectedDevice(int ReadAddress, int NumberOfBytes)
    {
      if (!this.IsCacheAvailable(ReadAddress, NumberOfBytes))
        return false;
      if (DeviceMemory.S3_DeviceMemoryLogger.IsDebugEnabled)
      {
        int num = ReadAddress + NumberOfBytes - 1;
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Read from: 0x" + ReadAddress.ToString("x04") + " to: 0x" + num.ToString("x04") + " Bytes:" + NumberOfBytes.ToString());
      }
      ByteField MemoryData;
      if (!this.MyMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, ReadAddress, NumberOfBytes, out MemoryData))
      {
        DeviceMemory.S3_DeviceMemoryLogger.Error("Read error on Address: 0x" + this.WriteBlockStartAddress.ToString("x04"));
        return false;
      }
      DeviceMemory.S3_DeviceMemoryLogger.Debug("Read ok");
      if (DeviceMemory.S3_DeviceMemoryLogger.IsTraceEnabled)
      {
        foreach (string message in ParameterService.GetMemoryInfo(MemoryData, ReadAddress))
          DeviceMemory.S3_DeviceMemoryLogger.Trace(message);
      }
      for (int index = 0; index < MemoryData.Count; ++index)
      {
        this.MemoryBytes[ReadAddress + index] = MemoryData.Data[index];
        this.ByteIsDefined[ReadAddress + index] = true;
      }
      this.SetMinMaxAddress(ReadAddress, MemoryData.Count);
      return true;
    }

    internal bool WriteDataToConnectedDevice(int Address, int ByteSize)
    {
      this.WriteBlockStartAddress = Address;
      ByteField byteField = new ByteField(ByteSize);
      for (int index = 0; index < ByteSize; ++index)
      {
        if (!this.ByteIsDefined[Address])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Write of not initialized data on Address: 0x" + Address.ToString("x04"));
          return false;
        }
        byteField.Add(this.MemoryBytes[Address++]);
      }
      if (!this.MyMeter.MyFunctions.MyCommands.WriteMemory(MemoryLocation.RAM, this.WriteBlockStartAddress, byteField))
      {
        DeviceMemory.S3_DeviceMemoryLogger.Error("Write error on Address: 0x" + this.WriteBlockStartAddress.ToString("x04"));
        return false;
      }
      return this.CheckWriteByRead(this.WriteBlockStartAddress, byteField);
    }

    internal bool IsBackupChecksumOk(
      S3_MemoryBlock backupBlock,
      int backupSize,
      out ushort newChecksum)
    {
      ushort ushortValue = this.GetUshortValue(backupBlock.BlockStartAddress);
      newChecksum = (ushort) 0;
      int num = backupBlock.BlockStartAddress + 2;
      for (; backupSize > 0; --backupSize)
        newChecksum += (ushort) this.MemoryBytes[num++];
      return (int) newChecksum == (int) ushortValue;
    }

    internal void SaveBackup(S3_MemoryBlock backupBlock, ushort backupSize)
    {
      ushort NewValue = 0;
      int blockStartAddress1 = this.BlockBackupInRAM.BlockStartAddress;
      int blockStartAddress2 = backupBlock.BlockStartAddress;
      int Address = blockStartAddress2 - 2;
      for (; backupSize > (ushort) 0; --backupSize)
      {
        NewValue += (ushort) this.MemoryBytes[blockStartAddress1];
        this.MemoryBytes[blockStartAddress2++] = this.MemoryBytes[blockStartAddress1++];
      }
      this.SetUshortValue(Address, NewValue);
    }

    internal bool WriteChangesToConnectedDevice(
      DeviceMemory CompareMemory,
      DeviceMemory.ControllerMemoryTypes ControllerMemoryType)
    {
      int MinAdrRestricted = -1;
      int MaxAdrRestricted = 0;
      for (int index = 0; index < this.ControllerSegmentsByAddress.Count; ++index)
      {
        if (this.ControllerSegmentsByAddress[index].ControllerMemoryType == ControllerMemoryType)
        {
          if (MinAdrRestricted < 0)
            MinAdrRestricted = this.ControllerSegmentsByAddress[index].StartAddress;
          MaxAdrRestricted = this.ControllerSegmentsByAddress[index].LastAddress;
        }
        else if (MinAdrRestricted >= 0)
          break;
      }
      if (ControllerMemoryType == DeviceMemory.ControllerMemoryTypes.RAM)
        MinAdrRestricted = this.MyMeter.MyParameters.AddressLables["SERIE3_BACKUP_I"];
      return this.WriteChangesToConnectedDevice(CompareMemory, MinAdrRestricted, MaxAdrRestricted);
    }

    internal bool WriteChangesToConnectedDevice(
      DeviceMemory CompareMemory,
      int MinAdrRestricted,
      int MaxAdrRestricted)
    {
      this.WriteBlockStartAddress = -1;
      this.WriteBlockEndAddress = -1;
      DeviceMemory.S3_DeviceMemoryLogger.Debug("Write changes to connected device");
      try
      {
        int length1 = this.ByteIsDefined.Length;
        int num1 = length1;
        if (CompareMemory.ByteIsDefined.Length > length1)
        {
          int length2 = CompareMemory.ByteIsDefined.Length;
        }
        else if (CompareMemory.ByteIsDefined.Length < num1)
          num1 = CompareMemory.ByteIsDefined.Length;
        int num2 = this.minDefinedAddress;
        if (CompareMemory.minDefinedAddress < num2)
          num2 = CompareMemory.minDefinedAddress;
        if (num2 < MinAdrRestricted)
          num2 = MinAdrRestricted;
        int num3 = this.minUnDefinedAddress;
        if (CompareMemory.minUnDefinedAddress > num3)
          num3 = CompareMemory.minUnDefinedAddress;
        if (num3 > MaxAdrRestricted)
          num3 = MaxAdrRestricted;
        int num4 = num3;
        if (num1 < num4)
          num4 = num1;
        int Address;
        for (Address = num2; Address < num4; ++Address)
        {
          bool flag = false;
          if (this.ByteIsDefined[Address] && CompareMemory.ByteIsDefined[Address])
          {
            if ((int) this.MemoryBytes[Address] != (int) CompareMemory.MemoryBytes[Address])
              flag = true;
          }
          else if (this.ByteIsDefined[Address])
            flag = true;
          else if (CompareMemory.ByteIsDefined[Address])
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Changed memory with undefined data", DeviceMemory.S3_DeviceMemoryLogger);
            goto label_28;
          }
          if (flag && !this.WriteByte(CompareMemory, this.MemoryBytes[Address], Address))
            goto label_28;
        }
        if (this.SendWriteBuffer())
        {
          DeviceMemory.S3_DeviceMemoryLogger.Debug("Write changes done");
          return true;
        }
label_28:
        DeviceMemory.S3_DeviceMemoryLogger.Error("Write changes error. WorkAddress: " + Address.ToString("x04"));
        return false;
      }
      catch (Exception ex)
      {
        string str = "Exception on write changes to device";
        DeviceMemory.S3_DeviceMemoryLogger.Error(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(str);
        return false;
      }
    }

    private bool WriteByte(DeviceMemory CompareMemory, byte TheByte, int Address)
    {
      if (this.WriteBuffer == null)
        this.WriteBuffer = new ByteField(512);
      try
      {
        if ((Address < this.WriteBlockStartAddress || Address > this.WriteBlockEndAddress) && !this.PrepareNewBlock(Address))
          return false;
        if (this.IsFlashBlock)
          return true;
        bool flag = false;
        int num = this.ramWriteStartAddress + this.WriteBuffer.Count;
        if (Address >= this.ramWriteStartAddress + this.WriteBuffer.Data.Length)
          flag = true;
        else if (num == Address)
          this.WriteBuffer.Add(TheByte);
        else if (Address - num >= 100)
        {
          flag = true;
        }
        else
        {
          for (int address = num; address < Address; ++address)
          {
            if (!this.ByteIsDefined[address])
            {
              flag = true;
              break;
            }
            if (CompareMemory.MyMeter.IsWriteProtected && CompareMemory.MyMeter.MyWriteProtTableManager.IsByteProtected((ushort) address))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            for (int index = num; index < Address; ++index)
              this.WriteBuffer.Add(this.MemoryBytes[index]);
            this.WriteBuffer.Add(TheByte);
          }
        }
        if (flag)
        {
          if (!this.SendWriteBuffer())
            return false;
          this.ramWriteStartAddress = Address;
          this.WriteBuffer.Add(TheByte);
        }
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Exceptin on WriteByte");
        return false;
      }
    }

    private bool PrepareNewBlock(int Address)
    {
      if (DeviceMemory.S3_DeviceMemoryLogger.IsDebugEnabled)
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Prepare new block. Adr: 0x" + Address.ToString("x04"));
      if (!this.SendWriteBuffer())
        return false;
      DeviceMemory.ControllerMemoryInfo memInfo = this.GetMemInfo(Address);
      if (memInfo.FlashBlockSize > 0)
      {
        this.IsFlashBlock = true;
        int flashBlockSize = memInfo.FlashBlockSize;
        this.WriteBlockStartAddress = Address & ~(flashBlockSize - 1);
        this.WriteBlockEndAddress = this.WriteBlockStartAddress + flashBlockSize - 1;
        string str = string.Empty;
        if (DeviceMemory.S3_DeviceMemoryLogger.IsDebugEnabled)
          str = " Address: 0x" + this.WriteBlockStartAddress.ToString("x04") + "; Size: 0x" + flashBlockSize.ToString("x04");
        for (int blockStartAddress = this.WriteBlockStartAddress; blockStartAddress <= this.WriteBlockEndAddress; ++blockStartAddress)
        {
          if (!this.ByteIsDefined[blockStartAddress])
          {
            DeviceMemory.S3_DeviceMemoryLogger.Debug("Flash block not complete. Read from device." + str);
            if (!this.ReadNotDefinedFlashBankData(this.WriteBlockStartAddress, memInfo.FlashBlockSize))
            {
              DeviceMemory.S3_DeviceMemoryLogger.Error("Read flash block error.");
              return false;
            }
            break;
          }
        }
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Erase flash block." + str);
        if (!this.MyMeter.MyFunctions.MyCommands.EraseFlash(this.WriteBlockStartAddress, flashBlockSize))
        {
          DeviceMemory.S3_DeviceMemoryLogger.Error("Erase flash block error." + str);
          return false;
        }
        for (int index = 0; index < flashBlockSize; ++index)
          this.WriteBuffer.Add(this.MemoryBytes[this.WriteBlockStartAddress + index]);
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Write flash block." + str);
        if (DeviceMemory.S3_DeviceMemoryLogger.IsTraceEnabled)
        {
          foreach (string message in ParameterService.GetMemoryInfo(this.WriteBuffer, this.WriteBlockStartAddress))
            DeviceMemory.S3_DeviceMemoryLogger.Trace(message);
        }
        if (!this.MyMeter.MyFunctions.MyCommands.WriteMemory(MemoryLocation.FLASH, this.WriteBlockStartAddress, this.WriteBuffer))
        {
          DeviceMemory.S3_DeviceMemoryLogger.Error("Write flash block error." + str);
          return false;
        }
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Write flash block ok.");
        if (!this.CheckWriteByRead(this.WriteBlockStartAddress, this.WriteBuffer))
          return false;
        this.WriteBuffer.Count = 0;
      }
      else
      {
        this.IsFlashBlock = false;
        this.WriteBlockStartAddress = Address;
        this.WriteBlockEndAddress = memInfo.LastAddress;
        this.ramWriteStartAddress = Address;
      }
      return true;
    }

    private DeviceMemory.ControllerMemoryInfo GetMemInfo(int Address)
    {
      for (int index = 0; index < this.ControllerSegmentsByAddress.Count - 1; ++index)
      {
        if (this.ControllerSegmentsByAddress[index].LastAddress >= Address)
          return this.ControllerSegmentsByAddress[index];
      }
      return (DeviceMemory.ControllerMemoryInfo) null;
    }

    private bool SendWriteBuffer()
    {
      try
      {
        if (this.WriteBuffer == null || this.WriteBuffer.Count == 0 || this.ramWriteStartAddress < 0)
          return true;
        string str = " Address: 0x" + this.ramWriteStartAddress.ToString("x04") + "; Size: 0x" + this.WriteBuffer.Count.ToString("x04");
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Write ram block." + str);
        if (!this.MyMeter.MyFunctions.MyCommands.WriteMemory(MemoryLocation.RAM, this.ramWriteStartAddress, this.WriteBuffer))
        {
          DeviceMemory.S3_DeviceMemoryLogger.Error("Write RAM block error." + str);
          return false;
        }
        if (DeviceMemory.S3_DeviceMemoryLogger.IsTraceEnabled)
        {
          foreach (string message in ParameterService.GetMemoryInfo(this.WriteBuffer, this.ramWriteStartAddress))
            DeviceMemory.S3_DeviceMemoryLogger.Trace(message);
        }
        if (!this.CheckWriteByRead(this.WriteBlockStartAddress, this.WriteBuffer))
          return false;
        this.WriteBuffer.Count = 0;
        return true;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Exceptin on SendWriteBuffer");
        return false;
      }
    }

    private bool ReadNotDefinedFlashBankData(int BankAddress, int BlockSize)
    {
      if (!this.IsCacheAvailable(BankAddress, BlockSize))
      {
        DeviceMemory.S3_DeviceMemoryLogger.Error("Read not defined flash bank data: Cache not avaailable");
        return false;
      }
      if (DeviceMemory.S3_DeviceMemoryLogger.IsDebugEnabled)
      {
        string str = " Address: 0x" + BankAddress.ToString("x04") + "; Size: 0x" + BlockSize.ToString("x04");
        DeviceMemory.S3_DeviceMemoryLogger.Debug("Read not defined flash bank data." + str);
      }
      ByteField MemoryData;
      if (!this.MyMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.FLASH, BankAddress, BlockSize, out MemoryData))
      {
        DeviceMemory.S3_DeviceMemoryLogger.Error("Read not defined flash bank data: DeviceCollector read error.");
        return false;
      }
      if (DeviceMemory.S3_DeviceMemoryLogger.IsTraceEnabled)
      {
        foreach (string message in ParameterService.GetMemoryInfo(MemoryData, BankAddress))
          DeviceMemory.S3_DeviceMemoryLogger.Trace(message);
      }
      for (int index = 0; index < MemoryData.Count; ++index)
      {
        if (!this.ByteIsDefined[BankAddress + index])
        {
          this.MemoryBytes[BankAddress + index] = MemoryData.Data[index];
          this.ByteIsDefined[BankAddress + index] = true;
        }
      }
      this.SetMinMaxAddress(BankAddress, MemoryData.Count);
      return true;
    }

    internal byte GetByteValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 1) ? (byte) 0 : this.MemoryBytes[Address++];
    }

    internal byte[] GetByteArray(int Address, int Size)
    {
      byte[] dst = this.IsCacheInitialised(Address, Size) ? new byte[Size] : throw new Exception("Data not available");
      Buffer.BlockCopy((Array) this.MemoryBytes, Address, (Array) dst, 0, Size);
      return dst;
    }

    internal bool SetByteValue(int Address, byte NewValue)
    {
      if (!this.IsCacheAvailable(Address, 1))
        return false;
      this.MemoryBytes[Address] = NewValue;
      NewValue >>= 8;
      this.SetMinMaxAddress(Address, 1);
      this.ByteIsDefined[Address] = true;
      return true;
    }

    internal bool SetByteArray(int Address, byte[] Value)
    {
      int length = Value.Length;
      if (!this.IsCacheAvailable(Address, length))
        return false;
      for (int index = 0; index < length; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = Value[index];
      }
      this.SetMinMaxAddress(Address, length);
      return true;
    }

    internal ushort GetUshortValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 2) ? (ushort) 0 : (ushort) ((int) (ushort) this.MemoryBytes[Address++] + (int) (ushort) ((uint) this.MemoryBytes[Address++] << 8));
    }

    internal bool SetUshortValue(int Address, ushort NewValue)
    {
      int num = 2;
      if (!this.IsCacheAvailable(Address, num))
        return false;
      for (int index = 0; index < num; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = (byte) NewValue;
        NewValue >>= 8;
      }
      this.SetMinMaxAddress(Address, num);
      return true;
    }

    internal bool SetShortValue(int Address, short NewValue)
    {
      int num = 2;
      if (!this.IsCacheAvailable(Address, num))
        return false;
      for (int index = 0; index < num; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = (byte) NewValue;
        NewValue >>= 8;
      }
      this.SetMinMaxAddress(Address, num);
      return true;
    }

    internal sbyte GetSByteValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 1) ? (sbyte) 0 : (sbyte) this.MemoryBytes[Address];
    }

    internal short GetShortValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 2) ? (short) 0 : BitConverter.ToInt16(this.MemoryBytes, Address);
    }

    internal int GetIntValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 4) ? 0 : BitConverter.ToInt32(this.MemoryBytes, Address);
    }

    internal DateTime GetDateTimeValue(int Address)
    {
      return ZR_Calendar.Cal_GetDateTime(this.GetUintValue(Address));
    }

    internal uint GetUintValue(int Address)
    {
      if (!this.IsCacheInitialised(Address, 4))
        return 0;
      uint uintValue = 0;
      for (int index = 0; index < 4; ++index)
        uintValue += (uint) this.MemoryBytes[Address++] << index * 8;
      return uintValue;
    }

    internal bool SetDateTimeValue(int Address, DateTime NewValue)
    {
      uint meterTime = ZR_Calendar.Cal_GetMeterTime(NewValue);
      return this.SetUintValue(Address, meterTime);
    }

    internal bool SetUintValue(int Address, uint NewValue)
    {
      int num = 4;
      if (!this.IsCacheAvailable(Address, num))
        return false;
      for (int index = 0; index < num; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = (byte) NewValue;
        NewValue >>= 8;
      }
      this.SetMinMaxAddress(Address, num);
      return true;
    }

    internal ulong GetUlongValue(int Address, int ByteSize)
    {
      if (!this.IsCacheInitialised(Address, ByteSize))
        return 0;
      ulong ulongValue = 0;
      for (int index = 0; index < ByteSize; ++index)
        ulongValue += (ulong) this.MemoryBytes[Address++] << index * 8;
      return ulongValue;
    }

    internal bool SetUlongValue(int Address, int ByteSize, ulong NewValue)
    {
      if (!this.IsCacheAvailable(Address, ByteSize))
        return false;
      for (int index = 0; index < ByteSize; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = (byte) NewValue;
        NewValue >>= 8;
      }
      this.SetMinMaxAddress(Address, ByteSize);
      return true;
    }

    internal float GetFloatValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 4) ? 0.0f : BitConverter.ToSingle(this.MemoryBytes, Address);
    }

    internal bool SetFloatValue(int Address, float NewValue)
    {
      if ((double) NewValue == double.NaN)
        return false;
      byte[] bytes = BitConverter.GetBytes(NewValue);
      if (!this.IsCacheAvailable(Address, bytes.Length))
        return false;
      for (int index = 0; index < bytes.Length; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = bytes[index];
      }
      this.SetMinMaxAddress(Address, bytes.Length);
      return true;
    }

    internal double GetDoubleValue(int Address)
    {
      return !this.IsCacheInitialised(Address, 8) ? 0.0 : BitConverter.ToDouble(this.MemoryBytes, Address);
    }

    internal bool SetDoubleValue(int Address, double NewValue)
    {
      byte[] bytes = BitConverter.GetBytes(NewValue);
      if (!this.IsCacheAvailable(Address, bytes.Length))
        return false;
      for (int index = 0; index < bytes.Length; ++index)
      {
        this.ByteIsDefined[Address] = true;
        this.MemoryBytes[Address++] = bytes[index];
      }
      this.SetMinMaxAddress(Address, bytes.Length);
      return true;
    }

    private bool IsCacheAvailable(int Address, int ByteSize)
    {
      if (this.MemoryBytes.Length >= Address + ByteSize)
        return true;
      ZR_ClassLibMessages.AddWarning("Acces at not available memory cache on Address:" + Address.ToString("x04"));
      return false;
    }

    private bool IsCacheInitialised(int Address, int ByteSize)
    {
      if (this.MemoryBytes.Length < Address + ByteSize)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read at not available memory cache on Address:" + Address.ToString());
        return false;
      }
      for (int index = 0; index < ByteSize; ++index)
      {
        if (!this.ByteIsDefined[Address])
        {
          ZR_ClassLibMessages.AddWarning("Acces at not available memory cache on Address:" + Address.ToString("x04"));
          return false;
        }
        ++Address;
      }
      return true;
    }

    internal bool IsCacheInitialisedNoWarning(int Address, int ByteSize)
    {
      if (this.MemoryBytes.Length < Address + ByteSize)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read at not available memory cache on Address:" + Address.ToString("x04"));
        return false;
      }
      for (int index = 0; index < ByteSize; ++index)
      {
        if (!this.ByteIsDefined[Address])
          return false;
        ++Address;
      }
      return true;
    }

    internal bool GarantCacheIsInitialised(int Address, int ByteSize)
    {
      if (this.MemoryBytes.Length < Address + ByteSize)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read at not available memory cache on Address:" + Address.ToString("x04"));
        return false;
      }
      for (int index1 = 0; index1 < ByteSize; ++index1)
      {
        if (!this.ByteIsDefined[Address])
        {
          if (this.MyMeter.MyFunctions.MyMeters.ConnectedMeter != null)
          {
            int num1 = Address + ByteSize;
            int ReadAddress = Address;
            int NumberOfBytes = ByteSize;
            if (NumberOfBytes < 200)
              NumberOfBytes = 200;
            if (Address >= this.flashStartAddress)
            {
              ReadAddress = (Address - this.flashStartAddress) / 200 * 200 + this.flashStartAddress;
              NumberOfBytes = 200;
              while (ReadAddress + NumberOfBytes < num1)
                NumberOfBytes += 200;
            }
            int num2 = this.MemoryBytes.Length - ReadAddress;
            if (NumberOfBytes > num2)
              NumberOfBytes = num2;
            if (!this.MyMeter.MyFunctions.MyMeters.ConnectedMeter.MyDeviceMemory.ReadDataFromConnectedDevice(ReadAddress, NumberOfBytes))
            {
              ZR_ClassLibMessages.AddErrorDescription("Read to cache error on Address:" + ReadAddress.ToString("x04"));
              return false;
            }
            if (this.MyMeter == this.MyMeter.MyFunctions.MyMeters.WorkMeter)
            {
              for (int index2 = 0; index2 < NumberOfBytes; ++index2)
              {
                this.MyMeter.MyDeviceMemory.MemoryBytes[ReadAddress + index2] = this.MyMeter.MyFunctions.MyMeters.ConnectedMeter.MyDeviceMemory.MemoryBytes[ReadAddress + index2];
                this.MyMeter.MyDeviceMemory.ByteIsDefined[ReadAddress + index2] = true;
              }
            }
            else if (this.MyMeter != this.MyMeter.MyFunctions.MyMeters.ConnectedMeter)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Update meter cache error on Address:" + Address.ToString("x04"));
              return false;
            }
          }
          else
          {
            ZR_ClassLibMessages.AddWarning("Read at uninitialised memory cache on Address:" + Address.ToString("x04"));
            return false;
          }
        }
        ++Address;
      }
      return true;
    }

    private bool CheckWriteByRead(int WriteBlockStartAddress, ByteField writeData)
    {
      if (!this.MyMeter.MyFunctions.checkWriteByRead)
        return true;
      ByteField MemoryData;
      if (!this.MyMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, WriteBlockStartAddress, writeData.Count, out MemoryData))
      {
        DeviceMemory.S3_DeviceMemoryLogger.Error("Read error on Address: 0x" + WriteBlockStartAddress.ToString("x04"));
        return false;
      }
      int num = WriteBlockStartAddress;
      for (int index = 0; index < writeData.Count; ++index)
      {
        if ((int) MemoryData.Data[index] != (int) writeData.Data[index])
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Data error on Address: 0x" + num.ToString("x04") + " Write: 0x" + writeData.Data[index].ToString("x02") + "Read: 0x" + MemoryData.Data[index].ToString("x02"), DeviceMemory.S3_DeviceMemoryLogger);
          return false;
        }
        ++num;
      }
      DeviceMemory.S3_DeviceMemoryLogger.Info("Write checked by read. Data ok!");
      return true;
    }

    internal DeviceMemory GetChangedMemory(DeviceMemory CompareMemory)
    {
      int length = this.ByteIsDefined.Length;
      int num1 = length;
      if (CompareMemory.ByteIsDefined.Length > length)
        length = CompareMemory.ByteIsDefined.Length;
      else if (CompareMemory.ByteIsDefined.Length < num1)
        num1 = CompareMemory.ByteIsDefined.Length;
      DeviceMemory changedMemory = new DeviceMemory(this.MyMeter, length);
      int minDefinedAddress = this.minDefinedAddress;
      if (CompareMemory.minDefinedAddress < minDefinedAddress)
        minDefinedAddress = CompareMemory.minDefinedAddress;
      int unDefinedAddress = this.minUnDefinedAddress;
      if (CompareMemory.minUnDefinedAddress < unDefinedAddress)
        unDefinedAddress = CompareMemory.minUnDefinedAddress;
      int num2 = unDefinedAddress;
      if (num1 < num2)
        num2 = num1;
      for (int index = minDefinedAddress; index < num2; ++index)
      {
        if (this.ByteIsDefined[index] && CompareMemory.ByteIsDefined[index])
        {
          if ((int) this.MemoryBytes[index] != (int) CompareMemory.MemoryBytes[index])
          {
            changedMemory.MemoryBytes[index] = this.MemoryBytes[index];
            changedMemory.ByteIsDefined[index] = true;
          }
        }
        else if (this.ByteIsDefined[index])
        {
          changedMemory.MemoryBytes[index] = this.MemoryBytes[index];
          changedMemory.ByteIsDefined[index] = true;
        }
        else
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Changed memory with undefined data");
          return (DeviceMemory) null;
        }
      }
      return changedMemory;
    }

    internal bool GetVolumeCounterValue(out byte volumeCounterValue)
    {
      volumeCounterValue = (byte) 0;
      ByteField MemoryData;
      if (this.MyMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, 912, 1, out MemoryData))
      {
        byte[] numArray = new byte[6];
        numArray[0] = MemoryData.Data[0];
        for (int index = 0; index < 5; ++index)
        {
          volumeCounterValue = MemoryData.Data[0];
          if (this.MyMeter.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, 912, 1, out MemoryData))
          {
            numArray[index + 1] = MemoryData.Data[0];
            if ((int) volumeCounterValue == (int) MemoryData.Data[0])
              return true;
          }
          else
            goto label_11;
        }
        string str = "";
        for (int index = 0; index < 6; ++index)
          str = str + " " + numArray[index].ToString("x02");
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Counter value not stable:" + str);
      }
label_11:
      ZR_ClassLibMessages.AddErrorDescription("Read volume counter error");
      return false;
    }

    internal enum PhysicalMemoryTypes
    {
      None,
      Register,
      RAM,
      Flash,
      ROM,
    }

    internal enum ControllerMemoryTypes
    {
      Peripherals,
      BootstrapLoader,
      InfoMemory,
      DeviceDecription,
      RAM,
      Unused,
      MainFlash,
      OutOfMemory,
    }

    internal class ControllerMemoryInfo
    {
      internal DeviceMemory.PhysicalMemoryTypes PhysicalMemoryType;
      internal DeviceMemory.ControllerMemoryTypes ControllerMemoryType;
      internal int StartAddress;
      internal int Size;
      internal int LastAddress;
      internal int FlashBlockSize;
      internal int FlashBank;

      internal ControllerMemoryInfo(
        DeviceMemory.ControllerMemoryTypes ControllerMemoryType,
        DeviceMemory.PhysicalMemoryTypes MemoryType,
        int StartAddress,
        int FlashBlockSize,
        int FlashBank)
      {
        this.ControllerMemoryType = ControllerMemoryType;
        this.PhysicalMemoryType = MemoryType;
        this.StartAddress = StartAddress;
        this.FlashBlockSize = FlashBlockSize;
        this.FlashBank = FlashBank;
      }
    }
  }
}
