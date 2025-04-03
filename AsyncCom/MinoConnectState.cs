// Decompiled with JetBrains decompiler
// Type: AsyncCom.MinoConnectState
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  internal class MinoConnectState
  {
    private MinoConnectSerialPort MyMiConPort;
    private MinoConnectState.MinoConnectPlugState DetectedDevice;
    private MinoConnectState.BaseStateEnum MinoConnectBaseState;
    private MinoConnectState.RadioModes ActiveRadioMode;
    private MinoConnectState.RadioCommands ActiveRadioCommand;
    private int RadioPacketErrorCounter;
    private bool RadioError;
    private MinoConnectState.ReceivedState LastReseicedState = MinoConnectState.ReceivedState.None;
    private int Baudrate;
    private string ByteFrame;
    private MinoConnectState.IrCombiHeadFunctionEnum IrCombiHeadFunction;
    private int IrDaPulseLength;
    private int AutoPowerOffTime;
    private DateTime StateTime;
    internal bool KeyReceived;
    internal bool FramingError;
    internal bool Overload;
    internal bool BatteryLow;
    internal bool StateChanged;
    private byte[] LastStateLine;
    private static string[] StateCommands;

    internal MinoConnectState(MinoConnectState minoConnectState)
    {
      this.AutoPowerOffTime = minoConnectState.AutoPowerOffTime;
      this.Baudrate = minoConnectState.Baudrate;
      this.ByteFrame = minoConnectState.ByteFrame;
      this.DetectedDevice = minoConnectState.DetectedDevice;
      this.FramingError = minoConnectState.FramingError;
      this.IrCombiHeadFunction = minoConnectState.IrCombiHeadFunction;
      this.IrDaPulseLength = minoConnectState.IrDaPulseLength;
      this.KeyReceived = minoConnectState.KeyReceived;
      this.LastStateLine = minoConnectState.LastStateLine;
      this.MinoConnectBaseState = minoConnectState.MinoConnectBaseState;
      this.MyMiConPort = minoConnectState.MyMiConPort;
      this.StateChanged = minoConnectState.StateChanged;
      this.StateTime = minoConnectState.StateTime;
    }

    internal MinoConnectState(MinoConnectSerialPort MyMiConPort)
    {
      this.MyMiConPort = MyMiConPort;
      if (MinoConnectState.StateCommands == null)
      {
        MinoConnectState.StateCommands = new string[Util.GetNamesOfEnum(typeof (MinoConnectState.BaseStateEnum)).Length];
        MinoConnectState.StateCommands[0] = "#com rsoff";
        MinoConnectState.StateCommands[1] = "#com rs232 BAUD FRAME\r\n#7von\r\n";
        MinoConnectState.StateCommands[2] = "#com rs232 BAUD FRAME\r\n#7voff\r\n";
        MinoConnectState.StateCommands[3] = "#com rs232 BAUD FRAME\r\n#3von\r\n";
        MinoConnectState.StateCommands[4] = "#com rs232 BAUD FRAME\r\n#7von\r\n";
        MinoConnectState.StateCommands[5] = "#com rs485 BAUD FRAME\r\n#7voff\r\n";
        MinoConnectState.StateCommands[6] = "#com rs485 BAUD FRAME\r\n#3von\r\n";
        MinoConnectState.StateCommands[7] = "#com rs485 BAUD FRAME\r\n#7von\r\n";
        MinoConnectState.StateCommands[8] = "#com radio radio2";
        MinoConnectState.StateCommands[9] = "#com radio radio3";
        MinoConnectState.StateCommands[11] = "#com rsoff";
      }
      this.ClearState();
    }

    internal void ClearState()
    {
      this.MinoConnectBaseState = MinoConnectState.BaseStateEnum.undefined;
      this.Baudrate = 0;
      this.ByteFrame = string.Empty;
      this.IrCombiHeadFunction = MinoConnectState.IrCombiHeadFunctionEnum.undefined;
      this.IrDaPulseLength = -1;
      this.KeyReceived = false;
      this.FramingError = false;
      this.Overload = false;
      this.BatteryLow = false;
      this.StateChanged = true;
      this.StateTime = DateTime.MinValue;
      this.AutoPowerOffTime = -1;
    }

    internal void SetFromAsyncCom(AsyncFunctions TheFunctions)
    {
      this.LastStateLine = (byte[]) null;
      this.LastReseicedState = MinoConnectState.ReceivedState.None;
      this.MinoConnectBaseState = TheFunctions.MinoConnectBaseState;
      this.AutoPowerOffTime = TheFunctions.MinoConnectAutoPowerOffTime;
      this.IrDaPulseLength = TheFunctions.MinoConnectIrDaPulseLength;
      this.Baudrate = TheFunctions.Baudrate;
      this.IrCombiHeadFunction = !TheFunctions.IrDa ? MinoConnectState.IrCombiHeadFunctionEnum.RoundUART : (!TheFunctions.IrDaDaveTailSide ? MinoConnectState.IrCombiHeadFunctionEnum.RoundIrDa : MinoConnectState.IrCombiHeadFunctionEnum.DoveTailIrDa);
      if (TheFunctions.Parity == "no")
      {
        if (TheFunctions.IrDa)
          this.ByteFrame = "i8n1";
        else
          this.ByteFrame = "8n1";
      }
      else if (TheFunctions.Parity == "even")
      {
        if (TheFunctions.IrDa)
          this.ByteFrame = "i8e1";
        else
          this.ByteFrame = "8e1";
      }
      else
      {
        if (!(TheFunctions.Parity == "odd"))
          return;
        this.ByteFrame = !TheFunctions.IrDa ? "8o1" : "i8o1";
      }
    }

    internal bool SetFromReceivedState(byte[] StateLine)
    {
      this.StateChanged = true;
      if (this.LastStateLine != null)
      {
        if (this.LastStateLine.Length == StateLine.Length)
        {
          for (int index = 0; index != StateLine.Length; ++index)
          {
            if ((int) this.LastStateLine[index] != (int) StateLine[index])
            {
              this.LastStateLine = StateLine;
              goto label_9;
            }
          }
          this.StateChanged = false;
          return true;
        }
      }
      else
      {
        this.StateChanged = true;
        this.LastStateLine = StateLine;
      }
label_9:
      switch (StateLine.Length)
      {
        case 7:
          this.DetectedDevice = (MinoConnectState.MinoConnectPlugState) this.HexFromChar(StateLine[2]);
          this.MinoConnectBaseState = MinoConnectState.GetBaseStateFromPlugState((MinoConnectState.MinoConnectPlugState) this.HexFromChar(StateLine[3]));
          int num1 = this.HexFromChar(StateLine[4]);
          this.KeyReceived = (num1 & 1) > 0;
          this.FramingError = (num1 & 2) > 0;
          this.Overload = (num1 & 4) > 0;
          this.BatteryLow = (num1 & 8) > 0;
          this.LastReseicedState = MinoConnectState.ReceivedState.OldRSxxxState;
          break;
        case 9:
          this.ActiveRadioMode = (MinoConnectState.RadioModes) this.HexFromChar(StateLine[2]);
          this.MinoConnectBaseState = this.ActiveRadioMode != MinoConnectState.RadioModes.RADIO2 ? (this.ActiveRadioMode != MinoConnectState.RadioModes.RADIO3 ? MinoConnectState.BaseStateEnum.WirelessMBus : MinoConnectState.BaseStateEnum.Radio3Receive) : MinoConnectState.BaseStateEnum.Radio2Receive;
          this.ActiveRadioCommand = (MinoConnectState.RadioCommands) this.HexFromChar(StateLine[3]);
          int packetErrorCounter = this.RadioPacketErrorCounter;
          this.RadioPacketErrorCounter &= -256;
          this.RadioPacketErrorCounter |= (this.HexFromChar(StateLine[4]) << 4) + this.HexFromChar(StateLine[5]);
          if (this.RadioPacketErrorCounter < packetErrorCounter)
            this.RadioPacketErrorCounter += 256;
          int num2 = this.HexFromChar(StateLine[6]);
          this.KeyReceived = (num2 & 1) > 0;
          this.RadioError = (num2 & 2) > 0;
          this.Overload = (num2 & 4) > 0;
          this.BatteryLow = (num2 & 8) > 0;
          this.LastReseicedState = MinoConnectState.ReceivedState.RadioState;
          break;
        default:
          return false;
      }
      return true;
    }

    internal bool IsEqual(MinoConnectState CompareState)
    {
      return this.MinoConnectBaseState == CompareState.MinoConnectBaseState && this.Baudrate == CompareState.Baudrate && !(this.ByteFrame != CompareState.ByteFrame) && this.IrCombiHeadFunction == CompareState.IrCombiHeadFunction && this.IrDaPulseLength == CompareState.IrDaPulseLength && this.KeyReceived == CompareState.KeyReceived && this.FramingError == CompareState.FramingError && this.Overload == CompareState.Overload && this.BatteryLow == CompareState.BatteryLow;
    }

    internal string GetChangeCommand(MinoConnectState RequiredState)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      bool flag = this.MyMiConPort.VersionValue >= 1.3M && this.IrDaPulseLength != RequiredState.IrDaPulseLength;
      if (((this.MinoConnectBaseState != RequiredState.MinoConnectBaseState || this.Baudrate != RequiredState.Baudrate ? 1 : (this.ByteFrame != RequiredState.ByteFrame ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        if (flag)
          stringBuilder.Append("#irp " + RequiredState.IrDaPulseLength.ToString() + "\r\n");
        string str = MinoConnectState.StateCommands[(int) RequiredState.MinoConnectBaseState].Replace("BAUD", RequiredState.Baudrate.ToString()).Replace("FRAME", RequiredState.ByteFrame);
        stringBuilder.Append(str);
      }
      if (this.AutoPowerOffTime != RequiredState.AutoPowerOffTime)
        stringBuilder.Append("#apo " + RequiredState.AutoPowerOffTime.ToString() + "\r\n");
      if (this.IrCombiHeadFunction != RequiredState.IrCombiHeadFunction)
      {
        switch (RequiredState.IrCombiHeadFunction)
        {
          case MinoConnectState.IrCombiHeadFunctionEnum.DoveTailIrDa:
            stringBuilder.Append("#out 0\r\n");
            break;
          case MinoConnectState.IrCombiHeadFunctionEnum.RoundIrDa:
            stringBuilder.Append("#out 2\r\n");
            break;
          case MinoConnectState.IrCombiHeadFunctionEnum.RoundUART:
            stringBuilder.Append("#out 1\r\n");
            break;
          default:
            stringBuilder.Append("#out 1\r\n");
            break;
        }
      }
      return stringBuilder.Length == 0 ? (string) null : stringBuilder.ToString();
    }

    public bool IsRequiredIrDaFilter
    {
      get
      {
        return this.Baudrate == 9600 && this.IrCombiHeadFunction == MinoConnectState.IrCombiHeadFunctionEnum.RoundIrDa;
      }
    }

    internal string GetStateString(MinoConnectState RequiredState)
    {
      if (RequiredState == null)
        RequiredState = this;
      StringBuilder stringBuilder = new StringBuilder(300);
      stringBuilder.Append("State: " + this.MinoConnectBaseState.ToString());
      if (this.MinoConnectBaseState != RequiredState.MinoConnectBaseState)
      {
        if (this.MinoConnectBaseState == MinoConnectState.BaseStateEnum.RS232_7V && RequiredState.MinoConnectBaseState == MinoConnectState.BaseStateEnum.IrCombiHead)
        {
          stringBuilder.Length = 0;
          stringBuilder.Append("State: " + RequiredState.MinoConnectBaseState.ToString());
        }
        else
          stringBuilder.Append(" <- " + RequiredState.MinoConnectBaseState.ToString());
      }
      stringBuilder.Append(ZR_Constants.SystemNewLine);
      if (this.LastReseicedState == MinoConnectState.ReceivedState.OldRSxxxState)
      {
        if (this.MinoConnectBaseState >= MinoConnectState.BaseStateEnum.IrCombiHead && this.MinoConnectBaseState <= MinoConnectState.BaseStateEnum.RS485_7V)
        {
          stringBuilder.Append("Baudrate:" + this.Baudrate.ToString());
          if (this.Baudrate != RequiredState.Baudrate)
            stringBuilder.Append(" <- " + RequiredState.Baudrate.ToString());
          stringBuilder.Append(ZR_Constants.SystemNewLine);
          stringBuilder.Append("Frame: " + this.ByteFrame);
          if (this.ByteFrame != RequiredState.ByteFrame)
            stringBuilder.Append(" <- " + RequiredState.ByteFrame);
          stringBuilder.Append(ZR_Constants.SystemNewLine);
          stringBuilder.Append("IrCombiHead: " + this.IrCombiHeadFunction.ToString());
          if (this.IrCombiHeadFunction != RequiredState.IrCombiHeadFunction)
            stringBuilder.Append(" <- " + RequiredState.IrCombiHeadFunction.ToString());
          stringBuilder.Append(ZR_Constants.SystemNewLine);
          if (this.ByteFrame[0] == 'i')
          {
            stringBuilder.Append("IrDaPulsLen: " + this.IrDaPulseLength.ToString());
            if (this.IrDaPulseLength != RequiredState.IrDaPulseLength)
              stringBuilder.Append(" <- " + RequiredState.IrDaPulseLength.ToString());
            stringBuilder.Append(ZR_Constants.SystemNewLine);
          }
        }
        if (this.FramingError)
          stringBuilder.Append("!!! Framing error !!!" + ZR_Constants.SystemNewLine);
        if (this.Overload)
          stringBuilder.Append("!!! Overload !!!" + ZR_Constants.SystemNewLine);
        if (this.BatteryLow)
          stringBuilder.Append("!!! Battery low !!!" + ZR_Constants.SystemNewLine);
      }
      else if (this.LastReseicedState == MinoConnectState.ReceivedState.RadioState)
      {
        if (this.RadioError)
          stringBuilder.Append("!!! Radio error !!!" + ZR_Constants.SystemNewLine);
        stringBuilder.Append("Packet error counts: " + this.RadioPacketErrorCounter.ToString() + ZR_Constants.SystemNewLine);
      }
      return stringBuilder.ToString();
    }

    private int HexFromChar(byte Char)
    {
      if (Char <= (byte) 57)
        return (int) Char - 48;
      return Char > (byte) 70 ? (int) Char - 97 + 10 : (int) Char - 65 + 10;
    }

    internal static MinoConnectState.BaseStateEnum GetBaseStateFromPlugState(
      MinoConnectState.MinoConnectPlugState PlugState)
    {
      switch (PlugState)
      {
        case MinoConnectState.MinoConnectPlugState.RS232:
          return MinoConnectState.BaseStateEnum.RS232;
        case MinoConnectState.MinoConnectPlugState.RS485:
          return MinoConnectState.BaseStateEnum.RS485;
        case MinoConnectState.MinoConnectPlugState.RS485_7V:
          return MinoConnectState.BaseStateEnum.RS485_7V;
        case MinoConnectState.MinoConnectPlugState.RS232_7V:
          return MinoConnectState.BaseStateEnum.RS232_7V;
        case MinoConnectState.MinoConnectPlugState.RS232_3V:
          return MinoConnectState.BaseStateEnum.RS232_3V;
        case MinoConnectState.MinoConnectPlugState.RS485_3V:
          return MinoConnectState.BaseStateEnum.RS485_3V;
        default:
          return MinoConnectState.BaseStateEnum.IrCombiHead;
      }
    }

    public enum MinoConnectPlugState
    {
      Auto,
      _SHORT_CIRCUIT,
      RS232,
      RS485,
      RS485_7V,
      RS232_7V,
      RS232_3V,
      IrCombiHead,
      RS485_3V,
      _UNDEF_1,
      AUTO_7V,
      _DISCONNECTED,
      _Overload,
      _NoInfo,
      undefined,
    }

    public enum BaseStateEnum
    {
      off,
      IrCombiHead,
      RS232,
      RS232_3V,
      RS232_7V,
      RS485,
      RS485_3V,
      RS485_7V,
      Radio2Receive,
      Radio3Receive,
      WirelessMBus,
      undefined,
    }

    private enum IrCombiHeadFunctionEnum
    {
      DoveTailIrDa,
      RoundIrDa,
      RoundUART,
      undefined,
    }

    private enum RadioModes
    {
      RADIO2,
      RADIO3,
      WMBUS_S1,
      WMBUS_S1M,
      WMBUS_S2,
      WMBUS_T1,
      WMBUS_T2_METER,
      WMBUS_T2_OTHER,
    }

    private enum RadioCommands
    {
      LOOP,
      RECEIVE,
      CAL_OOK,
      CAL_PN9,
      NULL,
    }

    private enum ReceivedState
    {
      None,
      OldRSxxxState,
      RadioState,
    }
  }
}
