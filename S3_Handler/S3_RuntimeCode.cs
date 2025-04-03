// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_RuntimeCode
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_RuntimeCode : S3_MemoryBlock
  {
    internal byte[] Codes;

    public S3_RuntimeCode(
      FunctionPrecompiled funcFromDB,
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.RuntimeCode, parentMemoryBlock)
    {
      this.Codes = funcFromDB.Codes;
      this.Alignment = 1;
      this.ByteSize = funcFromDB.Codes.Length;
    }

    public S3_RuntimeCode(
      byte[] Codes,
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_RuntimeCode sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.Codes = Codes;
      this.Alignment = 1;
      this.ByteSize = Codes.Length;
    }

    internal bool InsertData()
    {
      return this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress, this.Codes);
    }

    internal S3_RuntimeCode Clone(S3_Meter theCloneMeter, S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_RuntimeCode(this.Codes, theCloneMeter, cloneParentMemoryBlock, this);
    }
  }
}
