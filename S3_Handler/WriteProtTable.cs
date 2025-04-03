// Decompiled with JetBrains decompiler
// Type: S3_Handler.WriteProtTable
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class WriteProtTable : S3_MemoryBlock
  {
    public WriteProtTable(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.WriteProtTable, parentMemoryBlock)
    {
    }

    public WriteProtTable(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    internal bool CreateStructureObjects(ushort addr)
    {
      ushort addr1 = addr;
      while (this.MyMeter.MyDeviceMemory.GetUshortValue(this.StartAddressOfNextBlock) > (ushort) 0)
      {
        if (!new NotProtectedRange(this.MyMeter, (S3_MemoryBlock) this).CreateStructureObjects(ref addr1))
          return false;
      }
      S3_MemoryBlock s3MemoryBlock = new S3_MemoryBlock(this.MyMeter, 2, S3_MemorySegment.WriteProt, (S3_MemoryBlock) this);
      return true;
    }

    internal bool Clone(S3_Meter theCloneMeter)
    {
      WriteProtTable blockWriteProtTable = theCloneMeter.MyDeviceMemory.BlockWriteProtTable;
      blockWriteProtTable.Clear();
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is NotProtectedRange)
        {
          ((NotProtectedRange) childMemoryBlock).Clone(theCloneMeter);
        }
        else
        {
          S3_MemoryBlock s3MemoryBlock = new S3_MemoryBlock(theCloneMeter, (S3_MemoryBlock) blockWriteProtTable, childMemoryBlock, true);
        }
      }
      return true;
    }

    internal bool InsertData()
    {
      foreach (S3_MemoryBlock childMemoryBlock in this.childMemoryBlocks)
      {
        if (childMemoryBlock is NotProtectedRange)
          ((NotProtectedRange) childMemoryBlock).InsertData();
        else
          childMemoryBlock.Fill((byte) 0);
      }
      return true;
    }
  }
}
