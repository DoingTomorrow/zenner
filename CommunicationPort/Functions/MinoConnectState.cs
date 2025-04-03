// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.MinoConnectState
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using System;
using System.IO.Ports;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  internal class MinoConnectState
  {
    private MinoConnectState.MinoConnectPlugState DetectedDevice;
    private ZENNER.CommonLibrary.MinoConnectBaseStates MinoConnectBaseState;
    private MinoConnectState.RadioModes ActiveRadioMode;
    private MinoConnectState.RadioCommands ActiveRadioCommand;
    private int RadioPacketErrorCounter;
    private bool RadioError;
    private MinoConnectState.ReceivedState LastReceivedState = MinoConnectState.ReceivedState.None;
    internal Decimal VersionValue;
    private int Baudrate;
    private string ByteFrame;
    private MinoConnectState.CombiHeadFunctions CombiHeadFunction;
    private int IrDaPulseLength;
    private int AutoPowerOffTime;
    private DateTime StateTime;
    internal bool KeyReceived;
    internal bool FramingError;
    internal bool Overload;
    internal bool BatteryLow;
    internal bool StateChanged;
    private byte[] LastStateLine;
    private static string[] StateCommands = new string[Enum.GetNames(typeof (ZENNER.CommonLibrary.MinoConnectBaseStates)).Length];
    private ChannelLogger MiConChannelLogger;

    static MinoConnectState()
    {
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
      MinoConnectState.StateCommands[11] = "#com rs485 BAUD FRAME\r\n#3von\r\n";
      MinoConnectState.StateCommands[12] = "#com rs232 BAUD FRAME\r\n#7von\r\n";
      MinoConnectState.StateCommands[13] = "#com rsoff";
    }

    internal MinoConnectState(ChannelLogger MiConLogger)
    {
      this.MiConChannelLogger = MiConLogger;
      this.MinoConnectBaseState = ZENNER.CommonLibrary.MinoConnectBaseStates.undefined;
      this.Baudrate = 0;
      this.ByteFrame = string.Empty;
      this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.undefined;
      this.IrDaPulseLength = -1;
      this.KeyReceived = false;
      this.FramingError = false;
      this.Overload = false;
      this.BatteryLow = false;
      this.StateChanged = true;
      this.StateTime = DateTime.MinValue;
      this.AutoPowerOffTime = -1;
    }

    internal MinoConnectState(CommunicationByMinoConnect comBase, ChannelLogger MiConLogger)
      : this(MiConLogger)
    {
      this.SetFromConfiguration(comBase);
    }

    internal void SetFromMinoConnectState(MinoConnectState minoConnectState)
    {
      this.AutoPowerOffTime = minoConnectState.AutoPowerOffTime;
      this.Baudrate = minoConnectState.Baudrate;
      this.ByteFrame = minoConnectState.ByteFrame;
      this.DetectedDevice = minoConnectState.DetectedDevice;
      this.FramingError = minoConnectState.FramingError;
      this.Overload = minoConnectState.Overload;
      this.BatteryLow = minoConnectState.BatteryLow;
      this.CombiHeadFunction = minoConnectState.CombiHeadFunction;
      this.IrDaPulseLength = minoConnectState.IrDaPulseLength;
      this.KeyReceived = minoConnectState.KeyReceived;
      this.LastStateLine = minoConnectState.LastStateLine;
      this.MinoConnectBaseState = minoConnectState.MinoConnectBaseState;
      this.StateChanged = minoConnectState.StateChanged;
    }

    internal void SetFromConfiguration(CommunicationByMinoConnect comBase)
    {
      this.LastStateLine = (byte[]) null;
      this.LastReceivedState = MinoConnectState.ReceivedState.None;
      this.MinoConnectBaseState = comBase.MinoConnectBaseState;
      this.AutoPowerOffTime = comBase.configList.MinoConnectPowerOffTime;
      this.IrDaPulseLength = comBase.configList.MinoConnectIrDaPulseTime;
      this.Baudrate = comBase.configList.Baudrate;
      if (comBase.Parity == Parity.None)
        this.ByteFrame = "8n1";
      else if (comBase.Parity == Parity.Even)
      {
        this.ByteFrame = "8e1";
      }
      else
      {
        if (comBase.Parity != Parity.Odd)
          throw new Exception("Parity not defined");
        this.ByteFrame = "8o1";
      }
      if (comBase.configList.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.IrCombiHead.ToString())
      {
        if (comBase.IrDaSelection == IrDaSelection.DoveTailSide)
        {
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.DoveTailIrDa;
          this.ByteFrame = "i" + this.ByteFrame;
        }
        else if (comBase.IrDaSelection == IrDaSelection.RoundSide)
        {
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.RoundIrDa;
          this.ByteFrame = "i" + this.ByteFrame;
        }
        else
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.RoundUART;
      }
      else
      {
        if (!(comBase.configList.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.ZIN_CombiHead.ToString()))
          return;
        if (comBase.CombiHeadSelection == CombiHeadSelection.IrDA_DoveTailSide)
        {
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.DoveTailIrDa;
          this.ByteFrame = "i" + this.ByteFrame;
        }
        else if (comBase.CombiHeadSelection == CombiHeadSelection.IrDa_RoundSide)
        {
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.RoundIrDa;
          this.ByteFrame = "i" + this.ByteFrame;
        }
        else if (comBase.CombiHeadSelection == CombiHeadSelection.NFC)
        {
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.NFC;
          this.ByteFrame = "8e1";
        }
        else
          this.CombiHeadFunction = MinoConnectState.CombiHeadFunctions.RoundUART;
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
          this.LastReceivedState = MinoConnectState.ReceivedState.OldRSxxxState;
          break;
        case 9:
          this.ActiveRadioMode = (MinoConnectState.RadioModes) this.HexFromChar(StateLine[2]);
          this.MinoConnectBaseState = this.ActiveRadioMode != MinoConnectState.RadioModes.RADIO2 ? (this.ActiveRadioMode != MinoConnectState.RadioModes.RADIO3 ? ZENNER.CommonLibrary.MinoConnectBaseStates.WirelessMBus : ZENNER.CommonLibrary.MinoConnectBaseStates.Radio3Receive) : ZENNER.CommonLibrary.MinoConnectBaseStates.Radio2Receive;
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
          this.LastReceivedState = MinoConnectState.ReceivedState.RadioState;
          break;
        default:
          return false;
      }
      return true;
    }

    internal bool IsEqual(MinoConnectState CompareState)
    {
      return this.MinoConnectBaseState == CompareState.MinoConnectBaseState && this.Baudrate == CompareState.Baudrate && !(this.ByteFrame != CompareState.ByteFrame) && this.CombiHeadFunction == CompareState.CombiHeadFunction && this.IrDaPulseLength == CompareState.IrDaPulseLength && this.KeyReceived == CompareState.KeyReceived && this.FramingError == CompareState.FramingError && this.Overload == CompareState.Overload;
    }

    internal string GetChangeCommand(MinoConnectState RequiredState)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      bool flag = this.VersionValue >= 1.3M && this.IrDaPulseLength != RequiredState.IrDaPulseLength;
      if (((this.MinoConnectBaseState != RequiredState.MinoConnectBaseState || this.Baudrate != RequiredState.Baudrate ? 1 : (this.ByteFrame != RequiredState.ByteFrame ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        if (flag)
          stringBuilder.Append("#irp " + RequiredState.IrDaPulseLength.ToString() + "\r\n");
        string str = MinoConnectState.StateCommands[(int) RequiredState.MinoConnectBaseState].Replace("BAUD", RequiredState.Baudrate.ToString()).Replace("FRAME", RequiredState.ByteFrame);
        if (RequiredState.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.ZIN_CombiHead && RequiredState.CombiHeadFunction == MinoConnectState.CombiHeadFunctions.NFC)
          str = str.Replace("rs232", "rs485");
        stringBuilder.Append(str);
      }
      if (this.AutoPowerOffTime != RequiredState.AutoPowerOffTime)
        stringBuilder.Append("#apo " + RequiredState.AutoPowerOffTime.ToString() + "\r\n");
      if (this.CombiHeadFunction != RequiredState.CombiHeadFunction)
      {
        switch (RequiredState.CombiHeadFunction)
        {
          case MinoConnectState.CombiHeadFunctions.DoveTailIrDa:
            stringBuilder.Append("#out 0\r\n");
            break;
          case MinoConnectState.CombiHeadFunctions.RoundIrDa:
            stringBuilder.Append("#out 2\r\n");
            break;
          case MinoConnectState.CombiHeadFunctions.NFC:
            stringBuilder.Append("#out 3\r\n");
            break;
          default:
            stringBuilder.Append("#out 1\r\n");
            break;
        }
      }
      if (stringBuilder.Length == 0)
        return (string) null;
      string changeCommand = stringBuilder.ToString();
      this.MiConChannelLogger.Trace("ChangeCommand: " + changeCommand);
      return changeCommand;
    }

    public bool IsRequiredIrDaFilter
    {
      get
      {
        return this.Baudrate == 9600 && this.CombiHeadFunction == MinoConnectState.CombiHeadFunctions.RoundIrDa;
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
        if (this.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.RS232_7V && RequiredState.MinoConnectBaseState == ZENNER.CommonLibrary.MinoConnectBaseStates.IrCombiHead)
        {
          stringBuilder.Length = 0;
          stringBuilder.Append("State: " + RequiredState.MinoConnectBaseState.ToString());
        }
        else
          stringBuilder.Append(" <- " + RequiredState.MinoConnectBaseState.ToString());
      }
      stringBuilder.Append(ZR_Constants.SystemNewLine);
      if (this.LastReceivedState == MinoConnectState.ReceivedState.OldRSxxxState)
      {
        if (this.MinoConnectBaseState >= ZENNER.CommonLibrary.MinoConnectBaseStates.IrCombiHead && this.MinoConnectBaseState <= ZENNER.CommonLibrary.MinoConnectBaseStates.RS485_7V)
        {
          stringBuilder.Append("Baudrate:" + this.Baudrate.ToString());
          if (this.Baudrate != RequiredState.Baudrate)
            stringBuilder.Append(" <- " + RequiredState.Baudrate.ToString());
          stringBuilder.Append(ZR_Constants.SystemNewLine);
          stringBuilder.Append("Frame: " + this.ByteFrame);
          if (this.ByteFrame != RequiredState.ByteFrame)
            stringBuilder.Append(" <- " + RequiredState.ByteFrame);
          stringBuilder.Append(ZR_Constants.SystemNewLine);
          stringBuilder.Append("IrCombiHead: " + this.CombiHeadFunction.ToString());
          if (this.CombiHeadFunction != RequiredState.CombiHeadFunction)
            stringBuilder.Append(" <- " + RequiredState.CombiHeadFunction.ToString());
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
          stringBuilder.Append("!!! BatteryLow !!!" + ZR_Constants.SystemNewLine);
      }
      else if (this.LastReceivedState == MinoConnectState.ReceivedState.RadioState)
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

    internal static ZENNER.CommonLibrary.MinoConnectBaseStates GetBaseStateFromPlugState(
      MinoConnectState.MinoConnectPlugState PlugState)
    {
      switch (PlugState)
      {
        case MinoConnectState.MinoConnectPlugState.RS232:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS232;
        case MinoConnectState.MinoConnectPlugState.RS485:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS485;
        case MinoConnectState.MinoConnectPlugState.RS485_7V:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS485_7V;
        case MinoConnectState.MinoConnectPlugState.RS232_7V:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS232_7V;
        case MinoConnectState.MinoConnectPlugState.RS232_3V:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS232_3V;
        case MinoConnectState.MinoConnectPlugState.RS485_3V:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.RS485_3V;
        default:
          return ZENNER.CommonLibrary.MinoConnectBaseStates.IrCombiHead;
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

    private enum CombiHeadFunctions
    {
      undefined,
      RoundUART,
      DoveTailIrDa,
      RoundIrDa,
      NFC,
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
