// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareUpdateFunctions
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using Microsoft.Win32;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class FirmwareUpdateFunctions
  {
    internal string strFileName = string.Empty;
    internal string strFirmwareFile = string.Empty;
    internal string strBootloaderFile = string.Empty;
    internal byte[] baVectorInitTable;
    internal byte[] baFirstEightBytesFromHexFile;
    private CommunicationPortFunctions myPort = (CommunicationPortFunctions) null;
    private int mapID = -1;
    private SortedDictionary<uint, byte[]> dicBytesToFlash = (SortedDictionary<uint, byte[]>) null;
    internal SortedDictionary<uint, byte[]> dicBytesToFlashBOOTLOADER = (SortedDictionary<uint, byte[]>) null;
    internal SortedDictionary<uint, byte[]> dicBytesToFlashFIRMWARE = (SortedDictionary<uint, byte[]>) null;
    internal SortedDictionary<uint, byte[]> dicBytesToFlashMERGE = (SortedDictionary<uint, byte[]>) null;
    internal SortedDictionary<uint, byte[]> dicBytesToFlashBACKPACK = (SortedDictionary<uint, byte[]>) null;
    internal List<FirmwareBlockInfoClass> blockInfo_Firmware = (List<FirmwareBlockInfoClass>) null;
    internal List<FirmwareBlockInfoClass> blockInfo_Bootloader = (List<FirmwareBlockInfoClass>) null;
    internal List<FirmwareBlockInfoClass> blockInfo_NoVerify = (List<FirmwareBlockInfoClass>) null;
    internal SortedList<uint, HardwareTypeTables.ProgFilesRow> availBootloaderProgFiles = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
    internal SortedList<uint, HardwareTypeTables.ProgFilesRow> availFirmwareProgFiles = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
    internal bool isReady2GO;
    internal bool isReady2WriteFW;
    public bool isReady2EraseFW;
    public uint iFlashStartAdr = 134217728;
    public bool isFirstEightBytesLoaded;
    public bool isVectorTableLoaded;
    private FirmwareUpdateToolDeviceCommands fwUpdateToolCMDs;
    private BootLoaderFunctions BTLFunctions;
    public FirmwareVersion BSLversion = new FirmwareVersion(16781346U);
    public AddressRange ArmIdRange = new AddressRange(536346704U, 24U);
    internal FirmwareData dataOfBootloader = (FirmwareData) null;
    internal FirmwareData dataOfFrmware = (FirmwareData) null;
    internal List<FirmwareData> ListOfLoadedBootloader = (List<FirmwareData>) null;
    internal List<FirmwareData> ListOfLoadedFrmware = (List<FirmwareData>) null;
    internal uint iMeterKey = 134742016;
    internal uint iVectorTable = 134217728;
    private bool writeNOW;
    public bool isHandlerAvailable = false;
    public bool isHandlerFunctionsAvailable = false;
    private HandlerFunctionsForProduction _myHandlerForProduction;
    private IHandler _myHandlerFunctions;
    public string CoreVersion = "1.0.14";
    private FirmwareUpdateFunctions.UpdateDataBlock myUpDateBlock = (FirmwareUpdateFunctions.UpdateDataBlock) null;

    public bool isBSLrunning { get; private set; }

    public bool isBootLoaderFile { get; private set; }

    public bool isFirmWareFile { get; private set; }

    public uint iBootLoaderStartAddress { get; private set; }

    public uint iBootLoaderEndAddress { get; private set; }

    public DeviceIdentification deviceIdentification { get; private set; }

    public DeviceIdentification deviceIdentificationForBackup { get; private set; }

    public AddressRange BSL_AddressRange { get; private set; }

    public AddressRange FW_AddressRange { get; private set; }

    public uint verifyPageSize { get; set; }

    public string HardwareName { get; set; }

    public string firmwareFileData { get; private set; }

    public byte[] zippedBackUpData { get; private set; }

    public bool isDeviceIUW_NFC { get; set; }

    public HandlerFunctionsForProduction myHandlerForProduction
    {
      get => this._myHandlerForProduction;
      set
      {
        this._myHandlerForProduction = value;
        this.isHandlerAvailable = value != null;
      }
    }

    public IHandler myHandlerFunctions
    {
      get => this._myHandlerFunctions;
      set
      {
        this._myHandlerFunctions = value;
        this.isHandlerFunctionsAvailable = value != null;
      }
    }

    public IZDebugInfo oFUI { get; set; }

    public bool bIsFirmwareVerifyError { get; private set; }

    public bool bIsBootloaderVerifyError { get; private set; }

    public FirmwareUpdateFunctions(
      CommunicationPortFunctions port,
      FirmwareUpdateToolDeviceCommands fwUpToolDeviceCMD)
    {
      this.myPort = port;
      this.fwUpdateToolCMDs = fwUpToolDeviceCMD;
      this.BTLFunctions = new BootLoaderFunctions(this.myPort, this.fwUpdateToolCMDs);
      this.isReady2GO = false;
      this.isBootLoaderFile = false;
      this.isReady2EraseFW = false;
      this.isReady2WriteFW = false;
      this.isFirstEightBytesLoaded = false;
      this.isVectorTableLoaded = false;
      this.iBootLoaderStartAddress = 0U;
      this.iBootLoaderEndAddress = 0U;
      this.verifyPageSize = 128U;
      this.baFirstEightBytesFromHexFile = new byte[8];
      this.baVectorInitTable = new byte[192];
      this.dicBytesToFlash = new SortedDictionary<uint, byte[]>();
      this.deviceIdentification = (DeviceIdentification) null;
      this.ListOfLoadedBootloader = new List<FirmwareData>();
      this.ListOfLoadedFrmware = new List<FirmwareData>();
    }

    public void SetMapID(int mapID = 255) => this.mapID = mapID;

    public void clearVariables()
    {
      this.dicBytesToFlash = (SortedDictionary<uint, byte[]>) null;
      this.dicBytesToFlashBOOTLOADER = (SortedDictionary<uint, byte[]>) null;
      this.dicBytesToFlashFIRMWARE = (SortedDictionary<uint, byte[]>) null;
      this.dicBytesToFlashMERGE = (SortedDictionary<uint, byte[]>) null;
      this.dicBytesToFlashBACKPACK = (SortedDictionary<uint, byte[]>) null;
      this.availBootloaderProgFiles = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
      this.availFirmwareProgFiles = (SortedList<uint, HardwareTypeTables.ProgFilesRow>) null;
      this.isReady2GO = false;
      this.isReady2WriteFW = false;
      this.isReady2EraseFW = false;
      this.isBSLrunning = false;
      this.isBootLoaderFile = false;
      this.isFirmWareFile = false;
      this.iBootLoaderStartAddress = 0U;
      this.iBootLoaderEndAddress = 0U;
      this.iFlashStartAdr = 134217728U;
      this.isFirstEightBytesLoaded = false;
      this.isVectorTableLoaded = false;
      this.BSL_AddressRange = (AddressRange) null;
      this.deviceIdentification = (DeviceIdentification) null;
      this.bIsBootloaderVerifyError = false;
      this.bIsFirmwareVerifyError = false;
    }

    public async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceIdentification deviceIdentification = await this.fwUpdateToolCMDs.ReadVersionAsync(progress, token);
      this.deviceIdentification = deviceIdentification;
      deviceIdentification = (DeviceIdentification) null;
      ConfigList locCfgList = this.myPort.GetReadoutConfiguration();
      ConnectionProfileIdentification conProfileIdent = new ConnectionProfileIdentification(locCfgList.ConnectionProfileID);
      bool isNON_NFC = Enum.IsDefined(typeof (BTL_NON_NFC_Devices), (object) conProfileIdent.DeviceModelID);
      this.isDeviceIUW_NFC = locCfgList.BusMode.Contains("NFC") && !isNON_NFC;
      this.verifyPageSize = this.isDeviceIUW_NFC ? 512U : 128U;
      this.isBSLrunning = false;
      DeviceIdentification deviceIdentification1 = this.deviceIdentification;
      locCfgList = (ConfigList) null;
      conProfileIdent = (ConnectionProfileIdentification) null;
      return deviceIdentification1;
    }

    public async Task prepareUpdate(
      ProgressHandler progress,
      CancellationTokenSource token,
      uint flashStartAddress = 134217728)
    {
      if (this.isDeviceIUW_NFC)
      {
        this.readFirstEightBytesFromBootLoader();
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rreading first eight bytes from bootloader ... Done.");
        await this.writeFirstEightBytesToFLASH(progress, token.Token);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rwrite first eight bytes to FLASH ... Done.");
        await this.doSystemResetFunction(progress, token);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rdo System RESET ... Done.");
      }
      else
      {
        this.iFlashStartAdr = flashStartAddress;
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rprepare for entering BOOTLOADER mode ... Done.");
        this.readFirstEightBytesFromBootLoader(this.iFlashStartAdr);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rreading first eight bytes from bootloader ... Done.");
        await this.readVectorTableFromDevice(progress, token, this.iFlashStartAdr);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rread vector table from FLASH ... Done.");
        this.writeFirstEightBytesToFLASH(progress, token.Token, this.baFirstEightBytesFromHexFile, this.iFlashStartAdr).Wait(2000);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rwrite first eight bytes to FLASH ... Done.");
        this.doSystemResetFunction(progress, token).Wait(2000);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("\rdo System RESET ... Done.");
      }
      this.isReady2EraseFW = true;
    }

    public void readFirstEightBytesFromBootLoader(uint flashStartAddress = 134217728)
    {
      this.baFirstEightBytesFromHexFile = this.getFirstEightBytesFromBootLoader(flashStartAddress);
    }

    public byte[] getFirstEightBytesFromBootLoader(uint flashStartAddress = 134217728)
    {
      byte[] dst = new byte[8];
      if (this.dicBytesToFlashBOOTLOADER.Count == 0)
        throw new Exception("ERROR: BOOTLOADER file empty or not loaded correctly.");
      if ((int) flashStartAddress != (int) this.iFlashStartAdr)
        this.iFlashStartAdr = flashStartAddress;
      if (!this.isBootLoaderFile)
        throw new Exception("Please, select a BOOTLOADER file first.");
      using (IEnumerator<uint> enumerator = this.dicBytesToFlashBOOTLOADER.Keys.Where<uint>((System.Func<uint, bool>) (x => x >= this.iFlashStartAdr)).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          Buffer.BlockCopy((Array) this.dicBytesToFlashBOOTLOADER[enumerator.Current], 0, (Array) dst, 0, 8);
          this.isFirstEightBytesLoaded = true;
        }
      }
      return dst;
    }

    public byte[] getFirstEightBytesFromFirmware(uint flashStartAddress = 134217728)
    {
      byte[] dst = new byte[8];
      if (this.dicBytesToFlashFIRMWARE.Count == 0)
        throw new Exception("ERROR: FIRMWARE file empty or not loaded correctly.");
      if ((int) flashStartAddress != (int) this.iFlashStartAdr)
        this.iFlashStartAdr = flashStartAddress;
      using (IEnumerator<uint> enumerator = this.dicBytesToFlashFIRMWARE.Keys.Where<uint>((System.Func<uint, bool>) (x => x >= this.iFlashStartAdr)).GetEnumerator())
      {
        if (enumerator.MoveNext())
          Buffer.BlockCopy((Array) this.dicBytesToFlashFIRMWARE[enumerator.Current], 0, (Array) dst, 0, 8);
      }
      return dst;
    }

    public byte[] getVectorTableFromFirmware(uint flashStartAddress = 134217728)
    {
      int length = 192;
      byte[] dst = new byte[length];
      if (this.dicBytesToFlashFIRMWARE.Count == 0)
        throw new Exception("ERROR: FIRMWARE file empty or not loaded correctly.");
      if ((int) flashStartAddress != (int) this.iFlashStartAdr)
        this.iFlashStartAdr = flashStartAddress;
      foreach (uint key in this.dicBytesToFlashFIRMWARE.Keys.Where<uint>((System.Func<uint, bool>) (x => x >= this.iFlashStartAdr)))
      {
        byte[] src = this.dicBytesToFlashFIRMWARE[key];
        Buffer.BlockCopy((Array) src, 0, (Array) dst, dst.Length, src.Length);
        if (dst.Length >= length)
          break;
      }
      return dst;
    }

    public async Task readVectorTableFromDevice(
      ProgressHandler progress,
      CancellationTokenSource token,
      uint flashStartAddress = 134217728)
    {
      byte[] numArray = await this.getVectorTableFromDevice(progress, token, flashStartAddress);
      this.baVectorInitTable = numArray;
      numArray = (byte[]) null;
    }

    public async Task<byte[]> getVectorTableFromDevice(
      ProgressHandler progress,
      CancellationTokenSource token,
      uint flashStartAddress = 134217728)
    {
      uint iSize = 192;
      byte[] ba = new byte[(int) iSize];
      ba = await this.fwUpdateToolCMDs.ReadMemoryAsync(progress, token.Token, flashStartAddress, iSize, (byte) 128);
      byte[] vectorTableFromDevice = ba;
      ba = (byte[]) null;
      return vectorTableFromDevice;
    }

    public async Task<byte[]> getFirstEightBytesFromDevice(
      ProgressHandler progress,
      CancellationTokenSource token,
      uint flashStartAddress = 134217728)
    {
      uint iSize = 8;
      byte[] ba = new byte[(int) iSize];
      ba = await this.fwUpdateToolCMDs.ReadMemoryAsync(progress, token.Token, flashStartAddress, iSize, (byte) 128);
      byte[] eightBytesFromDevice = ba;
      ba = (byte[]) null;
      return eightBytesFromDevice;
    }

    public async Task writeFirstEightBytesToFLASH(
      ProgressHandler progress,
      CancellationToken token,
      uint startAddress = 134217728)
    {
      await this.writeFirstEightBytesToFLASH(progress, token, this.baFirstEightBytesFromHexFile, startAddress);
    }

    public async Task writeFirstEightBytesToFLASH(
      ProgressHandler progress,
      CancellationToken token,
      byte[] bytesToWrite,
      uint startAddress = 134217728)
    {
      try
      {
        await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, token, startAddress, bytesToWrite);
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while writing first 8 byte to FLASH memory of connected device.", ex);
      }
    }

    public async Task writeVectorTableToFLASH(
      ProgressHandler progress,
      CancellationTokenSource token,
      uint startAddress = 134217728)
    {
      await this.writeVectorTableToFLASH(progress, token, this.baVectorInitTable, startAddress);
    }

    public async Task writeVectorTableToFLASH(
      ProgressHandler progress,
      CancellationTokenSource token,
      byte[] bytesToWrite,
      uint startAddress = 134217728)
    {
      try
      {
        if (this.baVectorInitTable.Length == 0)
          throw new Exception("Vector Init table was not loaded correctly, try again please.");
        if (this.isBSLrunning)
          await this.BTLFunctions.writeMemoryAsync(progress, token, startAddress, bytesToWrite);
        else
          await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, token.Token, startAddress, bytesToWrite);
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while writing vector table to FLASH memory of connected device.", ex);
      }
    }

    public bool isFirmwarecompatibleWithActualSelectedBootloader(out string errorMessage)
    {
      errorMessage = string.Empty;
      string str1 = string.Empty;
      bool flag1 = true;
      if (this.dicBytesToFlashFIRMWARE == null || this.dicBytesToFlashFIRMWARE != null && this.dicBytesToFlashFIRMWARE.Count < 1)
        throw new Exception("Please select a FIRMWARE file first...");
      if (this.dicBytesToFlashBOOTLOADER == null || this.dicBytesToFlashBOOTLOADER != null && this.dicBytesToFlashBOOTLOADER.Count < 1)
        throw new Exception("Please select a BOOTLOADER file first...");
      int num1 = 0;
      foreach (uint key in this.dicBytesToFlashBOOTLOADER.Keys)
        num1 += this.dicBytesToFlashBOOTLOADER[key].Length;
      int num2 = 0;
      foreach (uint key in this.dicBytesToFlashFIRMWARE.Keys)
        num2 += this.dicBytesToFlashFIRMWARE[key].Length;
      int num3 = (int) Math.Ceiling((double) (num2 / this.dicBytesToFlashFIRMWARE.Keys.Count)) + 1;
      ref string local1 = ref errorMessage;
      string str2 = errorMessage;
      uint num4 = this.dicBytesToFlashBOOTLOADER.Keys.First<uint>();
      string str3 = num4.ToString("X8");
      string str4 = str2 + "\rStartaddress of Bootloader: 0x" + str3;
      local1 = str4;
      errorMessage = errorMessage + ", Size: " + num1.ToString() + " Bytes.";
      ref string local2 = ref errorMessage;
      string str5 = errorMessage;
      num4 = this.dicBytesToFlashFIRMWARE.Keys.First<uint>();
      string str6 = num4.ToString("X8");
      string str7 = str5 + "\rStartaddress of Firmware: 0x" + str6;
      local2 = str7;
      errorMessage = errorMessage + ", Size: " + num2.ToString() + " Bytes.";
      bool flag2 = false;
      foreach (uint key1 in this.dicBytesToFlashBOOTLOADER.Keys)
      {
        for (uint key2 = key1; (long) key2 < (long) key1 + (long) num3; ++key2)
        {
          if (this.dicBytesToFlashFIRMWARE.Keys.Contains<uint>(key2))
          {
            byte[] numArray = this.dicBytesToFlashFIRMWARE[key2];
            for (int index = 0; index < numArray.Length; ++index)
            {
              if (numArray[index] != byte.MaxValue && numArray[index] > (byte) 0)
              {
                str1 = string.Empty;
                str1 = str1 + "\rFirmware overlaps with bootloader!!!\rAddress: " + key2.ToString("x4");
                errorMessage = str1;
                return false;
              }
              if (!flag2)
                str1 += "\rFirmware overlaps with bootloader but uses 0x00 or 0xFF !!!";
              flag2 = true;
            }
          }
        }
      }
      errorMessage = str1;
      return flag1;
    }

    public async Task verifyFirmwareCRCAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      this.bIsFirmwareVerifyError = false;
      if (this.blockInfo_Firmware == null || this.blockInfo_Firmware.Count <= 0)
        return;
      bool next = true;
      short step = 0;
      int maxBlockSize = 8192;
      FirmwareBlockInfoClass localFWBIC = (FirmwareBlockInfoClass) null;
      foreach (FirmwareBlockInfoClass fwBIC in this.blockInfo_Firmware)
      {
        next = true;
        do
        {
          if ((long) fwBIC.blockSize > (long) maxBlockSize)
          {
            localFWBIC = new FirmwareBlockInfoClass();
            uint addADRstart = fwBIC.startAddress + (uint) step * (uint) maxBlockSize;
            uint addADRende = (uint) ((int) fwBIC.startAddress + (int) ++step * maxBlockSize - 1);
            localFWBIC.startAddress = addADRstart;
            localFWBIC.endAddress = addADRende;
            if (localFWBIC.endAddress > fwBIC.endAddress)
            {
              localFWBIC.endAddress = fwBIC.endAddress;
              next = false;
            }
            if (localFWBIC.startAddress < localFWBIC.endAddress)
            {
              byte[] locBA = new byte[(int) localFWBIC.endAddress - (int) localFWBIC.startAddress];
              int offset = ((int) step - 1) * maxBlockSize;
              Buffer.BlockCopy((Array) fwBIC.BlockBytes, offset, (Array) locBA, 0, locBA.Length);
              localFWBIC.BlockBytes = locBA;
              localFWBIC.isLocal = true;
              locBA = (byte[]) null;
            }
            else
              break;
          }
          else
          {
            localFWBIC = fwBIC;
            localFWBIC.isLocal = false;
            next = false;
          }
          if (localFWBIC.startAddress > 134217983U && (this.blockInfo_NoVerify == null || this.blockInfo_NoVerify != null && !this.blockInfo_NoVerify.Contains(localFWBIC)))
          {
            ushort crc16 = 0;
            if (!localFWBIC.isLocal)
              crc16 = await this.fwUpdateToolCMDs.VerifyMemoryAsync(progress, cancel.Token, localFWBIC.startAddress, localFWBIC.endAddress);
            else
              crc16 = await this.fwUpdateToolCMDs.VerifyMemoryAsync(progress, cancel.Token, localFWBIC.startAddress, localFWBIC.endAddress - 1U);
            if (this.oFUI != null)
            {
              this.oFUI.setDebugInfo(" ... verify in block: 0x" + localFWBIC.startAddress.ToString("x8") + " - 0x" + localFWBIC.endAddress.ToString("x8"));
              this.oFUI.setDebugInfo(" ... device_CRC: 0x" + crc16.ToString("x4") + " / block_CRC: 0x" + localFWBIC.crc16_CCITT.ToString("x4"));
            }
            if ((int) crc16 != (int) localFWBIC.crc16_CCITT)
            {
              this.bIsFirmwareVerifyError = true;
              int num;
              if (this.oFUI != null)
              {
                bool? nullable = this.oFUI.ignoreError();
                bool flag = false;
                num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
              }
              else
                num = 0;
              if (num != 0)
                throw new Exception("verify error in firmware!!!");
            }
          }
          else if (localFWBIC.startAddress > 134217983U && (this.blockInfo_NoVerify == null || this.blockInfo_NoVerify != null && this.blockInfo_NoVerify.Contains(localFWBIC)))
          {
            if (this.oFUI != null)
              this.oFUI.setDebugInfo(" ... NO verify at: 0x" + localFWBIC.startAddress.ToString("x8") + " - 0x" + localFWBIC.endAddress.ToString("x8"));
          }
          else if (this.oFUI != null)
            this.oFUI.setDebugInfo(" NO verify in block: 0x" + localFWBIC.startAddress.ToString("x8") + " - 0x" + localFWBIC.endAddress.ToString("x8"));
        }
        while (next);
      }
      localFWBIC = (FirmwareBlockInfoClass) null;
    }

    public async Task verifyBootloaderCRCAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      this.bIsBootloaderVerifyError = false;
      if (this.blockInfo_Bootloader == null || this.blockInfo_Bootloader.Count <= 0)
        return;
      foreach (FirmwareBlockInfoClass fwBIC in this.blockInfo_Bootloader)
      {
        ushort num = await this.fwUpdateToolCMDs.VerifyMemoryAsync(progress, cancel.Token, fwBIC.startAddress, fwBIC.endAddress);
        uint crc16 = (uint) num;
        if ((int) crc16 != (int) fwBIC.crc16)
        {
          this.bIsBootloaderVerifyError = true;
          if (this.oFUI != null)
            this.oFUI.setErrorInfo("\r ... verify ERROR in block: " + fwBIC.startAddress.ToString("x8") + " - " + fwBIC.endAddress.ToString("x8"));
          int num1;
          if (this.oFUI != null)
          {
            bool? nullable = this.oFUI.ignoreError();
            bool flag = false;
            num1 = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
          }
          else
            num1 = 0;
          if (num1 != 0)
            throw new Exception("verify error for bootloader!!!");
        }
      }
    }

    public async Task verifyFirmwareAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      AddressRange noCheckRange = null)
    {
      List<AddressRange> adrLST = new List<AddressRange>()
      {
        noCheckRange
      };
      await this.verifyFirmwareAsync(progress, cancel, adrLST);
      adrLST = (List<AddressRange>) null;
    }

    public async Task verifyFirmwareAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      List<AddressRange> noCheckRanges = null)
    {
      int t_out = 0;
      int offset = 0;
      uint pageSize = this.verifyPageSize;
      uint extSize = 16;
      List<byte> byte2Read = new List<byte>();
      int loops = 0;
      uint uiAddress = 0;
      bool isNoCheck = false;
      int actualPageSize = 16;
      int normalPageSize = 16;
      bool isTakeBytes = true;
      bool readDONE = false;
      bool isGAP = false;
      uint oldAddress = 0;
      byte[] oldDicBuf = (byte[]) null;
      int iSplitSize = this.dicBytesToFlashFIRMWARE.Count * 16 / (int) pageSize;
      progress.Split(iSplitSize);
      this.bIsFirmwareVerifyError = false;
      int lineLen = 0;
      foreach (uint address in this.dicBytesToFlashFIRMWARE.Keys)
        lineLen += this.dicBytesToFlashFIRMWARE[address].Length;
      double tempNP = (double) (lineLen / this.dicBytesToFlashFIRMWARE.Keys.Count);
      normalPageSize = (int) Math.Ceiling(tempNP) + 1;
      foreach (uint address in this.dicBytesToFlashFIRMWARE.Keys)
      {
        this.writeNOW = false;
        actualPageSize = this.dicBytesToFlashFIRMWARE[address].Length;
        readDONE = false;
        if (oldDicBuf != null)
        {
          byte2Read.AddRange((IEnumerable<byte>) oldDicBuf);
          uiAddress = oldAddress;
        }
        if (oldAddress > 0U && (long) oldAddress + (long) actualPageSize < (long) address)
        {
          isGAP = true;
          oldDicBuf = new byte[actualPageSize];
        }
        else
        {
          isGAP = false;
          oldDicBuf = (byte[]) null;
        }
        if (address >= 134217728U && address < 134217920U && address <= 134217920U || noCheckRanges != null && this.checkAddressIsInAddressRanges(address, (uint) actualPageSize, noCheckRanges))
        {
          extSize += (uint) actualPageSize;
          isNoCheck = true;
          if (byte2Read.Count == 0)
          {
            oldAddress = address;
            extSize = (uint) normalPageSize;
            if (this.oFUI != null)
            {
              this.oFUI.setDebugInfo(" ... no check on this adress (" + address.ToString("x8") + ")...");
              continue;
            }
            continue;
          }
          oldAddress = address;
          extSize = (uint) byte2Read.Count;
          if (this.oFUI != null)
            this.oFUI.setDebugInfo(" ... no check on this adress (" + address.ToString("x8") + ")...");
        }
        else
        {
          isTakeBytes = true;
          AddressRange noCheckRange = this.getAddressRangeFromRanges(address, noCheckRanges);
          if (noCheckRange != null && address < noCheckRange.StartAddress && (long) address + (long) actualPageSize >= (long) noCheckRange.StartAddress)
            actualPageSize = (int) noCheckRange.StartAddress - (int) address;
          else if (noCheckRange != null && address < noCheckRange.EndAddress && (long) address + (long) actualPageSize > (long) noCheckRange.EndAddress)
          {
            actualPageSize -= (int) ((long) address + (long) actualPageSize - (long) noCheckRange.EndAddress);
            isTakeBytes = false;
          }
          else if (this.BSL_AddressRange != null && address < this.BSL_AddressRange.StartAddress && (long) address + (long) actualPageSize >= (long) this.BSL_AddressRange.StartAddress)
            actualPageSize = (int) this.BSL_AddressRange.StartAddress - (int) address;
          else if (this.BSL_AddressRange != null && address <= this.BSL_AddressRange.EndAddress && (long) address + (long) actualPageSize > (long) this.BSL_AddressRange.EndAddress)
          {
            actualPageSize -= (int) ((long) address + (long) actualPageSize - (long) this.BSL_AddressRange.EndAddress);
            isTakeBytes = false;
          }
          isNoCheck = false;
          noCheckRange = (AddressRange) null;
        }
        if (byte2Read.Count == 0)
          uiAddress = address;
        byte[] dicBuf = this.dicBytesToFlashFIRMWARE[address];
        if (oldDicBuf != null)
          oldDicBuf = dicBuf;
        if (actualPageSize != normalPageSize)
        {
          if (isTakeBytes)
          {
            dicBuf = ((IEnumerable<byte>) this.dicBytesToFlashFIRMWARE[address]).Take<byte>(actualPageSize).ToArray<byte>();
          }
          else
          {
            dicBuf = ((IEnumerable<byte>) this.dicBytesToFlashFIRMWARE[address]).Skip<byte>(actualPageSize).ToArray<byte>();
            uiAddress = (uint) ((ulong) address + (ulong) actualPageSize + 1UL);
          }
          this.writeNOW = true;
        }
        if (dicBuf.Length < normalPageSize)
          offset = normalPageSize - dicBuf.Length;
        if (!isNoCheck && !isGAP)
          byte2Read.AddRange((IEnumerable<byte>) dicBuf);
        if ((long) byte2Read.Count == (long) pageSize || (long) byte2Read.Count + (long) extSize > (long) pageSize || loops == this.dicBytesToFlashFIRMWARE.Count || byte2Read.Count > 0 & isNoCheck || this.writeNOW || byte2Read.Count > 0 & isGAP)
        {
          uint pSize = (uint) byte2Read.Count;
          byte[] pBuffer = new byte[(int) pSize];
          byte[] locBuf = new byte[(int) pSize];
          locBuf = byte2Read.ToArray<byte>();
          if (pSize > 0U)
          {
            while (!readDONE)
            {
              try
              {
                pBuffer = await this.BSL_readFromDeviceAsync(progress, cancel, uiAddress, pSize);
                readDONE = true;
                t_out = 0;
              }
              catch (Exception ex)
              {
                if (t_out > 3)
                {
                  if (this.oFUI != null)
                    this.oFUI.setErrorInfo(" ... reading ERROR (TIMEOUT) ... adr: " + uiAddress.ToString("x8") + " - size: " + pSize.ToString());
                  throw new Exception("Timeout\n" + ex.Message);
                }
                ++t_out;
                if (this.oFUI != null)
                {
                  this.oFUI.setDebugInfo(" ... reading ERROR at address: " + uiAddress.ToString("x8"));
                  this.oFUI.setDebugInfo(" -> read again - Attempt(" + t_out.ToString() + ")!!! ");
                  bool? nullable = this.oFUI.ignoreError();
                  bool flag = true;
                  if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
                  {
                    this.oFUI.setErrorInfo("-E-> verify error at address: " + uiAddress.ToString("x8"));
                    this.oFUI.setErrorInfo("-E-> " + ex.Message);
                  }
                }
                readDONE = false;
              }
              finally
              {
                extSize = (uint) normalPageSize;
                byte2Read.Clear();
              }
            }
            if (!((IEnumerable<byte>) pBuffer).SequenceEqual<byte>((IEnumerable<byte>) locBuf))
            {
              pBuffer = ((IEnumerable<byte>) pBuffer).Take<byte>(pBuffer.Length - offset).ToArray<byte>();
              locBuf = ((IEnumerable<byte>) locBuf).Take<byte>(locBuf.Length - offset).ToArray<byte>();
              if (locBuf.Length != 0 && pBuffer.Length != 0)
              {
                int pos = 0;
                while (pos < locBuf.Length && pos < pBuffer.Length && (int) locBuf[pos] == (int) pBuffer[pos])
                  ++pos;
                bool allNullAndEmpty = this.isByteArrayNULLorEmpty(pBuffer) && this.isByteArrayNULLorEmpty(locBuf);
                if (!allNullAndEmpty && !((IEnumerable<byte>) pBuffer).SequenceEqual<byte>((IEnumerable<byte>) locBuf))
                {
                  if (this.oFUI != null)
                    this.oFUI.setDebugInfo(" ... VERIFY ERROR ... ");
                  bool? nullable;
                  int num1;
                  if (this.oFUI != null)
                  {
                    nullable = this.oFUI.ignoreError();
                    bool flag = true;
                    num1 = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
                  }
                  else
                    num1 = 0;
                  long num2;
                  if (num1 != 0)
                    this.oFUI.setDebugInfo("-E-> Firmware verify error at address: 0x" + ((long) uiAddress + (long) pos).ToString("x8"));
                  else if (this.oFUI != null)
                  {
                    IZDebugInfo oFui = this.oFUI;
                    num2 = (long) uiAddress + (long) pos;
                    string txt = " VERIFY mismatch at (0x" + num2.ToString("x8") + ") ... ";
                    oFui.setDebugInfo(txt);
                    this.oFUI.setDebugInfo(" Buffer file:   " + ZR_ClassLibrary.Util.ByteArrayToHexString(locBuf));
                    this.oFUI.setDebugInfo(" Buffer device: " + ZR_ClassLibrary.Util.ByteArrayToHexString(pBuffer));
                  }
                  this.bIsFirmwareVerifyError = true;
                  int num3;
                  if (this.oFUI != null)
                  {
                    nullable = this.oFUI.ignoreError();
                    bool flag = true;
                    num3 = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
                  }
                  else
                    num3 = 0;
                  if (num3 != 0)
                  {
                    num2 = (long) uiAddress + (long) pos;
                    throw new Exception("Firmware verify error at address: 0x" + num2.ToString("x8"));
                  }
                  num2 = (long) uiAddress + (long) pos;
                  throw new Exception("Firmware verify error at address: 0x" + num2.ToString("x8"));
                }
              }
            }
            else if (this.oFUI != null)
              this.oFUI.setDebugInfo(" ... verify of adr: 0x" + uiAddress.ToString("x8") + " - size: " + pSize.ToString() + " was OK.");
            if (isNoCheck)
              offset = 0;
            progress.Report("verifying Adress: 0x" + uiAddress.ToString("x8"));
          }
          pBuffer = (byte[]) null;
          locBuf = (byte[]) null;
        }
        oldAddress = address;
        ++loops;
        dicBuf = (byte[]) null;
      }
      byte2Read = (List<byte>) null;
      oldDicBuf = (byte[]) null;
    }

    private bool isByteArrayNULLorEmpty(byte[] local)
    {
      bool flag = false;
      foreach (byte num in local)
        flag = num == (byte) 0 || num == byte.MaxValue;
      return flag;
    }

    private AddressRange getAddressRangeFromRanges(uint address, List<AddressRange> noCheckRanges)
    {
      if (noCheckRanges != null)
      {
        foreach (AddressRange noCheckRange in noCheckRanges)
        {
          if (address >= noCheckRange.StartAddress && address <= noCheckRange.EndAddress)
            return noCheckRange;
        }
      }
      return (AddressRange) null;
    }

    private bool checkAddressIsInAddressRanges(
      uint address,
      uint actualPageSize,
      List<AddressRange> adrRanges)
    {
      bool flag = false;
      foreach (AddressRange adrRange in adrRanges)
      {
        if (address >= adrRange.StartAddress && address <= adrRange.EndAddress || this.BSL_AddressRange != null && address >= this.BSL_AddressRange.StartAddress && address <= this.BSL_AddressRange.EndAddress)
          return true;
      }
      return flag;
    }

    public async Task verifyBootLoaderAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      AddressRange noCheckRange = null,
      uint pageSize = 512)
    {
      int t_out = 0;
      uint bSize = pageSize;
      List<byte> byte2Write = new List<byte>();
      int loops = 0;
      uint Address = 0;
      uint lastAddress = this.BSL_AddressRange.EndAddress;
      Stopwatch watch = new Stopwatch();
      Stopwatch watchDur = new Stopwatch();
      watch.Start();
      foreach (uint adress in this.dicBytesToFlashBOOTLOADER.Keys)
      {
        if (noCheckRange == null || adress < noCheckRange.StartAddress && adress > noCheckRange.EndAddress)
        {
          watchDur.Restart();
          if (byte2Write.Count == 0)
            Address = adress;
          byte[] dicBuf = this.dicBytesToFlashBOOTLOADER[adress];
          byte2Write.AddRange((IEnumerable<byte>) dicBuf);
          if ((long) byte2Write.Count == (long) pageSize || loops == this.dicBytesToFlashBOOTLOADER.Count || lastAddress <= Address + pageSize)
          {
            bSize = (uint) byte2Write.Count;
            byte[] pBuffer = new byte[(int) bSize];
            byte[] locBuf = new byte[(int) bSize];
            locBuf = byte2Write.ToArray<byte>();
            bool? nullable;
            try
            {
              pBuffer = await this.fwUpdateToolCMDs.ReadMemoryAsync(progress, cancel.Token, Address, bSize);
            }
            catch (Exception ex)
            {
              if (t_out > 3)
              {
                int num;
                if (this.oFUI != null)
                {
                  nullable = this.oFUI.ignoreError();
                  bool flag = true;
                  num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
                }
                else
                  num = 0;
                if (num != 0)
                  throw new Exception("Timeout\n" + ex.Message);
              }
              ++t_out;
              if (this.oFUI != null)
              {
                this.oFUI.setErrorInfo("-E-> verify error at address: " + Address.ToString("x8"));
                this.oFUI.setErrorInfo("-E-> read attempt: " + t_out.ToString());
              }
            }
            if (!((IEnumerable<byte>) pBuffer).SequenceEqual<byte>((IEnumerable<byte>) locBuf))
            {
              int num;
              if (this.oFUI != null)
              {
                nullable = this.oFUI.ignoreError();
                bool flag = true;
                num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
              }
              else
                num = 0;
              if (num != 0)
                throw new Exception("Bootloader verify error at address: " + Address.ToString("x8"));
              this.oFUI.setErrorInfo("-E-> verify mismatch at address: " + Address.ToString("x8"));
            }
            else if (this.oFUI != null)
              this.oFUI.setDebugInfo("... verify at address: " + Address.ToString("x8") + " / Bytes: " + bSize.ToString() + " .. OK.");
            progress.Report("verifying Adress: " + Address.ToString("x8"));
            if (this.oFUI != null)
              this.oFUI.setDebugInfo("verify time: " + watchDur.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
            byte2Write.Clear();
            pBuffer = (byte[]) null;
            locBuf = (byte[]) null;
          }
          ++loops;
          dicBuf = (byte[]) null;
        }
      }
      if (this.oFUI == null)
      {
        byte2Write = (List<byte>) null;
        watch = (Stopwatch) null;
        watchDur = (Stopwatch) null;
      }
      else
      {
        this.oFUI.setDebugInfo("verify complete time: " + watch.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
        byte2Write = (List<byte>) null;
        watch = (Stopwatch) null;
        watchDur = (Stopwatch) null;
      }
    }

    public async Task writeBootLoaderToDevice128kb(
      ProgressHandler progress,
      CancellationToken token)
    {
      uint startAddress = 0;
      uint iPageSize = 128;
      int offset = 0;
      int loops = 0;
      byte[] bytesToWrite = new byte[(int) iPageSize];
      byte[] byteBlock = new byte[0];
      try
      {
        if (!this.isBootLoaderFile)
          throw new Exception("No BOOTLOADER file was loaded !!!");
        int iSplitSize = this.dicBytesToFlashBOOTLOADER.Count * 16 / (int) iPageSize * 2;
        progress.Split(iSplitSize);
        Stopwatch watch = new Stopwatch();
        Stopwatch watchDur = new Stopwatch();
        watch.Start();
        foreach (uint address in this.dicBytesToFlashBOOTLOADER.Keys)
        {
          watchDur.Restart();
          if (offset == 0)
            startAddress = address;
          Buffer.BlockCopy((Array) this.dicBytesToFlashBOOTLOADER[address], 0, (Array) bytesToWrite, offset, this.dicBytesToFlashBOOTLOADER[address].Length);
          offset += this.dicBytesToFlashBOOTLOADER[address].Length;
          ++loops;
          if ((long) offset == (long) iPageSize || this.dicBytesToFlashBOOTLOADER.Count == loops)
          {
            byte[] bootBytes = new byte[offset];
            Buffer.BlockCopy((Array) bytesToWrite, 0, (Array) bootBytes, 0, offset);
            string debugInfo = "Write at adress (" + startAddress.ToString("x8") + ") - " + bootBytes.Length.ToString() + " Bytes in " + watch.Elapsed.TotalSeconds.ToString("000.00") + " sec.";
            progress.Report(debugInfo);
            await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, token, startAddress, bootBytes);
            if (this.oFUI != null)
              this.oFUI.setDebugInfo("writing BOOTLOADER ... (" + startAddress.ToString("x8") + " - " + ((long) startAddress + (long) bootBytes.Length).ToString("x8") + ") - time:" + watchDur.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
            offset = 0;
            bytesToWrite = new byte[(int) iPageSize];
            bootBytes = (byte[]) null;
            debugInfo = (string) null;
          }
        }
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("writing BOOTLOADER took" + watch.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
        watchDur.Stop();
        watch.Stop();
        watch = (Stopwatch) null;
        watchDur = (Stopwatch) null;
        bytesToWrite = (byte[]) null;
        byteBlock = (byte[]) null;
      }
      catch (Exception ex)
      {
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("-E-> verify error at address: " + startAddress.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
        {
          bytesToWrite = (byte[]) null;
          byteBlock = (byte[]) null;
        }
        else
          throw new Exception("writeBootLoaderToDevice128kb:\nStartaddress: " + startAddress.ToString("x8") + "\nBytes: " + ZR_ClassLibrary.Util.ByteArrayToHexString(bytesToWrite) + "\nMessage: " + ex.Message);
      }
    }

    public async Task doSystemResetOverRegister(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      uint startAdress = 3758157068;
      byte[] value = BitConverter.GetBytes(100270084);
      await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, cancel.Token, startAdress, value);
      value = (byte[]) null;
    }

    public async Task doSystemResetFunction(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      await this.fwUpdateToolCMDs.ResetDeviceAsync(progress, cancel.Token);
      this.isReady2EraseFW = true;
    }

    public long revealMapIDForBSLFirmwareFromDatabase(out string allowedFW, uint firmwareVersion = 0)
    {
      long num = 0;
      allowedFW = string.Empty;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string str = "SELECT * from ProgFiles " + (firmwareVersion > 0U ? "where (FirmwareVersion = " + firmwareVersion.ToString() + ") and HardwareName in ('Bootloader') " : "where HardwareName in ('Bootloader') ") + " order by FirmwareVersion desc";
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(str.ToString(), newConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter.Fill((DataTable) progFilesDataTable);
          HardwareTypeTables.HardwareOverviewDataTable overviewDataTable = new HardwareTypeTables.HardwareOverviewDataTable();
          if (progFilesDataTable.Count > 0)
          {
            for (int index = 0; index < 1; ++index)
            {
              num = (long) progFilesDataTable[index].MapID;
              if (!progFilesDataTable[index].IsFirmwareDependenciesNull())
                allowedFW = progFilesDataTable[index].FirmwareDependencies;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while revealing mapid from database... ", ex);
      }
      return num;
    }

    public long revealHardwareTypeMapIDForBSLFirmwareFromDatabase(
      out string allowedFW,
      uint firmwareVersion = 0)
    {
      long num = 0;
      allowedFW = string.Empty;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          string str = "SELECT * from ProgFiles " + (firmwareVersion > 0U ? "where (FirmwareVersion = " + firmwareVersion.ToString() + ") and HardwareName in ('Bootloader') " : "where HardwareName in ('Bootloader') ") + " order by FirmwareVersion desc";
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(str.ToString(), newConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter.Fill((DataTable) progFilesDataTable);
          HardwareTypeTables.HardwareOverviewDataTable overviewDataTable = new HardwareTypeTables.HardwareOverviewDataTable();
          if (progFilesDataTable.Count > 0)
          {
            for (int index = 0; index < 1; ++index)
            {
              num = (long) progFilesDataTable[index].HardwareTypeMapID;
              if (!progFilesDataTable[index].IsFirmwareDependenciesNull())
                allowedFW = progFilesDataTable[index].FirmwareDependencies;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while revealing mapid from database... ", ex);
      }
      return num;
    }

    public uint revealMapIDForHardwareTypeFromDatabase(uint HardwareTypID)
    {
      string empty = string.Empty;
      if (HardwareTypID == 0U)
        throw new Exception("HardwareName is not set correctly.");
      uint hardwareTypeTable;
      try
      {
        hardwareTypeTable = HardwareTypeSupport.GetMapIDForHardwareTypeFromHardwareTypeTable(HardwareTypID);
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while loading BootLoader data from database", ex);
      }
      return hardwareTypeTable;
    }

    public List<FirmwareReleaseInfo> revealAllPossibleFirmwareForBootloaderFromDatabase(
      uint firmwareVersionBSL = 0)
    {
      return this.revealLatestFirmwareForBootloaderFromDatabase(firmwareVersionBSL, false, true);
    }

    public List<FirmwareReleaseInfo> revealLatestFirmwareForBootloaderFromDatabase(
      uint firmwareVersionBSL = 0,
      bool latest = true,
      bool allFiles = false)
    {
      if (firmwareVersionBSL == 0U)
        throw new Exception("FirmwareVersion is not set correctly");
      List<FirmwareReleaseInfo> firmwareReleaseInfoList = new List<FirmwareReleaseInfo>();
      DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT * from Progfiles WHERE FirmwareVersion = " + firmwareVersionBSL.ToString());
      DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable1 = new HardwareTypeTables.ProgFilesDataTable();
      dataAdapter1.Fill((DataTable) progFilesDataTable1);
      string str1 = progFilesDataTable1[0].FirmwareDependencies.Replace(';', ',');
      string str2 = string.Empty;
      string str3 = str1;
      char[] chArray = new char[1]{ ',' };
      foreach (string s in ((IEnumerable<string>) str3.Split(chArray)).ToList<string>())
      {
        uint result = 0;
        if (!string.IsNullOrEmpty(s))
        {
          uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result);
          str2 = str2 + result.ToString() + ",";
        }
      }
      stringBuilder.Clear();
      string str4 = str2.Substring(0, str2.Length - 1);
      stringBuilder.Append(" SELECT * from Progfiles ");
      stringBuilder.Append(" WHERE ProgFileName IS NOT NULL ");
      if (!allFiles)
      {
        stringBuilder.Append(" and ( ReleasedName LIKE '%Latest%' ");
        if (!latest)
          stringBuilder.Append("  OR ReleasedName lIKE '%Released%' ");
        stringBuilder.Append(" ) ");
      }
      stringBuilder.Append(" and FirmwareVersion IN (" + str4 + ") ORDER BY mapid desc");
      DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable2 = new HardwareTypeTables.ProgFilesDataTable();
      dataAdapter2.Fill((DataTable) progFilesDataTable2);
      foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable2)
      {
        string str5 = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
        string str6 = (string) null;
        if (!progFilesRow.IsReleaseCommentsNull())
          str6 = progFilesRow.ReleaseComments;
        firmwareReleaseInfoList.Add(new FirmwareReleaseInfo()
        {
          MapID = progFilesRow.MapID,
          ProgFilName = progFilesRow.ProgFileName,
          ReleaseText = progFilesRow.ReleasedName,
          FirmwareVersion = progFilesRow.FirmwareVersion,
          FirmwareVersionString = str5,
          ReleaseDescription = str6
        });
      }
      return firmwareReleaseInfoList;
    }

    public SortedList<uint, HardwareTypeTables.ProgFilesRow> revealAllPossibleFirmwareFromDatabase(
      uint firmwareVersion = 0,
      string hardwareName = "",
      string allPossibleFW4BSL = "")
    {
      uint num1 = firmwareVersion != 0U ? (uint) (ushort) (firmwareVersion >> 12 & 4095U) : 0U;
      SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList = new SortedList<uint, HardwareTypeTables.ProgFilesRow>((IComparer<uint>) new DescComparer<uint>());
      FirmwareVersion firmwareVersion1 = new FirmwareVersion(firmwareVersion);
      string str1 = string.Empty;
      if (!string.IsNullOrEmpty(allPossibleFW4BSL))
      {
        int num2 = 0;
        string str2 = allPossibleFW4BSL;
        char[] chArray = new char[1]{ ';' };
        foreach (string str3 in ((IEnumerable<string>) str2.Split(chArray)).ToList<string>())
        {
          if (!string.IsNullOrEmpty(str3.Trim()))
          {
            uint uint32 = Convert.ToUInt32(str3.Trim(), 16);
            str1 = str1 + (num2 == 0 ? "" : ", ") + uint32.ToString();
            ++num2;
          }
        }
      }
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT MapID, ProgFileName, options,sourceinfo, hardwarename, hardwaretypemapid, firmwareversion, releasedname,compatibleoverwritegroups, releasecomments, firmwaredependencies FROM Progfiles WHERE HardwareName not IN ('Bootloader') AND HexText IS NOT NULL AND ProgFileName IS NOT NULL " + (string.IsNullOrEmpty(str1) ? " " : " AND FirmwareVersion IN (" + str1 + ") ") + " ORDER BY MapID, FirmwareVersion DESC");
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter.Fill((DataTable) progFilesDataTable);
          List<FirmwareReleaseInfo> firmwareReleaseInfoList = new List<FirmwareReleaseInfo>();
          if (progFilesDataTable.Count > 0)
          {
            int num3 = 0;
            bool flag = true;
            while (flag)
            {
              for (int index = 0; index < progFilesDataTable.Count; ++index)
              {
                HardwareTypeTables.ProgFilesRow progFilesRow = progFilesDataTable.NewProgFilesRow();
                progFilesRow.HardwareName = progFilesDataTable[index].HardwareName;
                if (string.IsNullOrEmpty(hardwareName) || progFilesRow.HardwareName.Contains(hardwareName))
                {
                  progFilesRow.FirmwareVersion = progFilesDataTable[index].FirmwareVersion;
                  progFilesRow.HardwareTypeMapID = progFilesDataTable[index].HardwareTypeMapID;
                  progFilesRow.MapID = progFilesDataTable[index].MapID;
                  if (!progFilesDataTable[index].IsFirmwareDependenciesNull())
                    progFilesRow.FirmwareDependencies = progFilesDataTable[index].FirmwareDependencies;
                  if (!progFilesDataTable[index].IsProgFileNameNull())
                    progFilesRow.ProgFileName = progFilesDataTable[index].ProgFileName;
                  if (!progFilesDataTable[index].IsOptionsNull())
                    progFilesRow.Options = progFilesDataTable[index].Options;
                  if (!progFilesDataTable[index].IsReleasedNameNull())
                    progFilesRow.ReleasedName = progFilesDataTable[index].ReleasedName;
                  if (!progFilesDataTable[index].IsReleaseCommentsNull())
                    progFilesRow.ReleaseComments = progFilesDataTable[index].ReleaseComments;
                  if (!sortedList.ContainsKey((uint) index))
                    sortedList.Add((uint) index, progFilesRow);
                }
              }
              flag = sortedList.Count == 0;
              if (flag && num3 < 2)
                hardwareName = string.Empty;
              else if (flag && num3 >= 2)
                flag = false;
              ++num3;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while loading firmware data from database", ex);
      }
      return sortedList;
    }

    public List<FirmwareReleaseInfo> revealAllPossibleFirmwareFromDatabase(uint hardwareTypeMapID)
    {
      List<FirmwareReleaseInfo> firmwareReleaseInfoList = new List<FirmwareReleaseInfo>();
      List<FirmwareReleaseInfo> infoForHardwareType;
      try
      {
        infoForHardwareType = HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfoForHardwareType((int) hardwareTypeMapID);
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while loading firmware data from database", ex);
      }
      return infoForHardwareType;
    }

    public SortedList<uint, HardwareTypeTables.ProgFilesRow> revealLatestBootloaderForVersionFromDatabase(
      uint firmwareVersion = 0,
      string hardwareName = "Bootloader")
    {
      return this.revealAllPossibleBootloaderFromDatabase(firmwareVersion, hardwareName, true);
    }

    public SortedList<uint, HardwareTypeTables.ProgFilesRow> revealAllPossibleBootloaderFromDatabase(
      uint firmwareVersion = 0,
      string hardwareName = "Bootloader",
      bool getBootloader = false)
    {
      if (firmwareVersion == 0U)
        throw new Exception("No Device connected, please connect to a device and read Version first !!!");
      if (string.IsNullOrEmpty(hardwareName))
        throw new Exception("HardwareName is not set correctly.");
      this.availBootloaderProgFiles = new SortedList<uint, HardwareTypeTables.ProgFilesRow>();
      ushort num = (ushort) (firmwareVersion & 4095U);
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT * from Progfiles  where HardwareName like '%" + hardwareName + "%' order by FirmwareVersion desc");
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter.Fill((DataTable) progFilesDataTable);
          if (progFilesDataTable.Count > 0)
          {
            for (int index = 0; index < progFilesDataTable.Count; ++index)
            {
              HardwareTypeTables.ProgFilesRow progFilesRow = progFilesDataTable.NewProgFilesRow();
              progFilesRow.HardwareName = progFilesDataTable[index].HardwareName;
              progFilesRow.FirmwareVersion = progFilesDataTable[index].FirmwareVersion;
              progFilesRow.HardwareTypeMapID = progFilesDataTable[index].HardwareTypeMapID;
              progFilesRow.MapID = progFilesDataTable[index].MapID;
              if (!progFilesDataTable[index].IsProgFileNameNull())
                progFilesRow.ProgFileName = progFilesDataTable[index].ProgFileName;
              if (!progFilesDataTable[index].IsOptionsNull())
                progFilesRow.Options = progFilesDataTable[index].Options;
              if (!progFilesDataTable[index].IsHexTextNull())
                progFilesRow.HexText = progFilesDataTable[index].HexText;
              if (!progFilesDataTable[index].IsFirmwareDependenciesNull())
                progFilesRow.FirmwareDependencies = progFilesDataTable[index].FirmwareDependencies;
              string str = firmwareVersion.ToString("x8");
              if (!getBootloader)
              {
                if (!progFilesRow.IsFirmwareDependenciesNull() && progFilesRow.FirmwareDependencies.Contains(str))
                  this.availBootloaderProgFiles.Add((uint) index, progFilesRow);
              }
              else if ((long) progFilesRow.FirmwareVersion == (long) firmwareVersion)
                this.availBootloaderProgFiles.Add((uint) index, progFilesRow);
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while loading BootLoader data from database", ex);
      }
      return this.availBootloaderProgFiles;
    }

    public SortedList<uint, FirmwareReleaseInfo> revealAllBootloaderFromDatabase(uint FWVersion = 0)
    {
      if (FWVersion == 0U)
        throw new Exception("Firmware is not set correctly.");
      SortedList<uint, FirmwareReleaseInfo> sortedList = new SortedList<uint, FirmwareReleaseInfo>();
      List<FirmwareReleaseInfo> firmwareReleaseInfoList = new List<FirmwareReleaseInfo>();
      try
      {
        foreach (FirmwareReleaseInfo firmwareReleaseInfo in HardwareTypeSupport.GetBslFirmwareReleaseInfosForFirmwareVersion(FWVersion))
          sortedList.Add((uint) firmwareReleaseInfo.MapID, firmwareReleaseInfo);
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while loading BootLoader data from database", ex);
      }
      return sortedList;
    }

    public SortedDictionary<uint, byte[]> getDictionaryWithFirmware(bool getBootLoaderFirmware = false)
    {
      return getBootLoaderFirmware ? this.dicBytesToFlashBOOTLOADER : this.dicBytesToFlashFIRMWARE;
    }

    public bool loadFirmwareFromString(string locString, bool checkForBootloader = true)
    {
      try
      {
        this.strFirmwareFile = locString;
        this.dicBytesToFlashFIRMWARE = ProgFileTools.parseINTEL_HEX_file(this.strFirmwareFile);
        if (checkForBootloader)
        {
          if (!this.checkForBootLoaderFW(this.dicBytesToFlashFIRMWARE))
            this.blockInfo_Firmware = this.getBlocksFromFirmware();
          else
            this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        }
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public bool loadFirmwareFromFile(string FileName, bool checkForBootloader = true)
    {
      this.strFileName = FileName;
      try
      {
        TextReader textReader = (TextReader) File.OpenText(this.strFileName);
        this.strFirmwareFile = textReader.ReadToEnd();
        this.dicBytesToFlashFIRMWARE = ProgFileTools.parseINTEL_HEX_file(this.strFirmwareFile);
        if (checkForBootloader)
        {
          if (!this.checkForBootLoaderFW(this.dicBytesToFlashFIRMWARE))
            this.blockInfo_Firmware = this.getBlocksFromFirmware();
          else
            this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        }
        textReader.Close();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public string loadFirmwareFromFile(bool checkForBootloader = true)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "HEX files (*.hex)|*.hex|txt files (*.txt)|*.txt|All files (*.*)|*.*";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return (string) null;
      this.strFileName = openFileDialog.FileName;
      if (this.strFileName == string.Empty)
        return (string) null;
      try
      {
        TextReader textReader = (TextReader) File.OpenText(this.strFileName);
        this.strFirmwareFile = textReader.ReadToEnd();
        this.dicBytesToFlashFIRMWARE = ProgFileTools.parseINTEL_HEX_file(this.strFirmwareFile);
        if (checkForBootloader)
        {
          if (!this.checkForBootLoaderFW(this.dicBytesToFlashFIRMWARE))
            this.blockInfo_Firmware = this.getBlocksFromFirmware();
          else
            this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        }
        textReader.Close();
        return this.strFileName;
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while reading firmware file", ex);
      }
    }

    public string loadFirmwareFromFileToMerge()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "HEX files (*.hex)|*.hex|txt files (*.txt)|*.txt|All files (*.*)|*.*";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return (string) null;
      string fileName = openFileDialog.FileName;
      if (fileName == string.Empty)
        return (string) null;
      try
      {
        TextReader textReader = (TextReader) File.OpenText(fileName);
        this.dicBytesToFlashMERGE = ProgFileTools.parseINTEL_HEX_file(textReader.ReadToEnd());
        textReader.Close();
        return fileName;
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while reading firmware file", ex);
      }
    }

    public void loadFirmwareFileFromDB(uint mapID = 0, bool parseFile = true, AddressRange excludeAddresses = null)
    {
      this.firmwareFileData = mapID != 0U ? HardwareTypeSupport.GetFirmwareData(mapID).ProgrammerFileAsString : throw new Exception("MapID is not set correct for loading from DataBase...");
      if (!parseFile)
        return;
      this.dicBytesToFlashFIRMWARE = ProgFileTools.parseINTEL_HEX_file(this.firmwareFileData);
      if (!this.checkForBootLoaderFW(this.dicBytesToFlashFIRMWARE))
        this.blockInfo_Firmware = this.getBlocksFromFirmware(excludeAddresses: excludeAddresses);
      else
        this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
    }

    public string loadFirmwareFileFromDBasString(uint mapID = 0)
    {
      return mapID != 0U ? HardwareTypeSupport.GetFirmwareData(mapID).ProgrammerFileAsString : throw new Exception("MapID is not set correct for loading from DataBase...");
    }

    public SortedDictionary<uint, byte[]> loadFirmwareFileFromDBasSortedDictionary(
      uint mapID = 0,
      bool parseFile = true)
    {
      SortedDictionary<uint, byte[]> sortedDictionary = new SortedDictionary<uint, byte[]>();
      FirmwareData firmwareData = mapID != 0U ? HardwareTypeSupport.GetFirmwareData(mapID) : throw new Exception("MapID is not set correct for loading from DataBase...");
      if (parseFile)
        sortedDictionary = ProgFileTools.parseINTEL_HEX_file(firmwareData.ProgrammerFileAsString);
      return sortedDictionary;
    }

    public void setLoadedBootloader(FirmwareData bootloaderFWData)
    {
      this.dataOfBootloader = bootloaderFWData;
    }

    public FirmwareData getLoadedBootloader()
    {
      if (this.dataOfBootloader != null)
      {
        this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.dataOfBootloader.ProgrammerFileAsString);
        if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
          this.blockInfo_Firmware = this.getBlocksFromFirmware();
        else
          this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
      }
      return this.dataOfBootloader;
    }

    public FirmwareData loadNewestBootloaderFileFromDB(uint actualFWVersion, uint newFWVersion)
    {
      if (actualFWVersion == 0U)
        throw new Exception("Actual Firmware Version is not set correct, please connect to a device first...");
      this.dataOfBootloader = newFWVersion != 0U ? HardwareTypeSupport.GetNewestBSLFromProgFilesTable(actualFWVersion, newFWVersion) : throw new Exception("New Firmware Version is not set correct, please select a correct firmware file first...");
      if (this.dataOfBootloader != null)
      {
        this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.dataOfBootloader.ProgrammerFileAsString);
        if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
        {
          this.blockInfo_Firmware = this.getBlocksFromFirmware();
        }
        else
        {
          this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
          this.ListOfLoadedBootloader.Add(this.dataOfBootloader);
        }
      }
      return this.dataOfBootloader;
    }

    public string loadThatBootloaderFileFromDB(uint mapID, bool store = false)
    {
      bool flag = false;
      if (mapID <= 0U)
        throw new Exception("No BootLoader selected, please select BSL first ...");
      if (this.ListOfLoadedBootloader.Count > 0)
      {
        foreach (FirmwareData firmwareData in this.ListOfLoadedBootloader)
        {
          if (firmwareData.Options.Contains(new KeyValuePair<string, string>("MapID", mapID.ToString())))
          {
            this.dataOfBootloader = firmwareData;
            flag = true;
          }
        }
      }
      if (!flag)
      {
        this.dataOfBootloader = HardwareTypeSupport.GetFirmwareData(mapID);
        if (this.dataOfBootloader == null)
          throw new Exception("Error while loading bootloader file from database.");
      }
      this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.dataOfBootloader.ProgrammerFileAsString);
      if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
      {
        this.blockInfo_Firmware = this.getBlocksFromFirmware();
      }
      else
      {
        this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        if (!flag)
          this.ListOfLoadedBootloader.Add(this.dataOfBootloader);
      }
      return this.dataOfBootloader.ProgFileName;
    }

    public bool loadBootloaderFromFile(string FileName)
    {
      this.strFileName = FileName;
      try
      {
        TextReader textReader = (TextReader) File.OpenText(this.strFileName);
        this.strBootloaderFile = textReader.ReadToEnd();
        this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.strBootloaderFile);
        if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
          this.blockInfo_Firmware = this.getBlocksFromFirmware();
        else
          this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        textReader.Close();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public void loadBootloaderFromFile()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "HEX files (*.hex)|*.hex|txt files (*.txt)|*.txt|All files (*.*)|*.*";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.strFileName = openFileDialog.FileName;
      if (this.strFileName == string.Empty)
        return;
      try
      {
        TextReader textReader = (TextReader) File.OpenText(this.strFileName);
        this.strBootloaderFile = textReader.ReadToEnd();
        this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.strBootloaderFile);
        if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
          this.blockInfo_Firmware = this.getBlocksFromFirmware();
        else
          this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
        textReader.Close();
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR while reading firmware file", ex);
      }
    }

    public bool loadBootloaderFromstring(string locString)
    {
      try
      {
        this.strBootloaderFile = locString;
        this.dicBytesToFlashBOOTLOADER = ProgFileTools.parseINTEL_HEX_file(this.strBootloaderFile);
        if (!this.checkForBootLoaderFW(this.dicBytesToFlashBOOTLOADER))
          this.blockInfo_Firmware = this.getBlocksFromFirmware();
        else
          this.blockInfo_Bootloader = this.getBlocksFromFirmware(true);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public bool checkBootloaderFileName(string name)
    {
      if (string.IsNullOrEmpty(name) || !name.Contains("_V0x"))
        return false;
      try
      {
        this.BSLversion = new FirmwareVersion(this.getFirmwareVersionFromFilename(name));
        return true;
      }
      catch
      {
        return false;
      }
    }

    public uint getFirmwareVersionFromFilename(string filename)
    {
      if (string.IsNullOrEmpty(filename) || !filename.Contains("_V0x"))
        return 0;
      string str = filename.Substring(filename.IndexOf("_V0x") + 2, 10);
      try
      {
        return Convert.ToUInt32(str, 16);
      }
      catch
      {
        return 0;
      }
    }

    public bool checkForBootLoaderFW(SortedDictionary<uint, byte[]> dicFlashBytes)
    {
      int num1 = 0;
      bool flag = false;
      uint startAddress = 0;
      uint num2 = 0;
      this.dicBytesToFlash = dicFlashBytes;
      foreach (uint key in this.dicBytesToFlash.Keys)
      {
        if (!flag)
        {
          if (num1 == 0 && (key >= 134217984U && key < 134742016U || key >= 536870912U))
          {
            this.iBootLoaderStartAddress = key;
            this.isBootLoaderFile = true;
            this.isFirmWareFile = false;
          }
          else if (num1 == 0)
          {
            startAddress = key;
            this.isFirmWareFile = true;
            this.isBootLoaderFile = false;
          }
          flag = true;
          ++num1;
        }
        else if (flag && !this.isFirmWareFile)
        {
          if (this.isBootLoaderFile)
            this.iBootLoaderEndAddress = key + (uint) this.dicBytesToFlashBOOTLOADER[key].Length;
          if (this.isFirmWareFile)
            num2 = key + (uint) this.dicBytesToFlashFIRMWARE[key].Length;
        }
      }
      if (this.isBootLoaderFile)
        this.BSL_AddressRange = new AddressRange(this.iBootLoaderStartAddress, this.iBootLoaderEndAddress - this.iBootLoaderStartAddress);
      else
        this.FW_AddressRange = new AddressRange(startAddress, num2 - startAddress);
      return this.isBootLoaderFile;
    }

    public async Task<byte[]> BSL_readFromDeviceAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint startAddress,
      uint size)
    {
      byte[] numArray = await this.BTLFunctions.readMemoryAsync(progress, cancel, startAddress, size);
      return numArray;
    }

    public async Task<uint> BSL_getVersion(ProgressHandler progress, CancellationTokenSource cancel)
    {
      byte[] baVersion = await this.BTLFunctions.getVersionAsync(progress, cancel);
      this.isReady2EraseFW = true;
      this.isBSLrunning = true;
      uint fw = BitConverter.ToUInt32(baVersion, 0);
      this.BSLversion = new FirmwareVersion(fw);
      uint version = fw;
      baVersion = (byte[]) null;
      return version;
    }

    public async Task<ushort> BSL_getFLASHSize(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      bool readFromBSL = true)
    {
      uint adrFlashSize = 536346748;
      byte[] baSize = new byte[2];
      if (readFromBSL)
        baSize = await this.BTLFunctions.readMemoryAsync(progress, cancel, adrFlashSize, 2U);
      else
        baSize = await this.fwUpdateToolCMDs.ReadMemoryAsync(progress, cancel.Token, adrFlashSize, 2U);
      ushort uint16 = BitConverter.ToUInt16(baSize, 0);
      baSize = (byte[]) null;
      return uint16;
    }

    public async Task<byte[]> BSL_getARM_ID(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      bool readFromBSL = true)
    {
      uint adrARM_ID = this.ArmIdRange.StartAddress;
      uint size = this.ArmIdRange.ByteSize;
      byte[] baSize = new byte[(int) size];
      if (readFromBSL)
        baSize = await this.BTLFunctions.readMemoryAsync(progress, cancel, adrARM_ID, size);
      else
        baSize = await this.fwUpdateToolCMDs.ReadMemoryAsync(progress, cancel.Token, adrARM_ID, size);
      byte[] id = DeviceIdentification.GetArmUniqueID(baSize);
      byte[] armId = id;
      baSize = (byte[]) null;
      id = (byte[]) null;
      return armId;
    }

    public async Task<uint?> getMeterIDFromARM_ID(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      byte[] armUID)
    {
      uint? meterID = new uint?();
      if (armUID != null)
      {
        MeterUniqueIdByARM oMeterArmUID = new MeterUniqueIdByARM(DbBasis.PrimaryDB.BaseDbConnection);
        int num = await Task.Run<bool>((Func<bool>) (() => oMeterArmUID.ManageMeterID(armUID, ref meterID, true))) ? 1 : 0;
      }
      return meterID;
    }

    public async Task<uint?> createMeterIDFromARM_ID(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      byte[] armUID)
    {
      uint? meterID = new uint?();
      if (armUID != null)
      {
        MeterUniqueIdByARM oMeterArmUID = new MeterUniqueIdByARM(DbBasis.PrimaryDB.BaseDbConnection);
        int num = await Task.Run<bool>((Func<bool>) (() => oMeterArmUID.ManageMeterID(armUID, ref meterID))) ? 1 : 0;
      }
      return meterID;
    }

    public async Task BSL_eraseOldFW(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint StartAddresse = 134217728)
    {
      try
      {
        if (!this.isReady2EraseFW)
          throw new Exception("Device is not ready to write firmware to device...");
        uint pageSize = 128;
        ushort flashSize = await this.BSL_getFLASHSize(progress, cancel);
        uint loops = (uint) ((ulong) ((int) flashSize * 1024) / (ulong) pageSize);
        progress.Reset();
        progress.Split((int) loops);
        int num;
        for (int i = 0; (long) i < (long) loops; num = i++)
        {
          bool eraseNext = false;
          int tries = 0;
          while (!eraseNext)
          {
            try
            {
              num = i + 1;
              string message = "erasing Page " + num.ToString() + " of " + loops.ToString();
              progress.Report(message);
              await this.BTLFunctions.eraseMemoryAsync(progress, cancel, StartAddresse, pageSize);
              if (this.oFUI != null)
                this.oFUI.setDebugInfo("\r... Erasing address (" + StartAddresse.ToString("x8") + ")");
              StartAddresse += pageSize;
              eraseNext = true;
              message = (string) null;
            }
            catch (Exception ex)
            {
              if (tries++ > 3)
                throw new Exception("ERROR:" + ex.Message);
            }
          }
        }
        this.isReady2WriteFW = true;
      }
      catch (Exception ex)
      {
        this.isReady2WriteFW = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("\r-E-> Error erasing old firmware at address: " + StartAddresse.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          throw new Exception("Error while erasing firmware from device.\nAddress: " + StartAddresse.ToString("x8") + "\nMessage: " + ex.Message);
        if (this.oFUI == null)
          return;
        this.oFUI.setErrorInfo("\r-E-> Error erasing old firmware at address: " + StartAddresse.ToString("x8"));
      }
    }

    public async Task BSL_eraseOldFW_FAST(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint StartAddresse = 134217728)
    {
      try
      {
        if (!this.isReady2EraseFW)
          throw new Exception("Device is not ready to write firmware to device...");
        ushort flashSize = await this.BSL_getFLASHSize(progress, cancel);
        flashSize = (ushort) ((int) flashSize * 4096 - 1);
        uint FLASH_StartAdress = StartAddresse;
        uint BSL_StartAdress = this.BSL_AddressRange.StartAddress;
        uint BSL_EndAdress = this.BSL_AddressRange.EndAddress;
        uint pageSize = BSL_StartAdress - StartAddresse;
        progress.Reset();
        progress.Split(2);
        string message = "erasing FLASH starting at " + StartAddresse.ToString("x08") + " Bytes: " + pageSize.ToString();
        progress.Report(message);
        await this.BTLFunctions.eraseMemoryAsync(progress, cancel, StartAddresse, pageSize);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo("... erasing FLASH starting at " + StartAddresse.ToString("x08") + " Bytes: " + pageSize.ToString() + ") ... Done.");
        if (BSL_EndAdress < FLASH_StartAdress + (uint) flashSize)
        {
          StartAddresse = BSL_EndAdress + 1U;
          pageSize = (uint) ((int) FLASH_StartAdress + (int) flashSize - (int) StartAddresse + 1);
          message = "erasing FLASH starting at " + StartAddresse.ToString("x08") + " Bytes: " + pageSize.ToString();
          progress.Report(message);
          await this.BTLFunctions.eraseMemoryAsync(progress, cancel, StartAddresse, pageSize);
          if (this.oFUI != null)
            this.oFUI.setDebugInfo("... erasing FLASH starting at " + StartAddresse.ToString("x08") + " Bytes: " + pageSize.ToString() + ") ... Done.");
        }
        this.isReady2WriteFW = true;
        message = (string) null;
      }
      catch (Exception ex)
      {
        this.isReady2WriteFW = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("-E-> Error erasing old firmware at address: " + StartAddresse.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          throw new Exception("Error while erasing firmware from device.\nAddress: " + StartAddresse.ToString("x8") + "\nMessage: " + ex.Message);
        if (this.oFUI == null)
          return;
        this.oFUI.setErrorInfo("-E-> Error erasing old firmware at address: " + StartAddresse.ToString("x8"));
      }
    }

    public bool BSL_isIUW() => this.BTLFunctions.isIUW;

    public async Task BSL_writeNewFW(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      AddressRange[] noWriteRanges = null,
      uint pageSize = 448)
    {
      Stopwatch watch = new Stopwatch();
      Stopwatch watchDur = new Stopwatch();
      this.isReady2WriteFW = true;
      if (!this.isReady2WriteFW)
        throw new Exception("Old firmware was not erased!!!\n\nPlease, erase old firmware first.");
      if (this.dicBytesToFlashFIRMWARE == null || this.dicBytesToFlashFIRMWARE.Count == 0)
        throw new Exception("Firmware file was not loaded correctly.\nPlease load firmware file again.");
      if (pageSize % 4U > 0U)
        throw new Exception("PageSize not set correctly, needs to be a modulo 4!!!.\nPlease set a correct size for writing firmware file to device.");
      this.dicBytesToFlash = this.dicBytesToFlashFIRMWARE;
      uint startAddress = 0;
      uint startAddressOLD = 0;
      uint oldAddress = 0;
      uint iPageSize = pageSize;
      int iByteArrayLen = 0;
      int adrOffset = 0;
      int offset = 0;
      bool nextPage = true;
      byte[] bytesToWrite = new byte[(int) iPageSize];
      byte[] byteBlock = new byte[0];
      List<byte> tempBTW = new List<byte>();
      uint loop = 0;
      progress.Reset();
      int split = (int) ((long) (this.dicBytesToFlash.Count * 16) / (long) iPageSize);
      progress.Split(split);
      watch.Reset();
      watch.Start();
      try
      {
        foreach (uint address in this.dicBytesToFlash.Keys)
        {
          ++loop;
          if (iByteArrayLen == 0)
            iByteArrayLen = this.dicBytesToFlash[address].Length;
          if (this.BSL_AddressRange != null && address >= this.BSL_AddressRange.StartAddress && address <= this.BSL_AddressRange.EndAddress)
          {
            string debugInfo2_1 = "NO write at address (" + startAddress.ToString("x8") + ") - BOOTLOADER area !!!";
          }
          else
          {
            if (nextPage)
            {
              startAddress = adrOffset != 0 ? (uint) ((ulong) oldAddress + (ulong) adrOffset) : address;
              nextPage = false;
              if (startAddress % 4U > 0U)
                startAddress = startAddressOLD + iPageSize;
            }
            int num;
            TimeSpan elapsed;
            double totalSeconds;
            if ((long) (address - oldAddress) <= (long) iByteArrayLen || oldAddress == 0U)
            {
              if (byteBlock != null && (long) (byteBlock.Length + tempBTW.Count) > (long) iPageSize)
              {
                int itake = (int) ((long) byteBlock.Length - ((long) (byteBlock.Length + tempBTW.Count) - (long) iPageSize));
                tempBTW.AddRange(((IEnumerable<byte>) byteBlock).Take<byte>(itake));
                byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>(itake).ToArray<byte>();
                Buffer.BlockCopy((Array) this.dicBytesToFlash[address], 0, (Array) byteBlock, byteBlock.Length, this.dicBytesToFlash[address].Length);
              }
              else
              {
                if (byteBlock != null && byteBlock.Length != 0)
                {
                  tempBTW.AddRange((IEnumerable<byte>) byteBlock);
                  byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>(byteBlock.Length).ToArray<byte>();
                }
                int iLocalLenght = this.dicBytesToFlash[address].Length;
                if (iLocalLenght != iByteArrayLen || (long) (iLocalLenght + tempBTW.Count) > (long) iPageSize)
                {
                  if (byteBlock.Length != 0 && (long) (tempBTW.Count + byteBlock.Length) > (long) iPageSize)
                  {
                    uint iOff = (uint) ((ulong) iPageSize - (ulong) tempBTW.Count);
                    byte[] bta = new byte[(int) iOff];
                    Buffer.BlockCopy((Array) byteBlock, 0, (Array) bta, 0, (int) iOff);
                    tempBTW.AddRange((IEnumerable<byte>) bta);
                    byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>((int) iOff).ToArray<byte>();
                    offset = iByteArrayLen - (int) iOff;
                    bta = (byte[]) null;
                  }
                  else if (byteBlock != null && byteBlock.Length == 0 && (long) (iLocalLenght + tempBTW.Count) > (long) iPageSize)
                  {
                    offset = (int) iPageSize - tempBTW.Count;
                    adrOffset = offset;
                    tempBTW.AddRange(((IEnumerable<byte>) this.dicBytesToFlash[address]).Take<byte>(offset));
                    byteBlock = ((IEnumerable<byte>) this.dicBytesToFlash[address]).Skip<byte>(offset).ToArray<byte>();
                  }
                  else if (byteBlock != null && byteBlock.Length == 0 && (long) (iLocalLenght + tempBTW.Count) <= (long) iPageSize)
                    tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
                }
                else
                  tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
              }
            }
            else
            {
              int addCnt = tempBTW.Count % 4;
              if (addCnt != 0)
              {
                List<byte> addByte = new List<byte>();
                for (int i = 0; i < addCnt; num = i++)
                  addByte.Add(byte.MaxValue);
                Buffer.BlockCopy((Array) tempBTW.ToArray<byte>(), 0, (Array) bytesToWrite, 0, tempBTW.ToArray<byte>().Length);
                Buffer.BlockCopy((Array) addByte.ToArray<byte>(), 0, (Array) bytesToWrite, tempBTW.ToArray<byte>().Length + 1, addCnt);
                addByte = (List<byte>) null;
              }
              else
                bytesToWrite = tempBTW.ToArray<byte>();
              int doWrite = 0;
              while (true)
              {
                try
                {
                  await this.BTLFunctions.writeMemoryAsync(progress, cancel, startAddress, bytesToWrite);
                  break;
                }
                catch (Exception ex)
                {
                  if (ex.Message.ToLower().Contains("timeout") && doWrite < 3)
                    ++doWrite;
                  else
                    break;
                }
              }
              string[] strArray = new string[7]
              {
                "Write at adress (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray[3] = num.ToString();
              strArray[4] = " Bytes in ";
              elapsed = watchDur.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray[5] = totalSeconds.ToString("000.00");
              strArray[6] = " sec.";
              string debugInfo2_2 = string.Concat(strArray);
              if (this.oFUI != null)
                this.oFUI.setDebugInfo(debugInfo2_2);
              tempBTW.Clear();
              nextPage = false;
              offset = 0;
              startAddress = address;
              tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
              debugInfo2_2 = (string) null;
            }
            if ((long) tempBTW.Count >= (long) iPageSize || (long) loop == (long) this.dicBytesToFlash.Count)
            {
              string[] strArray1 = new string[7]
              {
                "Write at adress (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray1[3] = num.ToString();
              strArray1[4] = " Bytes in ";
              elapsed = watch.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray1[5] = totalSeconds.ToString("000.00");
              strArray1[6] = " sec.";
              string debugInfo = string.Concat(strArray1);
              progress.Report(debugInfo);
              watchDur.Restart();
              bytesToWrite = tempBTW.ToArray<byte>();
              int doWrite = 0;
              while (true)
              {
                try
                {
                  await this.BTLFunctions.writeMemoryAsync(progress, cancel, startAddress, bytesToWrite);
                  break;
                }
                catch (Exception ex)
                {
                  if (ex.Message.ToLower().Contains("timeout") && doWrite < 3)
                    ++doWrite;
                  else
                    break;
                }
              }
              string[] strArray2 = new string[7]
              {
                "Write at address (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray2[3] = num.ToString();
              strArray2[4] = " Bytes in ";
              elapsed = watchDur.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray2[5] = totalSeconds.ToString("000.00");
              strArray2[6] = " sec.";
              string debugInfo2_3 = string.Concat(strArray2);
              if (this.oFUI != null)
                this.oFUI.setDebugInfo(debugInfo2_3);
              tempBTW.Clear();
              startAddressOLD = startAddress;
              nextPage = true;
              debugInfo = (string) null;
              debugInfo2_3 = (string) null;
            }
            oldAddress = address;
          }
        }
        if (cancel.IsCancellationRequested)
          throw new Exception("Task cancelled by user !!!");
        this.isReady2GO = true;
        watch = (Stopwatch) null;
        watchDur = (Stopwatch) null;
        bytesToWrite = (byte[]) null;
        byteBlock = (byte[]) null;
        tempBTW = (List<byte>) null;
      }
      catch (Exception ex)
      {
        this.isReady2GO = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("-E-> write error at address: " + startAddress.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
        {
          watch = (Stopwatch) null;
          watchDur = (Stopwatch) null;
          bytesToWrite = (byte[]) null;
          byteBlock = (byte[]) null;
          tempBTW = (List<byte>) null;
        }
        else
          throw new Exception("Error while writing firmware to device.\nStartaddress: " + startAddress.ToString("x8") + "\nBytes:" + ZR_ClassLibrary.Util.ByteArrayToHexString(bytesToWrite) + "\nMessage: " + ex.Message);
      }
    }

    public async Task BSL_writeNewFW_IUW(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      AddressRange[] noWriteRanges = null,
      bool dontWriteAtBootloaderAddress = true)
    {
      Stopwatch watch = new Stopwatch();
      Stopwatch watchDur = new Stopwatch();
      this.isReady2WriteFW = true;
      if (!this.isReady2WriteFW)
        throw new Exception("Old firmware was not erased!!!\n\nPlease, erase old firmware first.");
      this.dicBytesToFlash = this.dicBytesToFlashFIRMWARE != null && this.dicBytesToFlashFIRMWARE.Count != 0 ? this.dicBytesToFlashFIRMWARE : throw new Exception("Firmware file was not loaded correctly.\nPlease load firmware file again.");
      uint startAddress = 0;
      uint startAddressOLD = 0;
      uint oldAdress = 0;
      uint iPageSize = 524287;
      int iByteArrayLen = 0;
      int adrOffset = 0;
      int offset = 0;
      bool nextPage = true;
      byte[] bytesToWrite = new byte[(int) iPageSize];
      byte[] byteBlock = new byte[0];
      List<byte> tempBTW = new List<byte>();
      uint loop = 0;
      progress.Reset();
      int split = this.dicBytesToFlash.Count * 16 / 448;
      progress.Split(split);
      watch.Reset();
      watch.Start();
      try
      {
        foreach (uint address in this.dicBytesToFlash.Keys)
        {
          ++loop;
          if (iByteArrayLen == 0)
            iByteArrayLen = this.dicBytesToFlash[address].Length;
          if (dontWriteAtBootloaderAddress && this.BSL_AddressRange != null && address >= this.BSL_AddressRange.StartAddress && address <= this.BSL_AddressRange.EndAddress)
          {
            if (this.oFUI != null)
              this.oFUI.setDebugInfo("NO write at address (" + startAddress.ToString("x8") + ") - BOOTLOADER area !!!");
          }
          else
          {
            if (nextPage)
            {
              startAddress = adrOffset != 0 ? (uint) ((ulong) oldAdress + (ulong) adrOffset) : address;
              nextPage = false;
              if (startAddress % 4U > 0U)
                startAddress = startAddressOLD + iPageSize;
            }
            int num;
            TimeSpan elapsed;
            double totalSeconds;
            if ((long) (address - oldAdress) <= (long) iByteArrayLen || oldAdress == 0U)
            {
              if (byteBlock != null && (long) (byteBlock.Length + tempBTW.Count) > (long) iPageSize)
              {
                int itake = (int) ((long) byteBlock.Length - ((long) (byteBlock.Length + tempBTW.Count) - (long) iPageSize));
                tempBTW.AddRange(((IEnumerable<byte>) byteBlock).Take<byte>(itake));
                byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>(itake).ToArray<byte>();
                Buffer.BlockCopy((Array) this.dicBytesToFlash[address], 0, (Array) byteBlock, byteBlock.Length, this.dicBytesToFlash[address].Length);
              }
              else
              {
                if (byteBlock != null && byteBlock.Length != 0)
                {
                  tempBTW.AddRange((IEnumerable<byte>) byteBlock);
                  byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>(byteBlock.Length).ToArray<byte>();
                }
                int iLocalLenght = this.dicBytesToFlash[address].Length;
                if (iLocalLenght != iByteArrayLen || (long) (iLocalLenght + tempBTW.Count) > (long) iPageSize)
                {
                  if (byteBlock.Length != 0 && (long) (tempBTW.Count + byteBlock.Length) > (long) iPageSize)
                  {
                    uint iOff = (uint) ((ulong) iPageSize - (ulong) tempBTW.Count);
                    byte[] bta = new byte[(int) iOff];
                    Buffer.BlockCopy((Array) byteBlock, 0, (Array) bta, 0, (int) iOff);
                    tempBTW.AddRange((IEnumerable<byte>) bta);
                    byteBlock = ((IEnumerable<byte>) byteBlock).Skip<byte>((int) iOff).ToArray<byte>();
                    offset = iByteArrayLen - (int) iOff;
                    bta = (byte[]) null;
                  }
                  else if (byteBlock != null && byteBlock.Length == 0 && (long) (iLocalLenght + tempBTW.Count) > (long) iPageSize)
                  {
                    offset = (int) iPageSize - tempBTW.Count;
                    adrOffset = offset;
                    tempBTW.AddRange(((IEnumerable<byte>) this.dicBytesToFlash[address]).Take<byte>(offset));
                    byteBlock = ((IEnumerable<byte>) this.dicBytesToFlash[address]).Skip<byte>(offset).ToArray<byte>();
                  }
                  else if (byteBlock != null && byteBlock.Length == 0 && (long) (iLocalLenght + tempBTW.Count) <= (long) iPageSize)
                    tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
                }
                else
                  tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
              }
            }
            else
            {
              int addCnt = tempBTW.Count % 4;
              if (addCnt != 0)
              {
                List<byte> addByte = new List<byte>();
                for (int i = 0; i < addCnt; num = i++)
                  addByte.Add(byte.MaxValue);
                Buffer.BlockCopy((Array) tempBTW.ToArray<byte>(), 0, (Array) bytesToWrite, 0, tempBTW.ToArray<byte>().Length);
                Buffer.BlockCopy((Array) addByte.ToArray<byte>(), 0, (Array) bytesToWrite, tempBTW.ToArray<byte>().Length + 1, addCnt);
                addByte = (List<byte>) null;
              }
              else
                bytesToWrite = tempBTW.ToArray<byte>();
              int doWrite = 0;
              while (true)
              {
                try
                {
                  await this.BTLFunctions.writeMemoryAsync(progress, cancel, startAddress, bytesToWrite);
                  break;
                }
                catch (Exception ex)
                {
                  if (ex.Message.ToLower().Contains("timeout") && doWrite < 3)
                  {
                    if (this.oFUI != null)
                      this.oFUI.setDebugInfo(" --> write at adress (" + startAddress.ToString("x8") + ") - timeout !!!");
                    ++doWrite;
                  }
                  else
                    break;
                }
              }
              string[] strArray = new string[7]
              {
                "Write at address (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray[3] = num.ToString();
              strArray[4] = " Bytes in ";
              elapsed = watchDur.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray[5] = totalSeconds.ToString("000.00");
              strArray[6] = " sec.";
              string debugInfo2 = string.Concat(strArray);
              if (this.oFUI != null)
                this.oFUI.setDebugInfo(debugInfo2);
              tempBTW.Clear();
              nextPage = false;
              offset = 0;
              startAddress = address;
              tempBTW.AddRange((IEnumerable<byte>) this.dicBytesToFlash[address]);
              debugInfo2 = (string) null;
            }
            if ((long) tempBTW.Count >= (long) iPageSize || (long) loop == (long) this.dicBytesToFlash.Count)
            {
              string[] strArray1 = new string[7]
              {
                "Write at address (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray1[3] = num.ToString();
              strArray1[4] = " Bytes in ";
              elapsed = watch.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray1[5] = totalSeconds.ToString("000.00");
              strArray1[6] = " sec.";
              string debugInfo = string.Concat(strArray1);
              progress.Report(debugInfo);
              watchDur.Restart();
              bytesToWrite = tempBTW.ToArray<byte>();
              int doWrite = 0;
              while (true)
              {
                try
                {
                  await this.BTLFunctions.writeMemoryAsync(progress, cancel, startAddress, bytesToWrite);
                  break;
                }
                catch (Exception ex)
                {
                  if (ex.Message.ToLower().Contains("timeout") && doWrite < 3)
                  {
                    if (this.oFUI != null)
                      this.oFUI.setDebugInfo(" --> write at address (" + startAddress.ToString("x8") + ") - timeout !!!");
                    ++doWrite;
                  }
                  else
                    break;
                }
              }
              string[] strArray2 = new string[7]
              {
                "Write at address (",
                startAddress.ToString("x8"),
                ") - ",
                null,
                null,
                null,
                null
              };
              num = tempBTW.Count;
              strArray2[3] = num.ToString();
              strArray2[4] = " Bytes in ";
              elapsed = watchDur.Elapsed;
              totalSeconds = elapsed.TotalSeconds;
              strArray2[5] = totalSeconds.ToString("000.00");
              strArray2[6] = " sec.";
              string debugInfo2 = string.Concat(strArray2);
              if (this.oFUI != null)
                this.oFUI.setDebugInfo(debugInfo2);
              tempBTW.Clear();
              startAddressOLD = startAddress;
              nextPage = true;
              debugInfo = (string) null;
              debugInfo2 = (string) null;
            }
            oldAdress = address;
          }
        }
        if (cancel.IsCancellationRequested)
          throw new Exception("Task cancelled by user !!!");
        this.isReady2GO = true;
        watch = (Stopwatch) null;
        watchDur = (Stopwatch) null;
        bytesToWrite = (byte[]) null;
        byteBlock = (byte[]) null;
        tempBTW = (List<byte>) null;
      }
      catch (Exception ex)
      {
        this.isReady2GO = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("-E-> write error at address: " + startAddress.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num == 0)
        {
          watch = (Stopwatch) null;
          watchDur = (Stopwatch) null;
          bytesToWrite = (byte[]) null;
          byteBlock = (byte[]) null;
          tempBTW = (List<byte>) null;
        }
        else
          throw new Exception("Error while writing firmware to device.\nStartaddress: " + startAddress.ToString("x8") + "\nBytes:" + ZR_ClassLibrary.Util.ByteArrayToHexString(bytesToWrite) + "\nMessage: " + ex.Message);
      }
    }

    public async Task BSL_writeNewFW_IUW_BLOCKMODE(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      AddressRange[] noWriteRanges = null)
    {
      Stopwatch watch = new Stopwatch();
      Stopwatch watchdur = new Stopwatch();
      this.isReady2GO = false;
      this.isReady2WriteFW = true;
      if (!this.isReady2WriteFW)
        throw new Exception("Old firmware was not erased!!!\n\nPlease, erase old firmware first.");
      this.dicBytesToFlash = this.dicBytesToFlashFIRMWARE != null && this.dicBytesToFlashFIRMWARE.Count != 0 ? this.dicBytesToFlashFIRMWARE : throw new Exception("Firmware file was not loaded correctly.\nPlease load firmware file again.");
      this.blockInfo_NoVerify = new List<FirmwareBlockInfoClass>();
      progress.Reset();
      watch.Start();
      if (this.blockInfo_Firmware == null || this.blockInfo_Firmware.Count <= 0)
      {
        watch = (Stopwatch) null;
        watchdur = (Stopwatch) null;
      }
      else
      {
        foreach (FirmwareBlockInfoClass fwBI in this.blockInfo_Firmware)
        {
          watchdur.Reset();
          watchdur.Start();
          int doWrite = 0;
          while (true)
          {
            try
            {
              if (this.oFUI != null)
                this.oFUI.setDebugInfo(" --> write at address (" + fwBI.startAddress.ToString("x8") + ") ");
              await this.BTLFunctions.writeMemoryAsync(progress, cancel, fwBI.startAddress, fwBI.BlockBytes);
              if (this.oFUI != null)
              {
                this.oFUI.setDebugInfo(" --> duration: " + watchdur.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              if (ex.Message.ToLower().Contains("timeout") && doWrite < 3)
              {
                if (this.oFUI != null)
                  this.oFUI.setDebugInfo(" --> write at address (" + fwBI.startAddress.ToString("x8") + ") - timeout !!!");
                ++doWrite;
              }
              else
              {
                if (ex.Message.ToUpper().Contains("NFC_ERR_WRTPRM"))
                {
                  this.blockInfo_NoVerify.Add(fwBI);
                  if (this.oFUI != null)
                  {
                    this.oFUI.setDebugInfo(" -!-> ERROR: " + ex.Message.Replace("\r", "").Replace("\n", "").Trim());
                    this.oFUI.setDebugInfo(" -!-> Block: " + fwBI.ToString(0U));
                    break;
                  }
                  break;
                }
                int num;
                if (this.oFUI != null)
                {
                  bool? nullable = this.oFUI.ignoreError();
                  bool flag = true;
                  num = nullable.GetValueOrDefault() == flag & nullable.HasValue ? 1 : 0;
                }
                else
                  num = 0;
                if (num == 0)
                  throw ex;
                this.oFUI.setDebugInfo(" --> ERROR: " + ex.Message);
                break;
              }
            }
          }
        }
        this.isReady2GO = true;
        if (this.oFUI != null)
          this.oFUI.setDebugInfo(" --> complete duration: " + watch.Elapsed.TotalSeconds.ToString("000.00") + " sec.");
        watch = (Stopwatch) null;
        watchdur = (Stopwatch) null;
      }
    }

    public List<FirmwareBlockInfoClass> getBlocksFromFirmware(
      bool isBootloader = false,
      AddressRange excludeAddresses = null)
    {
      Stopwatch stopwatch1 = new Stopwatch();
      Stopwatch stopwatch2 = new Stopwatch();
      if (isBootloader)
        this.dicBytesToFlash = this.dicBytesToFlashBOOTLOADER != null && this.dicBytesToFlashBOOTLOADER.Count != 0 ? this.dicBytesToFlashBOOTLOADER : throw new Exception("Bootloader file was not loaded correctly.\nPlease load bootloader file again.");
      else
        this.dicBytesToFlash = this.dicBytesToFlashFIRMWARE != null && this.dicBytesToFlashFIRMWARE.Count != 0 ? this.dicBytesToFlashFIRMWARE : throw new Exception("Firmware file was not loaded correctly.\nPlease load firmware file again.");
      List<FirmwareBlockInfoClass> blocksFromFirmware = new List<FirmwareBlockInfoClass>();
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 0;
      int num4 = 0;
      List<byte> byteList = (List<byte>) null;
      byte[] numArray = (byte[]) null;
      stopwatch1.Reset();
      stopwatch1.Start();
      try
      {
        FirmwareBlockInfoClass firmwareBlockInfoClass = (FirmwareBlockInfoClass) null;
        uint num5 = this.dicBytesToFlash.Keys.Last<uint>();
        foreach (uint key in this.dicBytesToFlash.Keys)
        {
          if (num4 == 0)
            num4 = this.dicBytesToFlash[key].Length;
          bool flag = false;
          if (num2 == 0U)
          {
            num1 = num3 > 0U ? num3 : key;
            firmwareBlockInfoClass = new FirmwareBlockInfoClass();
            firmwareBlockInfoClass.startAddress = num1;
            byteList = new List<byte>();
            if (numArray != null && numArray.Length != 0)
              byteList.AddRange((IEnumerable<byte>) numArray);
            numArray = (byte[]) null;
            num3 = 0U;
          }
          if ((long) (key - num2) == (long) num4 || num2 == 0U)
          {
            if (excludeAddresses != null)
            {
              if (key < excludeAddresses.StartAddress || key > excludeAddresses.EndAddress)
                byteList.AddRange((IEnumerable<byte>) this.dicBytesToFlash[key]);
            }
            else
              byteList.AddRange((IEnumerable<byte>) this.dicBytesToFlash[key]);
            flag = true;
          }
          if (num2 != 0U && (int) key != (int) num2 && (long) (key - num2) != (long) num4 || (int) key == (int) num5)
          {
            numArray = new byte[this.dicBytesToFlash[key].Length];
            Buffer.BlockCopy((Array) this.dicBytesToFlash[key], 0, (Array) numArray, 0, numArray.Length);
            num3 = key;
            firmwareBlockInfoClass.BlockBytes = byteList.ToArray();
            firmwareBlockInfoClass.blockSize = (uint) firmwareBlockInfoClass.BlockBytes.Length;
            firmwareBlockInfoClass.endAddress = (uint) ((int) firmwareBlockInfoClass.startAddress + (int) firmwareBlockInfoClass.blockSize - 1);
            blocksFromFirmware.Add(firmwareBlockInfoClass);
            num2 = 0U;
          }
          if (flag)
            num2 = key;
        }
      }
      catch (Exception ex)
      {
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("\r-E-> write error at address: " + num1.ToString("x8"));
        int num6;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num6 = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num6 = 0;
        if (num6 != 0)
          throw new Exception("Error while creating blocks for firmware.\nStartadress: " + num1.ToString("x8") + "\nMessage: " + ex.Message);
      }
      return blocksFromFirmware;
    }

    public async Task BSL_GO(ProgressHandler progress, CancellationTokenSource cancel)
    {
      if (!this.isReady2GO)
      {
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("\r-E-> Device is not prepared.");
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          throw new Exception("Device is not prepared!!!\n\nPlease, prepare, erase old and write new firmware first.");
      }
      await this.BTLFunctions.GO_Async(progress, cancel);
    }

    public async Task BSL_FORCE_GO(ProgressHandler progress, CancellationTokenSource cancel)
    {
      await this.BTLFunctions.GO_Async(progress, cancel);
    }

    public async Task<byte[]> BSL_readMeterKey(
      ProgressHandler progress,
      CancellationTokenSource cancelTokenSource)
    {
      if (!this.BSL_isIUW())
      {
        this.oFUI.setErrorInfo("\r-E-> Function for IUWs devices only !!!");
        return (byte[]) null;
      }
      byte[] numArray = await this.BTLFunctions.readMemoryAsync(progress, cancelTokenSource, this.iMeterKey, 4U);
      return numArray;
    }

    public async Task BSL_writeMeterKey(
      ProgressHandler progress,
      CancellationTokenSource cancelTokenSource,
      byte[] meterkey = null)
    {
      if (!this.BSL_isIUW())
      {
        this.oFUI.setErrorInfo("\r-E-> Function for IUWs devices only !!!");
      }
      else
      {
        if (meterkey == null)
          meterkey = new byte[4];
        await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, cancelTokenSource.Token, this.iMeterKey, meterkey);
      }
    }

    public async Task BSL_writeVectorTable(
      ProgressHandler progress,
      CancellationTokenSource cancelTokenSource,
      byte[] vector)
    {
      if (!this.BSL_isIUW())
      {
        this.oFUI.setErrorInfo("\r-E-> Function for IUWs devices only !!!");
      }
      else
      {
        if (vector == null || vector.Length > 8 || vector.Length < 8)
          throw new Exception("Wrong Vector for restarting firmware !!!");
        await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, cancelTokenSource.Token, this.iVectorTable, vector);
      }
    }

    public async Task deleteBSLfromDevice(ProgressHandler progress, CancellationTokenSource cancel)
    {
      await this.deleteBSLfromDevice(progress, cancel, 0U, 0U);
    }

    internal async Task deleteBSLfromDevice(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint startAdr,
      uint size)
    {
      try
      {
        if (!this.isBootLoaderFile)
          throw new Exception("Bootloader file is not loaded, please load a correct bootloader for this device...");
        if (startAdr == 0U)
        {
          startAdr = this.BSL_AddressRange.StartAddress;
          size = this.BSL_AddressRange.EndAddress - this.BSL_AddressRange.StartAddress;
        }
        uint pageSize = 128;
        ushort flashSize = (ushort) size;
        uint loops = (uint) flashSize / pageSize;
        byte[] bytesToWrite = new byte[128];
        for (int i = 0; i < bytesToWrite.Length; ++i)
          bytesToWrite[i] = byte.MaxValue;
        progress.Reset();
        progress.Split((int) loops);
        for (int i = 0; (long) i < (long) loops; ++i)
        {
          progress.Report("erasing Page " + (i + 1).ToString() + " of " + loops.ToString());
          await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, cancel.Token, startAdr, bytesToWrite);
          if (this.oFUI != null)
            this.oFUI.setDebugInfo("\r... erasing bootloader - adr: " + startAdr.ToString("x8") + " size: " + size.ToString());
          startAdr += pageSize;
        }
        bytesToWrite = (byte[]) null;
      }
      catch (Exception ex)
      {
        this.isReady2WriteFW = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("\r-E-> Error while erasing bootloader - adr: " + startAdr.ToString("x8") + " size: " + size.ToString());
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          throw new Exception("Error while deleting BOOTLOADER from device.\n\n" + ex.Message);
      }
    }

    public async Task<bool> overwriteBSLfromDevice(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      bool flag = await this.overwriteBSLfromDevice(progress, cancel, 0U);
      return flag;
    }

    internal async Task<bool> overwriteBSLfromDevice(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint startAddressBSL)
    {
      try
      {
        uint size = 0;
        uint pageSize = 128;
        uint startAdr = startAddressBSL;
        if (this.dicBytesToFlashBOOTLOADER == null || this.dicBytesToFlashBOOTLOADER.Count == 0)
          throw new Exception("Bootloader file is not loaded, please load a correct bootloader for this device...");
        if (this.dicBytesToFlashFIRMWARE == null || this.dicBytesToFlashFIRMWARE.Count == 0)
          throw new Exception("Firmware file is not loaded, please load a correct firmwarefile for this device...");
        if (startAdr == 0U)
          startAdr = this.BSL_AddressRange.StartAddress;
        if (startAdr < this.FW_AddressRange.EndAddress)
        {
          bool foundFWstartAdr = false;
          while (!foundFWstartAdr)
          {
            if (this.dicBytesToFlashFIRMWARE.ContainsKey(startAdr))
              foundFWstartAdr = true;
            else
              --startAdr;
          }
          size = this.BSL_AddressRange.EndAddress - this.BSL_AddressRange.StartAddress;
          uint loops = size / pageSize;
          progress.Reset();
          progress.Split((int) loops);
          while (startAdr <= this.BSL_AddressRange.EndAddress)
          {
            int offset = 0;
            bool isReady = false;
            byte[] bytesToWrite = new byte[(int) pageSize];
            while (!isReady)
            {
              int adressSize = this.dicBytesToFlashFIRMWARE[startAdr].Length;
              Buffer.BlockCopy((Array) this.dicBytesToFlashFIRMWARE[startAdr], 0, (Array) bytesToWrite, offset, adressSize);
              offset += adressSize;
              startAdr += (uint) adressSize;
              if (startAdr > this.BSL_AddressRange.EndAddress)
                isReady = true;
              if ((long) offset >= (long) pageSize)
                isReady = true;
            }
            await this.fwUpdateToolCMDs.WriteMemoryAsync(progress, cancel.Token, startAdr, bytesToWrite);
            if (this.oFUI != null)
              this.oFUI.setDebugInfo("\r... overwrite bootloader - adr: " + startAdr.ToString("x8") + " size: " + pageSize.ToString());
            bytesToWrite = (byte[]) null;
          }
          return true;
        }
        this.oFUI.setDebugInfo("\r... no data available for overwriting ... DONE.");
        return false;
      }
      catch (Exception ex)
      {
        this.isReady2WriteFW = false;
        if (this.oFUI != null)
          this.oFUI.setErrorInfo("\r-E-> Error while erasing bootloader - adr: " + startAddressBSL.ToString("x8"));
        int num;
        if (this.oFUI != null)
        {
          bool? nullable = this.oFUI.ignoreError();
          bool flag = true;
          num = !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          throw new Exception("Error while overwriting BOOTLOADER on device.\n\n" + ex.Message);
        return false;
      }
    }

    public string getInfoFromFirmwareFile(ref FirmwareInfoClass fwInfo, bool infoFromMergeFile = false)
    {
      SortedDictionary<uint, byte[]> sortedDictionary1 = new SortedDictionary<uint, byte[]>();
      SortedDictionary<uint, byte[]> sortedDictionary2 = this.dicBytesToFlashFIRMWARE;
      if (infoFromMergeFile)
        sortedDictionary2 = this.dicBytesToFlashMERGE;
      if (sortedDictionary2 == null || sortedDictionary2.Count <= 0)
        return (string) null;
      fwInfo.firstAddress = uint.MaxValue;
      fwInfo.lastAddress = 0U;
      fwInfo.stepSize = 0U;
      fwInfo.gapCounter = 0U;
      fwInfo.gapMessage = string.Empty;
      uint num1 = 0;
      foreach (uint key in sortedDictionary2.Keys)
      {
        fwInfo.firstAddress = key < fwInfo.firstAddress ? key : fwInfo.firstAddress;
        fwInfo.lastAddress = key > fwInfo.lastAddress ? key : fwInfo.lastAddress;
        uint num2 = key - fwInfo.oldKeyAdr;
        if (num1 > 0U && fwInfo.stepSize == 0U)
          fwInfo.stepSize = num2;
        if (num1 > 0U && fwInfo.stepSize > 0U)
          fwInfo.stepSize = (int) fwInfo.stepSize != (int) num2 ? fwInfo.stepSize : num2;
        if (num1 > 1U && (int) fwInfo.stepSize != (int) num2)
        {
          ++fwInfo.gapCounter;
          FirmwareInfoClass firmwareInfoClass = fwInfo;
          firmwareInfoClass.gapMessage = firmwareInfoClass.gapMessage + " -> GAP(" + fwInfo.gapCounter.ToString() + ") at 0x" + fwInfo.oldKeyAdr.ToString("x8") + " ... 0x" + key.ToString("x8") + "\r";
        }
        fwInfo.oldKeyAdr = key;
        ++num1;
      }
      return fwInfo.ToString();
    }

    public void createBackPack(FirmwareInfoClass fwFirmwareInfo)
    {
      uint length = (uint) this.dicBytesToFlashFIRMWARE[fwFirmwareInfo.lastAddress].Length;
      uint num1 = length % 4U;
      uint num2 = fwFirmwareInfo.lastAddress + length + num1;
      SortedDictionary<uint, byte[]> sortedDictionary = new SortedDictionary<uint, byte[]>();
      uint num3 = 0;
      foreach (uint key1 in this.dicBytesToFlashMERGE.Keys)
      {
        byte[] numArray = this.dicBytesToFlashMERGE[key1];
        uint num4 = num3 > 0U ? key1 - num3 : 0U;
        uint key2 = num2 + num4;
        num3 = key1;
        sortedDictionary.Add(key2, numArray);
        num2 = key2;
      }
      this.dicBytesToFlashBACKPACK = sortedDictionary;
    }

    public void mergeFirmwareAndBackPackFile()
    {
      foreach (uint key in this.dicBytesToFlashBACKPACK.Keys)
      {
        byte[] numArray = this.dicBytesToFlashBACKPACK[key];
        this.dicBytesToFlashFIRMWARE.Add(key, numArray);
      }
    }

    public void showBackUps(uint meterID)
    {
      this.zippedBackUpData = BackupWindow.ShowDialog((Window) null, new uint?(meterID));
    }

    public void loadLastBackupForDevice(uint meterID)
    {
      if (!this.myHandlerForProduction.LoadLastBackup((int) meterID))
        throw new Exception("unable to load Backup for meterid => " + meterID.ToString());
    }

    public void setBackupData()
    {
      if (this.zippedBackUpData != null && !this.myHandlerForProduction.SetBackup(this.zippedBackUpData))
        throw new Exception("Can not set backup to device !!!");
    }

    public async Task<bool> makeBackUp(ProgressHandler progress, CancellationToken token)
    {
      if (this.myHandlerForProduction == null)
        throw new Exception("Handler object was not set or deleted already!!!");
      if (this.oFUI != null)
        this.oFUI.setDebugInfo(" ... reading device for backup(DUMP) ...");
      this.myHandlerForProduction.MapCheckDisabled = true;
      int retVal = await this.myHandlerForProduction.ReadDeviceAsync(progress, token, ReadPartsSelection.Dump);
      this.deviceIdentificationForBackup = this.myHandlerForProduction.GetDeviceIdentification();
      if (this.oFUI != null)
      {
        this.oFUI.setDebugInfo(" ... DONE. ");
        this.oFUI.setDebugInfo(" ... write backup to database ...");
      }
      this.myHandlerForProduction.SaveMeterObject(HandlerMeterObjects.WorkMeter);
      this.myHandlerForProduction.SaveMeter();
      if (this.oFUI != null)
        this.oFUI.setDebugInfo(" ... DONE. ");
      return true;
    }

    public async Task<bool> writeBackUpToDevice(
      ProgressHandler progress,
      CancellationToken token,
      bool setBackDataExplizit = true)
    {
      bool overwriteOk = true;
      bool is_S4_Handler = this.myHandlerForProduction.ToString().Contains("S4_Handler");
      if (this.myHandlerForProduction == null)
        throw new Exception("Handler object was not set or deleted already!!!");
      if (this.oFUI != null)
        this.oFUI.setDebugInfo("\r...reading data from device to overwrite ...");
      int retVal = await this.myHandlerForProduction.ReadDeviceAsync(progress, token, ReadPartsSelection.AllWithoutLogger);
      if (this.zippedBackUpData != null & setBackDataExplizit)
        this.setBackupData();
      else if (this.zippedBackUpData == null & setBackDataExplizit)
        throw new Exception("Backup data not found !!!");
      if (this.oFUI != null)
        this.oFUI.setDebugInfo("...overwrite data ...");
      List<CommonOverwriteGroups> overwriteGroups;
      if (is_S4_Handler)
        overwriteGroups = new List<CommonOverwriteGroups>()
        {
          CommonOverwriteGroups.IdentData,
          CommonOverwriteGroups.ZeroFlowCalibration,
          CommonOverwriteGroups.RTC_Calibration,
          CommonOverwriteGroups.CarrierFrequencyCalibration,
          CommonOverwriteGroups.TemperatureSettings,
          CommonOverwriteGroups.TypeIdentification,
          CommonOverwriteGroups.UltrasonicHydraulicTestSetup,
          CommonOverwriteGroups.ConfigurationParameters,
          CommonOverwriteGroups.RamData,
          CommonOverwriteGroups.BasicConfiguration,
          CommonOverwriteGroups.DeviceProtection,
          CommonOverwriteGroups.MenuDefinition
        };
      else
        overwriteGroups = new List<CommonOverwriteGroups>()
        {
          CommonOverwriteGroups.IdentData
        };
      foreach (CommonOverwriteGroups group in overwriteGroups)
      {
        string actualGroup = string.Empty;
        try
        {
          CommonOverwriteGroups[] whatToOverwrite = new CommonOverwriteGroups[1]
          {
            group
          };
          if (this.oFUI != null)
          {
            actualGroup = group.ToString();
            this.oFUI.setDebugInfo(" --> Overwrite data group - " + actualGroup);
          }
          this.myHandlerForProduction.OverwriteSrcToDest(HandlerMeterObjects.BackupMeter, HandlerMeterObjects.WorkMeter, whatToOverwrite);
          whatToOverwrite = (CommonOverwriteGroups[]) null;
        }
        catch (Exception ex)
        {
          if (this.oFUI != null)
          {
            this.oFUI.setDebugInfo(" !!! ERROR while overriding group (" + actualGroup + ")");
            this.oFUI.setErrorInfo(" Origin Error: \r" + ex.Message);
          }
          overwriteOk = false;
        }
        actualGroup = (string) null;
      }
      if (this.oFUI != null)
        this.oFUI.setDebugInfo(" Overwrite data groups from backup ... DONE.");
      try
      {
        await this.myHandlerForProduction.WriteDeviceAsync(progress, token);
        if (this.oFUI != null)
          this.oFUI.setDebugInfo(" --> Backup successfully written!");
      }
      catch
      {
        if (this.oFUI != null)
          this.oFUI.setDebugInfo(" --> Error on write backup data to device!!!");
        overwriteOk = false;
      }
      if (is_S4_Handler)
      {
        try
        {
          await this.myHandlerForProduction.ResetDeviceAsync(progress, token);
          if (this.oFUI != null)
            this.oFUI.setDebugInfo(" --> Software reset done!");
        }
        catch
        {
          if (this.oFUI != null)
            this.oFUI.setDebugInfo(" --> Error on Software reset!!!");
          overwriteOk = false;
        }
      }
      bool device = overwriteOk;
      overwriteGroups = (List<CommonOverwriteGroups>) null;
      return device;
    }

    public string getBootloaderInfoString()
    {
      string bootloaderInfoString = string.Empty;
      if (this.blockInfo_Bootloader != null && this.dataOfBootloader != null)
      {
        string str = bootloaderInfoString + "\r-----------------------------------------------------" + "\r - Bootloader file: " + this.dataOfBootloader.ProgFileName + "\r - Blocks: " + this.blockInfo_Bootloader.Count.ToString();
        uint num = 1;
        foreach (FirmwareBlockInfoClass firmwareBlockInfoClass in this.blockInfo_Bootloader)
          str = str + "\r" + firmwareBlockInfoClass.ToString(num++);
        bootloaderInfoString = str + "\r-----------------------------------------------------";
      }
      return bootloaderInfoString;
    }

    public async Task<string> removeProtectionFromIUW(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      string retVal = string.Empty;
      if (this.myHandlerForProduction.IsProtected())
      {
        retVal += "\r-----------------------------------------------------";
        retVal += "\rThe device is write protected!";
        retVal += "\r --> Remove write protection: ";
        await this.myHandlerForProduction.ProtectionResetByDb(progress, cancelToken, DbBasis.PrimaryDB.BaseDbConnection);
        retVal += "OK";
      }
      string str = retVal;
      retVal = (string) null;
      return str;
    }

    public void setDeviceToUpdateBlock(DeviceIdentification devIdent)
    {
      if (this.myUpDateBlock == null)
        this.myUpDateBlock = new FirmwareUpdateFunctions.UpdateDataBlock();
      this.myUpDateBlock.deviceIdent = devIdent;
    }

    public async Task<bool> DoRunFirmwareUpdateInONE(
      ProgressHandler progress,
      CancellationTokenSource cancelTS)
    {
      bool retVal = false;
      if (this.myUpDateBlock == null)
      {
        this.oFUI.setDebugInfo("Please connect to a device first and update data!");
        return false;
      }
      string allowedFW = string.Empty;
      FirmwareUpdateFunctions.UpdateDataBlock updateDataBlock = this.myUpDateBlock;
      uint version = await this.BSL_getVersion(progress, cancelTS);
      updateDataBlock.BSL_Version = version;
      updateDataBlock = (FirmwareUpdateFunctions.UpdateDataBlock) null;
      if (this.myUpDateBlock.BSL_Version > 0U)
      {
        this.oFUI.setDebugInfo("This device is already in BOOTLOADER mode.");
        FirmwareVersion locFW = new FirmwareVersion(this.myUpDateBlock.BSL_Version);
        this.oFUI.setDebugInfo(" --> BOOTLOADER Version: " + locFW.Major.ToString() + "." + locFW.Minor.ToString() + " - " + locFW.TypeString + " for " + locFW.TypeBSLString);
        long mapid = this.revealMapIDForBSLFirmwareFromDatabase(out allowedFW, this.myUpDateBlock.BSL_Version);
        string bootLoaderFirmware = this.loadThatBootloaderFileFromDB((uint) mapid);
        this.getAllPossibleFWfromDB(allowedFW, true);
        this.oFUI.setDebugInfo("Choose a firmware to flash, now.");
        locFW = new FirmwareVersion();
        bootLoaderFirmware = (string) null;
      }
      return retVal;
    }

    private void getAllPossibleFWfromDB(
      string possibleFWforBootloader = "",
      bool isBootLoaderMode = false,
      bool isMinoBLE = false)
    {
      try
      {
        SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList;
        if (!isBootLoaderMode)
        {
          string hardwareName = this.myUpDateBlock.deviceIdent.FirmwareVersionObj.TypeString;
          switch (this.myUpDateBlock.deviceIdent.FirmwareVersionObj.Type)
          {
            case 18:
              hardwareName = "EDC_mBus";
              break;
            case 33:
              hardwareName = "EDC_ModBus";
              break;
          }
          sortedList = this.loadAllFirmwareFromDatabase(this.myUpDateBlock.deviceIdent.FirmwareVersion.Value, hardwareName, possibleFWforBootloader);
        }
        else
        {
          DeviceIdentification deviceIdentification = new DeviceIdentification(this.myUpDateBlock.BSL_Version);
          sortedList = this.loadAllFirmwareFromDatabase(deviceIdentification.FirmwareVersion.Value, deviceIdentification.FirmwareVersionObj.TypeBSLString, possibleFWforBootloader);
        }
        uint num = 0;
        if (sortedList.Count > 0)
        {
          List<int> intList = new List<int>();
          for (int index = 0; index < 3; ++index)
          {
            foreach (uint key in (IEnumerable<uint>) sortedList.Keys)
            {
              HardwareTypeTables.ProgFilesRow progFilesRow = sortedList[key];
              uint firmwareVersion1 = (uint) progFilesRow.FirmwareVersion;
              if ((!isMinoBLE || firmwareVersion1 > 50331721U) && (isMinoBLE || firmwareVersion1 < 50331721U))
              {
                if (firmwareVersion1 > num)
                  num = firmwareVersion1;
                FirmwareVersion firmwareVersion2 = new FirmwareVersion(firmwareVersion1);
                string str1 = ((IEnumerable<string>) progFilesRow.Options.Split(';')).SingleOrDefault<string>((System.Func<string, bool>) (x => x.ToLower().Contains("build")));
                string str2 = "FW: " + firmwareVersion2.VersionString + " / " + progFilesRow.HardwareName + (string.IsNullOrEmpty(str1) ? "" : " / " + str1);
                string str3 = (string) null;
                if (!progFilesRow.IsReleasedNameNull() && progFilesRow.ReleasedName.Length > 0)
                {
                  str3 = progFilesRow.ReleasedName;
                  str2 = str2 + " \"" + str3 + "\"";
                }
                switch (index)
                {
                  case 0:
                    if (str3 != null && str3.ToLower().Contains("released"))
                      break;
                    continue;
                  case 1:
                    if (str3 != null)
                      break;
                    continue;
                }
                string str4 = str2 + " - MAP: " + progFilesRow.MapID.ToString();
                if (!intList.Contains(progFilesRow.MapID))
                {
                  intList.Add(progFilesRow.MapID);
                  this.oFUI.setDebugInfo(string.Empty + "FW_" + progFilesRow.FirmwareVersion.ToString() + str4);
                }
              }
            }
          }
        }
        else
          this.oFUI.setErrorInfo("\rNo Firmware found in this database.\rMaybe you should use another database.");
      }
      catch (Exception ex)
      {
        this.oFUI.setErrorInfo("Message:\n" + ex.Message);
      }
    }

    public SortedList<uint, HardwareTypeTables.ProgFilesRow> loadAllFirmwareFromDatabase(
      uint firmwareVersion = 0,
      string hardwareName = "",
      string possibleFWFromBSL = "")
    {
      if (string.IsNullOrEmpty(hardwareName))
        throw new Exception("HardwareName is not set correctly.");
      SortedList<uint, HardwareTypeTables.ProgFilesRow> sortedList = new SortedList<uint, HardwareTypeTables.ProgFilesRow>();
      return this.revealAllPossibleFirmwareFromDatabase(firmwareVersion, hardwareName, possibleFWFromBSL);
    }

    private class UpdateDataBlock
    {
      public uint BlockId;
      public DeviceIdentification deviceIdent;
      public uint BSL_Version;
      public string bootLoaderFileData;
      public string firmwareFileData;
    }
  }
}
