// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_DisplayMenu
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_DisplayMenu : S3_MemoryBlock
  {
    internal S3_Function My_S3_Function;
    internal FunctionPrecompiled funcFromDB;
    internal string MenuName;
    internal byte[] Codes;

    public S3_DisplayMenu(
      S3_Meter MyMeter,
      S3_Function My_S3_Function,
      FunctionPrecompiled funcFromDB,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.DisplayMenu, parentMemoryBlock)
    {
      this.BaseConstructor(My_S3_Function, funcFromDB);
    }

    public S3_DisplayMenu(
      S3_Meter MyMeter,
      S3_Function My_S3_Function,
      FunctionPrecompiled funcFromDB,
      S3_MemoryBlock parentMemoryBlock,
      S3_DisplayMenu sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
      this.BaseConstructor(My_S3_Function, funcFromDB);
    }

    public S3_DisplayMenu(
      S3_Meter MyMeter,
      S3_Function My_S3_Function,
      FunctionPrecompiled funcFromDB,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.DisplayMenu, parentMemoryBlock, insertIndex)
    {
      this.BaseConstructor(My_S3_Function, funcFromDB);
    }

    private void BaseConstructor(S3_Function My_S3_Function, FunctionPrecompiled funcFromDB)
    {
      this.My_S3_Function = My_S3_Function;
      this.funcFromDB = funcFromDB;
      this.Alignment = 1;
      this.Codes = (byte[]) funcFromDB.Codes.Clone();
      this.ByteSize = this.Codes.Length;
    }

    internal bool InsertData()
    {
      return this.MyMeter.MyDeviceMemory.SetByteArray(this.BlockStartAddress, this.Codes);
    }

    internal S3_DisplayMenu Clone(
      S3_Meter theCloneMeter,
      S3_Function clone_S3_Function,
      FunctionPrecompiled clone_funcFromDB,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      return new S3_DisplayMenu(theCloneMeter, clone_S3_Function, clone_funcFromDB, cloneParentMemoryBlock, this);
    }
  }
}
