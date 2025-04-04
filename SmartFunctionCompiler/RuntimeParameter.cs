// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.RuntimeParameter
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
  public class RuntimeParameter
  {
    private byte[] CodeStorage;
    private ushort ParameterOffset;

    internal RuntimeParameter(byte[] code, ref ushort offset)
    {
      this.CodeStorage = code;
      this.ParameterOffset = offset;
      offset += (ushort) this.ParameterBytes.Length;
    }

    internal RuntimeParameter(byte[] code, ushort offset)
    {
      this.CodeStorage = code;
      this.ParameterOffset = offset;
    }

    public StorageTypeCodes StorageCode
    {
      get => (StorageTypeCodes) ((int) this.CodeStorage[(int) this.ParameterOffset] & 15);
    }

    public ParameterScaling Scaling
    {
      get => (ParameterScaling) ((int) this.CodeStorage[(int) this.ParameterOffset] & 240);
    }

    public DataTypeCodes TypeCode
    {
      get => (DataTypeCodes) this.CodeStorage[(int) this.ParameterOffset + 1];
    }

    public ushort ValueByteSize
    {
      get
      {
        return this.TypeCode != DataTypeCodes.ByteList ? RuntimeParameter.GetByteSizeFromType(this.TypeCode) : RuntimeParameter.GetByteSizeFromType(this.TypeCode, this.CodeStorage[(int) this.ParameterOffset + 2]);
      }
    }

    public ushort ValueOffset => (ushort) ((uint) this.ParameterOffset + 2U);

    public string Name
    {
      get
      {
        ushort startOffset = (ushort) ((uint) this.ParameterOffset + 2U + (uint) this.ValueByteSize);
        return RuntimeParameter.StringFromByteArray(this.CodeStorage, ref startOffset);
      }
    }

    public string LoggerParameterName
    {
      get
      {
        string name = this.Name;
        return !name.StartsWith("L_") || name.Length < 3 ? (string) null : name.Substring(2);
      }
    }

    public byte[] ParameterBytes
    {
      get
      {
        ushort length = (ushort) (2 + (int) this.ValueByteSize + this.Name.Length + 1);
        byte[] destinationArray = new byte[(int) length];
        Array.Copy((Array) this.CodeStorage, (int) this.ParameterOffset, (Array) destinationArray, 0, (int) length);
        return destinationArray;
      }
    }

    public byte[] GetValueBytes(ushort parameterOffset)
    {
      ushort valueByteSize = this.ValueByteSize;
      byte[] dst = new byte[(int) valueByteSize];
      Buffer.BlockCopy((Array) FunctionAccessStorage.GetStorageFromStorageCode(this.StorageCode), (int) parameterOffset, (Array) dst, 0, (int) valueByteSize);
      return dst;
    }

    public byte[] InitValueBytes
    {
      get
      {
        byte[] destinationArray = new byte[(int) this.ValueByteSize];
        Array.Copy((Array) this.CodeStorage, (int) this.ValueOffset, (Array) destinationArray, 0, destinationArray.Length);
        return destinationArray;
      }
    }

    internal ushort InitValue_UInt16 => BitConverter.ToUInt16(this.InitValueBytes, 0);

    internal Register GetValueRegister()
    {
      Register valueRegister = new Register();
      valueRegister.LoadValue(this.TypeCode, this.CodeStorage, (int) this.ValueOffset);
      return valueRegister;
    }

    internal static RuntimeParameter GetRuntimeParameter(
      string[] lines,
      ref StorageTypeCodes storageCode,
      ref int lineNumber)
    {
      while (lines[lineNumber].Length == 0)
        ++lineNumber;
      if (lines[lineNumber].Contains("ResetCode:"))
        return (RuntimeParameter) null;
      StorageTypeCodes result;
      if (Enum.TryParse<StorageTypeCodes>(lines[lineNumber], out result))
      {
        storageCode = result;
        ++lineNumber;
      }
      string[] ParameterParts = lines[lineNumber].Split(',');
      ++lineNumber;
      string theString = ParameterParts.Length >= 2 && ParameterParts.Length <= 4 ? ParameterParts[0].Trim() : throw new Exception("Illegal RuntimeParameter definition. Number of parts not ok");
      if (theString.Length > 20)
        throw new Exception("Parameter name exceeds 20 caracters: " + theString);
      DataTypeCodes typeCodeFromString = RuntimeParameter.GetTypeCodeFromString(ParameterParts[1]);
      ParameterScaling parameterScaling = ParameterScaling.none;
      byte[] collection;
      if (typeCodeFromString != DataTypeCodes.ByteList)
      {
        collection = ParameterParts.Length != 2 ? RuntimeParameter.GetValueBytesFromString(typeCodeFromString, ParameterParts[2]) : throw new Exception("Illegal RuntimeParameter definition. Number of parts not ok");
        if (ParameterParts.Length == 4)
        {
          switch (ParameterParts[3].Trim())
          {
            case "Volume":
              parameterScaling = ParameterScaling.Volume;
              if (typeCodeFromString != DataTypeCodes.Double)
                throw new Exception("Volume scaled parameters need the type double");
              break;
            case "Flow":
              parameterScaling = ParameterScaling.Flow;
              if (typeCodeFromString != DataTypeCodes.Float)
                throw new Exception("Flow scaled parameters need the type float");
              break;
            case "Temperature":
              parameterScaling = ParameterScaling.Temperature;
              if (typeCodeFromString != DataTypeCodes.Float)
                throw new Exception("Temperature scaled parameters need the type float");
              break;
            case "State":
              parameterScaling = ParameterScaling.State;
              if (typeCodeFromString != DataTypeCodes.UInt16)
                throw new Exception("State scaled parameters need the type UInt16");
              break;
            default:
              throw new Exception("Illegal scaling information.");
          }
        }
        if (theString == "Logger")
        {
          if (storageCode != StorageTypeCodes.ram && storageCode != StorageTypeCodes.flash)
            throw new Exception("Logger parameter needs storage ram or flash");
          if (typeCodeFromString != DataTypeCodes.UInt16)
            throw new Exception("Logger parameter needs type UInt16");
        }
        if (theString.StartsWith("L_") && storageCode != 0)
          throw new Exception("Temporary logger parameter 'L_...' needs storage ram");
      }
      else
        collection = RuntimeParameter.GetValueBytesForByteList(ParameterParts, lines, ref lineNumber);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) (storageCode | (StorageTypeCodes) parameterScaling));
      byteList.Add((byte) typeCodeFromString);
      byteList.AddRange((IEnumerable<byte>) collection);
      byteList.AddRange((IEnumerable<byte>) RuntimeParameter.ByteArrayFromString(theString));
      byte[] array = byteList.ToArray();
      ushort offset = 0;
      return new RuntimeParameter(array, ref offset);
    }

    public override string ToString()
    {
      string str = this.Name + ", " + this.StorageCode.ToString() + ", " + this.TypeCode.ToString() + ", " + RuntimeParameter.GetStringValueFromBytes(this.TypeCode, this.CodeStorage, (int) this.ValueOffset);
      return this.Scaling == ParameterScaling.none ? str : str + ", " + this.Scaling.ToString();
    }

    public string DeCompile()
    {
      string str = this.Name + ", " + this.TypeCode.ToString() + ", " + RuntimeParameter.GetStringValueFromBytes(this.TypeCode, this.CodeStorage, (int) this.ValueOffset);
      return this.Scaling == ParameterScaling.none ? str : str + ", " + this.Scaling.ToString();
    }

    public string ToString(ref ushort codeOffset)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("0x" + codeOffset.ToString("x03") + ": ");
      stringBuilder.Append(((byte) (this.StorageCode + (int) this.Scaling)).ToString("x02"));
      stringBuilder.Append(((byte) this.TypeCode).ToString("x02"));
      if (this.TypeCode != DataTypeCodes.ByteList)
      {
        for (int index = 0; index < 18; ++index)
        {
          if (index < (int) this.ValueByteSize)
            stringBuilder.Append(this.CodeStorage[(int) this.ValueOffset + index].ToString("x02"));
          else
            stringBuilder.Append(". ");
        }
      }
      else
      {
        for (int index = 0; index < (int) this.ValueByteSize; ++index)
          stringBuilder.Append(this.CodeStorage[(int) this.ValueOffset + index].ToString("x02"));
        stringBuilder.AppendLine();
        stringBuilder.Append(" -> ");
      }
      stringBuilder.Append(this.StorageCode.ToString());
      stringBuilder.Append("," + this.TypeCode.ToString());
      stringBuilder.Append("," + RuntimeParameter.GetStringValueFromBytes(this.TypeCode, this.CodeStorage, (int) this.ValueOffset));
      if (this.Scaling != 0)
        stringBuilder.Append("," + this.Scaling.ToString());
      stringBuilder.AppendLine();
      int index1 = 2 + (int) this.ValueByteSize;
      codeOffset += (ushort) index1;
      stringBuilder.Append("0x" + codeOffset.ToString("x03") + ": ");
      for (int index2 = 0; index2 < 20; ++index2)
      {
        if (index1 < this.ParameterBytes.Length)
        {
          stringBuilder.Append(this.ParameterBytes[index1].ToString("x02"));
          ++index1;
        }
        else
          stringBuilder.Append(". ");
      }
      stringBuilder.AppendLine(this.Name);
      codeOffset += (ushort) (this.ParameterBytes.Length - index1);
      return stringBuilder.ToString();
    }

    public object ScanValue(byte[] protocolData, ref ushort readOffset)
    {
      switch (this.TypeCode)
      {
        case DataTypeCodes.Int8:
          return (object) ByteArrayScanner16Bit.ScanSByte(protocolData, ref readOffset);
        case DataTypeCodes.Int16:
          return (object) ByteArrayScanner16Bit.ScanInt16(protocolData, ref readOffset);
        case DataTypeCodes.Int32:
          return (object) ByteArrayScanner16Bit.ScanInt32(protocolData, ref readOffset);
        case DataTypeCodes.Int64:
          return (object) ByteArrayScanner16Bit.ScanInt64(protocolData, ref readOffset);
        case DataTypeCodes.UInt8:
          return (object) ByteArrayScanner16Bit.ScanByte(protocolData, ref readOffset);
        case DataTypeCodes.UInt16:
          return (object) ByteArrayScanner16Bit.ScanUInt16(protocolData, ref readOffset);
        case DataTypeCodes.UInt32:
          return (object) ByteArrayScanner16Bit.ScanUInt32(protocolData, ref readOffset);
        case DataTypeCodes.UInt64:
          return (object) ByteArrayScanner16Bit.ScanUInt64(protocolData, ref readOffset);
        case DataTypeCodes.Float:
          return (object) ByteArrayScanner16Bit.ScanFloat(protocolData, ref readOffset);
        case DataTypeCodes.Double:
          return (object) ByteArrayScanner16Bit.ScanDouble(protocolData, ref readOffset);
        case DataTypeCodes.DateTime2000:
          return (object) ByteArrayScanner16Bit.ScanDateTime(protocolData, ref readOffset);
        default:
          return (object) null;
      }
    }

    internal static byte[] ByteArrayFromString(string theString)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(theString);
      Array.Resize<byte>(ref bytes, bytes.Length + 1);
      bytes[bytes.Length - 1] = (byte) 0;
      return bytes;
    }

    internal static string StringFromByteArray(byte[] theBytes, ref ushort startOffset)
    {
      int count = 0;
      while (true)
      {
        if (theBytes.Length >= (int) startOffset + count + 1)
        {
          if (theBytes[(int) startOffset + count] != (byte) 0)
            ++count;
          else
            goto label_5;
        }
        else
          break;
      }
      throw new Exception("String end not inside the byte[] array");
label_5:
      string str = Encoding.ASCII.GetString(theBytes, (int) startOffset, count);
      startOffset += (ushort) (count + 1);
      return str;
    }

    public static ushort GetByteSizeFromType(DataTypeCodes typeCode, byte typeFollowingByte = 0)
    {
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
          return 1;
        case DataTypeCodes.Int16:
          return 2;
        case DataTypeCodes.Int32:
          return 4;
        case DataTypeCodes.Int64:
          return 8;
        case DataTypeCodes.UInt8:
          return 1;
        case DataTypeCodes.UInt16:
          return 2;
        case DataTypeCodes.UInt32:
          return 4;
        case DataTypeCodes.UInt64:
          return 8;
        case DataTypeCodes.Float:
          return 4;
        case DataTypeCodes.Double:
          return 8;
        case DataTypeCodes.DateTime2000:
          return 4;
        case DataTypeCodes.ByteList:
          return (ushort) ((uint) typeFollowingByte + 1U);
        default:
          Interpreter.Error = SmartFunctionResult.IllegalDataTypeCode;
          return 0;
      }
    }

    internal static DataTypeCodes GetTypeCodeFromString(string typeCodeString)
    {
      string str1 = typeCodeString.Trim();
      if (str1 == DataTypeCodes.Int8.ToString())
        return DataTypeCodes.Int8;
      string str2 = str1;
      DataTypeCodes dataTypeCodes = DataTypeCodes.Int16;
      string str3 = dataTypeCodes.ToString();
      if (str2 == str3)
        return DataTypeCodes.Int16;
      string str4 = str1;
      dataTypeCodes = DataTypeCodes.Int32;
      string str5 = dataTypeCodes.ToString();
      if (str4 == str5)
        return DataTypeCodes.Int32;
      string str6 = str1;
      dataTypeCodes = DataTypeCodes.Int64;
      string str7 = dataTypeCodes.ToString();
      if (str6 == str7)
        return DataTypeCodes.Int64;
      string str8 = str1;
      dataTypeCodes = DataTypeCodes.UInt8;
      string str9 = dataTypeCodes.ToString();
      if (str8 == str9)
        return DataTypeCodes.UInt8;
      string str10 = str1;
      dataTypeCodes = DataTypeCodes.UInt16;
      string str11 = dataTypeCodes.ToString();
      if (str10 == str11)
        return DataTypeCodes.UInt16;
      string str12 = str1;
      dataTypeCodes = DataTypeCodes.UInt32;
      string str13 = dataTypeCodes.ToString();
      if (str12 == str13)
        return DataTypeCodes.UInt32;
      string str14 = str1;
      dataTypeCodes = DataTypeCodes.UInt64;
      string str15 = dataTypeCodes.ToString();
      if (str14 == str15)
        return DataTypeCodes.UInt64;
      string str16 = str1;
      dataTypeCodes = DataTypeCodes.Float;
      string str17 = dataTypeCodes.ToString();
      if (str16 == str17)
        return DataTypeCodes.Float;
      string str18 = str1;
      dataTypeCodes = DataTypeCodes.Double;
      string str19 = dataTypeCodes.ToString();
      if (str18 == str19)
        return DataTypeCodes.Double;
      string str20 = str1;
      dataTypeCodes = DataTypeCodes.DateTime2000;
      string str21 = dataTypeCodes.ToString();
      if (str20 == str21)
        return DataTypeCodes.DateTime2000;
      string str22 = str1;
      dataTypeCodes = DataTypeCodes.ByteList;
      string str23 = dataTypeCodes.ToString();
      if (str22 == str23)
        return DataTypeCodes.ByteList;
      throw new Exception("Illegal data type code");
    }

    internal static byte[] GetValueBytesFromString(DataTypeCodes typeCode, string ValueString)
    {
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
          return BitConverter.GetBytes((short) sbyte.Parse(ValueString));
        case DataTypeCodes.Int16:
          return BitConverter.GetBytes(short.Parse(ValueString));
        case DataTypeCodes.Int32:
          return BitConverter.GetBytes(int.Parse(ValueString));
        case DataTypeCodes.Int64:
          return BitConverter.GetBytes(long.Parse(ValueString));
        case DataTypeCodes.UInt8:
          return new byte[1]{ byte.Parse(ValueString) };
        case DataTypeCodes.UInt16:
          return BitConverter.GetBytes(ushort.Parse(ValueString));
        case DataTypeCodes.UInt32:
          return BitConverter.GetBytes(uint.Parse(ValueString));
        case DataTypeCodes.UInt64:
          return BitConverter.GetBytes(ulong.Parse(ValueString));
        case DataTypeCodes.Float:
          return BitConverter.GetBytes(float.Parse(ValueString, (IFormatProvider) CultureInfo.InvariantCulture));
        case DataTypeCodes.Double:
          return BitConverter.GetBytes(double.Parse(ValueString, (IFormatProvider) CultureInfo.InvariantCulture));
        case DataTypeCodes.DateTime2000:
          return BitConverter.GetBytes(CalendarBase2000.Cal_GetMeterTime(DateTime.Parse(ValueString)));
        default:
          throw new Exception("Illegal type code found");
      }
    }

    internal static byte[] GetValueBytesForByteList(
      string[] ParameterParts,
      string[] lines,
      ref int lineNumber)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 0);
      if (ParameterParts.Length > 2)
      {
        string[] strArray1 = ParameterParts[2].Trim().Split(';');
        string[] strArray2 = strArray1.Length == 2 ? strArray1[0].Trim().Split(':') : throw new Exception("Illegal ByteList bytes definition");
        byteList[0] = strArray2.Length == 2 && strArray2[0].StartsWith("NoOfBytes") ? byte.Parse(strArray2[1]) : throw new Exception("Illegal ByteList bytes definition");
        byte[] byteArray = ByteArrayScanner16Bit.HexStringToByteArray(strArray1[1].Replace(" ", ""));
        byteList.AddRange((IEnumerable<byte>) byteArray);
        return byteList.ToArray();
      }
      while (true)
      {
        if (lineNumber <= lines.Length)
        {
          if (lines[lineNumber].Length == 0)
            ++lineNumber;
          else if (!(lines[lineNumber] == "ByteListEnd"))
          {
            string[] strArray = lines[lineNumber].Split(':');
            if (strArray.Length >= 1 && strArray.Length <= 2)
            {
              if (strArray.Length == 2)
              {
                DataTypeCodes typeCodeFromString = RuntimeParameter.GetTypeCodeFromString(strArray[0]);
                byteList.AddRange((IEnumerable<byte>) RuntimeParameter.GetValueBytesFromString(typeCodeFromString, strArray[1]));
              }
              else
              {
                SmartFunctionLoggerEventType result1;
                if (Enum.TryParse<SmartFunctionLoggerEventType>(strArray[0], false, out result1))
                {
                  byteList.Add((byte) result1);
                }
                else
                {
                  LoRaAlarm result2;
                  if (Enum.TryParse<LoRaAlarm>(strArray[0], false, out result2))
                    byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((uint) result2));
                  else
                    goto label_19;
                }
              }
              ++lineNumber;
            }
            else
              goto label_12;
          }
          else
            goto label_10;
        }
        else
          break;
      }
      throw new Exception("End of ByteList not found");
label_10:
      byteList[0] = (byte) (byteList.Count - 1);
      ++lineNumber;
      return byteList.ToArray();
label_12:
      throw new Exception("Illegal number of line parts on line number: " + lineNumber.ToString());
label_19:
      throw new Exception("Illegal ByteList definition on line: " + lineNumber.ToString());
    }

    public static string GetStringValueFromBytes(
      DataTypeCodes typeCode,
      byte[] valueBytes,
      int offset)
    {
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
          return ((sbyte) valueBytes[offset]).ToString();
        case DataTypeCodes.Int16:
          return BitConverter.ToInt16(valueBytes, offset).ToString();
        case DataTypeCodes.Int32:
          return BitConverter.ToInt32(valueBytes, offset).ToString();
        case DataTypeCodes.Int64:
          return BitConverter.ToInt64(valueBytes, offset).ToString();
        case DataTypeCodes.UInt8:
          return valueBytes[offset].ToString();
        case DataTypeCodes.UInt16:
          return BitConverter.ToUInt16(valueBytes, offset).ToString();
        case DataTypeCodes.UInt32:
          return BitConverter.ToUInt32(valueBytes, offset).ToString();
        case DataTypeCodes.UInt64:
          return BitConverter.ToUInt64(valueBytes, offset).ToString();
        case DataTypeCodes.Float:
          return BitConverter.ToSingle(valueBytes, offset).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case DataTypeCodes.Double:
          return BitConverter.ToDouble(valueBytes, offset).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case DataTypeCodes.DateTime2000:
          return CalendarBase2000.Cal_GetDateTime(BitConverter.ToUInt32(valueBytes, offset)).ToString();
        case DataTypeCodes.ByteList:
          return new SmartFunctionByteList(valueBytes, offset).ToString();
        default:
          throw new Exception("Illegal type code found");
      }
    }
  }
}
