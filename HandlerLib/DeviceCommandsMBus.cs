// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceCommandsMBus
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using MBusLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceCommandsMBus : IZRCommand
  {
    private static Logger logger = LogManager.GetLogger(nameof (DeviceCommandsMBus));
    private DeviceVersionMBus connectedDeviceVersion;

    public MBus MBus { get; private set; }

    public DeviceVersionMBus ConnectedDeviceVersion
    {
      get => this.connectedDeviceVersion;
      set
      {
        this.connectedDeviceVersion = value;
        this.ConnectedReducedID = new byte[3];
        this.ConnectedReducedID[0] = this.connectedDeviceVersion.LongID[0];
        this.ConnectedReducedID[1] = this.connectedDeviceVersion.LongID[1];
        this.ConnectedReducedID[2] = (byte) 85;
        foreach (byte num in this.connectedDeviceVersion.LongID)
          this.ConnectedReducedID[2] += num;
      }
    }

    public byte[] ConnectedReducedID { get; private set; }

    public byte[] ConnectedProtectedID { get; private set; }

    public uint SeriesKey { get; private set; }

    public bool IsDeviceIdentified => this.ConnectedDeviceVersion != null;

    public DeviceCommandsMBus(IPort port)
    {
      this.MBus = port != null ? new MBus(port) : throw new ArgumentNullException(nameof (port));
    }

    public void SaveSeriesKey(uint key) => this.SeriesKey = key;

    public void ClearProtectedIdentification() => this.ConnectedProtectedID = (byte[]) null;

    public void SetProtectedIdentification(uint SeriesKey)
    {
      if (this.ConnectedReducedID == null)
        throw new HandlerMessageException("Protected identification can only be set if reduced identification available!");
      this.SeriesKey = SeriesKey;
      uint num1 = ((uint) this.ConnectedReducedID[0] << 8) + ((uint) this.ConnectedReducedID[1] << 16) + ((uint) this.ConnectedReducedID[2] << 24) + (uint) this.ConnectedReducedID[2];
      uint num2 = SeriesKey;
      uint num3 = SeriesKey;
      for (int index = 0; index < 8; ++index)
      {
        uint num4 = num3 + num1 + num2;
        num3 = num4 + num4;
        num1 >>= 1;
        num2 >>= 1;
      }
      this.ConnectedProtectedID = new byte[3];
      this.ConnectedProtectedID[0] = (byte) num3;
      this.ConnectedProtectedID[1] = (byte) (num3 >> 8);
      this.ConnectedProtectedID[2] = (byte) (num3 >> 16);
    }

    public virtual async Task<DeviceVersionMBus> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      DeviceVersionMBus deviceVersionMbus = await Task.Run<DeviceVersionMBus>((Func<DeviceVersionMBus>) (() => this.ReadVersion(progress, token)), token);
      return deviceVersionMbus;
    }

    public DeviceVersionMBus ReadVersion(ProgressHandler progress, CancellationToken token)
    {
      DeviceCommandsMBus.logger.Trace(nameof (ReadVersion));
      if (token != CancellationToken.None)
        token.ThrowIfCancellationRequested();
      if (progress != null)
      {
        progress.Split(new double[2]{ 10.0, 90.0 });
        progress.Report("Read version");
      }
      MBusFrame resultFrame = this.MBus.Repeater.GetResultFrame(new MBusFrame(new byte[5]
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 6
      }), progress, token);
      this.ConnectedDeviceVersion = resultFrame.Type == FrameType.LongFrame ? DeviceVersionMBus.Parse(resultFrame) : throw new Exception("Invalid M-Bus frame! Expected: LongFrame, Actual: " + resultFrame.Type.ToString());
      return this.ConnectedDeviceVersion;
    }

    public void SetIdentificationLikeInFirmware(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      DeviceCommandsMBus.logger.Trace(nameof (SetIdentificationLikeInFirmware));
      this.ConnectedDeviceVersion = new DeviceVersionMBus(serialNumberBCD, manufacturerCode, generation, mediumCode);
    }

    public static byte[] GetLongID(FixedDataHeader header)
    {
      byte[] byteArray = header.ToByteArray();
      byte[] dst = new byte[8];
      Buffer.BlockCopy((Array) byteArray, 0, (Array) dst, 0, 8);
      return dst;
    }

    public static byte[] GetLongID(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(serialNumberBCD));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(manufacturerCode));
      byteList.Add(generation);
      byteList.Add(mediumCode);
      return byteList.ToArray();
    }

    public static void CheckManufacturerResponse(MBusFrame response, byte[] requiredLongID)
    {
      byte[] numArray = response.Type == FrameType.LongFrame ? DeviceCommandsMBus.GetLongID(FixedDataHeader.Parse(response.UserData)) : throw new Exception("Long frame expected but not received");
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((int) numArray[index] != (int) requiredLongID[index])
          throw new Exception("Illegal device long ID");
      }
      if (response.UserData[12] != (byte) 15)
        throw new Exception("Illegal manufacturer DIF in receive frame");
    }

    public static byte[] Get_FC_EFC_AndData(MBusFrame response)
    {
      int count = response.UserData.Length - 13;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) response.UserData, 13, (Array) dst, 0, count);
      return dst;
    }

    public static void CheckManufacturerResponse(MBusFrameCrypt response, byte[] requiredLongID)
    {
      byte[] numArray = response.Type == FrameType.LongFrame ? DeviceCommandsMBus.GetLongID(FixedDataHeader.Parse(response.UserData)) : throw new Exception("Long frame expected but not received");
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((int) numArray[index] != (int) requiredLongID[index])
          throw new Exception("Illegal device long ID");
      }
      if (response.UserData[12] != (byte) 15)
        throw new Exception("Illegal manufacturer DIF in receive frame");
    }

    public static byte[] Get_FC_EFC_AndData(MBusFrameCrypt response)
    {
      int count = response.UserData.Length - 13;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) response.UserData, 13, (Array) dst, 0, count);
      return dst;
    }
  }
}
