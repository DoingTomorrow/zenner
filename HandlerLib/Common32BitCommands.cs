// Decompiled with JetBrains decompiler
// Type: HandlerLib.Common32BitCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using MBusLib.Exceptions;
using MBusLib.Utility;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;

#nullable disable
namespace HandlerLib
{
  public class Common32BitCommands : BaseMemoryAccess, IZRCommand
  {
    internal static Logger Common32BitCommands_Logger = LogManager.GetLogger(nameof (Common32BitCommands));
    private bool crypt = false;
    private string AESKey = (string) null;
    private AES_ENCRYPTION_MODE cryptMode;

    public bool enDeCrypt
    {
      get => this.crypt;
      set => this.crypt = value;
    }

    public string AES_Key
    {
      get => this.AESKey;
      set => this.AESKey = value;
    }

    public AES_ENCRYPTION_MODE enDeCryptMode
    {
      get => this.cryptMode;
      set => this.cryptMode = value;
    }

    public DeviceCommandsMBus DeviceCMD { get; set; }

    public Common32BitCommands(
      DeviceCommandsMBus deviceCMD,
      bool crypt = false,
      string AESkey = null,
      AES_ENCRYPTION_MODE ENC_MODE = AES_ENCRYPTION_MODE.MODE_7)
    {
      this.DeviceCMD = deviceCMD;
      this.AES_Key = AESkey;
      this.enDeCrypt = crypt;
      this.cryptMode = ENC_MODE;
    }

    public override async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace("ReadMemoryAsync: " + addressRange.ToString());
      byte[] readData = await this.ReadMemoryAsync(progress, cancelToken, addressRange, (byte) 90);
      deviceMemory.SetData(addressRange.StartAddress, readData);
      Common32BitCommands.Common32BitCommands_Logger.Trace("ReadMemoryAsync: done");
      readData = (byte[]) null;
    }

    public override async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace("WriteMemoryAsync: " + addressRange.ToString());
      byte[] writeData = deviceMemory.GetData(addressRange);
      await this.WriteMemoryAsync(progress, cancelToken, addressRange, writeData, (byte) 192);
      Common32BitCommands.Common32BitCommands_Logger.Trace("WriteMemoryAsync: done");
      writeData = (byte[]) null;
    }

    public virtual async Task<byte[]> TransmitAndReceiveVersionData(
      byte FC,
      byte EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> myCommand = new List<byte>();
      myCommand.Add((byte) 15);
      myCommand.AddRange((IEnumerable<byte>) new byte[3]);
      myCommand.Add(FC);
      myCommand.Add(EFC);
      byte[] resultData = (byte[]) null;
      MBusFrame request = new MBusFrame(myCommand.ToArray());
      MBusFrame response = await this.DeviceCMD.MBus.Repeater.GetResultFrameAsync(request, progress, cancelToken);
      resultData = DeviceCommandsMBus.Get_FC_EFC_AndData(response);
      request = (MBusFrame) null;
      response = (MBusFrame) null;
      byte[] versionData = resultData;
      myCommand = (List<byte>) null;
      resultData = (byte[]) null;
      return versionData;
    }

    public async Task<byte> GetMBusStatusByte(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      List<byte> myCommand = new List<byte>();
      myCommand.Add((byte) 15);
      myCommand.AddRange((IEnumerable<byte>) new byte[3]);
      myCommand.Add((byte) 6);
      MBusFrame request = new MBusFrame(myCommand.ToArray());
      MBusFrame response = await this.DeviceCMD.MBus.Repeater.GetResultFrameAsync(request, progress, cancelToken);
      VariableDataStructure vds = VariableDataStructure.Parse(response);
      byte status = vds.Header.Status;
      myCommand = (List<byte>) null;
      request = (MBusFrame) null;
      response = (MBusFrame) null;
      vds = (VariableDataStructure) null;
      return status;
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?(), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?(), data, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonRadioCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonRadioCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), data, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), data, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonLoRaCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonNBIoTCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonLoRaCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), data, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      CommonNBIoTCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), data, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      SpecialCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), (byte[]) null, progress, cancelToken)));
    }

    public async Task TransmitAndCheckAckAsync(
      Manufacturer_FC FC,
      SpecialCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, new byte?((byte) EFC), data, progress, cancelToken)));
    }

    private void TransmitAndCheckAck(
      Manufacturer_FC FC,
      byte? EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] data1 = this.TransmitAndGetData(FC, EFC, data, true, progress, cancelToken);
      if (this.enDeCrypt)
        return;
      if (data1 == null || data1.Length < 2)
        throw new Exception("Illegal ACK data received.");
      if (data1[0] != (byte) 254)
        throw new Exception("Illegal ACK code received");
      if ((Manufacturer_FC) data1[1] != FC)
        throw new Exception("Illegal FC inside ACK data received.");
      if (EFC.HasValue)
      {
        int num = data1.Length >= 3 ? (int) data1[2] : throw new Exception("Illegal ACK data length for EFC code using");
        byte? nullable1 = EFC;
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int valueOrDefault = nullable2.GetValueOrDefault();
        if (!(num == valueOrDefault & nullable2.HasValue))
          throw new Exception("Illegal EFC inside ACK data received.");
        if (data1.Length > 3)
        {
          if (data1.Length > 4)
            throw new Exception("Too much ACK data.");
          if (data1.Length == 4)
            throw new Exception("\nNACK: " + this.checkForNACKValue((int) data1[3]));
        }
      }
      else if (data1.Length > 2)
      {
        if (data1.Length > 3)
          throw new Exception("Too much ACK data.");
        if (data1.Length == 3)
          throw new Exception("\nNACK: " + this.checkForNACKValue((int) data1[2]));
      }
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?(), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonRadioCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonRadioCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken)));
      return dataAsync;
    }

    public byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      CommonRadioCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      return this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken);
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      byte[] data,
      bool ack_allowed,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, ack_allowed, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonLoRaCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      CommonLoRaCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      return this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken);
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonNBIoTCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      CommonNBIoTCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      return this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken);
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonNBIoTCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken)));
      return dataAsync;
    }

    public byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      return this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken);
    }

    public byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      CommonMBusCommands_EFC EFC,
      byte[] bytes,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      return this.TransmitAndGetData(FC, new byte?((byte) EFC), bytes, false, progress, cancelToken);
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      CommonLoRaCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      SpecialCommands_EFC EFC,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), (byte[]) null, false, progress, cancelToken)));
      return dataAsync;
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      Manufacturer_FC FC,
      SpecialCommands_EFC EFC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, new byte?((byte) EFC), data, false, progress, cancelToken)));
      return dataAsync;
    }

    public virtual byte[] TransmitAndGetData(
      Manufacturer_FC FC,
      byte? EFC,
      byte[] parameterBytes,
      bool ACK_allowed,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.CheckLicence_ReadOnly(FC, EFC, parameterBytes);
      if (this.DeviceCMD.ConnectedReducedID == null)
        throw new Exception("Please read version first! 'DeviceCMD.ConnectedReducedID == null'");
      int num1 = 0;
      if (parameterBytes != null)
        num1 = parameterBytes.Length;
      byte[] numArray1;
      int length1;
      if (!EFC.HasValue)
      {
        numArray1 = new byte[5 + num1];
        length1 = 15;
      }
      else
      {
        numArray1 = new byte[6 + num1];
        length1 = 16;
      }
      int num2 = 0;
      byte[] numArray2 = numArray1;
      int index1 = num2;
      int num3 = index1 + 1;
      numArray2[index1] = (byte) 15;
      for (int index2 = 0; index2 < 3; ++index2)
        numArray1[num3++] = this.DeviceCMD.ConnectedReducedID[index2];
      byte[] numArray3 = numArray1;
      int index3 = num3;
      int num4 = index3 + 1;
      int num5 = (int) FC;
      numArray3[index3] = (byte) num5;
      if (EFC.HasValue)
        numArray1[num4++] = EFC.Value;
      if (parameterBytes != null)
      {
        for (int index4 = 0; index4 < parameterBytes.Length; ++index4)
          numArray1[num4++] = parameterBytes[index4];
      }
      if (this.enDeCrypt && !string.IsNullOrEmpty(this.AES_Key))
      {
        this.DeviceCMD.MBus.Repeater.AES_Key = this.AES_Key;
        byte[] byteArray = ZENNER.CommonLibrary.Utility.HexStringToByteArray(this.AES_Key);
        MBusFrameCrypt previousFrame = MBusFrameCrypt.Parse(Direction.GatewayToDevice, new DateTime?(DateTime.Now), new MBusFrameCrypt(numArray1).ToByteArray());
        byte[] numArray4 = new byte[15];
        byte[] numArray5 = new byte[4];
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray5, 0, 4);
        int length2 = numArray5.Length;
        Buffer.BlockCopy((Array) numArray5, 0, (Array) numArray4, 0, numArray5.Length);
        byte[] numArray6 = numArray4;
        int index5 = length2;
        int length3 = index5 + 1;
        int num6 = (int) FC;
        numArray6[index5] = (byte) num6;
        if (EFC.HasValue)
          numArray4[length3++] = EFC.Value;
        if (parameterBytes != null)
        {
          for (int index6 = 0; index6 < parameterBytes.Length; ++index6)
            numArray4[length3++] = parameterBytes[index6];
        }
        byte[] numArray7 = new byte[length3];
        Buffer.BlockCopy((Array) numArray4, 0, (Array) numArray7, 0, numArray7.Length);
        MBusFrameCrypt resultFrame = this.DeviceCMD.MBus.Repeater.GetResultFrame(new MBusFrameCrypt(Direction.DeviceToGateway, numArray7, new int?(7), byteArray, previousFrame), progress, cancelToken);
        if (resultFrame == null)
          throw new ArgumentNullException("response");
        resultFrame.EncryptionKey = byteArray;
        if (resultFrame.Type != FrameType.LongFrame)
          throw new InvalidFrameException(string.Format("Expected: LongFrame, Actual: {0}", (object) resultFrame.Type), resultFrame.ToByteArray());
        VariableDataStructure variableDataStructure = resultFrame.IsVariableDataStructure ? VariableDataStructure.Parse(resultFrame) : throw new InvalidFrameException("Expected: VariableDataStructure", resultFrame.ToByteArray());
        int? nullable1 = resultFrame.EncryptionMode;
        int num7 = 7;
        if (nullable1.GetValueOrDefault() == num7 & nullable1.HasValue && variableDataStructure.MfgData != null && (variableDataStructure.MDH == (byte) 15 || variableDataStructure.MDH == byte.MaxValue))
        {
          int length4 = variableDataStructure.MfgData.Length;
          for (int index7 = variableDataStructure.MfgData.Length - 1; index7 >= 0; --index7)
          {
            if (variableDataStructure.MfgData[index7] == (byte) 47)
              --length4;
            if (variableDataStructure.MfgData[index7] == (byte) 22)
            {
              length4 -= 2;
              break;
            }
          }
          variableDataStructure.MfgData = variableDataStructure.MfgData.SubArray<byte>(0, length4);
        }
        if (variableDataStructure.MDH != (byte) 15)
          throw new InvalidFrameException("Missed manufacturer specific data header (0x0F)", resultFrame.ToByteArray());
        if (variableDataStructure.MfgData.Length >= 2 && variableDataStructure.MfgData[0] == (byte) 254 && (Manufacturer_FC) variableDataStructure.MfgData[1] == FC)
        {
          if (EFC.HasValue)
          {
            int num8;
            if (variableDataStructure.MfgData.Length >= 2)
            {
              int num9 = (int) variableDataStructure.MfgData[2];
              byte? nullable2 = EFC;
              nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
              int valueOrDefault = nullable1.GetValueOrDefault();
              num8 = !(num9 == valueOrDefault & nullable1.HasValue) ? 1 : 0;
            }
            else
              num8 = 1;
            if (num8 != 0)
              throw new InvalidFrameException("Wrong EFC", resultFrame.ToByteArray());
          }
          return (byte[]) null;
        }
        if (variableDataStructure.MfgData[0] == byte.MaxValue)
        {
          if ((Manufacturer_FC) variableDataStructure.MfgData[1] != FC)
            throw new InvalidFrameException(string.Format("Invalid FC. Expected: {0:X2}h, Actual: {1}h", (object) (int) FC, (object) variableDataStructure.MfgData[0]), resultFrame.ToByteArray());
          Nack nack = EFC.HasValue ? (Nack) variableDataStructure.MfgData[3] : (Nack) variableDataStructure.MfgData[2];
          if (nack == Nack.UnknownFunction)
            throw new InvalidFrameException("Unknown command !!!", resultFrame.ToByteArray());
          throw new NackException(FC.ToString(), nack, variableDataStructure.MfgData);
        }
        try
        {
          if (variableDataStructure.MfgData.Length < 5)
            throw new InvalidFrameException("Invalid ZR1 frame!", resultFrame.ToByteArray());
          if ((Manufacturer_FC) variableDataStructure.MfgData[3] != FC)
            throw new InvalidFrameException(string.Format("Invalid FC. Expected: {0:X2}h, Actual: {1}h", (object) (int) FC, (object) variableDataStructure.MfgData[3]), resultFrame.ToByteArray());
          int num10;
          if (EFC.HasValue && ((int) variableDataStructure.MfgData[4] & 128) != 128)
          {
            int num11 = (int) variableDataStructure.MfgData[4] & (int) sbyte.MaxValue;
            byte? nullable3 = EFC;
            nullable1 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            int valueOrDefault = nullable1.GetValueOrDefault();
            num10 = !(num11 == valueOrDefault & nullable1.HasValue) ? 1 : 0;
          }
          else
            num10 = 0;
          if (num10 != 0)
            throw new InvalidFrameException(string.Format("Invalid EFC. Expected: {0:X2}h, Actual: {1}h", (object) EFC.Value, (object) variableDataStructure.MfgData[1]), resultFrame.ToByteArray());
          int index8 = EFC.HasValue ? 5 : 4;
          return variableDataStructure.MfgData.SubArray<byte>(index8, variableDataStructure.MfgData.Length - index8);
        }
        catch
        {
          if ((Manufacturer_FC) variableDataStructure.MfgData[0] != FC)
            throw new InvalidFrameException(string.Format("Invalid FC. Expected: {0:X2}h, Actual: {1}h", (object) (int) FC, (object) variableDataStructure.MfgData[0]), resultFrame.ToByteArray());
          if (EFC.HasValue && (int) variableDataStructure.MfgData[1] != (int) EFC.Value)
            throw new InvalidFrameException(string.Format("Invalid EFC. Expected: {0:X2}h, Actual: {1}h", (object) EFC.Value, (object) variableDataStructure.MfgData[1]), resultFrame.ToByteArray());
          int index9 = EFC.HasValue ? 2 : 1;
          return variableDataStructure.MfgData.SubArray<byte>(index9, variableDataStructure.MfgData.Length - index9);
        }
      }
      else
      {
        MBusFrame resultFrame = this.DeviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(numArray1), progress, cancelToken);
        byte[] userData = resultFrame.UserData;
        DeviceCommandsMBus.CheckManufacturerResponse(resultFrame, this.DeviceCMD.ConnectedDeviceVersion.LongID);
        if (resultFrame.UserData[13] == byte.MaxValue)
        {
          if (resultFrame.UserData.Length < length1 + 1)
            throw new NACK_Exception("Illegal manufacturer NACK frame. No error code available.");
          string str1 = (string) null;
          if (resultFrame.UserData.Length > length1 + 1)
            str1 = ZENNER.CommonLibrary.Utility.ByteArrayToHexString(resultFrame.UserData, 0, length1);
          string str2 = this.checkForNACKValue((int) resultFrame.UserData[length1]);
          if (str1 != null)
            throw new NACK_Exception(str2 + "; Additional infos: " + str1);
          throw new NACK_Exception(str2 + " CMD: " + FC.ToString());
        }
        int srcOffset;
        if (resultFrame.UserData[13] == (byte) 254)
        {
          if (!ACK_allowed)
            throw new Exception("ACK received, data expected.");
          srcOffset = 13;
        }
        else
        {
          if ((Manufacturer_FC) resultFrame.UserData[13] != FC)
            throw new Exception("Illegal FC received");
          if (EFC.HasValue)
          {
            srcOffset = 15;
            if ((int) resultFrame.UserData[14] != (int) EFC.Value)
              throw new Exception("Illegal EFC received");
          }
          else
            srcOffset = 14;
        }
        byte[] dst;
        if (srcOffset < 0)
        {
          dst = (byte[]) null;
        }
        else
        {
          int count = resultFrame.UserData.Length - srcOffset;
          dst = new byte[count];
          Buffer.BlockCopy((Array) resultFrame.UserData, srcOffset, (Array) dst, 0, count);
        }
        return dst;
      }
    }

    private void CheckLicence_ReadOnly(Manufacturer_FC FC, byte? EFC, byte[] parameterBytes)
    {
      if (parameterBytes == null)
        return;
      byte? nullable1;
      int num1;
      if (FC != Manufacturer_FC.ReadMemory_0x84)
      {
        int? nullable2;
        if (FC == Manufacturer_FC.CommonRadioCommands_0x2f)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num2 = 36;
          if (nullable2.GetValueOrDefault() == num2 & nullable2.HasValue)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num3 = 1;
          if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num4 = 2;
          if (nullable2.GetValueOrDefault() == num4 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num5 = 3;
          if (nullable2.GetValueOrDefault() == num5 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num6 = 4;
          if (nullable2.GetValueOrDefault() == num6 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num7 = 5;
          if (nullable2.GetValueOrDefault() == num7 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num8 = 16;
          if (nullable2.GetValueOrDefault() == num8 & nullable2.HasValue && parameterBytes.Length == 4)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num9 = 17;
          if (nullable2.GetValueOrDefault() == num9 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num10 = 19;
          if (nullable2.GetValueOrDefault() == num10 & nullable2.HasValue && parameterBytes.Length == 1)
            goto label_26;
        }
        if (FC == Manufacturer_FC.CommonMBusCommands_0x34)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num11 = 21;
          if (nullable2.GetValueOrDefault() == num11 & nullable2.HasValue && parameterBytes.Length == 3)
            goto label_26;
        }
        if (FC == Manufacturer_FC.SpecialCommands_0x36)
        {
          nullable1 = EFC;
          nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
          int num12 = 10;
          if (nullable2.GetValueOrDefault() == num12 & nullable2.HasValue)
          {
            num1 = parameterBytes.Length == 1 ? 1 : 0;
            goto label_27;
          }
        }
        num1 = 0;
        goto label_27;
      }
label_26:
      num1 = 1;
label_27:
      if (num1 == 0 && UserManager.CheckPermission(UserManager.Right_ReadOnly))
      {
        string[] strArray = new string[5]
        {
          "Access denied! Command (FC): ",
          FC.ToString(),
          ", EFC: ",
          null,
          null
        };
        nullable1 = EFC;
        strArray[3] = nullable1.ToString();
        strArray[4] = " is not allowed on 'ReadOnly'.";
        throw new AccessDeniedException("Right\\ReadOnly", string.Concat(strArray));
      }
    }

    private string checkForNACKValue(int iCaseValue)
    {
      string str;
      switch (iCaseValue)
      {
        case 0:
          str = NACK_Messages.Unknown_function.ToString();
          break;
        case 1:
          str = NACK_Messages.Illegal_reduced_device_information.ToString();
          break;
        case 2:
          str = NACK_Messages.Illegal_request_frame_lenght.ToString();
          break;
        case 3:
          str = NACK_Messages.Parameter_error.ToString();
          break;
        case 4:
          str = NACK_Messages.Not_classified_error.ToString();
          break;
        case 5:
          str = NACK_Messages.Access_denied.ToString();
          break;
        case 6:
          str = NACK_Messages.Wrong_activation_code.ToString();
          break;
        case 7:
          str = NACK_Messages.Invalid_encryption_key.ToString();
          break;
        default:
          str = NACK_Messages.Illegal_NACK_code.ToString();
          break;
      }
      return str;
    }

    public async Task SetWriteProtectionAsync(
      byte[] key,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (key.Length != 4)
        throw new ArgumentException("Wrong Keylenght was set.");
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.SetWriteProtection_0x81, key, progress, token);
    }

    public async Task OpenWriteProtectionTemporarilyAsync(
      byte[] key,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (key.Length != 4 && key.Length != 8)
        throw new ArgumentException("Wrong Keylenght was set.");
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.OpenWriteProtectionTemp_0x82, key, progress, token);
    }

    public async Task SetModeAsync(byte mode, ProgressHandler progress, CancellationToken token)
    {
      byte[] data = new byte[1]{ mode };
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.GetSetMode_0x83, data, progress, token);
      data = (byte[]) null;
    }

    public async Task<byte> GetModeAsync(ProgressHandler progress, CancellationToken token)
    {
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetSetMode_0x83, progress, token);
      byte modeAsync = result[0];
      result = (byte[]) null;
      return modeAsync;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte count)
    {
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, address, count)), token);
      return numArray;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      AddressRange addressRange,
      byte maxBytesPerPacket)
    {
      if (addressRange == null)
        throw new NullReferenceException(nameof (addressRange));
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, addressRange.StartAddress, addressRange.ByteSize, maxBytesPerPacket)), token);
      return numArray;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      uint count,
      byte maxBytesPerPacket)
    {
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, address, count, maxBytesPerPacket)), token);
      return numArray;
    }

    public byte[] ReadMemory(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      uint count,
      byte maxBytesPerPacket)
    {
      if (count < 0U)
        throw new ArgumentOutOfRangeException(nameof (count));
      int subParts = maxBytesPerPacket >= (byte) 0 ? Convert.ToInt32(count / (uint) maxBytesPerPacket) : throw new ArgumentOutOfRangeException(nameof (maxBytesPerPacket));
      if (subParts <= 1 && count <= (uint) maxBytesPerPacket)
        return this.ReadMemory(progress, token, address, (byte) count);
      byte num = (byte) (count % (uint) maxBytesPerPacket);
      if (num > (byte) 0)
        ++subParts;
      List<byte> byteList = new List<byte>((int) count);
      uint address1 = address;
      byte count1 = maxBytesPerPacket;
      if (progress != null)
      {
        progress.Split(new double[2]{ 10.0, 90.0 });
        progress.Split(subParts);
        progress.Report("Read: 0x" + address.ToString("X4") + " " + byteList.Count.ToString() + " byte(s)");
      }
      for (int index = 1; index <= subParts; ++index)
      {
        if (index > 1)
          address1 += (uint) count1;
        count1 = (long) (index * (int) maxBytesPerPacket) >= (long) count ? (num > (byte) 0 ? num : maxBytesPerPacket) : maxBytesPerPacket;
        byte[] collection = this.ReadMemory(progress, token, address1, count1);
        byteList.AddRange((IEnumerable<byte>) collection);
      }
      return byteList.ToArray();
    }

    internal byte[] ReadMemory(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte count)
    {
      if (count <= (byte) 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.Add(count);
      byte[] data = this.TransmitAndGetData(Manufacturer_FC.ReadMemory_0x84, new byte?(), byteList.ToArray(), false, progress, token);
      if (data.Length != (int) count)
        throw new Exception("Invalid response by read the memory! Expected: " + count.ToString() + " bytes but receive: " + data.Length.ToString() + " byte(s)");
      return data;
    }

    public async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte[] buffer)
    {
      await Task.Run((Action) (() => this.WriteMemory(progress, token, address, buffer)), token);
    }

    public async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      AddressRange addressRange,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      if (addressRange == null)
        throw new NullReferenceException(nameof (addressRange));
      await Task.Run((Action) (() => this.WriteMemory(progress, token, addressRange.StartAddress, buffer, maxBytesPerPacket)), token);
    }

    public async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      await Task.Run((Action) (() => this.WriteMemory(progress, token, address, buffer, maxBytesPerPacket)), token);
    }

    private void WriteMemory(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentException(nameof (buffer));
      if (maxBytesPerPacket < (byte) 0)
        throw new ArgumentOutOfRangeException(nameof (maxBytesPerPacket));
      int int32 = Convert.ToInt32(buffer.Length / (int) maxBytesPerPacket);
      if (int32 <= 1 && buffer.Length <= (int) maxBytesPerPacket)
      {
        this.WriteMemory(progress, token, address, buffer);
      }
      else
      {
        byte num = (byte) ((uint) buffer.Length % (uint) maxBytesPerPacket);
        if (num > (byte) 0)
          ++int32;
        uint address1 = address;
        byte length = maxBytesPerPacket;
        int srcOffset = 0;
        progress.Split(int32 * 2 + 1);
        for (int index = 1; index <= int32; ++index)
        {
          if (index > 1)
            address1 += (uint) length;
          length = index * (int) maxBytesPerPacket >= buffer.Length ? (num > (byte) 0 ? num : maxBytesPerPacket) : maxBytesPerPacket;
          byte[] numArray = new byte[(int) length];
          Buffer.BlockCopy((Array) buffer, srcOffset, (Array) numArray, 0, numArray.Length);
          this.WriteMemory(progress, token, address1, numArray);
          srcOffset += numArray.Length;
        }
        if (srcOffset != buffer.Length)
          throw new Exception("Write memory failed! Written number of bytes is incorrect. Expected: " + buffer.Length.ToString() + ", Actual: " + srcOffset.ToString());
      }
    }

    public void WriteMemory(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      byte[] buffer)
    {
      if (address == 0U)
        throw new ArgumentException(nameof (address));
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      token.ThrowIfCancellationRequested();
      progress?.Split(new double[2]{ 10.0, 90.0 });
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.AddRange((IEnumerable<byte>) buffer);
      this.TransmitAndCheckAck(Manufacturer_FC.WriteMemory_0x85, new byte?(), byteList.ToArray(), progress, token);
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.RunBackup_0x86, progress, token);
    }

    public async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.ResetDevice_0x80, progress, token);
      ConfigList cfg = this.DeviceCMD.MBus.Repeater.Port.GetReadoutConfiguration();
      if (cfg.Baudrate != 9600)
      {
        cfg = (ConfigList) null;
      }
      else
      {
        await Task.Delay(2500, token);
        this.DeviceCMD.MBus.Repeater.Port.DiscardInBuffer();
        this.DeviceCMD.MBus.Repeater.Port.ForceWakeup();
        cfg = (ConfigList) null;
      }
    }

    public void ResetDevice(ProgressHandler progress, CancellationToken token)
    {
      this.TransmitAndCheckAck(Manufacturer_FC.ResetDevice_0x80, new byte?(), (byte[]) null, progress, token);
      Thread.Sleep(2500);
      if (this.DeviceCMD.MBus.Repeater.Port.GetReadoutConfiguration().Baudrate != 9600)
        return;
      this.DeviceCMD.MBus.Repeater.Port.DiscardInBuffer();
      this.DeviceCMD.MBus.Repeater.Port.ForceWakeup();
    }

    public async Task SetSystemTimeAsync(
      byte year,
      byte month,
      byte day,
      byte hour,
      byte min,
      byte sec,
      sbyte tz,
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.SystemTime sysTime = new Common32BitCommands.SystemTime();
      sysTime.Year = year;
      sysTime.Month = month;
      sysTime.Day = day;
      sysTime.Hour = hour;
      sysTime.Min = min;
      sysTime.Sec = sec;
      sysTime.TimeZone = tz;
      await this.SetSystemTimeAsync(sysTime, progress, token);
      sysTime = (Common32BitCommands.SystemTime) null;
    }

    public async Task SetSystemTimeAsync(
      Common32BitCommands.SystemTime sysTime,
      ProgressHandler progress,
      CancellationToken token)
    {
      List<byte> myCommand = new List<byte>();
      myCommand.Add(sysTime.Year);
      myCommand.Add(sysTime.Month);
      myCommand.Add(sysTime.Day);
      myCommand.Add(sysTime.Hour);
      myCommand.Add(sysTime.Min);
      myCommand.Add(sysTime.Sec);
      myCommand.Add((byte) ((uint) sysTime.TimeZone * 4U));
      byte[] data = myCommand.ToArray();
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.GetSetSystemTime_0x87, data, progress, token);
      myCommand = (List<byte>) null;
      data = (byte[]) null;
    }

    public async Task<Common32BitCommands.SystemTime> GetSystemTimeAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetSetSystemTime_0x87, progress, token);
      Common32BitCommands.SystemTime sysTime = new Common32BitCommands.SystemTime();
      sysTime.Year = result[0];
      sysTime.Month = result[1];
      sysTime.Day = result[2];
      sysTime.Hour = result[3];
      sysTime.Min = result[4];
      sysTime.Sec = result[5];
      sysTime.TimeZone = (sbyte) ((int) result[6] / 4);
      sysTime.basedata = result;
      Common32BitCommands.SystemTime systemTimeAsync = sysTime;
      result = (byte[]) null;
      sysTime = (Common32BitCommands.SystemTime) null;
      return systemTimeAsync;
    }

    public async Task SetKeyDateAsync(
      byte month,
      byte dayOfMonth,
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.KeyDate kDate = new Common32BitCommands.KeyDate();
      kDate.Month = month;
      kDate.DayOfMonth = dayOfMonth;
      kDate.FirstYear = byte.MaxValue;
      await this.SetKeyDateAsync(kDate, progress, token);
      kDate = (Common32BitCommands.KeyDate) null;
    }

    public async Task SetKeyDateAsync(
      Common32BitCommands.KeyDate kDate,
      ProgressHandler progress,
      CancellationToken token)
    {
      List<byte> myCommand = new List<byte>();
      myCommand.Add(kDate.Month);
      myCommand.Add(kDate.DayOfMonth);
      myCommand.Add(kDate.FirstYear);
      byte[] data = myCommand.ToArray();
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.GetSetKeyDate_0x88, data, progress, token);
      myCommand = (List<byte>) null;
      data = (byte[]) null;
    }

    public async Task<Common32BitCommands.KeyDate> GetKeyDateAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetSetKeyDate_0x88, progress, token);
      Common32BitCommands.KeyDate kDate = new Common32BitCommands.KeyDate();
      if (result.Length == 3 || result.Length == 2)
      {
        kDate.Month = result[0];
        kDate.DayOfMonth = result[1];
        if (result.Length == 3)
          kDate.FirstYear = result[2];
        kDate.basedata = result;
      }
      else
      {
        kDate.Month = (byte) 0;
        kDate.DayOfMonth = (byte) 0;
        kDate.FirstYear = (byte) 0;
      }
      Common32BitCommands.KeyDate keyDateAsync = kDate;
      result = (byte[]) null;
      kDate = (Common32BitCommands.KeyDate) null;
      return keyDateAsync;
    }

    public async Task SetRadioOperationAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] data = new byte[1]{ state };
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.GetSetRadioOperation_0x89, data, progress, token);
      data = (byte[]) null;
    }

    public async Task<byte> GetRadioOperationAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetSetRadioOperation_0x89, progress, token);
      byte radioOperationAsync = result[0];
      result = (byte[]) null;
      return radioOperationAsync;
    }

    public bool GetRadioOperation(ProgressHandler progress, CancellationToken token)
    {
      return BitConverter.ToBoolean(this.TransmitAndGetData(Manufacturer_FC.GetSetRadioOperation_0x89, new byte?(), (byte[]) null, false, progress, token), 0);
    }

    public async Task ClearResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      byte[] data = new byte[1];
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.ClearGetResetCounter_0x8a, data, progress, token);
      data = (byte[]) null;
    }

    public async Task<byte> GetResetCounterAsync(ProgressHandler progress, CancellationToken token)
    {
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.ClearGetResetCounter_0x8a, progress, token);
      byte resetCounterAsync = result[0];
      result = (byte[]) null;
      return resetCounterAsync;
    }

    public async Task SetLcdTestStateAsync(
      byte testState,
      byte[] ramData,
      ProgressHandler progress,
      CancellationToken token)
    {
      int ramSize = ramData != null ? ramData.Length : 0;
      int size = ramSize + 1;
      byte[] data = new byte[size];
      data[0] = testState;
      if (testState == byte.MaxValue)
        Buffer.BlockCopy((Array) ramData, 0, (Array) data, 1, ramData.Length);
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.SetLcdTestState_0x8b, data, progress, token);
      data = (byte[]) null;
    }

    public async Task SwitchLoRaLEDAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] data = new byte[1]{ state };
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.SwitchLoRaLED_0x8c, data, progress, token);
      data = (byte[]) null;
    }

    public async Task ActivateDeactivateDisplayAsync(
      byte state,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] data = new byte[1]{ state };
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.ActivateDeactivateDisplay_0x8d, data, progress, token);
      data = (byte[]) null;
    }

    public async Task TimeShiftAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.TimeShift_0x8e, data, progress, token);
    }

    public async Task ExecuteEventAsync(
      byte eventNumber,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] data = new byte[1]{ eventNumber };
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.ExecuteEvent_0x8f, data, progress, token);
      data = (byte[]) null;
    }

    public async Task SetRTC_Calibration(
      short calibrationClocks,
      ProgressHandler progress,
      CancellationToken token)
    {
      byte[] data = new byte[2];
      data = BitConverter.GetBytes(calibrationClocks);
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.SetRTCCalibration_0x90, data, progress, token);
      data = (byte[]) null;
    }

    public async Task<Common32BitCommands.Scenarios> GetCommunicationScenarioAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (GetCommunicationScenarioAsync));
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetCommunicationScenario_0x91, progress, token);
      Common32BitCommands.Scenarios scenarios = new Common32BitCommands.Scenarios();
      byte[] dataONE = new byte[2]{ result[1], result[0] };
      if (result.Length == 2)
      {
        scenarios.ScenarioOne = new ushort?(Convert.ToUInt16(ZENNER.CommonLibrary.Utility.ByteArrayToHexString(dataONE), 16));
        scenarios.basedata = dataONE;
      }
      if (result.Length == 4)
      {
        dataONE = new byte[2]{ result[1], result[0] };
        scenarios.ScenarioOne = new ushort?(Convert.ToUInt16(ZENNER.CommonLibrary.Utility.ByteArrayToHexString(dataONE), 16));
        byte[] dataTWO = new byte[2]{ result[3], result[2] };
        scenarios.ScenarioTwo = new ushort?(Convert.ToUInt16(ZENNER.CommonLibrary.Utility.ByteArrayToHexString(dataTWO), 16));
        scenarios.basedata = new byte[4]
        {
          result[1],
          result[0],
          result[3],
          result[2]
        };
        dataTWO = (byte[]) null;
      }
      Common32BitCommands.Scenarios communicationScenarioAsync = scenarios;
      result = (byte[]) null;
      scenarios = (Common32BitCommands.Scenarios) null;
      dataONE = (byte[]) null;
      return communicationScenarioAsync;
    }

    public Common32BitCommands.Scenarios GetCommunicationScenario(
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (GetCommunicationScenario));
      byte[] data = this.TransmitAndGetData(Manufacturer_FC.GetCommunicationScenario_0x91, new byte?(), (byte[]) null, false, progress, token);
      Common32BitCommands.Scenarios communicationScenario = new Common32BitCommands.Scenarios();
      communicationScenario.basedata = data;
      communicationScenario.ScenarioOne = new ushort?(BitConverter.ToUInt16(data, 0));
      if (data.Length == 4)
        communicationScenario.ScenarioTwo = new ushort?(BitConverter.ToUInt16(data, 2));
      return communicationScenario;
    }

    public async Task SetCommunicationScenarioAsync(
      byte[] scenario,
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (SetCommunicationScenarioAsync));
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.SetCommunicationScenario_0x92, scenario, progress, token);
    }

    public async Task<string> GetPrintedSerialNumberAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (GetPrintedSerialNumberAsync));
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetPrintedSerialNumber_0x93, progress, token);
      if (result.Length < 1 || result[result.Length - 1] > (byte) 0)
        throw new Exception("Illegal GetPrintedSerialNumber result");
      if (result.Length == 1)
        return string.Empty;
      return ZENNER.CommonLibrary.Utility.ZeroTerminatedAsciiStringToString(result) ?? throw new Exception("Illegal PrintedSerialNumber found");
    }

    public async Task SetLocalInterfaceEncryptionAsync(
      byte[] data,
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (SetLocalInterfaceEncryptionAsync));
      await this.TransmitAndCheckAckAsync(Manufacturer_FC.GetLocalInterfaceEncryption_0x94, data, progress, token);
    }

    public async Task<byte[]> GetLocalInterfaceEncryptionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      Common32BitCommands.Common32BitCommands_Logger.Trace(nameof (GetLocalInterfaceEncryptionAsync));
      byte[] result = await this.TransmitAndGetDataAsync(Manufacturer_FC.GetLocalInterfaceEncryption_0x94, progress, token);
      byte[] interfaceEncryptionAsync = result.Length >= 1 && result.Length <= 3 ? result : throw new Exception("Illegal GetLocalInterfaceEncryption result");
      result = (byte[]) null;
      return interfaceEncryptionAsync;
    }

    public class SystemTime : ReturnValue
    {
      public byte Year { get; set; }

      public byte Month { get; set; }

      public byte Day { get; set; }

      public byte Hour { get; set; }

      public byte Min { get; set; }

      public byte Sec { get; set; }

      public sbyte TimeZone { get; set; }

      public override string ToString()
      {
        return Convert.ToString((int) Convert.ToInt16(this.Year) + 2000) + "-" + Convert.ToInt16(this.Month).ToString("D2") + "-" + Convert.ToInt16(this.Day).ToString("D2") + " " + Convert.ToInt16(this.Hour).ToString("D2") + ":" + Convert.ToInt16(this.Min).ToString("D2") + ":" + Convert.ToInt16(this.Sec).ToString("D2") + " " + Convert.ToInt16(this.TimeZone).ToString("D2");
      }

      public SystemTime(DateTime dateTime, sbyte timezone)
      {
        this.Year = (byte) (dateTime.Year - 2000);
        this.Month = (byte) dateTime.Month;
        this.Day = (byte) dateTime.Day;
        this.Hour = (byte) dateTime.Hour;
        this.Min = (byte) dateTime.Minute;
        this.Sec = (byte) dateTime.Second;
        this.TimeZone = timezone;
      }

      public SystemTime()
      {
      }

      public DateTime GetAsDateTime()
      {
        return new DateTime((int) this.Year, (int) this.Month, (int) this.Day, (int) this.Hour, (int) this.Min, (int) this.Sec);
      }
    }

    public class KeyDate : ReturnValue
    {
      public byte Month { get; set; }

      public byte DayOfMonth { get; set; }

      public byte FirstYear { get; set; }
    }

    public class Scenarios : ReturnValue
    {
      public ushort? ScenarioOne { get; set; }

      public ushort? ScenarioTwo { get; set; }

      public Scenarios()
      {
        this.ScenarioOne = new ushort?();
        this.ScenarioTwo = new ushort?();
      }
    }
  }
}
