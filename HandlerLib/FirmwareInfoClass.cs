// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareInfoClass
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class FirmwareInfoClass
  {
    public uint firstAddress = uint.MaxValue;
    public uint lastAddress = 0;
    public uint stepSize = 0;
    public uint oldKeyAdr = 0;
    public uint gapCounter = 0;
    public string gapMessage = string.Empty;

    public override string ToString()
    {
      return " ... INFO ... \r Startaddress: 0x" + this.firstAddress.ToString("x8") + "\r Endaddress:   0x" + this.lastAddress.ToString("x8") + "\r Stepsize:     " + this.stepSize.ToString() + "\r Gaps:         " + this.gapCounter.ToString() + "\r" + this.gapMessage;
    }
  }
}
