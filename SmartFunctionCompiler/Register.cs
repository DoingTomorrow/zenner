// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.Register
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public class Register
  {
    public DataTypeCodes RegisterTypeCode;
    public object RegisterValue;

    public Register()
    {
      this.RegisterTypeCode = DataTypeCodes.Int32;
      this.RegisterValue = (object) 0;
    }

    public Register Clone()
    {
      return new Register()
      {
        RegisterTypeCode = this.RegisterTypeCode,
        RegisterValue = this.RegisterValue
      };
    }

    public void LoadValue(DataTypeCodes typeCode, byte[] storage, int offset)
    {
      this.RegisterTypeCode = typeCode;
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
          this.RegisterTypeCode = DataTypeCodes.Int32;
          this.RegisterValue = (object) (int) (sbyte) storage[offset];
          break;
        case DataTypeCodes.Int16:
          this.RegisterTypeCode = DataTypeCodes.Int32;
          this.RegisterValue = (object) (int) BitConverter.ToInt16(storage, offset);
          break;
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) BitConverter.ToInt32(storage, offset);
          break;
        case DataTypeCodes.Int64:
          this.RegisterTypeCode = DataTypeCodes.Double;
          this.RegisterValue = (object) (double) BitConverter.ToInt64(storage, offset);
          break;
        case DataTypeCodes.UInt8:
          this.RegisterTypeCode = DataTypeCodes.Int32;
          this.RegisterValue = (object) (int) storage[offset];
          break;
        case DataTypeCodes.UInt16:
          this.RegisterTypeCode = DataTypeCodes.Int32;
          this.RegisterValue = (object) (int) BitConverter.ToUInt16(storage, offset);
          break;
        case DataTypeCodes.UInt32:
          this.RegisterTypeCode = DataTypeCodes.Int32;
          this.RegisterValue = (object) (int) BitConverter.ToUInt32(storage, offset);
          break;
        case DataTypeCodes.UInt64:
          this.RegisterTypeCode = DataTypeCodes.Double;
          this.RegisterValue = (object) (double) (long) BitConverter.ToUInt64(storage, offset);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) BitConverter.ToSingle(storage, offset);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) BitConverter.ToDouble(storage, offset);
          break;
        default:
          throw new Exception("Illegal type code found");
      }
    }

    public void StoreValue(DataTypeCodes typeCode, byte[] storage, int offset)
    {
      this.Clone();
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
        case DataTypeCodes.Int16:
        case DataTypeCodes.Int32:
        case DataTypeCodes.UInt8:
        case DataTypeCodes.UInt16:
        case DataTypeCodes.UInt32:
          this.AccuToInt32();
          break;
        case DataTypeCodes.Int64:
        case DataTypeCodes.UInt64:
        case DataTypeCodes.Double:
          this.AccuToDouble();
          break;
        case DataTypeCodes.Float:
          this.AccuToFloat();
          break;
        default:
          Interpreter.Error = SmartFunctionResult.IllegalDataTypeCode;
          return;
      }
      byte[] bytes;
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          bytes = BitConverter.GetBytes((int) this.RegisterValue);
          break;
        case DataTypeCodes.Float:
          bytes = BitConverter.GetBytes((float) this.RegisterValue);
          break;
        case DataTypeCodes.Double:
          switch (typeCode)
          {
            case DataTypeCodes.Int64:
              bytes = BitConverter.GetBytes((long) (double) this.RegisterValue);
              break;
            case DataTypeCodes.UInt64:
              bytes = BitConverter.GetBytes((ulong) (double) this.RegisterValue);
              break;
            case DataTypeCodes.Double:
              bytes = BitConverter.GetBytes((double) this.RegisterValue);
              break;
            default:
              Interpreter.Error = SmartFunctionResult.IllegalDataTypeCode;
              return;
          }
          break;
        default:
          Interpreter.Error = SmartFunctionResult.IllegalDataTypeCode;
          return;
      }
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Int8:
        case DataTypeCodes.UInt8:
          storage[offset] = bytes[0];
          break;
        case DataTypeCodes.Int16:
        case DataTypeCodes.UInt16:
          Array.Copy((Array) bytes, 0, (Array) storage, offset, 2);
          break;
        case DataTypeCodes.Int32:
        case DataTypeCodes.UInt32:
        case DataTypeCodes.Float:
          Array.Copy((Array) bytes, 0, (Array) storage, offset, 4);
          break;
        case DataTypeCodes.Int64:
        case DataTypeCodes.UInt64:
        case DataTypeCodes.Double:
          Array.Copy((Array) bytes, 0, (Array) storage, offset, 8);
          break;
        default:
          Interpreter.Error = SmartFunctionResult.IllegalDataTypeCode;
          break;
      }
    }

    public void Add(Register srcRegister)
    {
      this.CheckType(srcRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return;
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) ((int) this.RegisterValue + (int) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (float) ((double) (float) this.RegisterValue + (double) (float) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) ((double) this.RegisterValue + (double) srcRegister.RegisterValue);
          break;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public void Sub(Register srcRegister)
    {
      this.CheckType(srcRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return;
      switch (srcRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) ((int) this.RegisterValue - (int) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (float) ((double) (float) this.RegisterValue - (double) (float) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) ((double) this.RegisterValue - (double) srcRegister.RegisterValue);
          break;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public void Mul(Register srcRegister)
    {
      this.CheckType(srcRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return;
      switch (srcRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) ((int) this.RegisterValue * (int) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (float) ((double) (float) this.RegisterValue * (double) (float) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) ((double) this.RegisterValue * (double) srcRegister.RegisterValue);
          break;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public void Div(Register srcRegister)
    {
      this.CheckType(srcRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return;
      switch (srcRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) ((int) this.RegisterValue / (int) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (float) ((double) (float) this.RegisterValue / (double) (float) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) ((double) this.RegisterValue / (double) srcRegister.RegisterValue);
          break;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public void Mod(Register srcRegister)
    {
      this.CheckType(srcRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return;
      switch (srcRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) ((int) this.RegisterValue % (int) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (float) ((double) (float) this.RegisterValue % (double) (float) srcRegister.RegisterValue);
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) ((double) this.RegisterValue % (double) srcRegister.RegisterValue);
          break;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool CompareEQ(Register cmpRegister)
    {
      this.CheckType(cmpRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return false;
      switch (cmpRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          return (int) this.RegisterValue == (int) cmpRegister.RegisterValue;
        case DataTypeCodes.Float:
          return (double) (float) this.RegisterValue == (double) (float) cmpRegister.RegisterValue;
        case DataTypeCodes.Double:
          return (double) this.RegisterValue == (double) cmpRegister.RegisterValue;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool CompareGE(Register cmpRegister)
    {
      this.CheckType(cmpRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return false;
      switch (cmpRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          return (int) this.RegisterValue >= (int) cmpRegister.RegisterValue;
        case DataTypeCodes.Float:
          return (double) (float) this.RegisterValue >= (double) (float) cmpRegister.RegisterValue;
        case DataTypeCodes.Double:
          return (double) this.RegisterValue >= (double) cmpRegister.RegisterValue;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool CompareLE(Register cmpRegister)
    {
      this.CheckType(cmpRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return false;
      switch (cmpRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          return (int) this.RegisterValue <= (int) cmpRegister.RegisterValue;
        case DataTypeCodes.Float:
          return (double) (float) this.RegisterValue <= (double) (float) cmpRegister.RegisterValue;
        case DataTypeCodes.Double:
          return (double) this.RegisterValue <= (double) cmpRegister.RegisterValue;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool CompareGT(Register cmpRegister)
    {
      this.CheckType(cmpRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return false;
      switch (cmpRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          return (int) this.RegisterValue > (int) cmpRegister.RegisterValue;
        case DataTypeCodes.Float:
          return (double) (float) this.RegisterValue > (double) (float) cmpRegister.RegisterValue;
        case DataTypeCodes.Double:
          return (double) this.RegisterValue > (double) cmpRegister.RegisterValue;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool CompareLT(Register cmpRegister)
    {
      this.CheckType(cmpRegister.RegisterTypeCode);
      if (Interpreter.IsError)
        return false;
      switch (cmpRegister.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          return (int) this.RegisterValue < (int) cmpRegister.RegisterValue;
        case DataTypeCodes.Float:
          return (double) (float) this.RegisterValue < (double) (float) cmpRegister.RegisterValue;
        case DataTypeCodes.Double:
          return (double) this.RegisterValue < (double) cmpRegister.RegisterValue;
        default:
          throw new Exception("Illegal register type");
      }
    }

    public bool IsNaN()
    {
      if (Interpreter.IsError)
        return false;
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Float:
          return float.IsNaN((float) this.RegisterValue);
        case DataTypeCodes.Double:
          return double.IsNaN((double) this.RegisterValue);
        default:
          throw new Exception("Illegal register type for NaN check");
      }
    }

    public void AccuToInt32()
    {
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (int) (float) this.RegisterValue;
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) (int) (double) this.RegisterValue;
          break;
      }
      this.RegisterTypeCode = DataTypeCodes.Int32;
    }

    public void AccuToFloat()
    {
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) (float) (int) this.RegisterValue;
          break;
        case DataTypeCodes.Double:
          this.RegisterValue = (object) (float) (double) this.RegisterValue;
          break;
      }
      this.RegisterTypeCode = DataTypeCodes.Float;
    }

    public void AccuToDouble()
    {
      switch (this.RegisterTypeCode)
      {
        case DataTypeCodes.Int32:
          this.RegisterValue = (object) (double) (int) this.RegisterValue;
          break;
        case DataTypeCodes.Float:
          this.RegisterValue = (object) (double) (float) this.RegisterValue;
          break;
      }
      this.RegisterTypeCode = DataTypeCodes.Double;
    }

    private void CheckType(DataTypeCodes checkTypeCode)
    {
      if (this.RegisterTypeCode == checkTypeCode)
        return;
      Interpreter.Error = SmartFunctionResult.RegisterOpterationWithDifferentTypes;
    }

    public ushort GetStorageByteSize(DataTypeCodes typeCode = DataTypeCodes.notDefined)
    {
      if (typeCode == DataTypeCodes.notDefined)
        typeCode = this.RegisterTypeCode;
      return Register.GetByteSizeFromType(typeCode);
    }

    public static ushort GetByteSizeFromType(DataTypeCodes typeCode)
    {
      switch (typeCode)
      {
        case DataTypeCodes.Int8:
        case DataTypeCodes.UInt8:
          return 1;
        case DataTypeCodes.Int16:
        case DataTypeCodes.UInt16:
          return 2;
        case DataTypeCodes.Int32:
        case DataTypeCodes.UInt32:
        case DataTypeCodes.Float:
          return 4;
        case DataTypeCodes.Int64:
        case DataTypeCodes.UInt64:
        case DataTypeCodes.Double:
          return 8;
        default:
          throw new Exception("Illegal type code found");
      }
    }

    public string GetValue() => this.RegisterValue.ToString();

    public string GetValueAndRegisterType()
    {
      return this.RegisterValue.ToString() + " <" + this.RegisterTypeCode.ToString() + ">";
    }
  }
}
