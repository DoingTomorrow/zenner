// Decompiled with JetBrains decompiler
// Type: S3_Handler.NotProtectedRange
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal sealed class NotProtectedRange : S3_MemoryBlock
  {
    public ushort NotProtectedLength { get; set; }

    public ushort NotProtectedAddress { get; set; }

    internal ushort memNotProtectedLength
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set
      {
        this.NotProtectedLength = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
      }
    }

    internal ushort memNotProtectedAddress
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 2);
      set
      {
        this.NotProtectedAddress = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 2, value);
      }
    }

    public NotProtectedRange(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.WriteProt, parentMemoryBlock)
    {
      this.ByteSize = 4;
    }

    public NotProtectedRange(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      NotProtectedRange sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMemoryBlock)
    {
    }

    internal bool CreateStructureObjects(ref ushort addr)
    {
      this.NotProtectedLength = this.memNotProtectedLength;
      this.NotProtectedAddress = this.memNotProtectedAddress;
      return true;
    }

    internal NotProtectedRange Clone(S3_Meter theCloneMeter)
    {
      return new NotProtectedRange(theCloneMeter, (S3_MemoryBlock) theCloneMeter.MyDeviceMemory.BlockWriteProtTable, this)
      {
        NotProtectedLength = this.NotProtectedLength,
        NotProtectedAddress = this.NotProtectedAddress
      };
    }

    internal bool InsertData()
    {
      this.memNotProtectedAddress = this.NotProtectedAddress;
      this.memNotProtectedLength = this.NotProtectedLength;
      return true;
    }
  }
}
