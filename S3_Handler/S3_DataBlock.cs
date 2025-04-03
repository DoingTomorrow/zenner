// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DataBlock
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_DataBlock : S3_MemoryBlock
  {
    internal byte[] Data;
    internal int NumberOfPreviesBytes;

    public S3_DataBlock(byte[] Data, S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.DataBlock, parentMemoryBlock)
    {
      this.Data = Data;
      this.Alignment = 1;
      this.ByteSize = Data.Length;
    }

    public S3_DataBlock(
      byte[] Data,
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_DataBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.Data = Data;
      this.Alignment = 1;
      this.ByteSize = Data.Length;
    }

    internal bool InsertData()
    {
      return this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress, this.Data);
    }

    internal S3_DataBlock Clone(S3_Meter theCloneMeter, S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_DataBlock(this.Data, theCloneMeter, cloneParentMemoryBlock, this);
    }
  }
}
