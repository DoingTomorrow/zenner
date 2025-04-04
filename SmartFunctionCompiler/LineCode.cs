// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.LineCode
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace SmartFunctionCompiler
{
  internal class LineCode
  {
    internal int SourceLine;
    internal string SourceText;
    internal byte Opcode;
    internal int? ParameterIndex;
    internal string LineLable;
    internal string JumpToLable;
    internal ushort CodeStartOffset;
    internal ushort? JumpDestinationOffset;
    internal bool FirstResetCode = false;
    internal bool FirstEventCode = false;
    internal bool Nead_IsAccuA_NaN_follow = false;
    internal List<byte> CodeBytes = new List<byte>();

    internal LineCode(string[] lines, List<RuntimeParameter> Parameters, ref int lineNumber)
    {
      this.SourceLine = -1;
      while (true)
      {
        if (lineNumber < lines.Length)
        {
          if (lines[lineNumber].Length == 0)
            ++lineNumber;
          else
            goto label_4;
        }
        else
          break;
      }
      return;
label_4:
      if (lines[lineNumber].EndsWith(":"))
      {
        if (lines[lineNumber].Length < 2)
          throw new Exception("Lable without name");
        this.LineLable = lines[lineNumber].Substring(0, lines[lineNumber].Length - 1);
        ++lineNumber;
        while (lines[lineNumber].Length == 0)
          ++lineNumber;
      }
      this.SourceLine = lineNumber;
      this.SourceText = lines[lineNumber];
      string[] lineParts = this.SourceText.Split(' ');
      if (lineParts.Length < 1)
        throw new Exception("Opcode not available");
      if (lineParts[0] == "Load")
      {
        if (lineParts.Length < 2)
          throw new Exception("Line token not available. Missed token index: 1");
        OpcodeLoadParameter result;
        if (Enum.TryParse<OpcodeLoadParameter>("Load" + lineParts[1], out result))
        {
          this.Opcode = (byte) result;
          this.CodeBytes.Add((byte) result);
          return;
        }
      }
      OpcodeNoParameter result1;
      if (Enum.TryParse<OpcodeNoParameter>(lineParts[0], out result1))
      {
        this.Opcode = (byte) result1;
        this.CodeBytes.Add((byte) result1);
      }
      else
      {
        OpcodeJump result2;
        if (Enum.TryParse<OpcodeJump>(lineParts[0], out result2))
        {
          this.Opcode = (byte) result2;
          this.CodeBytes.Add((byte) result2);
          this.JumpToLable = lineParts.Length >= 2 ? lineParts[1] : throw new Exception("No lable name found");
          this.CodeBytes.Add((byte) 0);
          this.CodeBytes.Add((byte) 0);
        }
        else
        {
          OpcodeLoadParameter result3;
          if (Enum.TryParse<OpcodeLoadParameter>(lineParts[0], out result3))
          {
            this.Opcode = (byte) result3;
            this.CodeBytes.Add((byte) result3);
            if (lineParts.Length < 2)
              throw new Exception("No Load parameter found");
            OpcodeLoadFirmwareParameter result4;
            if (Enum.TryParse<OpcodeLoadFirmwareParameter>("Load" + lineParts[1], out result4))
            {
              this.Opcode = (byte) result4;
              this.CodeBytes.Add((byte) result4);
              if (result4 == OpcodeLoadFirmwareParameter.LoadStateCounter)
              {
                if (lineParts.Length < 3)
                  throw new Exception("No LoadStateCounter parameter found");
                DeviceStateCounterID result5;
                if (!Enum.TryParse<DeviceStateCounterID>(lineParts[2], out result5))
                  throw new Exception("Illegal LoadStateCounter: " + lineParts[2]);
                this.CodeBytes.Add((byte) result5);
              }
              if (result4 == OpcodeLoadFirmwareParameter.LoadSmartFunctionState || result4 == OpcodeLoadFirmwareParameter.LoadSmartFunctionTimeoutActive)
              {
                if (lineParts.Length < 3)
                  throw new Exception("No event parameter found");
                SmartFunctionLoggerEventType result6;
                if (!Enum.TryParse<SmartFunctionLoggerEventType>(lineParts[2], out result6))
                  throw new Exception("Illegal event: " + lineParts[2]);
                if (result6 == SmartFunctionLoggerEventType.LoggerEvent_None || result6 >= SmartFunctionLoggerEventType.LoggerEvent_Auto_OFF)
                  throw new Exception("Illegal event. Only set events allowed: " + lineParts[2]);
                this.CodeBytes.Add((byte) result6);
              }
              if (result4 != OpcodeLoadFirmwareParameter.LoadMax5MinutesFlow && result4 != OpcodeLoadFirmwareParameter.LoadMin5MinutesFlow && result4 != OpcodeLoadFirmwareParameter.LoadMax5MinutesTemp && result4 != OpcodeLoadFirmwareParameter.LoadMin5MinutesTemp && result4 != OpcodeLoadFirmwareParameter.LoadMaxHourlyFlow && result4 != OpcodeLoadFirmwareParameter.LoadMinHourlyFlow && result4 != OpcodeLoadFirmwareParameter.LoadMaxHourlyTemp && result4 != OpcodeLoadFirmwareParameter.LoadMinHourlyTemp)
                return;
              this.Nead_IsAccuA_NaN_follow = true;
            }
            else
            {
              DataTypeCodes result7;
              if (Enum.TryParse<DataTypeCodes>(lineParts[1], out result7))
              {
                byte[] collection = lineParts.Length >= 3 ? RuntimeParameter.GetValueBytesFromString(result7, lineParts[2]) : throw new Exception("Initialisation value not found");
                this.CodeBytes.Add((byte) result7);
                this.CodeBytes.AddRange((IEnumerable<byte>) collection);
              }
              else
              {
                this.ParameterIndex = new int?(-1);
                string parameterName = lineParts[1];
                bool flag = false;
                if (lineParts[1].EndsWith("!"))
                {
                  parameterName = lineParts[1].Length >= 2 ? lineParts[1].Substring(0, lineParts[1].Length - 1) : throw new Exception("Illegal function parameter name");
                  flag = true;
                }
                this.ParameterIndex = new int?(Parameters.FindIndex((Predicate<RuntimeParameter>) (x => x.Name == parameterName)));
                int? parameterIndex = this.ParameterIndex;
                int num1 = 0;
                int num2;
                if (!(parameterIndex.GetValueOrDefault() < num1 & parameterIndex.HasValue))
                {
                  parameterIndex = this.ParameterIndex;
                  int num3 = 32;
                  num2 = parameterIndex.GetValueOrDefault() >= num3 & parameterIndex.HasValue ? 1 : 0;
                }
                else
                  num2 = 1;
                if (num2 != 0)
                  throw new Exception("Parameter not defined: " + parameterName);
                if (flag)
                {
                  parameterIndex = this.ParameterIndex;
                  this.ParameterIndex = parameterIndex.HasValue ? new int?(parameterIndex.GetValueOrDefault() + 32) : new int?();
                }
                this.CodeBytes.Add((byte) this.ParameterIndex.Value);
              }
            }
          }
          else
          {
            OpcodeSpacialParameter result8;
            if (Enum.TryParse<OpcodeSpacialParameter>(lineParts[0], out result8))
            {
              this.Opcode = (byte) result8;
              this.CodeBytes.Add((byte) result8);
              if (lineParts.Length < 2)
                throw new Exception("No parameter found");
              switch (result8)
              {
                case OpcodeSpacialParameter.Store:
                  this.ParameterIndex = new int?(Parameters.FindIndex((Predicate<RuntimeParameter>) (x => x.Name == lineParts[1])));
                  int? parameterIndex = this.ParameterIndex;
                  int num = 0;
                  if (parameterIndex.GetValueOrDefault() < num & parameterIndex.HasValue)
                    throw new Exception("Store parameter not found: " + lineParts[1]);
                  this.CodeBytes.Add((byte) this.ParameterIndex.Value);
                  return;
                case OpcodeSpacialParameter.SaveEvent:
                case OpcodeSpacialParameter.SaveParameterEvent:
                  this.CodeBytes.Add((byte) 68);
                  SmartFunctionLoggerEventType result9;
                  if (!Enum.TryParse<SmartFunctionLoggerEventType>(lineParts[1], out result9))
                  {
                    if (lineParts.Length != 3 || !(lineParts[1] == "UInt8"))
                      throw new Exception("Illegal LoggerEvent: " + lineParts[1]);
                    byte result10;
                    if (!(!lineParts[2].StartsWith("0x") ? byte.TryParse(lineParts[2], out result10) : byte.TryParse(lineParts[2].Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result10)))
                      throw new Exception("Illegal LoggerEvent number");
                    this.CodeBytes.Add(result10);
                    return;
                  }
                  if (lineParts.Length > 2)
                    throw new Exception("Illegal number of parameters: " + this.SourceText);
                  this.CodeBytes.Add((byte) result9);
                  return;
                case OpcodeSpacialParameter.LoRaSendAlarm:
                  this.CodeBytes.Add((byte) 70);
                  LoRaAlarm result11;
                  if (!Enum.TryParse<LoRaAlarm>(lineParts[1], out result11))
                  {
                    if (lineParts.Length != 3 || !(lineParts[1] == "UInt32"))
                      throw new Exception("Illegal LoRaAlarm: " + lineParts[1]);
                    uint result12;
                    if (!(!lineParts[2].StartsWith("0x") ? uint.TryParse(lineParts[2], out result12) : uint.TryParse(lineParts[2].Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result12)))
                      throw new Exception("Illegal LoRaAlarm number");
                    this.CodeBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(result12));
                    return;
                  }
                  this.CodeBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((uint) result11));
                  return;
                case OpcodeSpacialParameter.ErrorExit:
                  SmartFunctionResult result13;
                  if (!Enum.TryParse<SmartFunctionResult>(lineParts[1], out result13))
                    throw new Exception("Illegal SmartFunctionResult: " + lineParts[1]);
                  this.CodeBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) result13));
                  return;
              }
            }
            throw new Exception("Unknown opcode: " + lineParts[0]);
          }
        }
      }
    }

    internal LineCode(byte[] code, ref ushort offset)
    {
      try
      {
        this.CodeStartOffset = offset;
        byte num1 = ByteArrayScanner16Bit.ScanByte(code, ref offset);
        this.Opcode = num1;
        if (Enum.IsDefined(typeof (OpcodeNoParameter), (object) num1))
        {
          OpcodeNoParameter opcodeNoParameter = (OpcodeNoParameter) num1;
          this.SourceText = opcodeNoParameter != OpcodeNoParameter.IllegalOpcode ? opcodeNoParameter.ToString() : throw new Exception("Illegal opcode 0x00 found.");
        }
        else if (Enum.IsDefined(typeof (OpcodeJump), (object) num1))
        {
          this.SourceText = ((OpcodeJump) num1).ToString();
          this.JumpDestinationOffset = new ushort?(ByteArrayScanner16Bit.ScanUInt16(code, ref offset));
        }
        else if (Enum.IsDefined(typeof (OpcodeLoadParameter), (object) num1))
        {
          this.SourceText = ((OpcodeLoadParameter) num1).ToString();
          byte parameterByte = ByteArrayScanner16Bit.ScanByte(code, ref offset);
          if (this.CheckParameter(parameterByte) || this.CheckConstant(parameterByte, code, ref offset, this.Opcode))
            return;
          OpcodeLoadFirmwareParameter firmwareParameter = Enum.IsDefined(typeof (OpcodeLoadFirmwareParameter), (object) parameterByte) ? (OpcodeLoadFirmwareParameter) parameterByte : throw new Exception("Illegal Load extention on: " + ((int) offset - 1).ToString());
          this.SourceText = this.SourceText + " " + firmwareParameter.ToString().Substring(4);
          if (firmwareParameter == OpcodeLoadFirmwareParameter.LoadStateCounter)
          {
            byte num2 = ByteArrayScanner16Bit.ScanByte(code, ref offset);
            if (!Enum.IsDefined(typeof (DeviceStateCounterID), (object) num2))
              throw new Exception("Illegal DeviceStateCounterID");
            this.SourceText = this.SourceText + " " + ((DeviceStateCounterID) num2).ToString();
          }
          if (firmwareParameter != OpcodeLoadFirmwareParameter.LoadSmartFunctionState && firmwareParameter != OpcodeLoadFirmwareParameter.LoadSmartFunctionTimeoutActive)
            return;
          byte num3 = ByteArrayScanner16Bit.ScanByte(code, ref offset);
          if (!Enum.IsDefined(typeof (SmartFunctionLoggerEventType), (object) num3))
            throw new Exception("Illegal SmartFunctionLoggerEventType");
          this.SourceText = this.SourceText + " " + ((SmartFunctionLoggerEventType) num3).ToString();
        }
        else if (Enum.IsDefined(typeof (OpcodeSpacialParameter), (object) num1))
        {
          OpcodeSpacialParameter spacialParameter = (OpcodeSpacialParameter) num1;
          this.SourceText = spacialParameter.ToString();
          int num4;
          switch (spacialParameter)
          {
            case OpcodeSpacialParameter.Store:
              if (this.CheckParameter(ByteArrayScanner16Bit.ScanByte(code, ref offset)))
                return;
              throw new Exception("Illegal Store command parameter");
            case OpcodeSpacialParameter.SaveEvent:
              num4 = 1;
              break;
            default:
              num4 = spacialParameter == OpcodeSpacialParameter.SaveParameterEvent ? 1 : 0;
              break;
          }
          if (num4 != 0)
          {
            byte num5 = ByteArrayScanner16Bit.ScanByte(code, ref offset) == (byte) 68 ? ByteArrayScanner16Bit.ScanByte(code, ref offset) : throw new Exception("Illegal data type for SaveEvent parameter");
            if (!Enum.IsDefined(typeof (SmartFunctionLoggerEventType), (object) num5))
              this.SourceText = this.SourceText + " UInt8 " + num5.ToString();
            else
              this.SourceText = this.SourceText + " " + ((SmartFunctionLoggerEventType) num5).ToString();
          }
          else if (spacialParameter == OpcodeSpacialParameter.LoRaSendAlarm)
          {
            uint num6 = ByteArrayScanner16Bit.ScanByte(code, ref offset) == (byte) 70 ? ByteArrayScanner16Bit.ScanUInt32(code, ref offset) : throw new Exception("Illegal data type for SaveEvent parameter");
            if (!Enum.IsDefined(typeof (LoRaAlarm), (object) num6))
              this.SourceText = this.SourceText + " UInt32 0x" + num6.ToString("x08");
            else
              this.SourceText = this.SourceText + " " + ((LoRaAlarm) num6).ToString();
          }
          else
          {
            if (spacialParameter != OpcodeSpacialParameter.ErrorExit)
              throw new Exception("Not supported OpcodeSpacialParameter");
            ushort num7 = ByteArrayScanner16Bit.ScanUInt16(code, ref offset);
            if (!Enum.IsDefined(typeof (SmartFunctionResult), (object) num7))
              throw new Exception("Illegal SmartFunctionResult");
            this.SourceText = this.SourceText + " " + ((SmartFunctionResult) num7).ToString();
          }
        }
        else
          this.SourceText = Enum.IsDefined(typeof (OpcodeLoadFirmwareParameter), (object) num1) ? ((OpcodeLoadFirmwareParameter) num1).ToString() : throw new Exception("Illegal opcode 0x" + num1.ToString("x02") + " at offset: 0x" + ((int) offset - 1).ToString("x03"));
      }
      catch (Exception ex)
      {
        throw new Exception("LineCode exception on offset: 0x" + this.CodeStartOffset.ToString("x03") + Environment.NewLine, ex);
      }
    }

    private bool CheckParameter(byte parameterByte)
    {
      if (parameterByte < (byte) 32)
      {
        this.ParameterIndex = new int?((int) parameterByte);
        return true;
      }
      if (parameterByte >= (byte) 64)
        return false;
      this.ParameterIndex = new int?((int) parameterByte - 32);
      this.SourceText += " !";
      return true;
    }

    private bool CheckConstant(byte parameterByte, byte[] code, ref ushort offset, byte opcode)
    {
      if (parameterByte >= (byte) 96)
        return false;
      DataTypeCodes typeCode = Enum.IsDefined(typeof (DataTypeCodes), (object) parameterByte) ? (DataTypeCodes) parameterByte : throw new Exception("Illegal constant data type code: " + ((int) offset - 1).ToString());
      Register register = new Register();
      register.LoadValue(typeCode, code, (int) offset);
      offset += register.GetStorageByteSize(typeCode);
      if (opcode == (byte) 65 || opcode == (byte) 68)
      {
        if (typeCode != DataTypeCodes.UInt8)
          throw new Exception("Illegal type for SaveEvent: " + typeCode.ToString());
        this.SourceText = this.SourceText + " " + ((SmartFunctionLoggerEventType) (int) register.RegisterValue).ToString();
      }
      else if (opcode == (byte) 66)
      {
        if (typeCode != DataTypeCodes.UInt32)
          throw new Exception("Illegal type for LoRaSendAlarm: " + typeCode.ToString());
        this.SourceText = this.SourceText + " " + ((LoRaAlarm) (int) register.RegisterValue).ToString();
      }
      else
        this.SourceText = this.SourceText + " " + typeCode.ToString() + " " + register.GetValue();
      return true;
    }

    internal void SetParameterName(List<RuntimeParameter> allParameters)
    {
      if (!this.ParameterIndex.HasValue)
        return;
      if (this.ParameterIndex.Value < 0 || this.ParameterIndex.Value >= allParameters.Count)
        throw new Exception("Illegal parameter index");
      this.SourceText = !this.SourceText.EndsWith("!") ? this.SourceText + " " + allParameters[this.ParameterIndex.Value].Name : this.SourceText.Substring(0, this.SourceText.Length - 1) + allParameters[this.ParameterIndex.Value].Name + "!";
    }

    internal void AddJumpDestinationOffset(SortedList<ushort, string> lableOffsets)
    {
      if (!this.JumpDestinationOffset.HasValue || lableOffsets.ContainsKey(this.JumpDestinationOffset.Value))
        return;
      lableOffsets.Add(this.JumpDestinationOffset.Value, "Lable_" + this.JumpDestinationOffset.Value.ToString("x03"));
    }

    internal void AddJump(SortedList<ushort, string> lableOffsets)
    {
      if (!this.JumpDestinationOffset.HasValue)
        return;
      int index = lableOffsets.IndexOfKey(this.JumpDestinationOffset.Value);
      if (index < 0)
        throw new Exception("Lable not defined.");
      this.SourceText = this.SourceText + " " + lableOffsets.Values[index];
    }

    internal void AddLable(SortedList<ushort, string> lableOffsets)
    {
      int index = lableOffsets.IndexOfKey(this.CodeStartOffset);
      if (index < 0)
        return;
      if (lableOffsets.Values[index] == "EventCode")
        this.SourceText = Environment.NewLine + "# ************* Event code ***************" + Environment.NewLine + lableOffsets.Values[index] + ":" + Environment.NewLine + this.SourceText;
      else
        this.SourceText = Environment.NewLine + lableOffsets.Values[index] + ":" + Environment.NewLine + this.SourceText;
    }

    public void AddLableToList(SortedList<string, LineCode> labelList)
    {
      if (this.LineLable == null)
        return;
      if (labelList.ContainsKey(this.LineLable))
        throw new Exception("Label redefinition");
      labelList.Add(this.LineLable, this);
    }

    public void SetJumpOffset(SortedList<string, LineCode> labelList)
    {
      if (this.JumpToLable == null)
        return;
      if (!labelList.ContainsKey(this.JumpToLable))
        throw new Exception("Label not defined: " + this.JumpToLable);
      ushort codeStartOffset = labelList[this.JumpToLable].CodeStartOffset;
      this.CodeBytes.RemoveRange(this.CodeBytes.Count - 2, 2);
      this.CodeBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(codeStartOffset));
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.LineLable != null)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(this.LineLable + ":");
      }
      stringBuilder.Append("0x" + this.CodeStartOffset.ToString("x03") + ": ");
      for (int index = 0; index < this.CodeBytes.Count; ++index)
        stringBuilder.Append(this.CodeBytes[index].ToString("x02"));
      for (int count = this.CodeBytes.Count; count < 10; ++count)
        stringBuilder.Append(" .");
      stringBuilder.Append(" ");
      stringBuilder.AppendLine(this.SourceText);
      return stringBuilder.ToString();
    }
  }
}
