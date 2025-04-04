// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.OutputAnalyzeData
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using System;

#nullable disable
namespace SmokeDetectorHandler
{
  public class OutputAnalyzeData
  {
    private uint _serialNum;
    private uint _meterID;
    private string _firmwareVer;
    private ushort _hardwareVer;
    private ushort _numSmokeAlarm;
    private ushort _numTestAlarms;
    private DateTime _occurDatetime;
    private string _batteryForewarning;
    private string _batteryFault;
    private string _batteryWarningRadio;
    private string _testAlarmReleased;
    private string _smokeAlarmReleased;
    private string _smokeChamberPollutionForewarning;
    private string _smokeChamberPollutionWarning;
    private string _pushButtonFailure;
    private string _hornFailure;
    private string _removingDetection;
    private string _ingressAperturesObstructionDetected;
    private string _objectInSurroundingAreaDetected;
    private string _lED_Failure;
    private string _bit13_undefined;
    private string _bit14_undefined;
    private string _bit15_undefined;

    public uint SerialNum
    {
      get => this._serialNum;
      set => this._serialNum = value;
    }

    public uint MeterID
    {
      get => this._meterID;
      set => this._meterID = value;
    }

    public string FirmwareVer
    {
      get => this._firmwareVer;
      set => this._firmwareVer = value;
    }

    public ushort HardwareVer
    {
      get => this._hardwareVer;
      set => this._hardwareVer = value;
    }

    public ushort NumSmokeAlarm
    {
      get => this._numSmokeAlarm;
      set => this._numSmokeAlarm = value;
    }

    public ushort NumTestAlarms
    {
      get => this._numTestAlarms;
      set => this._numTestAlarms = value;
    }

    public DateTime OccurDatetime
    {
      get => this._occurDatetime;
      set => this._occurDatetime = value;
    }

    public string BatteryForewarning
    {
      get => this._batteryForewarning;
      set => this._batteryForewarning = value;
    }

    public string BatteryFault
    {
      get => this._batteryFault;
      set => this._batteryFault = value;
    }

    public string BatteryWarningRadio
    {
      get => this._batteryWarningRadio;
      set => this._batteryWarningRadio = value;
    }

    public string TestAlarmReleased
    {
      get => this._testAlarmReleased;
      set => this._testAlarmReleased = value;
    }

    public string SmokeAlarmReleased
    {
      get => this._smokeAlarmReleased;
      set => this._smokeAlarmReleased = value;
    }

    public string SmokeChamberPollutionForewarning
    {
      get => this._smokeChamberPollutionForewarning;
      set => this._smokeChamberPollutionForewarning = value;
    }

    public string SmokeChamberPollutionWarning
    {
      get => this._smokeChamberPollutionWarning;
      set => this._smokeChamberPollutionWarning = value;
    }

    public string PushButtonFailure
    {
      get => this._pushButtonFailure;
      set => this._pushButtonFailure = value;
    }

    public string HornFailure
    {
      get => this._hornFailure;
      set => this._hornFailure = value;
    }

    public string RemovingDetection
    {
      get => this._removingDetection;
      set => this._removingDetection = value;
    }

    public string IngressAperturesObstructionDetected
    {
      get => this._ingressAperturesObstructionDetected;
      set => this._ingressAperturesObstructionDetected = value;
    }

    public string ObjectInSurroundingAreaDetected
    {
      get => this._objectInSurroundingAreaDetected;
      set => this._objectInSurroundingAreaDetected = value;
    }

    public string LED_Failure
    {
      get => this._lED_Failure;
      set => this._lED_Failure = value;
    }

    public string Bit13_undefined
    {
      get => this._bit13_undefined;
      set => this._bit13_undefined = value;
    }

    public string Bit14_undefined
    {
      get => this._bit14_undefined;
      set => this._bit14_undefined = value;
    }

    public string Bit15_undefined
    {
      get => this._bit15_undefined;
      set => this._bit15_undefined = value;
    }
  }
}
