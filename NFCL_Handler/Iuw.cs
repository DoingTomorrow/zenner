// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.Iuw
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace NFCL_Handler
{
  internal class Iuw
  {
    private static Logger logger = LogManager.GetLogger(nameof (Iuw));

    internal static async Task<IuwData> TryToReadData(
      ProgressHandler progress,
      CancellationToken token,
      NFCL_DeviceCommands cmd)
    {
      IuwData r = new IuwData();
      try
      {
        byte[] getIdentification = await cmd.Special.SendToNfcDeviceAsync(new NfcFrame(NfcCommands.GetIdentification, "GetIdentification").NfcRequestFrame, progress, token);
        r.Ident = new NfcDeviceIdentification(getIdentification);
        uint? meterId = r.Ident.MeterID;
        ushort crcInitValue = (ushort) ((int) (ushort) (meterId.HasValue ? new uint?(meterId.GetValueOrDefault() >> 16) : new uint?()).Value ^ (int) (ushort) r.Ident.MeterID.Value);
        NfcFrame nfcFrameGetAvailableModuleConfigurations = new NfcFrame(NfcCommands.GetAvailableModuleConfigurations, "GetAvailableModuleConfigurations", new ushort?(crcInitValue));
        byte[] bytesOfGetAvailableModuleConfigurations = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetAvailableModuleConfigurations.NfcRequestFrame, progress, token);
        int headerLength = bytesOfGetAvailableModuleConfigurations[0] >= byte.MaxValue ? 4 : 2;
        if (bytesOfGetAvailableModuleConfigurations[headerLength - 1] != (byte) 51)
          throw new Exception("xxx Received commend != command");
        byte[] resultDataOnly = new byte[bytesOfGetAvailableModuleConfigurations.Length - headerLength - 2];
        Buffer.BlockCopy((Array) bytesOfGetAvailableModuleConfigurations, headerLength, (Array) resultDataOnly, 0, resultDataOnly.Length);
        int configCount = resultDataOnly.Length / 2;
        if (configCount * 2 != resultDataOnly.Length)
          throw new Exception("Illegal number of bytesOfGetAvailableModuleConfigurations");
        r.AvailableScenarios = new ushort[configCount];
        int scanOffset = 0;
        for (int i = 0; i < r.AvailableScenarios.Length; ++i)
          r.AvailableScenarios[i] = ByteArrayScanner.ScanUInt16(resultDataOnly, ref scanOffset);
        NfcFrame nfcFrameGetVolumeAndFlow = new NfcFrame(NfcCommands.GetVolumeAndFlow, "GetVolumeAndFlow", new ushort?(crcInitValue));
        byte[] bytesOfGetVolumeAndFlow = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetVolumeAndFlow.NfcRequestFrame, progress, token);
        r.IuwCurrentData = IuwCurrentData.Parse(bytesOfGetVolumeAndFlow, 2);
        NfcFrame nfcFrameGetBatteryEndDate = new NfcFrame(NfcCommands.GetBatteryEndDate, "GetBatteryEndDate", new ushort?(crcInitValue));
        byte[] bytesOfGetBatteryEndDate = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetBatteryEndDate.NfcRequestFrame, progress, token);
        r.BatteryEndDate = Iuw.ConvertToDateTime_SystemTime24(bytesOfGetBatteryEndDate, 2);
        NfcFrame nfcFrameGetCommunicationScenario = new NfcFrame(NfcCommands.IrDa_Compatible_Command, new byte[1]
        {
          (byte) 145
        }, "GetCommunicationScenario", new ushort?(crcInitValue));
        byte[] bytesOfGetCommunicationScenario = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetCommunicationScenario.NfcRequestFrame, progress, token);
        r.CommunicationScenario = BitConverter.ToUInt16(bytesOfGetCommunicationScenario, 3);
        NfcFrame nfcFrameGetKeyDate = new NfcFrame(NfcCommands.IrDa_Compatible_Command, new byte[1]
        {
          (byte) 136
        }, "GetKeyDate", new ushort?(crcInitValue));
        byte[] bytesOfGetKeyDate = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetKeyDate.NfcRequestFrame, progress, token);
        byte month = bytesOfGetKeyDate[3];
        byte day = bytesOfGetKeyDate[4];
        byte year = bytesOfGetKeyDate[5];
        DateTime t;
        if (DateTime.TryParse((2000 + (int) year).ToString() + "-" + month.ToString("00") + "-" + day.ToString("00") + " 00:00", out t))
          r.KeyDate = new DateTime?(t);
        List<byte> data = new List<byte>(3);
        data.AddRange((IEnumerable<byte>) BitConverter.GetBytes(r.CommunicationScenario));
        data.Add((byte) 1);
        NfcFrame nfcFrameGetModuleConfiguration = new NfcFrame(NfcCommands.GetModuleConfiguration, data.ToArray(), "GetModuleConfiguration", new ushort?(crcInitValue));
        byte[] bytesOfGetModuleConfiguration = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetModuleConfiguration.NfcRequestFrame, progress, token);
        int start = bytesOfGetModuleConfiguration[0] == byte.MaxValue ? 4 : 2;
        IuwModuleConfiguration cfg = IuwModuleConfiguration.Parse(bytesOfGetModuleConfiguration.SubArray<byte>(start, bytesOfGetModuleConfiguration.Length - (start + 1)));
        r.DevEUI = cfg.DevEUI != null ? new ulong?(BitConverter.ToUInt64(cfg.DevEUI, 0)) : new ulong?();
        r.JoinEUI = cfg.JoinEUI != null ? new ulong?(BitConverter.ToUInt64(cfg.JoinEUI, 0)) : new ulong?();
        r.AppKey = cfg.AppKey;
        r.AesKey = cfg.AesKey;
        return r;
      }
      catch (Exception ex)
      {
        Iuw.logger.Error<Exception>(ex);
        return new IuwData() { OccuredException = ex };
      }
    }

    public static DateTime? ConvertToDateTime_SystemTime24(byte[] buffer, int startIndex = 0)
    {
      int num1 = startIndex;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      byte num3 = numArray1[index1];
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      byte month = numArray2[index2];
      byte[] numArray3 = buffer;
      int index3 = num4;
      int num5 = index3 + 1;
      byte day = numArray3[index3];
      if (month == (byte) 0 || day == (byte) 0)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num3, (int) month, (int) day));
      }
      catch
      {
        return new DateTime?();
      }
    }

    internal static async Task TryToWrite(
      ProgressHandler progress,
      CancellationToken token,
      NFCL_DeviceCommands cmd,
      IuwData iuwDataOriginal,
      IuwData iuwData)
    {
      uint? meterId = iuwData.Ident.MeterID;
      ushort crcInitValue = (ushort) ((int) (ushort) (meterId.HasValue ? new uint?(meterId.GetValueOrDefault() >> 16) : new uint?()).Value ^ (int) (ushort) iuwData.Ident.MeterID.Value);
      if (iuwData.SetOperationMode != iuwDataOriginal.SetOperationMode && iuwData.SetOperationMode)
      {
        NfcFrame nfcFrameSetOperationMode = new NfcFrame(NfcCommands.SetTestMode, new byte[1], "SetOperationMode", new ushort?(crcInitValue));
        byte[] bytesOfSetOperationMode = await cmd.Special.SendToNfcDeviceAsync(nfcFrameSetOperationMode.NfcRequestFrame, progress, token);
        nfcFrameSetOperationMode = (NfcFrame) null;
        bytesOfSetOperationMode = (byte[]) null;
      }
      if ((int) iuwData.CommunicationScenario != (int) iuwDataOriginal.CommunicationScenario)
      {
        List<byte> bytes = new List<byte>();
        bytes.Add((byte) 146);
        bytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(iuwData.CommunicationScenario));
        NfcFrame nfcFrameSetCommunicationScenario = new NfcFrame(NfcCommands.IrDa_Compatible_Command, bytes.ToArray(), "SetCommunicationScenario", new ushort?(crcInitValue));
        byte[] bytesOfGetCommunicationScenario = await cmd.Special.SendToNfcDeviceAsync(nfcFrameSetCommunicationScenario.NfcRequestFrame, progress, token);
        bytes = (List<byte>) null;
        nfcFrameSetCommunicationScenario = (NfcFrame) null;
        bytesOfGetCommunicationScenario = (byte[]) null;
      }
      DateTime? keyDate1 = iuwData.KeyDate;
      DateTime? keyDate2 = iuwDataOriginal.KeyDate;
      if (keyDate1.HasValue == keyDate2.HasValue && (!keyDate1.HasValue || !(keyDate1.GetValueOrDefault() != keyDate2.GetValueOrDefault())))
        return;
      byte[] frameData = new byte[4]
      {
        (byte) 136,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      keyDate2 = iuwData.KeyDate;
      DateTime dateTime = keyDate2.Value;
      frameData[1] = (byte) dateTime.Month;
      keyDate2 = iuwData.KeyDate;
      dateTime = keyDate2.Value;
      frameData[2] = (byte) dateTime.Day;
      keyDate2 = iuwData.KeyDate;
      dateTime = keyDate2.Value;
      frameData[3] = (byte) (dateTime.Year - 2000);
      NfcFrame nfcFrameGetKeyDate = new NfcFrame(NfcCommands.IrDa_Compatible_Command, frameData, "SetKeyDate", new ushort?(crcInitValue));
      byte[] bytesOfGetKeyDate = await cmd.Special.SendToNfcDeviceAsync(nfcFrameGetKeyDate.NfcRequestFrame, progress, token);
      nfcFrameGetKeyDate = (NfcFrame) null;
      bytesOfGetKeyDate = (byte[]) null;
    }
  }
}
