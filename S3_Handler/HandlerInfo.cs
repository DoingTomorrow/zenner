// Decompiled with JetBrains decompiler
// Type: S3_Handler.HandlerInfo
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class HandlerInfo : S3_MemoryBlock
  {
    internal static Logger HandlerInfoLogger = LogManager.GetLogger(nameof (HandlerInfo));
    private string ParameterTypeUsingString;

    private ushort mem_BlockByteSize
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
    }

    private ushort mem_BlockChecksum
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 2);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 2, value);
    }

    private byte mem_ParameterTypeUsingStringByteSize
    {
      get => this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress + 4);
      set => this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress + 4, value);
    }

    internal string mem_ParameterTypeUsingString
    {
      get
      {
        byte usingStringByteSize = this.mem_ParameterTypeUsingStringByteSize;
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < (int) usingStringByteSize; ++index)
          stringBuilder.Append((char) this.MyMeter.MyDeviceMemory.GetByteValue(this.BlockStartAddress + 5 + index));
        return stringBuilder.ToString();
      }
      set
      {
        byte num = value.Length <= (int) byte.MaxValue ? (byte) value.Length : throw new Exception("Illegal ParameterTypeUsingString");
        for (int index = 0; index < (int) num; ++index)
          this.MyMeter.MyDeviceMemory.SetByteValue(this.BlockStartAddress + 5 + index, (byte) value[index]);
      }
    }

    public HandlerInfo(
      S3_Meter MyMeter,
      S3_MemorySegment segmentType,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, segmentType, parentMemoryBlock)
    {
    }

    public HandlerInfo(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock,
      bool byteSizeFromSourceBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock, byteSizeFromSourceBlock)
    {
    }

    internal bool CreateFromMemory()
    {
      HandlerInfo.HandlerInfoLogger.Info("Create HandlerInfo from memory");
      this.ParameterTypeUsingString = string.Empty;
      this.MyMeter.MyLinker.Link(this.MyMeter.MyDeviceMemory.BlockFlashBlock1);
      if (!this.MyMeter.MyDeviceMemory.AreAllRuntimeCodeAddressesOk())
        return false;
      int num = this.MyMeter.MyDeviceMemory.BlockFlashBlock2.BlockStartAddress - this.BlockStartAddress;
      int memBlockByteSize = (int) this.mem_BlockByteSize;
      if (memBlockByteSize == 0 || memBlockByteSize > num || (int) this.mem_ParameterTypeUsingStringByteSize + 5 > memBlockByteSize)
        return true;
      string parameterTypeUsingString = this.mem_ParameterTypeUsingString;
      if ((int) this.CreateBlockChecksum(parameterTypeUsingString) != (int) this.mem_BlockChecksum)
        return true;
      this.ParameterTypeUsingString = parameterTypeUsingString;
      if (this.MyMeter.TypeCreationString == null && this.ParameterTypeUsingString.Length > 0)
      {
        this.MyMeter.TypeCreationString = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_BaseTypeId.ToString()].GetUintValue().ToString() + ";" + this.ParameterTypeUsingString;
        this.MyMeter.MyIdentification.TypeCreationString = this.MyMeter.TypeCreationString;
      }
      this.ByteSize = memBlockByteSize;
      return true;
    }

    internal void CreateMemoryBlock()
    {
      this.ParameterTypeUsingString = this.MyMeter.TypeCreationString != null ? OverwriteWorkMeter.CreateParameterTypeUsingStringFromTypeCreationString(this.MyMeter.TypeCreationString) : string.Empty;
      this.ByteSize = this.ParameterTypeUsingString.Length + 5;
    }

    internal void InsertData()
    {
      if (OverwriteWorkMeter.CreateParameterTypeUsingStringFromTypeCreationString(this.MyMeter.TypeCreationString) != this.ParameterTypeUsingString)
        throw new Exception("Illegal change of ParameterTypeUsingString");
      if (this.ByteSize != this.ParameterTypeUsingString.Length + 5)
        throw new Exception("Illegal prepared HandlerInfo block");
      this.mem_BlockByteSize = (ushort) this.ByteSize;
      this.mem_BlockChecksum = this.CreateBlockChecksum(this.ParameterTypeUsingString);
      this.mem_ParameterTypeUsingStringByteSize = (byte) this.ParameterTypeUsingString.Length;
      this.mem_ParameterTypeUsingString = this.ParameterTypeUsingString;
    }

    private ushort CreateBlockChecksum(string tempParameterTypeUsingString)
    {
      ushort blockChecksum = (ushort) (23130U + (uint) (ushort) tempParameterTypeUsingString.Length);
      for (int index = 0; index < tempParameterTypeUsingString.Length; ++index)
        blockChecksum += (ushort) (byte) tempParameterTypeUsingString[index];
      return blockChecksum;
    }
  }
}
