// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GMMSettings
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public class GMMSettings
  {
    private static Logger logger = LogManager.GetLogger(nameof (GMMSettings));

    public GMMSettingsName Name { get; set; }

    public List<string> AsyncComSettingsList { get; set; }

    public List<string> DeviceCollectorSettingsList { get; set; }

    public List<string> ExpectedDevicesList { get; set; }

    public ReadoutType ReadoutType { get; set; }

    public bool IsMBusSettings
    {
      get
      {
        SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> collectorSettings = this.DeviceCollectorSettings;
        if (collectorSettings == null || !collectorSettings.ContainsKey(ZR_ClassLibrary.DeviceCollectorSettings.BusMode))
          return false;
        BusMode busMode = (BusMode) Enum.Parse(typeof (BusMode), collectorSettings[ZR_ClassLibrary.DeviceCollectorSettings.BusMode].ToString(), true);
        return busMode == BusMode.MBus || busMode == BusMode.MBusPointToPoint;
      }
    }

    public BusMode Mode
    {
      get
      {
        switch (this.Name)
        {
          case GMMSettingsName.None:
            return BusMode.MBus;
          case GMMSettingsName.MinolDevice_IrMinoHead:
          case GMMSettingsName.MinolDevice_IrCombiHeadDoveTailSide_MinoConnect:
            return BusMode.Minol_Device;
          case GMMSettingsName.Radio2_MinoHead:
          case GMMSettingsName.Radio2_MinoConnect:
            return BusMode.Radio2;
          case GMMSettingsName.Radio3_MinoHead:
          case GMMSettingsName.Radio3_MinoConnect:
            return BusMode.Radio3;
          case GMMSettingsName.MinomatV2_IrCombiHeadDoveTailSide_MinoConnect:
          case GMMSettingsName.MinomatV2_IrMinoHead:
            return BusMode.MinomatV2;
          case GMMSettingsName.MinomatV3_IrMinoHead:
          case GMMSettingsName.MinomatV3_IrCombiHeadRoundSide_MinoConnect:
            return BusMode.MinomatV3;
          case GMMSettingsName.MinomatV4_IrMinoHead:
          case GMMSettingsName.MinomatV4_IrCombiHeadDoveTailSide_MinoConnect:
            return BusMode.MinomatV4;
          case GMMSettingsName.RelayDevice_MinoConnect:
            return BusMode.RelayDevice;
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_Break_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect_EDC:
            return BusMode.MBusPointToPoint;
          case GMMSettingsName.Wavenis:
            return BusMode.WaveFlowRadio;
          case GMMSettingsName.WirelessMBusModeS1_MinoConnect:
            return BusMode.wMBusS1;
          case GMMSettingsName.WirelessMBusModeC1A_MinoConnect:
            return BusMode.wMBusC1A;
          case GMMSettingsName.WirelessMBusModeC1B_MinoConnect:
            return BusMode.wMBusC1B;
          case GMMSettingsName.WirelessMBusModeS1M_MinoConnect:
            return BusMode.wMBusS1M;
          case GMMSettingsName.WirelessMBusModeS2_MinoConnect:
            return BusMode.wMBusS2;
          case GMMSettingsName.WirelessMBusModeT1_MinoConnect:
            return BusMode.wMBusT1;
          case GMMSettingsName.WirelessMBusModeT2_meter_MinoConnect:
            return BusMode.wMBusT2_meter;
          case GMMSettingsName.WirelessMBusModeT2_other_MinoConnect:
            return BusMode.wMBusT2_other;
          case GMMSettingsName.Radio3_868_95_RUSSIA_MinoConnect:
            return BusMode.Radio3_868_95_RUSSIA;
          case GMMSettingsName.ModeMinomatRadioTest_MinoConnect:
            return BusMode.MinomatRadioTest;
          case GMMSettingsName.ModeRadioMS_MinoConnect:
            return BusMode.RadioMS;
          case GMMSettingsName.SmokeDetector_IrCombiHeadDoveTailSide_MinoConnect:
            return BusMode.SmokeDetector;
          default:
            throw new ArgumentException("Not supported bus mode: " + this.Name.ToString());
        }
      }
    }

    public TransceiverDevice Transceiver
    {
      get
      {
        switch (this.Name)
        {
          case GMMSettingsName.None:
          case GMMSettingsName.MBus_MeterVPN:
            return TransceiverDevice.None;
          case GMMSettingsName.MinolDevice_IrMinoHead:
          case GMMSettingsName.Radio2_MinoHead:
          case GMMSettingsName.Radio3_MinoHead:
          case GMMSettingsName.MinomatV2_IrMinoHead:
          case GMMSettingsName.MinomatV3_IrMinoHead:
          case GMMSettingsName.MinomatV4_IrMinoHead:
            return TransceiverDevice.MinoHead;
          case GMMSettingsName.Radio2_MinoConnect:
          case GMMSettingsName.Radio3_MinoConnect:
          case GMMSettingsName.MinolDevice_IrCombiHeadDoveTailSide_MinoConnect:
          case GMMSettingsName.MinomatV2_IrCombiHeadDoveTailSide_MinoConnect:
          case GMMSettingsName.MinomatV3_IrCombiHeadRoundSide_MinoConnect:
          case GMMSettingsName.MinomatV4_IrCombiHeadDoveTailSide_MinoConnect:
          case GMMSettingsName.RelayDevice_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_Break_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect:
          case GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect_EDC:
          case GMMSettingsName.WirelessMBusModeS1_MinoConnect:
          case GMMSettingsName.WirelessMBusModeC1A_MinoConnect:
          case GMMSettingsName.WirelessMBusModeC1B_MinoConnect:
          case GMMSettingsName.WirelessMBusModeS1M_MinoConnect:
          case GMMSettingsName.WirelessMBusModeS2_MinoConnect:
          case GMMSettingsName.WirelessMBusModeT1_MinoConnect:
          case GMMSettingsName.WirelessMBusModeT2_meter_MinoConnect:
          case GMMSettingsName.WirelessMBusModeT2_other_MinoConnect:
          case GMMSettingsName.Radio3_868_95_RUSSIA_MinoConnect:
          case GMMSettingsName.ModeMinomatRadioTest_MinoConnect:
          case GMMSettingsName.ModeRadioMS_MinoConnect:
          case GMMSettingsName.SmokeDetector_IrCombiHeadDoveTailSide_MinoConnect:
            return TransceiverDevice.MinoConnect;
          case GMMSettingsName.Wavenis:
            return TransceiverDevice.Wavenis;
          default:
            throw new ArgumentException("Unknown Transceiver type. Value: " + this.Name.ToString());
        }
      }
    }

    public GMMSettings()
    {
      this.AsyncComSettingsList = new List<string>();
      this.DeviceCollectorSettingsList = new List<string>();
      this.ExpectedDevicesList = new List<string>();
      this.Name = GMMSettingsName.None;
    }

    public GMMSettings(string settings)
      : this()
    {
      Dictionary<string, string> parametersAsList = ParameterService.GetAllParametersAsList(settings, ';');
      List<string> stringList1 = new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (ZR_ClassLibrary.DeviceCollectorSettings)));
      List<string> stringList2 = new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (ZR_ClassLibrary.AsyncComSettings)));
      foreach (KeyValuePair<string, string> keyValuePair in parametersAsList)
      {
        if (stringList1.Contains(keyValuePair.Key))
          this.DeviceCollectorSettingsList.Add(keyValuePair.Key + "=" + keyValuePair.Value);
        if (stringList2.Contains(keyValuePair.Key))
          this.AsyncComSettingsList.Add(keyValuePair.Key + "=" + keyValuePair.Value);
      }
    }

    public GMMSettings(
      SortedList<string, string> asyncComSettings,
      SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> deviceCollectorSettings)
      : this()
    {
      this.SetAsyncComSettings(asyncComSettings);
      this.SetDeviceCollectorSettings(deviceCollectorSettings);
    }

    public static GMMSettings Default_MinolDevice_IrCombiHeadDoveTailSide_MinoConnect
    {
      get
      {
        GMMSettings tailSideMinoConnect = new GMMSettings();
        tailSideMinoConnect.Name = GMMSettingsName.MinolDevice_IrCombiHeadDoveTailSide_MinoConnect;
        tailSideMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=0;ForceMinoConnectState=IrCombiHead;IrDaSelection=DoveTailSide;Parity=even;Port=COM1;MinoConnectPowerOffTime=3600;RecTime_BeforFirstByte=500;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=0;RecTransTime=10;TestEcho=False;EchoOn=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=200;TransTime_BreakTime=0;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=100;Wakeup=None;MinoConnectIrDaPulseTime=55;RecTime_OffsetPerBlock=50".Split(';'));
        tailSideMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=Minol_Device;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=3;".Split(';'));
        return tailSideMinoConnect;
      }
    }

    public static GMMSettings Default_MinomatV2_IrCombiHeadDoveTailSide_MinoConnect
    {
      get
      {
        GMMSettings tailSideMinoConnect = new GMMSettings();
        tailSideMinoConnect.Name = GMMSettingsName.MinomatV2_IrCombiHeadDoveTailSide_MinoConnect;
        tailSideMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=1500;EchoOn=False;ForceMinoConnectState=IrCombiHead;HardwareHandshake=True;IrDaSelection=DoveTailSide;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=600;TransTime_BreakTime=1400;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=100;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150".Split(';'));
        tailSideMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV2;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;DaKonId=48000066;MaxRequestRepeat=3;".Split(';'));
        return tailSideMinoConnect;
      }
    }

    public static GMMSettings Default_MinomatV4_IrCombiHeadDoveTailSide_MinoConnect
    {
      get
      {
        GMMSettings tailSideMinoConnect = new GMMSettings();
        tailSideMinoConnect.Name = GMMSettingsName.MinomatV4_IrCombiHeadDoveTailSide_MinoConnect;
        tailSideMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;EchoOn=False;ForceMinoConnectState=IrCombiHead;HardwareHandshake=True;IrDaSelection=DoveTailSide;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=0;RecTransTime=0;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=0;TransTime_BreakTime=0;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=0;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=500;".Split(';'));
        tailSideMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV4;FromTime=2011-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=4;".Split(';'));
        return tailSideMinoConnect;
      }
    }

    public static GMMSettings Default_MinomatV3_IrCombiHeadRoundSide_MinoConnect
    {
      get
      {
        GMMSettings roundSideMinoConnect = new GMMSettings();
        roundSideMinoConnect.Name = GMMSettingsName.MinomatV3_IrCombiHeadRoundSide_MinoConnect;
        roundSideMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;BusMode=MinomatV3;EchoOn=False;ForceMinoConnectState=IrCombiHead;HardwareHandshake=True;IrDaSelection=RoundSide;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=0;RecTransTime=0;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=0;TransTime_BreakTime=0;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=0;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=500".Split(';'));
        roundSideMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV3;FromTime=2011-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=4;".Split(';'));
        return roundSideMinoConnect;
      }
    }

    public static GMMSettings Default_MinomatV2_IrMinoHead
    {
      get
      {
        GMMSettings minomatV2IrMinoHead = new GMMSettings();
        minomatV2IrMinoHead.Name = GMMSettingsName.MinomatV2_IrMinoHead;
        minomatV2IrMinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=10000;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=600;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150".Split(';'));
        minomatV2IrMinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV2;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;DaKonId=48000066;MaxRequestRepeat=3;".Split(';'));
        return minomatV2IrMinoHead;
      }
    }

    public static GMMSettings Default_MinomatV3_IrMinoHead
    {
      get
      {
        GMMSettings minomatV3IrMinoHead = new GMMSettings();
        minomatV3IrMinoHead.Name = GMMSettingsName.MinomatV3_IrMinoHead;
        minomatV3IrMinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=600;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=500".Split(';'));
        minomatV3IrMinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV3;FromTime=2011-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=4;".Split(';'));
        return minomatV3IrMinoHead;
      }
    }

    public static GMMSettings Default_MinomatV4_IrMinoHead
    {
      get
      {
        GMMSettings minomatV4IrMinoHead = new GMMSettings();
        minomatV4IrMinoHead.Name = GMMSettingsName.MinomatV4_IrMinoHead;
        minomatV4IrMinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=2000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=600;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=500".Split(';'));
        minomatV4IrMinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MinomatV4;FromTime=2011-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=4;".Split(';'));
        return minomatV4IrMinoHead;
      }
    }

    public static GMMSettings Default_RelayDevice_MinoConnect
    {
      get
      {
        GMMSettings deviceMinoConnect = new GMMSettings();
        deviceMinoConnect.Name = GMMSettingsName.RelayDevice_MinoConnect;
        deviceMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=8000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=False;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=1000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=20;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=300;TransTime_BreakTime=800;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=600;Wakeup=None;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150".Split(';'));
        deviceMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=RelayDevice;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=2;".Split(';'));
        return deviceMinoConnect;
      }
    }

    public static GMMSettings Default_Radio2_MinoHead
    {
      get
      {
        GMMSettings defaultRadio2MinoHead = new GMMSettings();
        defaultRadio2MinoHead.Name = GMMSettingsName.Radio2_MinoHead;
        defaultRadio2MinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        defaultRadio2MinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.Radio2.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return defaultRadio2MinoHead;
      }
    }

    public static GMMSettings Default_Radio3_MinoHead
    {
      get
      {
        GMMSettings defaultRadio3MinoHead = new GMMSettings();
        defaultRadio3MinoHead.Name = GMMSettingsName.Radio3_MinoHead;
        defaultRadio3MinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=0;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        defaultRadio3MinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.Radio3.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return defaultRadio3MinoHead;
      }
    }

    public static GMMSettings Default_MinolDevice_IrMinoHead
    {
      get
      {
        GMMSettings deviceIrMinoHead = new GMMSettings();
        deviceIrMinoHead.Name = GMMSettingsName.MinolDevice_IrMinoHead;
        deviceIrMinoHead.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=38400;BreakIntervalTime=10000;EchoOn=False;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=3100;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoHead;TransTime_AfterBreak=400;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=400;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=50".Split(';'));
        deviceIrMinoHead.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=Minol_Device;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=3;".Split(';'));
        return deviceIrMinoHead;
      }
    }

    public static GMMSettings Default_Radio2_MinoConnect
    {
      get
      {
        GMMSettings radio2MinoConnect = new GMMSettings();
        radio2MinoConnect.Name = GMMSettingsName.Radio2_MinoConnect;
        radio2MinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        radio2MinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.Radio2.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return radio2MinoConnect;
      }
    }

    public static GMMSettings Default_Radio3_MinoConnect
    {
      get
      {
        GMMSettings radio3MinoConnect = new GMMSettings();
        radio3MinoConnect.Name = GMMSettingsName.Radio3_MinoConnect;
        radio3MinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        radio3MinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.Radio3.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return radio3MinoConnect;
      }
    }

    public static GMMSettings Default_Radio3_868_95_RUSSIA_MinoConnect
    {
      get
      {
        GMMSettings russiaMinoConnect = new GMMSettings();
        russiaMinoConnect.Name = GMMSettingsName.Radio3_868_95_RUSSIA_MinoConnect;
        russiaMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        russiaMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.Radio3_868_95_RUSSIA.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return russiaMinoConnect;
      }
    }

    public static GMMSettings Default_MBusP2P_IrCombiHeadRoundSide_ZVEI_Break_MinoConnect
    {
      get
      {
        GMMSettings breakMinoConnect = new GMMSettings();
        breakMinoConnect.Name = GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_Break_MinoConnect;
        breakMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=2400;COMserver=-;Port=COM1;Parity=even;EchoOn=False;TestEcho=True;RecTime_BeforFirstByte=3000;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=200;WaitBeforeRepeatTime=200;BreakIntervalTime=10000;MinoConnectPowerOffTime=3600;Wakeup=Break;TransceiverDevice=MinoConnect;ForceMinoConnectState=IrCombiHead;IrDaSelection=None;HardwareHandshake=True;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150;MinoConnectBaseState=IrCombiHead".Split(';'));
        breakMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBusPointToPoint;SelectedDeviceMBusType=MBus;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=3;".Split(';'));
        return breakMinoConnect;
      }
    }

    public static GMMSettings Default_MBusP2P_IrCombiHeadRoundSide_ZVEI_MinoConnect
    {
      get
      {
        GMMSettings sideZveiMinoConnect = new GMMSettings();
        sideZveiMinoConnect.Name = GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_ZVEI_MinoConnect;
        sideZveiMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=2400;COMserver=-;Port=COM1;Parity=even;EchoOn=False;TestEcho=False;RecTime_BeforFirstByte=1600;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=200;WaitBeforeRepeatTime=200;BreakIntervalTime=10000;MinoConnectPowerOffTime=3600;Wakeup=None;TransceiverDevice=MinoConnect;ForceMinoConnectState=IrCombiHead;IrDaSelection=None;HardwareHandshake=True;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150;MinoConnectBaseState=IrCombiHead".Split(';'));
        sideZveiMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBusPointToPoint;SelectedDeviceMBusType=ZR_Serie2;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=2;".Split(';'));
        return sideZveiMinoConnect;
      }
    }

    public static GMMSettings Default_MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect
    {
      get
      {
        GMMSettings sideIrDaMinoConnect = new GMMSettings();
        sideIrDaMinoConnect.Name = GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect;
        sideIrDaMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=115200;COMserver=-;Port=COM1;Parity=even;EchoOn=False;TestEcho=False;RecTime_BeforFirstByte=500;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=200;WaitBeforeRepeatTime=200;BreakIntervalTime=10000;MinoConnectPowerOffTime=3600;Wakeup=BaudrateCarrier;TransceiverDevice=MinoConnect;ForceMinoConnectState=IrCombiHead;IrDaSelection=RoundSide;HardwareHandshake=True;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=150;MinoConnectBaseState=IrCombiHead".Split(';'));
        sideIrDaMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBusPointToPoint;SelectedDeviceMBusType=ZR_Serie3;IsMultiTelegrammEnabled=True;SendFirstSND_NKE=True;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=3;".Split(';'));
        return sideIrDaMinoConnect;
      }
    }

    public static GMMSettings Default_MBus_MeterVPN
    {
      get
      {
        GMMSettings defaultMbusMeterVpn = new GMMSettings();
        defaultMbusMeterVpn.Name = GMMSettingsName.MBus_MeterVPN;
        defaultMbusMeterVpn.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=Remote_VPN;Baudrate=2400;COMserver=;Port=COM1;Parity=even;EchoOn=False;TestEcho=True;RecTime_BeforFirstByte=2600;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=5000;WaitBeforeRepeatTime=400;BreakIntervalTime=10000;MinoConnectPowerOffTime=1000;Wakeup=None;TransceiverDevice=None;ForceMinoConnectState=;IrDaSelection=None;HardwareHandshake=False;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=1700;MinoConnectBaseState=off".Split(';'));
        defaultMbusMeterVpn.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBus;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;DaKonId=;Password=;MaxRequestRepeat=2;ScanStartAddress=0;ScanStartSerialnumber=fffffff0;OrganizeStartAddress=1;CycleTime=5;OnlySecondaryAddressing=False;FastSecondaryAddressing=True;KeepExistingDestinationAddress=False;ChangeInterfaceBaudrateToo=False;UseExternalKeyForReading=True;BeepSignalOnReadResult=False;LogToFileEnabled=False;LogFilePath=".Split(';'));
        return defaultMbusMeterVpn;
      }
    }

    public static GMMSettings Default_Wavenis
    {
      get
      {
        GMMSettings defaultWavenis = new GMMSettings();
        defaultWavenis.Name = GMMSettingsName.Wavenis;
        defaultWavenis.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=2400;COMserver=-;Port=COM1;Parity=even;EchoOn=False;TestEcho=False;RecTime_BeforFirstByte=1000;RecTime_OffsetPerByte=10;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=200;WaitBeforeRepeatTime=500;BreakIntervalTime=10000;MinoConnectPowerOffTime=300;Wakeup=None;TransceiverDevice=Wavenis;ForceMinoConnectState=;IrDaSelection=None;HardwareHandshake=False;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=70;MinoConnectBaseState=IrCombiHead".Split(';'));
        defaultWavenis.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=WaveFlowRadio;DaKonId=;FromTime=01.01.2000 01:01:01;ToTime=01.01.2030 01:01:01;Password=;MaxRequestRepeat=2;ScanStartAddress=0;ScanStartSerialnumber=fffffff0;OrganizeStartAddress=1;CycleTime=5;OnlySecondaryAddressing=False;FastSecondaryAddressing=True;KeepExistingDestinationAddress=False;ChangeInterfaceBaudrateToo=False;UseExternalKeyForReading=True;BeepSignalOnReadResult=False;LogToFileEnabled=False;LogFilePath=".Split(';'));
        return defaultWavenis;
      }
    }

    public static GMMSettings Default_MBus
    {
      get
      {
        GMMSettings defaultMbus = new GMMSettings();
        defaultMbus.Name = GMMSettingsName.MBus;
        defaultMbus.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=2400;Port=COM1;Parity=even;EchoOn=False;TestEcho=False;RecTime_BeforFirstByte=2000;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=10;TransTime_BreakTime=700;TransTime_AfterBreak=1000;WaitBeforeRepeatTime=400;BreakIntervalTime=1000;MinoConnectPowerOffTime=1000;Wakeup=None;TransceiverDevice=None;ForceMinoConnectState=;IrDaSelection=None;HardwareHandshake=False;RecTime_OffsetPerBlock=100;".Split(';'));
        defaultMbus.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBus;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=2;ScanStartAddress=0;ScanStartSerialnumber=fffffff0;OrganizeStartAddress=1;CycleTime=5;OnlySecondaryAddressing=False;FastSecondaryAddressing=True;KeepExistingDestinationAddress=False;ChangeInterfaceBaudrateToo=False;UseExternalKeyForReading=True;BeepSignalOnReadResult=False;LogToFileEnabled=False".Split(';'));
        return defaultMbus;
      }
    }

    public static GMMSettings Default_WirelessMBusModeS1_MinoConnect
    {
      get
      {
        GMMSettings modeS1MinoConnect = new GMMSettings();
        modeS1MinoConnect.Name = GMMSettingsName.WirelessMBusModeS1_MinoConnect;
        modeS1MinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeS1MinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusS1.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeS1MinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeC1A_MinoConnect
    {
      get
      {
        GMMSettings modeC1AMinoConnect = new GMMSettings();
        modeC1AMinoConnect.Name = GMMSettingsName.WirelessMBusModeC1A_MinoConnect;
        modeC1AMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeC1AMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusC1A.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeC1AMinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeC1B_MinoConnect
    {
      get
      {
        GMMSettings modeC1BMinoConnect = new GMMSettings();
        modeC1BMinoConnect.Name = GMMSettingsName.WirelessMBusModeC1B_MinoConnect;
        modeC1BMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeC1BMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusC1B.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeC1BMinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeS1M_MinoConnect
    {
      get
      {
        GMMSettings modeS1MMinoConnect = new GMMSettings();
        modeS1MMinoConnect.Name = GMMSettingsName.WirelessMBusModeS1M_MinoConnect;
        modeS1MMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeS1MMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusS1M.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeS1MMinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeS2_MinoConnect
    {
      get
      {
        GMMSettings modeS2MinoConnect = new GMMSettings();
        modeS2MinoConnect.Name = GMMSettingsName.WirelessMBusModeS2_MinoConnect;
        modeS2MinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeS2MinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusS2.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeS2MinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeT1_MinoConnect
    {
      get
      {
        GMMSettings modeT1MinoConnect = new GMMSettings();
        modeT1MinoConnect.Name = GMMSettingsName.WirelessMBusModeT1_MinoConnect;
        modeT1MinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        modeT1MinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusT1.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return modeT1MinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeT2_meter_MinoConnect
    {
      get
      {
        GMMSettings meterMinoConnect = new GMMSettings();
        meterMinoConnect.Name = GMMSettingsName.WirelessMBusModeT2_meter_MinoConnect;
        meterMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        meterMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusT2_meter.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return meterMinoConnect;
      }
    }

    public static GMMSettings Default_WirelessMBusModeT2_other_MinoConnect
    {
      get
      {
        GMMSettings otherMinoConnect = new GMMSettings();
        otherMinoConnect.Name = GMMSettingsName.WirelessMBusModeT2_other_MinoConnect;
        otherMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        otherMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.wMBusT2_other.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return otherMinoConnect;
      }
    }

    public static GMMSettings Default_ModeRadioMS_MinoConnect
    {
      get
      {
        GMMSettings radioMsMinoConnect = new GMMSettings();
        radioMsMinoConnect.Name = GMMSettingsName.ModeRadioMS_MinoConnect;
        radioMsMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        radioMsMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.RadioMS.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;").Split(';'));
        return radioMsMinoConnect;
      }
    }

    public static GMMSettings Default_ModeMinomatRadioTest_MinoConnect
    {
      get
      {
        GMMSettings radioTestMinoConnect = new GMMSettings();
        radioTestMinoConnect.Name = GMMSettingsName.ModeMinomatRadioTest_MinoConnect;
        radioTestMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;EchoOn=False;ForceMinoConnectState=RS232;HardwareHandshake=false;IrDaSelection=None;MinoConnectPowerOffTime=3600;Parity=no;Port=COM1;RecTime_BeforFirstByte=4000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=10;RecTransTime=10;TestEcho=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=1000;TransTime_BreakTime=700;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=500;Wakeup=MinoHead;MinoConnectIrDaPulseTime=0".Split(';'));
        radioTestMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) ("BusMode=" + BusMode.MinomatRadioTest.ToString() + ";FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;DaKonId=250;").Split(';'));
        return radioTestMinoConnect;
      }
    }

    public static GMMSettings Default_MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect_EDC
    {
      get
      {
        GMMSettings daMinoConnectEdc = new GMMSettings();
        daMinoConnectEdc.Name = GMMSettingsName.MBusP2P_IrCombiHeadRoundSide_IrDa_MinoConnect_EDC;
        daMinoConnectEdc.AsyncComSettingsList.AddRange((IEnumerable<string>) "Type=COM;Baudrate=9600;COMserver=-;Port=COM1;Parity=even;EchoOn=False;TestEcho=False;RecTime_BeforFirstByte=1000;RecTime_OffsetPerByte=0;RecTime_GlobalOffset=0;TransTime_GlobalOffset=0;RecTransTime=30;TransTime_BreakTime=2592;TransTime_AfterBreak=50;WaitBeforeRepeatTime=200;BreakIntervalTime=10000;MinoConnectPowerOffTime=3600;Wakeup=BaudrateCarrier;TransceiverDevice=MinoConnect;ForceMinoConnectState=IrCombiHead;IrDaSelection=RoundSide;HardwareHandshake=False;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=50;MinoConnectBaseState=IrCombiHead".Split(';'));
        daMinoConnectEdc.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=MBusPointToPoint;SelectedDeviceMBusType=EDC;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=3;".Split(';'));
        return daMinoConnectEdc;
      }
    }

    public static GMMSettings Default_SmokeDetector_IrCombiHeadDoveTailSide_MinoConnect
    {
      get
      {
        GMMSettings tailSideMinoConnect = new GMMSettings();
        tailSideMinoConnect.Name = GMMSettingsName.SmokeDetector_IrCombiHeadDoveTailSide_MinoConnect;
        tailSideMinoConnect.AsyncComSettingsList.AddRange((IEnumerable<string>) "Baudrate=9600;BreakIntervalTime=10000;ForceMinoConnectState=IrCombiHead;IrDaSelection=DoveTailSide;Parity=even;Port=COM1;MinoConnectPowerOffTime=3600;RecTime_BeforFirstByte=1000;RecTime_GlobalOffset=0;RecTime_OffsetPerByte=0;RecTransTime=0;TestEcho=False;EchoOn=False;TransceiverDevice=MinoConnect;TransTime_AfterBreak=32;TransTime_BreakTime=2000;TransTime_GlobalOffset=0;Type=COM;WaitBeforeRepeatTime=0;Wakeup=BaudrateCarrier;MinoConnectIrDaPulseTime=0;RecTime_OffsetPerBlock=0;".Split(';'));
        tailSideMinoConnect.DeviceCollectorSettingsList.AddRange((IEnumerable<string>) "BusMode=SmokeDetector;FromTime=2008-01-01T01:01:01;ToTime=2030-01-01T01:01:01;MaxRequestRepeat=2;".Split(';'));
        return tailSideMinoConnect;
      }
    }

    public override string ToString() => this.Name.ToString();

    [XmlIgnore]
    public SortedList<ZR_ClassLibrary.AsyncComSettings, object> AsyncComSettings
    {
      get
      {
        SortedList<ZR_ClassLibrary.AsyncComSettings, object> asyncComSettings = new SortedList<ZR_ClassLibrary.AsyncComSettings, object>();
        SortedList<string, string> sortedList = GMMSettings.SplitSettings(this.AsyncComSettingsList);
        if (sortedList != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in sortedList)
          {
            if (Enum.IsDefined(typeof (ZR_ClassLibrary.AsyncComSettings), (object) keyValuePair.Key))
            {
              ZR_ClassLibrary.AsyncComSettings key = (ZR_ClassLibrary.AsyncComSettings) Enum.Parse(typeof (ZR_ClassLibrary.AsyncComSettings), keyValuePair.Key, true);
              string str = keyValuePair.Value;
              asyncComSettings.Add(key, (object) str);
            }
          }
        }
        return asyncComSettings;
      }
    }

    [XmlIgnore]
    public SortedList<string, string> AsyncComSettings_string_string
    {
      get
      {
        SortedList<string, string> settingsStringString = new SortedList<string, string>();
        SortedList<string, string> sortedList = GMMSettings.SplitSettings(this.AsyncComSettingsList);
        if (sortedList != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in sortedList)
          {
            if (Enum.IsDefined(typeof (ZR_ClassLibrary.AsyncComSettings), (object) keyValuePair.Key))
            {
              string key = keyValuePair.Key;
              string str = keyValuePair.Value;
              settingsStringString.Add(key, str);
            }
          }
        }
        return settingsStringString;
      }
    }

    [XmlIgnore]
    public SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> DeviceCollectorSettings
    {
      get
      {
        SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> collectorSettings = new SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object>();
        SortedList<string, string> sortedList = GMMSettings.SplitSettings(this.DeviceCollectorSettingsList);
        if (sortedList != null)
        {
          List<string> stringList = new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (ZR_ClassLibrary.DeviceCollectorSettings)));
          foreach (KeyValuePair<string, string> keyValuePair in sortedList)
          {
            if (stringList.Contains(keyValuePair.Key))
            {
              ZR_ClassLibrary.DeviceCollectorSettings key = (ZR_ClassLibrary.DeviceCollectorSettings) Enum.Parse(typeof (ZR_ClassLibrary.DeviceCollectorSettings), keyValuePair.Key, true);
              string str = keyValuePair.Value;
              collectorSettings.Add(key, (object) str);
            }
          }
        }
        return collectorSettings;
      }
    }

    public static SortedList<string, string> SplitSettings(List<string> settings)
    {
      if (settings == null)
        return (SortedList<string, string>) null;
      SortedList<string, string> sortedList = new SortedList<string, string>();
      for (int index = 0; index < settings.Count; ++index)
      {
        if (!string.IsNullOrEmpty(settings[index]))
        {
          string[] strArray = settings[index].Split('=');
          if (strArray.Length != 2)
            return (SortedList<string, string>) null;
          if (sortedList.ContainsKey(strArray[0]))
          {
            string str = "Wrong settings of GMM detected! The '" + strArray[0] + "' is twice.";
            GMMSettings.logger.Warn(str);
            ZR_ClassLibMessages.AddWarning(str);
          }
          else
            sortedList.Add(strArray[0], strArray[1]);
        }
      }
      return sortedList;
    }

    public string GetAsyncComSettingsString()
    {
      if (this.AsyncComSettingsList == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string asyncComSettings in this.AsyncComSettingsList)
      {
        char[] chArray = new char[1]{ '=' };
        string[] strArray = asyncComSettings.Split(chArray);
        if (strArray.Length == 2)
        {
          stringBuilder.Append(strArray[0]).Append(";");
          stringBuilder.Append(strArray[1]).Append(";");
        }
      }
      return stringBuilder.ToString();
    }

    public string GetDeviceCollectorSettingsString()
    {
      if (this.DeviceCollectorSettingsList == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string collectorSettings in this.DeviceCollectorSettingsList)
      {
        char[] chArray = new char[1]{ '=' };
        string[] strArray = collectorSettings.Split(chArray);
        if (strArray.Length == 2)
        {
          stringBuilder.Append(strArray[0]).Append(";");
          stringBuilder.Append(strArray[1]).Append(";");
        }
      }
      return stringBuilder.ToString();
    }

    public string GetCommunicationSettings()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.AsyncComSettingsList != null)
      {
        foreach (string asyncComSettings in this.AsyncComSettingsList)
        {
          if (asyncComSettings != null)
            stringBuilder.Append(asyncComSettings).Append(";");
        }
      }
      if (this.DeviceCollectorSettingsList != null)
      {
        foreach (string collectorSettings in this.DeviceCollectorSettingsList)
        {
          if (collectorSettings != null)
            stringBuilder.Append(collectorSettings).Append(";");
        }
      }
      return stringBuilder.ToString();
    }

    public SortedList<ReadoutSettings, object> GetReadoutSettings()
    {
      if (this.DeviceCollectorSettingsList == null)
        return (SortedList<ReadoutSettings, object>) null;
      SortedList<ReadoutSettings, object> readoutSettings = new SortedList<ReadoutSettings, object>();
      for (int index = 0; index < this.DeviceCollectorSettingsList.Count; ++index)
      {
        string str = this.DeviceCollectorSettingsList[index].Trim();
        if (!string.IsNullOrEmpty(str))
        {
          string[] strArray = str.Split('=');
          if (strArray.Length == 2 && new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (ReadoutSettings))).Contains(strArray[0]))
          {
            ReadoutSettings key = (ReadoutSettings) Enum.Parse(typeof (ReadoutSettings), strArray[0], true);
            object obj = (object) strArray[1];
            readoutSettings.Add(key, obj);
          }
        }
      }
      return readoutSettings;
    }

    public string GetReadoutSettingsValue(ReadoutSettings key)
    {
      if (this.DeviceCollectorSettingsList == null)
        return (string) null;
      for (int index = 0; index < this.DeviceCollectorSettingsList.Count; ++index)
      {
        if (this.DeviceCollectorSettingsList[index].StartsWith(key.ToString()))
          return this.DeviceCollectorSettingsList[index].Substring(key.ToString().Length + 1);
      }
      return string.Empty;
    }

    public string GetCommunicationStructureValue(string key)
    {
      string communicationSettings = this.GetCommunicationSettings();
      if (string.IsNullOrEmpty(communicationSettings))
        return string.Empty;
      int startIndex = communicationSettings.IndexOf(key + "=") + key.Length + 1;
      int num = communicationSettings.IndexOf(";", startIndex);
      return startIndex >= 0 && num > 0 && startIndex < num ? communicationSettings.Substring(startIndex, num - startIndex) : string.Empty;
    }

    public void UpdateCommunicationStructure(string key, string value)
    {
      if (this.AsyncComSettingsList != null)
      {
        for (int index = 0; index < this.AsyncComSettingsList.Count; ++index)
        {
          int num = this.AsyncComSettingsList[index].IndexOf(key);
          if (num >= 0 && this.AsyncComSettingsList[index].IndexOf("=", num + key.Length) >= 0)
          {
            this.AsyncComSettingsList[index] = key + "=" + value;
            return;
          }
        }
      }
      if (this.DeviceCollectorSettingsList == null)
        return;
      for (int index = 0; index < this.DeviceCollectorSettingsList.Count; ++index)
      {
        int num = this.DeviceCollectorSettingsList[index].IndexOf(key);
        if (num >= 0 && this.DeviceCollectorSettingsList[index].IndexOf("=", num + key.Length) >= 0)
        {
          this.DeviceCollectorSettingsList[index] = key + "=" + value;
          break;
        }
      }
    }

    public void AddOrUpdateReadoutSettings(ReadoutSettings key, string value)
    {
      if (this.DeviceCollectorSettingsList == null)
        this.DeviceCollectorSettingsList = new List<string>();
      if (string.IsNullOrEmpty(this.GetReadoutSettingsValue(key)))
      {
        this.DeviceCollectorSettingsList.Add(key.ToString() + "=" + value);
      }
      else
      {
        for (int index = 0; index < this.DeviceCollectorSettingsList.Count; ++index)
        {
          if (this.DeviceCollectorSettingsList[index].StartsWith(key.ToString()))
          {
            this.DeviceCollectorSettingsList[index] = key.ToString() + "=" + value;
            break;
          }
        }
      }
    }

    public static BusMode[] GetAvailableBusModes(TransceiverDevice transceiver)
    {
      switch (transceiver)
      {
        case TransceiverDevice.None:
          return new BusMode[0];
        case TransceiverDevice.MinoConnect:
          return new BusMode[21]
          {
            BusMode.MBus,
            BusMode.Minol_Device,
            BusMode.MinomatV2,
            BusMode.MinomatV3,
            BusMode.MinomatV4,
            BusMode.MinomatRadioTest,
            BusMode.RadioMS,
            BusMode.Radio2,
            BusMode.Radio3,
            BusMode.Radio3_868_95_RUSSIA,
            BusMode.MBusPointToPoint,
            BusMode.RelayDevice,
            BusMode.wMBusS1,
            BusMode.wMBusC1A,
            BusMode.wMBusC1B,
            BusMode.wMBusS1M,
            BusMode.wMBusS2,
            BusMode.wMBusT1,
            BusMode.wMBusT2_meter,
            BusMode.wMBusT2_other,
            BusMode.SmokeDetector
          };
        case TransceiverDevice.Wavenis:
          return new BusMode[1]{ BusMode.WaveFlowRadio };
        case TransceiverDevice.MinoHead:
          return new BusMode[6]
          {
            BusMode.Minol_Device,
            BusMode.MinomatV2,
            BusMode.MinomatV3,
            BusMode.MinomatV4,
            BusMode.Radio2,
            BusMode.Radio3
          };
        default:
          throw new ArgumentException("Not supported transceiver. Value: " + transceiver.ToString());
      }
    }

    public void SetSettings(string settings)
    {
      this.AsyncComSettingsList.Clear();
      this.DeviceCollectorSettingsList.Clear();
      if (settings == null)
        return;
      string[] strArray1 = settings.Split(';');
      if (strArray1 == null)
        return;
      foreach (string str1 in strArray1)
      {
        if (!string.IsNullOrEmpty(str1))
        {
          string[] strArray2 = str1.Split('=');
          if (strArray2 != null && strArray2.Length == 2)
          {
            string str2 = strArray2[0];
            string str3 = strArray2[1];
            if (Enum.IsDefined(typeof (ZR_ClassLibrary.AsyncComSettings), (object) str2))
              this.AsyncComSettingsList.Add(str1);
            else if (Enum.IsDefined(typeof (ZR_ClassLibrary.DeviceCollectorSettings), (object) str2))
              this.DeviceCollectorSettingsList.Add(str1);
          }
        }
      }
    }

    public void SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      if (asyncComSettings == null)
        return;
      List<string> stringList = new List<string>((IEnumerable<string>) Util.GetNamesOfEnum(typeof (ZR_ClassLibrary.AsyncComSettings)));
      foreach (KeyValuePair<string, string> asyncComSetting in asyncComSettings)
      {
        if (stringList.Contains(asyncComSetting.Key))
          this.AsyncComSettingsList.Add(asyncComSetting.Key + "=" + asyncComSetting.Value);
      }
    }

    public void SetAsyncComSettings(
      SortedList<ZR_ClassLibrary.AsyncComSettings, object> newAsyncComSettings)
    {
      if (newAsyncComSettings == null)
        return;
      SortedList<ZR_ClassLibrary.AsyncComSettings, object> asyncComSettings = this.AsyncComSettings;
      ZR_ClassLibrary.AsyncComSettings key;
      foreach (KeyValuePair<ZR_ClassLibrary.AsyncComSettings, object> newAsyncComSetting in newAsyncComSettings)
      {
        if (!asyncComSettings.ContainsKey(newAsyncComSetting.Key))
        {
          List<string> asyncComSettingsList = this.AsyncComSettingsList;
          key = newAsyncComSetting.Key;
          string str = key.ToString() + "=" + newAsyncComSetting.Value.ToString();
          asyncComSettingsList.Add(str);
        }
        else
        {
          key = newAsyncComSetting.Key;
          this.UpdateCommunicationStructure(key.ToString(), newAsyncComSetting.Value.ToString());
        }
      }
    }

    public bool SetAsyncComSettings(ZR_ClassLibrary.AsyncComSettings key, string value)
    {
      if (value == null)
        value = string.Empty;
      if (!this.AsyncComSettings.ContainsKey(key))
        this.AsyncComSettingsList.Add(key.ToString() + "=" + value);
      else
        this.UpdateCommunicationStructure(key.ToString(), value);
      return true;
    }

    public void SetDeviceCollectorSettings(
      SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> deviceCollectorSettings)
    {
      if (deviceCollectorSettings == null)
        return;
      SortedList<ZR_ClassLibrary.DeviceCollectorSettings, object> collectorSettings = this.DeviceCollectorSettings;
      ZR_ClassLibrary.DeviceCollectorSettings key;
      foreach (KeyValuePair<ZR_ClassLibrary.DeviceCollectorSettings, object> collectorSetting in deviceCollectorSettings)
      {
        if (!collectorSettings.ContainsKey(collectorSetting.Key))
        {
          List<string> collectorSettingsList = this.DeviceCollectorSettingsList;
          key = collectorSetting.Key;
          string str = key.ToString() + "=" + collectorSetting.Value?.ToString();
          collectorSettingsList.Add(str);
        }
        else
        {
          key = collectorSetting.Key;
          this.UpdateCommunicationStructure(key.ToString(), collectorSetting.Value.ToString());
        }
      }
    }
  }
}
