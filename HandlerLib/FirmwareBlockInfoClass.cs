// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareBlockInfoClass
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class FirmwareBlockInfoClass
  {
    public uint startAddress = uint.MaxValue;
    public uint endAddress = 0;
    public uint blockSize = 0;
    public bool isLocal = false;
    private byte[] _blockBytes = (byte[]) null;

    public ushort crc16 { get; private set; }

    public ushort crc16_CCITT { get; private set; }

    public byte[] BlockBytes
    {
      get => this._blockBytes;
      set
      {
        this._blockBytes = value;
        this.blockSize = (uint) this.BlockBytes.Length;
        this.endAddress = this.startAddress + this.blockSize;
        this.crc16_CCITT = CRC.CRC_CCITT(this._blockBytes);
        this.crc16 = CRC.CRC_16(this._blockBytes);
      }
    }

    public string ToString(uint blockNr = 0)
    {
      return "\r ... INFO for Firmware Block " + (blockNr > 0U ? blockNr.ToString() : "") + " \r Startaddress: 0x" + this.startAddress.ToString("x8") + "\r Endaddress:   0x" + this.endAddress.ToString("x8") + "\r Size:           " + this.blockSize.ToString() + " Bytes \r CRC_CCiTT:    0x" + this.crc16.ToString("x4") + "\r";
    }
  }
}
