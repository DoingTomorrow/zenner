// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DisplayFunction
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_DisplayFunction : S3_MemoryBlock
  {
    internal S3_Function MyFunction;

    public S3_DisplayFunction(
      S3_Meter MyMeter,
      S3_Function MyFunction,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.DisplayFunction, parentMemoryBlock)
    {
      this.MyFunction = MyFunction;
      this.Alignment = 1;
    }

    public S3_DisplayFunction(
      S3_Meter MyMeter,
      S3_Function MyFunction,
      S3_MemoryBlock parentMemoryBlock,
      S3_DisplayFunction sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.MyFunction = MyFunction;
      this.Alignment = 1;
    }

    public S3_DisplayFunction(
      S3_Meter MyMeter,
      S3_Function MyFunction,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.DisplayFunction, parentMemoryBlock, insertIndex)
    {
      this.MyFunction = MyFunction;
      this.Alignment = 1;
    }

    internal S3_DisplayFunction Clone(
      S3_Meter theCloneMeter,
      S3_Function theCloneFunction,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_DisplayFunction(theCloneMeter, theCloneFunction, cloneParentMemoryBlock, this);
    }
  }
}
