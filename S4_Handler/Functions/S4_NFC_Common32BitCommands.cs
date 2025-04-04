// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_NFC_Common32BitCommands
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_NFC_Common32BitCommands : Common32BitCommands
  {
    private static Logger S4_NFC_Common32BitCommandsLogger = LogManager.GetLogger(nameof (S4_NFC_Common32BitCommands));
    private S4_DeviceCommandsNFC Nfc;

    internal S4_NFC_Common32BitCommands(S4_DeviceCommandsNFC nfc)
      : base((DeviceCommandsMBus) null)
    {
      this.Nfc = nfc;
    }

    internal async Task WriteCommunicationScenario(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario,
      byte? modulOption = null)
    {
      byte[] data;
      if (!modulOption.HasValue)
        data = new byte[2];
      else
        data = new byte[3]
        {
          (byte) 0,
          (byte) 0,
          modulOption.Value
        };
      data[0] = (byte) scenario;
      data[1] = (byte) ((uint) scenario >> 8);
      byte[] resultAsync = await this.Nfc.CommonNfcCommands.SendIrCommandAndGetResultAsync(progress, cancelToken, Manufacturer_FC.SetCommunicationScenario_0x92, data);
      data = (byte[]) null;
    }

    internal async Task<ushort> ReadCommunicationScenario(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.Nfc.CommonNfcCommands.SendIrCommandAndGetResultAsync(progress, cancelToken, Manufacturer_FC.GetCommunicationScenario_0x91);
      ushort num = result.Length == 2 ? BitConverter.ToUInt16(result, 0) : throw new Exception("Illegal result length");
      result = (byte[]) null;
      return num;
    }

    public override async Task<byte[]> TransmitAndReceiveVersionData(
      byte FC,
      byte EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] onlyData = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData((Manufacturer_FC) FC, new byte?(EFC), (byte[]) null, false, progress, cancelToken)));
      byte[] result = new byte[onlyData.Length + 2];
      result[0] = FC;
      result[1] = EFC;
      Buffer.BlockCopy((Array) onlyData, 0, (Array) result, 2, onlyData.Length);
      byte[] versionData = result;
      onlyData = (byte[]) null;
      result = (byte[]) null;
      return versionData;
    }

    public override byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      byte? EFC,
      byte[] parameterBytes,
      bool ACK_allowed,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (FC == Manufacturer_FC.CommonRadioCommands_0x2f && EFC.HasValue)
      {
        switch (EFC.Value)
        {
          case 32:
            return this.NFC_ModeReplacement(progress, cancelToken, S4_DeviceModes.OperationMode, EFC, parameterBytes);
          case 33:
            return this.NFC_ModeReplacement(progress, cancelToken, S4_DeviceModes.RadioTestTransmitUnmodulatedCarrier, EFC, parameterBytes);
          case 34:
            return this.NFC_ModeReplacement(progress, cancelToken, S4_DeviceModes.RadioTestTransmitModulatedCarrier, EFC, parameterBytes);
          case 35:
            return this.NFC_ModeReplacement(progress, cancelToken, S4_DeviceModes.RadioTestSendTestPacket, EFC, parameterBytes);
          case 36:
            return this.NFC_ModeReplacement(progress, cancelToken, S4_DeviceModes.RadioTestReceiveTestPacket, EFC, parameterBytes);
        }
      }
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) FC);
      if (EFC.HasValue)
        byteList.Add(EFC.Value);
      if (parameterBytes != null)
        byteList.AddRange((IEnumerable<byte>) parameterBytes);
      S4_NFC_Common32BitCommands.S4_NFC_Common32BitCommandsLogger.Trace("Send IrDa_Compatible_Command " + FC.ToString());
      byte[] src = this.Nfc.CommonNfcCommands.StandardCommand(progress, cancelToken, NfcCommands.IrDa_Compatible_Command, byteList.ToArray());
      int num = src[0] >= byte.MaxValue ? 4 : 2;
      int srcOffset1 = src[num - 1] == (byte) 32 ? num : throw new Exception("Received commend != command");
      if ((Manufacturer_FC) src[srcOffset1] != FC)
      {
        if (src[srcOffset1] == (byte) 254)
        {
          S4_NFC_Common32BitCommands.S4_NFC_Common32BitCommandsLogger.Trace("MBus ACK received");
          byte[] dst = new byte[src.Length - srcOffset1 - 2];
          Buffer.BlockCopy((Array) src, srcOffset1, (Array) dst, 0, dst.Length);
          return dst;
        }
        if (src[srcOffset1] != byte.MaxValue)
          throw new Exception("Received Ir commend != command");
        S4_NFC_Common32BitCommands.S4_NFC_Common32BitCommandsLogger.Trace("MBus NACK received");
        int index1 = srcOffset1 + 1;
        if ((Manufacturer_FC) src[index1] != FC)
          throw new Exception("Unexpected FC error response command byte: 0x" + src[index1].ToString("x02"));
        int index2 = index1 + 1;
        if (EFC.HasValue)
        {
          if ((int) src[index2] != (int) EFC.Value)
            throw new Exception("Unexpected EFC error response command byte: 0x" + src[index2].ToString("x02"));
          ++index2;
        }
        throw new Exception("IrDa coded error: " + ((NACK_Messages) src[index2]).ToString());
      }
      int srcOffset2 = srcOffset1 + 1;
      if (EFC.HasValue)
      {
        if ((int) src[srcOffset2] != (int) EFC.Value)
          throw new Exception("Illegal EFC inside response");
        ++srcOffset2;
      }
      byte[] dst1 = new byte[src.Length - srcOffset2 - 2];
      Buffer.BlockCopy((Array) src, srcOffset2, (Array) dst1, 0, dst1.Length);
      return dst1;
    }

    private byte[] NFC_ModeReplacement(
      ProgressHandler progress,
      CancellationToken cancelToken,
      S4_DeviceModes deviceMode,
      byte? EFC,
      byte[] parameterBytes)
    {
      if (deviceMode != 0 && this.Nfc.GetDeviceStates(progress, cancelToken).DeviceMode != 0)
        this.Nfc.CommonNfcCommands.SetMode(S4_DeviceModes.OperationMode, progress, cancelToken);
      this.Nfc.CommonNfcCommands.SetMode(deviceMode, progress, cancelToken, parameterBytes);
      switch (deviceMode)
      {
        case S4_DeviceModes.RadioTestSendTestPacket:
          ushort num = parameterBytes != null && parameterBytes.Length >= 4 ? BitConverter.ToUInt16(parameterBytes, 0) : throw new Exception("Illegal RadioTestSendTestPacket options");
          BitConverter.ToUInt16(parameterBytes, 2);
          if (num == (ushort) 0)
          {
            DateTime dateTime = DateTime.Now.AddSeconds(2.0);
            do
            {
              Thread.Sleep(100);
              if (this.Nfc.GetDeviceStates(progress, cancelToken).DeviceMode != S4_DeviceModes.RadioTestSendTestPacket)
                goto label_18;
            }
            while (!(DateTime.Now > dateTime));
            throw new Exception("RadioTestSendTestPacket logical timeout");
          }
          break;
        case S4_DeviceModes.RadioTestReceiveTestPacket:
          DateTime dateTime1 = DateTime.Now.AddSeconds(2.0);
          BitConverter.ToUInt16(parameterBytes, 3);
          S4_SystemState deviceStates;
          do
          {
            Thread.Sleep(100);
            deviceStates = this.Nfc.GetDeviceStates(progress, cancelToken);
            if (deviceStates.DeviceMode != S4_DeviceModes.RadioTestReceiveTestPacket)
              goto label_14;
          }
          while (!(DateTime.Now > dateTime1));
          throw new Exception("RadioTestReceiveTestPacket logical timeout");
label_14:
          byte[] dst;
          if (deviceStates.ModeResultData != null)
          {
            dst = new byte[deviceStates.ModeResultData.Length + 2];
            dst[0] = (byte) 47;
            dst[1] = EFC.Value;
            Buffer.BlockCopy((Array) deviceStates.ModeResultData, 0, (Array) dst, 2, dst.Length);
          }
          else
            dst = new byte[3]
            {
              (byte) 254,
              (byte) 47,
              EFC.Value
            };
          return dst;
      }
label_18:
      return new byte[3]{ (byte) 254, (byte) 47, EFC.Value };
    }
  }
}
