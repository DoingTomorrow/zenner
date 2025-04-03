// Decompiled with JetBrains decompiler
// Type: HandlerLib.SpecialCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class SpecialCommands : IZRCommand
  {
    private static Logger logger = LogManager.GetLogger(nameof (SpecialCommands));
    private Common32BitCommands commonCMD;
    private bool crypt = false;
    private string AESKey = (string) null;

    public bool enDeCrypt
    {
      get => this.crypt;
      set
      {
        this.crypt = value;
        if (this.commonCMD == null)
          return;
        this.commonCMD.enDeCrypt = value;
      }
    }

    public string AES_Key
    {
      get => this.AESKey;
      set
      {
        this.AESKey = value;
        if (this.commonCMD == null)
          return;
        this.commonCMD.AES_Key = value;
      }
    }

    public SpecialCommands(Common32BitCommands commonCMD)
    {
      this.commonCMD = commonCMD;
      this.setCryptValuesFromBaseClass();
    }

    public void setCryptValuesFromBaseClass()
    {
      this.enDeCrypt = this.commonCMD.enDeCrypt;
      this.AES_Key = this.commonCMD.AES_Key;
    }

    public async Task<ushort> GetSpecialCommandFCVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSpecialCommandFCVersion_0x00, progress, cancelToken);
      ushort uint16 = BitConverter.ToUInt16(theData, 0);
      theData = (byte[]) null;
      return uint16;
    }

    public async Task<SpecialCommands.Metrology_Parameters> GetSCMetrologyParametersAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetMetrologyParameters_0x02, progress, cancelToken);
      SpecialCommands.Metrology_Parameters retData = new SpecialCommands.Metrology_Parameters();
      retData.basedata = theData;
      Buffer.BlockCopy((Array) theData, 0, (Array) retData.Identity, 0, 2);
      Buffer.BlockCopy((Array) theData, 2, (Array) retData.Options, 0, theData.Length - 2);
      SpecialCommands.Metrology_Parameters metrologyParametersAsync = retData;
      theData = (byte[]) null;
      retData = (SpecialCommands.Metrology_Parameters) null;
      return metrologyParametersAsync;
    }

    public async Task SetSCMetrologyParametersAsync(
      byte[] identity,
      byte[] options,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[12];
      data[0] = identity[0];
      data[1] = identity[1];
      Buffer.BlockCopy((Array) options, 0, (Array) data, 2, options.Length);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetMetrologyParameters_0x02, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<SpecialCommands.SD_Rules_Options> GetSDRulesAsync(
      byte rule,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[1]{ rule };
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetSDRules_0x0A, data, progress, cancelToken);
      SpecialCommands.SD_Rules_Options retData = new SpecialCommands.SD_Rules_Options();
      retData.basedata = theData;
      retData.Selection = theData[0];
      retData.Flag = theData[1];
      Buffer.BlockCopy((Array) theData, 2, (Array) retData.options, 0, 4);
      SpecialCommands.SD_Rules_Options sdRulesAsync = retData;
      data = (byte[]) null;
      theData = (byte[]) null;
      retData = (SpecialCommands.SD_Rules_Options) null;
      return sdRulesAsync;
    }

    public async Task SetSDRulesAsync(
      byte selection,
      byte flag,
      byte[] options,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[6]
      {
        selection,
        flag,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      Buffer.BlockCopy((Array) options, 0, (Array) data, 2, 4);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetSDRules_0x0A, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<byte[]> GetFlowCheckStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetClearFlowCheckStates_0x09, progress, cancelToken);
      byte[] flowCheckStateAsync = theData;
      theData = (byte[]) null;
      return flowCheckStateAsync;
    }

    public async Task ClearFlowCheckStatesAsync(
      ushort state,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = BitConverter.GetBytes(state);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetClearFlowCheckStates_0x09, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<byte> GetSummertimeCountingSuppressionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetSummertimeCouningSuppression_0x06, progress, cancelToken);
      byte suppressionAsync = theData[0];
      theData = (byte[]) null;
      return suppressionAsync;
    }

    public async Task SetSummertimeCountingSuppressionAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[1]{ state };
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetSummertimeCouningSuppression_0x06, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<byte[]> GetProductFactorAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetProductionFactor_0x04, progress, cancelToken);
      byte[] productFactorAsync = theData;
      theData = (byte[]) null;
      return productFactorAsync;
    }

    public async Task SetProductFactorAsync(
      byte[] factor,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data = new byte[2];
      Buffer.BlockCopy((Array) factor, 0, (Array) data, 0, 2);
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetSetProductionFactor_0x04, data, progress, cancelToken);
      data = (byte[]) null;
    }

    public async Task<byte[]> GetCurrentMeasuringModeAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetCurrentMeasuringMode_0x01, progress, cancelToken);
      byte[] measuringModeAsync = theData;
      theData = (byte[]) null;
      return measuringModeAsync;
    }

    public async Task<byte[]> SendToNfcDeviceAsync(
      byte[] NFCrequest,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Delay(200);
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.SendToNfcDevice_0x0B, NFCrequest, progress, cancelToken);
      byte[] nfcDeviceAsync = theData;
      theData = (byte[]) null;
      return nfcDeviceAsync;
    }

    public async Task<SpecialCommands.NFC_Identification> GetNfcDeviceIdentification(
      byte[] NFCrequest,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] theData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.GetNfcDeviceIdentification_0x0C, NFCrequest, progress, cancelToken);
      ushort arrayPos = 0;
      ushort version = (ushort) theData[0];
      SpecialCommands.NFC_Identification nfcIdent = new SpecialCommands.NFC_Identification();
      nfcIdent.basedata = theData;
      nfcIdent.IdentificationResponseVersion = theData[(int) arrayPos];
      ++arrayPos;
      nfcIdent.NFCProtocolVersion = theData[(int) arrayPos];
      ++arrayPos;
      nfcIdent.MBusMedium = theData[(int) arrayPos];
      ++arrayPos;
      nfcIdent.OBISMedium = theData[(int) arrayPos];
      ++arrayPos;
      Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.Manufacturer, 0, 2);
      arrayPos += (ushort) 2;
      nfcIdent.Generation = theData[(int) arrayPos];
      ++arrayPos;
      Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.Serialnumber, 0, 4);
      arrayPos += (ushort) 4;
      switch (version)
      {
        case 0:
          Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.HardwareIdentification, 0, 4);
          arrayPos += (ushort) 4;
          break;
        case 1:
          Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.HardwareIdentification, 0, 2);
          arrayPos += (ushort) 2;
          Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.SystemState, 0, 2);
          arrayPos += (ushort) 2;
          break;
        case 2:
          Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.HardwareIdentification, 0, 2);
          arrayPos += (ushort) 2;
          Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.SystemInfos, 0, 4);
          arrayPos += (ushort) 4;
          break;
      }
      Buffer.BlockCopy((Array) theData, (int) arrayPos, (Array) nfcIdent.FirmwareVersion, 0, 4);
      arrayPos += (ushort) 4;
      nfcIdent.MeterID = BitConverter.ToUInt32(theData, (int) arrayPos);
      arrayPos += (ushort) 4;
      nfcIdent.BuildRevision = BitConverter.ToUInt32(theData, (int) arrayPos);
      arrayPos += (ushort) 4;
      nfcIdent.BuildTime = BitConverter.ToUInt32(theData, (int) arrayPos);
      arrayPos += (ushort) 4;
      nfcIdent.CompilerVersion = BitConverter.ToUInt32(theData, (int) arrayPos);
      arrayPos += (ushort) 4;
      nfcIdent.FirmwareSignature = BitConverter.ToUInt16(theData, (int) arrayPos);
      arrayPos += (ushort) 2;
      nfcIdent.NumberOfAvailableParameterGroups = theData[(int) arrayPos];
      ++arrayPos;
      nfcIdent.NumberOfAvailableParameters = BitConverter.ToUInt16(theData, (int) arrayPos);
      arrayPos += (ushort) 2;
      nfcIdent.NumberOfSelectedParameterGroups = theData[(int) arrayPos];
      ++arrayPos;
      nfcIdent.NumberOfSelectedParameters = BitConverter.ToUInt16(theData, (int) arrayPos);
      arrayPos += (ushort) 2;
      nfcIdent.MaximumRecordLength = theData[(int) arrayPos];
      ++arrayPos;
      SpecialCommands.NFC_Identification deviceIdentification = nfcIdent;
      theData = (byte[]) null;
      nfcIdent = (SpecialCommands.NFC_Identification) null;
      return deviceIdentification;
    }

    public async Task SetReligiousDay(
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.SetReligiousDay_0x0D, data, progress, cancelToken);
    }

    public async Task SetSmartFunctions(
      byte[] Data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.commonCMD.TransmitAndCheckAckAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.SetGetSmartFunctions_0x0E, Data, progress, cancelToken);
    }

    public async Task<byte[]> GetSmartFunctions(
      byte SmartFunctionCode,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ReturnData = await this.commonCMD.TransmitAndGetDataAsync(Manufacturer_FC.SpecialCommands_0x36, SpecialCommands_EFC.SetGetSmartFunctions_0x0E, new byte[1]
      {
        SmartFunctionCode
      }, progress, cancelToken);
      byte[] smartFunctions = ReturnData;
      ReturnData = (byte[]) null;
      return smartFunctions;
    }

    public class SD_Rules_Options : ReturnValue
    {
      public byte Selection = 1;
      public byte Flag = 0;
      public byte[] options = new byte[4];
    }

    public class Metrology_Parameters : ReturnValue
    {
      public byte[] Identity = new byte[2];
      public byte[] Options = new byte[10];
    }

    public class NFC_Identification : ReturnValue
    {
      public byte IdentificationResponseVersion = 0;
      public byte NFCProtocolVersion = 0;
      public byte MBusMedium = 0;
      public byte OBISMedium = 0;
      public byte[] Manufacturer = new byte[2];
      public byte Generation = 0;
      public byte[] Serialnumber = new byte[4];
      public byte[] HardwareIdentification = new byte[4];
      public byte[] SystemState = new byte[2];
      public byte[] SystemInfos = new byte[4];
      public byte[] FirmwareVersion = new byte[4];
      public uint MeterID = 0;
      public uint BuildRevision = 0;
      public uint BuildTime = 0;
      public uint CompilerVersion = 0;
      public ushort FirmwareSignature = 0;
      public byte NumberOfAvailableParameterGroups = 0;
      public ushort NumberOfAvailableParameters = 0;
      public byte NumberOfSelectedParameterGroups = 0;
      public ushort NumberOfSelectedParameters = 0;
      public byte MaximumRecordLength = 0;
    }
  }
}
