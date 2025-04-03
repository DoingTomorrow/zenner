// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceMemoryStorage
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceMemoryStorage
  {
    public ReadPartsSelection ReadSelection;
    public int WriteOrder;
    private uint oldStartAddress;
    private uint _startAdr;
    internal uint EndAddress;
    internal byte[] Data;
    internal bool[] DataAvailable;
    public uint WriteSplitSize = 0;

    public DeviceMemoryType MemoryType { get; internal set; }

    public uint StartAddress
    {
      get => this._startAdr;
      set
      {
        this.oldStartAddress = this._startAdr;
        this._startAdr = value;
      }
    }

    public uint ByteSize
    {
      get => (uint) ((int) this.EndAddress - (int) this.StartAddress + 1);
      set
      {
        this.EndAddress = (uint) ((int) this.StartAddress + (int) value - 1);
        this.Resize();
      }
    }

    internal void Resize()
    {
      int destinationIndex = this.oldStartAddress > this.StartAddress ? (int) this.oldStartAddress - (int) this.StartAddress : 0;
      Array.Resize<byte>(ref this.Data, (int) this.ByteSize);
      Array.Resize<bool>(ref this.DataAvailable, (int) this.ByteSize);
      if (destinationIndex == 0)
        return;
      byte[] numArray = new byte[(int) this.ByteSize];
      bool[] flagArray = new bool[(int) this.ByteSize];
      Array.Copy((Array) this.Data, 0L, (Array) numArray, (long) destinationIndex, (long) this.ByteSize - (long) destinationIndex);
      Array.Copy((Array) this.DataAvailable, 0L, (Array) flagArray, (long) destinationIndex, (long) this.ByteSize - (long) destinationIndex);
      Array.Copy((Array) numArray, (Array) this.Data, (long) this.ByteSize);
      Array.Copy((Array) flagArray, (Array) this.DataAvailable, (long) this.ByteSize);
    }

    internal DeviceMemoryStorage(
      DeviceMemoryType deviceMemoryType,
      uint startAddress,
      uint byteSize)
    {
      this.MemoryType = deviceMemoryType;
      this.StartAddress = startAddress;
      this.EndAddress = (uint) ((int) startAddress + (int) byteSize - 1);
      this.Data = new byte[(int) byteSize];
      this.DataAvailable = new bool[(int) byteSize];
    }

    internal DeviceMemoryStorage(
      DeviceMemoryType deviceMemoryType,
      uint startAddress,
      uint byteSize,
      uint WriteSplitSize)
      : this(deviceMemoryType, startAddress, byteSize)
    {
      this.WriteSplitSize = WriteSplitSize;
    }

    public DeviceMemoryStorage Clone()
    {
      DeviceMemoryStorage deviceMemoryStorage = new DeviceMemoryStorage(this.MemoryType, this.StartAddress, this.ByteSize);
      deviceMemoryStorage.Data = new byte[(int) this.ByteSize];
      deviceMemoryStorage.DataAvailable = new bool[(int) this.ByteSize];
      Buffer.BlockCopy((Array) this.Data, 0, (Array) deviceMemoryStorage.Data, 0, (int) this.ByteSize);
      Buffer.BlockCopy((Array) this.DataAvailable, 0, (Array) deviceMemoryStorage.DataAvailable, 0, (int) this.ByteSize);
      deviceMemoryStorage.WriteSplitSize = this.WriteSplitSize;
      return deviceMemoryStorage;
    }

    public void IncludeDataFromPartStorage(DeviceMemoryStorage partStorage)
    {
      if (partStorage.StartAddress < this.StartAddress)
        throw new Exception("PartStorage lower then Storage");
      uint dstOffset = partStorage.StartAddress - this.StartAddress;
      if (dstOffset + partStorage.ByteSize > this.ByteSize)
        throw new Exception("PartStorage higher then Storage");
      Buffer.BlockCopy((Array) partStorage.Data, 0, (Array) this.Data, (int) dstOffset, (int) partStorage.ByteSize);
      Buffer.BlockCopy((Array) partStorage.DataAvailable, 0, (Array) this.DataAvailable, (int) dstOffset, (int) partStorage.ByteSize);
    }

    public AddressRange GetAddressRange() => new AddressRange(this.StartAddress, this.ByteSize);

    public override string ToString()
    {
      return "0x" + this.StartAddress.ToString("x8") + "-0x" + this.EndAddress.ToString("x8") + " size: 0x" + this.ByteSize.ToString("x4");
    }
  }
}
