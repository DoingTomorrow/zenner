// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.FunctionAccessStorage
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;

#nullable disable
namespace SmartFunctionCompiler
{
  public class FunctionAccessStorage
  {
    private const int FixByteSizeOnTop = 14;
    public byte[] FlashStorage;
    public ushort StorageOffset;

    public FunctionAccessStorage(byte[] flashStorage, ushort storageOffset)
    {
      this.FlashStorage = flashStorage;
      this.StorageOffset = storageOffset;
    }

    public ushort StorageOffsetNextFunction
    {
      get => BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset);
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.FlashStorage, (int) this.StorageOffset, 2);
      }
    }

    public ushort NumberOfFunctionBytes
    {
      get => BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 2);
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.FlashStorage, (int) this.StorageOffset + 2, 2);
      }
    }

    public SmartFunctionResult SmartFunctionResult
    {
      get
      {
        return (SmartFunctionResult) BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 4);
      }
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes((ushort) value), 0, (Array) this.FlashStorage, (int) this.StorageOffset + 4, 2);
      }
    }

    public ushort ErrorOffset
    {
      get => BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 6);
      set
      {
        Buffer.BlockCopy((Array) BitConverter.GetBytes(value), 0, (Array) this.FlashStorage, (int) this.StorageOffset + 6, 2);
      }
    }

    public ushort FunctionRamHeaderOffset
    {
      get => BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 8);
    }

    public byte NumberOfRuntimeParameters => this.FlashStorage[(int) this.StorageOffset + 10];

    public FirmwareEvents FunctionEvent
    {
      get => (FirmwareEvents) this.FlashStorage[(int) this.StorageOffset + 11];
    }

    public ushort CodeStartOffset
    {
      get => BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 12);
    }

    public ushort ResetCodeStartOffset
    {
      get
      {
        if (this.NumberOfRuntimeParameters <= (byte) 0)
          return (ushort) ((int) this.StorageOffset + 14 + this.FunctionName.Length + 5);
        ushort parameterFunctionOffset = this.GetRuntimeParameterFunctionOffset((byte) ((uint) this.NumberOfRuntimeParameters - 1U));
        RuntimeParameter runtimeParameter = new RuntimeParameter(this.FlashStorage, ref parameterFunctionOffset);
        return (ushort) ((uint) parameterFunctionOffset + 1U);
      }
    }

    public byte NumberOfResetCodeBytes
    {
      get => this.FlashStorage[(int) (ushort) ((uint) this.ResetCodeStartOffset - 1U)];
    }

    public ushort AccessStorageByteSize
    {
      get
      {
        return FunctionAccessStorage.GetByteSizeForParameters((ushort) this.NumberOfRuntimeParameters);
      }
    }

    public static ushort GetByteSizeForParameters(ushort numberOfParameters)
    {
      return (ushort) (14 + (int) numberOfParameters * 4);
    }

    public ushort RuntimeCodeStorageOffset
    {
      get => (ushort) ((uint) this.StorageOffset + (uint) this.AccessStorageByteSize);
    }

    public string FunctionName
    {
      get
      {
        ushort offset = (ushort) ((uint) this.StorageOffset + (uint) this.AccessStorageByteSize);
        return ByteArrayScanner16Bit.ScanString(this.FlashStorage, ref offset);
      }
    }

    public ushort GetRuntimeParameterFunctionOffset(byte parameterNumber)
    {
      if ((int) parameterNumber > (int) this.NumberOfRuntimeParameters)
        throw new Exception("ParameterNumber > NumberOfRuntimeParameters");
      return BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 14 + (int) parameterNumber * 4);
    }

    public ushort GetRuntimeParameterDataOffset(byte parameterNumber)
    {
      return BitConverter.ToUInt16(this.FlashStorage, (int) this.StorageOffset + 14 + (int) parameterNumber * 4 + 2);
    }

    public byte[] GetParameterValueBytes(byte parameterNumber)
    {
      return new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber)).GetValueBytes(this.GetRuntimeParameterDataOffset(parameterNumber));
    }

    public void LoadRegisterByParameterNumber(Register reg, byte parameterNumber)
    {
      RuntimeParameter runtimeParameter = new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber));
      reg.LoadValue(runtimeParameter.TypeCode, FunctionAccessStorage.GetStorageFromStorageCode(runtimeParameter.StorageCode), (int) this.GetRuntimeParameterDataOffset(parameterNumber));
    }

    public void LoadRegisterByParameterNumberInit(Register reg, byte parameterNumber)
    {
      RuntimeParameter runtimeParameter = new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber));
      reg.LoadValue(runtimeParameter.TypeCode, this.FlashStorage, (int) runtimeParameter.ValueByteSize);
    }

    public RuntimeParameter GetParameterObject(byte parameterNumber)
    {
      return new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber));
    }

    public string GetParameterValue(byte parameterNumber)
    {
      Register reg = new Register();
      this.LoadRegisterByParameterNumber(reg, parameterNumber);
      return reg.GetValueAndRegisterType();
    }

    public void StoreRegisterByParameterNumber(byte parameterNumber)
    {
      RuntimeParameter runtimeParameter = new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber));
      Interpreter.RegA.StoreValue(runtimeParameter.TypeCode, FunctionAccessStorage.GetStorageFromStorageCode(runtimeParameter.StorageCode), (int) this.GetRuntimeParameterDataOffset(parameterNumber));
    }

    internal static byte[] GetStorageFromStorageCode(StorageTypeCodes storageCode)
    {
      switch (storageCode)
      {
        case StorageTypeCodes.ram:
          return FunctionLoader.RamStorage;
        case StorageTypeCodes.flash:
          return FunctionLoader.FlashStorage;
        case StorageTypeCodes.backup:
          return FunctionLoader.BackupStorage;
        default:
          throw new Exception("Illegal storageCode");
      }
    }

    internal LoggerHeader GetLoggerHeader()
    {
      if (this.NumberOfRuntimeParameters == (byte) 0)
        return (LoggerHeader) null;
      RuntimeParameter parameterObject = this.GetParameterObject((byte) 0);
      if (parameterObject.Name != "Logger")
        return (LoggerHeader) null;
      switch (parameterObject.StorageCode)
      {
        case StorageTypeCodes.ram:
          return new LoggerHeader(FunctionLoader.RamStorage, this.GetRuntimeParameterDataOffset((byte) 0), parameterObject.InitValue_UInt16);
        case StorageTypeCodes.flash:
          return new LoggerHeader(this.FlashStorage, this.GetRuntimeParameterDataOffset((byte) 0), parameterObject.InitValue_UInt16);
        default:
          throw new Exception("Illegal Logger storage");
      }
    }

    public void GetOverview()
    {
      Interpreter.RunLog.AppendLine();
      Interpreter.RunLog.AppendLine("*** Function storage overview ***");
      Interpreter.RunLog.AppendLine("StorageOffsetNextFunction: . 0x" + this.StorageOffsetNextFunction.ToString("x04"));
      Interpreter.RunLog.AppendLine("NumberOfFunctionBytes: ..... " + this.NumberOfFunctionBytes.ToString());
      Interpreter.RunLog.AppendLine();
      Interpreter.RunLog.AppendLine("FunctionAccessStorageOffset: 0x" + this.StorageOffset.ToString("x04"));
      Interpreter.RunLog.AppendLine("RuntimeCodeStorageOffset: .. 0x" + this.RuntimeCodeStorageOffset.ToString("x04"));
      Interpreter.RunLog.AppendLine("ResetCodeStartOffset: ...... 0x" + this.ResetCodeStartOffset.ToString("x04"));
      Interpreter.RunLog.AppendLine("CodeStartOffset: ........... 0x" + this.CodeStartOffset.ToString("x04"));
      Interpreter.RunLog.AppendLine("NumberOfRuntimeParameters: . " + this.NumberOfRuntimeParameters.ToString());
      Interpreter.RunLog.AppendLine("FunctionRamHeaderOffset: .... 0x" + this.FunctionRamHeaderOffset.ToString("x04"));
      Interpreter.RunLog.AppendLine();
      Interpreter.RunLog.AppendLine("Parameters:");
      for (byte parameterNumber = 0; (int) parameterNumber < (int) this.NumberOfRuntimeParameters; ++parameterNumber)
      {
        RuntimeParameter runtimeParameter = new RuntimeParameter(this.FlashStorage, this.GetRuntimeParameterFunctionOffset(parameterNumber));
        ushort parameterDataOffset = this.GetRuntimeParameterDataOffset(parameterNumber);
        RuntimeParameter parameterObject = this.GetParameterObject(parameterNumber);
        string name = parameterObject.Name;
        string str = parameterObject.TypeCode == DataTypeCodes.ByteList ? new SmartFunctionByteList(parameterObject.ParameterBytes, 2).ToString() : this.GetParameterValue(parameterNumber);
        Interpreter.RunLog.AppendLine(name + "; " + runtimeParameter.StorageCode.ToString() + "; " + runtimeParameter.TypeCode.ToString() + "; offset: 0x" + parameterDataOffset.ToString("x04") + "; value = " + str);
      }
    }
  }
}
