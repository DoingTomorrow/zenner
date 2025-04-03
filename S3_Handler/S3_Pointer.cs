// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_Pointer
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class S3_Pointer : S3_MemoryBlock
  {
    internal string PointerName;

    public S3_Pointer(string PointerName, S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.Pointer, parentMemoryBlock)
    {
      this.PointerName = PointerName;
      this.Alignment = 1;
      this.ByteSize = 2;
    }

    public S3_Pointer(
      string PointerName,
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.Pointer, parentMemoryBlock, insertIndex)
    {
      this.PointerName = PointerName;
      this.Alignment = 1;
      this.ByteSize = 2;
    }

    public S3_Pointer(
      string PointerName,
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_Pointer sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.PointerName = PointerName;
      this.Alignment = 1;
      this.ByteSize = 2;
    }

    internal S3_Pointer Clone(S3_Meter theCloneMeter, S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_Pointer(this.PointerName, theCloneMeter, cloneParentMemoryBlock, this);
    }

    public override string ToString() => "Name: " + this.PointerName;
  }
}
