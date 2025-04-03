// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3_FunctionLayer
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class S3_FunctionLayer : S3_MemoryBlock
  {
    internal static Logger logger = LogManager.GetLogger(nameof (S3_FunctionLayer));
    internal ushort listControlWord;

    internal int LayerNr
    {
      get => (int) this.listControlWord >> 8 & 7;
      set => this.listControlWord = (ushort) ((int) this.listControlWord & 63743 | value << 8);
    }

    internal int NumberOfFunctions
    {
      get => (int) (ushort) ((uint) this.listControlWord & (uint) byte.MaxValue);
      set => this.listControlWord = (ushort) ((int) this.listControlWord & 65280 | value);
    }

    public S3_FunctionLayer(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.FunctionLayer, parentMemoryBlock)
    {
      this.firstChildMemoryBlockOffset = 2;
      this.ByteSize = 2;
      this.LayerNr = parentMemoryBlock.childMemoryBlocks.Count - 1;
    }

    public S3_FunctionLayer(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.FunctionLayer, parentMemoryBlock, insertIndex)
    {
      this.firstChildMemoryBlockOffset = 2;
      this.ByteSize = 2;
      this.LayerNr = insertIndex;
    }

    internal bool CreateStructureObjectsFromMemory(S3_Meter meter)
    {
      this.listControlWord = meter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      byte listControlWord = (byte) this.listControlWord;
      for (int index = 0; index < (int) listControlWord; ++index)
      {
        S3_Function s3Function = new S3_Function(this.MyMeter, (S3_MemoryBlock) this);
        if (!s3Function.CreateStructureObjectsOfFunctionTableFromMemory(meter))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not parse function table! Unknown function number detected.", S3_FunctionLayer.logger);
        s3Function.LayerNr = this.LayerNr;
        if (s3Function.DatabaseData.SetResources != null)
        {
          foreach (string setResource in s3Function.DatabaseData.SetResources)
          {
            if (!meter.MyResources.IsResourceAvailable(setResource) && !meter.MyResources.AddResource(setResource))
              return false;
          }
        }
      }
      return true;
    }

    internal bool InsertData()
    {
      if (!this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, this.listControlWord))
        return false;
      if (this.childMemoryBlocks == null)
        return true;
      foreach (S3_Function childMemoryBlock in this.childMemoryBlocks)
      {
        if (!childMemoryBlock.InsertData())
          return false;
      }
      return true;
    }

    internal S3_Function AddFunction(Function f, int pos)
    {
      S3_Function s3Function = new S3_Function(this.MyMeter, (S3_MemoryBlock) this, pos)
      {
        DatabaseData = f,
        FunctionNumber = (ushort) f.FunctionNumber,
        LayerNr = this.LayerNr
      };
      if (this.LayerNr == 1 && (pos == 0 || ((S3_Function) this.childMemoryBlocks[pos - 1]).IsProtected))
        s3Function.IsProtected = true;
      this.NumberOfFunctions = this.childMemoryBlocks.Count;
      return s3Function;
    }

    internal S3_FunctionLayer Clone(S3_FunctionTable clone_S3_FunctionTable)
    {
      S3_FunctionLayer clone_S3_FunctionLayer = new S3_FunctionLayer(clone_S3_FunctionTable.MyMeter, (S3_MemoryBlock) clone_S3_FunctionTable);
      clone_S3_FunctionLayer.listControlWord = this.listControlWord;
      if (this.childMemoryBlocks == null)
        return (S3_FunctionLayer) null;
      foreach (S3_Function childMemoryBlock in this.childMemoryBlocks)
        childMemoryBlock.Clone(clone_S3_FunctionLayer);
      return clone_S3_FunctionLayer;
    }
  }
}
