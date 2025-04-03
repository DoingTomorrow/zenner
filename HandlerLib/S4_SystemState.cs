// Decompiled with JetBrains decompiler
// Type: HandlerLib.S4_SystemState
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public class S4_SystemState
  {
    private static List<S4_SystemState.ModeMapping> CommonModeMapping = new List<S4_SystemState.ModeMapping>();
    private uint sysState;
    private uint sysInfos;

    static S4_SystemState()
    {
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.OperationMode,
        IuwModes = S4_DeviceModes.OperationMode
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.DeliveryMode,
        IuwModes = S4_DeviceModes.OperationMode
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.StandbyCurrentMode,
        IuwModes = S4_DeviceModes.OperationMode
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RTC_CalibrationMode,
        IuwModes = S4_DeviceModes.RtcCalibrationTestStart
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RTC_CalibrationVerifyMode,
        IuwModes = S4_DeviceModes.RtcCalibrationTestStart
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.UltrasonicLevelTest,
        IuwModes = S4_DeviceModes.TdcLevelTest
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.LcdTest,
        IuwModes = S4_DeviceModes.LcdTest
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitUnmodulatedCarrier,
        IuwModes = S4_DeviceModes.RadioTestTransmitUnmodulatedCarrier
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RadioTestTransmitModulatedCarrier,
        IuwModes = S4_DeviceModes.RadioTestTransmitModulatedCarrier
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RadioTestReceiveTestPacket,
        IuwModes = S4_DeviceModes.RadioTestReceiveTestPacket
      });
      S4_SystemState.CommonModeMapping.Add(new S4_SystemState.ModeMapping()
      {
        DeviceModes = HandlerFunctionsForProduction.CommonDeviceModes.RadioTestSendTestPacket,
        IuwModes = S4_DeviceModes.RadioTestSendTestPacket
      });
    }

    public S4_SystemState(byte[] receiveFrame)
    {
      int offset = 2;
      this.sysState = ByteArrayScanner.ScanUInt32(receiveFrame, ref offset);
      this.NfcPowerfail = new uint?((uint) ByteArrayScanner.ScanUInt16(receiveFrame, ref offset));
      this.sysInfos = ByteArrayScanner.ScanUInt32(receiveFrame, ref offset);
      int count = receiveFrame.Length - offset;
      if (count <= 0)
        return;
      this.ModeResultData = new byte[count];
      Buffer.BlockCopy((Array) receiveFrame, offset, (Array) this.ModeResultData, 0, count);
    }

    public S4_SystemState(uint systemInfo) => this.sysInfos = systemInfo;

    public byte[] ModeResultData { get; private set; }

    public SystemInfo SysInfo => (SystemInfo) this.sysInfos;

    public bool IsWriteProtected => S4_SystemState.GetWriteProtection(this.sysState);

    public bool IsMeterKeyDefined => S4_SystemState.GetMeterKeyDefined(this.sysState);

    public static bool GetWriteProtection(uint sysState) => (sysState & 1024U) > 0U;

    public static bool GetMeterKeyDefined(uint sysState) => (sysState & 512U) > 0U;

    public bool IsCRCRunning() => S4_SystemState.GetIsCRCRunning(this.sysState);

    public static bool GetIsCRCRunning(uint sysState) => (sysState & 32768U) > 0U;

    public bool IsTestModeEnabled
    {
      get => (this.DeviceMode & S4_DeviceModes.TestModePrepared) == S4_DeviceModes.TestModePrepared;
    }

    public S4_DeviceModes DeviceMode => (S4_DeviceModes) (byte) (this.sysState >> 16);

    public bool IsCommonDeviceModeActive(
      HandlerFunctionsForProduction.CommonDeviceModes commonDeviceMode)
    {
      S4_SystemState.ModeMapping modeMapping = S4_SystemState.CommonModeMapping.FirstOrDefault<S4_SystemState.ModeMapping>((Func<S4_SystemState.ModeMapping, bool>) (x => x.DeviceModes == commonDeviceMode));
      return modeMapping != null && this.DeviceMode == modeMapping.IuwModes;
    }

    public Test_State TestState => (Test_State) ((int) (this.sysState >> 16) & 15);

    public uint? NfcPowerfail { get; private set; }

    public override string ToString()
    {
      StringBuilder info = new StringBuilder();
      for (uint index = 1; index > 0U; index <<= 1)
      {
        if ((index & this.sysInfos) > 0U && Enum.IsDefined(typeof (SystemInfo), (object) index))
          this.appendFlagToList(info, ((SystemInfo) index).ToString());
      }
      return info.ToString();
    }

    private void appendFlagToList(StringBuilder info, string flag)
    {
      if (info.Length > 0)
        info.Append(',');
      info.Append(flag);
    }

    public string ToTextBlock()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("DeviceMode : " + this.DeviceMode.ToString());
      stringBuilder.AppendLine("NfcPowerfails : " + this.NfcPowerfail.ToString());
      stringBuilder.AppendLine("SysteInfoBits :");
      for (uint index = 1; index > 0U; index <<= 1)
      {
        if ((index & this.sysInfos) > 0U && Enum.IsDefined(typeof (SystemInfo), (object) index))
          stringBuilder.AppendLine("   " + ((SystemInfo) index).ToString());
      }
      return stringBuilder.ToString();
    }

    [Flags]
    public enum sysStateBits : uint
    {
      STATUS_METERKEY_IS_SET = 512, // 0x00000200
      STATUS_DEVICE_IS_PROTECTED = 1024, // 0x00000400
      STATUS_DEVICE_SLEEP = 2048, // 0x00000800
      STATUS_CRC_CHECK_IS_RUNNING = 32768, // 0x00008000
      STATUS_DEFBACKUP_AT_PROTECTION = 16384, // 0x00004000
      STATUS_BATTERY_DOWN = 8192, // 0x00002000
      STATUS_PVD_LOW_BAT = 4096, // 0x00001000
      STATUS_DEVICE_SLEEP_REQUEST = 256, // 0x00000100
    }

    private class ModeMapping
    {
      internal HandlerFunctionsForProduction.CommonDeviceModes DeviceModes;
      internal S4_DeviceModes IuwModes;
    }
  }
}
