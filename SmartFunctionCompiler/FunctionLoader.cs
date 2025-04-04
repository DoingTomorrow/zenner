// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.FunctionLoader
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using HandlerLib;
using System;

#nullable disable
namespace SmartFunctionCompiler
{
  internal class FunctionLoader
  {
    internal const string LoggerParameterName = "Logger";
    internal static byte[] FlashStorage;
    internal static byte[] RamStorage;
    internal static byte[] BackupStorage;
    internal static ushort FlashLoadOffset;
    internal static ushort RamLoadOffset;
    internal static ushort BackupLoadOffset;

    internal static void ResetStorage()
    {
      FunctionLoader.FlashStorage = new byte[2000];
      FunctionLoader.RamStorage = new byte[2000];
      FunctionLoader.BackupStorage = new byte[200];
      FunctionLoader.FlashLoadOffset = (ushort) 16;
      FunctionLoader.RamLoadOffset = (ushort) 48;
      FunctionLoader.BackupLoadOffset = (ushort) 64;
    }

    internal static void LoadFunction(byte[] runtimeCode)
    {
      Interpreter.Error = SmartFunctionResult.NoError;
      Interpreter.ErrorOffset = (ushort) 0;
      int alignmentAndReturnDiff1 = (int) FunctionLoader.MoveOffsetToAlignmentAndReturnDiff(ref FunctionLoader.FlashLoadOffset, 2);
      int alignmentAndReturnDiff2 = (int) FunctionLoader.MoveOffsetToAlignmentAndReturnDiff(ref FunctionLoader.RamLoadOffset, 2);
      int alignmentAndReturnDiff3 = (int) FunctionLoader.MoveOffsetToAlignmentAndReturnDiff(ref FunctionLoader.BackupLoadOffset, 2);
      ByteArrayScanner16Bit.ScanString(runtimeCode, ref Interpreter.ErrorOffset);
      ByteArrayScanner16Bit.ScanByte(runtimeCode, ref Interpreter.ErrorOffset);
      ByteArrayScanner16Bit.ScanByte(runtimeCode, ref Interpreter.ErrorOffset);
      FirmwareEvents theByte = (FirmwareEvents) ByteArrayScanner16Bit.ScanByte(runtimeCode, ref Interpreter.ErrorOffset);
      if (!Enum.IsDefined(typeof (FirmwareEvents), (object) theByte))
        Interpreter.Error = SmartFunctionResult.NotSupportedEvent;
      byte num1 = ByteArrayScanner16Bit.ScanByte(runtimeCode, ref Interpreter.ErrorOffset);
      if (num1 >= (byte) 32)
        Interpreter.Error = SmartFunctionResult.ToManyParameters;
      if (Interpreter.IsError)
        return;
      ushort offset1 = Interpreter.ErrorOffset;
      ushort num2 = offset1;
      int num3 = 0;
      int num4 = 6;
      int num5 = 0;
      ushort loggerEntryBytes = 4;
      ushort num6 = 0;
      StorageTypeCodes storageTypeCodes = StorageTypeCodes.ram;
      for (int index = 0; index < (int) num1; ++index)
      {
        Interpreter.ErrorOffset = offset1;
        RuntimeParameter runtimeParameter = new RuntimeParameter(runtimeCode, ref offset1);
        if (Interpreter.IsError)
          return;
        string name = runtimeParameter.Name;
        StorageTypeCodes storageCode = runtimeParameter.StorageCode;
        int valueByteSize = (int) runtimeParameter.ValueByteSize;
        switch (storageCode)
        {
          case StorageTypeCodes.ram:
            num4 += valueByteSize;
            break;
          case StorageTypeCodes.flash:
            num3 += valueByteSize;
            break;
          case StorageTypeCodes.backup:
            num5 += valueByteSize;
            break;
          default:
            Interpreter.Error = SmartFunctionResult.IllegalStorageTypeCode;
            return;
        }
        if (name.StartsWith("L_"))
          loggerEntryBytes += (ushort) valueByteSize;
        else if (name == "Logger")
        {
          if (index != 0)
          {
            Interpreter.Error = SmartFunctionResult.LoggerParameterNotFirstParameter;
            return;
          }
          if (runtimeParameter.TypeCode != DataTypeCodes.UInt16)
          {
            Interpreter.Error = SmartFunctionResult.LoggerParameterNoUInt16;
            return;
          }
          num6 = runtimeParameter.InitValue_UInt16;
          storageTypeCodes = runtimeParameter.StorageCode;
          if (storageTypeCodes != StorageTypeCodes.ram && storageTypeCodes != StorageTypeCodes.flash)
          {
            Interpreter.Error = SmartFunctionResult.IllegalStorageTypeCode;
            return;
          }
        }
      }
      byte num7 = runtimeCode[(int) offset1++];
      ushort num8 = offset1;
      ushort offset2 = (ushort) ((uint) num6 * (uint) loggerEntryBytes);
      int alignmentAndReturnDiff4 = (int) FunctionLoader.MoveOffsetToAlignmentAndReturnDiff(ref offset2, 2);
      if (offset2 > (ushort) 0)
        offset2 += (ushort) 4;
      if (storageTypeCodes == StorageTypeCodes.ram)
        num4 += (int) offset2;
      else
        num3 += (int) offset2;
      int sizeForParameters = (int) FunctionAccessStorage.GetByteSizeForParameters((ushort) num1);
      int num9 = (int) FunctionLoader.FlashLoadOffset + sizeForParameters + runtimeCode.Length + num3;
      if (FunctionLoader.FlashStorage.Length < num9)
      {
        Interpreter.ErrorOffset = (ushort) (num9 - FunctionLoader.FlashStorage.Length);
        Interpreter.Error = SmartFunctionResult.FlashOutOfMemory;
      }
      else if (FunctionLoader.RamStorage.Length < (int) FunctionLoader.RamLoadOffset + num4)
      {
        Interpreter.ErrorOffset = (ushort) ((int) FunctionLoader.RamLoadOffset + num4 - FunctionLoader.RamStorage.Length);
        Interpreter.Error = SmartFunctionResult.RamOutOfMemory;
      }
      else if (FunctionLoader.BackupStorage.Length < (int) FunctionLoader.BackupLoadOffset + num5)
      {
        Interpreter.ErrorOffset = (ushort) ((int) FunctionLoader.BackupLoadOffset + num5 - FunctionLoader.BackupStorage.Length);
        Interpreter.Error = SmartFunctionResult.BackupOutOfMemory;
      }
      else
      {
        ushort flashLoadOffset = FunctionLoader.FlashLoadOffset;
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, (ushort) 0, ref flashLoadOffset);
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, (ushort) runtimeCode.Length, ref flashLoadOffset);
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, (ushort) 0, ref flashLoadOffset);
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, (ushort) 0, ref flashLoadOffset);
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, FunctionLoader.RamLoadOffset, ref flashLoadOffset);
        FunctionRamHeader functionRamHeader = new FunctionRamHeader(FunctionLoader.RamLoadOffset)
        {
          NextEventTime = 0,
          FunctionCallCounter = 0
        };
        FunctionLoader.RamLoadOffset += (ushort) 8;
        ByteArrayScanner16Bit.ScanInByte(FunctionLoader.FlashStorage, num1, ref flashLoadOffset);
        ByteArrayScanner16Bit.ScanInByte(FunctionLoader.FlashStorage, (byte) theByte, ref flashLoadOffset);
        FunctionAccessStorage functionAccessStorage = new FunctionAccessStorage(FunctionLoader.FlashStorage, FunctionLoader.FlashLoadOffset);
        FunctionLoader.FlashLoadOffset += functionAccessStorage.AccessStorageByteSize;
        Array.Copy((Array) runtimeCode, 0, (Array) FunctionLoader.FlashStorage, (int) FunctionLoader.FlashLoadOffset, runtimeCode.Length);
        FunctionLoader.FlashLoadOffset += (ushort) runtimeCode.Length;
        int alignmentAndReturnDiff5 = (int) FunctionLoader.MoveOffsetToAlignmentAndReturnDiff(ref FunctionLoader.FlashLoadOffset, 2);
        functionAccessStorage.StorageOffsetNextFunction = FunctionLoader.FlashLoadOffset;
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, (ushort) 0, ref FunctionLoader.FlashLoadOffset);
        FunctionLoader.FlashLoadOffset -= (ushort) 2;
        ushort codeStorageOffset = functionAccessStorage.RuntimeCodeStorageOffset;
        ushort theValue1 = (ushort) ((uint) codeStorageOffset + (uint) num8 + (uint) num7);
        ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, theValue1, ref flashLoadOffset);
        offset1 = num2;
        for (int index = 0; index < (int) num1; ++index)
        {
          ushort theValue2 = (ushort) ((uint) offset1 + (uint) codeStorageOffset);
          RuntimeParameter runtimeParameter = new RuntimeParameter(runtimeCode, ref offset1);
          StorageTypeCodes storageCode = runtimeParameter.StorageCode;
          ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, theValue2, ref flashLoadOffset);
          switch (storageCode)
          {
            case StorageTypeCodes.ram:
              FunctionLoader.AllocateStorageAndInitParameter(runtimeCode, runtimeParameter, ref flashLoadOffset, FunctionLoader.RamStorage, ref FunctionLoader.RamLoadOffset, offset2, loggerEntryBytes);
              break;
            case StorageTypeCodes.flash:
              FunctionLoader.AllocateStorageAndInitParameter(runtimeCode, runtimeParameter, ref flashLoadOffset, FunctionLoader.FlashStorage, ref FunctionLoader.FlashLoadOffset, offset2, loggerEntryBytes);
              break;
            case StorageTypeCodes.backup:
              FunctionLoader.AllocateStorageAndInitParameter(runtimeCode, runtimeParameter, ref flashLoadOffset, FunctionLoader.BackupStorage, ref FunctionLoader.BackupLoadOffset, offset2, loggerEntryBytes);
              break;
            default:
              Interpreter.Error = SmartFunctionResult.IllegalStorageTypeCode;
              return;
          }
        }
      }
    }

    private static void AllocateStorageAndInitParameter(
      byte[] runtimeCode,
      RuntimeParameter runtimeParameter,
      ref ushort fas_writeOffset,
      byte[] storage,
      ref ushort storageLoadOffset,
      ushort loggerBytes,
      ushort loggerEntryBytes)
    {
      ByteArrayScanner16Bit.ScanInUInt16(FunctionLoader.FlashStorage, storageLoadOffset, ref fas_writeOffset);
      if (runtimeParameter.Name == "Logger")
      {
        ushort offset = storageLoadOffset;
        ByteArrayScanner16Bit.ScanInUInt16(storage, (ushort) ((uint) storageLoadOffset + 4U), ref offset);
        ByteArrayScanner16Bit.ScanInUInt16(storage, loggerEntryBytes, ref offset);
        storageLoadOffset += loggerBytes;
      }
      else
      {
        Array.Copy((Array) runtimeCode, (int) runtimeParameter.ValueOffset, (Array) storage, (int) storageLoadOffset, (int) runtimeParameter.ValueByteSize);
        storageLoadOffset += runtimeParameter.ValueByteSize;
      }
    }

    internal static ushort MoveOffsetToAlignmentAndReturnDiff(ref ushort offset, int alignment)
    {
      int num1 = (int) offset & ~(alignment - 1);
      if (num1 == (int) offset)
        return 0;
      int num2 = num1 + alignment;
      int alignmentAndReturnDiff = num2 - (int) offset;
      offset = (ushort) num2;
      return (ushort) alignmentAndReturnDiff;
    }
  }
}
