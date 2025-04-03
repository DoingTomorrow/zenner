// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_FunctionTable
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_FunctionTable : S3_MemoryBlock
  {
    public S3_FunctionTable(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.FunctionTable, parentMemoryBlock)
    {
    }

    public S3_FunctionTable(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_FunctionTable sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
    }

    internal bool CreateStructureObjectsFromMemory()
    {
      ushort ushortValue;
      do
      {
        ushortValue = (ushort) (byte) this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock);
        if (!new S3_FunctionLayer(this.MyMeter, (S3_MemoryBlock) this).CreateStructureObjectsFromMemory(this.MyMeter))
          return false;
      }
      while (ushortValue > (ushort) 0);
      return true;
    }

    internal bool InsertData()
    {
      if (this.childMemoryBlocks == null)
        return true;
      foreach (S3_FunctionLayer childMemoryBlock in this.childMemoryBlocks)
      {
        if (!childMemoryBlock.InsertData())
          return false;
      }
      return true;
    }

    internal S3_FunctionLayer AddLayer()
    {
      if (this.childMemoryBlocks == null || this.childMemoryBlocks.Count == 0)
      {
        S3_FunctionLayer s3FunctionLayer1 = new S3_FunctionLayer(this.MyMeter, (S3_MemoryBlock) this);
        S3_FunctionLayer s3FunctionLayer2 = new S3_FunctionLayer(this.MyMeter, (S3_MemoryBlock) this);
        return s3FunctionLayer1;
      }
      return this.childMemoryBlocks.Count - 1 < 7 ? new S3_FunctionLayer(this.MyMeter, (S3_MemoryBlock) this, this.childMemoryBlocks.Count - 1) : (S3_FunctionLayer) null;
    }

    internal S3_Function AddFunction(int layerNr, Function f, int pos)
    {
      if (layerNr > this.childMemoryBlocks.Count)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Layer not available", FunctionManager.logger);
        return (S3_Function) null;
      }
      S3_FunctionLayer childMemoryBlock = (S3_FunctionLayer) this.childMemoryBlocks[layerNr];
      if (layerNr == this.childMemoryBlocks.Count - 1)
      {
        S3_FunctionLayer s3FunctionLayer = new S3_FunctionLayer(this.MyMeter, (S3_MemoryBlock) this);
      }
      S3_Function s3Function = childMemoryBlock.AddFunction(f, pos);
      if (f.SetResources != null)
      {
        foreach (string setResource in f.SetResources)
          this.MyMeter.MyResources.AddResource(setResource);
      }
      return s3Function;
    }

    internal S3_FunctionTable Clone(S3_Meter theCloneMeter)
    {
      theCloneMeter.MyDeviceMemory.BlockFunctionTable.Clear();
      S3_FunctionTable clone_S3_FunctionTable = new S3_FunctionTable(theCloneMeter, theCloneMeter.MyDeviceMemory.BlockFunctionTable, this);
      foreach (S3_FunctionLayer childMemoryBlock in this.childMemoryBlocks)
        childMemoryBlock.Clone(clone_S3_FunctionTable);
      return clone_S3_FunctionTable;
    }
  }
}
