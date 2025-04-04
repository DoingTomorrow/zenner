// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.LoggerHeader
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace SmartFunctionCompiler
{
  public class LoggerHeader
  {
    public const int LoggerHeaderByteSize = 4;
    public byte[] Storage;
    private ushort StorageOffset;
    private ushort NumberOfReservedEntries;

    public LoggerHeader(byte[] storage, ushort storageOffset, ushort numberOfReservedEntries)
    {
      this.Storage = storage;
      this.StorageOffset = storageOffset;
      this.NumberOfReservedEntries = numberOfReservedEntries;
    }

    public ushort LoggerWriteOffset
    {
      get => BitConverter.ToUInt16(this.Storage, (int) this.StorageOffset);
      set
      {
        Array.Copy((Array) BitConverter.GetBytes(value), 0, (Array) this.Storage, (int) this.StorageOffset, 2);
      }
    }

    public ushort LoggerEntrySize
    {
      get => BitConverter.ToUInt16(this.Storage, (int) this.StorageOffset + 2);
      set
      {
        Array.Copy((Array) BitConverter.GetBytes(value), 0, (Array) this.Storage, (int) this.StorageOffset + 2, 2);
      }
    }

    public ushort LoggerStorageStartOffset => (ushort) ((uint) this.StorageOffset + 4U);

    public ushort LoggerStorageEndOffset
    {
      get
      {
        return (ushort) ((uint) this.LoggerStorageStartOffset + (uint) this.NumberOfReservedEntries * (uint) this.LoggerEntrySize);
      }
    }

    public ushort GetMaxNumberOfEntries()
    {
      return (ushort) (((uint) this.LoggerStorageEndOffset - (uint) this.LoggerStorageStartOffset) / (uint) this.LoggerEntrySize);
    }

    public ushort GetNumberOfEntries()
    {
      return BitConverter.ToUInt32(this.Storage, (int) this.LoggerWriteOffset) == 0U ? (ushort) (((uint) this.LoggerWriteOffset - (uint) this.LoggerStorageStartOffset) / (uint) this.LoggerEntrySize) : (ushort) (((uint) this.LoggerStorageEndOffset - (uint) this.LoggerStorageStartOffset) / (uint) this.LoggerEntrySize);
    }

    public byte[] GetLoggerBytes()
    {
      int length = (int) this.GetNumberOfEntries() * (int) this.LoggerEntrySize;
      byte[] loggerBytes = new byte[length];
      int storageStartOffset = (int) this.LoggerStorageStartOffset;
      for (int index = 0; index < length; ++index)
        loggerBytes[index] = this.Storage[index + storageStartOffset];
      return loggerBytes;
    }

    public byte[] GetLoggerProtokoll()
    {
      int numberOfEntries = (int) this.GetNumberOfEntries();
      int count = (int) this.LoggerEntrySize - 4;
      byte[] numArray = new byte[(count + 6) * numberOfEntries];
      ushort storageStartOffset = this.LoggerStorageStartOffset;
      ushort offset = 0;
      for (int index = 0; index < numberOfEntries; ++index)
      {
        DateTime theValue = ByteArrayScanner16Bit.ScanDateTimeBase2000(this.Storage, ref storageStartOffset);
        ByteArrayScanner16Bit.ScanInDateTime(numArray, theValue, ref offset);
        Buffer.BlockCopy((Array) this.Storage, (int) storageStartOffset, (Array) numArray, (int) offset, count);
        storageStartOffset += (ushort) count;
        offset += (ushort) count;
      }
      return ((IEnumerable<byte>) numArray).ToArray<byte>();
    }

    public void MoveAndSaveWriteOffset()
    {
      ushort num = (ushort) ((uint) this.LoggerWriteOffset + (uint) this.LoggerEntrySize);
      if ((int) num >= (int) this.LoggerStorageEndOffset)
        num = this.LoggerStorageStartOffset;
      this.LoggerWriteOffset = num;
    }

    public void SaveBytes(byte[] bytesToSave, ref ushort writeOffset)
    {
      ByteArrayScanner16Bit.ScanInBytes(this.Storage, bytesToSave, ref writeOffset);
    }
  }
}
