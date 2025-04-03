// Decompiled with JetBrains decompiler
// Type: S3_Handler.ListLink
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class ListLink : S3_MemoryBlock
  {
    public string Name { get; set; }

    internal ushort Address
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
    }

    public ListLink(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.ListLink, parentMemoryBlock)
    {
      this.ByteSize = 2;
    }

    public ListLink(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.ByteSize = 2;
    }

    public ListLink(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock, int insertIndex)
      : base(MyMeter, S3_MemorySegment.ListLink, parentMemoryBlock, insertIndex)
    {
      this.ByteSize = 2;
    }

    internal ListLink Clone(S3_Meter theCloneMeter, S3_MemoryBlock parentMemoryBlock)
    {
      return new ListLink(theCloneMeter, parentMemoryBlock, (S3_MemoryBlock) this)
      {
        Name = this.Name
      };
    }
  }
}
