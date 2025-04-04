// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.SmartFunctionParameter
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using SmartFunctionCompiler;
using System;

#nullable disable
namespace S4_Handler.Functions
{
  public class SmartFunctionParameter
  {
    public string ParameterName { get; private set; }

    public DataTypeCodes ParameterType { get; private set; }

    public ParameterScaling Scaling { get; private set; }

    public StorageTypeCodes StorageType { get; private set; }

    public string ParameterValue { get; set; }

    public SmartFunctionParameter Clone(
      SmartFunctionParameter sameParameterFromCode = null,
      bool removeByteListLength = false)
    {
      SmartFunctionParameter functionParameter = new SmartFunctionParameter();
      functionParameter.ParameterName = this.ParameterName;
      functionParameter.ParameterType = this.ParameterType;
      functionParameter.Scaling = this.Scaling;
      functionParameter.StorageType = this.StorageType;
      if (functionParameter.ParameterType == DataTypeCodes.ByteList && !removeByteListLength)
      {
        functionParameter.ParameterValue = this.ParameterValue;
      }
      else
      {
        int num = this.ParameterValue.IndexOf(';');
        functionParameter.ParameterValue = num != -1 ? this.ParameterValue.Substring(num + 1) : this.ParameterValue;
      }
      if (sameParameterFromCode != null)
      {
        functionParameter.Scaling = sameParameterFromCode.Scaling;
        functionParameter.StorageType = sameParameterFromCode.StorageType;
      }
      return functionParameter;
    }

    public SmartFunctionParameter() => this.Scaling = ParameterScaling.none;

    public SmartFunctionParameter(byte[] byteArray, ref int offset, bool fromCode = false)
    {
      if (fromCode)
      {
        byte num = ByteArrayScanner.ScanByte(byteArray, ref offset);
        this.Scaling = (ParameterScaling) ((int) num & 240);
        this.StorageType = (StorageTypeCodes) ((int) num & 15);
        this.ParameterType = (DataTypeCodes) ByteArrayScanner.ScanByte(byteArray, ref offset);
      }
      else
      {
        this.ParameterName = ByteArrayScanner.ScanString(byteArray, ref offset);
        this.ParameterType = (DataTypeCodes) ByteArrayScanner.ScanByte(byteArray, ref offset);
      }
      switch (this.ParameterType)
      {
        case DataTypeCodes.Int8:
          this.ParameterValue = ByteArrayScanner.ScanSByte(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.Int16:
          this.ParameterValue = ByteArrayScanner.ScanInt16(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.Int32:
          this.ParameterValue = ByteArrayScanner.ScanInt32(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.Int64:
          this.ParameterValue = ByteArrayScanner.ScanInt64(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.UInt8:
          this.ParameterValue = ByteArrayScanner.ScanByte(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.UInt16:
          this.ParameterValue = ByteArrayScanner.ScanUInt16(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.UInt32:
          this.ParameterValue = ByteArrayScanner.ScanUInt32(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.UInt64:
          this.ParameterValue = ByteArrayScanner.ScanUInt64(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.Float:
          this.ParameterValue = ByteArrayScanner.ScanFloat(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.Double:
          this.ParameterValue = ByteArrayScanner.ScanDouble(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.DateTime:
          this.ParameterValue = ByteArrayScanner.ScanDateTime(byteArray, ref offset).ToString();
          break;
        case DataTypeCodes.ByteList:
          SmartFunctionByteList functionByteList = new SmartFunctionByteList(byteArray, offset);
          this.ParameterValue = functionByteList.ToString();
          offset += (int) functionByteList.NumberOfBytes + 1;
          break;
        default:
          throw new Exception("Illegal type code found");
      }
      if (!fromCode)
        return;
      this.ParameterName = ByteArrayScanner.ScanString(byteArray, ref offset);
    }

    public void ScanIn(byte[] byteArray, ref int offset)
    {
      try
      {
        ByteArrayScanner.ScanInString(byteArray, this.ParameterName, ref offset);
        switch (this.ParameterType)
        {
          case DataTypeCodes.Int8:
            ByteArrayScanner.ScanInSByte(byteArray, sbyte.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.Int16:
            ByteArrayScanner.ScanInInt16(byteArray, short.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.Int32:
            ByteArrayScanner.ScanInInt32(byteArray, int.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.Int64:
            ByteArrayScanner.ScanInInt64(byteArray, long.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.UInt8:
            ByteArrayScanner.ScanInByte(byteArray, byte.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.UInt16:
            ByteArrayScanner.ScanInUInt16(byteArray, ushort.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.UInt32:
            ByteArrayScanner.ScanInUInt32(byteArray, uint.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.UInt64:
            ByteArrayScanner.ScanInUInt64(byteArray, ulong.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.Float:
            ByteArrayScanner.ScanInFloat(byteArray, float.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.Double:
            ByteArrayScanner.ScanInDouble(byteArray, double.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.DateTime2000:
            ByteArrayScanner.ScanInDateTime2000(byteArray, DateTime.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.DateTime:
            ByteArrayScanner.ScanInDateTime(byteArray, DateTime.Parse(this.ParameterValue), ref offset);
            break;
          case DataTypeCodes.ByteList:
            new SmartFunctionByteList(this.ParameterValue).ScanIn(byteArray, ref offset);
            break;
          default:
            throw new Exception("Illegal type code found");
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Parse in error on " + this.ParameterName, ex);
      }
    }

    public object ScanValue(byte[] protocolData, ref int readOffset)
    {
      switch (this.ParameterType)
      {
        case DataTypeCodes.Int8:
          return (object) ByteArrayScanner.ScanSByte(protocolData, ref readOffset);
        case DataTypeCodes.Int16:
          return (object) ByteArrayScanner.ScanInt16(protocolData, ref readOffset);
        case DataTypeCodes.Int32:
          return (object) ByteArrayScanner.ScanInt32(protocolData, ref readOffset);
        case DataTypeCodes.Int64:
          return (object) ByteArrayScanner.ScanInt64(protocolData, ref readOffset);
        case DataTypeCodes.UInt8:
          return (object) ByteArrayScanner.ScanByte(protocolData, ref readOffset);
        case DataTypeCodes.UInt16:
          return (object) ByteArrayScanner.ScanUInt16(protocolData, ref readOffset);
        case DataTypeCodes.UInt32:
          return (object) ByteArrayScanner.ScanUInt32(protocolData, ref readOffset);
        case DataTypeCodes.UInt64:
          return (object) ByteArrayScanner.ScanUInt64(protocolData, ref readOffset);
        case DataTypeCodes.Float:
          return (object) ByteArrayScanner.ScanFloat(protocolData, ref readOffset);
        case DataTypeCodes.Double:
          return (object) ByteArrayScanner.ScanDouble(protocolData, ref readOffset);
        case DataTypeCodes.DateTime2000:
          return (object) ByteArrayScanner.ScanDateTime(protocolData, ref readOffset);
        case DataTypeCodes.ByteList:
          SmartFunctionByteList functionByteList = new SmartFunctionByteList(protocolData, readOffset);
          readOffset += (int) functionByteList.NumberOfBytes + 1;
          return (object) functionByteList;
        default:
          return (object) null;
      }
    }

    public string LoggerParameterName
    {
      get
      {
        string parameterName = this.ParameterName;
        return !parameterName.StartsWith("L_") || parameterName.Length < 3 ? (string) null : parameterName.Substring(2);
      }
    }

    public override string ToString()
    {
      return this.ParameterName + " (" + this.ParameterType.ToString() + "): " + this.ParameterValue;
    }
  }
}
