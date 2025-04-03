// Decompiled with JetBrains decompiler
// Type: HandlerLib.SmartFunctionRuntimeResult
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

#nullable disable
namespace HandlerLib
{
  public class SmartFunctionRuntimeResult
  {
    public SmartFunctionResult FunctionResult { get; protected set; }

    public ushort? ErrorOffset { get; protected set; }

    public bool Blocked => this.FunctionResult != 0;

    public string Error => this.Blocked ? this.FunctionResult.ToString() : (string) null;

    public SmartFunctionRuntimeResult()
    {
    }

    public SmartFunctionRuntimeResult(byte[] response)
    {
      int offset = 2;
      this.FunctionResult = (SmartFunctionResult) ByteArrayScanner.ScanUInt16(response, ref offset);
      ushort num = ByteArrayScanner.ScanUInt16(response, ref offset);
      if (!this.Blocked)
        return;
      this.ErrorOffset = new ushort?(num);
    }
  }
}
