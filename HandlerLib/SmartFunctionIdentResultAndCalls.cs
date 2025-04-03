// Decompiled with JetBrains decompiler
// Type: HandlerLib.SmartFunctionIdentResultAndCalls
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class SmartFunctionIdentResultAndCalls : SmartFunctionIdentAndResult
  {
    public uint Calls { get; private set; }

    public SmartFunctionIdentResultAndCalls(byte[] byteArray, ref int offset)
    {
      this.Name = ByteArrayScanner.ScanString(byteArray, ref offset);
      this.Version = ByteArrayScanner.ScanByte(byteArray, ref offset);
      this.Calls = ByteArrayScanner.ScanUInt32(byteArray, ref offset);
      this.FunctionResult = (SmartFunctionResult) ByteArrayScanner.ScanUInt16(byteArray, ref offset);
      ushort num = ByteArrayScanner.ScanUInt16(byteArray, ref offset);
      if (!this.Blocked)
        return;
      this.ErrorOffset = new ushort?(num);
    }

    public override string ToString()
    {
      return this.Name.PadRight(30) + " ; Version:" + this.Version.ToString("d3") + " ; Calles:" + this.Calls.ToString("d5") + " ; Blocked:" + this.Blocked.ToString();
    }
  }
}
